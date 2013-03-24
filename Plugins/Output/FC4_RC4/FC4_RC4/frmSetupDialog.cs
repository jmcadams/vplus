namespace FC4_RC4 {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using VixenPlus;

	internal partial class frmSetupDialog : Form {
		private ComboBox[] m_addrCombos;
		private bool m_init = false;
		private XmlNode m_setupNode;
		private ComboBox[] m_typeCombos;
		private const int MAX_MODULES = 4;

		public frmSetupDialog(XmlNode setupNode) {
			this.InitializeComponent();
			this.m_setupNode = setupNode;
			string innerText = Xml.GetNodeAlways(this.m_setupNode, "name").InnerText;
			this.nudPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
			innerText = Xml.GetNodeAlways(this.m_setupNode, "threshold").InnerText;
			if (innerText.Length > 0) {
				this.nudThreshold.Value = int.Parse(innerText);
			}
			this.m_typeCombos = new ComboBox[] { this.cboGrp1Type, this.cboGrp2Type, this.cboGrp3Type, this.cboGrp4Type, this.cboGrp5Type, this.cboGrp6Type, this.cboGrp7Type, this.cboGrp8Type };
			this.m_addrCombos = new ComboBox[] { this.cboGrp1Addr, this.cboGrp2Addr, this.cboGrp3Addr, this.cboGrp4Addr, this.cboGrp5Addr, this.cboGrp6Addr, this.cboGrp7Addr, this.cboGrp8Addr };
			this.m_init = true;
			for (int i = 0; i < 8; i++) {
				this.m_typeCombos[i].SelectedIndex = this.m_typeCombos[i].Items.IndexOf(Xml.GetNodeAlways(this.m_setupNode, "Type" + i.ToString()).InnerText);
				this.m_addrCombos[i].SelectedIndex = this.m_addrCombos[i].Items.IndexOf(Xml.GetNodeAlways(this.m_setupNode, "Addr" + i.ToString()).InnerText);
			}
			this.m_init = false;
		}

		private void addrComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (!this.m_init) {
				ComboBox addrComboBox = (ComboBox)sender;
				this.ModuleAddressCheck(addrComboBox);
				this.MaxModuleCheck((ComboBox)this.grpChannels.GetNextControl(addrComboBox, false));
			}
		}

		private List<int> AddressesOfType(int typeIndex, ComboBox addrComboToIgnore) {
			List<int> list = new List<int>();
			if (typeIndex > 0) {
				foreach (ComboBox box2 in this.m_typeCombos) {
					ComboBox nextControl = (ComboBox)this.grpChannels.GetNextControl(box2, true);
					if ((addrComboToIgnore != nextControl) && (box2.SelectedIndex == typeIndex)) {
						list.Add(nextControl.SelectedIndex);
					}
				}
			}
			return list;
		}

		private void btnOK_Click(object sender, EventArgs e) {
			Xml.SetValue(this.m_setupNode, "name", "COM" + ((int)this.nudPort.Value).ToString());
			Xml.SetValue(this.m_setupNode, "threshold", ((int)this.nudThreshold.Value).ToString());
			for (int i = 0; i < 8; i++) {
				if (this.m_typeCombos[i].SelectedItem != null) {
					Xml.SetValue(this.m_setupNode, "Type" + i.ToString(), this.m_typeCombos[i].SelectedItem.ToString());
				}
				else {
					Xml.SetValue(this.m_setupNode, "Type" + i.ToString(), string.Empty);
				}
				if (this.m_addrCombos[i].SelectedItem != null) {
					Xml.SetValue(this.m_setupNode, "Addr" + i.ToString(), this.m_addrCombos[i].SelectedItem.ToString());
				}
				else {
					Xml.SetValue(this.m_setupNode, "Addr" + i.ToString(), string.Empty);
				}
			}
		}

		private int CountOf(int typeIndex) {
			int num = 0;
			foreach (ComboBox box in this.m_typeCombos) {
				if (box.SelectedIndex == typeIndex) {
					num++;
				}
			}
			return num;
		}

		private void frmSetupDialog_Load(object sender, EventArgs e) {
			foreach (ComboBox box in this.m_typeCombos) {
				if (box.SelectedIndex == -1) {
					box.SelectedIndex = 0;
				}
			}
		}

		private void MaxModuleCheck(ComboBox typeComboBox) {
			if (((typeComboBox.SelectedIndex != 0) && (typeComboBox.SelectedIndex != -1)) && (this.CountOf(typeComboBox.SelectedIndex) > 4)) {
				MessageBox.Show(string.Format("A maximum of {0} modules of the same type may be selected.\nThe selection will be reset to 'None'.", 4), "FC4-RC4", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				typeComboBox.SelectedIndex = 0;
			}
		}

		private void ModuleAddressCheck(ComboBox addrComboBox) {
			if (addrComboBox.SelectedIndex != -1) {
				int selectedIndex = ((ComboBox)this.grpChannels.GetNextControl(addrComboBox, false)).SelectedIndex;
				if (selectedIndex > 0) {
					if (this.AddressesOfType(selectedIndex, addrComboBox).Contains(addrComboBox.SelectedIndex)) {
						MessageBox.Show("A module of that type is already at that address.\nThe address will be reset.", "FC4 RC4", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						addrComboBox.SelectedIndex = -1;
					}
				}
				else {
					addrComboBox.SelectedIndex = -1;
				}
			}
		}

		private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (!this.m_init) {
				ComboBox typeComboBox = (ComboBox)sender;
				this.MaxModuleCheck(typeComboBox);
				this.ModuleAddressCheck((ComboBox)this.grpChannels.GetNextControl(typeComboBox, true));
			}
		}
	}
}