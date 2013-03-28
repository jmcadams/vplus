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
			this.m_executionInterface = null;
			this.m_systemInterface = null;
			this.m_gridRowHeight = 20;
			this.m_visibleRowCount = 0;
			this.m_visibleEventPeriods = 0;
			this.m_channelBackBrush = null;
			this.m_timeBackBrush = null;
			this.m_gridBackBrush = null;
			this.m_channelNameFont = new Font("Arial", 8f);
			this.m_timeFont = new Font("Arial", 8f);
			this.m_selectedChannel = null;
			this.m_currentlyEditingChannel = null;
			this.m_editingChannelSortedIndex = 0;
			this.m_gridGraphics = null;
			this.m_selectedRange = new Rectangle();
			this.m_periodPixelWidth = 30;
			this.m_selectionRectangle = new Rectangle();
			this.m_selectionBrush = new SolidBrush(Color.FromArgb(0x3f, Color.Blue));
			this.m_positionBrush = new SolidBrush(Color.FromArgb(0x3f, Color.Red));
			this.m_mouseDownInGrid = false;
			this.m_position = -1;
			this.m_previousPosition = -1;
			this.m_mouseChannelCaret = -1;
			this.m_mouseTimeCaret = -1;
			this.m_channelCaretBrush = new SolidBrush(Color.Gray);
			this.m_undoStack = new Stack();
			this.m_redoStack = new Stack();
			this.m_lineRect = new Rectangle(-1, -1, -1, -1);
			this.m_lastCellX = -1;
			this.m_lastCellY = -1;
			this.m_initializing = false;
			this.m_selectedEventIndex = -1;
			this.m_normalizedRange = new Rectangle();
			this.m_mouseDownAtInGrid = new Point();
			this.m_mouseDownAtInChannels = Point.Empty;
			this.m_waveformPCMData = null;
			this.m_waveformPixelData = null;
			this.m_waveformOffset = 0x24;
			this.m_showingOutputs = false;
			this.m_selectedLineIndex = 0;
			this.m_arrowBitmap = null;
			this.m_bookmarks = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
			this.m_showingGradient = true;
			this.m_actualLevels = false;
			this.m_showCellText = false;
			this.m_lastSelectableControl = null;
			if (Interfaces.Available.TryGetValue("IExecution", out obj2))
			{
				this.m_executionInterface = (IExecution) obj2;
			}
			if (Interfaces.Available.TryGetValue("ISystem", out obj2))
			{
				this.m_systemInterface = (ISystem) obj2;
			}
			this.m_toolStrips = new Dictionary<string, ToolStrip>();
			this.m_toolStripCheckStateChangeHandler = new EventHandler(this.toolStripItem_CheckStateChanged);
		}

		private void additionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ArithmeticPaste(ArithmeticOperation.Addition);
		}

		private void AddUndoItem(Rectangle blockAffected, UndoOriginalBehavior behavior)
		{
			if (blockAffected.Width != 0)
			{
				byte[,] affectedBlockData = this.GetAffectedBlockData(blockAffected);
				this.m_undoStack.Push(new UndoItem(blockAffected.Location, affectedBlockData, behavior, this.m_sequence, this.m_channelOrderMapping));
				this.toolStripButtonUndo.Enabled = this.undoToolStripMenuItem.Enabled = true;
				this.UpdateUndoText();
				this.toolStripButtonRedo.Enabled = this.redoToolStripMenuItem.Enabled = false;
				this.m_redoStack.Clear();
				this.UpdateRedoText();
				base.IsDirty = true;
			}
		}

		private void AddUndoItem(Rectangle blockAffected, UndoOriginalBehavior behavior, bool isRelative)
		{
			if (blockAffected.Width != 0)
			{
				if (isRelative)
				{
					blockAffected.X += this.hScrollBar1.Value;
					blockAffected.Y += this.vScrollBar1.Value;
				}
				this.AddUndoItem(blockAffected, behavior);
			}
		}

		private void AffectGrid(int startRow, int startCol, byte[,] values)
		{
			this.AddUndoItem(new Rectangle(startCol, startRow, values.GetLength(1), values.GetLength(0)), UndoOriginalBehavior.Overwrite);
			for (int i = 0; i < values.GetLength(0); i++)
			{
				int num = this.m_channelOrderMapping[startRow + i];
				for (int j = 0; j < values.GetLength(1); j++)
				{
					this.m_sequence.EventValues[num, startCol + j] = values[i, j];
				}
			}
			this.pictureBoxGrid.Refresh();
			base.IsDirty = true;
		}

		private void allChannelsToFullIntensityForThisEventToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.m_selectedEventIndex != -1)
			{
				Rectangle blockAffected = new Rectangle(this.m_selectedEventIndex, 0, 1, this.m_sequence.ChannelCount);
				this.AddUndoItem(blockAffected, UndoOriginalBehavior.Overwrite);
				for (int i = 0; i < this.m_sequence.ChannelCount; i++)
				{
					this.m_sequence.EventValues[this.m_channelOrderMapping[i], this.m_selectedEventIndex] = this.m_drawingLevel;
				}
				this.InvalidateRect(blockAffected);
			}
		}

		private void allEventsToFullIntensityToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.FillChannel(this.m_selectedLineIndex);
		}

		private void ArithmeticPaste(ArithmeticOperation operation)
		{
			if (this.m_systemInterface.Clipboard != null)
			{
				this.AddUndoItem(new Rectangle(this.m_normalizedRange.X, this.m_normalizedRange.Y, this.m_systemInterface.Clipboard.GetLength(1), this.m_systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite);
				byte[,] clipboard = this.m_systemInterface.Clipboard;
				int length = clipboard.GetLength(0);
				int num3 = clipboard.GetLength(1);
				for (int i = 0; (i < length) && ((this.m_normalizedRange.Top + i) < this.m_sequence.ChannelCount); i++)
				{
					int num4 = this.m_channelOrderMapping[this.m_normalizedRange.Top + i];
					for (int j = 0; (j < num3) && ((this.m_normalizedRange.Left + j) < this.m_sequence.TotalEventPeriods); j++)
					{
						byte num5 = this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j];
						switch (operation)
						{
							case ArithmeticOperation.Addition:
								this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = (byte) Math.Min(num5 + clipboard[i, j], (int) this.m_sequence.MaximumLevel);
								break;

							case ArithmeticOperation.Subtraction:
								this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = (byte) Math.Max(num5 - clipboard[i, j], (int) this.m_sequence.MinimumLevel);
								break;

							case ArithmeticOperation.Scale:
								this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = (byte) Math.Max(Math.Min(num5 * (((float) clipboard[i, j]) / 255f), (float) this.m_sequence.MaximumLevel), (float) this.m_sequence.MinimumLevel);
								break;

							case ArithmeticOperation.Min:
								this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = Math.Max(Math.Min(clipboard[i, j], num5), this.m_sequence.MinimumLevel);
								break;

							case ArithmeticOperation.Max:
								this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = Math.Min(Math.Max(clipboard[i, j], num5), this.m_sequence.MaximumLevel);
								break;
						}
						this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = Math.Min(this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j], this.m_sequence.MaximumLevel);
						this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = Math.Max(this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j], this.m_sequence.MinimumLevel);
					}
				}
				base.IsDirty = true;
				this.pictureBoxGrid.Refresh();
			}
		}

		private void ArrayToCells(byte[,] array)
		{
			int length = array.GetLength(0);
			int num3 = array.GetLength(1);
			for (int i = 0; (i < length) && ((this.m_normalizedRange.Top + i) < this.m_sequence.ChannelCount); i++)
			{
				int num4 = this.m_channelOrderMapping[this.m_normalizedRange.Top + i];
				for (int j = 0; (j < num3) && ((this.m_normalizedRange.Left + j) < this.m_sequence.TotalEventPeriods); j++)
				{
					this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = array[i, j];
				}
			}
			base.IsDirty = true;
		}

		private void AssignChannelArray(List<VixenPlus.Channel> channels)
		{
			this.m_sequence.Channels = channels;
			this.textBoxChannelCount.Text = this.m_sequence.ChannelCount.ToString();
			this.VScrollCheck();
		}

		private void attachSequenceToToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.openFileDialog1.Filter = "Profile | *.pro";
			this.openFileDialog1.DefaultExt = "pro";
			this.openFileDialog1.InitialDirectory = Paths.ProfilePath;
			this.openFileDialog1.FileName = string.Empty;
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				this.SetProfile(this.openFileDialog1.FileName);
			}
		}

		private void BooleanPaste(BooleanOperation operation)
		{
			if (this.m_systemInterface.Clipboard != null)
			{
				this.AddUndoItem(new Rectangle(this.m_normalizedRange.X, this.m_normalizedRange.Y, this.m_systemInterface.Clipboard.GetLength(1), this.m_systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite);
				byte[,] clipboard = this.m_systemInterface.Clipboard;
				int length = clipboard.GetLength(0);
				int num3 = clipboard.GetLength(1);
				for (int i = 0; (i < length) && ((this.m_normalizedRange.Top + i) < this.m_sequence.ChannelCount); i++)
				{
					int num4 = this.m_channelOrderMapping[this.m_normalizedRange.Top + i];
					for (int j = 0; (j < num3) && ((this.m_normalizedRange.Left + j) < this.m_sequence.TotalEventPeriods); j++)
					{
						switch (operation)
						{
							case BooleanOperation.OR:
							{
								byte num1 = this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j];
								num1 = (byte) (num1 | clipboard[i, j]);
								break;
							}
							case BooleanOperation.AND:
							{
								byte num6 = this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j];
								num6 = (byte) (num6 & clipboard[i, j]);
								break;
							}
							case BooleanOperation.XOR:
							{
								byte num7 = this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j];
								num7 = (byte) (num7 ^ clipboard[i, j]);
								break;
							}
							case BooleanOperation.NOR:
								this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = (byte) ~(clipboard[i, j] | this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j]);
								break;

							case BooleanOperation.NAND:
								this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = (byte) ~(clipboard[i, j] & this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j]);
								break;

							case BooleanOperation.XNOR:
								this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = (byte) ~(clipboard[i, j] ^ this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j]);
								break;
						}
					}
				}
				base.IsDirty = true;
				this.pictureBoxGrid.Refresh();
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
				int num14 = Math.Min(num16 + brush.Length, this.m_sequence.TotalEventPeriods) - num16;
				for (int j = 0; j < num14; j++)
				{
					this.m_sequence.EventValues[this.m_channelOrderMapping[top], num16 + j] = brush[j];
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
				this.m_sequence.EventValues[this.m_channelOrderMapping[top], left] = this.m_drawingLevel;
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
			byte[,] buffer = new byte[this.m_normalizedRange.Height, this.m_normalizedRange.Width];
			for (int i = 0; i < this.m_normalizedRange.Height; i++)
			{
				int num2 = this.m_channelOrderMapping[this.m_normalizedRange.Top + i];
				for (int j = 0; j < this.m_normalizedRange.Width; j++)
				{
					buffer[i, j] = this.m_sequence.EventValues[num2, this.m_normalizedRange.Left + j];
				}
			}
			return buffer;
		}

		private Rectangle CellsToPixels(Rectangle relativeCells)
		{
			Rectangle rectangle = new Rectangle();
			rectangle.X = (Math.Min(relativeCells.Left, relativeCells.Right) * this.m_periodPixelWidth) + 1;
			rectangle.Y = (Math.Min(relativeCells.Top, relativeCells.Bottom) * this.m_gridRowHeight) + 1;
			rectangle.Width = ((Math.Abs(relativeCells.Width) + 1) * this.m_periodPixelWidth) - 1;
			rectangle.Height = ((Math.Abs(relativeCells.Height) + 1) * this.m_gridRowHeight) - 1;
			return rectangle;
		}

		private bool ChannelClickValid()
		{
			bool flag = false;
			if (this.pictureBoxChannels.PointToClient(Control.MousePosition).Y > this.pictureBoxTime.Height)
			{
				this.m_selectedLineIndex = this.vScrollBar1.Value + ((this.pictureBoxChannels.PointToClient(Control.MousePosition).Y - this.pictureBoxTime.Height) / this.m_gridRowHeight);
				if (this.m_selectedLineIndex < this.m_sequence.ChannelCount)
				{
					this.m_editingChannelSortedIndex = this.m_channelOrderMapping[this.m_selectedLineIndex];
					flag = (this.m_editingChannelSortedIndex >= 0) && (this.m_editingChannelSortedIndex < this.m_sequence.ChannelCount);
				}
			}
			if (flag)
			{
				this.m_currentlyEditingChannel = this.SelectedChannel = this.m_sequence.Channels[this.m_editingChannelSortedIndex];
			}
			return flag;
		}

		private void ChannelCountChanged()
		{
			base.IsDirty = true;
			this.textBoxChannelCount.Text = this.m_sequence.ChannelCount.ToString();
			this.pictureBoxChannels.Refresh();
			this.pictureBoxGrid.Refresh();
		}

		private void channelOutputMaskToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.EditSequenceChannelMask();
		}

		private void channelPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ShowChannelProperties();
		}

		private DialogResult CheckDirty()
		{
			DialogResult none = DialogResult.None;
			if (base.IsDirty)
			{
				string str = (this.m_sequence.Name == null) ? "this unnamed sequence" : this.m_sequence.Name;
				none = MessageBox.Show(string.Format("Save changes to {0}?", str), Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (none == DialogResult.Yes)
				{
					this.m_systemInterface.InvokeSave(this);
				}
			}
			return none;
		}

		private void CheckMaximums()
		{
			bool flag = false;
			for (int i = 0; i < this.m_sequence.ChannelCount; i++)
			{
				int num3 = this.m_channelOrderMapping[i];
				for (int j = 0; j < this.m_sequence.TotalEventPeriods; j++)
				{
					byte num2 = this.m_sequence.EventValues[num3, j];
					if (num2 != 0)
					{
						this.m_sequence.EventValues[num3, j] = Math.Min(num2, this.m_sequence.MaximumLevel);
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
			for (int i = 0; i < this.m_sequence.ChannelCount; i++)
			{
				int num3 = this.m_channelOrderMapping[i];
				for (int j = 0; j < this.m_sequence.TotalEventPeriods; j++)
				{
					byte num2 = this.m_sequence.EventValues[num3, j];
					this.m_sequence.EventValues[num3, j] = Math.Max(num2, this.m_sequence.MinimumLevel);
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
			if (this.m_selectedEventIndex != -1)
			{
				Rectangle blockAffected = new Rectangle(this.m_selectedEventIndex, 0, 1, this.m_sequence.ChannelCount);
				this.AddUndoItem(blockAffected, UndoOriginalBehavior.Overwrite);
				for (int i = 0; i < this.m_sequence.ChannelCount; i++)
				{
					this.m_sequence.EventValues[this.m_channelOrderMapping[i], this.m_selectedEventIndex] = this.m_sequence.MinimumLevel;
				}
				this.InvalidateRect(blockAffected);
			}
		}

		private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Clear all events in the sequence?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Rectangle normalizedRange = this.m_normalizedRange;
				this.m_normalizedRange = new Rectangle(0, 0, this.m_sequence.TotalEventPeriods, this.m_sequence.ChannelCount);
				this.TurnCellsOff();
				this.m_normalizedRange = normalizedRange;
				this.pictureBoxGrid.Refresh();
			}
		}

		private void ClearChannel(int lineIndex)
		{
			this.AddUndoItem(new Rectangle(0, lineIndex, this.m_sequence.TotalEventPeriods, 1), UndoOriginalBehavior.Overwrite);
			for (int i = 0; i < this.m_sequence.TotalEventPeriods; i++)
			{
				this.m_sequence.EventValues[this.m_editingChannelSortedIndex, i] = this.m_sequence.MinimumLevel;
			}
			this.pictureBoxGrid.Refresh();
		}

		private void clearChannelEventsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ClearChannel(this.m_selectedLineIndex);
		}

		private void contextMenuChannels_Opening(object sender, CancelEventArgs e)
		{
			e.Cancel = !this.ChannelClickValid();
		}

		private void contextMenuGrid_Opening(object sender, CancelEventArgs e)
		{
			if (this.m_currentlyEditingChannel == null)
			{
				e.Cancel = true;
			}
			this.saveAsARoutineToolStripMenuItem.Enabled = this.m_normalizedRange.Width >= 1;
		}

		private void contextMenuTime_Opening(object sender, CancelEventArgs e)
		{
			this.m_selectedEventIndex = this.hScrollBar1.Value + (this.pictureBoxTime.PointToClient(Control.MousePosition).X / this.m_periodPixelWidth);
		}

		private void CopyCells()
		{
			this.m_systemInterface.Clipboard = this.CellsToArray();
		}

		private void copyChannelEventsToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			byte[,] buffer = new byte[1, this.m_sequence.TotalEventPeriods];
			for (int i = 0; i < this.m_sequence.TotalEventPeriods; i++)
			{
				buffer[0, i] = this.m_sequence.EventValues[this.m_editingChannelSortedIndex, i];
			}
			this.m_systemInterface.Clipboard = buffer;
		}

		private void copyChannelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ChannelCopyDialog(this.m_affectGrid, this.m_sequence, this.m_channelOrderMapping).Show();
		}

		private void createFromSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Profile objectInContext = new Profile();
			objectInContext.InheritChannelsFrom(this.m_sequence);
			objectInContext.InheritPlugInDataFrom(this.m_sequence);
			objectInContext.InheritSortsFrom(this.m_sequence);
			ProfileManagerDialog dialog = new ProfileManagerDialog(objectInContext);
			if ((dialog.ShowDialog() == DialogResult.OK) && (MessageBox.Show("Do you want to attach this sequence to the new profile?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
			{
				this.SetProfile(objectInContext);
			}
			dialog.Dispose();
		}

		private void currentProgramsSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SequenceSettingsDialog dialog = new SequenceSettingsDialog(this.m_sequence);
			int minimumLevel = this.m_sequence.MinimumLevel;
			int maximumLevel = this.m_sequence.MaximumLevel;
			int eventPeriod = this.m_sequence.EventPeriod;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				if (minimumLevel != this.m_sequence.MinimumLevel)
				{
					this.CheckMinimums();
					if (this.m_drawingLevel < this.m_sequence.MinimumLevel)
					{
						this.SetDrawingLevel(this.m_sequence.MinimumLevel);
						MessageBox.Show("Drawing level was below the sequence minimum, so it has been adjusted.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
				if (maximumLevel != this.m_sequence.MaximumLevel)
				{
					this.CheckMaximums();
					if (this.m_drawingLevel > this.m_sequence.MaximumLevel)
					{
						this.SetDrawingLevel(this.m_sequence.MaximumLevel);
						MessageBox.Show("Drawing level was above the sequence maximum, so it has been adjusted.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
				if (eventPeriod != this.m_sequence.EventPeriod)
				{
					this.HScrollCheck();
					this.ParseAudioWaveform();
					this.pictureBoxTime.Refresh();
				}
				this.pictureBoxGrid.Refresh();
			}
			dialog.Dispose();
		}

		private void DeleteChannelFromSort(int naturalIndex)
		{
			this.m_channelOrderMapping.Remove(naturalIndex);
			for (int i = 0; i < this.m_channelOrderMapping.Count; i++)
			{
				if (this.m_channelOrderMapping[i] > naturalIndex)
				{
					List<int> list;
					int num2;
					(list = this.m_channelOrderMapping)[num2 = i] = list[num2] - 1;
				}
			}
		}

		private void detachSequenceFromItsProfileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Do you wish to detach this sequence from its profile?\n\nThis will not cause anything to be deleted.\nVixen will attempt to reload channel and plugin data from the sequence.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				this.SetProfile((Profile) null);
			}
		}

		private void DimmingShimmerGenerator(byte[,] values, params int[] effectParameters)
		{
			int num = effectParameters[0];
			if (num != 0)
			{
				int num2 = (int) Math.Round((double) ((1000f / ((float) this.m_sequence.EventPeriod)) / ((float) num)), MidpointRounding.AwayFromZero);
				if (num2 != 0)
				{
					int length = values.GetLength(1);
					int num8 = values.GetLength(0);
					Random random = new Random();
					for (int i = 0; i < length; i += num2)
					{
						int num4 = Math.Min(length, i + num2) - i;
						byte num6 = (byte) Math.Max(random.NextDouble() * this.m_sequence.MaximumLevel, (double) this.m_sequence.MinimumLevel);
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
			this.toolStripButtonWaveform.Enabled = false;
			this.toolStripComboBoxWaveformZoom.Enabled = false;
			this.toolStripLabelWaveformZoom.Enabled = false;
		}

		private void DisableWaveformDisplay()
		{
			this.toolStripButtonWaveform.Checked = false;
			this.m_waveformPixelData = null;
			this.m_waveformPCMData = null;
			this.pictureBoxTime.Height = 60;
			this.pictureBoxTime.Refresh();
			this.pictureBoxChannels.Refresh();
			this.DisableWaveformButton();
		}

		private void DisjointedInsert(int x, int y, int width, int height, int[] channelIndexes)
		{
			for (int i = 0; i < height; i++)
			{
				int num = channelIndexes[i];
				for (int j = ((this.m_sequence.TotalEventPeriods - x) - width) - 1; j >= 0; j--)
				{
					this.m_sequence.EventValues[num, (j + x) + width] = this.m_sequence.EventValues[num, j + x];
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
					this.m_sequence.EventValues[num, j + x] = data[i, j];
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
				while (num2 < ((this.m_sequence.TotalEventPeriods - x) - width))
				{
					this.m_sequence.EventValues[num, num2 + x] = this.m_sequence.EventValues[num, (num2 + x) + width];
					num2++;
				}
			}
			for (num3 = 0; num3 < height; num3++)
			{
				num = channelIndexes[num3];
				for (num2 = 0; num2 < width; num2++)
				{
					this.m_sequence.EventValues[num, (this.m_sequence.TotalEventPeriods - width) + num2] = this.m_sequence.MinimumLevel;
				}
			}
		}

		

		private void DrawSelectedRange()
		{
			Point point = new Point();
			Point point2 = new Point();
			point.X = (this.m_normalizedRange.Left - this.hScrollBar1.Value) * this.m_periodPixelWidth;
			point.Y = (this.m_normalizedRange.Top - this.vScrollBar1.Value) * this.m_gridRowHeight;
			point2.X = (this.m_normalizedRange.Right - this.hScrollBar1.Value) * this.m_periodPixelWidth;
			point2.Y = (this.m_normalizedRange.Bottom - this.vScrollBar1.Value) * this.m_gridRowHeight;
			this.m_selectionRectangle.X = point.X;
			this.m_selectionRectangle.Y = point.Y;
			this.m_selectionRectangle.Width = point2.X - point.X;
			this.m_selectionRectangle.Height = point2.Y - point.Y;
			if (this.m_selectionRectangle.Width == 0)
			{
				this.m_selectionRectangle.Width = this.m_periodPixelWidth;
			}
			if (this.m_selectionRectangle.Height == 0)
			{
				this.m_selectionRectangle.Height = this.m_gridRowHeight;
			}
			this.pictureBoxGrid.Invalidate(this.m_selectionRectangle);
			this.pictureBoxGrid.Update();
		}

		private void EditSequenceChannelMask()
		{
			ChannelOutputMaskDialog dialog = new ChannelOutputMaskDialog(this.m_sequence.Channels);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				foreach (VixenPlus.Channel channel in this.m_sequence.Channels)
				{
					channel.Enabled = true;
				}
				foreach (int num in dialog.DisabledChannels)
				{
					this.m_sequence.Channels[num].Enabled = false;
				}
				base.IsDirty = true;
			}
			dialog.Dispose();
		}

		private void EnableWaveformButton()
		{
			if (this.m_sequence.Audio != null)
			{
				this.toolStripButtonWaveform.Enabled = true;
				if (this.toolStripButtonWaveform.Checked)
				{
					this.toolStripComboBoxWaveformZoom.Enabled = true;
					this.toolStripLabelWaveformZoom.Enabled = true;
				}
			}
		}

		private void EraseRectangleEntity(Rectangle rect)
		{
			rect.Offset(-this.hScrollBar1.Value, -this.vScrollBar1.Value);
			Rectangle rc = this.CellsToPixels(rect);
			rect.X = -1;
			this.pictureBoxGrid.Invalidate(rc);
		}

		private void EraseSelectedRange()
		{
			Rectangle rc = this.SelectionToRectangle();
			this.m_normalizedRange.Width = this.m_selectedRange.Width = 0;
			this.pictureBoxGrid.Invalidate(rc);
		}

		private void exportChannelNamesListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string path = Path.Combine(Paths.ImportExportPath, this.m_sequence.Name + "_channels.txt");
			StreamWriter writer = new StreamWriter(path);
			try
			{
				foreach (VixenPlus.Channel channel in this.m_sequence.Channels)
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
			this.AddUndoItem(new Rectangle(0, lineIndex, this.m_sequence.TotalEventPeriods, 1), UndoOriginalBehavior.Overwrite);
			for (int i = 0; i < this.m_sequence.TotalEventPeriods; i++)
			{
				this.m_sequence.EventValues[this.m_editingChannelSortedIndex, i] = this.m_drawingLevel;
			}
			this.pictureBoxGrid.Refresh();
			base.IsDirty = true;
		}

		private void flattenProfileIntoSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("This will detach the sequence from the profile and bring the profile data into the sequence.\nIs this what you want to do?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				Profile profile = this.m_sequence.Profile;
				this.m_sequence.Profile = null;
				List<VixenPlus.Channel> list = new List<VixenPlus.Channel>();
				list.AddRange(profile.Channels);
				this.m_sequence.Channels = list;
				this.m_sequence.Sorts.LoadFrom(profile.Sorts);
				this.m_sequence.PlugInData.LoadFromXml(profile.PlugInData.RootNode.ParentNode);
				base.IsDirty = true;
				this.ReactToProfileAssignment();
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
			if (blockAffected.Right > this.m_sequence.TotalEventPeriods)
			{
				blockAffected.Width = this.m_sequence.TotalEventPeriods - blockAffected.Left;
			}
			if (blockAffected.Bottom > this.m_sequence.ChannelCount)
			{
				blockAffected.Height = this.m_sequence.ChannelCount - blockAffected.Top;
			}
			byte[,] buffer = new byte[blockAffected.Height, blockAffected.Width];
			for (int i = 0; i < blockAffected.Height; i++)
			{
				int num3 = this.m_channelOrderMapping[blockAffected.Y + i];
				for (int j = 0; j < blockAffected.Width; j++)
				{
					buffer[i, j] = this.m_sequence.EventValues[num3, blockAffected.X + j];
				}
			}
			return buffer;
		}

		private int GetCellIntensity(int cellX, int cellY, out string intensityText)
		{
			if ((cellX >= 0) && (cellY >= 0))
			{
				int num;
				if (this.m_actualLevels)
				{
					num = this.m_sequence.EventValues[this.m_channelOrderMapping[cellY], cellX];
					intensityText = string.Format("{0}", num);
					return num;
				}
				num = (int) Math.Round((double) ((this.m_sequence.EventValues[this.m_channelOrderMapping[cellY], cellX] * 100f) / 255f), MidpointRounding.AwayFromZero);
				intensityText = string.Format("{0}%", num);
				return num;
			}
			intensityText = "";
			return 0;
		}

		private VixenPlus.Channel GetChannelAt(Point point)
		{
			return this.GetChannelAtSortedIndex(this.GetLineIndexAt(point));
		}

		private VixenPlus.Channel GetChannelAtSortedIndex(int index)
		{
			if (index < this.m_channelOrderMapping.Count)
			{
				return this.m_sequence.Channels[this.m_channelOrderMapping[index]];
			}
			return null;
		}

		private Rectangle GetChannelNameRect(VixenPlus.Channel channel)
		{
			if (channel != null)
			{
				return new Rectangle(0, ((this.GetChannelSortedIndex(channel) - this.vScrollBar1.Value) * this.m_gridRowHeight) + this.pictureBoxTime.Height, this.pictureBoxChannels.Width, this.m_gridRowHeight);
			}
			return Rectangle.Empty;
		}

		private int GetChannelNaturalIndex(VixenPlus.Channel channel)
		{
			return this.m_sequence.Channels.IndexOf(channel);
		}

		private int GetChannelSortedIndex(VixenPlus.Channel channel)
		{
			return this.m_channelOrderMapping.IndexOf(this.m_sequence.Channels.IndexOf(channel));
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
			return (((point.Y - this.pictureBoxTime.Height) / this.m_gridRowHeight) + this.vScrollBar1.Value);
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
			this.SetAudioSpeed(0.5f);
		}

		private void hScrollBar1_ValueChanged(object sender, EventArgs e)
		{
			this.pictureBoxGrid.Refresh();
			this.pictureBoxTime.Refresh();
		}

		private void HScrollCheck()
		{
			if (this.m_periodPixelWidth != 0)
			{
				this.m_visibleEventPeriods = this.pictureBoxGrid.Width / this.m_periodPixelWidth;
				this.hScrollBar1.LargeChange = this.m_visibleEventPeriods;
				this.hScrollBar1.Maximum = Math.Max(0, this.m_sequence.TotalEventPeriods - 1);
				this.hScrollBar1.Enabled = this.m_visibleEventPeriods < this.m_sequence.TotalEventPeriods;
				if (!this.hScrollBar1.Enabled)
				{
					this.hScrollBar1.Value = this.hScrollBar1.Minimum;
				}
				else if ((this.hScrollBar1.Value + this.m_visibleEventPeriods) > this.m_sequence.TotalEventPeriods)
				{
					this.m_selectedRange.X += this.m_visibleEventPeriods - this.m_sequence.TotalEventPeriods;
					this.m_normalizedRange.X += this.m_visibleEventPeriods - this.m_sequence.TotalEventPeriods;
					this.hScrollBar1.Value = this.m_sequence.TotalEventPeriods - this.m_visibleEventPeriods;
				}
			}
			if (this.hScrollBar1.Maximum >= 0)
			{
				if (this.hScrollBar1.Value == -1)
				{
					this.hScrollBar1.Value = 0;
				}
				if (this.hScrollBar1.Minimum == -1)
				{
					this.hScrollBar1.Minimum = 0;
				}
			}
		}

		private void importChannelNamesListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.m_sequence.Profile != null)
			{
				MessageBox.Show("Can't import channel names when attached to a profile.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				this.openFileDialog1.Filter = "Text file | *.txt";
				this.openFileDialog1.DefaultExt = "txt";
				this.openFileDialog1.InitialDirectory = Paths.ImportExportPath;
				this.openFileDialog1.FileName = string.Empty;
				if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					StreamReader reader = new StreamReader(this.openFileDialog1.FileName);
					List<string> list = new List<string>();
					try
					{
						string str;
						while ((str = reader.ReadLine()) != null)
						{
							list.Add(str);
						}
						this.SetChannelCount(list.Count);
						for (int i = 0; i < list.Count; i++)
						{
							this.m_sequence.Channels[i].Name = list[i];
						}
						this.pictureBoxChannels.Refresh();
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
			this.m_initializing = true;
			this.InitializeComponent();
			this.m_initializing = false;
			int num = this.pictureBoxTime.Height - 10;
			this.m_gridRowHeight = this.m_preferences.GetInteger("MaxRowHeight");
			this.m_periodPixelWidth = this.m_preferences.GetInteger("MaxColumnWidth");
			this.m_showPositionMarker = this.m_preferences.GetBoolean("ShowPositionMarker");
			this.m_autoScrolling = this.m_preferences.GetBoolean("AutoScrolling");
			this.m_intensityLargeDelta = this.m_preferences.GetInteger("IntensityLargeDelta");
			this.m_showingGradient = !this.m_preferences.GetBoolean("BarLevels");
			this.m_channelBackBrush = new SolidBrush(Color.White);
			this.m_timeBackBrush = new SolidBrush(Color.FromArgb(0xff, 0xff, 0xe0));
			this.m_gridBackBrush = new SolidBrush(Color.FromArgb(0xc0, 0xc0, 0xc0));
			this.m_arrowBitmap = new Bitmap(this.pictureBoxOutputArrow.Image);
			this.m_arrowBitmap.MakeTransparent(this.m_arrowBitmap.GetPixel(0, 0));
			this.m_gridGraphics = this.pictureBoxGrid.CreateGraphics();
			this.m_dimmingShimmerGenerator = new FrequencyEffectGenerator(this.DimmingShimmerGenerator);
			this.m_sparkleGenerator = new FrequencyEffectGenerator(this.SparkleGenerator);
			this.m_intensityAdjustDialog = new IntensityAdjustDialog(this.m_actualLevels);
			this.m_intensityAdjustDialog.VisibleChanged += new EventHandler(this.m_intensityAdjustDialog_VisibleChanged);
			this.m_affectGrid = new AffectGridDelegate(this.AffectGrid);
			this.m_pluginCheckHandler = new EventHandler(this.plugInItem_CheckedChanged);
			this.m_channelOrderMapping = new List<int>();
			for (int i = 0; i < this.m_sequence.ChannelCount; i++)
			{
				this.m_channelOrderMapping.Add(i);
			}
			this.ReadFromSequence();
			this.m_executionContextHandle = this.m_executionInterface.RequestContext(true, false, this);
			this.toolStripComboBoxColumnZoom.SelectedIndex = this.toolStripComboBoxColumnZoom.Items.Count - 1;
			this.toolStripComboBoxRowZoom.SelectedIndex = this.toolStripComboBoxRowZoom.Items.Count - 1;
			this.SyncAudioButton();
			this.m_mouseWheelVerticalDelta = this.m_preferences.GetInteger("MouseWheelVerticalDelta");
			this.m_mouseWheelHorizontalDelta = this.m_preferences.GetInteger("MouseWheelHorizontalDelta");
			if (this.m_preferences.GetBoolean("SaveZoomLevels"))
			{
				int index = this.toolStripComboBoxColumnZoom.Items.IndexOf(this.m_preferences.GetChildString("SaveZoomLevels", "column"));
				if (index != -1)
				{
					this.toolStripComboBoxColumnZoom.SelectedIndex = index;
				}
				index = this.toolStripComboBoxRowZoom.Items.IndexOf(this.m_preferences.GetChildString("SaveZoomLevels", "row"));
				if (index != -1)
				{
					this.toolStripComboBoxRowZoom.SelectedIndex = index;
				}
			}
			if (this.m_sequence.WindowWidth != 0)
			{
				base.Width = this.m_sequence.WindowWidth;
			}
			if (this.m_sequence.WindowHeight != 0)
			{
				base.Height = this.m_sequence.WindowHeight;
			}
			if (this.m_sequence.ChannelWidth != 0)
			{
				this.splitContainer1.SplitterDistance = this.m_sequence.ChannelWidth;
			}
			this.toolStripComboBoxWaveformZoom.SelectedItem = "100%";
			this.SetDrawingLevel(this.m_sequence.MaximumLevel);
			this.m_executionInterface.SetSynchronousContext(this.m_executionContextHandle, this.m_sequence);
			this.UpdateLevelDisplay();
			base.IsDirty = false;
			this.pictureBoxChannels.AllowDrop = true;
		}

		

		private void InsertChannelIntoSort(int naturalIndex, int sortedIndex)
		{
			for (int i = 0; i < this.m_channelOrderMapping.Count; i++)
			{
				if (this.m_channelOrderMapping[i] >= naturalIndex)
				{
					List<int> list;
					int num2;
					(list = this.m_channelOrderMapping)[num2 = i] = list[num2] + 1;
				}
			}
			this.m_channelOrderMapping.Insert(sortedIndex, naturalIndex);
		}

		private void IntensityAdjustDialogCheck()
		{
			if (!this.m_intensityAdjustDialog.Visible)
			{
				this.m_intensityAdjustDialog.Show();
				base.BringToFront();
			}
		}

		private void InvalidateRect(Rectangle rect)
		{
			rect.X -= this.hScrollBar1.Value;
			rect.Y -= this.vScrollBar1.Value;
			this.m_selectionRectangle.X = Math.Min(rect.Left, rect.Right) * this.m_periodPixelWidth;
			this.m_selectionRectangle.Y = Math.Min(rect.Top, rect.Bottom) * this.m_gridRowHeight;
			this.m_selectionRectangle.Width = Math.Abs((int) ((rect.Width + 1) * this.m_periodPixelWidth));
			this.m_selectionRectangle.Height = Math.Abs((int) ((rect.Height + 1) * this.m_gridRowHeight));
			if (this.m_selectionRectangle.Width == 0)
			{
				this.m_selectionRectangle.Width = this.m_periodPixelWidth;
			}
			if (this.m_selectionRectangle.Height == 0)
			{
				this.m_selectionRectangle.Height = this.m_gridRowHeight;
			}
			this.pictureBoxGrid.Invalidate(this.m_selectionRectangle);
			this.pictureBoxGrid.Update();
		}

		private int LineIndexToChannelIndex(int lineIndex)
		{
			if ((lineIndex >= 0) && (lineIndex < this.m_channelOrderMapping.Count))
			{
				return this.m_channelOrderMapping[lineIndex];
			}
			return -1;
		}

		private void loadARoutineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			byte[,] routine = this.GetRoutine();
			if (routine != null)
			{
				this.AddUndoItem(new Rectangle(this.m_normalizedRange.X, this.m_normalizedRange.Y, routine.GetLength(1), routine.GetLength(0)), UndoOriginalBehavior.Overwrite);
				this.ArrayToCells(routine);
				this.pictureBoxGrid.Refresh();
			}
		}

		private void loadRoutineToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.m_systemInterface.Clipboard = this.GetRoutine();
		}

		private void LoadSequencePlugins()
		{
			this.m_systemInterface.VerifySequenceHardwarePlugins(this.m_sequence);
			int num = 0;
			bool flag = false;
			this.toolStripDropDownButtonPlugins.DropDownItems.Clear();
			foreach (XmlNode node in this.m_sequence.PlugInData.GetAllPluginData())
			{
				ToolStripMenuItem item = new ToolStripMenuItem(string.Format("{0} ({1}-{2})", node.Attributes["name"].Value, node.Attributes["from"].Value, node.Attributes["to"].Value));
				item.Checked = bool.Parse(node.Attributes["enabled"].Value);
				item.CheckOnClick = true;
				item.CheckedChanged += this.m_pluginCheckHandler;
				item.Tag = num.ToString();
				num++;
				this.toolStripDropDownButtonPlugins.DropDownItems.Add(item);
				flag |= item.Checked;
			}
			if (this.toolStripDropDownButtonPlugins.DropDownItems.Count > 0)
			{
				this.toolStripDropDownButtonPlugins.DropDownItems.Add("-");
				this.toolStripDropDownButtonPlugins.DropDownItems.Add("Select all", null, new EventHandler(this.selectAllToolStripMenuItem_Click));
				this.toolStripDropDownButtonPlugins.DropDownItems.Add("Unselect all", null, new EventHandler(this.unselectAllToolStripMenuItem_Click));
			}
		}

		private void LoadSequenceSorts()
		{
			this.toolStripComboBoxChannelOrder.BeginUpdate();
			string item = (string) this.toolStripComboBoxChannelOrder.Items[0];
			string str2 = (string) this.toolStripComboBoxChannelOrder.Items[this.toolStripComboBoxChannelOrder.Items.Count - 1];
			this.toolStripComboBoxChannelOrder.Items.Clear();
			this.toolStripComboBoxChannelOrder.Items.Add(item);
			foreach (VixenPlus.SortOrder order in this.m_sequence.Sorts)
			{
				this.toolStripComboBoxChannelOrder.Items.Add(order);
			}
			this.toolStripComboBoxChannelOrder.Items.Add(str2);
			if (this.m_sequence.LastSort != -1)
			{
				this.toolStripComboBoxChannelOrder.SelectedIndex = this.m_sequence.LastSort + 1;
			}
			this.toolStripComboBoxChannelOrder.EndUpdate();
		}

		private void m_intensityAdjustDialog_VisibleChanged(object sender, EventArgs e)
		{
			if (!this.m_intensityAdjustDialog.Visible)
			{
				int left;
				int num2;
				int num4;
				int num8;
				this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
				int delta = this.m_intensityAdjustDialog.Delta;
				int bottom = this.m_normalizedRange.Bottom;
				int right = this.m_normalizedRange.Right;
				if (this.m_actualLevels)
				{
					for (num8 = this.m_normalizedRange.Top; num8 < bottom; num8++)
					{
						num2 = this.m_channelOrderMapping[num8];
						if (this.m_sequence.Channels[num2].Enabled)
						{
							left = this.m_normalizedRange.Left;
							while (left < right)
							{
								num4 = this.m_sequence.EventValues[num2, left] + delta;
								num4 = Math.Max(Math.Min(num4, this.m_sequence.MaximumLevel), this.m_sequence.MinimumLevel);
								this.m_sequence.EventValues[num2, left] = (byte) num4;
								left++;
							}
						}
					}
				}
				else
				{
					for (num8 = this.m_normalizedRange.Top; num8 < bottom; num8++)
					{
						num2 = this.m_channelOrderMapping[num8];
						if (this.m_sequence.Channels[num2].Enabled)
						{
							for (left = this.m_normalizedRange.Left; left < right; left++)
							{
								num4 = ((int) Math.Round((double) ((this.m_sequence.EventValues[num2, left] * 100f) / 255f), MidpointRounding.AwayFromZero)) + delta;
								int num5 = (int) Math.Round((double) ((((float) num4) / 100f) * 255f), MidpointRounding.AwayFromZero);
								num5 = Math.Max(Math.Min(num5, this.m_sequence.MaximumLevel), this.m_sequence.MinimumLevel);
								this.m_sequence.EventValues[num2, left] = (byte) num5;
							}
						}
					}
				}
				this.pictureBoxGrid.Refresh();
			}
			else
			{
				this.m_intensityAdjustDialog.LargeDelta = this.m_intensityLargeDelta;
			}
		}

		private void m_positionTimer_Tick(object sender, EventArgs e)
		{
			int num;
			if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle, out num) == 0)
			{
				this.ProgramEnded();
			}
			else
			{
				int num2 = num / this.m_sequence.EventPeriod;
				if (num2 != this.m_position)
				{
					this.toolStripLabelExecutionPoint.Text = string.Format("{0:d2}:{1:d2}", num / 0xea60, (num % 0xea60) / 0x3e8);
					this.m_previousPosition = this.m_position;
					this.m_position = num2;
					if ((this.m_position < this.hScrollBar1.Value) || (this.m_position > (this.hScrollBar1.Value + this.m_visibleEventPeriods)))
					{
						if (this.m_autoScrolling)
						{
							if (this.m_position != -1)
							{
								if (this.m_position >= this.m_sequence.TotalEventPeriods)
								{
									this.m_previousPosition = this.m_position = this.m_sequence.TotalEventPeriods - 1;
								}
								this.hScrollBar1.Value = this.m_position;
								this.toolStripLabelExecutionPoint.Text = string.Format("{0:d2}:{1:d2}", num / 0xea60, (num % 0xea60) / 0x3e8);
							}
						}
						else
						{
							this.UpdateProgress();
						}
					}
					else if (this.m_showPositionMarker)
					{
						this.UpdateProgress();
					}
					else
					{
						this.toolStripLabelExecutionPoint.Text = string.Format("{0:d2}:{1:d2}", num / 0xea60, (num % 0xea60) / 0x3e8);
					}
				}
			}
		}

		private void maxToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ArithmeticPaste(ArithmeticOperation.Max);
		}

		private void minToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ArithmeticPaste(ArithmeticOperation.Min);
		}

		public override EventSequence New()
		{
			return this.New(new EventSequence(this.m_systemInterface.UserPreferences));
		}

		public override EventSequence New(EventSequence seedSequence)
		{
			this.m_sequence = seedSequence;
			this.m_preferences = this.m_systemInterface.UserPreferences;
			this.m_dataNode = this.m_sequence.Extensions[this.FileExtension];
			this.Init();
			if (this.m_sequence.Name == null)
			{
				this.Text = "Unnamed sequence";
			}
			else
			{
				this.Text = this.m_sequence.Name;
			}
			return this.m_sequence;
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
			this.SetAudioSpeed(1f);
		}

		private void normalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.normalToolStripMenuItem.Checked = true;
			this.paintFromClipboardToolStripMenuItem.Checked = false;
		}

		public override void Notify(Notification notification, object data)
		{
			switch (notification)
			{
				case Notification.PreferenceChange:
					this.UpdateRowHeight();
					this.UpdateColumnWidth();
					this.m_showPositionMarker = this.m_preferences.GetBoolean("ShowPositionMarker");
					this.m_autoScrolling = this.m_preferences.GetBoolean("AutoScrolling");
					this.m_mouseWheelVerticalDelta = this.m_preferences.GetInteger("MouseWheelVerticalDelta");
					this.m_mouseWheelHorizontalDelta = this.m_preferences.GetInteger("MouseWheelHorizontalDelta");
					this.m_intensityLargeDelta = this.m_preferences.GetInteger("IntensityLargeDelta");
					this.RefreshAll();
					break;

				case Notification.KeyDown:
					this.StandardSequence_KeyDown(null, (KeyEventArgs) data);
					break;

				case Notification.SequenceChange:
					this.RefreshAll();
					base.IsDirty = true;
					break;

				case Notification.ProfileChange:
				{
					VixenPlus.SortOrder currentOrder = this.m_sequence.Sorts.CurrentOrder;
					this.m_sequence.ReloadProfile();
					this.m_sequence.Sorts.CurrentOrder = currentOrder;
					this.LoadSequenceSorts();
					this.RefreshAll();
					break;
				}
			}
		}

		public override void OnDirtyChanged(EventArgs e)
		{
			base.OnDirtyChanged(e);
			this.toolStripButtonSave.Enabled = base.IsDirty;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			bool flag = (Control.ModifierKeys & Keys.Shift) == Keys.None;
			if (this.m_preferences.GetBoolean("FlipScrollBehavior"))
			{
				flag = !flag;
			}
			if (flag)
			{
				if (e.Delta > 0)
				{
					if (this.vScrollBar1.Value >= (this.vScrollBar1.Minimum + this.m_mouseWheelVerticalDelta))
					{
						this.vScrollBar1.Value -= this.m_mouseWheelVerticalDelta;
					}
					else
					{
						this.vScrollBar1.Value = this.vScrollBar1.Minimum;
					}
				}
				else if (this.vScrollBar1.Value <= (this.vScrollBar1.Maximum - (this.m_visibleRowCount + this.m_mouseWheelVerticalDelta)))
				{
					this.vScrollBar1.Value += this.m_mouseWheelVerticalDelta;
				}
				else
				{
					this.vScrollBar1.Value = Math.Max((this.vScrollBar1.Maximum - this.m_visibleRowCount) + 1, 0);
				}
			}
			else if (e.Delta > 0)
			{
				if (this.hScrollBar1.Value >= (this.hScrollBar1.Minimum + this.m_mouseWheelHorizontalDelta))
				{
					this.hScrollBar1.Value -= this.m_mouseWheelHorizontalDelta;
				}
				else
				{
					this.hScrollBar1.Value = this.hScrollBar1.Minimum;
				}
			}
			else if (this.hScrollBar1.Value <= (this.hScrollBar1.Maximum - (this.m_visibleEventPeriods + this.m_mouseWheelHorizontalDelta)))
			{
				this.hScrollBar1.Value += this.m_mouseWheelHorizontalDelta;
			}
			else
			{
				this.hScrollBar1.Value = Math.Max((this.hScrollBar1.Maximum - this.m_visibleEventPeriods) + 1, 0);
			}
		}

		public override EventSequence Open(string filePath)
		{
			this.m_sequence = new EventSequence(filePath);
			this.Text = this.m_sequence.Name;
			this.m_dataNode = this.m_sequence.Extensions[this.FileExtension];
			this.m_preferences = this.m_systemInterface.UserPreferences;
			this.Init();
			return this.m_sequence;
		}

		private void otherToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SetVariablePlaybackSpeed(new Point(0, 0));
		}

		private void paintFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.normalToolStripMenuItem.Checked = false;
			this.paintFromClipboardToolStripMenuItem.Checked = true;
		}

		private void ParseAudioWaveform()
		{
			string str;
			if ((this.m_sequence.Audio != null) && File.Exists(str = Path.Combine(Paths.AudioPath, this.m_sequence.Audio.FileName)))
			{
				if (this.toolStripButtonWaveform.Checked)
				{
					VixenEditor.ProgressDialog dialog = new VixenEditor.ProgressDialog();
					dialog.Show();
					dialog.Message = "Parsing sound data, please wait...";
					this.Cursor = Cursors.WaitCursor;
					try
					{
						this.m_waveformPCMData = new uint[this.m_sequence.TotalEventPeriods * this.m_periodPixelWidth];
						this.m_waveformPixelData = new uint[this.m_sequence.TotalEventPeriods * this.m_periodPixelWidth];
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
						double num9 = num8 / ((double) this.m_sequence.TotalEventPeriods);
						double num10 = num9 / ((double) this.m_periodPixelWidth);
						double num11 = ((double) this.m_sequence.EventPeriod) / ((double) this.m_periodPixelWidth);
						double num12 = frequency / 1000f;
						int num14 = 0;
						int num15 = 0;
						num14 = 0;
						num15 = 0;
						uint num18 = 0;
						sound.getLength(ref num18, TIMEUNIT.MS);
						int index = 0;
						for (double i = 0.0; (index < this.m_waveformPCMData.Length) && (i < num18); i += num11)
						{
							int startSample = (int) (i * num12);
							uint num16 = this.GetSampleMinMax(startSample, (int) Math.Min(num10, num8 - startSample), sound, bits, channels);
							num14 = Math.Max(num14, (short) (num16 >> 0x10));
							num15 = Math.Min(num15, (short) (num16 & 0xffff));
							this.m_waveformPCMData[index] = num16;
							index++;
						}
						this.m_waveform100PercentAmplitude = this.m_waveformMaxAmplitude = Math.Max(num14, -num15);
						this.PCMToPixels(this.m_waveformPCMData, this.m_waveformPixelData);
						sound.release();
					}
					finally
					{
						this.Cursor = Cursors.Default;
						dialog.Hide();
					}
				}
				else
				{
					this.m_waveformPCMData = null;
					this.m_waveformPixelData = null;
				}
				this.EnableWaveformButton();
			}
			else
			{
				this.DisableWaveformDisplay();
			}
		}

		private void pasteFullChannelEventsFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.m_systemInterface.Clipboard != null)
			{
				this.AddUndoItem(new Rectangle(0, this.m_selectedLineIndex, this.m_systemInterface.Clipboard.GetLength(1), 1), UndoOriginalBehavior.Overwrite);
				int num2 = Math.Min(this.m_systemInterface.Clipboard.GetLength(1), this.m_sequence.TotalEventPeriods);
				for (int i = 0; i < num2; i++)
				{
					this.m_sequence.EventValues[this.m_editingChannelSortedIndex, i] = this.m_systemInterface.Clipboard[0, i];
				}
				base.IsDirty = true;
				this.pictureBoxGrid.Refresh();
			}
		}

		private void PasteOver()
		{
			if (this.m_systemInterface.Clipboard != null)
			{
				this.ArrayToCells(this.m_systemInterface.Clipboard);
				this.pictureBoxGrid.Refresh();
			}
		}

		private void PCMToPixels(uint[] PCMData, uint[] pixelData)
		{
			int length = PCMData.Length;
			short num3 = -32768;
			short num4 = 0x7fff;
			int waveformOffset = this.m_waveformOffset;
			int num6 = -this.m_waveformOffset;
			double num7 = ((double) this.m_waveformMaxAmplitude) / ((double) Math.Max(waveformOffset, num6));
			for (int i = 0; i < length; i++)
			{
				uint num2 = PCMData[i];
				num3 = (short) (((double) Math.Min((short) (num2 >> 0x10), this.m_waveformMaxAmplitude)) / num7);
				num3 = (short) Math.Max((short)num3, (short)0);
				num3 = (short) Math.Min(num3, waveformOffset);
				num4 = (short) (((double) Math.Max((short) (num2 & 0xffff), -this.m_waveformMaxAmplitude)) / num7);
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
				VixenPlus.Channel channelAt = this.GetChannelAt(this.pictureBoxChannels.PointToClient(new Point(e.X, e.Y)));
				if (data != channelAt)
				{
					if (e.Effect == DragDropEffects.Copy)
					{
						this.m_sequence.CopyChannel(data, channelAt);
						this.RefreshAll();
						base.IsDirty = true;
					}
					else if (e.Effect == DragDropEffects.Move)
					{
						int channelSortedIndex;
						int channelNaturalIndex = this.GetChannelNaturalIndex(data);
						this.m_channelOrderMapping.Remove(channelNaturalIndex);
						if (channelAt != null)
						{
							channelSortedIndex = this.GetChannelSortedIndex(channelAt);
						}
						else
						{
							channelSortedIndex = this.m_channelOrderMapping.Count;
						}
						this.m_channelOrderMapping.Insert(channelSortedIndex, channelNaturalIndex);
						this.RefreshAll();
						base.IsDirty = true;
					}
				}
			}
		}

		private void pictureBoxChannels_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(VixenPlus.Channel)))
			{
				Point point = this.pictureBoxChannels.PointToClient(new Point(e.X, e.Y));
				int lineIndexAt = this.GetLineIndexAt(point);
				if (this.vScrollBar1.Enabled)
				{
					if ((this.vScrollBar1.Value > this.vScrollBar1.Minimum) && ((lineIndexAt - this.vScrollBar1.Value) < 2))
					{
						this.vScrollBar1.Value--;
						Thread.Sleep(50);
						lineIndexAt = this.GetLineIndexAt(point);
					}
					else if ((this.vScrollBar1.Value < this.vScrollBar1.Maximum) && ((lineIndexAt - this.vScrollBar1.Value) > (this.m_visibleRowCount - 2)))
					{
						this.vScrollBar1.Value++;
						Thread.Sleep(50);
						lineIndexAt = this.GetLineIndexAt(point);
					}
				}
				if ((Control.ModifierKeys & Keys.Control) != Keys.None)
				{
					if ((lineIndexAt >= 0) && (lineIndexAt < this.m_sequence.ChannelCount))
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
			if (this.ChannelClickValid())
			{
				int num = 2;
				if (e.X < 10)
				{
					num = 0;
				}
				else if (this.m_showingOutputs && (e.X < 50))
				{
					num = 1;
				}
				switch (num)
				{
					case 1:
						this.ReorderChannelOutputs();
						break;

					case 2:
						this.ShowChannelProperties();
						break;
				}
			}
		}

		private void pictureBoxChannels_MouseDown(object sender, MouseEventArgs e)
		{
			this.ChannelClickValid();
			this.m_mouseDownAtInChannels = e.Location;
		}

		private void pictureBoxChannels_MouseMove(object sender, MouseEventArgs e)
		{
			if ((((e.Button & MouseButtons.Left) != MouseButtons.None) && (this.m_mouseDownAtInChannels != Point.Empty)) && ((Math.Abs((int) (e.X - this.m_mouseDownAtInChannels.X)) > 3) || (Math.Abs((int) (e.Y - this.m_mouseDownAtInChannels.Y)) > 3)))
			{
				base.DoDragDrop(this.SelectedChannel, DragDropEffects.Move | DragDropEffects.Copy);
			}
		}

		private void pictureBoxChannels_MouseUp(object sender, MouseEventArgs e)
		{
			this.m_mouseDownAtInChannels = Point.Empty;
		}

		private void pictureBoxChannels_Paint(object sender, PaintEventArgs e)
		{
			VixenPlus.Channel channel;
			int num2;
			int num3;
			int num5;
			e.Graphics.FillRectangle(this.m_channelBackBrush, e.Graphics.VisibleClipBounds);
			int height = this.pictureBoxTime.Height;
			if (this.toolStripComboBoxRowZoom.SelectedIndex <= 4)
			{
				num2 = 0;
			}
			else if (this.toolStripComboBoxRowZoom.SelectedIndex >= 8)
			{
				num2 = 3;
			}
			else
			{
				num2 = 1;
			}
			bool boolean = this.m_preferences.GetBoolean("ShowNaturalChannelNumber");
			int x = boolean ? (((this.m_sequence.ChannelCount.ToString().Length + 1) * 6) + 10) : 10;
			if (!this.m_showingOutputs)
			{
				for (num5 = this.vScrollBar1.Value; (num5 >= 0) && (num5 < this.m_sequence.ChannelCount); num5++)
				{
					num3 = this.m_channelOrderMapping[num5];
					channel = this.m_sequence.Channels[num3];
					if (channel == this.SelectedChannel)
					{
						e.Graphics.FillRectangle(SystemBrushes.Highlight, 0, height, this.pictureBoxChannels.Width, this.m_gridRowHeight);
					}
					else
					{
						e.Graphics.FillRectangle(channel.Brush, 0, height, this.pictureBoxChannels.Width, this.m_gridRowHeight);
					}
					if (boolean)
					{
						e.Graphics.DrawString(string.Format("{0}:", num3 + 1), this.m_channelNameFont, Brushes.Black, 10f, (float) (height + num2));
					}
					if (channel == this.SelectedChannel)
					{
						e.Graphics.DrawString(channel.Name, this.m_channelNameFont, SystemBrushes.HighlightText, (float) x, (float) (height + num2));
					}
					else
					{
						e.Graphics.DrawString(channel.Name, this.m_channelNameFont, Brushes.Black, (float) x, (float) (height + num2));
					}
					height += this.m_gridRowHeight;
				}
			}
			else
			{
				int num8;
				int width = Math.Min(this.m_gridRowHeight - 4, this.pictureBoxOutputArrow.Height);
				int num7 = (this.m_gridRowHeight - width) >> 1;
				SolidBrush brush = new SolidBrush(Color.White);
				if (boolean)
				{
					for (num5 = this.vScrollBar1.Value; (num5 >= 0) && (num5 < this.m_sequence.ChannelCount); num5++)
					{
						num3 = this.m_channelOrderMapping[num5];
						channel = this.m_sequence.Channels[num3];
						brush.Color = Color.FromArgb(0xc0, Color.Gray);
						if (channel == this.SelectedChannel)
						{
							e.Graphics.FillRectangle(SystemBrushes.Highlight, 0, height, this.pictureBoxChannels.Width, this.m_gridRowHeight);
						}
						else
						{
							e.Graphics.FillRectangle(channel.Brush, 0, height, this.pictureBoxChannels.Width, this.m_gridRowHeight);
						}
						e.Graphics.DrawString(string.Format("{0}:", num3 + 1), this.m_channelNameFont, Brushes.Black, 10f, (float) (height + num2));
						e.Graphics.FillRectangle(brush, x, height + 1, 40, this.m_gridRowHeight - 2);
						if (this.toolStripComboBoxRowZoom.SelectedIndex > 4)
						{
							e.Graphics.DrawRectangle(Pens.Black, x, height + 1, 40, this.m_gridRowHeight - 2);
						}
						e.Graphics.DrawImage(this.m_arrowBitmap, x + 2, height + num7, width, width);
						num8 = channel.OutputChannel + 1;
						e.Graphics.DrawString(num8.ToString(), this.m_channelNameFont, Brushes.Black, (float) (x + 0x10), (float) (height + num2));
						e.Graphics.DrawString(channel.Name, this.m_channelNameFont, Brushes.Black, (float) (x + 0x2c), (float) (height + num2));
						height += this.m_gridRowHeight;
					}
				}
				else
				{
					for (num5 = this.vScrollBar1.Value; (num5 >= 0) && (num5 < this.m_sequence.ChannelCount); num5++)
					{
						num3 = this.m_channelOrderMapping[num5];
						channel = this.m_sequence.Channels[num3];
						brush.Color = Color.FromArgb(0xc0, Color.Gray);
						e.Graphics.FillRectangle(channel.Brush, 0, height, this.pictureBoxChannels.Width, this.m_gridRowHeight);
						e.Graphics.FillRectangle(brush, 10, height + 1, 40, this.m_gridRowHeight - 2);
						if (this.toolStripComboBoxRowZoom.SelectedIndex > 4)
						{
							e.Graphics.DrawRectangle(Pens.Black, 10, height + 1, 40, this.m_gridRowHeight - 2);
						}
						e.Graphics.DrawImage(this.m_arrowBitmap, 12, height + num7, width, width);
						num8 = channel.OutputChannel + 1;
						e.Graphics.DrawString(num8.ToString(), this.m_channelNameFont, Brushes.Black, 26f, (float) (height + num2));
						e.Graphics.DrawString(channel.Name, this.m_channelNameFont, Brushes.Black, 54f, (float) (height + num2));
						height += this.m_gridRowHeight;
					}
				}
				brush.Dispose();
			}
			e.Graphics.FillRectangle(Brushes.White, 0, 0, 5, this.pictureBoxChannels.Height);
			if (this.m_mouseChannelCaret != -1)
			{
				e.Graphics.FillRectangle(this.m_channelCaretBrush, 0, ((this.m_mouseChannelCaret - this.vScrollBar1.Value) * this.m_gridRowHeight) + this.pictureBoxTime.Height, 5, this.m_gridRowHeight);
			}
		}

		private void pictureBoxChannels_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
		}

		private void pictureBoxChannels_Resize(object sender, EventArgs e)
		{
			this.pictureBoxChannels.Refresh();
		}

		private void pictureBoxGrid_DoubleClick(object sender, EventArgs e)
		{
			if (this.m_currentlyEditingChannel != null)
			{
				this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
				this.m_sequence.EventValues[this.m_editingChannelSortedIndex, this.m_normalizedRange.X] = (this.m_sequence.EventValues[this.m_editingChannelSortedIndex, this.m_normalizedRange.X] > this.m_sequence.MinimumLevel) ? this.m_sequence.MinimumLevel : this.m_drawingLevel;
				this.UpdateGrid(this.m_gridGraphics, new Rectangle((this.m_normalizedRange.X - this.hScrollBar1.Value) * this.m_periodPixelWidth, (this.m_editingChannelSortedIndex - this.vScrollBar1.Value) * this.m_gridRowHeight, this.m_periodPixelWidth, this.m_gridRowHeight));
			}
		}

		private void pictureBoxGrid_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.m_mouseDownInGrid = true;
				this.m_mouseDownAtInGrid.X = (e.X / this.m_periodPixelWidth) + this.hScrollBar1.Value;
				this.m_mouseDownAtInGrid.Y = (e.Y / this.m_gridRowHeight) + this.vScrollBar1.Value;
				if (this.m_normalizedRange.Width != 0)
				{
					this.EraseSelectedRange();
				}
				if ((Control.ModifierKeys & Keys.Control) != Keys.None)
				{
					this.m_selectedLineIndex = (e.Y / this.m_gridRowHeight) + this.vScrollBar1.Value;
					this.m_editingChannelSortedIndex = this.m_channelOrderMapping[this.m_selectedLineIndex];
					this.m_currentlyEditingChannel = this.m_sequence.Channels[this.m_editingChannelSortedIndex];
					this.m_lineRect.X = this.m_mouseDownAtInGrid.X;
					this.m_lineRect.Y = this.m_mouseDownAtInGrid.Y;
					this.m_lineRect.Width = 0;
					this.m_lineRect.Height = 0;
					this.InvalidateRect(this.m_lineRect);
				}
				else if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
				{
					Rectangle rect = new Rectangle();
					rect.X = this.m_normalizedRange.X;
					rect.Y = this.m_normalizedRange.Y;
					rect.Width = ((this.hScrollBar1.Value + ((int) Math.Floor((double) (((float) e.X) / ((float) this.m_periodPixelWidth))))) - this.m_normalizedRange.Left) + 1;
					if (rect.Width < 0)
					{
						rect.Width--;
					}
					rect.Height = ((this.vScrollBar1.Value + (e.Y / this.m_gridRowHeight)) - this.m_normalizedRange.Top) + 1;
					if (rect.Height < 0)
					{
						rect.Height--;
					}
					if (rect.Bottom > this.m_sequence.ChannelCount)
					{
						rect.Height = this.m_sequence.ChannelCount - rect.Y;
					}
					if (rect.Right > this.m_sequence.TotalEventPeriods)
					{
						rect.Width = this.m_sequence.TotalEventPeriods - rect.X;
					}
					this.m_normalizedRange = this.NormalizeRect(rect);
					this.DrawSelectedRange();
				}
				else if ((((e.Y / this.m_gridRowHeight) + this.vScrollBar1.Value) < this.m_sequence.ChannelCount) && (((e.X / this.m_periodPixelWidth) + this.hScrollBar1.Value) < this.m_sequence.TotalEventPeriods))
				{
					this.m_selectedLineIndex = (e.Y / this.m_gridRowHeight) + this.vScrollBar1.Value;
					this.m_editingChannelSortedIndex = this.m_channelOrderMapping[this.m_selectedLineIndex];
					this.m_currentlyEditingChannel = this.m_sequence.Channels[this.m_editingChannelSortedIndex];
					this.m_selectedRange.X = this.hScrollBar1.Value + ((int) Math.Floor((double) (((float) e.X) / ((float) this.m_periodPixelWidth))));
					this.m_selectedRange.Y = this.m_selectedLineIndex;
					this.m_selectedRange.Width = 1;
					this.m_selectedRange.Height = 1;
					this.m_normalizedRange = this.m_selectedRange;
					this.DrawSelectedRange();
				}
				else
				{
					this.m_currentlyEditingChannel = null;
					this.m_editingChannelSortedIndex = -1;
					this.m_selectedLineIndex = -1;
				}
				this.UpdatePositionLabel(this.m_normalizedRange, false);
			}
		}

		private void pictureBoxGrid_MouseLeave(object sender, EventArgs e)
		{
			this.toolStripLabelCellIntensity.Text = string.Empty;
			this.toolStripLabelCurrentCell.Text = string.Empty;
		}

		private void pictureBoxGrid_MouseMove(object sender, MouseEventArgs e)
		{
			int num8;
			Rectangle rectangle;
			int cellX = e.X / this.m_periodPixelWidth;
			int cellY = e.Y / this.m_gridRowHeight;
			this.toolStripLabelCellIntensity.Text = string.Empty;
			this.toolStripLabelCurrentCell.Text = string.Empty;
			if (cellX < 0)
			{
				cellX = 0;
			}
			if (cellY < 0)
			{
				cellY = 0;
			}
			cellX += this.hScrollBar1.Value;
			cellY += this.vScrollBar1.Value;
			if (cellX >= this.m_sequence.TotalEventPeriods)
			{
				cellX = this.m_sequence.TotalEventPeriods - 1;
			}
			if (cellY >= this.m_sequence.ChannelCount)
			{
				cellY = this.m_sequence.ChannelCount - 1;
			}
			if ((e.Button != MouseButtons.Left) || !this.m_mouseDownInGrid)
			{
				goto Label_0733;
			}
			if (this.m_lineRect.Left == -1)
			{
				int num7 = 0;
				if (e.X > this.pictureBoxGrid.Width)
				{
					num7 |= 0x10;
				}
				else if (e.X < 0)
				{
					num7 |= 0x1000;
				}
				if (e.Y > this.pictureBoxGrid.Height)
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
						this.ScrollSelectionUp(cellX, cellY);
						goto Label_0715;

					case 0x1000:
						this.ScrollSelectionLeft(cellX, cellY);
						goto Label_0715;

					case 0x1100:
						this.ScrollSelectionLeft(cellX, cellY);
						this.ScrollSelectionUp(cellX, cellY);
						goto Label_0715;

					case 0:
						this.EraseSelectedRange();
						if (cellX >= this.m_mouseDownAtInGrid.X)
						{
							if (cellX > this.m_mouseDownAtInGrid.X)
							{
								this.m_selectedRange.Width = (cellX - this.m_selectedRange.Left) + 1;
							}
							else
							{
								this.m_selectedRange.Width = 1;
							}
						}
						else
						{
							this.m_selectedRange.Width = cellX - this.m_selectedRange.Left;
						}
						if (cellY < this.m_mouseDownAtInGrid.Y)
						{
							this.m_selectedRange.Height = cellY - this.m_selectedRange.Top;
						}
						else if (cellY > this.m_mouseDownAtInGrid.Y)
						{
							this.m_selectedRange.Height = (cellY - this.m_selectedRange.Top) + 1;
						}
						else
						{
							this.m_selectedRange.Height = 1;
						}
						this.m_normalizedRange = this.NormalizeRect(this.m_selectedRange);
						this.DrawSelectedRange();
						goto Label_0715;

					case 1:
						this.ScrollSelectionDown(cellX, cellY);
						goto Label_0715;

					case 0x10:
						this.ScrollSelectionRight(cellX, cellY);
						goto Label_0715;

					case 0x11:
						this.ScrollSelectionRight(cellX, cellY);
						this.ScrollSelectionDown(cellX, cellY);
						goto Label_0715;
				}
				goto Label_0715;
			}
			this.EraseRectangleEntity(this.m_lineRect);
			if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
			{
				int num5;
				int num6;
				int num3 = cellX - this.m_mouseDownAtInGrid.X;
				int num4 = cellY - this.m_mouseDownAtInGrid.Y;
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
						this.m_lineRect.Width = -num6;
						this.m_lineRect.Height = -num6;
						goto Label_0473;

					case 2:
						this.m_lineRect.Width = 0;
						this.m_lineRect.Height = num4;
						goto Label_0473;

					case 3:
						this.m_lineRect.Width = num6;
						this.m_lineRect.Height = -num6;
						goto Label_0473;

					case 4:
						this.m_lineRect.Width = num3;
						this.m_lineRect.Height = 0;
						goto Label_0473;

					case 5:
						this.m_lineRect.Width = num6;
						this.m_lineRect.Height = num6;
						goto Label_0473;

					case 6:
						this.m_lineRect.Width = 0;
						this.m_lineRect.Height = num4;
						goto Label_0473;

					case 7:
						this.m_lineRect.Width = -num6;
						this.m_lineRect.Height = num6;
						goto Label_0473;

					case 8:
						this.m_lineRect.Width = num3;
						this.m_lineRect.Height = 0;
						goto Label_0473;
				}
			}
			else
			{
				if (cellX < this.m_mouseDownAtInGrid.X)
				{
					this.m_lineRect.Width = cellX - this.m_lineRect.Left;
				}
				else if (cellX > this.m_mouseDownAtInGrid.X)
				{
					this.m_lineRect.Width = cellX - this.m_lineRect.Left;
				}
				else
				{
					this.m_lineRect.Width = 0;
				}
				if (cellY < this.m_mouseDownAtInGrid.Y)
				{
					this.m_lineRect.Height = cellY - this.m_lineRect.Top;
				}
				else if (cellY > this.m_mouseDownAtInGrid.Y)
				{
					this.m_lineRect.Height = cellY - this.m_lineRect.Top;
				}
				else
				{
					this.m_lineRect.Height = 0;
				}
			}
		Label_0473:
			this.InvalidateRect(this.m_lineRect);
			this.UpdatePositionLabel(this.NormalizeRect(new Rectangle(this.m_lineRect.X, this.m_lineRect.Y, this.m_lineRect.Width + 1, this.m_lineRect.Height)), true);
			goto Label_0733;
		Label_0715:
			this.m_lastCellX = cellX;
			this.m_lastCellY = cellY;
			this.UpdatePositionLabel(this.m_normalizedRange, false);
		Label_0733:
			num8 = 0;
			int num9 = 0;
			int y = 0;
			int num11 = 0;
			if ((cellX != this.m_mouseTimeCaret) || (cellY != this.m_mouseChannelCaret))
			{
				num8 = (Math.Min(cellX, this.m_mouseTimeCaret) - this.hScrollBar1.Value) * this.m_periodPixelWidth;
				num9 = ((Math.Max(cellX, this.m_mouseTimeCaret) - this.hScrollBar1.Value) + 1) * this.m_periodPixelWidth;
				y = (Math.Min(cellY, this.m_mouseChannelCaret) - this.vScrollBar1.Value) * this.m_gridRowHeight;
				num11 = ((Math.Max(cellY, this.m_mouseChannelCaret) - this.vScrollBar1.Value) + 1) * this.m_gridRowHeight;
			}
			if (cellY != this.m_mouseChannelCaret)
			{
				rectangle = new Rectangle(0, this.pictureBoxTime.Height + (this.m_gridRowHeight * (this.m_mouseChannelCaret - this.vScrollBar1.Value)), 5, this.m_gridRowHeight);
				this.m_mouseChannelCaret = -1;
				this.pictureBoxChannels.Invalidate(rectangle);
				this.pictureBoxChannels.Update();
				if (cellY < this.m_sequence.ChannelCount)
				{
					this.m_mouseChannelCaret = cellY;
					rectangle.Y = this.pictureBoxTime.Height + (this.m_gridRowHeight * (this.m_mouseChannelCaret - this.vScrollBar1.Value));
					this.pictureBoxChannels.Invalidate(rectangle);
					this.pictureBoxChannels.Update();
				}
				else
				{
					this.m_mouseChannelCaret = -1;
				}
			}
			if (cellX != this.m_mouseTimeCaret)
			{
				rectangle = new Rectangle(this.m_periodPixelWidth * (this.m_mouseTimeCaret - this.hScrollBar1.Value), 0, this.m_periodPixelWidth, 5);
				this.m_mouseTimeCaret = -1;
				this.pictureBoxTime.Invalidate(rectangle);
				this.pictureBoxTime.Update();
				if (cellX < this.m_sequence.TotalEventPeriods)
				{
					this.m_mouseTimeCaret = cellX;
					rectangle.X = this.m_periodPixelWidth * (this.m_mouseTimeCaret - this.hScrollBar1.Value);
					this.pictureBoxTime.Invalidate(rectangle);
					this.pictureBoxTime.Update();
				}
				else
				{
					this.m_mouseTimeCaret = -1;
				}
			}
			if (num8 != num9)
			{
				this.pictureBoxGrid.Invalidate(new Rectangle(num8, 0, num9 - num8, this.pictureBoxGrid.Height));
				this.pictureBoxGrid.Update();
				this.pictureBoxGrid.Invalidate(new Rectangle(0, y, this.pictureBoxGrid.Width, num11 - y));
				this.pictureBoxGrid.Update();
			}
			if ((cellX >= 0) && (cellY >= 0))
			{
				string str;
				this.GetCellIntensity(cellX, cellY, out str);
				this.toolStripLabelCellIntensity.Text = str;
				this.toolStripLabelCurrentCell.Text = string.Format("{0} , {1}", this.TimeString(cellX * this.m_sequence.EventPeriod), this.m_sequence.Channels[this.m_channelOrderMapping[cellY]].Name);
			}
		}

		private void pictureBoxGrid_MouseUp(object sender, MouseEventArgs e)
		{
			this.m_mouseDownInGrid = false;
			if (this.m_lineRect.Left != -1)
			{
				bool flag = this.paintFromClipboardToolStripMenuItem.Checked && (this.m_systemInterface.Clipboard != null);
				if (flag)
				{
					Rectangle rect = this.NormalizeRect(this.m_lineRect);
					rect.Width += this.m_systemInterface.Clipboard.GetLength(1);
					this.EraseRectangleEntity(rect);
					rect.Width++;
					rect.Height++;
					this.AddUndoItem(rect, UndoOriginalBehavior.Overwrite);
				}
				else
				{
					this.EraseRectangleEntity(this.m_lineRect);
					Rectangle blockAffected = this.NormalizeRect(this.m_lineRect);
					blockAffected.Width++;
					blockAffected.Height++;
					this.AddUndoItem(blockAffected, UndoOriginalBehavior.Overwrite);
				}
				if (!flag)
				{
					this.BresenhamValues(this.m_lineRect);
				}
				else
				{
					byte[] brush = new byte[this.m_systemInterface.Clipboard.GetLength(1)];
					for (int i = 0; i < brush.Length; i++)
					{
						brush[i] = this.m_systemInterface.Clipboard[0, i];
					}
					this.BresenhamPaint(this.m_lineRect, brush);
				}
				this.m_lineRect.X = -1;
				this.UpdatePositionLabel(this.m_normalizedRange, false);
			}
		}

		private void pictureBoxGrid_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle(this.m_gridBackBrush, e.ClipRectangle);
			if (this.m_sequence.ChannelCount != 0)
			{
				int num2 = 0;
				int num3 = 0;
				Point point = new Point();
				Point point2 = new Point();
				int num = e.ClipRectangle.Left / this.m_periodPixelWidth;
				point.Y = e.ClipRectangle.Top;
				point2.Y = Math.Min(e.ClipRectangle.Bottom, (this.m_sequence.ChannelCount - this.vScrollBar1.Value) * this.m_gridRowHeight);
				int num4 = Math.Min(e.ClipRectangle.Right, (this.m_sequence.TotalEventPeriods - this.hScrollBar1.Value) * this.m_periodPixelWidth);
				num2 = 0;
				while (num2 < num4)
				{
					num2 = this.m_periodPixelWidth * num;
					point.X = num2;
					point2.X = num2;
					e.Graphics.DrawLine(Pens.Gray, point, point2);
					num++;
				}
				num = e.ClipRectangle.Top / this.m_gridRowHeight;
				point.X = e.ClipRectangle.Left;
				point2.X = Math.Min(e.ClipRectangle.Right, (this.m_sequence.TotalEventPeriods - this.hScrollBar1.Value) * this.m_periodPixelWidth);
				num4 = Math.Min(e.ClipRectangle.Bottom, (this.m_sequence.ChannelCount - this.vScrollBar1.Value) * this.m_gridRowHeight);
				num3 = 0;
				while (num3 < num4)
				{
					num3 = this.m_gridRowHeight * num;
					point.Y = num3;
					point2.Y = num3;
					e.Graphics.DrawLine(Pens.Gray, point, point2);
					num++;
				}
				this.UpdateGrid(e.Graphics, e.ClipRectangle);
				if (this.m_positionTimer.Enabled)
				{
					num3 = Math.Min(e.ClipRectangle.Bottom, (this.m_sequence.ChannelCount - this.vScrollBar1.Value) * this.m_gridRowHeight);
					if ((this.m_previousPosition != -1) && (this.m_previousPosition >= this.hScrollBar1.Value))
					{
						num2 = (this.m_previousPosition - this.hScrollBar1.Value) * this.m_periodPixelWidth;
						e.Graphics.DrawLine(Pens.Gray, num2, 0, num2, num3);
					}
					num2 = (this.m_position - this.hScrollBar1.Value) * this.m_periodPixelWidth;
					e.Graphics.DrawLine(Pens.Yellow, num2, 0, num2, num3);
				}
				else
				{
					if (this.m_lineRect.Left != -1)
					{
						int num7 = ((this.m_lineRect.Left - this.hScrollBar1.Value) * this.m_periodPixelWidth) + (this.m_periodPixelWidth >> 1);
						int num8 = ((this.m_lineRect.Top - this.vScrollBar1.Value) * this.m_gridRowHeight) + (this.m_gridRowHeight >> 1);
						int num9 = num7 + (this.m_lineRect.Width * this.m_periodPixelWidth);
						int num10 = num8 + (this.m_lineRect.Height * this.m_gridRowHeight);
						e.Graphics.DrawLine(Pens.Blue, num7, num8, num9, num10);
					}
					else
					{
						if (this.m_normalizedRange.Left > this.m_sequence.TotalEventPeriods)
						{
							this.m_normalizedRange.Width = 0;
						}
						Rectangle range = Rectangle.Intersect(this.m_normalizedRange, Rectangle.FromLTRB(this.hScrollBar1.Value, this.vScrollBar1.Value, (this.hScrollBar1.Value + this.m_visibleEventPeriods) + 1, (this.vScrollBar1.Value + this.m_visibleRowCount) + 1));
						if (!range.IsEmpty)
						{
							e.Graphics.FillRectangle(this.m_selectionBrush, this.RangeToRectangle(range));
						}
					}
					if (this.toolStripButtonToggleCrossHairs.Checked)
					{
						int num11 = ((this.m_mouseTimeCaret - this.hScrollBar1.Value) * this.m_periodPixelWidth) + ((int) (this.m_periodPixelWidth * 0.5f));
						int num12 = ((this.m_mouseChannelCaret - this.vScrollBar1.Value) * this.m_gridRowHeight) + ((int) (this.m_gridRowHeight * 0.5f));
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
			if (!this.m_initializing)
			{
				this.VScrollCheck();
				this.HScrollCheck();
				this.pictureBoxGrid.Refresh();
			}
		}

		private void pictureBoxTime_Paint(object sender, PaintEventArgs e)
		{
			int x;
			int num2;
			e.Graphics.FillRectangle(this.m_timeBackBrush, e.ClipRectangle);
			Point point = new Point();
			Point point2 = new Point();
			point.Y = this.pictureBoxTime.Height - 20;
			point2.Y = this.pictureBoxTime.Height - 5;
			if ((e.ClipRectangle.Bottom >= point.Y) || (e.ClipRectangle.Top <= point2.Y))
			{
				num2 = x = e.ClipRectangle.X / this.m_periodPixelWidth;
				for (x *= this.m_periodPixelWidth; (x < e.ClipRectangle.Right) && ((num2 + this.hScrollBar1.Value) <= this.m_sequence.TotalEventPeriods); x += this.m_periodPixelWidth)
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
			point.Y = this.pictureBoxTime.Height - 30;
			if ((e.ClipRectangle.Bottom >= point.Y) || (e.ClipRectangle.Top <= point2.Y))
			{
				x = e.ClipRectangle.X;
				float eventPeriod = this.m_sequence.EventPeriod;
				float num4 = (this.hScrollBar1.Value + (((float) e.ClipRectangle.Left) / ((float) this.m_periodPixelWidth))) * eventPeriod;
				int num5 = Math.Min((int) ((this.hScrollBar1.Value + (e.ClipRectangle.Right / this.m_periodPixelWidth)) * eventPeriod), this.m_sequence.Time);
				float num6 = 1000f / ((float) this.m_sequence.EventPeriod);
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
						x = e.ClipRectangle.Left + ((int) (((num2 - num4) / 1000f) * (this.m_periodPixelWidth * num6)));
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
						SizeF ef = e.Graphics.MeasureString(str, this.m_timeFont);
						e.Graphics.DrawString(str, this.m_timeFont, Brushes.Black, (float) (x - (ef.Width / 2f)), (float) ((point.Y - ef.Height) - 5f));
					}
					num2 += 0x3e8;
				}
			}
			point.Y = this.pictureBoxTime.Height - 0x23;
			point2.Y = this.pictureBoxTime.Height - 20;
			if (((e.ClipRectangle.Bottom >= point.Y) || (e.ClipRectangle.Top <= point2.Y)) && (this.m_showPositionMarker && (this.m_position != -1)))
			{
				x = this.m_periodPixelWidth * (this.m_position - this.hScrollBar1.Value);
				if (x < this.pictureBoxTime.Width)
				{
					point.X = x;
					point2.X = x;
					e.Graphics.DrawLine(Pens.Red, point, point2);
				}
			}
			if (this.m_mouseTimeCaret != -1)
			{
				e.Graphics.FillRectangle(this.m_channelCaretBrush, (this.m_mouseTimeCaret - this.hScrollBar1.Value) * this.m_periodPixelWidth, 0, this.m_periodPixelWidth, 5);
			}
			if (this.toolStripButtonWaveform.Checked)
			{
				int index = this.hScrollBar1.Value * this.m_periodPixelWidth;
				int num9 = Math.Min(index + ((this.m_visibleEventPeriods + 1) * this.m_periodPixelWidth), this.m_waveformPixelData.Length);
				int num7 = 0;
				while (index < num9)
				{
					e.Graphics.DrawLine(Pens.Blue, (float) num7, (float) (this.m_waveformOffset - (this.m_waveformPixelData[index] >> 0x10)), (float) num7, (float) (this.m_waveformOffset - ((short) (this.m_waveformPixelData[index] & 0xffff))));
					num7++;
					index++;
				}
			}
		}

		private void PlaceSparkle(byte[,] valueArray, int row, int startCol, int decayTime, byte minIntensity, byte maxIntensity)
		{
			int num = (int) (Math.Round((double) (1000f / ((float) this.m_sequence.EventPeriod)), MidpointRounding.AwayFromZero) * 0.1);
			int num2 = (int) Math.Round((double) (((float) decayTime) / ((float) this.m_sequence.EventPeriod)), MidpointRounding.AwayFromZero);
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
			this.playAtTheSelectedPointToolStripMenuItem.Checked = true;
			this.playOnlyTheSelectedRangeToolStripMenuItem.Checked = false;
			this.toolStripButtonPlayPoint.ToolTipText = "Play this sequence starting at the selection point (F6)";
		}

		private void playOnlyTheSelectedRangeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.playOnlyTheSelectedRangeToolStripMenuItem.Checked = true;
			this.playAtTheSelectedPointToolStripMenuItem.Checked = false;
			this.toolStripButtonPlayPoint.ToolTipText = "Play the selected range of this sequence (F6)";
		}

		private void plugInItem_CheckedChanged(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (ToolStripMenuItem) sender;
			this.m_sequence.PlugInData.GetPlugInData((string) item.Tag).Attributes["enabled"].Value = item.Checked.ToString();
			bool flag = false;
			foreach (ToolStripItem item2 in this.toolStripDropDownButtonPlugins.DropDownItems)
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
			List<OutputPlugin> list = new List<OutputPlugin>(this.m_sequence.PlugInData.GetOutputPlugins());
			List<string> list2 = new List<string>();
			if (this.m_printingChannelIndex == 0)
			{
				e.Graphics.DrawString(this.m_sequence.Name, font, Brushes.Black, (float) e.MarginBounds.Left, top);
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
			while ((this.m_printingChannelIndex < this.m_sequence.ChannelCount) && ((top + height) < e.MarginBounds.Bottom))
			{
				if ((this.m_printingChannelIndex % 2) == 1)
				{
					e.Graphics.FillRectangle(brush, (float) e.MarginBounds.Left, top - 1f, (float) e.MarginBounds.Width, height);
				}
				int num3 = this.m_printingChannelList[this.m_printingChannelIndex].OutputChannel + 1;
				e.Graphics.DrawString(this.m_printingChannelList[this.m_printingChannelIndex].Name, font4, Brushes.Black, (float) (e.MarginBounds.Left + 10), top);
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
				this.m_printingChannelIndex++;
				top += height;
			}
			brush.Dispose();
			font.Dispose();
			font2.Dispose();
			font3.Dispose();
			font4.Dispose();
			e.HasMorePages = this.m_printingChannelIndex < this.m_sequence.ChannelCount;
		}

		private void ProgramEnded()
		{
			this.m_positionTimer.Stop();
			this.SetEditingState(true);
			this.pictureBoxGrid.Refresh();
		}

		private void quarterSpeedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SetAudioSpeed(0.25f);
		}

		private void Ramp(int startingLevel, int endingLevel)
		{
			this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
			int bottom = this.m_normalizedRange.Bottom;
			int right = this.m_normalizedRange.Right;
			int left = this.m_normalizedRange.Left;
			for (int i = this.m_normalizedRange.Top; i < bottom; i++)
			{
				int num6 = this.m_channelOrderMapping[i];
				if (this.m_sequence.Channels[num6].Enabled)
				{
					for (int j = left; j < right; j++)
					{
						byte minimumLevel = (byte) (((((float) (j - left)) / ((float) ((right - left) - 1))) * (endingLevel - startingLevel)) + startingLevel);
						if (minimumLevel < this.m_sequence.MinimumLevel)
						{
							minimumLevel = this.m_sequence.MinimumLevel;
						}
						else if (minimumLevel > this.m_sequence.MaximumLevel)
						{
							minimumLevel = this.m_sequence.MaximumLevel;
						}
						this.m_sequence.EventValues[num6, j] = minimumLevel;
					}
				}
			}
			this.m_selectionRectangle.Width = 0;
			this.pictureBoxGrid.Invalidate(this.SelectionToRectangle());
		}

		private Rectangle RangeToRectangle(Rectangle range)
		{
			Rectangle rectangle = new Rectangle();
			rectangle.X = ((range.X - this.hScrollBar1.Value) * this.m_periodPixelWidth) + 1;
			rectangle.Y = ((range.Y - this.vScrollBar1.Value) * this.m_gridRowHeight) + 1;
			rectangle.Width = (range.Width * this.m_periodPixelWidth) - 1;
			rectangle.Height = (range.Height * this.m_gridRowHeight) - 1;
			return rectangle;
		}

		private void ReactEditingStateToProfileAssignment()
		{
			bool flag = this.m_sequence.Profile != null;
			this.textBoxChannelCount.ReadOnly = flag;
			this.toolStripDropDownButtonPlugins.Enabled = !flag;
			this.toolStripButtonSaveOrder.Enabled = !flag;
			this.toolStripButtonChannelOutputMask.Enabled = !flag;
		}

		private void ReactToProfileAssignment()
		{
			bool flag = this.m_sequence.Profile != null;
			this.flattenProfileIntoSequenceToolStripMenuItem.Enabled = flag;
			this.detachSequenceFromItsProfileToolStripMenuItem.Enabled = flag;
			this.channelOutputMaskToolStripMenuItem.Enabled = !flag;
			this.ReactEditingStateToProfileAssignment();
			this.SetOrderArraySize(this.m_sequence.ChannelCount);
			this.textBoxChannelCount.Text = this.m_sequence.ChannelCount.ToString();
			this.VScrollCheck();
			this.pictureBoxChannels.Refresh();
			this.pictureBoxGrid.Refresh();
			this.LoadSequenceSorts();
			this.LoadSequencePlugins();
		}

		private void ReadFromSequence()
		{
			this.Text = this.m_sequence.Name;
			this.SetProgramTime(this.m_sequence.Time);
			this.ReactToProfileAssignment();
			this.pictureBoxChannels.Refresh();
			this.VScrollCheck();
		}

		private void Redo()
		{
			if (this.m_redoStack.Count != 0)
			{
				UndoItem item = (UndoItem) this.m_redoStack.Pop();
				int height = 0;
				int width = 0;
				if (item.Data != null)
				{
					height = item.Data.GetLength(0);
					width = item.Data.GetLength(1);
				}
				this.toolStripButtonRedo.Enabled = this.redoToolStripMenuItem.Enabled = this.m_redoStack.Count > 0;
				base.IsDirty = true;
				UndoItem item2 = new UndoItem(item.Location, this.GetAffectedBlockData(new Rectangle(item.Location.X, item.Location.Y, item.Data.GetLength(1), item.Data.GetLength(0))), item.Behavior, this.m_sequence, this.m_channelOrderMapping);
				switch (item.Behavior)
				{
					case UndoOriginalBehavior.Overwrite:
						this.DisjointedOverwrite(item.Location.X, item.Location.Y, item.Data, item.ReferencedChannels);
						this.pictureBoxGrid.Invalidate(new Rectangle((item.Location.X - this.hScrollBar1.Value) * this.m_periodPixelWidth, (item.Location.Y - this.vScrollBar1.Value) * this.m_gridRowHeight, width * this.m_periodPixelWidth, height * this.m_gridRowHeight));
						break;

					case UndoOriginalBehavior.Removal:
						this.DisjointedRemove(item.Location.X, item.Location.Y, width, height, item.ReferencedChannels);
						this.pictureBoxGrid.Refresh();
						break;

					case UndoOriginalBehavior.Insertion:
						this.DisjointedInsert(item.Location.X, item.Location.Y, width, height, item.ReferencedChannels);
						this.DisjointedOverwrite(item.Location.X, item.Location.Y, item.Data, item.ReferencedChannels);
						this.pictureBoxGrid.Refresh();
						break;
				}
				this.UpdateRedoText();
				this.m_undoStack.Push(item2);
				this.toolStripButtonUndo.Enabled = this.undoToolStripMenuItem.Enabled = true;
				this.UpdateUndoText();
			}
		}

		private void RefreshAll()
		{
			this.SetOrderArraySize(this.m_sequence.ChannelCount);
			this.textBoxChannelCount.Text = this.m_sequence.ChannelCount.ToString();
			this.textBoxProgramLength.Text = this.TimeString(this.m_sequence.Time);
			this.pictureBoxGrid.Refresh();
			this.pictureBoxChannels.Refresh();
			this.pictureBoxTime.Refresh();
			this.VScrollCheck();
			this.HScrollCheck();
		}

		private void ReorderChannelOutputs()
		{
			if (this.m_sequence.Profile != null)
			{
				MessageBox.Show("This sequence is attached to a profile.\nChanges made to the profile's channel outputs will be reflected here.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				ChannelOrderDialog dialog = new ChannelOrderDialog(this.m_sequence.OutputChannels, null);
				dialog.Text = "Channel Output Mapping";
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					List<VixenPlus.Channel> channelMapping = dialog.ChannelMapping;
					foreach (VixenPlus.Channel channel in this.m_sequence.Channels)
					{
						channel.OutputChannel = channelMapping.IndexOf(channel);
					}
					if (this.m_showingOutputs)
					{
						this.pictureBoxChannels.Refresh();
						this.pictureBoxGrid.Refresh();
					}
					base.IsDirty = true;
				}
				dialog.Dispose();
			}
		}

		private void reorderChannelOutputsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ReorderChannelOutputs();
		}

		private void Reset()
		{
			this.hScrollBar1.Value = this.hScrollBar1.Minimum;
			this.toolStripLabelExecutionPoint.Text = "00:00";
		}

		private void resetAllToolbarsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			VixenEditor.ToolStripManager.LoadSettings(this, this.m_preferences.XmlDoc.DocumentElement, "reset");
			foreach (ToolStripItem item in this.toolbarsToolStripMenuItem.DropDownItems)
			{
				if ((item is ToolStripMenuItem) && (item.Tag != null))
				{
					((ToolStripMenuItem) item).Checked = true;
				}
			}
		}

		public override DialogResult RunWizard(ref EventSequence resultSequence)
		{
			NewSequenceWizardDialog dialog = new NewSequenceWizardDialog(this.m_systemInterface.UserPreferences);
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
				byte[,] buffer = this.CellsToArray();
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
			this.m_sequence.SaveTo(filePath);
			base.IsDirty = false;
		}

		private void saveToolbarPositionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			VixenEditor.ToolStripManager.SaveSettings(this, this.m_preferences.XmlDoc.DocumentElement);
			this.m_preferences.Flush();
			MessageBox.Show("Done.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ArithmeticPaste(ArithmeticOperation.Scale);
		}

		private void ScrollSelectionDown(int cellX, int cellY)
		{
			int num = this.pictureBoxGrid.PointToScreen(new Point(this.pictureBoxGrid.Left, this.pictureBoxGrid.Bottom)).Y - this.pictureBoxTime.Height;
			this.m_selectedRange.Width = (cellX + 1) - this.m_selectedRange.Left;
			while (Control.MousePosition.Y > num)
			{
				cellY = this.pictureBoxGrid.PointToClient(Control.MousePosition).Y / this.m_periodPixelWidth;
				cellY += this.vScrollBar1.Value;
				if (cellY >= (this.m_sequence.ChannelCount - 1))
				{
					this.m_normalizedRange.Height = this.m_selectedRange.Height = this.m_sequence.ChannelCount - this.m_selectedRange.Y;
					if (cellX != this.m_lastCellX)
					{
						this.pictureBoxGrid.Refresh();
					}
				}
				if (this.vScrollBar1.Value > (this.vScrollBar1.Maximum - this.vScrollBar1.LargeChange))
				{
					break;
				}
				if (this.vScrollBar1.Value < (this.vScrollBar1.Maximum - this.vScrollBar1.LargeChange))
				{
					this.m_selectedRange.Height++;
					this.m_normalizedRange.Height++;
				}
				this.vScrollBar1.Value++;
			}
		}

		private void ScrollSelectionLeft(int cellX, int cellY)
		{
			int x = this.pictureBoxGrid.PointToScreen(new Point(this.pictureBoxGrid.Left, this.pictureBoxGrid.Top)).X;
			while ((Control.MousePosition.X < x) && (this.hScrollBar1.Value != 0))
			{
				this.m_selectedRange.Height = (cellY + 1) - this.m_selectedRange.Top;
				this.m_selectedRange.Width--;
				this.m_normalizedRange = this.NormalizeRect(this.m_selectedRange);
				this.hScrollBar1.Value--;
			}
		}

		private void ScrollSelectionRight(int cellX, int cellY)
		{
			int x = this.pictureBoxGrid.PointToScreen(new Point(this.pictureBoxGrid.Right, this.pictureBoxGrid.Top)).X;
			this.m_selectedRange.Height = (cellY + 1) - this.m_selectedRange.Top;
			while (Control.MousePosition.X > x)
			{
				cellX = this.pictureBoxGrid.PointToClient(Control.MousePosition).X / this.m_periodPixelWidth;
				cellX += this.hScrollBar1.Value;
				if (cellX >= (this.m_sequence.TotalEventPeriods - 1))
				{
					this.m_normalizedRange.Width = this.m_selectedRange.Width = this.m_sequence.TotalEventPeriods - this.m_selectedRange.X;
					if (cellY != this.m_lastCellY)
					{
						this.pictureBoxGrid.Refresh();
					}
				}
				if (this.hScrollBar1.Value > (this.hScrollBar1.Maximum - this.hScrollBar1.LargeChange))
				{
					break;
				}
				if (this.hScrollBar1.Value < (this.hScrollBar1.Maximum - this.hScrollBar1.LargeChange))
				{
					this.m_selectedRange.Width++;
					this.m_normalizedRange.Width++;
				}
				this.hScrollBar1.Value++;
			}
		}

		private void ScrollSelectionUp(int cellX, int cellY)
		{
			int y = this.pictureBoxGrid.PointToScreen(new Point(this.pictureBoxGrid.Left, this.pictureBoxGrid.Top)).Y;
			while ((Control.MousePosition.Y < y) && (this.vScrollBar1.Value != 0))
			{
				this.m_selectedRange.Width = (cellX + 1) - this.m_selectedRange.Left;
				this.m_selectedRange.Height--;
				this.m_normalizedRange = this.NormalizeRect(this.m_selectedRange);
				this.vScrollBar1.Value--;
			}
		}

		private bool SelectableControlFocused()
		{
			Control terminalSelectableControl = this.GetTerminalSelectableControl();
			return ((terminalSelectableControl != null) && terminalSelectableControl.CanSelect);
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ToolStripItem item in this.toolStripDropDownButtonPlugins.DropDownItems)
			{
				if (!(item is ToolStripMenuItem))
				{
					break;
				}
				((ToolStripMenuItem) item).Checked = true;
			}
			if (this.toolStripDropDownButtonPlugins.DropDownItems.Count > 3)
			{
			}
		}

		private Rectangle SelectionToRectangle()
		{
			Rectangle rectangle = new Rectangle();
			rectangle.X = ((this.m_normalizedRange.X - this.hScrollBar1.Value) * this.m_periodPixelWidth) + 1;
			rectangle.Y = ((this.m_normalizedRange.Y - this.vScrollBar1.Value) * this.m_gridRowHeight) + 1;
			rectangle.Width = (this.m_normalizedRange.Width * this.m_periodPixelWidth) - 1;
			rectangle.Height = (this.m_normalizedRange.Height * this.m_gridRowHeight) - 1;
			return rectangle;
		}

		private void setAllChannelColorsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.m_sequence.Profile != null)
			{
				MessageBox.Show("This sequence is attached to a profile.\nChanges to the profile affect all sequences attached to it.\n\nIf this is what you want to do, please use the profile manager.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				AllChannelsColorDialog dialog = new AllChannelsColorDialog(this.m_sequence.Channels);
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					int channelCount = this.m_sequence.ChannelCount;
					List<Color> channelColors = dialog.ChannelColors;
					for (int i = 0; i < channelCount; i++)
					{
						this.m_sequence.Channels[i].Color = channelColors[i];
					}
				}
				dialog.Dispose();
				base.IsDirty = true;
				this.pictureBoxChannels.Refresh();
				this.pictureBoxGrid.Refresh();
			}
		}

		private void SetAudioSpeed(float rate)
		{
			if ((rate > 0f) && (rate <= 1f))
			{
				this.xToolStripMenuItem.Checked = false;
				this.xToolStripMenuItem1.Checked = false;
				this.xToolStripMenuItem2.Checked = false;
				this.normalToolStripMenuItem1.Checked = false;
				this.otherToolStripMenuItem.Checked = false;
				this.toolStripButtonPlaySpeedQuarter.Checked = false;
				this.toolStripButtonPlaySpeedHalf.Checked = false;
				this.toolStripButtonPlaySpeedThreeQuarters.Checked = false;
				this.toolStripButtonPlaySpeedNormal.Checked = false;
				this.toolStripButtonPlaySpeedVariable.Checked = false;
				if (rate == 0.25f)
				{
					this.xToolStripMenuItem.Checked = true;
					this.toolStripButtonPlaySpeedQuarter.Checked = true;
				}
				else if (rate == 0.5f)
				{
					this.xToolStripMenuItem1.Checked = true;
					this.toolStripButtonPlaySpeedHalf.Checked = true;
				}
				else if (rate == 0.75f)
				{
					this.xToolStripMenuItem2.Checked = true;
					this.toolStripButtonPlaySpeedThreeQuarters.Checked = true;
				}
				else if (rate == 1f)
				{
					this.normalToolStripMenuItem1.Checked = true;
					this.toolStripButtonPlaySpeedNormal.Checked = true;
				}
				else
				{
					this.otherToolStripMenuItem.Checked = true;
					this.toolStripButtonPlaySpeedVariable.Checked = true;
				}
				this.m_executionInterface.SetAudioSpeed(this.m_executionContextHandle, rate);
			}
		}

		private void SetChannelCount(int count)
		{
			if (count != this.m_sequence.ChannelCount)
			{
				int num;
				bool flag = false;
				int num2 = Math.Min(this.m_sequence.ChannelCount, count);
				for (num = 0; num < num2; num++)
				{
					if (this.m_sequence.Channels[num].OutputChannel > (count - 1))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (MessageBox.Show("With the new channel count, some channels would refer to outputs that no longer exist.\nTo keep the sequence valid, channel outputs would have to be reset.\n\nDo you want to keep the new channel count?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
					{
						this.textBoxChannelCount.Text = this.m_sequence.ChannelCount.ToString();
						return;
					}
					for (num = 0; num < this.m_sequence.ChannelCount; num++)
					{
						this.m_sequence.Channels[num].OutputChannel = num;
					}
				}
				this.SetOrderArraySize(count);
				this.m_sequence.ChannelCount = count;
				this.textBoxChannelCount.Text = count.ToString();
				this.VScrollCheck();
				this.pictureBoxChannels.Refresh();
				this.pictureBoxGrid.Refresh();
				base.IsDirty = true;
				MessageBox.Show("Channel count has been updated.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void SetDrawingLevel(byte level)
		{
			this.m_drawingLevel = level;
			if (this.m_actualLevels)
			{
				this.toolStripLabelCurrentDrawingIntensity.Text = level.ToString();
			}
			else
			{
				this.toolStripLabelCurrentDrawingIntensity.Text = string.Format("{0}%", (int) Math.Round((double) ((level * 100f) / 255f), MidpointRounding.AwayFromZero));
			}
		}

		private void SetEditingState(bool state)
		{
			if (state)
			{
				this.EnableWaveformButton();
			}
			else
			{
				this.DisableWaveformButton();
			}
			this.toolStripEditing.Enabled = state;
			this.toolStripEffect.Enabled = state;
			this.toolStripSequenceSettings.Enabled = state;
			this.toolStripDropDownButtonPlugins.Enabled = state;
			this.toolStripDisplaySettings.Enabled = state;
			this.ReactEditingStateToProfileAssignment();
		}

		private void SetOrderArraySize(int count)
		{
			if (count < this.m_channelOrderMapping.Count)
			{
				List<int> list = new List<int>();
				list.AddRange(this.m_channelOrderMapping);
				foreach (int num in list)
				{
					if (num >= count)
					{
						this.m_channelOrderMapping.Remove(num);
					}
				}
			}
			else
			{
				for (int i = this.m_channelOrderMapping.Count; i < count; i++)
				{
					this.m_channelOrderMapping.Add(i);
				}
			}
		}

		private void SetProfile(string filePath)
		{
			if (filePath != null)
			{
				this.SetProfile(new Profile(this.openFileDialog1.FileName));
			}
			else
			{
				this.SetProfile((Profile) null);
			}
		}

		private void SetProfile(Profile profile)
		{
			this.m_sequence.Profile = profile;
			this.ReactToProfileAssignment();
			base.IsDirty = true;
		}

		private bool SetProgramTime(int milliseconds)
		{
			try
			{
				this.m_sequence.Time = milliseconds;
			}
			catch
			{
				MessageBox.Show("Cannot set the sequence length.\nThere is audio associated which would exceed that length.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.textBoxProgramLength.Text = this.TimeString(this.m_sequence.Time);
				return false;
			}
			this.textBoxProgramLength.Text = this.TimeString(this.m_sequence.Time);
			this.HScrollCheck();
			this.pictureBoxTime.Refresh();
			this.pictureBoxGrid.Refresh();
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
			dialog.Rate = this.m_executionInterface.GetAudioSpeed(this.m_executionContextHandle);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.SetAudioSpeed(dialog.Rate);
			}
			dialog.Dispose();
		}

		private void ShowChannelProperties()
		{
			List<VixenPlus.Channel> channels = new List<VixenPlus.Channel>();
			channels.AddRange(this.m_sequence.Channels);
			for (int i = 0; i < channels.Count; i++)
			{
				channels[i] = this.m_sequence.Channels[this.m_channelOrderMapping[i]];
			}
			ChannelPropertyDialog dialog = new ChannelPropertyDialog(channels, this.SelectedChannel, true);
			dialog.ShowDialog();
			dialog.Dispose();
			this.pictureBoxChannels.Refresh();
			this.pictureBoxGrid.Refresh();
			base.IsDirty = true;
		}

		private void sortByChannelNumberToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.m_printingChannelList = this.m_sequence.Channels;
			if (this.printDialog.ShowDialog() == DialogResult.OK)
			{
				this.printDocument.DocumentName = "Vixen channel configuration";
				this.m_printingChannelIndex = 0;
				this.printDocument.Print();
			}
		}

		private void sortByChannelOutputToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.m_printingChannelList = new List<VixenPlus.Channel>();
			VixenPlus.Channel[] collection = new VixenPlus.Channel[this.m_sequence.ChannelCount];
			foreach (VixenPlus.Channel channel in this.m_sequence.Channels)
			{
				collection[channel.OutputChannel] = channel;
			}
			this.m_printingChannelList.AddRange(collection);
			if (this.printDialog.ShowDialog() == DialogResult.OK)
			{
				this.printDocument.DocumentName = "Vixen channel configuration";
				this.m_printingChannelIndex = 0;
				this.printDocument.Print();
			}
		}

		private void SparkleGenerator(byte[,] values, params int[] effectParameters)
		{
			int num = 0x3e8 / this.m_sequence.EventPeriod;
			int maxValue = num - effectParameters[0];
			int decayTime = effectParameters[1];
			for (int i = 0; i < values.GetLength(0); i++)
			{
				for (int k = 0; k < values.GetLength(1); k++)
				{
					values[i, k] = this.m_sequence.MinimumLevel;
				}
			}
			int num6 = (int) Math.Round((double) (((float) decayTime) / ((float) this.m_sequence.EventPeriod)), MidpointRounding.AwayFromZero);
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
					this.PlaceSparkle(values, numArray[j] - 1, j, decayTime, (byte) effectParameters[2], (byte) effectParameters[3]);
				}
			}
		}

		private void StandardSequence_Activated(object sender, EventArgs e)
		{
			base.ActiveControl = this.m_lastSelectableControl;
		}

		private void StandardSequence_Deactivate(object sender, EventArgs e)
		{
			this.m_lastSelectableControl = this.GetTerminalSelectableControl();
		}

		private void StandardSequence_FormClosing(object sender, FormClosingEventArgs e)
		{
			if ((e.CloseReason == CloseReason.UserClosing) && (this.CheckDirty() == DialogResult.Cancel))
			{
				e.Cancel = true;
			}
			else
			{
				if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle) != 0)
				{
					this.toolStripButtonStop_Click(null, null);
				}
				if (this.m_preferences.GetBoolean("SaveZoomLevels"))
				{
					this.m_preferences.SetChildString("SaveZoomLevels", "row", this.toolStripComboBoxRowZoom.SelectedItem.ToString());
					this.m_preferences.SetChildString("SaveZoomLevels", "column", this.toolStripComboBoxColumnZoom.SelectedItem.ToString());
					this.m_preferences.Flush();
				}
				this.m_sequence.UpdateMetrics(base.Width, base.Height, this.splitContainer1.SplitterDistance);
				this.m_executionInterface.ReleaseContext(this.m_executionContextHandle);
			}
		}

		private void StandardSequence_KeyDown(object sender, KeyEventArgs e)
		{
			int num;
			bool flag = this.m_executionInterface.EngineStatus(this.m_executionContextHandle) == 1;
			bool flag2 = this.m_normalizedRange.Width > 0;
			switch (e.KeyCode)
			{
				case Keys.Prior:
					if (this.vScrollBar1.Value > 0)
					{
						num = Math.Min(this.m_visibleRowCount, this.vScrollBar1.Value);
						this.m_selectedRange.Y -= num;
						this.m_normalizedRange.Y -= num;
						this.vScrollBar1.Value -= num;
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.Next:
					if (this.vScrollBar1.Value < (this.m_sequence.ChannelCount - this.m_visibleRowCount))
					{
						int num2 = Math.Min(this.m_sequence.ChannelCount - this.vScrollBar1.Value, this.m_visibleRowCount);
						int num3 = Math.Min(this.m_sequence.ChannelCount - this.m_normalizedRange.Bottom, this.m_visibleRowCount);
						num = Math.Min(num2, num3);
						this.m_selectedRange.Y += num;
						this.m_normalizedRange.Y += num;
						this.vScrollBar1.Value += num;
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.End:
					if (this.hScrollBar1.Value < (this.m_sequence.TotalEventPeriods - this.m_visibleEventPeriods))
					{
						int num4 = this.m_sequence.TotalEventPeriods - this.m_visibleEventPeriods;
						this.m_selectedRange.X = num4;
						this.m_normalizedRange.X = num4;
						this.hScrollBar1.Value = num4;
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.Home:
					if (this.hScrollBar1.Value > 0)
					{
						this.m_selectedRange.X = 0;
						this.m_normalizedRange.X = 0;
						this.hScrollBar1.Value = 0;
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
					if (this.toolStripButtonPlayPoint.Enabled)
					{
						this.toolStripButtonPlayPoint_Click(null, null);
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.F7:
					if (this.toolStripButtonPause.Enabled)
					{
						this.toolStripButtonPause_Click(null, null);
						e.Handled = true;
					}
					goto Label_0355;

				case Keys.F8:
					if (this.toolStripButtonStop.Enabled)
					{
						this.toolStripButtonStop_Click(null, null);
						e.Handled = true;
					}
					goto Label_0355;

				default:
					goto Label_0355;
			}
			if (this.toolStripButtonPlay.Enabled)
			{
				this.toolStripButtonPlay_Click(null, null);
				e.Handled = true;
			}
		Label_0355:
			if ((((Control.ModifierKeys & Keys.Control) != Keys.None) && (e.KeyCode >= Keys.D0)) && (e.KeyCode <= Keys.D9))
			{
				int index = ((int) e.KeyCode) - 0x30;
				if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
				{
					this.m_bookmarks[index] = (this.m_bookmarks[index] == this.m_normalizedRange.Left) ? -1 : this.m_normalizedRange.Left;
					this.pictureBoxTime.Refresh();
				}
				else if (this.m_bookmarks[index] != -1)
				{
					this.hScrollBar1.Value = this.m_bookmarks[index];
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
					if ((this.SelectableControlFocused() && !this.pictureBoxChannels.Focused) && !this.pictureBoxGrid.Focused)
					{
						goto Label_0C71;
					}
					if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle, out num7) != 1)
					{
						int left;
						int num13;
						this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
						int num10 = this.m_normalizedRange.Height * this.m_normalizedRange.Width;
						int num11 = 0;
						for (num13 = this.m_normalizedRange.Top; num13 < this.m_normalizedRange.Bottom; num13++)
						{
							num6 = this.m_channelOrderMapping[num13];
							left = this.m_normalizedRange.Left;
							while (left < this.m_normalizedRange.Right)
							{
								if (this.m_sequence.EventValues[num6, left] > this.m_sequence.MinimumLevel)
								{
									num11++;
								}
								left++;
							}
						}
						byte num14 = (num11 == num10) ? this.m_sequence.MinimumLevel : this.m_drawingLevel;
						for (num13 = this.m_normalizedRange.Top; num13 < this.m_normalizedRange.Bottom; num13++)
						{
							num6 = this.m_channelOrderMapping[num13];
							for (left = this.m_normalizedRange.Left; left < this.m_normalizedRange.Right; left++)
							{
								this.m_sequence.EventValues[num6, left] = num14;
							}
						}
						this.pictureBoxGrid.Invalidate(new Rectangle((this.m_normalizedRange.Left - this.hScrollBar1.Value) * this.m_periodPixelWidth, (this.m_normalizedRange.Top - this.vScrollBar1.Value) * this.m_gridRowHeight, this.m_normalizedRange.Width * this.m_periodPixelWidth, this.m_normalizedRange.Height * this.m_gridRowHeight));
						e.Handled = true;
						goto Label_0C71;
					}
					int x = num7 / this.m_sequence.EventPeriod;
					this.AddUndoItem(new Rectangle(x, this.m_normalizedRange.Top, 1, this.m_normalizedRange.Height), UndoOriginalBehavior.Overwrite);
					for (int i = this.m_normalizedRange.Top; i < this.m_normalizedRange.Bottom; i++)
					{
						num6 = this.m_channelOrderMapping[i];
						this.m_sequence.EventValues[num6, x] = this.m_drawingLevel;
					}
					this.pictureBoxGrid.Invalidate(new Rectangle((x - this.hScrollBar1.Value) * this.m_periodPixelWidth, (this.m_normalizedRange.Top - this.vScrollBar1.Value) * this.m_gridRowHeight, this.m_periodPixelWidth, this.m_normalizedRange.Height * this.m_gridRowHeight));
					return;
				}
				case Keys.Left:
					if (!this.pictureBoxChannels.Focused && !this.pictureBoxGrid.Focused)
					{
						goto Label_0C71;
					}
					if ((this.hScrollBar1.Value > 0) || (this.m_normalizedRange.Left > 0))
					{
						this.m_selectedRange.X--;
						this.m_normalizedRange.X--;
						if ((this.m_normalizedRange.Left + 1) <= this.hScrollBar1.Value)
						{
							this.hScrollBar1.Value--;
							break;
						}
						this.pictureBoxGrid.Invalidate(new Rectangle((this.m_normalizedRange.Left - this.hScrollBar1.Value) * this.m_periodPixelWidth, (this.m_normalizedRange.Top - this.vScrollBar1.Value) * this.m_gridRowHeight, (this.m_normalizedRange.Width + 1) * this.m_periodPixelWidth, this.m_normalizedRange.Height * this.m_gridRowHeight));
					}
					break;

				case Keys.Up:
					if ((this.pictureBoxChannels.Focused || (this.pictureBoxGrid.Focused && !e.Control)) && ((this.vScrollBar1.Value > 0) || (this.m_normalizedRange.Top > 0)))
					{
						this.m_selectedRange.Y--;
						this.m_normalizedRange.Y--;
						if ((this.m_normalizedRange.Top + 1) <= this.vScrollBar1.Value)
						{
							this.vScrollBar1.Value--;
						}
						else
						{
							this.pictureBoxGrid.Invalidate(new Rectangle((this.m_normalizedRange.Left - this.hScrollBar1.Value) * this.m_periodPixelWidth, (this.m_normalizedRange.Top - this.vScrollBar1.Value) * this.m_gridRowHeight, this.m_normalizedRange.Width * this.m_periodPixelWidth, (this.m_normalizedRange.Height + 1) * this.m_gridRowHeight));
						}
					}
					e.Handled = true;
					goto Label_0C71;

				case Keys.Right:
					if (this.pictureBoxChannels.Focused || this.pictureBoxGrid.Focused)
					{
						if (this.m_normalizedRange.Right < this.m_sequence.TotalEventPeriods)
						{
							this.m_selectedRange.X++;
							this.m_normalizedRange.X++;
							if (((this.m_normalizedRange.Right - 1) - this.hScrollBar1.Value) >= this.m_visibleEventPeriods)
							{
								this.hScrollBar1.Value++;
							}
							else
							{
								this.pictureBoxGrid.Invalidate(new Rectangle(((this.m_normalizedRange.Left - this.hScrollBar1.Value) - 1) * this.m_periodPixelWidth, (this.m_normalizedRange.Top - this.vScrollBar1.Value) * this.m_gridRowHeight, (this.m_normalizedRange.Width + 1) * this.m_periodPixelWidth, this.m_normalizedRange.Height * this.m_gridRowHeight));
							}
						}
						e.Handled = true;
					}
					goto Label_0C71;

				case Keys.Down:
					if ((this.pictureBoxChannels.Focused || (this.pictureBoxGrid.Focused && !e.Control)) && (this.m_normalizedRange.Bottom < this.m_sequence.ChannelCount))
					{
						this.m_selectedRange.Y++;
						this.m_normalizedRange.Y++;
						if (((this.m_normalizedRange.Bottom - 1) - this.vScrollBar1.Value) >= this.m_visibleRowCount)
						{
							this.vScrollBar1.Value++;
						}
						else
						{
							this.pictureBoxGrid.Invalidate(new Rectangle((this.m_normalizedRange.Left - this.hScrollBar1.Value) * this.m_periodPixelWidth, ((this.m_normalizedRange.Top - this.vScrollBar1.Value) - 1) * this.m_gridRowHeight, this.m_normalizedRange.Width * this.m_periodPixelWidth, (this.m_normalizedRange.Height + 1) * this.m_gridRowHeight));
						}
					}
					e.Handled = true;
					goto Label_0C71;

				default:
					goto Label_0C71;
			}
			e.Handled = true;
		Label_0C71:
			if (!flag && this.pictureBoxGrid.Focused)
			{
				if ((((e.KeyCode < Keys.A) || (e.KeyCode > Keys.Z)) || ((Control.ModifierKeys & Keys.Control) != Keys.None)) || ((Control.ModifierKeys & Keys.Alt) != Keys.None))
				{
					switch (e.KeyCode)
					{
						case Keys.Up:
							if (e.Control)
							{
								this.IntensityAdjustDialogCheck();
								this.m_intensityAdjustDialog.Delta = e.Alt ? 1 : this.m_intensityLargeDelta;
							}
							e.Handled = true;
							goto Label_0EBD;

						case Keys.Right:
							goto Label_0EBD;

						case Keys.Down:
							if (e.Control)
							{
								this.IntensityAdjustDialogCheck();
								this.m_intensityAdjustDialog.Delta = e.Alt ? -1 : -this.m_intensityLargeDelta;
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
							this.toolStripButtonRandom_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.B:
						case Keys.C:
						case Keys.D:
						case Keys.G:
						case Keys.U:
							goto Label_0EBD;

						case Keys.E:
							this.toolStripButtonShimmerDimming_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.F:
							if (!e.Shift)
							{
								this.toolStripButtonRampOff_Click(null, null);
							}
							else
							{
								this.toolStripButtonPartialRampOff_Click(null, null);
							}
							e.Handled = true;
							goto Label_0EBD;

						case Keys.H:
							this.toolStripButtonMirrorHorizontal_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.I:
							this.toolStripButtonIntensity_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.R:
							if (!e.Shift)
							{
								this.toolStripButtonRampOn_Click(null, null);
							}
							else
							{
								this.toolStripButtonPartialRampOn_Click(null, null);
							}
							e.Handled = true;
							goto Label_0EBD;

						case Keys.S:
							this.toolStripButtonSparkle_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.T:
							this.toolStripButtonInvert_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;

						case Keys.V:
							this.toolStripButtonMirrorVertical_Click(null, null);
							e.Handled = true;
							goto Label_0EBD;
					}
				}
			}
		Label_0EBD:
			if (!(flag || (!this.pictureBoxChannels.Focused && !this.pictureBoxGrid.Focused)))
			{
				int channelSortedIndex;
				switch (e.KeyCode)
				{
					case Keys.Insert:
						if (this.SelectedChannel != null)
						{
							channelSortedIndex = this.GetChannelSortedIndex(this.SelectedChannel);
							int naturalIndex = this.m_sequence.InsertChannel(channelSortedIndex);
							this.InsertChannelIntoSort(naturalIndex, channelSortedIndex);
							this.ChannelCountChanged();
						}
						e.Handled = true;
						return;

					case Keys.Delete:
						if (!e.Shift)
						{
							if (this.SelectedChannel != null)
							{
								this.ClearChannel(this.GetChannelSortedIndex(this.SelectedChannel));
							}
						}
						else if ((this.SelectedChannel != null) && (MessageBox.Show(string.Format("Delete channel {0}?", this.SelectedChannel.Name), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
						{
							channelSortedIndex = this.GetChannelSortedIndex(this.SelectedChannel);
							this.m_sequence.DeleteChannel(this.SelectedChannel.Id);
							this.DeleteChannelFromSort(channelSortedIndex);
							this.ChannelCountChanged();
						}
						e.Handled = true;
						return;

					case Keys.D6:
						if (e.Shift && (this.SelectedChannel != null))
						{
							this.FillChannel(this.GetChannelSortedIndex(this.SelectedChannel));
						}
						e.Handled = true;
						return;
				}
			}
		}

		private void StandardSequence_Load(object sender, EventArgs e)
		{
			VixenEditor.ToolStripManager.SaveSettings(this, this.m_preferences.XmlDoc.DocumentElement, "reset");
			this.m_preferences.Flush();
			VixenEditor.ToolStripManager.LoadSettings(this, this.m_preferences.XmlDoc.DocumentElement);
			ToolStripPanel[] panelArray = new ToolStripPanel[] { this.toolStripContainer1.TopToolStripPanel, this.toolStripContainer1.BottomToolStripPanel, this.toolStripContainer1.LeftToolStripPanel, this.toolStripContainer1.RightToolStripPanel };
			List<string> list = new List<string>();
			foreach (ToolStripPanel panel in panelArray)
			{
				foreach (ToolStrip strip in panel.Controls)
				{
					this.m_toolStrips[strip.Text] = strip;
					list.Add(strip.Text);
				}
			}
			list.Sort();
			int num = 0;
			foreach (string str in list)
			{
				ToolStripMenuItem item = new ToolStripMenuItem(str);
				item.Tag = this.m_toolStrips[str];
				item.Checked = this.m_toolStrips[str].Visible;
				item.CheckOnClick = true;
				item.CheckStateChanged += this.m_toolStripCheckStateChangeHandler;
				this.toolbarsToolStripMenuItem.DropDownItems.Insert(num++, item);
			}
			this.m_actualLevels = this.m_preferences.GetBoolean("ActualLevels");
			this.UpdateLevelDisplay();
		}

		private void subtractionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ArithmeticPaste(ArithmeticOperation.Subtraction);
		}

		private void SyncAudioButton()
		{
			this.toolStripButtonAudio.Checked = this.m_sequence.Audio != null;
			this.toolStripButtonAudio.ToolTipText = (this.m_sequence.Audio != null) ? this.m_sequence.Audio.Name : "Add audio";
		}

		private void textBoxChannelCount_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				e.Handled = true;
				int result = 0;
				if (int.TryParse(this.textBoxChannelCount.Text, out result))
				{
					if (result < this.m_sequence.ChannelCount)
					{
						if (MessageBox.Show("This will reduce the number of channels and potentially lose data.\n\nAccept new channel count?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							this.SetChannelCount(result);
						}
						else
						{
							this.textBoxChannelCount.Text = this.m_sequence.ChannelCount.ToString();
						}
					}
					else if (result > this.m_sequence.ChannelCount)
					{
						this.SetChannelCount(result);
					}
				}
				else
				{
					this.textBoxChannelCount.Text = this.m_sequence.ChannelCount.ToString();
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
				string text = this.textBoxProgramLength.Text;
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
					this.textBoxProgramLength.Text = this.TimeString(this.m_sequence.Time);
					MessageBox.Show("Not a valid format for time.\nUse one of the following:\n\nSeconds\nMinutes:Seconds\nSeconds.Milliseconds\nMinutes:Seconds.Milliseconds", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else if (this.SetProgramTime(num4))
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
			this.m_showingOutputs = !this.m_showingOutputs;
			this.pictureBoxChannels.Refresh();
		}

		private void toolStripButtonAudio_Click(object sender, EventArgs e)
		{
			Audio audio = this.m_sequence.Audio;
			int integer = this.m_preferences.GetInteger("SoundDevice");
			this.Cursor = Cursors.WaitCursor;
			AudioDialog dialog = new AudioDialog(this.m_sequence, this.m_preferences.GetBoolean("EventSequenceAutoSize"), integer);
			this.Cursor = Cursors.Default;
			Audio audio2 = this.m_sequence.Audio;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.SetProgramTime(this.m_sequence.Time);
				base.IsDirty = true;
				this.pictureBoxGrid.Refresh();
			}
			this.SyncAudioButton();
			base.IsDirty |= audio2 != this.m_sequence.Audio;
			if (audio != this.m_sequence.Audio)
			{
				this.ParseAudioWaveform();
				this.pictureBoxTime.Refresh();
			}
			dialog.Dispose();
		}

		private void toolStripButtonChangeIntensity_Click(object sender, EventArgs e)
		{
			DrawingIntensityDialog dialog = new DrawingIntensityDialog(this.m_sequence, this.m_drawingLevel, this.m_actualLevels);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.SetDrawingLevel(dialog.SelectedIntensity);
			}
			dialog.Dispose();
		}

		private void toolStripButtonChannelOutputMask_Click(object sender, EventArgs e)
		{
			this.EditSequenceChannelMask();
		}

		private void toolStripButtonCopy_Click(object sender, EventArgs e)
		{
			this.CopyCells();
		}

		private void toolStripButtonCut_Click(object sender, EventArgs e)
		{
			this.CopyCells();
			this.TurnCellsOff();
		}

		private void toolStripButtonDeleteOrder_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(string.Format("Delete channel order '{0}'?", this.toolStripComboBoxChannelOrder.Text), Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				this.m_sequence.Sorts.Remove((VixenPlus.SortOrder) this.toolStripComboBoxChannelOrder.SelectedItem);
				this.toolStripComboBoxChannelOrder.Items.RemoveAt(this.toolStripComboBoxChannelOrder.SelectedIndex);
				this.toolStripButtonDeleteOrder.Enabled = false;
				base.IsDirty = true;
			}
		}

		private void toolStripButtonFindAndReplace_Click(object sender, EventArgs e)
		{
			if ((this.m_normalizedRange.Width == 0) || (this.m_normalizedRange.Height == 0))
			{
				MessageBox.Show("There are no cells to search", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				FindAndReplaceDialog dialog = new FindAndReplaceDialog(this.m_sequence.MinimumLevel, this.m_sequence.MaximumLevel, this.m_actualLevels);
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					int left;
					int num4;
					int num5;
					this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
					byte findValue = dialog.FindValue;
					byte replaceWithValue = dialog.ReplaceWithValue;
					if (this.m_actualLevels)
					{
						for (num5 = this.m_normalizedRange.Top; num5 < this.m_normalizedRange.Bottom; num5++)
						{
							num4 = this.m_channelOrderMapping[num5];
							left = this.m_normalizedRange.Left;
							while (left < this.m_normalizedRange.Right)
							{
								if (this.m_sequence.EventValues[num4, left] == findValue)
								{
									this.m_sequence.EventValues[num4, left] = replaceWithValue;
								}
								left++;
							}
						}
					}
					else
					{
						for (num5 = this.m_normalizedRange.Top; num5 < this.m_normalizedRange.Bottom; num5++)
						{
							num4 = this.m_channelOrderMapping[num5];
							for (left = this.m_normalizedRange.Left; left < this.m_normalizedRange.Right; left++)
							{
								if (((byte) Math.Round((double) ((this.m_sequence.EventValues[num4, left] * 100f) / 255f), MidpointRounding.AwayFromZero)) == findValue)
								{
									this.m_sequence.EventValues[num4, left] = (byte) Math.Round((double) ((((float) replaceWithValue) / 100f) * 255f), MidpointRounding.AwayFromZero);
								}
							}
						}
					}
					base.IsDirty = true;
					this.pictureBoxGrid.Refresh();
				}
				dialog.Dispose();
			}
		}

		private void toolStripButtonInsertPaste_Click(object sender, EventArgs e)
		{
			if (this.m_systemInterface.Clipboard != null)
			{
				byte[,] clipboard = this.m_systemInterface.Clipboard;
				int length = clipboard.GetLength(0);
				int width = clipboard.GetLength(1);
				int left = this.m_normalizedRange.Left;
				int num5 = left + width;
				int num6 = this.m_sequence.TotalEventPeriods - num5;
				for (int i = 0; (i < length) && ((this.m_normalizedRange.Top + i) < this.m_sequence.ChannelCount); i++)
				{
					int num7 = this.m_channelOrderMapping[this.m_normalizedRange.Top + i];
					for (int j = Math.Min(num6, this.m_sequence.TotalEventPeriods - this.m_normalizedRange.Left) - 1; j >= 0; j--)
					{
						this.m_sequence.EventValues[num7, num5 + j] = this.m_sequence.EventValues[num7, left + j];
					}
				}
				this.PasteOver();
				this.AddUndoItem(new Rectangle(this.m_normalizedRange.X, this.m_normalizedRange.Y, width, length), UndoOriginalBehavior.Insertion);
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
				if (this.m_actualLevels)
				{
					str = "255";
					if ((this.m_normalizedRange.Width == 1) && (this.m_normalizedRange.Height == 1))
					{
						str = this.m_sequence.EventValues[this.m_channelOrderMapping[this.m_normalizedRange.Top], this.m_normalizedRange.Left].ToString();
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
					if ((this.m_normalizedRange.Width == 1) && (this.m_normalizedRange.Height == 1))
					{
						str = ((int) Math.Round((double) ((this.m_sequence.EventValues[this.m_channelOrderMapping[this.m_normalizedRange.Top], this.m_normalizedRange.Left] * 100f) / 255f), MidpointRounding.AwayFromZero)).ToString();
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
			this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
			int bottom = this.m_normalizedRange.Bottom;
			int right = this.m_normalizedRange.Right;
			for (int i = this.m_normalizedRange.Top; i < bottom; i++)
			{
				int num5 = this.m_channelOrderMapping[i];
				if (this.m_sequence.Channels[num5].Enabled)
				{
					for (int j = this.m_normalizedRange.Left; j < right; j++)
					{
						this.m_sequence.EventValues[num5, j] = (byte) result;
					}
				}
			}
			this.m_selectionRectangle.Width = 0;
			this.pictureBoxGrid.Invalidate(this.SelectionToRectangle());
		}

		private void toolStripButtonInvert_Click(object sender, EventArgs e)
		{
			this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
			int bottom = this.m_normalizedRange.Bottom;
			int right = this.m_normalizedRange.Right;
			for (int i = this.m_normalizedRange.Top; i < bottom; i++)
			{
				int num4 = this.m_channelOrderMapping[i];
				if (this.m_sequence.Channels[num4].Enabled)
				{
					for (int j = this.m_normalizedRange.Left; j < right; j++)
					{
						this.m_sequence.EventValues[num4, j] = (byte) (this.m_sequence.MaximumLevel - this.m_sequence.EventValues[num4, j]);
					}
				}
			}
			this.m_selectionRectangle.Width = 0;
			this.pictureBoxGrid.Invalidate(this.SelectionToRectangle());
		}

		private void toolStripButtonLoop_CheckedChanged(object sender, EventArgs e)
		{
			this.m_executionInterface.SetLoopState(this.m_executionContextHandle, this.toolStripButtonLoop.Checked);
		}

		private void toolStripButtonMirrorHorizontal_Click(object sender, EventArgs e)
		{
			byte[,] buffer = new byte[this.m_normalizedRange.Height, this.m_normalizedRange.Width];
			for (int i = 0; i < this.m_normalizedRange.Height; i++)
			{
				int num3 = this.m_channelOrderMapping[this.m_normalizedRange.Top + i];
				int num2 = 0;
				int num = this.m_normalizedRange.Width - 1;
				while (num >= 0)
				{
					buffer[i, num2] = this.m_sequence.EventValues[num3, this.m_normalizedRange.Left + num];
					num--;
					num2++;
				}
			}
			this.m_systemInterface.Clipboard = buffer;
		}

		private void toolStripButtonMirrorVertical_Click(object sender, EventArgs e)
		{
			byte[,] buffer = new byte[this.m_normalizedRange.Height, this.m_normalizedRange.Width];
			int num2 = 0;
			int num4 = this.m_normalizedRange.Height - 1;
			while (num4 >= 0)
			{
				int num3 = this.m_channelOrderMapping[this.m_normalizedRange.Top + num4];
				for (int i = 0; i < this.m_normalizedRange.Width; i++)
				{
					buffer[num2, i] = this.m_sequence.EventValues[num3, this.m_normalizedRange.Left + i];
				}
				num4--;
				num2++;
			}
			this.m_systemInterface.Clipboard = buffer;
		}

		private void toolStripButtonOff_Click(object sender, EventArgs e)
		{
			this.TurnCellsOff();
		}

		private void toolStripButtonOn_Click(object sender, EventArgs e)
		{
			this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
			int bottom = this.m_normalizedRange.Bottom;
			int right = this.m_normalizedRange.Right;
			for (int i = this.m_normalizedRange.Top; i < bottom; i++)
			{
				int num4 = this.m_channelOrderMapping[i];
				if (this.m_sequence.Channels[num4].Enabled)
				{
					for (int j = this.m_normalizedRange.Left; j < right; j++)
					{
						this.m_sequence.EventValues[num4, j] = this.m_drawingLevel;
					}
				}
			}
			this.m_selectionRectangle.Width = 0;
			this.pictureBoxGrid.Invalidate(this.SelectionToRectangle());
		}

		private void toolStripButtonOpaquePaste_Click(object sender, EventArgs e)
		{
			if (this.m_systemInterface.Clipboard != null)
			{
				this.AddUndoItem(new Rectangle(this.m_normalizedRange.X, this.m_normalizedRange.Y, this.m_systemInterface.Clipboard.GetLength(1), this.m_systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite);
				this.PasteOver();
			}
		}

		private void toolStripButtonPartialRampOff_Click(object sender, EventArgs e)
		{
			RampQueryDialog dialog = new RampQueryDialog(this.m_sequence.MinimumLevel, this.m_sequence.MaximumLevel, true, this.m_actualLevels);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.Ramp(dialog.StartingLevel, dialog.EndingLevel);
			}
			dialog.Dispose();
		}

		private void toolStripButtonPartialRampOn_Click(object sender, EventArgs e)
		{
			RampQueryDialog dialog = new RampQueryDialog(this.m_sequence.MinimumLevel, this.m_sequence.MaximumLevel, false, this.m_actualLevels);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.Ramp(dialog.StartingLevel, dialog.EndingLevel);
			}
			dialog.Dispose();
		}

		private void toolStripButtonPause_Click(object sender, EventArgs e)
		{
			if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle) == 1)
			{
				this.m_positionTimer.Stop();
				this.m_executionInterface.ExecutePause(this.m_executionContextHandle);
				this.SetEditingState(true);
			}
		}

		private void toolStripButtonPlay_Click(object sender, EventArgs e)
		{
			int sequencePosition = 0;
			int num = this.m_executionInterface.EngineStatus(this.m_executionContextHandle, out sequencePosition);
			if (num != 1)
			{
				if (num != 2)
				{
					this.Reset();
				}
				if (this.m_executionInterface.ExecutePlay(this.m_executionContextHandle, sequencePosition, 0))
				{
					this.m_positionTimer.Start();
					this.SetEditingState(false);
				}
			}
		}

		private void toolStripButtonPlayPoint_Click(object sender, EventArgs e)
		{
			if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle) != 1)
			{
				this.SetEditingState(false);
				if (this.playOnlyTheSelectedRangeToolStripMenuItem.Checked)
				{
					this.m_executionInterface.ExecutePlay(this.m_executionContextHandle, this.m_normalizedRange.Left * this.m_sequence.EventPeriod, this.m_normalizedRange.Right * this.m_sequence.EventPeriod);
				}
				else
				{
					this.m_executionInterface.ExecutePlay(this.m_executionContextHandle, this.m_normalizedRange.Left * this.m_sequence.EventPeriod, this.m_sequence.TotalEventPeriods * this.m_sequence.EventPeriod);
				}
				this.m_positionTimer.Start();
			}
		}

		private void toolStripButtonPlaySpeedHalf_Click(object sender, EventArgs e)
		{
			this.SetAudioSpeed(0.5f);
		}

		private void toolStripButtonPlaySpeedNormal_Click(object sender, EventArgs e)
		{
			this.SetAudioSpeed(1f);
		}

		private void toolStripButtonPlaySpeedQuarter_Click(object sender, EventArgs e)
		{
			this.SetAudioSpeed(0.25f);
		}

		private void toolStripButtonPlaySpeedThreeQuarters_Click(object sender, EventArgs e)
		{
			this.SetAudioSpeed(0.75f);
		}

		private void toolStripButtonPlaySpeedVariable_Click(object sender, EventArgs e)
		{
			this.SetVariablePlaybackSpeed(this.toolStripExecutionControl.PointToScreen(new Point(this.toolStripButtonPlaySpeedVariable.Bounds.Right, this.toolStripButtonPlaySpeedVariable.Bounds.Top)));
		}

		private void toolStripButtonRampOff_Click(object sender, EventArgs e)
		{
			this.Ramp(this.m_sequence.MaximumLevel, this.m_sequence.MinimumLevel);
		}

		private void toolStripButtonRampOn_Click(object sender, EventArgs e)
		{
			this.Ramp(this.m_sequence.MinimumLevel, this.m_sequence.MaximumLevel);
		}

		private void toolStripButtonRandom_Click(object sender, EventArgs e)
		{
			RandomParametersDialog dialog = new RandomParametersDialog(this.m_sequence.MinimumLevel, this.m_sequence.MaximumLevel, this.m_actualLevels);
			int maximumLevel = this.m_sequence.MaximumLevel;
			int intensityMax = this.m_sequence.MaximumLevel;
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
				this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
				List<int> list = new List<int>();
				Random random2 = new Random();
				int top = this.m_normalizedRange.Top;
				while (top < this.m_normalizedRange.Bottom)
				{
					num10 = this.m_channelOrderMapping[top];
					if (this.m_sequence.Channels[num10].Enabled)
					{
						left = this.m_normalizedRange.Left;
						while (left < this.m_normalizedRange.Right)
						{
							this.m_sequence.EventValues[num10, left] = this.m_sequence.MinimumLevel;
							left++;
						}
					}
					top++;
				}
				for (left = this.m_normalizedRange.Left; left < this.m_normalizedRange.Right; left += periodLength)
				{
					int num8;
					if (dialog.UseSaturation)
					{
						if (random2.Next(2) > 0)
						{
							num8 = (int) Math.Ceiling((double) ((this.m_normalizedRange.Height * saturationLevel) - 0.1));
						}
						else
						{
							num8 = (int) Math.Floor((double) (this.m_normalizedRange.Height * saturationLevel));
						}
					}
					else
					{
						num8 = 0;
						while (num8 == 0)
						{
							num8 = random2.Next(this.m_normalizedRange.Height + 1);
						}
					}
					list.Clear();
					for (top = this.m_normalizedRange.Top; top < this.m_normalizedRange.Bottom; top++)
					{
						num10 = this.m_channelOrderMapping[top];
						list.Add(num10);
					}
					byte drawingLevel = this.m_drawingLevel;
					while (num8-- > 0)
					{
						int index = random2.Next(list.Count);
						if (random != null)
						{
							drawingLevel = (byte) random.Next(maximumLevel, intensityMax + 1);
						}
						for (int i = 0; (i < periodLength) && ((left + i) < this.m_normalizedRange.Right); i++)
						{
							this.m_sequence.EventValues[list[index], left + i] = drawingLevel;
						}
						list.RemoveAt(index);
					}
				}
				this.m_selectionRectangle.Width = 0;
				this.pictureBoxGrid.Invalidate(this.SelectionToRectangle());
			}
			dialog.Dispose();
		}

		private void toolStripButtonRedo_Click(object sender, EventArgs e)
		{
			this.Redo();
		}

		private void toolStripButtonRemoveCells_Click(object sender, EventArgs e)
		{
			if (this.m_normalizedRange.Width != 0)
			{
				int right = this.m_normalizedRange.Right;
				int num2 = this.m_sequence.TotalEventPeriods - right;
				this.AddUndoItem(new Rectangle(this.m_normalizedRange.Left, this.m_normalizedRange.Top, this.m_normalizedRange.Width, this.m_normalizedRange.Height), UndoOriginalBehavior.Removal);
				for (int i = 0; i < this.m_normalizedRange.Height; i++)
				{
					int num3 = this.m_channelOrderMapping[this.m_normalizedRange.Top + i];
					int num4 = 0;
					while (num4 < num2)
					{
						this.m_sequence.EventValues[num3, (right - this.m_normalizedRange.Width) + num4] = this.m_sequence.EventValues[num3, right + num4];
						num4++;
					}
					for (num4 = (right + num2) - this.m_normalizedRange.Width; num4 < this.m_sequence.TotalEventPeriods; num4++)
					{
						this.m_sequence.EventValues[num3, num4] = this.m_sequence.MinimumLevel;
					}
				}
				this.pictureBoxGrid.Refresh();
			}
		}

		private void toolStripButtonSave_Click(object sender, EventArgs e)
		{
			this.m_systemInterface.InvokeSave(this);
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
				foreach (VixenPlus.SortOrder order2 in this.m_sequence.Sorts)
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
				item.ChannelIndexes.AddRange(this.m_channelOrderMapping);
				this.toolStripComboBoxChannelOrder.SelectedItem = item;
			}
			else
			{
				this.m_sequence.Sorts.Add(item = new VixenPlus.SortOrder(dialog.Response, this.m_channelOrderMapping));
				this.toolStripComboBoxChannelOrder.Items.Insert(this.toolStripComboBoxChannelOrder.Items.Count - 1, item);
				this.toolStripComboBoxChannelOrder.SelectedIndex = this.toolStripComboBoxChannelOrder.Items.Count - 2;
			}
			dialog.Dispose();
			base.IsDirty = true;
		}

		private void toolStripButtonShimmerDimming_Click(object sender, EventArgs e)
		{
			int maxFrequency = 0x3e8 / this.m_sequence.EventPeriod;
			EffectFrequencyDialog dialog = new EffectFrequencyDialog("Shimmer (dimming)", maxFrequency, this.m_dimmingShimmerGenerator);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
				int bottom = this.m_normalizedRange.Bottom;
				int right = this.m_normalizedRange.Right;
				byte[,] values = new byte[this.m_normalizedRange.Height, this.m_normalizedRange.Width];
				this.DimmingShimmerGenerator(values, new int[] { dialog.Frequency });
				int top = this.m_normalizedRange.Top;
				for (int i = 0; top < bottom; i++)
				{
					int num5 = this.m_channelOrderMapping[top];
					if (this.m_sequence.Channels[num5].Enabled)
					{
						int left = this.m_normalizedRange.Left;
						for (int j = 0; left < right; j++)
						{
							this.m_sequence.EventValues[num5, left] = values[i, j];
							left++;
						}
					}
					top++;
				}
				this.m_selectionRectangle.Width = 0;
				this.pictureBoxGrid.Invalidate(this.SelectionToRectangle());
			}
			dialog.Dispose();
		}

		private void toolStripButtonSparkle_Click(object sender, EventArgs e)
		{
			int maxFrequency = 0x3e8 / this.m_sequence.EventPeriod;
			SparkleParamsDialog dialog = new SparkleParamsDialog(maxFrequency, this.m_sparkleGenerator, this.m_sequence.MinimumLevel, this.m_sequence.MaximumLevel, this.m_drawingLevel, this.m_actualLevels);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
				int bottom = this.m_normalizedRange.Bottom;
				int right = this.m_normalizedRange.Right;
				byte[,] values = new byte[this.m_normalizedRange.Height, this.m_normalizedRange.Width];
				this.SparkleGenerator(values, new int[] { dialog.Frequency, dialog.DecayTime, dialog.MinimumIntensity, dialog.MaximumIntensity });
				int top = this.m_normalizedRange.Top;
				for (int i = 0; top < bottom; i++)
				{
					int num5 = this.m_channelOrderMapping[top];
					if (this.m_sequence.Channels[num5].Enabled)
					{
						int left = this.m_normalizedRange.Left;
						for (int j = 0; left < right; j++)
						{
							this.m_sequence.EventValues[num5, left] = values[i, j];
							left++;
						}
					}
					top++;
				}
				this.m_selectionRectangle.Width = 0;
				this.pictureBoxGrid.Invalidate(this.SelectionToRectangle());
			}
			dialog.Dispose();
		}

		private void toolStripButtonStop_Click(object sender, EventArgs e)
		{
			if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle) != 0)
			{
				this.m_positionTimer.Stop();
				this.ProgramEnded();
				this.m_executionInterface.ExecuteStop(this.m_executionContextHandle);
			}
		}

		private void toolStripButtonTestChannels_Click(object sender, EventArgs e)
		{
			try
			{
				new TestChannelsDialog(this.m_sequence, this.m_executionInterface).Show();
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
				new TestConsoleDialog(this.m_sequence, this.m_executionInterface).Show();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void toolStripButtonToggleCellText_Click(object sender, EventArgs e)
		{
			this.m_showCellText = !this.m_showCellText;
			this.pictureBoxGrid.Refresh();
		}

		private void toolStripButtonToggleCrossHairs_Click(object sender, EventArgs e)
		{
			if (!this.toolStripButtonToggleCrossHairs.Checked)
			{
				this.pictureBoxGrid.Invalidate(new Rectangle((this.m_mouseTimeCaret - this.hScrollBar1.Value) * this.m_periodPixelWidth, 0, this.m_periodPixelWidth, this.pictureBoxGrid.Height));
				this.pictureBoxGrid.Update();
				this.pictureBoxGrid.Invalidate(new Rectangle(0, (this.m_mouseChannelCaret - this.vScrollBar1.Value) * this.m_gridRowHeight, this.pictureBoxGrid.Width, this.m_gridRowHeight));
				this.pictureBoxGrid.Update();
			}
		}

		private void toolStripButtonToggleLevels_Click(object sender, EventArgs e)
		{
			this.m_actualLevels = !this.m_actualLevels;
			this.m_preferences.SetBoolean("ActualLevels", this.m_actualLevels);
			this.UpdateLevelDisplay();
			this.pictureBoxGrid.Refresh();
		}

		private void toolStripButtonToggleRamps_Click(object sender, EventArgs e)
		{
			this.m_showingGradient = !this.m_showingGradient;
			this.m_preferences.SetBoolean("BarLevels", !this.m_showingGradient);
			this.pictureBoxGrid.Refresh();
		}

		private void toolStripButtonTransparentPaste_Click(object sender, EventArgs e)
		{
			if (this.m_systemInterface.Clipboard != null)
			{
				this.AddUndoItem(new Rectangle(this.m_normalizedRange.X, this.m_normalizedRange.Y, this.m_systemInterface.Clipboard.GetLength(1), this.m_systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite);
				byte[,] clipboard = this.m_systemInterface.Clipboard;
				int length = clipboard.GetLength(0);
				int num3 = clipboard.GetLength(1);
				int minimumLevel = this.m_sequence.MinimumLevel;
				for (int i = 0; (i < length) && ((this.m_normalizedRange.Top + i) < this.m_sequence.ChannelCount); i++)
				{
					int num4 = this.m_channelOrderMapping[this.m_normalizedRange.Top + i];
					for (int j = 0; (j < num3) && ((this.m_normalizedRange.Left + j) < this.m_sequence.TotalEventPeriods); j++)
					{
						byte num5 = clipboard[i, j];
						if (num5 > minimumLevel)
						{
							this.m_sequence.EventValues[num4, this.m_normalizedRange.Left + j] = num5;
						}
					}
				}
				base.IsDirty = true;
				this.pictureBoxGrid.Refresh();
			}
		}

		private void toolStripButtonUndo_Click(object sender, EventArgs e)
		{
			this.Undo();
		}

		private void toolStripButtonWaveform_Click(object sender, EventArgs e)
		{
			if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle) != 1)
			{
				if ((this.m_waveformPixelData == null) || (this.m_waveformPCMData == null))
				{
					this.ParseAudioWaveform();
				}
				if (this.toolStripButtonWaveform.Checked)
				{
					this.pictureBoxTime.Height = 120;
					this.EnableWaveformButton();
				}
				else
				{
					this.pictureBoxTime.Height = 60;
					this.toolStripLabelWaveformZoom.Enabled = false;
					this.toolStripComboBoxWaveformZoom.Enabled = false;
				}
				this.pictureBoxTime.Refresh();
				this.pictureBoxChannels.Refresh();
			}
		}

		private void toolStripComboBoxChannelOrder_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.toolStripComboBoxChannelOrder.SelectedIndex != -1)
			{
				if ((this.m_sequence.Profile != null) && (this.toolStripComboBoxChannelOrder.SelectedIndex == 0))
				{
					this.toolStripComboBoxChannelOrder.SelectedIndex = -1;
					MessageBox.Show("This sequence is attached to a profile.\nChanges to the profile affect all sequences attached to it.\n\nIf this is what you want to do, please use the profile manager.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					if (this.toolStripComboBoxChannelOrder.SelectedIndex == 0)
					{
						if (this.m_sequence.ChannelCount == 0)
						{
							MessageBox.Show("There are no channels to reorder.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return;
						}
						this.toolStripButtonDeleteOrder.Enabled = false;
						this.toolStripComboBoxChannelOrder.SelectedIndex = -1;
						ChannelOrderDialog dialog = new ChannelOrderDialog(this.m_sequence.Channels, this.m_channelOrderMapping);
						if (dialog.ShowDialog() == DialogResult.OK)
						{
							this.m_channelOrderMapping.Clear();
							foreach (VixenPlus.Channel channel in dialog.ChannelMapping)
							{
								this.m_channelOrderMapping.Add(this.m_sequence.Channels.IndexOf(channel));
							}
							base.IsDirty = true;
						}
						dialog.Dispose();
					}
					else if (this.toolStripComboBoxChannelOrder.SelectedIndex == (this.toolStripComboBoxChannelOrder.Items.Count - 1))
					{
						this.toolStripButtonDeleteOrder.Enabled = false;
						this.toolStripComboBoxChannelOrder.SelectedIndex = -1;
						this.m_channelOrderMapping.Clear();
						for (int i = 0; i < this.m_sequence.ChannelCount; i++)
						{
							this.m_channelOrderMapping.Add(i);
						}
						this.m_sequence.LastSort = -1;
						if (this.m_sequence.Profile == null)
						{
							base.IsDirty = true;
						}
					}
					else
					{
						this.m_channelOrderMapping.Clear();
						this.m_channelOrderMapping.AddRange(((VixenPlus.SortOrder) this.toolStripComboBoxChannelOrder.SelectedItem).ChannelIndexes);
						this.m_sequence.LastSort = this.toolStripComboBoxChannelOrder.SelectedIndex - 1;
						this.toolStripButtonDeleteOrder.Enabled = true;
						if (this.m_sequence.Profile == null)
						{
							base.IsDirty = true;
						}
					}
					this.pictureBoxChannels.Refresh();
					this.pictureBoxGrid.Refresh();
				}
			}
		}

		private void toolStripComboBoxColumnZoom_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.toolStripComboBoxColumnZoom.SelectedIndex != -1)
			{
				this.UpdateColumnWidth();
			}
		}

		private void toolStripComboBoxRowZoom_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.toolStripComboBoxRowZoom.SelectedIndex != -1)
			{
				this.UpdateRowHeight();
			}
		}

		private void toolStripComboBoxWaveformZoom_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((this.m_executionInterface.EngineStatus(this.m_executionContextHandle) != 1) && ((this.toolStripComboBoxWaveformZoom.SelectedIndex != -1) && this.toolStripComboBoxWaveformZoom.Enabled))
			{
				string selectedItem = (string) this.toolStripComboBoxWaveformZoom.SelectedItem;
				this.m_waveformMaxAmplitude = (int) ((100f / ((float) Convert.ToInt32(selectedItem.Substring(0, selectedItem.Length - 1)))) * this.m_waveform100PercentAmplitude);
				this.PCMToPixels(this.m_waveformPCMData, this.m_waveformPixelData);
				this.pictureBoxTime.Refresh();
			}
		}

		private void toolStripDropDownButtonPlugins_Click(object sender, EventArgs e)
		{
			PluginListDialog dialog = new PluginListDialog(this.m_sequence);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.toolStripDropDownButtonPlugins.DropDownItems.Clear();
				int num = 0;
				bool flag = false;
				foreach (object[] objArray in dialog.MappedPluginList)
				{
					ToolStripMenuItem item = new ToolStripMenuItem((string) objArray[0]);
					item.Checked = (bool) objArray[1];
					item.CheckOnClick = true;
					item.CheckedChanged += new EventHandler(this.plugInItem_CheckedChanged);
					item.Tag = num.ToString();
					num++;
					this.toolStripDropDownButtonPlugins.DropDownItems.Add(item);
					flag |= item.Checked;
				}
				if (this.toolStripDropDownButtonPlugins.DropDownItems.Count > 0)
				{
					this.toolStripDropDownButtonPlugins.DropDownItems.Add("-");
					this.toolStripDropDownButtonPlugins.DropDownItems.Add("Select all", null, new EventHandler(this.selectAllToolStripMenuItem_Click));
					this.toolStripDropDownButtonPlugins.DropDownItems.Add("Unselect all", null, new EventHandler(this.unselectAllToolStripMenuItem_Click));
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
			this.BooleanPaste(BooleanOperation.AND);
		}

		private void toolStripMenuItemPasteNand_Click(object sender, EventArgs e)
		{
			this.BooleanPaste(BooleanOperation.NAND);
		}

		private void toolStripMenuItemPasteNor_Click(object sender, EventArgs e)
		{
			this.BooleanPaste(BooleanOperation.NOR);
		}

		private void toolStripMenuItemPasteOr_Click(object sender, EventArgs e)
		{
			this.BooleanPaste(BooleanOperation.OR);
		}

		private void toolStripMenuItemPasteXnor_Click(object sender, EventArgs e)
		{
			this.BooleanPaste(BooleanOperation.XNOR);
		}

		private void toolStripMenuItemPasteXor_Click(object sender, EventArgs e)
		{
			this.BooleanPaste(BooleanOperation.XOR);
		}

		private void TurnCellsOff()
		{
			this.AddUndoItem(this.m_normalizedRange, UndoOriginalBehavior.Overwrite);
			int bottom = this.m_normalizedRange.Bottom;
			int right = this.m_normalizedRange.Right;
			for (int i = this.m_normalizedRange.Top; i < bottom; i++)
			{
				int num4 = this.m_channelOrderMapping[i];
				if (this.m_sequence.Channels[num4].Enabled)
				{
					for (int j = this.m_normalizedRange.Left; j < right; j++)
					{
						this.m_sequence.EventValues[num4, j] = this.m_sequence.MinimumLevel;
					}
				}
			}
			this.m_selectionRectangle.Width = 0;
			this.pictureBoxGrid.Invalidate(this.SelectionToRectangle());
		}

		private void Undo()
		{
			if (this.m_undoStack.Count != 0)
			{
				UndoItem item = (UndoItem) this.m_undoStack.Pop();
				int length = 0;
				int num2 = 0;
				if (item.Data != null)
				{
					length = item.Data.GetLength(0);
					num2 = item.Data.GetLength(1);
				}
				this.toolStripButtonUndo.Enabled = this.undoToolStripMenuItem.Enabled = this.m_undoStack.Count > 0;
				if (this.m_undoStack.Count > 0)
				{
					base.IsDirty = true;
				}
				UndoItem item2 = new UndoItem(item.Location, this.GetAffectedBlockData(new Rectangle(item.Location.X, item.Location.Y, item.Data.GetLength(1), item.Data.GetLength(0))), item.Behavior, this.m_sequence, this.m_channelOrderMapping);
				switch (item.Behavior)
				{
					case UndoOriginalBehavior.Overwrite:
						this.DisjointedOverwrite(item.Location.X, item.Location.Y, item.Data, item.ReferencedChannels);
						this.pictureBoxGrid.Refresh();
						break;

					case UndoOriginalBehavior.Removal:
						this.DisjointedInsert(item.Location.X, item.Location.Y, item.Data.GetLength(1), item.Data.GetLength(0), item.ReferencedChannels);
						this.DisjointedOverwrite(item.Location.X, item.Location.Y, item.Data, item.ReferencedChannels);
						this.pictureBoxGrid.Refresh();
						break;

					case UndoOriginalBehavior.Insertion:
						this.DisjointedRemove(item.Location.X, item.Location.Y, item.Data.GetLength(1), item.Data.GetLength(0), item.ReferencedChannels);
						this.pictureBoxGrid.Refresh();
						break;
				}
				this.UpdateUndoText();
				this.m_redoStack.Push(item2);
				this.toolStripButtonRedo.Enabled = this.redoToolStripMenuItem.Enabled = true;
				this.UpdateRedoText();
			}
		}

		private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ToolStripItem item in this.toolStripDropDownButtonPlugins.DropDownItems)
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
			double num = (this.toolStripComboBoxColumnZoom.SelectedIndex + 1) * 0.1;
			this.m_periodPixelWidth = (int) (this.m_preferences.GetInteger("MaxColumnWidth") * num);
			this.HScrollCheck();
			this.ParseAudioWaveform();
			this.pictureBoxGrid.Refresh();
			this.pictureBoxTime.Refresh();
		}

		private void UpdateGrid(Graphics g, Rectangle clipRect)
		{
			if (this.m_sequence.ChannelCount != 0)
			{
				using (Font font = new Font(this.Font.FontFamily, (this.m_periodPixelWidth <= 20) ? ((float) 5) : ((this.m_periodPixelWidth <= 25) ? ((float) 6) : ((this.m_periodPixelWidth < 50) ? ((float) 8) : ((float) 10)))))
				{
					using (SolidBrush brush = new SolidBrush(Color.White))
					{
						int num;
						int num3;
						int num7;
						VixenPlus.Channel channel;
						string str;
						int num5 = ((clipRect.X / this.m_periodPixelWidth) * this.m_periodPixelWidth) + 1;
						int y = ((clipRect.Y / this.m_gridRowHeight) * this.m_gridRowHeight) + 1;
						int num6 = (clipRect.X / this.m_periodPixelWidth) + this.hScrollBar1.Value;
						int cellY = (clipRect.Y / this.m_gridRowHeight) + this.vScrollBar1.Value;
						if (!this.m_showingGradient)
						{
							goto Label_0329;
						}
						while ((y < clipRect.Bottom) && (cellY < this.m_sequence.ChannelCount))
						{
							num7 = this.m_channelOrderMapping[cellY];
							channel = this.m_sequence.Channels[num7];
							num3 = num5;
							num = num6;
							while ((num3 < clipRect.Right) && (num < this.m_sequence.TotalEventPeriods))
							{
								brush.Color = this.GetGradientColor(this.m_gridBackBrush.Color, channel.Color, this.m_sequence.EventValues[num7, num]);
								g.FillRectangle(brush, num3, y, this.m_periodPixelWidth - 1, this.m_gridRowHeight - 1);
								if (this.m_showCellText && (this.GetCellIntensity(num, cellY, out str) > 0))
								{
									g.DrawString(str, font, Brushes.Black, new RectangleF((float) num3, (float) y, (float) (this.m_periodPixelWidth - 1), (float) (this.m_gridRowHeight - 1)));
								}
								num3 += this.m_periodPixelWidth;
								num++;
							}
							y += this.m_gridRowHeight;
							cellY++;
						}
						return;
					Label_0222:
						num7 = this.m_channelOrderMapping[cellY];
						channel = this.m_sequence.Channels[num7];
						num3 = num5;
						for (num = num6; (num3 < clipRect.Right) && (num < this.m_sequence.TotalEventPeriods); num++)
						{
							int height = ((this.m_gridRowHeight - 1) * this.m_sequence.EventValues[num7, num]) / 0xff;
							g.FillRectangle(channel.Brush, num3, ((y + this.m_gridRowHeight) - 1) - height, this.m_periodPixelWidth - 1, height);
							if (this.m_showCellText && (this.GetCellIntensity(num, cellY, out str) > 0))
							{
								g.DrawString(str, font, Brushes.Black, new RectangleF((float) num3, (float) y, (float) (this.m_periodPixelWidth - 1), (float) (this.m_gridRowHeight - 1)));
							}
							num3 += this.m_periodPixelWidth;
						}
						y += this.m_gridRowHeight;
						cellY++;
					Label_0329:
						if ((y < clipRect.Bottom) && (cellY < this.m_sequence.ChannelCount))
						{
							goto Label_0222;
						}
					}
				}
			}
		}

		private void UpdateLevelDisplay()
		{
			this.SetDrawingLevel(this.m_drawingLevel);
			if (this.m_actualLevels)
			{
				this.toolStripButtonToggleLevels.Image = this.pictureBoxLevelPercent.Image;
				this.toolStripButtonToggleLevels.Text = this.toolStripButtonToggleLevels.ToolTipText = "Show intensity levels as percent (0-100%)";
			}
			else
			{
				this.toolStripButtonToggleLevels.Image = this.pictureBoxLevelNumber.Image;
				this.toolStripButtonToggleLevels.Text = this.toolStripButtonToggleLevels.ToolTipText = "Show actual intensity levels (0-255)";
			}
			this.m_intensityAdjustDialog.ActualLevels = this.m_actualLevels;
		}

		private void UpdatePositionLabel(Rectangle rect, bool zeroWidthIsValid)
		{
			int milliseconds = rect.Left * this.m_sequence.EventPeriod;
			string str = this.TimeString(milliseconds);
			if (rect.Width > 1)
			{
				int num2 = (rect.Right - 1) * this.m_sequence.EventPeriod;
				string str2 = this.TimeString(num2);
				this.labelPosition.Text = string.Format("{0} - {1}\n({2})", str, str2, this.TimeString(num2 - milliseconds));
			}
			else if (((rect.Width == 0) && zeroWidthIsValid) || (rect.Width == 1))
			{
				this.labelPosition.Text = str;
			}
			else
			{
				this.labelPosition.Text = string.Empty;
			}
		}

		private void UpdateProgress()
		{
			int x = (this.m_previousPosition - this.hScrollBar1.Value) * this.m_periodPixelWidth;
			this.pictureBoxTime.Invalidate(new Rectangle(x, this.pictureBoxTime.Height - 0x23, this.m_periodPixelWidth + this.m_periodPixelWidth, 15));
			this.pictureBoxGrid.Invalidate(new Rectangle(x, 0, this.m_periodPixelWidth + this.m_periodPixelWidth, this.pictureBoxGrid.Height));
		}

		private void UpdateRedoText()
		{
			if (this.m_redoStack.Count > 0)
			{
				this.toolStripButtonRedo.ToolTipText = this.redoToolStripMenuItem.Text = "Redo: " + ((UndoItem) this.m_redoStack.Peek()).ToString();
			}
			else
			{
				this.toolStripButtonRedo.ToolTipText = this.redoToolStripMenuItem.Text = "Redo";
			}
		}

		private void UpdateRowHeight()
		{
			if (this.toolStripComboBoxRowZoom.SelectedIndex >= 6)
			{
				if (!(this.m_channelNameFont.Size == 8f))
				{
					this.m_channelNameFont.Dispose();
					this.m_channelNameFont = new Font("Arial", 8f);
				}
			}
			else if (this.toolStripComboBoxRowZoom.SelectedIndex <= 3)
			{
				if (!(this.m_channelNameFont.Size == 5f))
				{
					this.m_channelNameFont.Dispose();
					this.m_channelNameFont = new Font("Arial", 5f);
				}
			}
			else if (!(this.m_channelNameFont.Size == 7f))
			{
				this.m_channelNameFont.Dispose();
				this.m_channelNameFont = new Font("Arial", 7f);
			}
			double num = (this.toolStripComboBoxRowZoom.SelectedIndex + 1) * 0.1;
			this.m_gridRowHeight = (int) (this.m_preferences.GetInteger("MaxRowHeight") * num);
			this.VScrollCheck();
			this.pictureBoxGrid.Refresh();
			this.pictureBoxChannels.Refresh();
		}

		private void UpdateUndoText()
		{
			if (this.m_undoStack.Count > 0)
			{
				this.toolStripButtonUndo.ToolTipText = this.undoToolStripMenuItem.Text = "Undo: " + ((UndoItem) this.m_undoStack.Peek()).ToString();
			}
			else
			{
				this.toolStripButtonUndo.ToolTipText = this.undoToolStripMenuItem.Text = "Undo";
			}
		}

		private void vScrollBar1_ValueChanged(object sender, EventArgs e)
		{
			this.pictureBoxGrid.Refresh();
			this.pictureBoxChannels.Refresh();
		}

		private void VScrollCheck()
		{
			this.m_visibleRowCount = this.pictureBoxGrid.Height / this.m_gridRowHeight;
			this.vScrollBar1.Maximum = this.m_sequence.ChannelCount - 1;
			this.vScrollBar1.LargeChange = this.m_visibleRowCount;
			this.vScrollBar1.Enabled = this.m_visibleRowCount < this.m_sequence.ChannelCount;
			if (!this.vScrollBar1.Enabled)
			{
				this.vScrollBar1.Value = this.vScrollBar1.Minimum;
			}
			else if ((this.vScrollBar1.Value + this.m_visibleRowCount) > this.m_sequence.ChannelCount)
			{
				this.m_selectedRange.Y += this.m_visibleRowCount - this.m_sequence.ChannelCount;
				this.m_normalizedRange.Y += this.m_visibleRowCount - this.m_sequence.ChannelCount;
				this.vScrollBar1.Value = this.m_sequence.ChannelCount - this.m_visibleRowCount;
			}
			if (this.vScrollBar1.Maximum >= 0)
			{
				if (this.vScrollBar1.Value == -1)
				{
					this.vScrollBar1.Value = 0;
				}
				if (this.vScrollBar1.Minimum == -1)
				{
					this.vScrollBar1.Minimum = 0;
				}
			}
		}

		private void xToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			this.SetAudioSpeed(0.75f);
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
				return "Vixen standard sequence user interface";
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
				return "Vixen standard sequence";
			}
		}

		private VixenPlus.Channel SelectedChannel
		{
			get
			{
				return this.m_selectedChannel;
			}
			set
			{
				if (this.m_selectedChannel != value)
				{
					VixenPlus.Channel selectedChannel = this.m_selectedChannel;
					this.m_selectedChannel = value;
					if (selectedChannel != null)
					{
						this.pictureBoxChannels.Invalidate(this.GetChannelNameRect(selectedChannel));
					}
					if (this.m_selectedChannel != null)
					{
						this.pictureBoxChannels.Invalidate(this.GetChannelNameRect(this.m_selectedChannel));
					}
					this.pictureBoxChannels.Update();
				}
			}
		}

		public override EventSequence Sequence
		{
			get
			{
				return this.m_sequence;
			}
			set
			{
				this.m_sequence = value;
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
	}
}

