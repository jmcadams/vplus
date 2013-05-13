using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FMOD;
using VixenPlus;
using VixenPlus.Dialogs;
    
namespace Spectrum
{
    //ComponentResourceManager manager = new ComponentResourceManager(typeof(SpectrumDialog));
    //pictureBoxPlay.Image = (Image)manager.GetObject("pictureBoxPlay.Image");
    //pictureBoxPause.Image = (Image)manager.GetObject("pictureBoxPause.Image");
    //pictureBoxStop.Image = (Image)manager.GetObject("pictureBoxStop.Image");
    //pictureBoxScaleUp.Image = (Image)manager.GetObject("pictureBoxScaleUp.Image");
    //pictureBoxScaleDown.Image = (Image)manager.GetObject("pictureBoxScaleDown.Image");
    //buttonAutoMap = new Button();

    internal partial class SpectrumDialog : Form
    {
        private FMOD.Channel _channel;
        private readonly Dictionary<FrequencyBand, List<int>> _bandChannelConnections;
        private readonly Font _bandFont = new Font("Arial", 8f);
        private readonly List<FrequencyBand> _bands;
        private Rectangle _bandsBounds;
        private readonly Dictionary<int, FrequencyBand> _channelBandConnections;
        private Rectangle _channelBounds;
        private readonly int _channelBoxWidth;
        private readonly int _channelsShown;
        private string _channelText = string.Empty;
        private bool _doingMaxSliderDrag;
        private bool _doingMinSliderDrag;
        private object _dragStartObject;
        private FrequencyBand _lastBand;
        private int _lastChannel = -1;
        private int _leftArrowChannel = -1;
        private bool _leftArrowEnabled;
        private readonly Point[] _leftArrowPoints;
        private readonly int _maxChannelsShown;
        private Point _mouseDownAt = Point.Empty;
        private Point _mouseLastAt = Point.Empty;
        private bool _paused;
        private bool _playing;
        private int _rightArrowChannel;
        private bool _rightArrowEnabled;
        private readonly Point[] _rightArrowPoints;
        private float _scaleFactor = 5f;
        private readonly EventSequence _eventSequence;
        private int _startChannel;
        private readonly Font _textFont = new Font("Arial", 14f);
        private bool _mUserSetResponseLevels;
        private Sound _sound;
        private readonly float[] _spectrum = new float[512];
        private FMOD.System _system;
        private Timer _timer;

        public SpectrumDialog(EventSequence sequence)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            _eventSequence = sequence;
            _bands = new List<FrequencyBand>();
            _channelBoxWidth = 12;
            _rightArrowChannel = sequence.ChannelCount;
            _bandChannelConnections = new Dictionary<FrequencyBand, List<int>>();
            _channelBandConnections = new Dictionary<int, FrequencyBand>();
            UpdateScaleLabel();
            var numArray = new[] { 
                20, 25, 31, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 
                800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000, 10000, 12500, 16000, 20000
             };
            foreach (int num in numArray)
            {
                _bands.Add(new FrequencyBand(num));
            }
            ErrCheck(Factory.System_Create(ref _system));
            ErrCheck(_system.init(0x20, INITFLAGS.NORMAL, IntPtr.Zero));
            int width = _bands.Count * 28;
            _bandsBounds = new Rectangle(((ClientRectangle.Width - 40 - width) >> 1) + 40, 50, width, 100);
            _maxChannelsShown = ClientRectangle.Width / _channelBoxWidth - 2;
            _leftArrowEnabled = false;
            if (_eventSequence.ChannelCount > _maxChannelsShown)
            {
                _channelsShown = _maxChannelsShown;
                _rightArrowEnabled = true;
            }
            else
            {
                _channelsShown = _eventSequence.ChannelCount;
                _rightArrowEnabled = false;
            }
            _startChannel = 0;
            _leftArrowChannel = _startChannel - 1;
            _rightArrowChannel = _startChannel + _channelsShown;
            int num4 = (_channelsShown + 2) * _channelBoxWidth;
            _channelBounds = new Rectangle(ClientRectangle.Width - num4 >> 1, 0xe1, num4, _channelBoxWidth);
            _channelBounds.X = Math.Max(_channelBounds.X, 2);
            var pointArray = new[] { new Point(_channelBounds.Left + 3, _channelBounds.Top + 6), new Point(_channelBounds.Left + 9, _channelBounds.Top + 3), new Point(_channelBounds.Left + 9, _channelBounds.Top + 9) };
            _leftArrowPoints = pointArray;
            pointArray = new[] { new Point(_channelBounds.Right - 3, _channelBounds.Top + 6), new Point(_channelBounds.Right - 9, _channelBounds.Top + 3), new Point(_channelBounds.Right - 9, _channelBounds.Top + 9) };
            _rightArrowPoints = pointArray;
            for (int i = 0; i < _bands.Count; i++)
            {
                FrequencyBand band = _bands[i];
                band.Region = new Rectangle((3 + _bandsBounds.Left) + (i * 28), 0x4b, 22, 0x4b);
                band.MinSliderRegion = new Rectangle(band.Region.X, (band.Region.Bottom - (band.Region.Height / 2)) - 4, band.Region.Width, 4);
                band.MaxSliderRegion = new Rectangle(band.Region.X, band.Region.Top, band.Region.Width, 4);
                if (!_mUserSetResponseLevels)
                {
                    band.ResponseLevelMax = 1f;
                    band.ResponseLevelMin = 0.5f;
                }
            }
            RESULT result = _system.createStream(Path.Combine(Paths.AudioPath, _eventSequence.Audio.FileName), MODE.SOFTWARE | MODE._2D, ref _sound);
            ErrCheck(result);
        }

        private void buttonAutoMap_Click(object sender, EventArgs e)
        {
            var dialog = new AutoMapDialog(_eventSequence.Channels, _bands);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int startChannelIndex = dialog.StartChannelIndex;
                int startBandIndex = dialog.StartBandIndex;
                int num3 = Math.Min(_bands.Count - startBandIndex, _eventSequence.Channels.Count - startChannelIndex);
                _bandChannelConnections.Clear();
                _channelBandConnections.Clear();
                for (int i = 0; i < num3; i++)
                {
                    FrequencyBand band = _bands[startBandIndex];
                    var list = new List<int>();
                    _bandChannelConnections[band] = list;
                    list.Add(startChannelIndex);
                    _channelBandConnections[startChannelIndex] = band;
                    startBandIndex++;
                    startChannelIndex++;
                }
                Refresh();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (_channelBandConnections.Count == 0)
            {
                MessageBox.Show(@"There are no mappings created.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                uint length = 0;
                uint position = 0;
                Stop();
                _sound.release();
                _system.close();
                _system.release();
                ErrCheck(Factory.System_Create(ref _system));
                _system.setOutput(OUTPUTTYPE.NOSOUND_NRT);
                ErrCheck(_system.init(0x20, INITFLAGS.STREAM_FROM_UPDATE, IntPtr.Zero));
                _system.createStream(Path.Combine(Paths.AudioPath, _eventSequence.Audio.FileName), MODE.SOFTWARE | MODE._2D, ref _sound);
                _sound.getLength(ref length, TIMEUNIT.MS);
                var dialog = new TranscribeDialog((int) length);
                dialog.Show();
                ErrCheck(_system.playSound(CHANNELINDEX.FREE, _sound, false, ref _channel));
                int num6 = _eventSequence.MaximumLevel - _eventSequence.MinimumLevel;
                while (position < length)
                {
                    _channel.getPosition(ref position, TIMEUNIT.MS);
                    dialog.Progress = (int) position;
                    GetSpectrumData();
                    var num3 = (int) (position / _eventSequence.EventPeriod);
                    if (num3 >= _eventSequence.TotalEventPeriods)
                    {
                        break;
                    }
                    foreach (FrequencyBand band in _bands)
                    {
                        float responseLevelMin = band.ResponseLevelMin;
                        float responseLevelMax = band.ResponseLevelMax;
                        float num7 = responseLevelMax - responseLevelMin;
                        if ((_bandChannelConnections.ContainsKey(band) && (band.CurrentScaledLevel >= responseLevelMin)) && (band.CurrentScaledLevel <= responseLevelMax))
                        {
                            foreach (int num8 in _bandChannelConnections[band])
                            {
                                _eventSequence.EventValues[num8, num3] = (byte) ((((band.CurrentScaledLevel - responseLevelMin) / num7) * num6) + _eventSequence.MinimumLevel);
                            }
                        }
                    }
                    _system.update();
                }
                _channel.stop();
                dialog.Hide();
                dialog.Dispose();
                _sound.release();
                _sound = null;
                _system.close();
                _system.release();
                _system = null;
            }
        }

        

        private void DrawSpectrum(Graphics g)
        {
            int numoutputchannels = 0;
            int samplerate = 0;
            var none = SOUND_FORMAT.NONE;
            var linear = DSP_RESAMPLER.LINEAR;
            int channeloffset;
            const int height = 25;
            const int num6 = 50;
            var b = new Rectangle(3, _bandsBounds.Bottom - height, 22, height);
            var rectangle3 = new Rectangle(3, _bandsBounds.Bottom - num6, 22, height);
            var rectangle4 = new Rectangle(3, _bandsBounds.Bottom - 75, 22, height);
            _system.getSoftwareFormat(ref samplerate, ref none, ref numoutputchannels, ref samplerate, ref linear, ref samplerate);
            for (channeloffset = 0; channeloffset < numoutputchannels; channeloffset++)
            {
                _system.getSpectrum(_spectrum, 0x200, channeloffset, DSP_FFT_WINDOW.TRIANGLE);
                foreach (FrequencyBand band in _bands)
                {
                    int num8;
                    float num7 = 0f;
                    for (int i = band.FmodLowFrequency; i < band.FmodHighFrequency; i++)
                    {
                        num7 = Math.Max(num7, _spectrum[i]);
                    }
                    Rectangle region = band.Region;
                    region.Height = Math.Min((int) ((75f * num7) * _scaleFactor), 0x4b);
                    region.Y = band.Region.Bottom - region.Height;
                    rectangle4.X = num8 = region.X;
                    b.X = rectangle3.X = num8;
                    g.FillRectangle(Brushes.Green, Rectangle.Intersect(region, b));
                    g.FillRectangle(Brushes.Yellow, Rectangle.Intersect(region, rectangle3));
                    g.FillRectangle(Brushes.Red, Rectangle.Intersect(region, rectangle4));
                }
            }
        }

        private void ErrCheck(RESULT result)
        {
            if (result != RESULT.OK)
            {
                _timer.Stop();
                MessageBox.Show(string.Concat(new object[] { "FMOD error! ", result, " - ", Error.String(result) }));
            }
        }

        private object FindObjectAt(Point point)
        {
            if (_bandsBounds.Contains(point))
            {
                foreach (FrequencyBand band in _bands)
                {
                    if (band.Region.Contains(point))
                    {
                        return band;
                    }
                }
            }
            else if (_channelBounds.Contains(point))
            {
                int num = ((point.X - _channelBounds.X) / _channelBoxWidth) - 1;
                return (_startChannel + num);
            }
            return null;
        }

        private void GetSpectrumData()
        {
            var numoutputchannels = 0;
            var samplerate = 0;
            var none = SOUND_FORMAT.NONE;
            var linear = DSP_RESAMPLER.LINEAR;
            int channeloffset;
            _system.getSoftwareFormat(ref samplerate, ref none, ref numoutputchannels, ref samplerate, ref linear, ref samplerate);
            for (channeloffset = 0; channeloffset < numoutputchannels; channeloffset++)
            {
                _system.getSpectrum(_spectrum, 0x200, channeloffset, DSP_FFT_WINDOW.TRIANGLE);
                foreach (FrequencyBand band in _bands)
                {
                    var num4 = 0f;
                    for (var i = band.FmodLowFrequency; i < band.FmodHighFrequency; i++)
                    {
                        num4 = Math.Max(num4, _spectrum[i]);
                    }
                    band.CurrentScaledLevel = num4 * _scaleFactor;
                }
            }
        }

        

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _doingMinSliderDrag = false;
            _doingMaxSliderDrag = false;
            if (Cursor == Cursors.SizeNS)
            {
                if (_lastBand.MinSliderRegion.Contains(e.Location))
                {
                    _lastBand.MouseOffset = e.Y - _lastBand.MinSliderRegion.Y;
                    _doingMinSliderDrag = true;
                }
                else
                {
                    _lastBand.MouseOffset = e.Y - _lastBand.MaxSliderRegion.Y;
                    _doingMaxSliderDrag = true;
                }
            }
            if (!_playing && (e.Button == MouseButtons.Left))
            {
                _dragStartObject = null;
                if (_channelText != string.Empty)
                {
                    _dragStartObject = _lastChannel;
                }
                else if ((_lastBand != null) && _bandsBounds.Contains(e.Location))
                {
                    _dragStartObject = _lastBand;
                }
                else if ((_lastChannel == _leftArrowChannel) && _leftArrowEnabled)
                {
                    _startChannel = Math.Max(0, _startChannel - _channelsShown);
                    _leftArrowChannel = _startChannel - 1;
                    _rightArrowChannel = _startChannel + _channelsShown;
                    _leftArrowEnabled = _startChannel > 0;
                    _rightArrowEnabled = _eventSequence.ChannelCount > (_maxChannelsShown + _startChannel);
                    Refresh();
                }
                else if ((_lastChannel == _rightArrowChannel) && _rightArrowEnabled)
                {
                    _startChannel = Math.Min(_eventSequence.ChannelCount - _channelsShown, _startChannel + _channelsShown);
                    _leftArrowChannel = _startChannel - 1;
                    _rightArrowChannel = _startChannel + _channelsShown;
                    _leftArrowEnabled = _startChannel > 0;
                    _rightArrowEnabled = _eventSequence.ChannelCount > (_maxChannelsShown + _startChannel);
                    Refresh();
                }
                if (_dragStartObject != null)
                {
                    _mouseLastAt = _mouseDownAt = e.Location;
                }
                else
                {
                    _mouseLastAt = _mouseDownAt = Point.Empty;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_doingMinSliderDrag)
            {
                _lastBand.MinSliderTop = e.Y - _lastBand.MouseOffset;
                if (!_playing)
                {
                    Invalidate(_bandsBounds);
                }
            }
            else if (_doingMaxSliderDrag)
            {
                _lastBand.MaxSliderTop = e.Y - _lastBand.MouseOffset;
                if (!_playing)
                {
                    Invalidate(_bandsBounds);
                }
            }
            else
            {
                if (_mouseDownAt != Point.Empty)
                {
                    _mouseLastAt = e.Location;
                }
                if (!_bandsBounds.Contains(e.Location))
                {
                    _lastBand = null;
                }
                if (!_channelBounds.Contains(e.Location))
                {
                    SetLastChannel(-1);
                }
                Cursor = Cursors.Default;
                if ((e.Button & MouseButtons.Left) == MouseButtons.None)
                {
                    if (_bandsBounds.Contains(e.Location))
                    {
                        foreach (FrequencyBand band in _bands)
                        {
                            if (band.Region.Contains(e.Location))
                            {
                                _lastBand = band;
                                if (band.MinSliderRegion.Contains(e.Location) || band.MaxSliderRegion.Contains(e.Location))
                                {
                                    Cursor = Cursors.SizeNS;
                                }
                                break;
                            }
                        }
                    }
                    else if (!(!_channelBounds.Contains(e.Location) || _playing))
                    {
                        int num = ((e.X - _channelBounds.X) / _channelBoxWidth) - 1;
                        SetLastChannel(_startChannel + num);
                    }
                }
                if (!_playing)
                {
                    Refresh();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_doingMinSliderDrag)
            {
                _doingMinSliderDrag = false;
                _mouseDownAt = Point.Empty;
            }
            else if (_doingMaxSliderDrag)
            {
                _doingMaxSliderDrag = false;
                _mouseDownAt = Point.Empty;
            }
            else if ((!_playing && (e.Button == MouseButtons.Left)) && (_mouseDownAt != Point.Empty))
            {
                _mouseDownAt = Point.Empty;
                object obj2 = FindObjectAt(e.Location);
                if (_dragStartObject != obj2)
                {
                    List<int> list;
                    FrequencyBand dragStartObject;
                    int current;
                    if (obj2 != null)
                    {
                        if (_dragStartObject.GetType() == obj2.GetType())
                        {
                            var frequencyBand = _dragStartObject as FrequencyBand;
                            if (frequencyBand != null)
                            {
                                if (_bandChannelConnections.TryGetValue(frequencyBand, out list) && (list.Count > 0))
                                {
                                    _bandChannelConnections.Remove(frequencyBand);
                                    _bandChannelConnections[(FrequencyBand) obj2] = list;
                                    using (List<int>.Enumerator enumerator = list.GetEnumerator())
                                    {
                                        while (enumerator.MoveNext())
                                        {
                                            current = enumerator.Current;
                                            _channelBandConnections[current] = (FrequencyBand) obj2;
                                        }
                                    }
                                }
                            }
                            else if (_channelBandConnections.TryGetValue((int) _dragStartObject, out dragStartObject))
                            {
                                list = _bandChannelConnections[dragStartObject];
                                list.Remove((int) _dragStartObject);
                                list.Add((int) obj2);
                                _channelBandConnections.Remove((int) _dragStartObject);
                                _channelBandConnections[(int) obj2] = dragStartObject;
                            }
                        }
                        else
                        {
                            var frequencyBand = _dragStartObject as FrequencyBand;
                            if (frequencyBand != null)
                            {
                                dragStartObject = frequencyBand;
                                current = (int) obj2;
                            }
                            else
                            {
                                dragStartObject = (FrequencyBand) obj2;
                                current = (int) _dragStartObject;
                            }
                            if (_channelBandConnections.ContainsKey(current))
                            {
                                return;
                            }
                            if (!_bandChannelConnections.TryGetValue(dragStartObject, out list))
                            {
                                list = new List<int>();
                                _bandChannelConnections[dragStartObject] = list;
                            }
                            list.Add(current);
                            _channelBandConnections[current] = dragStartObject;
                        }
                    }
                    else
                    {
                        var frequencyBand = _dragStartObject as FrequencyBand;
                        if (frequencyBand != null)
                        {
                            if (_bandChannelConnections.TryGetValue(frequencyBand, out list))
                            {
                                foreach (int num in list)
                                {
                                    _channelBandConnections.Remove(num);
                                }
                                list.Clear();
                            }
                        }
                        else
                        {
                            current = (int) _dragStartObject;
                            if (_channelBandConnections.TryGetValue(current, out dragStartObject))
                            {
                                dragStartObject = _channelBandConnections[current];
                                _channelBandConnections.Remove(current);
                                _bandChannelConnections[dragStartObject].Remove(current);
                            }
                        }
                    }
                    Refresh();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int left;
            Graphics graphics = e.Graphics;
            graphics.DrawRectangle(Pens.Black, _bandsBounds);
            for (left = _bandsBounds.Left + 28; left < _bandsBounds.Right; left += 28)
            {
                graphics.DrawLine(Pens.Black, left, _bandsBounds.Top, left, _bandsBounds.Bottom);
            }
            int num4 = _bandsBounds.Top + 2;
            left = _bandsBounds.Left;
            for (int i = 0; left < _bandsBounds.Right; i++)
            {
                graphics.FillRectangle(Brushes.Black, _bands[i].Region);
                graphics.DrawString(_bands[i].CenterFrequency, _bandFont, Brushes.DarkSlateBlue, left, num4);
                left += 28;
            }
            if (_playing)
            {
                DrawSpectrum(e.Graphics);
                _system.update();
            }
            foreach (FrequencyBand band in _bands)
            {
                graphics.FillRectangle(Brushes.Red, band.MaxSliderRegion);
                graphics.FillRectangle(Brushes.Blue, band.MinSliderRegion);
            }
            if (!_playing)
            {
                e.Graphics.DrawRectangle(Pens.Black, _channelBounds);
                int num2 = _channelBounds.X + _channelBoxWidth;
                List<VixenPlus.Channel> channels = _eventSequence.Channels;
                for (left = 0; left < _channelsShown; left++)
                {
                    e.Graphics.FillRectangle(channels[left + _startChannel].Brush, num2 + 1, _channelBounds.Top + 1, _channelBoxWidth - 1, _channelBoxWidth - 1);
                    e.Graphics.DrawLine(Pens.Black, num2, _channelBounds.Top, num2, _channelBounds.Bottom);
                    num2 += _channelBoxWidth;
                }
                e.Graphics.DrawLine(Pens.Black, num2, _channelBounds.Top, num2, _channelBounds.Bottom);
                e.Graphics.DrawString(_channelText, _textFont, Brushes.Black, _channelBounds.X, _channelBounds.Bottom + 15);
                e.Graphics.FillPolygon(_leftArrowEnabled ? Brushes.Black : Brushes.LightGray, _leftArrowPoints);
                e.Graphics.FillPolygon(_rightArrowEnabled ? Brushes.Black : Brushes.LightGray, _rightArrowPoints);
                const int num5 = 10;
                var num6 = (_channelBoxWidth - 7) / 2;
                const int num7 = 14;
                const int num8 = 3;
                foreach (FrequencyBand band in _bandChannelConnections.Keys)
                {
                    foreach (int num9 in _bandChannelConnections[band])
                    {
                        e.Graphics.DrawLine(Pens.Black, band.Region.Left + num7, (band.Region.Bottom - 10) + num8, (_channelBounds.Left + (((num9 + 1) - _startChannel) * _channelBoxWidth)) + (_channelBoxWidth / 2), (_channelBounds.Top + num6) + num8);
                        e.Graphics.FillEllipse(Brushes.White, band.Region.Left + num5, band.Region.Bottom - 10, 7, 7);
                        e.Graphics.FillEllipse(Brushes.Black, (_channelBounds.Left + (((num9 + 1) - _startChannel) * _channelBoxWidth)) + num6, _channelBounds.Top + num6, 7, 7);
                    }
                }
                if (_mouseDownAt != Point.Empty)
                {
                    e.Graphics.DrawLine(Pens.Black, _mouseDownAt, _mouseLastAt);
                }
            }
        }

        private void PauseResume()
        {
            if (_playing)
            {
                bool paused = false;
                _channel.getPaused(ref paused);
                _paused = !paused;
                _channel.setPaused(_paused);
            }
        }

        private void pictureBoxPause_Click(object sender, EventArgs e)
        {
            PauseResume();
        }

        private void pictureBoxPlay_Click(object sender, EventArgs e)
        {
            if (_paused)
            {
                PauseResume();
            }
            else if (!_playing)
            {
                _playing = true;
                RESULT result = _system.playSound(CHANNELINDEX.FREE, _sound, false, ref _channel);
                ErrCheck(result);
                _timer.Start();
            }
            Refresh();
        }

        private void pictureBoxScaleDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (_scaleFactor > 1f)
            {
                if (checkBoxLockSliders.Checked)
                {
                    ScaleSlidersTo(_scaleFactor - 0.5f);
                }
                _scaleFactor -= 0.5f;
                UpdateScaleLabel();
            }
        }

        private void pictureBoxScaleUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkBoxLockSliders.Checked)
            {
                ScaleSlidersTo(_scaleFactor + 0.5f);
            }
            _scaleFactor += 0.5f;
            UpdateScaleLabel();
        }

        private void pictureBoxStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void ScaleSlidersTo(float newScaleFactor)
        {
            foreach (FrequencyBand band in _bands)
            {
                int num = (band.Region.Bottom - band.MinSliderTop) - 4;
                band.MinSliderTop = (band.Region.Bottom - ((int) (num / _scaleFactor * newScaleFactor))) - 4;
                num = band.Region.Bottom - band.MaxSliderTop;
                band.MaxSliderTop = band.Region.Bottom - ((int) (num / _scaleFactor * newScaleFactor));
            }
            Invalidate(_bandsBounds);
        }

        private void SetLastChannel(int index)
        {
            if (index == -1)
            {
                _channelText = string.Empty;
                _lastChannel = -1;
            }
            else if ((index == _leftArrowChannel) || (index == _rightArrowChannel))
            {
                _lastChannel = index;
                _channelText = string.Empty;
            }
            else if (_lastChannel != index)
            {
                _lastChannel = index;
                _channelText = _eventSequence.Channels[index].Name;
            }
        }

        private void SpectrumDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_playing)
            {
                Stop();
            }
        }

        private void SpectrumDialog_Load(object sender, EventArgs e)
        {
        }

        private void SpectrumParamsDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            new HelpDialog("Indicate how you would like your sequence to respond to the frequency bands by dragging channels to bands and bands to channels.\n\nConnections can be broken by dragging them off of a band or channel.\n\nConnections can be moved by dragging them.\n\nA band can be connected to multiple channels, but a channel can only be connected to a single band.\n\nDrag the blue slider up and down in the band to set the response threshold.\nThe scale buttons on the left will adjust the magnitude of the frequency bands' levels.").ShowDialog();
        }

        private void Stop()
        {
            _playing = false;
            _paused = false;
            _timer.Stop();
            if (_channel != null)
            {
                _channel.stop();
                _channel = null;
            }
            Refresh();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Invalidate(_bandsBounds);
            if (_system != null)
            {
                _system.update();
            }
            if (_channel != null)
            {
                bool isplaying = false;
                _channel.isPlaying(ref isplaying);
                if (!isplaying)
                {
                    Stop();
                }
            }
        }

        private void UpdateScaleLabel()
        {
            labelScaleFactor.Text = string.Format("Scale\nx {0:F1}", _scaleFactor);
        }

        public bool LockSliders
        {
            get
            {
                return checkBoxLockSliders.Checked;
            }
            set
            {
                checkBoxLockSliders.Checked = value;
            }
        }

        public FrequencyBandMapping[] Mappings
        {
            get
            {
                var list = new List<FrequencyBandMapping>();
                foreach (FrequencyBand band in _bands)
                {
                    var item = new FrequencyBandMapping(band.ResponseLevelMin, band.ResponseLevelMax);
                    list.Add(item);
                    if (_bandChannelConnections.ContainsKey(band))
                    {
                        item.ChannelList.AddRange(_bandChannelConnections[band]);
                    }
                }
                return list.ToArray();
            }
            set
            {
                _mUserSetResponseLevels = value.Length > 0;
                int num = Math.Min(_bands.Count, value.Length);
                for (int i = 0; i < num; i++)
                {
                    List<int> list;
                    FrequencyBand key = _bands[i];
                    FrequencyBandMapping mapping = value[i];
                    key.ResponseLevelMin = mapping.ResponseLevelMin;
                    key.ResponseLevelMax = mapping.ResponseLevelMax;
                    if (!_bandChannelConnections.TryGetValue(key, out list))
                    {
                        list = new List<int>();
                        _bandChannelConnections[key] = list;
                    }
                    list.AddRange(mapping.ChannelList);
                    foreach (int num3 in mapping.ChannelList)
                    {
                        _channelBandConnections[num3] = key;
                    }
                }
            }
        }

        public float ScaleFactor
        {
            get
            {
                return _scaleFactor;
            }
            set
            {
                _scaleFactor = value;
                UpdateScaleLabel();
            }
        }
    }
}

