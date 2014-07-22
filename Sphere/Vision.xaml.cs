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
    public partial class Vision : UserControl
    {
        public Vision()
        {
            InitializeComponent();

            underwaterEffect = Resources["UnderwaterEffect"] as UnderwaterEffect;

            this.shaderTimer = new DispatcherTimer();
            this.shaderTimer.Interval = TimeSpan.FromSeconds(1.0 / 3.0);
            this.shaderTimer.Tick += shaderTimer_Tick;            
        }

        private UnderwaterEffect underwaterEffect;

        void shaderTimer_Tick(object sender, EventArgs e)
        {
            this.underwaterEffect.Timer += this.shaderTimer.Interval.TotalSeconds;
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
    }
}
