namespace VixenEditor {
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.ComponentModel;
    using System.Collections;

    internal partial class TestConsoleDialog {

        #region Windows Form Designer generated code

        private Button buttonDone;
        private ConsoleTrackBar consoleTrackBar1;
        private ConsoleTrackBar consoleTrackBar2;
        private ConsoleTrackBar consoleTrackBar3;
        private ConsoleTrackBar consoleTrackBar4;
        private ConsoleTrackBar consoleTrackBar5;
        private ConsoleTrackBar consoleTrackBar6;
        private ConsoleTrackBar consoleTrackBar7;
        private ConsoleTrackBar consoleTrackBarMaster;
        private GroupBox groupBox1;
        private GroupBox groupBox2;


        private void InitializeComponent() {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.consoleTrackBarMaster = new ConsoleTrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.consoleTrackBar7 = new ConsoleTrackBar();
            this.consoleTrackBar5 = new ConsoleTrackBar();
            this.consoleTrackBar6 = new ConsoleTrackBar();
            this.consoleTrackBar3 = new ConsoleTrackBar();
            this.consoleTrackBar4 = new ConsoleTrackBar();
            this.consoleTrackBar2 = new ConsoleTrackBar();
            this.consoleTrackBar1 = new ConsoleTrackBar();
            this.buttonDone = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.consoleTrackBarMaster);
            this.groupBox1.Location = new System.Drawing.Point(8, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(80, 295);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Master";
            // 
            // consoleTrackBarMaster
            // 
            this.consoleTrackBarMaster.AllowText = false;
            this.consoleTrackBarMaster.CascadeMasterEvents = false;
            this.consoleTrackBarMaster.Location = new System.Drawing.Point(6, 20);
            this.consoleTrackBarMaster.Master = null;
            this.consoleTrackBarMaster.Name = "consoleTrackBarMaster";
            this.consoleTrackBarMaster.ResetIndex = -1;
            this.consoleTrackBarMaster.Size = new System.Drawing.Size(68, 267);
            this.consoleTrackBarMaster.TabIndex = 0;
            this.consoleTrackBarMaster.Value = 0;
            this.consoleTrackBarMaster.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBarMaster_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.consoleTrackBar7);
            this.groupBox2.Controls.Add(this.consoleTrackBar5);
            this.groupBox2.Controls.Add(this.consoleTrackBar6);
            this.groupBox2.Controls.Add(this.consoleTrackBar3);
            this.groupBox2.Controls.Add(this.consoleTrackBar4);
            this.groupBox2.Controls.Add(this.consoleTrackBar2);
            this.groupBox2.Controls.Add(this.consoleTrackBar1);
            this.groupBox2.Location = new System.Drawing.Point(119, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(693, 295);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Channels";
            // 
            // consoleTrackBar7
            // 
            this.consoleTrackBar7.AllowText = true;
            this.consoleTrackBar7.CascadeMasterEvents = false;
            this.consoleTrackBar7.Location = new System.Drawing.Point(588, 17);
            this.consoleTrackBar7.Master = this.consoleTrackBarMaster;
            this.consoleTrackBar7.Name = "consoleTrackBar7";
            this.consoleTrackBar7.ResetIndex = -1;
            this.consoleTrackBar7.Size = new System.Drawing.Size(90, 268);
            this.consoleTrackBar7.TabIndex = 6;
            this.consoleTrackBar7.Value = 0;
            this.consoleTrackBar7.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
            // 
            // consoleTrackBar5
            // 
            this.consoleTrackBar5.AllowText = true;
            this.consoleTrackBar5.CascadeMasterEvents = false;
            this.consoleTrackBar5.Location = new System.Drawing.Point(492, 17);
            this.consoleTrackBar5.Master = this.consoleTrackBarMaster;
            this.consoleTrackBar5.Name = "consoleTrackBar5";
            this.consoleTrackBar5.ResetIndex = -1;
            this.consoleTrackBar5.Size = new System.Drawing.Size(90, 268);
            this.consoleTrackBar5.TabIndex = 5;
            this.consoleTrackBar5.Value = 0;
            this.consoleTrackBar5.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
            // 
            // consoleTrackBar6
            // 
            this.consoleTrackBar6.AllowText = true;
            this.consoleTrackBar6.CascadeMasterEvents = false;
            this.consoleTrackBar6.Location = new System.Drawing.Point(396, 18);
            this.consoleTrackBar6.Master = this.consoleTrackBarMaster;
            this.consoleTrackBar6.Name = "consoleTrackBar6";
            this.consoleTrackBar6.ResetIndex = -1;
            this.consoleTrackBar6.Size = new System.Drawing.Size(90, 268);
            this.consoleTrackBar6.TabIndex = 4;
            this.consoleTrackBar6.Value = 0;
            this.consoleTrackBar6.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
            // 
            // consoleTrackBar3
            // 
            this.consoleTrackBar3.AllowText = true;
            this.consoleTrackBar3.CascadeMasterEvents = false;
            this.consoleTrackBar3.Location = new System.Drawing.Point(300, 18);
            this.consoleTrackBar3.Master = this.consoleTrackBarMaster;
            this.consoleTrackBar3.Name = "consoleTrackBar3";
            this.consoleTrackBar3.ResetIndex = -1;
            this.consoleTrackBar3.Size = new System.Drawing.Size(90, 268);
            this.consoleTrackBar3.TabIndex = 3;
            this.consoleTrackBar3.Value = 0;
            this.consoleTrackBar3.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
            // 
            // consoleTrackBar4
            // 
            this.consoleTrackBar4.AllowText = true;
            this.consoleTrackBar4.CascadeMasterEvents = false;
            this.consoleTrackBar4.Location = new System.Drawing.Point(204, 19);
            this.consoleTrackBar4.Master = this.consoleTrackBarMaster;
            this.consoleTrackBar4.Name = "consoleTrackBar4";
            this.consoleTrackBar4.ResetIndex = -1;
            this.consoleTrackBar4.Size = new System.Drawing.Size(90, 268);
            this.consoleTrackBar4.TabIndex = 2;
            this.consoleTrackBar4.Value = 0;
            this.consoleTrackBar4.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
            // 
            // consoleTrackBar2
            // 
            this.consoleTrackBar2.AllowText = true;
            this.consoleTrackBar2.CascadeMasterEvents = false;
            this.consoleTrackBar2.Location = new System.Drawing.Point(108, 19);
            this.consoleTrackBar2.Master = this.consoleTrackBarMaster;
            this.consoleTrackBar2.Name = "consoleTrackBar2";
            this.consoleTrackBar2.ResetIndex = -1;
            this.consoleTrackBar2.Size = new System.Drawing.Size(90, 268);
            this.consoleTrackBar2.TabIndex = 1;
            this.consoleTrackBar2.Value = 0;
            this.consoleTrackBar2.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
            // 
            // consoleTrackBar1
            // 
            this.consoleTrackBar1.AllowText = true;
            this.consoleTrackBar1.CascadeMasterEvents = false;
            this.consoleTrackBar1.Location = new System.Drawing.Point(12, 20);
            this.consoleTrackBar1.Master = this.consoleTrackBarMaster;
            this.consoleTrackBar1.Name = "consoleTrackBar1";
            this.consoleTrackBar1.ResetIndex = -1;
            this.consoleTrackBar1.Size = new System.Drawing.Size(90, 268);
            this.consoleTrackBar1.TabIndex = 0;
            this.consoleTrackBar1.Value = 0;
            this.consoleTrackBar1.ValueChanged += new ConsoleTrackBar.ValueChangedHandler(this.consoleTrackBar_ValueChanged);
            // 
            // buttonDone
            // 
            this.buttonDone.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDone.Location = new System.Drawing.Point(737, 334);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 4;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // TestConsoleDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonDone;
            this.ClientSize = new System.Drawing.Size(824, 369);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::Properties.Resources.VixenPlus;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "TestConsoleDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Console";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestConsoleDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
        }
    }
}
