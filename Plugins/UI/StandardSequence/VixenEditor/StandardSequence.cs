using FMOD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using VixenPlus;
using VixenPlus.Dialogs;

namespace VixenEditor
{
	public partial class StandardSequence : UIBase
	{
		private const int CHANNEL_SCROLL_THRESHOLD = 2;
		private const int DRAG_THRESHOLD = 3;
		private bool m_actualLevels;
		private AffectGridDelegate m_affectGrid;
		private Bitmap m_arrowBitmap;
		private bool m_autoScrolling;
		private int[] m_bookmarks;
		private SolidBrush m_channelBackBrush;
		private SolidBrush m_channelCaretBrush;
		private Font m_channelNameFont;
		private List<int> m_channelOrderMapping;
		private VixenPlus.Channel m_currentlyEditingChannel;
		private XmlNode m_dataNode;
		private FrequencyEffectGenerator m_dimmingShimmerGenerator;
		private byte m_drawingLevel;
		private int m_editingChannelSortedIndex;
		private int m_executionContextHandle;
		private IExecution m_executionInterface;
		private SolidBrush m_gridBackBrush;
		private Graphics m_gridGraphics;
		private int m_gridRowHeight;
		private bool m_initializing;
		private int m_intensityLargeDelta;
		private int m_lastCellX;
		private int m_lastCellY;
		private Rectangle m_lineRect;
		private int m_mouseChannelCaret;
		private Point m_mouseDownAtInChannels;
		private Point m_mouseDownAtInGrid;
		private bool m_mouseDownInGrid;
		private int m_mouseTimeCaret;
		private int m_mouseWheelHorizontalDelta;
		private int m_mouseWheelVerticalDelta;
		private Rectangle m_normalizedRange;
		private int m_periodPixelWidth;
		private EventHandler m_pluginCheckHandler;
		private int m_position;
		private SolidBrush m_positionBrush;
		private Preference2 m_preferences;
		private int m_previousPosition;
		private int m_printingChannelIndex;
		private List<VixenPlus.Channel> m_printingChannelList;
		private Stack m_redoStack;
		private VixenPlus.Channel m_selectedChannel;
		private int m_selectedEventIndex;
		private int m_selectedLineIndex;
		private Rectangle m_selectedRange;
		private SolidBrush m_selectionBrush;
		private Rectangle m_selectionRectangle;
		private EventSequence m_sequence;
		private bool m_showCellText;
		private bool m_showingGradient;
		private bool m_showingOutputs;
		private bool m_showPositionMarker;
		private FrequencyEffectGenerator m_sparkleGenerator;
		private ISystem m_systemInterface;
		private SolidBrush m_timeBackBrush;
		private Font m_timeFont;
		private EventHandler m_toolStripCheckStateChangeHandler;
		private Dictionary<string, ToolStrip> m_toolStrips;
		private Stack m_undoStack;
		private int m_visibleEventPeriods;
		private int m_visibleRowCount;
		private int m_waveform100PercentAmplitude;
		private int m_waveformMaxAmplitude;
		private int m_waveformOffset;
		private uint[] m_waveformPCMData;
		private uint[] m_waveformPixelData;
		private const int POSITION_MARK_BOTTOM = 20;
		private const int POSITION_MARK_TOP = 0x23;
		private PrintDocument printDocument;

		public StandardSequence()
		{
			object obj2;
			m_executionInterface = null;
			m_systemInterface = null;
			m_gridRowHeight = 20;
			m_visibleRowCount = 0;
			m_visibleEventPeriods = 0;
			m_channelBackBrush = null;
			m_timeBackBrush = null;
			m_gridBackBrush = null;
			m_channelNameFont = new Font("Arial", 8f);
			m_timeFont = new Font("Arial", 8f);
			m_selectedChannel = null;
			m_currentlyEditingChannel = null;
			m_editingChannelSortedIndex = 0;
			m_gridGraphics = null;
			m_selectedRange = new Rectangle();
			m_periodPixelWidth = 30;
			m_selectionRectangle = new Rectangle();
			m_selectionBrush = new SolidBrush(Color.FromArgb(0x3f, Color.Blue));
			m_positionBrush = new SolidBrush(Color.FromArgb(0x3f, Color.Red));
			m_mouseDownInGrid = false;
			m_position = -1;
			m_previousPosition = -1;
			m_mouseChannelCaret = -1;
			m_mouseTimeCaret = -1;
			m_channelCaretBrush = new SolidBrush(Color.Gray);
			m_undoStack = new Stack();
			m_redoStack = new Stack();
			m_lineRect = new Rectangle(-1, -1, -1, -1);
			m_lastCellX = -1;
			m_lastCellY = -1;
			m_initializing = false;
			m_selectedEventIndex = -1;
			m_normalizedRange = new Rectangle();
			m_mouseDownAtInGrid = new Point();
			m_mouseDownAtInChannels = Point.Empty;
			m_waveformPCMData = null;
			m_waveformPixelData = null;
			m_waveformOffset = 0x24;
			m_showingOutputs = false;
			m_selectedLineIndex = 0;
			m_arrowBitmap = null;
			m_bookmarks = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
			m_showingGradient = true;
			m_actualLevels = false;
			m_showCellText = false;
			m_lastSelectableControl = null;
			if (Interfaces.Available.TryGetValue("IExecution", out obj2))
			{
				m_executionInterface = (IExecution) obj2;
			}
			if (Interfaces.Available.TryGetValue("ISystem", out obj2))
			{
				m_systemInterface = (ISystem) obj2;
			}
			m_toolStrips = new Dictionary<string, ToolStrip>();
			m_toolStripCheckStateChangeHandler = new EventHandler(toolStripItem_CheckStateChanged);
		}

		private void additionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ArithmeticPaste(ArithmeticOperation.Addition);
		}

		private void AddUndoItem(Rectangle blockAffected, UndoOriginalBehavior behavior)
		{
			if (blockAffected.Width != 0)
			{
				byte[,] affectedBlockData = GetAffectedBlockData(blockAffected);
				m_undoStack.Push(new UndoItem(blockAffected.Location, affectedBlockData, behavior, m_sequence, m_channelOrderMapping));
				toolStripButtonUndo.Enabled = undoToolStripMenuItem.Enabled = true;
				UpdateUndoText();
				toolStripButtonRedo.Enabled = redoToolStripMenuItem.Enabled = false;
				m_redoStack.Clear();
				UpdateRedoText();
				base.IsDirty = true;
			}
		}

		private void AddUndoItem(Rectangle blockAffected, UndoOriginalBehavior behavior, bool isRelative)
		{
			if (blockAffected.Width != 0)
			{
				if (isRelative)
				{
					blockAffected.X += hScrollBar1.Value;
					blockAffected.Y += vScrollBar1.Value;
				}
				AddUndoItem(blockAffected, behavior);
			}
		}

		private void AffectGrid(int startRow, int startCol, byte[,] values)
		{
			AddUndoItem(new Rectangle(startCol, startRow, values.GetLength(1), values.GetLength(0)), UndoOriginalBehavior.Overwrite);
			for (int i = 0; i < values.GetLength(0); i++)
			{
				int num = m_channelOrderMapping[startRow + i];
				for (int j = 0; j < values.GetLength(1); j++)
				{
					m_sequence.EventValues[num, startCol + j] = values[i, j];
				}
			}
			pictureBoxGrid.Refresh();
			base.IsDirty = true;
		}

		private void allChannelsToFullIntensityForThisEventToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m_selectedEventIndex != -1)
			{
				Rectangle blockAffected = new Rectangle(m_selectedEventIndex, 0, 1, m_sequence.ChannelCount);
				AddUndoItem(blockAffected, UndoOriginalBehavior.Overwrite);
				for (int i = 0; i < m_sequence.ChannelCount; i++)
				{
					m_sequence.EventValues[m_channelOrderMapping[i], m_selectedEventIndex] = m_drawingLevel;
				}
				InvalidateRect(blockAffected);
			}
		}

		private void allEventsToFullIntensityToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FillChannel(m_selectedLineIndex);
		}

		private void ArithmeticPaste(ArithmeticOperation operation)
		{
			if (m_systemInterface.Clipboard != null)
			{
				AddUndoItem(new Rectangle(m_normalizedRange.X, m_normalizedRange.Y, m_systemInterface.Clipboard.GetLength(1), m_systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite);
				byte[,] clipboard = m_systemInterface.Clipboard;
				int length = clipboard.GetLength(0);
				int num3 = clipboard.GetLength(1);
				for (int i = 0; (i < length) && ((m_normalizedRange.Top + i) < m_sequence.ChannelCount); i++)
				{
					int num4 = m_channelOrderMapping[m_normalizedRange.Top + i];
					for (int j = 0; (j < num3) && ((m_normalizedRange.Left + j) < m_sequence.TotalEventPeriods); j++)
					{
						byte num5 = m_sequence.EventValues[num4, m_normalizedRange.Left + j];
						switch (operation)
						{
							case ArithmeticOperation.Addition:
								m_sequence.EventValues[num4, m_normalizedRange.Left + j] = (byte) Math.Min(num5 + clipboard[i, j], (int) m_sequence.MaximumLevel);
								break;

							case ArithmeticOperation.Subtraction:
								m_sequence.EventValues[num4, m_normalizedRange.Left + j] = (byte) Math.Max(num5 - clipboard[i, j], (int) m_sequence.MinimumLevel);
								break;

							case ArithmeticOperation.Scale:
								m_sequence.EventValues[num4, m_normalizedRange.Left + j] = (byte) Math.Max(Math.Min(num5 * (((float) clipboard[i, j]) / 255f), (float) m_sequence.MaximumLevel), (float) m_sequence.MinimumLevel);
								break;

							case ArithmeticOperation.Min:
								m_sequence.EventValues[num4, m_normalizedRange.Left + j] = Math.Max(Math.Min(clipboard[i, j], num5), m_sequence.MinimumLevel);
								break;

							case ArithmeticOperation.Max:
								m_sequence.EventValues[num4, m_normalizedRange.Left + j] = Math.Min(Math.Max(clipboard[i, j], num5), m_sequence.MaximumLevel);
								break;
						}
						m_sequence.EventValues[num4, m_normalizedRange.Left + j] = Math.Min(m_sequence.EventValues[num4, m_normalizedRange.Left + j], m_sequence.MaximumLevel);
						m_sequence.EventValues[num4, m_normalizedRange.Left + j] = Math.Max(m_sequence.EventValues[num4, m_normalizedRange.Left + j], m_sequence.MinimumLevel);
					}
				}
				base.IsDirty = true;
				pictureBoxGrid.Refresh();
			}
		}

		private void ArrayToCells(byte[,] array)
		{
			int length = array.GetLength(0);
			int num3 = array.GetLength(1);
			for (int i = 0; (i < length) && ((m_normalizedRange.Top + i) < m_sequence.ChannelCount); i++)
			{
				int num4 = m_channelOrderMapping[m_normalizedRange.Top + i];
				for (int j = 0; (j < num3) && ((m_normalizedRange.Left + j) < m_sequence.TotalEventPeriods); j++)
				{
					m_sequence.EventValues[num4, m_normalizedRange.Left + j] = array[i, j];
				}
			}
			base.IsDirty = true;
		}

		private void AssignChannelArray(List<VixenPlus.Channel> channels)
		{
			m_sequence.Channels = channels;
			textBoxChannelCount.Text = m_sequence.ChannelCount.ToString();
			VScrollCheck();
		}

		private void attachSequenceToToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog1.Filter = "Profile | *.pro";
			openFileDialog1.DefaultExt = "pro";
			openFileDialog1.InitialDirectory = Paths.ProfilePath;
			openFileDialog1.FileName = string.Empty;
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				SetProfile(openFileDialog1.FileName);
			}
		}

		private void BooleanPaste(BooleanOperation operation)
		{
			if (m_systemInterface.Clipboard != null)
			{
				AddUndoItem(new Rectangle(m_normalizedRange.X, m_normalizedRange.Y, m_systemInterface.Clipboard.GetLength(1), m_systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite);
				byte[,] clipboard = m_systemInterface.Clipboard;
				int length = clipboard.GetLength(0);
				int num3 = clipboard.GetLength(1);
				for (int i = 0; (i < length) && ((m_normalizedRange.Top + i) < m_sequence.ChannelCount); i++)
				{
					int num4 = m_channelOrderMapping[m_normalizedRange.Top + i];
					for (int j = 0; (j < num3) && ((m_normalizedRange.Left + j) < m_sequence.TotalEventPeriods); j++)
					{
						switch (operation)
						{
							case BooleanOperation.OR:
							{
								byte num1 = m_sequence.EventValues[num4, m_normalizedRange.Left + j];
								num1 = (byte) (num1 | clipboard[i, j]);
								break;
							}
							case BooleanOperation.AND:
							{
								byte num6 = m_sequence.EventValues[num4, m_normalizedRange.Left + j];
								num6 = (byte) (num6 & clipboard[i, j]);
								break;
							}
							case BooleanOperation.XOR:
							{
								byte num7 = m_sequence.EventValues[num4, m_normalizedRange.Left + j];
								num7 = (byte) (num7 ^ clipboard[i, j]);
								break;
							}
							case BooleanOperation.NOR:
								m_sequence.EventValues[num4, m_normalizedRange.Left + j] = (byte) ~(clipboard[i, j] | m_sequence.EventValues[num4, m_normalizedRange.Left + j]);
								break;

							case BooleanOperation.NAND:
								m_sequence.EventValues[num4, m_normalizedRange.Left + j] = (byte) ~(clipboard[i, j] & m_sequence.EventValues[num4, m_normalizedRange.Left + j]);
								break;

							case BooleanOperation.XNOR:
								m_sequence.EventValues[num4, m_normalizedRange.Left + j] = (byte) ~(clipboard[i, j] ^ m_sequence.EventValues[num4, m_normalizedRange.Left + j]);
								break;
						}
					}
				}
				base.IsDirty = true;
				pictureBoxGrid.Refresh();
			}
		}

		private void BresenhamPaint(Rectangle rect, byte[] brush)
		{
			int num4;
			int num5;
			int num6;
			int num7;
			int num9;
			int num10;
			int num12;
			int num13;
			int num2 = Math.Abs(rect.Width);
			int num3 = Math.Abs(rect.Height);
			if (num2 >= num3)
			{
				num4 = num2 + 1;
				num5 = (num3 << 1) - num2;
				num6 = num3 << 1;
				num7 = (num3 - num2) << 1;
				num9 = 1;
				num10 = 1;
				num12 = 0;
				num13 = 1;
			}
			else
			{
				num4 = num3 + 1;
				num5 = (num2 << 1) - num3;
				num6 = num2 << 1;
				num7 = (num2 - num3) << 1;
				num9 = 0;
				num10 = 1;
				num12 = 1;
				num13 = 1;
			}
			if (rect.Left > rect.Right)
			{
				num9 = -num9;
				num10 = -num10;
			}
			if (rect.Top > rect.Bottom)
			{
				num12 = -num12;
				num13 = -num13;
			}
			int left = rect.Left;
			int top = rect.Top;
			for (int i = 0; i < num4; i++)
			{
				int num16 = left;
				int num14 = Math.Min(num16 + brush.Length, m_sequence.TotalEventPeriods) - num16;
				for (int j = 0; j < num14; j++)
				{
					m_sequence.EventValues[m_channelOrderMapping[top], num16 + j] = brush[j];
				}
				if (num5 < 0)
				{
					num5 += num6;
					left += num9;
					top += num12;
				}
				else
				{
					num5 += num7;
					left += num10;
					top += num13;
				}
			}
			base.IsDirty = true;
		}

		private void BresenhamValues(Rectangle rect)
		{
			int num4;
			int num5;
			int num6;
			int num7;
			int num9;
			int num10;
			int num12;
			int num13;
			int num2 = Math.Abs(rect.Width);
			int num3 = Math.Abs(rect.Height);
			if (num2 >= num3)
			{
				num4 = num2 + 1;
				num5 = (num3 << 1) - num2;
				num6 = num3 << 1;
				num7 = (num3 - num2) << 1;
				num9 = 1;
				num10 = 1;
				num12 = 0;
				num13 = 1;
			}
			else
			{
				num4 = num3 + 1;
				num5 = (num2 << 1) - num3;
				num6 = num2 << 1;
				num7 = (num2 - num3) << 1;
				num9 = 0;
				num10 = 1;
				num12 = 1;
				num13 = 1;
			}
			if (rect.Left > rect.Right)
			{
				num9 = -num9;
				num10 = -num10;
			}
			if (rect.Top > rect.Bottom)
			{
				num12 = -num12;
				num13 = -num13;
			}
			int left = rect.Left;
			int top = rect.Top;
			for (int i = 0; i < num4; i++)
			{
				m_sequence.EventValues[m_channelOrderMapping[top], left] = m_drawingLevel;
				if (num5 < 0)
				{
					num5 += num6;
					left += num9;
					top += num12;
				}
				else
				{
					num5 += num7;
					left += num10;
					top += num13;
				}
			}
			base.IsDirty = true;
		}

		private byte[,] CellsToArray()
		{
			byte[,] buffer = new byte[m_normalizedRange.Height, m_normalizedRange.Width];
			for (int i = 0; i < m_normalizedRange.Height; i++)
			{
				int num2 = m_channelOrderMapping[m_normalizedRange.Top + i];
				for (int j = 0; j < m_normalizedRange.Width; j++)
				{
					buffer[i, j] = m_sequence.EventValues[num2, m_normalizedRange.Left + j];
				}
			}
			return buffer;
		}

		private Rectangle CellsToPixels(Rectangle relativeCells)
		{
			Rectangle rectangle = new Rectangle();
			rectangle.X = (Math.Min(relativeCells.Left, relativeCells.Right) * m_periodPixelWidth) + 1;
			rectangle.Y = (Math.Min(relativeCells.Top, relativeCells.Bottom) * m_gridRowHeight) + 1;
			rectangle.Width = ((Math.Abs(relativeCells.Width) + 1) * m_periodPixelWidth) - 1;
			rectangle.Height = ((Math.Abs(relativeCells.Height) + 1) * m_gridRowHeight) - 1;
			return rectangle;
		}

		private bool ChannelClickValid()
		{
			bool flag = false;
			if (pictureBoxChannels.PointToClient(Control.MousePosition).Y > pictureBoxTime.Height)
			{
				m_selectedLineIndex = vScrollBar1.Value + ((pictureBoxChannels.PointToClient(Control.MousePosition).Y - pictureBoxTime.Height) / m_gridRowHeight);
				if (m_selectedLineIndex < m_sequence.ChannelCount)
				{
					m_editingChannelSortedIndex = m_channelOrderMapping[m_selectedLineIndex];
					flag = (m_editingChannelSortedIndex >= 0) && (m_editingChannelSortedIndex < m_sequence.ChannelCount);
				}
			}
			if (flag)
			{
				m_currentlyEditingChannel = SelectedChannel = m_sequence.Channels[m_editingChannelSortedIndex];
			}
			return flag;
		}

		private void ChannelCountChanged()
		{
			base.IsDirty = true;
			textBoxChannelCount.Text = m_sequence.ChannelCount.ToString();
			pictureBoxChannels.Refresh();
			pictureBoxGrid.Refresh();
		}

		private void channelOutputMaskToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditSequenceChannelMask();
		}

		private void channelPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowChannelProperties();
		}

		private DialogResult CheckDirty()
		{
			DialogResult none = DialogResult.None;
			if (base.IsDirty)
			{
				string str = (m_sequence.Name == null) ? "this unnamed sequence" : m_sequence.Name;
				none = MessageBox.Show(string.Format("Save changes to {0}?", str), Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (none == DialogResult.Yes)
				{
					m_systemInterface.InvokeSave(this);
				}
			}
			return none;
		}

		private void CheckMaximums()
		{
			bool flag = false;
			for (int i = 0; i < m_sequence.ChannelCount; i++)
			{
				int num3 = m_channelOrderMapping[i];
				for (int j = 0; j < m_sequence.TotalEventPeriods; j++)
				{
					byte num2 = m_sequence.EventValues[num3, j];
					if (num2 != 0)
					{
						m_sequence.EventValues[num3, j] = Math.Min(num2, m_sequence.MaximumLevel);
					}
				}
			}
			if (flag)
			{
				base.IsDirty = true;
			}
		}

		private void CheckMinimums()
		{
			bool flag = false;
			for (int i = 0; i < m_sequence.ChannelCount; i++)
			{
				int num3 = m_channelOrderMapping[i];
				for (int j = 0; j < m_sequence.TotalEventPeriods; j++)
				{
					byte num2 = m_sequence.EventValues[num3, j];
					m_sequence.EventValues[num3, j] = Math.Max(num2, m_sequence.MinimumLevel);
					flag = true;
				}
			}
			if (flag)
			{
				base.IsDirty = true;
			}
		}

		private void clearAllChannelsForThisEventToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m_selectedEventIndex != -1)
			{
				Rectangle blockAffected = new Rectangle(m_selectedEventIndex, 0, 1, m_sequence.ChannelCount);
				AddUndoItem(blockAffected, UndoOriginalBehavior.Overwrite);
				for (int i = 0; i < m_sequence.ChannelCount; i++)
				{
					m_sequence.EventValues[m_channelOrderMapping[i], m_selectedEventIndex] = m_sequence.MinimumLevel;
				}
				InvalidateRect(blockAffected);
			}
		}

		private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Clear all events in the sequence?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Rectangle normalizedRange = m_normalizedRange;
				m_normalizedRange = new Rectangle(0, 0, m_sequence.TotalEventPeriods, m_sequence.ChannelCount);
				TurnCellsOff();
				m_normalizedRange = normalizedRange;
				pictureBoxGrid.Refresh();
			}
		}

		private void ClearChannel(int lineIndex)
		{
			AddUndoItem(new Rectangle(0, lineIndex, m_sequence.TotalEventPeriods, 1), UndoOriginalBehavior.Overwrite);
			for (int i = 0; i < m_sequence.TotalEventPeriods; i++)
			{
				m_sequence.EventValues[m_editingChannelSortedIndex, i] = m_sequence.MinimumLevel;
			}
			pictureBoxGrid.Refresh();
		}

		private void clearChannelEventsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClearChannel(m_selectedLineIndex);
		}

		private void contextMenuChannels_Opening(object sender, CancelEventArgs e)
		{
			e.Cancel = !ChannelClickValid();
		}

		private void contextMenuGrid_Opening(object sender, CancelEventArgs e)
		{
			if (m_currentlyEditingChannel == null)
			{
				e.Cancel = true;
			}
			saveAsARoutineToolStripMenuItem.Enabled = m_normalizedRange.Width >= 1;
		}

		private void contextMenuTime_Opening(object sender, CancelEventArgs e)
		{
			m_selectedEventIndex = hScrollBar1.Value + (pictureBoxTime.PointToClient(Control.MousePosition).X / m_periodPixelWidth);
		}

		private void CopyCells()
		{
			m_systemInterface.Clipboard = CellsToArray();
		}

		private void copyChannelEventsToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			byte[,] buffer = new byte[1, m_sequence.TotalEventPeriods];
			for (int i = 0; i < m_sequence.TotalEventPeriods; i++)
			{
				buffer[0, i] = m_sequence.EventValues[m_editingChannelSortedIndex, i];
			}
			m_systemInterface.Clipboard = buffer;
		}

		private void copyChannelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ChannelCopyDialog(m_affectGrid, m_sequence, m_channelOrderMapping).Show();
		}

		private void createFromSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Profile objectInContext = new Profile();
			objectInContext.InheritChannelsFrom(m_sequence);
			objectInContext.InheritPlugInDataFrom(m_sequence);
			objectInContext.InheritSortsFrom(m_sequence);
			ProfileManagerDialog dialog = new ProfileManagerDialog(objectInContext);
			if ((dialog.ShowDialog() == DialogResult.OK) && (MessageBox.Show("Do you want to attach this sequence to the new profile?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
			{
				SetProfile(objectInContext);
			}
			dialog.Dispose();
		}

		private void currentProgramsSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SequenceSettingsDialog dialog = new SequenceSettingsDialog(m_sequence);
			int minimumLevel = m_sequence.MinimumLevel;
			int maximumLevel = m_sequence.MaximumLevel;
			int eventPeriod = m_sequence.EventPeriod;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				if (minimumLevel != m_sequence.MinimumLevel)
				{
					CheckMinimums();
					if (m_drawingLevel < m_sequence.MinimumLevel)
					{
						SetDrawingLevel(m_sequence.MinimumLevel);
						MessageBox.Show("Drawing level was below the sequence minimum, so it has been adjusted.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
				if (maximumLevel != m_sequence.MaximumLevel)
				{
					CheckMaximums();
					if (m_drawingLevel > m_sequence.MaximumLevel)
					{
						SetDrawingLevel(m_sequence.MaximumLevel);
						MessageBox.Show("Drawing level was above the sequence maximum, so it has been adjusted.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
				if (eventPeriod != m_sequence.EventPeriod)
				{
					HScrollCheck();
					ParseAudioWaveform();
					pictureBoxTime.Refresh();
				}
				pictureBoxGrid.Refresh();
			}
			dialog.Dispose();
		}

		private void DeleteChannelFromSort(int naturalIndex)
		{
			m_channelOrderMapping.Remove(naturalIndex);
			for (int i = 0; i < m_channelOrderMapping.Count; i++)
			{
				if (m_channelOrderMapping[i] > naturalIndex)
				{
					List<int> list;
					int num2;
					(list = m_channelOrderMapping)[num2 = i] = list[num2] - 1;
				}
			}
		}

		private void detachSequenceFromItsProfileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Do you wish to detach this sequence from its profile?\n\nThis will not cause anything to be deleted.\nVixen will attempt to reload channel and plugin data from the sequence.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				SetProfile((Profile) null);
			}
		}

		private void DimmingShimmerGenerator(byte[,] values, params int[] effectParameters)
		{
			int num = effectParameters[0];
			if (num != 0)
			{
				int num2 = (int) Math.Round((double) ((1000f / ((float) m_sequence.EventPeriod)) / ((float) num)), MidpointRounding.AwayFromZero);
				if (num2 != 0)
				{
					int length = values.GetLength(1);
					int num8 = values.GetLength(0);
					Random random = new Random();
					for (int i = 0; i < length; i += num2)
					{
						int num4 = Math.Min(length, i + num2) - i;
						byte num6 = (byte) Math.Max(random.NextDouble() * m_sequence.MaximumLevel, (double) m_sequence.MinimumLevel);
						for (int j = 0; j < num4; j++)
						{
							for (int k = 0; k < num8; k++)
							{
								values[k, i + j] = num6;
							}
						}
					}
				}
			}
		}

		private void DisableWaveformButton()
		{
			toolStripButtonWaveform.Enabled = false;
			toolStripComboBoxWaveformZoom.Enabled = false;
			toolStripLabelWaveformZoom.Enabled = false;
		}

		private void DisableWaveformDisplay()
		{
			toolStripButtonWaveform.Checked = false;
			m_waveformPixelData = null;
			m_waveformPCMData = null;
			pictureBoxTime.Height = 60;
			pictureBoxTime.Refresh();
			pictureBoxChannels.Refresh();
			DisableWaveformButton();
		}

		private void DisjointedInsert(int x, int y, int width, int height, int[] channelIndexes)
		{
			for (int i = 0; i < height; i++)
			{
				int num = channelIndexes[i];
				for (int j = ((m_sequence.TotalEventPeriods - x) - width) - 1; j >= 0; j--)
				{
					m_sequence.EventValues[num, (j + x) + width] = m_sequence.EventValues[num, j + x];
				}
			}
		}

		private void DisjointedOverwrite(int x, int y, byte[,] data, int[] channelIndexes)
		{
			for (int i = 0; i < data.GetLength(0); i++)
			{
				int num = channelIndexes[i];
				for (int j = 0; j < data.GetLength(1); j++)
				{
					m_sequence.EventValues[num, j + x] = data[i, j];
				}
			}
		}

		private void DisjointedRemove(int x, int y, int width, int height, int[] channelIndexes)
		{
			int num;
			int num2;
			int num3;
			for (num3 = 0; num3 < height; num3++)
			{
				num = channelIndexes[num3];
				num2 = 0;
				while (num2 < ((m_sequence.TotalEventPeriods - x) - width))
				{
					m_sequence.EventValues[num, num2 + x] = m_sequence.EventValues[num, (num2 + x) + width];
					num2++;
				}
			}
			for (num3 = 0; num3 < height; num3++)
			{
				num = channelIndexes[num3];
				for (num2 = 0; num2 < width; num2++)
				{
					m_sequence.EventValues[num, (m_sequence.TotalEventPeriods - width) + num2] = m_sequence.MinimumLevel;
				}
			}
		}

		

		private void DrawSelectedRange()
		{
			Point point = new Point();
			Point point2 = new Point();
			point.X = (m_normalizedRange.Left - hScrollBar1.Value) * m_periodPixelWidth;
			point.Y = (m_normalizedRange.Top - vScrollBar1.Value) * m_gridRowHeight;
			point2.X = (m_normalizedRange.Right - hScrollBar1.Value) * m_periodPixelWidth;
			point2.Y = (m_normalizedRange.Bottom - vScrollBar1.Value) * m_gridRowHeight;
			m_selectionRectangle.X = point.X;
			m_selectionRectangle.Y = point.Y;
			m_selectionRectangle.Width = point2.X - point.X;
			m_selectionRectangle.Height = point2.Y - point.Y;
			if (m_selectionRectangle.Width == 0)
			{
				m_selectionRectangle.Width = m_periodPixelWidth;
			}
			if (m_selectionRectangle.Height == 0)
			{
				m_selectionRectangle.Height = m_gridRowHeight;
			}
			pictureBoxGrid.Invalidate(m_selectionRectangle);
			pictureBoxGrid.Update();
		}

		private void EditSequenceChannelMask()
		{
			ChannelOutputMaskDialog dialog = new ChannelOutputMaskDialog(m_sequence.Channels);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				foreach (VixenPlus.Channel channel in m_sequence.Channels)
				{
					channel.Enabled = true;
				}
				foreach (int num in dialog.DisabledChannels)
				{
					m_sequence.Channels[num].Enabled = false;
				}
				base.IsDirty = true;
			}
			dialog.Dispose();
		}

		private void EnableWaveformButton()
		{
			if (m_sequence.Audio != null)
			{
				toolStripButtonWaveform.Enabled = true;
				if (toolStripButtonWaveform.Checked)
				{
					toolStripComboBoxWaveformZoom.Enabled = true;
					toolStripLabelWaveformZoom.Enabled = true;
				}
			}
		}

		private void EraseRectangleEntity(Rectangle rect)
		{
			rect.Offset(-hScrollBar1.Value, -vScrollBar1.Value);
			Rectangle rc = CellsToPixels(rect);
			rect.X = -1;
			pictureBoxGrid.Invalidate(rc);
		}

		private void EraseSelectedRange()
		{
			Rectangle rc = SelectionToRectangle();
			m_normalizedRange.Width = m_selectedRange.Width = 0;
			pictureBoxGrid.Invalidate(rc);
		}

		private void exportChannelNamesListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string path = Path.Combine(Paths.ImportExportPath, m_sequence.Name + "_channels.txt");
			StreamWriter writer = new StreamWriter(path);
			try
			{
				foreach (VixenPlus.Channel channel in m_sequence.Channels)
				{
					writer.WriteLine(channel.Name);
				}
				MessageBox.Show("Channel name list exported to " + path, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			finally
			{
				writer.Close();
			}
		}

		private void FillChannel(int lineIndex)
		{
			AddUndoItem(new Rectangle(0, lineIndex, m_sequence.TotalEventPeriods, 1), UndoOriginalBehavior.Overwrite);
			for (int i = 0; i < m_sequence.TotalEventPeriods; i++)
			{
				m_sequence.EventValues[m_editingChannelSortedIndex, i] = m_drawingLevel;
			}
			pictureBoxGrid.Refresh();
			base.IsDirty = true;
		}

		private void flattenProfileIntoSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("This will detach the sequence from the profile and bring the profile data into the sequence.\nIs this what you want to do?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				Profile profile = m_sequence.Profile;
				m_sequence.Profile = null;
				List<VixenPlus.Channel> list = new List<VixenPlus.Channel>();
				list.AddRange(profile.Channels);
				m_sequence.Channels = list;
				m_sequence.Sorts.LoadFrom(profile.Sorts);
				m_sequence.PlugInData.LoadFromXml(profile.PlugInData.RootNode.ParentNode);
				base.IsDirty = true;
				ReactToProfileAssignment();
			}
		}

		private byte[,] GetAffectedBlockData(Rectangle blockAffected)
		{
			if (blockAffected.Width < 0)
			{
				blockAffected.X += blockAffected.Width;
				blockAffected.Width = -blockAffected.Width;
			}
			if (blockAffected.Height < 0)
			{
				blockAffected.Y += blockAffected.Height;
				blockAffected.Height = -blockAffected.Height;
			}
			if (blockAffected.Right > m_sequence.TotalEventPeriods)
			{
				blockAffected.Width = m_sequence.TotalEventPeriods - blockAffected.Left;
			}
			if (blockAffected.Bottom > m_sequence.ChannelCount)
			{
				blockAffected.Height = m_sequence.ChannelCount - blockAffected.Top;
			}
			byte[,] buffer = new byte[blockAffected.Height, blockAffected.Width];
			for (int i = 0; i < blockAffected.Height; i++)
			{
				int num3 = m_channelOrderMapping[blockAffected.Y + i];
				for (int j = 0; j < blockAffected.Width; j++)
				{
					buffer[i, j] = m_sequence.EventValues[num3, blockAffected.X + j];
				}
			}
			return buffer;
		}

		private int GetCellIntensity(int cellX, int cellY, out string intensityText)
		{
			if ((cellX >= 0) && (cellY >= 0))
			{
				int num;
				if (m_actualLevels)
				{
					num = m_sequence.EventValues[m_channelOrderMapping[cellY], cellX];
					intensityText = string.Format("{0}", num);
					return num;
				}
				num = (int) Math.Round((double) ((m_sequence.EventValues[m_channelOrderMapping[cellY], cellX] * 100f) / 255f), MidpointRounding.AwayFromZero);
				intensityText = string.Format("{0}%", num);
				return num;
			}
			intensityText = "";
			return 0;
		}

		private VixenPlus.Channel GetChannelAt(Point point)
		{
			return GetChannelAtSortedIndex(GetLineIndexAt(point));
		}

		private VixenPlus.Channel GetChannelAtSortedIndex(int index)
		{
			if (index < m_channelOrderMapping.Count)
			{
				return m_sequence.Channels[m_channelOrderMapping[index]];
			}
			return null;
		}

		private Rectangle GetChannelNameRect(VixenPlus.Channel channel)
		{
			if (channel != null)
			{
				return new Rectangle(0, ((GetChannelSortedIndex(channel) - vScrollBar1.Value) * m_gridRowHeight) + pictureBoxTime.Height, pictureBoxChannels.Width, m_gridRowHeight);
			}
			return Rectangle.Empty;
		}

		private int GetChannelNaturalIndex(VixenPlus.Channel channel)
		{
			return m_sequence.Channels.IndexOf(channel);
		}

		private int GetChannelSortedIndex(VixenPlus.Channel channel)
		{
			return m_channelOrderMapping.IndexOf(m_sequence.Channels.IndexOf(channel));
		}

		private Color GetGradientColor(Color startColor, Color endColor, int level)
		{
			float num = ((float) level) / 255f;
			int red = (int) (((endColor.R - startColor.R) * num) + startColor.R);
			int green = (int) (((endColor.G - startColor.G) * num) + startColor.G);
			int blue = (int) (((endColor.B - startColor.B) * num) + startColor.B);
			return Color.FromArgb(red, green, blue);
		}

		private int GetLineIndexAt(Point point)
		{
			return (((point.Y - pictureBoxTime.Height) / m_gridRowHeight) + vScrollBar1.Value);
		}

		private byte[,] GetRoutine()
		{
			List<string[]> list = new List<string[]>();
			RoutineSelectDialog dialog = new RoutineSelectDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string str;
				StreamReader reader = new StreamReader(dialog.SelectedRoutine);
				while ((str = reader.ReadLine()) != null)
				{
					list.Add(str.Trim().Split(new char[] { ' ' }));
				}
				reader.Close();
				reader.Dispose();
				int length = list[0].Length;
				int count = list.Count;
				byte[,] buffer = new byte[count, length];
				for (int i = 0; i < count; i++)
				{
					for (int j = 0; j < length; j++)
					{
						buffer[i, j] = Convert.ToByte((string) list[i][j]);
					}
				}
				dialog.Dispose();
				return buffer;
			}
			dialog.Dispose();
			return null;
		}

		private uint GetSampleMinMax(int startSample, int sampleCount, Sound sound, int bits, int channels)
		{
			int num10;
			int num12;
			short num = -32768;
			short num2 = 0x7fff;
			int num3 = startSample + sampleCount;
			int num4 = (bits >> 3) * channels;
			int num5 = startSample * num4;
			int num6 = num3 * num4;
			int length = num4 * sampleCount;
			byte[] destination = new byte[length];
			IntPtr zero = IntPtr.Zero;
			IntPtr ptr2 = IntPtr.Zero;
			uint num8 = 0;
			uint num9 = 0;
			sound.@lock((uint) num5, (uint) length, ref zero, ref ptr2, ref num8, ref num9);
			Marshal.Copy(zero, destination, 0, length);
			num5 = 0;
			if (bits == 0x10)
			{
				for (num12 = 0; num12 < sampleCount; num12++)
				{
					num10 = 0;
					while (num10 < channels)
					{
						short num11 = BitConverter.ToInt16(destination, num5 + (num10 * 2));
						num = Math.Max(num, num11);
						num2 = Math.Min(num2, num11);
						num10++;
					}
					num5 += num4;
				}
			}
			else if (bits == 8)
			{
				for (num12 = 0; num12 < sampleCount; num12++)
				{
					for (num10 = 0; num10 < channels; num10++)
					{
						sbyte num13 = (sbyte) destination[num5 + num10];
						num = Math.Max(num, num13);
						num2 = Math.Min(num2, num13);
					}
					num5 += num4;
				}
			}
			sound.unlock(zero, ptr2, num8, num9);
			return (uint) ((num << 0x10) | ((ushort) num2));
		}

		private Control GetTerminalSelectableControl()
		{
			Control activeControl = base.ActiveControl;
			while (activeControl is IContainerControl)
			{
				activeControl = (activeControl as IContainerControl).ActiveControl;
			}
			return activeControl;
		}

		private void halfSpeedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetAudioSpeed(0.5f);
		}

		private void hScrollBar1_ValueChanged(object sender, EventArgs e)
		{
			pictureBoxGrid.Refresh();
			pictureBoxTime.Refresh();
		}

		private void HScrollCheck()
		{
			if (m_periodPixelWidth != 0)
			{
				m_visibleEventPeriods = pictureBoxGrid.Width / m_periodPixelWidth;
				hScrollBar1.LargeChange = m_visibleEventPeriods;
				hScrollBar1.Maximum = Math.Max(0, m_sequence.TotalEventPeriods - 1);
				hScrollBar1.Enabled = m_visibleEventPeriods < m_sequence.TotalEventPeriods;
				if (!hScrollBar1.Enabled)
				{
					hScrollBar1.Value = hScrollBar1.Minimum;
				}
				else if ((hScrollBar1.Value + m_visibleEventPeriods) > m_sequence.TotalEventPeriods)
				{
					m_selectedRange.X += m_visibleEventPeriods - m_sequence.TotalEventPeriods;
					m_normalizedRange.X += m_visibleEventPeriods - m_sequence.TotalEventPeriods;
					hScrollBar1.Value = m_sequence.TotalEventPeriods - m_visibleEventPeriods;
				}
			}
			if (hScrollBar1.Maximum >= 0)
			{
				if (hScrollBar1.Value == -1)
				{
					hScrollBar1.Value = 0;
				}
				if (hScrollBar1.Minimum == -1)
				{
					hScrollBar1.Minimum = 0;
				}
			}
		}

		private void importChannelNamesListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m_sequence.Profile != null)
			{
				MessageBox.Show("Can't import channel names when attached to a profile.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				openFileDialog1.Filter = "Text file | *.txt";
				openFileDialog1.DefaultExt = "txt";
				openFileDialog1.InitialDirectory = Paths.ImportExportPath;
				openFileDialog1.FileName = string.Empty;
				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					StreamReader reader = new StreamReader(openFileDialog1.FileName);
					List<string> list = new List<string>();
					try
					{
						string str;
						while ((str = reader.ReadLine()) != null)
						{
							list.Add(str);
						}
						SetChannelCount(list.Count);
						for (int i = 0; i < list.Count; i++)
						{
							m_sequence.Channels[i].Name = list[i];
						}
						pictureBoxChannels.Refresh();
						MessageBox.Show("Channel name list import complete", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
					finally
					{
						reader.Close();
						reader.Dispose();
					}
				}
			}
		}

		private void Init()
		{
			m_initializing = true;
			InitializeComponent();
			m_initializing = false;
			int num = pictureBoxTime.Height - 10;
			m_gridRowHeight = m_preferences.GetInteger("MaxRowHeight");
			m_periodPixelWidth = m_preferences.GetInteger("MaxColumnWidth");
			m_showPositionMarker = m_preferences.GetBoolean("ShowPositionMarker");
			m_autoScrolling = m_preferences.GetBoolean("AutoScrolling");
			m_intensityLargeDelta = m_preferences.GetInteger("IntensityLargeDelta");
			m_showingGradient = !m_preferences.GetBoolean("BarLevels");
			m_channelBackBrush = new SolidBrush(Color.White);
			m_timeBackBrush = new SolidBrush(Color.FromArgb(0xff, 0xff, 0xe0));
			m_gridBackBrush = new SolidBrush(Color.FromArgb(0xc0, 0xc0, 0xc0));
			//m_arrowBitmap = new Bitmap(pictureBoxOutputArrow.Image);
			//m_arrowBitmap.MakeTransparent(m_arrowBitmap.GetPixel(0, 0));
			m_gridGraphics = pictureBoxGrid.CreateGraphics();
			m_dimmingShimmerGenerator = new FrequencyEffectGenerator(DimmingShimmerGenerator);
			m_sparkleGenerator = new FrequencyEffectGenerator(SparkleGenerator);
			m_intensityAdjustDialog = new IntensityAdjustDialog(m_actualLevels);
			m_intensityAdjustDialog.VisibleChanged += new EventHandler(m_intensityAdjustDialog_VisibleChanged);
			m_affectGrid = new AffectGridDelegate(AffectGrid);
			m_pluginCheckHandler = new EventHandler(plugInItem_CheckedChanged);
			m_channelOrderMapping = new List<int>();
			for (int i = 0; i < m_sequence.ChannelCount; i++)
			{
				m_channelOrderMapping.Add(i);
			}
			ReadFromSequence();
			m_executionContextHandle = m_executionInterface.RequestContext(true, false, this);
			toolStripComboBoxColumnZoom.SelectedIndex = toolStripComboBoxColumnZoom.Items.Count - 1;
			toolStripComboBoxRowZoom.SelectedIndex = toolStripComboBoxRowZoom.Items.Count - 1;
			SyncAudioButton();
			m_mouseWheelVerticalDelta = m_preferences.GetInteger("MouseWheelVerticalDelta");
			m_mouseWheelHorizontalDelta = m_preferences.GetInteger("MouseWheelHorizontalDelta");
			if (m_preferences.GetBoolean("SaveZoomLevels"))
			{
				int index = toolStripComboBoxColumnZoom.Items.IndexOf(m_preferences.GetChildString("SaveZoomLevels", "column"));
				if (index != -1)
				{
					toolStripComboBoxColumnZoom.SelectedIndex = index;
				}
				index = toolStripComboBoxRowZoom.Items.IndexOf(m_preferences.GetChildString("SaveZoomLevels", "row"));
				if (index != -1)
				{
					toolStripComboBoxRowZoom.SelectedIndex = index;
				}
			}
			if (m_sequence.WindowWidth != 0)
			{
				base.Width = m_sequence.WindowWidth;
			}
			if (m_sequence.WindowHeight != 0)
			{
				base.Height = m_sequence.WindowHeight;
			}
			if (m_sequence.ChannelWidth != 0)
			{
				splitContainer1.SplitterDistance = m_sequence.ChannelWidth;
			}
			toolStripComboBoxWaveformZoom.SelectedItem = "100%";
			SetDrawingLevel(m_sequence.MaximumLevel);
			m_executionInterface.SetSynchronousContext(m_executionContextHandle, m_sequence);
			UpdateLevelDisplay();
			base.IsDirty = false;
			pictureBoxChannels.AllowDrop = true;
		}

		

		private void InsertChannelIntoSort(int naturalIndex, int sortedIndex)
		{
			for (int i = 0; i < m_channelOrderMapping.Count; i++)
			{
				if (m_channelOrderMapping[i] >= naturalIndex)
				{
					List<int> list;
					int num2;
					(list = m_channelOrderMapping)[num2 = i] = list[num2] + 1;
				}
			}
			m_channelOrderMapping.Insert(sortedIndex, naturalIndex);
		}

		private void IntensityAdjustDialogCheck()
		{
			if (!m_intensityAdjustDialog.Visible)
			{
				m_intensityAdjustDialog.Show();
				base.BringToFront();
			}
		}

		private void InvalidateRect(Rectangle rect)
		{
			rect.X -= hScrollBar1.Value;
			rect.Y -= vScrollBar1.Value;
			m_selectionRectangle.X = Math.Min(rect.Left, rect.Right) * m_periodPixelWidth;
			m_selectionRectangle.Y = Math.Min(rect.Top, rect.Bottom) * m_gridRowHeight;
			m_selectionRectangle.Width = Math.Abs((int) ((rect.Width + 1) * m_periodPixelWidth));
			m_selectionRectangle.Height = Math.Abs((int) ((rect.Height + 1) * m_gridRowHeight));
			if (m_selectionRectangle.Width == 0)
			{
				m_selectionRectangle.Width = m_periodPixelWidth;
			}
			if (m_selectionRectangle.Height == 0)
			{
				m_selectionRectangle.Height = m_gridRowHeight;
			}
			pictureBoxGrid.Invalidate(m_selectionRectangle);
			pictureBoxGrid.Update();
		}

		private int LineIndexToChannelIndex(int lineIndex)
		{
			if ((lineIndex >= 0) && (lineIndex < m_channelOrderMapping.Count))
			{
				return m_channelOrderMapping[lineIndex];
			}
			return -1;
		}

		private void loadARoutineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			byte[,] routine = GetRoutine();
			if (routine != null)
			{
				AddUndoItem(new Rectangle(m_normalizedRange.X, m_normalizedRange.Y, routine.GetLength(1), routine.GetLength(0)), UndoOriginalBehavior.Overwrite);
				ArrayToCells(routine);
				pictureBoxGrid.Refresh();
			}
		}

		private void loadRoutineToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_systemInterface.Clipboard = GetRoutine();
		}

		private void LoadSequencePlugins()
		{
			m_systemInterface.VerifySequenceHardwarePlugins(m_sequence);
			int num = 0;
			bool flag = false;
			toolStripDropDownButtonPlugins.DropDownItems.Clear();
			foreach (XmlNode node in m_sequence.PlugInData.GetAllPluginData())
			{
				ToolStripMenuItem item = new ToolStripMenuItem(string.Format("{0} ({1}-{2})", node.Attributes["name"].Value, node.Attributes["from"].Value, node.Attributes["to"].Value));
				item.Checked = bool.Parse(node.Attributes["enabled"].Value);
				item.CheckOnClick = true;
				item.CheckedChanged += m_pluginCheckHandler;
				item.Tag = num.ToString();
				num++;
				toolStripDropDownButtonPlugins.DropDownItems.Add(item);
				flag |= item.Checked;
			}
			if (toolStripDropDownButtonPlugins.DropDownItems.Count > 0)
			{
				toolStripDropDownButtonPlugins.DropDownItems.Add("-");
				toolStripDropDownButtonPlugins.DropDownItems.Add("Select all", null, new EventHandler(selectAllToolStripMenuItem_Click));
				toolStripDropDownButtonPlugins.DropDownItems.Add("Unselect all", null, new EventHandler(unselectAllToolStripMenuItem_Click));
			}
		}

		private void LoadSequenceSorts()
		{
			toolStripComboBoxChannelOrder.BeginUpdate();
			string item = (string) toolStripComboBoxChannelOrder.Items[0];
			string str2 = (string) toolStripComboBoxChannelOrder.Items[toolStripComboBoxChannelOrder.Items.Count - 1];
			toolStripComboBoxChannelOrder.Items.Clear();
			toolStripComboBoxChannelOrder.Items.Add(item);
			foreach (VixenPlus.SortOrder order in m_sequence.Sorts)
			{
				toolStripComboBoxChannelOrder.Items.Add(order);
			}
			toolStripComboBoxChannelOrder.Items.Add(str2);
			if (m_sequence.LastSort != -1)
			{
				toolStripComboBoxChannelOrder.SelectedIndex = m_sequence.LastSort + 1;
			}
			toolStripComboBoxChannelOrder.EndUpdate();
		}

		private void m_intensityAdjustDialog_VisibleChanged(object sender, EventArgs e)
		{
			if (!m_intensityAdjustDialog.Visible)
			{
				int left;
				int num2;
				int num4;
				int num8;
				AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
				int delta = m_intensityAdjustDialog.Delta;
				int bottom = m_normalizedRange.Bottom;
				int right = m_normalizedRange.Right;
				if (m_actualLevels)
				{
					for (num8 = m_normalizedRange.Top; num8 < bottom; num8++)
					{
						num2 = m_channelOrderMapping[num8];
						if (m_sequence.Channels[num2].Enabled)
						{
							left = m_normalizedRange.Left;
							while (left < right)
							{
								num4 = m_sequence.EventValues[num2, left] + delta;
								num4 = Math.Max(Math.Min(num4, m_sequence.MaximumLevel), m_sequence.MinimumLevel);
								m_sequence.EventValues[num2, left] = (byte) num4;
								left++;
							}
						}
					}
				}
				else
				{
					for (num8 = m_normalizedRange.Top; num8 < bottom; num8++)
					{
						num2 = m_channelOrderMapping[num8];
						if (m_sequence.Channels[num2].Enabled)
						{
							for (left = m_normalizedRange.Left; left < right; left++)
							{
								num4 = ((int) Math.Round((double) ((m_sequence.EventValues[num2, left] * 100f) / 255f), MidpointRounding.AwayFromZero)) + delta;
								int num5 = (int) Math.Round((double) ((((float) num4) / 100f) * 255f), MidpointRounding.AwayFromZero);
								num5 = Math.Max(Math.Min(num5, m_sequence.MaximumLevel), m_sequence.MinimumLevel);
								m_sequence.EventValues[num2, left] = (byte) num5;
							}
						}
					}
				}
				pictureBoxGrid.Refresh();
			}
			else
			{
				m_intensityAdjustDialog.LargeDelta = m_intensityLargeDelta;
			}
		}

		private void m_positionTimer_Tick(object sender, EventArgs e)
		{
			int num;
			if (m_executionInterface.EngineStatus(m_executionContextHandle, out num) == 0)
			{
				ProgramEnded();
			}
			else
			{
				int num2 = num / m_sequence.EventPeriod;
				if (num2 != m_position)
				{
					toolStripLabelExecutionPoint.Text = string.Format("{0:d2}:{1:d2}", num / 0xea60, (num % 0xea60) / 0x3e8);
					m_previousPosition = m_position;
					m_position = num2;
					if ((m_position < hScrollBar1.Value) || (m_position > (hScrollBar1.Value + m_visibleEventPeriods)))
					{
						if (m_autoScrolling)
						{
							if (m_position != -1)
							{
								if (m_position >= m_sequence.TotalEventPeriods)
								{
									m_previousPosition = m_position = m_sequence.TotalEventPeriods - 1;
								}
								hScrollBar1.Value = m_position;
								toolStripLabelExecutionPoint.Text = string.Format("{0:d2}:{1:d2}", num / 0xea60, (num % 0xea60) / 0x3e8);
							}
						}
						else
						{
							UpdateProgress();
						}
					}
					else if (m_showPositionMarker)
					{
						UpdateProgress();
					}
					else
					{
						toolStripLabelExecutionPoint.Text = string.Format("{0:d2}:{1:d2}", num / 0xea60, (num % 0xea60) / 0x3e8);
					}
				}
			}
		}

		private void maxToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ArithmeticPaste(ArithmeticOperation.Max);
		}

		private void minToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ArithmeticPaste(ArithmeticOperation.Min);
		}

		public override EventSequence New()
		{
			return New(new EventSequence(m_systemInterface.UserPreferences));
		}

		public override EventSequence New(EventSequence seedSequence)
		{
			m_sequence = seedSequence;
			m_preferences = m_systemInterface.UserPreferences;
			m_dataNode = m_sequence.Extensions[FileExtension];
			Init();
			if (m_sequence.Name == null)
			{
				Text = "Unnamed sequence";
			}
			else
			{
				Text = m_sequence.Name;
			}
			return m_sequence;
		}

		private Rectangle NormalizeRect(Rectangle rect)
		{
			Rectangle rectangle = new Rectangle();
			rectangle.X = Math.Min(rect.Left, rect.Right);
			rectangle.Y = Math.Min(rect.Top, rect.Bottom);
			rectangle.Width = Math.Abs(rect.Width);
			rectangle.Height = Math.Abs(rect.Height);
			if (rect.Width < 0)
			{
				rectangle.Width++;
			}
			if (rect.Height < 0)
			{
				rectangle.Height++;
			}
			return rectangle;
		}

		private void normalSpeedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetAudioSpeed(1f);
		}

		private void normalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			normalToolStripMenuItem.Checked = true;
			paintFromClipboardToolStripMenuItem.Checked = false;
		}

		public override void Notify(Notification notification, object data)
		{
			switch (notification)
			{
				case Notification.PreferenceChange:
					UpdateRowHeight();
					UpdateColumnWidth();
					m_showPositionMarker = m_preferences.GetBoolean("ShowPositionMarker");
					m_autoScrolling = m_preferences.GetBoolean("AutoScrolling");
					m_mouseWheelVerticalDelta = m_preferences.GetInteger("MouseWheelVerticalDelta");
					m_mouseWheelHorizontalDelta = m_preferences.GetInteger("MouseWheelHorizontalDelta");
					m_intensityLargeDelta = m_preferences.GetInteger("IntensityLargeDelta");
					RefreshAll();
					break;

				case Notification.KeyDown:
					StandardSequence_KeyDown(null, (KeyEventArgs) data);
					break;

				case Notification.SequenceChange:
					RefreshAll();
					base.IsDirty = true;
					break;

				case Notification.ProfileChange:
				{
					VixenPlus.SortOrder currentOrder = m_sequence.Sorts.CurrentOrder;
					m_sequence.ReloadProfile();
					m_sequence.Sorts.CurrentOrder = currentOrder;
					LoadSequenceSorts();
					RefreshAll();
					break;
				}
			}
		}

		public override void OnDirtyChanged(EventArgs e)
		{
			base.OnDirtyChanged(e);
			tbsSave.Enabled = base.IsDirty;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			bool flag = (Control.ModifierKeys & Keys.Shift) == Keys.None;
			if (m_preferences.GetBoolean("FlipScrollBehavior"))
			{
				flag = !flag;
			}
			if (flag)
			{
				if (e.Delta > 0)
				{
					if (vScrollBar1.Value >= (vScrollBar1.Minimum + m_mouseWheelVerticalDelta))
					{
						vScrollBar1.Value -= m_mouseWheelVerticalDelta;
					}
					else
					{
						vScrollBar1.Value = vScrollBar1.Minimum;
					}
				}
				else if (vScrollBar1.Value <= (vScrollBar1.Maximum - (m_visibleRowCount + m_mouseWheelVerticalDelta)))
				{
					vScrollBar1.Value += m_mouseWheelVerticalDelta;
				}
				else
				{
					vScrollBar1.Value = Math.Max((vScrollBar1.Maximum - m_visibleRowCount) + 1, 0);
				}
			}
			else if (e.Delta > 0)
			{
				if (hScrollBar1.Value >= (hScrollBar1.Minimum + m_mouseWheelHorizontalDelta))
				{
					hScrollBar1.Value -= m_mouseWheelHorizontalDelta;
				}
				else
				{
					hScrollBar1.Value = hScrollBar1.Minimum;
				}
			}
			else if (hScrollBar1.Value <= (hScrollBar1.Maximum - (m_visibleEventPeriods + m_mouseWheelHorizontalDelta)))
			{
				hScrollBar1.Value += m_mouseWheelHorizontalDelta;
			}
			else
			{
				hScrollBar1.Value = Math.Max((hScrollBar1.Maximum - m_visibleEventPeriods) + 1, 0);
			}
		}

		public override EventSequence Open(string filePath)
		{
			m_sequence = new EventSequence(filePath);
			Text = m_sequence.Name;
			m_dataNode = m_sequence.Extensions[FileExtension];
			m_preferences = m_systemInterface.UserPreferences;
			Init();
			return m_sequence;
		}

		private void otherToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetVariablePlaybackSpeed(new Point(0, 0));
		}

		private void paintFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			normalToolStripMenuItem.Checked = false;
			paintFromClipboardToolStripMenuItem.Checked = true;
		}

		private void ParseAudioWaveform()
		{
			string str;
			if ((m_sequence.Audio != null) && File.Exists(str = Path.Combine(Paths.AudioPath, m_sequence.Audio.FileName)))
			{
				if (toolStripButtonWaveform.Checked)
				{
					VixenEditor.ProgressDialog dialog = new VixenEditor.ProgressDialog();
					dialog.Show();
					dialog.Message = "Parsing sound data, please wait...";
					Cursor = Cursors.WaitCursor;
					try
					{
						m_waveformPCMData = new uint[m_sequence.TotalEventPeriods * m_periodPixelWidth];
						m_waveformPixelData = new uint[m_sequence.TotalEventPeriods * m_periodPixelWidth];
						SOUND_TYPE rAW = SOUND_TYPE.RAW;
						SOUND_FORMAT nONE = SOUND_FORMAT.NONE;
						float volume = 0f;
						float pan = 0f;
						int priority = 0;
						Sound sound = null;
						int channels = 0;
						int bits = 0;
						float frequency = 0f;
						uint length = 0;
						fmod.GetInstance(-1).SystemObject.createSound(str, MODE._2D | MODE.HARDWARE | MODE.CREATESAMPLE, ref sound);
						sound.getFormat(ref rAW, ref nONE, ref channels, ref bits);
						sound.getDefaults(ref frequency, ref volume, ref pan, ref priority);
						sound.getLength(ref length, TIMEUNIT.PCMBYTES);
						double num8 = (((double) length) / ((double) channels)) / ((double) (bits / 8));
						double num9 = num8 / ((double) m_sequence.TotalEventPeriods);
						double num10 = num9 / ((double) m_periodPixelWidth);
						double num11 = ((double) m_sequence.EventPeriod) / ((double) m_periodPixelWidth);
						double num12 = frequency / 1000f;
						int num14 = 0;
						int num15 = 0;
						num14 = 0;
						num15 = 0;
						uint num18 = 0;
						sound.getLength(ref num18, TIMEUNIT.MS);
						int index = 0;
						for (double i = 0.0; (index < m_waveformPCMData.Length) && (i < num18); i += num11)
						{
							int startSample = (int) (i * num12);
							uint num16 = GetSampleMinMax(startSample, (int) Math.Min(num10, num8 - startSample), sound, bits, channels);
							num14 = Math.Max(num14, (short) (num16 >> 0x10));
							num15 = Math.Min(num15, (short) (num16 & 0xffff));
							m_waveformPCMData[index] = num16;
							index++;
						}
						m_waveform100PercentAmplitude = m_waveformMaxAmplitude = Math.Max(num14, -num15);
						PCMToPixels(m_waveformPCMData, m_waveformPixelData);
						sound.release();
					}
					finally
					{
						Cursor = Cursors.Default;
						dialog.Hide();
					}
				}
				else
				{
					m_waveformPCMData = null;
					m_waveformPixelData = null;
				}
				EnableWaveformButton();
			}
			else
			{
				DisableWaveformDisplay();
			}
		}

		private void pasteFullChannelEventsFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m_systemInterface.Clipboard != null)
			{
				AddUndoItem(new Rectangle(0, m_selectedLineIndex, m_systemInterface.Clipboard.GetLength(1), 1), UndoOriginalBehavior.Overwrite);
				int num2 = Math.Min(m_systemInterface.Clipboard.GetLength(1), m_sequence.TotalEventPeriods);
				for (int i = 0; i < num2; i++)
				{
					m_sequence.EventValues[m_editingChannelSortedIndex, i] = m_systemInterface.Clipboard[0, i];
				}
				base.IsDirty = true;
				pictureBoxGrid.Refresh();
			}
		}

		private void PasteOver()
		{
			if (m_systemInterface.Clipboard != null)
			{
				ArrayToCells(m_systemInterface.Clipboard);
				pictureBoxGrid.Refresh();
			}
		}

		private void PCMToPixels(uint[] PCMData, uint[] pixelData)
		{
			int length = PCMData.Length;
			short num3 = -32768;
			short num4 = 0x7fff;
			int waveformOffset = m_waveformOffset;
			int num6 = -m_waveformOffset;
			double num7 = ((double) m_waveformMaxAmplitude) / ((double) Math.Max(waveformOffset, num6));
			for (int i = 0; i < length; i++)
			{
				uint num2 = PCMData[i];
				num3 = (short) (((double) Math.Min((short) (num2 >> 0x10), m_waveformMaxAmplitude)) / num7);
				num3 = (short) Math.Max((short)num3, (short)0);
				num3 = (short) Math.Min(num3, waveformOffset);
				num4 = (short) (((double) Math.Max((short) (num2 & 0xffff), -m_waveformMaxAmplitude)) / num7);
				num4 = (short)Math.Min((short)num4, (short)0);
				num4 = (short) Math.Max(num4, num6);
				pixelData[i] = (uint) ((num3 << 0x10) | ((ushort) num4));
			}
		}

		private void pictureBoxChannels_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(VixenPlus.Channel)))
			{
				VixenPlus.Channel data = (VixenPlus.Channel) e.Data.GetData(typeof(VixenPlus.Channel));
				VixenPlus.Channel channelAt = GetChannelAt(pictureBoxChannels.PointToClient(new Point(e.X, e.Y)));
				if (data != channelAt)
				{
					if (e.Effect == DragDropEffects.Copy)
					{
						m_sequence.CopyChannel(data, channelAt);
						RefreshAll();
						base.IsDirty = true;
					}
					else if (e.Effect == DragDropEffects.Move)
					{
						int channelSortedIndex;
						int channelNaturalIndex = GetChannelNaturalIndex(data);
						m_channelOrderMapping.Remove(channelNaturalIndex);
						if (channelAt != null)
						{
							channelSortedIndex = GetChannelSortedIndex(channelAt);
						}
						else
						{
							channelSortedIndex = m_channelOrderMapping.Count;
						}
						m_channelOrderMapping.Insert(channelSortedIndex, channelNaturalIndex);
						RefreshAll();
						base.IsDirty = true;
					}
				}
			}
		}

		private void pictureBoxChannels_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(VixenPlus.Channel)))
			{
				Point point = pictureBoxChannels.PointToClient(new Point(e.X, e.Y));
				int lineIndexAt = GetLineIndexAt(point);
				if (vScrollBar1.Enabled)
				{
					if ((vScrollBar1.Value > vScrollBar1.Minimum) && ((lineIndexAt - vScrollBar1.Value) < 2))
					{
						vScrollBar1.Value--;
						Thread.Sleep(50);
						lineIndexAt = GetLineIndexAt(point);
					}
					else if ((vScrollBar1.Value < vScrollBar1.Maximum) && ((lineIndexAt - vScrollBar1.Value) > (m_visibleRowCount - 2)))
					{
						vScrollBar1.Value++;
						Thread.Sleep(50);
						lineIndexAt = GetLineIndexAt(point);
					}
				}
				if ((Control.ModifierKeys & Keys.Control) != Keys.None)
				{
					if ((lineIndexAt >= 0) && (lineIndexAt < m_sequence.ChannelCount))
					{
						e.Effect = DragDropEffects.Copy;
					}
					else
					{
						e.Effect = DragDropEffects.None;
					}
				}
				else
				{
					e.Effect = DragDropEffects.Move;
				}
			}
		}

		private void pictureBoxChannels_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
		}

		private void pictureBoxChannels_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (ChannelClickValid())
			{
				int num = 2;
				if (e.X < 10)
				{
					num = 0;
				}
				else if (m_showingOutputs && (e.X < 50))
				{
					num = 1;
				}
				switch (num)
				{
					case 1:
						ReorderChannelOutputs();
						break;

					case 2:
						ShowChannelProperties();
						break;
				}
			}
		}

		private void pictureBoxChannels_MouseDown(object sender, MouseEventArgs e)
		{
			ChannelClickValid();
			m_mouseDownAtInChannels = e.Location;
		}

		private void pictureBoxChannels_MouseMove(object sender, MouseEventArgs e)
		{
			if ((((e.Button & MouseButtons.Left) != MouseButtons.None) && (m_mouseDownAtInChannels != Point.Empty)) && ((Math.Abs((int) (e.X - m_mouseDownAtInChannels.X)) > 3) || (Math.Abs((int) (e.Y - m_mouseDownAtInChannels.Y)) > 3)))
			{
				base.DoDragDrop(SelectedChannel, DragDropEffects.Move | DragDropEffects.Copy);
			}
		}

		private void pictureBoxChannels_MouseUp(object sender, MouseEventArgs e)
		{
			m_mouseDownAtInChannels = Point.Empty;
		}

		private void pictureBoxChannels_Paint(object sender, PaintEventArgs e)
		{
			VixenPlus.Channel channel;
			int num2;
			int num3;
			int num5;
			e.Graphics.FillRectangle(m_channelBackBrush, e.Graphics.VisibleClipBounds);
			int height = pictureBoxTime.Height;
			if (toolStripComboBoxRowZoom.SelectedIndex <= 4)
			{
				num2 = 0;
			}
			else if (toolStripComboBoxRowZoom.SelectedIndex >= 8)
			{
				num2 = 3;
			}
			else
			{
				num2 = 1;
			}
			bool boolean = m_preferences.GetBoolean("ShowNaturalChannelNumber");
			int x = boolean ? (((m_sequence.ChannelCount.ToString().Length + 1) * 6) + 10) : 10;
			if (!m_showingOutputs)
			{
				for (num5 = vScrollBar1.Value; (num5 >= 0) && (num5 < m_sequence.ChannelCount); num5++)
				{
					num3 = m_channelOrderMapping[num5];
					channel = m_sequence.Channels[num3];
					if (channel == SelectedChannel)
					{
						e.Graphics.FillRectangle(SystemBrushes.Highlight, 0, height, pictureBoxChannels.Width, m_gridRowHeight);
					}
					else
					{
						e.Graphics.FillRectangle(channel.Brush, 0, height, pictureBoxChannels.Width, m_gridRowHeight);
					}
					if (boolean)
					{
						e.Graphics.DrawString(string.Format("{0}:", num3 + 1), m_channelNameFont, Brushes.Black, 10f, (float) (height + num2));
					}
					if (channel == SelectedChannel)
					{
						e.Graphics.DrawString(channel.Name, m_channelNameFont, SystemBrushes.HighlightText, (float) x, (float) (height + num2));
					}
					else
					{
						e.Graphics.DrawString(channel.Name, m_channelNameFont, Brushes.Black, (float) x, (float) (height + num2));
					}
					height += m_gridRowHeight;
				}
			}
			else
			{
				int num8;
				int width = Math.Min(m_gridRowHeight - 4, pictureBoxOutputArrow.Height);
				int num7 = (m_gridRowHeight - width) >> 1;
				SolidBrush brush = new SolidBrush(Color.White);
				if (boolean)
				{
					for (num5 = vScrollBar1.Value; (num5 >= 0) && (num5 < m_sequence.ChannelCount); num5++)
					{
						num3 = m_channelOrderMapping[num5];
						channel = m_sequence.Channels[num3];
						brush.Color = Color.FromArgb(0xc0, Color.Gray);
						if (channel == SelectedChannel)
						{
							e.Graphics.FillRectangle(SystemBrushes.Highlight, 0, height, pictureBoxChannels.Width, m_gridRowHeight);
						}
						else
						{
							e.Graphics.FillRectangle(channel.Brush, 0, height, pictureBoxChannels.Width, m_gridRowHeight);
						}
						e.Graphics.DrawString(string.Format("{0}:", num3 + 1), m_channelNameFont, Brushes.Black, 10f, (float) (height + num2));
						e.Graphics.FillRectangle(brush, x, height + 1, 40, m_gridRowHeight - 2);
						if (toolStripComboBoxRowZoom.SelectedIndex > 4)
						{
							e.Graphics.DrawRectangle(Pens.Black, x, height + 1, 40, m_gridRowHeight - 2);
						}
						e.Graphics.DrawImage(m_arrowBitmap, x + 2, height + num7, width, width);
						num8 = channel.OutputChannel + 1;
						e.Graphics.DrawString(num8.ToString(), m_channelNameFont, Brushes.Black, (float) (x + 0x10), (float) (height + num2));
						e.Graphics.DrawString(channel.Name, m_channelNameFont, Brushes.Black, (float) (x + 0x2c), (float) (height + num2));
						height += m_gridRowHeight;
					}
				}
				else
				{
					for (num5 = vScrollBar1.Value; (num5 >= 0) && (num5 < m_sequence.ChannelCount); num5++)
					{
						num3 = m_channelOrderMapping[num5];
						channel = m_sequence.Channels[num3];
						brush.Color = Color.FromArgb(0xc0, Color.Gray);
						e.Graphics.FillRectangle(channel.Brush, 0, height, pictureBoxChannels.Width, m_gridRowHeight);
						e.Graphics.FillRectangle(brush, 10, height + 1, 40, m_gridRowHeight - 2);
						if (toolStripComboBoxRowZoom.SelectedIndex > 4)
						{
							e.Graphics.DrawRectangle(Pens.Black, 10, height + 1, 40, m_gridRowHeight - 2);
						}
						e.Graphics.DrawImage(m_arrowBitmap, 12, height + num7, width, width);
						num8 = channel.OutputChannel + 1;
						e.Graphics.DrawString(num8.ToString(), m_channelNameFont, Brushes.Black, 26f, (float) (height + num2));
						e.Graphics.DrawString(channel.Name, m_channelNameFont, Brushes.Black, 54f, (float) (height + num2));
						height += m_gridRowHeight;
					}
				}
				brush.Dispose();
			}
			e.Graphics.FillRectangle(Brushes.White, 0, 0, 5, pictureBoxChannels.Height);
			if (m_mouseChannelCaret != -1)
			{
				e.Graphics.FillRectangle(m_channelCaretBrush, 0, ((m_mouseChannelCaret - vScrollBar1.Value) * m_gridRowHeight) + pictureBoxTime.Height, 5, m_gridRowHeight);
			}
		}

		private void pictureBoxChannels_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
		}

		private void pictureBoxChannels_Resize(object sender, EventArgs e)
		{
			pictureBoxChannels.Refresh();
		}

		private void pictureBoxGrid_DoubleClick(object sender, EventArgs e)
		{
			if (m_currentlyEditingChannel != null)
			{
				AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
				m_sequence.EventValues[m_editingChannelSortedIndex, m_normalizedRange.X] = (m_sequence.EventValues[m_editingChannelSortedIndex, m_normalizedRange.X] > m_sequence.MinimumLevel) ? m_sequence.MinimumLevel : m_drawingLevel;
				UpdateGrid(m_gridGraphics, new Rectangle((m_normalizedRange.X - hScrollBar1.Value) * m_periodPixelWidth, (m_editingChannelSortedIndex - vScrollBar1.Value) * m_gridRowHeight, m_periodPixelWidth, m_gridRowHeight));
			}
		}

		private void pictureBoxGrid_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				m_mouseDownInGrid = true;
                lblFollowMouse.Visible = true;
				m_mouseDownAtInGrid.X = (e.X / m_periodPixelWidth) + hScrollBar1.Value;
				m_mouseDownAtInGrid.Y = (e.Y / m_gridRowHeight) + vScrollBar1.Value;
				if (m_normalizedRange.Width != 0)
				{
					EraseSelectedRange();
				}
				if ((Control.ModifierKeys & Keys.Control) != Keys.None)
				{
					m_selectedLineIndex = (e.Y / m_gridRowHeight) + vScrollBar1.Value;
					m_editingChannelSortedIndex = m_channelOrderMapping[m_selectedLineIndex];
					m_currentlyEditingChannel = m_sequence.Channels[m_editingChannelSortedIndex];
					m_lineRect.X = m_mouseDownAtInGrid.X;
					m_lineRect.Y = m_mouseDownAtInGrid.Y;
					m_lineRect.Width = 0;
					m_lineRect.Height = 0;
					InvalidateRect(m_lineRect);
				}
				else if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
				{
					Rectangle rect = new Rectangle();
					rect.X = m_normalizedRange.X;
					rect.Y = m_normalizedRange.Y;
					rect.Width = ((hScrollBar1.Value + ((int) Math.Floor((double) (((float) e.X) / ((float) m_periodPixelWidth))))) - m_normalizedRange.Left) + 1;
					if (rect.Width < 0)
					{
						rect.Width--;
					}
					rect.Height = ((vScrollBar1.Value + (e.Y / m_gridRowHeight)) - m_normalizedRange.Top) + 1;
					if (rect.Height < 0)
					{
						rect.Height--;
					}
					if (rect.Bottom > m_sequence.ChannelCount)
					{
						rect.Height = m_sequence.ChannelCount - rect.Y;
					}
					if (rect.Right > m_sequence.TotalEventPeriods)
					{
						rect.Width = m_sequence.TotalEventPeriods - rect.X;
					}
					m_normalizedRange = NormalizeRect(rect);
					DrawSelectedRange();
				}
				else if ((((e.Y / m_gridRowHeight) + vScrollBar1.Value) < m_sequence.ChannelCount) && (((e.X / m_periodPixelWidth) + hScrollBar1.Value) < m_sequence.TotalEventPeriods))
				{
					m_selectedLineIndex = (e.Y / m_gridRowHeight) + vScrollBar1.Value;
					m_editingChannelSortedIndex = m_channelOrderMapping[m_selectedLineIndex];
					m_currentlyEditingChannel = m_sequence.Channels[m_editingChannelSortedIndex];
					m_selectedRange.X = hScrollBar1.Value + ((int) Math.Floor((double) (((float) e.X) / ((float) m_periodPixelWidth))));
					m_selectedRange.Y = m_selectedLineIndex;
					m_selectedRange.Width = 1;
					m_selectedRange.Height = 1;
					m_normalizedRange = m_selectedRange;
					DrawSelectedRange();
				}
				else
				{
					m_currentlyEditingChannel = null;
					m_editingChannelSortedIndex = -1;
					m_selectedLineIndex = -1;
				}
				UpdatePositionLabel(m_normalizedRange, false);
                UpdateFollowMouse(new Point(m_mouseDownAtInGrid.X,m_mouseDownAtInGrid.Y));
			}
		}

		private void pictureBoxGrid_MouseLeave(object sender, EventArgs e)
		{
			toolStripLabelCellIntensity.Text = string.Empty;
			toolStripLabelCurrentCell.Text = string.Empty;
		}

		private void pictureBoxGrid_MouseMove(object sender, MouseEventArgs e)
		{
			int num8;
			Rectangle rectangle;
			int cellX = e.X / m_periodPixelWidth;
			int cellY = e.Y / m_gridRowHeight;
			toolStripLabelCellIntensity.Text = string.Empty;
			toolStripLabelCurrentCell.Text = string.Empty;
			if (cellX < 0)
			{
				cellX = 0;
			}
			if (cellY < 0)
			{
				cellY = 0;
			}
			cellX += hScrollBar1.Value;
			cellY += vScrollBar1.Value;
			if (cellX >= m_sequence.TotalEventPeriods)
			{
				cellX = m_sequence.TotalEventPeriods - 1;
			}
			if (cellY >= m_sequence.ChannelCount)
			{
				cellY = m_sequence.ChannelCount - 1;
			}
			if ((e.Button != MouseButtons.Left) || !m_mouseDownInGrid)
			{
				goto Label_0733;
			}
			if (m_lineRect.Left == -1)
			{
				int num7 = 0;
				if (e.X > pictureBoxGrid.Width)
				{
					num7 |= 0x10;
				}
				else if (e.X < 0)
				{
					num7 |= 0x1000;
				}
				if (e.Y > pictureBoxGrid.Height)
				{
					num7 |= 1;
				}
				else if (e.Y < 0)
				{
					num7 |= 0x100;
				}
				switch (num7)
				{
					case 0x100:
						ScrollSelectionUp(cellX, cellY);
						goto Label_0715;

					case 0x1000:
						ScrollSelectionLeft(cellX, cellY);
						goto Label_0715;

					case 0x1100:
						ScrollSelectionLeft(cellX, cellY);
						ScrollSelectionUp(cellX, cellY);
						goto Label_0715;

					case 0:
						EraseSelectedRange();
						if (cellX >= m_mouseDownAtInGrid.X)
						{
							if (cellX > m_mouseDownAtInGrid.X)
							{
								m_selectedRange.Width = (cellX - m_selectedRange.Left) + 1;
							}
							else
							{
								m_selectedRange.Width = 1;
							}
						}
						else
						{
							m_selectedRange.Width = cellX - m_selectedRange.Left;
						}
						if (cellY < m_mouseDownAtInGrid.Y)
						{
							m_selectedRange.Height = cellY - m_selectedRange.Top;
						}
						else if (cellY > m_mouseDownAtInGrid.Y)
						{
							m_selectedRange.Height = (cellY - m_selectedRange.Top) + 1;
						}
						else
						{
							m_selectedRange.Height = 1;
						}
						m_normalizedRange = NormalizeRect(m_selectedRange);
						DrawSelectedRange();
						goto Label_0715;

					case 1:
						ScrollSelectionDown(cellX, cellY);
						goto Label_0715;

					case 0x10:
						ScrollSelectionRight(cellX, cellY);
						goto Label_0715;

					case 0x11:
						ScrollSelectionRight(cellX, cellY);
						ScrollSelectionDown(cellX, cellY);
						goto Label_0715;
				}
				goto Label_0715;
			}
			EraseRectangleEntity(m_lineRect);
			if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
			{
				int num5;
				int num6;
				int num3 = cellX - m_mouseDownAtInGrid.X;
				int num4 = cellY - m_mouseDownAtInGrid.Y;
				if (num3 >= 0)
				{
					if (num4 >= 0)
					{
						if (num4 < (num3 >> 1))
						{
							num5 = 4;
						}
						else if (num3 < (num4 >> 1))
						{
							num5 = 6;
						}
						else
						{
							num5 = 5;
						}
					}
					else if (num3 < -(num4 >> 1))
					{
						num5 = 2;
					}
					else if (num4 >= -(num3 >> 1))
					{
						num5 = 4;
					}
					else
					{
						num5 = 3;
					}
				}
				else if (num4 >= 0)
				{
					if (num4 < -(num3 >> 1))
					{
						num5 = 8;
					}
					else if (num3 >= -(num4 >> 1))
					{
						num5 = 6;
					}
					else
					{
						num5 = 7;
					}
				}
				else if (num4 >= (num3 >> 1))
				{
					num5 = 8;
				}
				else if (num3 >= (num4 >> 1))
				{
					num5 = 2;
				}
				else
				{
					num5 = 1;
				}
				if (Math.Abs(num3) < Math.Abs(num4))
				{
					num6 = Math.Abs(num3);
				}
				else
				{
					num6 = Math.Abs(num4);
				}
				switch (num5)
				{
					case 1:
						m_lineRect.Width = -num6;
						m_lineRect.Height = -num6;
						goto Label_0473;

					case 2:
						m_lineRect.Width = 0;
						m_lineRect.Height = num4;
						goto Label_0473;

					case 3:
						m_lineRect.Width = num6;
						m_lineRect.Height = -num6;
						goto Label_0473;

					case 4:
						m_lineRect.Width = num3;
						m_lineRect.Height = 0;
						goto Label_0473;

					case 5:
						m_lineRect.Width = num6;
						m_lineRect.Height = num6;
						goto Label_0473;

					case 6:
						m_lineRect.Width = 0;
						m_lineRect.Height = num4;
						goto Label_0473;

					case 7:
						m_lineRect.Width = -num6;
						m_lineRect.Height = num6;
						goto Label_0473;

					case 8:
						m_lineRect.Width = num3;
						m_lineRect.Height = 0;
						goto Label_0473;
				}
			}
			else
			{
				if (cellX < m_mouseDownAtInGrid.X)
				{
					m_lineRect.Width = cellX - m_lineRect.Left;
				}
				else if (cellX > m_mouseDownAtInGrid.X)
				{
					m_lineRect.Width = cellX - m_lineRect.Left;
				}
				else
				{
					m_lineRect.Width = 0;
				}
				if (cellY < m_mouseDownAtInGrid.Y)
				{
					m_lineRect.Height = cellY - m_lineRect.Top;
				}
				else if (cellY > m_mouseDownAtInGrid.Y)
				{
					m_lineRect.Height = cellY - m_lineRect.Top;
				}
				else
				{
					m_lineRect.Height = 0;
				}
			}
		Label_0473:
			InvalidateRect(m_lineRect);
			UpdatePositionLabel(NormalizeRect(new Rectangle(m_lineRect.X, m_lineRect.Y, m_lineRect.Width + 1, m_lineRect.Height)), true);
            //UpdateFollowMouse(e.Location);
			goto Label_0733;
		Label_0715:
			m_lastCellX = cellX;
			m_lastCellY = cellY;
			UpdatePositionLabel(m_normalizedRange, false);
            //UpdateFollowMouse(e.Location);
		Label_0733:
			num8 = 0;
			int num9 = 0;
			int y = 0;
			int num11 = 0;
			if ((cellX != m_mouseTimeCaret) || (cellY != m_mouseChannelCaret))
			{
				num8 = (Math.Min(cellX, m_mouseTimeCaret) - hScrollBar1.Value) * m_periodPixelWidth;
				num9 = ((Math.Max(cellX, m_mouseTimeCaret) - hScrollBar1.Value) + 1) * m_periodPixelWidth;
				y = (Math.Min(cellY, m_mouseChannelCaret) - vScrollBar1.Value) * m_gridRowHeight;
				num11 = ((Math.Max(cellY, m_mouseChannelCaret) - vScrollBar1.Value) + 1) * m_gridRowHeight;
			}
			if (cellY != m_mouseChannelCaret)
			{
				rectangle = new Rectangle(0, pictureBoxTime.Height + (m_gridRowHeight * (m_mouseChannelCaret - vScrollBar1.Value)), 5, m_gridRowHeight);
				m_mouseChannelCaret = -1;
				pictureBoxChannels.Invalidate(rectangle);
				pictureBoxChannels.Update();
				if (cellY < m_sequence.ChannelCount)
				{
					m_mouseChannelCaret = cellY;
					rectangle.Y = pictureBoxTime.Height + (m_gridRowHeight * (m_mouseChannelCaret - vScrollBar1.Value));
					pictureBoxChannels.Invalidate(rectangle);
					pictureBoxChannels.Update();
				}
				else
				{
					m_mouseChannelCaret = -1;
				}
			}
			if (cellX != m_mouseTimeCaret)
			{
				rectangle = new Rectangle(m_periodPixelWidth * (m_mouseTimeCaret - hScrollBar1.Value), 0, m_periodPixelWidth, 5);
				m_mouseTimeCaret = -1;
				pictureBoxTime.Invalidate(rectangle);
				pictureBoxTime.Update();
				if (cellX < m_sequence.TotalEventPeriods)
				{
					m_mouseTimeCaret = cellX;
					rectangle.X = m_periodPixelWidth * (m_mouseTimeCaret - hScrollBar1.Value);
					pictureBoxTime.Invalidate(rectangle);
					pictureBoxTime.Update();
				}
				else
				{
					m_mouseTimeCaret = -1;
				}
			}
			if (num8 != num9)
			{
				pictureBoxGrid.Invalidate(new Rectangle(num8, 0, num9 - num8, pictureBoxGrid.Height));
				pictureBoxGrid.Update();
				pictureBoxGrid.Invalidate(new Rectangle(0, y, pictureBoxGrid.Width, num11 - y));
				pictureBoxGrid.Update();
			}
			if ((cellX >= 0) && (cellY >= 0))
			{
				string str;
				GetCellIntensity(cellX, cellY, out str);
				toolStripLabelCellIntensity.Text = str;
				toolStripLabelCurrentCell.Text = string.Format("{0} , {1}", TimeString(cellX * m_sequence.EventPeriod), m_sequence.Channels[m_channelOrderMapping[cellY]].Name);
			}
            UpdateFollowMouse(new Point(e.X,e.Y/*e.X + hScrollBar1.Size.Height , e.Y + vScrollBar1.Size.Width*/));
		}

		private void pictureBoxGrid_MouseUp(object sender, MouseEventArgs e)
		{
			m_mouseDownInGrid = false;
            lblFollowMouse.Visible = false;
			if (m_lineRect.Left != -1)
			{
				bool flag = paintFromClipboardToolStripMenuItem.Checked && (m_systemInterface.Clipboard != null);
				if (flag)
				{
					Rectangle rect = NormalizeRect(m_lineRect);
					rect.Width += m_systemInterface.Clipboard.GetLength(1);
					EraseRectangleEntity(rect);
					rect.Width++;
					rect.Height++;
					AddUndoItem(rect, UndoOriginalBehavior.Overwrite);
				}
				else
				{
					EraseRectangleEntity(m_lineRect);
					Rectangle blockAffected = NormalizeRect(m_lineRect);
					blockAffected.Width++;
					blockAffected.Height++;
					AddUndoItem(blockAffected, UndoOriginalBehavior.Overwrite);
				}
				if (!flag)
				{
					BresenhamValues(m_lineRect);
				}
				else
				{
					byte[] brush = new byte[m_systemInterface.Clipboard.GetLength(1)];
					for (int i = 0; i < brush.Length; i++)
					{
						brush[i] = m_systemInterface.Clipboard[0, i];
					}
					BresenhamPaint(m_lineRect, brush);
				}
				m_lineRect.X = -1;
				UpdatePositionLabel(m_normalizedRange, false);
                //UpdateFollowMouse(e.Location);
			}
		}

		private void pictureBoxGrid_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle(m_gridBackBrush, e.ClipRectangle);
			if (m_sequence.ChannelCount != 0)
			{
				int num2 = 0;
				int num3 = 0;
				Point point = new Point();
				Point point2 = new Point();
				int num = e.ClipRectangle.Left / m_periodPixelWidth;
				point.Y = e.ClipRectangle.Top;
				point2.Y = Math.Min(e.ClipRectangle.Bottom, (m_sequence.ChannelCount - vScrollBar1.Value) * m_gridRowHeight);
				int num4 = Math.Min(e.ClipRectangle.Right, (m_sequence.TotalEventPeriods - hScrollBar1.Value) * m_periodPixelWidth);
				num2 = 0;
				while (num2 < num4)
				{
					num2 = m_periodPixelWidth * num;
					point.X = num2;
					point2.X = num2;
					e.Graphics.DrawLine(Pens.Gray, point, point2);
					num++;
				}
				num = e.ClipRectangle.Top / m_gridRowHeight;
				point.X = e.ClipRectangle.Left;
				point2.X = Math.Min(e.ClipRectangle.Right, (m_sequence.TotalEventPeriods - hScrollBar1.Value) * m_periodPixelWidth);
				num4 = Math.Min(e.ClipRectangle.Bottom, (m_sequence.ChannelCount - vScrollBar1.Value) * m_gridRowHeight);
				num3 = 0;
				while (num3 < num4)
				{
					num3 = m_gridRowHeight * num;
					point.Y = num3;
					point2.Y = num3;
					e.Graphics.DrawLine(Pens.Gray, point, point2);
					num++;
				}
				UpdateGrid(e.Graphics, e.ClipRectangle);
				if (m_positionTimer.Enabled)
				{
					num3 = Math.Min(e.ClipRectangle.Bottom, (m_sequence.ChannelCount - vScrollBar1.Value) * m_gridRowHeight);
					if ((m_previousPosition != -1) && (m_previousPosition >= hScrollBar1.Value))
					{
						num2 = (m_previousPosition - hScrollBar1.Value) * m_periodPixelWidth;
						e.Graphics.DrawLine(Pens.Gray, num2, 0, num2, num3);
					}
					num2 = (m_position - hScrollBar1.Value) * m_periodPixelWidth;
					e.Graphics.DrawLine(Pens.Yellow, num2, 0, num2, num3);
				}
				else
				{
					if (m_lineRect.Left != -1)
					{
						int num7 = ((m_lineRect.Left - hScrollBar1.Value) * m_periodPixelWidth) + (m_periodPixelWidth >> 1);
						int num8 = ((m_lineRect.Top - vScrollBar1.Value) * m_gridRowHeight) + (m_gridRowHeight >> 1);
						int num9 = num7 + (m_lineRect.Width * m_periodPixelWidth);
						int num10 = num8 + (m_lineRect.Height * m_gridRowHeight);
						e.Graphics.DrawLine(Pens.Blue, num7, num8, num9, num10);
					}
					else
					{
						if (m_normalizedRange.Left > m_sequence.TotalEventPeriods)
						{
							m_normalizedRange.Width = 0;
						}
						Rectangle range = Rectangle.Intersect(m_normalizedRange, Rectangle.FromLTRB(hScrollBar1.Value, vScrollBar1.Value, (hScrollBar1.Value + m_visibleEventPeriods) + 1, (vScrollBar1.Value + m_visibleRowCount) + 1));
						if (!range.IsEmpty)
						{
							e.Graphics.FillRectangle(m_selectionBrush, RangeToRectangle(range));
						}
					}
					if (toolStripButtonToggleCrossHairs.Checked)
					{
						int num11 = ((m_mouseTimeCaret - hScrollBar1.Value) * m_periodPixelWidth) + ((int) (m_periodPixelWidth * 0.5f));
						int num12 = ((m_mouseChannelCaret - vScrollBar1.Value) * m_gridRowHeight) + ((int) (m_gridRowHeight * 0.5f));
						if (((num11 > e.ClipRectangle.Left) && (num11 < e.ClipRectangle.Right)) || ((num12 > e.ClipRectangle.Top) && (num12 < e.ClipRectangle.Bottom)))
						{
							e.Graphics.DrawLine(Pens.Yellow, num11, 0, num11, base.Height);
							e.Graphics.DrawLine(Pens.Yellow, 0, num12, base.Width, num12);
						}
					}
				}
			}
		}

		private void pictureBoxGrid_Resize(object sender, EventArgs e)
		{
			if (!m_initializing)
			{
				VScrollCheck();
				HScrollCheck();
				pictureBoxGrid.Refresh();
			}
		}

		private void pictureBoxTime_Paint(object sender, PaintEventArgs e)
		{
			int x;
			int num2;
			e.Graphics.FillRectangle(m_timeBackBrush, e.ClipRectangle);
			Point point = new Point();
			Point point2 = new Point();
			point.Y = pictureBoxTime.Height - 20;
			point2.Y = pictureBoxTime.Height - 5;
			if ((e.ClipRectangle.Bottom >= point.Y) || (e.ClipRectangle.Top <= point2.Y))
			{
				num2 = x = e.ClipRectangle.X / m_periodPixelWidth;
				for (x *= m_periodPixelWidth; (x < e.ClipRectangle.Right) && ((num2 + hScrollBar1.Value) <= m_sequence.TotalEventPeriods); x += m_periodPixelWidth)
				{
					if (num2 != 0)
					{
						point.X = x;
						point2.X = x;
						e.Graphics.DrawLine(Pens.Black, point, point2);
					}
					num2++;
				}
			}
			point.Y = pictureBoxTime.Height - 30;
			if ((e.ClipRectangle.Bottom >= point.Y) || (e.ClipRectangle.Top <= point2.Y))
			{
				x = e.ClipRectangle.X;
				float eventPeriod = m_sequence.EventPeriod;
				float num4 = (hScrollBar1.Value + (((float) e.ClipRectangle.Left) / ((float) m_periodPixelWidth))) * eventPeriod;
				int num5 = Math.Min((int) ((hScrollBar1.Value + (e.ClipRectangle.Right / m_periodPixelWidth)) * eventPeriod), m_sequence.Time);
				float num6 = 1000f / ((float) m_sequence.EventPeriod);
				if (!((num4 % 1000f) == 0f))
				{
					num2 = (((int) num4) / 0x3e8) * 0x3e8;
				}
				else
				{
					num2 = (int) num4;
				}
				while ((x < e.ClipRectangle.Right) && (num2 <= num5))
				{
					if (num2 != 0)
					{
						string str;
						x = e.ClipRectangle.Left + ((int) (((num2 - num4) / 1000f) * (m_periodPixelWidth * num6)));
						point.X = x;
						point2.X = x;
						e.Graphics.DrawLine(Pens.Black, point, point2);
						point.X++;
						point2.X++;
						e.Graphics.DrawLine(Pens.Black, point, point2);
						if (num2 >= 0xea60)
						{
							str = string.Format("{0}:{1:d2}", num2 / 0xea60, (num2 % 0xea60) / 0x3e8);
						}
						else
						{
							str = string.Format(":{0:d2}", num2 / 0x3e8);
						}
						SizeF ef = e.Graphics.MeasureString(str, m_timeFont);
						e.Graphics.DrawString(str, m_timeFont, Brushes.Black, (float) (x - (ef.Width / 2f)), (float) ((point.Y - ef.Height) - 5f));
					}
					num2 += 0x3e8;
				}
			}
			point.Y = pictureBoxTime.Height - 0x23;
			point2.Y = pictureBoxTime.Height - 20;
			if (((e.ClipRectangle.Bottom >= point.Y) || (e.ClipRectangle.Top <= point2.Y)) && (m_showPositionMarker && (m_position != -1)))
			{
				x = m_periodPixelWidth * (m_position - hScrollBar1.Value);
				if (x < pictureBoxTime.Width)
				{
					point.X = x;
					point2.X = x;
					e.Graphics.DrawLine(Pens.Red, point, point2);
				}
			}
			if (m_mouseTimeCaret != -1)
			{
				e.Graphics.FillRectangle(m_channelCaretBrush, (m_mouseTimeCaret - hScrollBar1.Value) * m_periodPixelWidth, 0, m_periodPixelWidth, 5);
			}
			if (toolStripButtonWaveform.Checked)
			{
				int index = hScrollBar1.Value * m_periodPixelWidth;
				int num9 = Math.Min(index + ((m_visibleEventPeriods + 1) * m_periodPixelWidth), m_waveformPixelData.Length);
				int num7 = 0;
				while (index < num9)
				{
					e.Graphics.DrawLine(Pens.Blue, (float) num7, (float) (m_waveformOffset - (m_waveformPixelData[index] >> 0x10)), (float) num7, (float) (m_waveformOffset - ((short) (m_waveformPixelData[index] & 0xffff))));
					num7++;
					index++;
				}
			}
		}

		private void PlaceSparkle(byte[,] valueArray, int row, int startCol, int decayTime, byte minIntensity, byte maxIntensity)
		{
			int num = (int) (Math.Round((double) (1000f / ((float) m_sequence.EventPeriod)), MidpointRounding.AwayFromZero) * 0.1);
			int num2 = (int) Math.Round((double) (((float) decayTime) / ((float) m_sequence.EventPeriod)), MidpointRounding.AwayFromZero);
			if ((startCol + num) >= valueArray.GetLength(1))
			{
				num = valueArray.GetLength(1) - startCol;
			}
			if (((startCol + num) + num2) >= valueArray.GetLength(1))
			{
				num2 = (valueArray.GetLength(1) - startCol) - num;
				if (num2 < 0)
				{
					num2 = 0;
				}
			}
			int num3 = 0;
			while (num3 < num)
			{
				valueArray[row, startCol + num3] = maxIntensity;
				num3++;
			}
			if (num2 != 0)
			{
				byte num4 = (byte) ((maxIntensity - minIntensity) / num2);
				byte num5 = (byte) (maxIntensity - num4);
				num3 = startCol + num;
				while (--num2 > 0)
				{
					valueArray[row, num3++] = num5;
					num5 = (byte) (num5 - num4);
				}
			}
		}

		private void playAtTheSelectedPointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			playAtTheSelectedPointToolStripMenuItem.Checked = true;
			playOnlyTheSelectedRangeToolStripMenuItem.Checked = false;
			tsbPlayFrom.ToolTipText = "Play this sequence starting at the selection point (F6)";
		}

		private void playOnlyTheSelectedRangeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			playOnlyTheSelectedRangeToolStripMenuItem.Checked = true;
			playAtTheSelectedPointToolStripMenuItem.Checked = false;
			tsbPlayFrom.ToolTipText = "Play the selected range of this sequence (F6)";
		}

		private void plugInItem_CheckedChanged(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (ToolStripMenuItem) sender;
			m_sequence.PlugInData.GetPlugInData((string) item.Tag).Attributes["enabled"].Value = item.Checked.ToString();
			bool flag = false;
			foreach (ToolStripItem item2 in toolStripDropDownButtonPlugins.DropDownItems)
			{
				if (item2 is ToolStripMenuItem)
				{
					flag |= ((ToolStripMenuItem) item2).Checked;
				}
			}
			base.IsDirty = true;
		}

		private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
		{
			Font font = new Font("Arial", 16f, FontStyle.Bold);
			Font font2 = new Font("Arial", 12f, FontStyle.Bold);
			Font font3 = new Font("Arial", 10f, FontStyle.Bold);
			Font font4 = new Font("Arial", 10f);
			float top = e.MarginBounds.Top;
			float height = e.Graphics.MeasureString("Mg", font4).Height;
			SolidBrush brush = new SolidBrush(Color.FromArgb(0xe8, 0xe8, 0xe8));
			List<OutputPlugin> list = new List<OutputPlugin>(m_sequence.PlugInData.GetOutputPlugins());
			List<string> list2 = new List<string>();
			if (m_printingChannelIndex == 0)
			{
				e.Graphics.DrawString(m_sequence.Name, font, Brushes.Black, (float) e.MarginBounds.Left, top);
				top += e.Graphics.MeasureString("Mg", font).Height;
				e.Graphics.DrawString(DateTime.Today.ToShortDateString(), font2, Brushes.Black, (float) e.MarginBounds.Left, top);
				top += e.Graphics.MeasureString("Mg", font2).Height;
				Pen pen = new Pen(Brushes.Black, 3f);
				e.Graphics.DrawLine(pen, (float) e.MarginBounds.Left, top, (float) e.MarginBounds.Right, top);
				pen.Dispose();
				top += 20f;
				e.Graphics.FillRectangle(brush, (float) e.MarginBounds.Left, top, (float) e.MarginBounds.Width, 24f);
				e.Graphics.DrawRectangle(Pens.Black, (float) e.MarginBounds.Left, top, (float) e.MarginBounds.Width, 24f);
				top += 4f;
				e.Graphics.DrawString("Name", font3, Brushes.Black, (float) (e.MarginBounds.Left + 10), top);
				e.Graphics.DrawString("Output #", font3, Brushes.Black, (float) (e.MarginBounds.Left + 0x113), top);
				e.Graphics.DrawString("Controller", font3, Brushes.Black, (float) (e.MarginBounds.Left + 350), top);
				top += 30f;
			}
			while ((m_printingChannelIndex < m_sequence.ChannelCount) && ((top + height) < e.MarginBounds.Bottom))
			{
				if ((m_printingChannelIndex % 2) == 1)
				{
					e.Graphics.FillRectangle(brush, (float) e.MarginBounds.Left, top - 1f, (float) e.MarginBounds.Width, height);
				}
				int num3 = m_printingChannelList[m_printingChannelIndex].OutputChannel + 1;
				e.Graphics.DrawString(m_printingChannelList[m_printingChannelIndex].Name, font4, Brushes.Black, (float) (e.MarginBounds.Left + 10), top);
				e.Graphics.DrawString(num3.ToString(), font4, Brushes.Black, (float) (e.MarginBounds.Left + 0x113), top);
				list2.Clear();
				foreach (OutputPlugin plugin in list)
				{
					if ((plugin.Enabled && (plugin.ChannelFrom <= num3)) && (plugin.ChannelTo >= num3))
					{
						list2.Add(plugin.Name);
					}
				}
				e.Graphics.DrawString(string.Join(", ", list2.ToArray()), font4, Brushes.Black, (float) (e.MarginBounds.Left + 350), top);
				m_printingChannelIndex++;
				top += height;
			}
			brush.Dispose();
			font.Dispose();
			font2.Dispose();
			font3.Dispose();
			font4.Dispose();
			e.HasMorePages = m_printingChannelIndex < m_sequence.ChannelCount;
		}

		private void ProgramEnded()
		{
			m_positionTimer.Stop();
			SetEditingState(true);
			pictureBoxGrid.Refresh();
		}

		private void quarterSpeedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetAudioSpeed(0.25f);
		}

		private void Ramp(int startingLevel, int endingLevel)
		{
			AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
			int bottom = m_normalizedRange.Bottom;
			int right = m_normalizedRange.Right;
			int left = m_normalizedRange.Left;
			for (int i = m_normalizedRange.Top; i < bottom; i++)
			{
				int num6 = m_channelOrderMapping[i];
				if (m_sequence.Channels[num6].Enabled)
				{
					for (int j = left; j < right; j++)
					{
						byte minimumLevel = (byte) (((((float) (j - left)) / ((float) ((right - left) - 1))) * (endingLevel - startingLevel)) + startingLevel);
						if (minimumLevel < m_sequence.MinimumLevel)
						{
							minimumLevel = m_sequence.MinimumLevel;
						}
						else if (minimumLevel > m_sequence.MaximumLevel)
						{
							minimumLevel = m_sequence.MaximumLevel;
						}
						m_sequence.EventValues[num6, j] = minimumLevel;
					}
				}
			}
			m_selectionRectangle.Width = 0;
			pictureBoxGrid.Invalidate(SelectionToRectangle());
		}

		private Rectangle RangeToRectangle(Rectangle range)
		{
			Rectangle rectangle = new Rectangle();
			rectangle.X = ((range.X - hScrollBar1.Value) * m_periodPixelWidth) + 1;
			rectangle.Y = ((range.Y - vScrollBar1.Value) * m_gridRowHeight) + 1;
			rectangle.Width = (range.Width * m_periodPixelWidth) - 1;
			rectangle.Height = (range.Height * m_gridRowHeight) - 1;
			return rectangle;
		}

		private void ReactEditingStateToProfileAssignment()
		{
			bool flag = m_sequence.Profile != null;
			textBoxChannelCount.ReadOnly = flag;
			toolStripDropDownButtonPlugins.Enabled = !flag;
			toolStripButtonSaveOrder.Enabled = !flag;
			toolStripButtonChannelOutputMask.Enabled = !flag;
		}

		private void ReactToProfileAssignment()
		{
			//TODO: This is always false for a reason right now
			//bool flag = m_sequence.Profile != null;
			var flag = false;
			flattenProfileIntoSequenceToolStripMenuItem.Enabled = flag;
			detachSequenceFromItsProfileToolStripMenuItem.Enabled = flag;
			channelOutputMaskToolStripMenuItem.Enabled = !flag;
			ReactEditingStateToProfileAssignment();
			SetOrderArraySize(m_sequence.ChannelCount);
			textBoxChannelCount.Text = m_sequence.ChannelCount.ToString();
			VScrollCheck();
			pictureBoxChannels.Refresh();
			pictureBoxGrid.Refresh();
			LoadSequenceSorts();
			LoadSequencePlugins();
		}

		private void ReadFromSequence()
		{
			Text = m_sequence.Name;
			SetProgramTime(m_sequence.Time);
			ReactToProfileAssignment();
			pictureBoxChannels.Refresh();
			VScrollCheck();
		}

		private void Redo()
		{
			if (m_redoStack.Count != 0)
			{
				UndoItem item = (UndoItem) m_redoStack.Pop();
				int height = 0;
				int width = 0;
				if (item.Data != null)
				{
					height = item.Data.GetLength(0);
					width = item.Data.GetLength(1);
				}
				toolStripButtonRedo.Enabled = redoToolStripMenuItem.Enabled = m_redoStack.Count > 0;
				base.IsDirty = true;
				UndoItem item2 = new UndoItem(item.Location, GetAffectedBlockData(new Rectangle(item.Location.X, item.Location.Y, item.Data.GetLength(1), item.Data.GetLength(0))), item.Behavior, m_sequence, m_channelOrderMapping);
				switch (item.Behavior)
				{
					case UndoOriginalBehavior.Overwrite:
						DisjointedOverwrite(item.Location.X, item.Location.Y, item.Data, item.ReferencedChannels);
						pictureBoxGrid.Invalidate(new Rectangle((item.Location.X - hScrollBar1.Value) * m_periodPixelWidth, (item.Location.Y - vScrollBar1.Value) * m_gridRowHeight, width * m_periodPixelWidth, height * m_gridRowHeight));
						break;

					case UndoOriginalBehavior.Removal:
						DisjointedRemove(item.Location.X, item.Location.Y, width, height, item.ReferencedChannels);
						pictureBoxGrid.Refresh();
						break;

					case UndoOriginalBehavior.Insertion:
						DisjointedInsert(item.Location.X, item.Location.Y, width, height, item.ReferencedChannels);
						DisjointedOverwrite(item.Location.X, item.Location.Y, item.Data, item.ReferencedChannels);
						pictureBoxGrid.Refresh();
						break;
				}
				UpdateRedoText();
				m_undoStack.Push(item2);
				toolStripButtonUndo.Enabled = undoToolStripMenuItem.Enabled = true;
				UpdateUndoText();
			}
		}

		private void RefreshAll()
		{
			SetOrderArraySize(m_sequence.ChannelCount);
			textBoxChannelCount.Text = m_sequence.ChannelCount.ToString();
			textBoxProgramLength.Text = TimeString(m_sequence.Time);
			pictureBoxGrid.Refresh();
			pictureBoxChannels.Refresh();
			pictureBoxTime.Refresh();
			VScrollCheck();
			HScrollCheck();
		}

		private void ReorderChannelOutputs()
		{
			if (m_sequence.Profile != null)
			{
				MessageBox.Show("This sequence is attached to a profile.\nChanges made to the profile's channel outputs will be reflected here.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				ChannelOrderDialog dialog = new ChannelOrderDialog(m_sequence.OutputChannels, null);
				dialog.Text = "Channel Output Mapping";
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					List<VixenPlus.Channel> channelMapping = dialog.ChannelMapping;
					foreach (VixenPlus.Channel channel in m_sequence.Channels)
					{
						channel.OutputChannel = channelMapping.IndexOf(channel);
					}
					if (m_showingOutputs)
					{
						pictureBoxChannels.Refresh();
						pictureBoxGrid.Refresh();
					}
					base.IsDirty = true;
				}
				dialog.Dispose();
			}
		}

		private void reorderChannelOutputsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ReorderChannelOutputs();
		}

		private void Reset()
		{
			hScrollBar1.Value = hScrollBar1.Minimum;
			toolStripLabelExecutionPoint.Text = "00:00";
		}

		private void resetAllToolbarsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			VixenEditor.ToolStripManager.LoadSettings(this, m_preferences.XmlDoc.DocumentElement, "reset");
			foreach (ToolStripItem item in toolbarsToolStripMenuItem.DropDownItems)
			{
				if ((item is ToolStripMenuItem) && (item.Tag != null))
				{
					((ToolStripMenuItem) item).Checked = true;
				}
			}
		}

		public override DialogResult RunWizard(ref EventSequence resultSequence)
		{
			NewSequenceWizardDialog dialog = new NewSequenceWizardDialog(m_systemInterface.UserPreferences);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				resultSequence = dialog.Sequence;
				dialog.Dispose();
				return DialogResult.OK;
			}
			dialog.Dispose();
			return DialogResult.Cancel;
		}

		private void saveAsARoutineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result;
			string path = string.Empty;
			TextQueryDialog dialog = new TextQueryDialog(Vendor.ProductName, "Name of the routine", string.Empty);
			do
			{
				if ((result = dialog.ShowDialog()) == DialogResult.OK)
				{
					path = Path.Combine(Paths.RoutinePath, Path.GetFileNameWithoutExtension(dialog.Response) + ".vir");
					if (File.Exists(path))
					{
						result = MessageBox.Show("File already exists.  Overwrite?", Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					}
				}
			}
			while (result == DialogResult.No);
			dialog.Dispose();
			if (result != DialogResult.Cancel)
			{
				StreamWriter writer = new StreamWriter(path);
				byte[,] buffer = CellsToArray();
				for (int i = 0; i < buffer.GetLength(0); i++)
				{
					for (int j = 0; j < buffer.GetLength(1); j++)
					{
						writer.Write(buffer[i, j].ToString() + " ");
					}
					writer.WriteLine();
				}
				writer.Close();
				MessageBox.Show(string.Format("Routine \"{0}\" has been saved", Path.GetFileNameWithoutExtension(path)), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		public override void SaveTo(string filePath)
		{
			m_sequence.SaveTo(filePath);
			base.IsDirty = false;
		}

		private void saveToolbarPositionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			VixenEditor.ToolStripManager.SaveSettings(this, m_preferences.XmlDoc.DocumentElement);
			m_preferences.Flush();
			MessageBox.Show("Done.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ArithmeticPaste(ArithmeticOperation.Scale);
		}

		private void ScrollSelectionDown(int cellX, int cellY)
		{
			int num = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Left, pictureBoxGrid.Bottom)).Y - pictureBoxTime.Height;
			m_selectedRange.Width = (cellX + 1) - m_selectedRange.Left;
			while (Control.MousePosition.Y > num)
			{
				cellY = pictureBoxGrid.PointToClient(Control.MousePosition).Y / m_periodPixelWidth;
				cellY += vScrollBar1.Value;
				if (cellY >= (m_sequence.ChannelCount - 1))
				{
					m_normalizedRange.Height = m_selectedRange.Height = m_sequence.ChannelCount - m_selectedRange.Y;
					if (cellX != m_lastCellX)
					{
						pictureBoxGrid.Refresh();
					}
				}
				if (vScrollBar1.Value > (vScrollBar1.Maximum - vScrollBar1.LargeChange))
				{
					break;
				}
				if (vScrollBar1.Value < (vScrollBar1.Maximum - vScrollBar1.LargeChange))
				{
					m_selectedRange.Height++;
					m_normalizedRange.Height++;
				}
				vScrollBar1.Value++;
			}
		}

		private void ScrollSelectionLeft(int cellX, int cellY)
		{
			int x = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Left, pictureBoxGrid.Top)).X;
			while ((Control.MousePosition.X < x) && (hScrollBar1.Value != 0))
			{
				m_selectedRange.Height = (cellY + 1) - m_selectedRange.Top;
				m_selectedRange.Width--;
				m_normalizedRange = NormalizeRect(m_selectedRange);
				hScrollBar1.Value--;
			}
		}

		private void ScrollSelectionRight(int cellX, int cellY)
		{
			int x = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Right, pictureBoxGrid.Top)).X;
			m_selectedRange.Height = (cellY + 1) - m_selectedRange.Top;
			while (Control.MousePosition.X > x)
			{
				cellX = pictureBoxGrid.PointToClient(Control.MousePosition).X / m_periodPixelWidth;
				cellX += hScrollBar1.Value;
				if (cellX >= (m_sequence.TotalEventPeriods - 1))
				{
					m_normalizedRange.Width = m_selectedRange.Width = m_sequence.TotalEventPeriods - m_selectedRange.X;
					if (cellY != m_lastCellY)
					{
						pictureBoxGrid.Refresh();
					}
				}
				if (hScrollBar1.Value > (hScrollBar1.Maximum - hScrollBar1.LargeChange))
				{
					break;
				}
				if (hScrollBar1.Value < (hScrollBar1.Maximum - hScrollBar1.LargeChange))
				{
					m_selectedRange.Width++;
					m_normalizedRange.Width++;
				}
				hScrollBar1.Value++;
			}
		}

		private void ScrollSelectionUp(int cellX, int cellY)
		{
			int y = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Left, pictureBoxGrid.Top)).Y;
			while ((Control.MousePosition.Y < y) && (vScrollBar1.Value != 0))
			{
				m_selectedRange.Width = (cellX + 1) - m_selectedRange.Left;
				m_selectedRange.Height--;
				m_normalizedRange = NormalizeRect(m_selectedRange);
				vScrollBar1.Value--;
			}
		}

		private bool SelectableControlFocused()
		{
			Control terminalSelectableControl = GetTerminalSelectableControl();
			return ((terminalSelectableControl != null) && terminalSelectableControl.CanSelect);
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ToolStripItem item in toolStripDropDownButtonPlugins.DropDownItems)
			{
				if (!(item is ToolStripMenuItem))
				{
					break;
				}
				((ToolStripMenuItem) item).Checked = true;
			}
			if (toolStripDropDownButtonPlugins.DropDownItems.Count > 3)
			{
			}
		}

		private Rectangle SelectionToRectangle()
		{
			Rectangle rectangle = new Rectangle();
			rectangle.X = ((m_normalizedRange.X - hScrollBar1.Value) * m_periodPixelWidth) + 1;
			rectangle.Y = ((m_normalizedRange.Y - vScrollBar1.Value) * m_gridRowHeight) + 1;
			rectangle.Width = (m_normalizedRange.Width * m_periodPixelWidth) - 1;
			rectangle.Height = (m_normalizedRange.Height * m_gridRowHeight) - 1;
			return rectangle;
		}

		private void setAllChannelColorsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m_sequence.Profile != null)
			{
				MessageBox.Show("This sequence is attached to a profile.\nChanges to the profile affect all sequences attached to it.\n\nIf this is what you want to do, please use the profile manager.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				AllChannelsColorDialog dialog = new AllChannelsColorDialog(m_sequence.Channels);
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					int channelCount = m_sequence.ChannelCount;
					List<Color> channelColors = dialog.ChannelColors;
					for (int i = 0; i < channelCount; i++)
					{
						m_sequence.Channels[i].Color = channelColors[i];
					}
				}
				dialog.Dispose();
				base.IsDirty = true;
				pictureBoxChannels.Refresh();
				pictureBoxGrid.Refresh();
			}
		}

		private void SetAudioSpeed(float rate)
		{
			if ((rate > 0f) && (rate <= 1f))
			{
				xToolStripMenuItem.Checked = false;
				xToolStripMenuItem1.Checked = false;
				xToolStripMenuItem2.Checked = false;
				normalToolStripMenuItem1.Checked = false;
				otherToolStripMenuItem.Checked = false;
				SpeedQtrTsb.Checked = false;
				SpeedHalfTsb.Checked = false;
				SpeedThreeQtrTsb.Checked = false;
				SpeedNormalTsb.Checked = false;
				SpeedVariableTsb.Checked = false;
				if (rate == 0.25f)
				{
					xToolStripMenuItem.Checked = true;
					SpeedQtrTsb.Checked = true;
				}
				else if (rate == 0.5f)
				{
					xToolStripMenuItem1.Checked = true;
					SpeedHalfTsb.Checked = true;
				}
				else if (rate == 0.75f)
				{
					xToolStripMenuItem2.Checked = true;
					SpeedThreeQtrTsb.Checked = true;
				}
				else if (rate == 1f)
				{
					normalToolStripMenuItem1.Checked = true;
					SpeedNormalTsb.Checked = true;
				}
				else
				{
					otherToolStripMenuItem.Checked = true;
					SpeedVariableTsb.Checked = true;
				}
				m_executionInterface.SetAudioSpeed(m_executionContextHandle, rate);
			}
		}

		private void SetChannelCount(int count)
		{
			if (count != m_sequence.ChannelCount)
			{
				int num;
				bool flag = false;
				int num2 = Math.Min(m_sequence.ChannelCount, count);
				for (num = 0; num < num2; num++)
				{
					if (m_sequence.Channels[num].OutputChannel > (count - 1))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (MessageBox.Show("With the new channel count, some channels would refer to outputs that no longer exist.\nTo keep the sequence valid, channel outputs would have to be reset.\n\nDo you want to keep the new channel count?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
					{
						textBoxChannelCount.Text = m_sequence.ChannelCount.ToString();
						return;
					}
					for (num = 0; num < m_sequence.ChannelCount; num++)
					{
						m_sequence.Channels[num].OutputChannel = num;
					}
				}
				SetOrderArraySize(count);
				m_sequence.ChannelCount = count;
				textBoxChannelCount.Text = count.ToString();
				VScrollCheck();
				pictureBoxChannels.Refresh();
				pictureBoxGrid.Refresh();
				base.IsDirty = true;
				MessageBox.Show("Channel count has been updated.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void SetDrawingLevel(byte level)
		{
			m_drawingLevel = level;
			if (m_actualLevels)
			{
				toolStripLabelCurrentDrawingIntensity.Text = level.ToString();
			}
			else
			{
				toolStripLabelCurrentDrawingIntensity.Text = string.Format("{0}%", (int) Math.Round((double) ((level * 100f) / 255f), MidpointRounding.AwayFromZero));
			}
		}

		private void SetEditingState(bool state)
		{
			if (state)
			{
				EnableWaveformButton();
			}
			else
			{
				DisableWaveformButton();
			}
			toolStripEditing.Enabled = state;
			toolStripEffect.Enabled = state;
			toolStripSequenceSettings.Enabled = state;
			toolStripDropDownButtonPlugins.Enabled = state;
			toolStripDisplaySettings.Enabled = state;
			ReactEditingStateToProfileAssignment();
		}

		private void SetOrderArraySize(int count)
		{
			if (count < m_channelOrderMapping.Count)
			{
				List<int> list = new List<int>();
				list.AddRange(m_channelOrderMapping);
				foreach (int num in list)
				{
					if (num >= count)
					{
						m_channelOrderMapping.Remove(num);
					}
				}
			}
			else
			{
				for (int i = m_channelOrderMapping.Count; i < count; i++)
				{
					m_channelOrderMapping.Add(i);
				}
			}
		}

		private void SetProfile(string filePath)
		{
			if (filePath != null)
			{
				SetProfile(new Profile(openFileDialog1.FileName));
			}
			else
			{
				SetProfile((Profile) null);
			}
		}

		private void SetProfile(Profile profile)
		{
			m_sequence.Profile = profile;
			ReactToProfileAssignment();
			base.IsDirty = true;
		}

		private bool SetProgramTime(int milliseconds)
		{
			try
			{
				m_sequence.Time = milliseconds;
			}
			catch
			{
				MessageBox.Show("Cannot set the sequence length.\nThere is audio associated which would exceed that length.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				textBoxProgramLength.Text = TimeString(m_sequence.Time);
				return false;
			}
			textBoxProgramLength.Text = TimeString(m_sequence.Time);
			HScrollCheck();
			pictureBoxTime.Refresh();
			pictureBoxGrid.Refresh();
			return true;
		}

		private void SetVariablePlaybackSpeed(Point dialogScreenCoords)
		{
			AudioSpeedDialog dialog = new AudioSpeedDialog();
			if ((dialogScreenCoords.X == 0) && (dialogScreenCoords.Y == 0))
			{
				dialog.StartPosition = FormStartPosition.CenterScreen;
			}
			else
			{
				dialog.StartPosition = FormStartPosition.Manual;
				dialog.Location = dialogScreenCoords;
			}
			dialog.Rate = m_executionInterface.GetAudioSpeed(m_executionContextHandle);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				SetAudioSpeed(dialog.Rate);
			}
			dialog.Dispose();
		}

		private void ShowChannelProperties()
		{
			List<VixenPlus.Channel> channels = new List<VixenPlus.Channel>();
			channels.AddRange(m_sequence.Channels);
			for (int i = 0; i < channels.Count; i++)
			{
				channels[i] = m_sequence.Channels[m_channelOrderMapping[i]];
			}
			ChannelPropertyDialog dialog = new ChannelPropertyDialog(channels, SelectedChannel, true);
			dialog.ShowDialog();
			dialog.Dispose();
			pictureBoxChannels.Refresh();
			pictureBoxGrid.Refresh();
			base.IsDirty = true;
		}

		private void sortByChannelNumberToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_printingChannelList = m_sequence.Channels;
			if (printDialog.ShowDialog() == DialogResult.OK)
			{
				printDocument.DocumentName = "Vixen channel configuration";
				m_printingChannelIndex = 0;
				printDocument.Print();
			}
		}

		private void sortByChannelOutputToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_printingChannelList = new List<VixenPlus.Channel>();
			VixenPlus.Channel[] collection = new VixenPlus.Channel[m_sequence.ChannelCount];
			foreach (VixenPlus.Channel channel in m_sequence.Channels)
			{
				collection[channel.OutputChannel] = channel;
			}
			m_printingChannelList.AddRange(collection);
			if (printDialog.ShowDialog() == DialogResult.OK)
			{
				printDocument.DocumentName = "Vixen channel configuration";
				m_printingChannelIndex = 0;
				printDocument.Print();
			}
		}

		private void SparkleGenerator(byte[,] values, params int[] effectParameters)
		{
			int num = 0x3e8 / m_sequence.EventPeriod;
			int maxValue = num - effectParameters[0];
			int decayTime = effectParameters[1];
			for (int i = 0; i < values.GetLength(0); i++)
			{
				for (int k = 0; k < values.GetLength(1); k++)
				{
					values[i, k] = m_sequence.MinimumLevel;
				}
			}
			int num6 = (int) Math.Round((double) (((float) decayTime) / ((float) m_sequence.EventPeriod)), MidpointRounding.AwayFromZero);
			int length = values.GetLength(0);
			int num8 = values.GetLength(1);
			int[] numArray = new int[num8];
			int index = 0;
			Random random = new Random();
			while (index < num8)
			{
				numArray[index] = random.Next(length) + 1;
				int num10 = random.Next(maxValue);
				index += Math.Max(num10, 1);
			}
			for (int j = 0; j < numArray.Length; j++)
			{
				if (numArray[j] != 0)
				{
					PlaceSparkle(values, numArray[j] - 1, j, decayTime, (byte) effectParameters[2], (byte) effectParameters[3]);
				}
			}
		}

		private void StandardSequence_Activated(object sender, EventArgs e)
		{
			base.ActiveControl = m_lastSelectableControl;
		}

		private void StandardSequence_Deactivate(object sender, EventArgs e)
		{
			m_lastSelectableControl = GetTerminalSelectableControl();
		}

		private void StandardSequence_FormClosing(object sender, FormClosingEventArgs e)
		{
			if ((e.CloseReason == CloseReason.UserClosing) && (CheckDirty() == DialogResult.Cancel))
			{
				e.Cancel = true;
			}
			else
			{
				if (m_executionInterface.EngineStatus(m_executionContextHandle) != 0)
				{
					toolStripButtonStop_Click(null, null);
				}
				if (m_preferences.GetBoolean("SaveZoomLevels"))
				{
					m_preferences.SetChildString("SaveZoomLevels", "row", toolStripComboBoxRowZoom.SelectedItem.ToString());
					m_preferences.SetChildString("SaveZoomLevels", "column", toolStripComboBoxColumnZoom.SelectedItem.ToString());
					m_preferences.Flush();
				}
				m_sequence.UpdateMetrics(base.Width, base.Height, splitContainer1.SplitterDistance);
				m_executionInterface.ReleaseContext(m_executionContextHandle);
			}
		}

		private void StandardSequence_KeyDown(object sender, KeyEventArgs e)
		{
			int num;
			bool flag = m_executionInterface.EngineStatus(m_executionContextHandle) == 1;
			bool flag2 = m_normalizedRange.Width > 0;
			switch (e.KeyCode)
			{
				case Keys.Prior:
					if (vScrollBar1.Value > 0)
					{
						num = Math.Min(m_visibleRowCount, vScrollBar1.Value);
						m_selectedRange.Y -= num;
						m_normalizedRange.Y -= num;
						vScrollBar1.Value -= num;
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.Next:
					if (vScrollBar1.Value < (m_sequence.ChannelCount - m_visibleRowCount))
					{
						int num2 = Math.Min(m_sequence.ChannelCount - vScrollBar1.Value, m_visibleRowCount);
						int num3 = Math.Min(m_sequence.ChannelCount - m_normalizedRange.Bottom, m_visibleRowCount);
						num = Math.Min(num2, num3);
						m_selectedRange.Y += num;
						m_normalizedRange.Y += num;
						vScrollBar1.Value += num;
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.End:
					if (hScrollBar1.Value < (m_sequence.TotalEventPeriods - m_visibleEventPeriods))
					{
						int num4 = m_sequence.TotalEventPeriods - m_visibleEventPeriods;
						m_selectedRange.X = num4;
						m_normalizedRange.X = num4;
						hScrollBar1.Value = num4;
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.Home:
					if (hScrollBar1.Value > 0)
					{
						m_selectedRange.X = 0;
						m_normalizedRange.X = 0;
						hScrollBar1.Value = 0;
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.F5:
				{
					if (!e.Alt)
					{
						break;
					}
					DelayedStartDialog dialog = new DelayedStartDialog();
					if (dialog.ShowDialog() == DialogResult.OK)
					{
						dialog.Dispose();
						e.Handled = true;
						break;
					}
					dialog.Dispose();
					return;
				}
				case Keys.F6:
					if (tsbPlayFrom.Enabled)
					{
						toolStripButtonPlayPoint_Click(null, null);
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.F7:
					if (tsbPause.Enabled)
					{
						toolStripButtonPause_Click(null, null);
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.F8:
					if (tsbStop.Enabled)
					{
						toolStripButtonStop_Click(null, null);
						e.Handled = true;
					}
					goto Label_0355;

				default:
					goto Label_0355;
			}
			if (tsbPlay.Enabled)
			{
				toolStripButtonPlay_Click(null, null);
				e.Handled = true;
			}
		Label_0355:
			if ((((Control.ModifierKeys & Keys.Control) != Keys.None) && (e.KeyCode >= Keys.D0)) && (e.KeyCode <= Keys.D9))
			{
				int index = ((int) e.KeyCode) - 0x30;
				if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
				{
					m_bookmarks[index] = (m_bookmarks[index] == m_normalizedRange.Left) ? -1 : m_normalizedRange.Left;
					pictureBoxTime.Refresh();
				}
				else if (m_bookmarks[index] != -1)
				{
					hScrollBar1.Value = m_bookmarks[index];
				}
				return;
			}
			if (!flag2)
			{
				goto Label_0EBD;
			}
			switch (e.KeyCode)
			{
				case Keys.Space:
				{
					int num6;
					int num7;
					if ((SelectableControlFocused() && !pictureBoxChannels.Focused) && !pictureBoxGrid.Focused)
					{
						goto Label_0C71;
					}
					if (m_executionInterface.EngineStatus(m_executionContextHandle, out num7) != 1)
					{
						int left;
						int num13;
						AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
						int num10 = m_normalizedRange.Height * m_normalizedRange.Width;
						int num11 = 0;
						for (num13 = m_normalizedRange.Top; num13 < m_normalizedRange.Bottom; num13++)
						{
							num6 = m_channelOrderMapping[num13];
							left = m_normalizedRange.Left;
							while (left < m_normalizedRange.Right)
							{
								if (m_sequence.EventValues[num6, left] > m_sequence.MinimumLevel)
								{
									num11++;
								}
								left++;
							}
						}
						byte num14 = (num11 == num10) ? m_sequence.MinimumLevel : m_drawingLevel;
						for (num13 = m_normalizedRange.Top; num13 < m_normalizedRange.Bottom; num13++)
						{
							num6 = m_channelOrderMapping[num13];
							for (left = m_normalizedRange.Left; left < m_normalizedRange.Right; left++)
							{
								m_sequence.EventValues[num6, left] = num14;
							}
						}
						pictureBoxGrid.Invalidate(new Rectangle((m_normalizedRange.Left - hScrollBar1.Value) * m_periodPixelWidth, (m_normalizedRange.Top - vScrollBar1.Value) * m_gridRowHeight, m_normalizedRange.Width * m_periodPixelWidth, m_normalizedRange.Height * m_gridRowHeight));
						e.Handled = true;
						goto Label_0C71;
					}
					int x = num7 / m_sequence.EventPeriod;
					AddUndoItem(new Rectangle(x, m_normalizedRange.Top, 1, m_normalizedRange.Height), UndoOriginalBehavior.Overwrite);
					for (int i = m_normalizedRange.Top; i < m_normalizedRange.Bottom; i++)
					{
						num6 = m_channelOrderMapping[i];
						m_sequence.EventValues[num6, x] = m_drawingLevel;
					}
					pictureBoxGrid.Invalidate(new Rectangle((x - hScrollBar1.Value) * m_periodPixelWidth, (m_normalizedRange.Top - vScrollBar1.Value) * m_gridRowHeight, m_periodPixelWidth, m_normalizedRange.Height * m_gridRowHeight));
					return;
				}
				case Keys.Left:
					if (!pictureBoxChannels.Focused && !pictureBoxGrid.Focused)
					{
						goto Label_0C71;
					}
					if ((hScrollBar1.Value > 0) || (m_normalizedRange.Left > 0))
					{
						m_selectedRange.X--;
						m_normalizedRange.X--;
						if ((m_normalizedRange.Left + 1) <= hScrollBar1.Value)
						{
							hScrollBar1.Value--;
							break;
						}
						pictureBoxGrid.Invalidate(new Rectangle((m_normalizedRange.Left - hScrollBar1.Value) * m_periodPixelWidth, (m_normalizedRange.Top - vScrollBar1.Value) * m_gridRowHeight, (m_normalizedRange.Width + 1) * m_periodPixelWidth, m_normalizedRange.Height * m_gridRowHeight));
					}
					break;

				case Keys.Up:
					if ((pictureBoxChannels.Focused || (pictureBoxGrid.Focused && !e.Control)) && ((vScrollBar1.Value > 0) || (m_normalizedRange.Top > 0)))
					{
						m_selectedRange.Y--;
						m_normalizedRange.Y--;
						if ((m_normalizedRange.Top + 1) <= vScrollBar1.Value)
						{
							vScrollBar1.Value--;
						}
						else
						{
							pictureBoxGrid.Invalidate(new Rectangle((m_normalizedRange.Left - hScrollBar1.Value) * m_periodPixelWidth, (m_normalizedRange.Top - vScrollBar1.Value) * m_gridRowHeight, m_normalizedRange.Width * m_periodPixelWidth, (m_normalizedRange.Height + 1) * m_gridRowHeight));
						}
					}
					e.Handled = true;
					goto Label_0C71;

				case Keys.Right:
					if (pictureBoxChannels.Focused || pictureBoxGrid.Focused)
					{
						if (m_normalizedRange.Right < m_sequence.TotalEventPeriods)
						{
							m_selectedRange.X++;
							m_normalizedRange.X++;
							if (((m_normalizedRange.Right - 1) - hScrollBar1.Value) >= m_visibleEventPeriods)
							{
								hScrollBar1.Value++;
							}
							else
							{
								pictureBoxGrid.Invalidate(new Rectangle(((m_normalizedRange.Left - hScrollBar1.Value) - 1) * m_periodPixelWidth, (m_normalizedRange.Top - vScrollBar1.Value) * m_gridRowHeight, (m_normalizedRange.Width + 1) * m_periodPixelWidth, m_normalizedRange.Height * m_gridRowHeight));
							}
						}
						e.Handled = true;
					}
					goto Label_0C71;

				case Keys.Down:
					if ((pictureBoxChannels.Focused || (pictureBoxGrid.Focused && !e.Control)) && (m_normalizedRange.Bottom < m_sequence.ChannelCount))
					{
						m_selectedRange.Y++;
						m_normalizedRange.Y++;
						if (((m_normalizedRange.Bottom - 1) - vScrollBar1.Value) >= m_visibleRowCount)
						{
							vScrollBar1.Value++;
						}
						else
						{
							pictureBoxGrid.Invalidate(new Rectangle((m_normalizedRange.Left - hScrollBar1.Value) * m_periodPixelWidth, ((m_normalizedRange.Top - vScrollBar1.Value) - 1) * m_gridRowHeight, m_normalizedRange.Width * m_periodPixelWidth, (m_normalizedRange.Height + 1) * m_gridRowHeight));
						}
					}
					e.Handled = true;
					goto Label_0C71;

				default:
					goto Label_0C71;
			}
			e.Handled = true;
		Label_0C71:
			if (!flag && pictureBoxGrid.Focused)
			{
				if ((((e.KeyCode < Keys.A) || (e.KeyCode > Keys.Z)) || ((Control.ModifierKeys & Keys.Control) != Keys.None)) || ((Control.ModifierKeys & Keys.Alt) != Keys.None))
				{
					switch (e.KeyCode)
					{
						case Keys.Up:
							if (e.Control)
							{
								IntensityAdjustDialogCheck();
								m_intensityAdjustDialog.Delta = e.Alt ? 1 : m_intensityLargeDelta;
							}
							e.Handled = true;
							goto Label_0EBD;

						case Keys.Right:
							goto Label_0EBD;

						case Keys.Down:
							if (e.Control)
							{
								IntensityAdjustDialogCheck();
								m_intensityAdjustDialog.Delta = e.Alt ? -1 : -m_intensityLargeDelta;
							}
							e.Handled = true;
							goto Label_0EBD;
					}
				}
				else
				{
					switch (e.KeyCode)
					{
						case Keys.A:
							toolStripButtonRandom_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.B:
						case Keys.C:
						case Keys.D:
						case Keys.G:
						case Keys.U:
							goto Label_0EBD;

						case Keys.E:
							toolStripButtonShimmerDimming_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.F:
							if (!e.Shift)
							{
								toolStripButtonRampOff_Click(null, null);
							}
							else
							{
								toolStripButtonPartialRampOff_Click(null, null);
							}
							e.Handled = true;
							goto Label_0EBD;

						case Keys.H:
							toolStripButtonMirrorHorizontal_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.I:
							toolStripButtonIntensity_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.R:
							if (!e.Shift)
							{
								toolStripButtonRampOn_Click(null, null);
							}
							else
							{
								toolStripButtonPartialRampOn_Click(null, null);
							}
							e.Handled = true;
							goto Label_0EBD;

						case Keys.S:
							toolStripButtonSparkle_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.T:
							toolStripButtonInvert_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.V:
							toolStripButtonMirrorVertical_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;
					}
				}
			}
		Label_0EBD:
			if (!(flag || (!pictureBoxChannels.Focused && !pictureBoxGrid.Focused)))
			{
				int channelSortedIndex;
				switch (e.KeyCode)
				{
					case Keys.Insert:
						if (SelectedChannel != null)
						{
							channelSortedIndex = GetChannelSortedIndex(SelectedChannel);
							int naturalIndex = m_sequence.InsertChannel(channelSortedIndex);
							InsertChannelIntoSort(naturalIndex, channelSortedIndex);
							ChannelCountChanged();
						}
						e.Handled = true;
						return;

					case Keys.Delete:
						if (!e.Shift)
						{
							if (SelectedChannel != null)
							{
								ClearChannel(GetChannelSortedIndex(SelectedChannel));
							}
						}
						else if ((SelectedChannel != null) && (MessageBox.Show(string.Format("Delete channel {0}?", SelectedChannel.Name), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
						{
							channelSortedIndex = GetChannelSortedIndex(SelectedChannel);
							m_sequence.DeleteChannel(SelectedChannel.Id);
							DeleteChannelFromSort(channelSortedIndex);
							ChannelCountChanged();
						}
						e.Handled = true;
						return;

					case Keys.D6:
						if (e.Shift && (SelectedChannel != null))
						{
							FillChannel(GetChannelSortedIndex(SelectedChannel));
						}
						e.Handled = true;
						return;
				}
			}
		}

		private void StandardSequence_Load(object sender, EventArgs e)
		{
			VixenEditor.ToolStripManager.SaveSettings(this, m_preferences.XmlDoc.DocumentElement, "reset");
			m_preferences.Flush();
			VixenEditor.ToolStripManager.LoadSettings(this, m_preferences.XmlDoc.DocumentElement);
			ToolStripPanel[] panelArray = new ToolStripPanel[] { toolStripContainer1.TopToolStripPanel, toolStripContainer1.BottomToolStripPanel, toolStripContainer1.LeftToolStripPanel, toolStripContainer1.RightToolStripPanel };
			List<string> list = new List<string>();
			foreach (ToolStripPanel panel in panelArray)
			{
				foreach (ToolStrip strip in panel.Controls)
				{
					m_toolStrips[strip.Text] = strip;
					list.Add(strip.Text);
				}
			}
			list.Sort();

			//this populates the toolstrip menu with the attached toolstrips
			var position = 4; //todo this should be resolved dynamically
			foreach (string str in list)
			{
				ToolStripMenuItem item = new ToolStripMenuItem(str);
				item.Tag = m_toolStrips[str];
				item.Checked = m_toolStrips[str].Visible;
				item.CheckOnClick = true;
				item.CheckStateChanged += m_toolStripCheckStateChangeHandler;
				toolbarsToolStripMenuItem.DropDownItems.Insert(position++, item);
			}
			m_actualLevels = m_preferences.GetBoolean("ActualLevels");
            UpdateToolbarMenu();
			UpdateLevelDisplay();
		}

		private void subtractionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ArithmeticPaste(ArithmeticOperation.Subtraction);
		}

		private void SyncAudioButton()
		{
			tsbAudio.Checked = m_sequence.Audio != null;
			tsbAudio.ToolTipText = (m_sequence.Audio != null) ? m_sequence.Audio.Name : "Add audio";
		}

		private void textBoxChannelCount_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				e.Handled = true;
				int result = 0;
				if (int.TryParse(textBoxChannelCount.Text, out result))
				{
					if (result < m_sequence.ChannelCount)
					{
						if (MessageBox.Show("This will reduce the number of channels and potentially lose data.\n\nAccept new channel count?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							SetChannelCount(result);
						}
						else
						{
							textBoxChannelCount.Text = m_sequence.ChannelCount.ToString();
						}
					}
					else if (result > m_sequence.ChannelCount)
					{
						SetChannelCount(result);
					}
				}
				else
				{
					textBoxChannelCount.Text = m_sequence.ChannelCount.ToString();
					MessageBox.Show("Please provide a valid number.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		private void textBoxProgramLength_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				int num2;
				int num3;
				int num4;
				e.Handled = true;
				string s = "0";
				string str2 = string.Empty;
				string str3 = "0";
				string text = textBoxProgramLength.Text;
				int index = text.IndexOf(':');
				if (index != -1)
				{
					s = text.Substring(0, index).Trim();
					text = text.Substring(index + 1);
				}
				index = text.IndexOf('.');
				if (index != -1)
				{
					str3 = text.Substring(index + 1).Trim();
					text = text.Substring(0, index);
				}
				str2 = text;
				try
				{
					num2 = int.Parse(s);
				}
				catch
				{
					num2 = 0;
				}
				try
				{
					num3 = int.Parse(str2);
				}
				catch
				{
					num3 = 0;
				}
				try
				{
					num4 = int.Parse(str3);
				}
				catch
				{
					num4 = 0;
				}
				num4 = (num4 + (num3 * 0x3e8)) + (num2 * 0xea60);
				if (num4 == 0)
				{
					textBoxProgramLength.Text = TimeString(m_sequence.Time);
					MessageBox.Show("Not a valid format for time.\nUse one of the following:\n\nSeconds\nMinutes:Seconds\nSeconds.Milliseconds\nMinutes:Seconds.Milliseconds", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else if (SetProgramTime(num4))
				{
					base.IsDirty = true;
					MessageBox.Show("Sequence length has been updated.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private string TimeString(int milliseconds)
		{
			return string.Format("{0:d2}:{1:d2}.{2:d3}", milliseconds / 0xea60, (milliseconds % 0xea60) / 0x3e8, milliseconds % 0x3e8);
		}

		private void toggleOutputChannelsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_showingOutputs = !m_showingOutputs;
			pictureBoxChannels.Refresh();
		}

		private void toolStripButtonAudio_Click(object sender, EventArgs e)
		{
			Audio audio = m_sequence.Audio;
			int integer = m_preferences.GetInteger("SoundDevice");
			Cursor = Cursors.WaitCursor;
			AudioDialog dialog = new AudioDialog(m_sequence, m_preferences.GetBoolean("EventSequenceAutoSize"), integer);
			Cursor = Cursors.Default;
			Audio audio2 = m_sequence.Audio;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				SetProgramTime(m_sequence.Time);
				base.IsDirty = true;
				pictureBoxGrid.Refresh();
			}
			SyncAudioButton();
			base.IsDirty |= audio2 != m_sequence.Audio;
			if (audio != m_sequence.Audio)
			{
				ParseAudioWaveform();
				pictureBoxTime.Refresh();
			}
			dialog.Dispose();
		}

		private void toolStripButtonChangeIntensity_Click(object sender, EventArgs e)
		{
			DrawingIntensityDialog dialog = new DrawingIntensityDialog(m_sequence, m_drawingLevel, m_actualLevels);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				SetDrawingLevel(dialog.SelectedIntensity);
			}
			dialog.Dispose();
		}

		private void toolStripButtonChannelOutputMask_Click(object sender, EventArgs e)
		{
			EditSequenceChannelMask();
		}

		private void toolStripButtonCopy_Click(object sender, EventArgs e)
		{
			CopyCells();
		}

		private void toolStripButtonCut_Click(object sender, EventArgs e)
		{
			CopyCells();
			TurnCellsOff();
		}

		private void toolStripButtonDeleteOrder_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(string.Format("Delete channel order '{0}'?", toolStripComboBoxChannelOrder.Text), Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				m_sequence.Sorts.Remove((VixenPlus.SortOrder) toolStripComboBoxChannelOrder.SelectedItem);
				toolStripComboBoxChannelOrder.Items.RemoveAt(toolStripComboBoxChannelOrder.SelectedIndex);
				toolStripButtonDeleteOrder.Enabled = false;
				base.IsDirty = true;
			}
		}

		private void toolStripButtonFindAndReplace_Click(object sender, EventArgs e)
		{
			if ((m_normalizedRange.Width == 0) || (m_normalizedRange.Height == 0))
			{
				MessageBox.Show("There are no cells to search", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				FindAndReplaceDialog dialog = new FindAndReplaceDialog(m_sequence.MinimumLevel, m_sequence.MaximumLevel, m_actualLevels);
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					int left;
					int num4;
					int num5;
					AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
					byte findValue = dialog.FindValue;
					byte replaceWithValue = dialog.ReplaceWithValue;
					if (m_actualLevels)
					{
						for (num5 = m_normalizedRange.Top; num5 < m_normalizedRange.Bottom; num5++)
						{
							num4 = m_channelOrderMapping[num5];
							left = m_normalizedRange.Left;
							while (left < m_normalizedRange.Right)
							{
								if (m_sequence.EventValues[num4, left] == findValue)
								{
									m_sequence.EventValues[num4, left] = replaceWithValue;
								}
								left++;
							}
						}
					}
					else
					{
						for (num5 = m_normalizedRange.Top; num5 < m_normalizedRange.Bottom; num5++)
						{
							num4 = m_channelOrderMapping[num5];
							for (left = m_normalizedRange.Left; left < m_normalizedRange.Right; left++)
							{
								if (((byte) Math.Round((double) ((m_sequence.EventValues[num4, left] * 100f) / 255f), MidpointRounding.AwayFromZero)) == findValue)
								{
									m_sequence.EventValues[num4, left] = (byte) Math.Round((double) ((((float) replaceWithValue) / 100f) * 255f), MidpointRounding.AwayFromZero);
								}
							}
						}
					}
					base.IsDirty = true;
					pictureBoxGrid.Refresh();
				}
				dialog.Dispose();
			}
		}

		private void toolStripButtonInsertPaste_Click(object sender, EventArgs e)
		{
			if (m_systemInterface.Clipboard != null)
			{
				byte[,] clipboard = m_systemInterface.Clipboard;
				int length = clipboard.GetLength(0);
				int width = clipboard.GetLength(1);
				int left = m_normalizedRange.Left;
				int num5 = left + width;
				int num6 = m_sequence.TotalEventPeriods - num5;
				for (int i = 0; (i < length) && ((m_normalizedRange.Top + i) < m_sequence.ChannelCount); i++)
				{
					int num7 = m_channelOrderMapping[m_normalizedRange.Top + i];
					for (int j = Math.Min(num6, m_sequence.TotalEventPeriods - m_normalizedRange.Left) - 1; j >= 0; j--)
					{
						m_sequence.EventValues[num7, num5 + j] = m_sequence.EventValues[num7, left + j];
					}
				}
				PasteOver();
				AddUndoItem(new Rectangle(m_normalizedRange.X, m_normalizedRange.Y, width, length), UndoOriginalBehavior.Insertion);
			}
		}

		private void toolStripButtonIntensity_Click(object sender, EventArgs e)
		{
			int result = 0;
			bool flag = false;
			while (!flag)
			{
				string str;
				TextQueryDialog dialog;
				flag = true;
				if (m_actualLevels)
				{
					str = "255";
					if ((m_normalizedRange.Width == 1) && (m_normalizedRange.Height == 1))
					{
						str = m_sequence.EventValues[m_channelOrderMapping[m_normalizedRange.Top], m_normalizedRange.Left].ToString();
					}
					dialog = new TextQueryDialog(Vendor.ProductName, "What intensity level (0-255)?", str);
					if (dialog.ShowDialog() != DialogResult.OK)
					{
						dialog.Dispose();
						return;
					}
					if (!int.TryParse(dialog.Response, out result))
					{
						MessageBox.Show("Not a valid number.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						flag = false;
					}
					if ((result < 0) || (result > 0xff))
					{
						MessageBox.Show("Not a valid value.\nPlease select a value between 0 and 255.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						flag = false;
					}
					dialog.Dispose();
				}
				else
				{
					str = "100";
					if ((m_normalizedRange.Width == 1) && (m_normalizedRange.Height == 1))
					{
						str = ((int) Math.Round((double) ((m_sequence.EventValues[m_channelOrderMapping[m_normalizedRange.Top], m_normalizedRange.Left] * 100f) / 255f), MidpointRounding.AwayFromZero)).ToString();
					}
					dialog = new TextQueryDialog(Vendor.ProductName, "What % intensity (0-100)?", str);
					if (dialog.ShowDialog() != DialogResult.OK)
					{
						dialog.Dispose();
						return;
					}
					try
					{
						result = (int) Math.Round((double) ((Convert.ToSingle(dialog.Response) * 255f) / 100f), MidpointRounding.AwayFromZero);
						if ((result < 0) || (result > 0xff))
						{
							MessageBox.Show("Not a valid value.\nPlease select a value between 0 and 100.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							flag = false;
						}
					}
					catch
					{
						MessageBox.Show("Not a valid number.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						flag = false;
					}
					finally
					{
						dialog.Dispose();
					}
				}
			}
			AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
			int bottom = m_normalizedRange.Bottom;
			int right = m_normalizedRange.Right;
			for (int i = m_normalizedRange.Top; i < bottom; i++)
			{
				int num5 = m_channelOrderMapping[i];
				if (m_sequence.Channels[num5].Enabled)
				{
					for (int j = m_normalizedRange.Left; j < right; j++)
					{
						m_sequence.EventValues[num5, j] = (byte) result;
					}
				}
			}
			m_selectionRectangle.Width = 0;
			pictureBoxGrid.Invalidate(SelectionToRectangle());
		}

		private void toolStripButtonInvert_Click(object sender, EventArgs e)
		{
			AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
			int bottom = m_normalizedRange.Bottom;
			int right = m_normalizedRange.Right;
			for (int i = m_normalizedRange.Top; i < bottom; i++)
			{
				int num4 = m_channelOrderMapping[i];
				if (m_sequence.Channels[num4].Enabled)
				{
					for (int j = m_normalizedRange.Left; j < right; j++)
					{
						m_sequence.EventValues[num4, j] = (byte) (m_sequence.MaximumLevel - m_sequence.EventValues[num4, j]);
					}
				}
			}
			m_selectionRectangle.Width = 0;
			pictureBoxGrid.Invalidate(SelectionToRectangle());
		}

		private void toolStripButtonLoop_CheckedChanged(object sender, EventArgs e)
		{
			m_executionInterface.SetLoopState(m_executionContextHandle, tsbLoop.Checked);
		}

		private void toolStripButtonMirrorHorizontal_Click(object sender, EventArgs e)
		{
			byte[,] buffer = new byte[m_normalizedRange.Height, m_normalizedRange.Width];
			for (int i = 0; i < m_normalizedRange.Height; i++)
			{
				int num3 = m_channelOrderMapping[m_normalizedRange.Top + i];
				int num2 = 0;
				int num = m_normalizedRange.Width - 1;
				while (num >= 0)
				{
					buffer[i, num2] = m_sequence.EventValues[num3, m_normalizedRange.Left + num];
					num--;
					num2++;
				}
			}
			m_systemInterface.Clipboard = buffer;
		}

		private void toolStripButtonMirrorVertical_Click(object sender, EventArgs e)
		{
			byte[,] buffer = new byte[m_normalizedRange.Height, m_normalizedRange.Width];
			int num2 = 0;
			int num4 = m_normalizedRange.Height - 1;
			while (num4 >= 0)
			{
				int num3 = m_channelOrderMapping[m_normalizedRange.Top + num4];
				for (int i = 0; i < m_normalizedRange.Width; i++)
				{
					buffer[num2, i] = m_sequence.EventValues[num3, m_normalizedRange.Left + i];
				}
				num4--;
				num2++;
			}
			m_systemInterface.Clipboard = buffer;
		}

		private void toolStripButtonOff_Click(object sender, EventArgs e)
		{
			TurnCellsOff();
		}

		private void toolStripButtonOn_Click(object sender, EventArgs e)
		{
			AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
			int bottom = m_normalizedRange.Bottom;
			int right = m_normalizedRange.Right;
			for (int i = m_normalizedRange.Top; i < bottom; i++)
			{
				int num4 = m_channelOrderMapping[i];
				if (m_sequence.Channels[num4].Enabled)
				{
					for (int j = m_normalizedRange.Left; j < right; j++)
					{
						m_sequence.EventValues[num4, j] = m_drawingLevel;
					}
				}
			}
			m_selectionRectangle.Width = 0;
			pictureBoxGrid.Invalidate(SelectionToRectangle());
		}

		private void toolStripButtonOpaquePaste_Click(object sender, EventArgs e)
		{
			if (m_systemInterface.Clipboard != null)
			{
				AddUndoItem(new Rectangle(m_normalizedRange.X, m_normalizedRange.Y, m_systemInterface.Clipboard.GetLength(1), m_systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite);
				PasteOver();
			}
		}

		private void toolStripButtonPartialRampOff_Click(object sender, EventArgs e)
		{
			RampQueryDialog dialog = new RampQueryDialog(m_sequence.MinimumLevel, m_sequence.MaximumLevel, true, m_actualLevels);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				Ramp(dialog.StartingLevel, dialog.EndingLevel);
			}
			dialog.Dispose();
		}

		private void toolStripButtonPartialRampOn_Click(object sender, EventArgs e)
		{
			RampQueryDialog dialog = new RampQueryDialog(m_sequence.MinimumLevel, m_sequence.MaximumLevel, false, m_actualLevels);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				Ramp(dialog.StartingLevel, dialog.EndingLevel);
			}
			dialog.Dispose();
		}

		private void toolStripButtonPause_Click(object sender, EventArgs e)
		{
			if (m_executionInterface.EngineStatus(m_executionContextHandle) == 1)
			{
				m_positionTimer.Stop();
				m_executionInterface.ExecutePause(m_executionContextHandle);
				SetEditingState(true);
			}
		}

		private void toolStripButtonPlay_Click(object sender, EventArgs e)
		{
			int sequencePosition = 0;
			int num = m_executionInterface.EngineStatus(m_executionContextHandle, out sequencePosition);
			if (num != 1)
			{
				if (num != 2)
				{
					Reset();
				}
				if (m_executionInterface.ExecutePlay(m_executionContextHandle, sequencePosition, 0))
				{
					m_positionTimer.Start();
					SetEditingState(false);
				}
			}
		}

		private void toolStripButtonPlayPoint_Click(object sender, EventArgs e)
		{
			if (m_executionInterface.EngineStatus(m_executionContextHandle) != 1)
			{
				SetEditingState(false);
				if (playOnlyTheSelectedRangeToolStripMenuItem.Checked)
				{
					m_executionInterface.ExecutePlay(m_executionContextHandle, m_normalizedRange.Left * m_sequence.EventPeriod, m_normalizedRange.Right * m_sequence.EventPeriod);
				}
				else
				{
					m_executionInterface.ExecutePlay(m_executionContextHandle, m_normalizedRange.Left * m_sequence.EventPeriod, m_sequence.TotalEventPeriods * m_sequence.EventPeriod);
				}
				m_positionTimer.Start();
			}
		}

		private void toolStripButtonPlaySpeedHalf_Click(object sender, EventArgs e)
		{
			SetAudioSpeed(0.5f);
		}

		private void toolStripButtonPlaySpeedNormal_Click(object sender, EventArgs e)
		{
			SetAudioSpeed(1f);
		}

		private void toolStripButtonPlaySpeedQuarter_Click(object sender, EventArgs e)
		{
			SetAudioSpeed(0.25f);
		}

		private void toolStripButtonPlaySpeedThreeQuarters_Click(object sender, EventArgs e)
		{
			SetAudioSpeed(0.75f);
		}

		private void toolStripButtonPlaySpeedVariable_Click(object sender, EventArgs e)
		{
			SetVariablePlaybackSpeed(toolStripExecutionControl.PointToScreen(new Point(SpeedVariableTsb.Bounds.Right, SpeedVariableTsb.Bounds.Top)));
		}

		private void toolStripButtonRampOff_Click(object sender, EventArgs e)
		{
			Ramp(m_sequence.MaximumLevel, m_sequence.MinimumLevel);
		}

		private void toolStripButtonRampOn_Click(object sender, EventArgs e)
		{
			Ramp(m_sequence.MinimumLevel, m_sequence.MaximumLevel);
		}

		private void toolStripButtonRandom_Click(object sender, EventArgs e)
		{
			RandomParametersDialog dialog = new RandomParametersDialog(m_sequence.MinimumLevel, m_sequence.MaximumLevel, m_actualLevels);
			int maximumLevel = m_sequence.MaximumLevel;
			int intensityMax = m_sequence.MaximumLevel;
			int num5 = 0;
			Random random = null;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				int left;
				int num10;
				float saturationLevel = dialog.SaturationLevel;
				int periodLength = dialog.PeriodLength;
				if (dialog.VaryIntensity)
				{
					maximumLevel = dialog.IntensityMin;
					intensityMax = dialog.IntensityMax;
					num5 = Math.Abs((int) (intensityMax - maximumLevel));
					random = new Random();
				}
				AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
				List<int> list = new List<int>();
				Random random2 = new Random();
				int top = m_normalizedRange.Top;
				while (top < m_normalizedRange.Bottom)
				{
					num10 = m_channelOrderMapping[top];
					if (m_sequence.Channels[num10].Enabled)
					{
						left = m_normalizedRange.Left;
						while (left < m_normalizedRange.Right)
						{
							m_sequence.EventValues[num10, left] = m_sequence.MinimumLevel;
							left++;
						}
					}
					top++;
				}
				for (left = m_normalizedRange.Left; left < m_normalizedRange.Right; left += periodLength)
				{
					int num8;
					if (dialog.UseSaturation)
					{
						if (random2.Next(2) > 0)
						{
							num8 = (int) Math.Ceiling((double) ((m_normalizedRange.Height * saturationLevel) - 0.1));
						}
						else
						{
							num8 = (int) Math.Floor((double) (m_normalizedRange.Height * saturationLevel));
						}
					}
					else
					{
						num8 = 0;
						while (num8 == 0)
						{
							num8 = random2.Next(m_normalizedRange.Height + 1);
						}
					}
					list.Clear();
					for (top = m_normalizedRange.Top; top < m_normalizedRange.Bottom; top++)
					{
						num10 = m_channelOrderMapping[top];
						list.Add(num10);
					}
					byte drawingLevel = m_drawingLevel;
					while (num8-- > 0)
					{
						int index = random2.Next(list.Count);
						if (random != null)
						{
							drawingLevel = (byte) random.Next(maximumLevel, intensityMax + 1);
						}
						for (int i = 0; (i < periodLength) && ((left + i) < m_normalizedRange.Right); i++)
						{
							m_sequence.EventValues[list[index], left + i] = drawingLevel;
						}
						list.RemoveAt(index);
					}
				}
				m_selectionRectangle.Width = 0;
				pictureBoxGrid.Invalidate(SelectionToRectangle());
			}
			dialog.Dispose();
		}

		private void toolStripButtonRedo_Click(object sender, EventArgs e)
		{
			Redo();
		}

		private void toolStripButtonRemoveCells_Click(object sender, EventArgs e)
		{
			if (m_normalizedRange.Width != 0)
			{
				int right = m_normalizedRange.Right;
				int num2 = m_sequence.TotalEventPeriods - right;
				AddUndoItem(new Rectangle(m_normalizedRange.Left, m_normalizedRange.Top, m_normalizedRange.Width, m_normalizedRange.Height), UndoOriginalBehavior.Removal);
				for (int i = 0; i < m_normalizedRange.Height; i++)
				{
					int num3 = m_channelOrderMapping[m_normalizedRange.Top + i];
					int num4 = 0;
					while (num4 < num2)
					{
						m_sequence.EventValues[num3, (right - m_normalizedRange.Width) + num4] = m_sequence.EventValues[num3, right + num4];
						num4++;
					}
					for (num4 = (right + num2) - m_normalizedRange.Width; num4 < m_sequence.TotalEventPeriods; num4++)
					{
						m_sequence.EventValues[num3, num4] = m_sequence.MinimumLevel;
					}
				}
				pictureBoxGrid.Refresh();
			}
		}

		private void toolStripButtonSave_Click(object sender, EventArgs e)
		{
			m_systemInterface.InvokeSave(this);
		}

		private void toolStripButtonSaveOrder_Click(object sender, EventArgs e)
		{
			VixenPlus.SortOrder item = null;
			TextQueryDialog dialog = new TextQueryDialog("New order", "What name would you like to give to this ordering of the channels?", string.Empty);
			DialogResult no = DialogResult.No;
			while (no == DialogResult.No)
			{
				if (dialog.ShowDialog() == DialogResult.Cancel)
				{
					dialog.Dispose();
					return;
				}
				no = DialogResult.Yes;
				foreach (VixenPlus.SortOrder order2 in m_sequence.Sorts)
				{
					if (order2.Name == dialog.Response)
					{
						if ((no = MessageBox.Show("This name is already in use.\nDo you want to overwrite it?", Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) == DialogResult.Cancel)
						{
							dialog.Dispose();
							return;
						}
						item = order2;
						break;
					}
				}
			}
			if (item != null)
			{
				item.ChannelIndexes.Clear();
				item.ChannelIndexes.AddRange(m_channelOrderMapping);
				toolStripComboBoxChannelOrder.SelectedItem = item;
			}
			else
			{
				m_sequence.Sorts.Add(item = new VixenPlus.SortOrder(dialog.Response, m_channelOrderMapping));
				toolStripComboBoxChannelOrder.Items.Insert(toolStripComboBoxChannelOrder.Items.Count - 1, item);
				toolStripComboBoxChannelOrder.SelectedIndex = toolStripComboBoxChannelOrder.Items.Count - 2;
			}
			dialog.Dispose();
			base.IsDirty = true;
		}

		private void toolStripButtonShimmerDimming_Click(object sender, EventArgs e)
		{
			int maxFrequency = 0x3e8 / m_sequence.EventPeriod;
			EffectFrequencyDialog dialog = new EffectFrequencyDialog("Shimmer (dimming)", maxFrequency, m_dimmingShimmerGenerator);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
				int bottom = m_normalizedRange.Bottom;
				int right = m_normalizedRange.Right;
				byte[,] values = new byte[m_normalizedRange.Height, m_normalizedRange.Width];
				DimmingShimmerGenerator(values, new int[] { dialog.Frequency });
				int top = m_normalizedRange.Top;
				for (int i = 0; top < bottom; i++)
				{
					int num5 = m_channelOrderMapping[top];
					if (m_sequence.Channels[num5].Enabled)
					{
						int left = m_normalizedRange.Left;
						for (int j = 0; left < right; j++)
						{
							m_sequence.EventValues[num5, left] = values[i, j];
							left++;
						}
					}
					top++;
				}
				m_selectionRectangle.Width = 0;
				pictureBoxGrid.Invalidate(SelectionToRectangle());
			}
			dialog.Dispose();
		}

		private void toolStripButtonSparkle_Click(object sender, EventArgs e)
		{
			int maxFrequency = 0x3e8 / m_sequence.EventPeriod;
			SparkleParamsDialog dialog = new SparkleParamsDialog(maxFrequency, m_sparkleGenerator, m_sequence.MinimumLevel, m_sequence.MaximumLevel, m_drawingLevel, m_actualLevels);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
				int bottom = m_normalizedRange.Bottom;
				int right = m_normalizedRange.Right;
				byte[,] values = new byte[m_normalizedRange.Height, m_normalizedRange.Width];
				SparkleGenerator(values, new int[] { dialog.Frequency, dialog.DecayTime, dialog.MinimumIntensity, dialog.MaximumIntensity });
				int top = m_normalizedRange.Top;
				for (int i = 0; top < bottom; i++)
				{
					int num5 = m_channelOrderMapping[top];
					if (m_sequence.Channels[num5].Enabled)
					{
						int left = m_normalizedRange.Left;
						for (int j = 0; left < right; j++)
						{
							m_sequence.EventValues[num5, left] = values[i, j];
							left++;
						}
					}
					top++;
				}
				m_selectionRectangle.Width = 0;
				pictureBoxGrid.Invalidate(SelectionToRectangle());
			}
			dialog.Dispose();
		}

		private void toolStripButtonStop_Click(object sender, EventArgs e)
		{
			if (m_executionInterface.EngineStatus(m_executionContextHandle) != 0)
			{
				m_positionTimer.Stop();
				ProgramEnded();
				m_executionInterface.ExecuteStop(m_executionContextHandle);
			}
		}

		private void toolStripButtonTestChannels_Click(object sender, EventArgs e)
		{
			try
			{
				new TestChannelsDialog(m_sequence, m_executionInterface).Show();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void toolStripButtonTestConsole_Click(object sender, EventArgs e)
		{
			try
			{
				new TestConsoleDialog(m_sequence, m_executionInterface).Show();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void toolStripButtonToggleCellText_Click(object sender, EventArgs e)
		{
			m_showCellText = !m_showCellText;
			toolStripButtonToggleCellText.BackgroundImage = m_showCellText
				                                                ? global::Properties.Resources.Ball_Green
				                                                : global::Properties.Resources.Ball_Red;
			pictureBoxGrid.Refresh();
		}

		private void toolStripButtonToggleCrossHairs_Click(object sender, EventArgs e)
		{
			if (!toolStripButtonToggleCrossHairs.Checked)
			{
				pictureBoxGrid.Invalidate(new Rectangle((m_mouseTimeCaret - hScrollBar1.Value) * m_periodPixelWidth, 0, m_periodPixelWidth, pictureBoxGrid.Height));
				pictureBoxGrid.Update();
				pictureBoxGrid.Invalidate(new Rectangle(0, (m_mouseChannelCaret - vScrollBar1.Value) * m_gridRowHeight, pictureBoxGrid.Width, m_gridRowHeight));
				pictureBoxGrid.Update();
			}
		}

		private void toolStripButtonToggleLevels_Click(object sender, EventArgs e)
		{
			m_actualLevels = !m_actualLevels;
			//toolStripButtonToggleLevels.Image = m_actualLevels
			//                                        ? global::Properties.Resources.level_Number
			//                                        : global::Properties.Resources.level_Percent;
			m_preferences.SetBoolean("ActualLevels", m_actualLevels);
			UpdateLevelDisplay();
			pictureBoxGrid.Refresh();
		}

		private void toolStripButtonToggleRamps_Click(object sender, EventArgs e)
		{
			m_showingGradient = !m_showingGradient;
			m_preferences.SetBoolean("BarLevels", !m_showingGradient);
			pictureBoxGrid.Refresh();
		}

		private void toolStripButtonTransparentPaste_Click(object sender, EventArgs e)
		{
			if (m_systemInterface.Clipboard != null)
			{
				AddUndoItem(new Rectangle(m_normalizedRange.X, m_normalizedRange.Y, m_systemInterface.Clipboard.GetLength(1), m_systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite);
				byte[,] clipboard = m_systemInterface.Clipboard;
				int length = clipboard.GetLength(0);
				int num3 = clipboard.GetLength(1);
				int minimumLevel = m_sequence.MinimumLevel;
				for (int i = 0; (i < length) && ((m_normalizedRange.Top + i) < m_sequence.ChannelCount); i++)
				{
					int num4 = m_channelOrderMapping[m_normalizedRange.Top + i];
					for (int j = 0; (j < num3) && ((m_normalizedRange.Left + j) < m_sequence.TotalEventPeriods); j++)
					{
						byte num5 = clipboard[i, j];
						if (num5 > minimumLevel)
						{
							m_sequence.EventValues[num4, m_normalizedRange.Left + j] = num5;
						}
					}
				}
				base.IsDirty = true;
				pictureBoxGrid.Refresh();
			}
		}

		private void toolStripButtonUndo_Click(object sender, EventArgs e)
		{
			Undo();
		}

		private void toolStripButtonWaveform_Click(object sender, EventArgs e)
		{
			if (m_executionInterface.EngineStatus(m_executionContextHandle) != 1)
			{
				if ((m_waveformPixelData == null) || (m_waveformPCMData == null))
				{
					ParseAudioWaveform();
				}
				if (toolStripButtonWaveform.Checked)
				{
					pictureBoxTime.Height = 120 * m_waveformMaxAmplitude;
					EnableWaveformButton();
				}
				else
				{
					pictureBoxTime.Height = 60;
					toolStripLabelWaveformZoom.Enabled = false;
					toolStripComboBoxWaveformZoom.Enabled = false;
				}
				pictureBoxTime.Refresh();
				pictureBoxChannels.Refresh();
			}
		}

		private void toolStripComboBoxChannelOrder_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (toolStripComboBoxChannelOrder.SelectedIndex != -1)
			{
				if ((m_sequence.Profile != null) && (toolStripComboBoxChannelOrder.SelectedIndex == 0))
				{
					toolStripComboBoxChannelOrder.SelectedIndex = -1;
					MessageBox.Show("This sequence is attached to a profile.\nChanges to the profile affect all sequences attached to it.\n\nIf this is what you want to do, please use the profile manager.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					if (toolStripComboBoxChannelOrder.SelectedIndex == 0)
					{
						if (m_sequence.ChannelCount == 0)
						{
							MessageBox.Show("There are no channels to reorder.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
						toolStripButtonDeleteOrder.Enabled = false;
						toolStripComboBoxChannelOrder.SelectedIndex = -1;
						ChannelOrderDialog dialog = new ChannelOrderDialog(m_sequence.Channels, m_channelOrderMapping);
						if (dialog.ShowDialog() == DialogResult.OK)
						{
							m_channelOrderMapping.Clear();
							foreach (VixenPlus.Channel channel in dialog.ChannelMapping)
							{
								m_channelOrderMapping.Add(m_sequence.Channels.IndexOf(channel));
							}
							base.IsDirty = true;
						}
						dialog.Dispose();
					}
					else if (toolStripComboBoxChannelOrder.SelectedIndex == (toolStripComboBoxChannelOrder.Items.Count - 1))
					{
						toolStripButtonDeleteOrder.Enabled = false;
						toolStripComboBoxChannelOrder.SelectedIndex = -1;
						m_channelOrderMapping.Clear();
						for (int i = 0; i < m_sequence.ChannelCount; i++)
						{
							m_channelOrderMapping.Add(i);
						}
						m_sequence.LastSort = -1;
						if (m_sequence.Profile == null)
						{
							base.IsDirty = true;
						}
					}
					else
					{
						m_channelOrderMapping.Clear();
						m_channelOrderMapping.AddRange(((VixenPlus.SortOrder) toolStripComboBoxChannelOrder.SelectedItem).ChannelIndexes);
						m_sequence.LastSort = toolStripComboBoxChannelOrder.SelectedIndex - 1;
						toolStripButtonDeleteOrder.Enabled = true;
						if (m_sequence.Profile == null)
						{
							base.IsDirty = true;
						}
					}
					pictureBoxChannels.Refresh();
					pictureBoxGrid.Refresh();
				}
			}
		}

		private void toolStripComboBoxColumnZoom_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (toolStripComboBoxColumnZoom.SelectedIndex != -1)
			{
				UpdateColumnWidth();
			}
		}

		private void toolStripComboBoxRowZoom_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (toolStripComboBoxRowZoom.SelectedIndex != -1)
			{
				UpdateRowHeight();
			}
		}

		private void toolStripComboBoxWaveformZoom_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((m_executionInterface.EngineStatus(m_executionContextHandle) != 1) && ((toolStripComboBoxWaveformZoom.SelectedIndex != -1) && toolStripComboBoxWaveformZoom.Enabled))
			{
				string selectedItem = (string) toolStripComboBoxWaveformZoom.SelectedItem;
				m_waveformMaxAmplitude = (int) ((100f / ((float) Convert.ToInt32(selectedItem.Substring(0, selectedItem.Length - 1)))) * m_waveform100PercentAmplitude);
				PCMToPixels(m_waveformPCMData, m_waveformPixelData);
				pictureBoxTime.Refresh();
			}
		}

		private void toolStripDropDownButtonPlugins_Click(object sender, EventArgs e)
		{
			PluginListDialog dialog = new PluginListDialog(m_sequence);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				toolStripDropDownButtonPlugins.DropDownItems.Clear();
				int num = 0;
				bool flag = false;
				foreach (object[] objArray in dialog.MappedPluginList)
				{
					ToolStripMenuItem item = new ToolStripMenuItem((string) objArray[0]);
					item.Checked = (bool) objArray[1];
					item.CheckOnClick = true;
					item.CheckedChanged += new EventHandler(plugInItem_CheckedChanged);
					item.Tag = num.ToString();
					num++;
					toolStripDropDownButtonPlugins.DropDownItems.Add(item);
					flag |= item.Checked;
				}
				if (toolStripDropDownButtonPlugins.DropDownItems.Count > 0)
				{
					toolStripDropDownButtonPlugins.DropDownItems.Add("-");
					toolStripDropDownButtonPlugins.DropDownItems.Add("Select all", null, new EventHandler(selectAllToolStripMenuItem_Click));
					toolStripDropDownButtonPlugins.DropDownItems.Add("Unselect all", null, new EventHandler(unselectAllToolStripMenuItem_Click));
				}
				base.IsDirty = true;
			}
			dialog.Dispose();
		}

		private void toolStripItem_CheckStateChanged(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (ToolStripMenuItem) sender;
			ToolStrip tag = (ToolStrip) item.Tag;
			tag.Visible = item.Checked;
		}

		private void toolStripMenuItemPasteAnd_Click(object sender, EventArgs e)
		{
			BooleanPaste(BooleanOperation.AND);
		}

		private void toolStripMenuItemPasteNand_Click(object sender, EventArgs e)
		{
			BooleanPaste(BooleanOperation.NAND);
		}

		private void toolStripMenuItemPasteNor_Click(object sender, EventArgs e)
		{
			BooleanPaste(BooleanOperation.NOR);
		}

		private void toolStripMenuItemPasteOr_Click(object sender, EventArgs e)
		{
			BooleanPaste(BooleanOperation.OR);
		}

		private void toolStripMenuItemPasteXnor_Click(object sender, EventArgs e)
		{
			BooleanPaste(BooleanOperation.XNOR);
		}

		private void toolStripMenuItemPasteXor_Click(object sender, EventArgs e)
		{
			BooleanPaste(BooleanOperation.XOR);
		}

		private void TurnCellsOff()
		{
			AddUndoItem(m_normalizedRange, UndoOriginalBehavior.Overwrite);
			int bottom = m_normalizedRange.Bottom;
			int right = m_normalizedRange.Right;
			for (int i = m_normalizedRange.Top; i < bottom; i++)
			{
				int num4 = m_channelOrderMapping[i];
				if (m_sequence.Channels[num4].Enabled)
				{
					for (int j = m_normalizedRange.Left; j < right; j++)
					{
						m_sequence.EventValues[num4, j] = m_sequence.MinimumLevel;
					}
				}
			}
			m_selectionRectangle.Width = 0;
			pictureBoxGrid.Invalidate(SelectionToRectangle());
		}

		private void Undo()
		{
			if (m_undoStack.Count != 0)
			{
				UndoItem item = (UndoItem) m_undoStack.Pop();
				int length = 0;
				int num2 = 0;
				if (item.Data != null)
				{
					length = item.Data.GetLength(0);
					num2 = item.Data.GetLength(1);
				}
				toolStripButtonUndo.Enabled = undoToolStripMenuItem.Enabled = m_undoStack.Count > 0;
				if (m_undoStack.Count > 0)
				{
					base.IsDirty = true;
				}
				UndoItem item2 = new UndoItem(item.Location, GetAffectedBlockData(new Rectangle(item.Location.X, item.Location.Y, item.Data.GetLength(1), item.Data.GetLength(0))), item.Behavior, m_sequence, m_channelOrderMapping);
				switch (item.Behavior)
				{
					case UndoOriginalBehavior.Overwrite:
						DisjointedOverwrite(item.Location.X, item.Location.Y, item.Data, item.ReferencedChannels);
						pictureBoxGrid.Refresh();
						break;

					case UndoOriginalBehavior.Removal:
						DisjointedInsert(item.Location.X, item.Location.Y, item.Data.GetLength(1), item.Data.GetLength(0), item.ReferencedChannels);
						DisjointedOverwrite(item.Location.X, item.Location.Y, item.Data, item.ReferencedChannels);
						pictureBoxGrid.Refresh();
						break;

					case UndoOriginalBehavior.Insertion:
						DisjointedRemove(item.Location.X, item.Location.Y, item.Data.GetLength(1), item.Data.GetLength(0), item.ReferencedChannels);
						pictureBoxGrid.Refresh();
						break;
				}
				UpdateUndoText();
				m_redoStack.Push(item2);
				toolStripButtonRedo.Enabled = redoToolStripMenuItem.Enabled = true;
				UpdateRedoText();
			}
		}

		private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ToolStripItem item in toolStripDropDownButtonPlugins.DropDownItems)
			{
				if (!(item is ToolStripMenuItem))
				{
					break;
				}
				((ToolStripMenuItem) item).Checked = false;
			}
		}

		private void UpdateColumnWidth()
		{
			double num = (toolStripComboBoxColumnZoom.SelectedIndex + 1) * 0.1;
			m_periodPixelWidth = (int) (m_preferences.GetInteger("MaxColumnWidth") * num);
			HScrollCheck();
			ParseAudioWaveform();
			pictureBoxGrid.Refresh();
			pictureBoxTime.Refresh();
		}

		private void UpdateGrid(Graphics g, Rectangle clipRect)
		{
			if (m_sequence.ChannelCount != 0)
			{
				using (Font font = new Font(Font.FontFamily, (m_periodPixelWidth <= 20) ? ((float) 5) : ((m_periodPixelWidth <= 25) ? ((float) 6) : ((m_periodPixelWidth < 50) ? ((float) 8) : ((float) 10)))))
				{
					using (SolidBrush brush = new SolidBrush(Color.White))
					{
						int num;
						int num3;
						int num7;
						VixenPlus.Channel channel;
						string str;
						int num5 = ((clipRect.X / m_periodPixelWidth) * m_periodPixelWidth) + 1;
						int y = ((clipRect.Y / m_gridRowHeight) * m_gridRowHeight) + 1;
						int num6 = (clipRect.X / m_periodPixelWidth) + hScrollBar1.Value;
						int cellY = (clipRect.Y / m_gridRowHeight) + vScrollBar1.Value;
						if (!m_showingGradient)
						{
							goto Label_0329;
						}
						while ((y < clipRect.Bottom) && (cellY < m_sequence.ChannelCount))
						{
							num7 = m_channelOrderMapping[cellY];
							channel = m_sequence.Channels[num7];
							num3 = num5;
							num = num6;
							while ((num3 < clipRect.Right) && (num < m_sequence.TotalEventPeriods))
							{
								brush.Color = GetGradientColor(m_gridBackBrush.Color, channel.Color, m_sequence.EventValues[num7, num]);
								g.FillRectangle(brush, num3, y, m_periodPixelWidth - 1, m_gridRowHeight - 1);
								if (m_showCellText && (GetCellIntensity(num, cellY, out str) > 0))
								{
									g.DrawString(str, font, Brushes.Black, new RectangleF((float) num3, (float) y, (float) (m_periodPixelWidth - 1), (float) (m_gridRowHeight - 1)));
								}
								num3 += m_periodPixelWidth;
								num++;
							}
							y += m_gridRowHeight;
							cellY++;
						}
						return;
					Label_0222:
						num7 = m_channelOrderMapping[cellY];
						channel = m_sequence.Channels[num7];
						num3 = num5;
						for (num = num6; (num3 < clipRect.Right) && (num < m_sequence.TotalEventPeriods); num++)
						{
							int height = ((m_gridRowHeight - 1) * m_sequence.EventValues[num7, num]) / 0xff;
							g.FillRectangle(channel.Brush, num3, ((y + m_gridRowHeight) - 1) - height, m_periodPixelWidth - 1, height);
							if (m_showCellText && (GetCellIntensity(num, cellY, out str) > 0))
							{
								g.DrawString(str, font, Brushes.Black, new RectangleF((float) num3, (float) y, (float) (m_periodPixelWidth - 1), (float) (m_gridRowHeight - 1)));
							}
							num3 += m_periodPixelWidth;
						}
						y += m_gridRowHeight;
						cellY++;
					Label_0329:
						if ((y < clipRect.Bottom) && (cellY < m_sequence.ChannelCount))
						{
							goto Label_0222;
						}
					}
				}
			}
		}

		private void UpdateLevelDisplay()
		{
			SetDrawingLevel(m_drawingLevel);
			if (m_actualLevels)
			{
				toolStripButtonToggleLevels.Image = global::Properties.Resources.Percent;
				//toolStripButtonToggleLevels.Image = pictureBoxLevelPercent.Image;
				toolStripButtonToggleLevels.Text = toolStripButtonToggleLevels.ToolTipText = "Show intensity levels as percent (0-100%)";
			}
			else
			{
				toolStripButtonToggleLevels.Image = global::Properties.Resources.number;
				//toolStripButtonToggleLevels.Image = pictureBoxLevelNumber.Image;
				toolStripButtonToggleLevels.Text = toolStripButtonToggleLevels.ToolTipText = "Show actual intensity levels (0-255)";
			}
			m_intensityAdjustDialog.ActualLevels = m_actualLevels;
		}

        private void UpdateToolbarMenu()
        {
            smallToolStripMenuItem.Checked = (ToolStripManager.iconSize == ToolStripManager.ICON_SIZE_SMALL);
            mediumToolStripMenuItem.Checked = (ToolStripManager.iconSize == ToolStripManager.ICON_SIZE_MEDIUM);
            largeToolStripMenuItem.Checked = (ToolStripManager.iconSize == ToolStripManager.ICON_SIZE_LARGE);
			lockToolbarToolStripMenuItem.Checked = ToolStripManager.Locked;
        }

        private void UpdatePositionLabel(Rectangle rect, bool zeroWidthIsValid)
        {
            int milliseconds = rect.Left * m_sequence.EventPeriod;
            string str = TimeString(milliseconds);
            if (rect.Width > 1)
            {
                int num2 = (rect.Right - 1) * m_sequence.EventPeriod;
                string str2 = TimeString(num2);
                labelPosition.Text = string.Format("{0} - {1}\n({2})", str, str2, TimeString(num2 - milliseconds));
            }
            else if (((rect.Width == 0) && zeroWidthIsValid) || (rect.Width == 1))
            {
                labelPosition.Text = str;
            }
            else
            {
                labelPosition.Text = string.Empty;
            }
        }

        private void UpdateFollowMouse(Point mousePoint)
        {
			var rowCount = m_normalizedRange.Height;
            lblFollowMouse.Text = labelPosition.Text + Environment.NewLine + rowCount  + " Channel" + ((rowCount == 1) ? "" : "s");
            mousePoint.X -= lblFollowMouse.Size.Width;
            mousePoint.Y += 24;
            lblFollowMouse.Location = mousePoint;

        }

		private void UpdateProgress()
		{
			int x = (m_previousPosition - hScrollBar1.Value) * m_periodPixelWidth;
			pictureBoxTime.Invalidate(new Rectangle(x, pictureBoxTime.Height - 0x23, m_periodPixelWidth + m_periodPixelWidth, 15));
			pictureBoxGrid.Invalidate(new Rectangle(x, 0, m_periodPixelWidth + m_periodPixelWidth, pictureBoxGrid.Height));
		}

		private void UpdateRedoText()
		{
			if (m_redoStack.Count > 0)
			{
				toolStripButtonRedo.ToolTipText = redoToolStripMenuItem.Text = "Redo: " + ((UndoItem) m_redoStack.Peek()).ToString();
			}
			else
			{
				toolStripButtonRedo.ToolTipText = redoToolStripMenuItem.Text = "Redo";
			}
		}

		private void UpdateRowHeight()
		{
			if (toolStripComboBoxRowZoom.SelectedIndex >= 6)
			{
				if (!(m_channelNameFont.Size == 8f))
				{
					m_channelNameFont.Dispose();
					m_channelNameFont = new Font("Arial", 8f);
				}
			}
			else if (toolStripComboBoxRowZoom.SelectedIndex <= 3)
			{
				if (!(m_channelNameFont.Size == 5f))
				{
					m_channelNameFont.Dispose();
					m_channelNameFont = new Font("Arial", 5f);
				}
			}
			else if (!(m_channelNameFont.Size == 7f))
			{
				m_channelNameFont.Dispose();
				m_channelNameFont = new Font("Arial", 7f);
			}
			double num = (toolStripComboBoxRowZoom.SelectedIndex + 1) * 0.1;
			m_gridRowHeight = (int) (m_preferences.GetInteger("MaxRowHeight") * num);
			VScrollCheck();
			pictureBoxGrid.Refresh();
			pictureBoxChannels.Refresh();
		}

		private void UpdateUndoText()
		{
			if (m_undoStack.Count > 0)
			{
				toolStripButtonUndo.ToolTipText = undoToolStripMenuItem.Text = "Undo: " + ((UndoItem) m_undoStack.Peek()).ToString();
			}
			else
			{
				toolStripButtonUndo.ToolTipText = undoToolStripMenuItem.Text = "Undo";
			}
		}

		private void vScrollBar1_ValueChanged(object sender, EventArgs e)
		{
			pictureBoxGrid.Refresh();
			pictureBoxChannels.Refresh();
		}

		private void VScrollCheck()
		{
			m_visibleRowCount = pictureBoxGrid.Height / m_gridRowHeight;
			vScrollBar1.Maximum = m_sequence.ChannelCount - 1;
			vScrollBar1.LargeChange = m_visibleRowCount;
			vScrollBar1.Enabled = m_visibleRowCount < m_sequence.ChannelCount;
			if (!vScrollBar1.Enabled)
			{
				vScrollBar1.Value = vScrollBar1.Minimum;
			}
			else if ((vScrollBar1.Value + m_visibleRowCount) > m_sequence.ChannelCount)
			{
				m_selectedRange.Y += m_visibleRowCount - m_sequence.ChannelCount;
				m_normalizedRange.Y += m_visibleRowCount - m_sequence.ChannelCount;
				vScrollBar1.Value = m_sequence.ChannelCount - m_visibleRowCount;
			}
			if (vScrollBar1.Maximum >= 0)
			{
				if (vScrollBar1.Value == -1)
				{
					vScrollBar1.Value = 0;
				}
				if (vScrollBar1.Minimum == -1)
				{
					vScrollBar1.Minimum = 0;
				}
			}
		}

		private void xToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			SetAudioSpeed(0.75f);
		}

		public override string Author
		{
			get
			{
				return "Vixen and VixenPlus Developers";
			}
		}

		public override string Description
		{
			get
			{
				return "Vixen+ sequence user interface";
			}
		}

		public override string FileExtension
		{
			get
			{
				return ".vix";
			}
		}

		public override string FileTypeDescription
		{
			get
			{
				return "Vixen/Vixen+ sequence";
			}
		}

		private VixenPlus.Channel SelectedChannel
		{
			get
			{
				return m_selectedChannel;
			}
			set
			{
				if (m_selectedChannel != value)
				{
					VixenPlus.Channel selectedChannel = m_selectedChannel;
					m_selectedChannel = value;
					if (selectedChannel != null)
					{
						pictureBoxChannels.Invalidate(GetChannelNameRect(selectedChannel));
					}
					if (m_selectedChannel != null)
					{
						pictureBoxChannels.Invalidate(GetChannelNameRect(m_selectedChannel));
					}
					pictureBoxChannels.Update();
				}
			}
		}

		public override EventSequence Sequence
		{
			get
			{
				return m_sequence;
			}
			set
			{
				m_sequence = value;
			}
		}

		private enum ArithmeticOperation
		{
			Addition,
			Subtraction,
			Scale,
			Min,
			Max
		}

		private enum BooleanOperation
		{
			OR,
			AND,
			XOR,
			NOR,
			NAND,
			XNOR
		}

		private delegate void ToolStripUpdateDelegate(int seconds);

        private void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resizeToolStrips(ToolStripManager.ICON_SIZE_SMALL);
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resizeToolStrips(ToolStripManager.ICON_SIZE_MEDIUM);
        }

        private void largeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resizeToolStrips(ToolStripManager.ICON_SIZE_LARGE);
        }

        private void resizeToolStrips(int widthAndHeight)
        {
            ToolStripManager.iconSize = widthAndHeight;
            ToolStripManager.resizeToolStrips(this);
            UpdateToolbarMenu();
        }

        private void lockToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var style = lockToolbarToolStripMenuItem.Checked ? ToolStripGripStyle.Hidden : ToolStripGripStyle.Visible;

			ToolStripManager.Locked = lockToolbarToolStripMenuItem.Checked;

            toolStripDisplaySettings.GripStyle = style;
            toolStripEditing.GripStyle = style;
            toolStripEffect.GripStyle = style;
            toolStripExecutionControl.GripStyle = style;
            toolStripSequenceSettings.GripStyle = style;
            toolStripText.GripStyle = style;
			//toolStripVisualizer.GripStyle = style;
        }

	}
}

