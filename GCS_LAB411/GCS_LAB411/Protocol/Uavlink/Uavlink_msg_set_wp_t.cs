using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.Protocol.Uavlink
{
    public class Uavlink_msg_set_wp_t
    {
        private static int LENGTH = 14;
        private Int16 _wpId;
        private float _targetX;
        private float _targetY;
        private float _targetZ;

        public void Encode(out byte[] _data)
        {
            byte[] data = new byte[LENGTH];
            int index = 0;

            BitConverter.GetBytes(_wpId).CopyTo(data, index);
            BitConverter.GetBytes(_targetX).CopyTo(data, index += 2);
            BitConverter.GetBytes(_targetY).CopyTo(data, index += 4);
            BitConverter.GetBytes(_targetZ).CopyTo(data, index += 4);
            _data = data;
        }
    }
}
