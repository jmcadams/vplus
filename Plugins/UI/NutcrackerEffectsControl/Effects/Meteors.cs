using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VixenPlus;

using VixenPlusCommon;
using VixenPlusCommon.Annotations;

namespace NutcrackerEffectsControl.Effects {
    [UsedImplicitly]
    public partial class Meteors : UserControl, INutcrackerEffect {

        private readonly bool _initializing = true;
        private const string MeteorsType = "ID_CHOICE_Meteors{0}_Type";
        private const string MeteorsCount = "ID_SLIDER_Meteors{0}_Count";
        private const string MeteorsLength = "ID_SLIDER_Meteors{0}_Length";
        private const string MeteorsFallUp = "ID_CHECKBOX_Meteors{0}_FallUp";

        public Meteors() {
            InitializeComponent();
            cbType.SelectedIndex = 0;
            _initializing = false;
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Meteors"; }
        }

        public string Notes {
            get { return String.Empty;  }
        }

        public bool UsesPalette {
            get { return true; }
        }

        public bool UsesSpeed {
            get { return true; }
        }

        public List<string> Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }

        private List<string> GetCurrentSettings() {
            return new List<string> {
                MeteorsCount + "=" + tbCount.Value,
                MeteorsFallUp + "=" + (chkBoxUp.Checked ? "1" : "0"),
                MeteorsLength + "=" + tbTrailLength.Value,
                MeteorsType + "=" + cbType.SelectedItem
            };
        }

        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var meteorsCount = string.Format(MeteorsCount, effectNum);
            var meteorsFallUp = string.Format(MeteorsFallUp, effectNum);
            var meteorsLength = string.Format(MeteorsLength, effectNum);
            var meteorsType = string.Format(MeteorsType, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] { '=' }))) {
                if (keyValue[0].Equals(meteorsCount)) {
                    tbCount.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(meteorsFallUp)) {
                    chkBoxUp.Checked = keyValue[1].Equals("1");
                }
                else if (keyValue[0].Equals(meteorsLength)) {
                    tbTrailLength.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(meteorsType)) {
                    var index = cbType.Items.IndexOf(keyValue[1]);
                    if (index >= 0) {
                        cbType.SelectedIndex = index;
                    }
                }
            }
        }

        private readonly List<MeteorClass> _meteors = new List<MeteorClass>();

        // for meteor effect
        private class MeteorClass {

            public int X;
            public int Y;
            public HSV HSV = new HSV();

            public bool HasExpired(int tailLength) {
                return (Y + tailLength < 0);
            }
        }

        private readonly Random _random = new Random();
        private int _lastRenderedEvent = -1;

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            if (eventToRender == 0) _meteors.Clear();

            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var mspeed = eventToRender - _lastRenderedEvent;
            _lastRenderedEvent = eventToRender;

            // create new meteors
            var hsv = new HSV();
            var hsv0 = palette[0].ToHSV();
            var hsv1 = (palette.Length > 1 ? palette[1] : palette[0]).ToHSV();
            var colorcnt = palette.Length;
            var count = bufferWidth * tbCount.Value / 100;
            var tailLength = (bufferHeight < 10) ? tbTrailLength.Value / 10 : bufferHeight * tbTrailLength.Value / 100;
            if (tailLength < 1) tailLength = 1;
            var tailStart = bufferHeight - tailLength;
            if (tailStart < 1) tailStart = 1;
            for (var i = 0; i < count; i++) {
                var m = new MeteorClass {X = _random.Next() % bufferWidth, Y = bufferHeight - 1 - (_random.Next() % tailStart)};
                switch (cbType.SelectedIndex) {
                    case 1:
                        m.HSV = hsv0.CreateRangeTo(hsv1);
                        break;
                    case 2:
                        m.HSV = palette[_random.Next() % colorcnt].ToHSV() ;
                        break;
                }
                _meteors.Add(m);
            }

            // render meteors
            foreach (var meteor in _meteors) {
                {
                    for (var ph = 0; ph < tailLength; ph++) {
                        switch (cbType.SelectedIndex) {
                            case 0:
                                hsv.Hue = _random.Next() % 1000 / 1000.0f;
                                hsv.Saturation = 1.0f;
                                hsv.Value = 1.0f;
                                break;
                            default:
                                hsv.SetToHSV(meteor.HSV);
                                break;
                        }
                        hsv.Value *= (float)(1.0 - (double)ph / tailLength);
                        if (meteor.X < 0 || meteor.X >= bufferWidth || meteor.Y + ph < 0 || meteor.Y + ph >= bufferHeight) {
                            continue;
                        }
                        var y = meteor.Y + (chkBoxUp.Checked ? -ph : ph);
                        if (y >= 0 && y < bufferHeight) {
                            buffer[y, meteor.X] = hsv.ToColor();
                        }
                    }
                    meteor.Y += (chkBoxUp.Checked ? mspeed : -mspeed);
                }
            }
            // delete old meteors
            var meteorNum = 0;
            while (meteorNum < _meteors.Count) {
                if (_meteors[meteorNum].HasExpired(tailLength)) {
                    _meteors.RemoveAt(meteorNum);
                }
                else {
                    meteorNum++;
                }
            }
            return buffer;
        }


        private void Meteors_ControlChanged(object sender, EventArgs e) {
            if (_initializing) return;
            OnControlChanged(this, e);
        }
    }
}
