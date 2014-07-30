using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Bas.Sphere
{
    /// <summary>
    /// Interaction logic for TestImage.xaml
    /// </summary>
    public partial class TestImage : UserControl, INotifyPropertyChanged
    {
        public TestImage()
        {
            InitializeComponent();

            this.SizeChanged += TestImage_SizeChanged;
        }

        void TestImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrontDotMargin = new Thickness(0.0, this.ActualHeight / 2.0, 0.0, 0.0);

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("FrontDotMargin"));
            }
        }

        public Thickness FrontDotMargin { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
