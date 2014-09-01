using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bas.Sphere.HandTracking
{
    public sealed class HandProximityChangedEventArgs : EventArgs
    {
        public float Proximity { get; set; }

        public HandProximityChangedEventArgs(float proximity)
        {
            Proximity = proximity;
        }
    }
}
