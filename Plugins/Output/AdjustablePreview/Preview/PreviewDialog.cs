using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using VixenPlus;

namespace Preview {
    public partial class PreviewDialog : OutputPlugInUIBase {
        private readonly uint[,] _backBuffer;
        private readonly int _cellSize;
        private readonly SolidBrush _channelBrush = new SolidBrush(Color.Black);
        private readonly Color[] _channelColors;
        private readonly Dictionary<int, List<uint>> _channelDictionary;
        private byte[] _channelValues = new byte[0];
        private readonly Image _originalBackground;
        private readonly int _startChannel;



        public PreviewDialog(XmlNode setupNode, IList<Channel> channels, int startChannel) {
            InitializeComponent();
            try {
                _startChannel = startChannel;
                _channelDictionary = new Dictionary<int, List<uint>>();
                var flag = bool.Parse(VixenPlus.Xml.GetNodeAlways(setupNode, "RedirectOutputs", "False").InnerText);
                _channelColors = new Color[channels.Count - startChannel];
                for (var i = startChannel; i < channels.Count; i++) {
                    var outputChannel = flag ? i : channels[i].OutputChannel;
                    if (outputChannel >= startChannel) {
                        _channelColors[outputChannel - startChannel] = channels[i].Color;
                    }
                }
                var xmlNodeList = setupNode.SelectNodes("Channels/Channel");
                if (xmlNodeList != null) {
                    foreach (XmlNode node in xmlNodeList) {
                        if (node.Attributes == null) {
                            continue;
                        }

                        var num3 = Convert.ToInt32(node.Attributes["number"].Value);
                        if (num3 < _startChannel) {
                            continue;
                        }

                        int num5;
                        var list = new List<uint>();
                        var buffer = Convert.FromBase64String(node.InnerText);
                        for (var j = 0; j < buffer.Length; j += 4) {
                            list.Add(BitConverter.ToUInt32(buffer, j));
                        }
                        if ((num5 = num3 - _startChannel) >= channels.Count) {
                            continue;
                        }
                        if (flag) {
                            _channelDictionary[num5] = list;
                        }
                        else {
                            _channelDictionary[channels[num5].OutputChannel] = list;
                        }
                    }
                }
                var selectSingleNode = setupNode.SelectSingleNode("BackgroundImage");
                if (selectSingleNode != null) {
                    var buffer2 = Convert.FromBase64String(selectSingleNode.InnerText);
                    if (buffer2.Length > 0) {
                        using (var stream = new MemoryStream(buffer2)) {
                            _originalBackground = new Bitmap(stream);
                        }
                    }
                }
                var node2 = setupNode.SelectSingleNode("Display");
                if (node2 == null) {
                    return;
                }

                var singleNode = node2.SelectSingleNode("PixelSize");
                if (singleNode != null) {
                    _cellSize = Convert.ToInt32(singleNode.InnerText);
                }
                if (_originalBackground == null) {
                    var xmlNode = node2.SelectSingleNode("Width");
                    if (xmlNode != null) {
                        var node = node2.SelectSingleNode("Height");
                        if (node != null) {
                            SetPictureBoxSize(Convert.ToInt32(xmlNode.InnerText) * (_cellSize + 1), Convert.ToInt32(node.InnerText) * (_cellSize + 1));
                        }
                    }
                }
                else {
                    SetPictureBoxSize(_originalBackground.Width, _originalBackground.Height);
                }
                var selectSingleNode1 = node2.SelectSingleNode("Brightness");
                if (selectSingleNode1 != null) {
                    SetBrightness((int.Parse(selectSingleNode1.InnerText) - 10) / 10f);
                }
                var singleNode1 = node2.SelectSingleNode("Height");
                if (singleNode1 != null) {
                    var xmlNode = node2.SelectSingleNode("Width");
                    if (xmlNode != null) {
                        _backBuffer = new uint[Convert.ToInt32(singleNode1.InnerText),Convert.ToInt32(xmlNode.InnerText)];
                    }
                }
            }
            catch (NullReferenceException exception) {
                throw new Exception(exception.Message + "\n\nHave you run the setup?");
            }
        }








        private void pictureBoxShowGrid_Paint(object sender, PaintEventArgs e) {
            e.Graphics.FillRectangle(Brushes.Transparent, _pictureBoxShowGrid.ClientRectangle);
            var length = _backBuffer.GetLength(0);
            var num2 = _backBuffer.GetLength(1);
            var num5 = _cellSize + 1;
            var y = 0;
            var num8 = 0;
            while (num8 < length) {
                var x = 0;
                var num7 = 0;
                while (num7 < num2) {
                    var num6 = _backBuffer[num8, num7];
                    if ((num6 & 0xff000000) > 0) {
                        _channelBrush.Color = Color.FromArgb((int) num6);
                        e.Graphics.FillRectangle(_channelBrush, x, y, _cellSize, _cellSize);
                    }
                    num7++;
                    x += num5;
                }
                num8++;
                y += num5;
            }
        }


        private static void PreviewDialog_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = e.CloseReason == CloseReason.UserClosing;
        }


        private void Recalc() {
            var index = 0;
            Array.Clear(_backBuffer, 0, _backBuffer.Length);
            var length = _backBuffer.GetLength(1);
            var num15 = _backBuffer.GetLength(0);
            foreach (var num16 in _channelValues) {
                var color = _channelColors[index];
                if (_channelDictionary.ContainsKey(index) && (num16 > 0)) {
                    foreach (var num17 in _channelDictionary[index]) {
                        var num6 = (int) (num17 >> 0x10);
                        var num7 = ((int) num17) & 0xffff;
                        if ((num6 < length) && (num7 < num15)) {
                            byte r;
                            byte g;
                            byte b;
                            byte num8;
                            var num2 = _backBuffer[num7, num6];
                            if (num2 == 0) {
                                num8 = num16;
                                r = color.R;
                                g = color.G;
                                b = color.B;
                            }
                            else {
                                num8 = Math.Max(num16, (byte) (num2 >> 24));
                                var num12 = (num2 >> 24) / ((float) num8);
                                var num13 = num16 / ((float) num8);
                                var num9 = (byte) ((num2 >> 0x10) & 0xff);
                                var num10 = (byte) ((num2 >> 8) & 0xff);
                                var num11 = (byte) (num2 & 0xff);
                                r = (byte) (((int) ((num9 * num12) + (color.R * num13))) >> 1);
                                g = (byte) (((int) ((num10 * num12) + (color.G * num13))) >> 1);
                                b = (byte) (((int) ((num11 * num12) + (color.B * num13))) >> 1);
                            }
                            num2 = (uint) ((((num8 << 24) | (r << 0x10)) | (g << 8)) | b);
                            _backBuffer[num7, num6] = num2;
                        }
                    }
                }
                index++;
            }
        }


        private void SetBrightness(float value) {
            if (_originalBackground == null) {
                return;
            }

            Image image = new Bitmap(_originalBackground);
            var newColorMatrix = new float[5][];
            var numArray2 = new float[5];
            numArray2[0] = 1f;
            newColorMatrix[0] = numArray2;
            numArray2 = new float[5];
            numArray2[1] = 1f;
            newColorMatrix[1] = numArray2;
            numArray2 = new float[5];
            numArray2[2] = 1f;
            newColorMatrix[2] = numArray2;
            numArray2 = new float[5];
            numArray2[3] = 1f;
            newColorMatrix[3] = numArray2;
            numArray2 = new float[5];
            numArray2[0] = value;
            numArray2[1] = value;
            numArray2[2] = value;
            numArray2[4] = 1f;
            newColorMatrix[4] = numArray2;
            var matrix = new ColorMatrix(newColorMatrix);
            var graphics = Graphics.FromImage(image);
            var imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(matrix);
            graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            graphics.Dispose();
            imageAttr.Dispose();
            _pictureBoxShowGrid.BackgroundImage = image;
        }


        private void SetPictureBoxSize(int width, int height) {
            var num = width - _pictureBoxShowGrid.Width;
            var num2 = height - _pictureBoxShowGrid.Height;
            Width += num;
            Height += num2;
            _pictureBoxShowGrid.Width += num;
            _pictureBoxShowGrid.Height += num2;
        }


        public void UpdateWith(byte[] channelValues) {
            _channelValues = channelValues;
            Recalc();
            _pictureBoxShowGrid.Refresh();
        }


        protected override CreateParams CreateParams {
            get {
                var createParams = base.CreateParams;
                createParams.ClassStyle |= 0x200;
                return createParams;
            }
        }
    }
}
