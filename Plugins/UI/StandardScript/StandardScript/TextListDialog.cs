namespace StandardScript
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class TextListDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components = null;
        private string m_childName;
        private XmlNode m_contextNode;
        private bool m_suppressBlanks;
        private TextBox textBoxItems;

        public TextListDialog(XmlNode contextNode, string childName, bool suppressBlanks)
        {
            this.InitializeComponent();
            this.m_contextNode = contextNode;
            this.m_childName = childName;
            this.m_suppressBlanks = suppressBlanks;
            List<string> list = new List<string>();
            foreach (XmlNode node in contextNode.SelectNodes(childName))
            {
                if (!(this.m_suppressBlanks && (!this.m_suppressBlanks || (node.InnerText.Trim().Length == 0))))
                {
                    list.Add(node.InnerText);
                }
            }
            this.textBoxItems.Lines = list.ToArray();
            base.ActiveControl = this.textBoxItems;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.m_contextNode.RemoveAll();
            foreach (string str in this.textBoxItems.Lines)
            {
                if (!(this.m_suppressBlanks && (!this.m_suppressBlanks || (str.Trim().Length == 0))))
                {
                    Xml.SetNewValue(this.m_contextNode, this.m_childName, str);
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
            this.buttonOK = new Button();
            this.textBoxItems = new TextBox();
            this.buttonCancel = new Button();
            base.SuspendLayout();
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x76, 0x11b);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.textBoxItems.AcceptsReturn = true;
            this.textBoxItems.Location = new Point(12, 12);
            this.textBoxItems.Multiline = true;
            this.textBoxItems.Name = "textBoxItems";
            this.textBoxItems.ScrollBars = ScrollBars.Vertical;
            this.textBoxItems.Size = new Size(0x106, 0x102);
            this.textBoxItems.TabIndex = 0;
            this.textBoxItems.WordWrap = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xc7, 0x11b);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x11e, 0x13e);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.textBoxItems);
            base.Controls.Add(this.buttonOK);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "TextListDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "TextListDialog";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

