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
        private bool isSelectingVision = false;
        private VisionType[] availableVisionTypes = new VisionType[] { VisionType.Death, VisionType.Treasure, VisionType.Travel };
        private int currentVisionTypeIndex = 0;
        
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

                var actualLeftHandProximity = (frame != null) ? GetHandProximity(frame.Hands.FirstOrDefault(h => h.IsLeft)) : 0.0f;
                var actualRightHandProximity = (frame != null) ? GetHandProximity(frame.Hands.FirstOrDefault(h => h.IsRight)) : 0.0f;

                currentLeftHandProximity = GetSmoothedProximity(actualLeftHandProximity, lastLeftHandProximity);
                currentRightHandProximity = GetSmoothedProximity(actualRightHandProximity, lastRightHandProximity);

                const float numSupportedHands = 2.0f;
                currentHandProximity = currentLeftHandProximity / numSupportedHands + currentRightHandProximity / numSupportedHands; // GetTotalHandProximity(this.previousFrameWithHands);
                Debug.WriteLine(currentHandProximity);
                TestForSummonGesture(frame);
                TestForVisionSelectionGesture(frame);
                
                // If the proximity has changed, fire the event.
                if ((currentHandProximity != lastHandProximity ||
                    currentLeftHandProximity != lastLeftHandProximity ||
                    currentRightHandProximity != lastRightHandProximity) &&
                    HandPositionChanged != null)
                {
                    lastHandProximity = currentHandProximity;
                    lastLeftHandProximity = currentLeftHandProximity;
                    lastRightHandProximity = currentRightHandProximity;
                    HandPositionChanged(this, new HandPositionChangedEventArgs(currentHandProximity, currentLeftHandProximity, currentRightHandProximity));                    
                }
            }
        }

        private float GetSmoothedProximity(float targetProximity, float currentProximity)
        {
            if (targetProximity != currentProximity)
            {
                if (targetProximity > currentProximity)
                {
                    var step = (float)timer.Interval.TotalSeconds;
                    var smoothedProximity = currentProximity + step;

                    return (smoothedProximity <= targetProximity) ? smoothedProximity : targetProximity;
                }
                else
                {
                    var step = 0.0f - (float)timer.Interval.TotalSeconds;
                    var smoothedProximity = currentProximity + step;

                    return (smoothedProximity >= targetProximity) ? smoothedProximity : targetProximity;
                }
            }
            else
            {
                return currentProximity;
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
            if (frame != null)
            {
                var leftHand = frame.Hands.SingleOrDefault(h => h.IsLeft);
                var rightHand = frame.Hands.SingleOrDefault(h => h.IsRight);

                if (leftHand != null && rightHand != null)
                {
                    if (rightHand.StabilizedPalmPosition.x < leftHand.StabilizedPalmPosition.x)
                    {
                        // Hands are crossed.
                        
                        if (!this.isSelectingVision)
                        {
                            this.isSelectingVision = true;
                            
                            // Test which hand is higher
                            if (rightHand.StabilizedPalmPosition.y > leftHand.StabilizedPalmPosition.y)
                            {
                                // Right hand is crossed over left.
                                // We use this gesture to select the next vision. 
                                this.currentVisionTypeIndex = (this.currentVisionTypeIndex < this.availableVisionTypes.Length) ? this.currentVisionTypeIndex + 1 : 0;
                                Debug.WriteLine("{0}\tHands crossed right over left, selected vision {1}", DateTime.Now.ToLongTimeString(), this.currentVisionTypeIndex);
                            }
                            else
                            {
                                // Left hand is crossed over right.
                                // We use this gesture to reset the vision to the first one.
                                this.currentVisionTypeIndex = 0;
                                Debug.WriteLine("{0}\tHands crossed left over right, reset vision to {1}", DateTime.Now.ToLongTimeString(), this.currentVisionTypeIndex);
                            }
                        }
                    }
                    else
                    {
                        if (this.isSelectingVision)
                        {
                            // Hands are no longer crossed, so we're no longer selecting anything.
                            this.isSelectingVision = false;
                            Debug.WriteLine("{0}\tNo longer crossed.", DateTime.Now.ToLongTimeString());
                        }                        
                    }
                }

            }
        }

        private void TestForSummonGesture(Frame frame)
        {
            // Test if the palms are turned upwards: if so, display the currently selected vision.
            if (VisionSummoned != null &&
                frame != null &&
                frame.Hands.Count == 2 &&
                frame.Hands[0].PalmNormal.DistanceTo(Vector.Up) < 1.0f &&
                frame.Hands[1].PalmNormal.DistanceTo(Vector.Up) < 1.0f)
            {
                VisionSummoned(this, new VisionSummonedEventArgs(this.availableVisionTypes[this.currentVisionTypeIndex]));
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
