using System.Windows.Forms;

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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nutcrackerEffectControl2 = new VixenEditor.NutcrackerEffectControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nutcrackerEffectControl1 = new VixenEditor.NutcrackerEffectControl();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblFrames = new System.Windows.Forms.Label();
            this.nudFrames = new System.Windows.Forms.NumericUpDown();
            this.cbRender = new System.Windows.Forms.CheckBox();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lblRows = new System.Windows.Forms.Label();
            this.nudColumns = new System.Windows.Forms.NumericUpDown();
            this.nudRows = new System.Windows.Forms.NumericUpDown();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRows)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nutcrackerEffectControl2);
            this.groupBox2.Location = new System.Drawing.Point(788, 352);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 198);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Effect 2";
            // 
            // nutcrackerEffectControl2
            // 
            this.nutcrackerEffectControl2.Location = new System.Drawing.Point(7, 20);
            this.nutcrackerEffectControl2.Name = "nutcrackerEffectControl2";
            this.nutcrackerEffectControl2.Size = new System.Drawing.Size(371, 173);
            this.nutcrackerEffectControl2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nutcrackerEffectControl1);
            this.groupBox1.Location = new System.Drawing.Point(398, 352);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 198);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Effect 1";
            // 
            // nutcrackerEffectControl1
            // 
            this.nutcrackerEffectControl1.Location = new System.Drawing.Point(7, 20);
            this.nutcrackerEffectControl1.Name = "nutcrackerEffectControl1";
            this.nutcrackerEffectControl1.Size = new System.Drawing.Size(371, 173);
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
            this.groupBox3.Controls.Add(this.lblFrames);
            this.groupBox3.Controls.Add(this.nudFrames);
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
            this.groupBox3.Size = new System.Drawing.Size(774, 334);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Nutcracker Options";
            // 
            // lblFrames
            // 
            this.lblFrames.AutoSize = true;
            this.lblFrames.Location = new System.Drawing.Point(516, 75);
            this.lblFrames.Name = "lblFrames";
            this.lblFrames.Size = new System.Drawing.Size(41, 13);
            this.lblFrames.TabIndex = 9;
            this.lblFrames.Text = "Frames";
            // 
            // nudFrames
            // 
            this.nudFrames.Location = new System.Drawing.Point(391, 73);
            this.nudFrames.Name = "nudFrames";
            this.nudFrames.Maximum = 200;
            this.nudFrames.Size = new System.Drawing.Size(120, 20);
            this.nudFrames.TabIndex = 8;
            this.nudFrames.ValueChanged += new System.EventHandler(this.nudFrames_ValueChanged);
            // 
            // cbRender
            // 
            this.cbRender.AutoSize = true;
            this.cbRender.Enabled = false;
            this.cbRender.Location = new System.Drawing.Point(390, 99);
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
            this.lblColumns.Location = new System.Drawing.Point(516, 48);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 6;
            this.lblColumns.Text = "Columns";
            // 
            // lblRows
            // 
            this.lblRows.AutoSize = true;
            this.lblRows.Location = new System.Drawing.Point(517, 22);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(34, 13);
            this.lblRows.TabIndex = 5;
            this.lblRows.Text = "Rows";
            // 
            // nudColumns
            // 
            this.nudColumns.Location = new System.Drawing.Point(390, 46);
            this.nudColumns.Maximum = 300;
            this.nudColumns.Name = "nudColumns";
            this.nudColumns.Size = new System.Drawing.Size(120, 20);
            this.nudColumns.TabIndex = 4;
            this.nudColumns.ValueChanged += new System.EventHandler(this.nudColumns_ValueChanged);
            // 
            // nudRows
            // 
            this.nudRows.Location = new System.Drawing.Point(391, 20);
            this.nudRows.Name = "nudRows";
            this.nudRows.Size = new System.Drawing.Size(120, 20);
            this.nudRows.TabIndex = 3;
            this.nudRows.ValueChanged += new System.EventHandler(this.nudRows_ValueChanged);
            // 
            // tbInfo
            // 
            this.tbInfo.Location = new System.Drawing.Point(7, 20);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.Size = new System.Drawing.Size(377, 308);
            this.tbInfo.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(612, 305);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(693, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRows)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbPreview;
        private NutcrackerEffectControl nutcrackerEffectControl1;
        private NutcrackerEffectControl nutcrackerEffectControl2;
        private GroupBox groupBox3;
        private Button btnOK;
        private Button btnCancel;
        private CheckBox cbRender;
        private Label lblColumns;
        private Label lblRows;
        private NumericUpDown nudColumns;
        private NumericUpDown nudRows;
        private TextBox tbInfo;
        private Label lblFrames;
        private NumericUpDown nudFrames;
    }
}