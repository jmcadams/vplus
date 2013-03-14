namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Vixen.Dialogs;

    internal class ProgramManagerDialog : Form
    {
        private AddEditRemove addEditRemove;
        private Button buttonCancel;
        private Button buttonOK;
        private Button buttonRadioAction;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBoxProgramPlugin;
        private Label label1;
        private Label labelProgramName;
        private ListBox listBoxPrograms;
        private ListBox listBoxProgramSequences;
        private ListBox listBoxSequences;
        private StringFormat m_clipFormat = new StringFormat(StringFormatFlags.NoWrap);
        private bool m_dirty = false;
        private object m_dragSource = null;
        private int m_executionContextHandle;
        private IExecution m_executionInterface;
        private bool m_internal = false;
        private Font m_programBoldFont = null;
        private SolidBrush m_programBrush = null;
        private Font m_programSmallFont = null;
        private NumericUpDown numericUpDownCrossFade;
        private OpenFileDialog openFileDialog;
        private PictureBox pictureBoxRun;
        private PictureBox pictureBoxRunProgram;
        private PictureBox pictureBoxStop;
        private RadioButton radioButtonProfile;
        private RadioButton radioButtonProgramPlugin;
        private RadioButton radioButtonSequencePlugin;
        private System.Windows.Forms.Timer timer;

        public ProgramManagerDialog(Host host)
        {
            ProgressDialog dialog = new ProgressDialog();
            this.InitializeComponent();
            this.pictureBoxRunProgram.Image = this.pictureBoxRun.Image;
            this.m_executionInterface = (IExecution) Interfaces.Available["IExecution"];
            this.m_executionContextHandle = this.m_executionInterface.RequestContext(true, false, null);
            this.m_executionInterface.SetSynchronousProgramChangeHandler(this.m_executionContextHandle, new ProgramChangeHandler(this.ProgramChanged));
            this.m_programBrush = new SolidBrush(Color.FromArgb(0xc2, 0xd3, 0xfc));
            this.m_programBoldFont = new Font(this.listBoxPrograms.Font.FontFamily, 12f, FontStyle.Bold);
            this.m_programSmallFont = new Font(this.listBoxPrograms.Font.FontFamily, 8f);
            dialog.Show();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                foreach (string str in Directory.GetFiles(Paths.SequencePath))
                {
                    dialog.Message = "Loading sequence " + Path.GetFileNameWithoutExtension(str);
                    try
                    {
                        this.listBoxSequences.Items.Add(new EventSequenceStub(str, false));
                    }
                    catch
                    {
                    }
                }
                dialog.Hide();
                foreach (string str in Directory.GetFiles(Paths.ProgramPath, "*.vpr"))
                {
                    SequenceProgram item = new SequenceProgram(str);
                    this.listBoxPrograms.Items.Add(item);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                GC.Collect();
            }
        }

        private void addEditRemove_AddClick(object sender, EventArgs e)
        {
            TextQueryDialog dialog = new TextQueryDialog("New Program", "Name for the new program", string.Empty);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SequenceProgram item = new SequenceProgram();
                item.Name = dialog.Response;
                this.listBoxPrograms.Items.Add(item);
                this.m_dirty = true;
            }
            dialog.Dispose();
        }

        private void addEditRemove_RemoveClick(object sender, EventArgs e)
        {
            if (this.listBoxPrograms.SelectedItem != null)
            {
                SequenceProgram selectedItem = (SequenceProgram) this.listBoxPrograms.SelectedItem;
                if (selectedItem != null)
                {
                    this.listBoxPrograms.Items.RemoveAt(this.listBoxPrograms.SelectedIndex);
                }
                this.m_dirty = true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.SavePrograms();
        }

        private void buttonRadioAction_Click(object sender, EventArgs e)
        {
            if (this.radioButtonProgramPlugin.Checked)
            {
                SequenceProgram selectedItem = (SequenceProgram) this.listBoxPrograms.SelectedItem;
                if (selectedItem.EventSequences.Count == 0)
                {
                    MessageBox.Show("You have no sequences in this program.\nThere must be at least one sequence defined.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    this.m_dirty = true;
                    PluginListDialog dialog = new PluginListDialog(selectedItem);
                    dialog.ShowDialog();
                    dialog.Dispose();
                }
            }
            else if (this.radioButtonProfile.Checked)
            {
                this.openFileDialog.DefaultExt = ".pro";
                this.openFileDialog.Filter = "Vixen Profile|*.pro";
                this.openFileDialog.FileName = string.Empty;
                this.openFileDialog.InitialDirectory = Paths.ProfilePath;
                if (this.openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ((SequenceProgram) this.listBoxPrograms.SelectedItem).Profile = new Profile(this.openFileDialog.FileName);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.m_programBrush != null)
            {
                this.m_programBrush.Dispose();
            }
            if (this.m_programBoldFont != null)
            {
                this.m_programBoldFont.Dispose();
            }
            if (this.m_programSmallFont != null)
            {
                this.m_programSmallFont.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GetCrossFadeValue()
        {
            if (!this.m_internal && (((int) this.numericUpDownCrossFade.Value) != (this.listBoxPrograms.SelectedItem as SequenceProgram).CrossFadeLength))
            {
                (this.listBoxPrograms.SelectedItem as SequenceProgram).CrossFadeLength = (int) this.numericUpDownCrossFade.Value;
                this.m_dirty = true;
            }
        }

        private void groupBoxProgramPlugin_EnabledChanged(object sender, EventArgs e)
        {
            this.pictureBoxRunProgram.Visible = this.groupBoxProgramPlugin.Enabled;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ProgramManagerDialog));
            this.listBoxPrograms = new ListBox();
            this.listBoxProgramSequences = new ListBox();
            this.groupBox1 = new GroupBox();
            this.addEditRemove = new AddEditRemove(this.components);
            this.groupBox2 = new GroupBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.listBoxSequences = new ListBox();
            this.groupBox3 = new GroupBox();
            this.groupBoxProgramPlugin = new GroupBox();
            this.label1 = new Label();
            this.numericUpDownCrossFade = new NumericUpDown();
            this.radioButtonSequencePlugin = new RadioButton();
            this.pictureBoxRunProgram = new PictureBox();
            this.buttonRadioAction = new Button();
            this.radioButtonProfile = new RadioButton();
            this.radioButtonProgramPlugin = new RadioButton();
            this.labelProgramName = new Label();
            this.openFileDialog = new OpenFileDialog();
            this.pictureBoxRun = new PictureBox();
            this.pictureBoxStop = new PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxProgramPlugin.SuspendLayout();
            this.numericUpDownCrossFade.BeginInit();
            ((ISupportInitialize) this.pictureBoxRunProgram).BeginInit();
            ((ISupportInitialize) this.pictureBoxRun).BeginInit();
            ((ISupportInitialize) this.pictureBoxStop).BeginInit();
            base.SuspendLayout();
            this.listBoxPrograms.AllowDrop = true;
            this.listBoxPrograms.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.listBoxPrograms.BackColor = SystemColors.Window;
            this.listBoxPrograms.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBoxPrograms.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.listBoxPrograms.ItemHeight = 0x4b;
            this.listBoxPrograms.Location = new Point(6, 0x2b);
            this.listBoxPrograms.Name = "listBoxPrograms";
            this.listBoxPrograms.Size = new Size(0xdb, 0x130);
            this.listBoxPrograms.TabIndex = 0;
            this.listBoxPrograms.DragDrop += new DragEventHandler(this.listBoxPrograms_DragDrop);
            this.listBoxPrograms.DrawItem += new DrawItemEventHandler(this.listBoxPrograms_DrawItem);
            this.listBoxPrograms.MouseMove += new MouseEventHandler(this.listBoxPrograms_MouseMove);
            this.listBoxPrograms.MouseDown += new MouseEventHandler(this.listBoxPrograms_MouseDown);
            this.listBoxPrograms.KeyDown += new KeyEventHandler(this.listBoxPrograms_KeyDown);
            this.listBoxPrograms.DragOver += new DragEventHandler(this.listBoxPrograms_DragOver);
            this.listBoxProgramSequences.AllowDrop = true;
            this.listBoxProgramSequences.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxProgramSequences.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBoxProgramSequences.FormattingEnabled = true;
            this.listBoxProgramSequences.ItemHeight = 0x4b;
            this.listBoxProgramSequences.Location = new Point(6, 0x13);
            this.listBoxProgramSequences.Name = "listBoxProgramSequences";
            this.listBoxProgramSequences.Size = new Size(0xdf, 0x211);
            this.listBoxProgramSequences.TabIndex = 0;
            this.listBoxProgramSequences.DragEnter += new DragEventHandler(this.listBoxProgramSongs_DragEnter);
            this.listBoxProgramSequences.DragDrop += new DragEventHandler(this.listBoxProgramSongs_DragDrop);
            this.listBoxProgramSequences.DrawItem += new DrawItemEventHandler(this.listBoxProgramSongs_DrawItem);
            this.listBoxProgramSequences.MouseMove += new MouseEventHandler(this.listBoxProgramSongs_MouseMove);
            this.listBoxProgramSequences.KeyDown += new KeyEventHandler(this.listBoxProgramSongs_KeyDown);
            this.groupBox1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.addEditRemove);
            this.groupBox1.Controls.Add(this.listBoxPrograms);
            this.groupBox1.Location = new Point(0xdb, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xe7, 0x16b);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Programs";
            this.addEditRemove.EditVisible = false;
            this.addEditRemove.Location = new Point(0xad, 0x11);
            this.addEditRemove.Name = "addEditRemove";
            this.addEditRemove.RemoveEnabled = false;
            this.addEditRemove.Size = new Size(0x34, 20);
            this.addEditRemove.TabIndex = 8;
            this.addEditRemove.AddClick += new EventHandler(this.addEditRemove_AddClick);
            this.addEditRemove.RemoveClick += new EventHandler(this.addEditRemove_RemoveClick);
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.listBoxProgramSequences);
            this.groupBox2.Location = new Point(0x1d0, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xeb, 0x234);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Program event sequences";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x21f, 0x246);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x270, 0x246);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.listBoxSequences.AllowDrop = true;
            this.listBoxSequences.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxSequences.FormattingEnabled = true;
            this.listBoxSequences.HorizontalScrollbar = true;
            this.listBoxSequences.Location = new Point(12, 0x13);
            this.listBoxSequences.Name = "listBoxSequences";
            this.listBoxSequences.Size = new Size(0xa8, 0x20c);
            this.listBoxSequences.TabIndex = 0;
            this.listBoxSequences.DragEnter += new DragEventHandler(this.listBoxSongs_DragEnter);
            this.listBoxSequences.DragDrop += new DragEventHandler(this.listBoxSongs_DragDrop);
            this.listBoxSequences.MouseMove += new MouseEventHandler(this.listBoxSongs_MouseMove);
            this.groupBox3.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox3.Controls.Add(this.listBoxSequences);
            this.groupBox3.Location = new Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0xc2, 0x234);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Available sequences";
            this.groupBoxProgramPlugin.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.groupBoxProgramPlugin.Controls.Add(this.label1);
            this.groupBoxProgramPlugin.Controls.Add(this.numericUpDownCrossFade);
            this.groupBoxProgramPlugin.Controls.Add(this.radioButtonSequencePlugin);
            this.groupBoxProgramPlugin.Controls.Add(this.pictureBoxRunProgram);
            this.groupBoxProgramPlugin.Controls.Add(this.buttonRadioAction);
            this.groupBoxProgramPlugin.Controls.Add(this.radioButtonProfile);
            this.groupBoxProgramPlugin.Controls.Add(this.radioButtonProgramPlugin);
            this.groupBoxProgramPlugin.Controls.Add(this.labelProgramName);
            this.groupBoxProgramPlugin.Enabled = false;
            this.groupBoxProgramPlugin.Location = new Point(0xdb, 0x17d);
            this.groupBoxProgramPlugin.Name = "groupBoxProgramPlugin";
            this.groupBoxProgramPlugin.Size = new Size(0xe7, 0xc3);
            this.groupBoxProgramPlugin.TabIndex = 5;
            this.groupBoxProgramPlugin.TabStop = false;
            this.groupBoxProgramPlugin.EnabledChanged += new EventHandler(this.groupBoxProgramPlugin_EnabledChanged);
            this.label1.Location = new Point(0x40, 0x94);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x8f, 0x1f);
            this.label1.TabIndex = 7;
            this.label1.Text = "second(s) of cross-fade between sequences";
            this.numericUpDownCrossFade.Location = new Point(10, 0x9a);
            this.numericUpDownCrossFade.Name = "numericUpDownCrossFade";
            this.numericUpDownCrossFade.Size = new Size(0x30, 20);
            this.numericUpDownCrossFade.TabIndex = 6;
            this.numericUpDownCrossFade.ValueChanged += new EventHandler(this.numericUpDownCrossFade_ValueChanged);
            this.numericUpDownCrossFade.Leave += new EventHandler(this.numericUpDownCrossFade_Leave);
            this.radioButtonSequencePlugin.AutoSize = true;
            this.radioButtonSequencePlugin.Location = new Point(10, 0x55);
            this.radioButtonSequencePlugin.Name = "radioButtonSequencePlugin";
            this.radioButtonSequencePlugin.Size = new Size(0xbc, 0x11);
            this.radioButtonSequencePlugin.TabIndex = 3;
            this.radioButtonSequencePlugin.TabStop = true;
            this.radioButtonSequencePlugin.Text = "Use each sequence's plugin setup";
            this.radioButtonSequencePlugin.UseVisualStyleBackColor = true;
            this.radioButtonSequencePlugin.CheckedChanged += new EventHandler(this.radioButtonProgramPlugin_CheckedChanged);
            this.pictureBoxRunProgram.Location = new Point(0xc9, 0x10);
            this.pictureBoxRunProgram.Name = "pictureBoxRunProgram";
            this.pictureBoxRunProgram.Size = new Size(0x10, 0x10);
            this.pictureBoxRunProgram.TabIndex = 4;
            this.pictureBoxRunProgram.TabStop = false;
            this.pictureBoxRunProgram.Visible = false;
            this.pictureBoxRunProgram.Click += new EventHandler(this.pictureBoxRunProgram_Click);
            this.buttonRadioAction.Location = new Point(10, 0x70);
            this.buttonRadioAction.Name = "buttonRadioAction";
            this.buttonRadioAction.Size = new Size(0x58, 0x17);
            this.buttonRadioAction.TabIndex = 4;
            this.buttonRadioAction.UseVisualStyleBackColor = true;
            this.buttonRadioAction.Click += new EventHandler(this.buttonRadioAction_Click);
            this.radioButtonProfile.AutoSize = true;
            this.radioButtonProfile.Location = new Point(10, 0x3d);
            this.radioButtonProfile.Name = "radioButtonProfile";
            this.radioButtonProfile.Size = new Size(0x54, 0x11);
            this.radioButtonProfile.TabIndex = 2;
            this.radioButtonProfile.Text = "Use a profile";
            this.radioButtonProfile.UseVisualStyleBackColor = true;
            this.radioButtonProfile.CheckedChanged += new EventHandler(this.radioButtonProgramPlugin_CheckedChanged);
            this.radioButtonProgramPlugin.AutoSize = true;
            this.radioButtonProgramPlugin.Location = new Point(10, 0x25);
            this.radioButtonProgramPlugin.Name = "radioButtonProgramPlugin";
            this.radioButtonProgramPlugin.Size = new Size(0x98, 0x11);
            this.radioButtonProgramPlugin.TabIndex = 1;
            this.radioButtonProgramPlugin.Text = "Use program's plugin setup";
            this.radioButtonProgramPlugin.UseVisualStyleBackColor = true;
            this.radioButtonProgramPlugin.CheckedChanged += new EventHandler(this.radioButtonProgramPlugin_CheckedChanged);
            this.labelProgramName.AutoSize = true;
            this.labelProgramName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelProgramName.Location = new Point(7, 0x11);
            this.labelProgramName.Name = "labelProgramName";
            this.labelProgramName.Size = new Size(0x7c, 13);
            this.labelProgramName.TabIndex = 0;
            this.labelProgramName.Text = "No program selected";
            this.pictureBoxRun.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.pictureBoxRun.Image = (Image) manager.GetObject("pictureBoxRun.Image");
            this.pictureBoxRun.Location = new Point(12, 0x249);
            this.pictureBoxRun.Name = "pictureBoxRun";
            this.pictureBoxRun.Size = new Size(0x10, 0x10);
            this.pictureBoxRun.TabIndex = 6;
            this.pictureBoxRun.TabStop = false;
            this.pictureBoxRun.Visible = false;
            this.pictureBoxStop.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.pictureBoxStop.Image = (Image) manager.GetObject("pictureBoxStop.Image");
            this.pictureBoxStop.Location = new Point(0x26, 0x249);
            this.pictureBoxStop.Name = "pictureBoxStop";
            this.pictureBoxStop.Size = new Size(0x10, 0x10);
            this.pictureBoxStop.TabIndex = 7;
            this.pictureBoxStop.TabStop = false;
            this.pictureBoxStop.Visible = false;
            this.timer.Tick += new EventHandler(this.timer_Tick);
            this.AllowDrop = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x2c5, 0x269);
            base.Controls.Add(this.pictureBoxStop);
            base.Controls.Add(this.pictureBoxRun);
            base.Controls.Add(this.groupBoxProgramPlugin);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Name = "ProgramManagerDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Program Management";
            base.DragDrop += new DragEventHandler(this.ProgramManagerDialog_DragDrop);
            base.FormClosing += new FormClosingEventHandler(this.ProgramManagerDialog_FormClosing);
            base.DragEnter += new DragEventHandler(this.ProgramManagerDialog_DragEnter);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBoxProgramPlugin.ResumeLayout(false);
            this.groupBoxProgramPlugin.PerformLayout();
            this.numericUpDownCrossFade.EndInit();
            ((ISupportInitialize) this.pictureBoxRunProgram).EndInit();
            ((ISupportInitialize) this.pictureBoxRun).EndInit();
            ((ISupportInitialize) this.pictureBoxStop).EndInit();
            base.ResumeLayout(false);
        }

        private void listBoxPrograms_DragDrop(object sender, DragEventArgs e)
        {
            int num = this.listBoxPrograms.IndexFromPoint(this.listBoxPrograms.PointToClient(new Point(e.X, e.Y)));
            SequenceProgram program = (SequenceProgram) this.listBoxPrograms.Items[num];
            EventSequenceStub data = (EventSequenceStub) e.Data.GetData("Vixen.EventSequenceStub");
            program.EventSequences.Add(data);
            data.RetrieveSequence();
            if (num == this.listBoxPrograms.SelectedIndex)
            {
                this.listBoxProgramSequences.Items.Add(data);
                this.RefreshProgramListBox();
            }
            else
            {
                this.listBoxPrograms.SelectedIndex = num;
            }
            this.m_dirty = true;
        }

        private void listBoxPrograms_DragOver(object sender, DragEventArgs e)
        {
            int num = this.listBoxPrograms.IndexFromPoint(this.listBoxPrograms.PointToClient(new Point(e.X, e.Y)));
            e.Effect = ((e.Data.GetDataPresent("Vixen.EventSequenceStub") && (sender != this.m_dragSource)) && (num != -1)) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void listBoxPrograms_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                Brush brush = ((e.State & DrawItemState.Selected) > DrawItemState.None) ? this.m_programBrush : Brushes.White;
                e.Graphics.FillRectangle(brush, (int) (e.Bounds.Left + 2), (int) (e.Bounds.Top + 2), (int) (e.Bounds.Width - 4), (int) (e.Bounds.Height - 4));
                SequenceProgram program = (SequenceProgram) this.listBoxPrograms.Items[e.Index];
                if ((e.State & DrawItemState.Selected) > DrawItemState.None)
                {
                    e.Graphics.DrawString(program.Name, this.m_programBoldFont, Brushes.Blue, (float) (e.Bounds.Left + 5), (float) (e.Bounds.Top + 7));
                    e.Graphics.DrawString(string.Format("Length: {0}:{1:d2}", program.Length / 0xea60, (program.Length % 0xea60) / 0x3e8), this.m_programSmallFont, Brushes.Black, (float) (e.Bounds.Left + 10), (float) (e.Bounds.Top + 0x21));
                    int count = program.EventSequences.Count;
                    if (count == 0)
                    {
                        e.Graphics.DrawString("Empty", this.m_programSmallFont, Brushes.Black, (float) (e.Bounds.Left + 10), (float) (e.Bounds.Top + 50));
                    }
                    else if (count == 1)
                    {
                        e.Graphics.DrawString("1 sequence", this.m_programSmallFont, Brushes.Black, (float) (e.Bounds.Left + 10), (float) (e.Bounds.Top + 50));
                    }
                    else
                    {
                        e.Graphics.DrawString(count.ToString() + " sequences", this.m_programSmallFont, Brushes.Black, (float) (e.Bounds.Left + 10), (float) (e.Bounds.Top + 50));
                    }
                }
                else
                {
                    e.Graphics.DrawString((sender as ListBox).Items[e.Index].ToString(), this.m_programBoldFont, Brushes.Black, (float) (e.Bounds.Left + 5), (float) (e.Bounds.Top + 7));
                }
            }
        }

        private void listBoxPrograms_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.listBoxPrograms.SelectedItem != null)
            {
                if ((((SequenceProgram) this.listBoxPrograms.SelectedItem) != null) && (e.KeyCode == Keys.Delete))
                {
                    this.listBoxPrograms.Items.RemoveAt(this.listBoxPrograms.SelectedIndex);
                }
                this.m_dirty = true;
            }
        }

        private void listBoxPrograms_MouseDown(object sender, MouseEventArgs e)
        {
            int num = this.listBoxPrograms.IndexFromPoint(e.X, e.Y);
            this.listBoxProgramSequences.BeginUpdate();
            this.listBoxProgramSequences.Items.Clear();
            this.m_internal = true;
            if (num != -1)
            {
                SequenceProgram program = (SequenceProgram) this.listBoxPrograms.Items[num];
                this.listBoxProgramSequences.Items.AddRange(program.EventSequences.ToArray());
                if (program.UseSequencePluginData)
                {
                    this.radioButtonSequencePlugin.Checked = true;
                }
                else if (program.Profile == null)
                {
                    this.radioButtonProgramPlugin.Checked = true;
                }
                else
                {
                    this.radioButtonProfile.Checked = true;
                }
                this.labelProgramName.Text = program.Name;
                this.numericUpDownCrossFade.Value = program.CrossFadeLength;
            }
            else
            {
                this.labelProgramName.Text = "No program selected";
                this.numericUpDownCrossFade.Value = 0M;
            }
            this.m_internal = false;
            this.listBoxProgramSequences.EndUpdate();
            this.groupBoxProgramPlugin.Enabled = this.addEditRemove.RemoveEnabled = this.listBoxPrograms.SelectedIndex != -1;
        }

        private void listBoxPrograms_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (this.listBoxPrograms.SelectedItem != null))
            {
                this.m_dragSource = sender;
                base.DoDragDrop((SequenceProgram) this.listBoxPrograms.SelectedItem, DragDropEffects.Move);
            }
        }

        private void listBoxProgramSongs_DragDrop(object sender, DragEventArgs e)
        {
            EventSequenceStub data;
            if (this.m_dragSource == this.listBoxProgramSequences)
            {
                data = (EventSequenceStub) e.Data.GetData("Vixen.EventSequenceStub");
                int index = this.listBoxProgramSequences.Items.IndexOf(data);
                int num2 = this.listBoxProgramSequences.IndexFromPoint(this.listBoxProgramSequences.PointToClient(new Point(e.X, e.Y)));
                if (num2 == -1)
                {
                    num2 = this.listBoxProgramSequences.Items.Count - 1;
                }
                if (index == num2)
                {
                    return;
                }
                SequenceProgram selectedItem = (SequenceProgram) this.listBoxPrograms.SelectedItem;
                this.listBoxProgramSequences.Items.RemoveAt(index);
                selectedItem.EventSequences.RemoveAt(index);
                this.listBoxProgramSequences.Items.Insert(num2, data);
                selectedItem.EventSequences.Insert(num2, data);
            }
            else if (this.m_dragSource == this.listBoxSequences)
            {
                data = (EventSequenceStub) e.Data.GetData("Vixen.EventSequenceStub");
                ((SequenceProgram) this.listBoxPrograms.SelectedItem).EventSequences.Add(data);
                this.listBoxProgramSequences.Items.Add(data);
                data.RetrieveSequence();
            }
            this.RefreshProgramListBox();
        }

        private void listBoxProgramSongs_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = ((e.Data.GetDataPresent("Vixen.EventSequenceStub") && ((this.m_dragSource == this.listBoxSequences) || (this.m_dragSource == this.listBoxProgramSequences))) && (this.listBoxPrograms.SelectedItem != null)) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void listBoxProgramSongs_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                Brush brush = ((e.State & DrawItemState.Selected) > DrawItemState.None) ? this.m_programBrush : Brushes.White;
                RectangleF rect = new RectangleF((float) (e.Bounds.Left + 2), (float) (e.Bounds.Top + 2), (float) (e.Bounds.Width - 4), (float) (e.Bounds.Height - 4));
                e.Graphics.FillRectangle(brush, rect);
                EventSequenceStub stub = (EventSequenceStub) this.listBoxProgramSequences.Items[e.Index];
                if ((e.State & DrawItemState.Selected) > DrawItemState.None)
                {
                    rect.X += 3f;
                    rect.Width -= 3f;
                    rect.Y += 5f;
                    rect.Height -= 5f;
                    e.Graphics.DrawString(stub.Name, this.m_programBoldFont, Brushes.Blue, rect, this.m_clipFormat);
                    e.Graphics.DrawString("Length: " + stub.LengthString, this.m_programSmallFont, Brushes.Black, (float) (e.Bounds.Left + 10), (float) (e.Bounds.Top + 0x21));
                    if (stub.AudioName != string.Empty)
                    {
                        rect.X += 5f;
                        rect.Width -= 5f;
                        rect.Y += 43f;
                        rect.Height -= 45f;
                        e.Graphics.DrawString("Audio: " + stub.AudioName, this.m_programSmallFont, Brushes.Black, rect, this.m_clipFormat);
                    }
                }
                else
                {
                    rect.X += 3f;
                    rect.Width -= 3f;
                    rect.Y += 5f;
                    rect.Height -= 5f;
                    e.Graphics.DrawString(stub.Name, this.m_programBoldFont, Brushes.Black, rect, this.m_clipFormat);
                }
            }
        }

        private void listBoxProgramSongs_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.listBoxProgramSequences.SelectedItem != null)
            {
                EventSequenceStub selectedItem = (EventSequenceStub) this.listBoxProgramSequences.SelectedItem;
                if ((selectedItem != null) && (e.KeyCode == Keys.Delete))
                {
                    this.RemoveSongFromProgram(selectedItem);
                }
            }
        }

        private void listBoxProgramSongs_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (this.listBoxProgramSequences.SelectedItem != null))
            {
                this.m_dragSource = sender;
                base.DoDragDrop((EventSequenceStub) this.listBoxProgramSequences.SelectedItem, DragDropEffects.Move);
            }
        }

        private void listBoxSongs_DragDrop(object sender, DragEventArgs e)
        {
            this.RemoveSongFromProgram((EventSequenceStub) this.listBoxProgramSequences.SelectedItem);
        }

        private void listBoxSongs_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent("Vixen.EventSequenceStub") && (this.m_dragSource == this.listBoxProgramSequences)) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void listBoxSongs_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (this.listBoxSequences.SelectedItem != null))
            {
                this.m_dragSource = sender;
                base.DoDragDrop((EventSequenceStub) this.listBoxSequences.SelectedItem, DragDropEffects.Move);
            }
        }

        private void numericUpDownCrossFade_Leave(object sender, EventArgs e)
        {
            this.GetCrossFadeValue();
        }

        private void numericUpDownCrossFade_ValueChanged(object sender, EventArgs e)
        {
            this.GetCrossFadeValue();
        }

        private void pictureBoxRunProgram_Click(object sender, EventArgs e)
        {
            SequenceProgram selectedItem = this.listBoxPrograms.SelectedItem as SequenceProgram;
            if (selectedItem.UseSequencePluginData)
            {
                foreach (EventSequenceStub stub in selectedItem.EventSequences)
                {
                    if (!this.VerifyOutputPlugins(stub.Sequence.PlugInData))
                    {
                        MessageBox.Show(string.Format("Sequence \"{0}\" does not have any output plugins setup and/or enabled.", stub.Name), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                }
            }
            else if (!this.VerifyOutputPlugins(selectedItem.PlugInData))
            {
                MessageBox.Show("Program does not have any output plugins setup and/or enabled.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            this.GetCrossFadeValue();
            if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle) != 0)
            {
                this.m_executionInterface.ExecuteStop(this.m_executionContextHandle);
                this.pictureBoxRunProgram.Image = this.pictureBoxRun.Image;
            }
            else
            {
                this.m_executionInterface.SetSynchronousContext(this.m_executionContextHandle, this.listBoxPrograms.SelectedItem as SequenceProgram);
                if (this.m_executionInterface.ExecutePlay(this.m_executionContextHandle))
                {
                    this.pictureBoxRunProgram.Image = this.pictureBoxStop.Image;
                }
            }
        }

        private void ProgramChanged(ProgramChange changeType)
        {
            switch (changeType)
            {
                case ProgramChange.End:
                    this.pictureBoxRunProgram.Image = this.pictureBoxRun.Image;
                    break;
            }
        }

        private void ProgramManagerDialog_DragDrop(object sender, DragEventArgs e)
        {
            if (this.m_dragSource == this.listBoxPrograms)
            {
                this.listBoxPrograms.Items.RemoveAt(this.listBoxPrograms.SelectedIndex);
            }
            else if (this.m_dragSource == this.listBoxProgramSequences)
            {
                this.RemoveSongFromProgram((EventSequenceStub) this.listBoxProgramSequences.SelectedItem);
            }
            this.m_dirty = true;
        }

        private void ProgramManagerDialog_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (this.m_dragSource != this.listBoxSequences) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void ProgramManagerDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.m_dirty)
            {
                switch (MessageBox.Show("Save changes?", Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;

                    case DialogResult.Yes:
                        this.SavePrograms();
                        break;
                }
            }
            if (!e.Cancel)
            {
                this.m_executionInterface.ReleaseContext(this.m_executionContextHandle);
                foreach (SequenceProgram program in this.listBoxPrograms.Items)
                {
                    program.Dispose();
                }
            }
        }

        private void radioButtonProgramPlugin_CheckedChanged(object sender, EventArgs e)
        {
            this.m_dirty = true;
            if (this.radioButtonProgramPlugin.Checked)
            {
                this.buttonRadioAction.Text = "Plugin Setup";
                this.buttonRadioAction.Visible = true;
                ((SequenceProgram) this.listBoxPrograms.SelectedItem).Profile = null;
            }
            else if (this.radioButtonProfile.Checked)
            {
                this.buttonRadioAction.Text = "Select Profile";
                this.buttonRadioAction.Visible = true;
            }
            else if (this.radioButtonSequencePlugin.Checked)
            {
                this.buttonRadioAction.Visible = false;
                ((SequenceProgram) this.listBoxPrograms.SelectedItem).Profile = null;
            }
            ((SequenceProgram) this.listBoxPrograms.SelectedItem).UseSequencePluginData = this.radioButtonSequencePlugin.Checked;
        }

        private void RefreshProgramListBox()
        {
            this.listBoxPrograms.BeginUpdate();
            this.listBoxPrograms.Refresh();
            this.listBoxPrograms.EndUpdate();
            this.m_dirty = true;
        }

        private void RemoveSongFromProgram(EventSequenceStub eventSequenceStub)
        {
            this.listBoxProgramSequences.Items.Remove(eventSequenceStub);
            ((SequenceProgram) this.listBoxPrograms.SelectedItem).EventSequences.Remove(eventSequenceStub);
            this.RefreshProgramListBox();
        }

        private void SavePrograms()
        {
            foreach (string str in Directory.GetFiles(Paths.ProgramPath))
            {
                File.Copy(str, Path.ChangeExtension(str, ".bak"));
            }
            string filePath = null;
            foreach (SequenceProgram program in this.listBoxPrograms.Items)
            {
                try
                {
                    filePath = Path.Combine(Paths.ProgramPath, program.Name + ".vpr");
                    program.SaveTo(filePath);
                    File.Delete(Path.ChangeExtension(filePath, ".bak"));
                }
                catch (Exception exception)
                {
                    string sourceFileName = Path.ChangeExtension(filePath, ".bak");
                    try
                    {
                        File.Copy(sourceFileName, filePath);
                        sourceFileName = null;
                    }
                    catch
                    {
                    }
                    foreach (string str4 in Directory.GetFiles(Paths.ProgramPath, "*.bak"))
                    {
                        if (str4 != sourceFileName)
                        {
                            File.Delete(str4);
                        }
                    }
                    if (sourceFileName == null)
                    {
                        MessageBox.Show(string.Format("Error while saving programs:\n\n{0}\n\nAffected file was successfully restored.", exception.Message), "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Error while saving programs:\n\n{0}\n\nAffected file could not be restored.  It was backed up to {1}.", exception.Message, sourceFileName), "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
        }

        private bool VerifyOutputPlugins(SetupData setupData)
        {
            return (setupData.GetAllPluginData(SetupData.PluginType.Output, true).Count > 0);
        }
    }
}

