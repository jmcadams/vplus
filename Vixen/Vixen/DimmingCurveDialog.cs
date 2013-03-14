namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    internal class DimmingCurveDialog : Form
    {
        private Button buttonCancel;
        private Button buttonExportToLibrary;
        private Button buttonImportFromLibrary;
        private Button buttonOK;
        private Button buttonResetToLinear;
        private Button buttonSwitchDisplay;
        private ComboBox comboBoxChannels;
        private ComboBox comboBoxExport;
        private ComboBox comboBoxImport;
        private IContainer components;
        private const int EXPORT_TO_FILE = 1;
        private const int EXPORT_TO_LIBRARY = 0;
        private const int IMPORT_FROM_FILE = 1;
        private const int IMPORT_FROM_LIBRARY = 0;
        private Label label1;
        private Label label2;
        private Label labelChannelValue;
        private Label labelMessage;
        private Label labelSequenceChannels;
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
        private Panel panel1;
        private PictureBox pictureBoxCurve;
        private PictureBox pictureBoxMini;

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
            this.m_curveRowPointsPerMiniPixel = this.m_availableValues / ((float) this.pictureBoxMini.Width);
            this.m_curveColPointsPerMiniPixel = this.m_availableValues / ((float) this.pictureBoxMini.Height);
            this.m_miniBoxBounds = new Rectangle(0, 0, (int) ((((float) (this.pictureBoxCurve.Width / this.m_gridSpacing)) / this.m_availableValues) * this.pictureBoxMini.Width), (int) ((((float) (this.pictureBoxCurve.Height / this.m_gridSpacing)) / this.m_availableValues) * this.pictureBoxMini.Height));
            this.m_miniBackBrush = new SolidBrush(this.pictureBoxMini.BackColor);
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
            if (MessageBox.Show("This will reset all values of the selected channel.\n\nContinue?", "Vixen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            this.m_miniBackBrush.Dispose();
            this.m_curveBackBrush.Dispose();
            this.m_miniBoxPen.Dispose();
            this.m_miniLinePen.Dispose();
            this.m_curveGridPen.Dispose();
            this.m_curveLinePen.Dispose();
            this.m_curvePointBrush.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBoxMini = new PictureBox();
            this.pictureBoxCurve = new PictureBox();
            this.labelChannelValue = new Label();
            this.buttonResetToLinear = new Button();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.comboBoxChannels = new ComboBox();
            this.labelSequenceChannels = new Label();
            this.buttonSwitchDisplay = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.buttonImportFromLibrary = new Button();
            this.buttonExportToLibrary = new Button();
            this.labelMessage = new Label();
            this.panel1 = new Panel();
            this.comboBoxImport = new ComboBox();
            this.comboBoxExport = new ComboBox();
            ((ISupportInitialize) this.pictureBoxMini).BeginInit();
            ((ISupportInitialize) this.pictureBoxCurve).BeginInit();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.pictureBoxMini.BackColor = Color.White;
            this.pictureBoxMini.Location = new Point(0, 0);
            this.pictureBoxMini.Name = "pictureBoxMini";
            this.pictureBoxMini.Size = new Size(0x100, 0x100);
            this.pictureBoxMini.TabIndex = 0;
            this.pictureBoxMini.TabStop = false;
            this.pictureBoxMini.MouseMove += new MouseEventHandler(this.pictureBoxMini_MouseMove);
            this.pictureBoxMini.MouseDown += new MouseEventHandler(this.pictureBoxMini_MouseDown);
            this.pictureBoxMini.Paint += new PaintEventHandler(this.pictureBoxMini_Paint);
            this.pictureBoxCurve.BackColor = Color.FloralWhite;
            this.pictureBoxCurve.Location = new Point(0x112, 0x1f);
            this.pictureBoxCurve.Name = "pictureBoxCurve";
            this.pictureBoxCurve.Size = new Size(0x1c1, 0x1c1);
            this.pictureBoxCurve.TabIndex = 1;
            this.pictureBoxCurve.TabStop = false;
            this.pictureBoxCurve.MouseLeave += new EventHandler(this.pictureBoxCurve_MouseLeave);
            this.pictureBoxCurve.MouseMove += new MouseEventHandler(this.pictureBoxCurve_MouseMove);
            this.pictureBoxCurve.Paint += new PaintEventHandler(this.pictureBoxCurve_Paint);
            this.pictureBoxCurve.MouseUp += new MouseEventHandler(this.pictureBoxCurve_MouseUp);
            this.labelChannelValue.AutoSize = true;
            this.labelChannelValue.Location = new Point(0x115, 13);
            this.labelChannelValue.Name = "labelChannelValue";
            this.labelChannelValue.Size = new Size(0, 13);
            this.labelChannelValue.TabIndex = 13;
            this.buttonResetToLinear.Enabled = false;
            this.buttonResetToLinear.Location = new Point(12, 0x125);
            this.buttonResetToLinear.Name = "buttonResetToLinear";
            this.buttonResetToLinear.Size = new Size(0x63, 0x17);
            this.buttonResetToLinear.TabIndex = 6;
            this.buttonResetToLinear.Text = "Reset to linear";
            this.buttonResetToLinear.UseVisualStyleBackColor = true;
            this.buttonResetToLinear.Click += new EventHandler(this.buttonResetToLinear_Click);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x238, 0x1eb);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x289, 0x1eb);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.comboBoxChannels.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxChannels.FormattingEnabled = true;
            this.comboBoxChannels.Location = new Point(15, 0x161);
            this.comboBoxChannels.Name = "comboBoxChannels";
            this.comboBoxChannels.Size = new Size(0xfd, 0x15);
            this.comboBoxChannels.TabIndex = 1;
            this.comboBoxChannels.SelectedIndexChanged += new EventHandler(this.comboBoxChannels_SelectedIndexChanged);
            this.labelSequenceChannels.AutoSize = true;
            this.labelSequenceChannels.Location = new Point(12, 0x151);
            this.labelSequenceChannels.Name = "labelSequenceChannels";
            this.labelSequenceChannels.Size = new Size(0x66, 13);
            this.labelSequenceChannels.TabIndex = 0;
            this.labelSequenceChannels.Text = "Sequence channels";
            this.buttonSwitchDisplay.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonSwitchDisplay.Location = new Point(0x164, 0x1f5);
            this.buttonSwitchDisplay.Name = "buttonSwitchDisplay";
            this.buttonSwitchDisplay.Size = new Size(120, 0x17);
            this.buttonSwitchDisplay.TabIndex = 14;
            this.buttonSwitchDisplay.Text = "Show";
            this.buttonSwitchDisplay.UseVisualStyleBackColor = true;
            this.buttonSwitchDisplay.Visible = false;
            this.buttonSwitchDisplay.Click += new EventHandler(this.buttonSwitchDisplay_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x115, 0x1e3);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Input >";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x102, 0x1df);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0, 13);
            this.label2.TabIndex = 9;
            this.buttonImportFromLibrary.Location = new Point(15, 0x18c);
            this.buttonImportFromLibrary.Name = "buttonImportFromLibrary";
            this.buttonImportFromLibrary.Size = new Size(0x4b, 0x17);
            this.buttonImportFromLibrary.TabIndex = 2;
            this.buttonImportFromLibrary.Text = "Import";
            this.buttonImportFromLibrary.UseVisualStyleBackColor = true;
            this.buttonImportFromLibrary.Click += new EventHandler(this.buttonImportFromLibrary_Click);
            this.buttonExportToLibrary.Location = new Point(15, 0x1a9);
            this.buttonExportToLibrary.Name = "buttonExportToLibrary";
            this.buttonExportToLibrary.Size = new Size(0x4b, 0x17);
            this.buttonExportToLibrary.TabIndex = 4;
            this.buttonExportToLibrary.Text = "Export";
            this.buttonExportToLibrary.UseVisualStyleBackColor = true;
            this.buttonExportToLibrary.Click += new EventHandler(this.buttonExportToLibrary_Click);
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new Point(12, 0x1f0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new Size(0, 13);
            this.labelMessage.TabIndex = 12;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBoxMini);
            this.panel1.Location = new Point(12, 0x1f);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x102, 0x102);
            this.panel1.TabIndex = 11;
            this.comboBoxImport.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxImport.FormattingEnabled = true;
            this.comboBoxImport.Items.AddRange(new object[] { "curve from the library", "curves from a file" });
            this.comboBoxImport.Location = new Point(0x60, 0x18e);
            this.comboBoxImport.Name = "comboBoxImport";
            this.comboBoxImport.Size = new Size(0x85, 0x15);
            this.comboBoxImport.TabIndex = 3;
            this.comboBoxExport.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxExport.FormattingEnabled = true;
            this.comboBoxExport.Items.AddRange(new object[] { "curve to the library", "curves to a file" });
            this.comboBoxExport.Location = new Point(0x60, 0x1ab);
            this.comboBoxExport.Name = "comboBoxExport";
            this.comboBoxExport.Size = new Size(0x85, 0x15);
            this.comboBoxExport.TabIndex = 5;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x2e0, 0x20e);
            base.Controls.Add(this.comboBoxExport);
            base.Controls.Add(this.comboBoxImport);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.labelMessage);
            base.Controls.Add(this.buttonExportToLibrary);
            base.Controls.Add(this.buttonImportFromLibrary);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonSwitchDisplay);
            base.Controls.Add(this.labelSequenceChannels);
            base.Controls.Add(this.comboBoxChannels);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.buttonResetToLinear);
            base.Controls.Add(this.labelChannelValue);
            base.Controls.Add(this.pictureBoxCurve);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DimmingCurveDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Dimming Curve";
            base.Paint += new PaintEventHandler(this.DimmingCurveDialog_Paint);
            ((ISupportInitialize) this.pictureBoxMini).EndInit();
            ((ISupportInitialize) this.pictureBoxCurve).EndInit();
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
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
                        this.pictureBoxMini.Invalidate(new Rectangle(((int) (((float) this.m_selectedPointAbsolute) / this.m_curveRowPointsPerMiniPixel)) - 1, 0, 3, this.pictureBoxMini.Height));
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
                    this.m_miniBoxBounds.X = Math.Max(0, Math.Min((int) ((this.pictureBoxMini.Width - this.m_miniBoxBounds.Width) - 1), (int) (e.X - (this.m_miniBoxBounds.Width / 2))));
                    this.m_miniBoxBounds.Y = Math.Max(0, Math.Min((int) ((this.pictureBoxMini.Height - this.m_miniBoxBounds.Height) - 1), (int) (e.Y - (this.m_miniBoxBounds.Height / 2))));
                    this.RedrawMiniBox(miniBoxBounds);
                    this.RedrawCurve();
                }
                this.m_miniMouseMinLocation.X = e.X - this.m_miniBoxBounds.X;
                this.m_miniMouseMinLocation.Y = e.Y - this.m_miniBoxBounds.Y;
                this.m_miniMouseMaxLocation.X = (this.pictureBoxMini.Width - (this.m_miniBoxBounds.Right - e.X)) - 1;
                this.m_miniMouseMaxLocation.Y = (this.pictureBoxMini.Height - (this.m_miniBoxBounds.Bottom - e.Y)) - 1;
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
                    e.Graphics.DrawLine(this.m_miniLinePen, (float) (((float) i) / this.m_curveRowPointsPerMiniPixel), (float) (this.pictureBoxMini.Height - (((float) this.m_points[i]) / this.m_curveColPointsPerMiniPixel)), (float) (((float) (i + 1)) / this.m_curveRowPointsPerMiniPixel), (float) (this.pictureBoxMini.Height - (((float) this.m_points[i + 1]) / this.m_curveColPointsPerMiniPixel)));
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
            this.pictureBoxMini.Refresh();
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
            this.pictureBoxMini.Invalidate(rc);
        }

        private void ResetCurrentToLinear()
        {
            this.ResetToLinear(this.m_points);
            this.CurveCalc();
            this.pictureBoxMini.Refresh();
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
                this.pictureBoxMini.Size = new Size(0x100, 0x100);
            }
            else
            {
                this.m_availableValues = 101f;
                this.pictureBoxMini.Size = new Size(100, 100);
            }
            this.pictureBoxMini.Refresh();
            this.RedrawCurve();
            this.buttonSwitchDisplay.Text = useActualLevels ? "Show % levels" : "Show actual levels";
        }
    }
}

