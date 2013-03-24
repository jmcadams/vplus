namespace LedTriks
{
    using LedTriksUtil;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;
    using VixenPlus.Dialogs;

    public partial class LedTriks : UIBase
    {
        private const int BOARD_HEIGHT = 0x10;
        private const int BOARD_WIDTH = 0x30;
        private Size m_boardLayout;
        private int m_cellSize;
        private List<uint> m_clipboard;
        private Dictionary<uint, uint> m_currentFrameCells;
        private int m_currentFrameIndex;
        private XmlNode m_dataNode;
        private int m_defaultFrameLength;
        private bool m_execute;
        private int m_executionContextHandle;
        private IExecution m_executionInterface;
        private List<Frame> m_frames;
        private Generator m_generator;
        private bool m_ghostPreviousState;
        private Point m_mouseAt;
        private bool m_pasting;
        private bool m_playing;
        private int m_playStartFrame;
        private Thread m_playThread;
        private Preference2 m_preferences;
        private Point m_previousPosition;
        private bool m_resizing;
        private EventSequence m_sequence;
        private SetFrameDelegate m_setFrame;
        private Stopwatch m_stopwatch;

        public LedTriks()
        {
            object obj3;
            this.m_sequence = null;
            this.m_resizing = false;
            this.m_mouseDown = MouseButtons.None;
            this.m_cellSize = 5;
            this.m_mouseAt = new Point();
            this.m_currentFrameIndex = -1;
            this.m_pasting = false;
            this.m_execute = false;
            this.m_executionInterface = null;
            this.m_executionContextHandle = 0;
            this.m_previousPosition = new Point(0, 0);
            this.m_generator = null;
            this.m_playing = false;
            this.InitializeComponent();
            object obj2 = null;
            if (Interfaces.Available.TryGetValue("ISystem", out obj2))
            {
                this.m_preferences = ((ISystem)obj2).UserPreferences;
            }
            this.m_generator = new Generator();
            this.m_clipboard = new List<uint>();
            this.m_setFrame = new SetFrameDelegate(this.SetFrame);
            this.SetDefaultFrameLength(50);
            this.m_currentFrameCells = new Dictionary<uint, uint>();
            this.m_frames = new List<Frame>();
            this.m_stopwatch = new Stopwatch();
            this.ResetAutoText();
            if (Interfaces.Available.TryGetValue("IExecution", out obj3))
            {
                this.m_executionInterface = (IExecution)obj3;
            }
        }

        private void attachedPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PluginListDialog dialog = new PluginListDialog(this.m_sequence);
            dialog.ShowDialog();
            dialog.Dispose();
            base.IsDirty = true;
        }

        private void boardLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BoardLayoutDialog dialog = new BoardLayoutDialog(this.m_boardLayout);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetBoardLayout(dialog.BoardLayout.Width, dialog.BoardLayout.Height);
            }
            dialog.Dispose();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.m_currentFrameCells.Clear();
            base.IsDirty = true;
            this.pictureBoxPreview.Refresh();
        }

        private void buttonClone_Click(object sender, EventArgs e)
        {
            this.m_frames.Insert(this.m_currentFrameIndex + 1, this.m_frames[this.m_currentFrameIndex].Clone());
            this.SetFrame(this.m_currentFrameIndex + 1);
            base.IsDirty = true;
        }

        private void buttonCreateAutoText_Click(object sender, EventArgs e)
        {
            if (this.m_generator.TextFont == null)
            {
                MessageBox.Show("Please select a font first.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    this.m_generator.IgnoreFontDescent = this.ignoreFontDescentToolStripMenuItem.Checked;
                    List<Frame> collection = this.m_generator.GenerateTextFrames(this.textBoxAutoText.Text);
                    if (this.autoresetAutoTextDefaultsToolStripMenuItem.Checked)
                    {
                        this.ResetAutoText();
                    }
                    this.m_frames.AddRange(collection);
                    this.SetFrame(this.m_frames.Count - 1);
                    this.UpdateTotalLength();
                    base.IsDirty = true;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {
            if (this.fontDialog.ShowDialog() == DialogResult.OK)
            {
                this.SetFont(this.fontDialog.Font);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (this.m_currentFrameIndex < (this.m_frames.Count - 1))
            {
                this.SetFrame(this.m_currentFrameIndex + 1);
            }
        }

        private void buttonOrganize_Click(object sender, EventArgs e)
        {
            FrameOrganizationDialog dialog = new FrameOrganizationDialog(this.m_frames, this.m_boardLayout.Width * 0x30, this.m_boardLayout.Height * 0x10);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_frames = dialog.Frames;
                if (this.m_currentFrameIndex >= this.m_frames.Count)
                {
                    if (this.m_frames.Count > 0)
                    {
                        this.SetFrame(this.m_frames.Count - 1);
                    }
                    else
                    {
                        this.NewFrame();
                    }
                }
                else
                {
                    this.SetFrame(this.m_currentFrameIndex);
                }
                base.IsDirty = true;
            }
            dialog.Dispose();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (this.m_currentFrameIndex > 0)
            {
                this.SetFrame(this.m_currentFrameIndex - 1);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if ((this.m_frames.Count != 0) && (!this.confirmOnFrameRemoveToolStripMenuItem.Checked || (MessageBox.Show("Remove this frame?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)))
            {
                this.m_frames.RemoveAt(this.m_currentFrameIndex);
                if (this.m_frames.Count > 0)
                {
                    this.UpdateTotalLength();
                    this.trackBarFrame.Maximum--;
                    this.SetFrame((this.m_currentFrameIndex == this.m_frames.Count) ? (this.m_frames.Count - 1) : this.m_currentFrameIndex);
                }
                else
                {
                    this.NewFrame();
                }
                base.IsDirty = true;
            }
        }

        private void buttonRemoveAll_Click(object sender, EventArgs e)
        {
            if ((this.m_frames.Count != 0) && (!this.confirmOnFrameRemoveToolStripMenuItem.Checked || (MessageBox.Show("Remove ALL frames?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)))
            {
                this.m_frames.Clear();
                this.trackBarFrame.Maximum = this.trackBarFrame.Minimum;
                this.NewFrame();
            }
        }

        private void buttonSaveAndCreateNew_Click(object sender, EventArgs e)
        {
            this.SaveFrame();
            this.NewFrame();
        }

        private void buttonSaveFrame_Click(object sender, EventArgs e)
        {
            this.SaveFrame();
        }

        private void checkBoxLoop_CheckedChanged(object sender, EventArgs e)
        {
            this.m_executionInterface.SetLoopState(this.m_executionContextHandle, this.checkBoxLoop.Checked);
        }

        private void copyThisFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_clipboard.Clear();
            this.m_clipboard.AddRange(this.m_frames[this.m_currentFrameIndex].Cells);
        }



        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_execute = true;
            this.previewToolStripMenuItem.Checked = false;
            this.executeToolStripMenuItem.Checked = true;
        }

        //~LedTriks()
        //{
        //    //base.Dispose();
        //}

        private void ghostThePreviousFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pictureBoxPreview.Refresh();
        }

        private void Init()
        {
            this.m_executionContextHandle = this.m_executionInterface.RequestContext(true, false, null);
            this.m_executionInterface.SetSynchronousContext(this.m_executionContextHandle, this.m_sequence);
        }



        private void LedTriks_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Stop();
            if ((e.CloseReason == CloseReason.UserClosing) && base.IsDirty)
            {
                switch (MessageBox.Show("Save changes?", "LedTriks", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;

                    case DialogResult.Yes:
                        ((ISystem)Interfaces.Available["ISystem"]).InvokeSave(this);
                        break;
                }
            }
            this.m_executionInterface.ReleaseContext(this.m_executionContextHandle);
        }

        private void LedTriks_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x001b')
            {
                e.Handled = true;
                if (this.m_pasting)
                {
                    this.m_pasting = false;
                    this.pictureBoxPreview.Refresh();
                }
            }
        }

        private void LedTriks_Load(object sender, EventArgs e)
        {
            this.SetFont(this.Font);
        }

        private void LedTriks_Resize(object sender, EventArgs e)
        {
            if (!this.m_resizing)
            {
                this.groupBoxPreviewControl.Left = (this.groupBoxPreviewControl.Parent.Width - this.groupBoxPreviewControl.Width) / 2;
                this.checkBoxLoop.Left = (this.checkBoxLoop.Parent.Width - this.checkBoxLoop.Width) / 2;
                this.SetBoardLayout(this.m_boardLayout.Width, this.m_boardLayout.Height);
            }
        }

        private void LedTriks_ResizeBegin(object sender, EventArgs e)
        {
            this.m_resizing = true;
        }

        private void LedTriks_ResizeEnd(object sender, EventArgs e)
        {
            this.m_resizing = false;
        }

        private void linkLabelMoreOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AutoTextOptionsDialog dialog = new AutoTextOptionsDialog(this.m_generator);
            dialog.ShowDialog();
            dialog.Dispose();
            base.AcceptButton = this.buttonCreateAutoText;
        }

        private void LoadFrames()
        {
            XmlNode node = this.m_dataNode.SelectSingleNode("Boards");
            this.SetBoardLayout(int.Parse(node.Attributes["width"].Value), int.Parse(node.Attributes["height"].Value));
            this.SetCellSize(int.Parse(this.m_dataNode.SelectSingleNode("CellSize").InnerText));
            foreach (XmlNode node2 in this.m_dataNode.SelectNodes("Frames/Frame"))
            {
                this.m_frames.Add(new Frame(node2));
            }
        }

        public override EventSequence New()
        {
            this.m_sequence = new EventSequence(this.m_preferences);
            this.m_sequence.Profile = null;
            this.m_sequence.ChannelCount = 0;
            this.m_sequence.Time = 0;
            this.Text = "<Unnamed sequence>";
            this.m_dataNode = this.m_sequence.Extensions[this.FileExtension];
            this.SetBoardLayout(1, 1);
            this.SetCellSize(5);
            this.NewFrame();
            this.Init();
            return this.m_sequence;
        }

        public override EventSequence New(EventSequence seedSequence)
        {
            return null;
        }

        private void NewFrame()
        {
            this.m_frames.Add(new Frame(this.m_defaultFrameLength));
            this.SetFrame(this.m_frames.Count - 1);
            base.IsDirty = true;
        }

        public override void Notify(Notification notification, object data)
        {
        }

        public override EventSequence Open(string filePath)
        {
            this.m_sequence = new EventSequence(filePath);
            this.Text = this.m_sequence.Name;
            this.m_dataNode = this.m_sequence.Extensions[this.FileExtension];
            this.LoadFrames();
            this.SetFrame(0);
            this.Init();
            return this.m_sequence;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_pasting = true;
            this.pictureBoxPreview.Refresh();
        }

        private void pictureBoxPlay_Click(object sender, EventArgs e)
        {
            this.PlayFrom(this.m_currentFrameIndex);
        }

        private void pictureBoxPlayFromBeginning_Click(object sender, EventArgs e)
        {
            this.PlayFrom(0);
        }

        private void pictureBoxPreview_MouseDown(object sender, MouseEventArgs e)
        {
            this.m_mouseDown |= e.Button;
            uint position = (uint)((this.m_mouseAt.X << 0x10) | this.m_mouseAt.Y);
            if ((this.m_mouseDown & MouseButtons.Left) != MouseButtons.None)
            {
                if (!this.m_pasting)
                {
                    this.SetAt(position);
                    this.pictureBoxPreview.Refresh();
                }
                else
                {
                    this.m_pasting = false;
                    foreach (uint num3 in this.m_clipboard)
                    {
                        int num4 = this.m_mouseAt.X - ((this.pictureBoxPreview.Width / 2) / this.m_cellSize);
                        int num5 = this.m_mouseAt.Y - ((this.pictureBoxPreview.Height / 2) / this.m_cellSize);
                        uint num2 = (uint)(((num3 & 0xffff0000) + (num4 << 0x10)) | ((num3 & 0xffff) + ((short)num5)));
                        this.m_currentFrameCells[num2] = num2;
                    }
                    this.pictureBoxPreview.Refresh();
                }
            }
            else if ((this.m_mouseDown & MouseButtons.Right) != MouseButtons.None)
            {
                this.ResetAt(position);
                this.pictureBoxPreview.Refresh();
            }
        }

        private void pictureBoxPreview_MouseMove(object sender, MouseEventArgs e)
        {
            int num = e.X / this.m_cellSize;
            int num2 = e.Y / this.m_cellSize;
            if ((num != this.m_previousPosition.X) || (num2 != this.m_previousPosition.Y))
            {
                if ((this.m_boardLayout.Width == 1) && (this.m_boardLayout.Height == 1))
                {
                    this.labelPosition.Text = string.Format("{0}, {1}", num % 0x30, num2);
                }
                else
                {
                    int num3 = (num / 0x30) + 1;
                    int num4 = num2 / 0x10;
                    this.labelPosition.Text = string.Format("{0}, {1}\n{2}, {3} in board {4}", new object[] { num, num2, num % 0x30, num2, (num4 * this.m_boardLayout.Width) + num3 });
                }
                this.m_mouseAt.X = num;
                this.m_mouseAt.Y = num2;
                uint position = (uint)((this.m_mouseAt.X << 0x10) | this.m_mouseAt.Y);
                if ((this.m_mouseDown & MouseButtons.Left) != MouseButtons.None)
                {
                    this.SetAt(position);
                }
                else if ((this.m_mouseDown & MouseButtons.Right) != MouseButtons.None)
                {
                    this.ResetAt(position);
                }
                Rectangle a = new Rectangle(this.m_previousPosition.X * this.m_cellSize, this.m_previousPosition.Y * this.m_cellSize, this.m_cellSize, this.m_cellSize);
                Rectangle b = new Rectangle(num * this.m_cellSize, num2 * this.m_cellSize, this.m_cellSize, this.m_cellSize);
                this.m_previousPosition.X = num;
                this.m_previousPosition.Y = num2;
                if (this.m_pasting)
                {
                    this.pictureBoxPreview.Refresh();
                }
                else
                {
                    this.pictureBoxPreview.Invalidate(Rectangle.Union(a, b));
                }
            }
        }

        private void pictureBoxPreview_MouseUp(object sender, MouseEventArgs e)
        {
            this.m_mouseDown &= ~e.Button;
        }

        private void pictureBoxPreview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Black, this.pictureBoxPreview.ClientRectangle);
            if ((this.m_currentFrameIndex > 0) && this.ghostThePreviousFrameToolStripMenuItem.Checked)
            {
                foreach (uint num in this.m_frames[this.m_currentFrameIndex - 1].Cells)
                {
                    e.Graphics.FillRectangle(Brushes.DarkRed, (float)((num >> 0x10) * this.m_cellSize), (float)((num & 0xffff) * this.m_cellSize), (float)this.m_cellSize, (float)this.m_cellSize);
                }
            }
            foreach (uint num in this.m_currentFrameCells.Keys)
            {
                e.Graphics.FillRectangle(Brushes.Red, (float)((num >> 0x10) * this.m_cellSize), (float)((num & 0xffff) * this.m_cellSize), (float)this.m_cellSize, (float)this.m_cellSize);
            }
            if (!this.m_pasting)
            {
                e.Graphics.FillRectangle(Brushes.Gray, this.m_mouseAt.X * this.m_cellSize, this.m_mouseAt.Y * this.m_cellSize, this.m_cellSize, this.m_cellSize);
            }
            else
            {
                int num2 = this.m_mouseAt.X - ((this.pictureBoxPreview.Width / 2) / this.m_cellSize);
                int num3 = this.m_mouseAt.Y - ((this.pictureBoxPreview.Height / 2) / this.m_cellSize);
                foreach (uint num in this.m_clipboard)
                {
                    e.Graphics.FillRectangle(Brushes.DarkMagenta, (float)((num2 + (num >> 0x10)) * this.m_cellSize), (float)((num3 + (num & 0xffff)) * this.m_cellSize), (float)this.m_cellSize, (float)this.m_cellSize);
                }
            }
        }

        private void pictureBoxShiftDown_Click(object sender, EventArgs e)
        {
            this.ShiftDown();
        }

        private void pictureBoxShiftDown_DoubleClick(object sender, EventArgs e)
        {
            this.ShiftDown();
        }

        private void pictureBoxShiftLeft_Click(object sender, EventArgs e)
        {
            this.ShiftLeft();
        }

        private void pictureBoxShiftLeft_DoubleClick(object sender, EventArgs e)
        {
            this.ShiftLeft();
        }

        private void pictureBoxShiftRight_Click(object sender, EventArgs e)
        {
            this.ShiftRight();
        }

        private void pictureBoxShiftRight_DoubleClick(object sender, EventArgs e)
        {
            this.ShiftRight();
        }

        private void pictureBoxShiftUp_Click(object sender, EventArgs e)
        {
            this.ShiftUp();
        }

        private void pictureBoxShiftUp_DoubleClick(object sender, EventArgs e)
        {
            this.ShiftUp();
        }

        private void pictureBoxStop_Click(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void PlayFrom(int frameIndex)
        {
            MethodInvoker method = null;
            this.Cursor = Cursors.WaitCursor;
            if (this.m_execute)
            {
                this.Save();
                this.m_playThread = null;
                if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle) == 1)
                {
                    return;
                }
                int millisecondStart = 0;
                int num3 = 0;
                while (num3 < frameIndex)
                {
                    millisecondStart += this.m_frames[num3].Length;
                    num3++;
                }
                int millisecondCount = millisecondStart;
                while (num3 < this.m_frames.Count)
                {
                    millisecondCount += this.m_frames[num3].Length;
                    num3++;
                }
                this.m_executionInterface.ExecutePlay(this.m_executionContextHandle, millisecondStart, millisecondCount);
            }
            else
            {
                if (this.m_playing || (this.m_frames.Count == 0))
                {
                    return;
                }
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate
                        {
                            this.m_ghostPreviousState = this.ghostThePreviousFrameToolStripMenuItem.Checked;
                            this.ghostThePreviousFrameToolStripMenuItem.Checked = false;
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    this.m_ghostPreviousState = this.ghostThePreviousFrameToolStripMenuItem.Checked;
                    this.ghostThePreviousFrameToolStripMenuItem.Checked = false;
                }
                this.m_playStartFrame = frameIndex;
                this.m_playThread = new Thread(new ThreadStart(this.PlayThread));
                this.m_playThread.Start();
            }
            this.Cursor = Cursors.Default;
        }

        private void PlayThread()
        {
            int playStartFrame = this.m_playStartFrame;
            int num2 = 0;
            this.SetUIState(false);
            this.m_playing = true;
            this.m_stopwatch.Reset();
            this.m_stopwatch.Start();
            while (this.m_playing && (playStartFrame < this.m_frames.Count))
            {
                num2 += this.m_frames[playStartFrame].Length;
                this.m_setFrame(playStartFrame);
                while (this.m_stopwatch.ElapsedMilliseconds < num2)
                {
                    if (!this.m_playing)
                    {
                        break;
                    }
                }
                playStartFrame++;
            }
            if (this.m_playing)
            {
                this.Stop();
                if (this.checkBoxLoop.Checked)
                {
                    this.PlayFrom(this.m_playStartFrame);
                }
                else
                {
                    this.SetUIState(true);
                }
            }
            else
            {
                this.Stop();
                this.SetUIState(true);
            }
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_execute = false;
            this.previewToolStripMenuItem.Checked = true;
            this.executeToolStripMenuItem.Checked = false;
        }

        private void RadioSelect(ToolStripMenuItem[] menuItems, string selectedMenuItemText)
        {
            ToolStripMenuItem item = null;
            foreach (ToolStripMenuItem item2 in menuItems)
            {
                item2.Checked = false;
                if (item2.Text == selectedMenuItemText)
                {
                    item = item2;
                }
            }
            item.Checked = true;
        }

        private void ResetAt(uint position)
        {
            if (this.m_currentFrameCells.ContainsKey(position))
            {
                this.m_currentFrameCells.Remove(position);
                base.IsDirty = true;
            }
        }

        private void ResetAutoText()
        {
            this.m_generator.TextScrollDirection = ScrollDirection.Left;
            this.m_generator.TextScrollExtent = ScrollExtent.OnAndOff;
        }

        public override DialogResult RunWizard(ref EventSequence resultSequence)
        {
            return DialogResult.None;
        }

        private void Save()
        {
            XmlNode nodeAlways = Xml.GetNodeAlways(this.m_dataNode, "Boards");
            Xml.SetAttribute(nodeAlways, "width", this.m_boardLayout.Width.ToString());
            Xml.SetAttribute(nodeAlways, "height", this.m_boardLayout.Height.ToString());
            Xml.SetValue(this.m_dataNode, "CellSize", this.m_cellSize.ToString());
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_dataNode, "Frames");
            foreach (Frame frame in this.m_frames)
            {
                frame.SaveToXml(emptyNodeAlways);
            }
            this.m_sequence.Time = this.TotalMilliseconds();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SaveFrame()
        {
            Frame frame = this.m_frames[this.m_currentFrameIndex];
            try
            {
                frame.Length = Convert.ToInt32(this.textBoxFrameLength.Text);
            }
            catch
            {
            }
            frame.Cells.Clear();
            frame.Cells.AddRange(this.m_currentFrameCells.Keys);
            this.UpdateTotalLength();
            base.IsDirty = true;
        }

        public override void SaveTo(string filePath)
        {
            this.Save();
            this.m_sequence.SaveTo(filePath);
            base.IsDirty = false;
            this.Text = this.m_sequence.Name;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SetAt(uint position)
        {
            if (!this.m_currentFrameCells.ContainsKey(position))
            {
                this.m_currentFrameCells[position] = position;
                base.IsDirty = true;
            }
        }

        private void SetBoardLayout(int width, int height)
        {
            this.m_boardLayout.Width = width;
            this.m_boardLayout.Height = height;
            this.pictureBoxPreview.Width = (this.m_cellSize * 0x30) * this.m_boardLayout.Width;
            this.pictureBoxPreview.Height = (this.m_cellSize * 0x10) * this.m_boardLayout.Height;
            if (this.pictureBoxPreview.Width > this.pictureBoxPreview.Parent.Width)
            {
                this.pictureBoxPreview.Left = 0;
            }
            else
            {
                this.pictureBoxPreview.Left = (this.pictureBoxPreview.Parent.Width - this.pictureBoxPreview.Width) / 2;
            }
            if (this.pictureBoxPreview.Height > this.pictureBoxPreview.Parent.Height)
            {
                this.pictureBoxPreview.Top = 0;
            }
            else
            {
                this.pictureBoxPreview.Top = (this.pictureBoxPreview.Parent.Height - this.pictureBoxPreview.Height) / 2;
            }
            this.pictureBoxPreview.Refresh();
            this.m_generator.BoardPixelHeight = height * 0x10;
            this.m_generator.BoardPixelWidth = width * 0x30;
        }

        private void SetCellSize(int cellSize)
        {
            this.m_cellSize = cellSize;
            this.SetBoardLayout(this.m_boardLayout.Width, this.m_boardLayout.Height);
            ToolStripMenuItem[] menuItems = new ToolStripMenuItem[] { this.toolStripMenuItem10, this.toolStripMenuItem11, this.toolStripMenuItem12, this.toolStripMenuItem13 };
            this.RadioSelect(menuItems, cellSize.ToString());
        }

        private void SetDefaultFrameLength(int milliseconds)
        {
            string str = milliseconds.ToString();
            this.m_defaultFrameLength = milliseconds;
            this.textBoxFrameLength.Text = str;
            this.toolStripTextBoxFrameLength.Text = str;
            this.m_generator.FrameLength = milliseconds;
        }

        private void SetFont(Font font)
        {
            if (font.Style != FontStyle.Regular)
            {
                this.labelFontName.Text = string.Format("{0}, {1}", font.FontFamily.Name, font.Style);
            }
            else
            {
                this.labelFontName.Text = font.FontFamily.Name;
            }
            this.m_generator.TextFont = font;
        }

        private void SetFrame(int index)
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate
                    {
                        this.SetFrame(index);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                this.trackBarFrame.Maximum = this.m_frames.Count;
                this.UpdateTotalLength();
                Frame frame = this.m_frames[index];
                this.textBoxFrameLength.Text = frame.Length.ToString();
                this.m_currentFrameCells.Clear();
                foreach (uint num in frame.Cells)
                {
                    this.m_currentFrameCells[num] = num;
                }
                int count = this.m_frames.Count;
                this.labelCurrentFrame.Text = string.Format("Frame {0} of {1}", index + 1, count);
                this.m_currentFrameIndex = index;
                this.trackBarFrame.Value = index + 1;
                this.pictureBoxPreview.Refresh();
            }
        }

        private void SetUIState(bool enabled)
        {
            MethodInvoker method = null;
            if (!base.IsDisposed && !base.Disposing)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate
                        {
                            this.buttonOrganize.Enabled = enabled;
                            this.groupBoxDisplay.Enabled = enabled;
                            this.groupBoxCurrentFrame.Enabled = enabled;
                            this.groupBoxAutoText.Enabled = enabled;
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    this.buttonOrganize.Enabled = enabled;
                    this.groupBoxDisplay.Enabled = enabled;
                    this.groupBoxCurrentFrame.Enabled = enabled;
                    this.groupBoxAutoText.Enabled = enabled;
                }
            }
        }

        private void ShiftDown()
        {
            List<uint> list = new List<uint>(this.m_currentFrameCells.Keys);
            this.m_currentFrameCells.Clear();
            foreach (uint num2 in list)
            {
                uint num = (num2 & 0xffff0000) | ((ushort)((num2 & 0xffff) + 1));
                this.m_currentFrameCells[num] = num;
            }
            base.IsDirty = true;
            this.pictureBoxPreview.Refresh();
        }

        private void ShiftLeft()
        {
            List<uint> list = new List<uint>(this.m_currentFrameCells.Keys);
            this.m_currentFrameCells.Clear();
            foreach (uint num2 in list)
            {
                uint num = num2 - 0x10000;
                this.m_currentFrameCells[num] = num;
            }
            base.IsDirty = true;
            this.pictureBoxPreview.Refresh();
        }

        private void ShiftRight()
        {
            List<uint> list = new List<uint>(this.m_currentFrameCells.Keys);
            this.m_currentFrameCells.Clear();
            foreach (uint num2 in list)
            {
                uint num = num2 + 0x10000;
                this.m_currentFrameCells[num] = num;
            }
            base.IsDirty = true;
            this.pictureBoxPreview.Refresh();
        }

        private void ShiftUp()
        {
            List<uint> list = new List<uint>(this.m_currentFrameCells.Keys);
            this.m_currentFrameCells.Clear();
            foreach (uint num2 in list)
            {
                uint num = (num2 & 0xffff0000) | ((ushort)((num2 & 0xffff) - 1));
                this.m_currentFrameCells[num] = num;
            }
            base.IsDirty = true;
            this.pictureBoxPreview.Refresh();
        }

        private void Stop()
        {
            MethodInvoker method = null;
            if (this.m_execute)
            {
                if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle) != 0)
                {
                    this.m_executionInterface.ExecuteStop(this.m_executionContextHandle);
                }
            }
            else if (this.m_playing)
            {
                this.m_playing = false;
                this.m_stopwatch.Stop();
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate
                        {
                            this.ghostThePreviousFrameToolStripMenuItem.Checked = this.m_ghostPreviousState;
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    this.ghostThePreviousFrameToolStripMenuItem.Checked = this.m_ghostPreviousState;
                }
            }
        }

        private void textBoxAutoText_Enter(object sender, EventArgs e)
        {
            base.AcceptButton = this.buttonCreateAutoText;
        }

        private void textBoxAutoText_Leave(object sender, EventArgs e)
        {
            base.AcceptButton = null;
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            this.SetCellSize(int.Parse(item.Text));
            base.IsDirty = true;
        }

        private void toolStripTextBoxFrameLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                try
                {
                    this.m_defaultFrameLength = Convert.ToInt32(this.toolStripTextBoxFrameLength.Text);
                    this.m_generator.FrameLength = this.m_defaultFrameLength;
                    base.IsDirty = true;
                }
                catch
                {
                }
            }
        }

        private int TotalMilliseconds()
        {
            int num = 0;
            foreach (Frame frame in this.m_frames)
            {
                num += frame.Length;
            }
            return num;
        }

        private void trackBarFrame_Scroll(object sender, EventArgs e)
        {
            this.SetFrame(this.trackBarFrame.Value - 1);
        }

        private void UpdateTotalLength()
        {
            int num = this.TotalMilliseconds();
            this.labelLength.Text = string.Format("{0}:{1:d2}.{2:d3}", num / 0xea60, (num % 0xea60) / 0x3e8, num % 0x3e8);
        }

        public override string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        public override string Description
        {
            get
            {
                return "LedTriks sequence editor";
            }
        }

        public override string FileExtension
        {
            get
            {
                return ".led";
            }
        }

        public override string FileTypeDescription
        {
            get
            {
                return "LedTriks sequence";
            }
        }

        public override EventSequence Sequence
        {
            get
            {
                return this.m_sequence;
            }
            set
            {
                this.m_sequence = value;
            }
        }

        private delegate void SetFrameDelegate(int index);
    }
}