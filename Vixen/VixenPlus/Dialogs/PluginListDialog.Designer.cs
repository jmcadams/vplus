namespace VixenPlus.Dialogs {
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;

    public sealed partial class PluginListDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonInput;
        private Button buttonOK;
        private Button buttonPluginSetup;
        private Button buttonRemove;
        private Button buttonUse;
        private CheckedListBox checkedListBoxSequencePlugins;
        private ColumnHeader columnHeaderExpandButton;
        private ColumnHeader columnHeaderPluginName;
        private ColumnHeader columnHeaderPortTypeIndex;
        private ColumnHeader columnHeaderPortTypeName;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ListView listViewOutputPorts;
        private ListView listViewPlugins;
        private PictureBox pictureBoxMinus;
        private PictureBox pictureBoxPlus;
        private ColumnHeader pluginName;
        private TextBox textBoxChannelFrom;
        private TextBox textBoxChannelTo;


        private void InitializeComponent() {
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Output",
                                                                                                       System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Input",
                                                                                                       System.Windows.Forms.HorizontalAlignment.Left);
            this.buttonUse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxChannelFrom = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxChannelTo = new System.Windows.Forms.TextBox();
            this.buttonPluginSetup = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkedListBoxSequencePlugins = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewOutputPorts = new System.Windows.Forms.ListView();
            this.columnHeaderPortTypeName = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPortTypeIndex = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderExpandButton = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPluginName = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.buttonRemove = new System.Windows.Forms.Button();
            this.pictureBoxPlus = new System.Windows.Forms.PictureBox();
            this.pictureBoxMinus = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonInput = new System.Windows.Forms.Button();
            this.listViewPlugins = new System.Windows.Forms.ListView();
            this.pluginName = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxPlus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxMinus)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonUse
            // 
            this.buttonUse.Enabled = false;
            this.buttonUse.Location = new System.Drawing.Point(214, 121);
            this.buttonUse.Name = "buttonUse";
            this.buttonUse.Size = new System.Drawing.Size(75, 23);
            this.buttonUse.TabIndex = 2;
            this.buttonUse.Text = "--- Use -->";
            this.buttonUse.UseVisualStyleBackColor = true;
            this.buttonUse.Click += new System.EventHandler(this.buttonUse_Click);
            // 
            // label1
            // 
            this.label1.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(513, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Channels";
            // 
            // textBoxChannelFrom
            // 
            this.textBoxChannelFrom.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChannelFrom.Location = new System.Drawing.Point(490, 58);
            this.textBoxChannelFrom.MaxLength = 4;
            this.textBoxChannelFrom.Name = "textBoxChannelFrom";
            this.textBoxChannelFrom.Size = new System.Drawing.Size(34, 20);
            this.textBoxChannelFrom.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(530, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "to";
            // 
            // textBoxChannelTo
            // 
            this.textBoxChannelTo.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChannelTo.Location = new System.Drawing.Point(552, 58);
            this.textBoxChannelTo.MaxLength = 4;
            this.textBoxChannelTo.Name = "textBoxChannelTo";
            this.textBoxChannelTo.Size = new System.Drawing.Size(34, 20);
            this.textBoxChannelTo.TabIndex = 9;
            // 
            // buttonPluginSetup
            // 
            this.buttonPluginSetup.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPluginSetup.Enabled = false;
            this.buttonPluginSetup.Location = new System.Drawing.Point(502, 84);
            this.buttonPluginSetup.Name = "buttonPluginSetup";
            this.buttonPluginSetup.Size = new System.Drawing.Size(75, 23);
            this.buttonPluginSetup.TabIndex = 10;
            this.buttonPluginSetup.Text = "Plugin Setup";
            this.buttonPluginSetup.UseVisualStyleBackColor = true;
            this.buttonPluginSetup.Click += new System.EventHandler(this.buttonPluginSetup_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(507, 273);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "Done";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(507, 244);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Visible = false;
            // 
            // checkedListBoxSequencePlugins
            // 
            this.checkedListBoxSequencePlugins.FormattingEnabled = true;
            this.checkedListBoxSequencePlugins.Location = new System.Drawing.Point(298, 33);
            this.checkedListBoxSequencePlugins.Name = "checkedListBoxSequencePlugins";
            this.checkedListBoxSequencePlugins.Size = new System.Drawing.Size(171, 139);
            this.checkedListBoxSequencePlugins.TabIndex = 5;
            this.checkedListBoxSequencePlugins.ItemCheck +=
                new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxSequencePlugins_ItemCheck);
            this.checkedListBoxSequencePlugins.SelectedIndexChanged += new System.EventHandler(this.listBoxSequencePlugins_SelectedIndexChanged);
            this.checkedListBoxSequencePlugins.DoubleClick += new System.EventHandler(this.checkedListBoxSequencePlugins_DoubleClick);
            this.checkedListBoxSequencePlugins.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxSequencePlugins_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listViewOutputPorts);
            this.groupBox1.Location = new System.Drawing.Point(12, 178);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(457, 118);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Port Configurations";
            // 
            // listViewOutputPorts
            // 
            this.listViewOutputPorts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {this.columnHeaderPortTypeName, this.columnHeaderPortTypeIndex, this.columnHeaderExpandButton, this.columnHeaderPluginName});
            this.listViewOutputPorts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewOutputPorts.Location = new System.Drawing.Point(6, 19);
            this.listViewOutputPorts.Name = "listViewOutputPorts";
            this.listViewOutputPorts.OwnerDraw = true;
            this.listViewOutputPorts.Size = new System.Drawing.Size(445, 93);
            this.listViewOutputPorts.TabIndex = 0;
            this.listViewOutputPorts.UseCompatibleStateImageBehavior = false;
            this.listViewOutputPorts.View = System.Windows.Forms.View.Details;
            this.listViewOutputPorts.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewOutputPorts_DrawItem);
            this.listViewOutputPorts.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listViewOutputPorts_DrawSubItem);
            this.listViewOutputPorts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewOutputPorts_MouseDown);
            // 
            // columnHeaderPortTypeName
            // 
            this.columnHeaderPortTypeName.Text = "PortTypeName";
            this.columnHeaderPortTypeName.Width = 55;
            // 
            // columnHeaderPortTypeIndex
            // 
            this.columnHeaderPortTypeIndex.Text = "PortTypeIndex";
            this.columnHeaderPortTypeIndex.Width = 41;
            // 
            // columnHeaderExpandButton
            // 
            this.columnHeaderExpandButton.Text = "";
            this.columnHeaderExpandButton.Width = 41;
            // 
            // columnHeaderPluginName
            // 
            this.columnHeaderPluginName.Text = "PluginName";
            this.columnHeaderPluginName.Width = 251;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new System.Drawing.Point(214, 150);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // pictureBoxPlus
            // 
            this.pictureBoxPlus.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxPlus.BackColor = System.Drawing.Color.White;
            this.pictureBoxPlus.Location = new System.Drawing.Point(498, 125);
            this.pictureBoxPlus.Name = "pictureBoxPlus";
            this.pictureBoxPlus.Size = new System.Drawing.Size(9, 9);
            this.pictureBoxPlus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxPlus.TabIndex = 12;
            this.pictureBoxPlus.TabStop = false;
            this.pictureBoxPlus.Visible = false;
            // 
            // pictureBoxMinus
            // 
            this.pictureBoxMinus.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxMinus.BackColor = System.Drawing.Color.Black;
            this.pictureBoxMinus.Location = new System.Drawing.Point(513, 125);
            this.pictureBoxMinus.Name = "pictureBoxMinus";
            this.pictureBoxMinus.Size = new System.Drawing.Size(9, 9);
            this.pictureBoxMinus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxMinus.TabIndex = 13;
            this.pictureBoxMinus.TabStop = false;
            this.pictureBoxMinus.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Available Plugins";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(295, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Plugins in Use";
            // 
            // buttonInput
            // 
            this.buttonInput.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInput.Enabled = false;
            this.buttonInput.Location = new System.Drawing.Point(502, 149);
            this.buttonInput.Name = "buttonInput";
            this.buttonInput.Size = new System.Drawing.Size(75, 23);
            this.buttonInput.TabIndex = 11;
            this.buttonInput.Text = "Inputs";
            this.buttonInput.UseVisualStyleBackColor = true;
            this.buttonInput.Click += new System.EventHandler(this.buttonInput_Click);
            // 
            // listViewPlugins
            // 
            this.listViewPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.pluginName});
            listViewGroup3.Header = "Output";
            listViewGroup3.Name = "listViewGroupOutput";
            listViewGroup4.Header = "Input";
            listViewGroup4.Name = "listViewGroupInput";
            this.listViewPlugins.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {listViewGroup3, listViewGroup4});
            this.listViewPlugins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewPlugins.HideSelection = false;
            this.listViewPlugins.Location = new System.Drawing.Point(12, 32);
            this.listViewPlugins.MultiSelect = false;
            this.listViewPlugins.Name = "listViewPlugins";
            this.listViewPlugins.Size = new System.Drawing.Size(196, 139);
            this.listViewPlugins.TabIndex = 15;
            this.listViewPlugins.UseCompatibleStateImageBehavior = false;
            this.listViewPlugins.View = System.Windows.Forms.View.Details;
            this.listViewPlugins.SelectedIndexChanged += new System.EventHandler(this.listViewPlugins_SelectedIndexChanged);
            this.listViewPlugins.DoubleClick += new System.EventHandler(this.listViewPlugins_DoubleClick);
            // 
            // PluginListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 308);
            this.Controls.Add(this.listViewPlugins);
            this.Controls.Add(this.buttonInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBoxMinus);
            this.Controls.Add(this.pictureBoxPlus);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkedListBoxSequencePlugins);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonPluginSetup);
            this.Controls.Add(this.textBoxChannelTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxChannelFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonUse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::Properties.Resources.VixenPlus;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginListDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sequence Plugin Mapping";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PluginListDialog_FormClosing);
            this.Load += new System.EventHandler(this.PluginListDialog_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxPlus)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxMinus)).EndInit();
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
    }
}
