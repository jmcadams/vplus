namespace StandardScript
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TabPageDoc : TabPage
    {
        private IContainer components = null;
        private Point m_caretPosition;
        private bool m_dirty = false;
        private string m_displayName;
        private string m_fileName;
        private bool m_newFile;
        private int m_newFileCount = 0;
        private RichTextBox m_textBox;

        public static  event EventHandler DirtyChanged;

        public event OnPositionChange PositionChange;

        public TabPageDoc(string fileName)
        {
            this.InitializeComponent();
            this.m_textBox = new RichTextBox();
            this.m_textBox.SuspendLayout();
            base.SuspendLayout();
            this.m_textBox.AcceptsTab = true;
            this.m_textBox.BorderStyle = BorderStyle.None;
            this.m_textBox.Dock = DockStyle.Fill;
            this.m_textBox.Font = new Font("Courier New", 10f);
            this.m_textBox.Location = new Point(0, 0);
            this.m_textBox.Name = "tabPageRichTextBox";
            this.m_textBox.Size = new Size(0x239, 0x1a3);
            this.m_textBox.TabIndex = 0;
            this.m_textBox.WordWrap = false;
            this.m_textBox.ScrollBars = RichTextBoxScrollBars.Both;
            this.m_textBox.TextChanged += new EventHandler(this.m_textBox_TextChanged);
            this.m_textBox.KeyUp += new KeyEventHandler(this.m_textBox_KeyUp);
            this.m_textBox.MouseDown += new MouseEventHandler(this.m_textBox_MouseDown);
            base.Controls.Add(this.m_textBox);
            this.m_textBox.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
            if (fileName == string.Empty)
            {
                this.Text = this.m_displayName = this.m_fileName = "untitled" + this.m_newFileCount.ToString();
                this.m_newFile = true;
                this.m_newFileCount++;
            }
            else
            {
                this.SetFileName(fileName);
                this.m_textBox.LoadFile(fileName, RichTextBoxStreamType.PlainText);
                this.m_newFile = false;
            }
            this.m_caretPosition = new Point();
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
            base.ResumeLayout(false);
        }

        private void m_textBox_KeyUp(object sender, KeyEventArgs e)
        {
            this.UpdatePosition();
        }

        private void m_textBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.UpdatePosition();
        }

        private void m_textBox_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = this.m_textBox.UndoActionName != string.Empty;
        }

        protected virtual void OnDirtyChanged(EventArgs e)
        {
            if (DirtyChanged != null)
            {
                DirtyChanged(this, e);
            }
        }

        public void SaveTo(string path)
        {
            if (this.m_fileName == string.Empty)
            {
                throw new Exception("Document has no file name.");
            }
            this.m_textBox.SaveFile(Path.Combine(path, this.m_fileName), RichTextBoxStreamType.PlainText);
            this.IsDirty = false;
        }

        private void SetFileName(string fileName)
        {
            this.Text = this.m_fileName = this.m_displayName = Path.GetFileName(fileName);
        }

        private void UpdatePosition()
        {
            int firstCharIndexOfCurrentLine = this.m_textBox.GetFirstCharIndexOfCurrentLine();
            this.m_caretPosition.Y = this.m_textBox.GetLineFromCharIndex(firstCharIndexOfCurrentLine);
            this.m_caretPosition.X = this.m_textBox.SelectionStart - firstCharIndexOfCurrentLine;
            if (this.PositionChange != null)
            {
                this.PositionChange(this.m_caretPosition);
            }
        }

        public string DisplayName
        {
            get
            {
                return this.m_displayName;
            }
        }

        public string FileName
        {
            get
            {
                return this.m_fileName;
            }
            set
            {
                this.SetFileName(value);
                this.m_newFile = false;
            }
        }

        public bool IsDirty
        {
            get
            {
                return this.m_dirty;
            }
            set
            {
                this.Text = string.Format("{0}{1}", this.m_displayName, value ? "*" : string.Empty);
                this.m_dirty = value;
                this.OnDirtyChanged(EventArgs.Empty);
            }
        }

        public bool IsNew
        {
            get
            {
                return this.m_newFile;
            }
        }

        public Point Position
        {
            get
            {
                return this.m_caretPosition;
            }
        }

        public RichTextBox TextBox
        {
            get
            {
                return this.m_textBox;
            }
        }

        public delegate void OnPositionChange(Point pt);
    }
}

