using System.ComponentModel;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    internal partial class DimmingCurveDialog {
        private IContainer components;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonExportToLibrary;
        private Button buttonImportFromLibrary;
        private Button buttonOK;
        private Button buttonResetToLinear;
        private Button buttonSwitchDisplay;
        private ComboBox comboBoxChannels;
        private ComboBox comboBoxExport;
        private ComboBox comboBoxImport;
        private Label label1;
        private Label label2;
        private Label labelChannelValue;
        private Label labelMessage;
        private Label labelSequenceChannels;
        private Panel panel1;
        private PictureBox pictureBoxCurve;
        private PictureBox pbMini;


        private void InitializeComponent() {
            this.pbMini = new System.Windows.Forms.PictureBox();
            this.pictureBoxCurve = new System.Windows.Forms.PictureBox();
            this.labelChannelValue = new System.Windows.Forms.Label();
            this.buttonResetToLinear = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBoxChannels = new System.Windows.Forms.ComboBox();
            this.labelSequenceChannels = new System.Windows.Forms.Label();
            this.buttonSwitchDisplay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonImportFromLibrary = new System.Windows.Forms.Button();
            this.buttonExportToLibrary = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxImport = new System.Windows.Forms.ComboBox();
            this.comboBoxExport = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbMini)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurve)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbMini
            // 
            this.pbMini.BackColor = System.Drawing.Color.White;
            this.pbMini.Location = new System.Drawing.Point(0, 0);
            this.pbMini.Name = "pbMini";
            this.pbMini.Size = new System.Drawing.Size(256, 256);
            this.pbMini.TabIndex = 0;
            this.pbMini.TabStop = false;
            this.pbMini.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxMini_Paint);
            this.pbMini.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMini_MouseDown);
            this.pbMini.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMini_MouseMove);
            // 
            // pictureBoxCurve
            // 
            this.pictureBoxCurve.BackColor = System.Drawing.Color.FloralWhite;
            this.pictureBoxCurve.Location = new System.Drawing.Point(274, 31);
            this.pictureBoxCurve.Name = "pictureBoxCurve";
            this.pictureBoxCurve.Size = new System.Drawing.Size(449, 449);
            this.pictureBoxCurve.TabIndex = 1;
            this.pictureBoxCurve.TabStop = false;
            this.pictureBoxCurve.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxCurve_Paint);
            this.pictureBoxCurve.MouseLeave += new System.EventHandler(this.pictureBoxCurve_MouseLeave);
            this.pictureBoxCurve.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCurve_MouseMove);
            this.pictureBoxCurve.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCurve_MouseUp);
            // 
            // labelChannelValue
            // 
            this.labelChannelValue.AutoSize = true;
            this.labelChannelValue.Location = new System.Drawing.Point(277, 13);
            this.labelChannelValue.Name = "labelChannelValue";
            this.labelChannelValue.Size = new System.Drawing.Size(0, 13);
            this.labelChannelValue.TabIndex = 13;
            // 
            // buttonResetToLinear
            // 
            this.buttonResetToLinear.Enabled = false;
            this.buttonResetToLinear.Location = new System.Drawing.Point(12, 293);
            this.buttonResetToLinear.Name = "buttonResetToLinear";
            this.buttonResetToLinear.Size = new System.Drawing.Size(99, 23);
            this.buttonResetToLinear.TabIndex = 6;
            this.buttonResetToLinear.Text = "Reset to linear";
            this.buttonResetToLinear.UseVisualStyleBackColor = true;
            this.buttonResetToLinear.Click += new System.EventHandler(this.buttonResetToLinear_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(568, 491);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(649, 491);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // comboBoxChannels
            // 
            this.comboBoxChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChannels.FormattingEnabled = true;
            this.comboBoxChannels.Location = new System.Drawing.Point(15, 353);
            this.comboBoxChannels.Name = "comboBoxChannels";
            this.comboBoxChannels.Size = new System.Drawing.Size(253, 21);
            this.comboBoxChannels.TabIndex = 1;
            this.comboBoxChannels.SelectedIndexChanged += new System.EventHandler(this.comboBoxChannels_SelectedIndexChanged);
            // 
            // labelSequenceChannels
            // 
            this.labelSequenceChannels.AutoSize = true;
            this.labelSequenceChannels.Location = new System.Drawing.Point(12, 337);
            this.labelSequenceChannels.Name = "labelSequenceChannels";
            this.labelSequenceChannels.Size = new System.Drawing.Size(102, 13);
            this.labelSequenceChannels.TabIndex = 0;
            this.labelSequenceChannels.Text = "Sequence channels";
            // 
            // buttonSwitchDisplay
            // 
            this.buttonSwitchDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSwitchDisplay.Location = new System.Drawing.Point(356, 501);
            this.buttonSwitchDisplay.Name = "buttonSwitchDisplay";
            this.buttonSwitchDisplay.Size = new System.Drawing.Size(120, 23);
            this.buttonSwitchDisplay.TabIndex = 14;
            this.buttonSwitchDisplay.Text = "Show";
            this.buttonSwitchDisplay.UseVisualStyleBackColor = true;
            this.buttonSwitchDisplay.Visible = false;
            this.buttonSwitchDisplay.Click += new System.EventHandler(this.buttonSwitchDisplay_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(277, 483);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Input >";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(258, 479);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 9;
            // 
            // buttonImportFromLibrary
            // 
            this.buttonImportFromLibrary.Location = new System.Drawing.Point(15, 396);
            this.buttonImportFromLibrary.Name = "buttonImportFromLibrary";
            this.buttonImportFromLibrary.Size = new System.Drawing.Size(75, 23);
            this.buttonImportFromLibrary.TabIndex = 2;
            this.buttonImportFromLibrary.Text = "Import";
            this.buttonImportFromLibrary.UseVisualStyleBackColor = true;
            this.buttonImportFromLibrary.Click += new System.EventHandler(this.buttonImportFromLibrary_Click);
            // 
            // buttonExportToLibrary
            // 
            this.buttonExportToLibrary.Location = new System.Drawing.Point(15, 425);
            this.buttonExportToLibrary.Name = "buttonExportToLibrary";
            this.buttonExportToLibrary.Size = new System.Drawing.Size(75, 23);
            this.buttonExportToLibrary.TabIndex = 4;
            this.buttonExportToLibrary.Text = "Export";
            this.buttonExportToLibrary.UseVisualStyleBackColor = true;
            this.buttonExportToLibrary.Click += new System.EventHandler(this.buttonExportToLibrary_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(12, 496);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(0, 13);
            this.labelMessage.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pbMini);
            this.panel1.Location = new System.Drawing.Point(12, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 258);
            this.panel1.TabIndex = 11;
            // 
            // comboBoxImport
            // 
            this.comboBoxImport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxImport.FormattingEnabled = true;
            this.comboBoxImport.Items.AddRange(new object[] {
            "curve from the library",
            "curves from a file"});
            this.comboBoxImport.Location = new System.Drawing.Point(96, 398);
            this.comboBoxImport.Name = "comboBoxImport";
            this.comboBoxImport.Size = new System.Drawing.Size(133, 21);
            this.comboBoxImport.TabIndex = 3;
            // 
            // comboBoxExport
            // 
            this.comboBoxExport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExport.FormattingEnabled = true;
            this.comboBoxExport.Items.AddRange(new object[] {
            "curve to the library",
            "curves to a file"});
            this.comboBoxExport.Location = new System.Drawing.Point(96, 427);
            this.comboBoxExport.Name = "comboBoxExport";
            this.comboBoxExport.Size = new System.Drawing.Size(133, 21);
            this.comboBoxExport.TabIndex = 5;
            // 
            // DimmingCurveDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(736, 526);
            this.Controls.Add(this.comboBoxExport);
            this.Controls.Add(this.comboBoxImport);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.buttonExportToLibrary);
            this.Controls.Add(this.buttonImportFromLibrary);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSwitchDisplay);
            this.Controls.Add(this.labelSequenceChannels);
            this.Controls.Add(this.comboBoxChannels);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonResetToLinear);
            this.Controls.Add(this.labelChannelValue);
            this.Controls.Add(this.pictureBoxCurve);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DimmingCurveDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dimming Curve";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DimmingCurveDialog_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pbMini)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurve)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            this._miniBackBrush.Dispose();
            this._curveBackBrush.Dispose();
            this._miniBoxPen.Dispose();
            this._miniLinePen.Dispose();
            this._curveGridPen.Dispose();
            this._curveLinePen.Dispose();
            this._curvePointBrush.Dispose();
            base.Dispose(disposing);
        }
    }
}
