namespace VixenPlus.Dialogs{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.Collections;

	internal partial class ChannelLayoutDialog{
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private ListBox listBoxChannels;
private ListBox listBoxPlugins;
private Panel panel1;
private Panel panel2;
private Panel panel3;
private PictureBox pictureBoxMiniMap;
private VixenControls.Toolbox toolbox1;
private VixenControls.VectorImageStrip vectorImageStrip1;

		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.vectorImageStrip1 = new VixenControls.VectorImageStrip();
			this.listBoxPlugins = new System.Windows.Forms.ListBox();
			this.toolbox1 = new VixenControls.Toolbox();
			this.listBoxChannels = new System.Windows.Forms.ListBox();
			this.pictureBoxMiniMap = new System.Windows.Forms.PictureBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxMiniMap)).BeginInit();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.LightGray;
			this.panel1.Controls.Add(this.vectorImageStrip1);
			this.panel1.Controls.Add(this.listBoxPlugins);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 445);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(792, 121);
			this.panel1.TabIndex = 1;
			// 
			// vectorImageStrip1
			// 
			this.vectorImageStrip1.AllowDrop = true;
			this.vectorImageStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.vectorImageStrip1.Location = new System.Drawing.Point(160, 10);
			this.vectorImageStrip1.Name = "vectorImageStrip1";
			this.vectorImageStrip1.Size = new System.Drawing.Size(620, 103);
			this.vectorImageStrip1.TabIndex = 2;
			this.vectorImageStrip1.DragDrop += new System.Windows.Forms.DragEventHandler(this.vectorImageStrip1_DragDrop);
			this.vectorImageStrip1.DragOver += new System.Windows.Forms.DragEventHandler(this.vectorImageStrip1_DragOver);
			// 
			// listBoxPlugins
			// 
			this.listBoxPlugins.BackColor = System.Drawing.Color.LightGray;
			this.listBoxPlugins.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBoxPlugins.ForeColor = System.Drawing.Color.Red;
			this.listBoxPlugins.FormattingEnabled = true;
			this.listBoxPlugins.Location = new System.Drawing.Point(6, 6);
			this.listBoxPlugins.Name = "listBoxPlugins";
			this.listBoxPlugins.Size = new System.Drawing.Size(145, 104);
			this.listBoxPlugins.TabIndex = 1;
			this.listBoxPlugins.SelectedIndexChanged += new System.EventHandler(this.listBoxPlugins_SelectedIndexChanged);
			// 
			// toolbox1
			// 
			this.toolbox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.toolbox1.Location = new System.Drawing.Point(0, 0);
			this.toolbox1.Name = "toolbox1";
			this.toolbox1.Size = new System.Drawing.Size(154, 445);
			this.toolbox1.TabIndex = 2;
			// 
			// listBoxChannels
			// 
			this.listBoxChannels.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBoxChannels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxChannels.FormattingEnabled = true;
			this.listBoxChannels.Location = new System.Drawing.Point(0, 0);
			this.listBoxChannels.Name = "listBoxChannels";
			this.listBoxChannels.Size = new System.Drawing.Size(153, 316);
			this.listBoxChannels.TabIndex = 3;
			// 
			// pictureBoxMiniMap
			// 
			this.pictureBoxMiniMap.Dock = System.Windows.Forms.DockStyle.Top;
			this.pictureBoxMiniMap.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxMiniMap.Name = "pictureBoxMiniMap";
			this.pictureBoxMiniMap.Size = new System.Drawing.Size(166, 121);
			this.pictureBoxMiniMap.TabIndex = 4;
			this.pictureBoxMiniMap.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.White;
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Controls.Add(this.pictureBoxMiniMap);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(626, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(166, 445);
			this.panel2.TabIndex = 5;
			// 
			// panel3
			// 
			this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel3.Controls.Add(this.listBoxChannels);
			this.panel3.Location = new System.Drawing.Point(10, 129);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(153, 316);
			this.panel3.TabIndex = 5;
			// 
			// ChannelLayoutDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.toolbox1);
			this.Controls.Add(this.panel1);
			this.Name = "ChannelLayoutDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Channel Layout";
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxMiniMap)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
