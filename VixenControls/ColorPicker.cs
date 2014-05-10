using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using VixenPlusCommon.Properties;

namespace VixenPlusCommon {
    [DefaultProperty("Color")]
    [DefaultEvent("ColorChanged")]
    public partial class ColorPicker : Form {

        private const string ControlPb = "pbCustom";
        private readonly Preference2 _pref = Preference2.GetInstance();
        private readonly Timer _clickTimer;
        private int _clickCount;
        private PictureBox _control;

        public ColorPicker(Color color, bool showNone = true) {
            InitializeComponent();

            _clickTimer = new Timer {Interval = SystemInformation.DoubleClickTime};
            _clickTimer.Tick += ResetClickTimer;
            
            ControlBox = false; // Workaround: If this is set to false in the designer.cs, it renders wrong.
            btnNone.Visible = showNone;
            SetColorOrImage(pbOriginalColor, color);
            SetEditorColor(color);
        }


        private void pbCustom_MouseDown(object sender, MouseEventArgs e) {
            _control = sender as PictureBox;
            if (null == _control) {
                return;
            }

            _clickTimer.Start();
            _clickCount++;

            SetEditorColor(_control.BackColor);
            pbNewColor.BackColor = _control.BackColor;

            if (_clickCount == 1) {
                foreach (var c in
                    from PictureBox c in (from object c in Controls where c is PictureBox select c) where c.Name.StartsWith(ControlPb) select c) {
                    c.BorderStyle = c == sender ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
                }
            }
            else {
                _clickTimer.Stop();
                DialogResult = DialogResult.OK;
            }
        }


        private void ResetClickTimer(object sender, EventArgs e) {
            _clickTimer.Stop();
            _clickCount = 0;
        }


        private void SetEditorColor(Color color) {
            //colorEditor1.Color = color == Color.Transparent ? Color.White : color; TODO Decide if I want to keep this.
            colorEditor1.Color = color;
        }


        public event EventHandler ColorEditorColorChanged;


        private void OnColorEditorColorChanged() {
            var handler = ColorEditorColorChanged;
            if (handler != null) {
                handler(this, new ColorEventArgs(colorEditor1.Color));
            }
        }


        private void colorEditor1_ColorChanged(object sender, EventArgs e) {
            SetColorOrImage(pbNewColor, colorEditor1.Color);
            OnColorEditorColorChanged();
        }


        public Color GetColor() {
            return pbNewColor.BackColor;
        }


        private static void SetColorOrImage(Control pb, Color color) {
            pb.BackColor = color;
            pb.BackgroundImage = color == Color.Transparent ? Resources.cellbackground : null;
        }


        private void btnAddColor_Click(object sender, EventArgs e) {
            if ((from object c in Controls select c as PictureBox).Count(
                    pb => pb != null && pb.Name.StartsWith(ControlPb) && pb.BorderStyle == BorderStyle.Fixed3D) > 0) {
                foreach (var c in from PictureBox c in (from object c in Controls where c is PictureBox select c)
                    where c.Name.StartsWith(ControlPb) && c.BorderStyle == BorderStyle.Fixed3D
                    select c) {
                    c.BackColor = colorEditor1.Color;
                }
            }
            else {
                pbCustom0.BackColor = colorEditor1.Color;
            }
        }

        private void pbOriginalColor_Click(object sender, EventArgs e) {
            SetEditorColor(pbOriginalColor.BackColor);
            pbNewColor.BackColor = pbOriginalColor.BackColor;
        }

        private void ColorPicker_Shown(object sender, EventArgs e) {
            var colors = GetDefaultOrCustomColors().Split(',');
            var colorCount = colors.Count() - 1;
            for (var i = 0; i < 16; i++) {
                var control = Controls.Find(string.Format("pbCustom{0:X}", i), true)[0];
                var color = Color.White;
                if (colorCount >= i) {
                    var raw = int.Parse(colors[i]);
                    var r = (raw & 0xFF);
                    var g = (raw & 0xFF00) >> 8;
                    var b = (raw & 0xFF0000) >> 16;
                    color = Color.FromArgb(r, g, b);
                }
                control.BackColor = color;
            }
        }


        private string GetDefaultOrCustomColors() {
            var def = _pref.GetStringDefault(Preference2.CustomColorsPreference);
            var cust = _pref.GetString(Preference2.CustomColorsPreference);
            const string basic ="255,65280,16711680,65535,16711935,16776960,16777215,0," + 
                                "8421631,8454016,16744576,12648447,16761087,16777154,12632256,4210752";
            return def == cust ? basic : cust;
        }


        private void ColorPicker_FormClosing(object sender, FormClosingEventArgs e) {
            var colors = (from PictureBox c in
                              (from object c in Controls where c is PictureBox select c)
                          where c.Name.StartsWith(ControlPb)
                          select c.BackColor).Reverse().ToArray();
            var color = new string[colors.Count()];
            for (var i = 0; i < colors.Count(); i++) {
                var raw = colors[i];
                color[i] = (raw.R + (raw.G << 8) + (raw.B << 16)).ToString(CultureInfo.InvariantCulture);
            }
            _pref.SetString(Preference2.CustomColorsPreference,string.Join(",", color));
            _pref.SaveSettings();
            _clickTimer.Tick -= ResetClickTimer;
            _clickTimer.Dispose();
        }

    }
}