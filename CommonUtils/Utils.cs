using System;
using System.Drawing;

namespace CommonUtils {
    public static class Utils {
        public const int IndexRowsOrHeight = 0;
        public const int IndexColsOrWidth = 1;
        public const byte Cell8BitMax = 255;
        public const int MillsPerSecond = 1000;
        public const int MillsPerMinute = 60 * MillsPerSecond;

        public const int ExecutionStopped = 0;
        public const int ExecutionPaused = 2;
        public const int ExecutionRunning = 1;


        // see: http://en.wikipedia.org/wiki/YIQ
        public static Brush GetTextColor(Color backgroundColor) {
            return ((backgroundColor.R * 299) + (backgroundColor.G * 587) + (backgroundColor.B * 114)) / 1000 >= 128 ? Brushes.Black : Brushes.White;
        }


        public static string TimeFormatMillsOnly(int mills) {
            return String.Format(":{0:d2}", mills / MillsPerSecond);
        }


        public static string TimeFormatWithoutMills(int mills, bool suppressLeadingZero = false) {
            return String.Format(suppressLeadingZero ? "{0:d}:{1:d2}" : "{0:d2}:{1:d2}", mills / MillsPerMinute,
                                 (mills % MillsPerMinute) / MillsPerSecond);
        }


        public static string TimeFormatWithMills(int mills) {
            return String.Format("{0:d2}:{1:d2}.{2:d3}", mills / MillsPerMinute, (mills % MillsPerMinute) / MillsPerSecond, mills % MillsPerSecond);
        }


        public static int ToPercentage(int value) {
            return (int) Math.Round(value * 100f / Cell8BitMax, MidpointRounding.AwayFromZero);
        }


        public static int ToValue(int percentage) {
            return (int) Math.Round(percentage / 100f * Cell8BitMax, MidpointRounding.AwayFromZero);
        }


        public static int ToValue(float percentage) {
            return (int) Math.Round(percentage / 100f * Cell8BitMax, MidpointRounding.AwayFromZero);
        }


        public static Bitmap ResizeImage(Image image, int size) {
            var result = new Bitmap(size, size);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var g = Graphics.FromImage(result)) {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            return result;
        }


        public static bool IsNearlyEqual(float a, float b) {
            const float epsilon = 0.00001f;
            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var diff = Math.Abs(a - b);

            // ReSharper disable CompareOfFloatsByEqualityOperator
            
            if (a == b) { // shortcut, handles infinities
                return true;
            }
            
            if (a == 0 || b == 0 || diff < float.MinValue) {
                // a or b is zero or both are extremely close to it relative error is less meaningful here
                return diff < (epsilon * float.MinValue);
            } 
            
            // use relative error
            return diff / (absA + absB) < epsilon;
            
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }
    }
}
