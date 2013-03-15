namespace LedTriks
{
    using LedTriksUtil;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;
    using Vixen.Dialogs;

    internal class FrameOrganizationDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components = null;
        private HScrollBar hScrollBar;
        private List<Frame> m_clipboard1;
        private Rectangle m_clipboard1Bounds;
        private Point m_clipboard1TimeLocation;
        private List<Frame> m_clipboard2;
        private Rectangle m_clipboard2Bounds;
        private Point m_clipboard2TimeLocation;
        private List<Frame> m_clipboard3;
        private Rectangle m_clipboard3Bounds;
        private Point m_clipboard3TimeLocation;
        private List<Frame> m_clipboard4;
        private Rectangle m_clipboard4Bounds;
        private Point m_clipboard4TimeLocation;
        private const int m_clipboardFormGutter = 100;
        private const int m_clipboardGutter = 30;
        private Font m_clipboardNumberFont = null;
        private const int m_clipboardNumberPanelWidth = 15;
        private Brush m_clipboardPanelBrush = null;
        private Brush m_clipboardPanelFontBrush = null;
        private Pen m_clipboardPanelPen = null;
        private const int m_clipboardPanelTextXOffset = 3;
        private IDataObject m_dragObject = null;
        private Rectangle m_folderBounds;
        private int m_frameHeight;
        private int m_frameListFrameWidth;
        private List<Frame> m_frames;
        private float m_framesVisible;
        private int m_frameWidth;
        private const string m_helpText = "Select a single frame by clicking on it in the timeline.\n\nSelect multiple frames by selecting a frame then Shift-clicking another.\n\nRemove frames from the timeline by dragging a selection to one of the four clipboards,\nor to the trash.\n\nMove frames around in the timeline by dragging the selection to a new point, or by\ndragging a clipboard to the timeline.\n\nMerge frames together by dragging a selection or clipboard to the timeline while holding\nthe Control key.\n\nFrames can be copied by selecting a range and pressing Ctrl-C.\n\nRoutines can be created by dragging a series of frames to the folder and loaded by\ndouble-clicking on the folder.";
        private int m_hoveredFrameIndex = -1;
        private int m_insertionIndex = -1;
        private const int m_largeFrame_Timeline_Spacing = 20;
        private Rectangle m_largeFrameBounds;
        private const int m_largeFrameCellSize = 4;
        private const int m_leftGutter = 50;
        private bool m_merging = false;
        private bool m_resizing = false;
        private int m_selectedFrameCount = 0;
        private int m_selectedFrameIndex = -1;
        private bool m_shiftDown = false;
        private List<Frame> m_timeline;
        private const int m_timeline_Clipboard_Spacing = 50;
        private Rectangle m_timelineBounds;
        private const int m_timelineFrameHorizontalSpace = 5;
        private const int m_timelineFrameVerticalSpace = 5;
        private TimeSpan m_timelineSelectionEnd;
        private TimeSpan m_timelineSelectionStart;
        private Point m_timelineSelectionTimeLocation;
        private const int m_topGutter = 50;
        private List<Frame> m_trash;
        private Rectangle m_trashBounds;
        private OpenFileDialog openFileDialog;
        private PictureBox pictureBoxEmptyBin;
        private PictureBox pictureBoxFolder;
        private PictureBox pictureBoxFullBin;
        private SaveFileDialog saveFileDialog;

        public FrameOrganizationDialog(List<Frame> frames, int frameWidth, int frameHeight)
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            this.m_frames = frames;
            this.m_timeline = new List<Frame>(this.m_frames);
            this.m_frameWidth = frameWidth;
            this.m_frameHeight = frameHeight;
            this.m_largeFrameBounds = new Rectangle(0, 50, frameWidth * 4, frameHeight * 4);
            this.m_timelineBounds = new Rectangle(50, 0, 0, 0);
            this.m_clipboard1Bounds = new Rectangle(100, 0, 0, 0);
            this.m_clipboard2Bounds = new Rectangle(100, 0, 0, 0);
            this.m_clipboard3Bounds = new Rectangle(100, 0, 0, 0);
            this.m_clipboard4Bounds = new Rectangle(100, 0, 0, 0);
            this.m_clipboardNumberFont = new Font("Arial", 10f, FontStyle.Bold);
            this.m_timelineSelectionTimeLocation = new Point(50, 0);
            this.m_clipboard1TimeLocation = new Point(100, 0);
            this.m_clipboard2TimeLocation = new Point(100, 0);
            this.m_clipboard3TimeLocation = new Point(100, 0);
            this.m_clipboard4TimeLocation = new Point(100, 0);
            this.hScrollBar.Maximum = this.m_timeline.Count - 1;
            this.m_clipboard1 = new List<Frame>();
            this.m_clipboard2 = new List<Frame>();
            this.m_clipboard3 = new List<Frame>();
            this.m_clipboard4 = new List<Frame>();
            this.m_clipboardPanelBrush = Brushes.LightSteelBlue;
            this.m_clipboardPanelPen = Pens.SteelBlue;
            this.m_clipboardPanelFontBrush = Brushes.SteelBlue;
            this.m_frameListFrameWidth = (5 + this.m_frameWidth) + 5;
            this.m_trash = new List<Frame>();
            this.m_trashBounds = new Rectangle(10, 0, this.pictureBoxEmptyBin.Width, this.pictureBoxEmptyBin.Height);
            this.m_folderBounds = new Rectangle(this.m_trashBounds.Right + 10, 0, this.pictureBoxFolder.Width, this.pictureBoxFolder.Height);
        }

        private void CalcSelectionSpan()
        {
            this.m_timelineSelectionStart = TimeSpan.FromMilliseconds((double) this.GetFrameStartTime(this.m_selectedFrameIndex, this.m_timeline));
            this.m_timelineSelectionEnd = TimeSpan.FromMilliseconds((double) this.GetFrameEndTime((this.m_selectedFrameIndex + this.m_selectedFrameCount) - 1, this.m_timeline));
            this.Refresh();
        }

        private void CopySelection()
        {
            if (this.m_selectedFrameCount > 0)
            {
                List<Frame> firstEmptyClipboard = this.FirstEmptyClipboard;
                if (firstEmptyClipboard == null)
                {
                    MessageBox.Show("To load a routine, there needs to be an empty clipboard.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    for (int i = 0; i < this.m_selectedFrameCount; i++)
                    {
                        firstEmptyClipboard.Add(this.m_timeline[this.m_selectedFrameIndex + i]);
                    }
                    this.Recalc();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.m_clipboardNumberFont != null)
            {
                this.m_clipboardNumberFont.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawFrameIn(Frame frame, Rectangle bounds, int cellSize, Graphics g)
        {
            foreach (uint num3 in frame.Cells)
            {
                int x = bounds.X + ((int) ((num3 >> 0x10) * cellSize));
                int y = bounds.Y + ((int) ((num3 & 0xffff) * cellSize));
                if (((x + cellSize) > bounds.Right) || ((y + cellSize) > bounds.Bottom))
                {
                    g.FillRectangle(Brushes.Red, Rectangle.Intersect(bounds, new Rectangle(x, y, cellSize, cellSize)));
                }
                else
                {
                    g.FillRectangle(Brushes.Red, x, y, cellSize, cellSize);
                }
            }
        }

        private void DrawFrameListIn(List<Frame> frames, Rectangle bounds, int startIndex, Graphics g)
        {
            if (frames.Count != 0)
            {
                Rectangle a = new Rectangle(0, (bounds.Top + 5) + 1, this.m_frameWidth, this.m_frameHeight);
                int num = bounds.Left + 1;
                for (int i = startIndex; (num < bounds.Right) && (i < frames.Count); i++)
                {
                    a.X = num + 5;
                    Rectangle rect = Rectangle.Intersect(a, bounds);
                    g.FillRectangle(Brushes.Black, rect);
                    this.DrawFrameIn(frames[i], Rectangle.Intersect(a, bounds), 1, g);
                    num += this.m_frameListFrameWidth;
                }
            }
        }

        private void FrameOrganizationDialog_DoubleClick(object sender, EventArgs e)
        {
            if (this.m_folderBounds.Contains(base.PointToClient(Control.MousePosition)))
            {
                List<Frame> firstEmptyClipboard = this.FirstEmptyClipboard;
                if (firstEmptyClipboard == null)
                {
                    MessageBox.Show("To load a routine, there needs to be an empty clipboard.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    this.openFileDialog.InitialDirectory = Paths.RoutinePath;
                    this.openFileDialog.Filter = "LedTriks routine|*.ltr";
                    this.openFileDialog.DefaultExt = ".ltr";
                    this.openFileDialog.FileName = string.Empty;
                    if (this.openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        XmlDocument document = new XmlDocument();
                        document.Load(this.openFileDialog.FileName);
                        foreach (XmlNode node in document.SelectNodes("//Frames/Frame"))
                        {
                            firstEmptyClipboard.Add(new Frame(node));
                        }
                        this.Recalc();
                    }
                }
            }
        }

        private void FrameOrganizationDialog_DragDrop(object sender, DragEventArgs e)
        {
            this.m_dragObject = null;
            Point pt = base.PointToClient(new Point(e.X, e.Y));
            FrameSelection data = (FrameSelection) e.Data.GetData(typeof(FrameSelection));
            List<Frame> destCollection = null;
            int destIndex = 0;
            if (this.m_timelineBounds.Contains(pt))
            {
                destCollection = this.m_timeline;
                destIndex = this.m_insertionIndex;
                this.m_insertionIndex = -1;
            }
            else if (this.m_clipboard1Bounds.Contains(pt))
            {
                destCollection = this.m_clipboard1;
            }
            else if (this.m_clipboard2Bounds.Contains(pt))
            {
                destCollection = this.m_clipboard2;
            }
            else if (this.m_clipboard3Bounds.Contains(pt))
            {
                destCollection = this.m_clipboard3;
            }
            else if (this.m_clipboard4Bounds.Contains(pt))
            {
                destCollection = this.m_clipboard4;
            }
            else if (this.m_trashBounds.Contains(pt))
            {
                destCollection = this.m_trash;
            }
            else if (this.m_folderBounds.Contains(pt))
            {
                this.SaveToRoutine(data);
                return;
            }
            if ((data.Source != destCollection) || (destCollection == this.m_timeline))
            {
                List<Frame> sourceCollection = new List<Frame>();
                for (int i = data.StartIndex; i < (data.StartIndex + data.Count); i++)
                {
                    sourceCollection.Add(data.Source[i]);
                }
                if (data.StartIndex >= destIndex)
                {
                    data.Source.RemoveRange(data.StartIndex, data.Count);
                    if (this.m_merging)
                    {
                        this.MergeFrames(sourceCollection, 0, sourceCollection.Count, destCollection, destIndex);
                    }
                    else
                    {
                        destCollection.InsertRange(destIndex, sourceCollection);
                    }
                }
                else
                {
                    if (this.m_merging)
                    {
                        this.MergeFrames(data.Source, 0, data.Count, destCollection, destIndex);
                    }
                    else
                    {
                        destCollection.InsertRange(destIndex, sourceCollection);
                    }
                    data.Source.RemoveRange(data.StartIndex, data.Count);
                }
                if (data.Source == destCollection)
                {
                    if (data.StartIndex >= destIndex)
                    {
                        this.m_selectedFrameIndex = destIndex;
                    }
                    else
                    {
                        this.m_selectedFrameIndex = destIndex - data.Count;
                    }
                }
                else
                {
                    this.m_selectedFrameIndex = -1;
                    this.m_selectedFrameCount = 0;
                    this.m_hoveredFrameIndex = -1;
                }
                this.Recalc();
            }
        }

        private void FrameOrganizationDialog_DragOver(object sender, DragEventArgs e)
        {
            this.m_dragObject = e.Data;
            Point pt = base.PointToClient(new Point(e.X, e.Y));
            e.Effect = (e.Data.GetDataPresent(typeof(FrameSelection)) && this.OverValidTarget()) ? DragDropEffects.Move : DragDropEffects.None;
            int insertionIndex = this.m_insertionIndex;
            if (this.m_timelineBounds.Contains(pt))
            {
                int num2 = Math.Min(this.m_timeline.Count, this.hScrollBar.Value + ((pt.X - this.m_timelineBounds.Left) / this.m_frameListFrameWidth));
                if (num2 <= (this.hScrollBar.Value + this.m_framesVisible))
                {
                    this.m_insertionIndex = num2;
                }
            }
            else
            {
                this.m_insertionIndex = -1;
            }
            if (insertionIndex != this.m_insertionIndex)
            {
                this.Refresh();
            }
        }

        private void FrameOrganizationDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            HelpDialog dialog = new HelpDialog("Select a single frame by clicking on it in the timeline.\n\nSelect multiple frames by selecting a frame then Shift-clicking another.\n\nRemove frames from the timeline by dragging a selection to one of the four clipboards,\nor to the trash.\n\nMove frames around in the timeline by dragging the selection to a new point, or by\ndragging a clipboard to the timeline.\n\nMerge frames together by dragging a selection or clipboard to the timeline while holding\nthe Control key.\n\nFrames can be copied by selecting a range and pressing Ctrl-C.\n\nRoutines can be created by dragging a series of frames to the folder and loaded by\ndouble-clicking on the folder.");
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void FrameOrganizationDialog_KeyDown(object sender, KeyEventArgs e)
        {
            this.m_shiftDown = e.Shift;
            this.m_merging = e.Control;
            if ((e.KeyData & (Keys.Control | Keys.C)) == (Keys.Control | Keys.C))
            {
                this.CopySelection();
            }
        }

        private void FrameOrganizationDialog_KeyUp(object sender, KeyEventArgs e)
        {
            this.m_shiftDown = e.Shift;
            this.m_merging = e.Control;
        }

        private void FrameOrganizationDialog_Load(object sender, EventArgs e)
        {
            this.Recalc();
        }

        private void FrameOrganizationDialog_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.m_timelineBounds.Contains(e.Location))
            {
                int num = this.hScrollBar.Value + ((e.X - this.m_timelineBounds.X) / this.m_frameListFrameWidth);
                if ((num < this.m_timeline.Count) && (num != this.m_selectedFrameIndex))
                {
                    if (this.m_shiftDown)
                    {
                        if (this.m_selectedFrameIndex != -1)
                        {
                            if (num > this.m_selectedFrameIndex)
                            {
                                this.m_selectedFrameCount = (num - this.m_selectedFrameIndex) + 1;
                            }
                            else
                            {
                                this.m_selectedFrameCount = (this.m_selectedFrameIndex - num) + 1;
                                this.m_selectedFrameIndex = num;
                            }
                            this.CalcSelectionSpan();
                        }
                    }
                    else if ((num < this.m_selectedFrameIndex) || (num > (this.m_selectedFrameIndex + this.m_selectedFrameCount)))
                    {
                        this.m_selectedFrameIndex = num;
                        this.m_selectedFrameCount = 1;
                        this.CalcSelectionSpan();
                    }
                }
            }
        }

        private void FrameOrganizationDialog_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.None)
            {
                if (this.m_timelineBounds.Contains(e.Location))
                {
                    int num = this.hScrollBar.Value + ((e.X - this.m_timelineBounds.X) / this.m_frameListFrameWidth);
                    if ((num != this.m_hoveredFrameIndex) && (num < this.m_timeline.Count))
                    {
                        this.m_hoveredFrameIndex = num;
                        this.Refresh();
                    }
                }
            }
            else
            {
                FrameSelection data = null;
                if (this.m_timelineBounds.Contains(e.Location) && (this.m_selectedFrameIndex >= 0))
                {
                    data = new FrameSelection(this.m_timeline, this.m_selectedFrameIndex, this.m_selectedFrameCount);
                }
                else if (this.m_clipboard1Bounds.Contains(e.Location) && (this.m_clipboard1.Count > 0))
                {
                    data = new FrameSelection(this.m_clipboard1, 0, this.m_clipboard1.Count);
                }
                else if (this.m_clipboard2Bounds.Contains(e.Location) && (this.m_clipboard2.Count > 0))
                {
                    data = new FrameSelection(this.m_clipboard2, 0, this.m_clipboard2.Count);
                }
                else if (this.m_clipboard3Bounds.Contains(e.Location) && (this.m_clipboard3.Count > 0))
                {
                    data = new FrameSelection(this.m_clipboard3, 0, this.m_clipboard3.Count);
                }
                else if (this.m_clipboard4Bounds.Contains(e.Location) && (this.m_clipboard4.Count > 0))
                {
                    data = new FrameSelection(this.m_clipboard4, 0, this.m_clipboard4.Count);
                }
                if (data != null)
                {
                    base.DoDragDrop(data, DragDropEffects.Move);
                }
            }
        }

        private void FrameOrganizationDialog_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && !this.m_shiftDown)
            {
                int num = this.hScrollBar.Value + ((e.X - this.m_timelineBounds.X) / this.m_frameListFrameWidth);
                if ((num >= this.m_selectedFrameIndex) && (num <= (this.m_selectedFrameIndex + this.m_selectedFrameCount)))
                {
                    this.m_selectedFrameIndex = num;
                    this.m_selectedFrameCount = 1;
                    this.CalcSelectionSpan();
                }
            }
        }

        private void FrameOrganizationDialog_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            KeyState state = new KeyState(e.KeyState);
            if (e.EscapePressed || (!state.LButton && !this.OverValidTarget()))
            {
                this.m_dragObject = null;
                e.Action = DragAction.Cancel;
                this.m_insertionIndex = -1;
                this.Refresh();
            }
            else if (this.m_merging != state.Ctrl)
            {
                this.m_merging = state.Ctrl;
                this.Refresh();
            }
        }

        private void FrameOrganizationDialog_Resize(object sender, EventArgs e)
        {
            if (!this.m_resizing)
            {
                this.Recalc();
            }
        }

        private void FrameOrganizationDialog_ResizeBegin(object sender, EventArgs e)
        {
            this.m_resizing = true;
        }

        private void FrameOrganizationDialog_ResizeEnd(object sender, EventArgs e)
        {
            this.m_resizing = false;
            this.Recalc();
        }

        private int GetFrameEndTime(int index, List<Frame> frames)
        {
            int num = 0;
            for (int i = 0; (i <= index) && (i < frames.Count); i++)
            {
                num += frames[i].Length;
            }
            return num;
        }

        private int GetFrameStartTime(int index, List<Frame> frames)
        {
            int num = 0;
            for (int i = 0; (i < index) && (i < frames.Count); i++)
            {
                num += frames[i].Length;
            }
            return num;
        }

        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrameOrganizationDialog));
            this.hScrollBar = new HScrollBar();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.pictureBoxEmptyBin = new PictureBox();
            this.pictureBoxFullBin = new PictureBox();
            this.pictureBoxFolder = new PictureBox();
            this.saveFileDialog = new SaveFileDialog();
            this.openFileDialog = new OpenFileDialog();
            ((ISupportInitialize) this.pictureBoxEmptyBin).BeginInit();
            ((ISupportInitialize) this.pictureBoxFullBin).BeginInit();
            ((ISupportInitialize) this.pictureBoxFolder).BeginInit();
            base.SuspendLayout();
            this.hScrollBar.Location = new Point(0x77, 0xbb);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new Size(0x11c, 0x11);
            this.hScrollBar.TabIndex = 0;
            this.hScrollBar.ValueChanged += new EventHandler(this.hScrollBar_ValueChanged);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x25d, 0x1c3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x2ae, 0x1c3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.pictureBoxEmptyBin.Image = (Image) manager.GetObject("pictureBoxEmptyBin.Image");
            this.pictureBoxEmptyBin.Location = new Point(0x26, 0x141);
            this.pictureBoxEmptyBin.Name = "pictureBoxEmptyBin";
            this.pictureBoxEmptyBin.Size = new Size(0x30, 0x30);
            this.pictureBoxEmptyBin.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxEmptyBin.TabIndex = 3;
            this.pictureBoxEmptyBin.TabStop = false;
            this.pictureBoxEmptyBin.Visible = false;
            this.pictureBoxFullBin.Image = (Image) manager.GetObject("pictureBoxFullBin.Image");
            this.pictureBoxFullBin.Location = new Point(0x69, 0x141);
            this.pictureBoxFullBin.Name = "pictureBoxFullBin";
            this.pictureBoxFullBin.Size = new Size(0x30, 0x30);
            this.pictureBoxFullBin.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxFullBin.TabIndex = 4;
            this.pictureBoxFullBin.TabStop = false;
            this.pictureBoxFullBin.Visible = false;
            this.pictureBoxFolder.Image = (Image) manager.GetObject("pictureBoxFolder.Image");
            this.pictureBoxFolder.Location = new Point(0x26, 0x183);
            this.pictureBoxFolder.Name = "pictureBoxFolder";
            this.pictureBoxFolder.Size = new Size(0x30, 0x30);
            this.pictureBoxFolder.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxFolder.TabIndex = 5;
            this.pictureBoxFolder.TabStop = false;
            this.pictureBoxFolder.Visible = false;
            this.AllowDrop = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x305, 0x1e6);
            base.Controls.Add(this.pictureBoxFolder);
            base.Controls.Add(this.pictureBoxFullBin);
            base.Controls.Add(this.pictureBoxEmptyBin);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.hScrollBar);
            base.HelpButton = true;
            base.KeyPreview = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x30d, 520);
            base.Name = "FrameOrganizationDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Frame Organization";
            base.QueryContinueDrag += new QueryContinueDragEventHandler(this.FrameOrganizationDialog_QueryContinueDrag);
            base.ResizeBegin += new EventHandler(this.FrameOrganizationDialog_ResizeBegin);
            base.DragDrop += new DragEventHandler(this.FrameOrganizationDialog_DragDrop);
            base.HelpButtonClicked += new CancelEventHandler(this.FrameOrganizationDialog_HelpButtonClicked);
            base.Resize += new EventHandler(this.FrameOrganizationDialog_Resize);
            base.DragOver += new DragEventHandler(this.FrameOrganizationDialog_DragOver);
            base.DoubleClick += new EventHandler(this.FrameOrganizationDialog_DoubleClick);
            base.MouseUp += new MouseEventHandler(this.FrameOrganizationDialog_MouseUp);
            base.KeyUp += new KeyEventHandler(this.FrameOrganizationDialog_KeyUp);
            base.MouseMove += new MouseEventHandler(this.FrameOrganizationDialog_MouseMove);
            base.KeyDown += new KeyEventHandler(this.FrameOrganizationDialog_KeyDown);
            base.ResizeEnd += new EventHandler(this.FrameOrganizationDialog_ResizeEnd);
            base.MouseDown += new MouseEventHandler(this.FrameOrganizationDialog_MouseDown);
            base.Load += new EventHandler(this.FrameOrganizationDialog_Load);
            ((ISupportInitialize) this.pictureBoxEmptyBin).EndInit();
            ((ISupportInitialize) this.pictureBoxFullBin).EndInit();
            ((ISupportInitialize) this.pictureBoxFolder).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void MergeFrames(List<Frame> sourceCollection, int sourceIndex, int count, List<Frame> destCollection, int destIndex)
        {
            count = Math.Min(count, destCollection.Count - destIndex);
            for (int i = 0; i < count; i++)
            {
                List<uint> cells = sourceCollection[sourceIndex + i].Cells;
                Frame frame = destCollection[destIndex + i].Clone();
                List<uint> list2 = frame.Cells;
                foreach (uint num2 in cells)
                {
                    if (!list2.Contains(num2))
                    {
                        list2.Add(num2);
                    }
                }
                destCollection[destIndex + i] = frame;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int num2;
            float num3;
            Rectangle rectangle;
            int num4;
            int height = (int) e.Graphics.MeasureString("1", this.m_clipboardNumberFont).Height;
            e.Graphics.FillRectangle(Brushes.Black, this.m_largeFrameBounds);
            if (this.m_hoveredFrameIndex != -1)
            {
                this.DrawFrameIn(this.m_timeline[this.m_hoveredFrameIndex], this.m_largeFrameBounds, 4, e.Graphics);
            }
            e.Graphics.FillRectangle(Brushes.White, this.m_timelineBounds);
            e.Graphics.DrawRectangle(Pens.Black, this.m_timelineBounds);
            if (this.m_selectedFrameCount > 0)
            {
                num2 = this.m_selectedFrameIndex + this.m_selectedFrameCount;
                num3 = this.hScrollBar.Value + this.m_framesVisible;
                rectangle = new Rectangle();
                for (num4 = this.m_selectedFrameIndex; num4 < num2; num4++)
                {
                    if ((num4 >= this.hScrollBar.Value) && (num4 < num3))
                    {
                        rectangle.X = (this.m_timelineBounds.X + 1) + ((num4 - this.hScrollBar.Value) * this.m_frameListFrameWidth);
                        rectangle.Y = this.m_timelineBounds.Top + 1;
                        rectangle.Width = this.m_frameListFrameWidth;
                        rectangle.Height = this.m_timelineBounds.Height - 1;
                        e.Graphics.FillRectangle(Brushes.LightSteelBlue, Rectangle.Intersect(this.m_timelineBounds, rectangle));
                    }
                }
                e.Graphics.DrawString(string.Format("{0:d2}:{1:d2}.{2:d3} - {3:d2}:{4:d2}.{5:d3}", new object[] { this.m_timelineSelectionStart.Minutes, this.m_timelineSelectionStart.Seconds, this.m_timelineSelectionStart.Milliseconds, this.m_timelineSelectionEnd.Minutes, this.m_timelineSelectionEnd.Seconds, this.m_timelineSelectionEnd.Milliseconds }), this.Font, this.m_clipboardPanelFontBrush, (float) this.m_timelineBounds.Left, (float) (this.m_timelineBounds.Top - 20));
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.White, this.m_timelineBounds.Left, this.m_timelineBounds.Top - 20, this.m_timelineBounds.Width, 20);
            }
            if (this.m_insertionIndex != -1)
            {
                if (!this.m_merging)
                {
                    e.Graphics.FillRectangle(Brushes.Gray, (this.m_timelineBounds.Left + 1) + ((this.m_insertionIndex - this.hScrollBar.Value) * this.m_frameListFrameWidth), this.m_timelineBounds.Top + 1, 5, this.m_timelineBounds.Height - 1);
                }
                else
                {
                    FrameSelection data = (FrameSelection) this.m_dragObject.GetData(typeof(FrameSelection));
                    int num5 = Math.Min(data.Count, this.m_timeline.Count - this.m_insertionIndex);
                    num2 = this.m_insertionIndex + num5;
                    num3 = this.hScrollBar.Value + this.m_framesVisible;
                    rectangle = new Rectangle();
                    for (num4 = this.m_insertionIndex; num4 < num2; num4++)
                    {
                        if ((num4 >= this.hScrollBar.Value) && (num4 < num3))
                        {
                            rectangle.X = (this.m_timelineBounds.X + 1) + ((num4 - this.hScrollBar.Value) * this.m_frameListFrameWidth);
                            rectangle.Y = this.m_timelineBounds.Top + 1;
                            rectangle.Width = this.m_frameListFrameWidth;
                            rectangle.Height = this.m_timelineBounds.Height - 1;
                            e.Graphics.FillRectangle(Brushes.LightCoral, Rectangle.Intersect(this.m_timelineBounds, rectangle));
                        }
                    }
                }
            }
            this.DrawFrameListIn(this.m_timeline, this.m_timelineBounds, this.hScrollBar.Value, e.Graphics);
            GraphicsPath path = this.RoundedRectPath(this.m_clipboard1Bounds.Left - 15, this.m_clipboard1Bounds.Top, 30, this.m_clipboard1Bounds.Height, 3);
            e.Graphics.FillPath(this.m_clipboardPanelBrush, path);
            e.Graphics.DrawPath(this.m_clipboardPanelPen, path);
            path.Dispose();
            e.Graphics.DrawString("1", this.m_clipboardNumberFont, this.m_clipboardPanelFontBrush, (float) ((this.m_clipboard1Bounds.Left - 15) + 3), (float) (this.m_clipboard1Bounds.Top + ((this.m_clipboard1Bounds.Height - height) / 2)));
            e.Graphics.FillRectangle(Brushes.White, this.m_clipboard1Bounds);
            e.Graphics.DrawRectangle(Pens.Black, this.m_clipboard1Bounds);
            this.DrawFrameListIn(this.m_clipboard1, this.m_clipboard1Bounds, 0, e.Graphics);
            path = this.RoundedRectPath(this.m_clipboard2Bounds.Left - 15, this.m_clipboard2Bounds.Top, 30, this.m_clipboard2Bounds.Height, 3);
            e.Graphics.FillPath(this.m_clipboardPanelBrush, path);
            e.Graphics.DrawPath(this.m_clipboardPanelPen, path);
            path.Dispose();
            e.Graphics.DrawString("2", this.m_clipboardNumberFont, this.m_clipboardPanelFontBrush, (float) ((this.m_clipboard2Bounds.Left - 15) + 3), (float) (this.m_clipboard2Bounds.Top + ((this.m_clipboard2Bounds.Height - height) / 2)));
            e.Graphics.FillRectangle(Brushes.White, this.m_clipboard2Bounds);
            e.Graphics.DrawRectangle(Pens.Black, this.m_clipboard2Bounds);
            this.DrawFrameListIn(this.m_clipboard2, this.m_clipboard2Bounds, 0, e.Graphics);
            path = this.RoundedRectPath(this.m_clipboard3Bounds.Left - 15, this.m_clipboard3Bounds.Top, 30, this.m_clipboard3Bounds.Height, 3);
            e.Graphics.FillPath(this.m_clipboardPanelBrush, path);
            e.Graphics.DrawPath(this.m_clipboardPanelPen, path);
            path.Dispose();
            e.Graphics.DrawString("3", this.m_clipboardNumberFont, this.m_clipboardPanelFontBrush, (float) ((this.m_clipboard3Bounds.Left - 15) + 3), (float) (this.m_clipboard3Bounds.Top + ((this.m_clipboard3Bounds.Height - height) / 2)));
            e.Graphics.FillRectangle(Brushes.White, this.m_clipboard3Bounds);
            e.Graphics.DrawRectangle(Pens.Black, this.m_clipboard3Bounds);
            this.DrawFrameListIn(this.m_clipboard3, this.m_clipboard3Bounds, 0, e.Graphics);
            path = this.RoundedRectPath(this.m_clipboard4Bounds.Left - 15, this.m_clipboard4Bounds.Top, 30, this.m_clipboard4Bounds.Height, 3);
            e.Graphics.FillPath(this.m_clipboardPanelBrush, path);
            e.Graphics.DrawPath(this.m_clipboardPanelPen, path);
            path.Dispose();
            e.Graphics.DrawString("4", this.m_clipboardNumberFont, this.m_clipboardPanelFontBrush, (float) ((this.m_clipboard4Bounds.Left - 15) + 3), (float) (this.m_clipboard4Bounds.Top + ((this.m_clipboard4Bounds.Height - height) / 2)));
            e.Graphics.FillRectangle(Brushes.White, this.m_clipboard4Bounds);
            e.Graphics.DrawRectangle(Pens.Black, this.m_clipboard4Bounds);
            this.DrawFrameListIn(this.m_clipboard4, this.m_clipboard4Bounds, 0, e.Graphics);
            e.Graphics.DrawImage((this.m_trash.Count == 0) ? this.pictureBoxEmptyBin.Image : this.pictureBoxFullBin.Image, this.m_trashBounds);
            e.Graphics.DrawImage(this.pictureBoxFolder.Image, this.m_folderBounds);
        }

        private bool OverValidTarget()
        {
            Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
            return ((((this.m_timelineBounds.Contains(pt) || (this.m_clipboard1Bounds.Contains(pt) && (this.m_clipboard1.Count == 0))) || ((this.m_clipboard2Bounds.Contains(pt) && (this.m_clipboard2.Count == 0)) || (this.m_clipboard3Bounds.Contains(pt) && (this.m_clipboard3.Count == 0)))) || ((this.m_clipboard4Bounds.Contains(pt) && (this.m_clipboard4.Count == 0)) || this.m_trashBounds.Contains(pt))) || this.m_folderBounds.Contains(pt));
        }

        private void Recalc()
        {
            this.m_largeFrameBounds.X = (base.ClientRectangle.Width - this.m_largeFrameBounds.Width) / 2;
            this.m_timelineBounds.Y = this.m_largeFrameBounds.Bottom + 20;
            this.m_timelineBounds.Width = base.ClientRectangle.Width - (this.m_timelineBounds.X * 2);
            this.m_timelineBounds.Height = (this.m_frameHeight + 10) + 1;
            this.m_framesVisible = ((float) this.m_timelineBounds.Width) / ((float) this.m_frameListFrameWidth);
            this.m_clipboard1Bounds.Y = this.m_timelineBounds.Bottom + 50;
            this.m_clipboard1Bounds.Width = base.ClientRectangle.Width - 200;
            this.m_clipboard1Bounds.Height = this.m_timelineBounds.Height;
            this.m_clipboard2Bounds.Y = this.m_clipboard1Bounds.Bottom + 30;
            this.m_clipboard2Bounds.Width = this.m_clipboard1Bounds.Width;
            this.m_clipboard2Bounds.Height = this.m_clipboard1Bounds.Height;
            this.m_clipboard3Bounds.Y = this.m_clipboard2Bounds.Bottom + 30;
            this.m_clipboard3Bounds.Width = this.m_clipboard2Bounds.Width;
            this.m_clipboard3Bounds.Height = this.m_clipboard2Bounds.Height;
            this.m_clipboard4Bounds.Y = this.m_clipboard3Bounds.Bottom + 30;
            this.m_clipboard4Bounds.Width = this.m_clipboard3Bounds.Width;
            this.m_clipboard4Bounds.Height = this.m_clipboard3Bounds.Height;
            this.m_trashBounds.Y = (base.ClientRectangle.Bottom - this.m_trashBounds.Left) - this.m_trashBounds.Height;
            this.m_folderBounds.Y = this.m_trashBounds.Y;
            if (this.m_trashBounds.Top < this.m_clipboard4Bounds.Bottom)
            {
                base.Height += (this.m_clipboard4Bounds.Bottom - this.m_trashBounds.Top) + 50;
            }
            if (this.m_timeline.Count == 0)
            {
                if (this.hScrollBar.Visible)
                {
                    this.hScrollBar.Visible = false;
                    this.hScrollBar.Minimum = this.hScrollBar.Maximum = this.hScrollBar.Value = 0;
                }
            }
            else
            {
                this.hScrollBar.Location = new Point(this.m_timelineBounds.Left, this.m_timelineBounds.Bottom + 1);
                this.hScrollBar.Width = this.m_timelineBounds.Width;
                this.hScrollBar.Maximum = this.m_timeline.Count - 1;
                this.hScrollBar.LargeChange = this.m_timelineBounds.Width / this.m_frameListFrameWidth;
                if (!this.hScrollBar.Visible)
                {
                    this.hScrollBar.Visible = true;
                }
            }
            this.Refresh();
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

        private void SaveToRoutine(FrameSelection selection)
        {
            this.saveFileDialog.InitialDirectory = Paths.RoutinePath;
            this.saveFileDialog.Filter = "LedTriks routine|*.ltr";
            this.saveFileDialog.DefaultExt = ".ltr";
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlDocument document = Xml.CreateXmlDocument("Frames");
                XmlNode documentElement = document.DocumentElement;
                int num = selection.StartIndex + selection.Count;
                for (int i = selection.StartIndex; i < num; i++)
                {
                    selection.Source[i].SaveToXml(documentElement);
                }
                document.Save(this.saveFileDialog.FileName);
            }
        }

        private List<Frame> FirstEmptyClipboard
        {
            get
            {
                if (this.m_clipboard1.Count == 0)
                {
                    return this.m_clipboard1;
                }
                if (this.m_clipboard2.Count == 0)
                {
                    return this.m_clipboard2;
                }
                if (this.m_clipboard3.Count == 0)
                {
                    return this.m_clipboard3;
                }
                if (this.m_clipboard4.Count == 0)
                {
                    return this.m_clipboard4;
                }
                return null;
            }
        }

        public List<Frame> Frames
        {
            get
            {
                return this.m_timeline;
            }
        }
    }
}

