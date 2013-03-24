namespace Preview
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    public class PreviewDialog : OutputPlugInUIBase
    {
		//private byte color = 0;
        private IContainer components = null;
        private uint[,] m_backBuffer;
        private int m_cellSize;
        private SolidBrush m_channelBrush = new SolidBrush(Color.Black);
        private Color[] m_channelColors;
        private Dictionary<int, List<uint>> m_channelDictionary;
        private byte[] m_channelValues = new byte[0];
        private Image m_originalBackground = null;
        private int m_startChannel;
        private PictureBox pictureBoxShowGrid;

        public PreviewDialog(XmlNode setupNode, List<Channel> channels, int startChannel)
        {
            this.InitializeComponent();
            try
            {
                this.m_startChannel = startChannel;
                this.m_channelDictionary = new Dictionary<int, List<uint>>();
                bool flag = bool.Parse(VixenPlus.Xml.GetNodeAlways(setupNode, "RedirectOutputs", "False").InnerText);
                this.m_channelColors = new Color[channels.Count - startChannel];
                for (int i = startChannel; i < channels.Count; i++)
                {
                    int outputChannel;
                    if (flag)
                    {
                        outputChannel = i;
                    }
                    else
                    {
                        outputChannel = channels[i].OutputChannel;
                    }
                    if (outputChannel >= startChannel)
                    {
                        this.m_channelColors[outputChannel - startChannel] = channels[i].Color;
                    }
                }
                foreach (XmlNode node in setupNode.SelectNodes("Channels/Channel"))
                {
                    int num3 = Convert.ToInt32(node.Attributes["number"].Value);
                    if (num3 >= this.m_startChannel)
                    {
                        int num5;
                        List<uint> list = new List<uint>();
                        byte[] buffer = Convert.FromBase64String(node.InnerText);
                        for (int j = 0; j < buffer.Length; j += 4)
                        {
                            list.Add(BitConverter.ToUInt32(buffer, j));
                        }
                        if ((num5 = num3 - this.m_startChannel) < channels.Count)
                        {
                            if (flag)
                            {
                                this.m_channelDictionary[num5] = list;
                            }
                            else
                            {
                                this.m_channelDictionary[channels[num5].OutputChannel] = list;
                            }
                        }
                    }
                }
                byte[] buffer2 = Convert.FromBase64String(setupNode.SelectSingleNode("BackgroundImage").InnerText);
                if (buffer2.Length > 0)
                {
                    MemoryStream stream = new MemoryStream(buffer2);
                    this.m_originalBackground = new Bitmap(stream);
                    stream.Dispose();
                }
                XmlNode node2 = setupNode.SelectSingleNode("Display");
                this.m_cellSize = Convert.ToInt32(node2.SelectSingleNode("PixelSize").InnerText);
                if (this.m_originalBackground == null)
                {
                    this.SetPictureBoxSize(Convert.ToInt32(node2.SelectSingleNode("Width").InnerText) * (this.m_cellSize + 1), Convert.ToInt32(node2.SelectSingleNode("Height").InnerText) * (this.m_cellSize + 1));
                }
                else
                {
                    this.SetPictureBoxSize(this.m_originalBackground.Width, this.m_originalBackground.Height);
                }
                this.SetBrightness(((float) (int.Parse(node2.SelectSingleNode("Brightness").InnerText) - 10)) / 10f);
                this.m_backBuffer = new uint[Convert.ToInt32(node2.SelectSingleNode("Height").InnerText), Convert.ToInt32(node2.SelectSingleNode("Width").InnerText)];
            }
            catch (NullReferenceException exception)
            {
                throw new Exception(exception.Message + "\n\nHave you run the setup?");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.m_channelBrush != null)
            {
                this.m_channelBrush.Dispose();
            }
            if (this.m_originalBackground != null)
            {
                this.m_originalBackground.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBoxShowGrid = new PictureBox();
            ((ISupportInitialize) this.pictureBoxShowGrid).BeginInit();
            base.SuspendLayout();
            this.pictureBoxShowGrid.BackColor = Color.Transparent;
            this.pictureBoxShowGrid.BackgroundImageLayout = ImageLayout.None;
            this.pictureBoxShowGrid.Location = new Point(0, 0);
            this.pictureBoxShowGrid.Name = "pictureBoxShowGrid";
            this.pictureBoxShowGrid.Size = new Size(0x240, 0x120);
            this.pictureBoxShowGrid.TabIndex = 14;
            this.pictureBoxShowGrid.TabStop = false;
            this.pictureBoxShowGrid.Paint += new PaintEventHandler(this.pictureBoxShowGrid_Paint);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.ClientSize = new Size(0x240, 0x11e);
            base.ControlBox = false;
            base.Controls.Add(this.pictureBoxShowGrid);
            base.Name = "PreviewDialog";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Sequence Preview";
            base.FormClosing += new FormClosingEventHandler(this.PreviewDialog_FormClosing);
            ((ISupportInitialize) this.pictureBoxShowGrid).EndInit();
            base.ResumeLayout(false);
        }

        private void pictureBoxShowGrid_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Transparent, this.pictureBoxShowGrid.ClientRectangle);
            int length = this.m_backBuffer.GetLength(0);
            int num2 = this.m_backBuffer.GetLength(1);
            int num5 = this.m_cellSize + 1;
            int y = 0;
            int num8 = 0;
            while (num8 < length)
            {
                int x = 0;
                int num7 = 0;
                while (num7 < num2)
                {
                    uint num6 = this.m_backBuffer[num8, num7];
                    if ((num6 & 0xff000000) > 0)
                    {
                        this.m_channelBrush.Color = Color.FromArgb((int) num6);
                        e.Graphics.FillRectangle(this.m_channelBrush, x, y, this.m_cellSize, this.m_cellSize);
                    }
                    num7++;
                    x += num5;
                }
                num8++;
                y += num5;
            }
        }

        private void PreviewDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = e.CloseReason == CloseReason.UserClosing;
        }

        private void Recalc()
        {
            int index = 0;
            Array.Clear(this.m_backBuffer, 0, this.m_backBuffer.Length);
            int length = this.m_backBuffer.GetLength(1);
            int num15 = this.m_backBuffer.GetLength(0);
            foreach (byte num16 in this.m_channelValues)
            {
                Color color = this.m_channelColors[index];
                if (this.m_channelDictionary.ContainsKey(index) && (num16 > 0))
                {
                    foreach (uint num17 in this.m_channelDictionary[index])
                    {
                        int num6 = (int) (num17 >> 0x10);
                        int num7 = ((int) num17) & 0xffff;
                        if ((num6 < length) && (num7 < num15))
                        {
                            byte r;
                            byte g;
                            byte b;
                            byte num8;
                            uint num2 = this.m_backBuffer[num7, num6];
                            if (num2 == 0)
                            {
                                num8 = num16;
                                r = color.R;
                                g = color.G;
                                b = color.B;
                            }
                            else
                            {
                                num8 = Math.Max(num16, (byte) (num2 >> 0x18));
                                float num12 = ((float) (num2 >> 0x18)) / ((float) num8);
                                float num13 = ((float) num16) / ((float) num8);
                                byte num9 = (byte) ((num2 >> 0x10) & 0xff);
                                byte num10 = (byte) ((num2 >> 8) & 0xff);
                                byte num11 = (byte) (num2 & 0xff);
                                r = (byte) (((int) ((num9 * num12) + (color.R * num13))) >> 1);
                                g = (byte) (((int) ((num10 * num12) + (color.G * num13))) >> 1);
                                b = (byte) (((int) ((num11 * num12) + (color.B * num13))) >> 1);
                            }
                            num2 = (uint) ((((num8 << 0x18) | (r << 0x10)) | (g << 8)) | b);
                            this.m_backBuffer[num7, num6] = num2;
                        }
                    }
                }
                index++;
            }
        }

        private void SetBrightness(float value)
        {
            if (this.m_originalBackground != null)
            {
                Image image = new Bitmap(this.m_originalBackground);
                float[][] newColorMatrix = new float[5][];
                float[] numArray2 = new float[5];
                numArray2[0] = 1f;
                newColorMatrix[0] = numArray2;
                numArray2 = new float[5];
                numArray2[1] = 1f;
                newColorMatrix[1] = numArray2;
                numArray2 = new float[5];
                numArray2[2] = 1f;
                newColorMatrix[2] = numArray2;
                numArray2 = new float[5];
                numArray2[3] = 1f;
                newColorMatrix[3] = numArray2;
                numArray2 = new float[5];
                numArray2[0] = value;
                numArray2[1] = value;
                numArray2[2] = value;
                numArray2[4] = 1f;
                newColorMatrix[4] = numArray2;
                ColorMatrix matrix = new ColorMatrix(newColorMatrix);
                Graphics graphics = Graphics.FromImage(image);
                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetColorMatrix(matrix);
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
                graphics.Dispose();
                imageAttr.Dispose();
                this.pictureBoxShowGrid.BackgroundImage = image;
            }
        }

        private void SetPictureBoxSize(int width, int height)
        {
            int num = width - this.pictureBoxShowGrid.Width;
            int num2 = height - this.pictureBoxShowGrid.Height;
            base.Width += num;
            base.Height += num2;
            this.pictureBoxShowGrid.Width += num;
            this.pictureBoxShowGrid.Height += num2;
        }

        public void UpdateWith(byte[] channelValues)
        {
            this.m_channelValues = channelValues;
            this.Recalc();
            this.pictureBoxShowGrid.Refresh();
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= 0x200;
                return createParams;
            }
        }
    }
}

