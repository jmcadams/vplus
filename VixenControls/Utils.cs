using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using VixenPlusCommon.Properties;

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

        private const string LogFileName = "crash.log";
        private const string UpdateLogFileName = "update.log";

        // ReSharper disable AssignNullToNotNullAttribute
        private static readonly string LogFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), LogFileName);
        private static readonly string UpdateLogFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), UpdateLogFileName);
        // ReSharper restore AssignNullToNotNullAttribute


        // see: http://en.wikipedia.org/wiki/YIQ
        public static Brush GetTextColor(this Color backgroundColor) {
            return (((backgroundColor.R * 299) + (backgroundColor.G * 587) + (backgroundColor.B * 114)) / 1000 >= 128) ? Brushes.Black : Brushes.White;
        }

        public static void ShowIoError(this string message, string caption) {
            MessageBox.Show(message + " Please try again later.", caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        public static Brush GetAlphaTextColor(this Color bg, Color bc) {
            var a1 = bg.A / 255.0;
            var a2 = 1 - a1;
            var c = Color.FromArgb(255, (int) (bg.R * a1 + bc.R * a2), (int) (bg.G * a1 + bc.G * a2), (int) (bg.B * a1 + bc.B * a2));
            return c.GetTextColor();
        }

        public static string GetVersion(Type t) {
            return Assembly.GetAssembly(t).GetName().Version.ToString();
            //return Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
                log.WriteLine("{0:O}:{1}", DateTime.Now, message);
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
            // ReSharper disable CompareOfFloatsByEqualityOperator

            if (a == b) { // shortcut, handles infinities
                return true;
            }

            // ReSharper restore CompareOfFloatsByEqualityOperator

            const float epsilon = 0.00001f;

            return Math.Abs(a - b) / (Math.Abs(a) + Math.Abs(b)) < epsilon;
            
        }


        public static Point GetBestLocation(this Form form, Point point, int offset) {
            form.Location = new Point(point.X - form.Width - offset, point.Y - form.Height - offset);
            var s = Screen.FromRectangle(form.Bounds).Bounds;
            var d = form.Bounds;
            var x = s.Contains(d) ? d.X : d.X < s.X ? s.X + offset : d.X;
            var y = s.Contains(d) ? d.Y : d.Y < s.Y ? s.Y + offset : d.Y;
            return new Point(x,y);
        }

        //private delegate void SetPropertyThreadSafeDelegate<TResult>(Control @this, Expression<Func<TResult>> property, TResult value);

        //public static void SetPropertyThreadSafe<TResult>(this Control @this, Expression<Func<TResult>> property, TResult value) {
        //    var propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;

        //    if (propertyInfo == null ||
        //        !@this.GetType().IsSubclassOf(propertyInfo.ReflectedType) ||
        //        @this.GetType().GetProperty(propertyInfo.Name, propertyInfo.PropertyType) == null) {
        //        throw new ArgumentException("The lambda expression 'property' must reference a valid property on this Control.");
        //    }

        //    if (@this.InvokeRequired) {
        //        @this.Invoke(new SetPropertyThreadSafeDelegate<TResult>(SetPropertyThreadSafe), new object[] { @this, property, value });
        //    }
        //    else {
        //        @this.GetType().InvokeMember(propertyInfo.Name, BindingFlags.SetProperty, null, @this, new object[] { value });
        //    }
        //}



        public static void ProcessException(this Exception ex, bool isTerminating) {
            ex.LogException(isTerminating);
            ex.ShowException(isTerminating);
        }


        public static void InformException(this Exception ex) {
            var msg = string.Format(Resources.InformOnError, LogFileName, ex.Message, ex.StackTrace, Vendor.ProductName);

            MessageBox.Show(msg, Resources.ErrorLogCreated, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        public static int Parse(this DataGridViewRow row) {
            return int.Parse(row.Tag.ToString());
        }

        private static void ShowException(this Exception exception, bool isTerminating) {
            var msgFormat = isTerminating ? Resources.CriticalErrorOccurred : Resources.SoftErrorOccured;
            var msg = string.Format(msgFormat, LogFileName, exception.Message, exception.StackTrace, Vendor.ProductName);
            var btns = isTerminating ? MessageBoxButtons.OK : MessageBoxButtons.YesNo;
            var icon = isTerminating ? MessageBoxIcon.Error : MessageBoxIcon.Question;

            var res = MessageBox.Show(msg, Resources.ErrorLogCreated, btns, icon);
            if (res == DialogResult.No || res == DialogResult.OK) {
                Application.Exit();
            }
        }


        private static void LogException(this Exception exception, bool isTerminating) {
            string.Format(Resources.FormattedVersion, Assembly.GetExecutingAssembly().GetName().Version).CrashLog();
            DateTime.Now.ToString(CultureInfo.InvariantCulture).CrashLog();
            string.Format("Is Terminating? {0}", isTerminating).CrashLog();
            exception.Message.CrashLog();
            exception.StackTrace.CrashLog();
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
