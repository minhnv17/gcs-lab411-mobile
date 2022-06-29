using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.Protocol.Uavlink
{
    public class Uavlink_msg_manual_control_t
    {
        private static int LENGTH = 16;
        public int x;
        public int y;
        public int z;
        public int r;
        public Uavlink_msg_manual_control_t()
        {

        }

        public void Encode(out byte[] _data)
        {
            byte[] data = new byte[LENGTH];
            int index = 0;
            BitConverter.GetBytes(x).CopyTo(data, index);
            BitConverter.GetBytes(y).CopyTo(data, index += 4);
            BitConverter.GetBytes(z).CopyTo(data, index += 4);
            BitConverter.GetBytes(r).CopyTo(data, index += 4);

            _data = data;
        }
    }
}
