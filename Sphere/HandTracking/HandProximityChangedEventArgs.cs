using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bas.Sphere.HandTracking
{
    public sealed class HandProximityChangedEventArgs : EventArgs
    {
        public double Proximity { get; set; }

        public HandProximityChangedEventArgs(double proximity)
        {
            Proximity = proximity;
        }
    }
}
