using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using NutcrackerEffects.Effects;

using VixenPlus.Annotations;

using VixenPlusCommon;

namespace Nutcracker.Effects {
    [UsedImplicitly]
    public partial class Fireworks : UserControl, INutcrackerEffect {

        private const string FireworksExplosions = "ID_SLIDER_Fireworks{0}_Number_Explosions";
        private const string FireworksCount = "ID_SLIDER_Fireworks{0}_Count";
        private const string FireworksVelocity = "ID_SLIDER_Fireworks{0}_Velocity";
        private const string FireworksFade = "ID_SLIDER_Fireworks{0}_Fade";
        private const string FireworksMultiColor = "ID_CHECKBOX_Fireworks{0}_Multicolor";


        public Fireworks() {
            InitializeComponent();
            InitFireworksBuffer();
        }


        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Fireworks"; }
        }

        public string Notes {
            get { return String.Empty; }
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
                FireworksCount + "=" + tbExplosionCount.Value,
                FireworksExplosions + "=" + tbExplosionParticles.Value,
                FireworksFade + "=" + tbParticleFade.Value,
                FireworksVelocity + "=" + tbParticleVelocity.Value,
                FireworksMultiColor + "=" + (cbMutliColor.Checked ? "1" : "0")
            };
        }


        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var fireworksCount = string.Format(FireworksCount, effectNum);
            var fireworksExplosions = string.Format(FireworksExplosions, effectNum);
            var fireworksFade = string.Format(FireworksFade, effectNum);
            var fireworksVelocity = string.Format(FireworksVelocity, effectNum);
            var fireworksMultiColor = string.Format(FireworksMultiColor, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] {'='}))) {
                if (keyValue[0].Equals(fireworksCount)) {
                    tbExplosionCount.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(fireworksExplosions)) {
                    tbExplosionParticles.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(fireworksFade)) {
                    tbParticleFade.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(fireworksVelocity)) {
                    tbParticleVelocity.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(fireworksMultiColor)) {
                    cbMutliColor.Checked = keyValue[1].Equals("1");
                }
            }
        }


        private RgbFireworks[] _fireworkBursts;


        private void InitFireworksBuffer() {
            if (_fireworkBursts != null) {
                return;
            }

            _fireworkBursts = new RgbFireworks[20000];
            for (var burstNum = 0; burstNum < 20000; burstNum++) {
                _fireworkBursts[burstNum] = new RgbFireworks();
            }
        }


        private static readonly Random Random = new Random();


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);

            var idxFlakes = 0;
            const int maxFlakes = 1000;
            var numberExplosions = tbExplosionCount.Value;


            if (eventToRender == 0 || _fireworkBursts == null) {
                InitFireworksBuffer();
                for (var i = 0; i < maxFlakes; i++) {
                    // ReSharper disable PossibleNullReferenceException
                    _fireworkBursts[i].IsActive = false;
                    // ReSharper restore PossibleNullReferenceException
                }
            }

            if (eventToRender%(101 - numberExplosions)*10 == 0) {
                var startX = (int) (bufferWidth*0.25 + (Random.Next()%(bufferWidth*0.75 - bufferWidth*0.25)));
                var startY = (int) (bufferHeight*0.25 + (Random.Next()%(bufferHeight*0.75 - bufferHeight*0.25)));
                var shellColor = palette[Random.Next()%palette.Length];
                for (var i = 0; i < tbExplosionParticles.Value; i++) {
                    do {
                        idxFlakes = (idxFlakes + 1)%maxFlakes;
                        // ReSharper disable PossibleNullReferenceException
                    } while (_fireworkBursts[idxFlakes].IsActive);
                    // ReSharper restore PossibleNullReferenceException

                    _fireworkBursts[idxFlakes].Reset(startX, startY, true, tbParticleVelocity.Value);
                    _fireworkBursts[idxFlakes].FireworkColor = shellColor;
                }
            }
            else {
                for (var i = 0; i < maxFlakes; i++) {
                    // ReSharper disable PossibleNullReferenceException
                    if (!_fireworkBursts[i].IsActive) {
                        // ReSharper restore PossibleNullReferenceException
                        continue;
                    }

                    _fireworkBursts[i].X += _fireworkBursts[i].Dx;
                    _fireworkBursts[i].Y +=
                        (float)
                        (-_fireworkBursts[i].Dy - _fireworkBursts[i].Cycles*_fireworkBursts[i].Cycles/10000000.0);
                    _fireworkBursts[i].Cycles += 20;

                    if (10000 == _fireworkBursts[i].Cycles) {
                        _fireworkBursts[i].IsActive = false;
                        continue;
                    }

                    if (_fireworkBursts[i].Y >= bufferHeight) {
                        _fireworkBursts[i].IsActive = false;
                        continue;
                    }

                    if (_fireworkBursts[i].X >= 0.0 && _fireworkBursts[i].X < bufferWidth) {
                        if (_fireworkBursts[i].Y >= 0.0 && cbMutliColor.Checked) {
                            _fireworkBursts[i].FireworkColor = palette[Random.Next()%palette.Length];
                        }
                    }
                    else {
                        _fireworkBursts[i].IsActive = false;
                    }
                }
            }


            for (var i = 0; i < 1000; i++) {
                // ReSharper disable PossibleNullReferenceException
                if (!_fireworkBursts[i].IsActive) {
                    // ReSharper restore PossibleNullReferenceException
                    continue;
                }
                var hsv = _fireworkBursts[i].FireworkColor.ToHSV();

                var v = (float) (((tbParticleFade.Value*10.0) - _fireworkBursts[i].Cycles)/(tbParticleFade.Value*10.0));
                if (v < 0) {
                    v = 0.0f;
                }

                hsv.Value = v;
                var x = (int) _fireworkBursts[i].X;
                var y = (int) _fireworkBursts[i].Y;
                if (x >= 0 && x < bufferWidth && y >= 0 && y < bufferHeight) {
                    buffer[y, x] = hsv.ToColor();
                }
            }
            return buffer;
        }


        private class RgbFireworks {
            public float X;
            public float Y;
            public float Dx;
            public float Dy;
            private float _vel;
            private float _angle;
            public bool IsActive;
            public int Cycles;
            public Color FireworkColor;


            public void Reset(int x, int y, bool active, float velocity) {
                X = x;
                Y = y;
                _vel = (Random.Next() - int.MaxValue/2)*velocity/(int.MaxValue/2);
                _angle = (float) (2*Math.PI*Random.Next()/int.MaxValue);
                Dx = (float) (_vel*Math.Cos(_angle));
                Dy = (float) (_vel*Math.Sin(_angle));
                IsActive = active;
                Cycles = 0;
                FireworkColor = Color.White;
            }
        }


        private void Fireworks_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, e);
        }
    }
}