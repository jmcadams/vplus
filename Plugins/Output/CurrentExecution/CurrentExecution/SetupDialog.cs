namespace CurrentExecution {
	using LedTriksUtil;
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using VixenPlus;
	using VixenPlus.Dialogs;

	public partial class SetupDialog : Form {
		private Size m_boardLayout;
		private int m_dotPitch;
		private Generator m_generator;
		private Color m_ledColor;
		private int m_ledSize;
		private int m_portAddress;
		private XmlNode m_setupNode;

		public SetupDialog(XmlNode setupNode) {
			this.InitializeComponent();
			try {
				this.m_setupNode = setupNode;
				this.m_generator = new Generator();
				this.m_boardLayout = new Size(1, 1);
				XmlNode nodeAlways = Xml.GetNodeAlways(this.m_setupNode, "Boards");
				if (nodeAlways.Attributes["width"] != null) {
					this.m_boardLayout.Width = int.Parse(nodeAlways.Attributes["width"].Value);
				}
				if (nodeAlways.Attributes["height"] != null) {
					this.m_boardLayout.Height = int.Parse(nodeAlways.Attributes["height"].Value);
				}
				this.textBoxMessage.Text = Xml.GetNodeAlways(this.m_setupNode, "Message", "[NAME]").InnerText;
				this.m_portAddress = int.Parse(Xml.GetNodeAlways(this.m_setupNode, "Address", "888").InnerText);
				XmlNode parentNode = this.m_setupNode["TextOptions"];
				if (parentNode != null) {
					this.m_generator.LoadFromXml(parentNode);
				}
				XmlNode contextNode = Xml.GetNodeAlways(this.m_setupNode, "Virtual");
				if (contextNode.Attributes["enabled"] != null) {
					this.SetVirtual(bool.Parse(contextNode.Attributes["enabled"].Value));
				}
				else {
					this.SetVirtual(false);
				}
				if (this.checkBoxVirtualHardware.Checked) {
					this.m_ledSize = int.Parse(Xml.GetNodeAlways(contextNode, "LEDSize", "3").InnerText);
					this.m_ledColor = Color.FromArgb(int.Parse(Xml.GetNodeAlways(contextNode, "LEDColor", "-65536").InnerText));
					this.m_dotPitch = int.Parse(Xml.GetNodeAlways(contextNode, "DotPitch", "9").InnerText);
				}
				else {
					this.m_ledSize = 3;
					this.m_ledColor = Color.Red;
					this.m_dotPitch = 9;
				}
			}
			catch (Exception exception) {
				MessageBox.Show(exception.Message + "\n" + exception.StackTrace);
				throw;
			}
		}

		private void buttonBoardLayout_Click(object sender, EventArgs e) {
			BoardLayoutDialog dialog = new BoardLayoutDialog(this.m_boardLayout);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_boardLayout = dialog.BoardLayout;
			}
			dialog.Dispose();
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			XmlNode nodeAlways = Xml.GetNodeAlways(this.m_setupNode, "Boards");
			Xml.SetAttribute(nodeAlways, "width", this.m_boardLayout.Width.ToString());
			Xml.SetAttribute(nodeAlways, "height", this.m_boardLayout.Height.ToString());
			Xml.SetValue(this.m_setupNode, "Address", this.m_portAddress.ToString());
			Xml.SetValue(this.m_setupNode, "Message", this.textBoxMessage.Text);
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_setupNode, "TextOptions");
			this.m_generator.SaveToXml(emptyNodeAlways);
			XmlNode contextNode = Xml.SetAttribute(this.m_setupNode, "Virtual", "enabled", this.checkBoxVirtualHardware.Checked.ToString());
			if (this.checkBoxVirtualHardware.Checked) {
				Xml.SetValue(contextNode, "LEDSize", this.m_ledSize.ToString());
				Xml.SetValue(contextNode, "LEDColor", this.m_ledColor.ToArgb().ToString());
				Xml.SetValue(contextNode, "DotPitch", this.m_dotPitch.ToString());
			}
		}

		private void buttonPortSetup_Click(object sender, EventArgs e) {
			ParallelSetupDialog dialog = new ParallelSetupDialog(this.m_portAddress);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_portAddress = dialog.PortAddress;
			}
			dialog.Dispose();
		}

		private void buttonTextOptions_Click(object sender, EventArgs e) {
			TextOptionsDialog dialog = new TextOptionsDialog(this.m_generator);
			if (dialog.ShowDialog() == DialogResult.OK) {
			}
			dialog.Dispose();
		}

		private void buttonVirtualDisplaySetup_Click(object sender, EventArgs e) {
			VirtualHardwareSetupDialog dialog = new VirtualHardwareSetupDialog(false);
			dialog.LEDSize = this.m_ledSize;
			dialog.LEDColor = this.m_ledColor;
			dialog.DotPitch = this.m_dotPitch;
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_ledSize = dialog.LEDSize;
				this.m_ledColor = dialog.LEDColor;
				this.m_dotPitch = dialog.DotPitch;
			}
			dialog.Dispose();
		}

		private void checkBoxVirtualHardware_CheckedChanged(object sender, EventArgs e) {
			this.SetVirtual(this.checkBoxVirtualHardware.Checked);
		}

		private void SetupDialog_FormClosing(object sender, FormClosingEventArgs e) {
			this.m_generator.Dispose();
		}

		private void SetVirtual(bool virtualEnabled) {
			this.labelPortAddress.Enabled = this.buttonPortSetup.Enabled = !virtualEnabled;
			this.buttonVirtualDisplaySetup.Enabled = virtualEnabled;
			this.checkBoxVirtualHardware.Checked = virtualEnabled;
		}

		private void textBoxMessage_Validating(object sender, CancelEventArgs e) {
			if (this.textBoxMessage.Text.Trim().Length == 0) {
				this.textBoxMessage.Text = "[NAME]";
			}
			else if (!this.textBoxMessage.Text.Contains("[NAME]")) {
				e.Cancel = true;
				MessageBox.Show(string.Format("The message needs to contain {0} to know where to put the sequence name in the message.", "[NAME]"), "Current Execution", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}
}