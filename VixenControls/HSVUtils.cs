using System;
using System.Drawing;

namespace VixenPlusCommon {
    public static class HSVUtils {
        public static HSV ToHSV(this Color color) {
            var chromaMax = (float) Math.Max(color.R, Math.Max(color.G, color.B));
            var chromaMin = (float) Math.Min(color.R, Math.Min(color.G, color.B));

            var hue = color.GetHue()/360;
            var saturation = chromaMax.IsNearlyEqual(0) ? 0 : 1f - (1f*chromaMin/chromaMax);
            var value = chromaMax/255f;

            return new HSV(hue, saturation, value);
        }

        public static Color ToColor(this HSV inHsv) {
            var hue = inHsv.Hue*360;
            var saturation = inHsv.Saturation;
            var value = inHsv.Value*255;

            var f = hue/60f - Math.Floor(hue/60f);
            var v = Convert.ToInt32((int) value);
            var p = Convert.ToInt32((int) (value*(1 - saturation)));
            var q = Convert.ToInt32(value*(1 - f*saturation));
            var t = Convert.ToInt32(value*(1 - (1 - f)*saturation));
            switch (Convert.ToInt32((int) (Math.Floor(hue/60)%6))) {
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



        public static HSV CreateRangeTo(this HSV hsv1, HSV hsv2) {
            var newHsv = new HSV(RandomRange(hsv1.Hue, hsv2.Hue), RandomRange(hsv1.Saturation, hsv2.Saturation), 1.0f);
            return newHsv;
        }


        public static Color GetMultiColorBlend(this Color[] palette, double blendRatio, bool circular) {
            var colorCount = palette.Length;
            if (colorCount <= 1) {
                return palette[0];
            }

            blendRatio = blendRatio >= 1.0 ? 0.99999 : blendRatio < 0.0 ? 0.0 : blendRatio;
            var index = blendRatio*(circular ? colorCount : colorCount - 1);
            var firstColor = (int) Math.Floor(index);
            var secondColor = (firstColor + 1)%colorCount;
            var ratio = index - firstColor;

            return Get2ColorBlend(palette[firstColor], palette[secondColor], ratio);
        }


        private static Random _random;

        // generates a random number between num1 and num2 inclusive
        private static float RandomRange(float num1, float num2) {
            double hi, lo;
            if (_random == null) _random = new Random();

            if (num1 < num2) {
                lo = num1;
                hi = num2;
            }
            else {
                lo = num2;
                hi = num1;
            }

            //return random.Next(lo, hi);
            return (float) (_random.NextDouble()*(hi - lo) + lo);
        }


        private static int BlendChannel(int firstColor, int secondColor, double ratio) {
            return firstColor + (int) Math.Floor(ratio*(secondColor - firstColor) + 0.5);
        }


        private static Color Get2ColorBlend(Color c1, Color c2, double ratio) {
            return Color.FromArgb(BlendChannel(c1.R, c2.R, ratio), BlendChannel(c1.G, c2.G, ratio),
                                  BlendChannel(c1.B, c2.B, ratio));
        }
    }
}
