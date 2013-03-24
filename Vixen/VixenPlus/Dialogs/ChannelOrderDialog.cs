using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Vixen.Dialogs
{
	public partial class ChannelOrderDialog : Form
	{
		private const int m_lineGutter = 10;
		private const int m_lineHeight = 40;
		private const int m_sideGutter = 10;
		private List<Channel> m_channelMapping;
		private List<Channel> m_channelNaturalOrder;
		private bool m_controlDown;
		private bool m_initializing;
		private int m_insertIndex;
		private int m_insertionIndex;
		private bool m_mouseDown;
		private int m_selectedIndex;
		private bool m_showNaturalNumber;

		public ChannelOrderDialog(List<Channel> channelList, List<int> channelOrder)
		{
			m_mouseDown = false;
			m_selectedIndex = -1;
			m_insertIndex = -1;
			m_initializing = true;
			m_insertionIndex = -1;
			m_controlDown = false;
			components = null;
			InitializeComponent();
			Construct(channelList, channelOrder);
		}

		public ChannelOrderDialog(List<Channel> channelList, List<int> channelOrder, string caption)
		{
			m_mouseDown = false;
			m_selectedIndex = -1;
			m_insertIndex = -1;
			m_initializing = true;
			m_insertionIndex = -1;
			m_controlDown = false;
			components = null;
			InitializeComponent();
			Construct(channelList, channelOrder);
			Text = caption;
		}

		public List<Channel> ChannelMapping
		{
			get { return m_channelMapping; }
		}

		private void CalcScrollParams()
		{
			if (!m_initializing)
			{
				var num = (int) Math.Round((((pictureBoxChannels.Height - 10))/40f), MidpointRounding.AwayFromZero);
				vScrollBar.Maximum = m_channelMapping.Count - 1;
				vScrollBar.LargeChange = num;
				if ((vScrollBar.LargeChange + vScrollBar.Value) > vScrollBar.Maximum)
				{
					vScrollBar.Value = (vScrollBar.Maximum - vScrollBar.LargeChange) + 1;
				}
			}
		}

		private void ChannelOrderDialog_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			var dialog =
				new HelpDialog(
					"Drag channels into their new positions, or\n\nDouble-click at an insertion point.  Then Ctrl+Click on channels to move to that point.\nThe insertion point will automatically move with each channel inserted.");
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void ChannelOrderDialog_KeyDown(object sender, KeyEventArgs e)
		{
			m_controlDown = e.Control;
		}

		private void ChannelOrderDialog_KeyUp(object sender, KeyEventArgs e)
		{
			m_controlDown = e.Control;
		}

		private void Construct(List<Channel> channelList, List<int> channelOrder)
		{
			m_initializing = false;
			m_channelNaturalOrder = new List<Channel>();
			m_channelNaturalOrder.AddRange(channelList);
			m_channelMapping = new List<Channel>();
			if (channelOrder == null)
			{
				m_channelMapping.AddRange(channelList);
			}
			else
			{
				foreach (int num in channelOrder)
				{
					m_channelMapping.Add(channelList[num]);
				}
			}
			m_showNaturalNumber =
				((ISystem) Interfaces.Available["ISystem"]).UserPreferences.GetBoolean("ShowNaturalChannelNumber");
			CalcScrollParams();
		}


		private void pictureBoxChannels_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			m_mouseDown = false;
			if (!m_controlDown)
			{
				m_insertionIndex = vScrollBar.Value + (e.Y/40);
				pictureBoxChannels.Refresh();
			}
		}

		private void pictureBoxChannels_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_controlDown)
			{
				if (m_insertionIndex != -1)
				{
					int index = vScrollBar.Value + (e.Y/40);
					if (m_insertionIndex != index)
					{
						if (m_insertionIndex > index)
						{
							m_insertionIndex--;
						}
						Channel item = m_channelMapping[index];
						m_channelMapping.RemoveAt(index);
						m_channelMapping.Insert(m_insertionIndex, item);
						if (m_insertionIndex < m_channelMapping.Count)
						{
							m_insertionIndex++;
						}
						if (m_insertionIndex < index)
						{
							if (vScrollBar.Value <= (vScrollBar.Maximum - vScrollBar.LargeChange))
							{
								vScrollBar.Value++;
							}
							else
							{
								pictureBoxChannels.Refresh();
							}
						}
						else
						{
							pictureBoxChannels.Refresh();
						}
					}
				}
			}
			else
			{
				m_mouseDown = true;
				m_selectedIndex = vScrollBar.Value + (e.Y/40);
			}
		}

		private void pictureBoxChannels_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_mouseDown && ((e.Y >= 0) && (e.Y <= pictureBoxChannels.Height)))
			{
				if (e.Y < 10)
				{
					ScrollUp();
				}
				else if (e.Y > (pictureBoxChannels.Height - 10))
				{
					ScrollDown();
				}
				else
				{
					int num = vScrollBar.Value + (e.Y/40);
					if (num == m_insertIndex)
					{
						return;
					}
					if (m_selectedIndex != num)
					{
						m_insertIndex = num;
					}
					else
					{
						m_insertIndex = -1;
					}
				}
				pictureBoxChannels.Refresh();
			}
		}

		private void pictureBoxChannels_MouseUp(object sender, MouseEventArgs e)
		{
			if (m_insertIndex != -1)
			{
				if (m_insertIndex != m_selectedIndex)
				{
					Channel item = m_channelMapping[m_selectedIndex];
					m_channelMapping.RemoveAt(m_selectedIndex);
					if (m_selectedIndex > m_insertIndex)
					{
						m_channelMapping.Insert(m_insertIndex, item);
					}
					else
					{
						m_channelMapping.Insert(m_insertIndex - 1, item);
					}
				}
				m_mouseDown = false;
				m_selectedIndex = -1;
				m_insertIndex = -1;
				pictureBoxChannels.Refresh();
			}
		}

		private void pictureBoxChannels_Paint(object sender, PaintEventArgs e)
		{
			if (m_channelMapping.Count != 0)
			{
				int num2;
				var pen = new Pen(Color.Black, 2f);
				var brush = new SolidBrush(Color.White);
				var rect = new Rectangle();
				var font = new Font("Arial", 14f, FontStyle.Bold);
				var brush2 = new SolidBrush(Color.Black);
				rect.Width = pictureBoxChannels.Width - 20;
				rect.X = 10;
				rect.Height = 30;
				rect.Y = 10;
				for (int i = 0; i < vScrollBar.LargeChange; i++)
				{
					Channel item = m_channelMapping[i + vScrollBar.Value];
					if (item.Color.ToArgb() != -1)
					{
						pen.Color = item.Color;
						brush.Color = Color.FromArgb(0x40, item.Color);
					}
					else
					{
						pen.Color = Color.Black;
						brush.Color = Color.White;
					}
					brush2.Color = pen.Color;
					e.Graphics.FillRectangle(brush, rect);
					e.Graphics.DrawRectangle(pen, rect);
					if (m_showNaturalNumber)
					{
						e.Graphics.DrawString(string.Format("{0}: {1}", m_channelNaturalOrder.IndexOf(item) + 1, item.Name), font,
						                      Brushes.Black, 15f, (rect.Top + 5));
					}
					else
					{
						e.Graphics.DrawString(item.Name, font, Brushes.Black, 15f, (rect.Top + 5));
					}
					rect.Y += 40;
				}
				if (m_insertIndex != -1)
				{
					num2 = (m_insertIndex - vScrollBar.Value)*40;
					int num3 = 5;
					var points = new[] {new Point(5, (num2 - 5) + num3), new Point(10, num2 + num3), new Point(5, (num2 + 5) + num3)};
					e.Graphics.DrawPolygon(Pens.Black, points);
				}
				if (m_insertionIndex != -1)
				{
					num2 = ((m_insertionIndex - vScrollBar.Value)*40) + 2;
					e.Graphics.FillRectangle(Brushes.Gray, 0, num2, pictureBoxChannels.Width, 6);
				}
				font.Dispose();
				brush.Dispose();
				brush2.Dispose();
				pen.Dispose();
			}
		}

		private void pictureBoxChannels_Resize(object sender, EventArgs e)
		{
			RecalcAndRedraw();
		}

		private void RecalcAndRedraw()
		{
			CalcScrollParams();
			pictureBoxChannels.Refresh();
		}

		private void ScrollDown()
		{
			int y = pictureBoxChannels.PointToScreen(new Point(pictureBoxChannels.Left, pictureBoxChannels.Bottom - 10)).Y;
			while (MousePosition.Y > y)
			{
				if (vScrollBar.Value > (vScrollBar.Maximum - vScrollBar.LargeChange))
				{
					break;
				}
				m_insertIndex++;
				vScrollBar.Value++;
			}
		}

		private void ScrollUp()
		{
			int y = pictureBoxChannels.PointToScreen(new Point(pictureBoxChannels.Left, pictureBoxChannels.Top + 10)).Y;
			while (MousePosition.Y < y)
			{
				if (vScrollBar.Value == 0)
				{
					break;
				}
				m_insertIndex--;
				vScrollBar.Value--;
				pictureBoxChannels.Refresh();
			}
		}

		private void vScrollBar_ValueChanged(object sender, EventArgs e)
		{
			pictureBoxChannels.Refresh();
		}
	}
}