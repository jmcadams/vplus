namespace Nutcracker {
    partial class NutcrackerPaletteManager {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.btnSave1 = new System.Windows.Forms.Button();
            this.btnLoad1 = new System.Windows.Forms.Button();
            this.btnDeletePalette = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.lbPalettes = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnSave1
            // 
            this.btnSave1.Location = new System.Drawing.Point(177, 12);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(75, 23);
            this.btnSave1.TabIndex = 2;
            this.btnSave1.Text = "Save";
            this.btnSave1.UseVisualStyleBackColor = true;
            this.btnSave1.Click += new System.EventHandler(this.Save_Click);
            // 
            // btnLoad1
            // 
            this.btnLoad1.Location = new System.Drawing.Point(177, 41);
            this.btnLoad1.Name = "btnLoad1";
            this.btnLoad1.Size = new System.Drawing.Size(75, 23);
            this.btnLoad1.TabIndex = 4;
            this.btnLoad1.Text = "Load";
            this.btnLoad1.UseVisualStyleBackColor = true;
            this.btnLoad1.Click += new System.EventHandler(this.Load_Click);
            // 
            // btnDeletePalette
            // 
            this.btnDeletePalette.Location = new System.Drawing.Point(177, 70);
            this.btnDeletePalette.Name = "btnDeletePalette";
            this.btnDeletePalette.Size = new System.Drawing.Size(75, 23);
            this.btnDeletePalette.TabIndex = 6;
            this.btnDeletePalette.Text = "Delete";
            this.btnDeletePalette.UseVisualStyleBackColor = true;
            this.btnDeletePalette.Click += new System.EventHandler(this.Delete_Click);
            // 
            // btnDone
            // 
            this.btnDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDone.Location = new System.Drawing.Point(177, 227);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 7;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            // 
            // lbPalettes
            // 
            this.lbPalettes.FormattingEnabled = true;
            this.lbPalettes.IntegralHeight = false;
            this.lbPalettes.Location = new System.Drawing.Point(12, 12);
            this.lbPalettes.Name = "lbPalettes";
            this.lbPalettes.Size = new System.Drawing.Size(159, 238);
            this.lbPalettes.TabIndex = 8;
            this.lbPalettes.SelectedIndexChanged += new System.EventHandler(this.lbPalettes_SelectedIndexChanged);
            // 
            // NutcrackerPaletteManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 262);
            this.ControlBox = false;
            this.Controls.Add(this.lbPalettes);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnDeletePalette);
            this.Controls.Add(this.btnLoad1);
            this.Controls.Add(this.btnSave1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NutcrackerPaletteManager";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Palette Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave1;
        private System.Windows.Forms.Button btnLoad1;
        private System.Windows.Forms.Button btnDeletePalette;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.ListBox lbPalettes;
    }
}