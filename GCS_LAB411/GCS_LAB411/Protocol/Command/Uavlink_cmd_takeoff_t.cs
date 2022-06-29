using GCS_Comunication.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.Protocol.Command
{
    public class Uavlink_cmd_takeoff_t
    {
        private static int LENGTH = 18;
        public float Altitude;
        public Uavlink_cmd_takeoff_t()
        {
            Altitude = 0.0f;
        }

        public void Encode(out byte[] _data)
        {
            byte[] data = new byte[LENGTH];
            int index = 0;

            
            BitConverter.GetBytes((UInt16)CommandId.UAVLINK_CMD_TAKEOFF).CopyTo(data, index);
            BitConverter.GetBytes(Altitude).CopyTo(data, index += 2);
            _data = data;
        }
    }
}
