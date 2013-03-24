using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Vixen
{
	internal partial class DimmingCurveDialog : Form
	{
		private const int EXPORT_TO_FILE = 1;
		private const int EXPORT_TO_LIBRARY = 0;
		private const int IMPORT_FROM_FILE = 1;
		private const int IMPORT_FROM_LIBRARY = 0;
		private readonly SolidBrush m_curveBackBrush;
		private readonly float m_curveColPointsPerMiniPixel;
		private readonly Color m_curveGridColor;
		private readonly Pen m_curveGridPen;
		private readonly Color m_curveLineColor;
		private readonly Pen m_curveLinePen;
		private readonly SolidBrush m_curvePointBrush;
		private readonly Color m_curvePointColor;
		private readonly float m_curveRowPointsPerMiniPixel;
		private readonly int m_dotPitch;
		private readonly int m_gridSpacing;
		private readonly float m_halfPointSize;
		private readonly SolidBrush m_miniBackBrush;
		private readonly Color m_miniBoxColor;
		private readonly Pen m_miniBoxPen;
		private readonly Color m_miniLineColor;
		private readonly Pen m_miniLinePen;
		private readonly Channel m_originalChannel;
		private readonly int m_pointSize;
		private readonly EventSequence m_sequence;
		private float m_availableValues;
		private int[,] m_curvePoints;
		private Rectangle m_miniBoxBounds;
		private Point m_miniMouseDownLast;
		private Point m_miniMouseMaxLocation;
		private Point m_miniMouseMinLocation;
		private byte[] m_points;
		private int m_selectedPointAbsolute;
		private int m_selectedPointRelative;
		private int m_startCurvePoint;
		private bool m_usingActualLevels;

		public DimmingCurveDialog(EventSequence sequence, Channel selectChannel)
		{
			Action<Channel> action = null;
			m_miniBoxColor = Color.BlueViolet;
			m_miniLineColor = Color.Blue;
			m_curveGridColor = Color.LightGray;
			m_curveLineColor = Color.Blue;
			m_curvePointColor = Color.Black;
			m_pointSize = 4;
			m_dotPitch = 4;
			m_miniMouseDownLast = new Point(-1, -1);
			m_miniMouseMinLocation = new Point(0, 0);
			m_miniMouseMaxLocation = new Point(0, 0);
			m_selectedPointAbsolute = -1;
			m_selectedPointRelative = -1;
			m_usingActualLevels = true;
			m_availableValues = 256f;
			components = null;
			InitializeComponent();
			if (sequence != null)
			{
				if (action == null)
				{
					action = delegate(Channel c) { comboBoxChannels.Items.Add(c.Clone()); };
				}
				sequence.Channels.ForEach(action);
				m_sequence = sequence;
			}
			else
			{
				labelSequenceChannels.Enabled = false;
				comboBoxChannels.Enabled = false;
				if (selectChannel != null)
				{
					m_originalChannel = selectChannel;
					comboBoxChannels.Items.Add(selectChannel = selectChannel.Clone());
				}
			}
			m_gridSpacing = m_pointSize + m_dotPitch;
			m_halfPointSize = (m_pointSize)/2f;
			m_curveRowPointsPerMiniPixel = m_availableValues/(pbMini.Width);
			m_curveColPointsPerMiniPixel = m_availableValues/(pbMini.Height);
			m_miniBoxBounds = new Rectangle(0, 0,
			                                (int) ((((pictureBoxCurve.Width/m_gridSpacing))/m_availableValues)*pbMini.Width),
			                                (int) ((((pictureBoxCurve.Height/m_gridSpacing))/m_availableValues)*pbMini.Height));
			m_miniBackBrush = new SolidBrush(pbMini.BackColor);
			m_curveBackBrush = new SolidBrush(pictureBoxCurve.BackColor);
			m_miniBoxPen = new Pen(m_miniBoxColor);
			m_miniLinePen = new Pen(m_miniLineColor);
			m_curveGridPen = new Pen(m_curveGridColor);
			m_curveLinePen = new Pen(m_curveLineColor);
			m_curvePointBrush = new SolidBrush(m_curvePointColor);
			if (comboBoxChannels.Items.Count > 0)
			{
				comboBoxChannels.SelectedItem = (selectChannel != null) ? selectChannel : comboBoxChannels.Items[0];
			}
			SwitchDisplay(Preference2.GetInstance().GetBoolean("ActualLevels"));
			comboBoxImport.SelectedIndex = 0;
			comboBoxExport.SelectedIndex = 0;
		}

		private void buttonExportToLibrary_Click(object sender, EventArgs e)
		{
			if (comboBoxExport.SelectedIndex == 0)
			{
				using (var library = new CurveLibrary())
				{
					var dialog = new CurveLibraryRecordEditDialog(null);
					if (dialog.ShowDialog() == DialogResult.OK)
					{
						CurveLibraryRecord libraryRecord = dialog.LibraryRecord;
						libraryRecord.CurveData = m_points;
						library.Import(libraryRecord);
						library.Save();
					}
					dialog.Dispose();
				}
			}
			else
			{
				var dialog2 = new CurveFileImportExportDialog(CurveFileImportExportDialog.ImportExport.Export);
				dialog2.ShowDialog();
				dialog2.Dispose();
			}
		}

		private void buttonImportFromLibrary_Click(object sender, EventArgs e)
		{
			if (comboBoxImport.SelectedIndex == 0)
			{
				var dialog = new CurveLibraryDialog();
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					(comboBoxChannels.SelectedItem as Channel).DimmingCurve = m_points = dialog.SelectedCurve;
					RedrawBoth();
				}
				dialog.Dispose();
			}
			else
			{
				var dialog2 = new CurveFileImportExportDialog(CurveFileImportExportDialog.ImportExport.Import);
				if (dialog2.ShowDialog() == DialogResult.OK)
				{
					CurveLibraryRecord selectedCurve = dialog2.SelectedCurve;
					if (selectedCurve != null)
					{
						(comboBoxChannels.SelectedItem as Channel).DimmingCurve = m_points = selectedCurve.CurveData;
						RedrawBoth();
					}
				}
				dialog2.Dispose();
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (m_sequence != null)
			{
				for (int i = 0; i < comboBoxChannels.Items.Count; i++)
				{
					m_sequence.Channels[i].DimmingCurve = (comboBoxChannels.Items[i] as Channel).DimmingCurve;
				}
			}
			else
			{
				var channel = (Channel) comboBoxChannels.Items[0];
				m_originalChannel.DimmingCurve = channel.DimmingCurve;
			}
		}

		private void buttonResetToLinear_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show("This will reset all values of the selected channel.\n\nContinue?", Vendor.ProductName,
				                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				ResetCurrentToLinear();
			}
		}

		private void buttonSwitchDisplay_Click(object sender, EventArgs e)
		{
			SwitchDisplay(!m_usingActualLevels);
		}

		private void comboBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selectedItem = (Channel) comboBoxChannels.SelectedItem;
			if (selectedItem != null)
			{
				if (selectedItem.DimmingCurve == null)
				{
					selectedItem.DimmingCurve = new byte[0x100];
					ResetToLinear(selectedItem.DimmingCurve);
				}
				ShowChannel(selectedItem);
				buttonResetToLinear.Enabled = true;
			}
			else
			{
				buttonResetToLinear.Enabled = false;
			}
		}

		private void CurveCalc()
		{
			var num = (int) (m_miniBoxBounds.Left*m_curveRowPointsPerMiniPixel);
			num = Math.Max(0, num - 1);
			var num2 = (int) (m_miniBoxBounds.Right*m_curveRowPointsPerMiniPixel);
			num2 = (int) Math.Min(m_availableValues - 1f, (num2 + 2));
			int index = Math.Min(num, num2);
			int num4 = Math.Max(num, num2);
			int num5 = pictureBoxCurve.Height - ((pictureBoxCurve.Height/m_gridSpacing)*m_gridSpacing);
			var num6 = (int) (m_availableValues - ((m_miniBoxBounds.Bottom + 1)*m_curveColPointsPerMiniPixel));
			var num7 = (int) (m_miniBoxBounds.Left*m_curveRowPointsPerMiniPixel);
			if ((m_curvePoints == null) || (m_curvePoints.GetLength(0) != ((num4 - index) + 1)))
			{
				m_curvePoints = new int[(num4 - index) + 1,2];
			}
			m_startCurvePoint = index;
			int num10 = -1;
			int num11 = -1;
			int num12 = 0;
			while (index < num4)
			{
				int num8 = (index - num7)*m_gridSpacing;
				int num9 = (pictureBoxCurve.Height - num5) - ((m_points[index] - num6)*m_gridSpacing);
				num10 = ((index + 1) - num7)*m_gridSpacing;
				num11 = (pictureBoxCurve.Height - num5) - ((m_points[index + 1] - num6)*m_gridSpacing);
				m_curvePoints[num12, 0] = num8;
				m_curvePoints[num12, 1] = num9;
				index++;
				num12++;
			}
			if (num10 != -1)
			{
				m_curvePoints[num12, 0] = num10;
				m_curvePoints[num12, 1] = num11;
			}
		}

		private void DimmingCurveDialog_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.TranslateTransform(-label2.Location.X, -label2.Location.Y, MatrixOrder.Append);
			e.Graphics.RotateTransform(-90f, MatrixOrder.Append);
			e.Graphics.TranslateTransform(label2.Location.X, label2.Location.Y, MatrixOrder.Append);
			e.Graphics.DrawString("Output >", Font, Brushes.Black, label2.Location);
			e.Graphics.ResetTransform();
		}


		private void library_Message(string message)
		{
			labelMessage.Text = message;
			labelMessage.Update();
		}

		private void library_Progess(int progressCount, int totalCount)
		{
			library_Message(string.Format("{0} of {1} completed", progressCount, totalCount));
		}

		private int MaxOf(params int[] values)
		{
			int num = values[0];
			foreach (int num2 in values)
			{
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		private int MinOf(params int[] values)
		{
			int num = values[0];
			foreach (int num2 in values)
			{
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		private void pictureBoxCurve_MouseLeave(object sender, EventArgs e)
		{
			Cursor = Cursors.Default;
		}

		private void pictureBoxCurve_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_points != null)
			{
				if (((e.Button & MouseButtons.Left) != MouseButtons.None) && (m_selectedPointRelative != -1))
				{
					int num = Math.Max(0, Math.Min(pictureBoxCurve.Height - 1, e.Y));
					if (((m_curvePoints[m_selectedPointRelative, 1] - num)%m_gridSpacing) == 0)
					{
						int selectedPointRelative;
						int num3;
						var rc = new Rectangle();
						if (m_selectedPointAbsolute == 0)
						{
							rc.X = m_curvePoints[m_selectedPointRelative, 0] - ((int) m_halfPointSize);
							selectedPointRelative = m_selectedPointRelative;
						}
						else
						{
							rc.X = m_curvePoints[m_selectedPointRelative - 1, 0] - ((int) m_halfPointSize);
							selectedPointRelative = m_selectedPointRelative - 1;
						}
						if (m_selectedPointAbsolute == (m_availableValues - 1f))
						{
							rc.Width = (m_curvePoints[m_selectedPointRelative, 0] + ((int) m_halfPointSize)) - rc.X;
							num3 = m_selectedPointRelative;
						}
						else
						{
							rc.Width = (m_curvePoints[m_selectedPointRelative + 1, 0] + ((int) m_halfPointSize)) - rc.X;
							num3 = m_selectedPointRelative + 1;
						}
						rc.Y =
							MinOf(new[]
								{
									num, m_curvePoints[m_selectedPointRelative, 1], m_curvePoints[selectedPointRelative, 1], m_curvePoints[num3, 1]
								}) - m_pointSize;
						rc.Height =
							((MaxOf(new[]
								{
									num, m_curvePoints[m_selectedPointRelative, 1], m_curvePoints[selectedPointRelative, 1], m_curvePoints[num3, 1]
								}) + m_pointSize) - rc.Y) + m_pointSize;
						m_points[m_selectedPointAbsolute] =
							(byte)
							(m_points[m_selectedPointAbsolute] + ((byte) ((m_curvePoints[m_selectedPointRelative, 1] - num)/m_gridSpacing)));
						m_curvePoints[m_selectedPointRelative, 1] = num;
						pictureBoxCurve.Invalidate(rc);
						pbMini.Invalidate(new Rectangle(((int) ((m_selectedPointAbsolute)/m_curveRowPointsPerMiniPixel)) - 1, 0, 3,
						                                pbMini.Height));
					}
				}
				else
				{
					int num10;
					int num11;
					int length = m_curvePoints.GetLength(0);
					int num5 = length >> 1;
					int x = e.X;
					int y = e.Y;
					if (x < (pictureBoxCurve.Width >> 1))
					{
						num10 = 0;
						num11 = num5;
					}
					else
					{
						num10 = num5;
						num11 = length;
					}
					int num12 = num10;
					while (num12 < num11)
					{
						int num8 = m_curvePoints[num12, 0];
						int num9 = m_curvePoints[num12, 1];
						if ((((x >= (num8 - m_halfPointSize)) && (x <= (num8 + m_halfPointSize))) && (y >= (num9 - m_halfPointSize))) &&
						    (y <= (num9 + m_halfPointSize)))
						{
							break;
						}
						num12++;
					}
					if (num12 < num11)
					{
						Cursor = Cursors.SizeNS;
						m_selectedPointRelative = num12;
						m_selectedPointAbsolute = m_startCurvePoint + num12;
					}
					else
					{
						Cursor = Cursors.Default;
						m_selectedPointRelative = -1;
						m_selectedPointAbsolute = -1;
					}
				}
				if (m_selectedPointAbsolute == -1)
				{
					labelChannelValue.Text = string.Empty;
				}
				else
				{
					labelChannelValue.Text = string.Format("Channel value {0} ({2:P0}) will output at {1} ({3:P0})",
					                                       new object[]
						                                       {
							                                       m_selectedPointAbsolute, m_points[m_selectedPointAbsolute],
							                                       (m_selectedPointAbsolute)/255f, (m_points[m_selectedPointAbsolute])/255f
						                                       });
				}
			}
		}

		private void pictureBoxCurve_MouseUp(object sender, MouseEventArgs e)
		{
			m_selectedPointAbsolute = -1;
			m_selectedPointRelative = -1;
			Cursor = Cursors.Default;
		}

		private void pictureBoxCurve_Paint(object sender, PaintEventArgs e)
		{
			if (m_points != null)
			{
				e.Graphics.FillRectangle(m_curveBackBrush, e.ClipRectangle);
				var num3 =
					(int)
					Math.Min(((m_miniBoxBounds.Left*m_curveRowPointsPerMiniPixel) + ((pictureBoxCurve.Width/m_gridSpacing) + 1)),
					         (m_availableValues - 1f));
				var num4 =
					(int)
					Math.Min(((m_miniBoxBounds.Top*m_curveColPointsPerMiniPixel) + ((pictureBoxCurve.Height/m_gridSpacing) + 1)),
					         (m_availableValues - 1f));
				num3 -= (int) (m_miniBoxBounds.Left*m_curveRowPointsPerMiniPixel);
				num4 -= (int) (m_miniBoxBounds.Top*m_curveColPointsPerMiniPixel);
				num3 *= m_gridSpacing;
				num4 *= m_gridSpacing;
				for (int i = (e.ClipRectangle.Top/m_gridSpacing)*m_gridSpacing; i <= num3; i += m_gridSpacing)
				{
					e.Graphics.DrawLine(m_curveGridPen, 0, i, num3, i);
				}
				for (int j = (e.ClipRectangle.Left/m_gridSpacing)*m_gridSpacing; j <= num4; j += m_gridSpacing)
				{
					e.Graphics.DrawLine(m_curveGridPen, j, 0, j, num4);
				}
				int num5 = Math.Max((e.ClipRectangle.Left/m_gridSpacing) - 1, 0);
				int num6 = Math.Min((e.ClipRectangle.Right/m_gridSpacing) + 3, m_curvePoints.GetLength(0));
				for (int k = num5; k < num6; k++)
				{
					float num7 = m_curvePoints[k, 0];
					float num8 = m_curvePoints[k, 1];
					if (k < (num6 - 1))
					{
						float num9 = m_curvePoints[k + 1, 0];
						float num10 = m_curvePoints[k + 1, 1];
						e.Graphics.DrawLine(m_curveLinePen, num7, num8, num9, num10);
					}
					e.Graphics.FillRectangle(m_curvePointBrush, num7 - m_halfPointSize, num8 - m_halfPointSize, m_pointSize,
					                         m_pointSize);
				}
			}
		}

		private void pictureBoxMini_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_points != null)
			{
				if (!m_miniBoxBounds.Contains(e.Location))
				{
					Rectangle miniBoxBounds = m_miniBoxBounds;
					m_miniBoxBounds.X = Math.Max(0,
					                             Math.Min(((pbMini.Width - m_miniBoxBounds.Width) - 1),
					                                      (e.X - (m_miniBoxBounds.Width/2))));
					m_miniBoxBounds.Y = Math.Max(0,
					                             Math.Min(((pbMini.Height - m_miniBoxBounds.Height) - 1),
					                                      (e.Y - (m_miniBoxBounds.Height/2))));
					RedrawMiniBox(miniBoxBounds);
					RedrawCurve();
				}
				m_miniMouseMinLocation.X = e.X - m_miniBoxBounds.X;
				m_miniMouseMinLocation.Y = e.Y - m_miniBoxBounds.Y;
				m_miniMouseMaxLocation.X = (pbMini.Width - (m_miniBoxBounds.Right - e.X)) - 1;
				m_miniMouseMaxLocation.Y = (pbMini.Height - (m_miniBoxBounds.Bottom - e.Y)) - 1;
				m_miniMouseDownLast = e.Location;
			}
		}

		private void pictureBoxMini_MouseMove(object sender, MouseEventArgs e)
		{
			if ((m_points != null) && ((e.Button & MouseButtons.Left) != MouseButtons.None))
			{
				int num = Math.Max(Math.Min(e.X, m_miniMouseMaxLocation.X), m_miniMouseMinLocation.X);
				int num2 = Math.Max(Math.Min(e.Y, m_miniMouseMaxLocation.Y), m_miniMouseMinLocation.Y);
				if ((num != m_miniMouseDownLast.X) || (num2 != m_miniMouseDownLast.Y))
				{
					Rectangle miniBoxBounds = m_miniBoxBounds;
					m_miniBoxBounds.X += num - m_miniMouseDownLast.X;
					m_miniMouseDownLast.X = num;
					m_miniBoxBounds.Y += num2 - m_miniMouseDownLast.Y;
					m_miniMouseDownLast.Y = num2;
					RedrawMiniBox(miniBoxBounds);
					RedrawCurve();
				}
			}
		}

		private void pictureBoxMini_Paint(object sender, PaintEventArgs e)
		{
			if (m_points != null)
			{
				e.Graphics.FillRectangle(m_miniBackBrush, e.ClipRectangle);
				var num = (int) (e.ClipRectangle.Left*m_curveRowPointsPerMiniPixel);
				num = Math.Max(0, num - 1);
				var num2 = (int) (e.ClipRectangle.Right*m_curveRowPointsPerMiniPixel);
				num2 = (int) Math.Min(m_availableValues - 1f, (num2 + 1));
				int num3 = Math.Min(num, num2);
				int num4 = Math.Max(num, num2);
				for (int i = num3; i < num4; i++)
				{
					e.Graphics.DrawLine(m_miniLinePen, ((i)/m_curveRowPointsPerMiniPixel),
					                    (pbMini.Height - ((m_points[i])/m_curveColPointsPerMiniPixel)),
					                    (((i + 1))/m_curveRowPointsPerMiniPixel),
					                    (pbMini.Height - ((m_points[i + 1])/m_curveColPointsPerMiniPixel)));
				}
				if (e.ClipRectangle.IntersectsWith(m_miniBoxBounds))
				{
					e.Graphics.DrawRectangle(m_miniBoxPen, m_miniBoxBounds);
				}
			}
		}

		private void RedrawBoth()
		{
			CurveCalc();
			pbMini.Refresh();
			pictureBoxCurve.Refresh();
		}

		private void RedrawCurve()
		{
			CurveCalc();
			pictureBoxCurve.Refresh();
		}

		private void RedrawMiniBox(Rectangle oldBounds)
		{
			Rectangle rc = Rectangle.Union(m_miniBoxBounds, oldBounds);
			rc.Inflate(1, 1);
			pbMini.Invalidate(rc);
		}

		private void ResetCurrentToLinear()
		{
			ResetToLinear(m_points);
			CurveCalc();
			pbMini.Refresh();
			pictureBoxCurve.Refresh();
		}

		private void ResetToLinear(byte[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				values[i] = (byte) i;
			}
		}

		private void ShowChannel(Channel channel)
		{
			m_points = channel.DimmingCurve;
			RedrawBoth();
		}

		private void SwitchDisplay(bool useActualLevels)
		{
			useActualLevels = true;
			m_usingActualLevels = useActualLevels;
			if (m_usingActualLevels)
			{
				m_availableValues = 256f;
				pbMini.Size = new Size(0x100, 0x100);
			}
			else
			{
				m_availableValues = 101f;
				pbMini.Size = new Size(100, 100);
			}
			pbMini.Refresh();
			RedrawCurve();
			buttonSwitchDisplay.Text = useActualLevels ? "Show % levels" : "Show actual levels";
		}
	}
}