namespace LedTriks {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class BoardLayoutDialog : Form {
		private Size m_boardLayout = new Size(0, 0);
		private int m_cellHeight;
		private int m_cellWidth;
		private Point m_mouseDownIn;
		private bool m_sizeExceeded = false;
		private const int MAXIMUM_BOARDS = 4;

		public BoardLayoutDialog(Size layout) {
			this.InitializeComponent();
			this.m_cellWidth = this.pictureBoxLayout.Width / 4;
			this.m_cellHeight = this.pictureBoxLayout.Height / 4;
			this.m_boardLayout = layout;
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			if (this.m_sizeExceeded) {
				base.DialogResult = System.Windows.Forms.DialogResult.None;
				MessageBox.Show("The layout exceeds the number of boards.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void Calc() {
			this.m_sizeExceeded = (this.m_boardLayout.Width * this.m_boardLayout.Height) > 4;
		}

		private void pictureBoxLayout_MouseDown(object sender, MouseEventArgs e) {
			this.m_mouseDownIn.X = e.X / this.m_cellWidth;
			this.m_mouseDownIn.Y = e.Y / this.m_cellHeight;
			this.m_boardLayout.Width = 1;
			this.m_boardLayout.Height = 1;
			this.UpdatePicture();
		}

		private void pictureBoxLayout_MouseMove(object sender, MouseEventArgs e) {
			if ((e.Button & MouseButtons.Left) != MouseButtons.None) {
				Size boardLayout = this.m_boardLayout;
				this.m_boardLayout.Width = ((e.X / this.m_cellWidth) - this.m_mouseDownIn.X) + 1;
				this.m_boardLayout.Height = ((e.Y / this.m_cellHeight) - this.m_mouseDownIn.Y) + 1;
				if (boardLayout != this.m_boardLayout) {
					this.UpdatePicture();
				}
			}
		}

		private void pictureBoxLayout_Paint(object sender, PaintEventArgs e) {
			Graphics graphics = e.Graphics;
			graphics.Clear(Color.White);
			if (this.m_boardLayout.Width > 0) {
				Brush brush = this.m_sizeExceeded ? Brushes.Red : Brushes.Green;
				graphics.FillRectangle(brush, (int)(this.m_mouseDownIn.X * this.m_cellWidth), (int)(this.m_mouseDownIn.Y * this.m_cellHeight), (int)(this.m_boardLayout.Width * this.m_cellWidth), (int)(this.m_boardLayout.Height * this.m_cellHeight));
			}
			Rectangle clientRectangle = this.pictureBoxLayout.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			graphics.DrawRectangle(Pens.Black, clientRectangle);
			for (int i = this.m_cellWidth; i < this.pictureBoxLayout.Width; i += this.m_cellWidth) {
				graphics.DrawLine(Pens.Black, i, 0, i, this.pictureBoxLayout.Height);
			}
			for (int j = this.m_cellHeight; j < this.pictureBoxLayout.Height; j += this.m_cellHeight) {
				graphics.DrawLine(Pens.Black, 0, j, this.pictureBoxLayout.Width, j);
			}
		}

		private void UpdatePicture() {
			this.Calc();
			this.pictureBoxLayout.Refresh();
		}

		public Size BoardLayout {
			get {
				return this.m_boardLayout;
			}
		}
	}
}