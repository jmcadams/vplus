using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker {
    partial class NutcrackerModelDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.cbColorLayout = new ComboBox();
            this.lblColorLayout = new Label();
            this.chkBoxUseGroup = new CheckBox();
            this.cbGroups = new ComboBox();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            this.chkBoxDisplay = new CheckBox();
            this.rbLtoR = new RadioButton();
            this.rbRtoL = new RadioButton();
            this.panel1 = new Panel();
            this.lblStartChannel = new Label();
            this.lblDirection = new Label();
            this.lblModelName = new Label();
            this.cbPreviewAs = new ComboBox();
            this.lblPreviewAs = new Label();
            this.panel2 = new Panel();
            this.lblNotes = new Label();
            this.lblModelNameValue = new Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbColorLayout
            // 
            this.cbColorLayout.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbColorLayout.FormattingEnabled = true;
            this.cbColorLayout.Items.AddRange(new object[] {
            "RGB",
            "RBG",
            "GRB",
            "GBR",
            "BRG",
            "BGR"});
            this.cbColorLayout.Location = new Point(192, 346);
            this.cbColorLayout.Name = "cbColorLayout";
            this.cbColorLayout.Size = new Size(171, 21);
            this.cbColorLayout.TabIndex = 5;
            // 
            // lblColorLayout
            // 
            this.lblColorLayout.AutoSize = true;
            this.lblColorLayout.Location = new Point(84, 349);
            this.lblColorLayout.Name = "lblColorLayout";
            this.lblColorLayout.Size = new Size(102, 13);
            this.lblColorLayout.TabIndex = 13;
            this.lblColorLayout.Text = "Channel Color Order";
            // 
            // chkBoxUseGroup
            // 
            this.chkBoxUseGroup.AutoSize = true;
            this.chkBoxUseGroup.Checked = true;
            this.chkBoxUseGroup.CheckState = CheckState.Checked;
            this.chkBoxUseGroup.Location = new Point(109, 321);
            this.chkBoxUseGroup.Name = "chkBoxUseGroup";
            this.chkBoxUseGroup.Size = new Size(77, 17);
            this.chkBoxUseGroup.TabIndex = 3;
            this.chkBoxUseGroup.Text = "Use Group";
            this.chkBoxUseGroup.UseVisualStyleBackColor = true;
            this.chkBoxUseGroup.Visible = false;
            // 
            // cbGroups
            // 
            this.cbGroups.DrawMode = DrawMode.OwnerDrawFixed;
            this.cbGroups.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbGroups.FormattingEnabled = true;
            this.cbGroups.Location = new Point(192, 319);
            this.cbGroups.Name = "cbGroups";
            this.cbGroups.Size = new Size(171, 21);
            this.cbGroups.TabIndex = 4;
            this.cbGroups.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(288, 419);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = DialogResult.OK;
            this.btnOk.Location = new Point(207, 419);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            // 
            // chkBoxDisplay
            // 
            this.chkBoxDisplay.AutoSize = true;
            this.chkBoxDisplay.Checked = true;
            this.chkBoxDisplay.CheckState = CheckState.Checked;
            this.chkBoxDisplay.Location = new Point(255, 396);
            this.chkBoxDisplay.Name = "chkBoxDisplay";
            this.chkBoxDisplay.Size = new Size(108, 17);
            this.chkBoxDisplay.TabIndex = 7;
            this.chkBoxDisplay.Text = "Part of my display";
            this.chkBoxDisplay.UseVisualStyleBackColor = true;
            this.chkBoxDisplay.Visible = false;
            // 
            // rbLtoR
            // 
            this.rbLtoR.AutoSize = true;
            this.rbLtoR.Checked = true;
            this.rbLtoR.Location = new Point(110, 3);
            this.rbLtoR.Name = "rbLtoR";
            this.rbLtoR.Size = new Size(54, 17);
            this.rbLtoR.TabIndex = 0;
            this.rbLtoR.TabStop = true;
            this.rbLtoR.Text = "L to R";
            this.rbLtoR.UseVisualStyleBackColor = true;
            // 
            // rbRtoL
            // 
            this.rbRtoL.AutoSize = true;
            this.rbRtoL.Location = new Point(170, 3);
            this.rbRtoL.Name = "rbRtoL";
            this.rbRtoL.Size = new Size(54, 17);
            this.rbRtoL.TabIndex = 1;
            this.rbRtoL.Text = "R to L";
            this.rbRtoL.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Location = new Point(13, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(350, 250);
            this.panel1.TabIndex = 2;
            // 
            // lblStartChannel
            // 
            this.lblStartChannel.AutoSize = true;
            this.lblStartChannel.Location = new Point(20, 322);
            this.lblStartChannel.Name = "lblStartChannel";
            this.lblStartChannel.Size = new Size(83, 13);
            this.lblStartChannel.TabIndex = 12;
            this.lblStartChannel.Text = "Start Channel or";
            this.lblStartChannel.Visible = false;
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Location = new Point(55, 5);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new Size(49, 13);
            this.lblDirection.TabIndex = 2;
            this.lblDirection.Text = "Direction";
            // 
            // lblModelName
            // 
            this.lblModelName.AutoSize = true;
            this.lblModelName.Location = new Point(119, 12);
            this.lblModelName.Name = "lblModelName";
            this.lblModelName.Size = new Size(67, 13);
            this.lblModelName.TabIndex = 10;
            this.lblModelName.Text = "Model Name";
            // 
            // cbPreviewAs
            // 
            this.cbPreviewAs.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbPreviewAs.FormattingEnabled = true;
            this.cbPreviewAs.Location = new Point(192, 36);
            this.cbPreviewAs.Name = "cbPreviewAs";
            this.cbPreviewAs.Size = new Size(171, 21);
            this.cbPreviewAs.TabIndex = 1;
            this.cbPreviewAs.SelectedIndexChanged += new EventHandler(this.cbPreviewAs_SelectedIndexChanged);
            // 
            // lblPreviewAs
            // 
            this.lblPreviewAs.AutoSize = true;
            this.lblPreviewAs.Location = new Point(126, 39);
            this.lblPreviewAs.Name = "lblPreviewAs";
            this.lblPreviewAs.Size = new Size(60, 13);
            this.lblPreviewAs.TabIndex = 11;
            this.lblPreviewAs.Text = "Preview As";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblDirection);
            this.panel2.Controls.Add(this.rbLtoR);
            this.panel2.Controls.Add(this.rbRtoL);
            this.panel2.Location = new Point(136, 370);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(227, 23);
            this.panel2.TabIndex = 6;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new Point(12, 9);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(101, 48);
            this.lblNotes.TabIndex = 14;
            // 
            // lblModelNameValue
            // 
            this.lblModelNameValue.AutoSize = true;
            this.lblModelNameValue.Location = new Point(192, 12);
            this.lblModelNameValue.Name = "lblModelNameValue";
            this.lblModelNameValue.Size = new Size(106, 13);
            this.lblModelNameValue.TabIndex = 15;
            this.lblModelNameValue.Text = "Nothing here rat king";
            // 
            // NutcrackerModelDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(375, 454);
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
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Nutcracker Model Management";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox cbColorLayout;
        private Label lblColorLayout;
        private CheckBox chkBoxUseGroup;
        private ComboBox cbGroups;
        private Button btnCancel;
        private Button btnOk;
        private CheckBox chkBoxDisplay;
        private RadioButton rbLtoR;
        private RadioButton rbRtoL;
        private Panel panel1;
        private Label lblStartChannel;
        private Label lblDirection;
        private Label lblModelName;
        private ComboBox cbPreviewAs;
        private Label lblPreviewAs;
        private Panel panel2;
        private Label lblNotes;
        private Label lblModelNameValue;
    }
}