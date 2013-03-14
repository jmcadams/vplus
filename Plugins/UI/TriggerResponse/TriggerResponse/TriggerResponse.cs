namespace TriggerResponse
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class TriggerResponse : UIBase
    {
        private Label labelStatus;
        private ListBox listBoxTriggers;
        private AddEditRemove m_addEditRemove;
        private string[] m_audioDevices;
        private XmlNode m_dataNode;
        private int m_fontHeight;
        private Bitmap m_greenArrowBitmap;
        private const int m_gutter = 10;
        private Preference2 m_preferences;
        private Bitmap m_redArrowBitmap;
        private SolidBrush m_selectedBrush;
        private EventSequence m_sequence;
        private ISystem m_systemInterface;
        private ITrigger m_triggerInterface;
        private OpenFileDialog openFileDialog;
        private PictureBox pictureBoxGreenArrow;
        private PictureBox pictureBoxRedArrow;
        private SaveFileDialog saveFileDialog;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private ToolStripButton toolStripButtonExecute;
        private ToolStripButton toolStripButtonSave;
        private ToolStripButton toolStripButtonStop;
        private ToolStripButton toolStripButtonTest;
        private ToolStripContainer toolStripContainer1;
        private ToolStripSeparator toolStripSeparator2;

        public TriggerResponse()
        {
            object obj2;
            this.m_fontHeight = 0;
            this.m_systemInterface = null;
            this.InitializeComponent();
            this.m_addEditRemove = new AddEditRemove();
            this.toolStripContainer1.ContentPanel.Controls.Add(this.m_addEditRemove);
            this.m_addEditRemove.Location = new Point(12, 6);
            this.m_addEditRemove.Name = "addEditRemove1";
            this.m_addEditRemove.AddClick += new EventHandler(this.m_addEditRemove_AddClick);
            this.m_addEditRemove.EditClick += new EventHandler(this.m_addEditRemove_EditClick);
            this.m_addEditRemove.RemoveClick += new EventHandler(this.m_addEditRemove_RemoveClick);
            this.m_addEditRemove.EditEnabled = false;
            this.m_addEditRemove.RemoveEnabled = false;
            if (Interfaces.Available.TryGetValue("ITrigger", out obj2))
            {
                this.m_triggerInterface = (ITrigger) obj2;
            }
            if (Interfaces.Available.TryGetValue("ISystem", out obj2))
            {
                this.m_systemInterface = (ISystem) obj2;
            }
            this.m_selectedBrush = new SolidBrush(Color.FromArgb(0xdd, 0xdd, 0xff));
            this.m_greenArrowBitmap = new Bitmap(this.pictureBoxGreenArrow.Image);
            this.m_greenArrowBitmap.MakeTransparent(this.m_greenArrowBitmap.GetPixel(0, 0));
            this.m_redArrowBitmap = new Bitmap(this.pictureBoxRedArrow.Image);
            this.m_redArrowBitmap.MakeTransparent(this.m_redArrowBitmap.GetPixel(0, 0));
            this.saveFileDialog.InitialDirectory = this.openFileDialog.InitialDirectory = Paths.SequencePath;
            this.saveFileDialog.Filter = this.openFileDialog.Filter = string.Format("{0}|*{1}", this.FileTypeDescription, this.FileExtension);
            this.m_audioDevices = this.m_systemInterface.AudioDevices;
            base.IsDirty = true;
        }

        protected override void Dispose(bool disposing)
        {
            this.m_greenArrowBitmap.Dispose();
            this.m_redArrowBitmap.Dispose();
            base.Dispose(disposing);
        }

        private void EditTriggerResponse()
        {
            List<ITriggerPlugin> triggers = this.GetTriggers();
            if (triggers.Count == 0)
            {
                MessageBox.Show("There are no trigger plugins available for use.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                TriggerEditDialog dialog = new TriggerEditDialog(triggers, this.m_audioDevices);
                dialog.SelectedItem = (MappedTriggerResponse) this.listBoxTriggers.SelectedItem;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.listBoxTriggers.Refresh();
                }
                dialog.Dispose();
            }
        }

        private List<ITriggerPlugin> GetTriggers()
        {
            List<ITriggerPlugin> list = new List<ITriggerPlugin>();
            foreach (ILoadable loadable in this.m_systemInterface.LoadableList("ITriggerPlugin"))
            {
                list.Add((ITriggerPlugin) loadable);
            }
            return list;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(TriggerResponse.TriggerResponse));
            this.toolStripContainer1 = new ToolStripContainer();
            this.pictureBoxRedArrow = new PictureBox();
            this.labelStatus = new Label();
            this.pictureBoxGreenArrow = new PictureBox();
            this.listBoxTriggers = new ListBox();
            this.toolStrip1 = new ToolStrip();
            this.toolStripButtonSave = new ToolStripButton();
            this.toolStrip2 = new ToolStrip();
            this.toolStripButtonExecute = new ToolStripButton();
            this.toolStripButtonStop = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.toolStripButtonTest = new ToolStripButton();
            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxRedArrow).BeginInit();
            ((ISupportInitialize) this.pictureBoxGreenArrow).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            base.SuspendLayout();
            this.toolStripContainer1.ContentPanel.BackColor = Color.White;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.pictureBoxRedArrow);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.labelStatus);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.pictureBoxGreenArrow);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.listBoxTriggers);
            this.toolStripContainer1.ContentPanel.Size = new Size(0x14d, 0x178);
            this.toolStripContainer1.Dock = DockStyle.Fill;
            this.toolStripContainer1.Location = new Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new Size(0x14d, 0x191);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
            this.pictureBoxRedArrow.Image = (Image) manager.GetObject("pictureBoxRedArrow.Image");
            this.pictureBoxRedArrow.Location = new Point(0xc7, 0x15f);
            this.pictureBoxRedArrow.Name = "pictureBoxRedArrow";
            this.pictureBoxRedArrow.Size = new Size(0x10, 0x10);
            this.pictureBoxRedArrow.TabIndex = 3;
            this.pictureBoxRedArrow.TabStop = false;
            this.pictureBoxRedArrow.Visible = false;
            this.labelStatus.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new Point(9, 0x157);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new Size(0, 13);
            this.labelStatus.TabIndex = 2;
            this.pictureBoxGreenArrow.Image = (Image) manager.GetObject("pictureBoxGreenArrow.Image");
            this.pictureBoxGreenArrow.Location = new Point(0xb1, 0x15f);
            this.pictureBoxGreenArrow.Name = "pictureBoxGreenArrow";
            this.pictureBoxGreenArrow.Size = new Size(0x10, 0x10);
            this.pictureBoxGreenArrow.TabIndex = 1;
            this.pictureBoxGreenArrow.TabStop = false;
            this.pictureBoxGreenArrow.Visible = false;
            this.listBoxTriggers.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxTriggers.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBoxTriggers.FormattingEnabled = true;
            this.listBoxTriggers.ItemHeight = 30;
            this.listBoxTriggers.Location = new Point(12, 0x20);
            this.listBoxTriggers.Name = "listBoxTriggers";
            this.listBoxTriggers.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxTriggers.Size = new Size(0x135, 0x112);
            this.listBoxTriggers.TabIndex = 0;
            this.listBoxTriggers.DrawItem += new DrawItemEventHandler(this.listBoxTriggers_DrawItem);
            this.listBoxTriggers.SelectedIndexChanged += new EventHandler(this.listBoxTriggers_SelectedIndexChanged);
            this.listBoxTriggers.DoubleClick += new EventHandler(this.listBoxTriggers_DoubleClick);
            this.listBoxTriggers.KeyDown += new KeyEventHandler(this.listBoxTriggers_KeyDown);
            this.toolStrip1.Dock = DockStyle.None;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripButtonSave });
            this.toolStrip1.Location = new Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x23, 0x19);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStripButtonSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = (Image) manager.GetObject("toolStripButtonSave.Image");
            this.toolStripButtonSave.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new Size(0x17, 0x16);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new EventHandler(this.toolStripButtonSave_Click);
            this.toolStrip2.Dock = DockStyle.None;
            this.toolStrip2.Items.AddRange(new ToolStripItem[] { this.toolStripButtonExecute, this.toolStripButtonStop, this.toolStripSeparator2, this.toolStripButtonTest });
            this.toolStrip2.Location = new Point(0x26, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new Size(0x61, 0x19);
            this.toolStrip2.TabIndex = 4;
            this.toolStripButtonExecute.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExecute.Image = (Image) manager.GetObject("toolStripButtonExecute.Image");
            this.toolStripButtonExecute.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonExecute.Name = "toolStripButtonExecute";
            this.toolStripButtonExecute.Size = new Size(0x17, 0x16);
            this.toolStripButtonExecute.Text = "Execute";
            this.toolStripButtonExecute.Click += new EventHandler(this.toolStripButtonExecute_Click);
            this.toolStripButtonStop.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStop.Image = (Image) manager.GetObject("toolStripButtonStop.Image");
            this.toolStripButtonStop.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new Size(0x17, 0x16);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.Click += new EventHandler(this.toolStripButtonStop_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.toolStripButtonTest.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonTest.Image = (Image) manager.GetObject("toolStripButtonTest.Image");
            this.toolStripButtonTest.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonTest.Name = "toolStripButtonTest";
            this.toolStripButtonTest.Size = new Size(0x21, 0x16);
            this.toolStripButtonTest.Text = "Test";
            this.toolStripButtonTest.ToolTipText = "Manually test responses";
            this.toolStripButtonTest.Click += new EventHandler(this.toolStripButtonTest_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.ClientSize = new Size(0x14d, 0x191);
            base.Controls.Add(this.toolStripContainer1);
            base.Name = "TriggerResponse";
            base.FormClosing += new FormClosingEventHandler(this.TriggerResponse_FormClosing);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            ((ISupportInitialize) this.pictureBoxRedArrow).EndInit();
            ((ISupportInitialize) this.pictureBoxGreenArrow).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            base.ResumeLayout(false);
        }

        private void listBoxTriggers_DoubleClick(object sender, EventArgs e)
        {
            this.EditTriggerResponse();
        }

        private void listBoxTriggers_DrawItem(object sender, DrawItemEventArgs e)
        {
            if ((e.Index != -1) && !base.Disposing)
            {
                if ((e.State & DrawItemState.Selected) > DrawItemState.None)
                {
                    e.Graphics.FillRectangle(this.m_selectedBrush, e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                }
                if (this.m_fontHeight == 0)
                {
                    this.m_fontHeight = (int) e.Graphics.MeasureString("Mg", this.listBoxTriggers.Font).Height;
                }
                int num = (((this.listBoxTriggers.Width / 2) - (this.pictureBoxGreenArrow.Width / 2)) - 10) - 10;
                MappedTriggerResponse response = (MappedTriggerResponse) this.listBoxTriggers.Items[e.Index];
                if (response.EcHandle != 0)
                {
                    e.Graphics.DrawImageUnscaled(this.m_greenArrowBitmap, (this.listBoxTriggers.Width - this.pictureBoxGreenArrow.Width) / 2, e.Bounds.Y + ((this.listBoxTriggers.ItemHeight - this.pictureBoxGreenArrow.Height) / 2));
                }
                else
                {
                    e.Graphics.DrawImageUnscaled(this.m_redArrowBitmap, (this.listBoxTriggers.Width - this.pictureBoxRedArrow.Width) / 2, e.Bounds.Y + ((this.listBoxTriggers.ItemHeight - this.pictureBoxRedArrow.Height) / 2));
                }
                RectangleF layoutRectangle = new RectangleF((float) (e.Bounds.X + 10), (float) (e.Bounds.Y + ((this.listBoxTriggers.ItemHeight - this.m_fontHeight) / 2)), (float) num, (float) this.m_fontHeight);
                e.Graphics.DrawString(response.Description, this.listBoxTriggers.Font, Brushes.Black, layoutRectangle);
                layoutRectangle.X = (this.listBoxTriggers.Width - 10) - layoutRectangle.Width;
                e.Graphics.DrawString(Path.GetFileNameWithoutExtension(response.SequenceFile), this.listBoxTriggers.Font, Brushes.Black, layoutRectangle);
            }
        }

        private void listBoxTriggers_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && (this.listBoxTriggers.SelectedIndices.Count > 0))
            {
                this.RemoveTriggerResponses();
            }
        }

        private void listBoxTriggers_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.m_addEditRemove.RemoveEnabled = this.listBoxTriggers.SelectedIndices.Count > 0;
            this.m_addEditRemove.EditEnabled = this.listBoxTriggers.SelectedIndices.Count == 1;
        }

        private void m_addEditRemove_AddClick(object sender, EventArgs e)
        {
            List<ITriggerPlugin> triggers = this.GetTriggers();
            if (triggers.Count == 0)
            {
                MessageBox.Show("There are no trigger plugins available for use.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                TriggerEditDialog dialog = new TriggerEditDialog(triggers, this.m_audioDevices);
                dialog.SelectedItem = new MappedTriggerResponse();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.listBoxTriggers.Items.Add(dialog.SelectedItem);
                }
                dialog.Dispose();
            }
        }

        private void m_addEditRemove_EditClick(object sender, EventArgs e)
        {
            this.EditTriggerResponse();
        }

        private void m_addEditRemove_RemoveClick(object sender, EventArgs e)
        {
            this.RemoveTriggerResponses();
        }

        public override EventSequence New()
        {
            return this.New(new EventSequence(this.m_systemInterface.UserPreferences));
        }

        public override EventSequence New(EventSequence seedSequence)
        {
            this.m_sequence = seedSequence;
            this.Text = this.m_sequence.Name;
            this.m_preferences = this.m_systemInterface.UserPreferences;
            this.m_dataNode = this.m_sequence.Extensions[this.FileExtension];
            this.GetTriggers();
            return this.m_sequence;
        }

        public override void Notify(Notification notification, object data)
        {
        }

        public override EventSequence Open(string filePath)
        {
            this.m_sequence = new EventSequence(filePath);
            this.Text = this.m_sequence.Name;
            this.m_dataNode = this.m_sequence.Extensions[this.FileExtension];
            this.m_preferences = this.m_systemInterface.UserPreferences;
            this.GetTriggers();
            XmlNode node = this.m_dataNode.SelectSingleNode("TriggerResponses");
            if (node != null)
            {
                foreach (XmlNode node2 in node.SelectNodes("TriggerResponse"))
                {
                    this.listBoxTriggers.Items.Add(new MappedTriggerResponse(node2));
                }
            }
            return this.m_sequence;
        }

        private void RemoveTriggerResponses()
        {
            if (MessageBox.Show("Remove selected trigger responses?", "Vixen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                List<object> list = new List<object>();
                foreach (object obj2 in this.listBoxTriggers.SelectedItems)
                {
                    list.Add(obj2);
                }
                this.listBoxTriggers.BeginUpdate();
                foreach (object obj2 in list)
                {
                    this.listBoxTriggers.Items.Remove(obj2);
                }
                this.listBoxTriggers.EndUpdate();
            }
        }

        public override DialogResult RunWizard(ref EventSequence resultSequence)
        {
            return DialogResult.None;
        }

        public override void SaveTo(string filePath)
        {
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_dataNode, "TriggerResponses");
            foreach (MappedTriggerResponse response in this.listBoxTriggers.Items)
            {
                emptyNodeAlways.AppendChild(response.SaveToXml(this.m_dataNode.OwnerDocument));
            }
            this.m_sequence.SaveTo(filePath);
            this.Text = this.m_sequence.Name;
        }

        private void Stop()
        {
            foreach (MappedTriggerResponse response in this.listBoxTriggers.Items)
            {
                this.m_triggerInterface.UnregisterResponse(response.TriggerType, response.TriggerIndex, response.EcHandle);
                response.EcHandle = 0;
            }
            this.listBoxTriggers.Refresh();
            this.labelStatus.Text = string.Empty;
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            if (this.labelStatus.Text == string.Empty)
            {
                int num = 0;
                foreach (MappedTriggerResponse response in this.listBoxTriggers.Items)
                {
                    num += ((response.EcHandle = this.m_triggerInterface.RegisterResponse(response.TriggerType, response.TriggerIndex, response.SequenceFile)) != 0) ? 1 : 0;
                }
                if (num == 1)
                {
                    this.labelStatus.Text = "1 response active";
                }
                else
                {
                    this.labelStatus.Text = string.Format("{0} responses active", num);
                }
                this.listBoxTriggers.Refresh();
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.m_systemInterface.InvokeSave(this);
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void toolStripButtonTest_Click(object sender, EventArgs e)
        {
            MappedTriggerResponse[] destination = new MappedTriggerResponse[this.listBoxTriggers.Items.Count];
            this.listBoxTriggers.Items.CopyTo(destination, 0);
            TriggerResponseTestDialog dialog = new TriggerResponseTestDialog(destination, this.m_triggerInterface);
            this.listBoxTriggers.Refresh();
            dialog.ShowDialog();
            this.listBoxTriggers.Refresh();
            dialog.Dispose();
        }

        private void TriggerResponse_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Stop();
        }

        public override string Author
        {
            get
            {
                return "K.C. Oaks";
            }
        }

        public override string Description
        {
            get
            {
                return "Vixen external trigger response user interface";
            }
        }

        public override string FileExtension
        {
            get
            {
                return ".vtr";
            }
        }

        public override string FileTypeDescription
        {
            get
            {
                return "Trigger response";
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
    }
}

