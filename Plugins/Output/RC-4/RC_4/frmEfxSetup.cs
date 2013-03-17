namespace RC_4 {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using Vixen;

	public partial class frmEfxSetup : Form {

		private XmlNode m_setupNode;

		public frmEfxSetup(XmlNode setupNode, bool disableThreshold) {
			this.InitializeComponent();
			this.m_setupNode = setupNode;
			string innerText = Xml.GetNodeAlways(this.m_setupNode, "name").InnerText;
			this.nudPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
			innerText = Xml.GetNodeAlways(this.m_setupNode, "baud").InnerText;
			this.cboBaud.SelectedIndex = this.cboBaud.Items.IndexOf(innerText);
			innerText = Xml.GetNodeAlways(this.m_setupNode, "addr").InnerText;
			this.cboAddress.SelectedIndex = int.Parse(innerText);
			if (!disableThreshold) {
				innerText = Xml.GetNodeAlways(this.m_setupNode, "threshold").InnerText;
				this.nudThreshold.Value = int.Parse(innerText);
			}
			else {
				this.grpThresh.Enabled = false;
			}
		}

		private void btnOK_Click(object sender, EventArgs e) {
			if ((this.cboBaud.SelectedIndex == -1) || (this.cboAddress.SelectedIndex == -1)) {
				MessageBox.Show("Please select both a baud rate and an address.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.DialogResult = DialogResult.None;
			}
			else {
				int num = (int)this.nudPort.Value;
				Xml.SetValue(this.m_setupNode, "name", "COM" + num.ToString());
				Xml.SetValue(this.m_setupNode, "baud", this.cboBaud.Text.ToString());
				Xml.SetValue(this.m_setupNode, "addr", this.cboAddress.SelectedIndex.ToString());
				if (this.grpThresh.Enabled) {
					Xml.SetValue(this.m_setupNode, "threshold", ((int)this.nudThreshold.Value).ToString());
				}
			}
		}

		private void cboAddress_SelectedIndexChanged(object sender, EventArgs e) {
			this.UpdateImage();
		}

		private void cboBaud_SelectedIndexChanged(object sender, EventArgs e) {
			this.UpdateImage();
		}


		//ComponentResourceManager manager = new ComponentResourceManager(typeof(frmEfxSetup));
		//this.imgLstSetup.ImageStream = (ImageListStreamer) manager.GetObject("imgLstSetup.ImageStream");
		//this.picSetup.InitialImage = (Image) manager.GetObject("picSetup.InitialImage");
		//this.imgLstSetup.Images.SetKeyName(0, "efx_setup4.png");
		//this.imgLstSetup.Images.SetKeyName(1, "efx_setup5.png");
		//this.imgLstSetup.Images.SetKeyName(2, "efx_setup6.png");
		//this.imgLstSetup.Images.SetKeyName(3, "efx_setup7.png");
		//this.imgLstSetup.Images.SetKeyName(4, "efx_setup0.png");
		//this.imgLstSetup.Images.SetKeyName(5, "efx_setup1.png");
		//this.imgLstSetup.Images.SetKeyName(6, "efx_setup2.png");
		//this.imgLstSetup.Images.SetKeyName(7, "efx_setup3.png");


		private void UpdateImage() {
			if ((this.cboBaud.SelectedIndex != -1) && (this.cboAddress.SelectedIndex != -1)) {
				this.picSetup.Image = this.imgLstSetup.Images[(this.cboBaud.SelectedIndex * 4) + this.cboAddress.SelectedIndex];
			}
		}
	}
}