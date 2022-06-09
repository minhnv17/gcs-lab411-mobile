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

        public void uavlink_state_decode(byte[] _byte)
        {
            connected = (sbyte)_byte[0];
            armed = (sbyte)_byte[1];
            mode = (sbyte)_byte[2];
            battery = (sbyte)_byte[3];
        }
    }
}
