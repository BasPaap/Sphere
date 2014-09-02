using Bas.Sphere.Properties;
using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Bas.Sphere.Extensions;
using System.Diagnostics;

namespace Bas.Sphere.HandTracking
{
    public class Hands
    {
        private Controller leapController;
        private DispatcherTimer timer = new DispatcherTimer();
        private float lastHandProximity = 0.0f;
        private const float maxHandDistance = 500.0f;
        private VisionType currentVisionType = VisionType.Death;

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
                    currentHandProximity = GetHandProximity(frame, currentHandProximity);
                    TestForSummonGesture(frame);
                    TestForVisionSelectionGesture(frame);
                }
                
                // If the proximity has changed, fire the event.
                if (currentHandProximity != lastHandProximity &&
                    HandProximityChanged != null)
                {
                    lastHandProximity = currentHandProximity;
                    HandProximityChanged(this, new HandProximityChangedEventArgs(currentHandProximity));
                }
            }
        }

        private float GetHandProximity(Frame frame, float currentHandProximity)
        {
            // Get the proximity of both hands to the activation zone.
            var currentHandDistance = frame.Hands[0].StabilizedPalmPosition.DistanceTo(frame.Hands[1].StabilizedPalmPosition);
            currentHandProximity = GetProximityFromDistance(currentHandDistance);
                        
            return currentHandProximity;
        }

        private DateTime lastVisionSelectionGestureAppearanceTime = DateTime.MinValue;
        private TimeSpan requiredVisionSelectionGestureDuration = TimeSpan.FromSeconds(0.5);
        private VisionType potentiallySelectedVisionType = VisionType.None;

        private void TestForVisionSelectionGesture(Frame frame)
        {            
                var fists = from h in frame.Hands
                            where h.SphereRadius < 38.0f
                            select h;

                var visionType = VisionType.None;

                if (fists.Count() == 2)
                {
                    visionType = VisionType.Death;
                }
                else if (fists.Count() == 1)
                {
                    visionType = fists.First().IsLeft ? VisionType.Travel : VisionType.Love;
                }
                else
                {
                    visionType = VisionType.None;
                }

                if (visionType != this.potentiallySelectedVisionType)
                {
                    this.lastVisionSelectionGestureAppearanceTime = DateTime.Now;
                    this.potentiallySelectedVisionType = visionType;
                }

                if (DateTime.Now - this.lastVisionSelectionGestureAppearanceTime >= requiredVisionSelectionGestureDuration && 
                    this.potentiallySelectedVisionType != VisionType.None)
                {
                    this.currentVisionType = this.potentiallySelectedVisionType;
                    Debug.WriteLine("{0}\tSelecting vision {1}", DateTime.Now.ToLongTimeString(), this.currentVisionType);
                }
            
        }

        private void TestForSummonGesture(Frame frame)
        {
            // Test if the palms are turned upwards: if so, display the currently selected vision.
            if ((frame.Hands[0].PalmNormal.DistanceTo(Vector.Up) < 1.0f &&
                 frame.Hands[1].PalmNormal.DistanceTo(Vector.Up) < 1.0f) &&
                 VisionSummoned != null)
            {
                VisionSummoned(this, new VisionSummonedEventArgs(this.currentVisionType));
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
            
            this.timer.Start();
        }

        private void DisconnectLeap()
        {
            if (this.leapController != null)
            {
                this.leapController = null;
            }

            this.timer.Stop();
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
