using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Vixen
{
	public partial class AllPluginsDialog
	{
        private IContainer components = null;

		#region Windows Form Designer generated code
		private ColumnHeader Author;
        private Button buttonDone;
        private ColumnHeader Description;
        private ColumnHeader FileName;
        private ListView lvPlugins;
        private ColumnHeader Location;
        private ColumnHeader PlugInName;
        private ColumnHeader Version;

		private void InitializeComponent()
		{
            this.lvPlugins = new System.Windows.Forms.ListView();
            this.PlugInName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Version = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Location = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Author = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvPlugins
            // 
            this.lvPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PlugInName,
            this.Description,
            this.FileName,
            this.Version,
            this.Location,
            this.Author});
            this.lvPlugins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvPlugins.Location = new System.Drawing.Point(12, 12);
            this.lvPlugins.Name = "lvPlugins";
            this.lvPlugins.Size = new System.Drawing.Size(549, 261);
            this.lvPlugins.TabIndex = 0;
            this.lvPlugins.UseCompatibleStateImageBehavior = false;
            this.lvPlugins.View = System.Windows.Forms.View.Details;
            // 
            // PlugInName
            // 
            this.PlugInName.Text = "Name";
            // 
            // Description
            // 
            this.Description.Text = "Description";
            // 
            // FileName
            // 
            this.FileName.Text = "File name";
            // 
            // Version
            // 
            this.Version.Text = "Version";
            // 
            // Location
            // 
            this.Location.Text = "Location";
            // 
            // Author
            // 
            this.Author.Text = "Author";
            // 
            // buttonDone
            // 
            this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonDone.Location = new System.Drawing.Point(486, 279);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 2;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            // 
            // AllPluginsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonDone;
            this.ClientSize = new System.Drawing.Size(573, 314);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.lvPlugins);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AllPluginsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "All Plugins";
            this.Load += new System.EventHandler(this.AllPluginsDialog_Load);
            this.ResumeLayout(false);

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
