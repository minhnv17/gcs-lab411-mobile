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
        private const int UAVLINK_MSG_ID_STATE_LEN = 4;
        private sbyte _connected;
        private sbyte _armed;
        private sbyte _mode;
        private sbyte _battery;

        public sbyte Connected
        {
            get { return _connected; }
        }
        public sbyte Armed
        {
            get { return _armed; }
        }
        public sbyte Mode
        {
            get { return _mode; }
        }
        public sbyte Battery
        {
            get { return _battery; }
        }

        public Uavlink_msg_state_t()
        {

        }
        public void Decode(byte[] data)
        {
            _connected = (sbyte)data[0];
            _armed = (sbyte)data[1];
            _mode = (sbyte)data[2];
            _battery = (sbyte)data[3];
        }
    }
}
