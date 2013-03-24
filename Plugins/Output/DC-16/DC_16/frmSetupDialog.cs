namespace DC_16 {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using Vixen;

	internal partial class frmSetupDialog : Form {

		private XmlNode m_setupNode;

		public frmSetupDialog(XmlNode setupNode) {
			int num;
			this.components = null;
			this.InitializeComponent();
			this.m_setupNode = setupNode;
			string innerText = Xml.GetNodeAlways(this.m_setupNode, "Name").InnerText;
			this.nudPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
			if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Threshold").InnerText, out num)) {
				this.nudThreshold.Value = num;
			}
			if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Group1").InnerText, out num)) {
				this.cboGrp1Addr.SelectedIndex = num;
			}
			if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Group2").InnerText, out num)) {
				this.cboGrp2Addr.SelectedIndex = num;
			}
			if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Group3").InnerText, out num)) {
				this.cboGrp3Addr.SelectedIndex = num;
			}
			if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Group4").InnerText, out num)) {
				this.cboGrp4Addr.SelectedIndex = num;
			}
		}

		private void btnOK_Click(object sender, EventArgs e) {
			if (((((this.cboGrp1Addr.SelectedIndex == -1) && (this.cboGrp2Addr.SelectedIndex == -1)) && (this.cboGrp3Addr.SelectedIndex == -1)) && (this.cboGrp4Addr.SelectedIndex == -1)) && (MessageBox.Show("No controllers have been selected to receive any of channel groupings.\nAre you sure that this is what you want to do?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)) {
				base.DialogResult = System.Windows.Forms.DialogResult.None;
			}
			else {
				int num = (int)this.nudPort.Value;
				Xml.SetValue(this.m_setupNode, "Name", "COM" + num.ToString());
				Xml.SetValue(this.m_setupNode, "Threshold", ((int)this.nudThreshold.Value).ToString());
				Xml.SetValue(this.m_setupNode, "Group1", this.cboGrp1Addr.SelectedIndex.ToString());
				Xml.SetValue(this.m_setupNode, "Group2", this.cboGrp2Addr.SelectedIndex.ToString());
				Xml.SetValue(this.m_setupNode, "Group3", this.cboGrp3Addr.SelectedIndex.ToString());
				Xml.SetValue(this.m_setupNode, "Group4", this.cboGrp4Addr.SelectedIndex.ToString());
			}
		}
	}
}