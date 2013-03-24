using EZ_8;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using VixenPlus;
using VixenPlus.Dialogs;

namespace EZ_8_Transfer
{
	internal partial class EZ8_UI : Form
	{
		private const float CRUISING_PERCENTAGE = 0.8f;
		private const int CRUISING_SPEED = 15;
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

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(EZ8_UI));
		//this.pictureBoxComputer.Image = (Image)manager.GetObject("pictureBoxComputer.Image");
		//this.pictureBoxArrow.Image = (Image)manager.GetObject("pictureBoxArrow.Image");
		//this.pictureBoxDevice.Image = (Image)manager.GetObject("pictureBoxDevice.Image");


		

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

