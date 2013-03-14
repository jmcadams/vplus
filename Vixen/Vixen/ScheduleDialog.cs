namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    internal class ScheduleDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private CheckBox checkBoxDisableSchedule;
        private IContainer components = null;
        private ContextMenuStrip contextMenuStrip;
        private int m_agendaItemHeight = 50;
        private Font m_agendaViewItemFont = new Font("Arial", 10f, FontStyle.Bold);
        private int m_agendaViewScrollBarValue = 0;
        private Font m_agendaViewTimeFont = new Font("Arial", 8f);
        private List<Vixen.Timer> m_applicableTimers;
        private SolidBrush m_backgroundBrush = new SolidBrush(Color.FromArgb(0xff, 0xff, 0xd5));
        private Rectangle m_buttonLeftBounds;
        private Rectangle m_buttonRightBounds;
        private DateTime m_currentDate;
        private DateView m_currentView;
        private string[] m_days = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        private Font m_dayViewHeaderFont = new Font("Arial", 12f, FontStyle.Bold);
        private int m_dayViewScrollBarValue = 0;
        private Rectangle m_drawingArea;
        private int m_halfHourHeight = 20;
        private Pen m_halfHourPen = new Pen(Color.FromArgb(0xff, 0xef, 0xc7));
        private int m_headerHeight = 30;
        private Pen m_hourPen = new Pen(Color.FromArgb(0xf6, 0xdb, 0xa2));
        private bool m_inLeftButtonBounds = false;
        private bool m_inRightButtonBounds = false;
        private int m_monthBarHeight = 10;
        private int m_monthViewBottomMargin = 40;
        private int m_monthViewColSize;
        private int m_monthViewLeftMargin = 40;
        private int m_monthViewRightMargin = 40;
        private int m_monthViewRowSize;
        private int m_monthViewTopMargin = 40;
        private Point m_mouseDownAt;
        private TimeSpan m_oneDayTimeSpan = new TimeSpan(1, 0, 0, 0);
        private TimeSpan m_oneWeekTimeSpan = new TimeSpan(7, 0, 0, 0);
        private bool m_resizing = false;
        private int m_timeGutter = 50;
        private Font m_timeLargeFont = new Font("Tahoma", 16f);
        private Pen m_timeLinePen = new Pen(Color.FromKnownColor(KnownColor.ControlDark));
        private List<Vixen.Timer> m_timers;
        private Vixen.Timers m_timersObject;
        private Font m_timeSmallFont = new Font("Tahoma", 8f);
        private int m_weekViewScrollBarValue = 0;
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

        public ScheduleDialog(Vixen.Timers timers)
        {
            this.m_resizing = true;
            this.InitializeComponent();
            this.m_resizing = false;
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            this.m_timersObject = timers;
            this.checkBoxDisableSchedule.Checked = timers.TimersDisabled;
            this.m_timers = new List<Vixen.Timer>();
            this.m_applicableTimers = new List<Vixen.Timer>();
            foreach (Vixen.Timer timer in timers.TimerArray)
            {
                this.m_timers.Add(timer.Clone());
            }
            this.m_drawingArea = new Rectangle();
            this.SetDrawingArea();
        }

        private void AddTimer(Vixen.Timer timer)
        {
            this.m_timers.Add(timer);
        }

        private void AgendaViewCalcs()
        {
            int num = this.m_drawingArea.Top + this.m_headerHeight;
            foreach (Vixen.Timer timer in this.m_applicableTimers)
            {
                timer.DisplayBounds.Clear();
            }
            this.m_applicableTimers.Sort();
            for (int i = this.vScrollBar.Value; i < this.m_applicableTimers.Count; i++)
            {
                this.m_applicableTimers[i].DisplayBounds.Add(new ReferenceRectF((float) this.m_drawingArea.Left, (float) num, (float) this.m_drawingArea.Width, (float) this.m_agendaItemHeight));
                num += this.m_agendaItemHeight;
            }
        }

        private void CalcDayBlocksIn(List<Vixen.Timer> applicableTimers, Rectangle area, TimeSpan visibleStart)
        {
            List<ReferenceRectF> list = new List<ReferenceRectF>();
            List<ReferenceRectF> list2 = new List<ReferenceRectF>();
            foreach (Vixen.Timer timer in applicableTimers)
            {
                ReferenceRectF b = new ReferenceRectF();
                b.X = area.X;
                b.Width = area.Width;
                TimeSpan span = timer.StartTime - visibleStart;
                b.Y = ((((float) span.TotalMinutes) / 30f) * this.m_halfHourHeight) + area.Top;
                b.Height = (((float) timer.TimerLength.TotalMinutes) / 30f) * this.m_halfHourHeight;
                if (list.Count > 0)
                {
                    float right = area.Right;
                    list2.Clear();
                    foreach (ReferenceRectF tf2 in list)
                    {
                        if (ReferenceRectF.Intersects(tf2, b))
                        {
                            list2.Add(tf2);
                            right = Math.Min(tf2.X, right);
                        }
                    }
                    if (right == area.X)
                    {
                        float x = area.X;
                        float num3 = area.Width / (list2.Count + 1);
                        foreach (ReferenceRectF tf3 in list2)
                        {
                            tf3.X = x;
                            tf3.Width = num3;
                            x += num3;
                        }
                        b.X = x;
                        b.Width = num3;
                    }
                    else
                    {
                        b.Width = right - area.X;
                    }
                }
                if (b.Y < area.Y)
                {
                    b.Height -= area.Y - b.Y;
                    b.Y += area.Y - b.Y;
                }
                if (b.Height > 0f)
                {
                    list.Add(b);
                    timer.DisplayBounds.Add(b);
                }
            }
        }

        private void CompileApplicableTimers(DateTime startDateTime, TimeSpan span)
        {
            this.m_applicableTimers = this.ReturnApplicableTimersSubset(startDateTime, span, this.m_timers);
            this.ViewCalcs();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this.m_mouseDownAt = base.PointToClient(Cursor.Position);
            switch (this.m_currentView)
            {
                case DateView.Day:
                    if ((((this.vScrollBar.Value + ((this.m_mouseDownAt.Y - (this.m_drawingArea.Y + this.m_headerHeight)) / this.m_halfHourHeight)) < 0x30) && (this.m_mouseDownAt.X >= (this.m_drawingArea.X + this.m_timeGutter))) && (this.m_mouseDownAt.Y >= (this.m_drawingArea.Y + this.m_headerHeight)))
                    {
                        break;
                    }
                    e.Cancel = true;
                    return;

                case DateView.Week:
                    if ((((this.vScrollBar.Value + ((this.m_mouseDownAt.Y - ((this.m_drawingArea.Y + this.m_headerHeight) + this.m_halfHourHeight)) / this.m_halfHourHeight)) < 0x30) && (this.m_mouseDownAt.X >= (this.m_drawingArea.X + this.m_timeGutter))) && (this.m_mouseDownAt.Y >= ((this.m_drawingArea.Y + this.m_headerHeight) + this.m_halfHourHeight)))
                    {
                        break;
                    }
                    e.Cancel = true;
                    return;

                case DateView.Month:
                    e.Cancel = true;
                    return;

                case DateView.Agenda:
                    if (((this.m_mouseDownAt.X >= this.m_drawingArea.X) && (this.m_mouseDownAt.Y >= (this.m_drawingArea.Y + this.m_headerHeight))) && (((((this.m_mouseDownAt.Y - this.m_drawingArea.Y) - this.m_headerHeight) / this.m_agendaItemHeight) + this.vScrollBar.Value) < this.m_applicableTimers.Count))
                    {
                        break;
                    }
                    e.Cancel = true;
                    return;
            }
            if (this.FindApplicableTimerAt(this.m_mouseDownAt) == null)
            {
                this.toolStripMenuItemRemove.Enabled = false;
                this.toolStripMenuItemAddEdit.Text = "Add";
            }
            else
            {
                this.toolStripMenuItemRemove.Enabled = true;
                this.toolStripMenuItemAddEdit.Text = "Edit";
            }
        }

        private void DayViewCalcs()
        {
            foreach (Vixen.Timer timer in this.m_applicableTimers)
            {
                timer.DisplayBounds.Clear();
            }
            this.CalcDayBlocksIn(this.m_applicableTimers, new Rectangle(this.m_drawingArea.Left + this.m_timeGutter, this.m_drawingArea.Top + this.m_headerHeight, (this.m_drawingArea.Width - this.m_timeGutter) - 1, (this.m_drawingArea.Height - this.m_headerHeight) - 1), new TimeSpan(this.vScrollBar.Value / 2, (this.vScrollBar.Value % 2) * 30, 0));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            this.m_backgroundBrush.Dispose();
            this.m_hourPen.Dispose();
            this.m_halfHourPen.Dispose();
            this.m_timeLargeFont.Dispose();
            this.m_timeSmallFont.Dispose();
            this.m_dayViewHeaderFont.Dispose();
            this.m_agendaViewItemFont.Dispose();
            this.m_agendaViewTimeFont.Dispose();
            this.m_timeLinePen.Dispose();
            base.Dispose(disposing);
        }

        private void DrawAgendaView(Graphics g)
        {
            g.FillRectangle(Brushes.White, this.m_drawingArea.Left, this.m_drawingArea.Top + this.m_headerHeight, this.m_drawingArea.Width, this.m_drawingArea.Height - this.m_headerHeight);
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(this.m_drawingArea.Left, this.m_drawingArea.Top, this.m_drawingArea.Width, this.m_headerHeight), Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
            g.FillRectangle(brush, this.m_drawingArea.Left, this.m_drawingArea.Top, this.m_drawingArea.Width, this.m_headerHeight);
            this.DrawHeaderButtons(g, this.m_inLeftButtonBounds, this.m_inRightButtonBounds);
            g.DrawString(this.m_currentDate.ToLongDateString(), this.m_dayViewHeaderFont, Brushes.White, (float) (this.m_drawingArea.Left + 10), (float) (this.m_drawingArea.Top + 5));
            int num = 0;
            for (int i = (this.m_drawingArea.Top + this.m_headerHeight) + this.m_agendaItemHeight; (num < this.m_applicableTimers.Count) && (i < this.m_drawingArea.Bottom); i += this.m_agendaItemHeight)
            {
                g.DrawLine(Pens.Gray, this.m_drawingArea.Left, i, this.m_drawingArea.Right, i);
                num++;
            }
            foreach (Vixen.Timer timer in this.m_applicableTimers)
            {
                Brush gray;
                if (timer.EndTime < DateTime.Now.TimeOfDay)
                {
                    gray = Brushes.Gray;
                }
                else if (timer.StartTime > DateTime.Now.TimeOfDay)
                {
                    gray = Brushes.Black;
                }
                else
                {
                    gray = Brushes.Red;
                }
                foreach (ReferenceRectF tf in timer.DisplayBounds)
                {
                    g.DrawString(timer.ProgramName, this.m_agendaViewItemFont, gray, (float) (tf.Left + 16f), (float) (tf.Top + 10f));
                    g.DrawString(string.Format("{0} - {1}", timer.StartDateTime.ToString("h:mm tt"), timer.EndDateTime.ToString("h:mm tt")), this.m_agendaViewTimeFont, gray, (float) (tf.Left + 16f), (float) (tf.Top + 30f));
                }
            }
        }

        private void DrawDayView(Graphics g)
        {
            if (g.ClipBounds.Left < this.m_timeGutter)
            {
                this.DrawTimes(g);
            }
            if (g.ClipBounds.Top < (this.m_drawingArea.Top + this.m_headerHeight))
            {
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(this.m_drawingArea.Left, this.m_drawingArea.Top, this.m_drawingArea.Width - this.m_timeGutter, this.m_headerHeight), Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
                g.FillRectangle(brush, this.m_drawingArea.Left + this.m_timeGutter, this.m_drawingArea.Top, this.m_drawingArea.Width - this.m_timeGutter, this.m_headerHeight);
                this.DrawHeaderButtons(g, this.m_inLeftButtonBounds, this.m_inRightButtonBounds);
                g.DrawString(this.m_currentDate.ToLongDateString(), this.m_dayViewHeaderFont, Brushes.White, (float) ((this.m_drawingArea.Left + this.m_timeGutter) + 10), (float) (this.m_drawingArea.Top + 5));
            }
            if (g.ClipBounds.Bottom > (this.m_drawingArea.Top + this.m_headerHeight))
            {
                this.DrawTimerBlocks(this.m_applicableTimers, g);
            }
        }

        private void DrawHeaderButtons(Graphics g, bool hoverLeft, bool hoverRight)
        {
            int width = 0x12;
            int num2 = 8;
            int x = this.m_drawingArea.Width - (((width + num2) + width) + num2);
            int y = this.m_drawingArea.Top + ((this.m_headerHeight - width) / 2);
            Point[] pointArray3 = new Point[] { new Point(x + 12, y + 5), new Point(x + 12, y + 13), new Point(x + 6, y + 9) };
            Point[] points = pointArray3;
            pointArray3 = new Point[] { new Point(((x + 6) + width) + num2, y + 5), new Point(((x + 6) + width) + num2, y + 13), new Point(((x + 12) + width) + num2, y + 9) };
            Point[] pointArray2 = pointArray3;
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(x + 1, y + 1, width - 1, this.m_headerHeight), Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
            LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(x + 1, y + 1, width - 1, width - 1), Color.FromArgb(0x77, 0xa5, 0xd6), Color.FromArgb(0x18, 0x4d, 0x94), 90f);
            this.m_buttonLeftBounds = new Rectangle(x, y, width, width);
            this.m_buttonRightBounds = new Rectangle((x + width) + num2, y, width, width);
            if (hoverLeft)
            {
                g.FillRectangle(brush2, (int) (x + 1), (int) (y + 1), (int) (width - 1), (int) (width - 1));
            }
            else
            {
                g.FillRectangle(brush, (int) (x + 1), (int) (y + 1), (int) (width - 1), (int) (width - 1));
            }
            if (hoverRight)
            {
                g.FillRectangle(brush2, (int) (((x + 1) + width) + num2), (int) (y + 1), (int) (width - 1), (int) (width - 1));
            }
            else
            {
                g.FillRectangle(brush, (int) (((x + 1) + width) + num2), (int) (y + 1), (int) (width - 1), (int) (width - 1));
            }
            this.DrawRoundRect(g, Pens.White, (float) x, (float) y, (float) width, (float) width, 3f);
            this.DrawRoundRect(g, Pens.White, (float) ((x + width) + num2), (float) y, (float) width, (float) width, 3f);
            g.FillPolygon(Brushes.White, points);
            g.FillPolygon(Brushes.White, pointArray2);
        }

        private void DrawMonthView(Graphics g)
        {
            LinearGradientBrush brush;
            g.FillRectangle(this.m_backgroundBrush, this.m_drawingArea.Left, this.m_drawingArea.Top + this.m_headerHeight, this.m_drawingArea.Width, this.m_drawingArea.Height - this.m_headerHeight);
            int num = DateTime.DaysInMonth(this.m_currentDate.Year, this.m_currentDate.Month);
            DateTime time = new DateTime(this.m_currentDate.Year, this.m_currentDate.Month, 1);
            int num2 = num + time.DayOfWeek;
            int num3 = ((num2 % 7) > 0) ? ((num2 / 7) + 1) : (num2 / 7);
            if (g.ClipBounds.Bottom > (this.m_drawingArea.Top + this.m_headerHeight))
            {
                int num6 = (this.m_drawingArea.Top + this.m_monthViewTopMargin) + this.m_headerHeight;
                int num7 = num6 + (this.m_monthViewRowSize * num3);
                int num4 = this.m_drawingArea.Left + this.m_monthViewLeftMargin;
                int num9 = 0;
                while (num9 <= 7)
                {
                    g.DrawLine(Pens.Black, num4, num6, num4, num7);
                    num9++;
                    num4 += this.m_monthViewColSize;
                }
                num4 = this.m_drawingArea.Left + this.m_monthViewLeftMargin;
                int num5 = num4 + (this.m_monthViewColSize * 7);
                num6 = (this.m_drawingArea.Top + this.m_monthViewTopMargin) + this.m_headerHeight;
                int num8 = 0;
                while (num8 <= num3)
                {
                    g.DrawLine(Pens.Black, num4, num6, num5, num6);
                    num8++;
                    num6 += this.m_monthViewRowSize;
                }
                int dayOfWeek = (int) time.DayOfWeek;
                num6 = ((this.m_drawingArea.Top + this.m_headerHeight) + this.m_monthViewTopMargin) + 5;
                DateTime startDateTime = time;
                float num12 = ((float) this.m_monthViewColSize) / 48f;
                for (int i = 1; i <= num; i++)
                {
                    num4 = (this.m_drawingArea.Left + this.m_monthViewLeftMargin) + (dayOfWeek * this.m_monthViewColSize);
                    if (startDateTime == this.m_currentDate)
                    {
                        brush = new LinearGradientBrush(new Rectangle(((this.m_drawingArea.Left + this.m_monthViewLeftMargin) + (dayOfWeek * this.m_monthViewColSize)) + 4, num6 - 1, 20, this.m_headerHeight), Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
                        g.FillRectangle(brush, ((this.m_drawingArea.Left + this.m_monthViewLeftMargin) + (dayOfWeek * this.m_monthViewColSize)) + 4, num6 - 1, 20, 15);
                        g.DrawString(i.ToString(), this.m_timeSmallFont, Brushes.White, (float) (num4 + 5), (float) num6);
                    }
                    else
                    {
                        g.DrawString(i.ToString(), this.m_timeSmallFont, Brushes.Black, (float) (num4 + 5), (float) num6);
                    }
                    g.FillRectangle(new SolidBrush(this.m_halfHourPen.Color), num4 + 1, ((num6 - 5) + this.m_monthViewRowSize) - 10, this.m_monthViewColSize - 2, 10);
                    SolidBrush brush2 = new SolidBrush(this.m_hourPen.Color);
                    float num13 = ((num6 - 5) + this.m_monthViewRowSize) - this.m_monthBarHeight;
                    foreach (Vixen.Timer timer in this.ReturnApplicableTimersSubset(startDateTime, this.m_oneDayTimeSpan, this.m_applicableTimers))
                    {
                        foreach (ReferenceRectF tf in timer.DisplayBounds)
                        {
                            g.FillRectangle(brush2, tf.ToRectangleF());
                        }
                    }
                    startDateTime = startDateTime.AddDays(1.0);
                    if (++dayOfWeek == 7)
                    {
                        dayOfWeek = 0;
                        num6 += this.m_monthViewRowSize;
                    }
                }
            }
            if (g.ClipBounds.Top < (this.m_drawingArea.Top + this.m_headerHeight))
            {
                brush = new LinearGradientBrush(new Rectangle(this.m_drawingArea.Left, this.m_drawingArea.Top, this.m_drawingArea.Width, this.m_headerHeight), Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
                g.FillRectangle(brush, this.m_drawingArea.Left, this.m_drawingArea.Top, this.m_drawingArea.Width, this.m_headerHeight);
                this.DrawHeaderButtons(g, this.m_inLeftButtonBounds, this.m_inRightButtonBounds);
                g.DrawString(this.m_currentDate.ToString("Y"), this.m_dayViewHeaderFont, Brushes.White, (float) (this.m_drawingArea.Left + 10), (float) (this.m_drawingArea.Top + 5));
            }
        }

        private void DrawRoundRect(Graphics g, Pen p, float X, float Y, float width, float height, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(X + radius, Y, (X + width) - (radius * 2f), Y);
            path.AddArc((X + width) - (radius * 2f), Y, radius * 2f, radius * 2f, 270f, 90f);
            path.AddLine((float) (X + width), (float) (Y + radius), (float) (X + width), (float) ((Y + height) - (radius * 2f)));
            path.AddArc((float) ((X + width) - (radius * 2f)), (float) ((Y + height) - (radius * 2f)), (float) (radius * 2f), (float) (radius * 2f), 0f, 90f);
            path.AddLine((float) ((X + width) - (radius * 2f)), (float) (Y + height), (float) (X + radius), (float) (Y + height));
            path.AddArc(X, (Y + height) - (radius * 2f), radius * 2f, radius * 2f, 90f, 90f);
            path.AddLine(X, (Y + height) - (radius * 2f), X, Y + radius);
            path.AddArc(X, Y, radius * 2f, radius * 2f, 180f, 90f);
            path.CloseFigure();
            g.DrawPath(p, path);
            path.Dispose();
        }

        private void DrawTimerBlocks(List<Vixen.Timer> timers, Graphics g)
        {
            RectangleF layoutRectangle = new RectangleF();
            foreach (Vixen.Timer timer in timers)
            {
                foreach (ReferenceRectF tf in timer.DisplayBounds)
                {
                    g.FillRectangle(Brushes.White, tf.ToRectangleF());
                    g.DrawRectangle(Pens.Black, tf.X, tf.Y, tf.Width, tf.Height);
                    layoutRectangle.X = tf.X + 2f;
                    layoutRectangle.Y = tf.Y;
                    layoutRectangle.Width = tf.Width;
                    layoutRectangle.Height = Math.Min(13f, tf.Height);
                    foreach (string str in timer.ToString().Split(new char[] { '|' }))
                    {
                        g.DrawString(str, this.m_timeSmallFont, Brushes.Black, layoutRectangle);
                        layoutRectangle.Y += layoutRectangle.Height;
                        if (layoutRectangle.Top >= tf.Bottom)
                        {
                            break;
                        }
                        if (layoutRectangle.Bottom > tf.Bottom)
                        {
                            layoutRectangle.Height = tf.Bottom - layoutRectangle.Top;
                        }
                    }
                }
            }
        }

        private void DrawTimes(Graphics g)
        {
            int num;
            g.FillRectangle(this.m_backgroundBrush, (int) (this.m_drawingArea.Left + this.m_timeGutter), (int) (this.m_drawingArea.Top + this.m_headerHeight), (int) (this.m_drawingArea.Width - this.m_timeGutter), (int) (this.m_drawingArea.Height - this.m_headerHeight));
            g.DrawLine(this.m_timeLinePen, this.m_drawingArea.Left, this.m_drawingArea.Top + this.m_headerHeight, (this.m_drawingArea.Left + this.m_timeGutter) - 5, this.m_drawingArea.Top + this.m_headerHeight);
            for (num = (this.m_drawingArea.Top + this.m_headerHeight) + (((this.vScrollBar.Value % 2) == 0) ? (this.m_halfHourHeight * 2) : this.m_halfHourHeight); num < this.m_drawingArea.Height; num += this.m_halfHourHeight * 2)
            {
                g.DrawLine(this.m_hourPen, this.m_drawingArea.Left + this.m_timeGutter, num, this.m_drawingArea.Width, num);
                g.DrawLine(this.m_timeLinePen, this.m_drawingArea.Left + 5, num, (this.m_drawingArea.Left + this.m_timeGutter) - 5, num);
            }
            for (num = (this.m_drawingArea.Top + this.m_headerHeight) + (((this.vScrollBar.Value % 2) == 0) ? this.m_halfHourHeight : (this.m_halfHourHeight * 2)); num < this.m_drawingArea.Height; num += this.m_halfHourHeight * 2)
            {
                g.DrawLine(this.m_halfHourPen, this.m_drawingArea.Left + this.m_timeGutter, num, this.m_drawingArea.Width, num);
            }
            int num2 = ((this.vScrollBar.Value % 2) == 1) ? ((this.vScrollBar.Value / 2) + 1) : (this.vScrollBar.Value / 2);
            int num3 = this.m_timeGutter >> 1;
            int num4 = -1;
            for (num = ((this.m_drawingArea.Top + 3) + this.m_headerHeight) + (((this.vScrollBar.Value % 2) == 0) ? 0 : this.m_halfHourHeight); (num < this.m_drawingArea.Height) && (num2 < 0x18); num += this.m_halfHourHeight * 2)
            {
                string str;
                string str2;
                if (num2 == 0)
                {
                    str = "12";
                }
                else if (num2 > 12)
                {
                    str = (num2 - 12).ToString();
                }
                else
                {
                    str = num2.ToString();
                }
                g.DrawString(str, this.m_timeLargeFont, Brushes.Black, (float) (((this.m_drawingArea.Left + num3) - ((int) g.MeasureString(str, this.m_timeLargeFont).Width)) + 6), (float) num);
                if (num4 != (num2 / 12))
                {
                    num4 = num2 / 12;
                    str2 = (num4 == 0) ? "am" : "pm";
                }
                else
                {
                    str2 = "00";
                }
                g.DrawString(str2, this.m_timeSmallFont, Brushes.Black, (float) ((this.m_drawingArea.Left + num3) + 2), (float) num);
                num2++;
            }
        }

        private void DrawWeekView(Graphics g)
        {
            float num2;
            int num3;
            g.FillRectangle(this.m_backgroundBrush, this.m_drawingArea.Left + this.m_timeGutter, this.m_drawingArea.Top + this.m_headerHeight, this.m_drawingArea.Width - this.m_timeGutter, 20);
            this.m_headerHeight = 50;
            this.DrawTimes(g);
            this.m_headerHeight = 30;
            float num = ((float) (this.m_drawingArea.Width - this.m_timeGutter)) / 7f;
            for (num3 = 1; num3 < 7; num3++)
            {
                num2 = (this.m_drawingArea.Left + (num * num3)) + this.m_timeGutter;
                g.DrawLine(Pens.Black, num2, (float) (this.m_drawingArea.Top + this.m_headerHeight), num2, (float) this.m_drawingArea.Height);
            }
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(this.m_drawingArea.Left, this.m_drawingArea.Top, this.m_drawingArea.Width - this.m_timeGutter, this.m_headerHeight), Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
            SolidBrush brush2 = new SolidBrush(this.m_halfHourPen.Color);
            int dayOfWeek = (int) this.m_currentDate.DayOfWeek;
            for (num3 = 0; num3 < 7; num3++)
            {
                num2 = (this.m_drawingArea.Left + (num * num3)) + this.m_timeGutter;
                if (dayOfWeek == num3)
                {
                    g.FillRectangle(brush, num2 + 2f, (float) ((this.m_drawingArea.Top + this.m_headerHeight) + 2), num - 5f, 17f);
                    g.DrawRectangle(Pens.Navy, num2 + 2f, (float) ((this.m_drawingArea.Top + this.m_headerHeight) + 2), num - 5f, 17f);
                    g.DrawString(this.m_days[num3], this.m_timeSmallFont, Brushes.White, num2 + ((num - g.MeasureString(this.m_days[num3], this.m_timeSmallFont).Width) / 2f), (float) ((this.m_drawingArea.Top + this.m_headerHeight) + 4));
                }
                else
                {
                    g.FillRectangle(brush2, num2 + 2f, (float) ((this.m_drawingArea.Top + this.m_headerHeight) + 2), num - 5f, 17f);
                    g.DrawRectangle(this.m_hourPen, num2 + 2f, (float) ((this.m_drawingArea.Top + this.m_headerHeight) + 2), num - 5f, 17f);
                    g.DrawString(this.m_days[num3], this.m_timeSmallFont, Brushes.Black, num2 + ((num - g.MeasureString(this.m_days[num3], this.m_timeSmallFont).Width) / 2f), (float) ((this.m_drawingArea.Top + this.m_headerHeight) + 4));
                }
            }
            g.FillRectangle(brush, this.m_drawingArea.Left + this.m_timeGutter, this.m_drawingArea.Top, this.m_drawingArea.Width - this.m_timeGutter, this.m_headerHeight);
            this.DrawHeaderButtons(g, this.m_inLeftButtonBounds, this.m_inRightButtonBounds);
            DateTime time = this.m_currentDate.AddDays((double) -this.m_currentDate.DayOfWeek);
            DateTime time2 = time.AddDays(6.0);
            g.DrawString(string.Format("{0}   -   {1}", time.ToString("m"), time2.ToString("m")), this.m_dayViewHeaderFont, Brushes.White, (float) ((this.m_drawingArea.Left + this.m_timeGutter) + 10), (float) (this.m_drawingArea.Top + 5));
            this.DrawTimerBlocks(this.m_applicableTimers, g);
        }

        private void EditOrAddTimerAt(Point point, int viewRelativeY, DateTime viewStartDate, TimeSpan viewLengthSpan)
        {
            Vixen.Timer timer = null;
            timer = this.FindApplicableTimerAt(point);
            int hour = (this.vScrollBar.Value + (viewRelativeY / this.m_halfHourHeight)) / 2;
            int minute = ((this.vScrollBar.Value + (viewRelativeY / this.m_halfHourHeight)) % 2) * 30;
            if (hour < 0x18)
            {
                TimerDialog dialog;
                if (timer == null)
                {
                    dialog = new TimerDialog(new DateTime(this.m_currentDate.Year, this.m_currentDate.Month, this.m_currentDate.Day, hour, minute, 0));
                }
                else
                {
                    dialog = new TimerDialog(timer);
                }
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (timer == null)
                    {
                        this.AddTimer(dialog.Timer);
                    }
                    else if (timer != dialog.Timer)
                    {
                        timer.Copy(dialog.Timer);
                    }
                    this.CompileApplicableTimers(viewStartDate, viewLengthSpan);
                    this.Refresh();
                }
                dialog.Dispose();
            }
        }

        private Vixen.Timer FindApplicableTimerAt(Point point)
        {
            foreach (Vixen.Timer timer in this.m_applicableTimers)
            {
                foreach (ReferenceRectF tf in timer.DisplayBounds)
                {
                    if (tf.Contains(point))
                    {
                        return timer;
                    }
                }
            }
            return null;
        }

        private void HoverNavButtons(Point location)
        {
            bool inLeftButtonBounds = this.m_inLeftButtonBounds;
            bool inRightButtonBounds = this.m_inRightButtonBounds;
            this.m_inLeftButtonBounds = this.m_buttonLeftBounds.Contains(location);
            this.m_inRightButtonBounds = this.m_buttonRightBounds.Contains(location);
            if ((inLeftButtonBounds != this.m_inLeftButtonBounds) || (inRightButtonBounds != this.m_inRightButtonBounds))
            {
                this.Refresh();
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ScheduleDialog));
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
            this.toolStripButtonToday.Image = (Image) manager.GetObject("toolStripButtonToday.Image");
            this.toolStripButtonToday.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonToday.Name = "toolStripButtonToday";
            this.toolStripButtonToday.Size = new Size(0x2c, 0x16);
            this.toolStripButtonToday.Text = "Today";
            this.toolStripButtonToday.ToolTipText = "Go to today";
            this.toolStripButtonToday.Click += new EventHandler(this.toolStripButtonToday_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.toolStripButtonDayView.CheckOnClick = true;
            this.toolStripButtonDayView.Image = (Image) manager.GetObject("toolStripButtonDayView.Image");
            this.toolStripButtonDayView.ImageTransparentColor = Color.White;
            this.toolStripButtonDayView.Name = "toolStripButtonDayView";
            this.toolStripButtonDayView.Size = new Size(0x4a, 0x16);
            this.toolStripButtonDayView.Text = "Day view";
            this.toolStripButtonDayView.Click += new EventHandler(this.toolStripButtonDayView_Click);
            this.toolStripButtonWeekView.CheckOnClick = true;
            this.toolStripButtonWeekView.Image = (Image) manager.GetObject("toolStripButtonWeekView.Image");
            this.toolStripButtonWeekView.ImageTransparentColor = Color.White;
            this.toolStripButtonWeekView.Name = "toolStripButtonWeekView";
            this.toolStripButtonWeekView.Size = new Size(0x53, 0x16);
            this.toolStripButtonWeekView.Text = "Week view";
            this.toolStripButtonWeekView.Click += new EventHandler(this.toolStripButtonWeekView_Click);
            this.toolStripButtonMonthView.CheckOnClick = true;
            this.toolStripButtonMonthView.Image = (Image) manager.GetObject("toolStripButtonMonthView.Image");
            this.toolStripButtonMonthView.ImageTransparentColor = Color.White;
            this.toolStripButtonMonthView.Name = "toolStripButtonMonthView";
            this.toolStripButtonMonthView.Size = new Size(90, 0x16);
            this.toolStripButtonMonthView.Text = "Month view";
            this.toolStripButtonMonthView.Click += new EventHandler(this.toolStripButtonMonthView_Click);
            this.toolStripButtonAgendaView.CheckOnClick = true;
            this.toolStripButtonAgendaView.Image = (Image) manager.GetObject("toolStripButtonAgendaView.Image");
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
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(530, 10);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
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
            base.AutoScaleMode = AutoScaleMode.Font;
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

        private void MonthViewCalcs()
        {
            foreach (Vixen.Timer timer in this.m_applicableTimers)
            {
                timer.DisplayBounds.Clear();
            }
            int num = DateTime.DaysInMonth(this.m_currentDate.Year, this.m_currentDate.Month);
            DateTime time = new DateTime(this.m_currentDate.Year, this.m_currentDate.Month, 1);
            int num2 = num + time.DayOfWeek;
            int num3 = ((num2 % 7) > 0) ? ((num2 / 7) + 1) : (num2 / 7);
            this.m_monthViewRowSize = (((this.m_drawingArea.Height - this.m_headerHeight) - this.m_monthViewTopMargin) - this.m_monthViewBottomMargin) / num3;
            this.m_monthViewColSize = ((this.m_drawingArea.Width - this.m_monthViewLeftMargin) - this.m_monthViewRightMargin) / 7;
            int dayOfWeek = (int) time.DayOfWeek;
            int num5 = ((this.m_drawingArea.Top + this.m_headerHeight) + this.m_monthViewTopMargin) + 5;
            DateTime startDateTime = time;
            float num8 = ((float) this.m_monthViewColSize) / 48f;
            for (int i = 1; i <= num; i++)
            {
                int num4 = (this.m_drawingArea.Left + this.m_monthViewLeftMargin) + (dayOfWeek * this.m_monthViewColSize);
                float num9 = ((num5 - 5) + this.m_monthViewRowSize) - this.m_monthBarHeight;
                foreach (Vixen.Timer timer in this.ReturnApplicableTimersSubset(startDateTime, this.m_oneDayTimeSpan, this.m_applicableTimers))
                {
                    ReferenceRectF item = new ReferenceRectF();
                    item.X = (num4 + 1) + ((timer.StartTime.Hours * 2) * num8);
                    item.Y = num9;
                    item.Width = (float) ((timer.TimerLength.TotalHours * 2.0) * num8);
                    item.Height = this.m_monthBarHeight;
                    timer.DisplayBounds.Add(item);
                }
                startDateTime = startDateTime.AddDays(1.0);
                if (++dayOfWeek == 7)
                {
                    dayOfWeek = 0;
                    num5 += this.m_monthViewRowSize;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            switch (this.m_currentView)
            {
                case DateView.Day:
                    this.DrawDayView(e.Graphics);
                    break;

                case DateView.Week:
                    this.DrawWeekView(e.Graphics);
                    break;

                case DateView.Month:
                    this.DrawMonthView(e.Graphics);
                    break;

                case DateView.Agenda:
                    this.DrawAgendaView(e.Graphics);
                    break;
            }
        }

        private List<Vixen.Timer> ReturnApplicableTimersSubset(DateTime startDateTime, TimeSpan span, List<Vixen.Timer> seedList)
        {
            List<Vixen.Timer> list = new List<Vixen.Timer>();
            DateTime time = startDateTime + span;
            foreach (Vixen.Timer timer in seedList)
            {
                int num;
                DateTime time4;
                int num3;
                if ((timer.RecurrenceStart >= time) || (timer.RecurrenceEnd < startDateTime))
                {
                    continue;
                }
                switch (timer.Recurrence)
                {
                    case RecurrenceType.None:
                    {
                        if ((timer.StartDateTime <= time) && (timer.EndDateTime >= startDateTime))
                        {
                            break;
                        }
                        continue;
                    }
                    case RecurrenceType.Daily:
                    {
                        DateTime time2 = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, timer.StartTime.Hours, timer.StartTime.Minutes, 0);
                        DateTime time3 = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, timer.EndTime.Hours, timer.EndTime.Minutes, 0);
                        if ((time2 <= time) && (time3 >= startDateTime))
                        {
                            goto Label_014B;
                        }
                        continue;
                    }
                    case RecurrenceType.Weekly:
                        num = 0;
                        if (span.TotalDays < 7.0)
                        {
                            goto Label_017B;
                        }
                        num = 0x7f;
                        goto Label_01C2;

                    case RecurrenceType.Monthly:
                    {
                        if ((startDateTime.Day <= timer.EndDateTime.Day) && (time.Day >= timer.StartDateTime.Day))
                        {
                            goto Label_0230;
                        }
                        continue;
                    }
                    case RecurrenceType.Yearly:
                        if ((((time.Month * 0x1f) + time.Day) < ((timer.StartDateTime.Month * 0x1f) + timer.StartDateTime.Day)) || (((startDateTime.Month * 0x1f) + startDateTime.Day) > ((timer.EndDateTime.Month * 0x1f) + timer.EndDateTime.Day)))
                        {
                            continue;
                        }
                        list.Add(timer);
                        goto Label_02C9;

                    default:
                        goto Label_02C9;
                }
                list.Add(timer);
                goto Label_02C9;
            Label_014B:
                list.Add(timer);
                goto Label_02C9;
            Label_017B:
                time4 = startDateTime;
                for (int i = 0; i < span.TotalDays; i++)
                {
                    num |= ((int) 1) << time4.DayOfWeek;
                    time4 = time4.AddDays(1.0);
                }
            Label_01C2:
                num3 = (int) timer.RecurrenceData;
                if ((num3 & num) > 0)
                {
                    list.Add(timer);
                }
                goto Label_02C9;
            Label_0230:
                list.Add(timer);
            Label_02C9:;
            }
            return list;
        }

        private void ScheduleDialog_Load(object sender, EventArgs e)
        {
            this.SetCurrentView(DateView.Day, DateTime.Today);
        }

        private void ScheduleDialog_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Y >= (this.m_drawingArea.Top + this.m_headerHeight))
            {
                switch (this.m_currentView)
                {
                    case DateView.Day:
                        if (e.Y >= (this.m_drawingArea.Top + this.m_headerHeight))
                        {
                            if (e.X >= (this.m_drawingArea.Left + this.m_timeGutter))
                            {
                                this.EditOrAddTimerAt(e.Location, (e.Y - this.m_headerHeight) - this.m_drawingArea.Top, this.m_currentDate, this.m_oneDayTimeSpan);
                                this.Refresh();
                            }
                            break;
                        }
                        break;

                    case DateView.Week:
                        if (e.Y >= (this.m_drawingArea.Top + this.m_headerHeight))
                        {
                            if (e.X >= (this.m_drawingArea.Left + this.m_timeGutter))
                            {
                                DateTime viewStartDate = this.m_currentDate.AddDays((double) -this.m_currentDate.DayOfWeek);
                                this.m_currentDate = viewStartDate.AddDays((double) ((int) (((float) (e.X - this.m_timeGutter)) / (((float) (this.m_drawingArea.Width - this.m_timeGutter)) / 7f))));
                                this.EditOrAddTimerAt(e.Location, ((e.Y - this.m_headerHeight) - this.m_halfHourHeight) - this.m_drawingArea.Top, viewStartDate, this.m_oneWeekTimeSpan);
                                this.Refresh();
                            }
                            break;
                        }
                        break;

                    case DateView.Month:
                    {
                        DateTime minValue = DateTime.MinValue;
                        if (this.TryGetMonthDateAt(e.Location, ref minValue))
                        {
                            this.SetCurrentView(DateView.Day, minValue);
                        }
                        break;
                    }
                    case DateView.Agenda:
                        if (e.Y >= (this.m_drawingArea.Top + this.m_headerHeight))
                        {
                            if (((((e.Y - this.m_drawingArea.Top) - this.m_headerHeight) / this.m_agendaItemHeight) + this.vScrollBar.Value) < this.m_applicableTimers.Count)
                            {
                                this.EditOrAddTimerAt(e.Location, (e.Y - this.m_headerHeight) - this.m_drawingArea.Top, this.m_currentDate, this.m_oneDayTimeSpan);
                            }
                            this.Refresh();
                        }
                        break;
                }
            }
        }

        private void ScheduleDialog_MouseDown(object sender, MouseEventArgs e)
        {
            switch (this.m_currentView)
            {
                case DateView.Day:
                case DateView.Agenda:
                    if (!this.m_inLeftButtonBounds)
                    {
                        if (this.m_inRightButtonBounds)
                        {
                            this.m_currentDate = this.m_currentDate.AddDays(1.0);
                            this.CompileApplicableTimers(this.m_currentDate, this.m_oneDayTimeSpan);
                            this.Refresh();
                        }
                        break;
                    }
                    this.m_currentDate = this.m_currentDate.AddDays(-1.0);
                    this.CompileApplicableTimers(this.m_currentDate, this.m_oneDayTimeSpan);
                    this.Refresh();
                    break;

                case DateView.Week:
                    if (!this.m_inLeftButtonBounds)
                    {
                        if (this.m_inRightButtonBounds)
                        {
                            this.m_currentDate = this.m_currentDate.AddDays(7.0);
                            this.CompileApplicableTimers(this.m_currentDate.AddDays((double) -this.m_currentDate.DayOfWeek), this.m_oneWeekTimeSpan);
                            this.Refresh();
                        }
                        break;
                    }
                    this.m_currentDate = this.m_currentDate.AddDays(-7.0);
                    this.CompileApplicableTimers(this.m_currentDate.AddDays((double) -this.m_currentDate.DayOfWeek), this.m_oneWeekTimeSpan);
                    this.Refresh();
                    break;

                case DateView.Month:
                    if (!this.m_inLeftButtonBounds)
                    {
                        if (this.m_inRightButtonBounds)
                        {
                            this.m_currentDate = this.m_currentDate.AddMonths(1);
                            this.Refresh();
                        }
                        break;
                    }
                    this.m_currentDate = this.m_currentDate.AddMonths(-1);
                    this.Refresh();
                    break;
            }
        }

        private void ScheduleDialog_MouseMove(object sender, MouseEventArgs e)
        {
            DateTime minValue;
            switch (this.m_currentView)
            {
                case DateView.Day:
                    this.SetToolTip(this, e.Location);
                    break;

                case DateView.Week:
                {
                    float num = ((float) (this.m_drawingArea.Width - this.m_timeGutter)) / 7f;
                    minValue = this.m_currentDate.AddDays((double) (-this.m_currentDate.DayOfWeek + ((int) (((float) (e.X - this.m_timeGutter)) / num))));
                    if (!this.SetToolTip(this, e.Location, this.ReturnApplicableTimersSubset(minValue, this.m_oneDayTimeSpan, this.m_applicableTimers)))
                    {
                        break;
                    }
                    return;
                }
                case DateView.Month:
                    minValue = DateTime.MinValue;
                    if (!this.TryGetMonthDateAt(e.Location, ref minValue) || !this.SetToolTip(this, e.Location, this.ReturnApplicableTimersSubset(minValue, this.m_oneDayTimeSpan, this.m_applicableTimers)))
                    {
                        break;
                    }
                    return;

                case DateView.Agenda:
                    this.SetToolTip(this, e.Location);
                    break;
            }
            this.HoverNavButtons(e.Location);
        }

        private void ScheduleDialog_Resize(object sender, EventArgs e)
        {
            if (!(this.m_resizing || (base.WindowState == FormWindowState.Minimized)))
            {
                this.VScrollCheck();
                this.ViewCalcs();
                this.Refresh();
            }
        }

        private void ScheduleDialog_ResizeBegin(object sender, EventArgs e)
        {
            this.m_resizing = true;
        }

        private void ScheduleDialog_ResizeEnd(object sender, EventArgs e)
        {
            this.m_resizing = false;
            this.VScrollCheck();
            this.ViewCalcs();
            this.Refresh();
        }

        private void SetCurrentView(DateView view)
        {
            this.SetCurrentView(view, this.m_currentDate);
        }

        private void SetCurrentView(DateView view, DateTime date)
        {
            switch (this.m_currentView)
            {
                case DateView.Day:
                    this.m_dayViewScrollBarValue = this.vScrollBar.Value;
                    this.toolStripButtonDayView.Checked = false;
                    break;

                case DateView.Week:
                    this.m_weekViewScrollBarValue = this.vScrollBar.Value;
                    this.toolStripButtonWeekView.Checked = false;
                    break;

                case DateView.Month:
                    this.toolStripButtonMonthView.Checked = false;
                    break;

                case DateView.Agenda:
                    this.m_agendaViewScrollBarValue = this.vScrollBar.Value;
                    this.toolStripButtonAgendaView.Checked = false;
                    break;
            }
            this.m_currentDate = date;
            this.m_currentView = view;
            this.VScrollCheck();
            switch (this.m_currentView)
            {
                case DateView.Day:
                    this.CompileApplicableTimers(date, this.m_oneDayTimeSpan);
                    this.toolStripButtonDayView.Checked = true;
                    break;

                case DateView.Week:
                    this.CompileApplicableTimers(this.m_currentDate.AddDays((double) -this.m_currentDate.DayOfWeek), this.m_oneWeekTimeSpan);
                    this.toolStripButtonWeekView.Checked = true;
                    break;

                case DateView.Month:
                    this.CompileApplicableTimers(new DateTime(this.m_currentDate.Year, this.m_currentDate.Month, 1), new TimeSpan(DateTime.DaysInMonth(this.m_currentDate.Year, this.m_currentDate.Month), 0, 0, 0));
                    this.toolStripButtonMonthView.Checked = true;
                    break;

                case DateView.Agenda:
                    this.CompileApplicableTimers(date, this.m_oneDayTimeSpan);
                    this.toolStripButtonAgendaView.Checked = true;
                    break;
            }
            this.Refresh();
        }

        private void SetDrawingArea()
        {
            this.m_drawingArea.X = 0;
            this.m_drawingArea.Y = this.toolStrip1.Bottom;
            this.m_drawingArea.Width = this.vScrollBar.Visible ? this.vScrollBar.Left : this.vScrollBar.Right;
            this.m_drawingArea.Height = this.panel1.Top;
        }

        private void SetToolTip(Control control, Point location)
        {
            this.SetToolTip(control, location, this.m_applicableTimers);
        }

        private bool SetToolTip(Control control, Point location, List<Vixen.Timer> timers)
        {
            string toolTip = this.toolTip.GetToolTip(control);
            foreach (Vixen.Timer timer in timers)
            {
                foreach (ReferenceRectF tf in timer.DisplayBounds)
                {
                    if (tf.Contains(location))
                    {
                        string caption = timer.ToString().Replace("|", "\r\n");
                        if (caption != toolTip)
                        {
                            this.toolTip.SetToolTip(control, caption);
                        }
                        return true;
                    }
                }
            }
            this.toolTip.SetToolTip(control, string.Empty);
            return false;
        }

        private void toolStripButtonAgendaView_Click(object sender, EventArgs e)
        {
            this.SetCurrentView(DateView.Agenda);
        }

        private void toolStripButtonDayView_Click(object sender, EventArgs e)
        {
            this.SetCurrentView(DateView.Day);
        }

        private void toolStripButtonMonthView_Click(object sender, EventArgs e)
        {
            this.SetCurrentView(DateView.Month);
        }

        private void toolStripButtonToday_Click(object sender, EventArgs e)
        {
            this.SetCurrentView(DateView.Day, DateTime.Today);
        }

        private void toolStripButtonWeekView_Click(object sender, EventArgs e)
        {
            this.SetCurrentView(DateView.Week);
        }

        private void toolStripMenuItemEdit_Click(object sender, EventArgs e)
        {
            switch (this.m_currentView)
            {
                case DateView.Day:
                case DateView.Agenda:
                    this.EditOrAddTimerAt(this.m_mouseDownAt, (this.m_mouseDownAt.Y - this.m_drawingArea.Y) - this.m_headerHeight, this.m_currentDate, this.m_oneDayTimeSpan);
                    break;

                case DateView.Week:
                {
                    DateTime viewStartDate = this.m_currentDate.AddDays((double) -this.m_currentDate.DayOfWeek);
                    this.EditOrAddTimerAt(this.m_mouseDownAt, ((this.m_mouseDownAt.Y - this.m_drawingArea.Y) - this.m_headerHeight) - this.m_halfHourHeight, viewStartDate, this.m_oneWeekTimeSpan);
                    break;
                }
            }
        }

        private void toolStripMenuItemRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Remove all occurrences of this timer?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Vixen.Timer item = this.FindApplicableTimerAt(this.m_mouseDownAt);
                this.m_timers.Remove(item);
                switch (this.m_currentView)
                {
                    case DateView.Day:
                    case DateView.Agenda:
                        this.CompileApplicableTimers(this.m_currentDate, this.m_oneDayTimeSpan);
                        this.Refresh();
                        break;

                    case DateView.Week:
                    {
                        DateTime startDateTime = this.m_currentDate.AddDays((double) -this.m_currentDate.DayOfWeek);
                        this.CompileApplicableTimers(startDateTime, this.m_oneWeekTimeSpan);
                        this.Refresh();
                        break;
                    }
                }
            }
        }

        private bool TryGetMonthDateAt(Point p, ref DateTime date)
        {
            if (p.Y < ((this.m_drawingArea.Top + this.m_headerHeight) + this.m_monthViewTopMargin))
            {
                return false;
            }
            if (p.Y > (this.m_drawingArea.Bottom - this.m_monthViewBottomMargin))
            {
                return false;
            }
            if (p.X < (this.m_drawingArea.Left + this.m_monthViewLeftMargin))
            {
                return false;
            }
            if (p.X > (this.m_drawingArea.Right - this.m_monthViewRightMargin))
            {
                return false;
            }
            int num = DateTime.DaysInMonth(this.m_currentDate.Year, this.m_currentDate.Month);
            DateTime time = new DateTime(this.m_currentDate.Year, this.m_currentDate.Month, 1);
            int num2 = num + time.DayOfWeek;
            int num3 = ((num2 % 7) > 0) ? ((num2 / 7) + 1) : (num2 / 7);
            int num4 = ((((this.m_drawingArea.Height - this.m_headerHeight) - this.m_monthViewTopMargin) - this.m_monthViewBottomMargin) / num3) + 1;
            int num5 = (((this.m_drawingArea.Width - this.m_monthViewLeftMargin) - this.m_monthViewRightMargin) / 7) + 1;
            int num6 = ((p.X - this.m_drawingArea.Left) - this.m_monthViewLeftMargin) / num5;
            int num7 = (((p.Y - this.m_drawingArea.Top) - this.m_headerHeight) - this.m_monthViewTopMargin) / num4;
            if ((num7 == 0) && (num6 < time.DayOfWeek))
            {
                return false;
            }
            int num8 = ((int) ((time.DayOfWeek + num) % (DayOfWeek.Saturday | DayOfWeek.Monday))) - 1;
            if (num8 == -1)
            {
                num8 = 7;
            }
            if ((num7 == (num3 - 1)) && (num6 > num8))
            {
                return false;
            }
            date = time.AddDays((double) (((num7 * 7) + num6) - time.DayOfWeek));
            return true;
        }

        private void UpdateCurrentScheduleView()
        {
            int y = this.toolStrip1.Bottom + this.m_headerHeight;
            switch (this.m_currentView)
            {
                case DateView.Day:
                    base.Invalidate(new Rectangle(0, y, this.m_drawingArea.Width, base.ClientRectangle.Height - y));
                    break;

                case DateView.Week:
                    base.Invalidate(new Rectangle(0, y + this.m_halfHourHeight, this.m_drawingArea.Width, (base.ClientRectangle.Height - y) - this.m_halfHourHeight));
                    break;

                case DateView.Month:
                    this.Refresh();
                    break;

                case DateView.Agenda:
                    this.Refresh();
                    break;
            }
        }

        private void ViewCalcs()
        {
            switch (this.m_currentView)
            {
                case DateView.Day:
                    this.DayViewCalcs();
                    break;

                case DateView.Week:
                    this.WeekViewCalcs();
                    break;

                case DateView.Month:
                    this.MonthViewCalcs();
                    break;

                case DateView.Agenda:
                    this.AgendaViewCalcs();
                    break;
            }
        }

        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            this.ViewCalcs();
            this.UpdateCurrentScheduleView();
        }

        private void VScrollCheck()
        {
            this.SetDrawingArea();
            switch (this.m_currentView)
            {
                case DateView.Day:
                    this.vScrollBar.Maximum = 0x30;
                    this.vScrollBar.LargeChange = (this.m_drawingArea.Height - this.m_headerHeight) / this.m_halfHourHeight;
                    this.vScrollBar.Value = this.m_dayViewScrollBarValue;
                    this.vScrollBar.Enabled = true;
                    this.vScrollBar.Visible = true;
                    break;

                case DateView.Week:
                    this.vScrollBar.Maximum = 0x30;
                    this.vScrollBar.LargeChange = ((this.m_drawingArea.Height - this.m_headerHeight) - 20) / this.m_halfHourHeight;
                    this.vScrollBar.Value = this.m_weekViewScrollBarValue;
                    this.vScrollBar.Enabled = true;
                    this.vScrollBar.Visible = true;
                    break;

                case DateView.Month:
                    this.vScrollBar.Visible = false;
                    break;

                case DateView.Agenda:
                {
                    int num = (this.m_drawingArea.Height - this.m_headerHeight) / this.m_agendaItemHeight;
                    if (num <= this.m_applicableTimers.Count)
                    {
                        this.vScrollBar.Maximum = this.m_applicableTimers.Count;
                        this.vScrollBar.LargeChange = num;
                        if (this.m_agendaViewScrollBarValue <= this.vScrollBar.Maximum)
                        {
                            this.vScrollBar.Value = this.m_agendaViewScrollBarValue;
                        }
                        else
                        {
                            this.vScrollBar.Value = this.vScrollBar.Maximum;
                        }
                        this.vScrollBar.Enabled = true;
                        this.vScrollBar.Visible = true;
                        break;
                    }
                    this.vScrollBar.Enabled = false;
                    this.vScrollBar.Value = 0;
                    break;
                }
            }
            if (this.vScrollBar.Visible && ((this.vScrollBar.Value + this.vScrollBar.LargeChange) > this.vScrollBar.Maximum))
            {
                this.vScrollBar.Value = (this.vScrollBar.Maximum - this.vScrollBar.LargeChange) + 1;
            }
            this.SetDrawingArea();
        }

        private void WeekViewCalcs()
        {
            foreach (Vixen.Timer timer in this.m_applicableTimers)
            {
                timer.DisplayBounds.Clear();
            }
            DateTime startDateTime = this.m_currentDate.AddDays((double) -this.m_currentDate.DayOfWeek);
            List<Vixen.Timer> applicableTimers = new List<Vixen.Timer>();
            float num = ((float) (this.m_drawingArea.Width - this.m_timeGutter)) / 7f;
            int num2 = 0;
            while (num2 < 7)
            {
                applicableTimers = this.ReturnApplicableTimersSubset(startDateTime, this.m_oneDayTimeSpan, this.m_applicableTimers);
                this.CalcDayBlocksIn(applicableTimers, new Rectangle((int) (((this.m_drawingArea.Left + this.m_timeGutter) + (num * num2)) + 2f), (this.m_drawingArea.Top + this.m_headerHeight) + this.m_halfHourHeight, ((int) num) - 2, ((this.m_drawingArea.Height - this.m_headerHeight) - this.m_halfHourHeight) - 1), new TimeSpan(this.vScrollBar.Value / 2, (this.vScrollBar.Value % 2) * 30, 0));
                num2++;
                startDateTime += this.m_oneDayTimeSpan;
            }
        }

        public bool ScheduleDisabled
        {
            get
            {
                return this.checkBoxDisableSchedule.Checked;
            }
        }

        public Vixen.Timers Timers
        {
            get
            {
                this.m_timersObject.TimerArray = this.m_timers.ToArray();
                return this.m_timersObject;
            }
        }

        private enum DateView
        {
            Day,
            Week,
            Month,
            Agenda
        }
    }
}

