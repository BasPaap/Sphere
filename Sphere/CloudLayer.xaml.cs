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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Bas.Sphere.Extensions;

namespace Bas.Sphere
{
    /// <summary>
    /// Interaction logic for CloudLayer.xaml
    /// </summary>
    public partial class CloudLayer : UserControl, INotifyPropertyChanged
    {
        public CloudLayer()
        {
            InitializeComponent();

            this.timer = new DispatcherTimer();
            this.timer.Tick += timer_Tick;
            this.timer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);
            this.timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            var clampedAcceleration = Acceleration.Clamp(0.0, 1.0);

            CurrentSpeed = (CurrentSpeed + (this.timer.Interval.TotalSeconds * AccelerationRate * clampedAcceleration)).Clamp(MinimumSpeed, AccelerationRate * clampedAcceleration);
                        
            RotationAngle -= currentSpeed;
            
            if (RotationAngle < 0.0)
            {
                RotationAngle = 360.0 - RotationAngle;
            }
        }

        private DispatcherTimer timer;
        
        public double MinimumSpeed { get; set; }
        public double AccelerationRate { get; set; }



        public double Acceleration
        {
            get { return (double)GetValue(AccelerationProperty); }
            set { SetValue(AccelerationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Acceleration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AccelerationProperty =
            DependencyProperty.Register("Acceleration", typeof(double), typeof(CloudLayer), new PropertyMetadata(0.0));
        
        
        private double currentSpeed;

        public double CurrentSpeed
        {
            get { return currentSpeed; }
            set 
            { 
                currentSpeed = value;
                NotifyPropertyChanged();
            }
        }

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
        
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
