using Bas.Sphere.Properties;
using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Bas.Sphere.Extensions;

namespace Bas.Sphere.HandTracking
{
    public class Hands
    {
        private Controller leapController;
        private DispatcherTimer timer = new DispatcherTimer();
        private float lastHandProximity = 0.0f;
        private const float maxHandDistance = 500.0f;

        public Hands()
        {
            this.timer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);
            this.timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (IsEnabled && this.leapController != null)
            {
                var frame = this.leapController.Frame();
                float currentHandProximity = 0.0f;

                if (frame != null &&
                    frame.Hands.Count == 2)
                {
                    var currentHandDistance = frame.Hands[0].StabilizedPalmPosition.DistanceTo(frame.Hands[1].StabilizedPalmPosition);
                    currentHandProximity = GetProximityFromDistance(currentHandDistance);

                    if (currentHandProximity != lastHandProximity &&
                        HandProximityChanged != null)
                    {
                        lastHandProximity = currentHandProximity;
                        HandProximityChanged(this, new HandProximityChangedEventArgs(currentHandProximity));
                    }
                }

                if (currentHandProximity != lastHandProximity &&
                    HandProximityChanged != null)
                {
                    lastHandProximity = currentHandProximity;
                    HandProximityChanged(this, new HandProximityChangedEventArgs(currentHandProximity));
                }
            }
        }

        private float GetProximityFromDistance(float currentHandDistance)
        {
            var length = maxHandDistance - Settings.Default.ActiveZoneHandDistance;
            var distance = currentHandDistance - Settings.Default.ActiveZoneHandDistance;
            var proximity = 1.0f - distance / length;

            return proximity.Clamp(0.0f, 1.0f);
        }

        private void ConnectLeap()
        {
            if (this.leapController == null)
            {
                this.leapController = new Controller();
            }

            this.timer.IsEnabled = true;
        }

        private void DisconnectLeap()
        {
            if (this.leapController != null)
            {
                this.leapController = null;
            }

            this.timer.IsEnabled = false;
        }

        public bool IsEnabled
        {
            get
            {
                return (this.leapController != null && this.leapController.IsConnected);
            }

            set
            {
                if (value == true && !IsEnabled)
                {
                    ConnectLeap();
                }
                else
                {
                    DisconnectLeap();
                }
            }
        }

        public event EventHandler<HandProximityChangedEventArgs> HandProximityChanged; // Wordt gefired als er een wijziging in de handproximity is
        public event EventHandler<VisionSummonedEventArgs> VisionSummoned; // Wordt gefired als iemand een bepaald vision-gebaar maakt.

        /// <summary>
        /// Stores the current hand position as the beginning of the "active" zone (where proximity is 1).
        /// </summary>
        public bool Calibrate()
        {
            if (IsEnabled)
            {
                var frame = this.leapController.Frame();

                if (frame != null &&
                    frame.Hands.Count == 2)
                {
                    Settings.Default.ActiveZoneHandDistance = frame.Hands[0].StabilizedPalmPosition.DistanceTo(frame.Hands[1].StabilizedPalmPosition);
                    return true;
                }
            }

            return false;
        }
    }
}
