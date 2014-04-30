using System.Windows.Forms;

namespace Controllers.Launcher {
    public partial class SetupDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonAdd;
        private Button buttonRemove;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ListView listViewPrograms;
        private OpenFileDialog openFileDialog;


        private void InitializeComponent() {
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listViewPrograms = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonFileDialog = new System.Windows.Forms.Button();
            this.textBoxParameters = new System.Windows.Forms.TextBox();
            this.textBoxTriggerValue = new System.Windows.Forms.TextBox();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonRemove
            // 
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new System.Drawing.Point(547, 32);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 5;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(547, 3);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 4;
            this.buttonAdd.Text = "Add New";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // listViewPrograms
            // 
            this.listViewPrograms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewPrograms.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewPrograms.LabelWrap = false;
            this.listViewPrograms.Location = new System.Drawing.Point(0, 0);
            this.listViewPrograms.MultiSelect = false;
            this.listViewPrograms.Name = "listViewPrograms";
            this.listViewPrograms.OwnerDraw = true;
            this.listViewPrograms.Size = new System.Drawing.Size(527, 195);
            this.listViewPrograms.TabIndex = 0;
            this.listViewPrograms.UseCompatibleStateImageBehavior = false;
            this.listViewPrograms.View = System.Windows.Forms.View.Details;
            this.listViewPrograms.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listViewPrograms_ColumnWidthChanging);
            this.listViewPrograms.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listViewPrograms_DrawColumnHeader);
            this.listViewPrograms.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewPrograms_DrawItem);
            this.listViewPrograms.SelectedIndexChanged += new System.EventHandler(this.listViewPrograms_SelectedIndexChanged);
            this.listViewPrograms.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewPrograms_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Program executable path";
            this.columnHeader1.Width = 275;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Parameters";
            this.columnHeader2.Width = 145;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Trigger level (%)";
            this.columnHeader3.Width = 90;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "exe";
            this.openFileDialog.Filter = "Executable files | *.exe";
            this.openFileDialog.Title = "Select Program To Launch";
            // 
            // buttonFileDialog
            // 
            this.buttonFileDialog.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFileDialog.Location = new System.Drawing.Point(3, 32);
            this.buttonFileDialog.Name = "buttonFileDialog";
            this.buttonFileDialog.Size = new System.Drawing.Size(24, 18);
            this.buttonFileDialog.TabIndex = 3;
            this.buttonFileDialog.Text = "...";
            this.buttonFileDialog.UseVisualStyleBackColor = true;
            this.buttonFileDialog.Visible = false;
            this.buttonFileDialog.Click += new System.EventHandler(this.buttonFileDialog_Click);
            // 
            // textBoxParameters
            // 
            this.textBoxParameters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxParameters.Location = new System.Drawing.Point(267, 33);
            this.textBoxParameters.Name = "textBoxParameters";
            this.textBoxParameters.Size = new System.Drawing.Size(145, 13);
            this.textBoxParameters.TabIndex = 7;
            this.textBoxParameters.Visible = false;
            this.textBoxParameters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPath_KeyDown);
            this.textBoxParameters.Leave += new System.EventHandler(this.textBoxPath_Leave);
            // 
            // textBoxTriggerValue
            // 
            this.textBoxTriggerValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTriggerValue.Location = new System.Drawing.Point(418, 33);
            this.textBoxTriggerValue.Name = "textBoxTriggerValue";
            this.textBoxTriggerValue.Size = new System.Drawing.Size(100, 13);
            this.textBoxTriggerValue.TabIndex = 8;
            this.textBoxTriggerValue.Visible = false;
            this.textBoxTriggerValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPath_KeyDown);
            this.textBoxTriggerValue.Leave += new System.EventHandler(this.textBoxPath_Leave);
            // 
            // textBoxPath
            // 
            this.textBoxPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPath.Location = new System.Drawing.Point(33, 33);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(100, 13);
            this.textBoxPath.TabIndex = 6;
            this.textBoxPath.Visible = false;
            this.textBoxPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPath_KeyDown);
            this.textBoxPath.Leave += new System.EventHandler(this.textBoxPath_Leave);
            // 
            // SetupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.textBoxTriggerValue);
            this.Controls.Add(this.textBoxParameters);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonFileDialog);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.listViewPrograms);
            this.Name = "SetupDialog";
            this.Size = new System.Drawing.Size(671, 238);
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

        private Button buttonFileDialog;
        private TextBox textBoxParameters;
        private TextBox textBoxTriggerValue;
        private TextBox textBoxPath;
    }
}
