namespace LedTriksUtil
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BoardLayoutDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components = null;
        private GroupBox groupBox2;
        private Label label1;
        private Size m_boardLayout = new Size(0, 0);
        private int m_cellHeight;
        private int m_cellWidth;
        private Point m_mouseDownIn;
        private bool m_sizeExceeded = false;
        private const int MAXIMUM_BOARDS = 4;
        private PictureBox pictureBoxLayout;

        public BoardLayoutDialog(Size layout)
        {
            this.InitializeComponent();
            this.m_cellWidth = this.pictureBoxLayout.Width / 4;
            this.m_cellHeight = this.pictureBoxLayout.Height / 4;
            this.m_boardLayout = layout;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.m_sizeExceeded)
            {
                base.DialogResult = DialogResult.None;
                MessageBox.Show("The layout exceeds the number of boards.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void Calc()
        {
            this.m_sizeExceeded = (this.m_boardLayout.Width * this.m_boardLayout.Height) > 4;
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
            this.groupBox2 = new GroupBox();
            this.label1 = new Label();
            this.pictureBoxLayout = new PictureBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox2.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxLayout).BeginInit();
            base.SuspendLayout();
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.pictureBoxLayout);
            this.groupBox2.Location = new Point(12, 0x10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x18e, 0x10d);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Layout";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x13, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x171, 0x1a);
            this.label1.TabIndex = 1;
            this.label1.Text = "Use your mouse to click and drag an area to select the layout of your boards.\r\nIf it turns red, you've exceeded the maximum number of boards.";
            this.pictureBoxLayout.BackColor = Color.White;
            this.pictureBoxLayout.Location = new Point(0x63, 0x37);
            this.pictureBoxLayout.Name = "pictureBoxLayout";
            this.pictureBoxLayout.Size = new Size(200, 200);
            this.pictureBoxLayout.TabIndex = 0;
            this.pictureBoxLayout.TabStop = false;
            this.pictureBoxLayout.MouseDown += new MouseEventHandler(this.pictureBoxLayout_MouseDown);
            this.pictureBoxLayout.MouseMove += new MouseEventHandler(this.pictureBoxLayout_MouseMove);
            this.pictureBoxLayout.Paint += new PaintEventHandler(this.pictureBoxLayout_Paint);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0xfe, 0x123);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x14f, 0x123);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1a6, 0x146);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox2);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "BoardLayoutDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Board Layout";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((ISupportInitialize) this.pictureBoxLayout).EndInit();
            base.ResumeLayout(false);
        }

        private void pictureBoxLayout_MouseDown(object sender, MouseEventArgs e)
        {
            this.m_mouseDownIn.X = e.X / this.m_cellWidth;
            this.m_mouseDownIn.Y = e.Y / this.m_cellHeight;
            this.m_boardLayout.Width = 1;
            this.m_boardLayout.Height = 1;
            this.UpdatePicture();
        }

        private void pictureBoxLayout_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.None)
            {
                Size boardLayout = this.m_boardLayout;
                this.m_boardLayout.Width = ((e.X / this.m_cellWidth) - this.m_mouseDownIn.X) + 1;
                this.m_boardLayout.Height = ((e.Y / this.m_cellHeight) - this.m_mouseDownIn.Y) + 1;
                if (boardLayout != this.m_boardLayout)
                {
                    this.UpdatePicture();
                }
            }
        }

        private void pictureBoxLayout_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(Color.White);
            if (this.m_boardLayout.Width > 0)
            {
                Brush brush = this.m_sizeExceeded ? Brushes.Red : Brushes.Green;
                graphics.FillRectangle(brush, (int) (this.m_mouseDownIn.X * this.m_cellWidth), (int) (this.m_mouseDownIn.Y * this.m_cellHeight), (int) (this.m_boardLayout.Width * this.m_cellWidth), (int) (this.m_boardLayout.Height * this.m_cellHeight));
            }
            Rectangle clientRectangle = this.pictureBoxLayout.ClientRectangle;
            clientRectangle.Width--;
            clientRectangle.Height--;
            graphics.DrawRectangle(Pens.Black, clientRectangle);
            for (int i = this.m_cellWidth; i < this.pictureBoxLayout.Width; i += this.m_cellWidth)
            {
                graphics.DrawLine(Pens.Black, i, 0, i, this.pictureBoxLayout.Height);
            }
            for (int j = this.m_cellHeight; j < this.pictureBoxLayout.Height; j += this.m_cellHeight)
            {
                graphics.DrawLine(Pens.Black, 0, j, this.pictureBoxLayout.Width, j);
            }
        }

        private void UpdatePicture()
        {
            this.Calc();
            this.pictureBoxLayout.Refresh();
        }

        public Size BoardLayout
        {
            get
            {
                return this.m_boardLayout;
            }
        }
    }
}

