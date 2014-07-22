using Bas.Sphere.ShaderEffects;
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
    /// Interaction logic for Vision.xaml
    /// </summary>
    public partial class Vision : UserControl, INotifyPropertyChanged
    {
        public Vision()
        {
            InitializeComponent();

            underwaterWithTransparencyEffect = Resources["UnderwaterWithTransparencyEffect"] as UnderwaterWithTransparencyEffect;

            this.shaderTimer = new DispatcherTimer();
            this.shaderTimer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);
            this.shaderTimer.Tick += shaderTimer_Tick;            
        }

        private UnderwaterWithTransparencyEffect underwaterWithTransparencyEffect;

        void shaderTimer_Tick(object sender, EventArgs e)
        {
            this.underwaterWithTransparencyEffect.Timer += this.shaderTimer.Interval.TotalSeconds;
        }

        private DispatcherTimer shaderTimer;

        public void Reveal()
        {
            // Start the shader effect
            this.shaderTimer.Start();            

            var revealStoryboard = Resources["RevealStoryboard"] as Storyboard;
            revealStoryboard.Begin();
        }

        private void RevealStoryboard_Completed(object sender, EventArgs e)
        {
            // Disable the shader effect to preserve resources
            this.shaderTimer.Stop();
        }
                        
        public VisionType Type
        {
            get { return (VisionType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(VisionType), typeof(Vision), new PropertyMetadata(VisionType.None));

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
