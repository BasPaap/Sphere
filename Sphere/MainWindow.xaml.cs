using System;
using System.Collections.Generic;
using System.Linq;
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
using Bas.Sphere.Properties;
using System.Windows.Threading;
using Bas.Sphere.HandTracking;

namespace Bas.Sphere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            this.calibrationTimer = new DispatcherTimer();
            this.calibrationTimer.Interval = TimeSpan.FromSeconds(1);
            this.calibrationTimer.Tick += calibrationTimer_Tick;

            this.hands = new Hands();
            this.hands.HandProximityChanged += hands_HandProximityChanged;
            this.hands.VisionSummoned += hands_VisionSummoned;
            this.hands.IsEnabled = Settings.Default.IsHandTrackingEnabled;
            
            Settings.Default.PropertyChanged += Settings_PropertyChanged;
        }

        void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsHandTrackingEnabled")
            {
                this.hands.IsEnabled = Settings.Default.IsHandTrackingEnabled;
            }
        }

        void hands_VisionSummoned(object sender, VisionSummonedEventArgs e)
        {
            Vision.Type = e.Vision;
            Vision.Reveal();            
        }

        void hands_HandProximityChanged(object sender, HandProximityChangedEventArgs e)
        {
            HandProximity = e.Proximity;
        }
        
        private void ExecuteSaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            MessageBox.Show("Opgeslagen!", "Sphere", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public double HandProximity
        {
            get { return (double)GetValue(HandProximityProperty); }
            set { SetValue(HandProximityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HandProximity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HandProximityProperty =
            DependencyProperty.Register("HandProximity", typeof(double), typeof(MainWindow), new PropertyMetadata(0.0));

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Settings.Default.IsControlsPanelVisible = !Settings.Default.IsControlsPanelVisible;
                    break;
                case Key.D:
                    Settings.Default.IsDebugControlsPanelVisible = !Settings.Default.IsDebugControlsPanelVisible;
                    break;
                case Key.P:
                    Settings.Default.IsProjectionControlsPanelVisible = !Settings.Default.IsProjectionControlsPanelVisible;
                    break;
                case Key.T:
                    IsTestImageVisibleCheckBox.IsChecked = !IsTestImageVisibleCheckBox.IsChecked;
                    break;
                case Key.F:
                    if (this.WindowStyle == System.Windows.WindowStyle.None)
                    {
                        this.ResizeMode = System.Windows.ResizeMode.CanResize;
                        this.WindowState = System.Windows.WindowState.Normal;
                        this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                    }
                    else
                    {
                        this.ResizeMode = System.Windows.ResizeMode.NoResize;

                        this.WindowState = System.Windows.WindowState.Maximized;
                        this.WindowStyle = System.Windows.WindowStyle.None;
                    }
                    break;
                default:
                    break;
            }
        }

        private void RevealVisionButton_Click(object sender, RoutedEventArgs e)
        {
            Vision.Reveal();
        }

        private void CalibrationButton_Click(object sender, RoutedEventArgs e)
        {
            CalibrationButton.IsEnabled = false;
            this.calibrationCountDown = 3;
            CalibrationTextBlock.Text = this.calibrationCountDown.ToString();
            CalibrationTextBlock.Visibility = System.Windows.Visibility.Visible;
            this.calibrationTimer.Start();
        }
        
        void calibrationTimer_Tick(object sender, EventArgs e)
        {
            this.calibrationCountDown--;
            CalibrationTextBlock.Text = this.calibrationCountDown.ToString();

            if (this.calibrationCountDown == 0)
            {
                var calibrationSucceeded = hands.Calibrate();
                CalibrationTextBlock.Text = calibrationSucceeded ? "!" : "?";                
            }
            else if (this.calibrationCountDown == -1)
            {
                this.calibrationTimer.Stop();
                CalibrationTextBlock.Visibility = System.Windows.Visibility.Hidden;
                CalibrationButton.IsEnabled = true;                
            }
        }

        int calibrationCountDown;
        DispatcherTimer calibrationTimer;
        private Hands hands;
    }
}
