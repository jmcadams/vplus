using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus
{
	internal partial class DiagnosticsDialog
    {
        private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonDone;
private Button buttonDumpTimers;
private Button buttonShowOutputPluginDurations;
private CheckBox checkBoxGetEventAverages;
private CheckBox checkBoxTraceTimers;
private DateTimePicker dateTimePickerTimerTraceFrom;
private DateTimePicker dateTimePickerTimerTraceTo;
private Label label1;
private System.Windows.Forms.TabControl tcPage;
private TabPage tabPageOutputPlugins;
private TabPage tabPageTimers;

		private void InitializeComponent()
        {
            this.tcPage = new System.Windows.Forms.TabControl();
            this.tabPageTimers = new TabPage();
            this.dateTimePickerTimerTraceTo = new DateTimePicker();
            this.label1 = new Label();
            this.dateTimePickerTimerTraceFrom = new DateTimePicker();
            this.checkBoxTraceTimers = new CheckBox();
            this.buttonDumpTimers = new Button();
            this.tabPageOutputPlugins = new TabPage();
            this.buttonShowOutputPluginDurations = new Button();
            this.checkBoxGetEventAverages = new CheckBox();
            this.buttonDone = new Button();
            this.tcPage.SuspendLayout();
            this.tabPageTimers.SuspendLayout();
            this.tabPageOutputPlugins.SuspendLayout();
            base.SuspendLayout();
            this.tcPage.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tcPage.Controls.Add(this.tabPageTimers);
            this.tcPage.Controls.Add(this.tabPageOutputPlugins);
            this.tcPage.Location = new Point(12, 12);
            this.tcPage.Name = "tabControl";
            this.tcPage.SelectedIndex = 0;
            this.tcPage.Size = new Size(0x1e5, 0xff);
            this.tcPage.TabIndex = 1;
            this.tabPageTimers.Controls.Add(this.dateTimePickerTimerTraceTo);
            this.tabPageTimers.Controls.Add(this.label1);
            this.tabPageTimers.Controls.Add(this.dateTimePickerTimerTraceFrom);
            this.tabPageTimers.Controls.Add(this.checkBoxTraceTimers);
            this.tabPageTimers.Controls.Add(this.buttonDumpTimers);
            this.tabPageTimers.Location = new Point(4, 0x16);
            this.tabPageTimers.Name = "tabPageTimers";
            this.tabPageTimers.Padding = new Padding(3);
            this.tabPageTimers.Size = new Size(0x1dd, 0xe5);
            this.tabPageTimers.TabIndex = 0;
            this.tabPageTimers.Text = "Timers";
            this.tabPageTimers.UseVisualStyleBackColor = true;
            this.dateTimePickerTimerTraceTo.Format = DateTimePickerFormat.Time;
            this.dateTimePickerTimerTraceTo.Location = new Point(0x11e, 0x55);
            this.dateTimePickerTimerTraceTo.Name = "dateTimePickerTimerTraceTo";
            this.dateTimePickerTimerTraceTo.Size = new Size(0x65, 20);
            this.dateTimePickerTimerTraceTo.TabIndex = 4;
            this.dateTimePickerTimerTraceTo.Value = new DateTime(0x7d7, 12, 9, 0x16, 0, 0, 0);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x108, 0x59);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x10, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "to";
            this.dateTimePickerTimerTraceFrom.Format = DateTimePickerFormat.Time;
            this.dateTimePickerTimerTraceFrom.Location = new Point(0x9d, 0x55);
            this.dateTimePickerTimerTraceFrom.Name = "dateTimePickerTimerTraceFrom";
            this.dateTimePickerTimerTraceFrom.Size = new Size(0x65, 20);
            this.dateTimePickerTimerTraceFrom.TabIndex = 2;
            this.dateTimePickerTimerTraceFrom.Value = new DateTime(0x7d7, 12, 9, 12, 0, 0, 0);
            this.checkBoxTraceTimers.AutoSize = true;
            this.checkBoxTraceTimers.Location = new Point(15, 0x58);
            this.checkBoxTraceTimers.Name = "checkBoxTraceTimers";
            this.checkBoxTraceTimers.Size = new Size(0x88, 0x11);
            this.checkBoxTraceTimers.TabIndex = 1;
            this.checkBoxTraceTimers.Text = "Start tracing timers from";
            this.checkBoxTraceTimers.UseVisualStyleBackColor = true;
            this.checkBoxTraceTimers.CheckedChanged += new EventHandler(this.checkBoxTraceTimers_CheckedChanged);
            this.buttonDumpTimers.Location = new Point(15, 0x23);
            this.buttonDumpTimers.Name = "buttonDumpTimers";
            this.buttonDumpTimers.Size = new Size(110, 0x17);
            this.buttonDumpTimers.TabIndex = 0;
            this.buttonDumpTimers.Text = "Dump timer states";
            this.buttonDumpTimers.UseVisualStyleBackColor = true;
            this.buttonDumpTimers.Click += new EventHandler(this.buttonDumpTimers_Click);
            this.tabPageOutputPlugins.Controls.Add(this.buttonShowOutputPluginDurations);
            this.tabPageOutputPlugins.Controls.Add(this.checkBoxGetEventAverages);
            this.tabPageOutputPlugins.Location = new Point(4, 0x16);
            this.tabPageOutputPlugins.Name = "tabPageOutputPlugins";
            this.tabPageOutputPlugins.Size = new Size(0x1dd, 0xe5);
            this.tabPageOutputPlugins.TabIndex = 1;
            this.tabPageOutputPlugins.Text = "Output plugins";
            this.tabPageOutputPlugins.UseVisualStyleBackColor = true;
            this.buttonShowOutputPluginDurations.Enabled = false;
            this.buttonShowOutputPluginDurations.Location = new Point(0x143, 40);
            this.buttonShowOutputPluginDurations.Name = "buttonShowOutputPluginDurations";
            this.buttonShowOutputPluginDurations.Size = new Size(0x4b, 0x17);
            this.buttonShowOutputPluginDurations.TabIndex = 1;
            this.buttonShowOutputPluginDurations.Text = "Show";
            this.buttonShowOutputPluginDurations.UseVisualStyleBackColor = true;
            this.buttonShowOutputPluginDurations.Click += new EventHandler(this.buttonShowOutputPluginDurations_Click);
            this.checkBoxGetEventAverages.AutoSize = true;
            this.checkBoxGetEventAverages.Location = new Point(0x24, 0x2c);
            this.checkBoxGetEventAverages.Name = "checkBoxGetEventAverages";
            this.checkBoxGetEventAverages.Size = new Size(0xea, 0x11);
            this.checkBoxGetEventAverages.TabIndex = 0;
            this.checkBoxGetEventAverages.Text = "Get average event durations for each plugin";
            this.checkBoxGetEventAverages.UseVisualStyleBackColor = true;
            this.checkBoxGetEventAverages.CheckedChanged += new EventHandler(this.checkBoxGetEventAverages_CheckedChanged);
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDone.Location = new Point(0x1a6, 0x111);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 2;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            base.AcceptButton = this.buttonDone;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonDone;
            base.ClientSize = new Size(0x1fd, 0x134);
            base.Controls.Add(this.buttonDone);
            base.Controls.Add(this.tcPage);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "DiagnosticsDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Diagnostics";
            this.tcPage.ResumeLayout(false);
            this.tabPageTimers.ResumeLayout(false);
            this.tabPageTimers.PerformLayout();
            this.tabPageOutputPlugins.ResumeLayout(false);
            this.tabPageOutputPlugins.PerformLayout();
            base.ResumeLayout(false);
        }
		#endregion

		protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}
