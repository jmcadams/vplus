using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace VixenPlusCommon {
    public static class Utils {
        public const int IndexRowsOrHeight = 0;
        public const int IndexColsOrWidth = 1;
        public const byte Cell8BitMax = 255;
        public const byte Cell8BitMin = 0;
        public const int MillsPerSecond = 1000;
        public const int MillsPerMinute = 60 * MillsPerSecond;
        public const int BytesPerK = 1024;
        public const int ExecutionStopped = 0;
        public const int ExecutionPaused = 2;
        public const int ExecutionRunning = 1;

        /// <summary>
        /// Constants, in hours for UpdateCheckTime
        /// </summary>
        public const double UpdateDaily = 24;
        public const double UpdateWeekly = 168;       // 7 x Daily
        public const double UpdateMonthly = 730.5;    // 1/3 Quarterly
        public const double UpdateQuarterly = 2191.5; // 1/4 Annual
        public const double UpdateAnnually = 8766;    // (365.25 * 24)
    
        public const string LogFileName = "crash.log";
        private const string UpdateLogFileName = "update.log";

        private static readonly string LogFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), LogFileName);
        private static readonly string UpdateLogFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), UpdateLogFileName);


        // see: http://en.wikipedia.org/wiki/YIQ
        public static Brush GetTextColor(this Color backgroundColor) {
            return ((backgroundColor.R * 299) + (backgroundColor.G * 587) + (backgroundColor.B * 114)) / 1000 >= 128 ? Brushes.Black : Brushes.White;
        }


        public static string GetVersion() {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static Color GetForeColor(this Color backgroundColor) {
            return ((backgroundColor.R * 299) + (backgroundColor.G * 587) + (backgroundColor.B * 114)) / 1000 >= 128 ? Color.Black : Color.White;
        }

        public static void CrashLog(this string message) {
            WriteLog(message, LogFile);
        }


        public static void UpdateLog(this string message) {
            WriteLog(message, UpdateLogFile);
        }


        private static void WriteLog(string message, string logFile) {
            using (var log = new StreamWriter(logFile, true)) {
                log.WriteLine(message);
            }
        }


        public static string ToHTML(this Color color) {
            return color == Color.Transparent ? "Transparent" : "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }


        public static Color FromHTML(this string value) {
            return ColorTranslator.FromHtml(value);
        }

        public static int ToInt(this string value) {
            int result;

            if (!Int32.TryParse(value, out result)) {
                result = 0;
            }

            return result;
        }


        public static string FormatMillsOnly(this int mills) {
            return String.Format(":{0:d2}", mills / MillsPerSecond);
        }


        public static string FormatNoMills(this int mills, bool suppressLeadingZero = false) {
            return String.Format(suppressLeadingZero ? "{0:d}:{1:d2}" : "{0:d2}:{1:d2}", mills / MillsPerMinute,
                                 (mills % MillsPerMinute) / MillsPerSecond);
        }


        public static string FormatFull(this int mills) {
            return String.Format("{0:d2}:{1:d2}.{2:d3}", mills / MillsPerMinute, (mills % MillsPerMinute) / MillsPerSecond, mills % MillsPerSecond);
        }


        public static int ToPercentage(this int value) {
            return (int)Math.Round(value * 100f / Cell8BitMax, MidpointRounding.AwayFromZero);
        }

        public static int ToPercentage(this byte value) {
            return (int)Math.Round(value * 100f / Cell8BitMax, MidpointRounding.AwayFromZero);
        }


        public static int ToValue(this int percentage) {
            return (int) Math.Round(percentage / 100f * Cell8BitMax, MidpointRounding.AwayFromZero);
        }


        public static int ToValue(this float percentage) {
            return (int) Math.Round(percentage / 100f * Cell8BitMax, MidpointRounding.AwayFromZero);
        }


        public static Bitmap ResizeImage(this Image image, int size) {
            var result = new Bitmap(size, size);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var g = Graphics.FromImage(result)) {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            return result;
        }


        public static Rectangle NormalizeRect(this Rectangle rect) {
            return new Rectangle {
                X = Math.Min(rect.Left, rect.Right), Y = Math.Min(rect.Top, rect.Bottom), Width = Math.Abs(rect.Width) + (rect.Width < 0 ? 1 : 0),
                Height = Math.Abs(rect.Height) + (rect.Height < 0 ? 1 : 0)
            };
        }


        public static bool IsNearlyEqual(this float a, float b) {
            const float epsilon = 0.00001f;
            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var diff = Math.Abs(a - b);

            // ReSharper disable CompareOfFloatsByEqualityOperator
            
            if (a == b) { // shortcut, handles infinities
                return true;
            }
            
            if (a == 0 || b == 0 || diff < Single.MinValue) {
                // a or b is zero or both are extremely close to it relative error is less meaningful here
                return diff < (epsilon * Single.MinValue);
            } 
            
            // use relative error
            return diff / (absA + absB) < epsilon;
            
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }


        public static Point GetBestLocation(this Form form, Point point, int offset) {
            form.Location = new Point(point.X - form.Width - offset, point.Y - form.Height - offset);
            var s = Screen.FromRectangle(form.Bounds).Bounds;
            var d = form.Bounds;
            var x = s.Contains(d) ? d.X : d.X < s.X ? s.X + offset : d.X;
            var y = s.Contains(d) ? d.Y : d.Y < s.Y ? s.Y + offset : d.Y;
            return new Point(x,y);
        }

        public static bool IsWindows64BitOS() {
#if __WIN64
            return true;
#else
            return false;
#endif
        }

    }
}
