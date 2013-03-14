namespace EZ_8_Transfer
{
    using EZ_8;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO.Ports;
    using System.Threading;
    using System.Windows.Forms;
    using Vixen;
    using Vixen.Dialogs;

    internal class EZ8_UI : Form
    {
        private System.Windows.Forms.Timer animationTimer;
        private Button buttonClose;
        private Button buttonPort;
        private Button buttonTransfer;
        private CheckBox checkBoxCodeProtect;
        private IContainer components;
        private System.Windows.Forms.Timer connectivityTimer;
        private const float CRUISING_PERCENTAGE = 0.8f;
        private const int CRUISING_SPEED = 15;
        private GroupBox groupBox1;
        private GroupBox groupBoxTransfer;
        private Label label1;
        private Label labelCodeProtect;
        private Label labelErasing;
        private Label labelPort;
        private Label labelRevision;
        private Label labelState;
        private AnimationState m_animationState;
        private bool m_codeProtect;
        private bool m_connected;
        private int m_cruisingDuration;
        private bool m_erasing;
        private EZ_8.EZ_8 m_hardware;
        private int m_leftOriginalPosition;
        private PictureBox m_leftPicture;
        private int m_progress;
        private int m_rightOriginalPosition;
        private PictureBox m_rightPicture;
        private EventSequence m_sequence;
        private bool m_sequenceModified;
        private int m_stage1Duration;
        private int m_stage1Velocity;
        private int m_stage2Duration;
        private int m_stage2Velocity;
        private int m_stage3Duration;
        private int m_stage3Velocity;
        private int m_stateCountdown;
        private TransferDirection m_transferDirection;
        private Panel panel1;
        private PictureBox pictureBoxArrow;
        private PictureBox pictureBoxComputer;
        private PictureBox pictureBoxDevice;
        private RadioButton radioButtonFromDevice;
        private RadioButton radioButtonToDevice;
        private const float STAGE1_PROPORTION = 0.3f;
        private const float STAGE1_VELOCITY_FACTOR = 0.3f;
        private const float STAGE2_PROPORTION = 0.3f;
        private const float STAGE2_VELOCITY_FACTOR = 0.6f;
        private const float STAGE3_PROPORTION = 0.4f;
        private const float STAGE3_VELOCITY_FACTOR = 0.8f;
        private const int VELOCITY_CHANGE_TIME = 500;

        public EZ8_UI(EZ_8.EZ_8 hardware)
        {
            int left;
            int num2;
            this.m_progress = 100;
            this.m_connected = false;
            this.m_codeProtect = false;
            this.m_sequenceModified = false;
            this.m_erasing = false;
            this.m_animationState = AnimationState.Stopped;
            this.m_transferDirection = TransferDirection.Upload;
            this.components = null;
            this.InitializeComponent();
            this.m_hardware = hardware;
            this.SetPort(this.m_hardware.PortName);
            this.m_hardware.DeviceError += new EZ_8.EZ_8.StringEventHandler(this.EZ_8_Error);
            this.m_hardware.TransferStart += new EZ_8.EZ_8.VoidEventHandler(this.EZ_8_TransferStart);
            this.m_hardware.TransferProgress += new EZ_8.EZ_8.IntEventHandler(this.EZ_8_TransferProgress);
            this.m_hardware.TransferEnd += new EZ_8.EZ_8.DataEventHandler(this.EZ_8_TransferEnd);
            this.m_hardware.EraseStart += new EZ_8.EZ_8.VoidEventHandler(this.m_hardware_EraseStart);
            this.m_hardware.EraseEnd += new EZ_8.EZ_8.VoidEventHandler(this.m_hardware_EraseEnd);
            if (this.pictureBoxDevice.Left < this.pictureBoxComputer.Left)
            {
                this.m_leftOriginalPosition = this.pictureBoxDevice.Left;
                this.m_rightOriginalPosition = this.pictureBoxComputer.Left;
                left = this.pictureBoxDevice.Left;
                num2 = this.pictureBoxComputer.Left;
            }
            else
            {
                this.m_leftOriginalPosition = this.pictureBoxComputer.Left;
                this.m_rightOriginalPosition = this.pictureBoxDevice.Left;
                left = this.pictureBoxComputer.Left;
                num2 = this.pictureBoxDevice.Left;
            }
            this.m_cruisingDuration = (int) (((num2 - left) * 0.8f) / 15f);
            this.m_stage1Velocity = 4;
            this.m_stage2Velocity = 9;
            this.m_stage3Velocity = 12;
            int num3 = (int) (((num2 - left) * 0.2f) / 2f);
            int num4 = (int) (num3 * 0.3f);
            int num5 = (int) (num3 * 0.3f);
            int num6 = (int) (num3 * 0.4f);
            this.m_stage1Duration = (num4 / this.m_stage1Velocity) + 1;
            this.m_stage2Duration = (num5 / this.m_stage2Velocity) + 1;
            this.m_stage3Duration = (num6 / this.m_stage3Velocity) + 1;
            this.UpdatePendingCodeProtect(this.checkBoxCodeProtect.Checked);
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            switch (this.m_animationState)
            {
                case AnimationState.Stopped:
                    this.animationTimer.Enabled = false;
                    this.pictureBoxArrow.Visible = true;
                    break;

                case AnimationState.Starting:
                    this.pictureBoxArrow.Visible = false;
                    this.m_stateCountdown = this.m_stage1Duration;
                    this.m_animationState = AnimationState.AcceleratingStage1;
                    break;

                case AnimationState.AcceleratingStage1:
                    this.m_leftPicture.Left += this.m_stage1Velocity;
                    this.m_rightPicture.Left -= this.m_stage1Velocity;
                    if (--this.m_stateCountdown == 0)
                    {
                        this.m_stateCountdown = this.m_stage2Duration;
                        this.m_animationState = AnimationState.AcceleratingStage2;
                    }
                    break;

                case AnimationState.AcceleratingStage2:
                    this.m_leftPicture.Left += this.m_stage2Velocity;
                    this.m_rightPicture.Left -= this.m_stage2Velocity;
                    if (--this.m_stateCountdown == 0)
                    {
                        this.m_stateCountdown = this.m_stage3Duration;
                        this.m_animationState = AnimationState.AcceleratingStage3;
                    }
                    break;

                case AnimationState.AcceleratingStage3:
                    this.m_leftPicture.Left += this.m_stage3Velocity;
                    this.m_rightPicture.Left -= this.m_stage3Velocity;
                    if (--this.m_stateCountdown == 0)
                    {
                        this.m_stateCountdown = this.m_cruisingDuration;
                        this.m_animationState = AnimationState.Cruising;
                    }
                    break;

                case AnimationState.Cruising:
                    this.m_leftPicture.Left += 15;
                    this.m_rightPicture.Left -= 15;
                    if (--this.m_stateCountdown == 0)
                    {
                        this.m_stateCountdown = this.m_stage3Duration;
                        this.m_animationState = AnimationState.DeceleratingStage3;
                    }
                    break;

                case AnimationState.DeceleratingStage1:
                    this.m_leftPicture.Left += this.m_stage1Velocity;
                    this.m_rightPicture.Left -= this.m_stage1Velocity;
                    if (--this.m_stateCountdown == 0)
                    {
                        this.m_animationState = AnimationState.Stopping;
                    }
                    break;

                case AnimationState.DeceleratingStage2:
                    this.m_leftPicture.Left += this.m_stage2Velocity;
                    this.m_rightPicture.Left -= this.m_stage2Velocity;
                    if (--this.m_stateCountdown == 0)
                    {
                        this.m_stateCountdown = this.m_stage1Duration;
                        this.m_animationState = AnimationState.DeceleratingStage1;
                    }
                    break;

                case AnimationState.DeceleratingStage3:
                    this.m_leftPicture.Left += this.m_stage3Velocity;
                    this.m_rightPicture.Left -= this.m_stage3Velocity;
                    if (--this.m_stateCountdown == 0)
                    {
                        this.m_stateCountdown = this.m_stage2Duration;
                        this.m_animationState = AnimationState.DeceleratingStage2;
                    }
                    break;

                case AnimationState.Stopping:
                    this.m_leftPicture.Left = this.m_rightOriginalPosition;
                    this.m_rightPicture.Left = this.m_leftOriginalPosition;
                    this.m_animationState = AnimationState.Stopped;
                    break;
            }
        }

        private void buttonPort_Click(object sender, EventArgs e)
        {
            this.connectivityTimer.Enabled = false;
            SerialPort serialPort = new SerialPort((this.m_hardware.PortName == null) ? SerialPort.GetPortNames()[0] : this.m_hardware.PortName, 0x9600);
            SerialSetupDialog dialog = new SerialSetupDialog(serialPort, true, false, false, false, false);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetPort(dialog.SelectedPort.PortName);
            }
            dialog.Dispose();
        }

        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            if (this.radioButtonFromDevice.Checked)
            {
                this.m_hardware.Upload();
            }
            else
            {
                int endOfShow;
                this.m_hardware.Configuration.EndOfShow = 0;
                if (this.m_sequence.ChannelCount > 8)
                {
                    endOfShow = this.m_sequence.TotalEventPeriods;
                    do
                    {
                        endOfShow--;
                    }
                    while ((this.m_sequence.EventValues[8, endOfShow] == 0) && (endOfShow > 0));
                    this.m_hardware.Configuration.EndOfShow = (ushort) endOfShow;
                }
                int channelNumber = 0;
                while (channelNumber < 8)
                {
                    endOfShow = this.m_sequence.TotalEventPeriods;
                    do
                    {
                        endOfShow--;
                    }
                    while ((this.m_sequence.EventValues[channelNumber, endOfShow] == 0) && (endOfShow > 0));
                    this.m_hardware.Configuration.SetEndOfChannel(channelNumber, (ushort) endOfShow);
                    channelNumber++;
                }
                endOfShow = this.m_hardware.Configuration.EndOfShow;
                byte[] values = new byte[endOfShow + 1];
                for (int i = 0; i <= endOfShow; i++)
                {
                    byte num2 = 0;
                    for (channelNumber = 7; channelNumber >= 0; channelNumber--)
                    {
                        num2 = (byte) (num2 << 1);
                        num2 = (byte) (num2 | ((this.m_sequence.EventValues[channelNumber, i] > 0x80) ? 1 : 0));
                    }
                    values[i] = num2;
                }
                this.m_hardware.EraseAll();
                while (this.m_erasing)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
                this.m_hardware.Download(values);
            }
        }

        private void checkBoxCodeProtect_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdatePendingCodeProtect(this.checkBoxCodeProtect.Checked);
        }

        private void connectivityTimer_Tick(object sender, EventArgs e)
        {
            if (this.m_hardware.IsAvailable)
            {
                string str;
                this.Revision = str = this.m_hardware.GetRevision();
                this.Connected = str != null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EZ_8_Error(string value)
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.EZ_8_Error(value);
                    };
                }
                base.Invoke(method);
            }
            else
            {
                MessageBox.Show(value, "EZ-8", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void EZ_8_TransferEnd(byte[] data)
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.EZ_8_TransferEnd(data);
                    };
                }
                base.Invoke(method);
            }
            else
            {
                this.m_progress = 100;
                this.pictureBoxArrow.Refresh();
                if ((this.m_transferDirection == TransferDirection.Upload) && (data != null))
                {
                    int num3;
                    if (this.m_sequence.Time < (data.Length * 0x21))
                    {
                        this.m_sequence.Time = data.Length * 0x21;
                        MessageBox.Show("Sequence length has been updated to accommodate the data length.", "EZ-8", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    for (num3 = 0; num3 < data.Length; num3++)
                    {
                        byte num2 = data[num3];
                        for (int i = 0; i < 8; i++)
                        {
                            this.m_sequence.EventValues[i, num3] = ((num2 & (((int) 1) << i)) > 0) ? this.m_sequence.MaximumLevel : this.m_sequence.MinimumLevel;
                        }
                    }
                    if (this.m_sequence.ChannelCount > 8)
                    {
                        for (num3 = 0; num3 < this.m_sequence.TotalEventPeriods; num3++)
                        {
                            this.m_sequence.EventValues[8, num3] = this.m_sequence.MinimumLevel;
                        }
                        this.m_sequence.EventValues[8, this.m_hardware.Configuration.EndOfShow] = this.m_sequence.MaximumLevel;
                    }
                    this.m_sequenceModified = true;
                }
                this.Cursor = Cursors.Default;
                this.SetUI(true);
                MessageBox.Show("Transfer complete", "EZ-8", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void EZ_8_TransferProgress(int value)
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.EZ_8_TransferProgress(value);
                    };
                }
                base.Invoke(method);
            }
            else
            {
                this.m_progress = value;
                this.pictureBoxArrow.Refresh();
            }
        }

        private void EZ_8_TransferStart()
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.EZ_8_TransferStart();
                    };
                }
                base.Invoke(method);
            }
            else
            {
                this.m_progress = 0;
                this.pictureBoxArrow.Refresh();
                this.SetUI(false);
                this.Cursor = Cursors.WaitCursor;
            }
        }

        private void EZ8_UI_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void EZ8_UI_VisibleChanged(object sender, EventArgs e)
        {
            if (base.Visible)
            {
                this.m_hardware.Open();
            }
            else
            {
                this.m_hardware.Close();
            }
            this.connectivityTimer.Enabled = this.m_hardware.IsOpen;
        }

        private void FromDeviceAnimation()
        {
            this.m_animationState = AnimationState.Starting;
            this.m_leftPicture = this.pictureBoxComputer;
            this.m_rightPicture = this.pictureBoxDevice;
            this.animationTimer.Enabled = true;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(EZ8_UI));
            this.buttonPort = new Button();
            this.labelPort = new Label();
            this.connectivityTimer = new System.Windows.Forms.Timer(this.components);
            this.labelState = new Label();
            this.panel1 = new Panel();
            this.pictureBoxComputer = new PictureBox();
            this.pictureBoxArrow = new PictureBox();
            this.pictureBoxDevice = new PictureBox();
            this.groupBox1 = new GroupBox();
            this.labelRevision = new Label();
            this.groupBoxTransfer = new GroupBox();
            this.labelErasing = new Label();
            this.checkBoxCodeProtect = new CheckBox();
            this.labelCodeProtect = new Label();
            this.buttonTransfer = new Button();
            this.radioButtonToDevice = new RadioButton();
            this.radioButtonFromDevice = new RadioButton();
            this.label1 = new Label();
            this.animationTimer = new System.Windows.Forms.Timer(this.components);
            this.buttonClose = new Button();
            this.panel1.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxComputer).BeginInit();
            ((ISupportInitialize) this.pictureBoxArrow).BeginInit();
            ((ISupportInitialize) this.pictureBoxDevice).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBoxTransfer.SuspendLayout();
            base.SuspendLayout();
            this.buttonPort.Location = new Point(15, 0x1d);
            this.buttonPort.Name = "buttonPort";
            this.buttonPort.Size = new Size(0x4b, 0x17);
            this.buttonPort.TabIndex = 0;
            this.buttonPort.Text = "Port";
            this.buttonPort.UseVisualStyleBackColor = true;
            this.buttonPort.Click += new EventHandler(this.buttonPort_Click);
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new Point(0x60, 0x22);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new Size(0x43, 13);
            this.labelPort.TabIndex = 1;
            this.labelPort.Text = "Not selected";
            this.connectivityTimer.Interval = 0x7d0;
            this.connectivityTimer.Tick += new EventHandler(this.connectivityTimer_Tick);
            this.labelState.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.labelState.ForeColor = Color.Red;
            this.labelState.Location = new Point(0x107, 0x22);
            this.labelState.Name = "labelState";
            this.labelState.Size = new Size(0x80, 13);
            this.labelState.TabIndex = 2;
            this.labelState.Text = "Disconnected";
            this.labelState.TextAlign = ContentAlignment.TopRight;
            this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.panel1.BackColor = Color.White;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBoxComputer);
            this.panel1.Controls.Add(this.pictureBoxArrow);
            this.panel1.Controls.Add(this.pictureBoxDevice);
            this.panel1.Location = new Point(14, 180);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x178, 0x4e);
            this.panel1.TabIndex = 6;
            this.pictureBoxComputer.Image = (Image) manager.GetObject("pictureBoxComputer.Image");
            this.pictureBoxComputer.Location = new Point(310, 14);
            this.pictureBoxComputer.Name = "pictureBoxComputer";
            this.pictureBoxComputer.Size = new Size(0x30, 0x30);
            this.pictureBoxComputer.TabIndex = 2;
            this.pictureBoxComputer.TabStop = false;
            this.pictureBoxArrow.Image = (Image) manager.GetObject("pictureBoxArrow.Image");
            this.pictureBoxArrow.Location = new Point(0x67, 0x17);
            this.pictureBoxArrow.Name = "pictureBoxArrow";
            this.pictureBoxArrow.Size = new Size(0xa8, 30);
            this.pictureBoxArrow.TabIndex = 1;
            this.pictureBoxArrow.TabStop = false;
            this.pictureBoxArrow.Paint += new PaintEventHandler(this.pictureBoxArrow_Paint);
            this.pictureBoxDevice.Image = (Image) manager.GetObject("pictureBoxDevice.Image");
            this.pictureBoxDevice.Location = new Point(15, 14);
            this.pictureBoxDevice.Name = "pictureBoxDevice";
            this.pictureBoxDevice.Size = new Size(0x30, 0x30);
            this.pictureBoxDevice.TabIndex = 0;
            this.pictureBoxDevice.TabStop = false;
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.labelRevision);
            this.groupBox1.Controls.Add(this.buttonPort);
            this.groupBox1.Controls.Add(this.labelPort);
            this.groupBox1.Controls.Add(this.labelState);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x194, 0x4d);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connectivity";
            this.labelRevision.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.labelRevision.Location = new Point(260, 0x35);
            this.labelRevision.Name = "labelRevision";
            this.labelRevision.Size = new Size(0x83, 13);
            this.labelRevision.TabIndex = 3;
            this.labelRevision.TextAlign = ContentAlignment.TopRight;
            this.groupBoxTransfer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBoxTransfer.Controls.Add(this.labelErasing);
            this.groupBoxTransfer.Controls.Add(this.checkBoxCodeProtect);
            this.groupBoxTransfer.Controls.Add(this.labelCodeProtect);
            this.groupBoxTransfer.Controls.Add(this.buttonTransfer);
            this.groupBoxTransfer.Controls.Add(this.radioButtonToDevice);
            this.groupBoxTransfer.Controls.Add(this.radioButtonFromDevice);
            this.groupBoxTransfer.Controls.Add(this.label1);
            this.groupBoxTransfer.Controls.Add(this.panel1);
            this.groupBoxTransfer.Enabled = false;
            this.groupBoxTransfer.Location = new Point(13, 0x5f);
            this.groupBoxTransfer.Name = "groupBoxTransfer";
            this.groupBoxTransfer.Size = new Size(0x193, 0x108);
            this.groupBoxTransfer.TabIndex = 1;
            this.groupBoxTransfer.TabStop = false;
            this.groupBoxTransfer.Text = "Upload/Download";
            this.labelErasing.AutoSize = true;
            this.labelErasing.ForeColor = Color.Blue;
            this.labelErasing.Location = new Point(0x11, 0x9d);
            this.labelErasing.Name = "labelErasing";
            this.labelErasing.Size = new Size(0x33, 13);
            this.labelErasing.TabIndex = 7;
            this.labelErasing.Text = "Erasing...";
            this.labelErasing.Visible = false;
            this.checkBoxCodeProtect.AutoSize = true;
            this.checkBoxCodeProtect.Enabled = false;
            this.checkBoxCodeProtect.Location = new Point(0xb5, 0x4c);
            this.checkBoxCodeProtect.Name = "checkBoxCodeProtect";
            this.checkBoxCodeProtect.Size = new Size(140, 0x11);
            this.checkBoxCodeProtect.TabIndex = 3;
            this.checkBoxCodeProtect.Text = "Turn on code protection";
            this.checkBoxCodeProtect.UseVisualStyleBackColor = true;
            this.checkBoxCodeProtect.CheckedChanged += new EventHandler(this.checkBoxCodeProtect_CheckedChanged);
            this.labelCodeProtect.AutoSize = true;
            this.labelCodeProtect.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelCodeProtect.ForeColor = Color.Red;
            this.labelCodeProtect.Location = new Point(0x7a, 0x93);
            this.labelCodeProtect.Name = "labelCodeProtect";
            this.labelCodeProtect.Size = new Size(0x9f, 0x10);
            this.labelCodeProtect.TabIndex = 5;
            this.labelCodeProtect.Text = "CODE PROTECT IS ON";
            this.labelCodeProtect.Visible = false;
            this.buttonTransfer.Location = new Point(0xa4, 0x79);
            this.buttonTransfer.Name = "buttonTransfer";
            this.buttonTransfer.Size = new Size(0x4b, 0x17);
            this.buttonTransfer.TabIndex = 4;
            this.buttonTransfer.Text = "Start";
            this.buttonTransfer.UseVisualStyleBackColor = true;
            this.buttonTransfer.Click += new EventHandler(this.buttonTransfer_Click);
            this.radioButtonToDevice.AutoSize = true;
            this.radioButtonToDevice.Location = new Point(0xa2, 0x39);
            this.radioButtonToDevice.Name = "radioButtonToDevice";
            this.radioButtonToDevice.Size = new Size(0xb5, 0x11);
            this.radioButtonToDevice.TabIndex = 2;
            this.radioButtonToDevice.Text = "From the sequence to the device";
            this.radioButtonToDevice.UseVisualStyleBackColor = true;
            this.radioButtonToDevice.CheckedChanged += new EventHandler(this.radioButtonToDevice_CheckedChanged);
            this.radioButtonFromDevice.AutoSize = true;
            this.radioButtonFromDevice.Checked = true;
            this.radioButtonFromDevice.Location = new Point(0xa2, 0x22);
            this.radioButtonFromDevice.Name = "radioButtonFromDevice";
            this.radioButtonFromDevice.Size = new Size(0xb5, 0x11);
            this.radioButtonFromDevice.TabIndex = 1;
            this.radioButtonFromDevice.TabStop = true;
            this.radioButtonFromDevice.Text = "From the device to the sequence";
            this.radioButtonFromDevice.UseVisualStyleBackColor = true;
            this.radioButtonFromDevice.CheckedChanged += new EventHandler(this.radioButtonFromDevice_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x1b, 0x24);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "I want to transfer data:";
            this.animationTimer.Interval = 50;
            this.animationTimer.Tick += new EventHandler(this.animationTimer_Tick);
            this.buttonClose.DialogResult = DialogResult.OK;
            this.buttonClose.Location = new Point(0x155, 0x16d);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new Size(0x4b, 0x17);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1ac, 400);
            base.Controls.Add(this.buttonClose);
            base.Controls.Add(this.groupBoxTransfer);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "EZ8_UI";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "EZ-8 Upload/Download";
            base.VisibleChanged += new EventHandler(this.EZ8_UI_VisibleChanged);
            base.FormClosing += new FormClosingEventHandler(this.EZ8_UI_FormClosing);
            this.panel1.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxComputer).EndInit();
            ((ISupportInitialize) this.pictureBoxArrow).EndInit();
            ((ISupportInitialize) this.pictureBoxDevice).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxTransfer.ResumeLayout(false);
            this.groupBoxTransfer.PerformLayout();
            base.ResumeLayout(false);
        }

        private void m_hardware_EraseEnd()
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.m_hardware_EraseEnd();
                    };
                }
                base.Invoke(method);
            }
            else
            {
                this.labelErasing.Visible = false;
                this.labelErasing.Refresh();
                this.Cursor = Cursors.Default;
                this.SetUI(true);
                this.m_erasing = false;
            }
        }

        private void m_hardware_EraseStart()
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.m_hardware_EraseStart();
                    };
                }
                base.Invoke(method);
            }
            else
            {
                this.m_erasing = true;
                this.SetUI(false);
                this.Cursor = Cursors.WaitCursor;
                this.labelErasing.Visible = true;
                this.labelErasing.Refresh();
            }
        }

        private void pictureBoxArrow_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, (this.m_progress * this.pictureBoxArrow.Width) / 100, 0, this.pictureBoxArrow.Width, this.pictureBoxArrow.Height);
        }

        private void radioButtonFromDevice_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonFromDevice.Checked)
            {
                this.UpdateTransferState();
                this.FromDeviceAnimation();
            }
        }

        private void radioButtonToDevice_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBoxCodeProtect.Enabled = this.radioButtonToDevice.Checked;
            if (this.radioButtonToDevice.Checked)
            {
                this.UpdateTransferState();
                this.ToDeviceAnimation();
            }
        }

        private void SetPort(string portName)
        {
            if (portName != null)
            {
                this.m_hardware.PortName = portName;
                this.labelPort.Text = portName;
            }
            else
            {
                if (this.m_hardware.IsOpen)
                {
                    this.m_hardware.Close();
                }
                this.m_hardware.PortName = null;
                this.connectivityTimer.Enabled = false;
                this.labelPort.Text = "Not selected";
            }
        }

        private void SetUI(bool state)
        {
            this.buttonTransfer.Enabled = this.buttonPort.Enabled = this.radioButtonFromDevice.Enabled = this.radioButtonToDevice.Enabled = state;
        }

        private void ToDeviceAnimation()
        {
            this.m_animationState = AnimationState.Starting;
            this.m_leftPicture = this.pictureBoxDevice;
            this.m_rightPicture = this.pictureBoxComputer;
            this.animationTimer.Enabled = true;
        }

        private void UpdatePendingCodeProtect(bool state)
        {
            this.m_hardware.SetCodeProtect(state);
        }

        private void UpdateTransferState()
        {
            this.buttonTransfer.Enabled = !this.CodeProtect || this.radioButtonToDevice.Checked;
            this.m_transferDirection = this.radioButtonToDevice.Checked ? TransferDirection.Download : TransferDirection.Upload;
        }

        private bool CodeProtect
        {
            get
            {
                return this.m_codeProtect;
            }
            set
            {
                if (this.m_codeProtect != value)
                {
                    this.m_codeProtect = value;
                    this.labelCodeProtect.Visible = value;
                    this.UpdateTransferState();
                }
            }
        }

        private bool Connected
        {
            get
            {
                return this.m_connected;
            }
            set
            {
                if (value != this.m_connected)
                {
                    this.m_connected = value;
                    if (value)
                    {
                        this.labelState.ForeColor = Color.Green;
                        this.labelState.Text = "Connected";
                    }
                    else
                    {
                        this.labelState.ForeColor = Color.Red;
                        this.labelState.Text = "Disconnected";
                    }
                    this.groupBoxTransfer.Enabled = this.m_connected;
                }
            }
        }

        private string Revision
        {
            set
            {
                this.CodeProtect = this.m_hardware.Configuration.CodeProtect;
                if (value != null)
                {
                    this.labelRevision.Text = value;
                }
                else
                {
                    this.labelRevision.Text = "";
                }
            }
        }

        public EventSequence Sequence
        {
            set
            {
                this.m_sequence = value;
            }
        }

        public bool SequenceModified
        {
            get
            {
                return this.m_sequenceModified;
            }
        }

        private enum AnimationState
        {
            Stopped,
            Starting,
            AcceleratingStage1,
            AcceleratingStage2,
            AcceleratingStage3,
            Cruising,
            DeceleratingStage1,
            DeceleratingStage2,
            DeceleratingStage3,
            Stopping
        }

        private enum TransferDirection
        {
            None,
            Upload,
            Download
        }
    }
}

