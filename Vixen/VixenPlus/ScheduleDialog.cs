using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VixenPlus
{
	internal partial class ScheduleDialog : Form
	{
		//TODO: Load these images the proper way
		//ComponentResourceManager manager = new ComponentResourceManager(typeof(ScheduleDialog));
		//this.toolStripButtonToday.Image = (Image) manager.GetObject("toolStripButtonToday.Image");
		//this.toolStripButtonDayView.Image = (Image) manager.GetObject("toolStripButtonDayView.Image");
		//this.toolStripButtonWeekView.Image = (Image) manager.GetObject("toolStripButtonWeekView.Image");
		//this.toolStripButtonMonthView.Image = (Image) manager.GetObject("toolStripButtonMonthView.Image");
		//this.toolStripButtonAgendaView.Image = (Image) manager.GetObject("toolStripButtonAgendaView.Image");
		private readonly Font m_agendaViewItemFont = new Font("Arial", 10f, FontStyle.Bold);
		private readonly Font m_agendaViewTimeFont = new Font("Arial", 8f);
		private readonly SolidBrush m_backgroundBrush = new SolidBrush(Color.FromArgb(0xff, 0xff, 0xd5));
		private readonly Font m_dayViewHeaderFont = new Font("Arial", 12f, FontStyle.Bold);

		private readonly string[] m_days = new[]
			{"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};

		private readonly Pen m_halfHourPen = new Pen(Color.FromArgb(0xff, 0xef, 0xc7));
		private readonly Pen m_hourPen = new Pen(Color.FromArgb(0xf6, 0xdb, 0xa2));
		private readonly TimeSpan m_oneDayTimeSpan = new TimeSpan(1, 0, 0, 0);
		private readonly TimeSpan m_oneWeekTimeSpan = new TimeSpan(7, 0, 0, 0);
		private readonly Font m_timeLargeFont = new Font("Tahoma", 16f);
		private readonly Pen m_timeLinePen = new Pen(Color.FromKnownColor(KnownColor.ControlDark));
		private readonly Font m_timeSmallFont = new Font("Tahoma", 8f);
		private readonly List<Timer> m_timers;
		private readonly Timers m_timersObject;
		private const int m_agendaItemHeight = 50;
		private int m_agendaViewScrollBarValue;
		private List<Timer> m_applicableTimers;

		private Rectangle m_buttonLeftBounds;
		private Rectangle m_buttonRightBounds;
		private DateTime m_currentDate;
		private DateView m_currentView;

		private int m_dayViewScrollBarValue;
		private Rectangle m_drawingArea;
		private const int m_halfHourHeight = 20;
		private int m_headerHeight = 30;
		private bool m_inLeftButtonBounds;
		private bool m_inRightButtonBounds;
		private const int m_monthBarHeight = 10;
		private const int m_monthViewBottomMargin = 40;
		private int m_monthViewColSize;
		private const int m_monthViewLeftMargin = 40;
		private const int m_monthViewRightMargin = 40;
		private int m_monthViewRowSize;
		private const int m_monthViewTopMargin = 40;
		private Point m_mouseDownAt;
		private bool m_resizing;
		private const int m_timeGutter = 50;
		private int m_weekViewScrollBarValue;

		public ScheduleDialog(Timers timers)
		{
			m_resizing = true;
			InitializeComponent();
			m_resizing = false;
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			m_timersObject = timers;
			checkBoxDisableSchedule.Checked = timers.TimersDisabled;
			m_timers = new List<Timer>();
			m_applicableTimers = new List<Timer>();
			foreach (Timer timer in timers.TimerArray)
			{
				m_timers.Add(timer.Clone());
			}
			m_drawingArea = new Rectangle();
			SetDrawingArea();
		}

		public bool ScheduleDisabled
		{
			get { return checkBoxDisableSchedule.Checked; }
		}

		public Timers Timers
		{
			get
			{
				m_timersObject.TimerArray = m_timers.ToArray();
				return m_timersObject;
			}
		}

		private void AddTimer(Timer timer)
		{
			m_timers.Add(timer);
		}

		private void AgendaViewCalcs()
		{
			int num = m_drawingArea.Top + m_headerHeight;
			foreach (Timer timer in m_applicableTimers)
			{
				timer.DisplayBounds.Clear();
			}
			m_applicableTimers.Sort();
			for (int i = vScrollBar.Value; i < m_applicableTimers.Count; i++)
			{
				m_applicableTimers[i].DisplayBounds.Add(new ReferenceRectF(m_drawingArea.Left, num, m_drawingArea.Width,
				                                                           m_agendaItemHeight));
				num += m_agendaItemHeight;
			}
		}

		private void CalcDayBlocksIn(List<Timer> applicableTimers, Rectangle area, TimeSpan visibleStart)
		{
			var list = new List<ReferenceRectF>();
			var list2 = new List<ReferenceRectF>();
			foreach (Timer timer in applicableTimers)
			{
				var b = new ReferenceRectF();
				b.X = area.X;
				b.Width = area.Width;
				TimeSpan span = timer.StartTime - visibleStart;
				b.Y = ((((float) span.TotalMinutes)/30f)*m_halfHourHeight) + area.Top;
				b.Height = (((float) timer.TimerLength.TotalMinutes)/30f)*m_halfHourHeight;
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
						float num3 = area.Width/(list2.Count + 1);
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
			m_applicableTimers = ReturnApplicableTimersSubset(startDateTime, span, m_timers);
			ViewCalcs();
		}

		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			m_mouseDownAt = base.PointToClient(Cursor.Position);
			switch (m_currentView)
			{
				case DateView.Day:
					if ((((vScrollBar.Value + ((m_mouseDownAt.Y - (m_drawingArea.Y + m_headerHeight))/m_halfHourHeight)) < 0x30) &&
					     (m_mouseDownAt.X >= (m_drawingArea.X + m_timeGutter))) &&
					    (m_mouseDownAt.Y >= (m_drawingArea.Y + m_headerHeight)))
					{
						break;
					}
					e.Cancel = true;
					return;

				case DateView.Week:
					if ((((vScrollBar.Value +
					       ((m_mouseDownAt.Y - ((m_drawingArea.Y + m_headerHeight) + m_halfHourHeight))/m_halfHourHeight)) < 0x30) &&
					     (m_mouseDownAt.X >= (m_drawingArea.X + m_timeGutter))) &&
					    (m_mouseDownAt.Y >= ((m_drawingArea.Y + m_headerHeight) + m_halfHourHeight)))
					{
						break;
					}
					e.Cancel = true;
					return;

				case DateView.Month:
					e.Cancel = true;
					return;

				case DateView.Agenda:
					if (((m_mouseDownAt.X >= m_drawingArea.X) && (m_mouseDownAt.Y >= (m_drawingArea.Y + m_headerHeight))) &&
					    (((((m_mouseDownAt.Y - m_drawingArea.Y) - m_headerHeight)/m_agendaItemHeight) + vScrollBar.Value) <
					     m_applicableTimers.Count))
					{
						break;
					}
					e.Cancel = true;
					return;
			}
			if (FindApplicableTimerAt(m_mouseDownAt) == null)
			{
				toolStripMenuItemRemove.Enabled = false;
				toolStripMenuItemAddEdit.Text = "Add";
			}
			else
			{
				toolStripMenuItemRemove.Enabled = true;
				toolStripMenuItemAddEdit.Text = "Edit";
			}
		}

		private void DayViewCalcs()
		{
			foreach (Timer timer in m_applicableTimers)
			{
				timer.DisplayBounds.Clear();
			}
			CalcDayBlocksIn(m_applicableTimers,
			                new Rectangle(m_drawingArea.Left + m_timeGutter, m_drawingArea.Top + m_headerHeight,
			                              (m_drawingArea.Width - m_timeGutter) - 1, (m_drawingArea.Height - m_headerHeight) - 1),
			                new TimeSpan(vScrollBar.Value/2, (vScrollBar.Value%2)*30, 0));
		}


		private void DrawAgendaView(Graphics g)
		{
			g.FillRectangle(Brushes.White, m_drawingArea.Left, m_drawingArea.Top + m_headerHeight, m_drawingArea.Width,
			                m_drawingArea.Height - m_headerHeight);
			var brush =
				new LinearGradientBrush(new Rectangle(m_drawingArea.Left, m_drawingArea.Top, m_drawingArea.Width, m_headerHeight),
				                        Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
			g.FillRectangle(brush, m_drawingArea.Left, m_drawingArea.Top, m_drawingArea.Width, m_headerHeight);
			DrawHeaderButtons(g, m_inLeftButtonBounds, m_inRightButtonBounds);
			g.DrawString(m_currentDate.ToLongDateString(), m_dayViewHeaderFont, Brushes.White, (m_drawingArea.Left + 10),
			             (m_drawingArea.Top + 5));
			int num = 0;
			for (int i = (m_drawingArea.Top + m_headerHeight) + m_agendaItemHeight;
			     (num < m_applicableTimers.Count) && (i < m_drawingArea.Bottom);
			     i += m_agendaItemHeight)
			{
				g.DrawLine(Pens.Gray, m_drawingArea.Left, i, m_drawingArea.Right, i);
				num++;
			}
			foreach (Timer timer in m_applicableTimers)
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
					g.DrawString(timer.ProgramName, m_agendaViewItemFont, gray, (tf.Left + 16f), (tf.Top + 10f));
					g.DrawString(
						string.Format("{0} - {1}", timer.StartDateTime.ToString("h:mm tt"), timer.EndDateTime.ToString("h:mm tt")),
						m_agendaViewTimeFont, gray, (tf.Left + 16f), (tf.Top + 30f));
				}
			}
		}

		private void DrawDayView(Graphics g)
		{
			if (g.ClipBounds.Left < m_timeGutter)
			{
				DrawTimes(g);
			}
			if (g.ClipBounds.Top < (m_drawingArea.Top + m_headerHeight))
			{
				var brush =
					new LinearGradientBrush(
						new Rectangle(m_drawingArea.Left, m_drawingArea.Top, m_drawingArea.Width - m_timeGutter, m_headerHeight),
						Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
				g.FillRectangle(brush, m_drawingArea.Left + m_timeGutter, m_drawingArea.Top, m_drawingArea.Width - m_timeGutter,
				                m_headerHeight);
				DrawHeaderButtons(g, m_inLeftButtonBounds, m_inRightButtonBounds);
				g.DrawString(m_currentDate.ToLongDateString(), m_dayViewHeaderFont, Brushes.White,
				             ((m_drawingArea.Left + m_timeGutter) + 10), (m_drawingArea.Top + 5));
			}
			if (g.ClipBounds.Bottom > (m_drawingArea.Top + m_headerHeight))
			{
				DrawTimerBlocks(m_applicableTimers, g);
			}
		}

		private void DrawHeaderButtons(Graphics g, bool hoverLeft, bool hoverRight)
		{
			const int width = 0x12;
			const int num2 = 8;
			int x = m_drawingArea.Width - (((width + num2) + width) + num2);
			int y = m_drawingArea.Top + ((m_headerHeight - width)/2);
			var pointArray3 = new[] {new Point(x + 12, y + 5), new Point(x + 12, y + 13), new Point(x + 6, y + 9)};
			Point[] points = pointArray3;
			pointArray3 = new[]
				{
					new Point(((x + 6) + width) + num2, y + 5), new Point(((x + 6) + width) + num2, y + 13),
					new Point(((x + 12) + width) + num2, y + 9)
				};
			Point[] pointArray2 = pointArray3;
			var brush = new LinearGradientBrush(new Rectangle(x + 1, y + 1, width - 1, m_headerHeight),
			                                    Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
			var brush2 = new LinearGradientBrush(new Rectangle(x + 1, y + 1, width - 1, width - 1),
			                                     Color.FromArgb(0x77, 0xa5, 0xd6), Color.FromArgb(0x18, 0x4d, 0x94), 90f);
			m_buttonLeftBounds = new Rectangle(x, y, width, width);
			m_buttonRightBounds = new Rectangle((x + width) + num2, y, width, width);
			g.FillRectangle(hoverLeft ? brush2 : brush, (x + 1), (y + 1), (width - 1), (width - 1));
			g.FillRectangle(hoverRight ? brush2 : brush, (((x + 1) + width) + num2), (y + 1), (width - 1), (width - 1));
			DrawRoundRect(g, Pens.White, x, y, width, width, 3f);
			DrawRoundRect(g, Pens.White, ((x + width) + num2), y, width, width, 3f);
			g.FillPolygon(Brushes.White, points);
			g.FillPolygon(Brushes.White, pointArray2);
		}

		private void DrawMonthView(Graphics g)
		{
			LinearGradientBrush brush;
			g.FillRectangle(m_backgroundBrush, m_drawingArea.Left, m_drawingArea.Top + m_headerHeight, m_drawingArea.Width,
			                m_drawingArea.Height - m_headerHeight);
			int num = DateTime.DaysInMonth(m_currentDate.Year, m_currentDate.Month);
			var time = new DateTime(m_currentDate.Year, m_currentDate.Month, 1);
			int num2 = num + (int) time.DayOfWeek;
			int num3 = ((num2%7) > 0) ? ((num2/7) + 1) : (num2/7);
			if (g.ClipBounds.Bottom > (m_drawingArea.Top + m_headerHeight))
			{
				int num6 = (m_drawingArea.Top + m_monthViewTopMargin) + m_headerHeight;
				int num7 = num6 + (m_monthViewRowSize*num3);
				int num4 = m_drawingArea.Left + m_monthViewLeftMargin;
				int num9 = 0;
				while (num9 <= 7)
				{
					g.DrawLine(Pens.Black, num4, num6, num4, num7);
					num9++;
					num4 += m_monthViewColSize;
				}
				num4 = m_drawingArea.Left + m_monthViewLeftMargin;
				int num5 = num4 + (m_monthViewColSize*7);
				num6 = (m_drawingArea.Top + m_monthViewTopMargin) + m_headerHeight;
				int num8 = 0;
				while (num8 <= num3)
				{
					g.DrawLine(Pens.Black, num4, num6, num5, num6);
					num8++;
					num6 += m_monthViewRowSize;
				}
				var dayOfWeek = (int) time.DayOfWeek;
				num6 = ((m_drawingArea.Top + m_headerHeight) + m_monthViewTopMargin) + 5;
				DateTime startDateTime = time;
				float num12 = (m_monthViewColSize)/48f;
				for (int i = 1; i <= num; i++)
				{
					num4 = (m_drawingArea.Left + m_monthViewLeftMargin) + (dayOfWeek*m_monthViewColSize);
					if (startDateTime == m_currentDate)
					{
						brush =
							new LinearGradientBrush(
								new Rectangle(((m_drawingArea.Left + m_monthViewLeftMargin) + (dayOfWeek*m_monthViewColSize)) + 4, num6 - 1, 20,
								              m_headerHeight), Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
						g.FillRectangle(brush, ((m_drawingArea.Left + m_monthViewLeftMargin) + (dayOfWeek*m_monthViewColSize)) + 4,
						                num6 - 1, 20, 15);
						g.DrawString(i.ToString(), m_timeSmallFont, Brushes.White, (num4 + 5), num6);
					}
					else
					{
						g.DrawString(i.ToString(), m_timeSmallFont, Brushes.Black, (num4 + 5), num6);
					}
					g.FillRectangle(new SolidBrush(m_halfHourPen.Color), num4 + 1, ((num6 - 5) + m_monthViewRowSize) - 10,
					                m_monthViewColSize - 2, 10);
					var brush2 = new SolidBrush(m_hourPen.Color);
					float num13 = ((num6 - 5) + m_monthViewRowSize) - m_monthBarHeight;
					foreach (Timer timer in ReturnApplicableTimersSubset(startDateTime, m_oneDayTimeSpan, m_applicableTimers))
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
						num6 += m_monthViewRowSize;
					}
				}
			}
			if (g.ClipBounds.Top < (m_drawingArea.Top + m_headerHeight))
			{
				brush =
					new LinearGradientBrush(new Rectangle(m_drawingArea.Left, m_drawingArea.Top, m_drawingArea.Width, m_headerHeight),
					                        Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
				g.FillRectangle(brush, m_drawingArea.Left, m_drawingArea.Top, m_drawingArea.Width, m_headerHeight);
				DrawHeaderButtons(g, m_inLeftButtonBounds, m_inRightButtonBounds);
				g.DrawString(m_currentDate.ToString("Y"), m_dayViewHeaderFont, Brushes.White, (m_drawingArea.Left + 10),
				             (m_drawingArea.Top + 5));
			}
		}

		private void DrawRoundRect(Graphics g, Pen p, float X, float Y, float width, float height, float radius)
		{
			var path = new GraphicsPath();
			path.AddLine(X + radius, Y, (X + width) - (radius*2f), Y);
			path.AddArc((X + width) - (radius*2f), Y, radius*2f, radius*2f, 270f, 90f);
			path.AddLine((X + width), (Y + radius), (X + width), ((Y + height) - (radius*2f)));
			path.AddArc(((X + width) - (radius*2f)), ((Y + height) - (radius*2f)), (radius*2f), (radius*2f), 0f, 90f);
			path.AddLine(((X + width) - (radius*2f)), (Y + height), (X + radius), (Y + height));
			path.AddArc(X, (Y + height) - (radius*2f), radius*2f, radius*2f, 90f, 90f);
			path.AddLine(X, (Y + height) - (radius*2f), X, Y + radius);
			path.AddArc(X, Y, radius*2f, radius*2f, 180f, 90f);
			path.CloseFigure();
			g.DrawPath(p, path);
			path.Dispose();
		}

		private void DrawTimerBlocks(List<Timer> timers, Graphics g)
		{
			var layoutRectangle = new RectangleF();
			foreach (Timer timer in timers)
			{
				foreach (ReferenceRectF tf in timer.DisplayBounds)
				{
					g.FillRectangle(Brushes.White, tf.ToRectangleF());
					g.DrawRectangle(Pens.Black, tf.X, tf.Y, tf.Width, tf.Height);
					layoutRectangle.X = tf.X + 2f;
					layoutRectangle.Y = tf.Y;
					layoutRectangle.Width = tf.Width;
					layoutRectangle.Height = Math.Min(13f, tf.Height);
					foreach (string str in timer.ToString().Split(new[] {'|'}))
					{
						g.DrawString(str, m_timeSmallFont, Brushes.Black, layoutRectangle);
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
			g.FillRectangle(m_backgroundBrush, (m_drawingArea.Left + m_timeGutter), (m_drawingArea.Top + m_headerHeight),
			                (m_drawingArea.Width - m_timeGutter), (m_drawingArea.Height - m_headerHeight));
			g.DrawLine(m_timeLinePen, m_drawingArea.Left, m_drawingArea.Top + m_headerHeight,
			           (m_drawingArea.Left + m_timeGutter) - 5, m_drawingArea.Top + m_headerHeight);
			for (
				num = (m_drawingArea.Top + m_headerHeight) + (((vScrollBar.Value%2) == 0) ? (m_halfHourHeight*2) : m_halfHourHeight);
				num < m_drawingArea.Height;
				num += m_halfHourHeight*2)
			{
				g.DrawLine(m_hourPen, m_drawingArea.Left + m_timeGutter, num, m_drawingArea.Width, num);
				g.DrawLine(m_timeLinePen, m_drawingArea.Left + 5, num, (m_drawingArea.Left + m_timeGutter) - 5, num);
			}
			for (
				num = (m_drawingArea.Top + m_headerHeight) + (((vScrollBar.Value%2) == 0) ? m_halfHourHeight : (m_halfHourHeight*2));
				num < m_drawingArea.Height;
				num += m_halfHourHeight*2)
			{
				g.DrawLine(m_halfHourPen, m_drawingArea.Left + m_timeGutter, num, m_drawingArea.Width, num);
			}
			int num2 = ((vScrollBar.Value%2) == 1) ? ((vScrollBar.Value/2) + 1) : (vScrollBar.Value/2);
			int num3 = m_timeGutter >> 1;
			int num4 = -1;
			for (num = ((m_drawingArea.Top + 3) + m_headerHeight) + (((vScrollBar.Value%2) == 0) ? 0 : m_halfHourHeight);
			     (num < m_drawingArea.Height) && (num2 < 0x18);
			     num += m_halfHourHeight*2)
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
				g.DrawString(str, m_timeLargeFont, Brushes.Black,
				             (((m_drawingArea.Left + num3) - ((int) g.MeasureString(str, m_timeLargeFont).Width)) + 6), num);
				if (num4 != (num2/12))
				{
					num4 = num2/12;
					str2 = (num4 == 0) ? "am" : "pm";
				}
				else
				{
					str2 = "00";
				}
				g.DrawString(str2, m_timeSmallFont, Brushes.Black, ((m_drawingArea.Left + num3) + 2), num);
				num2++;
			}
		}

		private void DrawWeekView(Graphics g)
		{
			float num2;
			int num3;
			g.FillRectangle(m_backgroundBrush, m_drawingArea.Left + m_timeGutter, m_drawingArea.Top + m_headerHeight,
			                m_drawingArea.Width - m_timeGutter, 20);
			m_headerHeight = 50;
			DrawTimes(g);
			m_headerHeight = 30;
			float num = ((m_drawingArea.Width - m_timeGutter))/7f;
			for (num3 = 1; num3 < 7; num3++)
			{
				num2 = (m_drawingArea.Left + (num*num3)) + m_timeGutter;
				g.DrawLine(Pens.Black, num2, (m_drawingArea.Top + m_headerHeight), num2, m_drawingArea.Height);
			}
			var brush =
				new LinearGradientBrush(
					new Rectangle(m_drawingArea.Left, m_drawingArea.Top, m_drawingArea.Width - m_timeGutter, m_headerHeight),
					Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
			var brush2 = new SolidBrush(m_halfHourPen.Color);
			var dayOfWeek = (int) m_currentDate.DayOfWeek;
			for (num3 = 0; num3 < 7; num3++)
			{
				num2 = (m_drawingArea.Left + (num*num3)) + m_timeGutter;
				if (dayOfWeek == num3)
				{
					g.FillRectangle(brush, num2 + 2f, ((m_drawingArea.Top + m_headerHeight) + 2), num - 5f, 17f);
					g.DrawRectangle(Pens.Navy, num2 + 2f, ((m_drawingArea.Top + m_headerHeight) + 2), num - 5f, 17f);
					g.DrawString(m_days[num3], m_timeSmallFont, Brushes.White,
					             num2 + ((num - g.MeasureString(m_days[num3], m_timeSmallFont).Width)/2f),
					             ((m_drawingArea.Top + m_headerHeight) + 4));
				}
				else
				{
					g.FillRectangle(brush2, num2 + 2f, ((m_drawingArea.Top + m_headerHeight) + 2), num - 5f, 17f);
					g.DrawRectangle(m_hourPen, num2 + 2f, ((m_drawingArea.Top + m_headerHeight) + 2), num - 5f, 17f);
					g.DrawString(m_days[num3], m_timeSmallFont, Brushes.Black,
					             num2 + ((num - g.MeasureString(m_days[num3], m_timeSmallFont).Width)/2f),
					             ((m_drawingArea.Top + m_headerHeight) + 4));
				}
			}
			g.FillRectangle(brush, m_drawingArea.Left + m_timeGutter, m_drawingArea.Top, m_drawingArea.Width - m_timeGutter,
			                m_headerHeight);
			DrawHeaderButtons(g, m_inLeftButtonBounds, m_inRightButtonBounds);
			DateTime time = m_currentDate.AddDays(-(double) m_currentDate.DayOfWeek);
			DateTime time2 = time.AddDays(6.0);
			g.DrawString(string.Format("{0}   -   {1}", time.ToString("m"), time2.ToString("m")), m_dayViewHeaderFont,
			             Brushes.White, ((m_drawingArea.Left + m_timeGutter) + 10), (m_drawingArea.Top + 5));
			DrawTimerBlocks(m_applicableTimers, g);
		}

		private void EditOrAddTimerAt(Point point, int viewRelativeY, DateTime viewStartDate, TimeSpan viewLengthSpan)
		{
			Timer timer;
			timer = FindApplicableTimerAt(point);
			int hour = (vScrollBar.Value + (viewRelativeY/m_halfHourHeight))/2;
			int minute = ((vScrollBar.Value + (viewRelativeY/m_halfHourHeight))%2)*30;
			if (hour < 0x18)
			{
				TimerDialog dialog;
				dialog = timer == null ? new TimerDialog(new DateTime(m_currentDate.Year, m_currentDate.Month, m_currentDate.Day, hour, minute, 0)) : new TimerDialog(timer);
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					if (timer == null)
					{
						AddTimer(dialog.Timer);
					}
					else if (timer != dialog.Timer)
					{
						timer.Copy(dialog.Timer);
					}
					CompileApplicableTimers(viewStartDate, viewLengthSpan);
					Refresh();
				}
				dialog.Dispose();
			}
		}

		private Timer FindApplicableTimerAt(Point point)
		{
			foreach (Timer timer in m_applicableTimers)
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
			bool inLeftButtonBounds = m_inLeftButtonBounds;
			bool inRightButtonBounds = m_inRightButtonBounds;
			m_inLeftButtonBounds = m_buttonLeftBounds.Contains(location);
			m_inRightButtonBounds = m_buttonRightBounds.Contains(location);
			if ((inLeftButtonBounds != m_inLeftButtonBounds) || (inRightButtonBounds != m_inRightButtonBounds))
			{
				Refresh();
			}
		}


		private void MonthViewCalcs()
		{
			foreach (Timer timer in m_applicableTimers)
			{
				timer.DisplayBounds.Clear();
			}
			int num = DateTime.DaysInMonth(m_currentDate.Year, m_currentDate.Month);
			var time = new DateTime(m_currentDate.Year, m_currentDate.Month, 1);
			int num2 = num + (int) time.DayOfWeek;
			int num3 = ((num2%7) > 0) ? ((num2/7) + 1) : (num2/7);
			m_monthViewRowSize = (((m_drawingArea.Height - m_headerHeight) - m_monthViewTopMargin) - m_monthViewBottomMargin)/
			                     num3;
			m_monthViewColSize = ((m_drawingArea.Width - m_monthViewLeftMargin) - m_monthViewRightMargin)/7;
			var dayOfWeek = (int) time.DayOfWeek;
			int num5 = ((m_drawingArea.Top + m_headerHeight) + m_monthViewTopMargin) + 5;
			DateTime startDateTime = time;
			float num8 = (m_monthViewColSize)/48f;
			for (int i = 1; i <= num; i++)
			{
				int num4 = (m_drawingArea.Left + m_monthViewLeftMargin) + (dayOfWeek*m_monthViewColSize);
				float num9 = ((num5 - 5) + m_monthViewRowSize) - m_monthBarHeight;
				foreach (Timer timer in ReturnApplicableTimersSubset(startDateTime, m_oneDayTimeSpan, m_applicableTimers))
				{
					var item = new ReferenceRectF();
					item.X = (num4 + 1) + ((timer.StartTime.Hours*2)*num8);
					item.Y = num9;
					item.Width = (float) ((timer.TimerLength.TotalHours*2.0)*num8);
					item.Height = m_monthBarHeight;
					timer.DisplayBounds.Add(item);
				}
				startDateTime = startDateTime.AddDays(1.0);
				if (++dayOfWeek == 7)
				{
					dayOfWeek = 0;
					num5 += m_monthViewRowSize;
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			switch (m_currentView)
			{
				case DateView.Day:
					DrawDayView(e.Graphics);
					break;

				case DateView.Week:
					DrawWeekView(e.Graphics);
					break;

				case DateView.Month:
					DrawMonthView(e.Graphics);
					break;

				case DateView.Agenda:
					DrawAgendaView(e.Graphics);
					break;
			}
		}

		private List<Timer> ReturnApplicableTimersSubset(DateTime startDateTime, TimeSpan span, List<Timer> seedList)
		{
			var list = new List<Timer>();
			DateTime time = startDateTime + span;
			foreach (Timer timer in seedList)
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
							var time2 = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, timer.StartTime.Hours,
							                         timer.StartTime.Minutes, 0);
							var time3 = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, timer.EndTime.Hours,
							                         timer.EndTime.Minutes, 0);
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
						if ((((time.Month*0x1f) + time.Day) < ((timer.StartDateTime.Month*0x1f) + timer.StartDateTime.Day)) ||
						    (((startDateTime.Month*0x1f) + startDateTime.Day) > ((timer.EndDateTime.Month*0x1f) + timer.EndDateTime.Day)))
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
					num |= (1) << (int) time4.DayOfWeek;
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
				Label_02C9:
				;
			}
			return list;
		}

		private void ScheduleDialog_Load(object sender, EventArgs e)
		{
			SetCurrentView(DateView.Day, DateTime.Today);
		}

		private void ScheduleDialog_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Y >= (m_drawingArea.Top + m_headerHeight))
			{
				switch (m_currentView)
				{
					case DateView.Day:
						if (e.Y >= (m_drawingArea.Top + m_headerHeight))
						{
							if (e.X >= (m_drawingArea.Left + m_timeGutter))
							{
								EditOrAddTimerAt(e.Location, (e.Y - m_headerHeight) - m_drawingArea.Top, m_currentDate, m_oneDayTimeSpan);
								Refresh();
							}
							break;
						}
						break;

					case DateView.Week:
						if (e.Y >= (m_drawingArea.Top + m_headerHeight))
						{
							if (e.X >= (m_drawingArea.Left + m_timeGutter))
							{
								DateTime viewStartDate = m_currentDate.AddDays(-(double) m_currentDate.DayOfWeek);
								m_currentDate =
									viewStartDate.AddDays(((int) (((e.X - m_timeGutter))/(((m_drawingArea.Width - m_timeGutter))/7f))));
								EditOrAddTimerAt(e.Location, ((e.Y - m_headerHeight) - m_halfHourHeight) - m_drawingArea.Top, viewStartDate,
								                 m_oneWeekTimeSpan);
								Refresh();
							}
							break;
						}
						break;

					case DateView.Month:
						{
							DateTime minValue = DateTime.MinValue;
							if (TryGetMonthDateAt(e.Location, ref minValue))
							{
								SetCurrentView(DateView.Day, minValue);
							}
							break;
						}
					case DateView.Agenda:
						if (e.Y >= (m_drawingArea.Top + m_headerHeight))
						{
							if (((((e.Y - m_drawingArea.Top) - m_headerHeight)/m_agendaItemHeight) + vScrollBar.Value) <
							    m_applicableTimers.Count)
							{
								EditOrAddTimerAt(e.Location, (e.Y - m_headerHeight) - m_drawingArea.Top, m_currentDate, m_oneDayTimeSpan);
							}
							Refresh();
						}
						break;
				}
			}
		}

		private void ScheduleDialog_MouseDown(object sender, MouseEventArgs e)
		{
			switch (m_currentView)
			{
				case DateView.Day:
				case DateView.Agenda:
					if (!m_inLeftButtonBounds)
					{
						if (m_inRightButtonBounds)
						{
							m_currentDate = m_currentDate.AddDays(1.0);
							CompileApplicableTimers(m_currentDate, m_oneDayTimeSpan);
							Refresh();
						}
						break;
					}
					m_currentDate = m_currentDate.AddDays(-1.0);
					CompileApplicableTimers(m_currentDate, m_oneDayTimeSpan);
					Refresh();
					break;

				case DateView.Week:
					if (!m_inLeftButtonBounds)
					{
						if (m_inRightButtonBounds)
						{
							m_currentDate = m_currentDate.AddDays(7.0);
							CompileApplicableTimers(m_currentDate.AddDays(-(double) m_currentDate.DayOfWeek), m_oneWeekTimeSpan);
							Refresh();
						}
						break;
					}
					m_currentDate = m_currentDate.AddDays(-7.0);
					CompileApplicableTimers(m_currentDate.AddDays(-(double) m_currentDate.DayOfWeek), m_oneWeekTimeSpan);
					Refresh();
					break;

				case DateView.Month:
					if (!m_inLeftButtonBounds)
					{
						if (m_inRightButtonBounds)
						{
							m_currentDate = m_currentDate.AddMonths(1);
							Refresh();
						}
						break;
					}
					m_currentDate = m_currentDate.AddMonths(-1);
					Refresh();
					break;
			}
		}

		private void ScheduleDialog_MouseMove(object sender, MouseEventArgs e)
		{
			DateTime minValue;
			switch (m_currentView)
			{
				case DateView.Day:
					SetToolTip(this, e.Location);
					break;

				case DateView.Week:
					{
						float num = ((m_drawingArea.Width - m_timeGutter))/7f;
						minValue = m_currentDate.AddDays(-(double) (m_currentDate.DayOfWeek + ((int) (((e.X - m_timeGutter))/num))));
						if (!SetToolTip(this, e.Location, ReturnApplicableTimersSubset(minValue, m_oneDayTimeSpan, m_applicableTimers)))
						{
							break;
						}
						return;
					}
				case DateView.Month:
					minValue = DateTime.MinValue;
					if (!TryGetMonthDateAt(e.Location, ref minValue) ||
					    !SetToolTip(this, e.Location, ReturnApplicableTimersSubset(minValue, m_oneDayTimeSpan, m_applicableTimers)))
					{
						break;
					}
					return;

				case DateView.Agenda:
					SetToolTip(this, e.Location);
					break;
			}
			HoverNavButtons(e.Location);
		}

		private void ScheduleDialog_Resize(object sender, EventArgs e)
		{
			if (!(m_resizing || (base.WindowState == FormWindowState.Minimized)))
			{
				VScrollCheck();
				ViewCalcs();
				Refresh();
			}
		}

		private void ScheduleDialog_ResizeBegin(object sender, EventArgs e)
		{
			m_resizing = true;
		}

		private void ScheduleDialog_ResizeEnd(object sender, EventArgs e)
		{
			m_resizing = false;
			VScrollCheck();
			ViewCalcs();
			Refresh();
		}

		private void SetCurrentView(DateView view)
		{
			SetCurrentView(view, m_currentDate);
		}

		private void SetCurrentView(DateView view, DateTime date)
		{
			switch (m_currentView)
			{
				case DateView.Day:
					m_dayViewScrollBarValue = vScrollBar.Value;
					toolStripButtonDayView.Checked = false;
					break;

				case DateView.Week:
					m_weekViewScrollBarValue = vScrollBar.Value;
					toolStripButtonWeekView.Checked = false;
					break;

				case DateView.Month:
					toolStripButtonMonthView.Checked = false;
					break;

				case DateView.Agenda:
					m_agendaViewScrollBarValue = vScrollBar.Value;
					toolStripButtonAgendaView.Checked = false;
					break;
			}
			m_currentDate = date;
			m_currentView = view;
			VScrollCheck();
			switch (m_currentView)
			{
				case DateView.Day:
					CompileApplicableTimers(date, m_oneDayTimeSpan);
					toolStripButtonDayView.Checked = true;
					break;

				case DateView.Week:
					CompileApplicableTimers(m_currentDate.AddDays(-(double) m_currentDate.DayOfWeek), m_oneWeekTimeSpan);
					toolStripButtonWeekView.Checked = true;
					break;

				case DateView.Month:
					CompileApplicableTimers(new DateTime(m_currentDate.Year, m_currentDate.Month, 1),
					                        new TimeSpan(DateTime.DaysInMonth(m_currentDate.Year, m_currentDate.Month), 0, 0, 0));
					toolStripButtonMonthView.Checked = true;
					break;

				case DateView.Agenda:
					CompileApplicableTimers(date, m_oneDayTimeSpan);
					toolStripButtonAgendaView.Checked = true;
					break;
			}
			Refresh();
		}

		private void SetDrawingArea()
		{
			m_drawingArea.X = 0;
			m_drawingArea.Y = toolStrip1.Bottom;
			m_drawingArea.Width = vScrollBar.Visible ? vScrollBar.Left : vScrollBar.Right;
			m_drawingArea.Height = panel1.Top;
		}

		private void SetToolTip(Control control, Point location)
		{
			SetToolTip(control, location, m_applicableTimers);
		}

		private bool SetToolTip(Control control, Point location, List<Timer> timers)
		{
			string toolTip = this.toolTip.GetToolTip(control);
			foreach (Timer timer in timers)
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
			SetCurrentView(DateView.Agenda);
		}

		private void toolStripButtonDayView_Click(object sender, EventArgs e)
		{
			SetCurrentView(DateView.Day);
		}

		private void toolStripButtonMonthView_Click(object sender, EventArgs e)
		{
			SetCurrentView(DateView.Month);
		}

		private void toolStripButtonToday_Click(object sender, EventArgs e)
		{
			SetCurrentView(DateView.Day, DateTime.Today);
		}

		private void toolStripButtonWeekView_Click(object sender, EventArgs e)
		{
			SetCurrentView(DateView.Week);
		}

		private void toolStripMenuItemEdit_Click(object sender, EventArgs e)
		{
			switch (m_currentView)
			{
				case DateView.Day:
				case DateView.Agenda:
					EditOrAddTimerAt(m_mouseDownAt, (m_mouseDownAt.Y - m_drawingArea.Y) - m_headerHeight, m_currentDate,
					                 m_oneDayTimeSpan);
					break;

				case DateView.Week:
					{
						DateTime viewStartDate = m_currentDate.AddDays(-(double) m_currentDate.DayOfWeek);
						EditOrAddTimerAt(m_mouseDownAt, ((m_mouseDownAt.Y - m_drawingArea.Y) - m_headerHeight) - m_halfHourHeight,
						                 viewStartDate, m_oneWeekTimeSpan);
						break;
					}
			}
		}

		private void toolStripMenuItemRemove_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show("Remove all occurrences of this timer?", Vendor.ProductName, MessageBoxButtons.YesNo,
				                MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Timer item = FindApplicableTimerAt(m_mouseDownAt);
				m_timers.Remove(item);
				switch (m_currentView)
				{
					case DateView.Day:
					case DateView.Agenda:
						CompileApplicableTimers(m_currentDate, m_oneDayTimeSpan);
						Refresh();
						break;

					case DateView.Week:
						{
							DateTime startDateTime = m_currentDate.AddDays(-(double) m_currentDate.DayOfWeek);
							CompileApplicableTimers(startDateTime, m_oneWeekTimeSpan);
							Refresh();
							break;
						}
				}
			}
		}

		private bool TryGetMonthDateAt(Point p, ref DateTime date)
		{
			if (p.Y < ((m_drawingArea.Top + m_headerHeight) + m_monthViewTopMargin))
			{
				return false;
			}
			if (p.Y > (m_drawingArea.Bottom - m_monthViewBottomMargin))
			{
				return false;
			}
			if (p.X < (m_drawingArea.Left + m_monthViewLeftMargin))
			{
				return false;
			}
			if (p.X > (m_drawingArea.Right - m_monthViewRightMargin))
			{
				return false;
			}
			int num = DateTime.DaysInMonth(m_currentDate.Year, m_currentDate.Month);
			var time = new DateTime(m_currentDate.Year, m_currentDate.Month, 1);
			int num2 = num + (int) time.DayOfWeek;
			int num3 = ((num2%7) > 0) ? ((num2/7) + 1) : (num2/7);
			int num4 = ((((m_drawingArea.Height - m_headerHeight) - m_monthViewTopMargin) - m_monthViewBottomMargin)/num3) + 1;
			int num5 = (((m_drawingArea.Width - m_monthViewLeftMargin) - m_monthViewRightMargin)/7) + 1;
			int num6 = ((p.X - m_drawingArea.Left) - m_monthViewLeftMargin)/num5;
			int num7 = (((p.Y - m_drawingArea.Top) - m_headerHeight) - m_monthViewTopMargin)/num4;
			if ((num7 == 0) && (num6 < (int) time.DayOfWeek))
			{
				return false;
			}
			int num8 = ((((int) time.DayOfWeek + num)%(int) (DayOfWeek.Saturday | DayOfWeek.Monday))) - 1;
			if (num8 == -1)
			{
				num8 = 7;
			}
			if ((num7 == (num3 - 1)) && (num6 > num8))
			{
				return false;
			}
			date = time.AddDays((double) (((num7*7) + num6) - time.DayOfWeek));
			return true;
		}

		private void UpdateCurrentScheduleView()
		{
			int y = toolStrip1.Bottom + m_headerHeight;
			switch (m_currentView)
			{
				case DateView.Day:
					base.Invalidate(new Rectangle(0, y, m_drawingArea.Width, base.ClientRectangle.Height - y));
					break;

				case DateView.Week:
					base.Invalidate(new Rectangle(0, y + m_halfHourHeight, m_drawingArea.Width,
					                              (base.ClientRectangle.Height - y) - m_halfHourHeight));
					break;

				case DateView.Month:
					Refresh();
					break;

				case DateView.Agenda:
					Refresh();
					break;
			}
		}

		private void ViewCalcs()
		{
			switch (m_currentView)
			{
				case DateView.Day:
					DayViewCalcs();
					break;

				case DateView.Week:
					WeekViewCalcs();
					break;

				case DateView.Month:
					MonthViewCalcs();
					break;

				case DateView.Agenda:
					AgendaViewCalcs();
					break;
			}
		}

		private void vScrollBar_ValueChanged(object sender, EventArgs e)
		{
			ViewCalcs();
			UpdateCurrentScheduleView();
		}

		private void VScrollCheck()
		{
			SetDrawingArea();
			switch (m_currentView)
			{
				case DateView.Day:
					vScrollBar.Maximum = 0x30;
					vScrollBar.LargeChange = (m_drawingArea.Height - m_headerHeight)/m_halfHourHeight;
					vScrollBar.Value = m_dayViewScrollBarValue;
					vScrollBar.Enabled = true;
					vScrollBar.Visible = true;
					break;

				case DateView.Week:
					vScrollBar.Maximum = 0x30;
					vScrollBar.LargeChange = ((m_drawingArea.Height - m_headerHeight) - 20)/m_halfHourHeight;
					vScrollBar.Value = m_weekViewScrollBarValue;
					vScrollBar.Enabled = true;
					vScrollBar.Visible = true;
					break;

				case DateView.Month:
					vScrollBar.Visible = false;
					break;

				case DateView.Agenda:
					{
						int num = (m_drawingArea.Height - m_headerHeight)/m_agendaItemHeight;
						if (num <= m_applicableTimers.Count)
						{
							vScrollBar.Maximum = m_applicableTimers.Count;
							vScrollBar.LargeChange = num;
							vScrollBar.Value = m_agendaViewScrollBarValue <= vScrollBar.Maximum ? m_agendaViewScrollBarValue : vScrollBar.Maximum;
							vScrollBar.Enabled = true;
							vScrollBar.Visible = true;
							break;
						}
						vScrollBar.Enabled = false;
						vScrollBar.Value = 0;
						break;
					}
			}
			if (vScrollBar.Visible && ((vScrollBar.Value + vScrollBar.LargeChange) > vScrollBar.Maximum))
			{
				vScrollBar.Value = (vScrollBar.Maximum - vScrollBar.LargeChange) + 1;
			}
			SetDrawingArea();
		}

		private void WeekViewCalcs()
		{
			foreach (Timer timer in m_applicableTimers)
			{
				timer.DisplayBounds.Clear();
			}
			DateTime startDateTime = m_currentDate.AddDays(-(double) m_currentDate.DayOfWeek);
			List<Timer> applicableTimers;
			float num = ((m_drawingArea.Width - m_timeGutter))/7f;
			int num2 = 0;
			while (num2 < 7)
			{
				applicableTimers = ReturnApplicableTimersSubset(startDateTime, m_oneDayTimeSpan, m_applicableTimers);
				CalcDayBlocksIn(applicableTimers,
				                new Rectangle((int) (((m_drawingArea.Left + m_timeGutter) + (num*num2)) + 2f),
				                              (m_drawingArea.Top + m_headerHeight) + m_halfHourHeight, ((int) num) - 2,
				                              ((m_drawingArea.Height - m_headerHeight) - m_halfHourHeight) - 1),
				                new TimeSpan(vScrollBar.Value/2, (vScrollBar.Value%2)*30, 0));
				num2++;
				startDateTime += m_oneDayTimeSpan;
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