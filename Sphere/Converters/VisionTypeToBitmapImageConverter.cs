using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Bas.Sphere.Converters
{
    public class VisionTypeToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is VisionType)
            {
                var fileName = string.Empty;

                switch ((VisionType)value)
                {
                    case VisionType.Death:
                        fileName = "skull holbein.png";
                        break;
                    case VisionType.Love:
                        fileName = "fall of man goltzius.png";
                        break;
                    case VisionType.Fortune:
                        fileName = "still life of coins francken.png";
                        break;
                    case VisionType.Travel:
                        fileName = "ship at sea edward moran.png";
                        break;
                    case VisionType.Treasure:
                        fileName = "treasure.png";
                        break;
                    case VisionType.Logo:
                        fileName = "logo.png";
                        break;
                    case VisionType.None:
                    default:
                        break;
                }

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(string.Format("pack://application:,,,/Images/{0}", fileName));
                    image.EndInit();

                    return image;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
