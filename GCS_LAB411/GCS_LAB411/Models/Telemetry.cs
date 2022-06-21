using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using VTGCS_WPF.Models;

namespace GCS_LAB411.Models
{
    public class Telemetry : BaseViewModel
    {
        public enum Mode
        {
            MANUAL,
            OFFBOARD,
            HOLD,
            POSITION,
            LAND
        };

        private bool _connected = false;
        public bool Connected
        {
            get => _connected;
            set => SetProperty(ref _connected, value);
        }

        private bool _arm = false;
        public bool Arm
        {
            get
            {
                return _arm;
            }
            set
            {
                SetProperty(ref _arm, value);
            }
        }

        private Mode _mode;
        public Mode CurMode
        {
            get
            {
                return _mode;
            }
            set
            {
                SetProperty(ref _mode, value);
            }
        }

        private float _battery;
        public float Battery
        {
            get
            {
                return _battery;
            }
            set
            {
                SetProperty(ref _battery, value);
            }
        }

        private bool _positionStatus = false;
        public bool PositionStatus
        {
            get
            {
                return _positionStatus;
            }
            set
            {
                SetProperty(ref _positionStatus, value);
            }
        }

        private float _altitude = 0;
        public float Altitude
        {
            get
            {
                return _altitude;
            }
            set
            {
                SetProperty(ref _altitude, value);
            }
        }

        private float _groundSpeed = 0;
        public float GroundSpeed
        {
            get
            {
                return _groundSpeed;
            }
            set
            {
                SetProperty(ref _groundSpeed, value);
            }
        }

        private float _positionX = 0;
        public float PositionX
        {
            get
            {
                return _positionX;
            }
            set
            {
                SetProperty(ref _positionX, value);
            }
        }

        private float _positionY = 0;
        public float PositionY
        {
            get
            {
                return _positionY;
            }
            set
            {
                SetProperty(ref _positionY, value);
            }
        }

        private double _latitude = 0;
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                SetProperty(ref _latitude, value);
            }
        }

        private double _longitude = 0;
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                SetProperty(ref _longitude, value);
            }
        }
    }
}
