using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VixenPlus
{
	internal partial class ExecutingTimerControlDialog : Form
	{
		private readonly TimerContext.OnExecutionChange m_onExecutionChangeHandler;
		private readonly TimerContext.OnExecutionEnd m_onExecutionEndHandler;


		public ExecutingTimerControlDialog()
		{
			InitializeComponent();
			m_onExecutionChangeHandler = context_ExecutionChange;
			m_onExecutionEndHandler = context_ExecutionEnd;
		}

		public int TimerCount
		{
			get { return listBoxTimers.Items.Count; }
		}

		public void AddTimer(TimerContext context)
		{
			IExecutable executable;
			Exception exception;
			if (Host.GetDebugValue("TimerTrace") != null)
			{
				Host.LogTo(Paths.TimerTraceFilePath,
				           string.Format("Executing timer for {0} at {1}", context.Timer.ProgramName, DateTime.Now));
			}
			if (!CanExecute(context))
			{
				return;
			}
			try
			{
				switch (context.Timer.ObjectType)
				{
					case ObjectType.Sequence:
						executable = new EventSequence(context.Timer.ProgramFileName);
						goto Label_00FC;

					case ObjectType.Program:
						executable = new SequenceProgram(context.Timer.ProgramFileName);
						goto Label_00FC;
				}
				MessageBox.Show("A timer with an unknown object tried to execute.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
				executable = null;
			}
			catch (Exception exception1)
			{
				exception = exception1;
				context.Timer.NotValidUntil = DateTime.Today + context.Timer.EndTime;
				MessageBox.Show(
					string.Format("There was an error trying to load a scheduled item:\n{0}\n\nThe error was:\n{1}",
					              context.Timer.ProgramFileName, exception.Message), Vendor.ProductName, MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
				executable = null;
			}
			Label_00FC:
			if (executable != null)
			{
				try
				{
					context.ExecutionInterface.SetSynchronousContext(context.ExecutionContextHandle, executable);
				}
				catch (Exception exception2)
				{
					exception = exception2;
					MessageBox.Show(exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				context.ExecutionChange += m_onExecutionChangeHandler;
				context.ExecutionEnd += m_onExecutionEndHandler;
				listBoxTimers.Items.Add(context);
				if (listBoxTimers.Items.Count == 1)
				{
					base.Show();
				}
				ExecuteTimer(context);
			}
		}

		private void buttonPause_Click(object sender, EventArgs e)
		{
			var selectedItem = (TimerContext) listBoxTimers.SelectedItem;
			selectedItem.ExecutionInterface.ExecutePause(selectedItem.ExecutionContextHandle);
		}

		private void buttonPauseAll_Click(object sender, EventArgs e)
		{
			foreach (TimerContext context in listBoxTimers.Items)
			{
				context.ExecutionInterface.ExecutePause(context.ExecutionContextHandle);
			}
		}

		private void buttonResume_Click(object sender, EventArgs e)
		{
			var selectedItem = (TimerContext) listBoxTimers.SelectedItem;
			int sequencePosition;
			if (selectedItem.ExecutionInterface.EngineStatus(selectedItem.ExecutionContextHandle, out sequencePosition) == 2)
			{
				selectedItem.ExecutionInterface.ExecutePlay(selectedItem.ExecutionContextHandle, sequencePosition, 0);
			}
		}

		private void buttonResumeAll_Click(object sender, EventArgs e)
		{
			var list = new List<TimerContext>();
			foreach (TimerContext context in listBoxTimers.Items)
			{
				list.Add(context);
			}
			int sequencePosition;
			foreach (TimerContext context in list)
			{
				if (context.ExecutionInterface.EngineStatus(context.ExecutionContextHandle, out sequencePosition) == 2)
				{
					context.ExecutionInterface.ExecutePlay(context.ExecutionContextHandle, sequencePosition, 0);
				}
			}
		}

		private void buttonStop_Click(object sender, EventArgs e)
		{
			StopExecutingTimer((TimerContext) listBoxTimers.SelectedItem);
		}

		private void buttonStopAll_Click(object sender, EventArgs e)
		{
			StopAll();
		}

		private bool CanExecute(TimerContext context)
		{
			if (Host.GetDebugValue("TimerTrace") != null)
			{
				Host.LogTo(Paths.TimerTraceFilePath, "Can execute?");
				Host.LogTo(Paths.TimerTraceFilePath, "  Stopping: " + context.Stopping.ToString());
				Host.LogTo(Paths.TimerTraceFilePath,
				           string.Format("  Timer length / Object length: {0}/{1} ({2})", context.Timer.TimerLength,
				                         context.Timer.ObjectLength, context.Timer.TimerLength >= context.Timer.ObjectLength));
				Host.LogTo(Paths.TimerTraceFilePath,
				           string.Format("  Now / end date and time: {0}/{1} ({2})", DateTime.Now, context.EndDateTime,
				                         DateTime.Now < context.EndDateTime));
			}
			return ((!context.Stopping && (context.Timer.TimerLength >= context.Timer.ObjectLength)) &&
			        (DateTime.Now < context.EndDateTime));
		}

		private bool CanLoop(TimerContext context)
		{
			if (Host.GetDebugValue("TimerTrace") != null)
			{
				Host.LogTo(Paths.TimerTraceFilePath, "Can loop?");
				Host.LogTo(Paths.TimerTraceFilePath, "  Repeat interval: " + context.Timer.RepeatInterval.ToString());
				Host.LogTo(Paths.TimerTraceFilePath,
				           string.Format("  Timer length / Object length: {0}/{1} ({2})", context.Timer.TimerLength,
				                         context.Timer.ObjectLength, context.Timer.TimerLength >= context.Timer.ObjectLength));
				Host.LogTo(Paths.TimerTraceFilePath,
				           string.Format("  {0} - {1} <= {2} ({3})",
				                         new object[]
					                         {
						                         context.Timer.ObjectLength.TotalMilliseconds,
						                         context.ExecutionInterface.GetObjectPosition(context.ExecutionContextHandle),
						                         TimeLeftInTimer(context),
						                         (context.Timer.ObjectLength.TotalMilliseconds -
						                          context.ExecutionInterface.GetObjectPosition(context.ExecutionContextHandle)) <=
						                         TimeLeftInTimer(context)
					                         }));
			}
			return ((((context.Timer.RepeatInterval == 0) && !context.Stopping) &&
			         (context.Timer.TimerLength > context.Timer.ObjectLength)) &&
			        ((context.Timer.ObjectLength.TotalMilliseconds -
			          context.ExecutionInterface.GetObjectPosition(context.ExecutionContextHandle)) <= TimeLeftInTimer(context)));
		}

		private void context_ExecutionChange(TimerContext context)
		{
			if (base.InvokeRequired)
			{
				base.BeginInvoke(m_onExecutionChangeHandler, new object[] {context});
			}
			else
			{
				int index = listBoxTimers.Items.IndexOf(context);
				listBoxTimers.BeginUpdate();
				listBoxTimers.Items.Remove(context);
				listBoxTimers.Items.Insert(index, context);
				listBoxTimers.EndUpdate();
				context.ExecutionInterface.SetLoopState(context.ExecutionContextHandle, CanLoop(context));
			}
		}

		private void context_ExecutionEnd(TimerContext context)
		{
			MethodInvoker method = null;
			if (base.InvokeRequired)
			{
				if (method == null)
				{
					method = delegate { RemoveTimer(context); };
				}
				base.Invoke(method);
			}
			else
			{
				RemoveTimer(context);
			}
		}


		private void ExecuteTimer(object contextObject)
		{
			var context = (TimerContext) contextObject;
			context.Timer.IsExecuting = true;
			context.Timer.LastExecution = DateTime.Now;
			if (
				!context.ExecutionInterface.ExecutePlay(context.ExecutionContextHandle,
				                                        ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean(
					                                        "LogAudioScheduled")))
			{
				MessageBox.Show(
					string.Format("There was a problem trying to execute the sequence \"{0}\".", context.Timer.ProgramName),
					Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RemoveTimer(context);
			}
			else
			{
				context.ExecutionInterface.SetLoopState(context.ExecutionContextHandle, CanLoop(context));
			}
		}

		private void ExecutingTimerControlDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (listBoxTimers.Items.Count > 0)
			{
				if (
					MessageBox.Show("This will cause all executing timers to stop.\n\nDo you wish to do this?", Vendor.ProductName,
					                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
				{
					StopAll();
				}
			}
			else
			{
				base.Hide();
			}
			e.Cancel = true;
		}

		private void ExecutingTimerControlDialog_VisibleChanged(object sender, EventArgs e)
		{
			timerWatchdog.Enabled = base.Visible;
		}

		public TimerContext GetContextOf(int executingTimerIndex)
		{
			if (executingTimerIndex < listBoxTimers.Items.Count)
			{
				return (TimerContext) listBoxTimers.Items[executingTimerIndex];
			}
			return null;
		}


		private void listBoxTimers_SelectedIndexChanged(object sender, EventArgs e)
		{
			buttonPause.Enabled = buttonResume.Enabled = buttonStop.Enabled = listBoxTimers.SelectedIndex != -1;
		}

		private void RemoveTimer(TimerContext context)
		{
			context.Timer.IsExecuting = false;
			context.Stopping = false;
			context.ExecutionChange -= m_onExecutionChangeHandler;
			context.ExecutionEnd -= m_onExecutionEndHandler;
			listBoxTimers.Items.Remove(context);
			if (listBoxTimers.Items.Count == 0)
			{
				base.Hide();
			}
			context.Dispose();
		}

		private void StopAll()
		{
			var list = new List<TimerContext>();
			foreach (TimerContext context in listBoxTimers.Items)
			{
				list.Add(context);
			}
			foreach (TimerContext context in list)
			{
				StopExecutingTimer(context);
			}
		}

		private void StopExecutingTimer(TimerContext context)
		{
			context.Stopping = true;
			if (context.ExecutionInterface.EngineStatus(context.ExecutionContextHandle) != 0)
			{
				context.ExecutionInterface.ExecuteStop(context.ExecutionContextHandle);
				context.Timer.NotValidUntil = DateTime.Today + context.Timer.EndTime;
			}
		}

		private int TimeLeftInTimer(TimerContext context)
		{
			return (int) context.EndDateTime.Subtract(DateTime.Now).TotalMilliseconds;
		}

		private void timerWatchdog_Tick(object sender, EventArgs e)
		{
			var list = new List<TimerContext>();
			foreach (TimerContext context in listBoxTimers.Items)
			{
				list.Add(context);
			}
			foreach (TimerContext context in list)
			{
				if ((((context.Timer.ObjectLength.TotalMilliseconds == 0.0) && !context.Stopping) &&
				     (context.ExecutionInterface.EngineStatus(context.ExecutionContextHandle) != 0)) &&
				    (DateTime.Now > context.EndDateTime))
				{
					StopExecutingTimer(context);
				}
			}
		}
	}
}