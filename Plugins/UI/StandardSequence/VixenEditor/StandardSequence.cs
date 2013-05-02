using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using FMOD;
using CommonUtils;

using Properties;

using VixenPlus;
using VixenPlus.Dialogs;

namespace VixenEditor {
    public partial class StandardSequence : UIBase {
        private bool _actualLevels;
        private AffectGridDelegate _affectGridDelegate;
        private bool _autoScrolling;
        private readonly int[] _bookmarks;
        private SolidBrush _channelBackBrush;
        private readonly SolidBrush _channelCaretBrush;
        private Font _channelNameFont;
        private Font _channelStrikeoutFont;
        private List<int> _channelOrderMapping;
        private VixenPlus.Channel _currentlyEditingChannel;
        private FrequencyEffectGenerator _dimmingShimmerGenerator;
        private byte _drawingLevel;
        private int _editingChannelSortedIndex;
        private int _executionContextHandle;
        private readonly IExecution _executionInterface;
        private SolidBrush _gridBackBrush;
        private Graphics _gridGraphics;
        private int _gridRowHeight;
        private bool _initializing;
        private int _intensityLargeDelta;
        private int _lastCellX;
        private int _lastCellY;
        private Rectangle _lineRect;
        private int _mouseChannelCaret;
        private Point _mouseDownAtInChannels;
        private Point _mouseDownAtInGrid;
        private bool _mouseDownInGrid;
        private int _mouseTimeCaret;
        private int _mouseWheelHorizontalDelta;
        private int _mouseWheelVerticalDelta;
        private Rectangle _selectedCells;
        private int _gridColWidth;
        private EventHandler _pluginCheckHandler;
        private int _position;
        private readonly SolidBrush _positionBrush;
        private Preference2 _preferences;
        private int _previousPosition;
        private readonly Stack _redoStack;
        private VixenPlus.Channel _selectedChannel;
        private int _selectedEventIndex;
        private int _selectedLineIndex;
        private Rectangle _selectedRange;
        private readonly SolidBrush _selectionBrush;
        private Rectangle _selectionRectangle;
        private EventSequence _sequence;
        private bool _showCellText;
        private bool _showingGradient;
        private bool _showingOutputs;
        private bool _showPositionMarker;
        private FrequencyEffectGenerator _sparkleGenerator;
        private readonly ISystem _systemInterface;
        private SolidBrush _timeBackBrush;
        private readonly Font _timeFont;
        private readonly EventHandler _toolStripCheckStateChangeHandler;
        private readonly Dictionary<string, ToolStrip> _toolStrips;
        private readonly Stack _undoStack;
        private int _visibleEventPeriods;
        private int _visibleRowCount;
        private int _waveformMaxAmplitude;
        private readonly int _waveformCenterLine;
        private uint[] _waveformPcmData;
        private uint[] _waveformPixelData;
        private PrintDocument _printDocument;

        private const int CaretSize = 5;
        private const int ToolbarInsertPosition = 4;
        private const int WaveformOffset = 6;


        public StandardSequence() {
            object obj2;
            _executionInterface = null;
            _systemInterface = null;
            _gridRowHeight = 20;
            _visibleRowCount = 0;
            _visibleEventPeriods = 0;
            _channelBackBrush = null;
            _timeBackBrush = null;
            _gridBackBrush = null;
            _channelNameFont = new Font("Arial", 8f);
            _channelStrikeoutFont = new Font("Arial", 8f, FontStyle.Strikeout);
            _timeFont = new Font("Arial", 8f);
            _selectedChannel = null;
            _currentlyEditingChannel = null;
            _editingChannelSortedIndex = 0;
            _gridGraphics = null;
            _selectedRange = new Rectangle();
            _gridColWidth = 30;
            _selectionRectangle = new Rectangle();
            _selectionBrush = new SolidBrush(Color.FromArgb(63, Color.Blue));
            _positionBrush = new SolidBrush(Color.FromArgb(63, Color.Red));
            _mouseDownInGrid = false;
            _position = -1;
            _previousPosition = -1;
            _mouseChannelCaret = -1;
            _mouseTimeCaret = -1;
            _channelCaretBrush = new SolidBrush(Color.Gray);
            _undoStack = new Stack();
            _redoStack = new Stack();
            _lineRect = new Rectangle(-1, -1, -1, -1);
            _lastCellX = -1;
            _lastCellY = -1;
            _initializing = false;
            _selectedEventIndex = -1;
            _selectedCells = new Rectangle();
            _mouseDownAtInGrid = new Point();
            _mouseDownAtInChannels = Point.Empty;
            _waveformPcmData = null;
            _waveformPixelData = null;
            _waveformCenterLine = 36;
            _showingOutputs = false;
            _selectedLineIndex = 0;
            _bookmarks = new[] {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
            _showingGradient = true;
            _actualLevels = false;
            _showCellText = false;
            m_lastSelectableControl = null;
            if (Interfaces.Available.TryGetValue("IExecution", out obj2)) {
                _executionInterface = (IExecution) obj2;
            }
            if (Interfaces.Available.TryGetValue("ISystem", out obj2)) {
                _systemInterface = (ISystem) obj2;
            }
            _toolStrips = new Dictionary<string, ToolStrip>();
            _toolStripCheckStateChangeHandler = toolStripItem_CheckStateChanged;
        }


        private void additionToolStripMenuItem_Click(object sender, EventArgs e) {
            ArithmeticPaste(ArithmeticOperation.Addition);
        }


        private void AddUndoItem(Rectangle blockAffected, UndoOriginalBehavior behavior, string originalAction) {
            if (blockAffected.Width == 0) {
                return;
            }

            var affectedBlockData = GetAffectedBlockData(blockAffected);
            _undoStack.Push(new UndoItem(blockAffected.Location, affectedBlockData, behavior, _sequence, _channelOrderMapping, originalAction));
            toolStripButtonUndo.Enabled = undoToolStripMenuItem.Enabled = true;
            UpdateUndoText();
            toolStripButtonRedo.Enabled = redoToolStripMenuItem.Enabled = false;
            _redoStack.Clear();
            UpdateRedoText();
            IsDirty = true;
        }


        private void AffectGrid(int startRow, int startCol, byte[,] values) {
            AddUndoItem(new Rectangle(startCol, startRow, values.GetLength(Utils.IndexColsOrWidth), values.GetLength(Utils.IndexRowsOrHeight)),
                        UndoOriginalBehavior.Overwrite, Resources.UndoText_CopyChannelData);
            CopyToEventValues(startCol, startRow, values);
            pictureBoxGrid.Refresh();
            IsDirty = true;
        }


        private void allChannelsToFullIntensityForThisEventToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_selectedEventIndex == -1) {
                return;
            }

            var blockAffected = new Rectangle(_selectedEventIndex, 0, 1, _sequence.ChannelCount);
            AddUndoItem(blockAffected, UndoOriginalBehavior.Overwrite, Resources.UndoText_EventOn);
            CopyToEventValues(_selectedEventIndex, 0, 1, _sequence.ChannelCount, _drawingLevel);
            InvalidateRect(blockAffected);
        }


        private void allEventsToFullIntensityToolStripMenuItem_Click(object sender, EventArgs e) {
            FillChannel(_selectedLineIndex);
        }


        private void ArithmeticPaste(ArithmeticOperation operation) {
            if (_systemInterface.Clipboard == null) {
                return;
            }

            var clipboard = _systemInterface.Clipboard;
            var height = clipboard.GetLength(Utils.IndexRowsOrHeight);
            var width = clipboard.GetLength(Utils.IndexColsOrWidth);

            AddUndoItem(new Rectangle(_selectedCells.X, _selectedCells.Y, width, height), UndoOriginalBehavior.Overwrite,
                        Resources.UndoText_ArithmeticPaste);

            var minLevel = _sequence.MinimumLevel;
            var maxLevel = _sequence.MaximumLevel;
            var rowOffset = _selectedCells.Top;
            var colOffset = _selectedCells.Left;
            for (var row = 0; (row < height) && ((rowOffset + row) < _sequence.ChannelCount); row++) {
                var channel = _channelOrderMapping[rowOffset + row];
                for (var col = 0; (col < width) && ((colOffset + col) < _sequence.TotalEventPeriods); col++) {
                    var currentValue = _sequence.EventValues[channel, colOffset + col];
                    var clipValue = clipboard[row, col];
                    switch (operation) {
                        case ArithmeticOperation.Addition:
                            currentValue = (byte) Math.Min(currentValue + clipValue, maxLevel);
                            break;

                        case ArithmeticOperation.Subtraction:
                            currentValue = (byte) Math.Max(currentValue - clipValue, minLevel);
                            break;

                        case ArithmeticOperation.Scale:
                            currentValue = (byte) Math.Max(Math.Min(currentValue * (clipValue / (float) Utils.Cell8BitMax), maxLevel), minLevel);
                            break;

                        case ArithmeticOperation.Min:
                            currentValue = Math.Max(Math.Min(clipValue, currentValue), minLevel);
                            break;

                        case ArithmeticOperation.Max:
                            currentValue = Math.Min(Math.Max(clipValue, currentValue), maxLevel);
                            break;
                    }
                    _sequence.EventValues[channel, colOffset + col] = currentValue;
                }
            }
            IsDirty = true;
            pictureBoxGrid.Refresh();
        }


        private void ArrayToCells(byte[,] array) {
            CopyToEventValues(_selectedCells.Left, _selectedCells.Top, array);
            IsDirty = true;
        }


        private void attachSequenceToToolStripMenuItem_Click(object sender, EventArgs e) {
            openFileDialog1.Filter = Resources.Profile + @" | *." + Vendor.ProfilExtension;
            openFileDialog1.DefaultExt = Vendor.ProfilExtension;
            openFileDialog1.InitialDirectory = Paths.ProfilePath;
            openFileDialog1.FileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                SetProfile(openFileDialog1.FileName);
            }
        }


        private void BooleanPaste(BooleanOperation operation) {
            if (_systemInterface.Clipboard == null) {
                return;
            }

            var clipboard = _systemInterface.Clipboard;
            var height = clipboard.GetLength(0);
            var width = clipboard.GetLength(1);
            var left = _selectedCells.Left;
            var top = _selectedCells.Top;

            AddUndoItem(new Rectangle(left, top, width, height), UndoOriginalBehavior.Overwrite, Resources.UndoText_BooleanPaste);

            for (var row = 0; row < height && top + row < _sequence.ChannelCount; row++) {
                var currentRow = _channelOrderMapping[top + row];
                for (var col = 0; col < width && left + col < _sequence.TotalEventPeriods; col++) {
                    var currentCol = left + col;
                    var currentValue = _sequence.EventValues[currentRow, currentCol];
                    var clipValue = clipboard[row, col];

                    switch (operation) {
                        case BooleanOperation.OR: {
                            currentValue |= clipValue;
                            break;
                        }
                        case BooleanOperation.AND: {
                            currentValue &= clipValue;
                            break;
                        }
                        case BooleanOperation.XOR: {
                            currentValue ^= clipValue;
                            break;
                        }
                        case BooleanOperation.NOR:
                            currentValue = (byte) ~(currentValue | clipValue);
                            break;

                        case BooleanOperation.NAND:
                            currentValue = (byte) ~(currentValue & clipValue);
                            break;

                        case BooleanOperation.XNOR:
                            currentValue = (byte) ~(currentValue ^ clipValue);
                            break;
                    }
                    _sequence.EventValues[currentRow, currentCol] = currentValue;
                }
            }
            IsDirty = true;
            pictureBoxGrid.Refresh();
        }


        private void BresenhamLine(Rectangle rect, IList<byte> brush) {
            int iterations;
            int diff;
            int negDiffIncr;
            int posDiffIncr;
            int negDiffLeftIncr;
            var posDiffLeftIncr = 1;
            int negDiffTopIncr;
            var posDiffTopIncr = 1;
            var width = Math.Abs(rect.Width);
            var height = Math.Abs(rect.Height);
            if (width >= height) {
                iterations = width + 1;
                diff = (height * 2) - width;
                negDiffIncr = height * 2;
                posDiffIncr = (height - width) * 2;
                negDiffLeftIncr = 1;
                negDiffTopIncr = 0;
            }
            else {
                iterations = height + 1;
                diff = (width * 2) - height;
                negDiffIncr = width * 2;
                posDiffIncr = (width - height) * 2;
                negDiffLeftIncr = 0;
                negDiffTopIncr = 1;
            }
            if (rect.Left > rect.Right) {
                negDiffLeftIncr = -negDiffLeftIncr;
                posDiffLeftIncr = -posDiffLeftIncr;
            }
            if (rect.Top > rect.Bottom) {
                negDiffTopIncr = -negDiffTopIncr;
                posDiffTopIncr = -posDiffTopIncr;
            }
            var left = rect.Left;
            var top = rect.Top;
            for (var i = 0; i < iterations; i++) {
                var columns = Math.Min(left + brush.Count, _sequence.TotalEventPeriods) - left;
                for (var j = 0; j < columns; j++) {
                    _sequence.EventValues[_channelOrderMapping[top], left + j] = brush[j];
                }
                if (diff < 0) {
                    diff += negDiffIncr;
                    left += negDiffLeftIncr;
                    top += negDiffTopIncr;
                }
                else {
                    diff += posDiffIncr;
                    left += posDiffLeftIncr;
                    top += posDiffTopIncr;
                }
            }
            IsDirty = true;
        }


        private void BresenhamLine(Rectangle rect) {
            BresenhamLine(rect, new List<byte> {_drawingLevel});
        }


        private byte[,] CellsToArray() {
            var buffer = new byte[_selectedCells.Height,_selectedCells.Width];
            for (var i = 0; i < _selectedCells.Height; i++) {
                var num2 = _channelOrderMapping[_selectedCells.Top + i];
                for (var j = 0; j < _selectedCells.Width; j++) {
                    buffer[i, j] = _sequence.EventValues[num2, _selectedCells.Left + j];
                }
            }
            return buffer;
        }


        private Rectangle CellsToRectangle(Rectangle relativeCells) {
            return new Rectangle {
                X = (Math.Min(relativeCells.Left, relativeCells.Right) * _gridColWidth) + 1,
                Y = (Math.Min(relativeCells.Top, relativeCells.Bottom) * _gridRowHeight) + 1,
                Width = ((Math.Abs(relativeCells.Width) + 1) * _gridColWidth) - 1,
                Height = ((Math.Abs(relativeCells.Height) + 1) * _gridRowHeight) - 1
            };
        }


        private bool ChannelClickValid() {
            var isValid = false;
            var mouseY = pictureBoxChannels.PointToClient(MousePosition).Y;

            if (mouseY > pictureBoxTime.Height + splitContainer2.SplitterWidth) {
                _selectedLineIndex = vScrollBar1.Value + (mouseY - (pictureBoxTime.Height + splitContainer2.SplitterWidth)) / _gridRowHeight;
                if (_selectedLineIndex < _sequence.ChannelCount) {
                    _editingChannelSortedIndex = _channelOrderMapping[_selectedLineIndex];
                    isValid = (_editingChannelSortedIndex >= 0) && (_editingChannelSortedIndex < _sequence.ChannelCount);
                }
            }

            if (isValid) {
                _currentlyEditingChannel = SelectedChannel = _sequence.Channels[_editingChannelSortedIndex];
            }

            return isValid;
        }


        private void ChannelCountChanged() {
            IsDirty = true;
            textBoxChannelCount.Text = _sequence.ChannelCount.ToString(CultureInfo.InvariantCulture);
            pictureBoxChannels.Refresh();
            pictureBoxGrid.Refresh();
        }


        private void channelOutputMaskToolStripMenuItem_Click(object sender, EventArgs e) {
            EditSequenceChannelMask();
        }


        private void channelPropertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowChannelProperties();
        }


        private DialogResult CheckDirty() {
            var dialogResult = DialogResult.None;

            if (IsDirty) {
                var prompt = string.Format(Resources.StringFormat_SaveChanges, _sequence.Name ?? Resources.SaveChanges_Unnamed);
                dialogResult = MessageBox.Show(prompt, Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes) {
                    _systemInterface.InvokeSave(this);
                }
            }

            return dialogResult;
        }


        private void CheckMaximums() {
            var dirtyFlag = false;

            for (var row = 0; row < _sequence.ChannelCount; row++) {
                var channel = _channelOrderMapping[row];
                for (var col = 0; col < _sequence.TotalEventPeriods; col++) {
                    var currentValue = _sequence.EventValues[channel, col];
                    if (currentValue == 0 || currentValue <= _sequence.MaximumLevel) {
                        continue;
                    }
                    _sequence.EventValues[channel, col] = _sequence.MaximumLevel;
                    dirtyFlag = true;
                }
            }

            if (dirtyFlag) {
                IsDirty = true;
            }
        }


        private void CheckMinimums() {
            var dirtyFlag = false;

            for (var row = 0; row < _sequence.ChannelCount; row++) {
                var channel = _channelOrderMapping[row];
                for (var col = 0; col < _sequence.TotalEventPeriods; col++) {
                    var currentValue = _sequence.EventValues[channel, col];
                    if (currentValue >= _sequence.MinimumLevel) {
                        continue;
                    }
                    _sequence.EventValues[channel, col] = _sequence.MinimumLevel;
                    dirtyFlag = true;
                }
            }

            if (dirtyFlag) {
                IsDirty = true;
            }
        }


        private void clearAllChannelsForThisEventToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_selectedEventIndex == -1) {
                return;
            }

            var blockAffected = new Rectangle(_selectedEventIndex, 0, 1, _sequence.ChannelCount);
            AddUndoItem(blockAffected, UndoOriginalBehavior.Overwrite, Resources.UndoText_ClearValues);
            CopyToEventValues(_selectedEventIndex, 0, 1, _sequence.ChannelCount, _sequence.MinimumLevel);
            InvalidateRect(blockAffected);
        }


        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e) {
            if (MessageBox.Show(Resources.ClearAllEventsInSequencePrompt, Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) {
                return;
            }

            var normalizedRange = _selectedCells;
            _selectedCells = new Rectangle(0, 0, _sequence.TotalEventPeriods, _sequence.ChannelCount);
            TurnCellsOff(Resources.UndoText_ClearAllEvents);
            _selectedCells = normalizedRange;
            pictureBoxGrid.Refresh();
        }


        private void ClearChannel(int lineIndex) {
            AddUndoItem(new Rectangle(0, lineIndex, _sequence.TotalEventPeriods, 1), UndoOriginalBehavior.Overwrite, Resources.UndoText_ClearChannel);
            CopyToEventValues(0, lineIndex, _sequence.TotalEventPeriods, 1, _sequence.MinimumLevel);
            pictureBoxGrid.Refresh();
        }


        private void clearChannelEventsToolStripMenuItem_Click(object sender, EventArgs e) {
            ClearChannel(_selectedLineIndex);
        }


        private void contextMenuChannels_Opening(object sender, CancelEventArgs e) {
            e.Cancel = !ChannelClickValid();
        }


        private void contextMenuGrid_Opening(object sender, CancelEventArgs e) {
            if (_currentlyEditingChannel == null) {
                e.Cancel = true;
            }
            saveAsARoutineToolStripMenuItem.Enabled = _selectedCells.Width >= 1;
        }


        private void contextMenuTime_Opening(object sender, CancelEventArgs e) {
            _selectedEventIndex = hScrollBar1.Value + (pictureBoxTime.PointToClient(MousePosition).X / _gridColWidth);
        }


        private void CopyCells() {
            _systemInterface.Clipboard = CellsToArray();
        }


        private void CopyToEventValues(int startCol, int startRow, byte[,] data) {
            var rows = data.GetLength(Utils.IndexRowsOrHeight);
            var columns = data.GetLength(Utils.IndexColsOrWidth);

            for (var row = 0; row < rows && startRow + row < _sequence.ChannelCount; row++) {
                var channel = _channelOrderMapping[startRow + row];
                for (var col = 0; col < columns && startCol + col < _sequence.TotalEventPeriods; col++) {
                    _sequence.EventValues[channel, startCol + col] = data[row, col];
                }
            }
        }


        private void CopyToEventValues(int startCol, int startRow, int width, int height, byte value) {
            var endRow = startRow + height;
            var endCol = startCol + width;

            for (var row = startRow; row < endRow; row++) {
                for (var col = startCol; col < endCol; col++) {
                    _sequence.EventValues[row, col] = value;
                }
            }
        }


        private void SetSelectedCellValue(byte value, bool mapChannel = true) {
            var rows = _selectedCells.Bottom;
            var columns = _selectedCells.Right;
            for (var row = _selectedCells.Top; row < rows; row++) {
                var channel = mapChannel ? _channelOrderMapping[row] : row;
                if (!_sequence.Channels[channel].Enabled) {
                    continue;
                }

                for (var col = _selectedCells.Left; col < columns; col++) {
                    _sequence.EventValues[channel, col] = value;
                }
            }
        }


        private void copyChannelEventsToClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
            var normalizedRange = _selectedCells;
            _selectedCells = new Rectangle(0, _editingChannelSortedIndex, _sequence.TotalEventPeriods, 1);
            _systemInterface.Clipboard = CellsToArray();
            _selectedCells = normalizedRange;
        }


        private void copyChannelToolStripMenuItem_Click(object sender, EventArgs e) {
            new ChannelCopyDialog(_affectGridDelegate, _sequence, _channelOrderMapping).Show();
        }


        private void createFromSequenceToolStripMenuItem_Click(object sender, EventArgs e) {
            var objectInContext = new Profile();
            objectInContext.InheritChannelsFrom(_sequence);
            objectInContext.InheritPlugInDataFrom(_sequence);
            objectInContext.InheritSortsFrom(_sequence);
            using (var dialog = new ProfileManagerDialog(objectInContext)) {
                if ((dialog.ShowDialog() == DialogResult.OK) &&
                    (MessageBox.Show(Resources.AttachToNewProfile, Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                     DialogResult.Yes)) {
                    SetProfile(objectInContext);
                }
            }
        }


        private void currentProgramsSettingsToolStripMenuItem_Click(object sender, EventArgs e) {
            var originalMinimumLevel = _sequence.MinimumLevel;
            var originalMaximumLevel = _sequence.MaximumLevel;
            var originalEventPeriod = _sequence.EventPeriod;

            using (var dialog = new SequenceSettingsDialog(_sequence)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }
            }

            if (originalMinimumLevel != _sequence.MinimumLevel) {
                CheckMinimums();
                if (_drawingLevel < _sequence.MinimumLevel) {
                    var message = String.Format(Resources.StringFormat_DrawingLevelBelow, _sequence.MinimumLevel, _drawingLevel);
                    SetDrawingLevel(_sequence.MinimumLevel);
                    MessageBox.Show(message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }

            if (originalMaximumLevel != _sequence.MaximumLevel) {
                CheckMaximums();
                if (_drawingLevel > _sequence.MaximumLevel) {
                    var message = String.Format(Resources.StringFormat_DrawingLevelAbove, _sequence.MaximumLevel, _drawingLevel);
                    SetDrawingLevel(_sequence.MaximumLevel);
                    MessageBox.Show(message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }

            if (originalEventPeriod != _sequence.EventPeriod) {
                HScrollCheck();
                ParseAudioWaveform();
                pictureBoxTime.Refresh();
            }

            pictureBoxGrid.Refresh();
        }


        private void DeleteChannelFromSort(int naturalIndex) {
            _channelOrderMapping.Remove(naturalIndex);
            for (var channel = 0; channel < _channelOrderMapping.Count; channel++) {
                if (_channelOrderMapping[channel] > naturalIndex) {
                    _channelOrderMapping[channel]--;
                }
            }
        }


        private void detachSequenceFromItsProfileToolStripMenuItem_Click(object sender, EventArgs e) {
            if (MessageBox.Show(Resources.DetachSequenceFromProfile, Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes) {
                SetProfile((Profile) null);
            }
        }


        private void DimmingShimmerGenerator(byte[,] values, params int[] effectParameters) {
            var effectsCount = effectParameters[0];
            if (effectsCount == 0) {
                return;
            }

            var colsPerCycle = (int) Math.Round(_sequence.EventsPerSecond / effectsCount, MidpointRounding.AwayFromZero);
            if (colsPerCycle == 0) {
                return;
            }

            var rows = values.GetLength(Utils.IndexRowsOrHeight);
            var totalCols = values.GetLength(Utils.IndexColsOrWidth);
            var random = new Random();

            for (var colOffset = 0; colOffset < totalCols; colOffset += colsPerCycle) {
                var randomValue = (byte) Math.Max(random.NextDouble() * _sequence.MaximumLevel, _sequence.MinimumLevel);
                var cols = Math.Min(totalCols, colOffset + colsPerCycle) - colOffset;
                for (var col = 0; col < cols; col++) {
                    for (var row = 0; row < rows; row++) {
                        values[row, colOffset + col] = randomValue;
                    }
                }
            }
        }


        private void DisableWaveformButton() {
            toolStripButtonWaveform.Enabled = false;
        }


        private void DisableWaveformDisplay() {
            toolStripButtonWaveform.Checked = false;
            _waveformPixelData = null;
            _waveformPcmData = null;
            pictureBoxTime.Height = 60;
            pictureBoxTime.Refresh();
            pictureBoxChannels.Refresh();
            DisableWaveformButton();
        }


        private void DisjointedInsert(int colOffset, int width, int height, IList<int> channelIndexes) {
            for (var row = 0; row < height; row++) {
                var channel = channelIndexes[row];
                for (var col = _sequence.TotalEventPeriods - colOffset - width - 1; col >= 0; col--) {
                    _sequence.EventValues[channel, col + colOffset + width] = _sequence.EventValues[channel, col + colOffset];
                }
            }
        }


        private void DisjointedOverwrite(int colOffset, byte[,] data, IList<int> channelIndexes) {
            for (var row = 0; row < data.GetLength(Utils.IndexRowsOrHeight); row++) {
                var channel = channelIndexes[row];
                for (var col = 0; col < data.GetLength(Utils.IndexColsOrWidth); col++) {
                    _sequence.EventValues[channel, col + colOffset] = data[row, col];
                }
            }
        }


        private void DisjointedRemove(int colOffset, int width, int height, IList<int> channelIndexes) {
            for (var row = 0; row < height; row++) {
                var channel = channelIndexes[row];
                for (var col = 0; col < _sequence.TotalEventPeriods - colOffset - width; col++) {
                    _sequence.EventValues[channel, col + colOffset] = _sequence.EventValues[channel, col + colOffset + width];
                }
            }

            for (var row = 0; row < height; row++) {
                var channel = channelIndexes[row];
                for (var col = 0; col < width; col++) {
                    _sequence.EventValues[channel, _sequence.TotalEventPeriods - width + col] = _sequence.MinimumLevel;
                }
            }
        }


        private void DrawSelectedRange() {
            var leftTop = new Point
            {X = (_selectedCells.Left - hScrollBar1.Value) * _gridColWidth, Y = (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight};

            var rightBottom = new Point
            {X = (_selectedCells.Right - hScrollBar1.Value) * _gridColWidth, Y = (_selectedCells.Bottom - vScrollBar1.Value) * _gridRowHeight};

            _selectionRectangle.X = leftTop.X;
            _selectionRectangle.Y = leftTop.Y;
            _selectionRectangle.Width = rightBottom.X - leftTop.X;
            _selectionRectangle.Height = rightBottom.Y - leftTop.Y;

            if (_selectionRectangle.Width == 0) {
                _selectionRectangle.Width = _gridColWidth;
            }

            if (_selectionRectangle.Height == 0) {
                _selectionRectangle.Height = _gridRowHeight;
            }

            pictureBoxGrid.Invalidate(_selectionRectangle);
            pictureBoxGrid.Update();
        }


        private void EditSequenceChannelMask() {
            using (var dialog = new ChannelOutputMaskDialog(_sequence.Channels)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                foreach (var channel in _sequence.Channels) {
                    channel.Enabled = true;
                }

                foreach (var num in dialog.DisabledChannels) {
                    _sequence.Channels[num].Enabled = false;
                }
            }
            IsDirty = true;
            pictureBoxChannels.Invalidate();
            pictureBoxChannels.Refresh();
        }


        private void EnableWaveformButton() {
            if (_sequence.Audio == null) {
                return;
            }

            toolStripButtonWaveform.Enabled = true;
        }


        private void EraseRectangleEntity(Rectangle rect) {
            rect.Offset(-hScrollBar1.Value, -vScrollBar1.Value);
            var rc = CellsToRectangle(rect);
            pictureBoxGrid.Invalidate(rc);
        }


        private void EraseSelectedRange() {
            var rc = SelectionToRectangle();
            _selectedCells.Width = _selectedRange.Width = 0;
            pictureBoxGrid.Invalidate(rc);
        }


        private void exportChannelNamesListToolStripMenuItem_Click(object sender, EventArgs e) {
            var path = Path.Combine(Paths.ImportExportPath, _sequence.Name + "_" + Resources.Channels + @".txt");
            using (var sw = new StreamWriter(path, false)) {
                foreach (var channel in _sequence.Channels) {
                    sw.WriteLine(channel.Name);
                }
            }
            MessageBox.Show(Resources.ChannelNameListExported + @" " + path, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        private void FillChannel(int lineIndex) {
            AddUndoItem(new Rectangle(0, lineIndex, _sequence.TotalEventPeriods, 1), UndoOriginalBehavior.Overwrite, Resources.UndoText_Fill);
            CopyToEventValues(0, _editingChannelSortedIndex, _sequence.TotalEventPeriods, 1, _drawingLevel);
            pictureBoxGrid.Refresh();
            IsDirty = true;
        }


        private void flattenProfileIntoSequenceToolStripMenuItem_Click(object sender, EventArgs e) {
            if (MessageBox.Show(Resources.FlattendProfileIntoSequence, Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.No) {
                return;
            }

            var flattenedProfile = _sequence.Profile;
            _sequence.Profile = null;
            var list = new List<VixenPlus.Channel>();
            list.AddRange(flattenedProfile.Channels);
            _sequence.Channels = list;
            _sequence.Sorts.LoadFrom(flattenedProfile.Sorts);
            _sequence.PlugInData.LoadFromXml(flattenedProfile.PlugInData.RootNode.ParentNode);
            IsDirty = true;
            ReactToProfileAssignment();
        }


        private byte[,] GetAffectedBlockData(Rectangle blockAffected) {
            if (blockAffected.Width < 0) {
                blockAffected.X += blockAffected.Width;
                blockAffected.Width = -blockAffected.Width;
            }

            if (blockAffected.Height < 0) {
                blockAffected.Y += blockAffected.Height;
                blockAffected.Height = -blockAffected.Height;
            }

            if (blockAffected.Right > _sequence.TotalEventPeriods) {
                blockAffected.Width = _sequence.TotalEventPeriods - blockAffected.Left;
            }

            if (blockAffected.Bottom > _sequence.ChannelCount) {
                blockAffected.Height = _sequence.ChannelCount - blockAffected.Top;
            }

            var buffer = new byte[blockAffected.Height,blockAffected.Width];
            for (var row = 0; row < blockAffected.Height; row++) {
                var channel = _channelOrderMapping[blockAffected.Y + row];
                for (var col = 0; col < blockAffected.Width; col++) {
                    buffer[row, col] = _sequence.EventValues[channel, blockAffected.X + col];
                }
            }

            return buffer;
        }


        private int GetCellIntensity(int column, int row, out string intensityText) {
            var intensity = 0;
            intensityText = "";

            if (column >= 0 && row >= 0) {
                var value = _sequence.EventValues[_channelOrderMapping[row], column];
                intensity = _actualLevels ? value : Utils.ToPercentage(value);
                intensityText = string.Format(_actualLevels ? "{0}" : "{0}%", intensity);
            }

            return intensity;
        }


        private VixenPlus.Channel GetChannelAt(Point point) {
            return GetChannelAtSortedIndex(GetLineIndexAt(point));
        }


        private VixenPlus.Channel GetChannelAtSortedIndex(int index) {
            return index < _channelOrderMapping.Count ? _sequence.Channels[_channelOrderMapping[index]] : null;
        }


        private Rectangle GetChannelNameRect(VixenPlus.Channel channel) {
            return channel != null
                       ? new Rectangle(0,
                                       (GetChannelSortedIndex(channel) - vScrollBar1.Value) * _gridRowHeight + pictureBoxTime.Height +
                                       splitContainer2.SplitterWidth, pictureBoxChannels.Width, _gridRowHeight) : Rectangle.Empty;
        }


        private int GetChannelNaturalIndex(VixenPlus.Channel channel) {
            return _sequence.Channels.IndexOf(channel);
        }


        private int GetChannelSortedIndex(VixenPlus.Channel channel) {
            return _channelOrderMapping.IndexOf(_sequence.Channels.IndexOf(channel));
        }


        private static Color GetGradientColor(Color startColor, Color endColor, int level) {
            var num = level / (float) Utils.Cell8BitMax;
            var r = (int) (((endColor.R - startColor.R) * num) + startColor.R);
            var g = (int) (((endColor.G - startColor.G) * num) + startColor.G);
            var b = (int) (((endColor.B - startColor.B) * num) + startColor.B);
            return Color.FromArgb(r, g, b);
        }


        private int GetLineIndexAt(Point point) {
            return (point.Y - (pictureBoxTime.Height + splitContainer2.SplitterWidth)) / _gridRowHeight + vScrollBar1.Value;
        }


        private static byte[,] GetRoutine() {
            var list = new List<string[]>();
            using (var dialog = new RoutineSelectDialog()) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    using (var sr = new StreamReader(dialog.SelectedRoutine)) {
                        string line;
                        while ((line = sr.ReadLine()) != null) {
                            list.Add(line.Trim().Split(new[] {' '}));
                        }
                    }
                    var length = list[0].Length;
                    var count = list.Count;
                    var buffer = new byte[count,length];
                    for (var row = 0; row < count; row++) {
                        for (var col = 0; col < length; col++) {
                            buffer[row, col] = Convert.ToByte(list[row][col]);
                        }
                    }
                    return buffer;
                }
            }
            return null;
        }


        private static uint GetSampleMinMax(int startSample, int sampleCount, Sound sound, int bitsPerSample, int audioChannels) {
            var bytesPerSample = (bitsPerSample >> 3) * audioChannels;

            var audioByte = startSample * bytesPerSample;
            var length = bytesPerSample * sampleCount;
            var ptr1 = IntPtr.Zero;
            var ptr2 = IntPtr.Zero;
            uint refLen1 = 0;
            uint refLen2 = 0;
            sound.@lock((uint) audioByte, (uint) length, ref ptr1, ref ptr2, ref refLen1, ref refLen2);

            var destination = new byte[length];
            Marshal.Copy(ptr1, destination, 0, length);

            audioByte = 0;
            var max = short.MinValue;
            var min = short.MaxValue;

            for (var currentSample = 0; currentSample < sampleCount; currentSample++) {
                for (var currentAudioChannel = 0; currentAudioChannel < audioChannels; currentAudioChannel++) {
                    var amplitude = (bitsPerSample == 16)
                                        ? BitConverter.ToInt16(destination, audioByte + (currentAudioChannel * 2))
                                        : (sbyte) destination[audioByte + currentAudioChannel];
                    max = Math.Max(max, amplitude);
                    min = Math.Min(min, amplitude);
                }
                audioByte += bytesPerSample;
            }

            sound.unlock(ptr1, ptr2, refLen1, refLen2);
            return (uint) ((max << 16) | ((ushort) min));
        }


        private Control GetTerminalSelectableControl() {
            var activeControl = ActiveControl;
            while (activeControl is IContainerControl) {
                activeControl = (activeControl as IContainerControl).ActiveControl;
            }
            return activeControl;
        }


        private void halfSpeedToolStripMenuItem_Click(object sender, EventArgs e) {
            SetAudioSpeed(0.5f);
        }


        private void hScrollBar1_ValueChanged(object sender, EventArgs e) {
            pictureBoxGrid.Refresh();
            pictureBoxTime.Refresh();
        }


        private void HScrollCheck() {
            if (_gridColWidth != 0) {
                _visibleEventPeriods = pictureBoxGrid.Width / _gridColWidth;
                hScrollBar1.LargeChange = _visibleEventPeriods;
                hScrollBar1.Maximum = Math.Max(0, _sequence.TotalEventPeriods - 1);
                hScrollBar1.Enabled = _visibleEventPeriods < _sequence.TotalEventPeriods;

                if (!hScrollBar1.Enabled) {
                    hScrollBar1.Value = hScrollBar1.Minimum;
                }
                else if ((hScrollBar1.Value + _visibleEventPeriods) > _sequence.TotalEventPeriods) {
                    _selectedRange.X += _visibleEventPeriods - _sequence.TotalEventPeriods;
                    _selectedCells.X += _visibleEventPeriods - _sequence.TotalEventPeriods;
                    hScrollBar1.Value = _sequence.TotalEventPeriods - _visibleEventPeriods;
                }
            }

            if (hScrollBar1.Maximum < 0) {
                return;
            }

            if (hScrollBar1.Value == -1) {
                hScrollBar1.Value = 0;
            }

            if (hScrollBar1.Minimum == -1) {
                hScrollBar1.Minimum = 0;
            }
        }


        private void importChannelNamesListToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_sequence.Profile != null) {
                MessageBox.Show(Resources.CantImportChannels, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                openFileDialog1.Filter = Resources.TextFile + @" | *.txt";
                openFileDialog1.DefaultExt = @"txt";
                openFileDialog1.InitialDirectory = Paths.ImportExportPath;
                openFileDialog1.FileName = string.Empty;
                if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                    using (var reader = new StreamReader(openFileDialog1.FileName)) {
                        var list = new List<string>();
                        string str;
                        while ((str = reader.ReadLine()) != null) {
                            list.Add(str);
                        }
                        SetChannelCount(list.Count);
                        for (var i = 0; i < list.Count; i++) {
                            _sequence.Channels[i].Name = list[i];
                        }
                        pictureBoxChannels.Refresh();
                        MessageBox.Show(Resources.ChannelNameImport, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }


        private void Init() {
            _initializing = true;
            InitializeComponent();
            _initializing = false;

            _gridRowHeight = _preferences.GetInteger("MaxRowHeight");
            _gridColWidth = _preferences.GetInteger("MaxColumnWidth");
            _showPositionMarker = _preferences.GetBoolean("ShowPositionMarker");
            _autoScrolling = _preferences.GetBoolean("AutoScrolling");
            _intensityLargeDelta = _preferences.GetInteger("IntensityLargeDelta");
            _showingGradient = !_preferences.GetBoolean("BarLevels");

            _channelBackBrush = new SolidBrush(Color.White);
            _timeBackBrush = new SolidBrush(Color.FromArgb(16, 16, 16));
            _gridBackBrush = new SolidBrush(Color.FromArgb(192, 192, 192));

            _gridGraphics = pictureBoxGrid.CreateGraphics();
            _dimmingShimmerGenerator = DimmingShimmerGenerator;
            _sparkleGenerator = SparkleGenerator;
            m_intensityAdjustDialog = new IntensityAdjustDialog(_actualLevels);
            m_intensityAdjustDialog.VisibleChanged += m_intensityAdjustDialog_VisibleChanged;
            _affectGridDelegate = AffectGrid;
            _pluginCheckHandler = plugInItem_CheckedChanged;
            _channelOrderMapping = new List<int>();
            for (var i = 0; i < _sequence.ChannelCount; i++) {
                _channelOrderMapping.Add(i);
            }
            ReadFromSequence();
            _executionContextHandle = _executionInterface.RequestContext(true, false, this);
            toolStripComboBoxColumnZoom.SelectedIndex = toolStripComboBoxColumnZoom.Items.Count - 1;
            toolStripComboBoxRowZoom.SelectedIndex = toolStripComboBoxRowZoom.Items.Count - 1;
            SyncAudioButton();
            _mouseWheelVerticalDelta = _preferences.GetInteger("MouseWheelVerticalDelta");
            _mouseWheelHorizontalDelta = _preferences.GetInteger("MouseWheelHorizontalDelta");
            if (_preferences.GetBoolean("SaveZoomLevels")) {
                int index = toolStripComboBoxColumnZoom.Items.IndexOf(_preferences.GetChildString("SaveZoomLevels", "column"));
                if (index != -1) {
                    toolStripComboBoxColumnZoom.SelectedIndex = index;
                }
                index = toolStripComboBoxRowZoom.Items.IndexOf(_preferences.GetChildString("SaveZoomLevels", "row"));
                if (index != -1) {
                    toolStripComboBoxRowZoom.SelectedIndex = index;
                }
            }
            if (_sequence.WindowWidth != 0) {
                Width = _sequence.WindowWidth;
            }
            if (_sequence.WindowHeight != 0) {
                Height = _sequence.WindowHeight;
            }
            if (_sequence.ChannelWidth != 0) {
                splitContainer1.SplitterDistance = _sequence.ChannelWidth;
            }
            splitContainer2.SplitterDistance = 60;
            splitContainer2.IsSplitterFixed = true;
            labelPosition.Text = String.Empty;
            SetDrawingLevel(_sequence.MaximumLevel);
            _executionInterface.SetSynchronousContext(_executionContextHandle, _sequence);
            UpdateLevelDisplay();
            IsDirty = false;
            pictureBoxChannels.AllowDrop = true;
            WindowState = FormWindowState.Maximized;
        }


        private void InsertChannelIntoSort(int naturalIndex, int sortedIndex) {
            for (var channel = 0; channel < _channelOrderMapping.Count; channel++) {
                if (_channelOrderMapping[channel] >= naturalIndex) {
                    _channelOrderMapping[channel]++;
                }
            }
            _channelOrderMapping.Insert(sortedIndex, naturalIndex);
        }


        private void IntensityAdjustDialogCheck() {
            if (m_intensityAdjustDialog.Visible) {
                return;
            }

            m_intensityAdjustDialog.Show();
            BringToFront();
        }


        private void InvalidateRect(Rectangle rect) {
            rect.Offset(-hScrollBar1.Value, -vScrollBar1.Value);
            _selectionRectangle.X = Math.Min(rect.Left, rect.Right) * _gridColWidth;
            _selectionRectangle.Y = Math.Min(rect.Top, rect.Bottom) * _gridRowHeight;
            _selectionRectangle.Width = Math.Abs((rect.Width + 1) * _gridColWidth);
            _selectionRectangle.Height = Math.Abs((rect.Height + 1) * _gridRowHeight);
            if (_selectionRectangle.Width == 0) {
                _selectionRectangle.Width = _gridColWidth;
            }
            if (_selectionRectangle.Height == 0) {
                _selectionRectangle.Height = _gridRowHeight;
            }
            pictureBoxGrid.Invalidate(_selectionRectangle);
            pictureBoxGrid.Update();
        }


        private void loadARoutineToolStripMenuItem_Click(object sender, EventArgs e) {
            var routine = GetRoutine();
            if (routine == null) {
                return;
            }

            AddUndoItem(
                new Rectangle(_selectedCells.X, _selectedCells.Y, routine.GetLength(Utils.IndexColsOrWidth),
                              routine.GetLength(Utils.IndexRowsOrHeight)), UndoOriginalBehavior.Overwrite, Resources.LoadRoutine);
            ArrayToCells(routine);
            pictureBoxGrid.Refresh();
        }


        private void loadRoutineToClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
            _systemInterface.Clipboard = GetRoutine();
        }


        private void LoadSequencePlugins() {
            _systemInterface.VerifySequenceHardwarePlugins(_sequence);
            var count = 0;
            toolStripDropDownButtonPlugins.DropDownItems.Clear();
            foreach (XmlNode node in _sequence.PlugInData.GetAllPluginData()) {
                if (node.Attributes == null) {
                    continue;
                }

                var menuText = string.Format("{0} ({1}-{2})", node.Attributes["name"].Value, node.Attributes["from"].Value,
                                             node.Attributes["to"].Value);
                var item = new ToolStripMenuItem(menuText)
                {Checked = bool.Parse(node.Attributes["enabled"].Value), CheckOnClick = true, Tag = count.ToString(CultureInfo.InvariantCulture)};
                item.CheckedChanged += _pluginCheckHandler;
                count++;
                toolStripDropDownButtonPlugins.DropDownItems.Add(item);
            }

            if (count <= 0) {
                return;
            }

            toolStripDropDownButtonPlugins.DropDownItems.Add("-");
            toolStripDropDownButtonPlugins.DropDownItems.Add(Resources.SelectAll, null, selectAllToolStripMenuItem_Click);
            toolStripDropDownButtonPlugins.DropDownItems.Add(Resources.UnselectAll, null, unselectAllToolStripMenuItem_Click);
        }


        private void LoadSequenceSorts() {
            toolStripComboBoxChannelOrder.BeginUpdate();
            var defineNewOrder = (string) toolStripComboBoxChannelOrder.Items[0];
            var restoreNaturalOrder = (string) toolStripComboBoxChannelOrder.Items[toolStripComboBoxChannelOrder.Items.Count - 1];
            toolStripComboBoxChannelOrder.Items.Clear();
            toolStripComboBoxChannelOrder.Items.Add(defineNewOrder);
            foreach (var order in _sequence.Sorts) {
                toolStripComboBoxChannelOrder.Items.Add(order);
            }
            toolStripComboBoxChannelOrder.Items.Add(restoreNaturalOrder);
            if (_sequence.LastSort != -1) {
                toolStripComboBoxChannelOrder.SelectedIndex = _sequence.LastSort + 1;
            }
            toolStripComboBoxChannelOrder.EndUpdate();
        }


        private void m_intensityAdjustDialog_VisibleChanged(object sender, EventArgs e) {
            if (!m_intensityAdjustDialog.Visible) {
                AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, Resources.UndoText_AdjustIntensity);
                var delta = m_intensityAdjustDialog.Delta;
                for (var top = _selectedCells.Top; top < _selectedCells.Bottom; top++) {
                    var channel = _channelOrderMapping[top];
                    if (!_sequence.Channels[channel].Enabled) {
                        continue;
                    }

                    for (var left = _selectedCells.Left; left < _selectedCells.Right; left++) {
                        var newValue = _actualLevels
                                           ? _sequence.EventValues[channel, left] + delta
                                           : Utils.ToValue(Utils.ToPercentage(_sequence.EventValues[channel, left]) + delta);
                        newValue = Math.Max(Math.Min(newValue, _sequence.MaximumLevel), _sequence.MinimumLevel);
                        _sequence.EventValues[channel, left] = (byte) newValue;
                    }
                }
                pictureBoxGrid.Refresh();
            }
            else {
                m_intensityAdjustDialog.LargeDelta = _intensityLargeDelta;
            }
        }


        private void m_positionTimer_Tick(object sender, EventArgs e) {
            int executionPosition;

            if (_executionInterface.EngineStatus(_executionContextHandle, out executionPosition) == Utils.ExecutionStopped) {
                ProgramEnded();
                return;
            }

            var currentPosition = executionPosition / _sequence.EventPeriod;
            if (currentPosition == _position) {
                return;
            }

            toolStripLabelExecutionPoint.Text = Utils.TimeFormatWithoutMills(executionPosition);
            _previousPosition = _position;
            _position = currentPosition;

            if ((_position < hScrollBar1.Value) || (_position > (hScrollBar1.Value + _visibleEventPeriods))) {
                if (_autoScrolling) {
                    if (_position != -1) {
                        if (_position >= _sequence.TotalEventPeriods) {
                            _previousPosition = _position = _sequence.TotalEventPeriods - 1;
                        }
                        hScrollBar1.Value = _position;
                        toolStripLabelExecutionPoint.Text = Utils.TimeFormatWithoutMills(executionPosition);
                    }
                }
                else {
                    UpdateProgress();
                }
            }
            else if (_showPositionMarker) {
                UpdateProgress();
            }
            else {
                toolStripLabelExecutionPoint.Text = Utils.TimeFormatWithoutMills(executionPosition);
            }
        }


        private void maxToolStripMenuItem_Click(object sender, EventArgs e) {
            ArithmeticPaste(ArithmeticOperation.Max);
        }


        private void minToolStripMenuItem_Click(object sender, EventArgs e) {
            ArithmeticPaste(ArithmeticOperation.Min);
        }


        public override EventSequence New() {
            return New(new EventSequence(_systemInterface.UserPreferences));
        }


        public override EventSequence New(EventSequence seedSequence) {
            _sequence = seedSequence;
            _preferences = _systemInterface.UserPreferences;
            Init();
            Text = _sequence.Name ?? Resources.UnnamedSequence;
            return _sequence;
        }


        private static Rectangle NormalizeRect(Rectangle rect) {
            return new Rectangle {
                X = Math.Min(rect.Left, rect.Right), Y = Math.Min(rect.Top, rect.Bottom), Width = Math.Abs(rect.Width) + (rect.Width < 0 ? 1 : 0),
                Height = Math.Abs(rect.Height) + (rect.Height < 0 ? 1 : 0)
            };
        }


        private void normalSpeedToolStripMenuItem_Click(object sender, EventArgs e) {
            SetAudioSpeed(1f);
        }


        private void normalToolStripMenuItem_Click(object sender, EventArgs e) {
            normalToolStripMenuItem.Checked = true;
            paintFromClipboardToolStripMenuItem.Checked = false;
        }


        public override void Notify(Notification notification, object data) {
            switch (notification) {
                case Notification.PreferenceChange:
                    UpdateRowHeight();
                    UpdateColumnWidth();
                    _showPositionMarker = _preferences.GetBoolean("ShowPositionMarker");
                    _autoScrolling = _preferences.GetBoolean("AutoScrolling");
                    _mouseWheelVerticalDelta = _preferences.GetInteger("MouseWheelVerticalDelta");
                    _mouseWheelHorizontalDelta = _preferences.GetInteger("MouseWheelHorizontalDelta");
                    _intensityLargeDelta = _preferences.GetInteger("IntensityLargeDelta");
                    RefreshAll();
                    break;

                case Notification.KeyDown:
                    StandardSequence_KeyDown(null, (KeyEventArgs) data);
                    break;

                case Notification.SequenceChange:
                    RefreshAll();
                    IsDirty = true;
                    break;

                case Notification.ProfileChange: {
                    var currentOrder = _sequence.Sorts.CurrentOrder;
                    _sequence.ReloadProfile();
                    _sequence.Sorts.CurrentOrder = currentOrder;
                    LoadSequenceSorts();
                    RefreshAll();
                    break;
                }
            }
        }


        public override void OnDirtyChanged(EventArgs e) {
            base.OnDirtyChanged(e);
            tbsSave.Enabled = IsDirty;
        }


        protected override void OnMouseWheel(MouseEventArgs e) {
            var isVertical = ModifierKeys != Keys.Shift;
            if (_preferences.GetBoolean("FlipScrollBehavior")) {
                isVertical = !isVertical;
            }

            if (isVertical) {
                DoMouseWheel(e, vScrollBar1, _mouseWheelVerticalDelta, _visibleRowCount);
            }
            else {
                DoMouseWheel(e, hScrollBar1, _mouseWheelHorizontalDelta, _visibleEventPeriods);
            }
        }


        private static void DoMouseWheel(MouseEventArgs e, ScrollBar sb, int delta, int max) {
            if (e.Delta > 0) {
                sb.Value = sb.Value >= sb.Minimum + delta ? sb.Value - delta : sb.Minimum;
            }
            else {
                sb.Value = sb.Value <= sb.Maximum - (max + delta) ? sb.Value + delta : Math.Max(sb.Maximum - max + 1, 0);
            }
        }


        public override EventSequence Open(string filePath) {
            _sequence = new EventSequence(filePath);
            Text = _sequence.Name;
            _preferences = _systemInterface.UserPreferences;
            Init();
            return _sequence;
        }


        private void otherToolStripMenuItem_Click(object sender, EventArgs e) {
            SetVariablePlaybackSpeed(new Point(0, 0));
        }


        private void paintFromClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
            normalToolStripMenuItem.Checked = false;
            paintFromClipboardToolStripMenuItem.Checked = true;
        }


        private void ParseAudioWaveform() {
            string str;
            if ((_sequence.Audio != null) && File.Exists(str = Path.Combine(Paths.AudioPath, _sequence.Audio.FileName))) {
                if (toolStripButtonWaveform.Checked) {
                    using (var dialog = new ProgressDialog()) {
                        dialog.Show();
                        dialog.Message = Resources.ComputingWaveform;
                        Cursor = Cursors.WaitCursor;
                        try {
                            _waveformPcmData = new uint[_sequence.TotalEventPeriods * _gridColWidth];
                            _waveformPixelData = new uint[_sequence.TotalEventPeriods * _gridColWidth];

                            Sound sound = null;
                            fmod.GetInstance(-1).SystemObject.createSound(str, MODE._2D | MODE.HARDWARE | MODE.CREATESAMPLE, ref sound);

                            var raw = SOUND_TYPE.RAW;
                            var none = SOUND_FORMAT.NONE;
                            var audioChannels = 0;
                            var bitsPerSample = 0;
                            sound.getFormat(ref raw, ref none, ref audioChannels, ref bitsPerSample);

                            var audioFrequency = 0f;
                            var volume = 0f;
                            var pan = 0f;
                            var priority = 0;
                            sound.getDefaults(ref audioFrequency, ref volume, ref pan, ref priority);

                            uint lengthInBytes = 0;
                            sound.getLength(ref lengthInBytes, TIMEUNIT.PCMBYTES);

                            uint lengthInMs = 0;
                            sound.getLength(ref lengthInMs, TIMEUNIT.MS);

                            var totalBytes = (lengthInBytes / (double) audioChannels) / (bitsPerSample / 8.0);
                            var bytesPerEvent = totalBytes / _sequence.TotalEventPeriods;
                            var pixelsPerByte = bytesPerEvent / _gridColWidth;
                            var pixelsPerEvent = _sequence.EventPeriod / ((double) _gridColWidth);
                            double hertz = audioFrequency / 1000f;
                            var posAmplitude = 0;
                            var negAmplitude = 0;

                            var index = 0;
                            for (var i = 0.0; (index < _waveformPcmData.Length) && (i < lengthInMs); i += pixelsPerEvent) {
                                var startSample = (int) (i * hertz);
                                var sampleMinMax = GetSampleMinMax(startSample, (int) Math.Min(pixelsPerByte, totalBytes - startSample), sound,
                                                                   bitsPerSample, audioChannels);
                                posAmplitude = Math.Max(posAmplitude, (short) (sampleMinMax >> 16));
                                negAmplitude = Math.Min(negAmplitude, (short) (sampleMinMax & 0xffff));
                                _waveformPcmData[index] = sampleMinMax;
                                index++;
                            }
                            _waveformMaxAmplitude = Math.Max(posAmplitude, -negAmplitude);
                            PcmToPixels(_waveformPcmData, _waveformPixelData);
                            sound.release();
                        }
                        finally {
                            Cursor = Cursors.Default;
                            dialog.Hide();
                        }
                    }
                }
                else {
                    _waveformPcmData = null;
                    _waveformPixelData = null;
                }
                EnableWaveformButton();
            }
            else {
                DisableWaveformDisplay();
            }
        }


        private void pasteFullChannelEventsFromClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_systemInterface.Clipboard == null) {
                return;
            }

            var width = Math.Min(_systemInterface.Clipboard.GetLength(Utils.IndexColsOrWidth), _sequence.TotalEventPeriods);
            AddUndoItem(new Rectangle(0, _selectedLineIndex, width, 1), UndoOriginalBehavior.Overwrite, Resources.PasteText);
            for (var col = 0; col < width; col++) {
                _sequence.EventValues[_editingChannelSortedIndex, col] = _systemInterface.Clipboard[0, col];
            }
            IsDirty = true;
            pictureBoxGrid.Refresh();
        }


        private void PasteOver() {
            if (_systemInterface.Clipboard == null) {
                return;
            }

            ArrayToCells(_systemInterface.Clipboard);
            pictureBoxGrid.Refresh();
        }


        private void PcmToPixels(IList<uint> pcmDataValues, IList<uint> pixelData) {
            var waveformOffset = _waveformCenterLine;
            var negativeOffset = -_waveformCenterLine;
            var amplitudeDivisor = _waveformMaxAmplitude / (double) Math.Max(waveformOffset, negativeOffset);
            for (var i = 0; i < pcmDataValues.Count; i++) {
                var pcmData = pcmDataValues[i];

                var lsb = (short) (Math.Min((short) (pcmData >> 16), _waveformMaxAmplitude) / amplitudeDivisor);
                lsb = Math.Max(lsb, (short) 0);
                lsb = (short) Math.Min(lsb, waveformOffset);

                var msb = (short) (Math.Max((short) (pcmData & 0xffff), -_waveformMaxAmplitude) / amplitudeDivisor);
                msb = Math.Min(msb, (short) 0);
                msb = (short) Math.Max(msb, negativeOffset);

                pixelData[i] = (uint) ((lsb << 16) | ((ushort) msb));
            }
        }


        private void pictureBoxChannels_DragDrop(object sender, DragEventArgs e) {

            if (!e.Data.GetDataPresent(typeof (VixenPlus.Channel))) {
                return;
            }

            var data = (VixenPlus.Channel) e.Data.GetData(typeof (VixenPlus.Channel));
            var channelAt = GetChannelAt(pictureBoxChannels.PointToClient(new Point(e.X, e.Y)));

            if (data == channelAt) {
                return;
            }

            switch (e.Effect) {
                case DragDropEffects.Copy:
                    _sequence.CopyChannel(data, channelAt);
                    RefreshAll();
                    IsDirty = true;
                    break;
                case DragDropEffects.Move:
                    var channelNaturalIndex = GetChannelNaturalIndex(data);
                    _channelOrderMapping.Remove(channelNaturalIndex);
                    var channelSortedIndex = channelAt != null ? GetChannelSortedIndex(channelAt) : _channelOrderMapping.Count;
                    _channelOrderMapping.Insert(channelSortedIndex, channelNaturalIndex);
                    RefreshAll();
                    IsDirty = true;
                    break;
            }
        }


        private void pictureBoxChannels_DragOver(object sender, DragEventArgs e) {
            const int delay = 20;
            if (!e.Data.GetDataPresent(typeof (VixenPlus.Channel))) {
                return;
            }

            var point = pictureBoxChannels.PointToClient(new Point(e.X, e.Y));
            var lineIndexAt = GetLineIndexAt(point);
            if (vScrollBar1.Enabled) {
                if ((vScrollBar1.Value > vScrollBar1.Minimum) && ((lineIndexAt - vScrollBar1.Value) < 2)) {
                    vScrollBar1.Value--;
                    Thread.Sleep(delay);
                    lineIndexAt = GetLineIndexAt(point);
                }
                else if ((vScrollBar1.Value < vScrollBar1.Maximum) && ((lineIndexAt - vScrollBar1.Value) > (_visibleRowCount - 2))) {
                    vScrollBar1.Value++;
                    Thread.Sleep(delay);
                    lineIndexAt = GetLineIndexAt(point);
                }
            }
            if ((ModifierKeys & Keys.Control) != Keys.None) {
                if ((lineIndexAt >= 0) && (lineIndexAt < _sequence.ChannelCount)) {
                    e.Effect = DragDropEffects.Copy;
                }
                else {
                    e.Effect = DragDropEffects.None;
                }
            }
            else {
                e.Effect = DragDropEffects.Move;
            }
        }


        private void pictureBoxChannels_MouseDoubleClick(object sender, MouseEventArgs e) {
            if ((!ChannelClickValid()) || e.X < 10) {
                return;
            }

            if (_showingOutputs && (e.X < 50)) {
                ReorderChannelOutputs();
            }
            else {
                ShowChannelProperties();
            }
        }


        private void pictureBoxChannels_MouseDown(object sender, MouseEventArgs e) {
            ChannelClickValid();
            _mouseDownAtInChannels = e.Location;
        }


        private void pictureBoxChannels_MouseMove(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) != MouseButtons.None && _mouseDownAtInChannels != Point.Empty &&
                (Math.Abs(e.X - _mouseDownAtInChannels.X) > 3 || Math.Abs(e.Y - _mouseDownAtInChannels.Y) > 3)) {
                DoDragDrop(SelectedChannel, DragDropEffects.Move | DragDropEffects.Copy);
            }
        }


        private void pictureBoxChannels_MouseUp(object sender, MouseEventArgs e) {
            _mouseDownAtInChannels = Point.Empty;
        }


        //todo when drawing channels, limit to the visible channels only
        private void pictureBoxChannels_Paint(object sender, PaintEventArgs e) {
            e.Graphics.FillRectangle(_channelBackBrush, e.Graphics.VisibleClipBounds);

            VixenPlus.Channel channel;
            int mappedChannel;
            int channelOffset;

            var height = pictureBoxTime.Height + splitContainer2.SplitterWidth;
            var selectedZoom = toolStripComboBoxRowZoom.SelectedIndex;
            var heightAddition = (selectedZoom <= 4) ? 0 : (selectedZoom >= 8) ? 3 : 1;
            var showNaturalChannelNumbers = _preferences.GetBoolean("ShowNaturalChannelNumber");
            var x = showNaturalChannelNumbers ? (_sequence.ChannelCount.ToString(CultureInfo.InvariantCulture).Length + 1) * 6 + 10 : 10;

            #region Not Showing Outputs

            if (!_showingOutputs) {
                for (channelOffset = vScrollBar1.Value; (channelOffset >= 0) && (channelOffset < _sequence.ChannelCount); channelOffset++) {

                    mappedChannel = _channelOrderMapping[channelOffset];
                    channel = _sequence.Channels[mappedChannel];
                    var isChannelSelected = (channel == SelectedChannel);

                    e.Graphics.FillRectangle(isChannelSelected ? SystemBrushes.Highlight : channel.Brush ?? _channelBackBrush, 0, height,
                                             pictureBoxChannels.Width, _gridRowHeight);

                    var textBrush = isChannelSelected
                                        ? SystemBrushes.HighlightText
                                        : Utils.GetTextColor(channel.Brush != null ? channel.Brush.Color : _channelBackBrush.Color);

                    if (showNaturalChannelNumbers) {
                        e.Graphics.DrawString(string.Format("{0}:", mappedChannel + 1), _channelNameFont, textBrush, 10f, height + heightAddition);
                    }

                    e.Graphics.DrawString(channel.Name, channel.Enabled ? _channelNameFont : _channelStrikeoutFont, textBrush, x,
                                          height + heightAddition);

                    height += _gridRowHeight;
                }
            }
                #endregion
                #region Showing Outputs

            else {
                using (var brush = new SolidBrush(Color.FromArgb(192, Color.Gray))) {
                    for (channelOffset = vScrollBar1.Value; (channelOffset >= 0) && (channelOffset < _sequence.ChannelCount); channelOffset++) {

                        mappedChannel = _channelOrderMapping[channelOffset];
                        channel = _sequence.Channels[mappedChannel];
                        var isChannelSelected = (channel == SelectedChannel);

                        e.Graphics.FillRectangle(
                            isChannelSelected && showNaturalChannelNumbers ? SystemBrushes.Highlight : channel.Brush ?? _channelBackBrush, 0, height,
                            pictureBoxChannels.Width, _gridRowHeight);

                        var textBrush = isChannelSelected
                                            ? SystemBrushes.HighlightText
                                            : Utils.GetTextColor(channel.Brush != null ? channel.Brush.Color : _channelBackBrush.Color);

                        if (showNaturalChannelNumbers) {
                            e.Graphics.DrawString(string.Format("{0}:", mappedChannel + 1), _channelNameFont, textBrush, 10f, height + heightAddition);
                        }
                        else {
                            x = 10;
                        }

                        e.Graphics.FillRectangle(brush, x, height + 1, 40, _gridRowHeight - 2);

                        if (toolStripComboBoxRowZoom.SelectedIndex > 4) {
                            e.Graphics.DrawRectangle(Pens.Black, x, height + 1, 40, _gridRowHeight - 2);
                        }

                        e.Graphics.DrawString(String.Format("{0}", channel.OutputChannel + 1),
                                              channel.Enabled ? _channelNameFont : _channelStrikeoutFont, textBrush, x + 16, height + heightAddition);

                        e.Graphics.DrawString(channel.Name, channel.Enabled ? _channelNameFont : _channelStrikeoutFont, textBrush, x + 44,
                                              height + heightAddition);

                        height += _gridRowHeight;
                    }

                    #endregion
                }
            }
            e.Graphics.FillRectangle(Brushes.White, 0, 0, 5, pictureBoxChannels.Height);
            if (_mouseChannelCaret != -1) {
                e.Graphics.FillRectangle(_channelCaretBrush, 0,
                                         ((_mouseChannelCaret - vScrollBar1.Value) * _gridRowHeight) + pictureBoxTime.Height +
                                         splitContainer2.SplitterWidth, 5, _gridRowHeight);
            }
        }


        private void pictureBoxChannels_Resize(object sender, EventArgs e) {
            pictureBoxChannels.Refresh();
        }


        private void pictureBoxGrid_DoubleClick(object sender, EventArgs e) {
            if (_currentlyEditingChannel == null) {
                return;
            }

            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, Resources.UndoText_DoubleClick);
            _sequence.EventValues[_editingChannelSortedIndex, _selectedCells.X] =
                (_sequence.EventValues[_editingChannelSortedIndex, _selectedCells.X] > _sequence.MinimumLevel)
                    ? _sequence.MinimumLevel : _drawingLevel;
            UpdateGrid(_gridGraphics,
                       new Rectangle((_selectedCells.X - hScrollBar1.Value) * _gridColWidth,
                                     (_editingChannelSortedIndex - vScrollBar1.Value) * _gridRowHeight, _gridColWidth, _gridRowHeight));
        }


        private void pictureBoxGrid_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) {
                return;
            }

            _mouseDownInGrid = true;
            lblFollowMouse.Visible = true & (_executionInterface.EngineStatus(_executionContextHandle) != Utils.ExecutionRunning);
            _mouseDownAtInGrid.X = (e.X / _gridColWidth) + hScrollBar1.Value;
            _mouseDownAtInGrid.Y = (e.Y / _gridRowHeight) + vScrollBar1.Value;

            if (_selectedCells.Width != 0) {
                EraseSelectedRange();
            }

            if ((ModifierKeys & Keys.Control) != Keys.None) {
                _selectedLineIndex = (e.Y / _gridRowHeight) + vScrollBar1.Value;
                _editingChannelSortedIndex = _channelOrderMapping[_selectedLineIndex];
                _currentlyEditingChannel = _sequence.Channels[_editingChannelSortedIndex];
                _lineRect.X = _mouseDownAtInGrid.X;
                _lineRect.Y = _mouseDownAtInGrid.Y;
                _lineRect.Width = 0;
                _lineRect.Height = 0;
                InvalidateRect(_lineRect);
            }
            else if ((ModifierKeys & Keys.Shift) != Keys.None) {
                var rect = new Rectangle {
                    X = _selectedCells.X, Y = _selectedCells.Y,
                    Width = (hScrollBar1.Value + (int) Math.Floor(e.X / (float) _gridColWidth) - _selectedCells.Left) + 1,
                    Height = (vScrollBar1.Value + (e.Y / _gridRowHeight) - _selectedCells.Top) + 1
                };

                if (rect.Width < 0) {
                    rect.Width--;
                }
                if (rect.Height < 0) {
                    rect.Height--;
                }
                if (rect.Bottom > _sequence.ChannelCount) {
                    rect.Height = _sequence.ChannelCount - rect.Y;
                }
                if (rect.Right > _sequence.TotalEventPeriods) {
                    rect.Width = _sequence.TotalEventPeriods - rect.X;
                }
                _selectedCells = NormalizeRect(rect);
                DrawSelectedRange();
            }
            else if ((e.Y / _gridRowHeight) + vScrollBar1.Value < _sequence.ChannelCount &&
                     (e.X / _gridColWidth) + hScrollBar1.Value < _sequence.TotalEventPeriods) {
                _selectedLineIndex = (e.Y / _gridRowHeight) + vScrollBar1.Value;
                _editingChannelSortedIndex = _channelOrderMapping[_selectedLineIndex];
                _currentlyEditingChannel = _sequence.Channels[_editingChannelSortedIndex];
                _selectedRange.X = hScrollBar1.Value + ((int) Math.Floor(e.X / ((float) _gridColWidth)));
                _selectedRange.Y = _selectedLineIndex;
                _selectedRange.Width = 1;
                _selectedRange.Height = 1;
                _selectedCells = _selectedRange;
                DrawSelectedRange();
            }
            else {
                _currentlyEditingChannel = null;
                _editingChannelSortedIndex = -1;
                _selectedLineIndex = -1;
            }
            UpdatePositionLabel(_selectedCells, false);
        }


        private void pictureBoxGrid_MouseLeave(object sender, EventArgs e) {
            toolStripLabelCellIntensity.Text = string.Empty;
            toolStripLabelCurrentCell.Text = string.Empty;
        }

        //TODO there are still issues with the label drawing and updating stuff that is not necessary.
        private void pictureBoxGrid_MouseMove(object sender, MouseEventArgs e) {
            var cellX = Math.Min((Math.Max(e.X / _gridColWidth, 0) + hScrollBar1.Value), _sequence.TotalEventPeriods - 1);
            var cellY = Math.Min((Math.Max(e.Y / _gridRowHeight, 0) + vScrollBar1.Value), _sequence.ChannelCount - 1);
            tsbPlayPoint.Enabled = _selectedCells.Width > 0;
            tsbPlayRange.Enabled = _selectedCells.Width > 1;

            if (cellX == _lastCellX && cellY == _lastCellY && pictureBoxGrid.ClientRectangle.Contains(e.Location)) {
                return;
            }

            toolStripLabelCellIntensity.Text = string.Empty;
            toolStripLabelCurrentCell.Text = string.Empty;
            if ((e.Button == MouseButtons.Left) && _mouseDownInGrid && _executionInterface.EngineStatus(_executionContextHandle) != Utils.ExecutionRunning) {
                if (_lineRect.Left == -1) {
                    DrawSelectionBox(e.X, e.Y, cellX, cellY);
                }
                else {
                    DrawChaseLine(cellX, cellY);
                }
            }

            DrawCarets(cellX, cellY);
            UpdateToolStrip(cellX, cellY);
            _lastCellX = cellX;
            _lastCellY = cellY;
        }


        private void DrawSelectionBox(int mouseX, int mouseY, int cellX, int cellY) {
            var directionFlag = (mouseX > pictureBoxGrid.Width ? 0x2 : (mouseX < 0 ? 0x8 : 0x0)) |
                                (mouseY > pictureBoxGrid.Height ? 0x1 : (mouseY < 0 ? 0x4 : 0x0));

            if (directionFlag == 0x0) {
                EraseSelectedRange();

                _selectedRange.Width = (cellX >= _mouseDownAtInGrid.X)
                                           ? (cellX > _mouseDownAtInGrid.X) ? cellX - _selectedRange.Left + 1 : 1 : cellX - _selectedRange.Left;

                _selectedRange.Height = (cellY < _mouseDownAtInGrid.Y)
                                            ? cellY - _selectedRange.Top : (cellY > _mouseDownAtInGrid.Y) ? cellY - _selectedRange.Top + 1 : 1;

                _selectedCells = NormalizeRect(_selectedRange);
                DrawSelectedRange();
            }

            if ((directionFlag & 0x1) == 0x1) {
                ScrollSelectionDown(cellX);
            }
            if ((directionFlag & 0x2) == 0x2) {
                ScrollSelectionRight(cellY);
            }
            if ((directionFlag & 0x4) == 0x4) {
                ScrollSelectionUp(cellX);
            }
            if ((directionFlag & 0x8) == 0x8) {
                ScrollSelectionLeft(cellY);
            }

            UpdatePositionLabel(_selectedCells, false);
        }


        private void DrawChaseLine(int cellX, int cellY) {
            EraseRectangleEntity(_lineRect);

            if ((ModifierKeys & Keys.Shift) != Keys.None) { // Constrained to 45 degree angles
                var x = cellX - _mouseDownAtInGrid.X;
                var xMidpoint = x / 2;
                var y = cellY - _mouseDownAtInGrid.Y;
                var yMidpoint = y / 2;
                var offset = Math.Abs(Math.Abs(x) < Math.Abs(y) ? x : y);
                string lineAngle;

                if (x >= 0) {
                    if (y >= 0) {
                        lineAngle = (y < xMidpoint) ? "N-S" : (x < yMidpoint) ? "E-W" : "NE"; // x >= 0 & y >= 0
                    }
                    else {
                        lineAngle = (y >= -xMidpoint) ? "N-S" : (x < -yMidpoint) ? "E-W" : "SE"; // x >= 0 & y < 0
                    }
                }
                else if (y >= 0) {
                    lineAngle = (y < -xMidpoint) ? "N-S" : (x >= -yMidpoint) ? "E-W" : "SW"; // x < 0 & y >= 0
                }
                else {
                    lineAngle = (y >= xMidpoint) ? "N-S" : (x >= yMidpoint) ? "E-W" : "NW"; // x < 0 & y < 0
                }

                switch (lineAngle) {
                    case "N-S":
                        _lineRect.Width = x;
                        _lineRect.Height = 0;
                        break;

                    case "E-W":
                        _lineRect.Width = 0;
                        _lineRect.Height = y;
                        break;

                    case "NE":
                        _lineRect.Width = offset;
                        _lineRect.Height = offset;
                        break;

                    case "SE":
                        _lineRect.Width = offset;
                        _lineRect.Height = -offset;
                        break;

                    case "SW":
                        _lineRect.Width = -offset;
                        _lineRect.Height = offset;
                        break;

                    case "NW":
                        _lineRect.Width = -offset;
                        _lineRect.Height = -offset;
                        break;
                }
            }
            else { // Freeform
                _lineRect.Width = (cellX != _mouseDownAtInGrid.X) ? cellX - _lineRect.Left : 0;
                _lineRect.Height = (cellY != _mouseDownAtInGrid.Y) ? cellY - _lineRect.Top : 0;
            }

            InvalidateRect(_lineRect);
            pictureBoxGrid.Refresh();
            UpdatePositionLabel(NormalizeRect(new Rectangle(_lineRect.X, _lineRect.Y, _lineRect.Width + 1, _lineRect.Height)), true);

        }


        private void DrawCarets(int cellX, int cellY) {
            var xMin = 0;
            var xMax = 0;
            var yMin = 0;
            var yMax = 0;
            if ((cellX != _mouseTimeCaret) || (cellY != _mouseChannelCaret)) {
                xMin = (Math.Min(cellX, _mouseTimeCaret) - hScrollBar1.Value) * _gridColWidth;
                xMax = ((Math.Max(cellX, _mouseTimeCaret) - hScrollBar1.Value) + 1) * _gridColWidth;
                yMin = (Math.Min(cellY, _mouseChannelCaret) - vScrollBar1.Value) * _gridRowHeight;
                yMax = ((Math.Max(cellY, _mouseChannelCaret) - vScrollBar1.Value) + 1) * _gridRowHeight;
            }

            if (cellY != _mouseChannelCaret) {
                var rectangle = new Rectangle(0,
                                              pictureBoxTime.Height + splitContainer2.SplitterWidth +
                                              (_gridRowHeight * (_mouseChannelCaret - vScrollBar1.Value)), CaretSize, _gridRowHeight);
                _mouseChannelCaret = -1;
                pictureBoxChannels.Invalidate(rectangle);
                pictureBoxChannels.Update();
                if (cellY < _sequence.ChannelCount) {
                    _mouseChannelCaret = cellY;
                    rectangle.Y = pictureBoxTime.Height + splitContainer2.SplitterWidth + (_gridRowHeight * (_mouseChannelCaret - vScrollBar1.Value));
                    pictureBoxChannels.Invalidate(rectangle);
                    pictureBoxChannels.Update();
                }
            }

            if (cellX != _mouseTimeCaret) {
                var rectangle = new Rectangle(_gridColWidth * (_mouseTimeCaret - hScrollBar1.Value), 0, _gridColWidth, CaretSize);
                _mouseTimeCaret = -1;
                pictureBoxTime.Invalidate(rectangle);
                pictureBoxTime.Update();
                if (cellX < _sequence.TotalEventPeriods) {
                    _mouseTimeCaret = cellX;
                    rectangle.X = _gridColWidth * (_mouseTimeCaret - hScrollBar1.Value);
                    pictureBoxTime.Invalidate(rectangle);
                    pictureBoxTime.Update();
                }
            }

            if (xMin == xMax || !toolStripButtonToggleCrossHairs.Checked) {
                return;
            }

            pictureBoxGrid.Invalidate(new Rectangle(xMin, 0, xMax - xMin, pictureBoxGrid.Height));
            pictureBoxGrid.Invalidate(new Rectangle(0, yMin, pictureBoxGrid.Width, yMax - yMin));
            pictureBoxGrid.Update();
        }


        private void UpdateToolStrip(int cellX, int cellY) {
            if ((cellX < 0) || (cellY < 0)) {
                return;
            }

            string intensity;
            GetCellIntensity(cellX, cellY, out intensity);
            toolStripLabelCellIntensity.Text = intensity;
            toolStripLabelCurrentCell.Text = string.Format("{0} , {1}", Utils.TimeFormatWithMills(cellX * _sequence.EventPeriod),
                                                           _sequence.Channels[_channelOrderMapping[cellY]].Name);
        }


        private void pictureBoxGrid_MouseUp(object sender, MouseEventArgs e) {
            _mouseDownInGrid = false;
            lblFollowMouse.Visible = false;
            if (_lineRect.Left == -1) {
                return;
            }

            if (paintFromClipboardToolStripMenuItem.Checked && (_systemInterface.Clipboard != null)) {
                var rect = NormalizeRect(_lineRect);
                var clipLen = _systemInterface.Clipboard.GetLength(Utils.IndexColsOrWidth);
                rect.Width += clipLen;
                EraseRectangleEntity(rect);
                rect.Width++;
                rect.Height++;
                AddUndoItem(rect, UndoOriginalBehavior.Overwrite, Resources.UndoText_ChaseLines);
                var brush = new byte[clipLen];
                for (var i = 0; i < clipLen; i++) {
                    brush[i] = _systemInterface.Clipboard[Utils.IndexRowsOrHeight, i];
                }
                BresenhamLine(_lineRect, brush);
            }
            else {
                EraseRectangleEntity(_lineRect);
                var rect = NormalizeRect(_lineRect);
                rect.Width++;
                rect.Height++;
                AddUndoItem(rect, UndoOriginalBehavior.Overwrite, Resources.UndoText_ChaseLines);
                BresenhamLine(_lineRect);
            }

            _lineRect.X = -1;
            UpdatePositionLabel(_selectedCells, false);
        }


        private void pictureBoxGrid_Paint(object sender, PaintEventArgs e) {
            e.Graphics.FillRectangle(_gridBackBrush, e.ClipRectangle);
            if (_sequence.ChannelCount == 0) {
                return;
            }

            var startPoint = new Point();
            var endPoint = new Point();

            var cellCount = e.ClipRectangle.Left / _gridColWidth;

            startPoint.Y = e.ClipRectangle.Top;
            endPoint.Y = Math.Min(e.ClipRectangle.Bottom, (_sequence.ChannelCount - vScrollBar1.Value) * _gridRowHeight);

            var pixelLimit = Math.Min(e.ClipRectangle.Right, (_sequence.TotalEventPeriods - hScrollBar1.Value) * _gridColWidth);
            var startPixelX = 0;
            while (startPixelX < pixelLimit) {
                startPixelX = _gridColWidth * cellCount;
                startPoint.X = startPixelX;
                endPoint.X = startPixelX;
                e.Graphics.DrawLine(Pens.Gray, startPoint, endPoint);
                cellCount++;
            }
            cellCount = e.ClipRectangle.Top / _gridRowHeight;
            startPoint.X = e.ClipRectangle.Left;
            endPoint.X = Math.Min(e.ClipRectangle.Right, (_sequence.TotalEventPeriods - hScrollBar1.Value) * _gridColWidth);
            pixelLimit = Math.Min(e.ClipRectangle.Bottom, (_sequence.ChannelCount - vScrollBar1.Value) * _gridRowHeight);
            var startPixelY = 0;
            while (startPixelY < pixelLimit) {
                startPixelY = _gridRowHeight * cellCount;
                startPoint.Y = startPixelY;
                endPoint.Y = startPixelY;
                e.Graphics.DrawLine(Pens.Gray, startPoint, endPoint);
                cellCount++;
            }
            UpdateGrid(e.Graphics, e.ClipRectangle);
            if (m_positionTimer.Enabled) {
                startPixelY = Math.Min(e.ClipRectangle.Bottom, (_sequence.ChannelCount - vScrollBar1.Value) * _gridRowHeight);
                if ((_previousPosition != -1) && (_previousPosition >= hScrollBar1.Value)) {
                    startPixelX = (_previousPosition - hScrollBar1.Value) * _gridColWidth;
                    e.Graphics.DrawLine(Pens.Gray, startPixelX, 0, startPixelX, startPixelY);
                }
                startPixelX = (_position - hScrollBar1.Value) * _gridColWidth;
                e.Graphics.DrawLine(Pens.Yellow, startPixelX, 0, startPixelX, startPixelY);
            }
            else {
                if (_lineRect.Left != -1) {
                    var left = ((_lineRect.Left - hScrollBar1.Value) * _gridColWidth) + (_gridColWidth >> 1);
                    var top = ((_lineRect.Top - vScrollBar1.Value) * _gridRowHeight) + (_gridRowHeight >> 1);
                    var width = left + (_lineRect.Width * _gridColWidth);
                    var height = top + (_lineRect.Height * _gridRowHeight);
                    e.Graphics.DrawLine(Pens.Blue, left, top, width, height);
                }
                else {
                    if (_selectedCells.Left > _sequence.TotalEventPeriods) {
                        _selectedCells.Width = 0;
                    }
                    var range = Rectangle.Intersect(_selectedCells,
                                                    Rectangle.FromLTRB(hScrollBar1.Value, vScrollBar1.Value,
                                                                       (hScrollBar1.Value + _visibleEventPeriods) + 1,
                                                                       (vScrollBar1.Value + _visibleRowCount) + 1));
                    if (!range.IsEmpty) {
                        e.Graphics.FillRectangle(_selectionBrush, RangeToRectangle(range));
                    }
                }
                if (toolStripButtonToggleCrossHairs.Checked) {
                    var x = ((_mouseTimeCaret - hScrollBar1.Value) * _gridColWidth) + ((int) (_gridColWidth * 0.5f));
                    var y = ((_mouseChannelCaret - vScrollBar1.Value) * _gridRowHeight) + ((int) (_gridRowHeight * 0.5f));
                    if (((x > e.ClipRectangle.Left) && (x < e.ClipRectangle.Right)) || ((y > e.ClipRectangle.Top) && (y < e.ClipRectangle.Bottom))) {
                        e.Graphics.DrawLine(Pens.Yellow, x, 0, x, Height);
                        e.Graphics.DrawLine(Pens.Yellow, 0, y, Width, y);
                    }
                }
            }
        }


        private void pictureBoxGrid_Resize(object sender, EventArgs e) {
            if (_initializing) {
                return;
            }

            VScrollCheck();
            HScrollCheck();
            pictureBoxGrid.Refresh();
        }


        //TODO This has got a bug when you don't use an event period that divides in 1000 evenly.
        private void pictureBoxTime_Paint(object sender, PaintEventArgs e) {
            e.Graphics.FillRectangle(_timeBackBrush, e.ClipRectangle);

            var topLeftPt = new Point(0, pictureBoxTime.Height - 20);
            var bottomRightPt = new Point(0, pictureBoxTime.Height - 5);
            var drawingRect = new Rectangle(0, topLeftPt.Y, pictureBoxTime.Width, bottomRightPt.Y - topLeftPt.Y);

            if (e.ClipRectangle.IntersectsWith(drawingRect)) {
                var x = e.ClipRectangle.X / _gridColWidth;
                var rightmosCell = x;
                for (x *= _gridColWidth; (x < e.ClipRectangle.Right) && ((rightmosCell + hScrollBar1.Value) <= _sequence.TotalEventPeriods);
                     x += _gridColWidth) {
                    if (rightmosCell != 0) {
                        topLeftPt.X = x;
                        bottomRightPt.X = x;
                        e.Graphics.DrawLine(Pens.White, topLeftPt, bottomRightPt);
                    }
                    rightmosCell++;
                }
            }

            topLeftPt.Y = pictureBoxTime.Height - 30;
            drawingRect.X = 0;
            drawingRect.Width = pictureBoxTime.Width;
            drawingRect.Y = topLeftPt.Y;
            drawingRect.Height = bottomRightPt.Y - topLeftPt.Y;

            if (e.ClipRectangle.IntersectsWith(drawingRect)) {
                var x = e.ClipRectangle.X;
                var eventPeriod = _sequence.EventPeriod;
                var startMills = (hScrollBar1.Value + (e.ClipRectangle.Left / (float) _gridColWidth)) * eventPeriod;
                var endMills = Math.Min((hScrollBar1.Value + (e.ClipRectangle.Right / _gridColWidth)) * eventPeriod, _sequence.Time);
                var eventCount = (float) _sequence.EventsPerSecond;

                var currentMills = (!0f.Equals(startMills % 1000f)) ? (int) startMills / 1000 * 1000 : (int) startMills;

                while ((x < e.ClipRectangle.Right) && (currentMills <= endMills)) {
                    if (currentMills != 0) {
                        x = e.ClipRectangle.Left + ((int) (((currentMills - startMills) / Utils.MillsPerSecond) * (_gridColWidth * eventCount)));
                        topLeftPt.X = x;
                        bottomRightPt.X = x;
                        e.Graphics.DrawLine(Pens.White, topLeftPt, bottomRightPt);
                        topLeftPt.X++;
                        bottomRightPt.X++;
                        e.Graphics.DrawLine(Pens.White, topLeftPt, bottomRightPt);
                        var time = currentMills >= Utils.MillsPerMinute
                                       ? Utils.TimeFormatWithoutMills(currentMills, true) : Utils.TimeFormatMillsOnly(currentMills);
                        var ef = e.Graphics.MeasureString(time, _timeFont);
                        e.Graphics.DrawString(time, _timeFont, Brushes.White, x - (ef.Width / 2f), topLeftPt.Y - ef.Height - 5f);
                    }
                    currentMills += Utils.MillsPerSecond;
                }
            }

            topLeftPt.Y = pictureBoxTime.Height - 35;
            bottomRightPt.Y = pictureBoxTime.Height - 20;
            drawingRect.X = 0;
            drawingRect.Width = pictureBoxTime.Width;
            drawingRect.Y = topLeftPt.Y;
            drawingRect.Height = bottomRightPt.Y - topLeftPt.Y;

            if (e.ClipRectangle.IntersectsWith(drawingRect) && _showPositionMarker && _position != -1) {
                var x = _gridColWidth * (_position - hScrollBar1.Value);
                if (x < pictureBoxTime.Width) {
                    topLeftPt.X = x;
                    bottomRightPt.X = x;
                    e.Graphics.DrawLine(Pens.Red, topLeftPt, bottomRightPt);
                }
            }
            if (_mouseTimeCaret != -1) {
                e.Graphics.FillRectangle(_channelCaretBrush, (_mouseTimeCaret - hScrollBar1.Value) * _gridColWidth, 0, _gridColWidth, CaretSize);
            }

            drawingRect.X = 0;
            drawingRect.Width = pictureBoxTime.Width;
            drawingRect.Y = WaveformOffset;
            drawingRect.Height = pictureBoxTime.Height;
            if (toolStripButtonWaveform.Checked && e.ClipRectangle.IntersectsWith(drawingRect)) {
                DrawWaveform(e);
            }
        }


        private void DrawWaveform(PaintEventArgs e) {
            var startPosition = hScrollBar1.Value * _gridColWidth;
            var endPosition = Math.Min(startPosition + ((_visibleEventPeriods + 1) * _gridColWidth), _waveformPixelData.Length);
            // 49f = 6px caret + 35px tick mark + 8px timeFont
            // 79f = 120px size when initially maximized - 49f from above.
            var scaleFactor = ((splitContainer2.SplitterDistance - 49f) / 71f);
            var centerLine = (int) (_waveformCenterLine * (scaleFactor)) + WaveformOffset;
            var x = 0;
            for (; startPosition < endPosition; startPosition++) {
                var pix = _waveformPixelData[startPosition];

                var y1 = centerLine - (short) (pix >> 16) * (scaleFactor);
                var y2 = centerLine - (short) (pix & 0xffff) * (scaleFactor);
                e.Graphics.DrawLine(Pens.White, x, y1, x, y2);
                x++;
            }
            e.Graphics.DrawLine(Pens.Red, 0, centerLine, x - 1, centerLine);
        }


        private void PlaceSparkle(byte[,] valueArray, int row, int startCol, int decayTime, byte minIntensity, byte maxIntensity) {
            var valueCount = valueArray.GetLength(Utils.IndexColsOrWidth);

            var minEvents = (int) (Math.Round(_sequence.EventsPerSecond * 0.1, MidpointRounding.AwayFromZero));
            if ((startCol + minEvents) >= valueCount) {
                minEvents = valueCount - startCol;
            }

            int columnOffset;
            for (columnOffset = 0; columnOffset < minEvents; columnOffset++) {
                valueArray[row, startCol + columnOffset] = maxIntensity;
            }

            var decayPeriods = (int) Math.Round(((float) decayTime) / _sequence.EventPeriod, MidpointRounding.AwayFromZero);
            if (((startCol + minEvents) + decayPeriods) >= valueCount) {
                decayPeriods = (valueCount - startCol) - minEvents;
            }

            if (decayPeriods <= 0) {
                return;
            }

            var currentDecay = (byte) ((maxIntensity - minIntensity) / decayPeriods);
            var currentValue = (byte) (maxIntensity - currentDecay);

            columnOffset = startCol + minEvents;
            while (--decayPeriods > 0) {
                valueArray[row, columnOffset++] = currentValue;
                currentValue -= currentDecay;
            }
        }


        //private void playAtTheSelectedPointToolStripMenuItem_Click(object sender, EventArgs e) {
        //    playAtTheSelectedPointToolStripMenuItem.Checked = true;
        //    playOnlyTheSelectedRangeToolStripMenuItem.Checked = false;
        //    tsbPlayFrom.ToolTipText = Resources.PlayFromStartPoint;
        //}


        //private void playOnlyTheSelectedRangeToolStripMenuItem_Click(object sender, EventArgs e) {
        //    playOnlyTheSelectedRangeToolStripMenuItem.Checked = true;
        //    playAtTheSelectedPointToolStripMenuItem.Checked = false;
        //    tsbPlayFrom.ToolTipText = Resources.PlayRange;
        //}


        private void plugInItem_CheckedChanged(object sender, EventArgs e) {
            var item = (ToolStripMenuItem) sender;
            var attributes = _sequence.PlugInData.GetPlugInData((string) item.Tag).Attributes;
            if (attributes != null) {
                attributes["enabled"].Value = item.Checked.ToString();
            }
            IsDirty = true;
        }


        private void ProgramEnded() {
            m_positionTimer.Stop();
            SetEditingState(true);
            UpdatePlayButtons(0);
            pictureBoxGrid.Refresh();
        }


        private void quarterSpeedToolStripMenuItem_Click(object sender, EventArgs e) {
            SetAudioSpeed(0.25f);
        }


        private void Ramp(int startingLevel, int endingLevel) {
            var originalAction = (startingLevel > endingLevel) ? Resources.UndoText_Fade : Resources.UndoText_Ramp;
            if ((startingLevel != _sequence.MinimumLevel && endingLevel != _sequence.MinimumLevel) ||
                (startingLevel != _sequence.MaximumLevel && endingLevel != _sequence.MaximumLevel)) {
                originalAction = Resources.UndoText_Partial + @" " + originalAction;
            }
            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, originalAction);
            var bottom = _selectedCells.Bottom;
            var right = _selectedCells.Right;
            var left = _selectedCells.Left;
            for (var top = _selectedCells.Top; top < bottom; top++) {
                var channelOrder = _channelOrderMapping[top];
                if (!_sequence.Channels[channelOrder].Enabled) {
                    continue;
                }

                for (var column = left; column < right; column++) {
                    var computedLevel = (byte) ((((column - left) / ((float) ((right - left) - 1))) * (endingLevel - startingLevel)) + startingLevel);
                    if (computedLevel < _sequence.MinimumLevel) {
                        computedLevel = _sequence.MinimumLevel;
                    }
                    else if (computedLevel > _sequence.MaximumLevel) {
                        computedLevel = _sequence.MaximumLevel;
                    }
                    _sequence.EventValues[channelOrder, column] = computedLevel;
                }
            }
            _selectionRectangle.Width = 0;
            pictureBoxGrid.Invalidate(SelectionToRectangle());
        }


        private Rectangle RangeToRectangle(Rectangle range) {
            return new Rectangle {
                X = ((range.X - hScrollBar1.Value) * _gridColWidth) + 1, Y = ((range.Y - vScrollBar1.Value) * _gridRowHeight) + 1,
                Width = (range.Width * _gridColWidth) - 1, Height = (range.Height * _gridRowHeight) - 1
            };
        }


        private void ReactEditingStateToProfileAssignment() {
            var flag = _sequence.Profile != null;
            textBoxChannelCount.ReadOnly = flag;
            toolStripDropDownButtonPlugins.Enabled = !flag;
            toolStripButtonSaveOrder.Enabled = !flag;
            toolStripButtonChannelOutputMask.Enabled = !flag;
        }


        private void ReactToProfileAssignment() {
            var flag = _sequence.Profile != null;
            profileToolStripLabel.Text = flag ? _sequence.Profile.Name : "Embedded";
            flattenProfileIntoSequenceToolStripMenuItem.Enabled = flag;
            detachSequenceFromItsProfileToolStripMenuItem.Enabled = flag;
            channelOutputMaskToolStripMenuItem.Enabled = !flag;
            ReactEditingStateToProfileAssignment();
            SetOrderArraySize(_sequence.ChannelCount);
            textBoxChannelCount.Text = _sequence.ChannelCount.ToString(CultureInfo.InvariantCulture);
            VScrollCheck();
            pictureBoxChannels.Refresh();
            pictureBoxGrid.Refresh();
            LoadSequenceSorts();
            LoadSequencePlugins();
        }


        private void ReadFromSequence() {
            Text = _sequence.Name;
            SetProgramTime(_sequence.Time);
            ReactToProfileAssignment();
            pictureBoxChannels.Refresh();
            VScrollCheck();
        }


        private void Redo() {
            if (_redoStack.Count == 0) {
                return;
            }

            var undo = (UndoItem) _redoStack.Pop();
            var height = undo.Data == null ? 0 : undo.Data.GetLength(Utils.IndexRowsOrHeight);
            var width = undo.Data == null ? 0 : undo.Data.GetLength(Utils.IndexColsOrWidth);

            toolStripButtonRedo.Enabled = redoToolStripMenuItem.Enabled = _redoStack.Count > 0;
            IsDirty = true;

            var redo = new UndoItem(undo.Location, GetAffectedBlockData(new Rectangle(undo.Location.X, undo.Location.Y, width, height)), undo.Behavior,
                                    _sequence, _channelOrderMapping, undo.OriginalAction);

            switch (undo.Behavior) {
                case UndoOriginalBehavior.Overwrite:
                    DisjointedOverwrite(undo.Location.X, undo.Data, undo.ReferencedChannels);
                    pictureBoxGrid.Invalidate(new Rectangle((undo.Location.X - hScrollBar1.Value) * _gridColWidth,
                                                            (undo.Location.Y - vScrollBar1.Value) * _gridRowHeight, width * _gridColWidth,
                                                            height * _gridRowHeight));
                    break;

                case UndoOriginalBehavior.Removal:
                    DisjointedRemove(undo.Location.X, width, height, undo.ReferencedChannels);
                    pictureBoxGrid.Refresh();
                    break;

                case UndoOriginalBehavior.Insertion:
                    DisjointedInsert(undo.Location.X, width, height, undo.ReferencedChannels);
                    DisjointedOverwrite(undo.Location.X, undo.Data, undo.ReferencedChannels);
                    pictureBoxGrid.Refresh();
                    break;
            }
            UpdateRedoText();
            _undoStack.Push(redo);
            toolStripButtonUndo.Enabled = undoToolStripMenuItem.Enabled = true;
            UpdateUndoText();
        }


        private void RefreshAll() {
            SetOrderArraySize(_sequence.ChannelCount);
            textBoxChannelCount.Text = _sequence.ChannelCount.ToString(CultureInfo.InvariantCulture);
            textBoxProgramLength.Text = Utils.TimeFormatWithMills(_sequence.Time);
            pictureBoxGrid.Refresh();
            pictureBoxChannels.Refresh();
            pictureBoxTime.Refresh();
            VScrollCheck();
            HScrollCheck();
        }


        private void ReorderChannelOutputs() {
            if (_sequence.Profile != null) {
                MessageBox.Show(Resources.UseProfileToEditChannels, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            using (var dialog = new ChannelOrderDialog(_sequence.OutputChannels, null) {Text = Resources.ChannelOrderDialogText}) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                var channelMapping = dialog.ChannelMapping;
                foreach (var channel in _sequence.Channels) {
                    channel.OutputChannel = channelMapping.IndexOf(channel);
                }
                if (_showingOutputs) {
                    pictureBoxChannels.Refresh();
                    pictureBoxGrid.Refresh();
                }
                IsDirty = true;
            }
        }


        private void reorderChannelOutputsToolStripMenuItem_Click(object sender, EventArgs e) {
            ReorderChannelOutputs();
        }


        private void Reset() {
            hScrollBar1.Value = hScrollBar1.Minimum;
            toolStripLabelExecutionPoint.Text = @"00:00";
        }


        private void resetAllToolbarsToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripManager.LoadSettings(this, _preferences.XmlDoc.DocumentElement, "reset");
            foreach (ToolStripItem item in toolbarsToolStripMenuItem.DropDownItems) {
                if ((item is ToolStripMenuItem) && (item.Tag != null)) {
                    ((ToolStripMenuItem) item).Checked = true;
                }
            }
        }


        public override DialogResult RunWizard(ref EventSequence resultSequence) {
            DialogResult result;
            using (var dialog = new NewSequenceWizardDialog(_systemInterface.UserPreferences)) {
                result = dialog.ShowDialog();
                if (result == DialogResult.OK) {
                    resultSequence = dialog.Sequence;
                }
            }
            return result;
        }


        private void saveAsARoutineToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult result;
            var path = string.Empty;
            using (var dialog = new TextQueryDialog(Vendor.ProductName, Resources.RoutineNamePrompt, string.Empty)) {
                do {
                    result = dialog.ShowDialog();
                    if (result != DialogResult.OK) {
                        continue;
                    }

                    path = Path.Combine(Paths.RoutinePath, Path.GetFileNameWithoutExtension(dialog.Response) + Vendor.RoutineExtension);
                    if (File.Exists(path)) {
                        result = MessageBox.Show(Resources.OverwriteFilePrompt, Vendor.ProductName, MessageBoxButtons.YesNoCancel,
                                                 MessageBoxIcon.Question);
                    }
                } while (result == DialogResult.No);
            }

            if (result == DialogResult.Cancel) {
                return;
            }

            using (var writer = new StreamWriter(path)) {
                var buffer = CellsToArray();
                var rows = buffer.GetLength(Utils.IndexRowsOrHeight);
                var cols = buffer.GetLength(Utils.IndexColsOrWidth);
                for (var row = 0; row < rows; row++) {
                    for (var col = 0; col < cols; col++) {
                        writer.Write(buffer[row, col].ToString(CultureInfo.InvariantCulture) + " ");
                    }
                    writer.WriteLine();
                }
            }
            MessageBox.Show(string.Format(Resources.StringFormat_RoutineSaved, Path.GetFileNameWithoutExtension(path)), Vendor.ProductName,
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        public override void SaveTo(string filePath) {
            _sequence.SaveTo(filePath);
            IsDirty = false;
        }


        private void saveToolbarPositionsToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripManager.SaveSettings(this, _preferences.XmlDoc.DocumentElement);
            _preferences.Flush();
            MessageBox.Show(Resources.ToolbarSettingsSaved, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        private void scaleToolStripMenuItem_Click(object sender, EventArgs e) {
            ArithmeticPaste(ArithmeticOperation.Scale);
        }


        private void ScrollSelectionDown(int cellX) {
            _selectedRange.Width = cellX + 1 - _selectedRange.Left;

            var y = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Left, pictureBoxGrid.Bottom)).Y - pictureBoxTime.Height;
            while (MousePosition.Y > y && MouseButtons == MouseButtons.Left) {
                var cellY = pictureBoxGrid.PointToClient(MousePosition).Y / _gridColWidth;
                cellY += vScrollBar1.Value;
                if (cellY >= (_sequence.ChannelCount - 1)) {
                    _selectedCells.Height = _selectedRange.Height = _sequence.ChannelCount - _selectedRange.Y;
                    if (cellX != _lastCellX) {
                        pictureBoxGrid.Refresh();
                    }
                }
                if (vScrollBar1.Value > (vScrollBar1.Maximum - vScrollBar1.LargeChange)) {
                    break;
                }
                if (vScrollBar1.Value < (vScrollBar1.Maximum - vScrollBar1.LargeChange)) {
                    _selectedRange.Height++;
                    _selectedCells.Height++;
                }
                vScrollBar1.Value++;
                // Ugly, but it is the only way I've found that we seem to be able to catch that the mouse button is not down anymore :(
                Application.DoEvents();
            }
        }


        private void ScrollSelectionLeft(int cellY) {
            var x = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Left, pictureBoxGrid.Top)).X;
            while (MousePosition.X < x && hScrollBar1.Value != 0 && MouseButtons == MouseButtons.Left) {
                _selectedRange.Height = (cellY + 1) - _selectedRange.Top;
                _selectedRange.Width--;
                _selectedCells = NormalizeRect(_selectedRange);
                hScrollBar1.Value--;
                // Ugly, but it is the only way I've found that we seem to be able to catch that the mouse button is not down anymore :(
                Application.DoEvents();
            }
        }


        private void ScrollSelectionRight(int cellY) {
            _selectedRange.Height = (cellY + 1) - _selectedRange.Top;

            var x = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Right, pictureBoxGrid.Top)).X;
            while (MousePosition.X > x && MouseButtons == MouseButtons.Left) {
                var cellX = pictureBoxGrid.PointToClient(MousePosition).X / _gridColWidth;
                cellX += hScrollBar1.Value;
                if (cellX >= (_sequence.TotalEventPeriods - 1)) {
                    _selectedCells.Width = _selectedRange.Width = _sequence.TotalEventPeriods - _selectedRange.X;
                    if (cellY != _lastCellY) {
                        pictureBoxGrid.Refresh();
                    }
                }
                if (hScrollBar1.Value > (hScrollBar1.Maximum - hScrollBar1.LargeChange)) {
                    break;
                }
                if (hScrollBar1.Value < (hScrollBar1.Maximum - hScrollBar1.LargeChange)) {
                    _selectedRange.Width++;
                    _selectedCells.Width++;
                }
                hScrollBar1.Value++;
                // Ugly, but it is the only way I've found that we seem to be able to catch that the mouse button is not down anymore :(
                Application.DoEvents();
            }
        }


        private void ScrollSelectionUp(int cellX) {
            var y = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Left, pictureBoxGrid.Top)).Y;
            while (MousePosition.Y < y && (vScrollBar1.Value != 0) && MouseButtons == MouseButtons.Left) {
                _selectedRange.Width = (cellX + 1) - _selectedRange.Left;
                _selectedRange.Height--;
                _selectedCells = NormalizeRect(_selectedRange);
                vScrollBar1.Value--;
                // Ugly, but it is the only way I've found that we seem to be able to catch that the mouse button is not down anymore :(
                Application.DoEvents();
            }
        }


        private bool SelectableControlFocused() {
            var terminalSelectableControl = GetTerminalSelectableControl();
            return ((terminalSelectableControl != null) && terminalSelectableControl.CanSelect);
        }


        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (ToolStripItem item in toolStripDropDownButtonPlugins.DropDownItems) {
                if (!(item is ToolStripMenuItem)) {
                    break;
                }
                ((ToolStripMenuItem) item).Checked = true;
            }
        }


        private Rectangle SelectionToRectangle() {
            return new Rectangle {
                X = ((_selectedCells.X - hScrollBar1.Value) * _gridColWidth) + 1, Y = ((_selectedCells.Y - vScrollBar1.Value) * _gridRowHeight) + 1,
                Width = (_selectedCells.Width * _gridColWidth) - 1, Height = (_selectedCells.Height * _gridRowHeight) - 1
            };
        }


        private void setAllChannelColorsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_sequence.Profile != null) {
                MessageBox.Show(Resources.UseProfileToEditChannels, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            using (var dialog = new AllChannelsColorDialog(_sequence.Channels)) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    var channelCount = _sequence.ChannelCount;
                    var channelColors = dialog.ChannelColors;
                    for (var i = 0; i < channelCount; i++) {
                        _sequence.Channels[i].Color = channelColors[i];
                    }
                }
            }
            IsDirty = true;
            pictureBoxChannels.Refresh();
            pictureBoxGrid.Refresh();
        }


        private void SetAudioSpeed(float rate) {
            if ((!(rate > 0f)) || (!(rate <= 1f))) {
                return;
            }

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
            if (Equals(rate, 0.25f)) {
                xToolStripMenuItem.Checked = true;
                SpeedQtrTsb.Checked = true;
            }
            else if (Equals(rate, 0.5f)) {
                xToolStripMenuItem1.Checked = true;
                SpeedHalfTsb.Checked = true;
            }
            else if (Equals(rate, 0.75f)) {
                xToolStripMenuItem2.Checked = true;
                SpeedThreeQtrTsb.Checked = true;
            }
            else if (Equals(rate, 1f)) {
                normalToolStripMenuItem1.Checked = true;
                SpeedNormalTsb.Checked = true;
            }
            else {
                otherToolStripMenuItem.Checked = true;
                SpeedVariableTsb.Checked = true;
            }

            SpeedQtrTsb.BackgroundImage = ToolStripManager.GetBackground(SpeedQtrTsb.Checked);
            SpeedHalfTsb.BackgroundImage = ToolStripManager.GetBackground(SpeedHalfTsb.Checked);
            SpeedThreeQtrTsb.BackgroundImage = ToolStripManager.GetBackground(SpeedThreeQtrTsb.Checked);
            SpeedNormalTsb.BackgroundImage = ToolStripManager.GetBackground(SpeedNormalTsb.Checked);
            SpeedVariableTsb.BackgroundImage = ToolStripManager.GetBackground(SpeedVariableTsb.Checked);

            _executionInterface.SetAudioSpeed(_executionContextHandle, rate);
        }


        private void SetChannelCount(int count) {
            if (count == _sequence.ChannelCount) {
                return;
            }

            int channel;
            var flag = false;
            var channelCount = Math.Min(_sequence.ChannelCount, count);
            for (channel = 0; channel < channelCount; channel++) {
                if (_sequence.Channels[channel].OutputChannel > (count - 1)) {
                    flag = true;
                    break;
                }
            }
            if (flag) {
                if (MessageBox.Show(Resources.NewChannelCountPrompt, Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) !=
                    DialogResult.Yes) {
                    textBoxChannelCount.Text = _sequence.ChannelCount.ToString(CultureInfo.InvariantCulture);
                    return;
                }
                for (channel = 0; channel < _sequence.ChannelCount; channel++) {
                    _sequence.Channels[channel].OutputChannel = channel;
                }
            }
            SetOrderArraySize(count);
            _sequence.ChannelCount = count;
            textBoxChannelCount.Text = count.ToString(CultureInfo.InvariantCulture);
            VScrollCheck();
            pictureBoxChannels.Refresh();
            pictureBoxGrid.Refresh();
            IsDirty = true;
            MessageBox.Show(Resources.ChannelCountUpdated, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        private void SetDrawingLevel(byte level) {
            _drawingLevel = level;
            toolStripLabelCurrentDrawingIntensity.Text = _actualLevels
                                                             ? level.ToString(CultureInfo.InvariantCulture)
                                                             : string.Format("{0}%", Utils.ToPercentage(level));
        }


        private void SetEditingState(bool state) {
            if (state) {
                EnableWaveformButton();
            }
            else {
                DisableWaveformButton();
            }
            toolStripEditing.Enabled = state;
            toolStripEffect.Enabled = state;
            toolStripSequenceSettings.Enabled = state;
            toolStripDropDownButtonPlugins.Enabled = state;
            toolStripDisplaySettings.Enabled = state;
            tsbLoop.Enabled = state;
            SpeedQtrTsb.Enabled = state;
            SpeedHalfTsb.Enabled = state;
            SpeedThreeQtrTsb.Enabled = state;
            SpeedNormalTsb.Enabled = state;
            SpeedVariableTsb.Enabled = state;
            ReactEditingStateToProfileAssignment();
        }


        private void SetOrderArraySize(int count) {
            if (count < _channelOrderMapping.Count) {
                var list = new List<int>();
                list.AddRange(_channelOrderMapping);
                foreach (var channel in list) {
                    if (channel >= count) {
                        _channelOrderMapping.Remove(channel);
                    }
                }
            }
            else {
                for (var i = _channelOrderMapping.Count; i < count; i++) {
                    _channelOrderMapping.Add(i);
                }
            }
        }


        private void SetProfile(string filePath) {
            if (filePath != null) {
                SetProfile(new Profile(openFileDialog1.FileName));
            }
            else {
                SetProfile((Profile) null);
            }
        }


        private void SetProfile(Profile profile) {
            _sequence.Profile = profile;
            ReactToProfileAssignment();
            IsDirty = true;
        }


        private void SetProgramTime(int milliseconds) {
            try {
                _sequence.Time = milliseconds;
            }
            catch {
                MessageBox.Show(Resources.SetProgramTimeError, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                textBoxProgramLength.Text = Utils.TimeFormatWithMills(_sequence.Time);
                return;
            }
            textBoxProgramLength.Text = Utils.TimeFormatWithMills(_sequence.Time);
            HScrollCheck();
            pictureBoxTime.Refresh();
            pictureBoxGrid.Refresh();
        }


        private void SetVariablePlaybackSpeed(Point dialogScreenCoords) {
            using (var dialog = new AudioSpeedDialog()) {
                if ((dialogScreenCoords.X == 0) && (dialogScreenCoords.Y == 0)) {
                    dialog.StartPosition = FormStartPosition.CenterScreen;
                }
                else {
                    dialog.StartPosition = FormStartPosition.Manual;
                    dialog.Location = dialogScreenCoords;
                }
                dialog.Rate = _executionInterface.GetAudioSpeed(_executionContextHandle);
                if (dialog.ShowDialog() == DialogResult.OK) {
                    SetAudioSpeed(dialog.Rate);
                }
            }
        }


        private void ShowChannelProperties() {
            var channels = new List<VixenPlus.Channel>();
            channels.AddRange(_sequence.Channels);
            for (var i = 0; i < channels.Count; i++) {
                channels[i] = _sequence.Channels[_channelOrderMapping[i]];
            }
            using (var dialog = new ChannelPropertyDialog(channels, SelectedChannel, true)) {
                dialog.ShowDialog();
            }
            pictureBoxChannels.Refresh();
            pictureBoxGrid.Refresh();
            IsDirty = true;
        }


        private void sortByChannelNumberToolStripMenuItem_Click(object sender, EventArgs e) {
            //_printingChannelList = _sequence.Channels;
            //if (printDialog.ShowDialog() != DialogResult.OK) {
            //    return;
            //}

            //_printDocument.DocumentName = "Vixen channel configuration";
            ////_printingChannelIndex = 0;
            //_printDocument.Print();
        }


        private void sortByChannelOutputToolStripMenuItem_Click(object sender, EventArgs e) {
            //_printingChannelList = new List<VixenPlus.Channel>();
            //var collection = new VixenPlus.Channel[_sequence.ChannelCount];
            //foreach (var channel in _sequence.Channels) {
            //    collection[channel.OutputChannel] = channel;
            //}
            //_printingChannelList.AddRange(collection);

            //if (printDialog.ShowDialog() != DialogResult.OK) {
            //    return;
            //}
            //_printDocument.DocumentName = "Vixen channel configuration";
            ////_printingChannelIndex = 0;
            //_printDocument.Print();
        }


        private void SparkleGenerator(byte[,] values, params int[] effectParameters) {
            var maxValue = (int) _sequence.EventsPerSecond - effectParameters[0];
            var decayTime = effectParameters[1];
            var height = values.GetLength(Utils.IndexRowsOrHeight);
            var width = values.GetLength(Utils.IndexColsOrWidth);


            for (var row = 0; row < height; row++) {
                for (var col = 0; col < width; col++) {
                    values[row, col] = _sequence.MinimumLevel;
                }
            }

            var sparkleArray = new int[width];
            var random = new Random();
            for (var index = 0; index < width;) {
                sparkleArray[index] = random.Next(height) + 1;
                index += Math.Max(random.Next(maxValue), 1);
            }
            for (var sparkleElement = 0; sparkleElement < sparkleArray.Length; sparkleElement++) {
                if (sparkleArray[sparkleElement] != 0) {
                    PlaceSparkle(values, sparkleArray[sparkleElement] - 1, sparkleElement, decayTime, (byte) effectParameters[2],
                                 (byte) effectParameters[3]);
                }
            }
        }


        private void StandardSequence_Activated(object sender, EventArgs e) {
            ActiveControl = m_lastSelectableControl;
        }


        private void StandardSequence_Deactivate(object sender, EventArgs e) {
            m_lastSelectableControl = GetTerminalSelectableControl();
        }


        private void StandardSequence_FormClosing(object sender, FormClosingEventArgs e) {
            if ((e.CloseReason == CloseReason.UserClosing) && (CheckDirty() == DialogResult.Cancel)) {
                e.Cancel = true;
            }
            else {
                if (_executionInterface.EngineStatus(_executionContextHandle) != Utils.ExecutionStopped) {
                    toolStripButtonStop_Click(null, null);
                }
                if (_preferences.GetBoolean("SaveZoomLevels")) {
                    _preferences.SetChildString("SaveZoomLevels", "row", toolStripComboBoxRowZoom.SelectedItem.ToString());
                    _preferences.SetChildString("SaveZoomLevels", "column", toolStripComboBoxColumnZoom.SelectedItem.ToString());
                    _preferences.Flush();
                }
                _sequence.UpdateMetrics(Width, Height, splitContainer1.SplitterDistance);
                _executionInterface.ReleaseContext(_executionContextHandle);
            }
        }


        private void StandardSequence_KeyDown(object sender, KeyEventArgs keyEvent) {
            HandelGlobalKeys(keyEvent);

            if (!keyEvent.Handled) HandleBookmarkKeys(keyEvent);

            var isNotRunning = _executionInterface.EngineStatus(_executionContextHandle) != Utils.ExecutionRunning;

            // Keys here only work on selected areas if they exist.
            if (!keyEvent.Handled && _selectedCells.Width > 0) {
                HandleSpaceAndArrowKeys(keyEvent);

                if (!keyEvent.Handled && isNotRunning && pictureBoxGrid.Focused) {
                    HandleIntensityAdjustKeys(keyEvent);
                    if (!keyEvent.Handled) HandleAtoZKeys(keyEvent);
                }
            }

            if (!keyEvent.Handled && isNotRunning && pictureBoxChannels.Focused && SelectedChannel != null) {
                HandleChannelKeyPress(keyEvent);
            }
        }


        private void HandleBookmarkKeys(KeyEventArgs e) {
            if (!e.Control || e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) {
                return;
            }

            var index = ((int) e.KeyCode) - (int) Keys.D0;
            e.Handled = true;

            if (e.Shift) {
                _bookmarks[index] = (_bookmarks[index] == _selectedCells.Left) ? -1 : _selectedCells.Left;
                pictureBoxTime.Refresh();
            }
            else if (_bookmarks[index] != -1) {
                hScrollBar1.Value = _bookmarks[index];
            }
        }


        private void HandleSpaceAndArrowKeys(KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Space: {
                    if ((SelectableControlFocused() && !pictureBoxChannels.Focused) && !pictureBoxGrid.Focused) {
                        break;
                    }

                    int currentPosition;
                    if (_executionInterface.EngineStatus(_executionContextHandle, out currentPosition) != Utils.ExecutionRunning) {

                        var nonZeroCellCount = 0;

                        for (var top = _selectedCells.Top; top < _selectedCells.Bottom; top++) {
                            var channel = _channelOrderMapping[top];
                            for (var left = _selectedCells.Left; left < _selectedCells.Right; left++) {
                                if (_sequence.EventValues[channel, left] > _sequence.MinimumLevel) {
                                    nonZeroCellCount++;
                                }
                            }
                        }

                        var selectedCellsCount = _selectedCells.Height * _selectedCells.Width;
                        var level = _drawingLevel;
                        var originalAction = Resources.OnText;
                        if (nonZeroCellCount == selectedCellsCount) {
                            level = _sequence.MinimumLevel;
                            originalAction = Resources.OffText;
                        }

                        AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, originalAction);

                        for (var top = _selectedCells.Top; top < _selectedCells.Bottom; top++) {
                            var channel = _channelOrderMapping[top];
                            for (var left = _selectedCells.Left; left < _selectedCells.Right; left++) {
                                _sequence.EventValues[channel, left] = level;
                            }
                        }

                        pictureBoxGrid.Invalidate(new Rectangle((_selectedCells.Left - hScrollBar1.Value) * _gridColWidth,
                                                                (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight,
                                                                _selectedCells.Width * _gridColWidth, _selectedCells.Height * _gridRowHeight));
                        e.Handled = true;
                        break;
                    }
                    var currentEvent = currentPosition / _sequence.EventPeriod;
                    AddUndoItem(new Rectangle(currentEvent, _selectedCells.Top, 1, _selectedCells.Height), UndoOriginalBehavior.Overwrite,
                                Resources.OnText);

                    for (var i = _selectedCells.Top; i < _selectedCells.Bottom; i++) {
                        var channel = _channelOrderMapping[i];
                        _sequence.EventValues[channel, currentEvent] = _drawingLevel;
                    }
                    pictureBoxGrid.Invalidate(new Rectangle((currentEvent - hScrollBar1.Value) * _gridColWidth,
                                                            (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight, _gridColWidth,
                                                            _selectedCells.Height * _gridRowHeight));
                    e.Handled = true;
                    break;
                }
                case Keys.Left:
                    if (!pictureBoxChannels.Focused && !pictureBoxGrid.Focused) {
                        break;
                    }
                    if ((hScrollBar1.Value > 0) || (_selectedCells.Left > 0)) {
                        e.Handled = true;
                        _selectedRange.X--;
                        _selectedCells.X--;
                        if ((_selectedCells.Left + 1) <= hScrollBar1.Value) {
                            hScrollBar1.Value--;
                            break;
                        }
                        pictureBoxGrid.Invalidate(new Rectangle((_selectedCells.Left - hScrollBar1.Value) * _gridColWidth,
                                                                (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight,
                                                                (_selectedCells.Width + 1) * _gridColWidth, _selectedCells.Height * _gridRowHeight));
                    }
                    break;

                case Keys.Up:
                    if ((pictureBoxChannels.Focused || (pictureBoxGrid.Focused && !e.Control)) &&
                        ((vScrollBar1.Value > 0) || (_selectedCells.Top > 0))) {
                        e.Handled = true;
                        _selectedRange.Y--;
                        _selectedCells.Y--;
                        if ((_selectedCells.Top + 1) <= vScrollBar1.Value) {
                            vScrollBar1.Value--;
                        }
                        else {
                            pictureBoxGrid.Invalidate(new Rectangle((_selectedCells.Left - hScrollBar1.Value) * _gridColWidth,
                                                                    (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight,
                                                                    _selectedCells.Width * _gridColWidth, (_selectedCells.Height + 1) * _gridRowHeight));
                        }
                    }
                    break;

                case Keys.Right:
                    if (pictureBoxChannels.Focused || pictureBoxGrid.Focused) {
                        if (_selectedCells.Right < _sequence.TotalEventPeriods) {
                            e.Handled = true;
                            _selectedRange.X++;
                            _selectedCells.X++;
                            if (((_selectedCells.Right - 1) - hScrollBar1.Value) >= _visibleEventPeriods) {
                                hScrollBar1.Value++;
                            }
                            else {
                                pictureBoxGrid.Invalidate(new Rectangle(((_selectedCells.Left - hScrollBar1.Value) - 1) * _gridColWidth,
                                                                        (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight,
                                                                        (_selectedCells.Width + 1) * _gridColWidth,
                                                                        _selectedCells.Height * _gridRowHeight));
                            }
                        }
                    }
                    break;

                case Keys.Down:
                    if ((pictureBoxChannels.Focused || (pictureBoxGrid.Focused && !e.Control)) && (_selectedCells.Bottom < _sequence.ChannelCount)) {
                        e.Handled = true;
                        _selectedRange.Y++;
                        _selectedCells.Y++;
                        if (((_selectedCells.Bottom - 1) - vScrollBar1.Value) >= _visibleRowCount) {
                            vScrollBar1.Value++;
                        }
                        else {
                            pictureBoxGrid.Invalidate(new Rectangle((_selectedCells.Left - hScrollBar1.Value) * _gridColWidth,
                                                                    ((_selectedCells.Top - vScrollBar1.Value) - 1) * _gridRowHeight,
                                                                    _selectedCells.Width * _gridColWidth, (_selectedCells.Height + 1) * _gridRowHeight));
                        }
                    }
                    break;

            }
        }


        /// <summary>
        ///  Process the A - Z keys and their SAC variants
        ///  Currently using A,E,F,H,I,R,S,T,V & X
        ///  Was also using: B,C,D,G & U - but these did nothing.
        /// </summary>
        /// <param name="keyEvent"></param>
        private void HandleAtoZKeys(KeyEventArgs keyEvent) {
            if (keyEvent.KeyCode < Keys.A || keyEvent.KeyCode > Keys.Z) {
                return;
            }

            switch (keyEvent.KeyCode) {
                case Keys.A:
                    toolStripButtonRandom_Click(null, null);
                    keyEvent.Handled = true;
                    break;

                case Keys.E:
                    toolStripButtonShimmerDimming_Click(null, null);
                    keyEvent.Handled = true;
                    break;

                case Keys.F:
                    if (keyEvent.Shift) {
                        toolStripButtonPartialRampOff_Click(null, null);
                    }
                    else {
                        toolStripButtonRampOff_Click(null, null);
                    }
                    keyEvent.Handled = true;
                    break;

                case Keys.H:
                    toolStripButtonMirrorHorizontal_Click(null, null);
                    keyEvent.Handled = true;
                    break;

                case Keys.I:
                    toolStripButtonIntensity_Click(null, null);
                    keyEvent.Handled = true;
                    break;

                case Keys.R:
                    if (keyEvent.Shift) {
                        toolStripButtonPartialRampOn_Click(null, null);
                    }
                    else {
                        toolStripButtonRampOn_Click(null, null);
                    }
                    keyEvent.Handled = true;
                    break;

                case Keys.S:
                    toolStripButtonSparkle_Click(null, null);
                    keyEvent.Handled = true;
                    break;

                case Keys.T:
                    toolStripButtonInvert_Click(null, null);
                    keyEvent.Handled = true;
                    break;

                case Keys.V:
                    toolStripButtonMirrorVertical_Click(null, null);
                    keyEvent.Handled = true;
                    break;

                case Keys.X:
                    toolStripButtonToggleCrossHairs_Click(null, null);
                    keyEvent.Handled = true;
                    break;
            }
        }


        private void HandleIntensityAdjustKeys(KeyEventArgs e) {
            if (((e.KeyCode != Keys.Up) && (e.KeyCode != Keys.Down)) || !e.Control) {
                return;
            }

            IntensityAdjustDialogCheck();
            var change = e.Alt ? 1 : _intensityLargeDelta;
            m_intensityAdjustDialog.Delta = (e.KeyCode == Keys.Up) ? change : -change;
            e.Handled = true;
        }


        private void HandelGlobalKeys(KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Prior:
                    if (vScrollBar1.Value > 0) {
                        var newPosition = Math.Min(_visibleRowCount, vScrollBar1.Value);
                        _selectedRange.Y -= newPosition;
                        _selectedCells.Y -= newPosition;
                        vScrollBar1.Value -= newPosition;
                        e.Handled = true;
                    }
                    break;

                case Keys.Next:
                    if (vScrollBar1.Value < _sequence.ChannelCount - _visibleRowCount) {
                        var num2 = Math.Min(_sequence.ChannelCount - vScrollBar1.Value, _visibleRowCount);
                        var num3 = Math.Min(_sequence.ChannelCount - _selectedCells.Bottom, _visibleRowCount);
                        var newPosition = Math.Min(num2, num3);
                        _selectedRange.Y += newPosition;
                        _selectedCells.Y += newPosition;
                        vScrollBar1.Value += newPosition;
                        e.Handled = true;
                    }
                    break;

                case Keys.End:
                    if (hScrollBar1.Value < _sequence.TotalEventPeriods - _visibleEventPeriods) {
                        var positionFromEnd = _sequence.TotalEventPeriods - _visibleEventPeriods;
                        _selectedRange.X = positionFromEnd;
                        _selectedCells.X = positionFromEnd;
                        hScrollBar1.Value = positionFromEnd;
                        e.Handled = true;
                    }
                    break;

                case Keys.Home:
                    if (hScrollBar1.Value > 0) {
                        _selectedRange.X = 0;
                        _selectedCells.X = 0;
                        hScrollBar1.Value = 0;
                        e.Handled = true;
                    }
                    break;

                case Keys.F5:
                    if (tsbPlay.Enabled) {
                        e.Handled = true; 
                        if (e.Alt) {
                            using (var dialog = new DelayedStartDialog()) {
                                if (dialog.ShowDialog() != DialogResult.OK) {
                                    return;
                                }
                            }
                        }
                        toolStripButtonPlay_Click(null, null);
                    }
                    break;

                case Keys.F6:
                    if (e.Alt && tsbPlayRange.Enabled) {
                        toolStripButtonPlayRange_Click(null, null);
                        e.Handled = true;
                    }else if (tsbPlayPoint.Enabled) {
                        toolStripButtonPlayPoint_Click(null, null);
                        e.Handled = true;
                    }
                    break;

                case Keys.F7:
                    if (tsbPause.Enabled) {
                        toolStripButtonPause_Click(null, null);
                        e.Handled = true;
                    }
                    break;

                case Keys.F8:
                    if (tsbStop.Enabled) {
                        toolStripButtonStop_Click(null, null);
                        e.Handled = true;
                    }
                    break;
            }
        }


        //TODO Need to test the insert and delete channel code
        private void HandleChannelKeyPress(KeyEventArgs e) {
            var selectedChannelIndex = GetChannelSortedIndex(SelectedChannel);

            switch (e.KeyCode) {
                case Keys.Insert:
                    e.Handled = true;

                    if (e.Shift) {
                        FillChannel(selectedChannelIndex);
                    }
                    else if (_sequence.Profile == null && _sequence.ChannelCount > 0) {
                        var naturalIndex = _sequence.InsertChannel(selectedChannelIndex);
                        InsertChannelIntoSort(naturalIndex, selectedChannelIndex);
                        ChannelCountChanged();
                    }
                    else {
                        e.Handled = false;
                    }
                    break;

                case Keys.Delete:
                    if (e.Shift) {
                        ClearChannel(selectedChannelIndex);
                    }
                    else if (_sequence.Profile == null && _sequence.ChannelCount > 0 &&
                             (MessageBox.Show(string.Format(Resources.StringFormat_DeleteChannel, SelectedChannel.Name),
                                              Resources.DeleteChannelConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                              DialogResult.Yes)) {
                        _sequence.DeleteChannel(SelectedChannel.Id);
                        DeleteChannelFromSort(selectedChannelIndex);
                        ChannelCountChanged();
                    }
                    e.Handled = true;
                    break;
            }
        }


        private void StandardSequence_Load(object sender, EventArgs e) {
            ToolStripManager.SaveSettings(this, _preferences.XmlDoc.DocumentElement, "reset");
            _preferences.Flush();
            ToolStripManager.LoadSettings(this, _preferences.XmlDoc.DocumentElement);
            var panelArray = new[] {
                toolStripContainer1.TopToolStripPanel, toolStripContainer1.BottomToolStripPanel, toolStripContainer1.LeftToolStripPanel,
                toolStripContainer1.RightToolStripPanel
            };
            var list = new List<string>();
            foreach (var panel in panelArray) {
                foreach (ToolStrip strip in panel.Controls) {
                    _toolStrips[strip.Text] = strip;
                    list.Add(strip.Text);
                }
            }
            list.Sort();

            //this populates the toolstrip menu with the attached toolstrips
            var position = ToolbarInsertPosition;
            foreach (var str in list) {
                var menuItem = new ToolStripMenuItem(str) {Tag = _toolStrips[str], Checked = _toolStrips[str].Visible, CheckOnClick = true};
                menuItem.CheckStateChanged += _toolStripCheckStateChangeHandler;
                toolbarsToolStripMenuItem.DropDownItems.Insert(position++, menuItem);
            }
            _actualLevels = _preferences.GetBoolean("ActualLevels");
            if (_actualLevels) {
                toolStripButtonToggleLevels.Image = Resources.Percent;
                toolStripButtonToggleCellText.Image = Resources.level_Number;
            }
            else {
                toolStripButtonToggleLevels.Image = Resources.number;
                toolStripButtonToggleCellText.Image = Resources.level_Percent;
            }
            UpdateToolbarMenu();
            UpdateLevelDisplay();
        }


        private void subtractionToolStripMenuItem_Click(object sender, EventArgs e) {
            ArithmeticPaste(ArithmeticOperation.Subtraction);
        }


        private void SyncAudioButton() {
            tsbAudio.Checked = _sequence.Audio != null;
            tsbAudio.ToolTipText = (_sequence.Audio != null) ? _sequence.Audio.Name : Resources.AudioButtonAddText;
        }


        /*
                private void textBoxChannelCount_KeyPress(object sender, KeyPressEventArgs keyEvent)
                {
                    if (keyEvent.KeyChar == '\r')
                    {
                        keyEvent.Handled = true;
                        int result = 0;
                        if (int.TryParse(textBoxChannelCount.Text, out result))
                        {
                            if (result < _sequence.ChannelCount)
                            {
                                if (MessageBox.Show("This will reduce the number of channels and potentially lose data.\n\nAccept new channel count?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    SetChannelCount(result);
                                }
                                else
                                {
                                    textBoxChannelCount.Text = _sequence.ChannelCount.ToString();
                                }
                            }
                            else if (result > _sequence.ChannelCount)
                            {
                                SetChannelCount(result);
                            }
                        }
                        else
                        {
                            textBoxChannelCount.Text = _sequence.ChannelCount.ToString();
                            MessageBox.Show("Please provide a valid number.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
        */

        /*
                private void textBoxProgramLength_KeyPress(object sender, KeyPressEventArgs keyEvent)
                {
                    if (keyEvent.KeyChar == '\r')
                    {
                        int heightAddition;
                        int mappedChannel;
                        int pixelLimit;
                        keyEvent.Handled = true;
                        string s = "0";
                        string str2 = string.Empty;
                        string str3 = "0";
                        string text = textBoxProgramLength.Text;
                        int startPosition = text.IndexOf(':');
                        if (startPosition != -1)
                        {
                            s = text.Substring(0, startPosition).Trim();
                            text = text.Substring(startPosition + 1);
                        }
                        startPosition = text.IndexOf('.');
                        if (startPosition != -1)
                        {
                            str3 = text.Substring(startPosition + 1).Trim();
                            text = text.Substring(0, startPosition);
                        }
                        str2 = text;
                        try
                        {
                            heightAddition = int.Parse(s);
                        }
                        catch
                        {
                            heightAddition = 0;
                        }
                        try
                        {
                            mappedChannel = int.Parse(str2);
                        }
                        catch
                        {
                            mappedChannel = 0;
                        }
                        try
                        {
                            pixelLimit = int.Parse(str3);
                        }
                        catch
                        {
                            pixelLimit = 0;
                        }
                        pixelLimit = (pixelLimit + (mappedChannel * 0x3e8)) + (heightAddition * 0xea60);
                        if (pixelLimit == 0)
                        {
                            textBoxProgramLength.Text = TimeString(_sequence.Time);
                            MessageBox.Show("Not a valid format for time.\nUse one of the following:\n\nSeconds\nMinutes:Seconds\nSeconds.Milliseconds\nMinutes:Seconds.Milliseconds", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else if (SetProgramTime(pixelLimit))
                        {
                            IsDirty = true;
                            MessageBox.Show("Sequence length has been updated.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
        */


        private void toggleOutputChannelsToolStripMenuItem_Click(object sender, EventArgs e) {
            _showingOutputs = !_showingOutputs;
            pictureBoxChannels.Refresh();
        }


        private void toolStripButtonAudio_Click(object sender, EventArgs e) {
            var originalAudio = _sequence.Audio;
            var autoSize = _preferences.GetBoolean("EventSequenceAutoSize");
            var soundDevice = _preferences.GetInteger("SoundDevice");

            using (var dialog = new AudioDialog(_sequence, autoSize, soundDevice)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }
            }

            if (originalAudio == _sequence.Audio) {
                return;
            }

            SyncAudioButton();
            SetProgramTime(_sequence.Time);
            pictureBoxGrid.Refresh();
            IsDirty = true;
            ParseAudioWaveform();
            pictureBoxTime.Refresh();
        }


        private void toolStripButtonChangeIntensity_Click(object sender, EventArgs e) {
            using (var dialog = new DrawingIntensityDialog(_sequence, _drawingLevel, _actualLevels)) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    SetDrawingLevel(dialog.SelectedIntensity);
                }
            }
        }


        private void toolStripButtonChannelOutputMask_Click(object sender, EventArgs e) {
            EditSequenceChannelMask();
        }


        private void toolStripButtonCopy_Click(object sender, EventArgs e) {
            CopyCells();
        }


        private void toolStripButtonCut_Click(object sender, EventArgs e) {
            CopyCells();
            TurnCellsOff(Resources.CutText);
        }


        private void toolStripButtonDeleteOrder_Click(object sender, EventArgs e) {
            if (
                MessageBox.Show(string.Format(Resources.StringFormat_DeleteChannelOrder, toolStripComboBoxChannelOrder.Text), Vendor.ProductName,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }

            _sequence.Sorts.Remove((VixenPlus.SortOrder) toolStripComboBoxChannelOrder.SelectedItem);
            toolStripComboBoxChannelOrder.Items.RemoveAt(toolStripComboBoxChannelOrder.SelectedIndex);
            toolStripButtonDeleteOrder.Enabled = false;
            IsDirty = true;
        }


        private void toolStripButtonFindAndReplace_Click(object sender, EventArgs e) {
            var entireSequence = (_selectedCells.Width == 0) || (_selectedCells.Height == 0);
            //if ((_selectedCells.Width == 0) || (_selectedCells.Height == 0)) {
                //MessageBox.Show(Resources.NoSelectionToSearch, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                //return;
            //}

            using (var dialog = new FindAndReplaceDialog(_sequence.MinimumLevel, _sequence.MaximumLevel, _actualLevels,
                                                         entireSequence ? "entire sequence" : "selected cells")) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                var replaceSelection = entireSequence ? new Rectangle(0, 0, _sequence.TotalEventPeriods, _sequence.ChannelCount) : _selectedCells;

                AddUndoItem(replaceSelection, UndoOriginalBehavior.Overwrite, Resources.FindAndReplace);
                var findValue = dialog.FindValue;
                var replaceWithValue = _actualLevels ? dialog.ReplaceWithValue : (byte) Utils.ToValue(dialog.ReplaceWithValue);
                for (var row = replaceSelection.Top; row < replaceSelection.Bottom; row++) {
                    var channel = _channelOrderMapping[row];
                    for (var left = replaceSelection.Left; left < replaceSelection.Right; left++) {
                        var currentValue = _actualLevels
                                               ? _sequence.EventValues[channel, left] : Utils.ToPercentage(_sequence.EventValues[channel, left]);
                        if (currentValue == findValue) {
                            _sequence.EventValues[channel, left] = replaceWithValue;
                        }
                    }
                }
                IsDirty = true;
                pictureBoxGrid.Refresh();
            }
        }


        private void toolStripButtonInsertPaste_Click(object sender, EventArgs e) {
            if (_systemInterface.Clipboard == null) {
                return;
            }

            var rows = _systemInterface.Clipboard.GetLength(Utils.IndexRowsOrHeight);
            var cols = _systemInterface.Clipboard.GetLength(Utils.IndexColsOrWidth);
            var oldColumnOffset = _selectedCells.Left;
            var newColumnOffset = oldColumnOffset + cols;
            var maxPeriods = Math.Min(_sequence.TotalEventPeriods - newColumnOffset, _sequence.TotalEventPeriods - oldColumnOffset);

            for (var row = 0; (row < rows) && ((_selectedCells.Top + row) < _sequence.ChannelCount); row++) {
                var channel = _channelOrderMapping[_selectedCells.Top + row];
                for (var col = maxPeriods - 1; col >= 0; col--) {
                    _sequence.EventValues[channel, newColumnOffset + col] = _sequence.EventValues[channel, oldColumnOffset + col];
                }
            }

            PasteOver();
            AddUndoItem(new Rectangle(_selectedCells.X, _selectedCells.Y, cols, rows), UndoOriginalBehavior.Insertion, Resources.UndoText_InsertPaste);
        }


        private void toolStripButtonIntensity_Click(object sender, EventArgs e) {
            var result = 0;
            var flag = false;
            var top = _selectedCells.Top;
            var left = _selectedCells.Left;
            var isSingleCell = (_selectedCells.Width == 1) && (_selectedCells.Height == 1);

            while (!flag) {
                flag = true;
                if (_actualLevels) {
                    var initialValue = isSingleCell
                                           ? _sequence.EventValues[_channelOrderMapping[top], left].ToString(CultureInfo.InvariantCulture) : @"255";

                    using (var dialog = new TextQueryDialog(Vendor.ProductName, Resources.IntensityLevelPrompt, initialValue)) {
                        if (dialog.ShowDialog() != DialogResult.OK) {
                            return;
                        }
                        if (!int.TryParse(dialog.Response, out result)) {
                            MessageBox.Show(Resources.InvalidNumber, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            flag = false;
                        }
                        if ((result < 0) || (result > 255)) {
                            MessageBox.Show(Resources.InvalidValue, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            flag = false;
                        }
                    }
                }
                else {
                    var initialValue = isSingleCell
                                           ? Utils.ToPercentage(_sequence.EventValues[_channelOrderMapping[top], left]).ToString(
                                               CultureInfo.InvariantCulture) : @"100";

                    using (var dialog = new TextQueryDialog(Vendor.ProductName, Resources.IntensityPercentPrompt, initialValue)) {
                        if (dialog.ShowDialog() != DialogResult.OK) {
                            return;
                        }
                        try {
                            result = Utils.ToValue(Convert.ToSingle(dialog.Response));
                            if ((result < 0) || (result > 255)) {
                                MessageBox.Show(Resources.InvalidPercentage, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                flag = false;
                            }
                        }
                        catch {
                            MessageBox.Show(Resources.InvalidNumber, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            flag = false;
                        }
                    }
                }
            }
            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, Resources.UndoText_Intensity);

            for (var row = top; row < _selectedCells.Bottom; row++) {
                var channel = _channelOrderMapping[row];
                if (!_sequence.Channels[channel].Enabled) {
                    continue;
                }
                for (var col = _selectedCells.Left; col < _selectedCells.Right; col++) {
                    _sequence.EventValues[channel, col] = (byte) result;
                }
            }

            _selectionRectangle.Width = 0;
            pictureBoxGrid.Invalidate(SelectionToRectangle());
        }


        private void toolStripButtonInvert_Click(object sender, EventArgs e) {
            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, Resources.UndoText_Invert);
            var bottom = _selectedCells.Bottom;
            var right = _selectedCells.Right;
            for (var row = _selectedCells.Top; row < bottom; row++) {
                var channel = _channelOrderMapping[row];
                if (!_sequence.Channels[channel].Enabled) {
                    continue;
                }
                for (var col = _selectedCells.Left; col < right; col++) {
                    _sequence.EventValues[channel, col] = (byte) (_sequence.MaximumLevel - _sequence.EventValues[channel, col]);
                }
            }
            _selectionRectangle.Width = 0;
            pictureBoxGrid.Invalidate(SelectionToRectangle());
        }


        private void toolStripButtonLoop_CheckedChanged(object sender, EventArgs e) {
            _executionInterface.SetLoopState(_executionContextHandle, tsbLoop.Checked);
            tsbLoop.BackgroundImage = ToolStripManager.GetBackground(tsbLoop.Checked);
        }


        private void toolStripButtonMirrorHorizontal_Click(object sender, EventArgs e) {
            var buffer = new byte[_selectedCells.Height,_selectedCells.Width];
            for (var row = 0; row < _selectedCells.Height; row++) {
                var channel = _channelOrderMapping[_selectedCells.Top + row];
                var column = 0;
                for (var count = _selectedCells.Width - 1; count >= 0; count--) {
                    buffer[row, column++] = _sequence.EventValues[channel, _selectedCells.Left + count];
                }
            }
            _systemInterface.Clipboard = buffer;
        }


        private void toolStripButtonMirrorVertical_Click(object sender, EventArgs e) {
            var buffer = new byte[_selectedCells.Height,_selectedCells.Width];
            var row = 0;

            for (var count = _selectedCells.Height - 1; count >= 0; count--) {
                var channel = _channelOrderMapping[_selectedCells.Top + count];
                for (var column = 0; column < _selectedCells.Width; column++) {
                    buffer[row, column] = _sequence.EventValues[channel, _selectedCells.Left + column];
                }
                row++;
            }
            _systemInterface.Clipboard = buffer;
        }


        private void toolStripButtonOff_Click(object sender, EventArgs e) {
            TurnCellsOff();
        }


        private void toolStripButtonOn_Click(object sender, EventArgs e) {
            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, Resources.OnText);
            SetSelectedCellValue(_drawingLevel);
            _selectionRectangle.Width = 0;
            pictureBoxGrid.Invalidate(SelectionToRectangle());
        }


        private void toolStripButtonOpaquePaste_Click(object sender, EventArgs e) {
            if (_systemInterface.Clipboard == null) {
                return;
            }

            AddUndoItem(
                new Rectangle(_selectedCells.X, _selectedCells.Y, _systemInterface.Clipboard.GetLength(1), _systemInterface.Clipboard.GetLength(0)),
                UndoOriginalBehavior.Overwrite, Resources.UndoText_OpaquePaste);
            PasteOver();
        }


        private void toolStripButtonPartialRampOff_Click(object sender, EventArgs e) {
            using (var dialog = new RampQueryDialog(_sequence.MinimumLevel, _sequence.MaximumLevel, true, _actualLevels)) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    Ramp(dialog.StartingLevel, dialog.EndingLevel);
                }
            }
        }


        private void toolStripButtonPartialRampOn_Click(object sender, EventArgs e) {
            using (var dialog = new RampQueryDialog(_sequence.MinimumLevel, _sequence.MaximumLevel, false, _actualLevels)) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    Ramp(dialog.StartingLevel, dialog.EndingLevel);
                }
            }
        }


        private void toolStripButtonPause_Click(object sender, EventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) != Utils.ExecutionRunning) {
                return;
            }

            UpdatePlayButtons(8);
            m_positionTimer.Stop();
            _executionInterface.ExecutePause(_executionContextHandle);
            SetEditingState(true);
        }


        private void toolStripButtonStop_Click(object sender, EventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) == Utils.ExecutionStopped) {
                return;
            }
            UpdatePlayButtons(0);
            m_positionTimer.Stop();
            ProgramEnded();
            _executionInterface.ExecuteStop(_executionContextHandle);
            SetEditingState(true);
        }


        private void toolStripButtonPlay_Click(object sender, EventArgs e) {
            int sequencePosition;
            var executionStatus = _executionInterface.EngineStatus(_executionContextHandle, out sequencePosition);
            
            if (executionStatus == Utils.ExecutionRunning) {
                return;
            }
            
            if (executionStatus != Utils.ExecutionPaused) {
                Reset();
            }
            
            if (!_executionInterface.ExecutePlay(_executionContextHandle, sequencePosition, 0)) {
                return;
            }

            UpdatePlayButtons(1);
            m_positionTimer.Start();
            SetEditingState(false);
        }


        private void toolStripButtonPlayPoint_Click(object sender, EventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) == Utils.ExecutionRunning) {
                return;
            }

            SetEditingState(false);
            _executionInterface.ExecutePlay(_executionContextHandle, _selectedCells.Left * _sequence.EventPeriod,
                                            _sequence.TotalEventPeriods * _sequence.EventPeriod);
            UpdatePlayButtons(2);
            m_positionTimer.Start();
        }


        //TODO Remove magic numbers
        private void UpdatePlayButtons(int button) {
            tsbPlay.BackgroundImage = ToolStripManager.GetBackground((button & 0x1) == 0x1);
            tsbPlayPoint.BackgroundImage = ToolStripManager.GetBackground((button & 0x2) == 0x2);
            tsbPlayRange.BackgroundImage = ToolStripManager.GetBackground((button & 0x4) == 0x4);
            tsbPause.BackgroundImage = ToolStripManager.GetBackground((button & 0x8) == 0x8);
            tsbPause.Enabled = tsbStop.Enabled = (button != 0);
        }


        private void toolStripButtonPlayRange_Click(object sender, EventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) == Utils.ExecutionRunning) {
                return;
            }

            SetEditingState(false);
            _executionInterface.ExecutePlay(_executionContextHandle, _selectedCells.Left * _sequence.EventPeriod,
                                            _selectedCells.Right * _sequence.EventPeriod);
            UpdatePlayButtons(4);
            m_positionTimer.Start();
        }


        private void toolStripButtonPlaySpeedHalf_Click(object sender, EventArgs e) {
            SetAudioSpeed(0.5f);
        }


        private void toolStripButtonPlaySpeedNormal_Click(object sender, EventArgs e) {
            SetAudioSpeed(1f);
        }


        private void toolStripButtonPlaySpeedQuarter_Click(object sender, EventArgs e) {
            SetAudioSpeed(0.25f);
        }


        private void toolStripButtonPlaySpeedThreeQuarters_Click(object sender, EventArgs e) {
            SetAudioSpeed(0.75f);
        }


        private void toolStripButtonPlaySpeedVariable_Click(object sender, EventArgs e) {
            SetVariablePlaybackSpeed(toolStripExecutionControl.PointToScreen(new Point(SpeedVariableTsb.Bounds.Right, SpeedVariableTsb.Bounds.Top)));
        }


        private void toolStripButtonRampOff_Click(object sender, EventArgs e) {
            Ramp(_sequence.MaximumLevel, _sequence.MinimumLevel);
        }


        private void toolStripButtonRampOn_Click(object sender, EventArgs e) {
            Ramp(_sequence.MinimumLevel, _sequence.MaximumLevel);
        }


        private void toolStripButtonRandom_Click(object sender, EventArgs e) {
            using (var dialog = new RandomParametersDialog(_sequence.MinimumLevel, _sequence.MaximumLevel, _actualLevels, _showingGradient)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, Resources.UndoText_Random);

                SetSelectedCellValue(_sequence.MinimumLevel);

                var events = dialog.GetEventValues(_selectedCells.Height, _selectedCells.Width);
                for (var row = _selectedCells.Top; row < _selectedCells.Bottom; row ++) {
                    var channel = _channelOrderMapping[row];
                    for (var col = _selectedCells.Left; col < _selectedCells.Right; col++) {
                        _sequence.EventValues[channel, col] = events[row - _selectedCells.Top, col - _selectedCells.Left];
                    }
                }

                _selectionRectangle.Width = 0;
                pictureBoxGrid.Invalidate(SelectionToRectangle());
            }
        }


        private void toolStripButtonRedo_Click(object sender, EventArgs e) {
            Redo();
        }


        private void toolStripButtonRemoveCells_Click(object sender, EventArgs e) {
            if (_selectedCells.Width == 0) {
                return;
            }

            var right = _selectedCells.Right;
            var shiftingColumnPoint = _sequence.TotalEventPeriods - right;
            AddUndoItem(new Rectangle(_selectedCells.Left, _selectedCells.Top, _selectedCells.Width, _selectedCells.Height),
                        UndoOriginalBehavior.Removal, Resources.UndoText_RemoveCells);
            for (var row = 0; row < _selectedCells.Height; row++) {
                var channel = _channelOrderMapping[_selectedCells.Top + row];
                for (var column = 0; column < shiftingColumnPoint; column++) {
                    _sequence.EventValues[channel, right + column - _selectedCells.Width] = _sequence.EventValues[channel, right + column];
                }
                for (var column = right + shiftingColumnPoint - _selectedCells.Width; column < _sequence.TotalEventPeriods; column++) {
                    _sequence.EventValues[channel, column] = _sequence.MinimumLevel;
                }
            }
            pictureBoxGrid.Refresh();
        }


        private void toolStripButtonSave_Click(object sender, EventArgs e) {
            _systemInterface.InvokeSave(this);
        }


        private void toolStripButtonSaveOrder_Click(object sender, EventArgs e) {
            VixenPlus.SortOrder newSortOrder = null;
            using (var dialog = new TextQueryDialog(Resources.ReorderNameHeading, Resources.ReorderNamePrompt, string.Empty)) {
                var dialogResult = DialogResult.No;
                while (dialogResult == DialogResult.No) {
                    if (dialog.ShowDialog() == DialogResult.Cancel) {
                        return;
                    }

                    dialogResult = DialogResult.Yes;
                    foreach (var sortOrder in _sequence.Sorts) {
                        if (sortOrder.Name != dialog.Response) {
                            continue;
                        }

                        if (
                            (dialogResult =
                             MessageBox.Show(Resources.OverwriteNamePrompt, Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) ==
                            DialogResult.Cancel) {
                            return;
                        }
                        newSortOrder = sortOrder;
                        break;
                    }
                }
                if (newSortOrder != null) {
                    newSortOrder.ChannelIndexes.Clear();
                    newSortOrder.ChannelIndexes.AddRange(_channelOrderMapping);
                    toolStripComboBoxChannelOrder.SelectedItem = newSortOrder;
                }
                else {
                    _sequence.Sorts.Add(newSortOrder = new VixenPlus.SortOrder(dialog.Response, _channelOrderMapping));
                    toolStripComboBoxChannelOrder.Items.Insert(toolStripComboBoxChannelOrder.Items.Count - 1, newSortOrder);
                    toolStripComboBoxChannelOrder.SelectedIndex = toolStripComboBoxChannelOrder.Items.Count - 2;
                }
            }
            IsDirty = true;
        }


        private void toolStripButtonShimmerDimming_Click(object sender, EventArgs e) {
            var maxFrequency = (int) _sequence.EventsPerSecond;
            using (var dialog = new EffectFrequencyDialog(Resources.ShimmerPrompt, maxFrequency, _dimmingShimmerGenerator)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, Resources.UndoText_Shimmer);

                var values = new byte[_selectedCells.Height,_selectedCells.Width];
                DimmingShimmerGenerator(values, new[] {dialog.Frequency});
                CopyToEventValues(_selectedCells.Left, _selectedCells.Top, values);
                _selectionRectangle.Width = 0;
                pictureBoxGrid.Invalidate(SelectionToRectangle());
            }
        }


        private void toolStripButtonSparkle_Click(object sender, EventArgs e) {
            var maxFrequency = (int) _sequence.EventsPerSecond;
            using (
                var dialog = new SparkleParamsDialog(maxFrequency, _sparkleGenerator, _sequence.MinimumLevel, _sequence.MaximumLevel, _drawingLevel,
                                                     _actualLevels)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, Resources.UndoText_Sparkle);

                var values = new byte[_selectedCells.Height,_selectedCells.Width];
                SparkleGenerator(values, new[] {dialog.Frequency, dialog.DecayTime, dialog.MinimumIntensity, dialog.MaximumIntensity});
                CopyToEventValues(_selectedCells.Left, _selectedCells.Top, values);
                _selectionRectangle.Width = 0;
                pictureBoxGrid.Invalidate(SelectionToRectangle());
            }
        }


        private void toolStripButtonTestChannels_Click(object sender, EventArgs e) {
            try {
                new TestChannelsDialog(_sequence, _executionInterface).Show();
            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void toolStripButtonTestConsole_Click(object sender, EventArgs e) {
            try {
                new TestConsoleDialog(_sequence, _executionInterface).Show();
            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void toolStripButtonToggleCellText_Click(object sender, EventArgs e) {
            _showCellText = !_showCellText;
            toolStripButtonToggleCellText.BackgroundImage = ToolStripManager.GetBackground(_showCellText);
            pictureBoxGrid.Refresh();
        }


        private void toolStripButtonToggleCrossHairs_Click(object sender, EventArgs e) {
            // Cant check on click here since the paint method depends on the crosshairs flag, so we set it right away.
            toolStripButtonToggleCrossHairs.Checked = !toolStripButtonToggleCrossHairs.Checked;
            toolStripButtonToggleCrossHairs.BackgroundImage = ToolStripManager.GetBackground(toolStripButtonToggleCrossHairs.Checked);

            pictureBoxGrid.Invalidate(new Rectangle((_mouseTimeCaret - hScrollBar1.Value) * _gridColWidth, 0, _gridColWidth, pictureBoxGrid.Height));
            pictureBoxGrid.Invalidate(new Rectangle(0, (_mouseChannelCaret - vScrollBar1.Value) * _gridRowHeight, pictureBoxGrid.Width, _gridRowHeight));
            pictureBoxGrid.Update();
        }


        private void toolStripButtonToggleLevels_Click(object sender, EventArgs e) {
            _actualLevels = !_actualLevels;
            _preferences.SetBoolean("ActualLevels", _actualLevels);
            toolStripButtonToggleCellText.Image = _actualLevels ? Resources.level_Number : Resources.level_Percent;
            UpdateLevelDisplay();
            pictureBoxGrid.Refresh();
        }


        private void toolStripButtonToggleRamps_Click(object sender, EventArgs e) {
            _showingGradient = !_showingGradient;
            _preferences.SetBoolean("BarLevels", !_showingGradient);
            pictureBoxGrid.Refresh();
        }


        private void toolStripButtonTransparentPaste_Click(object sender, EventArgs e) {
            if (_systemInterface.Clipboard == null) {
                return;
            }

            AddUndoItem(
                new Rectangle(_selectedCells.X, _selectedCells.Y, _systemInterface.Clipboard.GetLength(1), _systemInterface.Clipboard.GetLength(0)),
                UndoOriginalBehavior.Overwrite, Resources.UndoText_TransparentPaste);
            var clipboard = _systemInterface.Clipboard;
            var rows = clipboard.GetLength(Utils.IndexRowsOrHeight);
            var columns = clipboard.GetLength(Utils.IndexColsOrWidth);
            for (var row = 0; (row < rows) && (_selectedCells.Top + row < _sequence.ChannelCount); row++) {
                var channel = _channelOrderMapping[_selectedCells.Top + row];
                for (var col = 0; (col < columns) && (_selectedCells.Left + col < _sequence.TotalEventPeriods); col++) {
                    var newValue = clipboard[row, col];
                    if (newValue > _sequence.MinimumLevel) {
                        _sequence.EventValues[channel, _selectedCells.Left + col] = newValue;
                    }
                }
            }
            IsDirty = true;
            pictureBoxGrid.Refresh();
        }


        private void toolStripButtonUndo_Click(object sender, EventArgs e) {
            Undo();
        }


        private void toolStripButtonWaveform_Click(object sender, EventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) == Utils.ExecutionRunning) {
                return;
            }

            if ((_waveformPixelData == null) || (_waveformPcmData == null)) {
                ParseAudioWaveform();
            }

            if (toolStripButtonWaveform.Checked) {
                pictureBoxTime.Height = 120;
                splitContainer2.Panel1MinSize = 120;
                splitContainer2.Size = new Size(0, 120);
                splitContainer2.SplitterDistance = 120;
                splitContainer2.IsSplitterFixed = false;
                EnableWaveformButton();
            }
            else {
                pictureBoxTime.Height = 60;
                splitContainer2.Panel1MinSize = 60;
                splitContainer2.Size = new Size(0, 60);
                splitContainer2.SplitterDistance = 60;
                splitContainer2.IsSplitterFixed = true;
            }

            pictureBoxTime.Refresh();
            pictureBoxChannels.Refresh();
        }


        private void toolStripComboBoxChannelOrder_SelectedIndexChanged(object sender, EventArgs e) {
            if (toolStripComboBoxChannelOrder.SelectedIndex == -1) {
                return;
            }

            if ((_sequence.Profile != null) && (toolStripComboBoxChannelOrder.SelectedIndex == 0)) {
                toolStripComboBoxChannelOrder.SelectedIndex = -1;
                MessageBox.Show(Resources.UseProfileToEditChannels, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                if (toolStripComboBoxChannelOrder.SelectedIndex == 0) {
                    if (_sequence.ChannelCount == 0) {
                        MessageBox.Show(Resources.NoChannelsToReorder, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    toolStripButtonDeleteOrder.Enabled = false;
                    toolStripComboBoxChannelOrder.SelectedIndex = -1;
                    using (var dialog = new ChannelOrderDialog(_sequence.Channels, _channelOrderMapping)) {
                        if (dialog.ShowDialog() == DialogResult.OK) {
                            _channelOrderMapping.Clear();
                            foreach (var channel in dialog.ChannelMapping) {
                                _channelOrderMapping.Add(_sequence.Channels.IndexOf(channel));
                            }
                            IsDirty = true;
                        }
                    }
                }
                else if (toolStripComboBoxChannelOrder.SelectedIndex == (toolStripComboBoxChannelOrder.Items.Count - 1)) {
                    toolStripButtonDeleteOrder.Enabled = false;
                    toolStripComboBoxChannelOrder.SelectedIndex = -1;
                    _channelOrderMapping.Clear();
                    for (var channel = 0; channel < _sequence.ChannelCount; channel++) {
                        _channelOrderMapping.Add(channel);
                    }
                    _sequence.LastSort = -1;
                    if (_sequence.Profile == null) {
                        IsDirty = true;
                    }
                }
                else {
                    _channelOrderMapping.Clear();
                    _channelOrderMapping.AddRange(((VixenPlus.SortOrder) toolStripComboBoxChannelOrder.SelectedItem).ChannelIndexes);
                    _sequence.LastSort = toolStripComboBoxChannelOrder.SelectedIndex - 1;
                    toolStripButtonDeleteOrder.Enabled = true;
                    if (_sequence.Profile == null) {
                        IsDirty = true;
                    }
                }
                pictureBoxChannels.Refresh();
                pictureBoxGrid.Refresh();
            }
        }


        private void toolStripComboBoxColumnZoom_SelectedIndexChanged(object sender, EventArgs e) {
            if (toolStripComboBoxColumnZoom.SelectedIndex != -1) {
                UpdateColumnWidth();
            }
        }


        private void toolStripComboBoxRowZoom_SelectedIndexChanged(object sender, EventArgs e) {
            if (toolStripComboBoxRowZoom.SelectedIndex != -1) {
                UpdateRowHeight();
            }
        }


        private void toolStripDropDownButtonPlugins_Click(object sender, EventArgs e) {
            using (var dialog = new PluginListDialog(_sequence)) {
                if (dialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                toolStripDropDownButtonPlugins.DropDownItems.Clear();
                var tagValue = 0;
                foreach (object[] objArray in dialog.MappedPluginList) {
                    var item = new ToolStripMenuItem((string) objArray[0])
                    {Checked = (bool) objArray[1], CheckOnClick = true, Tag = tagValue.ToString(CultureInfo.InvariantCulture)};
                    item.CheckedChanged += plugInItem_CheckedChanged;
                    tagValue++;
                    toolStripDropDownButtonPlugins.DropDownItems.Add(item);
                }
                if (toolStripDropDownButtonPlugins.DropDownItems.Count > 0) {
                    toolStripDropDownButtonPlugins.DropDownItems.Add("-");
                    toolStripDropDownButtonPlugins.DropDownItems.Add(Resources.SelectAll, null, selectAllToolStripMenuItem_Click);
                    toolStripDropDownButtonPlugins.DropDownItems.Add(Resources.UnselectAll, null, unselectAllToolStripMenuItem_Click);
                }
                IsDirty = true;
            }
        }


        private void toolStripItem_CheckStateChanged(object sender, EventArgs e) {
            var item = (ToolStripMenuItem) sender;
            var tag = (ToolStrip) item.Tag;
            tag.Visible = item.Checked;
        }


        private void toolStripMenuItemPasteAnd_Click(object sender, EventArgs e) {
            BooleanPaste(BooleanOperation.AND);
        }


        private void toolStripMenuItemPasteNand_Click(object sender, EventArgs e) {
            BooleanPaste(BooleanOperation.NAND);
        }


        private void toolStripMenuItemPasteNor_Click(object sender, EventArgs e) {
            BooleanPaste(BooleanOperation.NOR);
        }


        private void toolStripMenuItemPasteOr_Click(object sender, EventArgs e) {
            BooleanPaste(BooleanOperation.OR);
        }


        private void toolStripMenuItemPasteXnor_Click(object sender, EventArgs e) {
            BooleanPaste(BooleanOperation.XNOR);
        }


        private void toolStripMenuItemPasteXor_Click(object sender, EventArgs e) {
            BooleanPaste(BooleanOperation.XOR);
        }


        private void TurnCellsOff(string actionMessage = "Off") {
            if (actionMessage == "Off") actionMessage = Resources.OffText;
            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, actionMessage);
            SetSelectedCellValue(_sequence.MinimumLevel);
            _selectionRectangle.Width = 0;
            pictureBoxGrid.Invalidate(SelectionToRectangle());
        }


        private void Undo() {
            if (_undoStack.Count == 0) {
                return;
            }

            var undo = (UndoItem) _undoStack.Pop();
            toolStripButtonUndo.Enabled = undoToolStripMenuItem.Enabled = _undoStack.Count > 0;

            if (_undoStack.Count > 0) {
                IsDirty = true;
            }

            var width = undo.Data.GetLength(Utils.IndexColsOrWidth);
            var height = undo.Data.GetLength(Utils.IndexRowsOrHeight);
            var redo = new UndoItem(undo.Location, GetAffectedBlockData(new Rectangle(undo.Location.X, undo.Location.Y, width, height)), undo.Behavior,
                                    _sequence, _channelOrderMapping, undo.OriginalAction);

            switch (undo.Behavior) {
                case UndoOriginalBehavior.Overwrite:
                    DisjointedOverwrite(undo.Location.X, undo.Data, undo.ReferencedChannels);
                    pictureBoxGrid.Refresh();
                    break;

                case UndoOriginalBehavior.Removal:
                    DisjointedInsert(undo.Location.X, width, height, undo.ReferencedChannels);
                    DisjointedOverwrite(undo.Location.X, undo.Data, undo.ReferencedChannels);
                    pictureBoxGrid.Refresh();
                    break;

                case UndoOriginalBehavior.Insertion:
                    DisjointedRemove(undo.Location.X, width, height, undo.ReferencedChannels);
                    pictureBoxGrid.Refresh();
                    break;
            }

            UpdateUndoText();
            _redoStack.Push(redo);
            toolStripButtonRedo.Enabled = redoToolStripMenuItem.Enabled = true;
            UpdateRedoText();
        }


        private void unselectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (ToolStripItem item in toolStripDropDownButtonPlugins.DropDownItems) {
                if (!(item is ToolStripMenuItem)) {
                    break;
                }
                ((ToolStripMenuItem) item).Checked = false;
            }
        }


        private void UpdateColumnWidth() {
            var num = (toolStripComboBoxColumnZoom.SelectedIndex + 1) * 0.1;
            _gridColWidth = (int) (_preferences.GetInteger("MaxColumnWidth") * num);
            HScrollCheck();
            ParseAudioWaveform();
            pictureBoxGrid.Refresh();
            pictureBoxTime.Refresh();
        }


        private void UpdateGrid(Graphics g, Rectangle clipRect) {
            if (_sequence.ChannelCount == 0) {
                return;
            }

            var fontSize = (_gridColWidth <= 20) ? 5 : ((_gridColWidth <= 25) ? 6 : ((_gridColWidth < 50) ? 8 : 10));
            using (var font = new Font(Font.FontFamily, fontSize))
            using (var brush = new SolidBrush(Color.White)) {
                var initialX = (clipRect.X / _gridColWidth * _gridColWidth) + 1;
                var startEvent = (clipRect.X / _gridColWidth) + hScrollBar1.Value;
                var channelIndex = (clipRect.Y / _gridRowHeight) + vScrollBar1.Value;

                var y = (clipRect.Y / _gridRowHeight * _gridRowHeight) + 1;
                while ((y < clipRect.Bottom) && (channelIndex < _sequence.ChannelCount)) {
                    var currentChannel = _channelOrderMapping[channelIndex];
                    var channel = _sequence.Channels[currentChannel];
                    var x = initialX;

                    for (var @event = startEvent; (x < clipRect.Right) && (@event < _sequence.TotalEventPeriods); @event++) {
                        if (_showingGradient) {
                            brush.Color = GetGradientColor(_gridBackBrush.Color, channel.Color, _sequence.EventValues[currentChannel, @event]);
                            g.FillRectangle(brush, x, y, _gridColWidth - 1, _gridRowHeight - 1);
                        }
                        else {
                            var height = ((_gridRowHeight - 1) * _sequence.EventValues[currentChannel, @event]) / 255;
                            g.FillRectangle(channel.Brush ?? _channelBackBrush, x, ((y + _gridRowHeight) - 1) - height, _gridColWidth - 1, height);
                        }

                        string cellIntensity;
                        if (_showCellText && (GetCellIntensity(@event, channelIndex, out cellIntensity) > 0)) {
                            g.DrawString(cellIntensity, font, Brushes.Black, new RectangleF(x, y, (_gridColWidth - 1), (_gridRowHeight - 1)));
                        }
                        x += _gridColWidth;
                    }
                    y += _gridRowHeight;
                    channelIndex++;
                }
            }
        }


        private void UpdateLevelDisplay() {
            SetDrawingLevel(_drawingLevel);
            if (_actualLevels) {
                toolStripButtonToggleLevels.Image = Resources.Percent;
                toolStripButtonToggleLevels.ToolTipText = Resources.ToolTip_IntensityPercent;
            }
            else {
                toolStripButtonToggleLevels.Image = Resources.number;
                toolStripButtonToggleLevels.ToolTipText = Resources.ToolTip_IntensityLevel;
            }
            m_intensityAdjustDialog.ActualLevels = _actualLevels;
        }


        private void UpdateToolbarMenu() {
            smallToolStripMenuItem.Checked = (ToolStripManager.IconSize == ToolStripManager.IconSizeSmall);
            mediumToolStripMenuItem.Checked = (ToolStripManager.IconSize == ToolStripManager.IconSizeMedium);
            largeToolStripMenuItem.Checked = (ToolStripManager.IconSize == ToolStripManager.IconSizeLarge);
            lockToolbarToolStripMenuItem.Checked = ToolStripManager.Locked;
        }


        private void UpdatePositionLabel(Rectangle rect, bool zeroWidthIsValid) {
            var startMills = rect.Left * _sequence.EventPeriod;
            var startTime = Utils.TimeFormatWithMills(startMills);
            if (rect.Width > 1) {
                var endMills = (rect.Right - 1) * _sequence.EventPeriod;
                var endTime = Utils.TimeFormatWithMills(endMills);
                var elapsedTime = Utils.TimeFormatWithMills(endMills - startMills + _sequence.EventPeriod);
                labelPosition.Text = string.Format("{0} - {1}\n({2})", startTime, endTime, elapsedTime);
                UpdateFollowMouse();
            }
            else if (((rect.Width == 0) && zeroWidthIsValid) || (rect.Width == 1)) {
                labelPosition.Text = startTime;
                UpdateFollowMouse();
            }
            else {
                labelPosition.Text = string.Empty;
            }
        }


        private void UpdateFollowMouse() {
            var rowCount = _selectedCells.Height;
            lblFollowMouse.Text = labelPosition.Text + Environment.NewLine + rowCount + @" " +
                                  (rowCount == 1 ? Resources.Channel : Resources.Channels);
            
            var position = Cursor.Position;
            position.X -= lblFollowMouse.Width;
            position.X = ((position.X / _gridColWidth) + 1) * _gridColWidth;
            position.Y = ((position.Y / _gridRowHeight) + 1) * _gridRowHeight;
            position = pictureBoxGrid.PointToClient(position);

            var tl = (new Point(pictureBoxGrid.ClientRectangle.Left, pictureBoxGrid.ClientRectangle.Top));
            var br = (new Point(pictureBoxGrid.ClientRectangle.Right, pictureBoxGrid.ClientRectangle.Bottom));
            if (position.Y + lblFollowMouse.Height > br.Y) {
                position.Y = br.Y - lblFollowMouse.Height;
            } else
                if (position.Y < tl.Y) {
                    position.Y = 0;
                }

            if (position.X < tl.X) {
                position.X = 0;
            }

            lblFollowMouse.Location = position;
        }


        private void UpdateProgress() {
            var x = (_previousPosition - hScrollBar1.Value) * _gridColWidth;
            pictureBoxTime.Invalidate(new Rectangle(x, pictureBoxTime.Height - 35, _gridColWidth + _gridColWidth, 15));
            pictureBoxGrid.Invalidate(new Rectangle(x, 0, _gridColWidth + _gridColWidth, pictureBoxGrid.Height));
        }


        private void UpdateRedoText() {
            if (_redoStack.Count > 0) {
                toolStripButtonRedo.ToolTipText = redoToolStripMenuItem.Text = Resources.RedoText + @": " + _redoStack.Peek();
            }
            else {
                toolStripButtonRedo.ToolTipText = redoToolStripMenuItem.Text = Resources.RedoTextEmpty;
            }
            toolStripEditing.SuspendLayout();
            toolStripButtonRedo.Visible = false;
            toolStripButtonRedo.Visible = true;
            toolStripEditing.ResumeLayout(true);
        }


        private void UpdateRowHeight() {
            var fontSize = 0f;

            if (toolStripComboBoxRowZoom.SelectedIndex >= 6 && !(Equals(_channelNameFont.Size, 8f))) {
                fontSize = 8f;
            }
            else if (toolStripComboBoxRowZoom.SelectedIndex <= 3 && !(Equals(_channelNameFont.Size, 5f))) {
                fontSize = 5f;
            }
            else if (!(Equals(_channelNameFont.Size, 7f))) {
                fontSize = 7f;
            }

            if (fontSize > 0f) {
                _channelNameFont.Dispose();
                _channelNameFont = new Font("Arial", fontSize);
                _channelStrikeoutFont.Dispose();
                _channelStrikeoutFont = new Font("Arial", fontSize, FontStyle.Strikeout);
            }

            var num = (toolStripComboBoxRowZoom.SelectedIndex + 1) * 0.1;
            _gridRowHeight = (int) (_preferences.GetInteger("MaxRowHeight") * num);
            VScrollCheck();
            pictureBoxGrid.Refresh();
            pictureBoxChannels.Refresh();
        }


        private void UpdateUndoText() {
            if (_undoStack.Count > 0) {
                toolStripButtonUndo.ToolTipText = undoToolStripMenuItem.Text = Resources.UndoText + @": " + _undoStack.Peek();
            }
            else {
                toolStripButtonUndo.ToolTipText = undoToolStripMenuItem.Text = Resources.UndoTextEmpty;
            }
            toolStripEditing.SuspendLayout();
            toolStripButtonUndo.Visible = false;
            toolStripButtonUndo.Visible = true;
            toolStripEditing.ResumeLayout(true);
        }


        private void vScrollBar1_ValueChanged(object sender, EventArgs e) {
            pictureBoxGrid.Refresh();
            pictureBoxChannels.Refresh();
        }


        private void VScrollCheck() {
            _visibleRowCount = pictureBoxGrid.Height / _gridRowHeight;
            vScrollBar1.Maximum = _sequence.ChannelCount - 1;
            vScrollBar1.LargeChange = _visibleRowCount;
            vScrollBar1.Enabled = _visibleRowCount < _sequence.ChannelCount;
            if (!vScrollBar1.Enabled) {
                vScrollBar1.Value = vScrollBar1.Minimum;
            }
            else if ((vScrollBar1.Value + _visibleRowCount) > _sequence.ChannelCount) {
                _selectedRange.Y += _visibleRowCount - _sequence.ChannelCount;
                _selectedCells.Y += _visibleRowCount - _sequence.ChannelCount;
                vScrollBar1.Value = _sequence.ChannelCount - _visibleRowCount;
            }
            if (vScrollBar1.Maximum >= 0) {
                if (vScrollBar1.Value == -1) {
                    vScrollBar1.Value = 0;
                }
                if (vScrollBar1.Minimum == -1) {
                    vScrollBar1.Minimum = 0;
                }
            }
        }


        private void xToolStripMenuItem2_Click(object sender, EventArgs e) {
            SetAudioSpeed(0.75f);
        }


        public override string Author {
            get { return "Vixen and VixenPlus Developers"; }
        }

        public override string Description {
            get { return "Vixen+ sequence user interface"; }
        }

        public override string FileExtension {
            get { return @"." + Vendor.SequenceExtension; }
        }

        public override string FileTypeDescription {
            get { return "Vixen/Vixen+ sequence"; }
        }

        private VixenPlus.Channel SelectedChannel {
            get { return _selectedChannel; }
            set {
                if (_selectedChannel == value) {
                    return;
                }

                var selectedChannel = _selectedChannel;
                _selectedChannel = value;
                if (selectedChannel != null) {
                    pictureBoxChannels.Invalidate(GetChannelNameRect(selectedChannel));
                }
                if (_selectedChannel != null) {
                    pictureBoxChannels.Invalidate(GetChannelNameRect(_selectedChannel));
                }
                pictureBoxChannels.Update();
            }
        }

        public override EventSequence Sequence {
            get { return _sequence; }
            set { _sequence = value; }
        }

        private enum ArithmeticOperation {
            Addition,
            Subtraction,
            Scale,
            Min,
            Max
        }

        private enum BooleanOperation {
            OR,
            AND,
            XOR,
            NOR,
            NAND,
            XNOR
        }


        private void smallToolStripMenuItem_Click(object sender, EventArgs e) {
            ResizeToolStrips(ToolStripManager.IconSizeSmall);
        }


        private void mediumToolStripMenuItem_Click(object sender, EventArgs e) {
            ResizeToolStrips(ToolStripManager.IconSizeMedium);
        }


        private void largeToolStripMenuItem_Click(object sender, EventArgs e) {
            ResizeToolStrips(ToolStripManager.IconSizeLarge);
        }


        private void ResizeToolStrips(int widthAndHeight) {
            ToolStripManager.IconSize = widthAndHeight;
            ToolStripManager.ResizeToolStrips(this);
            UpdateToolbarMenu();
        }


        private void lockToolbarToolStripMenuItem_Click(object sender, EventArgs e) {
            var style = lockToolbarToolStripMenuItem.Checked ? ToolStripGripStyle.Hidden : ToolStripGripStyle.Visible;

            ToolStripManager.Locked = lockToolbarToolStripMenuItem.Checked;

            toolStripDisplaySettings.GripStyle = style;
            toolStripEditing.GripStyle = style;
            toolStripEffect.GripStyle = style;
            toolStripExecutionControl.GripStyle = style;
            toolStripSequenceSettings.GripStyle = style;
            toolStripText.GripStyle = style;
        }


        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) == Utils.ExecutionRunning) {
                return;
            }

            Cursor.Current = Cursors.Default;
            pictureBoxTime.Height = e.SplitY;
            pictureBoxTime.Refresh();
            pictureBoxChannels.Refresh();
            pictureBoxGrid.Refresh();
        }


        private void splitContainer2_SplitterMoving(object sender, SplitterCancelEventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) == Utils.ExecutionRunning) {
                e.Cancel = true;
                return;
            }

            Cursor.Current = Cursors.HSplit;
        }


        private void splitContainer1_SplitterMoving(object sender, SplitterCancelEventArgs e) {
            Cursor.Current = Cursors.VSplit;
        }


        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) {
            Cursor.Current = Cursors.Default;
        }


        private void pictureBoxTime_DoubleClick(object sender, EventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) != Utils.ExecutionStopped) {
                return;
            }

            toolStripButtonWaveform.Checked = !toolStripButtonWaveform.Checked;
            toolStripButtonWaveform_Click(null, null);
        }


        private void openSequenceTsb_Click(object sender, EventArgs e) {
            SendKeys.Send("^o");
        }

        private void tsbSaveAs_Click(object sender, EventArgs e) {
            SendKeys.Send("^a");
            //_systemInterface.InstantiateForm()
        }

        private void profileToolStripLabel_Click(object sender, EventArgs e) {
            if (_sequence.Profile == null) {
                return;
            }

            using (var dialog = new ProfileManagerDialog(_sequence.Profile)) {
                if ((dialog.ShowDialog() == DialogResult.OK) &&
                    (MessageBox.Show(Resources.AttachToNewProfile, Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                     DialogResult.Yes)) {
                         SetProfile(_sequence.Profile);
                }
            }
        }

        private void newSeqTsb_Click(object sender, EventArgs e) {
            var tsi = new ToolStripMenuItem {Tag = new StandardSequence()};
            _systemInterface.InvokeNew(tsi);
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e) {
            _selectedCells.Width = _selectedCells.Height = 0;
            pictureBoxGrid.Refresh();
        }
    }
}