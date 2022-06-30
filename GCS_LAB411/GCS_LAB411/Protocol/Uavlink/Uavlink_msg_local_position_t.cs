using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.Protocol.Uavlink
{
    public class Uavlink_msg_local_position_t
    {
        private float _posX;
        private float _posY;
        private float _posZ;
        private float _vx;
        private float _vy;
        private float _vz;

        public float PosX
        {
            get { return _posX; }
            set { this._posX = value; }
        }

        public float PosY
        {
            get { return _posY; }
            set { this._posY = value; }
        }
        public float PosZ
        {
            get { return _posZ; }
            set { this._posZ = value; }
        }
        public float Vx
        {
            get { return _vx; }
            set { this._vx = value; }
        }
        public float Vy
        {
            get { return _vy; }
            set { this._vy = value; }
        }
        public float Vz
        {
            get { return _vz; }
            set { this._vz = value; }
        }

        public void Decode(byte[] data)
        {
            int index = 0;

            _posX = (float)BitConverter.ToInt16(data, index) / 1000.0f;
            _posY = (float)BitConverter.ToInt16(data, index += 2) / 1000.0f;
            _posZ = (float)BitConverter.ToInt16(data, index += 2) / 1000.0f;
            _vx = (float)BitConverter.ToInt16(data, index += 2) / 100.0f;
            _vy = (float)BitConverter.ToInt16(data, index += 2) / 100.0f;
            _vz = (float)BitConverter.ToInt16(data, index += 2) / 100.0f;
        }
    }
}
