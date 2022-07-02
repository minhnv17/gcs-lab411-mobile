using GCS_Comunication.Protocol;
using GCS_Comunication.Protocol.Uavlink;
using GCS_LAB411.Protocol.Command;
using GCS_LAB411.Protocol.Uavlink;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GCS_Comunication.Comunication
{
    public class GCS_Com
    {
        public delegate void DelegateState(Uavlink_msg_state_t tele);
        public delegate void DelegateLinked(bool isLinked);
        public delegate void DelegateLocalPos(Uavlink_msg_local_position_t tele);
        public delegate void DelegateGlobalPos(Uavlink_msg_global_position_t tele);

        public event DelegateState StateChanged;
        public event DelegateLocalPos LocalPosChanged;
        public event DelegateGlobalPos GlobalPosChanged;
        public event DelegateLinked LinkedChanged;

        private Udp_Connect _connect;

        private Thread _receivingThread;
        private Thread _parsingThread;
        private Thread _sendingThread;

        private Thread _manualControl;

        private Timer _linkedTimer1Hz;

        private BlockingCollection<Uavlink_message_t> _sendQueue;
        private BlockingCollection<byte[]> _recvQueue;

        public GCS_Com(Udp_Connect connect)
        {
            _connect = connect;

            _receivingThread = new Thread(new ThreadStart(ReceivingThreadFunction));
            _receivingThread.IsBackground = true;

            _parsingThread = new Thread(new ThreadStart(ParsingThreadFunction));
            _parsingThread.IsBackground = true;

            _sendingThread = new Thread(new ThreadStart(SendingThreadFunction));
            _sendingThread.IsBackground = true;

            _sendQueue = new BlockingCollection<Uavlink_message_t>();
            _recvQueue = new BlockingCollection<byte[]>();

            _manualControl = new Thread(new ThreadStart(HandleSendManualControl));
            _manualControl.IsBackground = true;

            _receivingThread.Start();
            _parsingThread.Start();
            _sendingThread.Start();
            //_manualControl.Start();

            _linkedTimer1Hz = new Timer(new TimerCallback(LinkedTimer1Hz), null, 0, 1000);
        }

        private void ReceivingThreadFunction()
        {
            while (_parsingThread != null)
            {
                try
                {
                    if (_connect != null && _connect.IsOpen)
                    {
                        // Receive packet from network, then put to queue which is processed by parser
                        byte[] buffer;
                        if (_connect.ReceiveData(out buffer) > 0)
                        {
                            _linkCount = 0;
                            _recvQueue.Add(buffer);
                        }
                    }
                }
                catch { }
                Thread.Sleep(1);
            }
        }
        private void ParsingThreadFunction()
        {
            while (_parsingThread != null)
            {
                Uavlink_message_t message = new Uavlink_message_t();
                byte[] buffer = _recvQueue.Take();

                message.Decode(buffer);
                OnReceivedMessage(message);
                Thread.Sleep(1);
            }
        }

        private void SendingThreadFunction()
        {
            while (_parsingThread != null)
            {
                {
                    Uavlink_message_t message = _sendQueue.Take();
                    if (message != null)
                    {
                        byte[] buffer;
                        message.Encode(out buffer);
                        int size = buffer.Length;
                        while (size > 0)
                        {
                            try
                            {
                                size -= _connect.SendData(buffer);
                            }
                            catch (Exception ex)
                            {
                                return;
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void OnReceivedMessage(Uavlink_message_t message)
        {
            switch(message.Msgid)
            {
                case MessageId.UAVLINK_MSG_ID_STATE:
                    OnReceivedState(message.Payload);
                    break;

                case MessageId.UAVLINK_MSG_ID_GLOBAL_POSITION:
                    OnReceivedGlobalPos(message.Payload);
                    break;

                case MessageId.UAVLINK_MSG_ID_LOCAL_POSITION:
                    OnReceivedLocalPos(message.Payload);
                    break;

                default:
                    break;
            }
        }
        public void Dispose()
        {
            _connect?.DoDisconnect();
            _connect = null;

            //kill thread

            _parsingThread?.Abort();
            _sendingThread?.Abort();
            _receivingThread?.Abort();

            _parsingThread?.Join();
            _sendingThread?.Join();
            _receivingThread?.Join();

            _sendingThread = null;
            _parsingThread = null;
            _receivingThread = null;

            _sendQueue?.Dispose();
            _recvQueue?.Dispose();
            _sendQueue = null;
            _recvQueue = null;
        }

        private uint _linkCount = 0;
        private void LinkedTimer1Hz(object state)
        {
            _linkCount++;
            if (_linkCount > 5)
            {
                LinkedChanged?.Invoke(false);
                _linkCount = 6;
            }
            else LinkedChanged?.Invoke(true);
        }

        private void OnReceivedState(byte[] data)
        {
            Uavlink_msg_state_t state = new Uavlink_msg_state_t();
            state.Decode(data);
            StateChanged?.Invoke(state);
        }

        private void OnReceivedGlobalPos(byte[] data)
        {
            Uavlink_msg_global_position_t tele = new Uavlink_msg_global_position_t();
            tele.Decode(data);
            GlobalPosChanged?.Invoke(tele);
        }

        private void OnReceivedLocalPos(byte[] data)
        {
            Uavlink_msg_local_position_t tele = new Uavlink_msg_local_position_t();
            tele.Decode(data);
            LocalPosChanged?.Invoke(tele);
        }

        public Task<Tuple<bool, string>> SendCommandTakeoff(float altitude)
        {
            Func<Tuple<bool, string>> sendCommand = () =>
            {
                Tuple<bool, string> resAnswers = new Tuple<bool, string>(false, null);
                byte[] takeoff_data, message_pack;

                Uavlink_cmd_takeoff_t takeoffcmd = new Uavlink_cmd_takeoff_t();
                takeoffcmd.Altitude = altitude;
                takeoffcmd.Encode(out takeoff_data);

                Uavlink_message_t message = new Uavlink_message_t();
                message.Msgid = MessageId.UAVLINK_MSG_ID_COMMAND;
                message.LenPayload = (sbyte)takeoff_data.Length;
                message.Payload = takeoff_data;
                message.Encode(out message_pack);

                SendMessage(message);
                resAnswers = Tuple.Create(true, "send command ok");
                return resAnswers;
            };
            var task = new Task<Tuple<bool, string>>(sendCommand);
            task.Start();
            return task;
        }

        public void SendCommandSetMode(int mode)
        {
            byte[] setmode_data, message_pack;

            Uavlink_cmd_setmode_t setmodeCmd = new Uavlink_cmd_setmode_t();
            setmodeCmd.Mode = (byte)mode;

            setmodeCmd.Encode(out setmode_data);
            Uavlink_message_t message = new Uavlink_message_t();
            message.Msgid = MessageId.UAVLINK_MSG_ID_COMMAND;
            message.LenPayload = (sbyte)setmode_data.Length;
            message.Payload = setmode_data;
            message.Encode(out message_pack);

            SendMessage(message);
        }

        public Task<Tuple<bool, string>> SendCommandArmDisarm(byte armdisarm)
        {
            Func<Tuple<bool, string>> sendCommand = () =>
            {
                Tuple<bool, string> resAnswers = new Tuple<bool, string>(false, null);
                byte[] message_pack;
                byte[] data = new byte[3];
                BitConverter.GetBytes((UInt16)CommandId.UAVLINK_CMD_ARM).CopyTo(data, 0);
                data[2] = armdisarm;

                Uavlink_message_t message = new Uavlink_message_t();
                message.Msgid = MessageId.UAVLINK_MSG_ID_COMMAND;
                message.LenPayload = 3;
                message.Payload = data;
                message.Encode(out message_pack);
                SendMessage(message);
                resAnswers = Tuple.Create(true, "send command ok");
                return resAnswers;
            };
            var task = new Task<Tuple<bool, string>>(sendCommand);
            task.Start();
            return task;
        }

        public void SendCommandLand()
        {
            byte[] message_pack;

            Uavlink_message_t message = new Uavlink_message_t();
            message.Msgid = MessageId.UAVLINK_MSG_ID_COMMAND;
            message.LenPayload = 2;
            message.Payload = BitConverter.GetBytes((UInt16)CommandId.UAVLINK_CMD_LAND);
            message.Encode(out message_pack);

            SendMessage(message);
        }

        private void HandleSendManualControl()
        {
            Uavlink_msg_manual_control_t manual_msg = new Uavlink_msg_manual_control_t();
            manual_msg.x = 500;
            manual_msg.y = 0;
            manual_msg.z = 0;
            manual_msg.r = 0;

            byte[] manual_pack;
            manual_msg.Encode(out manual_pack);

            Uavlink_message_t message = new Uavlink_message_t();
            message.Msgid = MessageId.UAVLINK_MSG_ID_MANUAL_CONTROL;
            message.LenPayload = (sbyte)manual_pack.Length;
            message.Payload = manual_pack;

            while (true)
            {
                SendMessage(message);
                Thread.Sleep(100);
            }
        }

        private void SendMessage(Uavlink_message_t message)
        {
            _sendQueue.Add(message);
        }
    }
}
