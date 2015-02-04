using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    internal partial class TestChannelsDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonDone;
        private Button buttonSelectAll;
        private Button buttonUnselectAll;
        private Label labelLevel;
        private ListBox listBoxChannels;
        private TrackBar trackBar;

        private void InitializeComponent() {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TestChannelsDialog));
            this.buttonDone = new Button();
            this.listBoxChannels = new ListBox();
            this.labelLevel = new Label();
            this.trackBar = new TrackBar();
            this.buttonUnselectAll = new Button();
            this.buttonSelectAll = new Button();
            ((ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonDone
            // 
            this.buttonDone.DialogResult = DialogResult.OK;
            this.buttonDone.Location = new Point(191, 376);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(75, 23);
            this.buttonDone.TabIndex = 3;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new EventHandler(this.buttonDone_Click);
            // 
            // listBoxChannels
            // 
            this.listBoxChannels.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new Point(12, 12);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxChannels.Size = new Size(254, 355);
            this.listBoxChannels.TabIndex = 8;
            this.listBoxChannels.DrawItem += new DrawItemEventHandler(this.listBox_DrawItem);
            this.listBoxChannels.SelectedIndexChanged += new EventHandler(this.listBoxChannels_SelectedIndexChanged);
            // 
            // labelLevel
            // 
            this.labelLevel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.labelLevel.Location = new Point(285, 376);
            this.labelLevel.Name = "labelLevel";
            this.labelLevel.Size = new Size(45, 20);
            this.labelLevel.TabIndex = 7;
            this.labelLevel.Text = "0";
            this.labelLevel.TextAlign = ContentAlignment.TopCenter;
            // 
            // trackBar
            // 
            this.trackBar.Location = new Point(285, 12);
            this.trackBar.Maximum = 100;
            this.trackBar.Name = "trackBar";
            this.trackBar.Orientation = Orientation.Vertical;
            this.trackBar.Size = new Size(45, 355);
            this.trackBar.TabIndex = 6;
            this.trackBar.TickFrequency = 15;
            this.trackBar.TickStyle = TickStyle.Both;
            this.trackBar.ValueChanged += new EventHandler(this.trackBar_ValueChanged);
            // 
            // buttonUnselectAll
            // 
            this.buttonUnselectAll.Location = new Point(93, 376);
            this.buttonUnselectAll.Name = "buttonUnselectAll";
            this.buttonUnselectAll.Size = new Size(75, 23);
            this.buttonUnselectAll.TabIndex = 5;
            this.buttonUnselectAll.Text = "Unselect all";
            this.buttonUnselectAll.UseVisualStyleBackColor = true;
            this.buttonUnselectAll.Click += new EventHandler(this.buttonAllOff_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new Point(12, 376);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new Size(75, 23);
            this.buttonSelectAll.TabIndex = 4;
            this.buttonSelectAll.Text = "Select all";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new EventHandler(this.buttonAllOn_Click);
            // 
            // TestChannelsDialog
            // 
            this.AcceptButton = this.buttonDone;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonDone;
            this.ClientSize = new Size(342, 411);
            this.Controls.Add(this.listBoxChannels);
            this.Controls.Add(this.labelLevel);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.buttonUnselectAll);
            this.Controls.Add(this.buttonSelectAll);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TestChannelsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Test Channels";
            this.FormClosing += new FormClosingEventHandler(this.TestChannelsDialog_FormClosing);
            ((ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}