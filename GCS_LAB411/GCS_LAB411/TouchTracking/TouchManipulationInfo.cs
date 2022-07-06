using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCS_LAB411.TouchTracking
{
    class TouchManipulationInfo
    {
        public SKPoint PreviousPoint { set; get; }

        public SKPoint NewPoint { set; get; }
    }
}
