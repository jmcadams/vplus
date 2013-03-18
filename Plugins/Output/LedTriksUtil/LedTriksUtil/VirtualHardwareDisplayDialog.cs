namespace LedTriksUtil
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    internal class VirtualHardwareDisplayDialog : OutputPlugInUIBase
    {
        private const int BOARD_HEIGHT = 0x10;
        private const int BOARD_WIDTH = 0x30;
        private IContainer components;
        private Size m_boardLayout;
        private Point[] m_boardLocations;
        private Point[,] m_boardMultipliers;
        private int m_boardsUsed;
        private List<uint> m_cells;
        private int m_cellSize;
        private SolidBrush m_ledBrush;
        private int m_ledSize;
        private const int MAX_BOARDS = 4;

        public VirtualHardwareDisplayDialog(Size boardLayout, int ledSize, Color ledColor, int dotPitch)
        {
            int num;
            int num2;
            this.m_ledSize = 4;
            this.m_boardLocations = new Point[4];
            this.m_boardMultipliers = new Point[4, 4];
            this.components = null;
            this.InitializeComponent();
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.ClientSize = new Size(((boardLayout.Width * 0x30) * (this.m_ledSize + dotPitch)) - dotPitch, ((boardLayout.Height * 0x10) * (this.m_ledSize + dotPitch)) - dotPitch);
            this.m_cells = new List<uint>();
            this.m_cellSize = this.m_ledSize + dotPitch;
            this.m_ledBrush = new SolidBrush(ledColor);
            this.m_boardLayout = boardLayout;
            this.m_ledSize = ledSize;
            this.m_boardsUsed = this.m_boardLayout.Width * this.m_boardLayout.Height;
            for (num2 = 0; num2 < this.m_boardLayout.Height; num2++)
            {
                num = 0;
                while (num < this.m_boardLayout.Width)
                {
                    this.m_boardLocations[(num2 * this.m_boardLayout.Width) + num] = new Point(num, num2);
                    num++;
                }
            }
            for (num2 = 0; num2 < 4; num2++)
            {
                for (num = 0; num < 4; num++)
                {
                    if ((num2 < this.m_boardLayout.Height) && (num < this.m_boardLayout.Width))
                    {
                        *(this.m_boardMultipliers[num2, num]) = new Point(num * 0x30, num2 * 0x10);
                    }
                    else
                    {
                        *(this.m_boardMultipliers[num2, num]) = new Point(-1, -1);
                    }
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
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x11c, 0x108);
            base.ControlBox = false;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "DisplayDialog";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "LedTriks Virtual Display";
            base.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.FillRectangle(Brushes.Black, e.ClipRectangle);
            foreach (uint num3 in this.m_cells)
            {
                int x = (int) (num3 >> 0x10);
                int y = ((int) num3) & 0xffff;
                graphics.FillRectangle(this.m_ledBrush, x, y, this.m_ledSize, this.m_ledSize);
            }
        }

        public void OutputFrame(ParsedFrame frame)
        {
            this.m_cells.Clear();
            for (short i = 0; i < 0x10; i = (short) (i + 1))
            {
                for (short j = 0; j < 0x30; j = (short) (j + 1))
                {
                    byte num3 = frame.CellData(i, j);
                    byte num5 = 1;
                    for (int k = 0; k < this.m_boardsUsed; k++)
                    {
                        Point point = this.m_boardLocations[k];
                        Point point2 = *(this.m_boardMultipliers[point.Y, point.X]);
                        if ((num3 & num5) != 0)
                        {
                            this.m_cells.Add((uint) ((((j + point2.X) << 0x10) * this.m_cellSize) | ((i + point2.Y) * this.m_cellSize)));
                        }
                        num5 = (byte) (num5 << 1);
                    }
                }
            }
            if (!base.IsDisposed)
            {
                if (base.InvokeRequired)
                {
                    base.Invoke(new MethodInvoker(this.Refresh));
                }
                else
                {
                    this.Refresh();
                }
            }
        }
    }
}

