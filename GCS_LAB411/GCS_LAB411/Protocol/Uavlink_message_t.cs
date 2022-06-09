using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS_Comunication.Protocol
{

    public class Uavlink_message_t
    {
        public enum MessageId : byte
        {
            UAVLINK_MSG_ID_STATE = 0x01,
            UAVLINK_MSG_ID_GLOBAL_POSITION = 0x02,
            UAVLINK_MSG_ID_LOCAL_POSITION = 0x03,
            TakeoffCmd = 0x04,
            LandCmd = 0x05,
            FlytoCmd = 0x06,
            SetWPCmd = 0x07
        }

        public MessageId msgid;
        public sbyte len;
        public byte[] payload;
    }
}
