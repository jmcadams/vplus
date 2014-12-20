using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlusCommon{
    public partial class TextQueryDialog{
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Button buttonCancel;
        private Button buttonOK;
        private Label labelQuery;
        private TextBox textBoxResponse;

        private void InitializeComponent()
        {
            this.labelQuery = new Label();
            this.textBoxResponse = new TextBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.SuspendLayout();
            // 
            // labelQuery
            // 
            this.labelQuery.AutoSize = true;
            this.labelQuery.Location = new Point(11, 12);
            this.labelQuery.Name = "labelQuery";
            this.labelQuery.Size = new Size(35, 13);
            this.labelQuery.TabIndex = 0;
            this.labelQuery.Text = "label1";
            // 
            // textBoxResponse
            // 
            this.textBoxResponse.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.textBoxResponse.Location = new Point(10, 76);
            this.textBoxResponse.Name = "textBoxResponse";
            this.textBoxResponse.Size = new Size(396, 20);
            this.textBoxResponse.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(246, 111);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(327, 111);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // TextQueryDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(414, 142);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxResponse);
            this.Controls.Add(this.labelQuery);
            this.MinimumSize = new Size(430, 180);
            this.Name = "TextQueryDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

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
