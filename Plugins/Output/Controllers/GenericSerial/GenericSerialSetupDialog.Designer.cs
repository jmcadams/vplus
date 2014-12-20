using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Controllers.Common;

namespace Controllers.GenericSerial {
    internal partial class GenericSerialSetupDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private CheckBox cbFooter;
        private CheckBox cbHeader;
        private GroupBox gbPacketData;
        private TextBox tbFooter;
        private TextBox tbHeader;


        private void InitializeComponent() {
            this.gbPacketData = new GroupBox();
            this.tbFooter = new TextBox();
            this.cbFooter = new CheckBox();
            this.tbHeader = new TextBox();
            this.cbHeader = new CheckBox();
            this.serialSetup1 = new SerialSetup();
            this.groupBox1 = new GroupBox();
            this.gbPacketData.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPacketData
            // 
            this.gbPacketData.Controls.Add(this.tbFooter);
            this.gbPacketData.Controls.Add(this.cbFooter);
            this.gbPacketData.Controls.Add(this.tbHeader);
            this.gbPacketData.Controls.Add(this.cbHeader);
            this.gbPacketData.Location = new Point(339, 13);
            this.gbPacketData.Name = "gbPacketData";
            this.gbPacketData.Size = new Size(288, 129);
            this.gbPacketData.TabIndex = 3;
            this.gbPacketData.TabStop = false;
            this.gbPacketData.Text = "Packet Data";
            // 
            // tbFooter
            // 
            this.tbFooter.Location = new Point(32, 98);
            this.tbFooter.Name = "tbFooter";
            this.tbFooter.Size = new Size(204, 20);
            this.tbFooter.TabIndex = 3;
            // 
            // cbFooter
            // 
            this.cbFooter.AutoSize = true;
            this.cbFooter.Location = new Point(13, 75);
            this.cbFooter.Name = "cbFooter";
            this.cbFooter.Size = new Size(176, 17);
            this.cbFooter.TabIndex = 2;
            this.cbFooter.Text = "Each packet sends this footer...";
            this.cbFooter.UseVisualStyleBackColor = true;
            // 
            // tbHeader
            // 
            this.tbHeader.Location = new Point(32, 49);
            this.tbHeader.Name = "tbHeader";
            this.tbHeader.Size = new Size(204, 20);
            this.tbHeader.TabIndex = 1;
            // 
            // cbHeader
            // 
            this.cbHeader.AutoSize = true;
            this.cbHeader.Location = new Point(13, 26);
            this.cbHeader.Name = "cbHeader";
            this.cbHeader.Size = new Size(182, 17);
            this.cbHeader.TabIndex = 0;
            this.cbHeader.Text = "Each packet sends this header...";
            this.cbHeader.UseVisualStyleBackColor = true;
            // 
            // serialSetup1
            // 
            this.serialSetup1.Location = new Point(6, 19);
            this.serialSetup1.Name = "serialSetup1";
            this.serialSetup1.SelectedPorts = null;
            this.serialSetup1.Size = new Size(308, 140);
            this.serialSetup1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.serialSetup1);
            this.groupBox1.Location = new Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(320, 175);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // DialogSerialSetup
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbPacketData);
            this.Name = "GenericSerialSetupDialog";
            this.Size = new Size(642, 200);
            this.gbPacketData.ResumeLayout(false);
            this.gbPacketData.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private SerialSetup serialSetup1;
        private GroupBox groupBox1;
    }
}
