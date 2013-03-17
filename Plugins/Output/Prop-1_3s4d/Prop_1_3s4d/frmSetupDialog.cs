namespace Prop_1_3s4d {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using Vixen;

	internal partial class frmSetupDialog : Form {

		private XmlNode m_setupNode;

		public frmSetupDialog(XmlNode setupNode) {
			this.InitializeComponent();
			this.m_setupNode = setupNode;
			string innerText = Xml.GetNodeAlways(this.m_setupNode, "name").InnerText;
			this.nudPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
			innerText = Xml.GetNodeAlways(this.m_setupNode, "threshold").InnerText;
			this.numericUpDownThreshold.Value = (innerText.Length > 0) ? int.Parse(innerText) : 50;
			innerText = Xml.GetNodeAlways(this.m_setupNode, "analogMin").InnerText;
			this.nudAnaMin.Value = (innerText.Length > 0) ? int.Parse(innerText) : 100;
			innerText = Xml.GetNodeAlways(this.m_setupNode, "analogMax").InnerText;
			this.nudAnaMax.Value = (innerText.Length > 0) ? int.Parse(innerText) : 200;
		}

		private void btnOK_Click(object sender, EventArgs e) {
			if (this.nudAnaMin.Value >= this.nudAnaMax.Value) {
				MessageBox.Show("Minimum must be below the maximum.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.DialogResult = System.Windows.Forms.DialogResult.None;
			}
			else {
				int num = (int)this.nudPort.Value;
				Xml.SetValue(this.m_setupNode, "name", "COM" + num.ToString());
				num = (int)this.numericUpDownThreshold.Value;
				Xml.SetValue(this.m_setupNode, "threshold", num.ToString());
				num = (int)this.nudAnaMin.Value;
				Xml.SetValue(this.m_setupNode, "analogMin", num.ToString());
				Xml.SetValue(this.m_setupNode, "analogMax", ((int)this.nudAnaMax.Value).ToString());
			}
		}
	}
}