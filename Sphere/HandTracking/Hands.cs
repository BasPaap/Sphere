using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bas.Sphere.HandTracking
{
    public class Hands
    {
        public bool IsEnabled
        {
            get;
            set;
        }

        public event EventHandler<HandProximityChangedEventArgs> HandProximityChanged;
        public event EventHandler<VisionSummonedEventArgs> VisionSummoned;

        public void Calibrate()
        {
            if (IsEnabled)
            {
                throw new NotImplementedException();
            }
        }
    }
}
