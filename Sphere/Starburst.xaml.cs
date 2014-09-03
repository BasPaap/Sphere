using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Bas.Sphere
{
    /// <summary>
    /// Interaction logic for Starburst.xaml
    /// </summary>
    public partial class Starburst : UserControl, INotifyPropertyChanged
    {
        public Starburst()
        {
            InitializeComponent();

            this.timer = new DispatcherTimer();
            this.timer.Tick += timer_Tick;
            this.timer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            RotationAngle += RotationSpeed * timer.Interval.TotalSeconds;

            if (RotationAngle > 360.0)
            {
                RotationAngle = RotationAngle % 360.0;
            }

            RotationAngle2 = rotationAngle * ParallaxDistance;
            RotationAngle3 = rotationAngle * ParallaxDistance * 2.0;
        }
        
        public double RotationSpeed
        {
            get { return (double)GetValue(RotationSpeedProperty); }
            set { SetValue(RotationSpeedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RotationSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotationSpeedProperty =
            DependencyProperty.Register("RotationSpeed", typeof(double), typeof(Starburst), new PropertyMetadata(0.0));



        public Boolean IsRevealed
        {
            get { return (Boolean)GetValue(IsRevealedProperty); }
            set { SetValue(IsRevealedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRevealed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRevealedProperty =
            DependencyProperty.Register("IsRevealed", typeof(Boolean), typeof(Starburst), new PropertyMetadata(false, IsRevealedChanged));

        private static void IsRevealedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var starburst = d as Starburst;
            var revealStoryboard = starburst.Resources["RevealStoryboard"] as Storyboard;
            var dissolveStoryboard = starburst.Resources["DissolveStoryboard"] as Storyboard;

            if ((bool)e.NewValue == true)
            {
                starburst.timer.Start();
                revealStoryboard.Begin();
                starburst.AmbientSoundMediaElement.Play();
            }
            else
            {
                dissolveStoryboard.Begin();
            }
        }
        
        public double ParallaxDistance
        {
            get { return (double)GetValue(ParallaxDistanceProperty); }
            set { SetValue(ParallaxDistanceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParallaxDistance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParallaxDistanceProperty =
            DependencyProperty.Register("ParallaxDistance", typeof(double), typeof(Starburst), new PropertyMetadata(0.0));



        private double rotationAngle;

        public double RotationAngle
        {
            get { return rotationAngle; }
            set
            {
                rotationAngle = value;
                NotifyPropertyChanged();
            }
        }

        private double rotationAngle2;

        public double RotationAngle2
        {
            get { return rotationAngle2; }
            set
            {
                rotationAngle2 = value;
                NotifyPropertyChanged();
            }
        }

        private double rotationAngle3;

        public double RotationAngle3
        {
            get { return rotationAngle3; }
            set
            {
                rotationAngle3 = value;
                NotifyPropertyChanged();
            }
        }

        private double firstGradientStop;

        public double FirstGradientStop
        {
            get { return firstGradientStop; }
            set 
            { 
                firstGradientStop = value;
                NotifyPropertyChanged();
            }
        }

        private double secondGradientStop;

        public double SecondGradientStop
        {
            get { return secondGradientStop; }
            set
            {
                secondGradientStop = value;
                NotifyPropertyChanged();
            }
        }
        
        private DispatcherTimer timer;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void DissolveStoryboard_Completed(object sender, EventArgs e)
        {
            this.timer.Stop();
            AmbientSoundMediaElement.Stop();                                
        }
        
        private void RepeatingMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).Play();
        }
    }
}
