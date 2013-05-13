using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus
{
    internal sealed partial class CopySequenceDialog
    {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonApply;
private Button buttonAutoMap;
private Button buttonCancel;
private Button buttonOK;
private Button buttonReset;
private CheckBox checkBoxChannelDefs;
private CheckBox checkBoxSequenceLength;
private ColumnHeader columnHeader1;
private ColumnHeader columnHeader2;
private ComboBox comboBoxDestChannels;
private ComboBox comboBoxDestSequence;
private ComboBox comboBoxSourceSequence;
private GroupBox gbAll;
private GroupBox groupBox2;
private Label label1;
private Label label2;
private ListView listViewMapping;

        private void InitializeComponent()
        {
            this.gbAll = new GroupBox();
            this.buttonApply = new Button();
            this.label2 = new Label();
            this.label1 = new Label();
            this.comboBoxDestSequence = new ComboBox();
            this.comboBoxSourceSequence = new ComboBox();
            this.groupBox2 = new GroupBox();
            this.buttonAutoMap = new Button();
            this.comboBoxDestChannels = new ComboBox();
            this.buttonReset = new Button();
            this.listViewMapping = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.checkBoxChannelDefs = new CheckBox();
            this.checkBoxSequenceLength = new CheckBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.gbAll.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.gbAll.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gbAll.Controls.Add(this.buttonApply);
            this.gbAll.Controls.Add(this.label2);
            this.gbAll.Controls.Add(this.label1);
            this.gbAll.Controls.Add(this.comboBoxDestSequence);
            this.gbAll.Controls.Add(this.comboBoxSourceSequence);
            this.gbAll.Location = new Point(16, 14);
            this.gbAll.Name = "groupBox1";
            this.gbAll.Size = new Size(426, 137);
            this.gbAll.TabIndex = 0;
            this.gbAll.TabStop = false;
            this.gbAll.Text = "Sequences";
            this.buttonApply.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonApply.Location = new Point(316, 100);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new Size(75, 23);
            this.buttonApply.TabIndex = 5;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new EventHandler(this.buttonApply_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(22, 70);
            this.label2.Name = "label2";
            this.label2.Size = new Size(112, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Destination Sequence";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(22, 29);
            this.label1.Name = "label1";
            this.label1.Size = new Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Source Sequence";
            this.comboBoxDestSequence.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDestSequence.FormattingEnabled = true;
            this.comboBoxDestSequence.Location = new Point(155, 67);
            this.comboBoxDestSequence.Name = "comboBoxDestSequence";
            this.comboBoxDestSequence.Size = new Size(236, 21);
            this.comboBoxDestSequence.TabIndex = 4;
            this.comboBoxDestSequence.SelectedIndexChanged += new EventHandler(this.comboBoxSourceSequence_SelectedIndexChanged);
            this.comboBoxSourceSequence.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSourceSequence.FormattingEnabled = true;
            this.comboBoxSourceSequence.Location = new Point(155, 26);
            this.comboBoxSourceSequence.Name = "comboBoxSourceSequence";
            this.comboBoxSourceSequence.Size = new Size(236, 21);
            this.comboBoxSourceSequence.TabIndex = 2;
            this.comboBoxSourceSequence.SelectedIndexChanged += new EventHandler(this.comboBoxSourceSequence_SelectedIndexChanged);
            this.groupBox2.Controls.Add(this.buttonAutoMap);
            this.groupBox2.Controls.Add(this.comboBoxDestChannels);
            this.groupBox2.Controls.Add(this.buttonReset);
            this.groupBox2.Controls.Add(this.listViewMapping);
            this.groupBox2.Location = new Point(16, 171);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(425, 283);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Channel mappings";
            this.groupBox2.Leave += new EventHandler(this.groupBox2_Leave);
            this.buttonAutoMap.Location = new Point(16, 249);
            this.buttonAutoMap.Name = "buttonAutoMap";
            this.buttonAutoMap.Size = new Size(75, 23);
            this.buttonAutoMap.TabIndex = 2;
            this.buttonAutoMap.Text = "Auto map";
            this.buttonAutoMap.UseVisualStyleBackColor = true;
            this.buttonAutoMap.Click += new EventHandler(this.buttonAutoMap_Click);
            this.comboBoxDestChannels.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDestChannels.Items.AddRange(new object[] { "none" });
            this.comboBoxDestChannels.Location = new Point(251, 199);
            this.comboBoxDestChannels.Name = "comboBoxDestChannels";
            this.comboBoxDestChannels.Size = new Size(121, 21);
            this.comboBoxDestChannels.TabIndex = 1;
            this.comboBoxDestChannels.Visible = false;
            this.comboBoxDestChannels.SelectedIndexChanged += new EventHandler(this.comboBoxDestChannels_SelectedIndexChanged);
            this.comboBoxDestChannels.Leave += new EventHandler(this.comboBoxDestChannels_Leave);
            this.buttonReset.Location = new Point(97, 249);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new Size(108, 23);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Reset all mappings";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new EventHandler(this.buttonReset_Click);
            this.listViewMapping.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listViewMapping.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2 });
            this.listViewMapping.FullRowSelect = true;
            this.listViewMapping.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listViewMapping.HideSelection = false;
            this.listViewMapping.Location = new Point(16, 23);
            this.listViewMapping.MultiSelect = false;
            this.listViewMapping.Name = "listViewMapping";
            this.listViewMapping.OwnerDraw = true;
            this.listViewMapping.Size = new Size(390, 215);
            this.listViewMapping.TabIndex = 0;
            this.listViewMapping.UseCompatibleStateImageBehavior = false;
            this.listViewMapping.View = View.Details;
            this.listViewMapping.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.listViewMapping_DrawColumnHeader);
            this.listViewMapping.DrawItem += new DrawListViewItemEventHandler(this.listViewMapping_DrawItem);
            this.listViewMapping.SelectedIndexChanged += new EventHandler(this.listViewMapping_SelectedIndexChanged);
            this.listViewMapping.Leave += new EventHandler(this.listViewMapping_Leave);
            this.listViewMapping.Enter += new EventHandler(this.listViewMapping_Enter);
            this.listViewMapping.DrawSubItem += new DrawListViewSubItemEventHandler(this.listViewMapping_DrawSubItem);
            this.columnHeader1.Text = "Source channel";
            this.columnHeader2.Text = "Destination channel";
            this.checkBoxChannelDefs.AutoSize = true;
            this.checkBoxChannelDefs.Location = new Point(20, 474);
            this.checkBoxChannelDefs.Name = "checkBoxChannelDefs";
            this.checkBoxChannelDefs.Size = new Size(201, 17);
            this.checkBoxChannelDefs.TabIndex = 2;
            this.checkBoxChannelDefs.Text = "Copy definitions of selected channels";
            this.checkBoxChannelDefs.UseVisualStyleBackColor = true;
            this.checkBoxSequenceLength.AutoSize = true;
            this.checkBoxSequenceLength.Location = new Point(20, 497);
            this.checkBoxSequenceLength.Name = "checkBoxSequenceLength";
            this.checkBoxSequenceLength.Size = new Size(263, 17);
            this.checkBoxSequenceLength.TabIndex = 3;
            this.checkBoxSequenceLength.Text = "Ensure destination sequence has adequate length";
            this.checkBoxSequenceLength.UseVisualStyleBackColor = true;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new Point(295, 527);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(376, 527);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(463, 559);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.checkBoxSequenceLength);
            base.Controls.Add(this.checkBoxChannelDefs);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.gbAll);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "CopySequenceDialog";
            base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Copy event sequence data";
            base.FormClosing += new FormClosingEventHandler(this.CopySequenceDialog_FormClosing);
            this.gbAll.ResumeLayout(false);
            this.gbAll.PerformLayout();
            this.groupBox2.ResumeLayout(false);
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
