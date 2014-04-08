//TODO: Add support for movies

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using VixenPlus.Annotations;

using VixenPlusCommon;

namespace Nutcracker.Effects {
    [UsedImplicitly]
    public partial class Picture : UserControl, INutcrackerEffect {

        private readonly bool _initializing = true;
        private const string PicturesFileName = "ID_TEXTCTRL_Pictures{0}_Filename";
        private const string PicturesDirection = "ID_CHOICE_Pictures{0}_Direction";
        private const string PicturesGifSpeed = "ID_SLIDER_Pictures{0}_GifSpeed";

        public Picture() {
            InitializeComponent();
            cbDirection.SelectedIndex = 0;
            _initializing = false;
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Pictures"; }
        }

        public string Notes {
            get { return "Movies coming soon! Does not use palette."; }
        }

        public bool UsesPalette {
            get { return false; }
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
                PicturesDirection + "=" + cbDirection.SelectedItem,
                PicturesFileName + "=" + txtBoxFile.Text,
                PicturesGifSpeed + "=" + tbGifSpeed.Value
            };
        }

        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var picturesDirection = string.Format(PicturesDirection, effectNum);
            var picturesFileName = string.Format(PicturesFileName, effectNum);
            var picturesGifSpeed = string.Format(PicturesGifSpeed, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] { '=' }))) {
                if (keyValue[0].Equals(picturesDirection)) {
                    var index = cbDirection.Items.IndexOf(keyValue[1]);
                    if (index >= 0) {
                        cbDirection.SelectedIndex = index;
                    }
                }
                else if (keyValue[0].Equals(picturesFileName)) {
                    txtBoxFile.Text = keyValue[1];
                }
                else if (keyValue[0].Equals(picturesGifSpeed)) {
                    tbGifSpeed.Value = keyValue[1].ToInt(); 
                }
            }
        }

        private string _pictureName = "";
        private FastPixel _fp;
        private Color[,] _buffer;
        private int _bufferHeight;
        private int _bufferWidth;

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            _buffer = buffer;
            _bufferHeight = buffer.GetLength(0);
            _bufferWidth = buffer.GetLength(1);
            const int speedfactor = 4;

            if (txtBoxFile.Text != _pictureName) {
                _pictureName = txtBoxFile.Text;
                var image = Image.FromFile(_pictureName);
                _fp = new FastPixel(new Bitmap(image));
            }

            if (_fp == null) {
                return _buffer;
            }

            var imgwidth = _fp.Width;
            var imght = _fp.Height;
            var yoffset = (_bufferHeight + imght) / 2;
            var xoffset = (imgwidth - _bufferWidth) / 2;
            var limit = (cbDirection.SelectedIndex < 2) ? imgwidth + _bufferWidth : imght + _bufferHeight;
            var movement = Convert.ToInt32((eventToRender % (limit * speedfactor)) / speedfactor);

            _fp.Lock();
            for (var x = 0; x < imgwidth; x++) {
                for (var y = 0; y < imght; y++) {
                    var fpColor = _fp.GetPixel(x, y);
                    if (fpColor == Color.Transparent) {
                        fpColor = Color.Black;
                    }

                    switch (cbDirection.SelectedIndex) {
                        case 0: // left
                            SetPixel(x + _bufferWidth - movement, yoffset - y, fpColor);
                            break;
                        case 1: // right
                            SetPixel(x + movement - imgwidth, yoffset - y, fpColor);
                            break;
                        case 2: // up
                            SetPixel(x - xoffset, movement - y, fpColor);
                            break;
                        case 3: // down
                            SetPixel(x - xoffset, _bufferHeight + imght - y - movement, fpColor);
                            break;
                        default: // no movement - centered
                            SetPixel(x - xoffset, yoffset - y, fpColor);
                            break;
                    }
                }
            }
            _fp.Unlock();
            return _buffer;
        }


        private void SetPixel(int x, int y, Color color) {
            if (x >= 0 && x < _bufferWidth && y >= 0 && y < _bufferHeight) {
                _buffer[y, x] = color;
            }
        }


        private class FastPixel {
            private byte[] _rgbValues;
            private BitmapData _bmpData;
            private IntPtr _bmpPtr;
            private bool _locked;
            private readonly bool _isAlpha;
            private readonly Bitmap _bitmap;
            private readonly int _width;
            private readonly int _height;

            public int Width {
                get {
                    return _width;
                }
            }

            public int Height {
                get {
                    return _height;
                }
            }


            public FastPixel(Bitmap bitmap) {
                // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
                if (bitmap.PixelFormat == (bitmap.PixelFormat | PixelFormat.Indexed))
                    throw new Exception("Cannot lock an Indexed image.");

                _bitmap = bitmap;
                _isAlpha = (bitmap.PixelFormat == (bitmap.PixelFormat | PixelFormat.Alpha));
                _width = bitmap.Width;
                _height = bitmap.Height;
                // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
            }

            public void Lock() {
                if (_locked)
                    throw new Exception("Bitmap already locked.");

                var rect = new Rectangle(0, 0, _width, _height);
                _bmpData = _bitmap.LockBits(rect, ImageLockMode.ReadWrite, _bitmap.PixelFormat);
                _bmpPtr = _bmpData.Scan0;

                _rgbValues = new byte[ _width * _height * (_isAlpha ? 4 : 3)];
                Marshal.Copy(_bmpPtr, _rgbValues, 0, _rgbValues.Length);
                _locked = true;
            }

            public void Unlock() {
                if (!_locked)
                    throw new Exception("Bitmap not locked.");
               
                _bitmap.UnlockBits(_bmpData);
                _locked = false;
            }

            public Color GetPixel(int x, int y) {
                if (!_locked)
                    throw new Exception("Bitmap not locked.");

                if (_isAlpha) {
                    var index = ((y * _width + x) * 4);
                    int b = _rgbValues[index];
                    int g = _rgbValues[index + 1];
                    int r = _rgbValues[index + 2];
                    return Color.FromArgb(255, r, g, b);
                }
                else {
                    var index = ((y * _width + x) * 3);
                    int b = _rgbValues[index];
                    int g = _rgbValues[index + 1];
                    int r = _rgbValues[index + 2];
                    return Color.FromArgb(r, g, b);
                }
            }
        }


        private void Pictures_ControlChanged(object sender, EventArgs e) {
            if (_initializing) return;
            OnControlChanged(this, e);
        }

        private void btnFile_Click(object sender, EventArgs e) {
            using (var dialog = new OpenFileDialog()) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }
                txtBoxFile.Text = dialog.FileName;
            }
        }
    }
}
