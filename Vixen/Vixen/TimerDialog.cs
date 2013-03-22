namespace Vixen {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;

	internal partial class TimerDialog : Form {

		private string m_fileTypeFilter;
		private ObjectType m_objectType;
		private string m_selectedObjectFileName;
		private TimeSpan m_selectedObjectLength;
		private Vixen.Timer m_timer;

		public TimerDialog(DateTime startDateTime) {
			this.m_timer = null;
			this.m_selectedObjectFileName = string.Empty;
			this.m_selectedObjectLength = TimeSpan.MinValue;
			this.components = null;
			this.InitializeComponent();
			this.m_fileTypeFilter = ((ISystem)Interfaces.Available["ISystem"]).KnownFileTypesFilter;
			this.comboBoxStartTime.SelectedIndex = (startDateTime.Hour * 2) + (startDateTime.Minute / 30);
			this.dateTimePickerRecurrenceStartDate.Value = this.dateTimePickerRecurrenceEndDate.Value = this.dateTimePickerExecutionStartDate.Value = startDateTime;
			this.comboBoxRecurrenceType.SelectedIndex = 0;
			this.tabControl.SelectedTab = this.tabPageNone;
		}

		public TimerDialog(Vixen.Timer timer) {
			this.m_timer = null;
			this.m_selectedObjectFileName = string.Empty;
			this.m_selectedObjectLength = TimeSpan.MinValue;
			this.components = null;
			this.InitializeComponent();
			this.m_fileTypeFilter = ((ISystem)Interfaces.Available["ISystem"]).KnownFileTypesFilter;
			this.dateTimePickerExecutionStartDate.Value = timer.StartDate;
			this.labelObject.Text = string.Format("{0} ({1:d2}:{2:d2})", timer.ProgramName, timer.ObjectLength.Minutes, timer.ObjectLength.Seconds);
			this.m_selectedObjectFileName = timer.ProgramFileName;
			this.m_selectedObjectLength = timer.ObjectLength;
			this.m_objectType = timer.ObjectType;
			if ((timer.StartTime.TotalMinutes % 30.0) == 0.0) {
				this.comboBoxStartTime.SelectedIndex = (int)(timer.StartTime.TotalMinutes / 30.0);
			}
			else {
				this.comboBoxStartTime.Text = timer.StartDateTime.ToShortTimeString();
			}
			if (timer.TimerLength != timer.ObjectLength) {
				this.checkBoxRepeat.Checked = true;
				this.PopulateEndTimeValues(DateTime.Parse(this.comboBoxStartTime.Text));
				this.comboBoxEndTime.SelectedIndex = ((int)(timer.TimerLength.TotalMinutes / 30.0)) - 1;
				if (timer.RepeatInterval != 0) {
					this.checkBoxEvery.Checked = true;
					this.textBoxRepeatInterval.Text = timer.RepeatInterval.ToString();
				}
			}
			if (timer.Recurrence == RecurrenceType.None) {
				this.comboBoxRecurrenceType.SelectedIndex = 0;
			}
			else {
				this.comboBoxRecurrenceType.SelectedIndex = (int)timer.Recurrence;
				this.dateTimePickerRecurrenceStartDate.Value = timer.RecurrenceStart;
				this.dateTimePickerRecurrenceEndDate.Value = timer.RecurrenceEnd;
				switch (timer.Recurrence) {
					case RecurrenceType.Weekly: {
							int recurrenceData = (int)timer.RecurrenceData;
							this.checkBoxSunday.Checked = (recurrenceData & 1) != 0;
							this.checkBoxMonday.Checked = (recurrenceData & 2) != 0;
							this.checkBoxTuesday.Checked = (recurrenceData & 4) != 0;
							this.checkBoxWednesday.Checked = (recurrenceData & 8) != 0;
							this.checkBoxThursday.Checked = (recurrenceData & 0x10) != 0;
							this.checkBoxFriday.Checked = (recurrenceData & 0x20) != 0;
							this.checkBoxSaturday.Checked = (recurrenceData & 0x40) != 0;
							goto Label_037C;
						}
					case RecurrenceType.Monthly: {
							string str = (string)timer.RecurrenceData;
							if (timer.RecurrenceData is string) {
								if (!(str == "first")) {
									this.radioButtonLastDay.Checked = true;
								}
								else {
									this.radioButtonFirstDay.Checked = true;
								}
							}
							else {
								this.textBoxSpecificDay.Text = str;
							}
							goto Label_037C;
						}
					case RecurrenceType.Yearly:
						this.dateTimePickerYearlyDate.Value = (DateTime)timer.RecurrenceData;
						goto Label_037C;
				}
			}
		Label_037C:
			this.m_timer = timer;
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			DateTime time;
			if (this.m_timer == null) {
				this.m_timer = new Vixen.Timer();
			}
			StringBuilder builder = new StringBuilder();
			if (this.m_selectedObjectFileName == string.Empty) {
				builder.AppendLine("* Nothing has been selected to execute");
			}
			else {
				this.m_timer.ObjectType = this.m_objectType;
				this.m_timer.ProgramFileName = this.m_selectedObjectFileName;
			}
			this.m_timer.StartDate = this.dateTimePickerExecutionStartDate.Value;
			if (!DateTime.TryParse(this.comboBoxStartTime.Text, out time)) {
				builder.AppendLine("* Start time is not a valid time");
			}
			else {
				this.m_timer.StartTime = time.TimeOfDay;
			}
			if (this.m_selectedObjectLength != TimeSpan.MinValue) {
				this.m_timer.ObjectLength = new TimeSpan(0, 0, 0, 0, (int)this.m_selectedObjectLength.TotalMilliseconds);
			}
			if (this.checkBoxRepeat.Checked) {
				this.m_timer.RepeatInterval = 0;
				TimeSpan span = (TimeSpan)(time.AddHours((double)((this.comboBoxEndTime.SelectedIndex + 1) * 0.5f)) - time);
				if (span.TotalMinutes <= 0.0) {
					builder.AppendLine("* End time needs to be after the start time");
				}
				else {
					this.m_timer.TimerLength = span;
				}
				if (this.m_timer.TimerLength < this.m_timer.ObjectLength) {
					this.m_timer.TimerLength = this.m_timer.ObjectLength;
					MessageBox.Show("Timer length has been adjusted so that it's at least as long as the scheduled item.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				if (this.checkBoxEvery.Checked) {
					try {
						this.m_timer.RepeatInterval = Convert.ToInt32(this.textBoxRepeatInterval.Text);
					}
					catch {
						builder.AppendLine("* The repeat interval must be 0 (for end-to-end repeating) or greater");
					}
					if (((this.m_timer.ObjectLength.TotalMinutes * 2.0) + this.m_timer.RepeatInterval) > this.m_timer.TimerLength.TotalMinutes) {
						this.m_timer.TimerLength = this.m_timer.ObjectLength;
						this.m_timer.RepeatInterval = 0;
						MessageBox.Show("Since the scheduled item would not repeat with the given interval\nand timer duration, it has been changed to a single-occurence timer", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
			}
			else {
				this.m_timer.TimerLength = this.m_timer.ObjectLength;
			}
			this.m_timer.Recurrence = (RecurrenceType)Math.Max(this.comboBoxRecurrenceType.SelectedIndex, 0);
			if (this.comboBoxRecurrenceType.SelectedIndex <= 0) {
				this.m_timer.RecurrenceStart = DateTime.MinValue;
				this.m_timer.RecurrenceEnd = DateTime.MinValue;
			}
			else {
				this.m_timer.RecurrenceStart = this.dateTimePickerRecurrenceStartDate.Value.Date;
				this.m_timer.RecurrenceEnd = this.dateTimePickerRecurrenceEndDate.Value.Date.Add(new TimeSpan(0x17, 0x3b, 0));
				switch (this.m_timer.Recurrence) {
					case RecurrenceType.Weekly: {
							int num = 0;
							if (this.checkBoxSunday.Checked) {
								num |= 1;
							}
							if (this.checkBoxMonday.Checked) {
								num |= 2;
							}
							if (this.checkBoxTuesday.Checked) {
								num |= 4;
							}
							if (this.checkBoxWednesday.Checked) {
								num |= 8;
							}
							if (this.checkBoxThursday.Checked) {
								num |= 0x10;
							}
							if (this.checkBoxFriday.Checked) {
								num |= 0x20;
							}
							if (this.checkBoxSaturday.Checked) {
								num |= 0x40;
							}
							this.m_timer.RecurrenceData = num;
							goto Label_054B;
						}
					case RecurrenceType.Monthly:
						if (!this.radioButtonFirstDay.Checked) {
							if (this.radioButtonLastDay.Checked) {
								this.m_timer.RecurrenceData = "last";
							}
							else {
								int num2 = 0;
								try {
									num2 = Convert.ToInt32(this.textBoxSpecificDay.Text);
								}
								catch {
									builder.AppendLine("* Day listed for monthly recurrence is not a valid day");
								}
								if ((num2 < 1) || (num2 > 0x1f)) {
									builder.AppendLine("* Day listed for monthly recurrence is not a valid day");
								}
								else {
									this.m_timer.RecurrenceData = num2;
								}
							}
						}
						else {
							this.m_timer.RecurrenceData = "first";
						}
						goto Label_054B;

					case RecurrenceType.Yearly:
						this.m_timer.RecurrenceData = this.dateTimePickerYearlyDate.Value;
						goto Label_054B;
				}
			}
		Label_054B:
			if (builder.Length > 0) {
				base.DialogResult = System.Windows.Forms.DialogResult.None;
				MessageBox.Show("The following items need to be corrected before\na timer can be created:\n\n" + builder.ToString(), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void buttonSelectProgram_Click(object sender, EventArgs e) {
			this.openFileDialog.Filter = string.Format("{0} program|*.{1}", Vendor.ProductName, Vendor.ProgramExtension);
			this.openFileDialog.InitialDirectory = Paths.ProgramPath;
			this.openFileDialog.FileName = string.Empty;
			if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
				using (IScheduledObject obj2 = this.LoadProgram(this.openFileDialog.FileName)) {
					this.labelObject.Text = obj2.Name + string.Format(" ({0:d2}:{1:d2})", obj2.Length / 0xea60, (obj2.Length % 0xea60) / 0x3e8);
					this.m_selectedObjectFileName = obj2.FileName;
					this.m_selectedObjectLength = new TimeSpan(0, 0, 0, 0, obj2.Length);
					this.m_objectType = ObjectType.Program;
				}
			}
		}

		private void buttonSelectSequence_Click(object sender, EventArgs e) {
			this.openFileDialog.Filter = this.m_fileTypeFilter;
			this.openFileDialog.InitialDirectory = Paths.SequencePath;
			this.openFileDialog.FileName = string.Empty;
			if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
				using (IScheduledObject obj2 = this.LoadSequence(this.openFileDialog.FileName)) {
					this.labelObject.Text = string.Format("{0} ({1:d2}:{2:d2})", Path.GetFileName(this.openFileDialog.FileName), obj2.Length / 0xea60, (obj2.Length % 0xea60) / 0x3e8);
					this.m_selectedObjectFileName = obj2.FileName;
					this.m_selectedObjectLength = new TimeSpan(0, 0, 0, 0, obj2.Length);
					this.m_objectType = ObjectType.Sequence;
				}
			}
		}

		private void checkBoxEvery_CheckedChanged(object sender, EventArgs e) {
			this.textBoxRepeatInterval.Enabled = this.labelMinutes.Enabled = this.checkBoxEvery.Checked;
		}

		private void checkBoxRepeat_CheckedChanged(object sender, EventArgs e) {
			this.labelUntil.Enabled = this.comboBoxEndTime.Enabled = this.checkBoxEvery.Enabled = this.checkBoxRepeat.Checked;
		}

		private void comboBoxRecurrenceType_SelectedIndexChanged(object sender, EventArgs e) {
			this.panelRecurrenceRange.Enabled = this.comboBoxRecurrenceType.SelectedIndex > 0;
			switch (this.comboBoxRecurrenceType.SelectedIndex) {
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

		private void comboBoxStartTime_SelectedIndexChanged(object sender, EventArgs e) {
			DateTime time;
			if (!DateTime.TryParse(this.comboBoxStartTime.Text, out time)) {
				MessageBox.Show("Start time is not a valid time", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else {
				this.PopulateEndTimeValues(time);
			}
		}





		private IScheduledObject LoadProgram(string fileName) {
			return new SequenceProgram(fileName);
		}

		private IScheduledObject LoadSequence(string fileName) {
			return new EventSequence(fileName);
		}

		private void PopulateEndTimeValues(DateTime startTime) {
			this.comboBoxEndTime.BeginUpdate();
			try {
				DateTime minValue = DateTime.MinValue;
				int count = -1;
				if (this.comboBoxEndTime.SelectedIndex != -1) {
					DateTime time2;
					string[] strArray = ((string)this.comboBoxEndTime.SelectedItem).Split(new char[] { ' ' });
					if ((strArray[0][0] >= '0') && (strArray[0][0] <= '9')) {
						time2 = DateTime.Parse(string.Format("{0} {1}", strArray[0], strArray[1]));
						minValue = new DateTime(startTime.Year, startTime.Month, startTime.Day, time2.Hour, time2.Minute, 0);
					}
					else {
						time2 = DateTime.Parse(string.Format("{0} {1}", strArray[1], strArray[2]));
						minValue = new DateTime(startTime.Year, startTime.Month, startTime.Day + 1, time2.Hour, time2.Minute, 0);
					}
				}
				this.comboBoxEndTime.Items.Clear();
				this.comboBoxEndTime.Items.Add(startTime.AddMinutes(30.0).ToShortTimeString() + " (30 minutes)");
				for (float i = 1f; i < 24f; i += 0.5f) {
					DateTime time3 = startTime.AddHours((double)i);
					if (time3.CompareTo(minValue) == 0) {
						count = this.comboBoxEndTime.Items.Count;
					}
					if (startTime.DayOfWeek != time3.DayOfWeek) {
						this.comboBoxEndTime.Items.Add(string.Format("{0} {1} ({2} {3})", new object[] { time3.DayOfWeek, time3.ToShortTimeString(), i, (i > 1f) ? "hours" : "hour" }));
					}
					else {
						this.comboBoxEndTime.Items.Add(string.Format("{0} ({1} {2})", time3.ToShortTimeString(), i, (i > 1f) ? "hours" : "hour"));
					}
				}
				if (count != -1) {
					this.comboBoxEndTime.SelectedIndex = count;
				}
			}
			finally {
				this.comboBoxEndTime.EndUpdate();
			}
		}

		public Vixen.Timer Timer {
			get {
				return this.m_timer;
			}
		}
	}
}

