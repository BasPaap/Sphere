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
using Bas.Sphere.Extensions;

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
                    ControlsStackPanel.Visibility = ControlsStackPanel.Visibility.ToOpposite();
                    break;
                case Key.D:
                    DebugControlsStackPanel.Visibility = DebugControlsStackPanel.Visibility.ToOpposite();
                    break;
                case Key.P:
                    ProjectionControlsStackPanel.Visibility = ProjectionControlsStackPanel.Visibility.ToOpposite();
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


    }
}
