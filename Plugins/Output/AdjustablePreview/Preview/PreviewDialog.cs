using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Xml;



using VixenPlus;

using VixenPlusCommon;

namespace Preview {
    public partial class PreviewDialog : OutputPlugInUIBase {
        private uint[,] _backBuffer;
        private int _cellSize;
        private readonly SolidBrush _channelBrush = new SolidBrush(Color.Black);
        private readonly Color[] _channelColors;
        private readonly Dictionary<int, List<uint>> _channelDictionary;
        private byte[] _channelValues = new byte[0];
        private Image _originalBackground;
        private readonly int _startChannel;


        public PreviewDialog(XmlNode setupNode, IList<Channel> channels, int startChannel) {
            InitializeComponent();
            _startChannel = startChannel;
            _channelDictionary = new Dictionary<int, List<uint>>();
            _channelColors = new Color[channels.Count - startChannel];
            var isOutputRedirected = bool.Parse(VixenPlus.Xml.GetNodeAlways(setupNode, "RedirectOutputs", "False").InnerText);

            SetChannelColors(channels, startChannel, isOutputRedirected);
            SetChannelBitmaps(channels, isOutputRedirected, setupNode.SelectNodes("Channels/Channel"));
            SetBackgroundImage(setupNode.SelectSingleNode("BackgroundImage"));
            SetDisplayInfo(setupNode.SelectSingleNode("Display"));
        }


        private void SetDisplayInfo(XmlNode displayInfo) {
            if (displayInfo == null) {
                _backBuffer = new uint[0, 0];
                ImproperSetupMessage();
                return;
            }

            var singleNode = displayInfo.SelectSingleNode("PixelSize");
            if (singleNode != null) {
                _cellSize = Convert.ToInt32(singleNode.InnerText);
            }

            if (_originalBackground == null) {
                var obgWidth = displayInfo.SelectSingleNode("Width");
                var obgHeight = displayInfo.SelectSingleNode("Height");

                if (obgWidth != null && obgHeight != null) {
                    SetPictureBoxSize(Convert.ToInt32(obgWidth.InnerText) * (_cellSize + 1), Convert.ToInt32(obgHeight.InnerText) * (_cellSize + 1));
                }
            }
            else {
                SetPictureBoxSize(_originalBackground.Width, _originalBackground.Height);
            }

            var bgBrightness = displayInfo.SelectSingleNode("Brightness");
            if (bgBrightness != null) {
                SetBrightness((int.Parse(bgBrightness.InnerText) - 10) / 10f);
            }

            var bgWidth = displayInfo.SelectSingleNode("Width");
            var bgHeight = displayInfo.SelectSingleNode("Height");
            if (bgWidth != null && bgHeight != null) {
                _backBuffer = new uint[Convert.ToInt32(bgHeight.InnerText),Convert.ToInt32(bgWidth.InnerText)];
            }
        }


        private void ImproperSetupMessage() {
            const string text = @"Preview not setup properly!";

            SizeF textSize;
            using (var g = pictureBoxShowGrid.CreateGraphics()) {
                textSize = g.MeasureString(text, pictureBoxShowGrid.Font);
            }

            var img = new Bitmap((int) textSize.Width, (int) textSize.Height);
            using (var drawing = Graphics.FromImage(img)) {
                drawing.Clear(Color.Black);
                Brush textBrush = new SolidBrush(Color.White);
                drawing.DrawString(text, pictureBoxShowGrid.Font, textBrush, 0, 0);
                drawing.Save();
                textBrush.Dispose();
            }
            _originalBackground = img;

            SetPictureBoxSize(_originalBackground.Width, _originalBackground.Height);
            SetBrightness(0f);
        }


        private void SetChannelColors(IList<Channel> channels, int startChannel, bool isOutputRedirected) {
            for (var i = startChannel; i < channels.Count; i++) {
                var outputChannel = isOutputRedirected ? i : channels[i].OutputChannel;
                if (outputChannel >= startChannel) {
                    _channelColors[outputChannel - startChannel] = channels[i].Color;
                }
            }
        }


        private void SetBackgroundImage(XmlNode xmlBackgroundImage) {
            if (xmlBackgroundImage == null) {
                return;
            }

            var bitmapData = Convert.FromBase64String(xmlBackgroundImage.InnerText);
            if (bitmapData.Length <= 0) {
                return;
            }

            using (var stream = new MemoryStream(bitmapData)) {
                _originalBackground = new Bitmap(stream);
            }
        }


        private void SetChannelBitmaps(IList<Channel> channels, bool isOutputRedirected, IEnumerable xmlChannels) {
            if (xmlChannels == null) {
                ImproperSetupMessage();
                return;
            }

            foreach (XmlNode aChannel in xmlChannels) {
                if (aChannel.Attributes == null) {
                    continue;
                }

                var channelNumber = Convert.ToInt32(aChannel.Attributes["number"].Value);
                if (channelNumber < _startChannel) {
                    continue;
                }

                var channelBitmapData = new List<uint>();
                var bitmap = Convert.FromBase64String(aChannel.InnerText);
                for (var i = 0; i < bitmap.Length; i += 4) {
                    channelBitmapData.Add(BitConverter.ToUInt32(bitmap, i));
                }

                int channelIndex;
                if ((channelIndex = channelNumber - _startChannel) >= channels.Count) {
                    continue;
                }

                var mappedIndex = isOutputRedirected ? channelIndex : channels[channelIndex].OutputChannel;
                _channelDictionary[mappedIndex] = channelBitmapData;
            }
        }


        private void pictureBoxShowGrid_Paint(object sender, PaintEventArgs e) {
            e.Graphics.FillRectangle(Brushes.Transparent, pictureBoxShowGrid.ClientRectangle);
            var height = _backBuffer.GetLength(Utils.IndexRowsOrHeight);
            var width = _backBuffer.GetLength(Utils.IndexColsOrWidth);
            var cellOffset = _cellSize + 1;

            for (var row = 0; row < height; row++) {
                for (var column = 0; column < width; column++) {
                    var pointEventValue = _backBuffer[row, column];
                    if ((pointEventValue & 0xff000000) <= 0) {
                        continue;
                    }
                    _channelBrush.Color = Color.FromArgb((int) pointEventValue);
                    e.Graphics.FillRectangle(_channelBrush, column * cellOffset, row * cellOffset, _cellSize, _cellSize);
                }
            }
        }


        private void PreviewDialog_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = e.CloseReason == CloseReason.UserClosing;
        }


        private void RecalcColors() {
            Array.Clear(_backBuffer, 0, _backBuffer.Length);
            var height = _backBuffer.GetLength(Utils.IndexRowsOrHeight);
            var width = _backBuffer.GetLength(Utils.IndexColsOrWidth);
            var index = 0;
            foreach (var alpha in _channelValues) {
                var channelColor = _channelColors[index];
                if (_channelDictionary.ContainsKey(index) && (alpha > 0)) {
                    foreach (var point in _channelDictionary[index]) {
                        var column = (int) (point >> 16);
                        var row = ((int) point) & 0xffff;
                        if ((column >= width) || (row >= height)) {
                            continue;
                        }

                        var a = alpha;
                        var r = channelColor.R;
                        var g = channelColor.G;
                        var b = channelColor.B;
                        var pointEventValue = _backBuffer[row, column];
                        if (pointEventValue != 0) {
                            var eventColor = Color.FromArgb((int) pointEventValue);

                            a = Math.Max(alpha, eventColor.A);
                            var channelAlpha = alpha / ((float) a);
                            var existingAlpha = eventColor.A / ((float) a);

                            r = (byte) (((int) ((eventColor.R * existingAlpha) + (channelColor.R * channelAlpha))) >> 1);
                            g = (byte) (((int) ((eventColor.G * existingAlpha) + (channelColor.G * channelAlpha))) >> 1);
                            b = (byte) (((int) ((eventColor.B * existingAlpha) + (channelColor.B * channelAlpha))) >> 1);
                        }
                        pointEventValue = (uint) ((((a << 24) | (r << 16)) | (g << 8)) | b);
                        _backBuffer[row, column] = pointEventValue;
                    }
                }
                index++;
            }
        }


        private void SetBrightness(float opacity) {
            if (_originalBackground == null) {
                return;
            }

            pictureBoxShowGrid.BackgroundImage = BgImage.GetImage(_originalBackground, opacity);
        }


        private void SetPictureBoxSize(int width, int height) {
            var computedWidth = width - pictureBoxShowGrid.Width;
            var computedHeight = height - pictureBoxShowGrid.Height;
            Width += computedWidth;
            Height += computedHeight;
            pictureBoxShowGrid.Width += computedWidth;
            pictureBoxShowGrid.Height += computedHeight;
        }


        public void UpdateWith(byte[] channelValues) {
            _channelValues = channelValues;
            RecalcColors();
            pictureBoxShowGrid.Refresh();
        }


        private const int ClassStyleNoClose = 0x200;

        protected override CreateParams CreateParams {
            get {
                var createParams = base.CreateParams;
                createParams.ClassStyle |= ClassStyleNoClose;
                return createParams;
            }
        }
    }
}
