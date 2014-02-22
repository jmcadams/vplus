using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using Common;

using CommonControls;

using VixenPlus.Properties;

namespace VixenPlus.Dialogs {
    internal partial class DimmingCurveDialog : Form {
        private readonly SolidBrush _curveBackBrush;
        private readonly float _curveColPointsPerMiniPixel;
        private readonly Pen _curveGridPen;
        private readonly Pen _curveLinePen;
        private readonly SolidBrush _curvePointBrush;
        private readonly float _curveRowPointsPerMiniPixel;
        private readonly EventSequence _eventSequence;
        private readonly int _gridSpacing;
        private readonly float _halfPointSize;
        private readonly SolidBrush _miniBackBrush;
        private readonly Pen _miniBoxPen;
        private readonly Pen _miniLinePen;
        private readonly Channel _originalChannel;
        private readonly int _pointSize;
        private float _availableValues;
        private int[,] _curvePoints;
        private Rectangle _miniBoxBounds;
        private Point _miniMouseDownLast;
        private Point _miniMouseMaxLocation;
        private Point _miniMouseMinLocation;
        private byte[] _points;
        private int _selectedPointAbsolute;
        private int _selectedPointRelative;
        private int _startCurvePoint;
        private bool _usingActualLevels;


        public DimmingCurveDialog(EventSequence sequence, Channel selectChannel) {
            var miniBoxColor = Color.BlueViolet;
            var miniLineColor = Color.Blue;
            var curveGridColor = Color.LightGray;
            var curveLineColor = Color.Blue;
            var curvePointColor = Color.Black;
            _pointSize = 4;
            const int dotPitch = 4;
            _miniMouseDownLast = new Point(-1, -1);
            _miniMouseMinLocation = new Point(0, 0);
            _miniMouseMaxLocation = new Point(0, 0);
            _selectedPointAbsolute = -1;
            _selectedPointRelative = -1;
            _usingActualLevels = true;
            _availableValues = 256f;
            components = null;
            InitializeComponent();
            Icon = Resources.VixenPlus;
            if (sequence != null) {
                Action<Channel> action = c => comboBoxChannels.Items.Add(c.Clone());
                sequence.Channels.ForEach(action);
                _eventSequence = sequence;
            }
            else {
                labelSequenceChannels.Enabled = false;
                comboBoxChannels.Enabled = false;
                if (selectChannel != null) {
                    _originalChannel = selectChannel;
                    comboBoxChannels.Items.Add(selectChannel = selectChannel.Clone());
                }
            }
            _gridSpacing = _pointSize + dotPitch;
            _halfPointSize = (_pointSize) / 2f;
            _curveRowPointsPerMiniPixel = _availableValues / (pbMini.Width);
            _curveColPointsPerMiniPixel = _availableValues / (pbMini.Height);
            if (pictureBoxCurve != null) {
                _miniBoxBounds = new Rectangle(0, 0, (int) (pictureBoxCurve.Width / (float) _gridSpacing / _availableValues * pbMini.Width),
                                               (int) (pictureBoxCurve.Height / (float) _gridSpacing / _availableValues * pbMini.Height));
            }
            _miniBackBrush = new SolidBrush(pbMini.BackColor);
            if (pictureBoxCurve != null) {
                _curveBackBrush = new SolidBrush(pictureBoxCurve.BackColor);
            }
            _miniBoxPen = new Pen(miniBoxColor);
            _miniLinePen = new Pen(miniLineColor);
            _curveGridPen = new Pen(curveGridColor);
            _curveLinePen = new Pen(curveLineColor);
            _curvePointBrush = new SolidBrush(curvePointColor);
            if (comboBoxChannels.Items.Count > 0) {
                comboBoxChannels.SelectedItem = selectChannel ?? comboBoxChannels.Items[0];
            }
            SwitchDisplay(Preference2.GetInstance().GetBoolean("ActualLevels"));
            comboBoxImport.SelectedIndex = 0;
            comboBoxExport.SelectedIndex = 0;
        }


        private void buttonExportToLibrary_Click(object sender, EventArgs e) {
            if (comboBoxExport.SelectedIndex == 0) {
                using (var library = new CurveLibrary()) {
                    var dialog = new CurveLibraryRecordEditDialog(null);
                    if (dialog.ShowDialog() == DialogResult.OK) {
                        var libraryRecord = dialog.LibraryRecord;
                        libraryRecord.CurveData = _points;
                        library.Import(libraryRecord);
                        library.Save();
                    }
                    dialog.Dispose();
                }
            }
            else {
                var dialog2 = new CurveFileImportExportDialog(CurveFileImportExportDialog.ImportExport.Export);
                dialog2.ShowDialog();
                dialog2.Dispose();
            }
        }


        private void buttonImportFromLibrary_Click(object sender, EventArgs e) {
            if (comboBoxImport.SelectedIndex == 0) {
                using (var dialog = new CurveLibraryDialog()) {
                    if (dialog.ShowDialog() != DialogResult.OK) {
                        return;
                    }
                    if (comboBoxChannels != null) {
                        ((Channel) comboBoxChannels.SelectedItem).DimmingCurve = _points = dialog.SelectedCurve;
                    }
                    RedrawBoth();
                }
            }
            else {
                using (var dialog2 = new CurveFileImportExportDialog(CurveFileImportExportDialog.ImportExport.Import)) {
                    if (dialog2.ShowDialog() != DialogResult.OK) {
                        return;
                    }
                    var selectedCurve = dialog2.SelectedCurve;
                    if (selectedCurve == null) {
                        return;
                    }
                    var channel = comboBoxChannels.SelectedItem as Channel;
                    if (channel != null) {
                        channel.DimmingCurve = _points = selectedCurve.CurveData;
                    }
                    RedrawBoth();
                }
            }
        }


        private void buttonOK_Click(object sender, EventArgs e) {
            if (_eventSequence != null) {
                for (var i = 0; i < comboBoxChannels.Items.Count; i++) {
                    var channel = comboBoxChannels.Items[i] as Channel;
                    if (channel != null) {
                        _eventSequence.Channels[i].DimmingCurve = channel.DimmingCurve;
                    }
                }
            }
            else {
                var channel = (Channel) comboBoxChannels.Items[0];
                _originalChannel.DimmingCurve = channel.DimmingCurve;
            }
        }


        private void buttonResetToLinear_Click(object sender, EventArgs e) {
            if (MessageBox.Show(Resources.VerifyChannelValueReset, Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes) {
                ResetCurrentToLinear();
            }
        }


        private void buttonSwitchDisplay_Click(object sender, EventArgs e) {
            SwitchDisplay(!_usingActualLevels);
        }


        private void comboBoxChannels_SelectedIndexChanged(object sender, EventArgs e) {
            var selectedItem = (Channel) comboBoxChannels.SelectedItem;
            if (selectedItem != null) {
                if (selectedItem.DimmingCurve == null) {
                    selectedItem.DimmingCurve = new byte[256];
                    ResetToLinear(selectedItem.DimmingCurve);
                }
                ShowChannel(selectedItem);
                buttonResetToLinear.Enabled = true;
            }
            else {
                buttonResetToLinear.Enabled = false;
            }
        }


        private void CurveCalc() {
            var num = (int) (_miniBoxBounds.Left * _curveRowPointsPerMiniPixel);
            num = Math.Max(0, num - 1);
            var num2 = (int) (_miniBoxBounds.Right * _curveRowPointsPerMiniPixel);
            num2 = (int) Math.Min(_availableValues - 1f, (num2 + 2));
            var index = Math.Min(num, num2);
            var num4 = Math.Max(num, num2);
            var num5 = pictureBoxCurve.Height - ((pictureBoxCurve.Height / _gridSpacing) * _gridSpacing);
            var num6 = (int) (_availableValues - ((_miniBoxBounds.Bottom + 1) * _curveColPointsPerMiniPixel));
            var num7 = (int) (_miniBoxBounds.Left * _curveRowPointsPerMiniPixel);
            if ((_curvePoints == null) || (_curvePoints.GetLength(0) != ((num4 - index) + 1))) {
                _curvePoints = new int[(num4 - index) + 1,2];
            }
            _startCurvePoint = index;
            var num10 = -1;
            var num11 = -1;
            var num12 = 0;
            while (index < num4) {
                var num8 = (index - num7) * _gridSpacing;
                var num9 = (pictureBoxCurve.Height - num5) - ((_points[index] - num6) * _gridSpacing);
                num10 = ((index + 1) - num7) * _gridSpacing;
                num11 = (pictureBoxCurve.Height - num5) - ((_points[index + 1] - num6) * _gridSpacing);
                _curvePoints[num12, 0] = num8;
                _curvePoints[num12, 1] = num9;
                index++;
                num12++;
            }
            if (num10 == -1) {
                return;
            }
            _curvePoints[num12, 0] = num10;
            _curvePoints[num12, 1] = num11;
        }


        private void DimmingCurveDialog_Paint(object sender, PaintEventArgs e) {
            e.Graphics.TranslateTransform(-label2.Location.X, -label2.Location.Y, MatrixOrder.Append);
            e.Graphics.RotateTransform(-90f, MatrixOrder.Append);
            e.Graphics.TranslateTransform(label2.Location.X, label2.Location.Y, MatrixOrder.Append);
            e.Graphics.DrawString("Output >", Font, Brushes.Black, label2.Location);
            e.Graphics.ResetTransform();
        }


        private void pictureBoxCurve_MouseLeave(object sender, EventArgs e) {
            Cursor = Cursors.Default;
        }


        private void pictureBoxCurve_MouseMove(object sender, MouseEventArgs e) {
            if (_points == null) {
                return;
            }
            if (((e.Button & MouseButtons.Left) != MouseButtons.None) && (_selectedPointRelative != -1)) {
                var num = Math.Max(0, Math.Min(pictureBoxCurve.Height - 1, e.Y));
                if (((_curvePoints[_selectedPointRelative, 1] - num) % _gridSpacing) == 0) {
                    int selectedPointRelative;
                    int num3;
                    var rc = new Rectangle();
                    if (_selectedPointAbsolute == 0) {
                        rc.X = _curvePoints[_selectedPointRelative, 0] - ((int) _halfPointSize);
                        selectedPointRelative = _selectedPointRelative;
                    }
                    else {
                        rc.X = _curvePoints[_selectedPointRelative - 1, 0] - ((int) _halfPointSize);
                        selectedPointRelative = _selectedPointRelative - 1;
                    }
                    if (((float)_selectedPointAbsolute).IsNearlyEqual((_availableValues - 1f))) {
                        rc.Width = (_curvePoints[_selectedPointRelative, 0] + ((int) _halfPointSize)) - rc.X;
                        num3 = _selectedPointRelative;
                    }
                    else {
                        rc.Width = (_curvePoints[_selectedPointRelative + 1, 0] + ((int) _halfPointSize)) - rc.X;
                        num3 = _selectedPointRelative + 1;
                    }
                    rc.Y =
                        (new[] {num, _curvePoints[_selectedPointRelative, 1], _curvePoints[selectedPointRelative, 1], _curvePoints[num3, 1]}).Min() -
                        _pointSize;
                    rc.Height =
                        ((((new[] {num, _curvePoints[_selectedPointRelative, 1], _curvePoints[selectedPointRelative, 1], _curvePoints[num3, 1]}).Max()) +
                          _pointSize) - rc.Y) + _pointSize;
                    _points[_selectedPointAbsolute] =
                        (byte) (_points[_selectedPointAbsolute] + ((byte) ((_curvePoints[_selectedPointRelative, 1] - num) / _gridSpacing)));
                    _curvePoints[_selectedPointRelative, 1] = num;
                    pictureBoxCurve.Invalidate(rc);
                    pbMini.Invalidate(new Rectangle(((int) ((_selectedPointAbsolute) / _curveRowPointsPerMiniPixel)) - 1, 0, 3, pbMini.Height));
                }
            }
            else {
                int num10;
                int num11;
                var length = _curvePoints.GetLength(0);
                var num5 = length >> 1;
                var x = e.X;
                var y = e.Y;
                if (x < (pictureBoxCurve.Width >> 1)) {
                    num10 = 0;
                    num11 = num5;
                }
                else {
                    num10 = num5;
                    num11 = length;
                }
                var num12 = num10;
                while (num12 < num11) {
                    var num8 = _curvePoints[num12, 0];
                    var num9 = _curvePoints[num12, 1];
                    if ((((x >= (num8 - _halfPointSize)) && (x <= (num8 + _halfPointSize))) && (y >= (num9 - _halfPointSize))) &&
                        (y <= (num9 + _halfPointSize))) {
                        break;
                    }
                    num12++;
                }
                if (num12 < num11) {
                    Cursor = Cursors.SizeNS;
                    _selectedPointRelative = num12;
                    _selectedPointAbsolute = _startCurvePoint + num12;
                }
                else {
                    Cursor = Cursors.Default;
                    _selectedPointRelative = -1;
                    _selectedPointAbsolute = -1;
                }
            }
            if (_selectedPointAbsolute == -1) {
                labelChannelValue.Text = string.Empty;
            }
            else {
                labelChannelValue.Text = string.Format("Channel value {0} ({2:P0}) will output at {1} ({3:P0})",
                                                       new object[] {
                                                           _selectedPointAbsolute, _points[_selectedPointAbsolute], (_selectedPointAbsolute) / 255f,
                                                           (_points[_selectedPointAbsolute]) / 255f
                                                       });
            }
        }


        private void pictureBoxCurve_MouseUp(object sender, MouseEventArgs e) {
            _selectedPointAbsolute = -1;
            _selectedPointRelative = -1;
            Cursor = Cursors.Default;
        }


        private void pictureBoxCurve_Paint(object sender, PaintEventArgs e) {
            if (_points == null) {
                return;
            }
            e.Graphics.FillRectangle(_curveBackBrush, e.ClipRectangle);
            var num3 =
                (int)
                Math.Min(((_miniBoxBounds.Left * _curveRowPointsPerMiniPixel) + ((pictureBoxCurve.Width / _gridSpacing) + 1)), (_availableValues - 1f));
            var num4 =
                (int)
                Math.Min(((_miniBoxBounds.Top * _curveColPointsPerMiniPixel) + ((pictureBoxCurve.Height / _gridSpacing) + 1)), (_availableValues - 1f));
            num3 -= (int) (_miniBoxBounds.Left * _curveRowPointsPerMiniPixel);
            num4 -= (int) (_miniBoxBounds.Top * _curveColPointsPerMiniPixel);
            num3 *= _gridSpacing;
            num4 *= _gridSpacing;
            for (var i = (e.ClipRectangle.Top / _gridSpacing) * _gridSpacing; i <= num3; i += _gridSpacing) {
                e.Graphics.DrawLine(_curveGridPen, 0, i, num3, i);
            }
            for (var j = (e.ClipRectangle.Left / _gridSpacing) * _gridSpacing; j <= num4; j += _gridSpacing) {
                e.Graphics.DrawLine(_curveGridPen, j, 0, j, num4);
            }
            var num5 = Math.Max((e.ClipRectangle.Left / _gridSpacing) - 1, 0);
            var num6 = Math.Min((e.ClipRectangle.Right / _gridSpacing) + 3, _curvePoints.GetLength(0));
            for (var k = num5; k < num6; k++) {
                float num7 = _curvePoints[k, 0];
                float num8 = _curvePoints[k, 1];
                if (k < (num6 - 1)) {
                    float num9 = _curvePoints[k + 1, 0];
                    float num10 = _curvePoints[k + 1, 1];
                    e.Graphics.DrawLine(_curveLinePen, num7, num8, num9, num10);
                }
                e.Graphics.FillRectangle(_curvePointBrush, num7 - _halfPointSize, num8 - _halfPointSize, _pointSize, _pointSize);
            }
        }


        private void pictureBoxMini_MouseDown(object sender, MouseEventArgs e) {
            if (_points == null) {
                return;
            }
            if (!_miniBoxBounds.Contains(e.Location)) {
                var miniBoxBounds = _miniBoxBounds;
                _miniBoxBounds.X = Math.Max(0, Math.Min(((pbMini.Width - _miniBoxBounds.Width) - 1), (e.X - (_miniBoxBounds.Width / 2))));
                _miniBoxBounds.Y = Math.Max(0, Math.Min(((pbMini.Height - _miniBoxBounds.Height) - 1), (e.Y - (_miniBoxBounds.Height / 2))));
                RedrawMiniBox(miniBoxBounds);
                RedrawCurve();
            }
            _miniMouseMinLocation.X = e.X - _miniBoxBounds.X;
            _miniMouseMinLocation.Y = e.Y - _miniBoxBounds.Y;
            _miniMouseMaxLocation.X = (pbMini.Width - (_miniBoxBounds.Right - e.X)) - 1;
            _miniMouseMaxLocation.Y = (pbMini.Height - (_miniBoxBounds.Bottom - e.Y)) - 1;
            _miniMouseDownLast = e.Location;
        }


        private void pictureBoxMini_MouseMove(object sender, MouseEventArgs e) {
            if ((_points == null) || ((e.Button & MouseButtons.Left) == MouseButtons.None)) {
                return;
            }
            var num = Math.Max(Math.Min(e.X, _miniMouseMaxLocation.X), _miniMouseMinLocation.X);
            var num2 = Math.Max(Math.Min(e.Y, _miniMouseMaxLocation.Y), _miniMouseMinLocation.Y);
            if ((num == _miniMouseDownLast.X) && (num2 == _miniMouseDownLast.Y)) {
                return;
            }
            var miniBoxBounds = _miniBoxBounds;
            _miniBoxBounds.X += num - _miniMouseDownLast.X;
            _miniMouseDownLast.X = num;
            _miniBoxBounds.Y += num2 - _miniMouseDownLast.Y;
            _miniMouseDownLast.Y = num2;
            RedrawMiniBox(miniBoxBounds);
            RedrawCurve();
        }


        private void pictureBoxMini_Paint(object sender, PaintEventArgs e) {
            if (_points == null) {
                return;
            }
            e.Graphics.FillRectangle(_miniBackBrush, e.ClipRectangle);
            var num = (int) (e.ClipRectangle.Left * _curveRowPointsPerMiniPixel);
            num = Math.Max(0, num - 1);
            var num2 = (int) (e.ClipRectangle.Right * _curveRowPointsPerMiniPixel);
            num2 = (int) Math.Min(_availableValues - 1f, (num2 + 1));
            var num3 = Math.Min(num, num2);
            var num4 = Math.Max(num, num2);
            for (var i = num3; i < num4; i++) {
                e.Graphics.DrawLine(_miniLinePen, ((i) / _curveRowPointsPerMiniPixel), (pbMini.Height - ((_points[i]) / _curveColPointsPerMiniPixel)),
                                    (((i + 1)) / _curveRowPointsPerMiniPixel), (pbMini.Height - ((_points[i + 1]) / _curveColPointsPerMiniPixel)));
            }
            if (e.ClipRectangle.IntersectsWith(_miniBoxBounds)) {
                e.Graphics.DrawRectangle(_miniBoxPen, _miniBoxBounds);
            }
        }


        private void RedrawBoth() {
            CurveCalc();
            pbMini.Refresh();
            pictureBoxCurve.Refresh();
        }


        private void RedrawCurve() {
            CurveCalc();
            pictureBoxCurve.Refresh();
        }


        private void RedrawMiniBox(Rectangle oldBounds) {
            var rc = Rectangle.Union(_miniBoxBounds, oldBounds);
            rc.Inflate(1, 1);
            pbMini.Invalidate(rc);
        }


        private void ResetCurrentToLinear() {
            ResetToLinear(_points);
            CurveCalc();
            pbMini.Refresh();
            pictureBoxCurve.Refresh();
        }


        private static void ResetToLinear(byte[] values) {
            for (var i = 0; i < values.Length; i++) {
                values[i] = (byte) i;
            }
        }


        private void ShowChannel(Channel channel) {
            _points = channel.DimmingCurve;
            RedrawBoth();
        }


        private void SwitchDisplay(bool useActualLevels) {
            _usingActualLevels = useActualLevels;
            if (_usingActualLevels) {
                _availableValues = 256f;
                pbMini.Size = new Size(256, 256);
            }
            else {
                _availableValues = 101f;
                pbMini.Size = new Size(100, 100);
            }
            pbMini.Refresh();
            RedrawCurve();
            buttonSwitchDisplay.Text = useActualLevels ? Resources.ShowPercentage : Resources.ShowActual;
        }
    }
}
