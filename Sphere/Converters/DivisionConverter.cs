using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bas.Sphere.Converters
{
    public class DivisionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double doubleParameter;

            if (value is double && double.TryParse((string)parameter, NumberStyles.Float, CultureInfo.InvariantCulture, out doubleParameter))
            {
                return (double)value / doubleParameter;
            }

            return 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
