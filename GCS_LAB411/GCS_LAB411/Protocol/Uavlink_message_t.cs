using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS_Comunication.Protocol
{
    public enum MessageId : byte
    {
        UAVLINK_MSG_ID_STATE = 0x01,
        UAVLINK_MSG_ID_GLOBAL_POSITION = 0x02,
        UAVLINK_MSG_ID_LOCAL_POSITION = 0x03,
        UAVLINK_MSG_ID_COMMAND = 0x04
    }
    public class Uavlink_message_t
    {
        
        public Uavlink_message_t()
        {

        }

        MessageId _msgid;
        private sbyte _lenPayload;
        private byte[] _payload;

        public MessageId Msgid
        {
            get { return _msgid; }
            set { this._msgid = value; }
        }
        public sbyte LenPayload
        {
            get { return _lenPayload; }
            set { this._lenPayload = value; }
        }
        public byte[] Payload
        {
            get { return _payload; }
            set { this._payload = value; }
        }

        public void Encode(out byte[] pack)
        {
            byte[] data = new byte[1+1+_lenPayload];
            data[0] = (byte)_msgid;
            data[1] = (byte)_lenPayload;
            for (int i = 0; i< _lenPayload; i++)
            {
                data[i + 2] = _payload[i];
            }
            pack = data;
        }

        public void Decode(byte[] data)
        {
            _msgid = (MessageId)data[0];
            _lenPayload = (sbyte)data[1];
            _payload = new byte[_lenPayload];
            for (int i = 0; i < _lenPayload; i++)
            {
                _payload[i] = data[i + 2];
            }
        }
    }
}
