using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bas.Sphere.Converters
{
    public class BrightnessToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double minimumOpacity;

            if (!double.TryParse((string)parameter, NumberStyles.Float, CultureInfo.InvariantCulture, out minimumOpacity))
            {
                minimumOpacity = 0.0;
            }

            if (value is double)
            {
                var totalOpacity = (double)value;

                var range = 1.0 - minimumOpacity;
                                
                var opacity = 1.0 - (minimumOpacity + (totalOpacity * range));

                return opacity;
            }

            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
