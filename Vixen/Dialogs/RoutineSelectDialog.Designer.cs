using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    public partial class RoutineSelectDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonOK;
        private ListBox listBoxRoutines;


        private void InitializeComponent() {
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.listBoxRoutines = new ListBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new Point(304, 422);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(385, 422);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // listBoxRoutines
            // 
            this.listBoxRoutines.Anchor =
                ((AnchorStyles)
                 ((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) |
                   AnchorStyles.Right)));
            this.listBoxRoutines.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBoxRoutines.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point,
                                                                ((byte) (0)));
            this.listBoxRoutines.ForeColor = Color.DarkSlateBlue;
            this.listBoxRoutines.ItemHeight = 100;
            this.listBoxRoutines.Location = new Point(12, 12);
            this.listBoxRoutines.Name = "listBoxRoutines";
            this.listBoxRoutines.ScrollAlwaysVisible = true;
            this.listBoxRoutines.Size = new Size(448, 404);
            this.listBoxRoutines.TabIndex = 3;
            this.listBoxRoutines.DrawItem += new DrawItemEventHandler(this.listBoxRoutines_DrawItem);
            this.listBoxRoutines.SelectedIndexChanged += new EventHandler(this.listBoxRoutines_SelectedIndexChanged);
            this.listBoxRoutines.MouseDoubleClick += new MouseEventHandler(this.listBoxRoutines_MouseDoubleClick);
            // 
            // RoutineSelectDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(472, 457);
            this.Controls.Add(this.listBoxRoutines);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Name = "RoutineSelectDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Routine Selection";
            this.ResizeBegin += new EventHandler(this.RoutineSelectDialog_ResizeBegin);
            this.ResizeEnd += new EventHandler(this.RoutineSelectDialog_ResizeEnd);
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
