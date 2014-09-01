using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bas.Sphere.Converters
{
    class HandProximityToBooleanConverter : IValueConverter
    {
        private bool isRevealed = false;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                var proximity = (double)value;

                if (isRevealed)
                {
                    // If the starburst has already been revealed, just keep saying it's revealed unless hand proximity is absolutely zero.
                    isRevealed = proximity != 0.0;                    
                }
                else
                {
                    // If the starburst is not yet revealed, it will be when proximity equals 1.0.
                    isRevealed = proximity == 1.0;
                }

                return isRevealed;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
