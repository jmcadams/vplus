using System.ComponentModel;
using System.Windows.Forms;

namespace VixenEditor
{

    public partial class ConsoleTrackBar {
        private readonly IContainer components = null;
        private Label _label;
        private Panel _panelText;
        private TextBox _textBox;
        private TrackBar _trackBar;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this._trackBar = new System.Windows.Forms.TrackBar();
            this._textBox = new System.Windows.Forms.TextBox();
            this._label = new System.Windows.Forms.Label();
            this._panelText = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // _trackBar
            // 
            this._trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._trackBar.LargeChange = 16;
            this._trackBar.Location = new System.Drawing.Point(33, 3);
            this._trackBar.Maximum = 255;
            this._trackBar.Name = "_trackBar";
            this._trackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this._trackBar.Size = new System.Drawing.Size(45, 229);
            this._trackBar.TabIndex = 1;
            this._trackBar.TickFrequency = 16;
            this._trackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this._trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // _textBox
            // 
            this._textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox.Location = new System.Drawing.Point(38, 238);
            this._textBox.Name = "_textBox";
            this._textBox.Size = new System.Drawing.Size(35, 20);
            this._textBox.TabIndex = 2;
            this._textBox.Text = "0";
            this._textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this._textBox.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // _label
            // 
            this._label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._label.AutoSize = true;
            this._label.Location = new System.Drawing.Point(40, 261);
            this._label.Name = "_label";
            this._label.Size = new System.Drawing.Size(21, 13);
            this._label.TabIndex = 3;
            this._label.Text = "0%";
            // 
            // _panelText
            // 
            this._panelText.BackColor = System.Drawing.Color.Gainsboro;
            this._panelText.Dock = System.Windows.Forms.DockStyle.Left;
            this._panelText.Enabled = false;
            this._panelText.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._panelText.Location = new System.Drawing.Point(0, 0);
            this._panelText.Name = "_panelText";
            this._panelText.Size = new System.Drawing.Size(18, 284);
            this._panelText.TabIndex = 5;
            this._panelText.EnabledChanged += new System.EventHandler(this.panelText_EnabledChanged);
            this._panelText.Click += new System.EventHandler(this.panelText_Click);
            this._panelText.Paint += new System.Windows.Forms.PaintEventHandler(this.panelText_Paint);
            // 
            // ConsoleTrackBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._panelText);
            this.Controls.Add(this._label);
            this.Controls.Add(this._textBox);
            this.Controls.Add(this._trackBar);
            this.Name = "ConsoleTrackBar";
            this.Size = new System.Drawing.Size(90, 284);
            ((System.ComponentModel.ISupportInitialize)(this._trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    }
}
