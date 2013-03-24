namespace MIDIReader
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Vixen;
    using Vixen.Dialogs;

    internal partial class MIDIReaderDialog : Form
    {
        private const int CHANNEL_OFFSET = 350;
        private const int CONNECTION_ENDPOINT_SIZE = 7;
        private const int CONNECTION_ENDPOINT_VERTICAL_OFFSET = 10;
        private const int KEYBOARD_OFFSET = 150;
        private int m_blackKeyHalfWidth;
        private Size m_blackKeySize;
        private Rectangle m_channelBounds;
        private int m_channelBoxWidth;
        private Dictionary<int, List<Key>> m_channelKeyConnections;
        private int m_channelsShown;
        private string m_channelText = string.Empty;
        private object m_dragStartObject = null;
        private bool[] m_enabledKeyMap;
        private Rectangle m_keyboardBounds;
        private Dictionary<Key, List<int>> m_keyChannelConnections;
        private Dictionary<int, Key> m_keyLookup;
        private int m_lastChannel = -1;
        private Key m_lastKey = null;
        private Point m_lastMouseLocation;
        private int m_leftArrowChannel = -1;
        private bool m_leftArrowEnabled;
        private Point[] m_leftArrowPoints;
        private int m_maxChannelsShown;
        private MIDIFile m_midiFile;
        private Point m_mouseDownAt = Point.Empty;
        private Point m_mouseLastAt = Point.Empty;
        private string m_noteText = string.Empty;
        private SolidBrush m_noteTextBrush = new SolidBrush(Color.Black);
        private List<Key>[] m_octaveKeys;
        private bool m_resizing = false;
        private int m_rightArrowChannel;
        private bool m_rightArrowEnabled;
        private Point[] m_rightArrowPoints;
        private EventSequence m_sequence;
        private int m_startChannel;
        private Font m_textFont = new Font("Arial", 14f);
        private Size m_whiteKeySize;
        public const int MIDI_START_NOTE_ID = 0x15;

        public MIDIReaderDialog(EventSequence sequence, bool[] enabledKeyMap, MIDIFile midiFile)
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            this.m_midiFile = midiFile;
            this.m_sequence = sequence;
            this.m_enabledKeyMap = enabledKeyMap;
            this.m_whiteKeySize = new Size(15, 90);
            this.m_blackKeySize = new Size(9, 0x38);
            this.m_blackKeyHalfWidth = this.m_blackKeySize.Width / 2;
            this.m_channelBoxWidth = 12;
            this.m_rightArrowChannel = sequence.ChannelCount;
            this.m_lastMouseLocation = new Point(0, 0);
            this.m_octaveKeys = new List<Key>[9];
            for (int i = 0; i < this.m_octaveKeys.Length; i++)
            {
                this.m_octaveKeys[i] = new List<Key>();
            }
            this.m_keyChannelConnections = new Dictionary<Key, List<int>>();
            this.m_channelKeyConnections = new Dictionary<int, List<Key>>();
            this.m_keyLookup = new Dictionary<int, Key>();
            this.CalcPlacements();
        }

        private void BoxShadow(Graphics g, Rectangle boxBounds, Color shadowColor, Color bgColor)
        {
            int num4;
            int num = 2;
            int num2 = 5;
            int num3 = 2;
            Pen pen = new Pen(shadowColor);
            Rectangle rectangle = new Rectangle(boxBounds.X - 1, boxBounds.Y + num2, (boxBounds.Width - num2) + 1, (boxBounds.Height - num2) + 1);
            for (num4 = 0; num4 < num; num4++)
            {
                g.DrawLine(pen, rectangle.Left, rectangle.Top, rectangle.Left, rectangle.Bottom);
                g.DrawLine(pen, rectangle.Left, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
                rectangle.X--;
                rectangle.Height++;
                rectangle.Width++;
            }
            int num5 = (shadowColor.R - bgColor.R) / (num3 + 1);
            int num6 = (shadowColor.G - bgColor.G) / (num3 + 1);
            int num7 = (shadowColor.B - bgColor.B) / (num3 + 1);
            for (num4 = 0; num4 < num3; num4++)
            {
                pen.Color = Color.FromArgb(pen.Color.R - num5, pen.Color.G - num6, pen.Color.B - num7);
                g.DrawLine(pen, rectangle.Left, rectangle.Top, rectangle.Left, rectangle.Bottom);
                g.DrawLine(pen, rectangle.Left, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
                rectangle.X--;
                rectangle.Height++;
                rectangle.Width++;
            }
        }

        private void buttonAutoMap_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will remove all current connections and map notes to channels in order.\n\nDo you want to continue?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.m_keyChannelConnections.Clear();
                this.m_channelKeyConnections.Clear();
                int num = Math.Min(0x58, this.m_sequence.ChannelCount);
                int item = 0;
                for (int i = 0; (i < 0x58) && (item < this.m_sequence.ChannelCount); i++)
                {
                    Key key = this.m_keyLookup[i + 0x15];
                    if (key.Enabled)
                    {
                        List<int> list = new List<int>();
                        this.m_keyChannelConnections[key] = list;
                        list.Add(item);
                        List<Key> list2 = new List<Key>();
                        this.m_channelKeyConnections[item] = list2;
                        list2.Add(key);
                        item++;
                    }
                }
                base.Invalidate(false);
                base.Update();
            }
        }

        private void CalcPlacements()
        {
            int width = 0x34 * this.m_whiteKeySize.Width;
            bool flag = this.m_octaveKeys[0].Count == 0;
            this.m_keyboardBounds = new Rectangle((base.ClientRectangle.Width - width) >> 1, 150, width, this.m_whiteKeySize.Height);
            this.m_keyboardBounds.X = Math.Max(this.m_keyboardBounds.X, 5);
            this.m_maxChannelsShown = (base.ClientRectangle.Width / this.m_channelBoxWidth) - 2;
            this.m_leftArrowEnabled = false;
            if (this.m_sequence.ChannelCount > this.m_maxChannelsShown)
            {
                this.m_channelsShown = this.m_maxChannelsShown;
                this.m_rightArrowEnabled = true;
            }
            else
            {
                this.m_channelsShown = this.m_sequence.ChannelCount;
                this.m_rightArrowEnabled = false;
            }
            this.m_startChannel = 0;
            this.m_leftArrowChannel = this.m_startChannel - 1;
            this.m_rightArrowChannel = this.m_startChannel + this.m_channelsShown;
            int num3 = (this.m_channelsShown + 2) * this.m_channelBoxWidth;
            this.m_channelBounds = new Rectangle((base.ClientRectangle.Width - num3) >> 1, 350, num3, this.m_channelBoxWidth);
            this.m_channelBounds.X = Math.Max(this.m_channelBounds.X, 2);
            Point[] pointArray = new Point[] { new Point(this.m_channelBounds.Left + 3, this.m_channelBounds.Top + 6), new Point(this.m_channelBounds.Left + 9, this.m_channelBounds.Top + 3), new Point(this.m_channelBounds.Left + 9, this.m_channelBounds.Top + 9) };
            this.m_leftArrowPoints = pointArray;
            pointArray = new Point[] { new Point(this.m_channelBounds.Right - 3, this.m_channelBounds.Top + 6), new Point(this.m_channelBounds.Right - 9, this.m_channelBounds.Top + 3), new Point(this.m_channelBounds.Right - 9, this.m_channelBounds.Top + 9) };
            this.m_rightArrowPoints = pointArray;
            int x = (this.m_keyboardBounds.X + this.m_whiteKeySize.Width) - this.m_blackKeyHalfWidth;
            char ch = 'A';
            int index = 0;
            int num6 = 0x15;
            int num7 = this.m_keyboardBounds.X + 1;
            for (int i = 0; i < 0x34; i++)
            {
                Key key;
                if (flag)
                {
                    key = new Key();
                    key.Id = num6;
                    key.Enabled = this.m_enabledKeyMap[key.Id - 0x15];
                    key.Note = ch;
                    key.Pitch = Pitch.Natural;
                    key.Octave = index;
                    this.m_octaveKeys[index].Add(key);
                    this.m_keyLookup[key.Id] = key;
                }
                else
                {
                    key = this.m_keyLookup[num6];
                }
                num6++;
                key.Region = new Rectangle(num7, this.m_keyboardBounds.Y + 1, this.m_whiteKeySize.Width - 1, this.m_whiteKeySize.Height - 1);
                if (((ch != 'B') && (ch != 'E')) && ((x + this.m_blackKeySize.Width) < this.m_keyboardBounds.Right))
                {
                    if (flag)
                    {
                        key = new Key();
                        key.Id = num6;
                        key.Enabled = this.m_enabledKeyMap[key.Id - 0x15];
                        key.Note = ch;
                        key.Pitch = Pitch.Sharp;
                        key.Octave = index;
                        this.m_octaveKeys[index].Insert(0, key);
                        this.m_keyLookup[key.Id] = key;
                    }
                    else
                    {
                        key = this.m_keyLookup[num6];
                    }
                    num6++;
                    key.Region = new Rectangle(x, this.m_keyboardBounds.Y + 1, this.m_blackKeySize.Width, this.m_blackKeySize.Height);
                }
                ch = (char) (ch + '\x0001');
                if (ch == 'H')
                {
                    ch = 'A';
                }
                if (ch == 'C')
                {
                    index++;
                }
                x += this.m_whiteKeySize.Width;
                num7 += this.m_whiteKeySize.Width;
            }
            this.Refresh();
        }

        

        

        private void MIDIReaderDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            HelpDialog dialog = new HelpDialog("Indicate how you would like your sequence to respond to the MIDI note events by dragging channels to keys and keys to channels.\n\nConnections can be broken by dragging them off of a key or channel.\n\nConnections can be moved by dragging them.\n\n");
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void MIDIReaderDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.F12) && e.Alt)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    string filePath = Path.Combine(Paths.AddinPath, "mididump.txt");
                    this.m_midiFile.DebugDump(filePath);
                    MessageBox.Show("MIDI data dumped into " + filePath, "MIDIReader", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void MIDIReaderDialog_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.m_dragStartObject = null;
                if (this.m_channelText != string.Empty)
                {
                    this.m_dragStartObject = this.m_lastChannel;
                }
                else if (this.m_noteText != string.Empty)
                {
                    if (this.m_lastKey.Enabled)
                    {
                        this.m_dragStartObject = this.m_lastKey;
                    }
                }
                else if ((this.m_lastChannel == this.m_leftArrowChannel) && this.m_leftArrowEnabled)
                {
                    this.m_startChannel = Math.Max(0, this.m_startChannel - this.m_channelsShown);
                    this.m_leftArrowChannel = this.m_startChannel - 1;
                    this.m_rightArrowChannel = this.m_startChannel + this.m_channelsShown;
                    this.m_leftArrowEnabled = this.m_startChannel > 0;
                    this.m_rightArrowEnabled = this.m_sequence.ChannelCount > (this.m_maxChannelsShown + this.m_startChannel);
                    base.Invalidate(false);
                    base.Update();
                }
                else if ((this.m_lastChannel == this.m_rightArrowChannel) && this.m_rightArrowEnabled)
                {
                    this.m_startChannel = Math.Min((int) (this.m_sequence.ChannelCount - this.m_channelsShown), (int) (this.m_startChannel + this.m_channelsShown));
                    this.m_leftArrowChannel = this.m_startChannel - 1;
                    this.m_rightArrowChannel = this.m_startChannel + this.m_channelsShown;
                    this.m_leftArrowEnabled = this.m_startChannel > 0;
                    this.m_rightArrowEnabled = this.m_sequence.ChannelCount > (this.m_maxChannelsShown + this.m_startChannel);
                    base.Invalidate(false);
                    base.Update();
                }
                if (this.m_dragStartObject != null)
                {
                    this.m_mouseLastAt = this.m_mouseDownAt = e.Location;
                }
                else
                {
                    this.m_mouseLastAt = this.m_mouseDownAt = Point.Empty;
                }
            }
        }

        private void MIDIReaderDialog_MouseMove(object sender, MouseEventArgs e)
        {
            int num;
            if (this.m_mouseDownAt != Point.Empty)
            {
                this.m_mouseLastAt = e.Location;
            }
            if (!this.m_keyboardBounds.Contains(e.Location))
            {
                this.SetLastKey(null);
            }
            if (!this.m_channelBounds.Contains(e.Location))
            {
                this.SetLastChannel(-1);
            }
            if (this.m_keyboardBounds.Contains(e.Location))
            {
                num = ((e.X - this.m_keyboardBounds.X) / this.m_whiteKeySize.Width) - 2;
                List<Key> list = this.m_octaveKeys[(num < 0) ? 0 : ((num / 7) + 1)];
                foreach (Key key in list)
                {
                    if (key.Region.Contains(e.Location))
                    {
                        this.SetLastKey(key);
                        break;
                    }
                }
            }
            else if (this.m_channelBounds.Contains(e.Location))
            {
                num = ((e.X - this.m_channelBounds.X) / this.m_channelBoxWidth) - 1;
                this.SetLastChannel(this.m_startChannel + num);
            }
            base.Invalidate(false);
            base.Update();
        }

        private void MIDIReaderDialog_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (this.m_mouseDownAt != Point.Empty))
            {
                List<int> list;
                List<Key> list2;
                this.m_mouseDownAt = Point.Empty;
                object lastChannel = null;
                if (this.m_channelText != string.Empty)
                {
                    lastChannel = this.m_lastChannel;
                }
                else if ((this.m_noteText != string.Empty) && this.m_lastKey.Enabled)
                {
                    lastChannel = this.m_lastKey;
                }
                if (lastChannel != null)
                {
                    int current;
                    Key dragStartObject;
                    if (this.m_dragStartObject.GetType() == lastChannel.GetType())
                    {
                        if (this.m_dragStartObject is Key)
                        {
                            if (this.m_keyChannelConnections.TryGetValue((Key) this.m_dragStartObject, out list) && (list.Count > 0))
                            {
                                this.m_keyChannelConnections[(Key) lastChannel] = list;
                                this.m_keyChannelConnections.Remove((Key) this.m_dragStartObject);
                                using (List<int>.Enumerator enumerator = list.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        current = enumerator.Current;
                                        list2 = this.m_channelKeyConnections[current];
                                        list2.Remove((Key) this.m_dragStartObject);
                                        list2.Add((Key) lastChannel);
                                    }
                                }
                            }
                        }
                        else if (this.m_channelKeyConnections.TryGetValue((int) this.m_dragStartObject, out list2) && (list2.Count > 0))
                        {
                            this.m_channelKeyConnections[(int) lastChannel] = list2;
                            this.m_channelKeyConnections.Remove((int) this.m_dragStartObject);
                            using (List<Key>.Enumerator enumerator2 = list2.GetEnumerator())
                            {
                                while (enumerator2.MoveNext())
                                {
                                    dragStartObject = enumerator2.Current;
                                    list = this.m_keyChannelConnections[dragStartObject];
                                    list.Remove((int) this.m_dragStartObject);
                                    list.Add((int) lastChannel);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.m_dragStartObject is Key)
                        {
                            dragStartObject = (Key) this.m_dragStartObject;
                            current = (int) lastChannel;
                        }
                        else
                        {
                            dragStartObject = (Key) lastChannel;
                            current = (int) this.m_dragStartObject;
                        }
                        if (!this.m_keyChannelConnections.TryGetValue(dragStartObject, out list))
                        {
                            list = new List<int>();
                            this.m_keyChannelConnections[dragStartObject] = list;
                        }
                        list.Add(current);
                        if (!this.m_channelKeyConnections.TryGetValue(current, out list2))
                        {
                            list2 = new List<Key>();
                            this.m_channelKeyConnections[current] = list2;
                        }
                        list2.Add(dragStartObject);
                        base.Invalidate(false);
                        base.Update();
                    }
                }
                else
                {
                    if (this.m_dragStartObject is Key)
                    {
                        if (this.m_keyChannelConnections.TryGetValue((Key) this.m_dragStartObject, out list))
                        {
                            foreach (int num in list)
                            {
                                if (this.m_channelKeyConnections.TryGetValue(num, out list2))
                                {
                                    list2.Remove((Key) this.m_dragStartObject);
                                }
                            }
                            list.Clear();
                        }
                    }
                    else if (this.m_channelKeyConnections.TryGetValue((int) this.m_dragStartObject, out list2))
                    {
                        foreach (Key key in list2)
                        {
                            if (this.m_keyChannelConnections.TryGetValue(key, out list))
                            {
                                list.Remove((int) this.m_dragStartObject);
                            }
                        }
                        list2.Clear();
                    }
                    base.Invalidate(false);
                    base.Update();
                }
            }
        }

        private void MIDIReaderDialog_Resize(object sender, EventArgs e)
        {
            if (!this.m_resizing)
            {
                this.CalcPlacements();
            }
        }

        private void MIDIReaderDialog_ResizeBegin(object sender, EventArgs e)
        {
            this.m_resizing = true;
        }

        private void MIDIReaderDialog_ResizeEnd(object sender, EventArgs e)
        {
            this.m_resizing = false;
            this.CalcPlacements();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.m_resizing)
            {
                int num;
                e.Graphics.FillRectangle(Brushes.White, base.Bounds);
                this.BoxShadow(e.Graphics, this.m_keyboardBounds, Color.FromArgb(0x80, 0x80, 0x80), Color.White);
                e.Graphics.DrawRectangle(Pens.Black, this.m_keyboardBounds);
                int num2 = this.m_keyboardBounds.X + this.m_whiteKeySize.Width;
                for (num = 1; num < 0x34; num++)
                {
                    e.Graphics.DrawLine(Pens.Black, num2, this.m_keyboardBounds.Top, num2, this.m_keyboardBounds.Bottom);
                    num2 += this.m_whiteKeySize.Width;
                }
                SolidBrush brush = new SolidBrush(Color.White);
                foreach (List<Key> list in this.m_octaveKeys)
                {
                    brush.Color = Color.FromArgb(0xff, 230, 230);
                    foreach (Key key in list)
                    {
                        if ((key.Pitch == Pitch.Natural) && key.Enabled)
                        {
                            e.Graphics.FillRectangle(brush, key.Region);
                        }
                    }
                    brush.Color = Color.FromArgb(100, 0, 0);
                    foreach (Key key in list)
                    {
                        if (key.Pitch != Pitch.Natural)
                        {
                            e.Graphics.FillRectangle(key.Enabled ? brush : Brushes.Black, key.Region);
                        }
                    }
                }
                e.Graphics.DrawRectangle(Pens.Black, this.m_channelBounds);
                num2 = this.m_channelBounds.X + this.m_channelBoxWidth;
                List<Channel> channels = this.m_sequence.Channels;
                for (num = 0; num < this.m_channelsShown; num++)
                {
                    e.Graphics.FillRectangle(channels[num + this.m_startChannel].Brush, (int) (num2 + 1), (int) (this.m_channelBounds.Top + 1), (int) (this.m_channelBoxWidth - 1), (int) (this.m_channelBoxWidth - 1));
                    e.Graphics.DrawLine(Pens.Black, num2, this.m_channelBounds.Top, num2, this.m_channelBounds.Bottom);
                    num2 += this.m_channelBoxWidth;
                }
                e.Graphics.DrawLine(Pens.Black, num2, this.m_channelBounds.Top, num2, this.m_channelBounds.Bottom);
                e.Graphics.DrawString(this.m_noteText, this.m_textFont, this.m_noteTextBrush, (float) this.m_keyboardBounds.X, (float) (this.m_keyboardBounds.Y - 0x2d));
                e.Graphics.DrawString(this.m_channelText, this.m_textFont, Brushes.Black, (float) this.m_channelBounds.X, (float) (this.m_channelBounds.Bottom + 15));
                e.Graphics.FillPolygon(this.m_leftArrowEnabled ? Brushes.Black : Brushes.LightGray, this.m_leftArrowPoints);
                e.Graphics.FillPolygon(this.m_rightArrowEnabled ? Brushes.Black : Brushes.LightGray, this.m_rightArrowPoints);
                int num3 = (this.m_whiteKeySize.Width - 7) / 2;
                int num4 = (this.m_blackKeySize.Width - 7) / 2;
                int num6 = (this.m_channelBoxWidth - 7) / 2;
                foreach (Key key in this.m_keyChannelConnections.Keys)
                {
                    foreach (int num7 in this.m_keyChannelConnections[key])
                    {
                        int num5 = (key.Pitch == Pitch.Natural) ? num3 : num4;
                        e.Graphics.FillEllipse(Brushes.Black, key.Region.Left + num5, key.Region.Bottom - 10, 7, 7);
                        e.Graphics.FillEllipse(Brushes.Black, (this.m_channelBounds.Left + (((num7 + 1) - this.m_startChannel) * this.m_channelBoxWidth)) + num6, this.m_channelBounds.Top + num6, 7, 7);
                        e.Graphics.DrawLine(Pens.Black, (int) (key.Region.Left + (key.Region.Width / 2)), (int) ((key.Region.Bottom - 10) + 3), (int) ((this.m_channelBounds.Left + (((num7 + 1) - this.m_startChannel) * this.m_channelBoxWidth)) + (this.m_channelBoxWidth / 2)), (int) ((this.m_channelBounds.Top + num6) + 3));
                    }
                }
                if (this.m_mouseDownAt != Point.Empty)
                {
                    e.Graphics.DrawLine(Pens.Black, this.m_mouseDownAt, this.m_mouseLastAt);
                }
            }
        }

        private void SetLastChannel(int index)
        {
            if (index == -1)
            {
                this.m_channelText = string.Empty;
                this.m_lastChannel = -1;
            }
            else if ((index == this.m_leftArrowChannel) || (index == this.m_rightArrowChannel))
            {
                this.m_lastChannel = index;
                this.m_channelText = string.Empty;
            }
            else if (this.m_lastChannel != index)
            {
                this.m_lastChannel = index;
                this.m_channelText = this.m_sequence.Channels[index].Name;
            }
        }

        private void SetLastKey(Key key)
        {
            if (key == null)
            {
                this.m_noteText = string.Empty;
                this.m_lastKey = null;
            }
            else if ((this.m_lastKey == null) || (key.Signature != this.m_lastKey.Signature))
            {
                string str;
                this.m_lastKey = key;
                if (((key.Note == 'C') && (key.Octave == 4)) && (key.Pitch == Pitch.Natural))
                {
                    str = key.ToString() + " (Middle C)";
                }
                else
                {
                    str = key.ToString();
                }
                if (!key.Enabled)
                {
                    this.m_noteTextBrush.Color = Color.LightGray;
                }
                else
                {
                    this.m_noteTextBrush.Color = Color.Black;
                }
                this.m_noteText = str;
            }
        }

        public Dictionary<Key, List<int>> KeyChannelMapping
        {
            get
            {
                return this.m_keyChannelConnections;
            }
        }

        public KeyChannelMapping[] Mappings
        {
            get
            {
                List<KeyChannelMapping> list = new List<KeyChannelMapping>();
                foreach (Key key in this.m_keyChannelConnections.Keys)
                {
                    KeyChannelMapping item = new KeyChannelMapping(key.Id);
                    list.Add(item);
                    item.ChannelList.AddRange(this.m_keyChannelConnections[key]);
                }
                return list.ToArray();
            }
            set
            {
                foreach (KeyChannelMapping mapping in value)
                {
                    Key item = this.m_keyLookup[mapping.KeyID];
                    List<int> list = this.m_keyChannelConnections[item] = new List<int>();
                    list.AddRange(mapping.ChannelList);
                    foreach (int num in list)
                    {
                        List<Key> list2;
                        if (!this.m_channelKeyConnections.TryGetValue(num, out list2))
                        {
                            list2 = new List<Key>();
                            this.m_channelKeyConnections[num] = list2;
                        }
                        list2.Add(item);
                    }
                }
            }
        }
    }
}

