using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using VixenPlus;

namespace Preview {
    public partial class SetupDialog : Form {
        private string _backgroundImageFileName;
        private int _cellSize;
        private Dictionary<int, List<uint>> _channelDictionary;
        private readonly List<Channel> _channels;
        private bool _controlDown;
        private bool _dirty;
        private Image _originalBackground;
        private bool _resizing;
        private readonly SetupData _setupData;
        private readonly XmlNode _setupNode;
        private readonly int _startChannel;


        public SetupDialog(SetupData setupData, XmlNode setupNode, List<Channel> channels, int startChannel) {
            int num;
            _setupNode = null;
            _setupData = null;
            _backgroundImageFileName = string.Empty;
            _dirty = false;
            _channelDictionary = new Dictionary<int, List<uint>>();
            _channels = null;
            _controlDown = false;
            _originalBackground = null;
            _resizing = false;
            components = null;
            InitializeComponent();
            _setupData = setupData;
            _setupNode = setupNode;
            _startChannel = startChannel;
            toolStripComboBoxPixelSize.SelectedIndex = 7;
            toolStripComboBoxChannels.BeginUpdate();
            for (num = _startChannel; num < channels.Count; num++) {
                toolStripComboBoxChannels.Items.Add(string.Format("{0}: {1}", num + 1, channels[num].Name));
            }
            toolStripComboBoxChannels.EndUpdate();
            _channels = new List<Channel>();
            for (num = _startChannel; num < channels.Count; num++) {
                _channels.Add(channels[num]);
            }
            UpdateFromSetupNode();
        }


        private void allChannelsToolStripMenuItem_Click(object sender, EventArgs e) {
            _channelDictionary.Clear();
            pictureBoxSetupGrid.Refresh();
            _dirty = true;
        }


        private void buttonOK_Click(object sender, EventArgs e) {
            UpdateSetup();
            _dirty = false;
        }


        //ComponentResourceManager manager = new ComponentResourceManager(typeof(SetupDialog));
        //toolStripDropDownButtonUpdate.Image = (Image)manager.GetObject("toolStripDropDownButtonUpdate.Image");
        //toolStripButtonResetSize.Image = (Image)manager.GetObject("toolStripButtonResetSize.Image");
        //toolStripDropDownButtonClear.Image = (Image)manager.GetObject("toolStripDropDownButtonClear.Image");
        //toolStripButtonLoadImage.Image = (Image)manager.GetObject("toolStripButtonLoadImage.Image");
        //toolStripButtonClearImage.Image = (Image)manager.GetObject("toolStripButtonClearImage.Image");
        //toolStripButtonSaveImage.Image = (Image)manager.GetObject("toolStripButtonSaveImage.Image");
        //toolStripButtonReorder.Image = (Image)manager.GetObject("toolStripButtonReorder.Image");

        private void pictureBoxSetupGrid_MouseEvent(object sender, MouseEventArgs e) {
            if ((e.X < 0) || (e.Y < 0)) {
                return;
            }

            var x = e.X / (_cellSize + 1);
            var y = e.Y / (_cellSize + 1);
            if ((x >= pictureBoxSetupGrid.Width) || (y >= pictureBoxSetupGrid.Height)) {
                return;
            }

            switch (e.Button) {
                case MouseButtons.Left:
                    SetPixelChannelReference(toolStripComboBoxChannels.SelectedIndex, x, y);
                    break;
                case MouseButtons.Right:
                    if (!_controlDown) {
                        ResetPixelChannelReference(toolStripComboBoxChannels.SelectedIndex, x, y);
                    }
                    else {
                        ResetPixelChannelReference(-1, x, y);
                    }
                    break;
                default:
                    var item = (uint) ((x << 0x10) | y);
                    var builder = new StringBuilder();
                    foreach (var num4 in _channelDictionary.Keys) {
                        if ((num4 < _channels.Count) && _channelDictionary[num4].Contains(item)) {
                            builder.AppendFormat("{0}, ", toolStripComboBoxChannels.Items[num4]);
                        }
                    }
                    if (builder.Length > 0) {
                        var str = builder.ToString();
                        labelChannel.Text = str.Substring(0, str.Length - 2);
                    }
                    else {
                        labelChannel.Text = string.Empty;
                    }
                    break;
            }
        }


        private void pictureBoxSetupGrid_Paint(object sender, PaintEventArgs e) {
            var hasOriginalBackground = _originalBackground != null;
            using (var brush = new SolidBrush(Color.Black)) {
                var selectedIndex = toolStripComboBoxChannels.SelectedIndex;
                e.Graphics.FillRectangle(hasOriginalBackground ? Brushes.Transparent : Brushes.Black, e.ClipRectangle.Left, e.ClipRectangle.Top,
                                         e.ClipRectangle.Width, e.ClipRectangle.Height);
                int num;
                int num2;
                foreach (var num4 in _channelDictionary.Keys) {
                    if ((num4 == selectedIndex) || (num4 >= _channels.Count)) {
                        continue;
                    }

                    brush.Color = Color.FromArgb(0x80, _channels[num4].Color);
                    foreach (var num5 in _channelDictionary[num4]) {
                        num = (int) ((num5 >> 0x10) * (_cellSize + 1));
                        num2 = (int) ((num5 & 0xffff) * (_cellSize + 1));
                        if (e.ClipRectangle.Contains(num, num2)) {
                            e.Graphics.FillRectangle(brush, num, num2, _cellSize, _cellSize);
                        }
                    }
                }

                if (!_channelDictionary.ContainsKey(selectedIndex)) {
                    return;
                }

                var channel = _channels[selectedIndex];
                foreach (var num5 in _channelDictionary[selectedIndex]) {
                    num = (int) ((num5 >> 0x10) * (_cellSize + 1));
                    num2 = (int) ((num5 & 0xffff) * (_cellSize + 1));
                    if (e.ClipRectangle.Contains(num, num2)) {
                        e.Graphics.FillRectangle(channel.Brush, num, num2, _cellSize, _cellSize);
                    }
                }
            }
        }


        private void ResetPixelChannelReference(int channelIndex, int x, int y) {
            if (channelIndex == -1) {
                var item = (uint) ((x << 0x10) | y);
                foreach (var list in _channelDictionary.Values) {
                    list.Remove(item);
                }
                pictureBoxSetupGrid.Invalidate(new Rectangle(x * (_cellSize + 1), y * (_cellSize + 1), _cellSize, _cellSize));
            }
            else {
                List<uint> list2;
                if (_channelDictionary.TryGetValue(channelIndex, out list2)) {
                    list2.Remove((uint) ((x << 0x10) | y));
                    pictureBoxSetupGrid.Invalidate(new Rectangle(x * (_cellSize + 1), y * (_cellSize + 1), _cellSize, _cellSize));
                }
            }
            _dirty = true;
        }


        private void selectedChannelToolStripMenuItem_Click(object sender, EventArgs e) {
            if (toolStripComboBoxChannels.SelectedIndex == -1) {
                return;
            }

            _channelDictionary.Remove(toolStripComboBoxChannels.SelectedIndex);
            pictureBoxSetupGrid.Refresh();
            _dirty = true;
        }


        private void SetBackground(Image bitmap) {
            if (_originalBackground != null) {
                _originalBackground.Dispose();
            }
            _originalBackground = bitmap;
            labelBrightness.Visible = trackBarBrightness.Visible = toolStripButtonSaveImage.Enabled = bitmap != null;
            toolStripTextBoxResolutionX.Enabled = toolStripTextBoxResolutionY.Enabled = bitmap == null;
            if (bitmap != null) {
                trackBarBrightness.Value = 10;
            }
        }


        private void SetBrightness(float opacity) {
            if (_originalBackground == null) {
                return;
            }

            Image image = new Bitmap(_originalBackground);

            using (var g = Graphics.FromImage(image)) {
                using (var attributes = new ImageAttributes()) {
                    var matrix = new ColorMatrix {Matrix40 = opacity, Matrix41 = opacity, Matrix42 = opacity};
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    attributes.SetColorMatrix(matrix);
                    g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
            }

            pictureBoxSetupGrid.BackgroundImage = image;
        }


        private void SetPictureBoxSize(int width, int height) {
            pictureBoxSetupGrid.Width = width;
            pictureBoxSetupGrid.Height = height;
            UpdatePosition();
            pictureBoxSetupGrid.Refresh();
        }


        private void SetPixelChannelReference(int channelIndex, int x, int y) {
            if (channelIndex == -1) {
                return;
            }

            List<uint> list;
            if (!_channelDictionary.TryGetValue(channelIndex, out list)) {
                list = new List<uint>();
                _channelDictionary[channelIndex] = list;
            }
            var item = (uint) ((x << 0x10) | y);
            if (!list.Contains(item)) {
                list.Add(item);
            }
            pictureBoxSetupGrid.Invalidate(new Rectangle(x * (_cellSize + 1), y * (_cellSize + 1), _cellSize, _cellSize));
            _dirty = true;
        }


        private void SetupDialog_FormClosing(object sender, FormClosingEventArgs e) {
            if (!_dirty) {
                return;
            }

            switch (MessageBox.Show("Keep changes?", "Vixen preview", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;

                case DialogResult.Yes:
                    UpdateSetup();
                    break;
            }
        }


        private void SetupDialog_KeyDown(object sender, KeyEventArgs e) {
            _controlDown = e.Control;
        }


        private void SetupDialog_KeyUp(object sender, KeyEventArgs e) {
            _controlDown = e.Control;
        }


        private void SetupDialog_Resize(object sender, EventArgs e) {
            if (!_resizing) {
                UpdatePosition();
            }
        }


        private void SetupDialog_ResizeBegin(object sender, EventArgs e) {
            _resizing = true;
        }


        private void SetupDialog_ResizeEnd(object sender, EventArgs e) {
            _resizing = false;
            UpdatePosition();
        }


        private void toolStripButtonClearImage_Click(object sender, EventArgs e) {
            pictureBoxSetupGrid.BackgroundImage = null;
            SetBackground(null);
            _backgroundImageFileName = string.Empty;
            _dirty = true;
        }


        private void toolStripButtonLoadImage_Click(object sender, EventArgs e) {
            if (openFileDialog.ShowDialog() != DialogResult.OK) {
                return;
            }

            byte[] buffer;
            using (var stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int) stream.Length);
            }
            Bitmap bitmap;
            using (var stream2 = new MemoryStream(buffer)) {
                bitmap = new Bitmap(stream2);
            }
            SetPictureBoxSize(bitmap.Width, bitmap.Height);
            SetBackground(bitmap);
            SetBrightness(0f);
            _backgroundImageFileName = openFileDialog.FileName;
            _dirty = true;
        }


        private void toolStripButtonReorder_Click(object sender, EventArgs e) {
            using (var reorder = new ReorderDialog(_channels, _channelDictionary)) {
                reorder.ShowDialog();
                if (reorder.DialogResult == DialogResult.OK) {
                    _channelDictionary = reorder.ChannelDictionary;
                }
            }
        }


        private void toolStripButtonResetSize_Click(object sender, EventArgs e) {
            if (_originalBackground == null) {
                return;
            }

            SetPictureBoxSize(_originalBackground.Width, _originalBackground.Height);
            UpdateSizeUI();
        }


        private void toolStripButtonSaveImage_Click(object sender, EventArgs e) {
            if (saveFileDialog.ShowDialog() != DialogResult.OK) {
                return;
            }

            new Bitmap(_originalBackground).Save(Path.ChangeExtension(saveFileDialog.FileName, ".jpg"), ImageFormat.Jpeg);
            MessageBox.Show("File saved.", "Preview", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        private void toolStripComboBoxChannels_SelectedIndexChanged(object sender, EventArgs e) {
            pictureBoxSetupGrid.Refresh();
        }


        private void toolStripDropDownButtonUpdate_Click(object sender, EventArgs e) {
            int width;
            int height;
            _cellSize = toolStripComboBoxPixelSize.SelectedIndex + 1;
            try {
                width = int.Parse(toolStripTextBoxResolutionX.Text) * (_cellSize + 1);
            }
            catch {
                width = pictureBoxSetupGrid.Width;
            }
            try {
                height = int.Parse(toolStripTextBoxResolutionY.Text) * (_cellSize + 1);
            }
            catch {
                height = pictureBoxSetupGrid.Height;
            }
            SetPictureBoxSize(width, height);
        }


        private void trackBarBrightness_ValueChanged(object sender, EventArgs e) {
            SetBrightness((trackBarBrightness.Value - 10) / 10f);
        }


        private void UpdateFromSetupNode() {
            var width = 64;
            var height = 32;
            _cellSize = 8;

            checkBoxRedirectOutputs.Checked = _setupData.GetBoolean(_setupNode, "RedirectOutputs", false);
            var setupDataNode = _setupNode.SelectSingleNode("Display");
            if (setupDataNode != null) {
                var heightNode = setupDataNode.SelectSingleNode("Height");
                if (heightNode != null) {
                    height = Convert.ToInt32(heightNode.InnerText);
                }
                var widthNode = setupDataNode.SelectSingleNode("Width");
                if (widthNode != null) {
                    width = Convert.ToInt32(widthNode.InnerText);
                }
                var pixelSizeNode = setupDataNode.SelectSingleNode("PixelSize");
                if (pixelSizeNode != null) {
                    _cellSize = Convert.ToInt32(pixelSizeNode.InnerText);
                }
            }

            SetPictureBoxSize(width * (_cellSize + 1), height * (_cellSize + 1));
            UpdateSizeUI();
            var backgroundImageNode = _setupNode.SelectSingleNode("BackgroundImage");
            if (backgroundImageNode != null) {
                var buffer = Convert.FromBase64String(backgroundImageNode.InnerText);
                if (buffer.Length > 0) {
                    using (var stream = new MemoryStream(buffer)) {
                        SetBackground(new Bitmap(stream));
                    }
                }
                else {
                    SetBackground(null);
                }
            }
            if (setupDataNode != null) {
                trackBarBrightness.Value = _setupData.GetInteger(setupDataNode, "Brightness", 10);
                trackBarBrightness_ValueChanged(null, null);
            }
            var channelNode = _setupNode.SelectNodes("Channels/Channel");
            if (channelNode != null) {
                foreach (XmlNode channel in channelNode) {
                    if (channel.Attributes != null) {
                        var num3 = Convert.ToInt32(channel.Attributes["number"].Value);
                        if (num3 < _startChannel) {
                            continue;
                        }
                        var list = new List<uint>();
                        var buffer2 = Convert.FromBase64String(channel.InnerText);
                        for (var i = 0; i < buffer2.Length; i += 4) {
                            list.Add(BitConverter.ToUInt32(buffer2, i));
                        }
                        _channelDictionary[num3 - _startChannel] = list;
                    }
                }
            }
            pictureBoxSetupGrid.Refresh();
        }


        private void UpdatePosition() {
            var point = new Point();
            if (pictureBoxSetupGrid.Width > panelPictureBoxContainer.Width) {
                point.X = 0;
            }
            else {
                point.X = ((panelPictureBoxContainer.Width - pictureBoxSetupGrid.Width) / 2) + ClientRectangle.Left;
            }
            if (pictureBoxSetupGrid.Height > panelPictureBoxContainer.Height) {
                point.Y = 0;
            }
            else {
                point.Y = ((panelPictureBoxContainer.Height - pictureBoxSetupGrid.Height) / 2) + ClientRectangle.Top;
            }
            pictureBoxSetupGrid.Location = point;
            int num = trackBarBrightness.Right - labelBrightness.Left;
            int num2 = panel1.Width / 2;
            labelBrightness.Left = num2 - (num / 2);
            trackBarBrightness.Left = (labelBrightness.Left + num) - trackBarBrightness.Width;
        }


        private void UpdateSetup() {
            var contextNode = Xml.SetValue(_setupNode, "Display", string.Empty);
            Xml.SetValue(contextNode, "Height", toolStripTextBoxResolutionY.Text);
            Xml.SetValue(contextNode, "Width", toolStripTextBoxResolutionX.Text);
            Xml.SetValue(contextNode, "PixelSize", _cellSize.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(contextNode, "Brightness", trackBarBrightness.Value.ToString(CultureInfo.InvariantCulture));
            if (_originalBackground != null) {
                if (_backgroundImageFileName != string.Empty) {
                    using (var stream = new FileStream(_backgroundImageFileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        var buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int) stream.Length);
                        var backgroundImageNode = _setupNode.SelectSingleNode("BackgroundImage");
                        if (backgroundImageNode != null) {
                            backgroundImageNode.InnerText = Convert.ToBase64String(buffer);
                        }
                    }
                }
            }
            else {
                var backgroundImageNode = _setupNode.SelectSingleNode("BackgroundImage");
                if (backgroundImageNode != null) {
                    backgroundImageNode.InnerText = Convert.ToBase64String(new byte[0]);
                }
            }
            var emptyNodeAlways = Xml.GetEmptyNodeAlways(_setupNode, "Channels");
            var list = new List<byte>();
            foreach (var num in _channelDictionary.Keys) {
                list.Clear();
                var node = Xml.SetNewValue(emptyNodeAlways, "Channel", string.Empty);
                Xml.SetAttribute(node, "number", (num + _startChannel).ToString(CultureInfo.InvariantCulture));
                foreach (var num2 in _channelDictionary[num]) {
                    list.AddRange(BitConverter.GetBytes(num2));
                }
                node.InnerText = Convert.ToBase64String(list.ToArray());
            }
            _setupData.SetBoolean(_setupNode, "RedirectOutputs", checkBoxRedirectOutputs.Checked);
        }


        private void UpdateSizeUI() {
            toolStripTextBoxResolutionX.Text = (pictureBoxSetupGrid.Width / (_cellSize + 1)).ToString(CultureInfo.InvariantCulture);
            toolStripTextBoxResolutionY.Text = (pictureBoxSetupGrid.Height / (_cellSize + 1)).ToString(CultureInfo.InvariantCulture);
            toolStripComboBoxPixelSize.SelectedIndex = _cellSize - 1;
        }
    }
}
