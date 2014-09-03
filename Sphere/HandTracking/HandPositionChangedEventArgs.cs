using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bas.Sphere.HandTracking
{
    public sealed class HandPositionChangedEventArgs : EventArgs
    {
        public float TotalProximity { get; set; }

        public HandPositionChangedEventArgs(float totalProximity)
        {
            TotalProximity = totalProximity;
        }
    }
}
