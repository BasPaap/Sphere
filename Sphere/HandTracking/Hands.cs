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
        private float lastLeftHandProximity = 0.0f;
        private float lastRightHandProximity = 0.0f;
        private const float maxHandDistanceToEdge = 200.0f;
        private VisionType currentVisionType = VisionType.Death;

        public Hands()
        {
            this.timer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);
            this.timer.Tick += timer_Tick;

            if (Settings.Default.SphereCenter == null)
            {
                Settings.Default.SphereCenter = Vector.Zero;                
            }
        }

        private int currentHandlessFrameCount = 0;
        private const int maxHandlessFrameCount = 30;

        private Frame previousFrameWithHands = null;

        void timer_Tick(object sender, EventArgs e)
        {
            if (IsEnabled && this.leapController != null)
            {
                var frame = this.leapController.Frame();
                float currentHandProximity = 0.0f;
                float currentLeftHandProximity = 0.0f;
                float currentRightHandProximity = 0.0f;

                if (frame != null &&
                    frame.Hands.Count > 0)
                {
                    Debug.WriteLine("{0}\tNew frame, {1} hands.", DateTime.Now.ToLongTimeString(), frame.Hands.Count);
                    this.previousFrameWithHands = frame;
                    this.currentHandlessFrameCount = 0;
                }
                else 
                {
                    if (frame != null)
                    {
                        Debug.WriteLine("{0}\tprevious frame, {1} hands.", DateTime.Now.ToLongTimeString(), frame.Hands.Count);
                    }

                    this.currentHandlessFrameCount++;

                    if (this.currentHandlessFrameCount > maxHandlessFrameCount)
                    {
                        this.previousFrameWithHands = null;
                    }
                }

                if (previousFrameWithHands != null)
                {
                    currentLeftHandProximity = GetHandProximity(this.previousFrameWithHands.Hands.FirstOrDefault(h => h.IsLeft));
                    currentRightHandProximity = GetHandProximity(this.previousFrameWithHands.Hands.FirstOrDefault(h => h.IsRight));
                    currentHandProximity = GetTotalHandProximity(this.previousFrameWithHands);
                    TestForSummonGesture(this.previousFrameWithHands);
                    TestForVisionSelectionGesture(this.previousFrameWithHands);
                }
                
                // If the proximity has changed, fire the event.
                if (currentHandProximity != lastHandProximity &&
                    currentLeftHandProximity != lastLeftHandProximity &&
                    currentRightHandProximity != lastRightHandProximity &&
                    HandPositionChanged != null)
                {
                    lastHandProximity = currentHandProximity;
                    lastLeftHandProximity = currentLeftHandProximity;
                    lastRightHandProximity = currentRightHandProximity;
                    HandPositionChanged(this, new HandPositionChangedEventArgs(currentHandProximity, currentLeftHandProximity, currentRightHandProximity));
                }
            }
        }

        private float GetHandProximity(Hand hand)
        {
            if (hand != null)
            {
                var handDistance = hand.StabilizedPalmPosition.DistanceTo(Settings.Default.SphereCenter) - Settings.Default.SphereRadius;
                return GetProximityFromDistance(handDistance);
            }

            return 0.0f;
        }

        private float GetTotalHandProximity(Frame frame)
        {
            const int numSupportedHands = 2;
            var totalProximity = 0.0f;

            // Get the proximity, which is a product of both hands' distance to the edge of the sphere.
            foreach (var hand in frame.Hands.Take(numSupportedHands))
            {
                totalProximity += GetHandProximity(hand) / (float)numSupportedHands; // Divide by the number of supported hands so that one hand can only activate it halfway.
            }

            return totalProximity;
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
            if (VisionSummoned != null &&
                frame.Hands.Count == 2 &&
                frame.Hands[0].PalmNormal.DistanceTo(Vector.Up) < 1.0f &&
                frame.Hands[1].PalmNormal.DistanceTo(Vector.Up) < 1.0f)
                
            {
                VisionSummoned(this, new VisionSummonedEventArgs(this.currentVisionType));
            }
        }
                
        private float GetProximityFromDistance(float currentHandDistanceToEdge)
        {
            // Proximity is 1 if we are touching the edge, 0 if we are the maximum distance away from the edge.
            var proximity = 1.0f - currentHandDistanceToEdge / maxHandDistanceToEdge;
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

        public event EventHandler<HandPositionChangedEventArgs> HandPositionChanged; // Wordt gefired als er een wijziging in de handen is
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
                    Settings.Default.SphereCenter = (frame.Hands[0].StabilizedPalmPosition + frame.Hands[1].StabilizedPalmPosition) / 2.0f;
                    Settings.Default.SphereRadius = frame.Hands[0].StabilizedPalmPosition.DistanceTo(Settings.Default.SphereCenter);
                    return true;
                }
            }

            return false;
        }
    }
}
