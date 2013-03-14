namespace Vixen.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TextQueryDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components = null;
        private Label labelQuery;
        private TextBox textBoxResponse;

        public TextQueryDialog(string caption, string query, string response)
        {
            this.InitializeComponent();
            this.Text = caption;
            this.labelQuery.Text = query;
            this.textBoxResponse.Text = response;
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
            this.labelQuery = new Label();
            this.textBoxResponse = new TextBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            base.SuspendLayout();
            this.labelQuery.AutoSize = true;
            this.labelQuery.Location = new Point(11, 12);
            this.labelQuery.Name = "labelQuery";
            this.labelQuery.Size = new Size(0x23, 13);
            this.labelQuery.TabIndex = 0;
            this.labelQuery.Text = "label1";
            this.textBoxResponse.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.textBoxResponse.Location = new Point(10, 0x21);
            this.textBoxResponse.Name = "textBoxResponse";
            this.textBoxResponse.Size = new Size(0x18c, 20);
            this.textBoxResponse.TabIndex = 1;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0xf6, 0x44);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x147, 0x44);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x19e, 0x63);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.textBoxResponse);
            base.Controls.Add(this.labelQuery);
            base.Name = "TextQueryDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public string Caption
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        public string Query
        {
            get
            {
                return this.labelQuery.Text;
            }
            set
            {
                this.labelQuery.Text = value;
            }
        }

        public string Response
        {
            get
            {
                return this.textBoxResponse.Text;
            }
            set
            {
                this.textBoxResponse.Text = value;
            }
        }
    }
}

