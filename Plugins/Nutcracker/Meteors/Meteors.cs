using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using CommonUtils;

using VixenPlus;


namespace Meteors {
    public partial class Meteors : UserControl, INutcrackerEffect {
        public Meteors() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Meteors"; }
        } 

        private readonly List<MeteorClass> _meteors = new List<MeteorClass>();

        // for meteor effect
        public class MeteorClass {

            public int X;
            public int Y;
            public HSVUtils HSV = new HSVUtils();

            public bool HasExpired(int tailLength) {
                return (Y + tailLength < 0);
            }
        }

        private readonly Random _random = new Random();
        private int _lastRenderedEvent = -1;

        //todo not working quite right.
        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            if (eventToRender == 0)
                _meteors.Clear();

            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var mspeed = eventToRender - _lastRenderedEvent;
            _lastRenderedEvent = eventToRender;

            // create new meteors
            var hsv = new HSVUtils();
            var hsv0 = HSVUtils.ColorToHSV(palette[0]);
            var hsv1 = palette.Length > 1 ? HSVUtils.ColorToHSV(palette[1]) : HSVUtils.ColorToHSV(palette[0]);
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
                        m.HSV = HSVUtils.SetRangeColor(hsv0, hsv1);
                        break;
                    case 2:
                        m.HSV = HSVUtils.ColorToHSV(palette[_random.Next() % colorcnt]) ;
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
                        if (meteor.X >= 0 && meteor.X < bufferWidth && meteor.Y + ph >= 0 && meteor.Y + ph < bufferHeight) {
                            buffer[meteor.Y + ph, meteor.X] = HSVUtils.HSVtoColor(hsv);
                        }
                    }
                    meteor.Y -= mspeed;
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
            OnControlChanged(this, new EventArgs());
        }
    }
}
