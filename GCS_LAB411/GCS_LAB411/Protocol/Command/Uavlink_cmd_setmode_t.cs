using GCS_Comunication.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.Protocol.Command
{
    public class Uavlink_cmd_setmode_t
    {
        public byte Mode;
        public Uavlink_cmd_setmode_t()
        {
        }

        public void Encode(out byte[] _data)
        {
            byte[] data = new byte[3];
            int index = 0;

            BitConverter.GetBytes((UInt16)CommandId.UAVLINK_CMD_SETMODE).CopyTo(data, index);
            BitConverter.GetBytes(Mode).CopyTo(data, index += 2);
            _data = data;
        }
    }
}
