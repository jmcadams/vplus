using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs
{
    public partial class ChannelOrderDialog : Form
    {
        private List<Channel> _channelMapping;
        private List<Channel> _channelNaturalOrder;
        private bool _controlDown;
        private bool _initializing;
        private int _insertIndex;
        private int _insertionIndex;
        private bool _mouseDown;
        private int _selectedIndex;
        private bool _showNaturalNumber;

        public ChannelOrderDialog(List<Channel> channelList, IEnumerable<int> channelOrder)
        {
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

        public ChannelOrderDialog(List<Channel> channelList, IEnumerable<int> channelOrder, string caption)
        {
            _mouseDown = false;
            _selectedIndex = -1;
            _insertIndex = -1;
            _initializing = true;
            _insertionIndex = -1;
            _controlDown = false;
            components = null;
            InitializeComponent();
            Construct(channelList, channelOrder);
            Text = caption;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public List<Channel> ChannelMapping
        {
            get { return _channelMapping; }
        }

        private void CalcScrollParams()
        {
            if (!_initializing)
            {
                var num = (int) Math.Round((((pictureBoxChannels.Height - 10))/40f), MidpointRounding.AwayFromZero);
                vScrollBar.Maximum = _channelMapping.Count - 1;
                vScrollBar.LargeChange = num;
                if ((vScrollBar.LargeChange + vScrollBar.Value) > vScrollBar.Maximum)
                {
                    vScrollBar.Value = (vScrollBar.Maximum - vScrollBar.LargeChange) + 1;
                }
            }
        }

        private void ChannelOrderDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            var dialog =
                new HelpDialog(
                    "Drag channels into their new positions, or\n\nDouble-click at an insertion point.  Then Ctrl+Click on channels to move to that point.\nThe insertion point will automatically move with each channel inserted.");
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void ChannelOrderDialog_KeyDown(object sender, KeyEventArgs e)
        {
            _controlDown = e.Control;
        }

        private void ChannelOrderDialog_KeyUp(object sender, KeyEventArgs e)
        {
            _controlDown = e.Control;
        }

        private void Construct(List<Channel> channelList, IEnumerable<int> channelOrder)
        {
            _initializing = false;
            _channelNaturalOrder = new List<Channel>();
            _channelNaturalOrder.AddRange(channelList);
            _channelMapping = new List<Channel>();
            if (channelOrder == null)
            {
                _channelMapping.AddRange(channelList);
            }
            else
            {
                foreach (int num in channelOrder)
                {
                    _channelMapping.Add(channelList[num]);
                }
            }
            _showNaturalNumber =
                ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ShowNaturalChannelNumber");
            CalcScrollParams();
        }


        private void pictureBoxChannels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
            if (!_controlDown)
            {
                _insertionIndex = vScrollBar.Value + (e.Y/40);
                pictureBoxChannels.Refresh();
            }
        }

        private void pictureBoxChannels_MouseDown(object sender, MouseEventArgs e)
        {
            if (_controlDown)
            {
                if (_insertionIndex != -1)
                {
                    int index = vScrollBar.Value + (e.Y/40);
                    if (_insertionIndex != index)
                    {
                        if (_insertionIndex > index)
                        {
                            _insertionIndex--;
                        }
                        Channel item = _channelMapping[index];
                        _channelMapping.RemoveAt(index);
                        _channelMapping.Insert(_insertionIndex, item);
                        if (_insertionIndex < _channelMapping.Count)
                        {
                            _insertionIndex++;
                        }
                        if (_insertionIndex < index)
                        {
                            if (vScrollBar.Value <= (vScrollBar.Maximum - vScrollBar.LargeChange))
                            {
                                vScrollBar.Value++;
                            }
                            else
                            {
                                pictureBoxChannels.Refresh();
                            }
                        }
                        else
                        {
                            pictureBoxChannels.Refresh();
                        }
                    }
                }
            }
            else
            {
                _mouseDown = true;
                _selectedIndex = vScrollBar.Value + (e.Y/40);
            }
        }

        private void pictureBoxChannels_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown && ((e.Y >= 0) && (e.Y <= pictureBoxChannels.Height)))
            {
                if (e.Y < 10)
                {
                    ScrollUp();
                }
                else if (e.Y > (pictureBoxChannels.Height - 10))
                {
                    ScrollDown();
                }
                else
                {
                    int num = vScrollBar.Value + (e.Y/40);
                    if (num == _insertIndex)
                    {
                        return;
                    }
                    if (_selectedIndex != num)
                    {
                        _insertIndex = num;
                    }
                    else
                    {
                        _insertIndex = -1;
                    }
                }
                pictureBoxChannels.Refresh();
            }
        }

        private void pictureBoxChannels_MouseUp(object sender, MouseEventArgs e)
        {
            if (_insertIndex != -1)
            {
                if (_insertIndex != _selectedIndex)
                {
                    Channel item = _channelMapping[_selectedIndex];
                    _channelMapping.RemoveAt(_selectedIndex);
                    if (_selectedIndex > _insertIndex)
                    {
                        _channelMapping.Insert(_insertIndex, item);
                    }
                    else
                    {
                        _channelMapping.Insert(_insertIndex - 1, item);
                    }
                }
                _mouseDown = false;
                _selectedIndex = -1;
                _insertIndex = -1;
                pictureBoxChannels.Refresh();
            }
        }

        private void pictureBoxChannels_Paint(object sender, PaintEventArgs e)
        {
            if (_channelMapping.Count != 0)
            {
                int num2;
                var pen = new Pen(Color.Black, 2f);
                var brush = new SolidBrush(Color.White);
                var rect = new Rectangle();
                var font = new Font("Arial", 14f, FontStyle.Bold);
                var brush2 = new SolidBrush(Color.Black);
                rect.Width = pictureBoxChannels.Width - 20;
                rect.X = 10;
                rect.Height = 30;
                rect.Y = 10;
                for (int i = 0; i < vScrollBar.LargeChange; i++)
                {
                    Channel item = _channelMapping[i + vScrollBar.Value];
                    if (item.Color.ToArgb() != -1)
                    {
                        pen.Color = item.Color;
                        brush.Color = Color.FromArgb(64, item.Color);
                    }
                    else
                    {
                        pen.Color = Color.Black;
                        brush.Color = Color.White;
                    }
                    brush2.Color = pen.Color;
                    e.Graphics.FillRectangle(brush, rect);
                    e.Graphics.DrawRectangle(pen, rect);
                    e.Graphics.DrawString(
                        _showNaturalNumber ? string.Format("{0}: {1}", _channelNaturalOrder.IndexOf(item) + 1, item.Name) : item.Name,
                        font,
                        Brushes.Black, 15f, (rect.Top + 5));
                    rect.Y += 40;
                }
                if (_insertIndex != -1)
                {
                    num2 = (_insertIndex - vScrollBar.Value)*40;
                    const int num3 = 5;
                    var points = new[] {new Point(5, (num2 - 5) + num3), new Point(10, num2 + num3), new Point(5, (num2 + 5) + num3)};
                    e.Graphics.DrawPolygon(Pens.Black, points);
                }
                if (_insertionIndex != -1)
                {
                    num2 = ((_insertionIndex - vScrollBar.Value)*40) + 2;
                    e.Graphics.FillRectangle(Brushes.Gray, 0, num2, pictureBoxChannels.Width, 6);
                }
                font.Dispose();
                brush.Dispose();
                brush2.Dispose();
                pen.Dispose();
            }
        }

        private void pictureBoxChannels_Resize(object sender, EventArgs e)
        {
            RecalcAndRedraw();
        }

        private void RecalcAndRedraw()
        {
            CalcScrollParams();
            pictureBoxChannels.Refresh();
        }

        private void ScrollDown()
        {
            int y = pictureBoxChannels.PointToScreen(new Point(pictureBoxChannels.Left, pictureBoxChannels.Bottom - 10)).Y;
            while (MousePosition.Y > y)
            {
                if (vScrollBar.Value > (vScrollBar.Maximum - vScrollBar.LargeChange))
                {
                    break;
                }
                _insertIndex++;
                vScrollBar.Value++;
            }
        }

        private void ScrollUp()
        {
            int y = pictureBoxChannels.PointToScreen(new Point(pictureBoxChannels.Left, pictureBoxChannels.Top + 10)).Y;
            while (MousePosition.Y < y)
            {
                if (vScrollBar.Value == 0)
                {
                    break;
                }
                _insertIndex--;
                vScrollBar.Value--;
                pictureBoxChannels.Refresh();
            }
        }

        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            pictureBoxChannels.Refresh();
        }
    }
}