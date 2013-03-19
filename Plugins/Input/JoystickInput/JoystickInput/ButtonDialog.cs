namespace JoystickInput {
	using JoystickManager;
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class ButtonDialog : Form {

		public ButtonDialog() {
			this.InitializeComponent();
			foreach (Joystick joystick in JoystickManager.Joysticks) {
				ListViewGroup group = this.listViewJoystickButtons.Groups.Add(joystick.Name, joystick.Name);
				foreach (ButtonResource resource in joystick.Buttons) {
					this.listViewJoystickButtons.Items.Add(new ListViewItem(resource.Name, group));
				}
			}
		}

		private void ButtonDialog_FormClosing(object sender, FormClosingEventArgs e) {
			this.timerPoll.Enabled = false;
			JoystickManager.ReleaseAll();
		}

		private void ButtonDialog_Load(object sender, EventArgs e) {
			JoystickManager.AcquireAll();
			this.timerPoll.Enabled = true;
		}

		private void timerPoll_Tick(object sender, EventArgs e) {
			int num = 0;
			foreach (Joystick joystick in JoystickManager.Joysticks) {
				joystick.Poll();
				foreach (ButtonResource resource in joystick.Buttons) {
					this.listViewJoystickButtons.Items[num].Selected = resource.Pressed;
					num++;
				}
			}
		}
	}
}