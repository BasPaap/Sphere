﻿using System;
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

        
        public double RevealProgress
        {
            get { return (double)GetValue(RevealProgressProperty); }
            set { SetValue(RevealProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RevealProgress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RevealProgressProperty =
            DependencyProperty.Register("RevealProgress", typeof(double), typeof(Starburst), new PropertyMetadata(0.0, RevealProgressChanged));

        private static void RevealProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var revealProgress = ((double)e.NewValue).Clamp(0.0, 1.0);

            var starburst = d as Starburst;

            starburst.FirstGradientStop = revealProgress;
            starburst.SecondGradientStop = (revealProgress * 2.0).Clamp(0.0, 1.0);
            
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
