using GCS_Comunication.Protocol;
using GCS_Comunication.Protocol.Uavlink;
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
        public delegate void DelegateLocalPos(Uavlink_msg_local_position_t tele);
        public delegate void DelegateGlobalPos(Uavlink_msg_global_position_t tele);

        public event DelegateState StateChanged;
        public event DelegateLocalPos LocalPosChanged;
        public event DelegateGlobalPos GlobalPosChanged;

        private Udp_Connect _connect;

        private Thread _receivingThread;
        private Thread _parsingThread;
        private Thread _sendingThread;

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

            _receivingThread.Start();
            _parsingThread.Start();
            _sendingThread.Start();
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
    }
}
