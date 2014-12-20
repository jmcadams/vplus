using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    public partial class AudioSpeedDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonSet;
        private Label labelValue;
        private TrackBar trackBar;


        private void InitializeComponent() {
            this.trackBar = new TrackBar();
            this.labelValue = new Label();
            this.buttonSet = new Button();
            this.btnCancel = new Button();
            ((ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar
            // 
            this.trackBar.Location = new Point(12, 36);
            this.trackBar.Maximum = 100;
            this.trackBar.Minimum = 1;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new Size(240, 45);
            this.trackBar.TabIndex = 0;
            this.trackBar.Value = 100;
            this.trackBar.Scroll += new EventHandler(this.trackBar_Scroll);
            this.trackBar.KeyDown += new KeyEventHandler(this.trackBar_KeyDown);
            // 
            // labelValue
            // 
            this.labelValue.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.labelValue.Location = new Point(12, 9);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new Size(240, 24);
            this.labelValue.TabIndex = 1;
            this.labelValue.Text = "Full Speed";
            this.labelValue.TextAlign = ContentAlignment.TopCenter;
            // 
            // buttonSet
            // 
            this.buttonSet.DialogResult = DialogResult.OK;
            this.buttonSet.Location = new Point(177, 87);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new Size(75, 23);
            this.buttonSet.TabIndex = 2;
            this.buttonSet.Text = "Set Speed";
            this.buttonSet.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(96, 86);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // AudioSpeedDialog
            // 
            this.BackColor = Color.White;
            this.CancelButton = this.btnCancel;
            this.CausesValidation = false;
            this.ClientSize = new Size(270, 124);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.trackBar);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximumSize = new Size(276, 130);
            this.MinimumSize = new Size(276, 130);
            this.Name = "AudioSpeedDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.KeyPress += new KeyPressEventHandler(this.AudioSpeedDialog_KeyPress);
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

        private Button btnCancel;
    }
}