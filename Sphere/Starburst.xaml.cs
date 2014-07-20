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
            this.timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            RotationAngle += RotationSpeed * timer.Interval.TotalSeconds;

            if (RotationAngle > 360.0)
            {
                RotationAngle = RotationAngle % 360.0;
            }
        }
        
        public double RotationSpeed
        {
            get { return (double)GetValue(RotationSpeedProperty); }
            set { SetValue(RotationSpeedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RotationSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotationSpeedProperty =
            DependencyProperty.Register("RotationSpeed", typeof(double), typeof(Starburst), new PropertyMetadata(0.0));
                
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


        private DispatcherTimer timer;

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
