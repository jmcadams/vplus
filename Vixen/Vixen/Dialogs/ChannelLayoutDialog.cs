namespace Vixen.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Vixen;
    using VixenControls;

    internal class ChannelLayoutDialog : Form
    {
        private IContainer components = null;
        private ListBox listBoxChannels;
        private ListBox listBoxPlugins;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private PictureBox pictureBoxMiniMap;
        private Toolbox toolbox1;
        private VectorImageStrip vectorImageStrip1;

        public ChannelLayoutDialog(IExecutable executableObject)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetTerminalDirectory(string directoryPath)
        {
            if (directoryPath.EndsWith(@"\"))
            {
                directoryPath = directoryPath.TrimEnd(new char[] { '\\' });
            }
            return directoryPath.Substring(directoryPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.vectorImageStrip1 = new VectorImageStrip();
            this.listBoxPlugins = new ListBox();
            this.toolbox1 = new Toolbox();
            this.listBoxChannels = new ListBox();
            this.pictureBoxMiniMap = new PictureBox();
            this.panel2 = new Panel();
            this.panel3 = new Panel();
            this.panel1.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxMiniMap).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BackColor = Color.LightGray;
            this.panel1.Controls.Add(this.vectorImageStrip1);
            this.panel1.Controls.Add(this.listBoxPlugins);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x1bd);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x318, 0x79);
            this.panel1.TabIndex = 1;
            this.vectorImageStrip1.AllowDrop = true;
            this.vectorImageStrip1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.vectorImageStrip1.Location = new Point(160, 10);
            this.vectorImageStrip1.Name = "vectorImageStrip1";
            this.vectorImageStrip1.Size = new Size(620, 0x67);
            this.vectorImageStrip1.TabIndex = 2;
            this.vectorImageStrip1.DragOver += new DragEventHandler(this.vectorImageStrip1_DragOver);
            this.vectorImageStrip1.DragDrop += new DragEventHandler(this.vectorImageStrip1_DragDrop);
            this.listBoxPlugins.BackColor = Color.LightGray;
            this.listBoxPlugins.BorderStyle = BorderStyle.None;
            this.listBoxPlugins.ForeColor = Color.Red;
            this.listBoxPlugins.FormattingEnabled = true;
            this.listBoxPlugins.Location = new Point(6, 6);
            this.listBoxPlugins.Name = "listBoxPlugins";
            this.listBoxPlugins.Size = new Size(0x91, 0x68);
            this.listBoxPlugins.TabIndex = 1;
            this.listBoxPlugins.SelectedIndexChanged += new EventHandler(this.listBoxPlugins_SelectedIndexChanged);
            this.toolbox1.Dock = DockStyle.Left;
            this.toolbox1.Location = new Point(0, 0);
            this.toolbox1.Name = "toolbox1";
            this.toolbox1.Size = new Size(0x9a, 0x1bd);
            this.toolbox1.TabIndex = 2;
            this.listBoxChannels.BorderStyle = BorderStyle.None;
            this.listBoxChannels.Dock = DockStyle.Fill;
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new Point(0, 0);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.Size = new Size(0x99, 0x138);
            this.listBoxChannels.TabIndex = 3;
            this.pictureBoxMiniMap.Dock = DockStyle.Top;
            this.pictureBoxMiniMap.Location = new Point(0, 0);
            this.pictureBoxMiniMap.Name = "pictureBoxMiniMap";
            this.pictureBoxMiniMap.Size = new Size(0xa6, 0x79);
            this.pictureBoxMiniMap.TabIndex = 4;
            this.pictureBoxMiniMap.TabStop = false;
            this.panel2.BackColor = Color.White;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.pictureBoxMiniMap);
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(0x272, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0xa6, 0x1bd);
            this.panel2.TabIndex = 5;
            this.panel3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel3.Controls.Add(this.listBoxChannels);
            this.panel3.Location = new Point(10, 0x81);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x99, 0x13c);
            this.panel3.TabIndex = 5;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x318, 0x236);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.toolbox1);
            base.Controls.Add(this.panel1);
            base.Name = "ChannelLayoutDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Channel Layout";
            this.panel1.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxMiniMap).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void listBoxPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void vectorImageStrip1_DragDrop(object sender, DragEventArgs e)
        {
        }

        private void vectorImageStrip1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Controller)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}

