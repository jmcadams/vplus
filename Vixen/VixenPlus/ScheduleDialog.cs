using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
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
        private const int AgendaItemHeight = 50;
        private const int HalfHourHeight = 20;
        private const int MonthBarHeight = 10;
        private const int MonthViewBottomMargin = 40;
        private const int MonthViewLeftMargin = 40;
        private const int MonthViewRightMargin = 40;
        private const int MonthViewTopMargin = 40;
        private const int TimeGutter = 50;
        private readonly Font _agendaViewItemFont = new Font("Arial", 10f, FontStyle.Bold);
        private readonly Font _agendaViewTimeFont = new Font("Arial", 8f);
        private readonly SolidBrush _backgroundBrush = new SolidBrush(Color.FromArgb(0xff, 0xff, 0xd5));
        private readonly Font _dayViewHeaderFont = new Font("Arial", 12f, FontStyle.Bold);

        private readonly string[] _days = new[]
            {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};

        private readonly Pen _halfHourPen = new Pen(Color.FromArgb(255, 239, 199));
        private readonly Pen _hourPen = new Pen(Color.FromArgb(246, 219, 162));
        private readonly TimeSpan _oneDayTimeSpan = new TimeSpan(1, 0, 0, 0);
        private readonly TimeSpan _oneWeekTimeSpan = new TimeSpan(7, 0, 0, 0);
        private readonly Font _timeLargeFont = new Font("Tahoma", 16f);
        private readonly Pen _timeLinePen = new Pen(Color.FromKnownColor(KnownColor.ControlDark));
        private readonly Font _timeSmallFont = new Font("Tahoma", 8f);
        private readonly List<Timer> _timers;
        private readonly Timers _timersObject;
        private int _agendaViewScrollBarValue;
        private List<Timer> _applicableTimers;

        private Rectangle _buttonLeftBounds;
        private Rectangle _buttonRightBounds;
        private DateTime _currentDate;
        private DateView _currentView;

        private int _dayViewScrollBarValue;
        private Rectangle _drawingArea;
        private int _headerHeight = 30;
        private bool _inLeftButtonBounds;
        private bool _inRightButtonBounds;
        private bool _isResizing;
        private int _monthViewColSize;
        private int _monthViewRowSize;
        private Point _mouseDownAt;
        private int _weekViewScrollBarValue;

        public ScheduleDialog(Timers timers)
        {
            _isResizing = true;
            InitializeComponent();
            _isResizing = false;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            _timersObject = timers;
            checkBoxDisableSchedule.Checked = timers.TimersDisabled;
            _timers = new List<Timer>();
            _applicableTimers = new List<Timer>();
            foreach (Timer timer in timers.TimerArray)
            {
                _timers.Add(timer.Clone());
            }
            _drawingArea = new Rectangle();
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
                _timersObject.TimerArray = _timers.ToArray();
                return _timersObject;
            }
        }

        private void AddTimer(Timer timer)
        {
            _timers.Add(timer);
        }

        private void AgendaViewCalcs()
        {
            int num = _drawingArea.Top + _headerHeight;
            foreach (Timer timer in _applicableTimers)
            {
                timer.DisplayBounds.Clear();
            }
            _applicableTimers.Sort();
            for (int i = vScrollBar.Value; i < _applicableTimers.Count; i++)
            {
                _applicableTimers[i].DisplayBounds.Add(new ReferenceRectF(_drawingArea.Left, num, _drawingArea.Width,
                                                                          AgendaItemHeight));
                num += AgendaItemHeight;
            }
        }

        private void CalcDayBlocksIn(IEnumerable<Timer> applicableTimers, Rectangle area, TimeSpan visibleStart)
        {
            var list = new List<ReferenceRectF>();
            var list2 = new List<ReferenceRectF>();
            foreach (Timer timer in applicableTimers)
            {
                var b = new ReferenceRectF {X = area.X, Width = area.Width};
                TimeSpan span = timer.StartTime - visibleStart;
                b.Y = ((((float) span.TotalMinutes)/30f)*HalfHourHeight) + area.Top;
                b.Height = (((float) timer.TimerLength.TotalMinutes)/30f)*HalfHourHeight;
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
            _applicableTimers = ReturnApplicableTimersSubset(startDateTime, span, _timers);
            ViewCalcs();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            _mouseDownAt = PointToClient(Cursor.Position);
            switch (_currentView)
            {
                case DateView.Day:
                    if ((((vScrollBar.Value + ((_mouseDownAt.Y - (_drawingArea.Y + _headerHeight))/HalfHourHeight)) < 0x30) &&
                         (_mouseDownAt.X >= (_drawingArea.X + TimeGutter))) &&
                        (_mouseDownAt.Y >= (_drawingArea.Y + _headerHeight)))
                    {
                        break;
                    }
                    e.Cancel = true;
                    return;

                case DateView.Week:
                    if ((((vScrollBar.Value +
                           ((_mouseDownAt.Y - ((_drawingArea.Y + _headerHeight) + HalfHourHeight))/HalfHourHeight)) < 0x30) &&
                         (_mouseDownAt.X >= (_drawingArea.X + TimeGutter))) &&
                        (_mouseDownAt.Y >= ((_drawingArea.Y + _headerHeight) + HalfHourHeight)))
                    {
                        break;
                    }
                    e.Cancel = true;
                    return;

                case DateView.Month:
                    e.Cancel = true;
                    return;

                case DateView.Agenda:
                    if (((_mouseDownAt.X >= _drawingArea.X) && (_mouseDownAt.Y >= (_drawingArea.Y + _headerHeight))) &&
                        (((((_mouseDownAt.Y - _drawingArea.Y) - _headerHeight)/AgendaItemHeight) + vScrollBar.Value) <
                         _applicableTimers.Count))
                    {
                        break;
                    }
                    e.Cancel = true;
                    return;
            }
            if (FindApplicableTimerAt(_mouseDownAt) == null)
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
            foreach (Timer timer in _applicableTimers)
            {
                timer.DisplayBounds.Clear();
            }
            CalcDayBlocksIn(_applicableTimers,
                            new Rectangle(_drawingArea.Left + TimeGutter, _drawingArea.Top + _headerHeight,
                                          (_drawingArea.Width - TimeGutter) - 1, (_drawingArea.Height - _headerHeight) - 1),
                            new TimeSpan(vScrollBar.Value/2, (vScrollBar.Value%2)*30, 0));
        }


        private void DrawAgendaView(Graphics g)
        {
            g.FillRectangle(Brushes.White, _drawingArea.Left, _drawingArea.Top + _headerHeight, _drawingArea.Width,
                            _drawingArea.Height - _headerHeight);
            var brush =
                new LinearGradientBrush(new Rectangle(_drawingArea.Left, _drawingArea.Top, _drawingArea.Width, _headerHeight),
                                        Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
            g.FillRectangle(brush, _drawingArea.Left, _drawingArea.Top, _drawingArea.Width, _headerHeight);
            DrawHeaderButtons(g, _inLeftButtonBounds, _inRightButtonBounds);
            g.DrawString(_currentDate.ToLongDateString(), _dayViewHeaderFont, Brushes.White, (_drawingArea.Left + 10),
                         (_drawingArea.Top + 5));
            int num = 0;
            for (int i = (_drawingArea.Top + _headerHeight) + AgendaItemHeight;
                 (num < _applicableTimers.Count) && (i < _drawingArea.Bottom);
                 i += AgendaItemHeight)
            {
                g.DrawLine(Pens.Gray, _drawingArea.Left, i, _drawingArea.Right, i);
                num++;
            }
            foreach (Timer timer in _applicableTimers)
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
                    g.DrawString(timer.ProgramName, _agendaViewItemFont, gray, (tf.Left + 16f), (tf.Top + 10f));
                    g.DrawString(
                        string.Format("{0} - {1}", timer.StartDateTime.ToString("h:mm tt"), timer.EndDateTime.ToString("h:mm tt")),
                        _agendaViewTimeFont, gray, (tf.Left + 16f), (tf.Top + 30f));
                }
            }
        }

        private void DrawDayView(Graphics g)
        {
            if (g.ClipBounds.Left < TimeGutter)
            {
                DrawTimes(g);
            }
            if (g.ClipBounds.Top < (_drawingArea.Top + _headerHeight))
            {
                var brush =
                    new LinearGradientBrush(
                        new Rectangle(_drawingArea.Left, _drawingArea.Top, _drawingArea.Width - TimeGutter, _headerHeight),
                        Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
                g.FillRectangle(brush, _drawingArea.Left + TimeGutter, _drawingArea.Top, _drawingArea.Width - TimeGutter,
                                _headerHeight);
                DrawHeaderButtons(g, _inLeftButtonBounds, _inRightButtonBounds);
                g.DrawString(_currentDate.ToLongDateString(), _dayViewHeaderFont, Brushes.White,
                             ((_drawingArea.Left + TimeGutter) + 10), (_drawingArea.Top + 5));
            }
            if (g.ClipBounds.Bottom > (_drawingArea.Top + _headerHeight))
            {
                DrawTimerBlocks(_applicableTimers, g);
            }
        }

        private void DrawHeaderButtons(Graphics g, bool hoverLeft, bool hoverRight)
        {
            const int width = 0x12;
            const int num2 = 8;
            int x = _drawingArea.Width - (((width + num2) + width) + num2);
            int y = _drawingArea.Top + ((_headerHeight - width)/2);
            var pointArray3 = new[] {new Point(x + 12, y + 5), new Point(x + 12, y + 13), new Point(x + 6, y + 9)};
            Point[] points = pointArray3;
            pointArray3 = new[]
                {
                    new Point(((x + 6) + width) + num2, y + 5), new Point(((x + 6) + width) + num2, y + 13),
                    new Point(((x + 12) + width) + num2, y + 9)
                };
            Point[] pointArray2 = pointArray3;
            var brush = new LinearGradientBrush(new Rectangle(x + 1, y + 1, width - 1, _headerHeight),
                                                Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
            var brush2 = new LinearGradientBrush(new Rectangle(x + 1, y + 1, width - 1, width - 1),
                                                 Color.FromArgb(0x77, 0xa5, 0xd6), Color.FromArgb(24, 0x4d, 0x94), 90f);
            _buttonLeftBounds = new Rectangle(x, y, width, width);
            _buttonRightBounds = new Rectangle((x + width) + num2, y, width, width);
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
            g.FillRectangle(_backgroundBrush, _drawingArea.Left, _drawingArea.Top + _headerHeight, _drawingArea.Width,
                            _drawingArea.Height - _headerHeight);
            int num = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
            var time = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            int num2 = num + (int) time.DayOfWeek;
            int num3 = ((num2%7) > 0) ? ((num2/7) + 1) : (num2/7);
            if (g.ClipBounds.Bottom > (_drawingArea.Top + _headerHeight))
            {
                int num6 = (_drawingArea.Top + MonthViewTopMargin) + _headerHeight;
                int num7 = num6 + (_monthViewRowSize*num3);
                int num4 = _drawingArea.Left + MonthViewLeftMargin;
                int num9 = 0;
                while (num9 <= 7)
                {
                    g.DrawLine(Pens.Black, num4, num6, num4, num7);
                    num9++;
                    num4 += _monthViewColSize;
                }
                num4 = _drawingArea.Left + MonthViewLeftMargin;
                int num5 = num4 + (_monthViewColSize*7);
                num6 = (_drawingArea.Top + MonthViewTopMargin) + _headerHeight;
                int num8 = 0;
                while (num8 <= num3)
                {
                    g.DrawLine(Pens.Black, num4, num6, num5, num6);
                    num8++;
                    num6 += _monthViewRowSize;
                }
                var dayOfWeek = (int) time.DayOfWeek;
                num6 = ((_drawingArea.Top + _headerHeight) + MonthViewTopMargin) + 5;
                DateTime startDateTime = time;
                for (int i = 1; i <= num; i++)
                {
                    num4 = (_drawingArea.Left + MonthViewLeftMargin) + (dayOfWeek*_monthViewColSize);
                    if (startDateTime == _currentDate)
                    {
                        brush =
                            new LinearGradientBrush(
                                new Rectangle(((_drawingArea.Left + MonthViewLeftMargin) + (dayOfWeek*_monthViewColSize)) + 4, num6 - 1, 20,
                                              _headerHeight), Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
                        g.FillRectangle(brush, ((_drawingArea.Left + MonthViewLeftMargin) + (dayOfWeek*_monthViewColSize)) + 4,
                                        num6 - 1, 20, 15);
                        g.DrawString(i.ToString(CultureInfo.InvariantCulture), _timeSmallFont, Brushes.White, (num4 + 5), num6);
                    }
                    else
                    {
                        g.DrawString(i.ToString(CultureInfo.InvariantCulture), _timeSmallFont, Brushes.Black, (num4 + 5), num6);
                    }
                    g.FillRectangle(new SolidBrush(_halfHourPen.Color), num4 + 1, ((num6 - 5) + _monthViewRowSize) - 10,
                                    _monthViewColSize - 2, 10);
                    var brush2 = new SolidBrush(_hourPen.Color);
                    foreach (Timer timer in ReturnApplicableTimersSubset(startDateTime, _oneDayTimeSpan, _applicableTimers))
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
                        num6 += _monthViewRowSize;
                    }
                }
            }
            if (g.ClipBounds.Top < (_drawingArea.Top + _headerHeight))
            {
                brush =
                    new LinearGradientBrush(new Rectangle(_drawingArea.Left, _drawingArea.Top, _drawingArea.Width, _headerHeight),
                                            Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
                g.FillRectangle(brush, _drawingArea.Left, _drawingArea.Top, _drawingArea.Width, _headerHeight);
                DrawHeaderButtons(g, _inLeftButtonBounds, _inRightButtonBounds);
                g.DrawString(_currentDate.ToString("Y"), _dayViewHeaderFont, Brushes.White, (_drawingArea.Left + 10),
                             (_drawingArea.Top + 5));
            }
        }

        private void DrawRoundRect(Graphics g, Pen p, float x, float y, float width, float height, float radius)
        {
            var path = new GraphicsPath();
            path.AddLine(x + radius, y, (x + width) - (radius*2f), y);
            path.AddArc((x + width) - (radius*2f), y, radius*2f, radius*2f, 270f, 90f);
            path.AddLine((x + width), (y + radius), (x + width), ((y + height) - (radius*2f)));
            path.AddArc(((x + width) - (radius*2f)), ((y + height) - (radius*2f)), (radius*2f), (radius*2f), 0f, 90f);
            path.AddLine(((x + width) - (radius*2f)), (y + height), (x + radius), (y + height));
            path.AddArc(x, (y + height) - (radius*2f), radius*2f, radius*2f, 90f, 90f);
            path.AddLine(x, (y + height) - (radius*2f), x, y + radius);
            path.AddArc(x, y, radius*2f, radius*2f, 180f, 90f);
            path.CloseFigure();
            g.DrawPath(p, path);
            path.Dispose();
        }

        private void DrawTimerBlocks(IEnumerable<Timer> timers, Graphics g)
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
                        g.DrawString(str, _timeSmallFont, Brushes.Black, layoutRectangle);
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
            g.FillRectangle(_backgroundBrush, (_drawingArea.Left + TimeGutter), (_drawingArea.Top + _headerHeight),
                            (_drawingArea.Width - TimeGutter), (_drawingArea.Height - _headerHeight));
            g.DrawLine(_timeLinePen, _drawingArea.Left, _drawingArea.Top + _headerHeight,
                       (_drawingArea.Left + TimeGutter) - 5, _drawingArea.Top + _headerHeight);
            for (
                num = (_drawingArea.Top + _headerHeight) + (((vScrollBar.Value%2) == 0) ? (HalfHourHeight*2) : HalfHourHeight);
                num < _drawingArea.Height;
                num += HalfHourHeight*2)
            {
                g.DrawLine(_hourPen, _drawingArea.Left + TimeGutter, num, _drawingArea.Width, num);
                g.DrawLine(_timeLinePen, _drawingArea.Left + 5, num, (_drawingArea.Left + TimeGutter) - 5, num);
            }
            for (
                num = (_drawingArea.Top + _headerHeight) + (((vScrollBar.Value%2) == 0) ? HalfHourHeight : (HalfHourHeight*2));
                num < _drawingArea.Height;
                num += HalfHourHeight*2)
            {
                g.DrawLine(_halfHourPen, _drawingArea.Left + TimeGutter, num, _drawingArea.Width, num);
            }
            int num2 = ((vScrollBar.Value%2) == 1) ? ((vScrollBar.Value/2) + 1) : (vScrollBar.Value/2);
            const int num3 = TimeGutter >> 1;
            int num4 = -1;
            for (num = ((_drawingArea.Top + 3) + _headerHeight) + (((vScrollBar.Value%2) == 0) ? 0 : HalfHourHeight);
                 (num < _drawingArea.Height) && (num2 < 24);
                 num += HalfHourHeight*2)
            {
                string str;
                string str2;
                if (num2 == 0)
                {
                    str = "12";
                }
                else if (num2 > 12)
                {
                    str = (num2 - 12).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    str = num2.ToString(CultureInfo.InvariantCulture);
                }
                g.DrawString(str, _timeLargeFont, Brushes.Black,
                             (((_drawingArea.Left + num3) - ((int) g.MeasureString(str, _timeLargeFont).Width)) + 6), num);
                if (num4 != (num2/12))
                {
                    num4 = num2/12;
                    str2 = (num4 == 0) ? "am" : "pm";
                }
                else
                {
                    str2 = "00";
                }
                g.DrawString(str2, _timeSmallFont, Brushes.Black, ((_drawingArea.Left + num3) + 2), num);
                num2++;
            }
        }

        private void DrawWeekView(Graphics g)
        {
            float num2;
            int num3;
            g.FillRectangle(_backgroundBrush, _drawingArea.Left + TimeGutter, _drawingArea.Top + _headerHeight,
                            _drawingArea.Width - TimeGutter, 20);
            _headerHeight = 50;
            DrawTimes(g);
            _headerHeight = 30;
            float num = ((_drawingArea.Width - TimeGutter))/7f;
            for (num3 = 1; num3 < 7; num3++)
            {
                num2 = (_drawingArea.Left + (num*num3)) + TimeGutter;
                g.DrawLine(Pens.Black, num2, (_drawingArea.Top + _headerHeight), num2, _drawingArea.Height);
            }
            var brush =
                new LinearGradientBrush(
                    new Rectangle(_drawingArea.Left, _drawingArea.Top, _drawingArea.Width - TimeGutter, _headerHeight),
                    Color.FromArgb(0x59, 0x87, 0xd6), Color.FromArgb(4, 0x39, 0x94), 90f);
            var brush2 = new SolidBrush(_halfHourPen.Color);
            var dayOfWeek = (int) _currentDate.DayOfWeek;
            for (num3 = 0; num3 < 7; num3++)
            {
                num2 = (_drawingArea.Left + (num*num3)) + TimeGutter;
                if (dayOfWeek == num3)
                {
                    g.FillRectangle(brush, num2 + 2f, ((_drawingArea.Top + _headerHeight) + 2), num - 5f, 17f);
                    g.DrawRectangle(Pens.Navy, num2 + 2f, ((_drawingArea.Top + _headerHeight) + 2), num - 5f, 17f);
                    g.DrawString(_days[num3], _timeSmallFont, Brushes.White,
                                 num2 + ((num - g.MeasureString(_days[num3], _timeSmallFont).Width)/2f),
                                 ((_drawingArea.Top + _headerHeight) + 4));
                }
                else
                {
                    g.FillRectangle(brush2, num2 + 2f, ((_drawingArea.Top + _headerHeight) + 2), num - 5f, 17f);
                    g.DrawRectangle(_hourPen, num2 + 2f, ((_drawingArea.Top + _headerHeight) + 2), num - 5f, 17f);
                    g.DrawString(_days[num3], _timeSmallFont, Brushes.Black,
                                 num2 + ((num - g.MeasureString(_days[num3], _timeSmallFont).Width)/2f),
                                 ((_drawingArea.Top + _headerHeight) + 4));
                }
            }
            g.FillRectangle(brush, _drawingArea.Left + TimeGutter, _drawingArea.Top, _drawingArea.Width - TimeGutter,
                            _headerHeight);
            DrawHeaderButtons(g, _inLeftButtonBounds, _inRightButtonBounds);
            DateTime time = _currentDate.AddDays(-(double) _currentDate.DayOfWeek);
            DateTime time2 = time.AddDays(6.0);
            g.DrawString(string.Format("{0}   -   {1}", time.ToString("m"), time2.ToString("m")), _dayViewHeaderFont,
                         Brushes.White, ((_drawingArea.Left + TimeGutter) + 10), (_drawingArea.Top + 5));
            DrawTimerBlocks(_applicableTimers, g);
        }

        private void EditOrAddTimerAt(Point point, int viewRelativeY, DateTime viewStartDate, TimeSpan viewLengthSpan)
        {
            Timer timer = FindApplicableTimerAt(point);
            int hour = (vScrollBar.Value + (viewRelativeY/HalfHourHeight))/2;
            int minute = ((vScrollBar.Value + (viewRelativeY/HalfHourHeight))%2)*30;
            if (hour < 24)
            {
                TimerDialog dialog = timer == null
                                         ? new TimerDialog(new DateTime(_currentDate.Year, _currentDate.Month, _currentDate.Day, hour,
                                                                        minute, 0))
                                         : new TimerDialog(timer);
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
            foreach (Timer timer in _applicableTimers)
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
            bool inLeftButtonBounds = _inLeftButtonBounds;
            bool inRightButtonBounds = _inRightButtonBounds;
            _inLeftButtonBounds = _buttonLeftBounds.Contains(location);
            _inRightButtonBounds = _buttonRightBounds.Contains(location);
            if ((inLeftButtonBounds != _inLeftButtonBounds) || (inRightButtonBounds != _inRightButtonBounds))
            {
                Refresh();
            }
        }


        private void MonthViewCalcs()
        {
            foreach (Timer timer in _applicableTimers)
            {
                timer.DisplayBounds.Clear();
            }
            int num = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
            var time = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            int num2 = num + (int) time.DayOfWeek;
            int num3 = ((num2%7) > 0) ? ((num2/7) + 1) : (num2/7);
            _monthViewRowSize = (((_drawingArea.Height - _headerHeight) - MonthViewTopMargin) - MonthViewBottomMargin)/
                                num3;
            _monthViewColSize = ((_drawingArea.Width - MonthViewLeftMargin) - MonthViewRightMargin)/7;
            var dayOfWeek = (int) time.DayOfWeek;
            int num5 = ((_drawingArea.Top + _headerHeight) + MonthViewTopMargin) + 5;
            DateTime startDateTime = time;
            float num8 = (_monthViewColSize)/48f;
            for (int i = 1; i <= num; i++)
            {
                int num4 = (_drawingArea.Left + MonthViewLeftMargin) + (dayOfWeek*_monthViewColSize);
                float num9 = ((num5 - 5) + _monthViewRowSize) - MonthBarHeight;
                foreach (Timer timer in ReturnApplicableTimersSubset(startDateTime, _oneDayTimeSpan, _applicableTimers))
                {
                    var item = new ReferenceRectF
                        {
                            X = (num4 + 1) + ((timer.StartTime.Hours*2)*num8),
                            Y = num9,
                            Width = (float) ((timer.TimerLength.TotalHours*2.0)*num8),
                            Height = MonthBarHeight
                        };
                    timer.DisplayBounds.Add(item);
                }
                startDateTime = startDateTime.AddDays(1.0);
                if (++dayOfWeek == 7)
                {
                    dayOfWeek = 0;
                    num5 += _monthViewRowSize;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            switch (_currentView)
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

        private List<Timer> ReturnApplicableTimersSubset(DateTime startDateTime, TimeSpan span, IEnumerable<Timer> seedList)
        {
            var list = new List<Timer>();
            DateTime time = startDateTime + span;
            foreach (Timer timer in seedList)
            {
                int num;
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
                DateTime time4 = startDateTime;
                for (int i = 0; i < span.TotalDays; i++)
                {
                    num |= (1) << (int) time4.DayOfWeek;
                    time4 = time4.AddDays(1.0);
                }
                Label_01C2:
                var num3 = (int) timer.RecurrenceData;
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
            if (e.Y >= (_drawingArea.Top + _headerHeight))
            {
                switch (_currentView)
                {
                    case DateView.Day:
                        if (e.Y >= (_drawingArea.Top + _headerHeight))
                        {
                            if (e.X >= (_drawingArea.Left + TimeGutter))
                            {
                                EditOrAddTimerAt(e.Location, (e.Y - _headerHeight) - _drawingArea.Top, _currentDate, _oneDayTimeSpan);
                                Refresh();
                            }
                        }
                        break;

                    case DateView.Week:
                        if (e.Y >= (_drawingArea.Top + _headerHeight))
                        {
                            if (e.X >= (_drawingArea.Left + TimeGutter))
                            {
                                DateTime viewStartDate = _currentDate.AddDays(-(double) _currentDate.DayOfWeek);
                                _currentDate =
                                    viewStartDate.AddDays(((int) (((e.X - TimeGutter))/(((_drawingArea.Width - TimeGutter))/7f))));
                                EditOrAddTimerAt(e.Location, ((e.Y - _headerHeight) - HalfHourHeight) - _drawingArea.Top, viewStartDate,
                                                 _oneWeekTimeSpan);
                                Refresh();
                            }
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
                        if (e.Y >= (_drawingArea.Top + _headerHeight))
                        {
                            if (((((e.Y - _drawingArea.Top) - _headerHeight)/AgendaItemHeight) + vScrollBar.Value) <
                                _applicableTimers.Count)
                            {
                                EditOrAddTimerAt(e.Location, (e.Y - _headerHeight) - _drawingArea.Top, _currentDate, _oneDayTimeSpan);
                            }
                            Refresh();
                        }
                        break;
                }
            }
        }

        private void ScheduleDialog_MouseDown(object sender, MouseEventArgs e)
        {
            switch (_currentView)
            {
                case DateView.Day:
                case DateView.Agenda:
                    if (!_inLeftButtonBounds)
                    {
                        if (_inRightButtonBounds)
                        {
                            _currentDate = _currentDate.AddDays(1.0);
                            CompileApplicableTimers(_currentDate, _oneDayTimeSpan);
                            Refresh();
                        }
                        break;
                    }
                    _currentDate = _currentDate.AddDays(-1.0);
                    CompileApplicableTimers(_currentDate, _oneDayTimeSpan);
                    Refresh();
                    break;

                case DateView.Week:
                    if (!_inLeftButtonBounds)
                    {
                        if (_inRightButtonBounds)
                        {
                            _currentDate = _currentDate.AddDays(7.0);
                            CompileApplicableTimers(_currentDate.AddDays(-(double) _currentDate.DayOfWeek), _oneWeekTimeSpan);
                            Refresh();
                        }
                        break;
                    }
                    _currentDate = _currentDate.AddDays(-7.0);
                    CompileApplicableTimers(_currentDate.AddDays(-(double) _currentDate.DayOfWeek), _oneWeekTimeSpan);
                    Refresh();
                    break;

                case DateView.Month:
                    if (!_inLeftButtonBounds)
                    {
                        if (_inRightButtonBounds)
                        {
                            _currentDate = _currentDate.AddMonths(1);
                            Refresh();
                        }
                        break;
                    }
                    _currentDate = _currentDate.AddMonths(-1);
                    Refresh();
                    break;
            }
        }

        private void ScheduleDialog_MouseMove(object sender, MouseEventArgs e)
        {
            DateTime minValue;
            switch (_currentView)
            {
                case DateView.Day:
                    SetToolTip(this, e.Location);
                    break;

                case DateView.Week:
                    {
                        float num = ((_drawingArea.Width - TimeGutter))/7f;
                        minValue = _currentDate.AddDays(-(double) (_currentDate.DayOfWeek + ((int) (((e.X - TimeGutter))/num))));
                        if (!SetToolTip(this, e.Location, ReturnApplicableTimersSubset(minValue, _oneDayTimeSpan, _applicableTimers)))
                        {
                            break;
                        }
                        return;
                    }
                case DateView.Month:
                    minValue = DateTime.MinValue;
                    if (!TryGetMonthDateAt(e.Location, ref minValue) ||
                        !SetToolTip(this, e.Location, ReturnApplicableTimersSubset(minValue, _oneDayTimeSpan, _applicableTimers)))
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
            if (!(_isResizing || (WindowState == FormWindowState.Minimized)))
            {
                VScrollCheck();
                ViewCalcs();
                Refresh();
            }
        }

        private void ScheduleDialog_ResizeBegin(object sender, EventArgs e)
        {
            _isResizing = true;
        }

        private void ScheduleDialog_ResizeEnd(object sender, EventArgs e)
        {
            _isResizing = false;
            VScrollCheck();
            ViewCalcs();
            Refresh();
        }

        private void SetCurrentView(DateView view)
        {
            SetCurrentView(view, _currentDate);
        }

        private void SetCurrentView(DateView view, DateTime date)
        {
            switch (_currentView)
            {
                case DateView.Day:
                    _dayViewScrollBarValue = vScrollBar.Value;
                    toolStripButtonDayView.Checked = false;
                    break;

                case DateView.Week:
                    _weekViewScrollBarValue = vScrollBar.Value;
                    toolStripButtonWeekView.Checked = false;
                    break;

                case DateView.Month:
                    toolStripButtonMonthView.Checked = false;
                    break;

                case DateView.Agenda:
                    _agendaViewScrollBarValue = vScrollBar.Value;
                    toolStripButtonAgendaView.Checked = false;
                    break;
            }
            _currentDate = date;
            _currentView = view;
            VScrollCheck();
            switch (_currentView)
            {
                case DateView.Day:
                    CompileApplicableTimers(date, _oneDayTimeSpan);
                    toolStripButtonDayView.Checked = true;
                    break;

                case DateView.Week:
                    CompileApplicableTimers(_currentDate.AddDays(-(double) _currentDate.DayOfWeek), _oneWeekTimeSpan);
                    toolStripButtonWeekView.Checked = true;
                    break;

                case DateView.Month:
                    CompileApplicableTimers(new DateTime(_currentDate.Year, _currentDate.Month, 1),
                                            new TimeSpan(DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month), 0, 0, 0));
                    toolStripButtonMonthView.Checked = true;
                    break;

                case DateView.Agenda:
                    CompileApplicableTimers(date, _oneDayTimeSpan);
                    toolStripButtonAgendaView.Checked = true;
                    break;
            }
            Refresh();
        }

        private void SetDrawingArea()
        {
            _drawingArea.X = 0;
            _drawingArea.Y = toolStrip1.Bottom;
            _drawingArea.Width = vScrollBar.Visible ? vScrollBar.Left : vScrollBar.Right;
            _drawingArea.Height = panel1.Top;
        }

        private void SetToolTip(Control control, Point location)
        {
            SetToolTip(control, location, _applicableTimers);
        }

        private bool SetToolTip(Control control, Point location, IEnumerable<Timer> timers)
        {
            string tip = toolTip.GetToolTip(control);
            foreach (Timer timer in timers)
            {
                foreach (ReferenceRectF tf in timer.DisplayBounds)
                {
                    if (tf.Contains(location))
                    {
                        string caption = timer.ToString().Replace("|", "\r\n");
                        if (caption != tip)
                        {
                            toolTip.SetToolTip(control, caption);
                        }
                        return true;
                    }
                }
            }
            toolTip.SetToolTip(control, string.Empty);
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
            switch (_currentView)
            {
                case DateView.Day:
                case DateView.Agenda:
                    EditOrAddTimerAt(_mouseDownAt, (_mouseDownAt.Y - _drawingArea.Y) - _headerHeight, _currentDate,
                                     _oneDayTimeSpan);
                    break;

                case DateView.Week:
                    {
                        DateTime viewStartDate = _currentDate.AddDays(-(double) _currentDate.DayOfWeek);
                        EditOrAddTimerAt(_mouseDownAt, ((_mouseDownAt.Y - _drawingArea.Y) - _headerHeight) - HalfHourHeight,
                                         viewStartDate, _oneWeekTimeSpan);
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
                Timer item = FindApplicableTimerAt(_mouseDownAt);
                _timers.Remove(item);
                switch (_currentView)
                {
                    case DateView.Day:
                    case DateView.Agenda:
                        CompileApplicableTimers(_currentDate, _oneDayTimeSpan);
                        Refresh();
                        break;

                    case DateView.Week:
                        {
                            DateTime startDateTime = _currentDate.AddDays(-(double) _currentDate.DayOfWeek);
                            CompileApplicableTimers(startDateTime, _oneWeekTimeSpan);
                            Refresh();
                            break;
                        }
                }
            }
        }

        private bool TryGetMonthDateAt(Point p, ref DateTime date)
        {
            if (p.Y < ((_drawingArea.Top + _headerHeight) + MonthViewTopMargin))
            {
                return false;
            }
            if (p.Y > (_drawingArea.Bottom - MonthViewBottomMargin))
            {
                return false;
            }
            if (p.X < (_drawingArea.Left + MonthViewLeftMargin))
            {
                return false;
            }
            if (p.X > (_drawingArea.Right - MonthViewRightMargin))
            {
                return false;
            }
            int num = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
            var time = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            int num2 = num + (int) time.DayOfWeek;
            int num3 = ((num2%7) > 0) ? ((num2/7) + 1) : (num2/7);
            int num4 = ((((_drawingArea.Height - _headerHeight) - MonthViewTopMargin) - MonthViewBottomMargin)/num3) + 1;
            int num5 = (((_drawingArea.Width - MonthViewLeftMargin) - MonthViewRightMargin)/7) + 1;
            int num6 = ((p.X - _drawingArea.Left) - MonthViewLeftMargin)/num5;
            int num7 = (((p.Y - _drawingArea.Top) - _headerHeight) - MonthViewTopMargin)/num4;
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
            int y = toolStrip1.Bottom + _headerHeight;
            switch (_currentView)
            {
                case DateView.Day:
                    Invalidate(new Rectangle(0, y, _drawingArea.Width, ClientRectangle.Height - y));
                    break;

                case DateView.Week:
                    Invalidate(new Rectangle(0, y + HalfHourHeight, _drawingArea.Width,
                                             (ClientRectangle.Height - y) - HalfHourHeight));
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
            switch (_currentView)
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
            switch (_currentView)
            {
                case DateView.Day:
                    vScrollBar.Maximum = 0x30;
                    vScrollBar.LargeChange = (_drawingArea.Height - _headerHeight)/HalfHourHeight;
                    vScrollBar.Value = _dayViewScrollBarValue;
                    vScrollBar.Enabled = true;
                    vScrollBar.Visible = true;
                    break;

                case DateView.Week:
                    vScrollBar.Maximum = 0x30;
                    vScrollBar.LargeChange = ((_drawingArea.Height - _headerHeight) - 20)/HalfHourHeight;
                    vScrollBar.Value = _weekViewScrollBarValue;
                    vScrollBar.Enabled = true;
                    vScrollBar.Visible = true;
                    break;

                case DateView.Month:
                    vScrollBar.Visible = false;
                    break;

                case DateView.Agenda:
                    {
                        int num = (_drawingArea.Height - _headerHeight)/AgendaItemHeight;
                        if (num <= _applicableTimers.Count)
                        {
                            vScrollBar.Maximum = _applicableTimers.Count;
                            vScrollBar.LargeChange = num;
                            vScrollBar.Value = _agendaViewScrollBarValue <= vScrollBar.Maximum
                                                   ? _agendaViewScrollBarValue
                                                   : vScrollBar.Maximum;
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
            foreach (Timer timer in _applicableTimers)
            {
                timer.DisplayBounds.Clear();
            }
            DateTime startDateTime = _currentDate.AddDays(-(double) _currentDate.DayOfWeek);
            float num = ((_drawingArea.Width - TimeGutter))/7f;
            int num2 = 0;
            while (num2 < 7)
            {
                List<Timer> applicableTimers = ReturnApplicableTimersSubset(startDateTime, _oneDayTimeSpan, _applicableTimers);
                CalcDayBlocksIn(applicableTimers,
                                new Rectangle((int) (((_drawingArea.Left + TimeGutter) + (num*num2)) + 2f),
                                              (_drawingArea.Top + _headerHeight) + HalfHourHeight, ((int) num) - 2,
                                              ((_drawingArea.Height - _headerHeight) - HalfHourHeight) - 1),
                                new TimeSpan(vScrollBar.Value/2, (vScrollBar.Value%2)*30, 0));
                num2++;
                startDateTime += _oneDayTimeSpan;
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