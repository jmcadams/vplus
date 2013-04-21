using System;
using System.Drawing;

namespace CommonUtils {
    public static class Utils {
        public const int IndexRowsOrHeight = 0;
        public const int IndexColsOrWidth = 1;
        public const byte Cell8BitMax = 255;
        public const int MillsPerSecond = 1000;
        public const int MillsPerMinute = 60 * MillsPerSecond;

        public static Brush GetTextColor(Color backgroundColor) {
            return ((backgroundColor.R * 299) + (backgroundColor.G * 587) + (backgroundColor.B * 114)) / 1000 >= 128 ? Brushes.Black : Brushes.White;
        }


        public static string TimeFormatMillsOnly(int mills) {
            return String.Format(":{0:d2}", mills / MillsPerSecond);
        }


        public static string TimeFormatWithoutMills(int mills, bool suppressLeadingZero = false) {
            return String.Format(suppressLeadingZero ? "{0:d}:{1:d2}" : "{0:d2}:{1:d2}", mills / MillsPerMinute, (mills % MillsPerMinute) / MillsPerSecond);
        }


        public static string TimeFormatWithMills(int mills) {
            return String.Format("{0:d2}:{1:d2}.{2:d3}", mills / MillsPerMinute, (mills % MillsPerMinute) / MillsPerSecond, mills % MillsPerSecond);
        }


        public static int ToPercentage(int value) {
            return (int)Math.Round(value * 100f / Cell8BitMax, MidpointRounding.AwayFromZero);
        }


        public static int ToValue(int percentage) {
            return (int)Math.Round(percentage / 100f * Cell8BitMax, MidpointRounding.AwayFromZero);
        }

        public static int ToValue(float percentage) {
            return (int)Math.Round(percentage / 100f * Cell8BitMax, MidpointRounding.AwayFromZero);
        }
    }
}
