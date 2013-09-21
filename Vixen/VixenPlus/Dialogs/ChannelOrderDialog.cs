using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CommonUtils;

namespace VixenPlus.Dialogs {
    public partial class ChannelOrderDialog : Form {
        private List<Channel> _channelNaturalOrder;
        private bool _controlDown;
        private bool _initializing;
        private int _insertIndex;
        private int _insertionIndex;
        private bool _mouseDown;
        private int _selectedIndex;
        private bool _showNaturalNumber;

        private const int RowHeight = 40;
        private const int ScrollPoint = 10;


        public ChannelOrderDialog(IList<Channel> channelList, IEnumerable<int> channelOrder) {
            _mouseDown = false;
            _selectedIndex = -1;
            _insertIndex = -1;
            _initializing = true;
            _insertionIndex = -1;
            _controlDown = false;
            components = null;
            InitializeComponent();
            Construct(channelList, channelOrder);
        }


        public ChannelOrderDialog(IList<Channel> channelList, IEnumerable<int> channelOrder, string caption) : this(channelList, channelOrder) {
            Text = caption;
        }


        public override sealed string Text {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public List<Channel> ChannelMapping { get; private set; }


        private void CalcScrollParams() {
            if (_initializing) {
                return;
            }

            var num = (int) Math.Round((((pictureBoxChannels.Height - 10)) / (float) RowHeight), MidpointRounding.AwayFromZero);
            vScrollBar.Maximum = ChannelMapping.Count - 1;
            vScrollBar.LargeChange = num;
            if ((vScrollBar.LargeChange + vScrollBar.Value) > vScrollBar.Maximum) {
                vScrollBar.Value = (vScrollBar.Maximum - vScrollBar.LargeChange) + 1;
            }
        }


        private void ChannelOrderDialog_HelpButtonClicked(object sender, CancelEventArgs e) {
            e.Cancel = true;
            using (
                var dialog =
                    new HelpDialog(
                        "Drag channels into their new positions, or\n\nDouble-click at an insertion point.  Then Ctrl+Click on channels to move to that point.\nThe insertion point will automatically move with each channel inserted.")
                ) {
                dialog.ShowDialog();
            }
        }


        private void ChannelOrderDialog_KeyDown(object sender, KeyEventArgs e) {
            _controlDown = e.Control;
        }


        private void ChannelOrderDialog_KeyUp(object sender, KeyEventArgs e) {
            _controlDown = e.Control;
        }


        private void Construct(IList<Channel> channelList, IEnumerable<int> channelOrder) {
            _initializing = false;
            _channelNaturalOrder = new List<Channel>();
            _channelNaturalOrder.AddRange(channelList);
            ChannelMapping = new List<Channel>();
            if (channelOrder == null) {
                ChannelMapping.AddRange(channelList);
            }
            else {
                foreach (var num in channelOrder) {
                    ChannelMapping.Add(channelList[num]);
                }
            }
            _showNaturalNumber = ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ShowNaturalChannelNumber");
            CalcScrollParams();
        }


        private void pictureBoxChannels_MouseDoubleClick(object sender, MouseEventArgs e) {
            _mouseDown = false;
            if (_controlDown) {
                return;
            }
            _insertionIndex = vScrollBar.Value + (e.Y / RowHeight);
            pictureBoxChannels.Refresh();
        }


        private void pictureBoxChannels_MouseDown(object sender, MouseEventArgs e) {
            if (_controlDown) {
                if (_insertionIndex == -1) {
                    return;
                }
                var index = vScrollBar.Value + (e.Y / RowHeight);
                if (_insertionIndex == index) {
                    return;
                }
                if (_insertionIndex > index) {
                    _insertionIndex--;
                }
                var item = ChannelMapping[index];
                ChannelMapping.RemoveAt(index);
                ChannelMapping.Insert(_insertionIndex, item);
                if (_insertionIndex < ChannelMapping.Count) {
                    _insertionIndex++;
                }
                if (_insertionIndex < index) {
                    if (vScrollBar.Value <= (vScrollBar.Maximum - vScrollBar.LargeChange)) {
                        vScrollBar.Value++;
                    }
                    else {
                        pictureBoxChannels.Refresh();
                    }
                }
                else {
                    pictureBoxChannels.Refresh();
                }
            }
            else {
                _mouseDown = true;
                _selectedIndex = vScrollBar.Value + (e.Y / RowHeight);
            }
        }


        private void pictureBoxChannels_MouseMove(object sender, MouseEventArgs e) {
            if (!_mouseDown || ((e.Y < 0) || (e.Y > pictureBoxChannels.Height))) {
                return;
            }

            if (e.Y < ScrollPoint) {
                ScrollUp();
            }
            else if (e.Y > (pictureBoxChannels.Height - ScrollPoint)) {
                ScrollDown();
            }
            else {
                var num = vScrollBar.Value + (e.Y / RowHeight);
                if (num == _insertIndex) {
                    return;
                }
                if (_selectedIndex != num) {
                    _insertIndex = num;
                }
                else {
                    _insertIndex = -1;
                }
            }
            pictureBoxChannels.Refresh();
        }


        private void pictureBoxChannels_MouseUp(object sender, MouseEventArgs e) {
            if (_insertIndex == -1) {
                return;
            }

            if (_insertIndex != _selectedIndex && _selectedIndex <= ChannelMapping.Count) {
                var item = ChannelMapping[_selectedIndex];
                ChannelMapping.RemoveAt(_selectedIndex);
                _insertIndex = Math.Min(_insertIndex, ChannelMapping.Count + 1);
                if (_selectedIndex > _insertIndex) {
                    ChannelMapping.Insert(_insertIndex, item);
                }
                else {
                    ChannelMapping.Insert(_insertIndex - 1, item);
                }
            }
            _mouseDown = false;
            _selectedIndex = -1;
            _insertIndex = -1;
            pictureBoxChannels.Refresh();
        }


        private void pictureBoxChannels_Paint(object sender, PaintEventArgs e) {
            if (ChannelMapping.Count == 0) {
                return;
            }

            var boarderPen = new Pen(Color.Black, 2f);
            var backgroundBrush = new SolidBrush(Color.White);
            var font = new Font("Arial", 14f, FontStyle.Bold);

            const int channelMargin = 10;
            var rect = new Rectangle
            {Width = pictureBoxChannels.Width - channelMargin * 2, X = channelMargin, Height = RowHeight - channelMargin, Y = channelMargin};

            for (var i = 0; i < vScrollBar.LargeChange; i++) {
                var channel = ChannelMapping[i + vScrollBar.Value];
                if (channel.Color.ToArgb() != -1) {
                    boarderPen.Color = channel.Color;
                    backgroundBrush.Color = Color.FromArgb(64, channel.Color);
                }
                else {
                    boarderPen.Color = Color.Black;
                    backgroundBrush.Color = Color.White;
                }
                e.Graphics.FillRectangle(backgroundBrush, rect);
                e.Graphics.DrawRectangle(boarderPen, rect);
                e.Graphics.DrawString(
                    _showNaturalNumber ? string.Format("{0}: {1}", _channelNaturalOrder.IndexOf(channel) + 1, channel.Name) : channel.Name, font,
                    backgroundBrush.Color.GetTextColor(), 15f, (rect.Top + 5));
                rect.Y += RowHeight;
            }

            if (_insertIndex != -1) {
                var caretY = (_insertIndex - vScrollBar.Value) * RowHeight;
                const int insertionHalf = 5;
                var points = new[] {
                    new Point(insertionHalf, (caretY - insertionHalf) + insertionHalf), new Point(insertionHalf * 2, caretY + insertionHalf),
                    new Point(insertionHalf, (caretY + insertionHalf) + insertionHalf)
                };
                e.Graphics.DrawPolygon(Pens.Black, points);
            }
            if (_insertionIndex != -1) {
                var caretY = ((_insertionIndex - vScrollBar.Value) * RowHeight) + 2;
                e.Graphics.FillRectangle(Brushes.Gray, 0, caretY, pictureBoxChannels.Width, 6);
            }
            font.Dispose();
            backgroundBrush.Dispose();
            boarderPen.Dispose();
        }


        private void pictureBoxChannels_Resize(object sender, EventArgs e) {
            RecalcAndRedraw();
        }


        private void RecalcAndRedraw() {
            CalcScrollParams();
            pictureBoxChannels.Refresh();
        }


        private void ScrollDown() {
            var y = pictureBoxChannels.PointToScreen(new Point(pictureBoxChannels.Left, pictureBoxChannels.Bottom - ScrollPoint)).Y;
            while (MousePosition.Y > y) {
                if (vScrollBar.Value > (vScrollBar.Maximum - vScrollBar.LargeChange)) {
                    break;
                }
                _insertIndex++;
                vScrollBar.Value++;
            }
        }


        private void ScrollUp() {
            var y = pictureBoxChannels.PointToScreen(new Point(pictureBoxChannels.Left, pictureBoxChannels.Top + ScrollPoint)).Y;
            while (MousePosition.Y < y) {
                if (vScrollBar.Value == 0) {
                    break;
                }
                _insertIndex--;
                vScrollBar.Value--;
                pictureBoxChannels.Refresh();
            }
        }


        private void vScrollBar_ValueChanged(object sender, EventArgs e) {
            pictureBoxChannels.Refresh();
        }
    }
}
