using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor
{

    public sealed partial class ConsoleTrackBar {
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
            this._trackBar = new TrackBar();
            this._textBox = new TextBox();
            this._label = new Label();
            this._panelText = new Panel();
            ((ISupportInitialize)(this._trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // _trackBar
            // 
            this._trackBar.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Right)));
            this._trackBar.LargeChange = 16;
            this._trackBar.Location = new Point(33, 3);
            this._trackBar.Maximum = 255;
            this._trackBar.Name = "_trackBar";
            this._trackBar.Orientation = Orientation.Vertical;
            this._trackBar.Size = new Size(45, 229);
            this._trackBar.TabIndex = 1;
            this._trackBar.TickFrequency = 16;
            this._trackBar.TickStyle = TickStyle.Both;
            this._trackBar.Scroll += new EventHandler(this.trackBar_Scroll);
            // 
            // _textBox
            // 
            this._textBox.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this._textBox.Location = new Point(38, 238);
            this._textBox.Name = "_textBox";
            this._textBox.Size = new Size(35, 20);
            this._textBox.TabIndex = 2;
            this._textBox.Text = "0";
            this._textBox.KeyPress += new KeyPressEventHandler(this.textBox_KeyPress);
            this._textBox.Leave += new EventHandler(this.textBox_Leave);
            // 
            // _label
            // 
            this._label.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this._label.AutoSize = true;
            this._label.Location = new Point(40, 261);
            this._label.Name = "_label";
            this._label.Size = new Size(21, 13);
            this._label.TabIndex = 3;
            this._label.Text = "0%";
            // 
            // _panelText
            // 
            this._panelText.BackColor = Color.Gainsboro;
            this._panelText.Dock = DockStyle.Left;
            this._panelText.Enabled = false;
            this._panelText.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this._panelText.Location = new Point(0, 0);
            this._panelText.Name = "_panelText";
            this._panelText.Size = new Size(18, 284);
            this._panelText.TabIndex = 5;
            this._panelText.EnabledChanged += new EventHandler(this.panelText_EnabledChanged);
            this._panelText.Click += new EventHandler(this.panelText_Click);
            this._panelText.Paint += new PaintEventHandler(this.panelText_Paint);
            // 
            // ConsoleTrackBar
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this._panelText);
            this.Controls.Add(this._label);
            this.Controls.Add(this._textBox);
            this.Controls.Add(this._trackBar);
            this.Name = "ConsoleTrackBar";
            this.Size = new Size(90, 284);
            ((ISupportInitialize)(this._trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    }
}
