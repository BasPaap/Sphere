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
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public event EventHandler<HandProximityChangedEventArgs> HandProximityChanged;
        public event EventHandler<VisionSummonedEventArgs> VisionSummoned;
    }
}
