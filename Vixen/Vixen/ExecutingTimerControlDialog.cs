namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class ExecutingTimerControlDialog : Form
    {
        private Button buttonPause;
        private Button buttonPauseAll;
        private Button buttonResume;
        private Button buttonResumeAll;
        private Button buttonStop;
        private Button buttonStopAll;
        private IContainer components = null;
        private GroupBox groupBox1;
        private ListBox listBoxTimers;
        private TimerContext.OnExecutionChange m_onExecutionChangeHandler;
        private TimerContext.OnExecutionEnd m_onExecutionEndHandler;
        private System.Windows.Forms.Timer timerWatchdog;

        public ExecutingTimerControlDialog()
        {
            this.InitializeComponent();
            this.m_onExecutionChangeHandler = new TimerContext.OnExecutionChange(this.context_ExecutionChange);
            this.m_onExecutionEndHandler = new TimerContext.OnExecutionEnd(this.context_ExecutionEnd);
        }

        public void AddTimer(TimerContext context)
        {
            IExecutable executable;
            Exception exception;
            if (Host.GetDebugValue("TimerTrace") != null)
            {
                Host.LogTo(Paths.TimerTraceFilePath, string.Format("Executing timer for {0} at {1}", context.Timer.ProgramName, DateTime.Now));
            }
            if (!this.CanExecute(context))
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
                MessageBox.Show("A timer with an unknown object tried to execute.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                executable = null;
            }
            catch (Exception exception1)
            {
                exception = exception1;
                context.Timer.NotValidUntil = DateTime.Today + context.Timer.EndTime;
                MessageBox.Show(string.Format("There was an error trying to load a scheduled item:\n{0}\n\nThe error was:\n{1}", context.Timer.ProgramFileName, exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                context.ExecutionChange += this.m_onExecutionChangeHandler;
                context.ExecutionEnd += this.m_onExecutionEndHandler;
                this.listBoxTimers.Items.Add(context);
                if (this.listBoxTimers.Items.Count == 1)
                {
                    base.Show();
                }
                this.ExecuteTimer(context);
            }
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            TimerContext selectedItem = (TimerContext) this.listBoxTimers.SelectedItem;
            selectedItem.ExecutionInterface.ExecutePause(selectedItem.ExecutionContextHandle);
        }

        private void buttonPauseAll_Click(object sender, EventArgs e)
        {
            foreach (TimerContext context in this.listBoxTimers.Items)
            {
                context.ExecutionInterface.ExecutePause(context.ExecutionContextHandle);
            }
        }

        private void buttonResume_Click(object sender, EventArgs e)
        {
            TimerContext selectedItem = (TimerContext) this.listBoxTimers.SelectedItem;
            int sequencePosition = 0;
            if (selectedItem.ExecutionInterface.EngineStatus(selectedItem.ExecutionContextHandle, out sequencePosition) == 2)
            {
                selectedItem.ExecutionInterface.ExecutePlay(selectedItem.ExecutionContextHandle, sequencePosition, 0);
            }
        }

        private void buttonResumeAll_Click(object sender, EventArgs e)
        {
            List<TimerContext> list = new List<TimerContext>();
            foreach (TimerContext context in this.listBoxTimers.Items)
            {
                list.Add(context);
            }
            int sequencePosition = 0;
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
            this.StopExecutingTimer((TimerContext) this.listBoxTimers.SelectedItem);
        }

        private void buttonStopAll_Click(object sender, EventArgs e)
        {
            this.StopAll();
        }

        private bool CanExecute(TimerContext context)
        {
            if (Host.GetDebugValue("TimerTrace") != null)
            {
                Host.LogTo(Paths.TimerTraceFilePath, "Can execute?");
                Host.LogTo(Paths.TimerTraceFilePath, "  Stopping: " + context.Stopping.ToString());
                Host.LogTo(Paths.TimerTraceFilePath, string.Format("  Timer length / Object length: {0}/{1} ({2})", context.Timer.TimerLength, context.Timer.ObjectLength, context.Timer.TimerLength >= context.Timer.ObjectLength));
                Host.LogTo(Paths.TimerTraceFilePath, string.Format("  Now / end date and time: {0}/{1} ({2})", DateTime.Now, context.EndDateTime, DateTime.Now < context.EndDateTime));
            }
            return ((!context.Stopping && (context.Timer.TimerLength >= context.Timer.ObjectLength)) && (DateTime.Now < context.EndDateTime));
        }

        private bool CanLoop(TimerContext context)
        {
            if (Host.GetDebugValue("TimerTrace") != null)
            {
                Host.LogTo(Paths.TimerTraceFilePath, "Can loop?");
                Host.LogTo(Paths.TimerTraceFilePath, "  Repeat interval: " + context.Timer.RepeatInterval.ToString());
                Host.LogTo(Paths.TimerTraceFilePath, string.Format("  Timer length / Object length: {0}/{1} ({2})", context.Timer.TimerLength, context.Timer.ObjectLength, context.Timer.TimerLength >= context.Timer.ObjectLength));
                Host.LogTo(Paths.TimerTraceFilePath, string.Format("  {0} - {1} <= {2} ({3})", new object[] { context.Timer.ObjectLength.TotalMilliseconds, context.ExecutionInterface.GetObjectPosition(context.ExecutionContextHandle), this.TimeLeftInTimer(context), (context.Timer.ObjectLength.TotalMilliseconds - context.ExecutionInterface.GetObjectPosition(context.ExecutionContextHandle)) <= this.TimeLeftInTimer(context) }));
            }
            return ((((context.Timer.RepeatInterval == 0) && !context.Stopping) && (context.Timer.TimerLength > context.Timer.ObjectLength)) && ((context.Timer.ObjectLength.TotalMilliseconds - context.ExecutionInterface.GetObjectPosition(context.ExecutionContextHandle)) <= this.TimeLeftInTimer(context)));
        }

        private void context_ExecutionChange(TimerContext context)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(this.m_onExecutionChangeHandler, new object[] { context });
            }
            else
            {
                int index = this.listBoxTimers.Items.IndexOf(context);
                this.listBoxTimers.BeginUpdate();
                this.listBoxTimers.Items.Remove(context);
                this.listBoxTimers.Items.Insert(index, context);
                this.listBoxTimers.EndUpdate();
                context.ExecutionInterface.SetLoopState(context.ExecutionContextHandle, this.CanLoop(context));
            }
        }

        private void context_ExecutionEnd(TimerContext context)
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.RemoveTimer(context);
                    };
                }
                base.Invoke(method);
            }
            else
            {
                this.RemoveTimer(context);
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

        private void ExecuteTimer(object contextObject)
        {
            TimerContext context = (TimerContext) contextObject;
            context.Timer.IsExecuting = true;
            context.Timer.LastExecution = DateTime.Now;
            if (!context.ExecutionInterface.ExecutePlay(context.ExecutionContextHandle, ((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("LogAudioScheduled")))
            {
                MessageBox.Show(string.Format("There was a problem trying to execute the sequence \"{0}\".", context.Timer.ProgramName), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.RemoveTimer(context);
            }
            else
            {
                context.ExecutionInterface.SetLoopState(context.ExecutionContextHandle, this.CanLoop(context));
            }
        }

        private void ExecutingTimerControlDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.listBoxTimers.Items.Count > 0)
            {
                if (MessageBox.Show("This will cause all executing timers to stop.\n\nDo you wish to do this?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    this.StopAll();
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
            this.timerWatchdog.Enabled = base.Visible;
        }

        public TimerContext GetContextOf(int executingTimerIndex)
        {
            if (executingTimerIndex < this.listBoxTimers.Items.Count)
            {
                return (TimerContext) this.listBoxTimers.Items[executingTimerIndex];
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.groupBox1 = new GroupBox();
            this.listBoxTimers = new ListBox();
            this.buttonPause = new Button();
            this.buttonResume = new Button();
            this.buttonStop = new Button();
            this.buttonPauseAll = new Button();
            this.buttonResumeAll = new Button();
            this.buttonStopAll = new Button();
            this.timerWatchdog = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.listBoxTimers);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(200, 0x79);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Executing Timers";
            this.listBoxTimers.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxTimers.FormattingEnabled = true;
            this.listBoxTimers.Location = new Point(6, 0x13);
            this.listBoxTimers.Name = "listBoxTimers";
            this.listBoxTimers.Size = new Size(0xbc, 0x52);
            this.listBoxTimers.TabIndex = 0;
            this.listBoxTimers.SelectedIndexChanged += new EventHandler(this.listBoxTimers_SelectedIndexChanged);
            this.buttonPause.Enabled = false;
            this.buttonPause.Location = new Point(0xf2, 0x1f);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new Size(0x4b, 0x17);
            this.buttonPause.TabIndex = 1;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new EventHandler(this.buttonPause_Click);
            this.buttonResume.Enabled = false;
            this.buttonResume.Location = new Point(0xf2, 60);
            this.buttonResume.Name = "buttonResume";
            this.buttonResume.Size = new Size(0x4b, 0x17);
            this.buttonResume.TabIndex = 2;
            this.buttonResume.Text = "Resume";
            this.buttonResume.UseVisualStyleBackColor = true;
            this.buttonResume.Click += new EventHandler(this.buttonResume_Click);
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new Point(0xf2, 0x59);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new Size(0x4b, 0x17);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new EventHandler(this.buttonStop_Click);
            this.buttonPauseAll.Location = new Point(0x161, 0x1f);
            this.buttonPauseAll.Name = "buttonPauseAll";
            this.buttonPauseAll.Size = new Size(0x4b, 0x17);
            this.buttonPauseAll.TabIndex = 4;
            this.buttonPauseAll.Text = "Pause all";
            this.buttonPauseAll.UseVisualStyleBackColor = true;
            this.buttonPauseAll.Click += new EventHandler(this.buttonPauseAll_Click);
            this.buttonResumeAll.Location = new Point(0x161, 60);
            this.buttonResumeAll.Name = "buttonResumeAll";
            this.buttonResumeAll.Size = new Size(0x4b, 0x17);
            this.buttonResumeAll.TabIndex = 5;
            this.buttonResumeAll.Text = "Resume all";
            this.buttonResumeAll.UseVisualStyleBackColor = true;
            this.buttonResumeAll.Click += new EventHandler(this.buttonResumeAll_Click);
            this.buttonStopAll.Location = new Point(0x161, 0x59);
            this.buttonStopAll.Name = "buttonStopAll";
            this.buttonStopAll.Size = new Size(0x4b, 0x17);
            this.buttonStopAll.TabIndex = 6;
            this.buttonStopAll.Text = "Stop all";
            this.buttonStopAll.UseVisualStyleBackColor = true;
            this.buttonStopAll.Click += new EventHandler(this.buttonStopAll_Click);
            this.timerWatchdog.Interval = 0x3e8;
            this.timerWatchdog.Tick += new EventHandler(this.timerWatchdog_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1c7, 0x91);
            base.Controls.Add(this.buttonStopAll);
            base.Controls.Add(this.buttonResumeAll);
            base.Controls.Add(this.buttonPauseAll);
            base.Controls.Add(this.buttonStop);
            base.Controls.Add(this.buttonResume);
            base.Controls.Add(this.buttonPause);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "ExecutingTimerControlDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Executing Timer Control";
            base.TopMost = true;
            base.VisibleChanged += new EventHandler(this.ExecutingTimerControlDialog_VisibleChanged);
            base.FormClosing += new FormClosingEventHandler(this.ExecutingTimerControlDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void listBoxTimers_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonPause.Enabled = this.buttonResume.Enabled = this.buttonStop.Enabled = this.listBoxTimers.SelectedIndex != -1;
        }

        private void RemoveTimer(TimerContext context)
        {
            context.Timer.IsExecuting = false;
            context.Stopping = false;
            context.ExecutionChange -= this.m_onExecutionChangeHandler;
            context.ExecutionEnd -= this.m_onExecutionEndHandler;
            this.listBoxTimers.Items.Remove(context);
            if (this.listBoxTimers.Items.Count == 0)
            {
                base.Hide();
            }
            context.Dispose();
        }

        private void StopAll()
        {
            List<TimerContext> list = new List<TimerContext>();
            foreach (TimerContext context in this.listBoxTimers.Items)
            {
                list.Add(context);
            }
            foreach (TimerContext context in list)
            {
                this.StopExecutingTimer(context);
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
            List<TimerContext> list = new List<TimerContext>();
            foreach (TimerContext context in this.listBoxTimers.Items)
            {
                list.Add(context);
            }
            foreach (TimerContext context in list)
            {
                if ((((context.Timer.ObjectLength.TotalMilliseconds == 0.0) && !context.Stopping) && (context.ExecutionInterface.EngineStatus(context.ExecutionContextHandle) != 0)) && (DateTime.Now > context.EndDateTime))
                {
                    this.StopExecutingTimer(context);
                }
            }
        }

        public int TimerCount
        {
            get
            {
                return this.listBoxTimers.Items.Count;
            }
        }
    }
}

