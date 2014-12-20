using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker {
    partial class NutcrackerPaletteManager {
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
            this.btnSave1 = new Button();
            this.btnLoad1 = new Button();
            this.btnDeletePalette = new Button();
            this.btnDone = new Button();
            this.lbPalettes = new ListBox();
            this.SuspendLayout();
            // 
            // btnSave1
            // 
            this.btnSave1.Location = new Point(177, 12);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new Size(75, 23);
            this.btnSave1.TabIndex = 2;
            this.btnSave1.Text = "Save";
            this.btnSave1.UseVisualStyleBackColor = true;
            this.btnSave1.Click += new EventHandler(this.Save_Click);
            // 
            // btnLoad1
            // 
            this.btnLoad1.Location = new Point(177, 41);
            this.btnLoad1.Name = "btnLoad1";
            this.btnLoad1.Size = new Size(75, 23);
            this.btnLoad1.TabIndex = 4;
            this.btnLoad1.Text = "Load";
            this.btnLoad1.UseVisualStyleBackColor = true;
            this.btnLoad1.Click += new EventHandler(this.Load_Click);
            // 
            // btnDeletePalette
            // 
            this.btnDeletePalette.Location = new Point(177, 70);
            this.btnDeletePalette.Name = "btnDeletePalette";
            this.btnDeletePalette.Size = new Size(75, 23);
            this.btnDeletePalette.TabIndex = 6;
            this.btnDeletePalette.Text = "Delete";
            this.btnDeletePalette.UseVisualStyleBackColor = true;
            this.btnDeletePalette.Click += new EventHandler(this.Delete_Click);
            // 
            // btnDone
            // 
            this.btnDone.DialogResult = DialogResult.OK;
            this.btnDone.Location = new Point(177, 227);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new Size(75, 23);
            this.btnDone.TabIndex = 7;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            // 
            // lbPalettes
            // 
            this.lbPalettes.FormattingEnabled = true;
            this.lbPalettes.IntegralHeight = false;
            this.lbPalettes.Location = new Point(12, 12);
            this.lbPalettes.Name = "lbPalettes";
            this.lbPalettes.Size = new Size(159, 238);
            this.lbPalettes.TabIndex = 8;
            this.lbPalettes.SelectedIndexChanged += new EventHandler(this.lbPalettes_SelectedIndexChanged);
            // 
            // NutcrackerPaletteManager
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(264, 262);
            this.ControlBox = false;
            this.Controls.Add(this.lbPalettes);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnDeletePalette);
            this.Controls.Add(this.btnLoad1);
            this.Controls.Add(this.btnSave1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NutcrackerPaletteManager";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Palette Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnSave1;
        private Button btnLoad1;
        private Button btnDeletePalette;
        private Button btnDone;
        private ListBox lbPalettes;
    }
}