using GCS_Comunication.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.Protocol.Command
{
    public class Uavlink_cmd_takeoff_t
    {
        public float Altitude;
        public Uavlink_cmd_takeoff_t()
        {
            Altitude = 0;
        }

        public void Encode(out byte[] _data)
        {
            byte[] data = new byte[8];
            int index = 0;

            BitConverter.GetBytes((int)CommandId.UAV_CMD_TAKEOFF).CopyTo(data, index);
            BitConverter.GetBytes(Altitude).CopyTo(data, index += 4);
            _data = data;
        }
    }
}
