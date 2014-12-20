using System;
using System.ComponentModel;
using System.Drawing;
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
            this.pbMini = new PictureBox();
            this.pictureBoxCurve = new PictureBox();
            this.labelChannelValue = new Label();
            this.buttonResetToLinear = new Button();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.comboBoxChannels = new ComboBox();
            this.labelSequenceChannels = new Label();
            this.buttonSwitchDisplay = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.buttonImportFromLibrary = new Button();
            this.buttonExportToLibrary = new Button();
            this.labelMessage = new Label();
            this.panel1 = new Panel();
            this.comboBoxImport = new ComboBox();
            this.comboBoxExport = new ComboBox();
            ((ISupportInitialize)(this.pbMini)).BeginInit();
            ((ISupportInitialize)(this.pictureBoxCurve)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbMini
            // 
            this.pbMini.BackColor = Color.White;
            this.pbMini.Location = new Point(0, 0);
            this.pbMini.Name = "pbMini";
            this.pbMini.Size = new Size(256, 256);
            this.pbMini.TabIndex = 0;
            this.pbMini.TabStop = false;
            this.pbMini.Paint += new PaintEventHandler(this.pictureBoxMini_Paint);
            this.pbMini.MouseDown += new MouseEventHandler(this.pictureBoxMini_MouseDown);
            this.pbMini.MouseMove += new MouseEventHandler(this.pictureBoxMini_MouseMove);
            // 
            // pictureBoxCurve
            // 
            this.pictureBoxCurve.BackColor = Color.FloralWhite;
            this.pictureBoxCurve.Location = new Point(274, 31);
            this.pictureBoxCurve.Name = "pictureBoxCurve";
            this.pictureBoxCurve.Size = new Size(449, 449);
            this.pictureBoxCurve.TabIndex = 1;
            this.pictureBoxCurve.TabStop = false;
            this.pictureBoxCurve.Paint += new PaintEventHandler(this.pictureBoxCurve_Paint);
            this.pictureBoxCurve.MouseLeave += new EventHandler(this.pictureBoxCurve_MouseLeave);
            this.pictureBoxCurve.MouseMove += new MouseEventHandler(this.pictureBoxCurve_MouseMove);
            this.pictureBoxCurve.MouseUp += new MouseEventHandler(this.pictureBoxCurve_MouseUp);
            // 
            // labelChannelValue
            // 
            this.labelChannelValue.AutoSize = true;
            this.labelChannelValue.Location = new Point(277, 13);
            this.labelChannelValue.Name = "labelChannelValue";
            this.labelChannelValue.Size = new Size(0, 13);
            this.labelChannelValue.TabIndex = 13;
            // 
            // buttonResetToLinear
            // 
            this.buttonResetToLinear.Enabled = false;
            this.buttonResetToLinear.Location = new Point(12, 293);
            this.buttonResetToLinear.Name = "buttonResetToLinear";
            this.buttonResetToLinear.Size = new Size(99, 23);
            this.buttonResetToLinear.TabIndex = 6;
            this.buttonResetToLinear.Text = "Reset to linear";
            this.buttonResetToLinear.UseVisualStyleBackColor = true;
            this.buttonResetToLinear.Click += new EventHandler(this.buttonResetToLinear_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(568, 491);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(649, 491);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // comboBoxChannels
            // 
            this.comboBoxChannels.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxChannels.FormattingEnabled = true;
            this.comboBoxChannels.Location = new Point(15, 353);
            this.comboBoxChannels.Name = "comboBoxChannels";
            this.comboBoxChannels.Size = new Size(253, 21);
            this.comboBoxChannels.TabIndex = 1;
            this.comboBoxChannels.SelectedIndexChanged += new EventHandler(this.comboBoxChannels_SelectedIndexChanged);
            // 
            // labelSequenceChannels
            // 
            this.labelSequenceChannels.AutoSize = true;
            this.labelSequenceChannels.Location = new Point(12, 337);
            this.labelSequenceChannels.Name = "labelSequenceChannels";
            this.labelSequenceChannels.Size = new Size(102, 13);
            this.labelSequenceChannels.TabIndex = 0;
            this.labelSequenceChannels.Text = "Sequence channels";
            // 
            // buttonSwitchDisplay
            // 
            this.buttonSwitchDisplay.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.buttonSwitchDisplay.Location = new Point(356, 501);
            this.buttonSwitchDisplay.Name = "buttonSwitchDisplay";
            this.buttonSwitchDisplay.Size = new Size(120, 23);
            this.buttonSwitchDisplay.TabIndex = 14;
            this.buttonSwitchDisplay.Text = "Show";
            this.buttonSwitchDisplay.UseVisualStyleBackColor = true;
            this.buttonSwitchDisplay.Visible = false;
            this.buttonSwitchDisplay.Click += new EventHandler(this.buttonSwitchDisplay_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(277, 483);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Input >";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(258, 479);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0, 13);
            this.label2.TabIndex = 9;
            // 
            // buttonImportFromLibrary
            // 
            this.buttonImportFromLibrary.Location = new Point(15, 396);
            this.buttonImportFromLibrary.Name = "buttonImportFromLibrary";
            this.buttonImportFromLibrary.Size = new Size(75, 23);
            this.buttonImportFromLibrary.TabIndex = 2;
            this.buttonImportFromLibrary.Text = "Import";
            this.buttonImportFromLibrary.UseVisualStyleBackColor = true;
            this.buttonImportFromLibrary.Click += new EventHandler(this.buttonImportFromLibrary_Click);
            // 
            // buttonExportToLibrary
            // 
            this.buttonExportToLibrary.Location = new Point(15, 425);
            this.buttonExportToLibrary.Name = "buttonExportToLibrary";
            this.buttonExportToLibrary.Size = new Size(75, 23);
            this.buttonExportToLibrary.TabIndex = 4;
            this.buttonExportToLibrary.Text = "Export";
            this.buttonExportToLibrary.UseVisualStyleBackColor = true;
            this.buttonExportToLibrary.Click += new EventHandler(this.buttonExportToLibrary_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new Point(12, 496);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new Size(0, 13);
            this.labelMessage.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pbMini);
            this.panel1.Location = new Point(12, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(258, 258);
            this.panel1.TabIndex = 11;
            // 
            // comboBoxImport
            // 
            this.comboBoxImport.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxImport.FormattingEnabled = true;
            this.comboBoxImport.Items.AddRange(new object[] {
            "curve from the library",
            "curves from a file"});
            this.comboBoxImport.Location = new Point(96, 398);
            this.comboBoxImport.Name = "comboBoxImport";
            this.comboBoxImport.Size = new Size(133, 21);
            this.comboBoxImport.TabIndex = 3;
            // 
            // comboBoxExport
            // 
            this.comboBoxExport.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxExport.FormattingEnabled = true;
            this.comboBoxExport.Items.AddRange(new object[] {
            "curve to the library",
            "curves to a file"});
            this.comboBoxExport.Location = new Point(96, 427);
            this.comboBoxExport.Name = "comboBoxExport";
            this.comboBoxExport.Size = new Size(133, 21);
            this.comboBoxExport.TabIndex = 5;
            // 
            // DimmingCurveDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(736, 526);
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
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DimmingCurveDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Dimming Curve";
            this.Paint += new PaintEventHandler(this.DimmingCurveDialog_Paint);
            ((ISupportInitialize)(this.pbMini)).EndInit();
            ((ISupportInitialize)(this.pictureBoxCurve)).EndInit();
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
