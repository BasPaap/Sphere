using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bas.Sphere.Extensions
{
    public static class MathExtensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            var actualMax = (max.CompareTo(min) < 0) ? min : max;

            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(actualMax) > 0) return max;
            else return val;
        }
    }
}
