namespace Keyboard {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class SetupDialog : Form {
		private const int FADE_START = 500;
		private Pen m_blackPen;
		private Pen m_bluePen;
		private byte m_fadeColorLevel;
		private int m_fadeStartCount;
		private byte m_fadeStep;
		private Inputs m_inputs;
		private Dictionary<Keys, Rectangle> m_keyImageMap;
		private Dictionary<Keys, Inputs.InputType> m_keyInputs;
		private int m_pulseStepCount;
		private Pen m_redPen;
		private const int PULSE_DURATION = 0x3e8;

		public SetupDialog(Inputs inputs) {
			this.InitializeComponent();
			this.m_blackPen = new Pen(Color.Black, 2f);
			this.m_redPen = new Pen(Color.Red, 2f);
			this.m_bluePen = new Pen(Color.Blue, 2f);
			this.m_keyImageMap = new Dictionary<Keys, Rectangle>();
			this.m_keyInputs = new Dictionary<Keys, Inputs.InputType>();
			this.m_inputs = inputs;
			this.m_keyImageMap[Keys.Escape] = new Rectangle(13, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F1] = new Rectangle(0x4e, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F2] = new Rectangle(0x75, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F3] = new Rectangle(0x9a, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F4] = new Rectangle(0xc1, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F5] = new Rectangle(0xfe, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F6] = new Rectangle(0x124, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F7] = new Rectangle(330, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F8] = new Rectangle(0x170, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F9] = new Rectangle(0x1ad, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F10] = new Rectangle(0x1d3, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F11] = new Rectangle(0x1f9, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.F12] = new Rectangle(0x21f, 12, 0x20, 0x20);
			this.m_keyImageMap[Keys.Oemtilde] = new Rectangle(13, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.D1] = new Rectangle(0x33, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.D2] = new Rectangle(0x59, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.D3] = new Rectangle(0x7f, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.D4] = new Rectangle(0xa5, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.D5] = new Rectangle(0xcb, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.D6] = new Rectangle(0xf1, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.D7] = new Rectangle(0x117, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.D8] = new Rectangle(0x13e, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.D9] = new Rectangle(0x163, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.D0] = new Rectangle(0x189, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.OemMinus] = new Rectangle(0x1af, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.Oemplus] = new Rectangle(0x1d5, 0x54, 0x20, 0x20);
			this.m_keyImageMap[Keys.Back] = new Rectangle(0x1fb, 0x54, 0x44, 0x20);
			this.m_keyImageMap[Keys.Tab] = new Rectangle(13, 0x7c, 50, 0x20);
			this.m_keyImageMap[Keys.Q] = new Rectangle(0x45, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.W] = new Rectangle(0x6b, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.E] = new Rectangle(0x91, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.R] = new Rectangle(0xb7, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.T] = new Rectangle(0xdd, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.Y] = new Rectangle(0x102, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.U] = new Rectangle(0x129, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.I] = new Rectangle(0x14f, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.O] = new Rectangle(0x175, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.P] = new Rectangle(0x19b, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.OemOpenBrackets] = new Rectangle(0x1c1, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.OemCloseBrackets] = new Rectangle(0x1e7, 0x7c, 0x20, 0x20);
			this.m_keyImageMap[Keys.OemBackslash] = new Rectangle(0x20e, 0x7c, 50, 0x20);
			this.m_keyImageMap[Keys.Capital] = new Rectangle(13, 0xa3, 60, 0x20);
			this.m_keyImageMap[Keys.A] = new Rectangle(0x4f, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.S] = new Rectangle(0x75, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.D] = new Rectangle(0x9b, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.F] = new Rectangle(0xc2, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.G] = new Rectangle(0xe8, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.H] = new Rectangle(270, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.J] = new Rectangle(0x133, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.K] = new Rectangle(0x15a, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.L] = new Rectangle(0x180, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.OemSemicolon] = new Rectangle(0x1a6, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.OemQuotes] = new Rectangle(0x1cd, 0xa3, 0x20, 0x20);
			this.m_keyImageMap[Keys.Return] = new Rectangle(0x1f2, 0xa3, 0x4d, 0x20);
			this.m_keyImageMap[Keys.LShiftKey] = new Rectangle(13, 0xca, 0x57, 0x20);
			this.m_keyImageMap[Keys.Z] = new Rectangle(0x69, 0xca, 0x20, 0x20);
			this.m_keyImageMap[Keys.X] = new Rectangle(0x90, 0xca, 0x20, 0x20);
			this.m_keyImageMap[Keys.C] = new Rectangle(0xb6, 0xca, 0x20, 0x20);
			this.m_keyImageMap[Keys.V] = new Rectangle(220, 0xca, 0x20, 0x20);
			this.m_keyImageMap[Keys.B] = new Rectangle(0x103, 0xca, 0x20, 0x20);
			this.m_keyImageMap[Keys.N] = new Rectangle(0x129, 0xca, 0x20, 0x20);
			this.m_keyImageMap[Keys.M] = new Rectangle(0x14f, 0xca, 0x20, 0x20);
			this.m_keyImageMap[Keys.Oemcomma] = new Rectangle(0x175, 0xca, 0x20, 0x20);
			this.m_keyImageMap[Keys.OemPeriod] = new Rectangle(0x19c, 0xca, 0x20, 0x20);
			this.m_keyImageMap[Keys.Divide] = new Rectangle(450, 0xca, 0x20, 0x20);
			this.m_keyImageMap[Keys.RShiftKey] = new Rectangle(0x1e8, 0xca, 0x57, 0x20);
			this.m_keyImageMap[Keys.LControlKey] = new Rectangle(13, 0xf2, 50, 0x20);
			this.m_keyImageMap[Keys.Space] = new Rectangle(0xa8, 0xf2, 0xcb, 0x20);
			this.m_keyImageMap[Keys.RControlKey] = new Rectangle(0x20c, 0xf2, 50, 0x20);
			this.ReadSetupData();
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			this.WriteSetupData();
		}

		private void Cycle(Keys key) {
			Rectangle rectangle = this.m_keyImageMap[key];
			if (this.m_keyInputs.ContainsKey(key)) {
				if (((Inputs.InputType)this.m_keyInputs[key]) == Inputs.InputType.Latching) {
					this.m_keyInputs.Remove(key);
				}
				else {
					Dictionary<Keys, Inputs.InputType> dictionary;
					Keys keys;
					(dictionary = this.m_keyInputs)[keys = key] = ((Inputs.InputType)dictionary[keys]) + Inputs.InputType.First;
				}
			}
			else {
				this.m_keyInputs[key] = Inputs.InputType.First;
			}
			this.pictureBoxKeyboard.Invalidate(new Rectangle(rectangle.X - 1, rectangle.Y - 1, rectangle.Width + 2, rectangle.Height + 2));
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(SetupDialog));
		//this.pictureBoxKeyboard.Image = (Image)manager.GetObject("pictureBoxKeyboard.Image");

		private void pictureBox3_Paint(object sender, PaintEventArgs e) {
			e.Graphics.DrawRectangle(Pens.Black, this.pictureBox3.ClientRectangle.X, this.pictureBox3.ClientRectangle.Y, this.pictureBox3.ClientRectangle.Width - 1, this.pictureBox3.ClientRectangle.Height - 1);
		}

		private void pictureBoxKeyboard_Paint(object sender, PaintEventArgs e) {
			foreach (Keys keys in this.m_keyImageMap.Keys) {
				if (this.m_keyInputs.ContainsKey(keys)) {
					switch (this.m_keyInputs[keys]) {
						case Inputs.InputType.First:
							e.Graphics.DrawRectangle(this.m_redPen, this.m_keyImageMap[keys]);
							break;

						case Inputs.InputType.Latching:
							e.Graphics.DrawRectangle(this.m_bluePen, this.m_keyImageMap[keys]);
							break;
					}
				}
				else {
					e.Graphics.DrawRectangle(this.m_blackPen, this.m_keyImageMap[keys]);
				}
			}
		}

		private void PulseLabel() {
			this.m_pulseStepCount = 0x3e8 / this.timerPulse.Interval;
			this.m_fadeStartCount = 500 / this.timerPulse.Interval;
			this.m_fadeColorLevel = 0;
			this.m_fadeStep = (byte)(0xff / this.m_fadeStartCount);
			this.labelKeyNotAvailable.ForeColor = Color.Black;
			this.labelKeyNotAvailable.Visible = true;
			this.timerPulse.Enabled = true;
		}

		private void ReadSetupData() {
			this.m_keyInputs.Clear();
			foreach (KeyInput input in this.m_inputs.ReadAll()) {
				this.m_keyInputs[input.Key] = input.InputType;
			}
		}

		private void SetupDialog_KeyDown(object sender, KeyEventArgs e) {
			e.Handled = true;
			if (this.m_keyImageMap.ContainsKey(e.KeyCode)) {
				this.Cycle(e.KeyCode);
			}
			else {
				this.PulseLabel();
			}
		}

		private void timerPulse_Tick(object sender, EventArgs e) {
			if (--this.m_pulseStepCount == 0) {
				this.timerPulse.Enabled = false;
				this.labelKeyNotAvailable.Visible = false;
			}
			else if (this.m_pulseStepCount < this.m_fadeStartCount) {
				this.m_fadeColorLevel = (byte)(this.m_fadeColorLevel + this.m_fadeStep);
				this.labelKeyNotAvailable.ForeColor = Color.FromArgb(this.m_fadeColorLevel, this.m_fadeColorLevel, this.m_fadeColorLevel);
				this.labelKeyNotAvailable.Update();
			}
		}

		private void WriteSetupData() {
			this.m_inputs.Clear();
			foreach (Keys keys in this.m_keyInputs.Keys) {
				this.m_inputs.Write(keys, this.m_keyInputs[keys], false);
			}
		}
	}
}