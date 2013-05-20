using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus {
    internal partial class DiagnosticsDialog {
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


        private void InitializeComponent() {
            this.tcPage = new System.Windows.Forms.TabControl();
            this.tabPageTimers = new System.Windows.Forms.TabPage();
            this.dateTimePickerTimerTraceTo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerTimerTraceFrom = new System.Windows.Forms.DateTimePicker();
            this.checkBoxTraceTimers = new System.Windows.Forms.CheckBox();
            this.buttonDumpTimers = new System.Windows.Forms.Button();
            this.tabPageOutputPlugins = new System.Windows.Forms.TabPage();
            this.buttonShowOutputPluginDurations = new System.Windows.Forms.Button();
            this.checkBoxGetEventAverages = new System.Windows.Forms.CheckBox();
            this.buttonDone = new System.Windows.Forms.Button();
            this.tcPage.SuspendLayout();
            this.tabPageTimers.SuspendLayout();
            this.tabPageOutputPlugins.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcPage
            // 
            this.tcPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcPage.Controls.Add(this.tabPageTimers);
            this.tcPage.Controls.Add(this.tabPageOutputPlugins);
            this.tcPage.Location = new System.Drawing.Point(12, 12);
            this.tcPage.Name = "tcPage";
            this.tcPage.SelectedIndex = 0;
            this.tcPage.Size = new System.Drawing.Size(485, 255);
            this.tcPage.TabIndex = 1;
            // 
            // tabPageTimers
            // 
            this.tabPageTimers.Controls.Add(this.dateTimePickerTimerTraceTo);
            this.tabPageTimers.Controls.Add(this.label1);
            this.tabPageTimers.Controls.Add(this.dateTimePickerTimerTraceFrom);
            this.tabPageTimers.Controls.Add(this.checkBoxTraceTimers);
            this.tabPageTimers.Controls.Add(this.buttonDumpTimers);
            this.tabPageTimers.Location = new System.Drawing.Point(4, 22);
            this.tabPageTimers.Name = "tabPageTimers";
            this.tabPageTimers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTimers.Size = new System.Drawing.Size(477, 229);
            this.tabPageTimers.TabIndex = 0;
            this.tabPageTimers.Text = "Timers";
            this.tabPageTimers.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerTimerTraceTo
            // 
            this.dateTimePickerTimerTraceTo.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerTimerTraceTo.Location = new System.Drawing.Point(286, 85);
            this.dateTimePickerTimerTraceTo.Name = "dateTimePickerTimerTraceTo";
            this.dateTimePickerTimerTraceTo.Size = new System.Drawing.Size(101, 20);
            this.dateTimePickerTimerTraceTo.TabIndex = 4;
            this.dateTimePickerTimerTraceTo.Value = new System.DateTime(2007, 12, 9, 22, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(264, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "to";
            // 
            // dateTimePickerTimerTraceFrom
            // 
            this.dateTimePickerTimerTraceFrom.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerTimerTraceFrom.Location = new System.Drawing.Point(157, 85);
            this.dateTimePickerTimerTraceFrom.Name = "dateTimePickerTimerTraceFrom";
            this.dateTimePickerTimerTraceFrom.Size = new System.Drawing.Size(101, 20);
            this.dateTimePickerTimerTraceFrom.TabIndex = 2;
            this.dateTimePickerTimerTraceFrom.Value = new System.DateTime(2007, 12, 9, 12, 0, 0, 0);
            // 
            // checkBoxTraceTimers
            // 
            this.checkBoxTraceTimers.AutoSize = true;
            this.checkBoxTraceTimers.Location = new System.Drawing.Point(15, 88);
            this.checkBoxTraceTimers.Name = "checkBoxTraceTimers";
            this.checkBoxTraceTimers.Size = new System.Drawing.Size(136, 17);
            this.checkBoxTraceTimers.TabIndex = 1;
            this.checkBoxTraceTimers.Text = "Start tracing timers from";
            this.checkBoxTraceTimers.UseVisualStyleBackColor = true;
            this.checkBoxTraceTimers.CheckedChanged += new System.EventHandler(this.checkBoxTraceTimers_CheckedChanged);
            // 
            // buttonDumpTimers
            // 
            this.buttonDumpTimers.Location = new System.Drawing.Point(15, 35);
            this.buttonDumpTimers.Name = "buttonDumpTimers";
            this.buttonDumpTimers.Size = new System.Drawing.Size(110, 23);
            this.buttonDumpTimers.TabIndex = 0;
            this.buttonDumpTimers.Text = "Dump timer states";
            this.buttonDumpTimers.UseVisualStyleBackColor = true;
            this.buttonDumpTimers.Click += new System.EventHandler(this.buttonDumpTimers_Click);
            // 
            // tabPageOutputPlugins
            // 
            this.tabPageOutputPlugins.Controls.Add(this.buttonShowOutputPluginDurations);
            this.tabPageOutputPlugins.Controls.Add(this.checkBoxGetEventAverages);
            this.tabPageOutputPlugins.Location = new System.Drawing.Point(4, 22);
            this.tabPageOutputPlugins.Name = "tabPageOutputPlugins";
            this.tabPageOutputPlugins.Size = new System.Drawing.Size(477, 229);
            this.tabPageOutputPlugins.TabIndex = 1;
            this.tabPageOutputPlugins.Text = "Output plugins";
            this.tabPageOutputPlugins.UseVisualStyleBackColor = true;
            // 
            // buttonShowOutputPluginDurations
            // 
            this.buttonShowOutputPluginDurations.Enabled = false;
            this.buttonShowOutputPluginDurations.Location = new System.Drawing.Point(323, 40);
            this.buttonShowOutputPluginDurations.Name = "buttonShowOutputPluginDurations";
            this.buttonShowOutputPluginDurations.Size = new System.Drawing.Size(75, 23);
            this.buttonShowOutputPluginDurations.TabIndex = 1;
            this.buttonShowOutputPluginDurations.Text = "Show";
            this.buttonShowOutputPluginDurations.UseVisualStyleBackColor = true;
            this.buttonShowOutputPluginDurations.Click += new System.EventHandler(this.buttonShowOutputPluginDurations_Click);
            // 
            // checkBoxGetEventAverages
            // 
            this.checkBoxGetEventAverages.AutoSize = true;
            this.checkBoxGetEventAverages.Location = new System.Drawing.Point(36, 44);
            this.checkBoxGetEventAverages.Name = "checkBoxGetEventAverages";
            this.checkBoxGetEventAverages.Size = new System.Drawing.Size(234, 17);
            this.checkBoxGetEventAverages.TabIndex = 0;
            this.checkBoxGetEventAverages.Text = "Get average event durations for each plugin";
            this.checkBoxGetEventAverages.UseVisualStyleBackColor = true;
            this.checkBoxGetEventAverages.CheckedChanged += new System.EventHandler(this.checkBoxGetEventAverages_CheckedChanged);
            // 
            // buttonDone
            // 
            this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDone.Location = new System.Drawing.Point(422, 273);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 2;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            // 
            // DiagnosticsDialog
            // 
            this.AcceptButton = this.buttonDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonDone;
            this.ClientSize = new System.Drawing.Size(509, 308);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.tcPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::Properties.Resources.VixenPlus;
            this.Name = "DiagnosticsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Diagnostics";
            this.tcPage.ResumeLayout(false);
            this.tabPageTimers.ResumeLayout(false);
            this.tabPageTimers.PerformLayout();
            this.tabPageOutputPlugins.ResumeLayout(false);
            this.tabPageOutputPlugins.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
