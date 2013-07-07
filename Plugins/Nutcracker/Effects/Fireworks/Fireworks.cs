using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using CommonUtils;
using VixenPlus;

namespace Fireworks {
    public partial class Fireworks : UserControl, INutcrackerEffect {
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

        public XmlElement Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }

        private static XmlElement GetCurrentSettings() {
            return Xml.CreateXmlDocument().DocumentElement;
        }

        private static void Setup(XmlElement settings) {
            System.Diagnostics.Debug.Print(settings.ToString());
        }

        private RgbFireworks[] _fireworkBursts; 

        private void InitFireworksBuffer()
        {
            if (_fireworkBursts != null) {
                return;
            }

            _fireworkBursts = new RgbFireworks[20000];
            for (var burstNum = 0; burstNum < 20000; burstNum++)
            {
                _fireworkBursts[burstNum] = new RgbFireworks();
            }
        }

        static private readonly Random Random = new Random();


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);

            var idxFlakes = 0;
            const int maxFlakes = 1000;
            var numberExplosions = tbExplosionCount.Value;


            if (eventToRender == 0 || _fireworkBursts == null) {
                for (var i = 0; i < maxFlakes; i++) {
                    _fireworkBursts[i].IsActive = false;
                }
            }

            if (eventToRender % (101 - numberExplosions) * 10 == 0) {
                var startX = (int)(bufferWidth * 0.25 + (Random.Next() % (bufferWidth * 0.75 - bufferWidth * 0.25)));
                var startY = (int)(bufferHeight * 0.25 + (Random.Next() % (bufferHeight * 0.75 - bufferHeight * 0.25)));
                var shellColor = palette[Random.Next() % palette.Length];
                for (var i = 0; i < tbExplosionParticles.Value; i++) {
                    do {
                        idxFlakes = (idxFlakes + 1) % maxFlakes;
                    } while (_fireworkBursts[idxFlakes].IsActive);

                    _fireworkBursts[idxFlakes].Reset(startX, startY, true, tbParticleVelocity.Value);
                    _fireworkBursts[idxFlakes].FireworkColor = shellColor;
                }
            }
            else {
                for (var i = 0; i < maxFlakes; i++) {
                    if (!_fireworkBursts[i].IsActive) {
                        continue;
                    }

                    _fireworkBursts[i].X += _fireworkBursts[i].Dx;
                    _fireworkBursts[i].Y += (float) (-_fireworkBursts[i].Dy - _fireworkBursts[i].Cycles * _fireworkBursts[i].Cycles / 10000000.0);
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
                            _fireworkBursts[i].FireworkColor = palette[Random.Next() % palette.Length];
                        }
                    }
                    else {
                        _fireworkBursts[i].IsActive = false;
                    }
                }
            }


            for (var i = 0; i < 1000; i++) {
                if (!_fireworkBursts[i].IsActive) {
                    continue;
                }
                var hsv = HSVUtils.ColorToHSV(_fireworkBursts[i].FireworkColor);

                var v = (float) (((tbParticleFade.Value * 10.0) - _fireworkBursts[i].Cycles) / (tbParticleFade.Value * 10.0));
                if (v < 0) {
                    v = 0.0f;
                }

                hsv.Value = v;
                var x = (int) _fireworkBursts[i].X;
                var y = (int) _fireworkBursts[i].Y;
                if (x >= 0 && x < bufferWidth && y >= 0 && y < bufferHeight) {
                    buffer[y, x] = HSVUtils.HSVtoColor(hsv);
                }
            }
            return buffer;
        }


        public class RgbFireworks
        {
            public const int MaxCycle = 4096;
            public const int MaxNewBurstFlakes = 10;
            public float X;
            public float Y;
            public float Dx;
            public float Dy;
            public float Vel;
            public float Angle;
            public bool IsActive;
            public int Cycles;
            public Color FireworkColor;

            public void Reset(int x, int y, bool active, float velocity)
            {
                X = x;
                Y = y;
                Vel = (Random.Next() - int.MaxValue / 2) * velocity / (int.MaxValue / 2);
                Angle = (float)(2 * Math.PI * Random.Next() / int.MaxValue);
                Dx = (float)(Vel * Math.Cos(Angle));
                Dy = (float)(Vel * Math.Sin(Angle));
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
