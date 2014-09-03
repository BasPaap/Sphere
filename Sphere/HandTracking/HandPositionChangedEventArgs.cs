using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bas.Sphere.HandTracking
{
    public sealed class HandPositionChangedEventArgs : EventArgs
    {
        public float TotalProximity { get; set; }
        public float LeftHandProximity { get; set; }
        public float RightHandProximity { get; set; }

        public HandPositionChangedEventArgs(float totalProximity, float leftHandProximity, float rightHandProximity)
        {
            TotalProximity = totalProximity;
            LeftHandProximity = leftHandProximity;
            RightHandProximity = rightHandProximity;
        }
    }
}
