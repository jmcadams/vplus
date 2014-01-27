using System.Windows.Forms;

namespace Dialogs {
    public partial class RoutineSelectDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonOK;
        private ListBox listBoxRoutines;


        private void InitializeComponent() {
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.listBoxRoutines = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(304, 422);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(385, 422);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // listBoxRoutines
            // 
            this.listBoxRoutines.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                   System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxRoutines.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxRoutines.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,
                                                                ((byte) (0)));
            this.listBoxRoutines.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.listBoxRoutines.ItemHeight = 100;
            this.listBoxRoutines.Location = new System.Drawing.Point(12, 12);
            this.listBoxRoutines.Name = "listBoxRoutines";
            this.listBoxRoutines.ScrollAlwaysVisible = true;
            this.listBoxRoutines.Size = new System.Drawing.Size(448, 404);
            this.listBoxRoutines.TabIndex = 3;
            this.listBoxRoutines.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxRoutines_DrawItem);
            this.listBoxRoutines.SelectedIndexChanged += new System.EventHandler(this.listBoxRoutines_SelectedIndexChanged);
            this.listBoxRoutines.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxRoutines_MouseDoubleClick);
            // 
            // RoutineSelectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(472, 457);
            this.Controls.Add(this.listBoxRoutines);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Name = "RoutineSelectDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Routine Selection";
            this.ResizeBegin += new System.EventHandler(this.RoutineSelectDialog_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.RoutineSelectDialog_ResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            foreach (Routine routine in this.listBoxRoutines.Items) {
                routine.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
