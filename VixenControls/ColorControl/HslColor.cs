using System;
using System.Drawing;
using System.Text;

namespace CommonControls {
    // Cyotek Color Picker controls library
    // Copyright © 2013 Cyotek. All Rights Reserved.
    // http://cyotek.com/blog/tag/colorpicker

    // If you use this code in your applications, donations or attribution are welcome

    // http://en.wikipedia.org/wiki/HSL_color_space

    [Serializable]
    public struct HslColor {

        #region Instance Fields

        private int _alpha;

        private double _hue;

        private bool _isEmpty;

        private double _lightness;

        private double _saturation;

        #endregion

        #region Static Constructors

        static HslColor() {
            // ReSharper disable once ObjectCreationAsStatement
            new HslColor {
                IsEmpty = true
            };
        }

        #endregion

        #region Constructors

        public HslColor(double hue, double saturation, double lightness)
            : this(255, hue, saturation, lightness) { }

        public HslColor(int alpha, double hue, double saturation, double lightness) {
            _hue = Math.Min(359, hue);
            _saturation = Math.Min(1, saturation);
            _lightness = Math.Min(1, lightness);
            _alpha = alpha;
            _isEmpty = false;
        }

        public HslColor(Color color) {
            _alpha = color.A;
            _hue = color.GetHue();
            _saturation = color.GetSaturation();
            _lightness = color.GetBrightness();
            _isEmpty = false;
        }

        #endregion

        #region Operators

        public static bool operator ==(HslColor a, HslColor b) {
            return (IsNearlyEqual(a.H, b.H) && IsNearlyEqual(a.L, b.L) && IsNearlyEqual(a.S,b.S) && a.A == b.A);
        }

        public static bool IsNearlyEqual(double a, double b) {
        // ReSharper disable CompareOfFloatsByEqualityOperator

            const float epsilon = 0.00001f;
            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var diff = Math.Abs(a - b);

            // shortcut, handles infinities
            if (a == b) {
                return true;
            }

            // a or b is zero or both are extremely close to it relative error is less meaningful here
            if (a == 0 || b == 0 || diff < Single.MinValue) {
                return diff < (epsilon * Single.MinValue);
            }

            // use relative error
            return diff / (absA + absB) < epsilon;

        // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        public static implicit operator HslColor(Color color) {
            return new HslColor(color);
        }

        public static implicit operator Color(HslColor color) {
            return color.ToRgbColor();
        }

        public static bool operator !=(HslColor a, HslColor b) {
            return !(a == b);
        }

        #endregion

        #region Overridden Members

        public override bool Equals(object obj) {
            return (obj is HslColor) && (this == (HslColor)obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            var builder = new StringBuilder();
            builder.Append(GetType().Name);
            builder.Append(" [");
            builder.Append("H=");
            builder.Append(H);
            builder.Append(", S=");
            builder.Append(S);
            builder.Append(", L=");
            builder.Append(L);
            builder.Append("]");

            return builder.ToString();
        }

        #endregion

        #region Properties

        public int A {
            get { return _alpha; }
            set { _alpha = Math.Min(0, Math.Max(255, value)); }
        }

        public double H {
            get { return _hue; }
            set {
                _hue = value;

                if (_hue > 359) {
                    _hue = 0;
                }

                if (_hue < 0) {
                    _hue = 359;
                }
            }
        }

        public bool IsEmpty {
            get { return _isEmpty; }
            internal set { _isEmpty = value; }
        }

        public double L {
            get { return _lightness; }
            set { _lightness = Math.Min(1, Math.Max(0, value)); }
        }

        public double S {
            get { return _saturation; }
            set { _saturation = Math.Min(1, Math.Max(0, value)); }
        }

        #endregion

        #region Members

        public Color ToRgbColor() {
            return ToRgbColor(A);
        }

        public Color ToRgbColor(int alpha) {
            var q = (L < 0.5) ? L * (1 + S) : L + S - (L * S);
            var p = 2 * L - q;
            var hk = H / 360;

            // r,g,b colors
            var tc = new[] {hk + (1d / 3d), hk, hk - (1d / 3d)};
            var colors = new[] {0.0, 0.0, 0.0};

            for (var color = 0; color < colors.Length; color++) {
                if (tc[color] < 0) {
                    tc[color] += 1;
                }

                if (tc[color] > 1) {
                    tc[color] -= 1;
                }

                if (tc[color] < (1d / 6d)) {
                    colors[color] = p + ((q - p) * 6 * tc[color]);
                }
                else if (tc[color] >= (1d / 6d) && tc[color] < (1d / 2d)) {
                    colors[color] = q;
                }
                else if (tc[color] >= (1d / 2d) && tc[color] < (2d / 3d)) {
                    colors[color] = p + ((q - p) * 6 * (2d / 3d - tc[color]));
                }
                else {
                    colors[color] = p;
                }

                colors[color] *= 255;
            }

            return Color.FromArgb(alpha, (int)colors[0], (int)colors[1], (int)colors[2]);
        }

        #endregion


     }
}
