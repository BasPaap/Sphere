using Bas.Sphere.ShaderEffects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
        MediaPlayer visionMediaPlayer;

        public Vision()
        {
            InitializeComponent();

            underwaterWithTransparencyEffect = Resources["UnderwaterWithTransparencyEffect"] as UnderwaterWithTransparencyEffect;

            this.shaderTimer = new DispatcherTimer(DispatcherPriority.Normal); // To stop the animation from jittering when it's not animating.
            this.shaderTimer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);
            this.shaderTimer.Tick += shaderTimer_Tick;            

            this.visionMediaPlayer = new MediaPlayer();
            this.visionMediaPlayer.Open(new Uri("pack://siteoforigin:,,,/Audio/vision.wav"));

            FileNames = new Collection<string>(Directory.EnumerateFiles(System.IO.Path.Combine(Environment.CurrentDirectory, Properties.Settings.Default.VisionsFolder), "*.png").OrderBy(f => f).ToList());
            FileName = FileNames.First();
        }
        
        private UnderwaterWithTransparencyEffect underwaterWithTransparencyEffect;

        void shaderTimer_Tick(object sender, EventArgs e)
        {
            this.underwaterWithTransparencyEffect.Timer += this.shaderTimer.Interval.TotalSeconds;
        }

        private DispatcherTimer shaderTimer;

        public void Reveal()
        {
            // Only start the reveal if the vision is not already being revealed.

            if (!this.shaderTimer.IsEnabled)
            {
                this.visionMediaPlayer.Position = TimeSpan.Zero;
                this.visionMediaPlayer.Play();

                Debug.WriteLine("{0}\tRevealing vision.", new [] { DateTime.Now.ToLongTimeString() });
            
                // Start the shader effect
                this.shaderTimer.Start();

                var revealStoryboard = Resources["RevealStoryboard"] as Storyboard;
                revealStoryboard.Begin();
            }
        }

        public void Hide()
        {
            var hideStoryboard = Resources["HideStoryboard"] as Storyboard;
            hideStoryboard.Begin();
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
               
        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(Vision), new PropertyMetadata(null));
               
        public Collection<string> FileNames
            {
            get { return (Collection<string>)GetValue(FileNamesProperty); }
            set { SetValue(FileNamesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileNames.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileNamesProperty =
            DependencyProperty.Register("FileNames", typeof(Collection<string>), typeof(Vision), new PropertyMetadata(null));


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
