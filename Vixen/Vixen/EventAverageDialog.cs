namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class EventAverageDialog : Form
    {
        private Button buttonDone;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private IContainer components = null;
        private ListView listView;

        public EventAverageDialog()
        {
            this.InitializeComponent();
            this.listView.Columns[0].Width = 0x92;
            this.listView.Columns[1].Width = 0x57;
            this.listView.Columns[2].Width = 0xb8;
            string debugValue = string.Empty;
            for (int i = 0; debugValue != null; i++)
            {
                debugValue = Host.GetDebugValue("event_average_" + i.ToString());
                if (debugValue != null)
                {
                    string[] strArray = debugValue.Split(new char[] { '|' });
                    ListViewItem item = new ListViewItem(new string[] { strArray[0], string.Format("{0} - {1}", strArray[1], strArray[2]), strArray[3] });
                    this.listView.Items.Add(item);
                }
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
            this.listView = new ListView();
            this.buttonDone = new Button();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            base.SuspendLayout();
            this.listView.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2, this.columnHeader3 });
            this.listView.Dock = DockStyle.Top;
            this.listView.FullRowSelect = true;
            this.listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listView.Location = new Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new Size(0x1ba, 0xcf);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = View.Details;
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = DialogResult.OK;
            this.buttonDone.Location = new Point(0x163, 0xd5);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 1;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.columnHeader1.Text = "Plugin";
            this.columnHeader2.Text = "Channels";
            this.columnHeader3.Text = "Average duration (ms)";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1ba, 0xf8);
            base.Controls.Add(this.buttonDone);
            base.Controls.Add(this.listView);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "EventAverageDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Plugin Event Averages";
            base.ResumeLayout(false);
        }
    }
}

