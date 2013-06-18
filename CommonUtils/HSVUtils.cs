using System;
using System.Drawing;

namespace CommonUtils {
    public class HSVUtils {
        private static Random _random;


        public HSVUtils(float hue = 0f, float saturation = 0f, float value = 0f) {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }


        public float Hue { get; set; }

        public float Saturation { get; set; }

        public float Value { get; set; }


        public void SetToHSV(HSVUtils hsv) {
            Hue = hsv.Hue;
            Saturation = hsv.Saturation;
            Value = hsv.Value;
        }


        public static HSVUtils ColorToHSV(Color color) {
            var chromaMax = (float) Math.Max(color.R, Math.Max(color.G, color.B));
            var chromaMin = (float) Math.Min(color.R, Math.Min(color.G, color.B));

            var hue = color.GetHue() / 360;
            var saturation = Utils.IsNearlyEqual(chromaMax, 0) ? 0 : 1f - (1f * chromaMin / chromaMax);
            var value = chromaMax / 255f;

            return new HSVUtils(hue, saturation, value);
        }


        public static Color HSVtoColor(HSVUtils inHsv) {
            var hue = inHsv.Hue * 360;
            var saturation = inHsv.Saturation;
            var value = inHsv.Value * 255;

            var f = hue / 60f - Math.Floor(hue / 60f);
            var v = Convert.ToInt32(value);
            var p = Convert.ToInt32(value * (1 - saturation));
            var q = Convert.ToInt32(value * (1 - f * saturation));
            var t = Convert.ToInt32(value * (1 - (1 - f) * saturation));
            switch (Convert.ToInt32(Math.Floor(hue / 60) % 6)) {
                case 0:
                    return Color.FromArgb(255, v, t, p);
                case 1:
                    return Color.FromArgb(255, q, v, p);
                case 2:
                    return Color.FromArgb(255, p, v, t);
                case 3:
                    return Color.FromArgb(255, p, q, v);
                case 4:
                    return Color.FromArgb(255, t, p, v);
                default:
                    return Color.FromArgb(255, v, p, q);
            }
        }


        // generates a random number between num1 and num2 inclusive
        private static float RandomRange(float num1, float num2) {
            double hi, lo;
            InitRandom();

            if (num1 < num2) {
                lo = num1;
                hi = num2;
            }
            else {
                lo = num2;
                hi = num1;
            }

            //return random.Next(lo, hi);
            return (float) (_random.NextDouble() * (hi - lo) + lo);
        }


        private static void InitRandom() {
            if (_random == null)
                _random = new Random();
        }


        public static HSVUtils SetRangeColor(HSVUtils hsv1, HSVUtils hsv2) {
            var newHsv = new HSVUtils(RandomRange(hsv1.Hue, hsv2.Hue), RandomRange(hsv1.Saturation, hsv2.Saturation), 1.0f);
            return newHsv;
        }


        public static Color GetMultiColorBlend(double n, bool circular, Color[] palette) {
            var colorCount = palette.Length;
            if (colorCount <= 1) {
                return palette[0];
            }

            n = n >= 1.0 ? 0.99999 : n < 0.0 ? 0.0 : n;
            var index = n * (circular ? colorCount : colorCount - 1);
            var firstColor = (int) Math.Floor(index);
            var secondColor = (firstColor + 1) % colorCount;
            var ratio = index - firstColor;

            return Get2ColorBlend(palette[firstColor], palette[secondColor], ratio);
        }


        private static int ChannelBlend(int firstColor, int secondColor, double ratio) {
            return firstColor + (int) Math.Floor(ratio * (secondColor - firstColor) + 0.5);
        }


        public static Color Get2ColorBlend(Color c1, Color c2, double ratio) {
            return Color.FromArgb(ChannelBlend(c1.R, c2.R, ratio), ChannelBlend(c1.G, c2.G, ratio), ChannelBlend(c1.B, c2.B, ratio));
        }
    }

}
