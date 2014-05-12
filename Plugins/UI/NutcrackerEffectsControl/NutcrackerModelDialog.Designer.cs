namespace Nutcracker {
    partial class NutcrackerModelDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.cbColorLayout = new System.Windows.Forms.ComboBox();
            this.lblColorLayout = new System.Windows.Forms.Label();
            this.chkBoxUseGroup = new System.Windows.Forms.CheckBox();
            this.cbGroups = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chkBoxDisplay = new System.Windows.Forms.CheckBox();
            this.rbLtoR = new System.Windows.Forms.RadioButton();
            this.rbRtoL = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStartChannel = new System.Windows.Forms.Label();
            this.lblDirection = new System.Windows.Forms.Label();
            this.lblModelName = new System.Windows.Forms.Label();
            this.cbPreviewAs = new System.Windows.Forms.ComboBox();
            this.lblPreviewAs = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblNotes = new System.Windows.Forms.Label();
            this.lblModelNameValue = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbColorLayout
            // 
            this.cbColorLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColorLayout.FormattingEnabled = true;
            this.cbColorLayout.Items.AddRange(new object[] {
            "RGB",
            "RBG",
            "GRB",
            "GBR",
            "BRG",
            "BGR"});
            this.cbColorLayout.Location = new System.Drawing.Point(192, 346);
            this.cbColorLayout.Name = "cbColorLayout";
            this.cbColorLayout.Size = new System.Drawing.Size(171, 21);
            this.cbColorLayout.TabIndex = 5;
            // 
            // lblColorLayout
            // 
            this.lblColorLayout.AutoSize = true;
            this.lblColorLayout.Location = new System.Drawing.Point(84, 349);
            this.lblColorLayout.Name = "lblColorLayout";
            this.lblColorLayout.Size = new System.Drawing.Size(102, 13);
            this.lblColorLayout.TabIndex = 13;
            this.lblColorLayout.Text = "Channel Color Order";
            // 
            // chkBoxUseGroup
            // 
            this.chkBoxUseGroup.AutoSize = true;
            this.chkBoxUseGroup.Checked = true;
            this.chkBoxUseGroup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxUseGroup.Location = new System.Drawing.Point(109, 321);
            this.chkBoxUseGroup.Name = "chkBoxUseGroup";
            this.chkBoxUseGroup.Size = new System.Drawing.Size(77, 17);
            this.chkBoxUseGroup.TabIndex = 3;
            this.chkBoxUseGroup.Text = "Use Group";
            this.chkBoxUseGroup.UseVisualStyleBackColor = true;
            this.chkBoxUseGroup.Visible = false;
            // 
            // cbGroups
            // 
            this.cbGroups.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroups.FormattingEnabled = true;
            this.cbGroups.Location = new System.Drawing.Point(192, 319);
            this.cbGroups.Name = "cbGroups";
            this.cbGroups.Size = new System.Drawing.Size(171, 21);
            this.cbGroups.TabIndex = 4;
            this.cbGroups.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(288, 419);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(207, 419);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chkBoxDisplay
            // 
            this.chkBoxDisplay.AutoSize = true;
            this.chkBoxDisplay.Checked = true;
            this.chkBoxDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxDisplay.Location = new System.Drawing.Point(255, 396);
            this.chkBoxDisplay.Name = "chkBoxDisplay";
            this.chkBoxDisplay.Size = new System.Drawing.Size(108, 17);
            this.chkBoxDisplay.TabIndex = 7;
            this.chkBoxDisplay.Text = "Part of my display";
            this.chkBoxDisplay.UseVisualStyleBackColor = true;
            this.chkBoxDisplay.Visible = false;
            // 
            // rbLtoR
            // 
            this.rbLtoR.AutoSize = true;
            this.rbLtoR.Checked = true;
            this.rbLtoR.Location = new System.Drawing.Point(110, 3);
            this.rbLtoR.Name = "rbLtoR";
            this.rbLtoR.Size = new System.Drawing.Size(54, 17);
            this.rbLtoR.TabIndex = 0;
            this.rbLtoR.TabStop = true;
            this.rbLtoR.Text = "L to R";
            this.rbLtoR.UseVisualStyleBackColor = true;
            // 
            // rbRtoL
            // 
            this.rbRtoL.AutoSize = true;
            this.rbRtoL.Location = new System.Drawing.Point(170, 3);
            this.rbRtoL.Name = "rbRtoL";
            this.rbRtoL.Size = new System.Drawing.Size(54, 17);
            this.rbRtoL.TabIndex = 1;
            this.rbRtoL.Text = "R to L";
            this.rbRtoL.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(13, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 250);
            this.panel1.TabIndex = 2;
            // 
            // lblStartChannel
            // 
            this.lblStartChannel.AutoSize = true;
            this.lblStartChannel.Location = new System.Drawing.Point(20, 322);
            this.lblStartChannel.Name = "lblStartChannel";
            this.lblStartChannel.Size = new System.Drawing.Size(83, 13);
            this.lblStartChannel.TabIndex = 12;
            this.lblStartChannel.Text = "Start Channel or";
            this.lblStartChannel.Visible = false;
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Location = new System.Drawing.Point(55, 5);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(49, 13);
            this.lblDirection.TabIndex = 2;
            this.lblDirection.Text = "Direction";
            // 
            // lblModelName
            // 
            this.lblModelName.AutoSize = true;
            this.lblModelName.Location = new System.Drawing.Point(119, 12);
            this.lblModelName.Name = "lblModelName";
            this.lblModelName.Size = new System.Drawing.Size(67, 13);
            this.lblModelName.TabIndex = 10;
            this.lblModelName.Text = "Model Name";
            // 
            // cbPreviewAs
            // 
            this.cbPreviewAs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPreviewAs.FormattingEnabled = true;
            this.cbPreviewAs.Location = new System.Drawing.Point(192, 36);
            this.cbPreviewAs.Name = "cbPreviewAs";
            this.cbPreviewAs.Size = new System.Drawing.Size(171, 21);
            this.cbPreviewAs.TabIndex = 1;
            this.cbPreviewAs.SelectedIndexChanged += new System.EventHandler(this.cbPreviewAs_SelectedIndexChanged);
            // 
            // lblPreviewAs
            // 
            this.lblPreviewAs.AutoSize = true;
            this.lblPreviewAs.Location = new System.Drawing.Point(126, 39);
            this.lblPreviewAs.Name = "lblPreviewAs";
            this.lblPreviewAs.Size = new System.Drawing.Size(60, 13);
            this.lblPreviewAs.TabIndex = 11;
            this.lblPreviewAs.Text = "Preview As";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblDirection);
            this.panel2.Controls.Add(this.rbLtoR);
            this.panel2.Controls.Add(this.rbRtoL);
            this.panel2.Location = new System.Drawing.Point(136, 370);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 23);
            this.panel2.TabIndex = 6;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(12, 9);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(101, 48);
            this.lblNotes.TabIndex = 14;
            // 
            // lblModelNameValue
            // 
            this.lblModelNameValue.AutoSize = true;
            this.lblModelNameValue.Location = new System.Drawing.Point(192, 12);
            this.lblModelNameValue.Name = "lblModelNameValue";
            this.lblModelNameValue.Size = new System.Drawing.Size(106, 13);
            this.lblModelNameValue.TabIndex = 15;
            this.lblModelNameValue.Text = "Nothing here rat king";
            // 
            // NutcrackerModelDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 454);
            this.ControlBox = false;
            this.Controls.Add(this.lblModelNameValue);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.cbPreviewAs);
            this.Controls.Add(this.lblPreviewAs);
            this.Controls.Add(this.lblModelName);
            this.Controls.Add(this.lblStartChannel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkBoxDisplay);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkBoxUseGroup);
            this.Controls.Add(this.cbGroups);
            this.Controls.Add(this.cbColorLayout);
            this.Controls.Add(this.lblColorLayout);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NutcrackerModelDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nutcracker Model Management";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbColorLayout;
        private System.Windows.Forms.Label lblColorLayout;
        private System.Windows.Forms.CheckBox chkBoxUseGroup;
        private System.Windows.Forms.ComboBox cbGroups;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkBoxDisplay;
        private System.Windows.Forms.RadioButton rbLtoR;
        private System.Windows.Forms.RadioButton rbRtoL;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStartChannel;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.Label lblModelName;
        private System.Windows.Forms.ComboBox cbPreviewAs;
        private System.Windows.Forms.Label lblPreviewAs;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Label lblModelNameValue;
    }
}