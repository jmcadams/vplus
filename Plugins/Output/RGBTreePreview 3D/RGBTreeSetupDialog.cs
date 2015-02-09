using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using VixenPlus;

public partial class RGBTreeSetupDialog : Form {
    private SolidBrush _mBrush;
    private int[] _mCoord = new int[2];
    private int _mCounter;
    private XmlNode _mNode;
    private List<List<int[]>> _mStarburstCoordinates;
    private bool _oneByone;
    private List<int[]> _redrawList;
    private RadioButton _selectedType = new RadioButton();
    private readonly XmlNode _mSetupNode;
    private readonly int _mStartChannel;
    private readonly int _mTotalChannels;


    public RGBTreeSetupDialog(XmlNode setupNode) {
        _mSetupNode = setupNode;
        if (setupNode.Attributes != null) {
            _mStartChannel = Convert.ToInt16(setupNode.Attributes["from"].Value);
            int mEndChannel = Convert.ToInt16(setupNode.Attributes["to"].Value);

            if (_mStartChannel >= mEndChannel) {
                throw new Exception("End channel smaller than Start channel");
            }
            if (_mStartChannel == 0 || mEndChannel == 0) {
                throw new Exception("Value cannot be 0");
            }

            _mTotalChannels = mEndChannel - _mStartChannel + 1;
        }

        _mStartChannel--;

        InitializeComponent();

        WindowState = FormWindowState.Maximized;
        Location = new Point(0, 0);

        track_posX.Maximum = Size.Width;
        track_posY.Maximum = Size.Height;

        tb_Available.Text = _mTotalChannels.ToString();

        group_Type.Controls.Add(rb_bottom_top);
        group_Type.Controls.Add(rb_updown_next);
        group_Type.Controls.Add(rb_updown_180);

        tb_Height.Text = track_Height.Value.ToString();
        tb_Ellipse_X.Text = track_Ellipse_X.Value.ToString();
        tb_Ellipse_Y.Text = track_Ellipse_Y.Value.ToString();

        foreach (var radioButton in group_Type.Controls.OfType<RadioButton>()) {
            radioButton.CheckedChanged += radioButton_CheckedChanged;
        }

        if (setupNode.SelectSingleNode("Settings") != null) {
            _mNode = setupNode.SelectSingleNode("Settings/PixelSize");
            if (_mNode != null) {
                track_PixelSize.Value = Convert.ToInt16(_mNode.InnerText);
                tb_pixelSize.Text = _mNode.InnerText;
            }

            _mNode = setupNode.SelectSingleNode("Settings/Type");
            string selType;
            if (_mNode != null) {
                selType = (_mNode.InnerText);
                _selectedType.Tag = selType;
            }
            else {
                selType = "rb_bottom_top";
                _selectedType.Tag = selType;
            }

            _mNode = setupNode.SelectSingleNode("Settings/PosX");
            if (_mNode != null) {
                track_posX.Maximum = Convert.ToInt16(_mNode.InnerText) > Width ? Convert.ToInt16(_mNode.InnerText) : Width;

                track_posX.Value = Convert.ToInt16(_mNode.InnerText);
                tb_posX.Text = _mNode.InnerText;
            }
            else {
                track_posX.Maximum = Size.Width;
                track_posX.Value = Convert.ToInt16((Convert.ToDouble(Size.Width) - Convert.ToDouble(controlPanel.Size.Width)) / 2);
                tb_posX.Text = track_posX.Value.ToString();
            }


            _mNode = setupNode.SelectSingleNode("Settings/PosY");
            if (_mNode != null) {
                track_posY.Maximum = Convert.ToInt16(_mNode.InnerText) > Height ? Convert.ToInt16(_mNode.InnerText) : Height;

                track_posY.Value = Convert.ToInt16(_mNode.InnerText);
                tb_posY.Text = _mNode.InnerText;
            }
            else {
                track_posY.Maximum = Size.Height;
                track_posY.Value = Convert.ToInt16((Size.Height) - 100);
                tb_posY.Text = track_posY.Value.ToString();
            }

            _mNode = setupNode.SelectSingleNode("Settings/Strings");
            if (_mNode != null) {
                tb_noStrings.Text = _mNode.InnerText;
            }

            _mNode = setupNode.SelectSingleNode("Settings/Pixels");
            if (_mNode != null) {
                tb_noPixels.Text = _mNode.InnerText;
            }

            _mNode = setupNode.SelectSingleNode("Settings/EllipseX");
            if (_mNode != null) {
                track_Ellipse_X.Value = Convert.ToInt16(_mNode.InnerText);
                tb_Ellipse_X.Text = track_Ellipse_X.Value.ToString();
            }

            _mNode = setupNode.SelectSingleNode("Settings/EllipseY");
            if (_mNode != null) {
                track_Ellipse_Y.Value = Convert.ToInt16(_mNode.InnerText);
                tb_Ellipse_Y.Text = track_Ellipse_Y.Value.ToString();
            }

            _mNode = setupNode.SelectSingleNode("Settings/EllipseHeight");
            if (_mNode != null) {
                track_Height.Value = Convert.ToInt16(_mNode.InnerText);
                tb_Height.Text = track_Height.Value.ToString();
            }

            _mNode = setupNode.SelectSingleNode("Settings/StartPosition");
            if (_mNode != null) {
                comboClock.SelectedItem = _mNode.InnerText;
            }
            else {
                comboClock.SelectedIndex = 3;
            }

            _mNode = setupNode.SelectSingleNode("Settings/Direction");
            if (_mNode != null) {
                comboDirection.SelectedItem = _mNode.InnerText;
            }
            else {
                comboDirection.SelectedIndex = 0;
            }


            if (IsValidValue(tb_noPixels) && IsValidValue(tb_noStrings)) {
                _mStarburstCoordinates = Tree3DCalculate();
                _oneByone = false;
                pictureBox.Refresh();
            }
        }


        foreach (var radioButton in group_Type.Controls.OfType<RadioButton>()) {
            radioButton.Checked = radioButton.Tag.ToString() == _selectedType.Tag.ToString();
        }


        if (IsValidValue(tb_noPixels) && IsValidValue(tb_noStrings)) {
            _mStarburstCoordinates = Tree3DCalculate();
            _oneByone = false;
            pictureBox.Refresh();
        }
    }


    private void btn_setupOK_Click(object sender, EventArgs e) {
        if (IsValidValue(tb_noPixels) && IsValidValue(tb_noStrings)) {
            _mStarburstCoordinates = Tree3DCalculate();
            var contextNode = _mSetupNode.SelectSingleNode("Settings");
            if (contextNode != null) {
                contextNode.RemoveAll();

                _mNode = Xml.SetNewValue(contextNode, "Strings", tb_noStrings.Text);
                _mNode = Xml.SetNewValue(contextNode, "Pixels", tb_noPixels.Text);
                _mNode = Xml.SetNewValue(contextNode, "PixelSize", tb_pixelSize.Text);
                _mNode = Xml.SetNewValue(contextNode, "PosX", tb_posX.Text);
                _mNode = Xml.SetNewValue(contextNode, "PosY", tb_posY.Text);
                _mNode = Xml.SetNewValue(contextNode, "Type", _selectedType.Tag.ToString());
                _mNode = Xml.SetNewValue(contextNode, "Width", Size.Width.ToString());
                _mNode = Xml.SetNewValue(contextNode, "Height", Size.Height.ToString());
                _mNode = Xml.SetNewValue(contextNode, "EllipseX", tb_Ellipse_X.Text);
                _mNode = Xml.SetNewValue(contextNode, "EllipseY", tb_Ellipse_Y.Text);
                _mNode = Xml.SetNewValue(contextNode, "EllipseHeight", tb_Height.Text);
                _mNode = Xml.SetNewValue(contextNode, "StartPosition", comboClock.SelectedItem.ToString());
                _mNode = Xml.SetNewValue(contextNode, "Direction", comboDirection.SelectedItem.ToString());
            }
            DialogResult = DialogResult.OK;
        }
    }


    private void btn_Create_Click(object sender, EventArgs e) {
        var posX = 0;
        var posY = 0;
        var tempX = 1;
        var tempX2 = 0;
        var tempY = 0;
        var previousString = 1;
        var byteList = new List<byte>();

        _redrawList = new List<int[]>();

        if (IsValidValue(tb_noPixels) && IsValidValue(tb_noStrings)) {
            var numberStrings = int.Parse(tb_noStrings.Text);
            var numberPixels = int.Parse(tb_noPixels.Text);
            var maxY = numberPixels / 2;
            var noOutputs = _mStartChannel;

            tb_Requested.Text = Convert.ToString(numberPixels * numberStrings * 3);

            if (_mTotalChannels >= (numberPixels * numberStrings * 3)) {
                _mStarburstCoordinates = Tree3DCalculate();
                if (_mSetupNode.SelectSingleNode("Channels") == null) {
                    Xml.GetNodeAlways(_mSetupNode, "Channels");
                }

                var contextNode = _mSetupNode.SelectSingleNode("Channels");
                if (contextNode != null) {
                    contextNode.RemoveAll();
                    for (var i = 1; i <= numberStrings; i++) {
                        for (var y = 1; y <= numberPixels; y++) {
                            List<int[]> circleStrings;

                            var counter = y - 1;
                            switch (_selectedType.Tag.ToString()) {
                                case "rb_bottom_top": {
                                    circleStrings = _mStarburstCoordinates[y - 1];
                                    posX = circleStrings[i - 1][0];
                                    posY = circleStrings[i - 1][1];
                                    break;
                                }
                                case "rb_updown_next": {
                                    if (y <= maxY) {
                                        tempY = (y - 1);
                                        circleStrings = _mStarburstCoordinates[Math.Abs(tempY)];
                                    }
                                    else {
                                        circleStrings = _mStarburstCoordinates[Math.Abs(tempY)];
                                        tempY--;
                                    }

                                    if (counter == maxY) {
                                        tempX++;
                                    }

                                    if (previousString < i) {
                                        tempX++;
                                        previousString++;
                                    }
                                    posX = circleStrings[tempX - 1][0];
                                    posY = circleStrings[tempX - 1][1];
                                    break;
                                }
                                case "rb_updown_180": {
                                    if (y <= maxY) {
                                        tempY = (y - 1);
                                        circleStrings = _mStarburstCoordinates[Math.Abs(tempY)];
                                    }
                                    else {
                                        circleStrings = _mStarburstCoordinates[Math.Abs(tempY)];
                                        tempY--;
                                    }

                                    posX = circleStrings[tempX2][0];
                                    posY = circleStrings[tempX2][1];
                                    if (y == maxY) {
                                        tempX2 += numberStrings;
                                    }

                                    if (previousString < i) {
                                        tempX2 = i - 1;
                                        previousString++;

                                        posX = circleStrings[tempX2][0];
                                        posY = circleStrings[tempX2][1];
                                    }
                                    break;
                                }
                            }
                            var num = (uint) (posX << 16 | posY);

                            byteList.Clear();
                            byteList.AddRange(BitConverter.GetBytes(num));

                            var byteArray = byteList.ToArray();
                            var valuesPreview = Convert.ToBase64String(byteArray);

                            //Red
                            _mNode = Xml.SetNewValue(contextNode, "Channel", valuesPreview);
                            Xml.SetAttribute(_mNode, "number", noOutputs.ToString());
                            noOutputs++;

                            //Green
                            _mNode = Xml.SetNewValue(contextNode, "Channel", valuesPreview);
                            Xml.SetAttribute(_mNode, "number", noOutputs.ToString());
                            noOutputs++;

                            //Blue
                            _mNode = Xml.SetNewValue(contextNode, "Channel", valuesPreview);
                            Xml.SetAttribute(_mNode, "number", noOutputs.ToString());
                            noOutputs++;

                            var redrawCoord = new int[2];
                            redrawCoord[0] = posX;
                            redrawCoord[1] = posY;
                            _redrawList.Add(redrawCoord);
                        }
                    }
                }

                //TODO HUH?
                for (_mCounter = 0; _mCounter < _redrawList.Count; _mCounter++) {
                    _oneByone = true;
                    pictureBox.Refresh();
                    Thread.Sleep(2);
                }
            }
            else {
                MessageBox.Show("Not enough available channels for amount of strings/pixels", "Input Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        else {
            MessageBox.Show("Invalid entries", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    private static bool IsValidValue(Control textBox) {
        int dummy;
        return int.TryParse(textBox.Text, out dummy);
    }


    private void pb_previewArea_Paint(object sender, PaintEventArgs e) {
        e.Graphics.FillRectangle(Brushes.Transparent, pictureBox.ClientRectangle);

        if (_mStarburstCoordinates != null && _oneByone == false) {
            e.Graphics.FillRectangle(Brushes.Transparent, pictureBox.ClientRectangle);
            foreach (var mCircle in _mStarburstCoordinates) {
                _mBrush = new SolidBrush(Color.White);
                foreach (var mCoordinates in mCircle) {
                    var x = mCoordinates[0];
                    var y = mCoordinates[1];
                    e.Graphics.FillRectangle(_mBrush, new Rectangle(x, y, track_PixelSize.Value, track_PixelSize.Value));
                }
            }
        }

        else if (_oneByone) {
            _mBrush = new SolidBrush(Color.Red);

            {
                for (var w = 0; w <= _mCounter; w++) {
                    if (w < _redrawList.Count) {
                        _mCoord = _redrawList[w];
                        e.Graphics.FillRectangle(_mBrush, new Rectangle(_mCoord[0], _mCoord[1], track_PixelSize.Value, track_PixelSize.Value));
                    }
                }
            }
            _oneByone = false;
        }
    }


    private void track_PixelSize_Scroll(object sender, EventArgs e) {
        tb_pixelSize.Text = track_PixelSize.Value.ToString();
        _oneByone = false;
        pictureBox.Refresh();
    }


    private void track_posX_Scroll(object sender, EventArgs e) {
        tb_posX.Text = track_posX.Value.ToString();
        if (IsValidValue(tb_noPixels) && IsValidValue(tb_noStrings)) {
            _mStarburstCoordinates = Tree3DCalculate();
            _oneByone = false;
            pictureBox.Refresh();
        }


        pictureBox.Refresh();
    }


    private void track_posY_Scroll(object sender, EventArgs e) {
        tb_posY.Text = track_posY.Value.ToString();

        if (IsValidValue(tb_noPixels) && IsValidValue(tb_noStrings)) {
            _mStarburstCoordinates = Tree3DCalculate();
            _oneByone = false;
            pictureBox.Refresh();
        }

        pictureBox.Refresh();
    }


    private void form_Setup_Resize(object sender, EventArgs e) {
        track_posX.Maximum = Size.Width;
        if (track_posX.Value > Size.Width) {
            track_posX.Value = Convert.ToInt16((Convert.ToDouble(Size.Width) - Convert.ToDouble(controlPanel.Size.Width)) / 2);
        }

        track_posY.Maximum = Size.Height;
        if (track_posY.Value > Size.Height) {
            track_posY.Value = Convert.ToInt16(Convert.ToDouble(Size.Height));
        }
    }


    private void radioButton_CheckedChanged(object sender, EventArgs e) {
        var radioButton = sender as RadioButton;
        if (radioButton == null) {
            return;
        }

        if (radioButton.Checked) {
            _selectedType = radioButton;
            if (!IsValidValue(tb_noPixels) || !IsValidValue(tb_noStrings)) {
                return;
            }
            _mStarburstCoordinates = Tree3DCalculate();
            _oneByone = false;
            pictureBox.Refresh();
        }
        else if (_selectedType == radioButton) {
            _selectedType = null;
        }
    }


    private void tb_noStrings_TextChanged(object sender, EventArgs e) {
        if (IsValidValue(tb_noPixels) && IsValidValue(tb_noStrings)) {
            _mStarburstCoordinates = Tree3DCalculate();
            tb_Requested.Text = Convert.ToString(int.Parse(tb_noPixels.Text) * int.Parse(tb_noStrings.Text) * 3);
            _oneByone = false;
            pictureBox.Refresh();
        }
    }


    private void tb_noPixels_TextChanged(object sender, EventArgs e) {
        if (IsValidValue(tb_noPixels) && IsValidValue(tb_noStrings)) {
            _mStarburstCoordinates = Tree3DCalculate();
            tb_Requested.Text = Convert.ToString(int.Parse(tb_noPixels.Text) * int.Parse(tb_noStrings.Text) * 3);
            _oneByone = false;
            pictureBox.Refresh();
        }
    }


    private void track_Height_Scroll(object sender, EventArgs e) {
        tb_Height.Text = track_Height.Value.ToString();
        _mStarburstCoordinates = Tree3DCalculate();
        _oneByone = false;
        pictureBox.Refresh();
    }


    private void track_Ellipse_X_Scroll(object sender, EventArgs e) {
        tb_Ellipse_X.Text = track_Ellipse_X.Value.ToString();
        _mStarburstCoordinates = Tree3DCalculate();
        _oneByone = false;
        pictureBox.Refresh();
    }


    private void track_Ellipse_Y_Scroll(object sender, EventArgs e) {
        tb_Ellipse_Y.Text = track_Ellipse_Y.Value.ToString();
        _mStarburstCoordinates = Tree3DCalculate();
        _oneByone = false;
        pictureBox.Refresh();
    }


    private List<List<int[]>> Tree3DCalculate() {
        var circleNumber = new List<List<int[]>>();

        var noPixels = 1;
        int noStrings;
        var maxPixels = 1;
        var maxStrings = 1;
        var startPosition = Math.PI;
        var direction = -1;

        var divisorValue = noPixels * 2;
        circleNumber.Clear();

        int.TryParse(tb_noPixels.Text, out noPixels);
        int.TryParse(tb_noStrings.Text, out noStrings);

        switch (_selectedType.Tag.ToString()) {
            case "rb_bottom_top": {
                maxPixels = noPixels;
                maxStrings = noStrings;
                divisorValue = 2;
                break;
            }

            case "rb_updown_next": {
                maxPixels = noPixels / 2;
                maxStrings = noStrings * 2;
                divisorValue = 4;
                break;
            }

            case "rb_updown_180": {
                maxPixels = noPixels / 2;
                maxStrings = noStrings * 2;
                divisorValue = 4;
                break;
            }
        }

        switch (comboDirection.SelectedIndex) {
            case 0: {
                direction = -1;
                break;
            }
            case 1: {
                direction = 1;
                break;
            }
        }


        switch (comboClock.SelectedIndex) {
            case 0: {
                startPosition = 3 * Math.PI / 2;
                break;
            }
            case 1: {
                startPosition = 0;
                break;
            }
            case 2: {
                startPosition = Math.PI / 2;
                break;
            }
            case 3: {
                startPosition = Math.PI;
                break;
            }
        }


        for (var j = 1; j <= maxPixels; j++) {
            var h = maxPixels - j + 2;
            var circulos = new List<int[]>();
            for (var i = 0; i < maxStrings; i++) {
                var radius = direction * i * 2 * Math.PI / maxStrings + startPosition;
                var pointX = (int) (((double) (track_Ellipse_X.Value) * h / divisorValue) * Math.Cos(radius));
                var pointY = (int) (((double) (track_Ellipse_Y.Value) * h / divisorValue) * Math.Sin(radius));
                circulos.Add(new[] { pointX + track_posX.Value, pointY + track_posY.Value - j * track_Height.Value });
            }
            circleNumber.Add(circulos);
        }

        return circleNumber;
    }


    private void comboClock_SelectedIndexChanged(object sender, EventArgs e) {
        _mStarburstCoordinates = Tree3DCalculate();
        _oneByone = false;
        pictureBox.Refresh();
    }


    private void comboDirection_SelectedIndexChanged(object sender, EventArgs e) {
        _mStarburstCoordinates = Tree3DCalculate();
        _oneByone = false;
        pictureBox.Refresh();
    }
}