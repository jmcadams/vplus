namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    internal class AllPluginsDialog : Form
    {
        private ColumnHeader Author;
        private Button buttonDone;
        private IContainer components = null;
        private ColumnHeader Description;
        private ColumnHeader FileName;
        private ListView listViewPlugins;
        private ColumnHeader Location;
        private ColumnHeader PlugInName;
        private ColumnHeader Version;

        public AllPluginsDialog()
        {
            this.InitializeComponent();
        }

        private void AddAssembly(Assembly assembly, string relativePath, System.Type implementor)
        {
        }

        private void AllPluginsDialog_Load(object sender, EventArgs e)
        {
            string[] strArray = new string[] { Paths.AddinPath, Paths.OutputPluginPath, Paths.TriggerPluginPath, Paths.UIPluginPath };
            Assembly assembly = null;
            System.Type implementor = null;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                foreach (string str2 in strArray)
                {
                    string relativePath = str2.Substring(Paths.BinaryPath.Length);
                    foreach (string str3 in Directory.GetFiles(str2, "*.dll", SearchOption.TopDirectoryOnly))
                    {
                        if (assembly != null)
                        {
                            this.AddAssembly(assembly, relativePath, implementor);
                        }
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listViewPlugins = new ListView();
            this.PlugInName = new ColumnHeader();
            this.Description = new ColumnHeader();
            this.FileName = new ColumnHeader();
            this.Version = new ColumnHeader();
            this.Location = new ColumnHeader();
            this.Author = new ColumnHeader();
            this.buttonDone = new Button();
            base.SuspendLayout();
            this.listViewPlugins.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listViewPlugins.Columns.AddRange(new ColumnHeader[] { this.PlugInName, this.Description, this.FileName, this.Version, this.Location, this.Author });
            this.listViewPlugins.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listViewPlugins.Location = new Point(12, 12);
            this.listViewPlugins.Name = "listViewPlugins";
            this.listViewPlugins.Size = new Size(0x225, 0x105);
            this.listViewPlugins.TabIndex = 0;
            this.listViewPlugins.UseCompatibleStateImageBehavior = false;
            this.listViewPlugins.View = View.Details;
            this.PlugInName.Text = "Name";
            this.Description.Text = "Description";
            this.FileName.Text = "File name";
            this.Version.Text = "Version";
            this.Location.Text = "Location";
            this.Author.Text = "Author";
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = DialogResult.Cancel;
            this.buttonDone.Location = new Point(0x1e6, 0x117);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 2;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonDone;
            base.ClientSize = new Size(0x23d, 0x13a);
            base.Controls.Add(this.buttonDone);
            base.Controls.Add(this.listViewPlugins);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AllPluginsDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "All Plugins";
            base.Load += new EventHandler(this.AllPluginsDialog_Load);
            base.ResumeLayout(false);
        }
    }
}

