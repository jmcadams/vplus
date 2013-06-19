using System.Windows.Forms;

using NutcrackerEffectsControl;

namespace VixenEditor
{
    partial class NutcrackerControlDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nutcrackerEffectControl2 = new NutcrackerEffectsControl.NutcrackerEffectControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nutcrackerEffectControl1 = new NutcrackerEffectsControl.NutcrackerEffectControl();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbAverage = new System.Windows.Forms.RadioButton();
            this.rbLayer = new System.Windows.Forms.RadioButton();
            this.rbUnmask2 = new System.Windows.Forms.RadioButton();
            this.rbUnmask1 = new System.Windows.Forms.RadioButton();
            this.rbMask2 = new System.Windows.Forms.RadioButton();
            this.rbMask1 = new System.Windows.Forms.RadioButton();
            this.rbEffect2 = new System.Windows.Forms.RadioButton();
            this.rbEffect1 = new System.Windows.Forms.RadioButton();
            this.cbRender = new System.Windows.Forms.CheckBox();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lblRows = new System.Windows.Forms.Label();
            this.nudColumns = new System.Windows.Forms.NumericUpDown();
            this.nudRows = new System.Windows.Forms.NumericUpDown();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.timerRender = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRows)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nutcrackerEffectControl2);
            this.groupBox2.Location = new System.Drawing.Point(788, 304);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 250);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Effect 2";
            // 
            // nutcrackerEffectControl2
            // 
            this.nutcrackerEffectControl2.Location = new System.Drawing.Point(7, 20);
            this.nutcrackerEffectControl2.Name = "nutcrackerEffectControl2";
            this.nutcrackerEffectControl2.Size = new System.Drawing.Size(371, 225);
            this.nutcrackerEffectControl2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nutcrackerEffectControl1);
            this.groupBox1.Location = new System.Drawing.Point(398, 304);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 250);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Effect 1";
            // 
            // nutcrackerEffectControl1
            // 
            this.nutcrackerEffectControl1.Location = new System.Drawing.Point(7, 20);
            this.nutcrackerEffectControl1.Name = "nutcrackerEffectControl1";
            this.nutcrackerEffectControl1.Size = new System.Drawing.Size(371, 225);
            this.nutcrackerEffectControl1.TabIndex = 0;
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.Black;
            this.pbPreview.Location = new System.Drawing.Point(12, 12);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(380, 538);
            this.pbPreview.TabIndex = 2;
            this.pbPreview.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.btnLoad);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.cbRender);
            this.groupBox3.Controls.Add(this.lblColumns);
            this.groupBox3.Controls.Add(this.lblRows);
            this.groupBox3.Controls.Add(this.nudColumns);
            this.groupBox3.Controls.Add(this.nudRows);
            this.groupBox3.Controls.Add(this.tbInfo);
            this.groupBox3.Controls.Add(this.btnOK);
            this.groupBox3.Controls.Add(this.btnCancel);
            this.groupBox3.Location = new System.Drawing.Point(398, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(774, 286);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Nutcracker Options";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(92, 257);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Save Effects";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(7, 257);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(79, 23);
            this.btnLoad.TabIndex = 9;
            this.btnLoad.Text = "Load Effects";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbAverage);
            this.groupBox4.Controls.Add(this.rbLayer);
            this.groupBox4.Controls.Add(this.rbUnmask2);
            this.groupBox4.Controls.Add(this.rbUnmask1);
            this.groupBox4.Controls.Add(this.rbMask2);
            this.groupBox4.Controls.Add(this.rbMask1);
            this.groupBox4.Controls.Add(this.rbEffect2);
            this.groupBox4.Controls.Add(this.rbEffect1);
            this.groupBox4.Location = new System.Drawing.Point(612, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(156, 204);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Layer Method";
            // 
            // rbAverage
            // 
            this.rbAverage.AutoSize = true;
            this.rbAverage.Location = new System.Drawing.Point(6, 178);
            this.rbAverage.Name = "rbAverage";
            this.rbAverage.Size = new System.Drawing.Size(123, 17);
            this.rbAverage.TabIndex = 7;
            this.rbAverage.Text = "Average Effect 1 && 2";
            this.rbAverage.UseVisualStyleBackColor = true;
            // 
            // rbLayer
            // 
            this.rbLayer.AutoSize = true;
            this.rbLayer.Location = new System.Drawing.Point(6, 155);
            this.rbLayer.Name = "rbLayer";
            this.rbLayer.Size = new System.Drawing.Size(109, 17);
            this.rbLayer.TabIndex = 6;
            this.rbLayer.Text = "Layer Effect 1 && 2";
            this.rbLayer.UseVisualStyleBackColor = true;
            // 
            // rbUnmask2
            // 
            this.rbUnmask2.AutoSize = true;
            this.rbUnmask2.Location = new System.Drawing.Point(6, 132);
            this.rbUnmask2.Name = "rbUnmask2";
            this.rbUnmask2.Size = new System.Drawing.Size(114, 17);
            this.rbUnmask2.TabIndex = 5;
            this.rbUnmask2.Text = "Effect 2 is Unmask";
            this.rbUnmask2.UseVisualStyleBackColor = true;
            // 
            // rbUnmask1
            // 
            this.rbUnmask1.AutoSize = true;
            this.rbUnmask1.Location = new System.Drawing.Point(6, 109);
            this.rbUnmask1.Name = "rbUnmask1";
            this.rbUnmask1.Size = new System.Drawing.Size(114, 17);
            this.rbUnmask1.TabIndex = 4;
            this.rbUnmask1.Text = "Effect 1 is Unmask";
            this.rbUnmask1.UseVisualStyleBackColor = true;
            // 
            // rbMask2
            // 
            this.rbMask2.AutoSize = true;
            this.rbMask2.Location = new System.Drawing.Point(6, 86);
            this.rbMask2.Name = "rbMask2";
            this.rbMask2.Size = new System.Drawing.Size(101, 17);
            this.rbMask2.TabIndex = 3;
            this.rbMask2.Text = "Effect 2 is Mask";
            this.rbMask2.UseVisualStyleBackColor = true;
            // 
            // rbMask1
            // 
            this.rbMask1.AutoSize = true;
            this.rbMask1.Location = new System.Drawing.Point(6, 65);
            this.rbMask1.Name = "rbMask1";
            this.rbMask1.Size = new System.Drawing.Size(101, 17);
            this.rbMask1.TabIndex = 2;
            this.rbMask1.Text = "Effect 1 is Mask";
            this.rbMask1.UseVisualStyleBackColor = true;
            // 
            // rbEffect2
            // 
            this.rbEffect2.AutoSize = true;
            this.rbEffect2.Location = new System.Drawing.Point(6, 42);
            this.rbEffect2.Name = "rbEffect2";
            this.rbEffect2.Size = new System.Drawing.Size(62, 17);
            this.rbEffect2.TabIndex = 1;
            this.rbEffect2.Text = "Effect 2";
            this.rbEffect2.UseVisualStyleBackColor = true;
            // 
            // rbEffect1
            // 
            this.rbEffect1.AutoSize = true;
            this.rbEffect1.Checked = true;
            this.rbEffect1.Location = new System.Drawing.Point(6, 19);
            this.rbEffect1.Name = "rbEffect1";
            this.rbEffect1.Size = new System.Drawing.Size(62, 17);
            this.rbEffect1.TabIndex = 0;
            this.rbEffect1.TabStop = true;
            this.rbEffect1.Text = "Effect 1";
            this.rbEffect1.UseVisualStyleBackColor = true;
            // 
            // cbRender
            // 
            this.cbRender.AutoSize = true;
            this.cbRender.Location = new System.Drawing.Point(177, 72);
            this.cbRender.Name = "cbRender";
            this.cbRender.Size = new System.Drawing.Size(61, 17);
            this.cbRender.TabIndex = 7;
            this.cbRender.Text = "Render";
            this.cbRender.UseVisualStyleBackColor = true;
            this.cbRender.CheckedChanged += new System.EventHandler(this.cbRender_CheckedChanged);
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(302, 48);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 6;
            this.lblColumns.Text = "Columns";
            // 
            // lblRows
            // 
            this.lblRows.AutoSize = true;
            this.lblRows.Location = new System.Drawing.Point(303, 22);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(34, 13);
            this.lblRows.TabIndex = 5;
            this.lblRows.Text = "Rows";
            // 
            // nudColumns
            // 
            this.nudColumns.Location = new System.Drawing.Point(176, 46);
            this.nudColumns.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudColumns.Name = "nudColumns";
            this.nudColumns.Size = new System.Drawing.Size(120, 20);
            this.nudColumns.TabIndex = 4;
            this.nudColumns.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // nudRows
            // 
            this.nudRows.Location = new System.Drawing.Point(177, 20);
            this.nudRows.Name = "nudRows";
            this.nudRows.Size = new System.Drawing.Size(120, 20);
            this.nudRows.TabIndex = 3;
            this.nudRows.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // tbInfo
            // 
            this.tbInfo.Location = new System.Drawing.Point(7, 20);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.Size = new System.Drawing.Size(164, 73);
            this.tbInfo.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(612, 257);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(693, 257);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // timerRender
            // 
            this.timerRender.Interval = 50;
            this.timerRender.Tick += new System.EventHandler(this.timerRender_Tick);
            // 
            // NutcrackerControlDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 562);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pbPreview);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NutcrackerControlDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate Nutcracker Effect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NutcrackerControlDialog_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRows)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbPreview;
        private GroupBox groupBox3;
        private Button btnOK;
        private Button btnCancel;
        private CheckBox cbRender;
        private Label lblColumns;
        private Label lblRows;
        private NumericUpDown nudColumns;
        private NumericUpDown nudRows;
        private TextBox tbInfo;
        private NutcrackerEffectControl nutcrackerEffectControl2;
        private NutcrackerEffectControl nutcrackerEffectControl1;
        private GroupBox groupBox4;
        private RadioButton rbAverage;
        private RadioButton rbLayer;
        private RadioButton rbUnmask2;
        private RadioButton rbUnmask1;
        private RadioButton rbMask2;
        private RadioButton rbMask1;
        private RadioButton rbEffect2;
        private RadioButton rbEffect1;
        private Button button1;
        private Button btnLoad;
        private Timer timerRender;
    }
}