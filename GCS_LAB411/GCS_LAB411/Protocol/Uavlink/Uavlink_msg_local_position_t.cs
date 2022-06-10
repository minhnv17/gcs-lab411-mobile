using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.Protocol.Uavlink
{
    public class Uavlink_msg_local_position_t
    {
        private float _posX;
        private float _posY;
        private float _posz;
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
        public float _Posz
        {
            get { return _posz; }
            set { this._posz = value; }
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

            _posX = (float)BitConverter.ToDouble(data, index);
            _posY = (float)BitConverter.ToDouble(data, index += 4);
            _posz = (float)BitConverter.ToDouble(data, index += 4);
            _vx = (float)BitConverter.ToDouble(data, index += 4);
            _vy = (float)BitConverter.ToDouble(data, index += 4);
            _vz = (float)BitConverter.ToDouble(data, index += 4);
        }
    }
}
