namespace Preview {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;
	using System.Xml;
	using Vixen;

	public partial class SetupDialog : Form {
		private string m_backgroundImageFileName;
		private int m_cellSize;
		private Dictionary<int, List<uint>> m_channelDictionary;
		private List<Channel> m_channels;
		private bool m_controlDown;
		private bool m_dirty;
		private Image m_originalBackground;
		private bool m_resizing;
		private SetupData m_setupData;
		private XmlNode m_setupNode;
		private int m_startChannel;

		public SetupDialog(SetupData setupData, XmlNode setupNode, List<Channel> channels, int startChannel) {
			int num;
			this.m_setupNode = null;
			this.m_setupData = null;
			this.m_backgroundImageFileName = string.Empty;
			this.m_dirty = false;
			this.m_channelDictionary = new Dictionary<int, List<uint>>();
			this.m_channels = null;
			this.m_controlDown = false;
			this.m_originalBackground = null;
			this.m_resizing = false;
			this.components = null;
			this.InitializeComponent();
			this.m_setupData = setupData;
			this.m_setupNode = setupNode;
			this.m_startChannel = startChannel;
			this.toolStripComboBoxPixelSize.SelectedIndex = 7;
			this.toolStripComboBoxChannels.BeginUpdate();
			for (num = this.m_startChannel; num < channels.Count; num++) {
				this.toolStripComboBoxChannels.Items.Add(string.Format("{0}: {1}", num + 1, channels[num].Name));
			}
			this.toolStripComboBoxChannels.EndUpdate();
			this.m_channels = new List<Channel>();
			for (num = this.m_startChannel; num < channels.Count; num++) {
				this.m_channels.Add(channels[num]);
			}
			this.UpdateFromSetupNode();
		}

		private void allChannelsToolStripMenuItem_Click(object sender, EventArgs e) {
			this.m_channelDictionary.Clear();
			this.pictureBoxSetupGrid.Refresh();
			this.m_dirty = true;
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			this.UpdateSetup();
			this.m_dirty = false;
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(SetupDialog));
		//this.toolStripDropDownButtonUpdate.Image = (Image)manager.GetObject("toolStripDropDownButtonUpdate.Image");
		//this.toolStripButtonResetSize.Image = (Image)manager.GetObject("toolStripButtonResetSize.Image");
		//this.toolStripDropDownButtonClear.Image = (Image)manager.GetObject("toolStripDropDownButtonClear.Image");
		//this.toolStripButtonLoadImage.Image = (Image)manager.GetObject("toolStripButtonLoadImage.Image");
		//this.toolStripButtonClearImage.Image = (Image)manager.GetObject("toolStripButtonClearImage.Image");
		//this.toolStripButtonSaveImage.Image = (Image)manager.GetObject("toolStripButtonSaveImage.Image");
		//this.toolStripButtonReorder.Image = (Image)manager.GetObject("toolStripButtonReorder.Image");

		private void pictureBoxSetupGrid_MouseEvent(object sender, MouseEventArgs e) {
			if ((e.X >= 0) && (e.Y >= 0)) {
				int x = e.X / (this.m_cellSize + 1);
				int y = e.Y / (this.m_cellSize + 1);
				if ((x < this.pictureBoxSetupGrid.Width) && (y < this.pictureBoxSetupGrid.Height)) {
					if (e.Button == MouseButtons.Left) {
						this.SetPixelChannelReference(this.toolStripComboBoxChannels.SelectedIndex, x, y);
					}
					else if (e.Button == MouseButtons.Right) {
						if (!this.m_controlDown) {
							this.ResetPixelChannelReference(this.toolStripComboBoxChannels.SelectedIndex, x, y);
						}
						else {
							this.ResetPixelChannelReference(-1, x, y);
						}
					}
					else {
						uint item = (uint)((x << 0x10) | y);
						StringBuilder builder = new StringBuilder();
						foreach (int num4 in this.m_channelDictionary.Keys) {
							if ((num4 < this.m_channels.Count) && this.m_channelDictionary[num4].Contains(item)) {
								builder.AppendFormat("{0}, ", this.toolStripComboBoxChannels.Items[num4]);
							}
						}
						if (builder.Length > 0) {
							string str = builder.ToString();
							this.labelChannel.Text = str.Substring(0, str.Length - 2);
						}
						else {
							this.labelChannel.Text = string.Empty;
						}
					}
				}
			}
		}

		private void pictureBoxSetupGrid_Paint(object sender, PaintEventArgs e) {
			int num;
			int num2;
			bool flag = this.m_originalBackground != null;
			SolidBrush brush = new SolidBrush(Color.Black);
			int selectedIndex = this.toolStripComboBoxChannels.SelectedIndex;
			if (flag) {
				e.Graphics.FillRectangle(Brushes.Transparent, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width, e.ClipRectangle.Height);
			}
			else {
				e.Graphics.FillRectangle(Brushes.Black, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width, e.ClipRectangle.Height);
			}
			foreach (int num4 in this.m_channelDictionary.Keys) {
				if ((num4 != selectedIndex) && (num4 < this.m_channels.Count)) {
					brush.Color = Color.FromArgb(0x80, this.m_channels[num4].Color);
					foreach (uint num5 in this.m_channelDictionary[num4]) {
						num = (int)((num5 >> 0x10) * (this.m_cellSize + 1));
						num2 = (int)((num5 & 0xffff) * (this.m_cellSize + 1));
						if (e.ClipRectangle.Contains(num, num2)) {
							e.Graphics.FillRectangle(brush, num, num2, this.m_cellSize, this.m_cellSize);
						}
					}
				}
			}
			if (this.m_channelDictionary.ContainsKey(selectedIndex)) {
				Channel channel = this.m_channels[selectedIndex];
				foreach (uint num5 in this.m_channelDictionary[selectedIndex]) {
					num = (int)((num5 >> 0x10) * (this.m_cellSize + 1));
					num2 = (int)((num5 & 0xffff) * (this.m_cellSize + 1));
					if (e.ClipRectangle.Contains(num, num2)) {
						e.Graphics.FillRectangle(channel.Brush, num, num2, this.m_cellSize, this.m_cellSize);
					}
				}
			}
			brush.Dispose();
		}

		private void ResetPixelChannelReference(int channelIndex, int x, int y) {
			if (channelIndex == -1) {
				uint item = (uint)((x << 0x10) | y);
				foreach (List<uint> list in this.m_channelDictionary.Values) {
					list.Remove(item);
				}
				this.pictureBoxSetupGrid.Invalidate(new Rectangle(x * (this.m_cellSize + 1), y * (this.m_cellSize + 1), this.m_cellSize, this.m_cellSize));
			}
			else {
				List<uint> list2;
				if (this.m_channelDictionary.TryGetValue(channelIndex, out list2)) {
					list2.Remove((uint)((x << 0x10) | y));
					this.pictureBoxSetupGrid.Invalidate(new Rectangle(x * (this.m_cellSize + 1), y * (this.m_cellSize + 1), this.m_cellSize, this.m_cellSize));
				}
			}
			this.m_dirty = true;
		}

		private void selectedChannelToolStripMenuItem_Click(object sender, EventArgs e) {
			if (this.toolStripComboBoxChannels.SelectedIndex != -1) {
				this.m_channelDictionary.Remove(this.toolStripComboBoxChannels.SelectedIndex);
				this.pictureBoxSetupGrid.Refresh();
				this.m_dirty = true;
			}
		}

		private void SetBackground(Bitmap bitmap) {
			if (this.m_originalBackground != null) {
				this.m_originalBackground.Dispose();
			}
			this.m_originalBackground = bitmap;
			this.labelBrightness.Visible = this.trackBarBrightness.Visible = this.toolStripButtonSaveImage.Enabled = bitmap != null;
			this.toolStripTextBoxResolutionX.Enabled = this.toolStripTextBoxResolutionY.Enabled = bitmap == null;
			if (bitmap != null) {
				this.trackBarBrightness.Value = 10;
			}
		}

		private void SetBrightness(float value) {
			if (this.m_originalBackground != null) {
				Image image = new Bitmap(this.m_originalBackground);
				float[][] newColorMatrix = new float[5][];
				float[] numArray2 = new float[5];
				numArray2[0] = 1f;
				newColorMatrix[0] = numArray2;
				numArray2 = new float[5];
				numArray2[1] = 1f;
				newColorMatrix[1] = numArray2;
				numArray2 = new float[5];
				numArray2[2] = 1f;
				newColorMatrix[2] = numArray2;
				numArray2 = new float[5];
				numArray2[3] = 1f;
				newColorMatrix[3] = numArray2;
				numArray2 = new float[5];
				numArray2[0] = value;
				numArray2[1] = value;
				numArray2[2] = value;
				numArray2[4] = 1f;
				newColorMatrix[4] = numArray2;
				ColorMatrix matrix = new ColorMatrix(newColorMatrix);
				Graphics graphics = Graphics.FromImage(image);
				ImageAttributes imageAttr = new ImageAttributes();
				imageAttr.SetColorMatrix(matrix);
				graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
				graphics.Dispose();
				imageAttr.Dispose();
				this.pictureBoxSetupGrid.BackgroundImage = image;
			}
		}

		private void SetPictureBoxSize(int width, int height) {
			this.pictureBoxSetupGrid.Width = width;
			this.pictureBoxSetupGrid.Height = height;
			this.UpdatePosition();
			this.pictureBoxSetupGrid.Refresh();
		}

		private void SetPixelChannelReference(int channelIndex, int x, int y) {
			if (channelIndex != -1) {
				List<uint> list;
				if (!this.m_channelDictionary.TryGetValue(channelIndex, out list)) {
					list = new List<uint>();
					this.m_channelDictionary[channelIndex] = list;
				}
				uint item = (uint)((x << 0x10) | y);
				if (!list.Contains(item)) {
					list.Add(item);
				}
				this.pictureBoxSetupGrid.Invalidate(new Rectangle(x * (this.m_cellSize + 1), y * (this.m_cellSize + 1), this.m_cellSize, this.m_cellSize));
				this.m_dirty = true;
			}
		}

		private void SetupDialog_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.m_dirty) {
				switch (MessageBox.Show("Keep changes?", "Vixen preview", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) {
					case DialogResult.Cancel:
						e.Cancel = true;
						break;

					case DialogResult.Yes:
						this.UpdateSetup();
						break;
				}
			}
		}

		private void SetupDialog_KeyDown(object sender, KeyEventArgs e) {
			this.m_controlDown = e.Control;
		}

		private void SetupDialog_KeyUp(object sender, KeyEventArgs e) {
			this.m_controlDown = e.Control;
		}

		private void SetupDialog_Resize(object sender, EventArgs e) {
			if (!this.m_resizing) {
				this.UpdatePosition();
			}
		}

		private void SetupDialog_ResizeBegin(object sender, EventArgs e) {
			this.m_resizing = true;
		}

		private void SetupDialog_ResizeEnd(object sender, EventArgs e) {
			this.m_resizing = false;
			this.UpdatePosition();
		}

		private void toolStripButtonClearImage_Click(object sender, EventArgs e) {
			this.pictureBoxSetupGrid.BackgroundImage = null;
			this.SetBackground(null);
			this.m_backgroundImageFileName = string.Empty;
			this.m_dirty = true;
		}

		private void toolStripButtonLoadImage_Click(object sender, EventArgs e) {
			if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
				FileStream stream = new FileStream(this.openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				byte[] buffer = new byte[stream.Length];
				stream.Read(buffer, 0, (int)stream.Length);
				stream.Close();
				stream.Dispose();
				MemoryStream stream2 = new MemoryStream(buffer);
				Bitmap bitmap = new Bitmap(stream2);
				stream2.Close();
				stream2.Dispose();
				this.SetPictureBoxSize(bitmap.Width, bitmap.Height);
				this.SetBackground(bitmap);
				this.SetBrightness(0f);
				this.m_backgroundImageFileName = this.openFileDialog.FileName;
				this.m_dirty = true;
			}
		}

		private void toolStripButtonReorder_Click(object sender, EventArgs e) {
		}

		private void toolStripButtonResetSize_Click(object sender, EventArgs e) {
			if (this.m_originalBackground != null) {
				this.SetPictureBoxSize(this.m_originalBackground.Width, this.m_originalBackground.Height);
				this.UpdateSizeUI();
			}
		}

		private void toolStripButtonSaveImage_Click(object sender, EventArgs e) {
			if (this.saveFileDialog.ShowDialog() == DialogResult.OK) {
				new Bitmap(this.m_originalBackground).Save(Path.ChangeExtension(this.saveFileDialog.FileName, ".jpg"), ImageFormat.Jpeg);
				MessageBox.Show("File saved.", "Preview", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void toolStripComboBoxChannels_SelectedIndexChanged(object sender, EventArgs e) {
			this.pictureBoxSetupGrid.Refresh();
		}

		private void toolStripDropDownButtonUpdate_Click(object sender, EventArgs e) {
			int width;
			int height;
			this.m_cellSize = this.toolStripComboBoxPixelSize.SelectedIndex + 1;
			try {
				width = int.Parse(this.toolStripTextBoxResolutionX.Text) * (this.m_cellSize + 1);
			}
			catch {
				width = this.pictureBoxSetupGrid.Width;
			}
			try {
				height = int.Parse(this.toolStripTextBoxResolutionY.Text) * (this.m_cellSize + 1);
			}
			catch {
				height = this.pictureBoxSetupGrid.Height;
			}
			this.SetPictureBoxSize(width, height);
		}

		private void trackBarBrightness_ValueChanged(object sender, EventArgs e) {
			this.SetBrightness(((float)(this.trackBarBrightness.Value - 10)) / 10f);
		}

		private void UpdateFromSetupNode() {
			int num;
			int num2;
			this.checkBoxRedirectOutputs.Checked = this.m_setupData.GetBoolean(this.m_setupNode, "RedirectOutputs", false);
			XmlNode setupDataNode = this.m_setupNode.SelectSingleNode("Display");
			if (setupDataNode != null) {
				num2 = Convert.ToInt32(setupDataNode.SelectSingleNode("Height").InnerText);
				num = Convert.ToInt32(setupDataNode.SelectSingleNode("Width").InnerText);
				this.m_cellSize = Convert.ToInt32(setupDataNode.SelectSingleNode("PixelSize").InnerText);
			}
			else {
				num2 = 0x20;
				num = 0x40;
				this.m_cellSize = 8;
			}
			this.SetPictureBoxSize(num * (this.m_cellSize + 1), num2 * (this.m_cellSize + 1));
			this.UpdateSizeUI();
			byte[] buffer = Convert.FromBase64String(this.m_setupNode.SelectSingleNode("BackgroundImage").InnerText);
			if (buffer.Length > 0) {
				MemoryStream stream = new MemoryStream(buffer);
				this.SetBackground(new Bitmap(stream));
				stream.Dispose();
			}
			else {
				this.SetBackground(null);
			}
			if (setupDataNode != null) {
				this.trackBarBrightness.Value = this.m_setupData.GetInteger(setupDataNode, "Brightness", 10);
				this.trackBarBrightness_ValueChanged(null, null);
			}
			foreach (XmlNode node2 in this.m_setupNode.SelectNodes("Channels/Channel")) {
				int num3 = Convert.ToInt32(node2.Attributes["number"].Value);
				if (num3 >= this.m_startChannel) {
					List<uint> list = new List<uint>();
					byte[] buffer2 = Convert.FromBase64String(node2.InnerText);
					for (int i = 0; i < buffer2.Length; i += 4) {
						list.Add(BitConverter.ToUInt32(buffer2, i));
					}
					this.m_channelDictionary[num3 - this.m_startChannel] = list;
				}
			}
			this.pictureBoxSetupGrid.Refresh();
		}

		private void UpdatePosition() {
			Point point = new Point();
			if (this.pictureBoxSetupGrid.Width > this.panelPictureBoxContainer.Width) {
				point.X = 0;
			}
			else {
				point.X = ((this.panelPictureBoxContainer.Width - this.pictureBoxSetupGrid.Width) / 2) + base.ClientRectangle.Left;
			}
			if (this.pictureBoxSetupGrid.Height > this.panelPictureBoxContainer.Height) {
				point.Y = 0;
			}
			else {
				point.Y = ((this.panelPictureBoxContainer.Height - this.pictureBoxSetupGrid.Height) / 2) + base.ClientRectangle.Top;
			}
			this.pictureBoxSetupGrid.Location = point;
			int num = this.trackBarBrightness.Right - this.labelBrightness.Left;
			int num2 = this.panel1.Width / 2;
			this.labelBrightness.Left = num2 - (num / 2);
			this.trackBarBrightness.Left = (this.labelBrightness.Left + num) - this.trackBarBrightness.Width;
		}

		private void UpdateSetup() {
			XmlNode contextNode = Preview.Xml.SetValue(this.m_setupNode, "Display", string.Empty);
			Preview.Xml.SetValue(contextNode, "Height", this.toolStripTextBoxResolutionY.Text);
			Preview.Xml.SetValue(contextNode, "Width", this.toolStripTextBoxResolutionX.Text);
			Preview.Xml.SetValue(contextNode, "PixelSize", this.m_cellSize.ToString());
			Preview.Xml.SetValue(contextNode, "Brightness", this.trackBarBrightness.Value.ToString());
			if (this.m_originalBackground != null) {
				if (this.m_backgroundImageFileName != string.Empty) {
					FileStream stream = new FileStream(this.m_backgroundImageFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
					byte[] buffer = new byte[stream.Length];
					stream.Read(buffer, 0, (int)stream.Length);
					this.m_setupNode.SelectSingleNode("BackgroundImage").InnerText = Convert.ToBase64String(buffer);
					stream.Close();
					stream.Dispose();
				}
			}
			else {
				this.m_setupNode.SelectSingleNode("BackgroundImage").InnerText = Convert.ToBase64String(new byte[0]);
			}
			XmlNode emptyNodeAlways = Preview.Xml.GetEmptyNodeAlways(this.m_setupNode, "Channels");
			List<byte> list = new List<byte>();
			foreach (int num in this.m_channelDictionary.Keys) {
				list.Clear();
				XmlNode node = Preview.Xml.SetNewValue(emptyNodeAlways, "Channel", string.Empty);
				Preview.Xml.SetAttribute(node, "number", (num + this.m_startChannel).ToString());
				foreach (uint num2 in this.m_channelDictionary[num]) {
					list.AddRange(BitConverter.GetBytes(num2));
				}
				node.InnerText = Convert.ToBase64String(list.ToArray());
			}
			this.m_setupData.SetBoolean(this.m_setupNode, "RedirectOutputs", this.checkBoxRedirectOutputs.Checked);
		}

		private void UpdateSizeUI() {
			this.toolStripTextBoxResolutionX.Text = (this.pictureBoxSetupGrid.Width / (this.m_cellSize + 1)).ToString();
			this.toolStripTextBoxResolutionY.Text = (this.pictureBoxSetupGrid.Height / (this.m_cellSize + 1)).ToString();
			this.toolStripComboBoxPixelSize.SelectedIndex = this.m_cellSize - 1;
		}
	}
}