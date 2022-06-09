using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCS_Comunication.Protocol.Uavlink
{
    public class Uavlink_global_position_int_t
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

            _lat = BitConverter.ToDouble(data, index);
            _lon = BitConverter.ToDouble(data, index += 4);
            _alt = (float)BitConverter.ToDouble(data, index += 4);
            _vx = (float)BitConverter.ToDouble(data, index += 4);
            _vy = (float)BitConverter.ToDouble(data, index += 4);
            _vz = (float)BitConverter.ToDouble(data, index += 4);
        }
    }
}
