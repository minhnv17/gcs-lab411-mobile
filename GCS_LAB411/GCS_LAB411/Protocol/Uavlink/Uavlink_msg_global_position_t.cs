using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS_Comunication.Protocol.Uavlink
{
    public class Uavlink_msg_global_position_t
    {
        private double _lat;
        private double _lon;
        private float _alt;
        private float _vx;
        private float _vy;
        private float _vz;

        public double Lat
        {
            get { return _lat; }
            set { this._lat = value; }
        }

        public double Lon
        {
            get { return _lon; }
            set { this._lon = value; }
        }
        public float Alt
        {
            get { return _alt; }
            set { this._alt = value; }
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

            _lat = (double)BitConverter.ToInt32(data, index) / 10000000.0f;
            _lon = (double)BitConverter.ToInt32(data, index += 4) / 10000000.0f;
            _alt = (float)BitConverter.ToInt16(data, index += 4) / 100.0f;
            _vx = (float)BitConverter.ToInt16(data, index += 2) / 100.0f;
            _vy = (float)BitConverter.ToInt16(data, index += 2) / 100.0f;
            _vz = (float)BitConverter.ToInt16(data, index += 2) / 100.0f;
        }
    }
}
