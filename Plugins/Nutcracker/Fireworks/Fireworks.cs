using System;
using System.Drawing;
using System.Windows.Forms;
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


            // This does not work with 1 string, so use a try/catch block to prevent errors
            try {
                if (eventToRender == 0) {
                    for (var i = 0; i < maxFlakes; i++) {
                        _fireworkBursts[i].IsActive = false;
                    }
                }

                var mod100 = eventToRender%(101 - numberExplosions)*10;
                if (mod100 == 0) {

                    var x25 = (int) (bufferWidth*0.25);
                    var x75 = (int) (bufferWidth*0.75);
                    var y25 = (int) (bufferHeight*0.25);
                    var y75 = (int) (bufferHeight*0.75);
                    var startX = x25 + (Random.Next() % (x75 - x25));
                    var startY = y25 + (Random.Next() % (y75 - y25));
                    // turn off all bursts

                    // Create new bursts
                    for (var i = 0; i < tbExplosionParticles.Value; i++) {
                        do {
                            idxFlakes = (idxFlakes + 1)%maxFlakes;
                        } while (_fireworkBursts[idxFlakes].IsActive);

                        _fireworkBursts[idxFlakes].Reset(startX, startY, true, tbParticleVelocity.Value);
                    }
                }
                else {
                    for (var i = 0; i < maxFlakes; i++) {
                        if (_fireworkBursts[i].IsActive) {
                            _fireworkBursts[i].X += _fireworkBursts[i].Dx;
                            _fireworkBursts[i].Y +=
                                (float)
                                (-_fireworkBursts[i].Dy - _fireworkBursts[i].Cycles*_fireworkBursts[i].Cycles/10000000.0);
                            _fireworkBursts[i].Cycles += 20;
                            if (10000 == _fireworkBursts[i].Cycles)
                            {
                                _fireworkBursts[i].IsActive = false;
                                continue;
                            }
                            if (_fireworkBursts[i].Y >= bufferHeight) {
                                _fireworkBursts[i].IsActive = false;
                                continue;
                            }
                            if (_fireworkBursts[i].X >= 0.0 && _fireworkBursts[i].X < bufferWidth) {
                                if (_fireworkBursts[i].Y >= 0.0) {
                                    // sean we need to set color here
                                }
                            }
                            else {
                                _fireworkBursts[i].IsActive = false;
                            }
                        }
                    }
                }

                Color rgbcolor;
                switch (mod100) {
                    case 0:
                        rgbcolor = Color.Cyan;
                        break;
                    case 1:
                        rgbcolor = Color.Red;
                        break;
                    case 2:
                        rgbcolor = Color.FromArgb(0, 255, 0);
                        break;
                    case 3:
                        rgbcolor = Color.Blue;
                        break;
                    case 4:
                        rgbcolor = Color.Yellow;
                        break;
                    case 5:
                        rgbcolor = Color.FromArgb(0, 255, 0);
                        break;
                    default:
                        rgbcolor = Color.White;
                        break;
                }
                var hsv = HSVUtils.ColorToHSV(rgbcolor);

                for (var i = 0; i < 1000; i++) {
                    if (!_fireworkBursts[i].IsActive) {
                        continue;
                    }

                    var v = (float)(((tbParticleFade.Value * 10.0) - _fireworkBursts[i].Cycles) / (tbParticleFade.Value * 10.0));
                    if (v < 0) v = 0.0f;

                    hsv.Value = v;
                    buffer[(int)_fireworkBursts[i].X, (int)_fireworkBursts[i].Y] = HSVUtils.HSVtoColor(hsv);
                }
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch
                // ReSharper restore EmptyGeneralCatchClause
            {}

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
            }
        }

        private void Fireworks_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
