using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus
{
	internal partial class CopyDataDialog2
    {
        private IContainer components = null;

		#region Windows Form Designer generated code
		private Button btnCopy;
private Button buttonDone;
private Button buttonFromFile;
private Button buttonFromProgram;
private Button buttonFromSequence;
private Button buttontoFile;
private Button buttonToProgram;
private Button buttonToSequence;
private CheckBox checkBoxShowAllNodes;
private GroupBox groupBox1;
private GroupBox groupBox2;
private Label label1;
private Label label2;
private Label labelFrom;
private Label labelTo;
private OpenFileDialog openFileDialog;
private SaveFileDialog saveFileDialog;
private TreeView treeViewFrom;

		private void InitializeComponent()
        {
            this.btnCopy = new Button();
            this.treeViewFrom = new TreeView();
            this.buttonDone = new Button();
            this.groupBox2 = new GroupBox();
            this.buttontoFile = new Button();
            this.labelTo = new Label();
            this.buttonToProgram = new Button();
            this.buttonToSequence = new Button();
            this.groupBox1 = new GroupBox();
            this.buttonFromFile = new Button();
            this.labelFrom = new Label();
            this.buttonFromProgram = new Button();
            this.buttonFromSequence = new Button();
            this.label1 = new Label();
            this.checkBoxShowAllNodes = new CheckBox();
            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.label2 = new Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.btnCopy.Location = new Point(292, 177);
            this.btnCopy.Name = "buttonCopy";
            this.btnCopy.Size = new Size(75, 23);
            this.btnCopy.TabIndex = 6;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new EventHandler(this.buttonCopy_Click);
            this.treeViewFrom.CheckBoxes = true;
            this.treeViewFrom.HideSelection = false;
            this.treeViewFrom.Location = new Point(292, 31);
            this.treeViewFrom.Name = "treeViewFrom";
            this.treeViewFrom.Size = new Size(216, 119);
            this.treeViewFrom.TabIndex = 7;
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDone.Location = new Point(431, 264);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(75, 23);
            this.buttonDone.TabIndex = 8;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.groupBox2.Controls.Add(this.buttontoFile);
            this.groupBox2.Controls.Add(this.labelTo);
            this.groupBox2.Controls.Add(this.buttonToProgram);
            this.groupBox2.Controls.Add(this.buttonToSequence);
            this.groupBox2.Location = new Point(12, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(253, 85);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Copy To";
            this.buttontoFile.Location = new Point(166, 19);
            this.buttontoFile.Name = "buttontoFile";
            this.buttontoFile.Size = new Size(70, 23);
            this.buttontoFile.TabIndex = 2;
            this.buttontoFile.Text = "File";
            this.buttontoFile.UseVisualStyleBackColor = true;
            this.buttontoFile.Click += new EventHandler(this.buttontoFile_Click);
            this.labelTo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelTo.Location = new Point(20, 50);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new Size(216, 32);
            this.labelTo.TabIndex = 3;
            this.buttonToProgram.Location = new Point(90, 19);
            this.buttonToProgram.Name = "buttonToProgram";
            this.buttonToProgram.Size = new Size(70, 23);
            this.buttonToProgram.TabIndex = 1;
            this.buttonToProgram.Text = "Program";
            this.buttonToProgram.UseVisualStyleBackColor = true;
            this.buttonToProgram.Click += new EventHandler(this.buttonToProgram_Click);
            this.buttonToSequence.Location = new Point(14, 19);
            this.buttonToSequence.Name = "buttonToSequence";
            this.buttonToSequence.Size = new Size(70, 23);
            this.buttonToSequence.TabIndex = 0;
            this.buttonToSequence.Text = "Sequence";
            this.buttonToSequence.UseVisualStyleBackColor = true;
            this.buttonToSequence.Click += new EventHandler(this.buttonToSequence_Click);
            this.groupBox1.Controls.Add(this.buttonFromFile);
            this.groupBox1.Controls.Add(this.labelFrom);
            this.groupBox1.Controls.Add(this.buttonFromProgram);
            this.groupBox1.Controls.Add(this.buttonFromSequence);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(253, 85);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Copy From";
            this.buttonFromFile.Location = new Point(166, 19);
            this.buttonFromFile.Name = "buttonFromFile";
            this.buttonFromFile.Size = new Size(70, 23);
            this.buttonFromFile.TabIndex = 2;
            this.buttonFromFile.Text = "File";
            this.buttonFromFile.UseVisualStyleBackColor = true;
            this.buttonFromFile.Click += new EventHandler(this.buttonFromFile_Click);
            this.labelFrom.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelFrom.Location = new Point(19, 51);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new Size(217, 31);
            this.labelFrom.TabIndex = 3;
            this.buttonFromProgram.Location = new Point(90, 19);
            this.buttonFromProgram.Name = "buttonFromProgram";
            this.buttonFromProgram.Size = new Size(70, 23);
            this.buttonFromProgram.TabIndex = 1;
            this.buttonFromProgram.Text = "Program";
            this.buttonFromProgram.UseVisualStyleBackColor = true;
            this.buttonFromProgram.Click += new EventHandler(this.buttonFromProgram_Click);
            this.buttonFromSequence.Location = new Point(14, 19);
            this.buttonFromSequence.Name = "buttonFromSequence";
            this.buttonFromSequence.Size = new Size(70, 23);
            this.buttonFromSequence.TabIndex = 0;
            this.buttonFromSequence.Text = "Sequence";
            this.buttonFromSequence.UseVisualStyleBackColor = true;
            this.buttonFromSequence.Click += new EventHandler(this.buttonFromSequence_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(290, 12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(71, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Data to copy:";
            this.checkBoxShowAllNodes.AutoSize = true;
            this.checkBoxShowAllNodes.Location = new Point(292, 154);
            this.checkBoxShowAllNodes.Name = "checkBoxShowAllNodes";
            this.checkBoxShowAllNodes.Size = new Size(98, 17);
            this.checkBoxShowAllNodes.TabIndex = 10;
            this.checkBoxShowAllNodes.Text = "Show all nodes";
            this.checkBoxShowAllNodes.UseVisualStyleBackColor = true;
            this.checkBoxShowAllNodes.CheckedChanged += new EventHandler(this.checkBoxShowAllNodes_CheckedChanged);
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(13, 218);
            this.label2.Name = "label2";
            this.label2.Size = new Size(492, 37);
            this.label2.TabIndex = 11;
            this.label2.Text = "WARNING: This tool allows you to copy anything to your benefit or detriment.  Please be aware of this as you use it.";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(518, 299);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.checkBoxShowAllNodes);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnCopy);
            base.Controls.Add(this.treeViewFrom);
            base.Controls.Add(this.buttonDone);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "CopyDataDialog2";
            base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Copy Data";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
		#endregion

		protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}
