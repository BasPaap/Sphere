using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Bas.Sphere.Extensions;

namespace Bas.Sphere.Converters
{
    public class BarrelDistortionToScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
            if (value is double)
            {
                var barrelDistortionAmount = (double)value;

                return 1.0 + 0.4636678 * barrelDistortionAmount;
            }

            return 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
