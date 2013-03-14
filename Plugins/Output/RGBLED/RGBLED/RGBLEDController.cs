namespace RGBLED
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class RGBLEDController : UserControl
    {
        private Button buttonDown;
        private Button buttonRemove;
        private Button buttonUp;
        private ComboBox comboBoxConfiguration;
        private IContainer components = null;
        private GroupBox groupBox;
        private Label label1;

        public event OnIndexChange IndexChange;

        public RGBLEDController(int id)
        {
            this.InitializeComponent();
            this.groupBox.Text = "Controller " + id.ToString();
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            int index = base.Parent.Controls.IndexOf(this);
            if (index > 0)
            {
                base.Parent.Controls.SetChildIndex(this, index - 1);
                if (this.IndexChange != null)
                {
                    this.IndexChange();
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            base.Parent.Controls.Remove(this);
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = base.Parent.Controls.IndexOf(this);
            if (index < (base.Parent.Controls.Count - 1))
            {
                base.Parent.Controls.SetChildIndex(this, index + 1);
                if (this.IndexChange != null)
                {
                    this.IndexChange();
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
            this.groupBox = new GroupBox();
            this.label1 = new Label();
            this.comboBoxConfiguration = new ComboBox();
            this.buttonRemove = new Button();
            this.buttonUp = new Button();
            this.buttonDown = new Button();
            this.groupBox.SuspendLayout();
            base.SuspendLayout();
            this.groupBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox.Controls.Add(this.buttonDown);
            this.groupBox.Controls.Add(this.buttonUp);
            this.groupBox.Controls.Add(this.buttonRemove);
            this.groupBox.Controls.Add(this.comboBoxConfiguration);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Location = new Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new Size(0x12f, 0x5d);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "groupBox1";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 0x1b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Configuration:";
            this.comboBoxConfiguration.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.comboBoxConfiguration.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxConfiguration.FormattingEnabled = true;
            this.comboBoxConfiguration.Items.AddRange(new object[] { "Software PWM", "Hardware PWM" });
            this.comboBoxConfiguration.Location = new Point(0x54, 0x18);
            this.comboBoxConfiguration.Name = "comboBoxConfiguration";
            this.comboBoxConfiguration.Size = new Size(0x97, 0x15);
            this.comboBoxConfiguration.TabIndex = 1;
            this.buttonRemove.Location = new Point(9, 0x3a);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new Size(0x4b, 0x17);
            this.buttonRemove.TabIndex = 2;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
            this.buttonUp.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonUp.FlatStyle = FlatStyle.System;
            this.buttonUp.Font = new Font("Wingdings", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.buttonUp.Location = new Point(0xf1, 0x17);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new Size(0x19, 0x17);
            this.buttonUp.TabIndex = 3;
            this.buttonUp.Text = "\x00e9";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new EventHandler(this.buttonUp_Click);
            this.buttonDown.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonDown.FlatStyle = FlatStyle.System;
            this.buttonDown.Font = new Font("Wingdings", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.buttonDown.Location = new Point(0x110, 0x17);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new Size(0x19, 0x17);
            this.buttonDown.TabIndex = 4;
            this.buttonDown.Text = "\x00ea";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new EventHandler(this.buttonDown_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.groupBox);
            base.Name = "RGBLEDController";
            base.Size = new Size(0x12f, 0x5d);
            base.ParentChanged += new EventHandler(this.RGBLEDController_ParentChanged);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            base.ResumeLayout(false);
        }

        private void RGBLEDController_ParentChanged(object sender, EventArgs e)
        {
            if (base.Parent != null)
            {
                base.Width = base.Parent.ClientRectangle.Width;
            }
        }

        public string Configuration
        {
            get
            {
                return this.comboBoxConfiguration.SelectedItem.ToString();
            }
            set
            {
                if (value.ToLower() == "hardware")
                {
                    this.comboBoxConfiguration.SelectedIndex = 1;
                }
                else
                {
                    this.comboBoxConfiguration.SelectedIndex = 0;
                }
            }
        }

        public int ID
        {
            set
            {
                this.groupBox.Text = "Controller " + value.ToString();
            }
        }

        public delegate void OnIndexChange();
    }
}

