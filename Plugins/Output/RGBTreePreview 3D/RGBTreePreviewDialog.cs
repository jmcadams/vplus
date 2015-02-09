using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using VixenPlus;

public partial class RGBTreePreviewDialog : OutputPlugInUIBase {
    private readonly Dictionary<int, List<uint>> _channelDictionary;

    private byte[] _channelValues;
    private readonly int _drawSize;
    private SolidBrush _brush;
    private const int DrawSizeDefault = 3;


    public RGBTreePreviewDialog(XmlNode setupNode, IList<Channel> channels) {
        InitializeComponent();

        if (null == setupNode || null == setupNode.Attributes || null == setupNode.Attributes["from"] ||
            null == setupNode.SelectNodes("Channels/Channel")) {
            throw new NullReferenceException("setupNode has a null value in a non-null field");
        }

        Width = ParseNode(setupNode.SelectSingleNode("Settings/Width"), Screen.PrimaryScreen.WorkingArea.Width);
        Height = ParseNode(setupNode.SelectSingleNode("Settings/Height"), Screen.PrimaryScreen.WorkingArea.Width);
        _drawSize = ParseNode(setupNode.SelectSingleNode("Settings/PixelSize"), DrawSizeDefault);

        ClientSize = new Size(Width, Height);
        pictureBox.Size = new Size(Width, Height);

        // Checked above stupid resharper.
        // ReSharper disable once PossibleNullReferenceException
        var startChannel = int.Parse(setupNode.Attributes["from"].Value) - 1;
        _channelDictionary = new Dictionary<int, List<uint>>();

        // Checked above stupid resharper.
        // ReSharper disable once PossibleNullReferenceException
        foreach (XmlNode xmlNode in setupNode.SelectNodes("Channels/Channel")) {
            if (xmlNode.Attributes == null) {
                continue;
            }

            var channelNumber = Convert.ToInt32(xmlNode.Attributes["number"].Value);

            if (channelNumber < startChannel) {
                continue;
            }

            var outputChannel = new List<uint>();
            var encodedChannelData = Convert.FromBase64String(xmlNode.InnerText);

            var startIndex = 0;
            while (startIndex < encodedChannelData.Length) {
                outputChannel.Add(BitConverter.ToUInt32(encodedChannelData, startIndex));
                startIndex += 4;
            }
            _channelDictionary[channels[channelNumber - startChannel].OutputChannel] = outputChannel;
        }
    }


    private static int ParseNode(XmlNode node, int nullDefault) {
        return (null == node) ? nullDefault : int.Parse(node.InnerText);
    }


    public void VixenEvent(byte[] channelValues) {
        _channelValues = channelValues;
        pictureBox.Refresh();
    }


    private void pictureBox_Paint(object sender, PaintEventArgs e) {
        if (_channelValues == null) {
            return;
        }

        e.Graphics.FillRectangle(Brushes.Transparent, pictureBox.ClientRectangle);
        for (var i = 0; i < _channelValues.Length; i += 3) {
            _brush = new SolidBrush(Color.FromArgb(_channelValues[i], _channelValues[i + 1], _channelValues[i + 2]));
            foreach (var decodeNum in  _channelDictionary[i]) {
                var x = (int) ((decodeNum) >> 16);
                var y = (int) ((decodeNum) & ushort.MaxValue);
                e.Graphics.FillRectangle(_brush, new Rectangle(x, y, _drawSize, _drawSize));
            }
        }
    }
}