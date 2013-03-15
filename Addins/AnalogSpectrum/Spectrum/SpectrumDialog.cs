namespace Spectrum
{
    using FMOD;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Vixen;
    using Vixen.Dialogs;

    internal class SpectrumDialog : Form
    {
        private const int BAND_HEIGHT = 100;
        private const int BAND_LEVEL_GUTTER = 3;
        private const int BAND_LEVEL_MAX_HEIGHT = 0x4b;
        private const int BANDS_OFFSET = 50;
        private Button buttonAutoMap;
        private Button buttonCancel;
        private Button buttonOK;
        private FMOD.Channel channel = null;
        private const int CHANNEL_OFFSET = 0xe1;
        private CheckBox checkBoxLockSliders;
        private IContainer components = null;
        private const int CONNECTION_ENDPOINT_SIZE = 7;
        private const int CONNECTION_ENDPOINT_VERTICAL_OFFSET = 10;
        private Label labelScaleFactor;
        private Dictionary<FrequencyBand, List<int>> m_bandChannelConnections;
        private Font m_bandFont = new Font("Arial", 8f);
        private List<FrequencyBand> m_bands;
        private Rectangle m_bandsBounds;
        private Dictionary<int, FrequencyBand> m_channelBandConnections;
        private Rectangle m_channelBounds;
        private int m_channelBoxWidth;
        private int m_channelsShown;
        private string m_channelText = string.Empty;
        private bool m_doingMaxSliderDrag = false;
        private bool m_doingMinSliderDrag = false;
        private object m_dragStartObject = null;
        private FrequencyBand m_lastBand = null;
        private int m_lastChannel = -1;
        private Point m_lastMouseLocation;
        private int m_leftArrowChannel = -1;
        private bool m_leftArrowEnabled;
        private Point[] m_leftArrowPoints;
        private int m_maxChannelsShown;
        private Point m_mouseDownAt = Point.Empty;
        private Point m_mouseLastAt = Point.Empty;
        private bool m_paused = false;
        private bool m_playing = false;
        private int m_rightArrowChannel;
        private bool m_rightArrowEnabled;
        private Point[] m_rightArrowPoints;
        private float m_scaleFactor = 5f;
        private EventSequence m_sequence;
        private int m_startChannel;
        private Font m_textFont = new Font("Arial", 14f);
        private bool m_userSetResponseLevels = false;
        private PictureBox pictureBoxPause;
        private PictureBox pictureBoxPlay;
        private PictureBox pictureBoxScaleDown;
        private PictureBox pictureBoxScaleUp;
        private PictureBox pictureBoxStop;
        private const int SINGLE_BAND_WIDTH = 0x1c;
        private const int SLIDER_HEIGHT = 4;
        private Sound sound = null;
        private float[] spectrum = new float[0x200];
        private const int SPECTRUMSIZE = 0x200;
        private System system = null;
        private System.Windows.Forms.Timer timer;

        public SpectrumDialog(EventSequence sequence)
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            this.m_sequence = sequence;
            this.m_bands = new List<FrequencyBand>();
            this.m_channelBoxWidth = 12;
            this.m_rightArrowChannel = sequence.ChannelCount;
            this.m_lastMouseLocation = new Point(0, 0);
            this.m_bandChannelConnections = new Dictionary<FrequencyBand, List<int>>();
            this.m_channelBandConnections = new Dictionary<int, FrequencyBand>();
            this.UpdateScaleLabel();
            int[] numArray = new int[] { 
                20, 0x19, 0x1f, 40, 50, 0x3f, 80, 100, 0x7d, 160, 200, 250, 0x13b, 400, 500, 630, 
                800, 0x3e8, 0x4e2, 0x640, 0x7d0, 0x9c4, 0xc4e, 0xfa0, 0x1388, 0x189c, 0x1f40, 0x2710, 0x30d4, 0x3e80, 0x4e20
             };
            foreach (int num in numArray)
            {
                this.m_bands.Add(new FrequencyBand(num));
            }
            this.ERRCHECK(Factory.System_Create(ref this.system));
            this.ERRCHECK(this.system.init(0x20, INITFLAG.NORMAL, IntPtr.Zero));
            int width = this.m_bands.Count * 0x1c;
            this.m_bandsBounds = new Rectangle((((base.ClientRectangle.Width - 40) - width) >> 1) + 40, 50, width, 100);
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
            int num4 = (this.m_channelsShown + 2) * this.m_channelBoxWidth;
            this.m_channelBounds = new Rectangle((base.ClientRectangle.Width - num4) >> 1, 0xe1, num4, this.m_channelBoxWidth);
            this.m_channelBounds.X = Math.Max(this.m_channelBounds.X, 2);
            Point[] pointArray = new Point[] { new Point(this.m_channelBounds.Left + 3, this.m_channelBounds.Top + 6), new Point(this.m_channelBounds.Left + 9, this.m_channelBounds.Top + 3), new Point(this.m_channelBounds.Left + 9, this.m_channelBounds.Top + 9) };
            this.m_leftArrowPoints = pointArray;
            pointArray = new Point[] { new Point(this.m_channelBounds.Right - 3, this.m_channelBounds.Top + 6), new Point(this.m_channelBounds.Right - 9, this.m_channelBounds.Top + 3), new Point(this.m_channelBounds.Right - 9, this.m_channelBounds.Top + 9) };
            this.m_rightArrowPoints = pointArray;
            for (int i = 0; i < this.m_bands.Count; i++)
            {
                FrequencyBand band = this.m_bands[i];
                band.Region = new Rectangle((3 + this.m_bandsBounds.Left) + (i * 0x1c), 0x4b, 0x16, 0x4b);
                band.MinSliderRegion = new Rectangle(band.Region.X, (band.Region.Bottom - (band.Region.Height / 2)) - 4, band.Region.Width, 4);
                band.MaxSliderRegion = new Rectangle(band.Region.X, band.Region.Top, band.Region.Width, 4);
                if (!this.m_userSetResponseLevels)
                {
                    band.ResponseLevelMax = 1f;
                    band.ResponseLevelMin = 0.5f;
                }
            }
            RESULT result = this.system.createStream(Path.Combine(Paths.AudioPath, this.m_sequence.Audio.FileName), MODE.SOFTWARE | MODE._2D, ref this.sound);
            this.ERRCHECK(result);
        }

        private void buttonAutoMap_Click(object sender, EventArgs e)
        {
            AutoMapDialog dialog = new AutoMapDialog(this.m_sequence.Channels, this.m_bands);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int startChannelIndex = dialog.StartChannelIndex;
                int startBandIndex = dialog.StartBandIndex;
                int num3 = Math.Min((int) (this.m_bands.Count - startBandIndex), (int) (this.m_sequence.Channels.Count - startChannelIndex));
                this.m_bandChannelConnections.Clear();
                this.m_channelBandConnections.Clear();
                for (int i = 0; i < num3; i++)
                {
                    FrequencyBand band = this.m_bands[startBandIndex];
                    List<int> list = new List<int>();
                    this.m_bandChannelConnections[band] = list;
                    list.Add(startChannelIndex);
                    this.m_channelBandConnections[startChannelIndex] = band;
                    startBandIndex++;
                    startChannelIndex++;
                }
                this.Refresh();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.m_channelBandConnections.Count == 0)
            {
                MessageBox.Show("There are no mappings created.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                uint length = 0;
                uint position = 0;
                this.Stop();
                this.sound.release();
                this.system.close();
                this.system.release();
                this.ERRCHECK(Factory.System_Create(ref this.system));
                this.system.setOutput(OUTPUTTYPE.NOSOUND_NRT);
                this.ERRCHECK(this.system.init(0x20, INITFLAG.STREAM_FROM_UPDATE, IntPtr.Zero));
                this.system.createStream(Path.Combine(Paths.AudioPath, this.m_sequence.Audio.FileName), MODE.SOFTWARE | MODE._2D, ref this.sound);
                this.sound.getLength(ref length, TIMEUNIT.MS);
                TranscribeDialog dialog = new TranscribeDialog((int) length);
                dialog.Show();
                this.ERRCHECK(this.system.playSound(CHANNELINDEX.FREE, this.sound, false, ref this.channel));
                int num6 = this.m_sequence.MaximumLevel - this.m_sequence.MinimumLevel;
                while (position < length)
                {
                    this.channel.getPosition(ref position, TIMEUNIT.MS);
                    dialog.Progress = (int) position;
                    this.GetSpectrumData();
                    int num3 = (int) (((ulong) position) / ((long) this.m_sequence.EventPeriod));
                    if (num3 >= this.m_sequence.TotalEventPeriods)
                    {
                        break;
                    }
                    foreach (FrequencyBand band in this.m_bands)
                    {
                        float responseLevelMin = band.ResponseLevelMin;
                        float responseLevelMax = band.ResponseLevelMax;
                        float num7 = responseLevelMax - responseLevelMin;
                        if ((this.m_bandChannelConnections.ContainsKey(band) && (band.CurrentScaledLevel >= responseLevelMin)) && (band.CurrentScaledLevel <= responseLevelMax))
                        {
                            foreach (int num8 in this.m_bandChannelConnections[band])
                            {
                                this.m_sequence.EventValues[num8, num3] = (byte) ((((band.CurrentScaledLevel - responseLevelMin) / num7) * num6) + this.m_sequence.MinimumLevel);
                            }
                        }
                    }
                    this.system.update();
                }
                this.channel.stop();
                dialog.Hide();
                dialog.Dispose();
                this.sound.release();
                this.sound = null;
                this.system.close();
                this.system.release();
                this.system = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            RESULT result;
            if (this.sound != null)
            {
                result = this.sound.release();
                this.ERRCHECK(result);
            }
            if (this.system != null)
            {
                result = this.system.close();
                this.ERRCHECK(result);
                result = this.system.release();
                this.ERRCHECK(result);
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            this.m_textFont.Dispose();
            this.m_bandFont.Dispose();
            base.Dispose(disposing);
        }

        private void DrawSpectrum(Graphics g)
        {
            int numoutputchannels = 0;
            int samplerate = 0;
            SOUND_FORMAT nONE = SOUND_FORMAT.NONE;
            DSP_RESAMPLER lINEAR = DSP_RESAMPLER.LINEAR;
            int channeloffset = 0;
            int height = 0x19;
            int num6 = 50;
            Rectangle b = new Rectangle(3, this.m_bandsBounds.Bottom - height, 0x16, height);
            Rectangle rectangle3 = new Rectangle(3, this.m_bandsBounds.Bottom - num6, 0x16, height);
            Rectangle rectangle4 = new Rectangle(3, this.m_bandsBounds.Bottom - 0x4b, 0x16, height);
            this.system.getSoftwareFormat(ref samplerate, ref nONE, ref numoutputchannels, ref samplerate, ref lINEAR, ref samplerate);
            float num7 = 0f;
            for (channeloffset = 0; channeloffset < numoutputchannels; channeloffset++)
            {
                this.system.getSpectrum(this.spectrum, 0x200, channeloffset, DSP_FFT_WINDOW.TRIANGLE);
                foreach (FrequencyBand band in this.m_bands)
                {
                    int num8;
                    num7 = 0f;
                    for (int i = band.FmodLowFrequency; i < band.FmodHighFrequency; i++)
                    {
                        num7 = Math.Max(num7, this.spectrum[i]);
                    }
                    Rectangle region = band.Region;
                    region.Height = Math.Min((int) ((75f * num7) * this.m_scaleFactor), 0x4b);
                    region.Y = band.Region.Bottom - region.Height;
                    rectangle4.X = num8 = region.X;
                    b.X = rectangle3.X = num8;
                    g.FillRectangle(Brushes.Green, Rectangle.Intersect(region, b));
                    g.FillRectangle(Brushes.Yellow, Rectangle.Intersect(region, rectangle3));
                    g.FillRectangle(Brushes.Red, Rectangle.Intersect(region, rectangle4));
                }
            }
        }

        private void ERRCHECK(RESULT result)
        {
            if (result != RESULT.OK)
            {
                this.timer.Stop();
                MessageBox.Show(string.Concat(new object[] { "FMOD error! ", result, " - ", Error.String(result) }));
            }
        }

        private object FindObjectAt(Point point)
        {
            if (this.m_bandsBounds.Contains(point))
            {
                foreach (FrequencyBand band in this.m_bands)
                {
                    if (band.Region.Contains(point))
                    {
                        return band;
                    }
                }
            }
            else if (this.m_channelBounds.Contains(point))
            {
                int num = ((point.X - this.m_channelBounds.X) / this.m_channelBoxWidth) - 1;
                return (this.m_startChannel + num);
            }
            return null;
        }

        private void GetSpectrumData()
        {
            int numoutputchannels = 0;
            int samplerate = 0;
            SOUND_FORMAT nONE = SOUND_FORMAT.NONE;
            DSP_RESAMPLER lINEAR = DSP_RESAMPLER.LINEAR;
            int channeloffset = 0;
            this.system.getSoftwareFormat(ref samplerate, ref nONE, ref numoutputchannels, ref samplerate, ref lINEAR, ref samplerate);
            float num4 = 0f;
            for (channeloffset = 0; channeloffset < numoutputchannels; channeloffset++)
            {
                this.system.getSpectrum(this.spectrum, 0x200, channeloffset, DSP_FFT_WINDOW.TRIANGLE);
                foreach (FrequencyBand band in this.m_bands)
                {
                    num4 = 0f;
                    for (int i = band.FmodLowFrequency; i < band.FmodHighFrequency; i++)
                    {
                        num4 = Math.Max(num4, this.spectrum[i]);
                    }
                    band.CurrentScaledLevel = num4 * this.m_scaleFactor;
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SpectrumDialog));
            this.buttonAutoMap = new Button();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.pictureBoxPlay = new PictureBox();
            this.pictureBoxPause = new PictureBox();
            this.pictureBoxStop = new PictureBox();
            this.pictureBoxScaleUp = new PictureBox();
            this.pictureBoxScaleDown = new PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.labelScaleFactor = new Label();
            this.checkBoxLockSliders = new CheckBox();
            ((ISupportInitialize) this.pictureBoxPlay).BeginInit();
            ((ISupportInitialize) this.pictureBoxPause).BeginInit();
            ((ISupportInitialize) this.pictureBoxStop).BeginInit();
            ((ISupportInitialize) this.pictureBoxScaleUp).BeginInit();
            ((ISupportInitialize) this.pictureBoxScaleDown).BeginInit();
            base.SuspendLayout();
            this.buttonAutoMap.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonAutoMap.Location = new Point(12, 0x143);
            this.buttonAutoMap.Name = "buttonAutoMap";
            this.buttonAutoMap.Size = new Size(0x4b, 0x17);
            this.buttonAutoMap.TabIndex = 1;
            this.buttonAutoMap.Text = "Auto Map";
            this.buttonAutoMap.UseVisualStyleBackColor = true;
            this.buttonAutoMap.Click += new EventHandler(this.buttonAutoMap_Click);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x31c, 0x143);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x36d, 0x143);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.pictureBoxPlay.Image = (Image) manager.GetObject("pictureBoxPlay.Image");
            this.pictureBoxPlay.Location = new Point(0x1c4, 0x124);
            this.pictureBoxPlay.Name = "pictureBoxPlay";
            this.pictureBoxPlay.Size = new Size(0x10, 0x10);
            this.pictureBoxPlay.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxPlay.TabIndex = 3;
            this.pictureBoxPlay.TabStop = false;
            this.pictureBoxPlay.Click += new EventHandler(this.pictureBoxPlay_Click);
            this.pictureBoxPause.Image = (Image) manager.GetObject("pictureBoxPause.Image");
            this.pictureBoxPause.Location = new Point(0x1da, 0x124);
            this.pictureBoxPause.Name = "pictureBoxPause";
            this.pictureBoxPause.Size = new Size(0x10, 0x10);
            this.pictureBoxPause.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxPause.TabIndex = 4;
            this.pictureBoxPause.TabStop = false;
            this.pictureBoxPause.Click += new EventHandler(this.pictureBoxPause_Click);
            this.pictureBoxStop.Image = (Image) manager.GetObject("pictureBoxStop.Image");
            this.pictureBoxStop.Location = new Point(0x1f0, 0x124);
            this.pictureBoxStop.Name = "pictureBoxStop";
            this.pictureBoxStop.Size = new Size(0x10, 0x10);
            this.pictureBoxStop.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxStop.TabIndex = 5;
            this.pictureBoxStop.TabStop = false;
            this.pictureBoxStop.Click += new EventHandler(this.pictureBoxStop_Click);
            this.pictureBoxScaleUp.Image = (Image) manager.GetObject("pictureBoxScaleUp.Image");
            this.pictureBoxScaleUp.Location = new Point(12, 0x52);
            this.pictureBoxScaleUp.Name = "pictureBoxScaleUp";
            this.pictureBoxScaleUp.Size = new Size(0x10, 0x10);
            this.pictureBoxScaleUp.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxScaleUp.TabIndex = 6;
            this.pictureBoxScaleUp.TabStop = false;
            this.pictureBoxScaleUp.MouseDown += new MouseEventHandler(this.pictureBoxScaleUp_MouseDown);
            this.pictureBoxScaleDown.Image = (Image) manager.GetObject("pictureBoxScaleDown.Image");
            this.pictureBoxScaleDown.Location = new Point(12, 0x68);
            this.pictureBoxScaleDown.Name = "pictureBoxScaleDown";
            this.pictureBoxScaleDown.Size = new Size(0x10, 0x10);
            this.pictureBoxScaleDown.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxScaleDown.TabIndex = 7;
            this.pictureBoxScaleDown.TabStop = false;
            this.pictureBoxScaleDown.MouseDown += new MouseEventHandler(this.pictureBoxScaleDown_MouseDown);
            this.timer.Interval = 50;
            this.timer.Tick += new EventHandler(this.timer_Tick);
            this.labelScaleFactor.AutoSize = true;
            this.labelScaleFactor.Location = new Point(10, 0x80);
            this.labelScaleFactor.Name = "labelScaleFactor";
            this.labelScaleFactor.Size = new Size(0x23, 13);
            this.labelScaleFactor.TabIndex = 8;
            this.labelScaleFactor.Text = "label1";
            this.checkBoxLockSliders.AutoSize = true;
            this.checkBoxLockSliders.Location = new Point(14, 300);
            this.checkBoxLockSliders.Name = "checkBoxLockSliders";
            this.checkBoxLockSliders.Size = new Size(0x10f, 0x11);
            this.checkBoxLockSliders.TabIndex = 0;
            this.checkBoxLockSliders.Text = "Adjust sliders automatically when adjusting the scale";
            this.checkBoxLockSliders.UseVisualStyleBackColor = true;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x3c4, 0x166);
            base.Controls.Add(this.checkBoxLockSliders);
            base.Controls.Add(this.labelScaleFactor);
            base.Controls.Add(this.pictureBoxScaleDown);
            base.Controls.Add(this.pictureBoxScaleUp);
            base.Controls.Add(this.pictureBoxStop);
            base.Controls.Add(this.pictureBoxPause);
            base.Controls.Add(this.pictureBoxPlay);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.buttonAutoMap);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.HelpButton = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SpectrumDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Frequency Spectrum Analysis";
            base.HelpButtonClicked += new CancelEventHandler(this.SpectrumParamsDialog_HelpButtonClicked);
            base.FormClosing += new FormClosingEventHandler(this.SpectrumDialog_FormClosing);
            base.Load += new EventHandler(this.SpectrumDialog_Load);
            ((ISupportInitialize) this.pictureBoxPlay).EndInit();
            ((ISupportInitialize) this.pictureBoxPause).EndInit();
            ((ISupportInitialize) this.pictureBoxStop).EndInit();
            ((ISupportInitialize) this.pictureBoxScaleUp).EndInit();
            ((ISupportInitialize) this.pictureBoxScaleDown).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.m_doingMinSliderDrag = false;
            this.m_doingMaxSliderDrag = false;
            if (this.Cursor == Cursors.SizeNS)
            {
                if (this.m_lastBand.MinSliderRegion.Contains(e.Location))
                {
                    this.m_lastBand.MouseOffset = e.Y - this.m_lastBand.MinSliderRegion.Y;
                    this.m_doingMinSliderDrag = true;
                }
                else
                {
                    this.m_lastBand.MouseOffset = e.Y - this.m_lastBand.MaxSliderRegion.Y;
                    this.m_doingMaxSliderDrag = true;
                }
            }
            if (!this.m_playing && (e.Button == MouseButtons.Left))
            {
                this.m_dragStartObject = null;
                if (this.m_channelText != string.Empty)
                {
                    this.m_dragStartObject = this.m_lastChannel;
                }
                else if ((this.m_lastBand != null) && this.m_bandsBounds.Contains(e.Location))
                {
                    this.m_dragStartObject = this.m_lastBand;
                }
                else if ((this.m_lastChannel == this.m_leftArrowChannel) && this.m_leftArrowEnabled)
                {
                    this.m_startChannel = Math.Max(0, this.m_startChannel - this.m_channelsShown);
                    this.m_leftArrowChannel = this.m_startChannel - 1;
                    this.m_rightArrowChannel = this.m_startChannel + this.m_channelsShown;
                    this.m_leftArrowEnabled = this.m_startChannel > 0;
                    this.m_rightArrowEnabled = this.m_sequence.ChannelCount > (this.m_maxChannelsShown + this.m_startChannel);
                    this.Refresh();
                }
                else if ((this.m_lastChannel == this.m_rightArrowChannel) && this.m_rightArrowEnabled)
                {
                    this.m_startChannel = Math.Min((int) (this.m_sequence.ChannelCount - this.m_channelsShown), (int) (this.m_startChannel + this.m_channelsShown));
                    this.m_leftArrowChannel = this.m_startChannel - 1;
                    this.m_rightArrowChannel = this.m_startChannel + this.m_channelsShown;
                    this.m_leftArrowEnabled = this.m_startChannel > 0;
                    this.m_rightArrowEnabled = this.m_sequence.ChannelCount > (this.m_maxChannelsShown + this.m_startChannel);
                    this.Refresh();
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.m_doingMinSliderDrag)
            {
                this.m_lastBand.MinSliderTop = e.Y - this.m_lastBand.MouseOffset;
                if (!this.m_playing)
                {
                    base.Invalidate(this.m_bandsBounds);
                }
            }
            else if (this.m_doingMaxSliderDrag)
            {
                this.m_lastBand.MaxSliderTop = e.Y - this.m_lastBand.MouseOffset;
                if (!this.m_playing)
                {
                    base.Invalidate(this.m_bandsBounds);
                }
            }
            else
            {
                if (this.m_mouseDownAt != Point.Empty)
                {
                    this.m_mouseLastAt = e.Location;
                }
                if (!this.m_bandsBounds.Contains(e.Location))
                {
                    this.m_lastBand = null;
                }
                if (!this.m_channelBounds.Contains(e.Location))
                {
                    this.SetLastChannel(-1);
                }
                this.Cursor = Cursors.Default;
                if ((e.Button & MouseButtons.Left) == MouseButtons.None)
                {
                    if (this.m_bandsBounds.Contains(e.Location))
                    {
                        foreach (FrequencyBand band in this.m_bands)
                        {
                            if (band.Region.Contains(e.Location))
                            {
                                this.m_lastBand = band;
                                if (band.MinSliderRegion.Contains(e.Location) || band.MaxSliderRegion.Contains(e.Location))
                                {
                                    this.Cursor = Cursors.SizeNS;
                                }
                                break;
                            }
                        }
                    }
                    else if (!(!this.m_channelBounds.Contains(e.Location) || this.m_playing))
                    {
                        int num = ((e.X - this.m_channelBounds.X) / this.m_channelBoxWidth) - 1;
                        this.SetLastChannel(this.m_startChannel + num);
                    }
                }
                if (!this.m_playing)
                {
                    this.Refresh();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.m_doingMinSliderDrag)
            {
                this.m_doingMinSliderDrag = false;
                this.m_mouseDownAt = Point.Empty;
            }
            else if (this.m_doingMaxSliderDrag)
            {
                this.m_doingMaxSliderDrag = false;
                this.m_mouseDownAt = Point.Empty;
            }
            else if ((!this.m_playing && (e.Button == MouseButtons.Left)) && (this.m_mouseDownAt != Point.Empty))
            {
                this.m_mouseDownAt = Point.Empty;
                object obj2 = this.FindObjectAt(e.Location);
                if (this.m_dragStartObject != obj2)
                {
                    List<int> list;
                    FrequencyBand dragStartObject;
                    int current;
                    if (obj2 != null)
                    {
                        if (this.m_dragStartObject.GetType() == obj2.GetType())
                        {
                            if (this.m_dragStartObject is FrequencyBand)
                            {
                                if (this.m_bandChannelConnections.TryGetValue((FrequencyBand) this.m_dragStartObject, out list) && (list.Count > 0))
                                {
                                    this.m_bandChannelConnections.Remove((FrequencyBand) this.m_dragStartObject);
                                    this.m_bandChannelConnections[(FrequencyBand) obj2] = list;
                                    using (List<int>.Enumerator enumerator = list.GetEnumerator())
                                    {
                                        while (enumerator.MoveNext())
                                        {
                                            current = enumerator.Current;
                                            this.m_channelBandConnections[current] = (FrequencyBand) obj2;
                                        }
                                    }
                                }
                            }
                            else if (this.m_channelBandConnections.TryGetValue((int) this.m_dragStartObject, out dragStartObject))
                            {
                                list = this.m_bandChannelConnections[dragStartObject];
                                list.Remove((int) this.m_dragStartObject);
                                list.Add((int) obj2);
                                this.m_channelBandConnections.Remove((int) this.m_dragStartObject);
                                this.m_channelBandConnections[(int) obj2] = dragStartObject;
                            }
                        }
                        else
                        {
                            if (this.m_dragStartObject is FrequencyBand)
                            {
                                dragStartObject = (FrequencyBand) this.m_dragStartObject;
                                current = (int) obj2;
                            }
                            else
                            {
                                dragStartObject = (FrequencyBand) obj2;
                                current = (int) this.m_dragStartObject;
                            }
                            if (this.m_channelBandConnections.ContainsKey(current))
                            {
                                return;
                            }
                            if (!this.m_bandChannelConnections.TryGetValue(dragStartObject, out list))
                            {
                                list = new List<int>();
                                this.m_bandChannelConnections[dragStartObject] = list;
                            }
                            list.Add(current);
                            this.m_channelBandConnections[current] = dragStartObject;
                        }
                    }
                    else if (this.m_dragStartObject is FrequencyBand)
                    {
                        if (this.m_bandChannelConnections.TryGetValue((FrequencyBand) this.m_dragStartObject, out list))
                        {
                            foreach (int num in list)
                            {
                                this.m_channelBandConnections.Remove(num);
                            }
                            list.Clear();
                        }
                    }
                    else
                    {
                        current = (int) this.m_dragStartObject;
                        if (this.m_channelBandConnections.TryGetValue(current, out dragStartObject))
                        {
                            dragStartObject = this.m_channelBandConnections[current];
                            this.m_channelBandConnections.Remove(current);
                            this.m_bandChannelConnections[dragStartObject].Remove(current);
                        }
                    }
                    this.Refresh();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int left;
            Graphics graphics = e.Graphics;
            graphics.DrawRectangle(Pens.Black, this.m_bandsBounds);
            for (left = this.m_bandsBounds.Left + 0x1c; left < this.m_bandsBounds.Right; left += 0x1c)
            {
                graphics.DrawLine(Pens.Black, left, this.m_bandsBounds.Top, left, this.m_bandsBounds.Bottom);
            }
            int num4 = this.m_bandsBounds.Top + 2;
            left = this.m_bandsBounds.Left;
            for (int i = 0; left < this.m_bandsBounds.Right; i++)
            {
                graphics.FillRectangle(Brushes.Black, this.m_bands[i].Region);
                graphics.DrawString(this.m_bands[i].CenterFrequency, this.m_bandFont, Brushes.DarkSlateBlue, (float) left, (float) num4);
                left += 0x1c;
            }
            if (this.m_playing)
            {
                this.DrawSpectrum(e.Graphics);
                this.system.update();
            }
            foreach (FrequencyBand band in this.m_bands)
            {
                graphics.FillRectangle(Brushes.Red, band.MaxSliderRegion);
                graphics.FillRectangle(Brushes.Blue, band.MinSliderRegion);
            }
            if (!this.m_playing)
            {
                e.Graphics.DrawRectangle(Pens.Black, this.m_channelBounds);
                int num2 = this.m_channelBounds.X + this.m_channelBoxWidth;
                List<Vixen.Channel> channels = this.m_sequence.Channels;
                for (left = 0; left < this.m_channelsShown; left++)
                {
                    e.Graphics.FillRectangle(channels[left + this.m_startChannel].Brush, (int) (num2 + 1), (int) (this.m_channelBounds.Top + 1), (int) (this.m_channelBoxWidth - 1), (int) (this.m_channelBoxWidth - 1));
                    e.Graphics.DrawLine(Pens.Black, num2, this.m_channelBounds.Top, num2, this.m_channelBounds.Bottom);
                    num2 += this.m_channelBoxWidth;
                }
                e.Graphics.DrawLine(Pens.Black, num2, this.m_channelBounds.Top, num2, this.m_channelBounds.Bottom);
                e.Graphics.DrawString(this.m_channelText, this.m_textFont, Brushes.Black, (float) this.m_channelBounds.X, (float) (this.m_channelBounds.Bottom + 15));
                e.Graphics.FillPolygon(this.m_leftArrowEnabled ? Brushes.Black : Brushes.LightGray, this.m_leftArrowPoints);
                e.Graphics.FillPolygon(this.m_rightArrowEnabled ? Brushes.Black : Brushes.LightGray, this.m_rightArrowPoints);
                int num5 = 10;
                int num6 = (this.m_channelBoxWidth - 7) / 2;
                int num7 = 14;
                int num8 = 3;
                foreach (FrequencyBand band in this.m_bandChannelConnections.Keys)
                {
                    foreach (int num9 in this.m_bandChannelConnections[band])
                    {
                        e.Graphics.DrawLine(Pens.Black, (int) (band.Region.Left + num7), (int) ((band.Region.Bottom - 10) + num8), (int) ((this.m_channelBounds.Left + (((num9 + 1) - this.m_startChannel) * this.m_channelBoxWidth)) + (this.m_channelBoxWidth / 2)), (int) ((this.m_channelBounds.Top + num6) + num8));
                        e.Graphics.FillEllipse(Brushes.White, band.Region.Left + num5, band.Region.Bottom - 10, 7, 7);
                        e.Graphics.FillEllipse(Brushes.Black, (this.m_channelBounds.Left + (((num9 + 1) - this.m_startChannel) * this.m_channelBoxWidth)) + num6, this.m_channelBounds.Top + num6, 7, 7);
                    }
                }
                if (this.m_mouseDownAt != Point.Empty)
                {
                    e.Graphics.DrawLine(Pens.Black, this.m_mouseDownAt, this.m_mouseLastAt);
                }
            }
        }

        private void PauseResume()
        {
            if (this.m_playing)
            {
                bool paused = false;
                this.channel.getPaused(ref paused);
                this.m_paused = !paused;
                this.channel.setPaused(this.m_paused);
            }
        }

        private void pictureBoxPause_Click(object sender, EventArgs e)
        {
            this.PauseResume();
        }

        private void pictureBoxPlay_Click(object sender, EventArgs e)
        {
            if (this.m_paused)
            {
                this.PauseResume();
            }
            else if (!this.m_playing)
            {
                this.m_playing = true;
                RESULT result = this.system.playSound(CHANNELINDEX.FREE, this.sound, false, ref this.channel);
                this.ERRCHECK(result);
                this.timer.Start();
            }
            this.Refresh();
        }

        private void pictureBoxScaleDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.m_scaleFactor > 1f)
            {
                if (this.checkBoxLockSliders.Checked)
                {
                    this.ScaleSlidersTo(this.m_scaleFactor - 0.5f);
                }
                this.m_scaleFactor -= 0.5f;
                this.UpdateScaleLabel();
            }
        }

        private void pictureBoxScaleUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.checkBoxLockSliders.Checked)
            {
                this.ScaleSlidersTo(this.m_scaleFactor + 0.5f);
            }
            this.m_scaleFactor += 0.5f;
            this.UpdateScaleLabel();
        }

        private void pictureBoxStop_Click(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void ScaleSlidersTo(float newScaleFactor)
        {
            foreach (FrequencyBand band in this.m_bands)
            {
                int num = (band.Region.Bottom - band.MinSliderTop) - 4;
                band.MinSliderTop = (band.Region.Bottom - ((int) ((((float) num) / this.m_scaleFactor) * newScaleFactor))) - 4;
                num = band.Region.Bottom - band.MaxSliderTop;
                band.MaxSliderTop = band.Region.Bottom - ((int) ((((float) num) / this.m_scaleFactor) * newScaleFactor));
            }
            base.Invalidate(this.m_bandsBounds);
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

        private void SpectrumDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.m_playing)
            {
                this.Stop();
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
            this.m_playing = false;
            this.m_paused = false;
            this.timer.Stop();
            if (this.channel != null)
            {
                this.channel.stop();
                this.channel = null;
            }
            this.Refresh();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            base.Invalidate(this.m_bandsBounds);
            if (this.system != null)
            {
                this.system.update();
            }
            if (this.channel != null)
            {
                bool isplaying = false;
                this.channel.isPlaying(ref isplaying);
                if (!isplaying)
                {
                    this.Stop();
                }
            }
        }

        private void UpdateScaleLabel()
        {
            this.labelScaleFactor.Text = string.Format("Scale\nx {0:F1}", this.m_scaleFactor);
        }

        public bool LockSliders
        {
            get
            {
                return this.checkBoxLockSliders.Checked;
            }
            set
            {
                this.checkBoxLockSliders.Checked = value;
            }
        }

        public FrequencyBandMapping[] Mappings
        {
            get
            {
                List<FrequencyBandMapping> list = new List<FrequencyBandMapping>();
                foreach (FrequencyBand band in this.m_bands)
                {
                    FrequencyBandMapping item = new FrequencyBandMapping(band.ResponseLevelMin, band.ResponseLevelMax);
                    list.Add(item);
                    if (this.m_bandChannelConnections.ContainsKey(band))
                    {
                        item.ChannelList.AddRange(this.m_bandChannelConnections[band]);
                    }
                }
                return list.ToArray();
            }
            set
            {
                this.m_userSetResponseLevels = value.Length > 0;
                int num = Math.Min(this.m_bands.Count, value.Length);
                for (int i = 0; i < num; i++)
                {
                    List<int> list;
                    FrequencyBand key = this.m_bands[i];
                    FrequencyBandMapping mapping = value[i];
                    key.ResponseLevelMin = mapping.ResponseLevelMin;
                    key.ResponseLevelMax = mapping.ResponseLevelMax;
                    if (!this.m_bandChannelConnections.TryGetValue(key, out list))
                    {
                        list = new List<int>();
                        this.m_bandChannelConnections[key] = list;
                    }
                    list.AddRange(mapping.ChannelList);
                    foreach (int num3 in mapping.ChannelList)
                    {
                        this.m_channelBandConnections[num3] = key;
                    }
                }
            }
        }

        public float ScaleFactor
        {
            get
            {
                return this.m_scaleFactor;
            }
            set
            {
                this.m_scaleFactor = value;
                this.UpdateScaleLabel();
            }
        }
    }
}

