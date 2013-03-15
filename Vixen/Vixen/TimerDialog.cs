namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    internal class TimerDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private Button buttonSelectProgram;
        private Button buttonSelectSequence;
        private CheckBox checkBoxEvery;
        private CheckBox checkBoxFriday;
        private CheckBox checkBoxMonday;
        private CheckBox checkBoxRepeat;
        private CheckBox checkBoxSaturday;
        private CheckBox checkBoxSunday;
        private CheckBox checkBoxThursday;
        private CheckBox checkBoxTuesday;
        private CheckBox checkBoxWednesday;
        private ComboBox comboBoxEndTime;
        private ComboBox comboBoxRecurrenceType;
        private ComboBox comboBoxStartTime;
        private IContainer components;
        private DateTimePicker dateTimePickerExecutionStartDate;
        private DateTimePicker dateTimePickerRecurrenceEndDate;
        private DateTimePicker dateTimePickerRecurrenceStartDate;
        private DateTimePicker dateTimePickerYearlyDate;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label labelMinutes;
        private Label labelObject;
        private Label labelRecurrenceTypeExplanation;
        private Label labelUntil;
        private string m_fileTypeFilter;
        private ObjectType m_objectType;
        private string m_selectedObjectFileName;
        private TimeSpan m_selectedObjectLength;
        private Vixen.Timer m_timer;
        private OpenFileDialog openFileDialog;
        private Panel panel1;
        private Panel panel2;
        private Panel panelRecurrenceRange;
        private RadioButton radioButtonFirstDay;
        private RadioButton radioButtonLastDay;
        private RadioButton radioButtonSpecificDay;
        private Vixen.TabControl tabControl;
        private TabPage tabPageDaily;
        private TabPage tabPageMonthly;
        private TabPage tabPageNone;
        private TabPage tabPageWeekly;
        private TabPage tabPageYearly;
        private TextBox textBoxRepeatInterval;
        private TextBox textBoxSpecificDay;

        public TimerDialog(DateTime startDateTime)
        {
            this.m_timer = null;
            this.m_selectedObjectFileName = string.Empty;
            this.m_selectedObjectLength = TimeSpan.MinValue;
            this.components = null;
            this.InitializeComponent();
            this.m_fileTypeFilter = ((ISystem) Interfaces.Available["ISystem"]).KnownFileTypesFilter;
            this.comboBoxStartTime.SelectedIndex = (startDateTime.Hour * 2) + (startDateTime.Minute / 30);
            this.dateTimePickerRecurrenceStartDate.Value = this.dateTimePickerRecurrenceEndDate.Value = this.dateTimePickerExecutionStartDate.Value = startDateTime;
            this.comboBoxRecurrenceType.SelectedIndex = 0;
            this.tabControl.SelectedTab = this.tabPageNone;
        }

        public TimerDialog(Vixen.Timer timer)
        {
            this.m_timer = null;
            this.m_selectedObjectFileName = string.Empty;
            this.m_selectedObjectLength = TimeSpan.MinValue;
            this.components = null;
            this.InitializeComponent();
            this.m_fileTypeFilter = ((ISystem) Interfaces.Available["ISystem"]).KnownFileTypesFilter;
            this.dateTimePickerExecutionStartDate.Value = timer.StartDate;
            this.labelObject.Text = string.Format("{0} ({1:d2}:{2:d2})", timer.ProgramName, timer.ObjectLength.Minutes, timer.ObjectLength.Seconds);
            this.m_selectedObjectFileName = timer.ProgramFileName;
            this.m_selectedObjectLength = timer.ObjectLength;
            this.m_objectType = timer.ObjectType;
            if ((timer.StartTime.TotalMinutes % 30.0) == 0.0)
            {
                this.comboBoxStartTime.SelectedIndex = (int) (timer.StartTime.TotalMinutes / 30.0);
            }
            else
            {
                this.comboBoxStartTime.Text = timer.StartDateTime.ToShortTimeString();
            }
            if (timer.TimerLength != timer.ObjectLength)
            {
                this.checkBoxRepeat.Checked = true;
                this.PopulateEndTimeValues(DateTime.Parse(this.comboBoxStartTime.Text));
                this.comboBoxEndTime.SelectedIndex = ((int) (timer.TimerLength.TotalMinutes / 30.0)) - 1;
                if (timer.RepeatInterval != 0)
                {
                    this.checkBoxEvery.Checked = true;
                    this.textBoxRepeatInterval.Text = timer.RepeatInterval.ToString();
                }
            }
            if (timer.Recurrence == RecurrenceType.None)
            {
                this.comboBoxRecurrenceType.SelectedIndex = 0;
            }
            else
            {
                this.comboBoxRecurrenceType.SelectedIndex = (int) timer.Recurrence;
                this.dateTimePickerRecurrenceStartDate.Value = timer.RecurrenceStart;
                this.dateTimePickerRecurrenceEndDate.Value = timer.RecurrenceEnd;
                switch (timer.Recurrence)
                {
                    case RecurrenceType.Weekly:
                    {
                        int recurrenceData = (int) timer.RecurrenceData;
                        this.checkBoxSunday.Checked = (recurrenceData & 1) != 0;
                        this.checkBoxMonday.Checked = (recurrenceData & 2) != 0;
                        this.checkBoxTuesday.Checked = (recurrenceData & 4) != 0;
                        this.checkBoxWednesday.Checked = (recurrenceData & 8) != 0;
                        this.checkBoxThursday.Checked = (recurrenceData & 0x10) != 0;
                        this.checkBoxFriday.Checked = (recurrenceData & 0x20) != 0;
                        this.checkBoxSaturday.Checked = (recurrenceData & 0x40) != 0;
                        goto Label_037C;
                    }
                    case RecurrenceType.Monthly:
                    {
                        string str = (string) timer.RecurrenceData;
                        if (timer.RecurrenceData is string)
                        {
                            if (!(str == "first"))
                            {
                                this.radioButtonLastDay.Checked = true;
                            }
                            else
                            {
                                this.radioButtonFirstDay.Checked = true;
                            }
                        }
                        else
                        {
                            this.textBoxSpecificDay.Text = str;
                        }
                        goto Label_037C;
                    }
                    case RecurrenceType.Yearly:
                        this.dateTimePickerYearlyDate.Value = (DateTime) timer.RecurrenceData;
                        goto Label_037C;
                }
            }
        Label_037C:
            this.m_timer = timer;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DateTime time;
            if (this.m_timer == null)
            {
                this.m_timer = new Vixen.Timer();
            }
            StringBuilder builder = new StringBuilder();
            if (this.m_selectedObjectFileName == string.Empty)
            {
                builder.AppendLine("* Nothing has been selected to execute");
            }
            else
            {
                this.m_timer.ObjectType = this.m_objectType;
                this.m_timer.ProgramFileName = this.m_selectedObjectFileName;
            }
            this.m_timer.StartDate = this.dateTimePickerExecutionStartDate.Value;
            if (!DateTime.TryParse(this.comboBoxStartTime.Text, out time))
            {
                builder.AppendLine("* Start time is not a valid time");
            }
            else
            {
                this.m_timer.StartTime = time.TimeOfDay;
            }
            if (this.m_selectedObjectLength != TimeSpan.MinValue)
            {
                this.m_timer.ObjectLength = new TimeSpan(0, 0, 0, 0, (int) this.m_selectedObjectLength.TotalMilliseconds);
            }
            if (this.checkBoxRepeat.Checked)
            {
                this.m_timer.RepeatInterval = 0;
                TimeSpan span = (TimeSpan) (time.AddHours((double) ((this.comboBoxEndTime.SelectedIndex + 1) * 0.5f)) - time);
                if (span.TotalMinutes <= 0.0)
                {
                    builder.AppendLine("* End time needs to be after the start time");
                }
                else
                {
                    this.m_timer.TimerLength = span;
                }
                if (this.m_timer.TimerLength < this.m_timer.ObjectLength)
                {
                    this.m_timer.TimerLength = this.m_timer.ObjectLength;
                    MessageBox.Show("Timer length has been adjusted so that it's at least as long as the scheduled item.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (this.checkBoxEvery.Checked)
                {
                    try
                    {
                        this.m_timer.RepeatInterval = Convert.ToInt32(this.textBoxRepeatInterval.Text);
                    }
                    catch
                    {
                        builder.AppendLine("* The repeat interval must be 0 (for end-to-end repeating) or greater");
                    }
                    if (((this.m_timer.ObjectLength.TotalMinutes * 2.0) + this.m_timer.RepeatInterval) > this.m_timer.TimerLength.TotalMinutes)
                    {
                        this.m_timer.TimerLength = this.m_timer.ObjectLength;
                        this.m_timer.RepeatInterval = 0;
                        MessageBox.Show("Since the scheduled item would not repeat with the given interval\nand timer duration, it has been changed to a single-occurence timer", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
            else
            {
                this.m_timer.TimerLength = this.m_timer.ObjectLength;
            }
            this.m_timer.Recurrence = (RecurrenceType) Math.Max(this.comboBoxRecurrenceType.SelectedIndex, 0);
            if (this.comboBoxRecurrenceType.SelectedIndex <= 0)
            {
                this.m_timer.RecurrenceStart = DateTime.MinValue;
                this.m_timer.RecurrenceEnd = DateTime.MinValue;
            }
            else
            {
                this.m_timer.RecurrenceStart = this.dateTimePickerRecurrenceStartDate.Value.Date;
                this.m_timer.RecurrenceEnd = this.dateTimePickerRecurrenceEndDate.Value.Date.Add(new TimeSpan(0x17, 0x3b, 0));
                switch (this.m_timer.Recurrence)
                {
                    case RecurrenceType.Weekly:
                    {
                        int num = 0;
                        if (this.checkBoxSunday.Checked)
                        {
                            num |= 1;
                        }
                        if (this.checkBoxMonday.Checked)
                        {
                            num |= 2;
                        }
                        if (this.checkBoxTuesday.Checked)
                        {
                            num |= 4;
                        }
                        if (this.checkBoxWednesday.Checked)
                        {
                            num |= 8;
                        }
                        if (this.checkBoxThursday.Checked)
                        {
                            num |= 0x10;
                        }
                        if (this.checkBoxFriday.Checked)
                        {
                            num |= 0x20;
                        }
                        if (this.checkBoxSaturday.Checked)
                        {
                            num |= 0x40;
                        }
                        this.m_timer.RecurrenceData = num;
                        goto Label_054B;
                    }
                    case RecurrenceType.Monthly:
                        if (!this.radioButtonFirstDay.Checked)
                        {
                            if (this.radioButtonLastDay.Checked)
                            {
                                this.m_timer.RecurrenceData = "last";
                            }
                            else
                            {
                                int num2 = 0;
                                try
                                {
                                    num2 = Convert.ToInt32(this.textBoxSpecificDay.Text);
                                }
                                catch
                                {
                                    builder.AppendLine("* Day listed for monthly recurrence is not a valid day");
                                }
                                if ((num2 < 1) || (num2 > 0x1f))
                                {
                                    builder.AppendLine("* Day listed for monthly recurrence is not a valid day");
                                }
                                else
                                {
                                    this.m_timer.RecurrenceData = num2;
                                }
                            }
                        }
                        else
                        {
                            this.m_timer.RecurrenceData = "first";
                        }
                        goto Label_054B;

                    case RecurrenceType.Yearly:
                        this.m_timer.RecurrenceData = this.dateTimePickerYearlyDate.Value;
                        goto Label_054B;
                }
            }
        Label_054B:
            if (builder.Length > 0)
            {
                base.DialogResult = System.Windows.Forms.DialogResult.None;
                MessageBox.Show("The following items need to be corrected before\na timer can be created:\n\n" + builder.ToString(), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonSelectProgram_Click(object sender, EventArgs e)
        {
            this.openFileDialog.Filter = string.Format("{0} program|*.{1}", Vendor.ProductName, Vendor.ProgramExtension);
            this.openFileDialog.InitialDirectory = Paths.ProgramPath;
            this.openFileDialog.FileName = string.Empty;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (IScheduledObject obj2 = this.LoadProgram(this.openFileDialog.FileName))
                {
                    this.labelObject.Text = obj2.Name + string.Format(" ({0:d2}:{1:d2})", obj2.Length / 0xea60, (obj2.Length % 0xea60) / 0x3e8);
                    this.m_selectedObjectFileName = obj2.FileName;
                    this.m_selectedObjectLength = new TimeSpan(0, 0, 0, 0, obj2.Length);
                    this.m_objectType = ObjectType.Program;
                }
            }
        }

        private void buttonSelectSequence_Click(object sender, EventArgs e)
        {
            this.openFileDialog.Filter = this.m_fileTypeFilter;
            this.openFileDialog.InitialDirectory = Paths.SequencePath;
            this.openFileDialog.FileName = string.Empty;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (IScheduledObject obj2 = this.LoadSequence(this.openFileDialog.FileName))
                {
                    this.labelObject.Text = string.Format("{0} ({1:d2}:{2:d2})", Path.GetFileName(this.openFileDialog.FileName), obj2.Length / 0xea60, (obj2.Length % 0xea60) / 0x3e8);
                    this.m_selectedObjectFileName = obj2.FileName;
                    this.m_selectedObjectLength = new TimeSpan(0, 0, 0, 0, obj2.Length);
                    this.m_objectType = ObjectType.Sequence;
                }
            }
        }

        private void checkBoxEvery_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxRepeatInterval.Enabled = this.labelMinutes.Enabled = this.checkBoxEvery.Checked;
        }

        private void checkBoxRepeat_CheckedChanged(object sender, EventArgs e)
        {
            this.labelUntil.Enabled = this.comboBoxEndTime.Enabled = this.checkBoxEvery.Enabled = this.checkBoxRepeat.Checked;
        }

        private void comboBoxRecurrenceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panelRecurrenceRange.Enabled = this.comboBoxRecurrenceType.SelectedIndex > 0;
            switch (this.comboBoxRecurrenceType.SelectedIndex)
            {
                case -1:
                    this.labelRecurrenceTypeExplanation.Text = string.Empty;
                    this.tabControl.SelectedTab = this.tabPageNone;
                    break;

                case 0:
                    this.labelRecurrenceTypeExplanation.Text = "The timer will only execute once";
                    this.tabControl.SelectedTab = this.tabPageNone;
                    break;

                case 1:
                    this.labelRecurrenceTypeExplanation.Text = "The timer will execute at the same time every day";
                    this.tabControl.SelectedTab = this.tabPageDaily;
                    break;

                case 2:
                    this.labelRecurrenceTypeExplanation.Text = "The timer will execute at the same time on the selected days";
                    this.tabControl.SelectedTab = this.tabPageWeekly;
                    break;

                case 3:
                    this.labelRecurrenceTypeExplanation.Text = "The timer will execute at the same time on the same day every month";
                    this.tabControl.SelectedTab = this.tabPageMonthly;
                    break;

                case 4:
                    this.labelRecurrenceTypeExplanation.Text = "The timer will execute at the same time on the same date every year";
                    this.tabControl.SelectedTab = this.tabPageYearly;
                    break;
            }
        }

        private void comboBoxStartTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime time;
            if (!DateTime.TryParse(this.comboBoxStartTime.Text, out time))
            {
                MessageBox.Show("Start time is not a valid time", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.PopulateEndTimeValues(time);
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
            this.components = new Container();
            this.panel1 = new Panel();
            this.groupBox3 = new GroupBox();
            this.labelObject = new Label();
            this.buttonSelectProgram = new Button();
            this.buttonSelectSequence = new Button();
            this.groupBox2 = new GroupBox();
            this.panel2 = new Panel();
            this.panelRecurrenceRange = new Panel();
            this.dateTimePickerRecurrenceStartDate = new DateTimePicker();
            this.dateTimePickerRecurrenceEndDate = new DateTimePicker();
            this.label3 = new Label();
            this.label4 = new Label();
            this.tabControl = new Vixen.TabControl(this.components);
            this.tabPageNone = new TabPage();
            this.tabPageDaily = new TabPage();
            this.tabPageWeekly = new TabPage();
            this.checkBoxSaturday = new CheckBox();
            this.checkBoxFriday = new CheckBox();
            this.checkBoxThursday = new CheckBox();
            this.checkBoxWednesday = new CheckBox();
            this.checkBoxTuesday = new CheckBox();
            this.checkBoxMonday = new CheckBox();
            this.checkBoxSunday = new CheckBox();
            this.tabPageMonthly = new TabPage();
            this.textBoxSpecificDay = new TextBox();
            this.radioButtonSpecificDay = new RadioButton();
            this.radioButtonLastDay = new RadioButton();
            this.radioButtonFirstDay = new RadioButton();
            this.label5 = new Label();
            this.tabPageYearly = new TabPage();
            this.dateTimePickerYearlyDate = new DateTimePicker();
            this.label6 = new Label();
            this.labelRecurrenceTypeExplanation = new Label();
            this.comboBoxRecurrenceType = new ComboBox();
            this.groupBox1 = new GroupBox();
            this.labelMinutes = new Label();
            this.textBoxRepeatInterval = new TextBox();
            this.checkBoxEvery = new CheckBox();
            this.comboBoxEndTime = new ComboBox();
            this.labelUntil = new Label();
            this.checkBoxRepeat = new CheckBox();
            this.comboBoxStartTime = new ComboBox();
            this.label2 = new Label();
            this.dateTimePickerExecutionStartDate = new DateTimePicker();
            this.label1 = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.openFileDialog = new OpenFileDialog();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panelRecurrenceRange.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageWeekly.SuspendLayout();
            this.tabPageMonthly.SuspendLayout();
            this.tabPageYearly.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x1a2, 0x1c9);
            this.panel1.TabIndex = 0;
            this.groupBox3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox3.Controls.Add(this.labelObject);
            this.groupBox3.Controls.Add(this.buttonSelectProgram);
            this.groupBox3.Controls.Add(this.buttonSelectSequence);
            this.groupBox3.Location = new Point(13, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x188, 0x53);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Item to Execute";
            this.labelObject.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelObject.Location = new Point(0x91, 0x1b);
            this.labelObject.Name = "labelObject";
            this.labelObject.Size = new Size(0xf1, 0x35);
            this.labelObject.TabIndex = 2;
            this.buttonSelectProgram.Location = new Point(0x12, 0x31);
            this.buttonSelectProgram.Name = "buttonSelectProgram";
            this.buttonSelectProgram.Size = new Size(0x6a, 0x17);
            this.buttonSelectProgram.TabIndex = 1;
            this.buttonSelectProgram.Text = "Select Program";
            this.buttonSelectProgram.UseVisualStyleBackColor = true;
            this.buttonSelectProgram.Click += new EventHandler(this.buttonSelectProgram_Click);
            this.buttonSelectSequence.Location = new Point(0x12, 20);
            this.buttonSelectSequence.Name = "buttonSelectSequence";
            this.buttonSelectSequence.Size = new Size(0x6a, 0x17);
            this.buttonSelectSequence.TabIndex = 0;
            this.buttonSelectSequence.Text = "Select Sequence";
            this.buttonSelectSequence.UseVisualStyleBackColor = true;
            this.buttonSelectSequence.Click += new EventHandler(this.buttonSelectSequence_Click);
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.panelRecurrenceRange);
            this.groupBox2.Controls.Add(this.tabControl);
            this.groupBox2.Controls.Add(this.labelRecurrenceTypeExplanation);
            this.groupBox2.Controls.Add(this.comboBoxRecurrenceType);
            this.groupBox2.Location = new Point(13, 0x102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x188, 0xc4);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recurrence";
            this.panel2.BorderStyle = BorderStyle.Fixed3D;
            this.panel2.Location = new Point(0x85, 0x18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(4, 0x88);
            this.panel2.TabIndex = 8;
            this.panelRecurrenceRange.Controls.Add(this.dateTimePickerRecurrenceStartDate);
            this.panelRecurrenceRange.Controls.Add(this.dateTimePickerRecurrenceEndDate);
            this.panelRecurrenceRange.Controls.Add(this.label3);
            this.panelRecurrenceRange.Controls.Add(this.label4);
            this.panelRecurrenceRange.Enabled = false;
            this.panelRecurrenceRange.Location = new Point(14, 50);
            this.panelRecurrenceRange.Name = "panelRecurrenceRange";
            this.panelRecurrenceRange.Size = new Size(0x70, 0x5f);
            this.panelRecurrenceRange.TabIndex = 7;
            this.dateTimePickerRecurrenceStartDate.Format = DateTimePickerFormat.Short;
            this.dateTimePickerRecurrenceStartDate.Location = new Point(5, 0x1a);
            this.dateTimePickerRecurrenceStartDate.Name = "dateTimePickerRecurrenceStartDate";
            this.dateTimePickerRecurrenceStartDate.Size = new Size(0x6a, 20);
            this.dateTimePickerRecurrenceStartDate.TabIndex = 4;
            this.dateTimePickerRecurrenceEndDate.Format = DateTimePickerFormat.Short;
            this.dateTimePickerRecurrenceEndDate.Location = new Point(5, 0x41);
            this.dateTimePickerRecurrenceEndDate.Name = "dateTimePickerRecurrenceEndDate";
            this.dateTimePickerRecurrenceEndDate.Size = new Size(0x6a, 20);
            this.dateTimePickerRecurrenceEndDate.TabIndex = 6;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(2, 10);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x35, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Start date";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(2, 0x31);
            this.label4.Name = "label4";
            this.label4.Size = new Size(50, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "End date";
            this.tabControl.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tabControl.Controls.Add(this.tabPageNone);
            this.tabControl.Controls.Add(this.tabPageDaily);
            this.tabControl.Controls.Add(this.tabPageWeekly);
            this.tabControl.Controls.Add(this.tabPageMonthly);
            this.tabControl.Controls.Add(this.tabPageYearly);
            this.tabControl.HideTabs = true;
            this.tabControl.Location = new Point(140, 0x22);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0xf6, 0x6f);
            this.tabControl.TabIndex = 2;
            this.tabPageNone.BackColor = SystemColors.Control;
            this.tabPageNone.Location = new Point(0, 0);
            this.tabPageNone.Name = "tabPageNone";
            this.tabPageNone.Size = new Size(0xf6, 0x6f);
            this.tabPageNone.TabIndex = 2;
            this.tabPageNone.Text = "tabPageNone";
            this.tabPageDaily.BackColor = SystemColors.Control;
            this.tabPageDaily.Location = new Point(0, 0);
            this.tabPageDaily.Name = "tabPageDaily";
            this.tabPageDaily.Padding = new Padding(3);
            this.tabPageDaily.Size = new Size(0xf6, 0x6f);
            this.tabPageDaily.TabIndex = 0;
            this.tabPageDaily.Text = "tabPageDaily";
            this.tabPageWeekly.BackColor = SystemColors.Control;
            this.tabPageWeekly.Controls.Add(this.checkBoxSaturday);
            this.tabPageWeekly.Controls.Add(this.checkBoxFriday);
            this.tabPageWeekly.Controls.Add(this.checkBoxThursday);
            this.tabPageWeekly.Controls.Add(this.checkBoxWednesday);
            this.tabPageWeekly.Controls.Add(this.checkBoxTuesday);
            this.tabPageWeekly.Controls.Add(this.checkBoxMonday);
            this.tabPageWeekly.Controls.Add(this.checkBoxSunday);
            this.tabPageWeekly.Location = new Point(0, 0);
            this.tabPageWeekly.Name = "tabPageWeekly";
            this.tabPageWeekly.Padding = new Padding(3);
            this.tabPageWeekly.Size = new Size(0xf6, 0x6f);
            this.tabPageWeekly.TabIndex = 1;
            this.tabPageWeekly.Text = "tabPageWeekly";
            this.checkBoxSaturday.AutoSize = true;
            this.checkBoxSaturday.Location = new Point(0x7b, 0x38);
            this.checkBoxSaturday.Name = "checkBoxSaturday";
            this.checkBoxSaturday.Size = new Size(0x44, 0x11);
            this.checkBoxSaturday.TabIndex = 6;
            this.checkBoxSaturday.Text = "Saturday";
            this.checkBoxSaturday.UseVisualStyleBackColor = true;
            this.checkBoxFriday.AutoSize = true;
            this.checkBoxFriday.Location = new Point(0x7b, 0x21);
            this.checkBoxFriday.Name = "checkBoxFriday";
            this.checkBoxFriday.Size = new Size(0x36, 0x11);
            this.checkBoxFriday.TabIndex = 5;
            this.checkBoxFriday.Text = "Friday";
            this.checkBoxFriday.UseVisualStyleBackColor = true;
            this.checkBoxThursday.AutoSize = true;
            this.checkBoxThursday.Location = new Point(0x7b, 10);
            this.checkBoxThursday.Name = "checkBoxThursday";
            this.checkBoxThursday.Size = new Size(70, 0x11);
            this.checkBoxThursday.TabIndex = 4;
            this.checkBoxThursday.Text = "Thursday";
            this.checkBoxThursday.UseVisualStyleBackColor = true;
            this.checkBoxWednesday.AutoSize = true;
            this.checkBoxWednesday.Location = new Point(0x12, 0x4f);
            this.checkBoxWednesday.Name = "checkBoxWednesday";
            this.checkBoxWednesday.Size = new Size(0x53, 0x11);
            this.checkBoxWednesday.TabIndex = 3;
            this.checkBoxWednesday.Text = "Wednesday";
            this.checkBoxWednesday.UseVisualStyleBackColor = true;
            this.checkBoxTuesday.AutoSize = true;
            this.checkBoxTuesday.Location = new Point(0x12, 0x38);
            this.checkBoxTuesday.Name = "checkBoxTuesday";
            this.checkBoxTuesday.Size = new Size(0x43, 0x11);
            this.checkBoxTuesday.TabIndex = 2;
            this.checkBoxTuesday.Text = "Tuesday";
            this.checkBoxTuesday.UseVisualStyleBackColor = true;
            this.checkBoxMonday.AutoSize = true;
            this.checkBoxMonday.Location = new Point(0x12, 0x21);
            this.checkBoxMonday.Name = "checkBoxMonday";
            this.checkBoxMonday.Size = new Size(0x40, 0x11);
            this.checkBoxMonday.TabIndex = 1;
            this.checkBoxMonday.Text = "Monday";
            this.checkBoxMonday.UseVisualStyleBackColor = true;
            this.checkBoxSunday.AutoSize = true;
            this.checkBoxSunday.Location = new Point(0x12, 10);
            this.checkBoxSunday.Name = "checkBoxSunday";
            this.checkBoxSunday.Size = new Size(0x3e, 0x11);
            this.checkBoxSunday.TabIndex = 0;
            this.checkBoxSunday.Text = "Sunday";
            this.checkBoxSunday.UseVisualStyleBackColor = true;
            this.tabPageMonthly.BackColor = SystemColors.Control;
            this.tabPageMonthly.Controls.Add(this.textBoxSpecificDay);
            this.tabPageMonthly.Controls.Add(this.radioButtonSpecificDay);
            this.tabPageMonthly.Controls.Add(this.radioButtonLastDay);
            this.tabPageMonthly.Controls.Add(this.radioButtonFirstDay);
            this.tabPageMonthly.Controls.Add(this.label5);
            this.tabPageMonthly.Location = new Point(0, 0);
            this.tabPageMonthly.Name = "tabPageMonthly";
            this.tabPageMonthly.Size = new Size(0xf6, 0x6f);
            this.tabPageMonthly.TabIndex = 3;
            this.tabPageMonthly.Text = "tabPageMonthly";
            this.textBoxSpecificDay.Location = new Point(0x99, 0x51);
            this.textBoxSpecificDay.MaxLength = 2;
            this.textBoxSpecificDay.Name = "textBoxSpecificDay";
            this.textBoxSpecificDay.Size = new Size(0x25, 20);
            this.textBoxSpecificDay.TabIndex = 4;
            this.radioButtonSpecificDay.AutoSize = true;
            this.radioButtonSpecificDay.Location = new Point(0x40, 0x52);
            this.radioButtonSpecificDay.Name = "radioButtonSpecificDay";
            this.radioButtonSpecificDay.Size = new Size(0x53, 0x11);
            this.radioButtonSpecificDay.TabIndex = 3;
            this.radioButtonSpecificDay.Text = "Specific day";
            this.radioButtonSpecificDay.UseVisualStyleBackColor = true;
            this.radioButtonLastDay.AutoSize = true;
            this.radioButtonLastDay.Location = new Point(0x40, 0x3b);
            this.radioButtonLastDay.Name = "radioButtonLastDay";
            this.radioButtonLastDay.Size = new Size(0x41, 0x11);
            this.radioButtonLastDay.TabIndex = 2;
            this.radioButtonLastDay.Text = "Last day";
            this.radioButtonLastDay.UseVisualStyleBackColor = true;
            this.radioButtonFirstDay.AutoSize = true;
            this.radioButtonFirstDay.Checked = true;
            this.radioButtonFirstDay.Location = new Point(0x41, 0x24);
            this.radioButtonFirstDay.Name = "radioButtonFirstDay";
            this.radioButtonFirstDay.Size = new Size(0x40, 0x11);
            this.radioButtonFirstDay.TabIndex = 1;
            this.radioButtonFirstDay.TabStop = true;
            this.radioButtonFirstDay.Text = "First day";
            this.radioButtonFirstDay.UseVisualStyleBackColor = true;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x13, 10);
            this.label5.Name = "label5";
            this.label5.Size = new Size(180, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Execute on which day of the month?";
            this.tabPageYearly.BackColor = SystemColors.Control;
            this.tabPageYearly.Controls.Add(this.dateTimePickerYearlyDate);
            this.tabPageYearly.Controls.Add(this.label6);
            this.tabPageYearly.Location = new Point(0, 0);
            this.tabPageYearly.Name = "tabPageYearly";
            this.tabPageYearly.Size = new Size(0xf6, 0x6f);
            this.tabPageYearly.TabIndex = 4;
            this.tabPageYearly.Text = "tabPageYearly";
            this.dateTimePickerYearlyDate.CustomFormat = "MMMM dd";
            this.dateTimePickerYearlyDate.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerYearlyDate.Location = new Point(0x41, 0x2c);
            this.dateTimePickerYearlyDate.Name = "dateTimePickerYearlyDate";
            this.dateTimePickerYearlyDate.Size = new Size(0x68, 20);
            this.dateTimePickerYearlyDate.TabIndex = 2;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x13, 0x12);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0xab, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Execute on which day of the year?";
            this.labelRecurrenceTypeExplanation.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.labelRecurrenceTypeExplanation.AutoSize = true;
            this.labelRecurrenceTypeExplanation.ForeColor = SystemColors.ActiveCaption;
            this.labelRecurrenceTypeExplanation.Location = new Point(15, 0xac);
            this.labelRecurrenceTypeExplanation.Name = "labelRecurrenceTypeExplanation";
            this.labelRecurrenceTypeExplanation.Size = new Size(0, 13);
            this.labelRecurrenceTypeExplanation.TabIndex = 1;
            this.comboBoxRecurrenceType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxRecurrenceType.FormattingEnabled = true;
            this.comboBoxRecurrenceType.Items.AddRange(new object[] { "None", "Daily", "Weekly", "Monthly", "Yearly" });
            this.comboBoxRecurrenceType.Location = new Point(0x12, 0x17);
            this.comboBoxRecurrenceType.Name = "comboBoxRecurrenceType";
            this.comboBoxRecurrenceType.Size = new Size(0x6b, 0x15);
            this.comboBoxRecurrenceType.TabIndex = 0;
            this.comboBoxRecurrenceType.SelectedIndexChanged += new EventHandler(this.comboBoxRecurrenceType_SelectedIndexChanged);
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.labelMinutes);
            this.groupBox1.Controls.Add(this.textBoxRepeatInterval);
            this.groupBox1.Controls.Add(this.checkBoxEvery);
            this.groupBox1.Controls.Add(this.comboBoxEndTime);
            this.groupBox1.Controls.Add(this.labelUntil);
            this.groupBox1.Controls.Add(this.checkBoxRepeat);
            this.groupBox1.Controls.Add(this.comboBoxStartTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dateTimePickerExecutionStartDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(13, 0x65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x188, 0x97);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Execution Start";
            this.labelMinutes.AutoSize = true;
            this.labelMinutes.Enabled = false;
            this.labelMinutes.Location = new Point(0xb8, 110);
            this.labelMinutes.Name = "labelMinutes";
            this.labelMinutes.Size = new Size(0x2b, 13);
            this.labelMinutes.TabIndex = 9;
            this.labelMinutes.Text = "minutes";
            this.textBoxRepeatInterval.Enabled = false;
            this.textBoxRepeatInterval.Location = new Point(0x94, 0x6b);
            this.textBoxRepeatInterval.MaxLength = 2;
            this.textBoxRepeatInterval.Name = "textBoxRepeatInterval";
            this.textBoxRepeatInterval.Size = new Size(30, 20);
            this.textBoxRepeatInterval.TabIndex = 8;
            this.checkBoxEvery.AutoSize = true;
            this.checkBoxEvery.Enabled = false;
            this.checkBoxEvery.Location = new Point(0x56, 0x6d);
            this.checkBoxEvery.Name = "checkBoxEvery";
            this.checkBoxEvery.Size = new Size(0x37, 0x11);
            this.checkBoxEvery.TabIndex = 7;
            this.checkBoxEvery.Text = " every";
            this.checkBoxEvery.UseVisualStyleBackColor = true;
            this.checkBoxEvery.CheckedChanged += new EventHandler(this.checkBoxEvery_CheckedChanged);
            this.comboBoxEndTime.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxEndTime.DropDownWidth = 150;
            this.comboBoxEndTime.Enabled = false;
            this.comboBoxEndTime.FormattingEnabled = true;
            this.comboBoxEndTime.Items.AddRange(new object[] { 
                "12:00 AM", "12:30 AM", "1:00 AM", "1:30 AM", "2:00 AM", "2:30 AM", "3:00 AM", "3:30 AM", "4:00 AM", "4:30 AM", "5:00 AM", "5:30 AM", "6:00 AM", "6:30 AM", "7:00 AM", "7:30 AM", 
                "8:00 AM", "8:30 AM", "9:00 AM", "9:30 AM", "10:00 AM", "10:30 AM", "11:00 AM", "11:30 AM", "12:00 PM", "12:30 PM", "1:00 PM", "1:30 PM", "2:00 PM", "2:30 PM", "3:00 PM", "3:30 PM", 
                "4:00 PM", "4:30 PM", "5:00 PM", "5:30 PM", "6:00 PM", "6:30 PM", "7:00 PM", "7:30 PM", "8:00 PM", "8:30 PM", "9:00 PM", "9:30 PM", "10:00 PM", "10:30 PM", "11:00 PM", "11:30 PM"
             });
            this.comboBoxEndTime.Location = new Point(0xb1, 80);
            this.comboBoxEndTime.Name = "comboBoxEndTime";
            this.comboBoxEndTime.Size = new Size(140, 0x15);
            this.comboBoxEndTime.TabIndex = 6;
            this.labelUntil.AutoSize = true;
            this.labelUntil.Enabled = false;
            this.labelUntil.Location = new Point(0x91, 0x53);
            this.labelUntil.Name = "labelUntil";
            this.labelUntil.Size = new Size(0x1a, 13);
            this.labelUntil.TabIndex = 5;
            this.labelUntil.Text = "until";
            this.checkBoxRepeat.AutoSize = true;
            this.checkBoxRepeat.Location = new Point(0x56, 0x52);
            this.checkBoxRepeat.Name = "checkBoxRepeat";
            this.checkBoxRepeat.Size = new Size(0x38, 0x11);
            this.checkBoxRepeat.TabIndex = 4;
            this.checkBoxRepeat.Text = "repeat";
            this.checkBoxRepeat.UseVisualStyleBackColor = true;
            this.checkBoxRepeat.CheckedChanged += new EventHandler(this.checkBoxRepeat_CheckedChanged);
            this.comboBoxStartTime.FormattingEnabled = true;
            this.comboBoxStartTime.Items.AddRange(new object[] { 
                "12:00 AM", "12:30 AM", "1:00 AM", "1:30 AM", "2:00 AM", "2:30 AM", "3:00 AM", "3:30 AM", "4:00 AM", "4:30 AM", "5:00 AM", "5:30 AM", "6:00 AM", "6:30 AM", "7:00 AM", "7:30 AM", 
                "8:00 AM", "8:30 AM", "9:00 AM", "9:30 AM", "10:00 AM", "10:30 AM", "11:00 AM", "11:30 AM", "12:00 PM", "12:30 PM", "1:00 PM", "1:30 PM", "2:00 PM", "2:30 PM", "3:00 PM", "3:30 PM", 
                "4:00 PM", "4:30 PM", "5:00 PM", "5:30 PM", "6:00 PM", "6:30 PM", "7:00 PM", "7:30 PM", "8:00 PM", "8:30 PM", "9:00 PM", "9:30 PM", "10:00 PM", "10:30 PM", "11:00 PM", "11:30 PM"
             });
            this.comboBoxStartTime.Location = new Point(0x94, 0x2d);
            this.comboBoxStartTime.Name = "comboBoxStartTime";
            this.comboBoxStartTime.Size = new Size(0x4a, 0x15);
            this.comboBoxStartTime.TabIndex = 3;
            this.comboBoxStartTime.SelectedIndexChanged += new EventHandler(this.comboBoxStartTime_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x65, 0x30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Start at";
            this.dateTimePickerExecutionStartDate.CustomFormat = "dddd M/dd/yyyy";
            this.dateTimePickerExecutionStartDate.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerExecutionStartDate.Location = new Point(0x94, 0x13);
            this.dateTimePickerExecutionStartDate.Name = "dateTimePickerExecutionStartDate";
            this.dateTimePickerExecutionStartDate.Size = new Size(0xa9, 20);
            this.dateTimePickerExecutionStartDate.TabIndex = 1;
            this.dateTimePickerExecutionStartDate.Value = new DateTime(0x7d6, 12, 0x1b, 0, 0, 0, 0);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x79, 0x17);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x15, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "On";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0xf7, 0x1cf);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x148, 0x1cf);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x19f, 0x1f2);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "TimerDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Timer";
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panelRecurrenceRange.ResumeLayout(false);
            this.panelRecurrenceRange.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageWeekly.ResumeLayout(false);
            this.tabPageWeekly.PerformLayout();
            this.tabPageMonthly.ResumeLayout(false);
            this.tabPageMonthly.PerformLayout();
            this.tabPageYearly.ResumeLayout(false);
            this.tabPageYearly.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private IScheduledObject LoadProgram(string fileName)
        {
            return new SequenceProgram(fileName);
        }

        private IScheduledObject LoadSequence(string fileName)
        {
            return new EventSequence(fileName);
        }

        private void PopulateEndTimeValues(DateTime startTime)
        {
            this.comboBoxEndTime.BeginUpdate();
            try
            {
                DateTime minValue = DateTime.MinValue;
                int count = -1;
                if (this.comboBoxEndTime.SelectedIndex != -1)
                {
                    DateTime time2;
                    string[] strArray = ((string) this.comboBoxEndTime.SelectedItem).Split(new char[] { ' ' });
                    if ((strArray[0][0] >= '0') && (strArray[0][0] <= '9'))
                    {
                        time2 = DateTime.Parse(string.Format("{0} {1}", strArray[0], strArray[1]));
                        minValue = new DateTime(startTime.Year, startTime.Month, startTime.Day, time2.Hour, time2.Minute, 0);
                    }
                    else
                    {
                        time2 = DateTime.Parse(string.Format("{0} {1}", strArray[1], strArray[2]));
                        minValue = new DateTime(startTime.Year, startTime.Month, startTime.Day + 1, time2.Hour, time2.Minute, 0);
                    }
                }
                this.comboBoxEndTime.Items.Clear();
                this.comboBoxEndTime.Items.Add(startTime.AddMinutes(30.0).ToShortTimeString() + " (30 minutes)");
                for (float i = 1f; i < 24f; i += 0.5f)
                {
                    DateTime time3 = startTime.AddHours((double) i);
                    if (time3.CompareTo(minValue) == 0)
                    {
                        count = this.comboBoxEndTime.Items.Count;
                    }
                    if (startTime.DayOfWeek != time3.DayOfWeek)
                    {
                        this.comboBoxEndTime.Items.Add(string.Format("{0} {1} ({2} {3})", new object[] { time3.DayOfWeek, time3.ToShortTimeString(), i, (i > 1f) ? "hours" : "hour" }));
                    }
                    else
                    {
                        this.comboBoxEndTime.Items.Add(string.Format("{0} ({1} {2})", time3.ToShortTimeString(), i, (i > 1f) ? "hours" : "hour"));
                    }
                }
                if (count != -1)
                {
                    this.comboBoxEndTime.SelectedIndex = count;
                }
            }
            finally
            {
                this.comboBoxEndTime.EndUpdate();
            }
        }

        public Vixen.Timer Timer
        {
            get
            {
                return this.m_timer;
            }
        }
    }
}

