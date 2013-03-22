namespace Vixen.Dialogs {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.IO;
	using System.Windows.Forms;
	using Vixen;

	public partial class ProfileManagerDialog : Form {
		private List<int> m_channelOrderMapping;
		private Profile m_contextProfile = null;
		private Color m_pictureBoxHoverColor = Color.FromArgb(80, 80, 0xff);
		private SolidBrush m_pictureBrush = new SolidBrush(Color.Black);
		private Color m_pictureDisabledColor = Color.FromArgb(0xc0, 0xc0, 0xc0);
		private Color m_pictureEnabledColor = Color.FromArgb(0x80, 0x80, 0xff);
		private Font m_pictureFont = new Font("Arial", 13f, FontStyle.Bold);
		private Pen m_picturePen = new Pen(Color.Black, 2f);

		public ProfileManagerDialog(object objectInContext) {
			this.InitializeComponent();
			foreach (string str in Directory.GetFiles(Paths.ProfilePath, "*.pro")) {
				try {
					this.listBoxProfiles.Items.Add(new Profile(str));
				}
				catch {
				}
			}
			Bitmap bitmap = new Bitmap(this.pictureBoxReturnFromProfileEdit.Image);
			bitmap.MakeTransparent();
			this.pictureBoxReturnFromProfileEdit.Image = bitmap;
			this.m_channelOrderMapping = new List<int>();
			if (objectInContext is Profile) {
				if (this.listBoxProfiles.Items.IndexOf(objectInContext) == -1) {
					this.listBoxProfiles.Items.Add((Profile)objectInContext);
				}
				this.EditProfile((Profile)objectInContext);
			}
			else {
				this.tabControl.SelectedTab = this.tabProfiles;
			}
		}

		private void AddProfileChannel() {
			int num = this.treeViewProfile.Nodes.Count + 1;
			Channel channelObject = new Channel("Channel " + num.ToString(), 0);
			this.m_contextProfile.AddChannelObject(channelObject);
			this.treeViewProfile.Nodes.Add(channelObject.Name).Tag = channelObject;
			this.m_channelOrderMapping.Add(this.m_channelOrderMapping.Count);
		}

		private void buttonAddMultipleProfileChannels_Click(object sender, EventArgs e) {
			int num;
			try {
				num = Convert.ToInt32(this.textBoxProfileChannelCount.Text);
			}
			catch {
				MessageBox.Show("Need to have a valid number for the number of channels to add.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			this.treeViewProfile.BeginUpdate();
			while (num-- > 0) {
				this.AddProfileChannel();
			}
			this.treeViewProfile.EndUpdate();
		}

		private void buttonAddProfileChannel_Click(object sender, EventArgs e) {
			this.AddProfileChannel();
		}

		private void buttonChangeProfileName_Click(object sender, EventArgs e) {
			if (this.ChangeProfileName()) {
				this.labelProfileName.Text = this.m_contextProfile.Name;
			}
		}

		private void buttonDone_Click(object sender, EventArgs e) {
			foreach (Profile profile in this.listBoxProfiles.Items) {
				if (!(!(profile.FileName == string.Empty) || this.ChangeProfileName())) {
					base.DialogResult = System.Windows.Forms.DialogResult.None;
					break;
				}
				profile.SaveToFile();
			}
		}

		private void buttonPlugins_Click(object sender, EventArgs e) {
			PluginListDialog dialog = new PluginListDialog(this.m_contextProfile);
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void buttonRemoveProfileChannels_Click(object sender, EventArgs e) {
			this.RemoveSelectedProfileChannelObjects();
		}

		private bool ChangeProfileName() {
			TextQueryDialog dialog = new TextQueryDialog("Profile Name", "Name for this profile", this.m_contextProfile.Name);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_contextProfile.Name = dialog.Response;
				dialog.Dispose();
				return true;
			}
			dialog.Dispose();
			return false;
		}

		private void comboBoxChannelOrder_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.comboBoxChannelOrder.SelectedIndex != -1) {
				List<Channel> channels = this.m_contextProfile.Channels;
				if (this.comboBoxChannelOrder.SelectedIndex == 0) {
					if (channels.Count == 0) {
						MessageBox.Show("There are no channels to reorder.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
					this.pictureBoxProfileDeleteChannelOrder.Enabled = false;
					this.comboBoxChannelOrder.SelectedIndex = -1;
					List<Channel> channelList = this.m_contextProfile.Channels;
					ChannelOrderDialog dialog = new ChannelOrderDialog(channelList, this.m_channelOrderMapping);
					if (dialog.ShowDialog() == DialogResult.OK) {
						this.m_channelOrderMapping.Clear();
						foreach (Channel channel in dialog.ChannelMapping) {
							this.m_channelOrderMapping.Add(channelList.IndexOf(channel));
						}
					}
					dialog.Dispose();
				}
				else if (this.comboBoxChannelOrder.SelectedIndex == (this.comboBoxChannelOrder.Items.Count - 1)) {
					this.pictureBoxProfileDeleteChannelOrder.Enabled = false;
					this.comboBoxChannelOrder.SelectedIndex = -1;
					this.m_channelOrderMapping.Clear();
					for (int i = 0; i < channels.Count; i++) {
						this.m_channelOrderMapping.Add(i);
					}
					this.m_contextProfile.LastSort = -1;
				}
				else {
					this.m_channelOrderMapping.Clear();
					this.m_channelOrderMapping.AddRange(((Vixen.SortOrder)this.comboBoxChannelOrder.SelectedItem).ChannelIndexes);
					this.m_contextProfile.LastSort = this.comboBoxChannelOrder.SelectedIndex;
					this.pictureBoxProfileDeleteChannelOrder.Enabled = true;
				}
				this.ReloadProfileChannelObjects();
			}
		}



		private void EditProfile(Profile profile) {
			this.m_contextProfile = profile;
			this.labelProfileName.Text = profile.Name;
			this.UpdateSortList();
			this.ReloadProfileChannelObjects();
			this.comboBoxChannelOrder.SelectedIndex = this.m_contextProfile.LastSort;
			this.tabEditProfile.Tag = this.tabControl.SelectedTab;
			this.tabControl.SelectedTab = this.tabEditProfile;
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(ProfileManagerDialog));
		//this.pictureBoxProfileDeleteChannelOrder.Image = (Image)manager.GetObject("pictureBoxProfileDeleteChannelOrder.Image");
		//this.pictureBoxProfileSaveChannelOrder.Image = (Image)manager.GetObject("pictureBoxProfileSaveChannelOrder.Image");
		//this.pictureBoxProfileChannelColors.Image = (Image)manager.GetObject("pictureBoxProfileChannelColors.Image");
		//this.pictureBoxProfileChannelOutputMask.Image = (Image)manager.GetObject("pictureBoxProfileChannelOutputMask.Image");
		//this.pictureBoxProfileChannelOutputs.Image = (Image)manager.GetObject("pictureBoxProfileChannelOutputs.Image");
		//this.pictureBoxReturnFromProfileEdit.Image = (Image)manager.GetObject("pictureBoxReturnFromProfileEdit.Image");



		private void listBoxProfiles_DoubleClick(object sender, EventArgs e) {
			if (this.listBoxProfiles.SelectedIndex != -1) {
				this.EditProfile((Profile)this.listBoxProfiles.SelectedItem);
			}
		}

		private void listBoxProfiles_KeyDown(object sender, KeyEventArgs e) {
			if ((this.listBoxProfiles.SelectedIndex != -1) && (e.KeyCode == Keys.Delete)) {
				this.RemoveProfile((Profile)this.listBoxProfiles.SelectedItem);
			}
		}

		private void listBoxProfiles_SelectedIndexChanged(object sender, EventArgs e) {
			this.pictureBoxEditProfile.Enabled = this.pictureBoxRemoveProfile.Enabled = this.listBoxProfiles.SelectedIndex != -1;
		}

		private void PictureBase(PictureBox pb, Graphics g) {
			Color color = (this.m_hoveredButton == pb) ? this.m_pictureBoxHoverColor : (pb.Enabled ? this.m_pictureEnabledColor : this.m_pictureDisabledColor);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle clientRectangle = pb.ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			this.m_picturePen.Color = color;
			this.m_pictureBrush.Color = color;
			g.FillEllipse(Brushes.White, clientRectangle);
			g.DrawEllipse(this.m_picturePen, clientRectangle);
		}

		private void pictureBoxAddProfile_Click(object sender, EventArgs e) {
			TextQueryDialog dialog = new TextQueryDialog("New Profile", "Name for the new profile", string.Empty);
			if (dialog.ShowDialog() == DialogResult.OK) {
				Profile item = new Profile();
				item.Name = dialog.Response;
				this.listBoxProfiles.Items.Add(item);
				this.EditProfile(item);
			}
			dialog.Dispose();
		}

		private void pictureBoxAddProfile_MouseEnter(object sender, EventArgs e) {
			this.m_hoveredButton = (PictureBox)sender;
			((PictureBox)sender).Refresh();
		}

		private void pictureBoxAddProfile_MouseLeave(object sender, EventArgs e) {
			this.m_hoveredButton = null;
			((PictureBox)sender).Refresh();
		}

		private void pictureBoxAddProfile_Paint(object sender, PaintEventArgs e) {
			this.PictureBase((PictureBox)sender, e.Graphics);
			e.Graphics.DrawString("+", this.m_pictureFont, this.m_pictureBrush, (float)3f, (float)1f);
		}

		private void pictureBoxEditProfile_Click(object sender, EventArgs e) {
			this.EditProfile((Profile)this.listBoxProfiles.SelectedItem);
		}

		private void pictureBoxEditProfile_Paint(object sender, PaintEventArgs e) {
			this.PictureBase((PictureBox)sender, e.Graphics);
			e.Graphics.DrawLine(this.m_picturePen, 6, 11, 11, 6);
			e.Graphics.DrawLine(this.m_picturePen, 9, 14, 14, 9);
		}

		private void pictureBoxProfileChannelColors_Click(object sender, EventArgs e) {
			List<Channel> channels = this.m_contextProfile.Channels;
			AllChannelsColorDialog dialog = new AllChannelsColorDialog(channels);
			if (dialog.ShowDialog() == DialogResult.OK) {
				List<Color> channelColors = dialog.ChannelColors;
				for (int i = 0; i < channels.Count; i++) {
					channels[i].Color = channelColors[i];
				}
			}
			dialog.Dispose();
		}

		private void pictureBoxProfileChannelOutputMask_Click(object sender, EventArgs e) {
			List<Channel> channels = this.m_contextProfile.Channels;
			ChannelOutputMaskDialog dialog = new ChannelOutputMaskDialog(channels);
			if (dialog.ShowDialog() == DialogResult.OK) {
				foreach (Channel channel in channels) {
					channel.Enabled = true;
				}
				foreach (int num in dialog.DisabledChannels) {
					channels[num].Enabled = false;
				}
			}
			dialog.Dispose();
		}

		private void pictureBoxProfileChannelOutputs_Click(object sender, EventArgs e) {
			ChannelOrderDialog dialog = new ChannelOrderDialog(this.m_contextProfile.OutputChannels, null, "Channel output mapping");
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_contextProfile.OutputChannels = dialog.ChannelMapping;
			}
			dialog.Dispose();
		}

		private void pictureBoxProfileDeleteChannelOrder_Click(object sender, EventArgs e) {
			if (MessageBox.Show(string.Format("Delete channel order '{0}'?", this.comboBoxChannelOrder.Text), Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
				this.m_contextProfile.Sorts.Remove((Vixen.SortOrder)this.comboBoxChannelOrder.SelectedItem);
				this.comboBoxChannelOrder.Items.RemoveAt(this.comboBoxChannelOrder.SelectedIndex);
				this.pictureBoxProfileDeleteChannelOrder.Enabled = false;
			}
		}

		private void pictureBoxProfileSaveChannelOrder_Click(object sender, EventArgs e) {
			Vixen.SortOrder item = null;
			TextQueryDialog dialog = new TextQueryDialog("New order", "What name would you like to give to this ordering of the channels?", string.Empty);
			DialogResult no = DialogResult.No;
			while (no == DialogResult.No) {
				if (dialog.ShowDialog() == DialogResult.Cancel) {
					dialog.Dispose();
					return;
				}
				no = DialogResult.Yes;
				foreach (Vixen.SortOrder order2 in this.m_contextProfile.Sorts) {
					if (order2.Name == dialog.Response) {
						if ((no = MessageBox.Show("This name is already in use.\nDo you want to overwrite it?", Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) == DialogResult.Cancel) {
							dialog.Dispose();
							return;
						}
						item = order2;
						break;
					}
				}
			}
			dialog.Dispose();
			if (item != null) {
				item.ChannelIndexes.Clear();
				item.ChannelIndexes.AddRange(this.m_channelOrderMapping);
				this.comboBoxChannelOrder.SelectedItem = item;
			}
			else {
				this.m_contextProfile.Sorts.Add(item = new Vixen.SortOrder(dialog.Response, this.m_channelOrderMapping));
				item.ChannelIndexes.Clear();
				item.ChannelIndexes.AddRange(this.m_channelOrderMapping);
				this.comboBoxChannelOrder.Items.Insert(this.comboBoxChannelOrder.Items.Count - 1, item);
				this.comboBoxChannelOrder.SelectedIndex = this.comboBoxChannelOrder.Items.Count - 2;
			}
		}

		private void pictureBoxRemoveProfile_Click(object sender, EventArgs e) {
			this.RemoveProfile((Profile)this.listBoxProfiles.SelectedItem);
		}

		private void pictureBoxRemoveProfile_Paint(object sender, PaintEventArgs e) {
			this.PictureBase((PictureBox)sender, e.Graphics);
			e.Graphics.DrawString("-", this.m_pictureFont, this.m_pictureBrush, (float)4f, (float)0f);
		}

		private void pictureBoxReturnFromChannelGroupEdit_Click(object sender, EventArgs e) {
			if (this.tabControl.SelectedTab.Tag == this.tabProfiles) {
				this.UpdateProfiles();
			}
			else if (this.tabControl.SelectedTab.Tag == this.tabEditProfile) {
				this.ReloadProfileChannelObjects();
			}
			this.tabControl.SelectedTab = (TabPage)this.tabControl.SelectedTab.Tag;
		}

		private void ReloadProfileChannelObjects() {
			int index = -1;
			if (this.treeViewProfile.SelectedNode != null) {
				if (this.treeViewProfile.SelectedNode.Level == 0) {
					if (this.treeViewProfile.SelectedNode.IsExpanded) {
						index = this.treeViewProfile.SelectedNode.Index;
					}
				}
				else if ((this.treeViewProfile.SelectedNode.Level == 1) && this.treeViewProfile.SelectedNode.Parent.IsExpanded) {
					index = this.treeViewProfile.SelectedNode.Parent.Index;
				}
			}
			this.buttonRemoveProfileChannels.Enabled = false;
			this.treeViewProfile.BeginUpdate();
			this.treeViewProfile.Nodes.Clear();
			List<Channel> channels = this.m_contextProfile.Channels;
			foreach (int num2 in this.m_channelOrderMapping) {
				Channel channel = channels[num2];
				this.treeViewProfile.Nodes.Add(channel.Name).Tag = channel;
			}
			if (index != -1) {
				this.treeViewProfile.Nodes[index].Expand();
			}
			this.treeViewProfile.EndUpdate();
		}

		private void RemoveProfile(Profile profile) {
			if (MessageBox.Show(string.Format("Remove profile {0}?\n\nThis will affect any sequences that use this profile.", profile.Name), Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
				File.Delete(profile.FileName);
				this.listBoxProfiles.Items.Remove(profile);
			}
		}

		private void RemoveSelectedProfileChannelObjects() {
			if (this.treeViewProfile.SelectedNode.Level != 0) {
				this.buttonRemoveProfileChannels.Enabled = false;
			}
			else if (MessageBox.Show("Remove the selected item from this profile?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
				this.m_channelOrderMapping.RemoveAt(this.m_contextProfile.Channels.IndexOf((Channel)this.treeViewProfile.SelectedNode.Tag));
				this.m_contextProfile.RemoveChannelObject((Channel)this.treeViewProfile.SelectedNode.Tag);
				this.treeViewProfile.Nodes.Remove(this.treeViewProfile.SelectedNode);
			}
		}

		private void treeViewProfile_AfterSelect(object sender, TreeViewEventArgs e) {
			this.buttonRemoveProfileChannels.Enabled = (this.treeViewProfile.SelectedNode != null) && (e.Node.Level == 0);
		}

		private void treeViewProfile_DoubleClick(object sender, EventArgs e) {
			if (this.treeViewProfile.SelectedNode != null) {
				Channel tag = (Channel)this.treeViewProfile.SelectedNode.Tag;
				List<Channel> channels = new List<Channel>();
				foreach (TreeNode node in this.treeViewProfile.Nodes) {
					channels.Add((Channel)node.Tag);
				}
				ChannelPropertyDialog dialog = new ChannelPropertyDialog(channels, tag, false);
				dialog.ShowDialog();
				this.ReloadProfileChannelObjects();
				dialog.Dispose();
			}
		}

		private void treeViewProfile_KeyDown(object sender, KeyEventArgs e) {
			if ((this.treeViewProfile.SelectedNode != null) && (e.KeyCode == Keys.Delete)) {
				this.RemoveSelectedProfileChannelObjects();
			}
		}

		private void UpdateProfiles() {
			List<Profile> list = new List<Profile>();
			foreach (Profile profile in this.listBoxProfiles.Items) {
				list.Add(profile);
			}
			this.listBoxProfiles.SelectedIndex = -1;
			this.listBoxProfiles.BeginUpdate();
			this.listBoxProfiles.Items.Clear();
			foreach (Profile profile in list) {
				this.listBoxProfiles.Items.Add(profile);
			}
			this.listBoxProfiles.EndUpdate();
		}

		private void UpdateSortList() {
			this.comboBoxChannelOrder.BeginUpdate();
			string item = (string)this.comboBoxChannelOrder.Items[0];
			string str2 = (string)this.comboBoxChannelOrder.Items[this.comboBoxChannelOrder.Items.Count - 1];
			this.comboBoxChannelOrder.Items.Clear();
			this.comboBoxChannelOrder.Items.Add(item);
			foreach (Vixen.SortOrder order in this.m_contextProfile.Sorts) {
				this.comboBoxChannelOrder.Items.Add(order);
			}
			this.comboBoxChannelOrder.Items.Add(str2);
			this.comboBoxChannelOrder.EndUpdate();
			int count = this.m_contextProfile.Channels.Count;
			this.m_channelOrderMapping.Clear();
			for (int i = 0; i < count; i++) {
				this.m_channelOrderMapping.Add(i);
			}
		}
	}
}

