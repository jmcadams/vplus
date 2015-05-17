using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Controllers.E131.Controls;

namespace Controllers.E131.J1Sys {
    //-------------------------------------------------------------
    //
    //	UnicastForm() - form to get a new unicast ip address
    //
    //-------------------------------------------------------------

    public class UnicastForm : Form {
        private IPTextBox _ipTextBox;
        private Button btnIpOkay;
        private Button btnIpCancel;
        private Label label1;
        //private Button _okButton, _cancelButton;

        private IContainer components;


        public UnicastForm() {
            InitializeComponent();
        }


        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        public string IPAddrText {
            get { return _ipTextBox.Text; }
        }


        private void InitializeComponent() {
            this._ipTextBox = new Controllers.E131.Controls.IPTextBox();
            this.btnIpOkay = new System.Windows.Forms.Button();
            this.btnIpCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _ipTextBox
            // 
            this._ipTextBox.Location = new System.Drawing.Point(13, 30);
            this._ipTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._ipTextBox.Name = "_ipTextBox";
            this._ipTextBox.Size = new System.Drawing.Size(132, 20);
            this._ipTextBox.TabIndex = 0;
            this._ipTextBox.Text = "0.0.0.0";
            // 
            // btnIpOkay
            // 
            this.btnIpOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnIpOkay.Location = new System.Drawing.Point(13, 74);
            this.btnIpOkay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnIpOkay.Name = "btnIpOkay";
            this.btnIpOkay.Size = new System.Drawing.Size(100, 28);
            this.btnIpOkay.TabIndex = 1;
            this.btnIpOkay.Text = "Ok";
            this.btnIpOkay.UseVisualStyleBackColor = true;
            // 
            // btnIpCancel
            // 
            this.btnIpCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnIpCancel.Location = new System.Drawing.Point(121, 74);
            this.btnIpCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnIpCancel.Name = "btnIpCancel";
            this.btnIpCancel.Size = new System.Drawing.Size(100, 28);
            this.btnIpCancel.TabIndex = 2;
            this.btnIpCancel.Text = "Cancel";
            this.btnIpCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter your Unicast IP Below";
            // 
            // UnicastForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 116);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnIpCancel);
            this.Controls.Add(this.btnIpOkay);
            this.Controls.Add(this._ipTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UnicastForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }


}
