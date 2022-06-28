using GCS_Comunication.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.Protocol.Command
{
    public class Uavlink_cmd_flyto_t
    {
        public byte AllWP;
        public int WPId;
        public Uavlink_cmd_flyto_t()
        {
            AllWP = 1;
            WPId = 0;
        }

        public void Encode(out byte[] _data)
        {
            byte[] data = new byte[9];
            int index = 0;

            BitConverter.GetBytes((int)CommandId.UAV_CMD_FLYTO).CopyTo(data, index);
            BitConverter.GetBytes(AllWP).CopyTo(data, index += 4);
            BitConverter.GetBytes(WPId).CopyTo(data, index += 1);
            _data = data;
        }
    }
}
