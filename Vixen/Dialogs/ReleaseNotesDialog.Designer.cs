using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    sealed partial class ReleaseNotesDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tbNotes = new TextBox();
            this.btnDone = new Button();
            this.SuspendLayout();
            // 
            // tbNotes
            // 
            this.tbNotes.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.tbNotes.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.tbNotes.Location = new Point(13, 13);
            this.tbNotes.Multiline = true;
            this.tbNotes.Name = "tbNotes";
            this.tbNotes.ReadOnly = true;
            this.tbNotes.ScrollBars = ScrollBars.Both;
            this.tbNotes.Size = new Size(659, 608);
            this.tbNotes.TabIndex = 0;
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.btnDone.Location = new Point(597, 627);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new Size(75, 23);
            this.btnDone.TabIndex = 1;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new EventHandler(this.btnDone_Click);
            // 
            // ReleaseNotesDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(684, 662);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.tbNotes);
            this.Name = "ReleaseNotesDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Release Notes";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox tbNotes;
        private Button btnDone;
    }
}