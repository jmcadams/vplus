using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Vixen
{
	internal partial class TimerDialog : Form
	{
		private readonly string m_fileTypeFilter;
		private ObjectType m_objectType;
		private string m_selectedObjectFileName;
		private TimeSpan m_selectedObjectLength;
		private Timer m_timer;

		public TimerDialog(DateTime startDateTime)
		{
			m_timer = null;
			m_selectedObjectFileName = string.Empty;
			m_selectedObjectLength = TimeSpan.MinValue;
			components = null;
			InitializeComponent();
			m_fileTypeFilter = ((ISystem) Interfaces.Available["ISystem"]).KnownFileTypesFilter;
			comboBoxStartTime.SelectedIndex = (startDateTime.Hour*2) + (startDateTime.Minute/30);
			dateTimePickerRecurrenceStartDate.Value =
				dateTimePickerRecurrenceEndDate.Value = dateTimePickerExecutionStartDate.Value = startDateTime;
			comboBoxRecurrenceType.SelectedIndex = 0;
			tabControl.SelectedTab = tabPageNone;
		}

		public TimerDialog(Timer timer)
		{
			m_timer = null;
			m_selectedObjectFileName = string.Empty;
			m_selectedObjectLength = TimeSpan.MinValue;
			components = null;
			InitializeComponent();
			m_fileTypeFilter = ((ISystem) Interfaces.Available["ISystem"]).KnownFileTypesFilter;
			dateTimePickerExecutionStartDate.Value = timer.StartDate;
			labelObject.Text = string.Format("{0} ({1:d2}:{2:d2})", timer.ProgramName, timer.ObjectLength.Minutes,
			                                 timer.ObjectLength.Seconds);
			m_selectedObjectFileName = timer.ProgramFileName;
			m_selectedObjectLength = timer.ObjectLength;
			m_objectType = timer.ObjectType;
			if ((timer.StartTime.TotalMinutes%30.0) == 0.0)
			{
				comboBoxStartTime.SelectedIndex = (int) (timer.StartTime.TotalMinutes/30.0);
			}
			else
			{
				comboBoxStartTime.Text = timer.StartDateTime.ToShortTimeString();
			}
			if (timer.TimerLength != timer.ObjectLength)
			{
				checkBoxRepeat.Checked = true;
				PopulateEndTimeValues(DateTime.Parse(comboBoxStartTime.Text));
				comboBoxEndTime.SelectedIndex = ((int) (timer.TimerLength.TotalMinutes/30.0)) - 1;
				if (timer.RepeatInterval != 0)
				{
					checkBoxEvery.Checked = true;
					textBoxRepeatInterval.Text = timer.RepeatInterval.ToString();
				}
			}
			if (timer.Recurrence == RecurrenceType.None)
			{
				comboBoxRecurrenceType.SelectedIndex = 0;
			}
			else
			{
				comboBoxRecurrenceType.SelectedIndex = (int) timer.Recurrence;
				dateTimePickerRecurrenceStartDate.Value = timer.RecurrenceStart;
				dateTimePickerRecurrenceEndDate.Value = timer.RecurrenceEnd;
				switch (timer.Recurrence)
				{
					case RecurrenceType.Weekly:
						{
							var recurrenceData = (int) timer.RecurrenceData;
							checkBoxSunday.Checked = (recurrenceData & 1) != 0;
							checkBoxMonday.Checked = (recurrenceData & 2) != 0;
							checkBoxTuesday.Checked = (recurrenceData & 4) != 0;
							checkBoxWednesday.Checked = (recurrenceData & 8) != 0;
							checkBoxThursday.Checked = (recurrenceData & 0x10) != 0;
							checkBoxFriday.Checked = (recurrenceData & 0x20) != 0;
							checkBoxSaturday.Checked = (recurrenceData & 0x40) != 0;
							goto Label_037C;
						}
					case RecurrenceType.Monthly:
						{
							var str = (string) timer.RecurrenceData;
							if (timer.RecurrenceData is string)
							{
								if (!(str == "first"))
								{
									radioButtonLastDay.Checked = true;
								}
								else
								{
									radioButtonFirstDay.Checked = true;
								}
							}
							else
							{
								textBoxSpecificDay.Text = str;
							}
							goto Label_037C;
						}
					case RecurrenceType.Yearly:
						dateTimePickerYearlyDate.Value = (DateTime) timer.RecurrenceData;
						goto Label_037C;
				}
			}
			Label_037C:
			m_timer = timer;
		}

		public Timer Timer
		{
			get { return m_timer; }
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			DateTime time;
			if (m_timer == null)
			{
				m_timer = new Timer();
			}
			var builder = new StringBuilder();
			if (m_selectedObjectFileName == string.Empty)
			{
				builder.AppendLine("* Nothing has been selected to execute");
			}
			else
			{
				m_timer.ObjectType = m_objectType;
				m_timer.ProgramFileName = m_selectedObjectFileName;
			}
			m_timer.StartDate = dateTimePickerExecutionStartDate.Value;
			if (!DateTime.TryParse(comboBoxStartTime.Text, out time))
			{
				builder.AppendLine("* Start time is not a valid time");
			}
			else
			{
				m_timer.StartTime = time.TimeOfDay;
			}
			if (m_selectedObjectLength != TimeSpan.MinValue)
			{
				m_timer.ObjectLength = new TimeSpan(0, 0, 0, 0, (int) m_selectedObjectLength.TotalMilliseconds);
			}
			if (checkBoxRepeat.Checked)
			{
				m_timer.RepeatInterval = 0;
				TimeSpan span = (time.AddHours(((comboBoxEndTime.SelectedIndex + 1)*0.5f)) - time);
				if (span.TotalMinutes <= 0.0)
				{
					builder.AppendLine("* End time needs to be after the start time");
				}
				else
				{
					m_timer.TimerLength = span;
				}
				if (m_timer.TimerLength < m_timer.ObjectLength)
				{
					m_timer.TimerLength = m_timer.ObjectLength;
					MessageBox.Show("Timer length has been adjusted so that it's at least as long as the scheduled item.",
					                Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				if (checkBoxEvery.Checked)
				{
					try
					{
						m_timer.RepeatInterval = Convert.ToInt32(textBoxRepeatInterval.Text);
					}
					catch
					{
						builder.AppendLine("* The repeat interval must be 0 (for end-to-end repeating) or greater");
					}
					if (((m_timer.ObjectLength.TotalMinutes*2.0) + m_timer.RepeatInterval) > m_timer.TimerLength.TotalMinutes)
					{
						m_timer.TimerLength = m_timer.ObjectLength;
						m_timer.RepeatInterval = 0;
						MessageBox.Show(
							"Since the scheduled item would not repeat with the given interval\nand timer duration, it has been changed to a single-occurence timer",
							Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
			}
			else
			{
				m_timer.TimerLength = m_timer.ObjectLength;
			}
			m_timer.Recurrence = (RecurrenceType) Math.Max(comboBoxRecurrenceType.SelectedIndex, 0);
			if (comboBoxRecurrenceType.SelectedIndex <= 0)
			{
				m_timer.RecurrenceStart = DateTime.MinValue;
				m_timer.RecurrenceEnd = DateTime.MinValue;
			}
			else
			{
				m_timer.RecurrenceStart = dateTimePickerRecurrenceStartDate.Value.Date;
				m_timer.RecurrenceEnd = dateTimePickerRecurrenceEndDate.Value.Date.Add(new TimeSpan(0x17, 0x3b, 0));
				switch (m_timer.Recurrence)
				{
					case RecurrenceType.Weekly:
						{
							int num = 0;
							if (checkBoxSunday.Checked)
							{
								num |= 1;
							}
							if (checkBoxMonday.Checked)
							{
								num |= 2;
							}
							if (checkBoxTuesday.Checked)
							{
								num |= 4;
							}
							if (checkBoxWednesday.Checked)
							{
								num |= 8;
							}
							if (checkBoxThursday.Checked)
							{
								num |= 0x10;
							}
							if (checkBoxFriday.Checked)
							{
								num |= 0x20;
							}
							if (checkBoxSaturday.Checked)
							{
								num |= 0x40;
							}
							m_timer.RecurrenceData = num;
							goto Label_054B;
						}
					case RecurrenceType.Monthly:
						if (!radioButtonFirstDay.Checked)
						{
							if (radioButtonLastDay.Checked)
							{
								m_timer.RecurrenceData = "last";
							}
							else
							{
								int num2 = 0;
								try
								{
									num2 = Convert.ToInt32(textBoxSpecificDay.Text);
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
									m_timer.RecurrenceData = num2;
								}
							}
						}
						else
						{
							m_timer.RecurrenceData = "first";
						}
						goto Label_054B;

					case RecurrenceType.Yearly:
						m_timer.RecurrenceData = dateTimePickerYearlyDate.Value;
						goto Label_054B;
				}
			}
			Label_054B:
			if (builder.Length > 0)
			{
				base.DialogResult = DialogResult.None;
				MessageBox.Show("The following items need to be corrected before\na timer can be created:\n\n" + builder,
				                Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void buttonSelectProgram_Click(object sender, EventArgs e)
		{
			openFileDialog.Filter = string.Format("{0} program|*.{1}", Vendor.ProductName, Vendor.ProgramExtension);
			openFileDialog.InitialDirectory = Paths.ProgramPath;
			openFileDialog.FileName = string.Empty;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				using (IScheduledObject obj2 = LoadProgram(openFileDialog.FileName))
				{
					labelObject.Text = obj2.Name + string.Format(" ({0:d2}:{1:d2})", obj2.Length/0xea60, (obj2.Length%0xea60)/0x3e8);
					m_selectedObjectFileName = obj2.FileName;
					m_selectedObjectLength = new TimeSpan(0, 0, 0, 0, obj2.Length);
					m_objectType = ObjectType.Program;
				}
			}
		}

		private void buttonSelectSequence_Click(object sender, EventArgs e)
		{
			openFileDialog.Filter = m_fileTypeFilter;
			openFileDialog.InitialDirectory = Paths.SequencePath;
			openFileDialog.FileName = string.Empty;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				using (IScheduledObject obj2 = LoadSequence(openFileDialog.FileName))
				{
					labelObject.Text = string.Format("{0} ({1:d2}:{2:d2})", Path.GetFileName(openFileDialog.FileName),
					                                 obj2.Length/0xea60, (obj2.Length%0xea60)/0x3e8);
					m_selectedObjectFileName = obj2.FileName;
					m_selectedObjectLength = new TimeSpan(0, 0, 0, 0, obj2.Length);
					m_objectType = ObjectType.Sequence;
				}
			}
		}

		private void checkBoxEvery_CheckedChanged(object sender, EventArgs e)
		{
			textBoxRepeatInterval.Enabled = labelMinutes.Enabled = checkBoxEvery.Checked;
		}

		private void checkBoxRepeat_CheckedChanged(object sender, EventArgs e)
		{
			labelUntil.Enabled = comboBoxEndTime.Enabled = checkBoxEvery.Enabled = checkBoxRepeat.Checked;
		}

		private void comboBoxRecurrenceType_SelectedIndexChanged(object sender, EventArgs e)
		{
			panelRecurrenceRange.Enabled = comboBoxRecurrenceType.SelectedIndex > 0;
			switch (comboBoxRecurrenceType.SelectedIndex)
			{
				case -1:
					labelRecurrenceTypeExplanation.Text = string.Empty;
					tabControl.SelectedTab = tabPageNone;
					break;

				case 0:
					labelRecurrenceTypeExplanation.Text = "The timer will only execute once";
					tabControl.SelectedTab = tabPageNone;
					break;

				case 1:
					labelRecurrenceTypeExplanation.Text = "The timer will execute at the same time every day";
					tabControl.SelectedTab = tabPageDaily;
					break;

				case 2:
					labelRecurrenceTypeExplanation.Text = "The timer will execute at the same time on the selected days";
					tabControl.SelectedTab = tabPageWeekly;
					break;

				case 3:
					labelRecurrenceTypeExplanation.Text = "The timer will execute at the same time on the same day every month";
					tabControl.SelectedTab = tabPageMonthly;
					break;

				case 4:
					labelRecurrenceTypeExplanation.Text = "The timer will execute at the same time on the same date every year";
					tabControl.SelectedTab = tabPageYearly;
					break;
			}
		}

		private void comboBoxStartTime_SelectedIndexChanged(object sender, EventArgs e)
		{
			DateTime time;
			if (!DateTime.TryParse(comboBoxStartTime.Text, out time))
			{
				MessageBox.Show("Start time is not a valid time", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
			}
			else
			{
				PopulateEndTimeValues(time);
			}
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
			comboBoxEndTime.BeginUpdate();
			try
			{
				DateTime minValue = DateTime.MinValue;
				int count = -1;
				if (comboBoxEndTime.SelectedIndex != -1)
				{
					DateTime time2;
					string[] strArray = ((string) comboBoxEndTime.SelectedItem).Split(new[] {' '});
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
				comboBoxEndTime.Items.Clear();
				comboBoxEndTime.Items.Add(startTime.AddMinutes(30.0).ToShortTimeString() + " (30 minutes)");
				for (float i = 1f; i < 24f; i += 0.5f)
				{
					DateTime time3 = startTime.AddHours(i);
					if (time3.CompareTo(minValue) == 0)
					{
						count = comboBoxEndTime.Items.Count;
					}
					if (startTime.DayOfWeek != time3.DayOfWeek)
					{
						comboBoxEndTime.Items.Add(string.Format("{0} {1} ({2} {3})",
						                                        new object[]
							                                        {
								                                        time3.DayOfWeek, time3.ToShortTimeString(), i,
								                                        (i > 1f) ? "hours" : "hour"
							                                        }));
					}
					else
					{
						comboBoxEndTime.Items.Add(string.Format("{0} ({1} {2})", time3.ToShortTimeString(), i, (i > 1f) ? "hours" : "hour"));
					}
				}
				if (count != -1)
				{
					comboBoxEndTime.SelectedIndex = count;
				}
			}
			finally
			{
				comboBoxEndTime.EndUpdate();
			}
		}
	}
}