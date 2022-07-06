using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.Protocol.Uavlink
{
    public class Uavlink_msg_setwp_t
    {
        private static int LENGTH = 16;
        public int WaypointID;
        public float TargetX;
        public float TargetY;
        public float TargetZ;

        public Uavlink_msg_setwp_t()
        {
            WaypointID = 0;
            TargetX = 0;
            TargetY = 0;
            TargetZ = 0;
        }

        public void Encode(out byte[] _data)
        {
            byte[] data = new byte[LENGTH];
            int index = 0;

            BitConverter.GetBytes(WaypointID).CopyTo(data, index);
            BitConverter.GetBytes(TargetX).CopyTo(data, index += 4);
            BitConverter.GetBytes(TargetY).CopyTo(data, index += 4);
            BitConverter.GetBytes(TargetZ).CopyTo(data, index += 4);
            _data = data;
        }
    }
}
