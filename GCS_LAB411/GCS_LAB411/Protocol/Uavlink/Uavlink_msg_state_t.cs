using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GCS_Comunication.Protocol.Uavlink_message_t;

namespace GCS_Comunication.Protocol.Uavlink
{
    public class Uavlink_msg_state_t
    {
        public const int UAVLINK_MSG_ID_STATE_LEN = 4;
        public sbyte connected;
        public sbyte armed;
        public sbyte mode;
        public sbyte battery;
        public void Decode(byte[] data)
        {
            connected = (sbyte)data[0];
            armed = (sbyte)data[1];
            mode = (sbyte)data[2];
            battery = (sbyte)data[3];
        }
    }
}
