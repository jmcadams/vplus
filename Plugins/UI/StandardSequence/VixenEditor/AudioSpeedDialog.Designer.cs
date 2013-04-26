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
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.labelValue = new System.Windows.Forms.Label();
            this.buttonSet = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(12, 36);
            this.trackBar.Maximum = 100;
            this.trackBar.Minimum = 1;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(240, 45);
            this.trackBar.TabIndex = 0;
            this.trackBar.Value = 100;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            this.trackBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBar_KeyDown);
            // 
            // labelValue
            // 
            this.labelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelValue.Location = new System.Drawing.Point(12, 9);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(240, 24);
            this.labelValue.TabIndex = 1;
            this.labelValue.Text = "Full Speed";
            this.labelValue.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonSet
            // 
            this.buttonSet.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSet.Location = new System.Drawing.Point(177, 87);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(75, 23);
            this.buttonSet.TabIndex = 2;
            this.buttonSet.Text = "Set Speed";
            this.buttonSet.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(96, 86);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // AudioSpeedDialog
            // 
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(270, 124);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.trackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(276, 130);
            this.MinimumSize = new System.Drawing.Size(276, 130);
            this.Name = "AudioSpeedDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AudioSpeedDialog_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
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