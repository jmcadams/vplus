using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus
{
	internal partial class ScheduleDialog
    {
        private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private CheckBox checkBoxDisableSchedule;
private ContextMenuStrip contextMenuStrip;
private MenuStrip menuStrip;
private Panel panel1;
private ToolStrip toolStrip1;
private ToolStripButton toolStripButtonAgendaView;
private ToolStripButton toolStripButtonDayView;
private ToolStripButton toolStripButtonMonthView;
private ToolStripButton toolStripButtonToday;
private ToolStripButton toolStripButtonWeekView;
private ToolStripMenuItem toolStripMenuItemAddEdit;
private ToolStripMenuItem toolStripMenuItemRemove;
private ToolStripSeparator toolStripSeparator1;
private ToolStripSeparator toolStripSeparator2;
private ToolTip toolTip;
private VScrollBar vScrollBar;

		private void InitializeComponent()
        {
            this.components = new Container();
            this.menuStrip = new MenuStrip();
            this.toolStrip1 = new ToolStrip();
            this.toolStripButtonToday = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.toolStripButtonDayView = new ToolStripButton();
            this.toolStripButtonWeekView = new ToolStripButton();
            this.toolStripButtonMonthView = new ToolStripButton();
            this.toolStripButtonAgendaView = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.panel1 = new Panel();
            this.checkBoxDisableSchedule = new CheckBox();
            this.buttonCancel = new Button();
            this.buttonOK = new Button();
            this.vScrollBar = new VScrollBar();
            this.contextMenuStrip = new ContextMenuStrip(this.components);
            this.toolStripMenuItemAddEdit = new ToolStripMenuItem();
            this.toolStripMenuItemRemove = new ToolStripMenuItem();
            this.toolTip = new ToolTip(this.components);
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            base.SuspendLayout();
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(0x269, 0x18);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            this.menuStrip.Visible = false;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripButtonToday, this.toolStripSeparator1, this.toolStripButtonDayView, this.toolStripButtonWeekView, this.toolStripButtonMonthView, this.toolStripButtonAgendaView, this.toolStripSeparator2 });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x269, 0x19);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStripButtonToday.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripButtonToday.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonToday.Name = "toolStripButtonToday";
            this.toolStripButtonToday.Size = new Size(0x2c, 0x16);
            this.toolStripButtonToday.Text = "Today";
            this.toolStripButtonToday.ToolTipText = "Go to today";
            this.toolStripButtonToday.Click += new EventHandler(this.toolStripButtonToday_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.toolStripButtonDayView.CheckOnClick = true;
            this.toolStripButtonDayView.ImageTransparentColor = Color.White;
            this.toolStripButtonDayView.Name = "toolStripButtonDayView";
            this.toolStripButtonDayView.Size = new Size(0x4a, 0x16);
            this.toolStripButtonDayView.Text = "Day view";
            this.toolStripButtonDayView.Click += new EventHandler(this.toolStripButtonDayView_Click);
            this.toolStripButtonWeekView.CheckOnClick = true;
            this.toolStripButtonWeekView.ImageTransparentColor = Color.White;
            this.toolStripButtonWeekView.Name = "toolStripButtonWeekView";
            this.toolStripButtonWeekView.Size = new Size(0x53, 0x16);
            this.toolStripButtonWeekView.Text = "Week view";
            this.toolStripButtonWeekView.Click += new EventHandler(this.toolStripButtonWeekView_Click);
            this.toolStripButtonMonthView.CheckOnClick = true;
            this.toolStripButtonMonthView.ImageTransparentColor = Color.White;
            this.toolStripButtonMonthView.Name = "toolStripButtonMonthView";
            this.toolStripButtonMonthView.Size = new Size(90, 0x16);
            this.toolStripButtonMonthView.Text = "Month view";
            this.toolStripButtonMonthView.Click += new EventHandler(this.toolStripButtonMonthView_Click);
            this.toolStripButtonAgendaView.CheckOnClick = true;
            this.toolStripButtonAgendaView.ImageTransparentColor = Color.White;
            this.toolStripButtonAgendaView.Name = "toolStripButtonAgendaView";
            this.toolStripButtonAgendaView.Size = new Size(0x5f, 0x16);
            this.toolStripButtonAgendaView.Text = "Agenda view";
            this.toolStripButtonAgendaView.Click += new EventHandler(this.toolStripButtonAgendaView_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.panel1.Controls.Add(this.checkBoxDisableSchedule);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x1eb);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x269, 0x2d);
            this.panel1.TabIndex = 9;
            this.checkBoxDisableSchedule.AutoSize = true;
            this.checkBoxDisableSchedule.Location = new Point(12, 14);
            this.checkBoxDisableSchedule.Name = "checkBoxDisableSchedule";
            this.checkBoxDisableSchedule.Size = new Size(0x7d, 0x11);
            this.checkBoxDisableSchedule.TabIndex = 2;
            this.checkBoxDisableSchedule.Text = "Disable the schedule";
            this.checkBoxDisableSchedule.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(530, 10);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x1c1, 10);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.vScrollBar.Dock = DockStyle.Right;
            this.vScrollBar.Location = new Point(600, 0x19);
            this.vScrollBar.Maximum = 0x30;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new Size(0x11, 0x1d2);
            this.vScrollBar.TabIndex = 10;
            this.vScrollBar.ValueChanged += new EventHandler(this.vScrollBar_ValueChanged);
            this.contextMenuStrip.Items.AddRange(new ToolStripItem[] { this.toolStripMenuItemAddEdit, this.toolStripMenuItemRemove });
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new Size(0x76, 0x30);
            this.contextMenuStrip.Opening += new CancelEventHandler(this.contextMenuStrip_Opening);
            this.toolStripMenuItemAddEdit.Name = "toolStripMenuItemAddEdit";
            this.toolStripMenuItemAddEdit.Size = new Size(0x75, 0x16);
            this.toolStripMenuItemAddEdit.Text = "Edit";
            this.toolStripMenuItemAddEdit.Click += new EventHandler(this.toolStripMenuItemEdit_Click);
            this.toolStripMenuItemRemove.Name = "toolStripMenuItemRemove";
            this.toolStripMenuItemRemove.Size = new Size(0x75, 0x16);
            this.toolStripMenuItemRemove.Text = "Remove";
            this.toolStripMenuItemRemove.Click += new EventHandler(this.toolStripMenuItemRemove_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x269, 0x218);
            this.ContextMenuStrip = this.contextMenuStrip;
            base.Controls.Add(this.vScrollBar);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            base.MainMenuStrip = this.menuStrip;
            base.Name = "ScheduleDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Scheduler";
            base.MouseDoubleClick += new MouseEventHandler(this.ScheduleDialog_MouseDoubleClick);
            base.ResizeBegin += new EventHandler(this.ScheduleDialog_ResizeBegin);
            base.Resize += new EventHandler(this.ScheduleDialog_Resize);
            base.MouseMove += new MouseEventHandler(this.ScheduleDialog_MouseMove);
            base.ResizeEnd += new EventHandler(this.ScheduleDialog_ResizeEnd);
            base.MouseDown += new MouseEventHandler(this.ScheduleDialog_MouseDown);
            base.Load += new EventHandler(this.ScheduleDialog_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
		#endregion

		protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            this._backgroundBrush.Dispose();
            this._hourPen.Dispose();
            this._halfHourPen.Dispose();
            this._timeLargeFont.Dispose();
            this._timeSmallFont.Dispose();
            this._dayViewHeaderFont.Dispose();
            this._agendaViewItemFont.Dispose();
            this._agendaViewTimeFont.Dispose();
            this._timeLinePen.Dispose();
            base.Dispose(disposing);
        }
	}
}
