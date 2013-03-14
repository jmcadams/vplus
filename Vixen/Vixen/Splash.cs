namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    internal class Splash : Form
    {
        private IContainer components = null;
        private Bitmap m_backgroundPicture = null;
        private SolidBrush m_bgBrush = null;
        private Pen m_borderPen = null;
        private Bitmap m_bulbOff = null;
        private Bitmap m_bulbOn = null;
        private Point[][] m_taskBulbPoints;
        private int m_taskIndex = -1;
        private string m_text = string.Empty;
        private SolidBrush m_textBrush = null;

        public Splash()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            this.m_bgBrush = new SolidBrush(Color.FromArgb(0x80, 0x80, 0x94));
            this.m_borderPen = new Pen(Color.FromArgb(160, 160, 0xb1), 3f);
            this.m_textBrush = new SolidBrush(Color.FromArgb(0xfe, 0xfe, 0xfe));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.m_backgroundPicture != null)
            {
                this.m_backgroundPicture.Dispose();
            }
            if (this.m_bulbOff != null)
            {
                this.m_bulbOff.Dispose();
            }
            if (this.m_bulbOn != null)
            {
                this.m_bulbOn.Dispose();
            }
            if (this.m_bgBrush != null)
            {
                this.m_bgBrush.Dispose();
            }
            if (this.m_borderPen != null)
            {
                this.m_borderPen.Dispose();
            }
            if (this.m_textBrush != null)
            {
                this.m_textBrush.Dispose();
            }
            base.Dispose(disposing);
        }

        private Color GetColor(XmlNode node, Color defaultColor)
        {
            if (node != null)
            {
                int num;
                int num2;
                int num3;
                string[] strArray = node.InnerText.Split(new char[] { ',' });
                if ((((strArray.Length == 3) && int.TryParse(strArray[0].Trim(), out num)) && int.TryParse(strArray[1].Trim(), out num2)) && int.TryParse(strArray[2].Trim(), out num3))
                {
                    return Color.FromArgb(num, num2, num3);
                }
                return defaultColor;
            }
            return defaultColor;
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.White;
            base.ClientSize = new Size(300, 0x181);
            this.DoubleBuffered = true;
            this.Font = new Font("Arial", 14f, FontStyle.Bold, GraphicsUnit.Point, 0);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Margin = new Padding(7, 6, 7, 6);
            base.Name = "Splash";
            base.StartPosition = FormStartPosition.CenterScreen;
            base.TopMost = true;
            base.TransparencyKey = Color.Transparent;
            base.Load += new EventHandler(this.Splash_Load);
            base.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            RectangleF ef;
            GraphicsPath path = this.RoundedRectPath(3, 0x152, base.Width - 6, 0x2c, 8);
            e.Graphics.FillPath(this.m_bgBrush, path);
            e.Graphics.DrawPath(this.m_borderPen, path);
            path.Dispose();
            float width = e.Graphics.MeasureString(this.m_text, this.Font).Width;
            if (width > (base.Width - 20))
            {
                ef = new RectangleF(10f, 350f, (float) (base.Width - 20), 30f);
            }
            else
            {
                ef = new RectangleF((base.Width - width) / 2f, 350f, width, 30f);
            }
            e.Graphics.DrawString(this.m_text, this.Font, this.m_textBrush, ef);
            path = this.RoundedRectPath(3, 3, base.Width - 6, 0x13f, 8);
            e.Graphics.FillPath(this.m_bgBrush, path);
            e.Graphics.DrawPath(this.m_borderPen, path);
            e.Graphics.DrawImage(this.m_backgroundPicture, 0, 0);
            int num2 = 0;
            foreach (Point[] pointArray in this.m_taskBulbPoints)
            {
                Bitmap image = (num2 <= this.m_taskIndex) ? this.m_bulbOn : this.m_bulbOff;
                foreach (Point point in pointArray)
                {
                    e.Graphics.DrawImage(image, point);
                }
                num2++;
            }
            path.Dispose();
        }

        private GraphicsPath RoundedRectPath(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(x + radius, y, (x + width) - (radius * 2), y);
            path.AddArc((x + width) - (radius * 2), y, radius * 2, radius * 2, 270f, 90f);
            path.AddLine((int) (x + width), (int) (y + radius), (int) (x + width), (int) ((y + height) - (radius * 2)));
            path.AddArc((int) ((x + width) - (radius * 2)), (int) ((y + height) - (radius * 2)), (int) (radius * 2), (int) (radius * 2), 0f, 90f);
            path.AddLine((int) ((x + width) - (radius * 2)), (int) (y + height), (int) (x + radius), (int) (y + height));
            path.AddArc(x, (y + height) - (radius * 2), radius * 2, radius * 2, 90f, 90f);
            path.AddLine(x, (y + height) - (radius * 2), x, y + radius);
            path.AddArc(x, y, radius * 2, radius * 2, 180f, 90f);
            path.CloseFigure();
            return path;
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            this.m_backgroundPicture = new Bitmap(base.Width, base.Height);
            this.m_bulbOn = new Bitmap(1, 1);
            this.m_bulbOff = new Bitmap(1, 1);
            this.m_taskBulbPoints = new Point[0][];
            string path = Path.Combine(Paths.DataPath, "splash.config");
            if (File.Exists(path))
            {
                XmlDocument document = new XmlDocument();
                document.Load(path);
                this.m_bgBrush.Color = this.GetColor(document.SelectSingleNode("//Splash/Colors/Background"), this.m_bgBrush.Color);
                this.m_borderPen.Color = this.GetColor(document.SelectSingleNode("//Splash/Colors/Border"), this.m_borderPen.Color);
                this.m_textBrush.Color = this.GetColor(document.SelectSingleNode("//Splash/Colors/Text"), this.m_textBrush.Color);
                XmlNode node = document.SelectSingleNode("//Splash/Background");
                if (node != null)
                {
                    path = Path.Combine(Paths.DataPath, node.InnerText);
                    if (File.Exists(path))
                    {
                        this.m_backgroundPicture = new Bitmap(path);
                        this.m_backgroundPicture.MakeTransparent(Color.White);
                    }
                }
                node = document.SelectSingleNode("//Splash/Bulbs/On");
                if (node != null)
                {
                    path = Path.Combine(Paths.DataPath, node.InnerText);
                    if (File.Exists(path))
                    {
                        this.m_bulbOn = new Bitmap(path);
                        this.m_bulbOn.MakeTransparent(Color.White);
                    }
                }
                node = document.SelectSingleNode("//Splash/Bulbs/Off");
                if (node != null)
                {
                    path = Path.Combine(Paths.DataPath, node.InnerText);
                    if (File.Exists(path))
                    {
                        this.m_bulbOff = new Bitmap(path);
                        this.m_bulbOff.MakeTransparent(Color.White);
                    }
                }
                node = document.SelectSingleNode("//Splash/Tasks");
                if (node != null)
                {
                    XmlNodeList list = node.SelectNodes("Task");
                    this.m_taskBulbPoints = new Point[list.Count][];
                    int index = 0;
                    int num4 = this.m_bulbOn.Width >> 1;
                    int num5 = this.m_bulbOn.Height >> 1;
                    foreach (XmlNode node2 in list)
                    {
                        string[] strArray = node2.InnerText.Split(new char[] { ',' });
                        this.m_taskBulbPoints[index] = new Point[strArray.Length >> 1];
                        if (strArray.Length != 1)
                        {
                            int num2 = 0;
                            for (int i = 0; num2 < strArray.Length; i++)
                            {
                                this.m_taskBulbPoints[index][i] = new Point(Convert.ToInt32(strArray[num2]) - num4, Convert.ToInt32(strArray[num2 + 1]) - num5);
                                num2 += 2;
                            }
                            index++;
                        }
                    }
                }
            }
        }

        public string Task
        {
            set
            {
                this.m_text = value;
                if (this.m_taskIndex < this.m_taskBulbPoints.GetLength(0))
                {
                    this.m_taskIndex++;
                    this.Refresh();
                }
            }
        }
    }
}

