namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    internal partial class DimmingCurveDialog : Form
    {
        private const int EXPORT_TO_FILE = 1;
        private const int EXPORT_TO_LIBRARY = 0;
        private const int IMPORT_FROM_FILE = 1;
        private const int IMPORT_FROM_LIBRARY = 0;
        private float m_availableValues;
        private SolidBrush m_curveBackBrush;
        private float m_curveColPointsPerMiniPixel;
        private Color m_curveGridColor;
        private Pen m_curveGridPen;
        private Color m_curveLineColor;
        private Pen m_curveLinePen;
        private SolidBrush m_curvePointBrush;
        private Color m_curvePointColor;
        private int[,] m_curvePoints;
        private float m_curveRowPointsPerMiniPixel;
        private int m_dotPitch;
        private int m_gridSpacing;
        private float m_halfPointSize;
        private SolidBrush m_miniBackBrush;
        private Rectangle m_miniBoxBounds;
        private Color m_miniBoxColor;
        private Pen m_miniBoxPen;
        private Color m_miniLineColor;
        private Pen m_miniLinePen;
        private Point m_miniMouseDownLast;
        private Point m_miniMouseMaxLocation;
        private Point m_miniMouseMinLocation;
        private Channel m_originalChannel;
        private byte[] m_points;
        private int m_pointSize;
        private int m_selectedPointAbsolute;
        private int m_selectedPointRelative;
        private EventSequence m_sequence;
        private int m_startCurvePoint;
        private bool m_usingActualLevels;

        public DimmingCurveDialog(EventSequence sequence, Channel selectChannel)
        {
            Action<Channel> action = null;
            this.m_miniBoxColor = Color.BlueViolet;
            this.m_miniLineColor = Color.Blue;
            this.m_curveGridColor = Color.LightGray;
            this.m_curveLineColor = Color.Blue;
            this.m_curvePointColor = Color.Black;
            this.m_pointSize = 4;
            this.m_dotPitch = 4;
            this.m_miniMouseDownLast = new Point(-1, -1);
            this.m_miniMouseMinLocation = new Point(0, 0);
            this.m_miniMouseMaxLocation = new Point(0, 0);
            this.m_selectedPointAbsolute = -1;
            this.m_selectedPointRelative = -1;
            this.m_usingActualLevels = true;
            this.m_availableValues = 256f;
            this.components = null;
            this.InitializeComponent();
            if (sequence != null)
            {
                if (action == null)
                {
                    action = delegate (Channel c) {
                        this.comboBoxChannels.Items.Add(c.Clone());
                    };
                }
                sequence.Channels.ForEach(action);
                this.m_sequence = sequence;
            }
            else
            {
                this.labelSequenceChannels.Enabled = false;
                this.comboBoxChannels.Enabled = false;
                if (selectChannel != null)
                {
                    this.m_originalChannel = selectChannel;
                    this.comboBoxChannels.Items.Add(selectChannel = selectChannel.Clone());
                }
            }
            this.m_gridSpacing = this.m_pointSize + this.m_dotPitch;
            this.m_halfPointSize = ((float) this.m_pointSize) / 2f;
            this.m_curveRowPointsPerMiniPixel = this.m_availableValues / ((float) this.pbMini.Width);
            this.m_curveColPointsPerMiniPixel = this.m_availableValues / ((float) this.pbMini.Height);
            this.m_miniBoxBounds = new Rectangle(0, 0, (int) ((((float) (this.pictureBoxCurve.Width / this.m_gridSpacing)) / this.m_availableValues) * this.pbMini.Width), (int) ((((float) (this.pictureBoxCurve.Height / this.m_gridSpacing)) / this.m_availableValues) * this.pbMini.Height));
            this.m_miniBackBrush = new SolidBrush(this.pbMini.BackColor);
            this.m_curveBackBrush = new SolidBrush(this.pictureBoxCurve.BackColor);
            this.m_miniBoxPen = new Pen(this.m_miniBoxColor);
            this.m_miniLinePen = new Pen(this.m_miniLineColor);
            this.m_curveGridPen = new Pen(this.m_curveGridColor);
            this.m_curveLinePen = new Pen(this.m_curveLineColor);
            this.m_curvePointBrush = new SolidBrush(this.m_curvePointColor);
            if (this.comboBoxChannels.Items.Count > 0)
            {
                this.comboBoxChannels.SelectedItem = (selectChannel != null) ? selectChannel : this.comboBoxChannels.Items[0];
            }
            this.SwitchDisplay(Preference2.GetInstance().GetBoolean("ActualLevels"));
            this.comboBoxImport.SelectedIndex = 0;
            this.comboBoxExport.SelectedIndex = 0;
        }

        private void buttonExportToLibrary_Click(object sender, EventArgs e)
        {
            if (this.comboBoxExport.SelectedIndex == 0)
            {
                using (CurveLibrary library = new CurveLibrary())
                {
                    CurveLibraryRecordEditDialog dialog = new CurveLibraryRecordEditDialog(null);
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        CurveLibraryRecord libraryRecord = dialog.LibraryRecord;
                        libraryRecord.CurveData = this.m_points;
                        library.Import(libraryRecord);
                        library.Save();
                    }
                    dialog.Dispose();
                }
            }
            else
            {
                CurveFileImportExportDialog dialog2 = new CurveFileImportExportDialog(CurveFileImportExportDialog.ImportExport.Export);
                dialog2.ShowDialog();
                dialog2.Dispose();
            }
        }

        private void buttonImportFromLibrary_Click(object sender, EventArgs e)
        {
            if (this.comboBoxImport.SelectedIndex == 0)
            {
                CurveLibraryDialog dialog = new CurveLibraryDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    (this.comboBoxChannels.SelectedItem as Channel).DimmingCurve = this.m_points = dialog.SelectedCurve;
                    this.RedrawBoth();
                }
                dialog.Dispose();
            }
            else
            {
                CurveFileImportExportDialog dialog2 = new CurveFileImportExportDialog(CurveFileImportExportDialog.ImportExport.Import);
                if (dialog2.ShowDialog() == DialogResult.OK)
                {
                    CurveLibraryRecord selectedCurve = dialog2.SelectedCurve;
                    if (selectedCurve != null)
                    {
                        (this.comboBoxChannels.SelectedItem as Channel).DimmingCurve = this.m_points = selectedCurve.CurveData;
                        this.RedrawBoth();
                    }
                }
                dialog2.Dispose();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.m_sequence != null)
            {
                for (int i = 0; i < this.comboBoxChannels.Items.Count; i++)
                {
                    this.m_sequence.Channels[i].DimmingCurve = (this.comboBoxChannels.Items[i] as Channel).DimmingCurve;
                }
            }
            else
            {
                Channel channel = (Channel) this.comboBoxChannels.Items[0];
                this.m_originalChannel.DimmingCurve = channel.DimmingCurve;
            }
        }

        private void buttonResetToLinear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will reset all values of the selected channel.\n\nContinue?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.ResetCurrentToLinear();
            }
        }

        private void buttonSwitchDisplay_Click(object sender, EventArgs e)
        {
            this.SwitchDisplay(!this.m_usingActualLevels);
        }

        private void comboBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            Channel selectedItem = (Channel) this.comboBoxChannels.SelectedItem;
            if (selectedItem != null)
            {
                if (selectedItem.DimmingCurve == null)
                {
                    selectedItem.DimmingCurve = new byte[0x100];
                    this.ResetToLinear(selectedItem.DimmingCurve);
                }
                this.ShowChannel(selectedItem);
                this.buttonResetToLinear.Enabled = true;
            }
            else
            {
                this.buttonResetToLinear.Enabled = false;
            }
        }

        private void CurveCalc()
        {
            int num = (int) (this.m_miniBoxBounds.Left * this.m_curveRowPointsPerMiniPixel);
            num = Math.Max(0, num - 1);
            int num2 = (int) (this.m_miniBoxBounds.Right * this.m_curveRowPointsPerMiniPixel);
            num2 = (int) Math.Min(this.m_availableValues - 1f, (float) (num2 + 2));
            int index = Math.Min(num, num2);
            int num4 = Math.Max(num, num2);
            int num5 = this.pictureBoxCurve.Height - ((this.pictureBoxCurve.Height / this.m_gridSpacing) * this.m_gridSpacing);
            int num6 = (int) (this.m_availableValues - ((this.m_miniBoxBounds.Bottom + 1) * this.m_curveColPointsPerMiniPixel));
            int num7 = (int) (this.m_miniBoxBounds.Left * this.m_curveRowPointsPerMiniPixel);
            if ((this.m_curvePoints == null) || (this.m_curvePoints.GetLength(0) != ((num4 - index) + 1)))
            {
                this.m_curvePoints = new int[(num4 - index) + 1, 2];
            }
            this.m_startCurvePoint = index;
            int num10 = -1;
            int num11 = -1;
            int num12 = 0;
            while (index < num4)
            {
                int num8 = (index - num7) * this.m_gridSpacing;
                int num9 = (this.pictureBoxCurve.Height - num5) - ((this.m_points[index] - num6) * this.m_gridSpacing);
                num10 = ((index + 1) - num7) * this.m_gridSpacing;
                num11 = (this.pictureBoxCurve.Height - num5) - ((this.m_points[index + 1] - num6) * this.m_gridSpacing);
                this.m_curvePoints[num12, 0] = num8;
                this.m_curvePoints[num12, 1] = num9;
                index++;
                num12++;
            }
            if (num10 != -1)
            {
                this.m_curvePoints[num12, 0] = num10;
                this.m_curvePoints[num12, 1] = num11;
            }
        }

        private void DimmingCurveDialog_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform((float) -this.label2.Location.X, (float) -this.label2.Location.Y, MatrixOrder.Append);
            e.Graphics.RotateTransform(-90f, MatrixOrder.Append);
            e.Graphics.TranslateTransform((float) this.label2.Location.X, (float) this.label2.Location.Y, MatrixOrder.Append);
            e.Graphics.DrawString("Output >", this.Font, Brushes.Black, (PointF) this.label2.Location);
            e.Graphics.ResetTransform();
        }

        

        

        private void library_Message(string message)
        {
            this.labelMessage.Text = message;
            this.labelMessage.Update();
        }

        private void library_Progess(int progressCount, int totalCount)
        {
            this.library_Message(string.Format("{0} of {1} completed", progressCount, totalCount));
        }

        private int MaxOf(params int[] values)
        {
            int num = values[0];
            foreach (int num2 in values)
            {
                if (num2 > num)
                {
                    num = num2;
                }
            }
            return num;
        }

        private int MinOf(params int[] values)
        {
            int num = values[0];
            foreach (int num2 in values)
            {
                if (num2 < num)
                {
                    num = num2;
                }
            }
            return num;
        }

        private void pictureBoxCurve_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void pictureBoxCurve_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.m_points != null)
            {
                if (((e.Button & MouseButtons.Left) != MouseButtons.None) && (this.m_selectedPointRelative != -1))
                {
                    int num = Math.Max(0, Math.Min(this.pictureBoxCurve.Height - 1, e.Y));
                    if (((this.m_curvePoints[this.m_selectedPointRelative, 1] - num) % this.m_gridSpacing) == 0)
                    {
                        int selectedPointRelative;
                        int num3;
                        Rectangle rc = new Rectangle();
                        if (this.m_selectedPointAbsolute == 0)
                        {
                            rc.X = this.m_curvePoints[this.m_selectedPointRelative, 0] - ((int) this.m_halfPointSize);
                            selectedPointRelative = this.m_selectedPointRelative;
                        }
                        else
                        {
                            rc.X = this.m_curvePoints[this.m_selectedPointRelative - 1, 0] - ((int) this.m_halfPointSize);
                            selectedPointRelative = this.m_selectedPointRelative - 1;
                        }
                        if (this.m_selectedPointAbsolute == (this.m_availableValues - 1f))
                        {
                            rc.Width = (this.m_curvePoints[this.m_selectedPointRelative, 0] + ((int) this.m_halfPointSize)) - rc.X;
                            num3 = this.m_selectedPointRelative;
                        }
                        else
                        {
                            rc.Width = (this.m_curvePoints[this.m_selectedPointRelative + 1, 0] + ((int) this.m_halfPointSize)) - rc.X;
                            num3 = this.m_selectedPointRelative + 1;
                        }
                        rc.Y = this.MinOf(new int[] { num, this.m_curvePoints[this.m_selectedPointRelative, 1], this.m_curvePoints[selectedPointRelative, 1], this.m_curvePoints[num3, 1] }) - this.m_pointSize;
                        rc.Height = ((this.MaxOf(new int[] { num, this.m_curvePoints[this.m_selectedPointRelative, 1], this.m_curvePoints[selectedPointRelative, 1], this.m_curvePoints[num3, 1] }) + this.m_pointSize) - rc.Y) + this.m_pointSize;
                        this.m_points[this.m_selectedPointAbsolute] = (byte) (this.m_points[this.m_selectedPointAbsolute] + ((byte) ((this.m_curvePoints[this.m_selectedPointRelative, 1] - num) / this.m_gridSpacing)));
                        this.m_curvePoints[this.m_selectedPointRelative, 1] = num;
                        this.pictureBoxCurve.Invalidate(rc);
                        this.pbMini.Invalidate(new Rectangle(((int) (((float) this.m_selectedPointAbsolute) / this.m_curveRowPointsPerMiniPixel)) - 1, 0, 3, this.pbMini.Height));
                    }
                }
                else
                {
                    int num10;
                    int num11;
                    int length = this.m_curvePoints.GetLength(0);
                    int num5 = length >> 1;
                    int x = e.X;
                    int y = e.Y;
                    if (x < (this.pictureBoxCurve.Width >> 1))
                    {
                        num10 = 0;
                        num11 = num5;
                    }
                    else
                    {
                        num10 = num5;
                        num11 = length;
                    }
                    int num12 = num10;
                    while (num12 < num11)
                    {
                        int num8 = this.m_curvePoints[num12, 0];
                        int num9 = this.m_curvePoints[num12, 1];
                        if ((((x >= (num8 - this.m_halfPointSize)) && (x <= (num8 + this.m_halfPointSize))) && (y >= (num9 - this.m_halfPointSize))) && (y <= (num9 + this.m_halfPointSize)))
                        {
                            break;
                        }
                        num12++;
                    }
                    if (num12 < num11)
                    {
                        this.Cursor = Cursors.SizeNS;
                        this.m_selectedPointRelative = num12;
                        this.m_selectedPointAbsolute = this.m_startCurvePoint + num12;
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        this.m_selectedPointRelative = -1;
                        this.m_selectedPointAbsolute = -1;
                    }
                }
                if (this.m_selectedPointAbsolute == -1)
                {
                    this.labelChannelValue.Text = string.Empty;
                }
                else
                {
                    this.labelChannelValue.Text = string.Format("Channel value {0} ({2:P0}) will output at {1} ({3:P0})", new object[] { this.m_selectedPointAbsolute, this.m_points[this.m_selectedPointAbsolute], ((float) this.m_selectedPointAbsolute) / 255f, ((float) this.m_points[this.m_selectedPointAbsolute]) / 255f });
                }
            }
        }

        private void pictureBoxCurve_MouseUp(object sender, MouseEventArgs e)
        {
            this.m_selectedPointAbsolute = -1;
            this.m_selectedPointRelative = -1;
            this.Cursor = Cursors.Default;
        }

        private void pictureBoxCurve_Paint(object sender, PaintEventArgs e)
        {
            if (this.m_points != null)
            {
                e.Graphics.FillRectangle(this.m_curveBackBrush, e.ClipRectangle);
                int num3 = (int) Math.Min((float) ((this.m_miniBoxBounds.Left * this.m_curveRowPointsPerMiniPixel) + ((this.pictureBoxCurve.Width / this.m_gridSpacing) + 1)), (float) (this.m_availableValues - 1f));
                int num4 = (int) Math.Min((float) ((this.m_miniBoxBounds.Top * this.m_curveColPointsPerMiniPixel) + ((this.pictureBoxCurve.Height / this.m_gridSpacing) + 1)), (float) (this.m_availableValues - 1f));
                num3 -= (int) (this.m_miniBoxBounds.Left * this.m_curveRowPointsPerMiniPixel);
                num4 -= (int) (this.m_miniBoxBounds.Top * this.m_curveColPointsPerMiniPixel);
                num3 *= this.m_gridSpacing;
                num4 *= this.m_gridSpacing;
                for (int i = (e.ClipRectangle.Top / this.m_gridSpacing) * this.m_gridSpacing; i <= num3; i += this.m_gridSpacing)
                {
                    e.Graphics.DrawLine(this.m_curveGridPen, 0, i, num3, i);
                }
                for (int j = (e.ClipRectangle.Left / this.m_gridSpacing) * this.m_gridSpacing; j <= num4; j += this.m_gridSpacing)
                {
                    e.Graphics.DrawLine(this.m_curveGridPen, j, 0, j, num4);
                }
                int num5 = Math.Max((e.ClipRectangle.Left / this.m_gridSpacing) - 1, 0);
                int num6 = Math.Min((e.ClipRectangle.Right / this.m_gridSpacing) + 3, this.m_curvePoints.GetLength(0));
                for (int k = num5; k < num6; k++)
                {
                    float num7 = this.m_curvePoints[k, 0];
                    float num8 = this.m_curvePoints[k, 1];
                    if (k < (num6 - 1))
                    {
                        float num9 = this.m_curvePoints[k + 1, 0];
                        float num10 = this.m_curvePoints[k + 1, 1];
                        e.Graphics.DrawLine(this.m_curveLinePen, num7, num8, num9, num10);
                    }
                    e.Graphics.FillRectangle(this.m_curvePointBrush, num7 - this.m_halfPointSize, num8 - this.m_halfPointSize, (float) this.m_pointSize, (float) this.m_pointSize);
                }
            }
        }

        private void pictureBoxMini_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.m_points != null)
            {
                if (!this.m_miniBoxBounds.Contains(e.Location))
                {
                    Rectangle miniBoxBounds = this.m_miniBoxBounds;
                    this.m_miniBoxBounds.X = Math.Max(0, Math.Min((int) ((this.pbMini.Width - this.m_miniBoxBounds.Width) - 1), (int) (e.X - (this.m_miniBoxBounds.Width / 2))));
                    this.m_miniBoxBounds.Y = Math.Max(0, Math.Min((int) ((this.pbMini.Height - this.m_miniBoxBounds.Height) - 1), (int) (e.Y - (this.m_miniBoxBounds.Height / 2))));
                    this.RedrawMiniBox(miniBoxBounds);
                    this.RedrawCurve();
                }
                this.m_miniMouseMinLocation.X = e.X - this.m_miniBoxBounds.X;
                this.m_miniMouseMinLocation.Y = e.Y - this.m_miniBoxBounds.Y;
                this.m_miniMouseMaxLocation.X = (this.pbMini.Width - (this.m_miniBoxBounds.Right - e.X)) - 1;
                this.m_miniMouseMaxLocation.Y = (this.pbMini.Height - (this.m_miniBoxBounds.Bottom - e.Y)) - 1;
                this.m_miniMouseDownLast = e.Location;
            }
        }

        private void pictureBoxMini_MouseMove(object sender, MouseEventArgs e)
        {
            if ((this.m_points != null) && ((e.Button & MouseButtons.Left) != MouseButtons.None))
            {
                int num = Math.Max(Math.Min(e.X, this.m_miniMouseMaxLocation.X), this.m_miniMouseMinLocation.X);
                int num2 = Math.Max(Math.Min(e.Y, this.m_miniMouseMaxLocation.Y), this.m_miniMouseMinLocation.Y);
                if ((num != this.m_miniMouseDownLast.X) || (num2 != this.m_miniMouseDownLast.Y))
                {
                    Rectangle miniBoxBounds = this.m_miniBoxBounds;
                    this.m_miniBoxBounds.X += num - this.m_miniMouseDownLast.X;
                    this.m_miniMouseDownLast.X = num;
                    this.m_miniBoxBounds.Y += num2 - this.m_miniMouseDownLast.Y;
                    this.m_miniMouseDownLast.Y = num2;
                    this.RedrawMiniBox(miniBoxBounds);
                    this.RedrawCurve();
                }
            }
        }

        private void pictureBoxMini_Paint(object sender, PaintEventArgs e)
        {
            if (this.m_points != null)
            {
                e.Graphics.FillRectangle(this.m_miniBackBrush, e.ClipRectangle);
                int num = (int) (e.ClipRectangle.Left * this.m_curveRowPointsPerMiniPixel);
                num = Math.Max(0, num - 1);
                int num2 = (int) (e.ClipRectangle.Right * this.m_curveRowPointsPerMiniPixel);
                num2 = (int) Math.Min(this.m_availableValues - 1f, (float) (num2 + 1));
                int num3 = Math.Min(num, num2);
                int num4 = Math.Max(num, num2);
                for (int i = num3; i < num4; i++)
                {
                    e.Graphics.DrawLine(this.m_miniLinePen, (float) (((float) i) / this.m_curveRowPointsPerMiniPixel), (float) (this.pbMini.Height - (((float) this.m_points[i]) / this.m_curveColPointsPerMiniPixel)), (float) (((float) (i + 1)) / this.m_curveRowPointsPerMiniPixel), (float) (this.pbMini.Height - (((float) this.m_points[i + 1]) / this.m_curveColPointsPerMiniPixel)));
                }
                if (e.ClipRectangle.IntersectsWith(this.m_miniBoxBounds))
                {
                    e.Graphics.DrawRectangle(this.m_miniBoxPen, this.m_miniBoxBounds);
                }
            }
        }

        private void RedrawBoth()
        {
            this.CurveCalc();
            this.pbMini.Refresh();
            this.pictureBoxCurve.Refresh();
        }

        private void RedrawCurve()
        {
            this.CurveCalc();
            this.pictureBoxCurve.Refresh();
        }

        private void RedrawMiniBox(Rectangle oldBounds)
        {
            Rectangle rc = Rectangle.Union(this.m_miniBoxBounds, oldBounds);
            rc.Inflate(1, 1);
            this.pbMini.Invalidate(rc);
        }

        private void ResetCurrentToLinear()
        {
            this.ResetToLinear(this.m_points);
            this.CurveCalc();
            this.pbMini.Refresh();
            this.pictureBoxCurve.Refresh();
        }

        private void ResetToLinear(byte[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = (byte) i;
            }
        }

        private void ShowChannel(Channel channel)
        {
            this.m_points = channel.DimmingCurve;
            this.RedrawBoth();
        }

        private void SwitchDisplay(bool useActualLevels)
        {
            useActualLevels = true;
            this.m_usingActualLevels = useActualLevels;
            if (this.m_usingActualLevels)
            {
                this.m_availableValues = 256f;
                this.pbMini.Size = new Size(0x100, 0x100);
            }
            else
            {
                this.m_availableValues = 101f;
                this.pbMini.Size = new Size(100, 100);
            }
            this.pbMini.Refresh();
            this.RedrawCurve();
            this.buttonSwitchDisplay.Text = useActualLevels ? "Show % levels" : "Show actual levels";
        }
    }
}

