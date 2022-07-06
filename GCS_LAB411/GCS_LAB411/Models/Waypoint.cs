using System;
using System.Collections.Generic;
using System.Text;
using VTGCS_WPF.Models;

namespace GCS_LAB411.Models
{
    public class Waypoint : PropertyChangedBase 
    {
        private int _waypointID;
        public int WaypointID
        {
            get { return _waypointID; }
            set { SetProperty(ref _waypointID, value); }
        }

        private float _posX;
        public float PosX
        {
            get { return _posX; }
            set { SetProperty(ref _posX, value); }
        }

        private float _posY;
        public float PosY
        {
            get { return _posY; }
            set { SetProperty(ref _posY, value); }
        }

        private bool _isComplete;
        public bool IsComplete
        {
            get { return _isComplete; }
            set { SetProperty(ref _isComplete, value); }
        }
    }
}
