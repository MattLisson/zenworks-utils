using System;
using System.Collections.Generic;

namespace Zenworks.Utils {
    public static class MathExtensions {
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T> {
            if (min.CompareTo(value) > 0) {
                return min;
            }

            if (value.CompareTo(max) > 0) {
                return max;
            }

            return value;
        }
    }
}
