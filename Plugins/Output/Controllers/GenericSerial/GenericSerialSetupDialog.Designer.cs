using System.Windows.Forms;

namespace Controllers.GenericSerial {
    internal partial class GenericSerialSetupDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private CheckBox cbFooter;
        private CheckBox cbHeader;
        private GroupBox gbPacketData;
        private TextBox tbFooter;
        private TextBox tbHeader;


        private void InitializeComponent() {
            this.gbPacketData = new System.Windows.Forms.GroupBox();
            this.tbFooter = new System.Windows.Forms.TextBox();
            this.cbFooter = new System.Windows.Forms.CheckBox();
            this.tbHeader = new System.Windows.Forms.TextBox();
            this.cbHeader = new System.Windows.Forms.CheckBox();
            this.serialSetup1 = new Controllers.Common.SerialSetup();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.gbPacketData.Location = new System.Drawing.Point(339, 13);
            this.gbPacketData.Name = "gbPacketData";
            this.gbPacketData.Size = new System.Drawing.Size(288, 129);
            this.gbPacketData.TabIndex = 3;
            this.gbPacketData.TabStop = false;
            this.gbPacketData.Text = "Packet Data";
            // 
            // tbFooter
            // 
            this.tbFooter.Location = new System.Drawing.Point(32, 98);
            this.tbFooter.Name = "tbFooter";
            this.tbFooter.Size = new System.Drawing.Size(204, 20);
            this.tbFooter.TabIndex = 3;
            // 
            // cbFooter
            // 
            this.cbFooter.AutoSize = true;
            this.cbFooter.Location = new System.Drawing.Point(13, 75);
            this.cbFooter.Name = "cbFooter";
            this.cbFooter.Size = new System.Drawing.Size(176, 17);
            this.cbFooter.TabIndex = 2;
            this.cbFooter.Text = "Each packet sends this footer...";
            this.cbFooter.UseVisualStyleBackColor = true;
            // 
            // tbHeader
            // 
            this.tbHeader.Location = new System.Drawing.Point(32, 49);
            this.tbHeader.Name = "tbHeader";
            this.tbHeader.Size = new System.Drawing.Size(204, 20);
            this.tbHeader.TabIndex = 1;
            // 
            // cbHeader
            // 
            this.cbHeader.AutoSize = true;
            this.cbHeader.Location = new System.Drawing.Point(13, 26);
            this.cbHeader.Name = "cbHeader";
            this.cbHeader.Size = new System.Drawing.Size(182, 17);
            this.cbHeader.TabIndex = 0;
            this.cbHeader.Text = "Each packet sends this header...";
            this.cbHeader.UseVisualStyleBackColor = true;
            // 
            // serialSetup1
            // 
            this.serialSetup1.Location = new System.Drawing.Point(6, 19);
            this.serialSetup1.Name = "serialSetup1";
            this.serialSetup1.SelectedPorts = null;
            this.serialSetup1.Size = new System.Drawing.Size(308, 140);
            this.serialSetup1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.serialSetup1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 175);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // DialogSerialSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbPacketData);
            this.Name = "GenericSerialSetupDialog";
            this.Size = new System.Drawing.Size(642, 200);
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

        private Common.SerialSetup serialSetup1;
        private GroupBox groupBox1;
    }
}
