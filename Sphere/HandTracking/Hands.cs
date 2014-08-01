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
            get; // Geef de status van de leap terug
            set; // Enable of disable de leap;
        }

        public event EventHandler<HandProximityChangedEventArgs> HandProximityChanged; // Wordt gefired als er een wijziging in de handproximity is
        public event EventHandler<VisionSummonedEventArgs> VisionSummoned; // Wordt gefired als iemand een bepaald vision-gebaar maakt.

        /// <summary>
        /// Stores the current hand position as the beginning of the "active" zone (where proximity is 1).
        /// </summary>
        public void Calibrate()
        {
            if (IsEnabled)
            {
                throw new NotImplementedException();
            }
        }
    }
}
