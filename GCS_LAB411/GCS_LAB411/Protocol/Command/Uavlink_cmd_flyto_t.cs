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
            byte[] data = new byte[10];
            int index = 0;

            BitConverter.GetBytes((UInt16)CommandId.UAVLINK_CMD_FLYTO).CopyTo(data, index);
            BitConverter.GetBytes((float)AllWP).CopyTo(data, index += 2);
            BitConverter.GetBytes(WPId).CopyTo(data, index += 4);
            _data = data;
        }
    }
}
