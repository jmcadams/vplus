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
using VixenPlus;
using VixenPlus.Dialogs;

namespace VixenEditor {
    public partial class StandardSequence : UIBase {
        private bool _actualLevels;
        private AffectGridDelegate _affectGridDelegate;
        //private readonly Bitmap _arrowBitmap;
        private bool _autoScrolling;
        private readonly int[] _bookmarks;
        private SolidBrush _channelBackBrush;
        private readonly SolidBrush _channelCaretBrush;
        private Font _channelNameFont;
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
        private int _periodPixelWidth;
        private EventHandler _pluginCheckHandler;
        private int _position;
        private readonly SolidBrush _positionBrush;
        private Preference2 _preferences;
        private int _previousPosition;
        //private int _printingChannelIndex;
        private List<VixenPlus.Channel> _printingChannelList;
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
        private int _waveform100PercentAmplitude;
        private int _waveformMaxAmplitude;
        private readonly int _waveformOffset;
        private uint[] _waveformPcmData;
        private uint[] _waveformPixelData;
        private PrintDocument _printDocument;


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
            _timeFont = new Font("Arial", 8f);
            _selectedChannel = null;
            _currentlyEditingChannel = null;
            _editingChannelSortedIndex = 0;
            _gridGraphics = null;
            _selectedRange = new Rectangle();
            _periodPixelWidth = 30;
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
            _waveformOffset = 0x24;
            _showingOutputs = false;
            _selectedLineIndex = 0;
            //_arrowBitmap = null;
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
            if (blockAffected.Width != 0) {
                byte[,] affectedBlockData = GetAffectedBlockData(blockAffected);
                _undoStack.Push(new UndoItem(blockAffected.Location, affectedBlockData, behavior, _sequence, _channelOrderMapping, originalAction));
                toolStripButtonUndo.Enabled = undoToolStripMenuItem.Enabled = true;
                UpdateUndoText();
                toolStripButtonRedo.Enabled = redoToolStripMenuItem.Enabled = false;
                _redoStack.Clear();
                UpdateRedoText();
                IsDirty = true;
            }
        }


        /*
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
        */


        private void AffectGrid(int startRow, int startCol, byte[,] values) {
            AddUndoItem(new Rectangle(startCol, startRow, values.GetLength(1), values.GetLength(0)), UndoOriginalBehavior.Overwrite, "Copy Channel Data");
            for (int i = 0; i < values.GetLength(0); i++) {
                int num = _channelOrderMapping[startRow + i];
                for (int j = 0; j < values.GetLength(1); j++) {
                    _sequence.EventValues[num, startCol + j] = values[i, j];
                }
            }
            pictureBoxGrid.Refresh();
            IsDirty = true;
        }


        private void allChannelsToFullIntensityForThisEventToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_selectedEventIndex != -1) {
                var blockAffected = new Rectangle(_selectedEventIndex, 0, 1, _sequence.ChannelCount);
                AddUndoItem(blockAffected, UndoOriginalBehavior.Overwrite,"On");
                for (int i = 0; i < _sequence.ChannelCount; i++) {
                    _sequence.EventValues[_channelOrderMapping[i], _selectedEventIndex] = _drawingLevel;
                }
                InvalidateRect(blockAffected);
            }
        }


        private void allEventsToFullIntensityToolStripMenuItem_Click(object sender, EventArgs e) {
            FillChannel(_selectedLineIndex);
        }


        private void ArithmeticPaste(ArithmeticOperation operation) {
            if (_systemInterface.Clipboard != null) {
                AddUndoItem(
                    new Rectangle(_selectedCells.X, _selectedCells.Y, _systemInterface.Clipboard.GetLength(1),
                                  _systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite,"Arithmetic Paste");
                var clipboard = _systemInterface.Clipboard;
                var length = clipboard.GetLength(0);
                var num3 = clipboard.GetLength(1);
                for (var i = 0; (i < length) && ((_selectedCells.Top + i) < _sequence.ChannelCount); i++) {
                    var num4 = _channelOrderMapping[_selectedCells.Top + i];
                    for (var j = 0; (j < num3) && ((_selectedCells.Left + j) < _sequence.TotalEventPeriods); j++) {
                        var num5 = _sequence.EventValues[num4, _selectedCells.Left + j];
                        switch (operation) {
                            case ArithmeticOperation.Addition:
                                _sequence.EventValues[num4, _selectedCells.Left + j] =
                                    (byte) Math.Min(num5 + clipboard[i, j], _sequence.MaximumLevel);
                                break;

                            case ArithmeticOperation.Subtraction:
                                _sequence.EventValues[num4, _selectedCells.Left + j] =
                                    (byte) Math.Max(num5 - clipboard[i, j], _sequence.MinimumLevel);
                                break;

                            case ArithmeticOperation.Scale:
                                _sequence.EventValues[num4, _selectedCells.Left + j] =
                                    (byte) Math.Max(Math.Min(num5 * (clipboard[i, j] / 255f), _sequence.MaximumLevel), _sequence.MinimumLevel);
                                break;

                            case ArithmeticOperation.Min:
                                _sequence.EventValues[num4, _selectedCells.Left + j] = Math.Max(Math.Min(clipboard[i, j], num5),
                                                                                                  _sequence.MinimumLevel);
                                break;

                            case ArithmeticOperation.Max:
                                _sequence.EventValues[num4, _selectedCells.Left + j] = Math.Min(Math.Max(clipboard[i, j], num5),
                                                                                                  _sequence.MaximumLevel);
                                break;
                        }
                        _sequence.EventValues[num4, _selectedCells.Left + j] = Math.Min(_sequence.EventValues[num4, _selectedCells.Left + j],
                                                                                          _sequence.MaximumLevel);
                        _sequence.EventValues[num4, _selectedCells.Left + j] = Math.Max(_sequence.EventValues[num4, _selectedCells.Left + j],
                                                                                          _sequence.MinimumLevel);
                    }
                }
                IsDirty = true;
                pictureBoxGrid.Refresh();
            }
        }


        private void ArrayToCells(byte[,] array) {
            var length = array.GetLength(0);
            var num3 = array.GetLength(1);
            for (var i = 0; (i < length) && ((_selectedCells.Top + i) < _sequence.ChannelCount); i++) {
                var num4 = _channelOrderMapping[_selectedCells.Top + i];
                for (var j = 0; (j < num3) && ((_selectedCells.Left + j) < _sequence.TotalEventPeriods); j++) {
                    _sequence.EventValues[num4, _selectedCells.Left + j] = array[i, j];
                }
            }
            IsDirty = true;
        }


        /*
                private void AssignChannelArray(List<VixenPlus.Channel> channels)
                {
                    _sequence.Channels = channels;
                    textBoxChannelCount.Text = _sequence.ChannelCount.ToString(CultureInfo.InvariantCulture);
                    VScrollCheck();
                }
        */


        private void attachSequenceToToolStripMenuItem_Click(object sender, EventArgs e) {
            openFileDialog1.Filter = @"Profile | *.pro";
            openFileDialog1.DefaultExt = @"pro";
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

            AddUndoItem(new Rectangle(left, top, width, height), UndoOriginalBehavior.Overwrite, "Boolean Paste");

            for (var row = 0; (row < height) && (top + row < _sequence.ChannelCount); row++) {
                var currentRow = _channelOrderMapping[top + row];
                for (var col = 0; (col < width) && ((left + col) < _sequence.TotalEventPeriods); col++) {
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


        private void BresenhamLine(Rectangle rect, byte[] brush) {
            int iterations;
            int diff;
            int negDiffIncr;
            int posDiffIncr;
            int negDiffLeftIncr;
            int posDiffLeftIncr;
            int negDiffTopIncr;
            int posDiffTopIncr;
            var width = Math.Abs(rect.Width);
            var height = Math.Abs(rect.Height);
            if (width >= height) {
                iterations = width + 1;
                diff = (height << 1) - width;
                negDiffIncr = height << 1;
                posDiffIncr = (height - width) << 1;
                negDiffLeftIncr = 1;
                posDiffLeftIncr = 1;
                negDiffTopIncr = 0;
                posDiffTopIncr = 1;
            }
            else {
                iterations = height + 1;
                diff = (width << 1) - height;
                negDiffIncr = width << 1;
                posDiffIncr = (width - height) << 1;
                negDiffLeftIncr = 0;
                posDiffLeftIncr = 1;
                negDiffTopIncr = 1;
                posDiffTopIncr = 1;
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
                //int num16 = left;
                var columns = Math.Min(left + brush.Length, _sequence.TotalEventPeriods) - left;
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
            int iterations;
            int diff;
            int negDiffIncr;
            int posDiffIncr;
            int negDiffLeftIncr;
            int posDiffLeftIncr;
            int negDiffTopIncr;
            int posDiffTopIncr;
            var width = Math.Abs(rect.Width);
            var height = Math.Abs(rect.Height);
            if (width >= height) {
                iterations = width + 1;
                diff = (height << 1) - width;
                negDiffIncr = height << 1;
                posDiffIncr = (height - width) << 1;
                negDiffLeftIncr = 1;
                posDiffLeftIncr = 1;
                negDiffTopIncr = 0;
                posDiffTopIncr = 1;
            }
            else {
                iterations = height + 1;
                diff = (width << 1) - height;
                negDiffIncr = width << 1;
                posDiffIncr = (width - height) << 1;
                negDiffLeftIncr = 0;
                posDiffLeftIncr = 1;
                negDiffTopIncr = 1;
                posDiffTopIncr = 1;
            }
            if (rect.Left > rect.Right) {
                negDiffLeftIncr = -negDiffLeftIncr;
                posDiffLeftIncr = -posDiffLeftIncr;
            }
            if (rect.Top > rect.Bottom) {
                negDiffTopIncr = -negDiffTopIncr;
                posDiffTopIncr = -posDiffTopIncr;
            }
            int left = rect.Left;
            int top = rect.Top;
            for (var i = 0; i < iterations; i++) {
                _sequence.EventValues[_channelOrderMapping[top], left] = _drawingLevel;
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


        private Rectangle CellsToPixels(Rectangle relativeCells) {
            return new Rectangle {
                                     X = (Math.Min(relativeCells.Left, relativeCells.Right) * _periodPixelWidth) + 1,
                                     Y = (Math.Min(relativeCells.Top, relativeCells.Bottom) * _gridRowHeight) + 1,
                                     Width = ((Math.Abs(relativeCells.Width) + 1) * _periodPixelWidth) - 1,
                                     Height = ((Math.Abs(relativeCells.Height) + 1) * _gridRowHeight) - 1
                                 };
        }


        private bool ChannelClickValid() {
            var flag = false;
            if (pictureBoxChannels.PointToClient(MousePosition).Y > pictureBoxTime.Height) {
                _selectedLineIndex = vScrollBar1.Value +
                                     ((pictureBoxChannels.PointToClient(MousePosition).Y - pictureBoxTime.Height) / _gridRowHeight);
                if (_selectedLineIndex < _sequence.ChannelCount) {
                    _editingChannelSortedIndex = _channelOrderMapping[_selectedLineIndex];
                    flag = (_editingChannelSortedIndex >= 0) && (_editingChannelSortedIndex < _sequence.ChannelCount);
                }
            }
            if (flag) {
                _currentlyEditingChannel = SelectedChannel = _sequence.Channels[_editingChannelSortedIndex];
            }
            return flag;
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
            var none = DialogResult.None;
            if (IsDirty) {
                var str = _sequence.Name ?? "this unnamed sequence";
                none = MessageBox.Show(string.Format("Save changes to {0}?", str), Vendor.ProductName, MessageBoxButtons.YesNoCancel,
                                       MessageBoxIcon.Question);
                if (none == DialogResult.Yes) {
                    _systemInterface.InvokeSave(this);
                }
            }
            return none;
        }


        private void CheckMaximums() {
            for (var i = 0; i < _sequence.ChannelCount; i++) {
                var num3 = _channelOrderMapping[i];
                for (var j = 0; j < _sequence.TotalEventPeriods; j++) {
                    var num2 = _sequence.EventValues[num3, j];
                    if (num2 != 0) {
                        _sequence.EventValues[num3, j] = Math.Min(num2, _sequence.MaximumLevel);
                    }
                }
            }
        }


        private void CheckMinimums() {
            bool flag = false;
            for (int i = 0; i < _sequence.ChannelCount; i++) {
                int num3 = _channelOrderMapping[i];
                for (int j = 0; j < _sequence.TotalEventPeriods; j++) {
                    byte num2 = _sequence.EventValues[num3, j];
                    _sequence.EventValues[num3, j] = Math.Max(num2, _sequence.MinimumLevel);
                    flag = true;
                }
            }
            if (flag) {
                IsDirty = true;
            }
        }


        private void clearAllChannelsForThisEventToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_selectedEventIndex != -1) {
                var blockAffected = new Rectangle(_selectedEventIndex, 0, 1, _sequence.ChannelCount);
                AddUndoItem(blockAffected, UndoOriginalBehavior.Overwrite, "Clear values");
                for (var i = 0; i < _sequence.ChannelCount; i++) {
                    _sequence.EventValues[_channelOrderMapping[i], _selectedEventIndex] = _sequence.MinimumLevel;
                }
                InvalidateRect(blockAffected);
            }
        }


        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Clear all events in the sequence?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes) {
                var normalizedRange = _selectedCells;
                _selectedCells = new Rectangle(0, 0, _sequence.TotalEventPeriods, _sequence.ChannelCount);
                TurnCellsOff();
                _selectedCells = normalizedRange;
                pictureBoxGrid.Refresh();
            }
        }


        private void ClearChannel(int lineIndex) {
            AddUndoItem(new Rectangle(0, lineIndex, _sequence.TotalEventPeriods, 1), UndoOriginalBehavior.Overwrite, "Clear Channel");
            for (var i = 0; i < _sequence.TotalEventPeriods; i++) {
                _sequence.EventValues[_editingChannelSortedIndex, i] = _sequence.MinimumLevel;
            }
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
            _selectedEventIndex = hScrollBar1.Value + (pictureBoxTime.PointToClient(MousePosition).X / _periodPixelWidth);
        }


        private void CopyCells() {
            _systemInterface.Clipboard = CellsToArray();
        }


        private void copyChannelEventsToClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
            var buffer = new byte[1,_sequence.TotalEventPeriods];
            for (var i = 0; i < _sequence.TotalEventPeriods; i++) {
                buffer[0, i] = _sequence.EventValues[_editingChannelSortedIndex, i];
            }
            _systemInterface.Clipboard = buffer;
        }


        private void copyChannelToolStripMenuItem_Click(object sender, EventArgs e) {
            new ChannelCopyDialog(_affectGridDelegate, _sequence, _channelOrderMapping).Show();
        }


        private void createFromSequenceToolStripMenuItem_Click(object sender, EventArgs e) {
            var objectInContext = new Profile();
            objectInContext.InheritChannelsFrom(_sequence);
            objectInContext.InheritPlugInDataFrom(_sequence);
            objectInContext.InheritSortsFrom(_sequence);
            var dialog = new ProfileManagerDialog(objectInContext);
            if ((dialog.ShowDialog() == DialogResult.OK) &&
                (MessageBox.Show("Do you want to attach this sequence to the new profile?", Vendor.ProductName, MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question) == DialogResult.Yes)) {
                SetProfile(objectInContext);
            }
            dialog.Dispose();
        }


        private void currentProgramsSettingsToolStripMenuItem_Click(object sender, EventArgs e) {
            var dialog = new SequenceSettingsDialog(_sequence);
            int minimumLevel = _sequence.MinimumLevel;
            int maximumLevel = _sequence.MaximumLevel;
            var eventPeriod = _sequence.EventPeriod;
            if (dialog.ShowDialog() == DialogResult.OK) {
                if (minimumLevel != _sequence.MinimumLevel) {
                    CheckMinimums();
                    if (_drawingLevel < _sequence.MinimumLevel) {
                        SetDrawingLevel(_sequence.MinimumLevel);
                        MessageBox.Show("Drawing level was below the sequence minimum, so it has been adjusted.", Vendor.ProductName,
                                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                if (maximumLevel != _sequence.MaximumLevel) {
                    CheckMaximums();
                    if (_drawingLevel > _sequence.MaximumLevel) {
                        SetDrawingLevel(_sequence.MaximumLevel);
                        MessageBox.Show("Drawing level was above the sequence maximum, so it has been adjusted.", Vendor.ProductName,
                                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                if (eventPeriod != _sequence.EventPeriod) {
                    HScrollCheck();
                    ParseAudioWaveform();
                    pictureBoxTime.Refresh();
                }
                pictureBoxGrid.Refresh();
            }
            dialog.Dispose();
        }


        private void DeleteChannelFromSort(int naturalIndex) {
            _channelOrderMapping.Remove(naturalIndex);
            for (var i = 0; i < _channelOrderMapping.Count; i++) {
                if (_channelOrderMapping[i] > naturalIndex) {
                    List<int> list;
                    int num2;
                    (list = _channelOrderMapping)[num2 = i] = list[num2] - 1;
                }
            }
        }


        private void detachSequenceFromItsProfileToolStripMenuItem_Click(object sender, EventArgs e) {
            if (
                MessageBox.Show(
                    "Do you wish to detach this sequence from its profile?\n\nThis will not cause anything to be deleted.\nVixen will attempt to reload channel and plugin data from the sequence.",
                    Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                SetProfile((Profile) null);
            }
        }


        private void DimmingShimmerGenerator(byte[,] values, params int[] effectParameters) {
            var effectsCount = effectParameters[0];
            if (effectsCount == 0) {
                return;
            }

            var rowPerCycle = (int) Math.Round(_sequence.EventsPerSecond / effectsCount, MidpointRounding.AwayFromZero);
            if (rowPerCycle == 0) {
                return;
            }

            var totalRows = values.GetLength(1);
            var cols = values.GetLength(0);
            var random = new Random();
            for (var rowOffset = 0; rowOffset < totalRows; rowOffset += rowPerCycle) {
                var randomValue = (byte) Math.Max(random.NextDouble() * _sequence.MaximumLevel, _sequence.MinimumLevel);

                var rows = Math.Min(totalRows, rowOffset + rowPerCycle) - rowOffset;
                for (var row = 0; row < rows; row++) {
                    for (var col = 0; col < cols; col++) {
                        values[col, rowOffset + row] = randomValue;
                    }
                }
            }
        }


        private void DisableWaveformButton() {
            toolStripButtonWaveform.Enabled = false;
            toolStripComboBoxWaveformZoom.Enabled = false;
            toolStripLabelWaveformZoom.Enabled = false;
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


        private void DisjointedInsert(int x, int width, int height, int[] channelIndexes) {
            for (var i = 0; i < height; i++) {
                var num = channelIndexes[i];
                for (var j = ((_sequence.TotalEventPeriods - x) - width) - 1; j >= 0; j--) {
                    _sequence.EventValues[num, (j + x) + width] = _sequence.EventValues[num, j + x];
                }
            }
        }


        private void DisjointedOverwrite(int x, byte[,] data, int[] channelIndexes) {
            for (var i = 0; i < data.GetLength(0); i++) {
                var num = channelIndexes[i];
                for (var j = 0; j < data.GetLength(1); j++) {
                    _sequence.EventValues[num, j + x] = data[i, j];
                }
            }
        }


        private void DisjointedRemove(int x, int width, int height, int[] channelIndexes) {
            int num;
            int num2;
            int num3;
            for (num3 = 0; num3 < height; num3++) {
                num = channelIndexes[num3];
                num2 = 0;
                while (num2 < ((_sequence.TotalEventPeriods - x) - width)) {
                    _sequence.EventValues[num, num2 + x] = _sequence.EventValues[num, (num2 + x) + width];
                    num2++;
                }
            }
            for (num3 = 0; num3 < height; num3++) {
                num = channelIndexes[num3];
                for (num2 = 0; num2 < width; num2++) {
                    _sequence.EventValues[num, (_sequence.TotalEventPeriods - width) + num2] = _sequence.MinimumLevel;
                }
            }
        }



        private void DrawSelectedRange() {
            var point = new Point();
            var point2 = new Point();
            point.X = (_selectedCells.Left - hScrollBar1.Value) * _periodPixelWidth;
            point.Y = (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight;
            point2.X = (_selectedCells.Right - hScrollBar1.Value) * _periodPixelWidth;
            point2.Y = (_selectedCells.Bottom - vScrollBar1.Value) * _gridRowHeight;
            _selectionRectangle.X = point.X;
            _selectionRectangle.Y = point.Y;
            _selectionRectangle.Width = point2.X - point.X;
            _selectionRectangle.Height = point2.Y - point.Y;
            if (_selectionRectangle.Width == 0) {
                _selectionRectangle.Width = _periodPixelWidth;
            }
            if (_selectionRectangle.Height == 0) {
                _selectionRectangle.Height = _gridRowHeight;
            }
            pictureBoxGrid.Invalidate(_selectionRectangle);
            pictureBoxGrid.Update();
        }


        private void EditSequenceChannelMask() {
            var dialog = new ChannelOutputMaskDialog(_sequence.Channels);
            if (dialog.ShowDialog() == DialogResult.OK) {
                foreach (VixenPlus.Channel channel in _sequence.Channels) {
                    channel.Enabled = true;
                }
                foreach (int num in dialog.DisabledChannels) {
                    _sequence.Channels[num].Enabled = false;
                }
                IsDirty = true;
            }
            dialog.Dispose();
        }


        private void EnableWaveformButton() {
            if (_sequence.Audio != null) {
                toolStripButtonWaveform.Enabled = true;
                if (toolStripButtonWaveform.Checked) {
                    toolStripComboBoxWaveformZoom.Enabled = true;
                    toolStripLabelWaveformZoom.Enabled = true;
                }
            }
        }


        private void EraseRectangleEntity(Rectangle rect) {
            rect.Offset(-hScrollBar1.Value, -vScrollBar1.Value);
            var rc = CellsToPixels(rect);
            rect.X = -1;
            pictureBoxGrid.Invalidate(rc);
        }


        private void EraseSelectedRange() {
            Rectangle rc = SelectionToRectangle();
            _selectedCells.Width = _selectedRange.Width = 0;
            pictureBoxGrid.Invalidate(rc);
        }


        private void exportChannelNamesListToolStripMenuItem_Click(object sender, EventArgs e) {
            string path = Path.Combine(Paths.ImportExportPath, _sequence.Name + "_channels.txt");
            var writer = new StreamWriter(path);
            try {
                foreach (VixenPlus.Channel channel in _sequence.Channels) {
                    writer.WriteLine(channel.Name);
                }
                MessageBox.Show("Channel name list exported to " + path, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally {
                writer.Close();
            }
        }


        private void FillChannel(int lineIndex) {
            AddUndoItem(new Rectangle(0, lineIndex, _sequence.TotalEventPeriods, 1), UndoOriginalBehavior.Overwrite,"Fill");
            for (var i = 0; i < _sequence.TotalEventPeriods; i++) {
                _sequence.EventValues[_editingChannelSortedIndex, i] = _drawingLevel;
            }
            pictureBoxGrid.Refresh();
            IsDirty = true;
        }


        private void flattenProfileIntoSequenceToolStripMenuItem_Click(object sender, EventArgs e) {
            if (
                MessageBox.Show(
                    "This will detach the sequence from the profile and bring the profile data into the sequence.\nIs this what you want to do?",
                    Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No) {
                Profile profile = _sequence.Profile;
                _sequence.Profile = null;
                var list = new List<VixenPlus.Channel>();
                list.AddRange(profile.Channels);
                _sequence.Channels = list;
                _sequence.Sorts.LoadFrom(profile.Sorts);
                _sequence.PlugInData.LoadFromXml(profile.PlugInData.RootNode.ParentNode);
                IsDirty = true;
                ReactToProfileAssignment();
            }
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
            for (var i = 0; i < blockAffected.Height; i++) {
                var num3 = _channelOrderMapping[blockAffected.Y + i];
                for (var j = 0; j < blockAffected.Width; j++) {
                    buffer[i, j] = _sequence.EventValues[num3, blockAffected.X + j];
                }
            }
            return buffer;
        }


        private int GetCellIntensity(int cellX, int cellY, out string intensityText) {
            if ((cellX >= 0) && (cellY >= 0)) {
                int num;
                if (_actualLevels) {
                    num = _sequence.EventValues[_channelOrderMapping[cellY], cellX];
                    intensityText = string.Format("{0}", num);
                    return num;
                }
                num = (int) Math.Round(_sequence.EventValues[_channelOrderMapping[cellY], cellX] * 100f / 255f, MidpointRounding.AwayFromZero);
                intensityText = string.Format("{0}%", num);
                return num;
            }
            intensityText = "";
            return 0;
        }


        private VixenPlus.Channel GetChannelAt(Point point) {
            return GetChannelAtSortedIndex(GetLineIndexAt(point));
        }


        private VixenPlus.Channel GetChannelAtSortedIndex(int index) {
            if (index < _channelOrderMapping.Count) {
                return _sequence.Channels[_channelOrderMapping[index]];
            }
            return null;
        }


        private Rectangle GetChannelNameRect(VixenPlus.Channel channel) {
            if (channel != null) {
                return new Rectangle(0, ((GetChannelSortedIndex(channel) - vScrollBar1.Value) * _gridRowHeight) + pictureBoxTime.Height,
                                     pictureBoxChannels.Width, _gridRowHeight);
            }
            return Rectangle.Empty;
        }


        private int GetChannelNaturalIndex(VixenPlus.Channel channel) {
            return _sequence.Channels.IndexOf(channel);
        }


        private int GetChannelSortedIndex(VixenPlus.Channel channel) {
            return _channelOrderMapping.IndexOf(_sequence.Channels.IndexOf(channel));
        }


        private static Color GetGradientColor(Color startColor, Color endColor, int level) {
            var num = level / 255f;
            var red = (int) (((endColor.R - startColor.R) * num) + startColor.R);
            var green = (int) (((endColor.G - startColor.G) * num) + startColor.G);
            var blue = (int) (((endColor.B - startColor.B) * num) + startColor.B);
            return Color.FromArgb(red, green, blue);
        }


        private int GetLineIndexAt(Point point) {
            return (((point.Y - pictureBoxTime.Height) / _gridRowHeight) + vScrollBar1.Value);
        }


        private static byte[,] GetRoutine() {
            var list = new List<string[]>();
            var dialog = new RoutineSelectDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                string str;
                var reader = new StreamReader(dialog.SelectedRoutine);
                while ((str = reader.ReadLine()) != null) {
                    list.Add(str.Trim().Split(new[] {' '}));
                }
                reader.Close();
                reader.Dispose();
                var length = list[0].Length;
                var count = list.Count;
                var buffer = new byte[count,length];
                for (var i = 0; i < count; i++) {
                    for (var j = 0; j < length; j++) {
                        buffer[i, j] = Convert.ToByte(list[i][j]);
                    }
                }
                dialog.Dispose();
                return buffer;
            }
            dialog.Dispose();
            return null;
        }


        private static uint GetSampleMinMax(int startSample, int sampleCount, Sound sound, int bits, int channels) {
            int num10;
            int num12;
            short num = -32768;
            short num2 = 32767;
            //var num3 = startSample + sampleCount;
            var num4 = (bits >> 3) * channels;
            var num5 = startSample * num4;
            //var num6 = num3 * num4;
            var length = num4 * sampleCount;
            var destination = new byte[length];
            var zero = IntPtr.Zero;
            var ptr2 = IntPtr.Zero;
            uint num8 = 0;
            uint num9 = 0;
            sound.@lock((uint) num5, (uint) length, ref zero, ref ptr2, ref num8, ref num9);
            Marshal.Copy(zero, destination, 0, length);
            num5 = 0;
            if (bits == 16) {
                for (num12 = 0; num12 < sampleCount; num12++) {
                    num10 = 0;
                    while (num10 < channels) {
                        var num11 = BitConverter.ToInt16(destination, num5 + (num10 * 2));
                        num = Math.Max(num, num11);
                        num2 = Math.Min(num2, num11);
                        num10++;
                    }
                    num5 += num4;
                }
            }
            else if (bits == 8) {
                for (num12 = 0; num12 < sampleCount; num12++) {
                    for (num10 = 0; num10 < channels; num10++) {
                        var num13 = (sbyte) destination[num5 + num10];
                        num = Math.Max(num, num13);
                        num2 = Math.Min(num2, num13);
                    }
                    num5 += num4;
                }
            }
            sound.unlock(zero, ptr2, num8, num9);
            return (uint) ((num << 16) | ((ushort) num2));
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
            if (_periodPixelWidth != 0) {
                _visibleEventPeriods = pictureBoxGrid.Width / _periodPixelWidth;
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
            if (hScrollBar1.Maximum >= 0) {
                if (hScrollBar1.Value == -1) {
                    hScrollBar1.Value = 0;
                }
                if (hScrollBar1.Minimum == -1) {
                    hScrollBar1.Minimum = 0;
                }
            }
        }


        private void importChannelNamesListToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_sequence.Profile != null) {
                MessageBox.Show("Can't import channel names when attached to a profile.", Vendor.ProductName, MessageBoxButtons.OK,
                                MessageBoxIcon.Hand);
            }
            else {
                openFileDialog1.Filter = "Text file | *.txt";
                openFileDialog1.DefaultExt = "txt";
                openFileDialog1.InitialDirectory = Paths.ImportExportPath;
                openFileDialog1.FileName = string.Empty;
                if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    var list = new List<string>();
                    try {
                        string str;
                        while ((str = reader.ReadLine()) != null) {
                            list.Add(str);
                        }
                        SetChannelCount(list.Count);
                        for (var i = 0; i < list.Count; i++) {
                            _sequence.Channels[i].Name = list[i];
                        }
                        pictureBoxChannels.Refresh();
                        MessageBox.Show("Channel name list import complete", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    finally {
                        reader.Close();
                        reader.Dispose();
                    }
                }
            }
        }


        private void Init() {
            _initializing = true;
            InitializeComponent();
            _initializing = false;
            _gridRowHeight = _preferences.GetInteger("MaxRowHeight");
            _periodPixelWidth = _preferences.GetInteger("MaxColumnWidth");
            _showPositionMarker = _preferences.GetBoolean("ShowPositionMarker");
            _autoScrolling = _preferences.GetBoolean("AutoScrolling");
            _intensityLargeDelta = _preferences.GetInteger("IntensityLargeDelta");
            _showingGradient = !_preferences.GetBoolean("BarLevels");
            _channelBackBrush = new SolidBrush(Color.White);
            _timeBackBrush = new SolidBrush(Color.FromArgb(16, 16, 16));
            _gridBackBrush = new SolidBrush(Color.FromArgb(192, 192, 192));
            //_arrowBitmap = new Bitmap(pictureBoxOutputArrow.Image);
            //_arrowBitmap.MakeTransparent(_arrowBitmap.GetPixel(0, 0));
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
            splitContainer2.SplitterDistance = 60; // pictureBoxTime.Height - hScrollBar1.Height;
            toolStripComboBoxWaveformZoom.SelectedItem = "100%";
            SetDrawingLevel(_sequence.MaximumLevel);
            _executionInterface.SetSynchronousContext(_executionContextHandle, _sequence);
            UpdateLevelDisplay();
            IsDirty = false;
            pictureBoxChannels.AllowDrop = true;
        }



        private void InsertChannelIntoSort(int naturalIndex, int sortedIndex) {
            for (int i = 0; i < _channelOrderMapping.Count; i++) {
                if (_channelOrderMapping[i] >= naturalIndex) {
                    List<int> list;
                    int num2;
                    (list = _channelOrderMapping)[num2 = i] = list[num2] + 1;
                }
            }
            _channelOrderMapping.Insert(sortedIndex, naturalIndex);
        }


        private void IntensityAdjustDialogCheck() {
            if (!m_intensityAdjustDialog.Visible) {
                m_intensityAdjustDialog.Show();
                BringToFront();
            }
        }


        private void InvalidateRect(Rectangle rect) {
            rect.X -= hScrollBar1.Value;
            rect.Y -= vScrollBar1.Value;
            _selectionRectangle.X = Math.Min(rect.Left, rect.Right) * _periodPixelWidth;
            _selectionRectangle.Y = Math.Min(rect.Top, rect.Bottom) * _gridRowHeight;
            _selectionRectangle.Width = Math.Abs((rect.Width + 1) * _periodPixelWidth);
            _selectionRectangle.Height = Math.Abs((rect.Height + 1) * _gridRowHeight);
            if (_selectionRectangle.Width == 0) {
                _selectionRectangle.Width = _periodPixelWidth;
            }
            if (_selectionRectangle.Height == 0) {
                _selectionRectangle.Height = _gridRowHeight;
            }
            pictureBoxGrid.Invalidate(_selectionRectangle);
            pictureBoxGrid.Update();
        }


        /*
                private int LineIndexToChannelIndex(int lineIndex)
                {
                    if ((lineIndex >= 0) && (lineIndex < _channelOrderMapping.Count))
                    {
                        return _channelOrderMapping[lineIndex];
                    }
                    return -1;
                }
        */


        private void loadARoutineToolStripMenuItem_Click(object sender, EventArgs e) {
            var routine = GetRoutine();
            if (routine != null) {
                AddUndoItem(new Rectangle(_selectedCells.X, _selectedCells.Y, routine.GetLength(1), routine.GetLength(0)),
                            UndoOriginalBehavior.Overwrite, "Load Routine");
                ArrayToCells(routine);
                pictureBoxGrid.Refresh();
            }
        }


        private void loadRoutineToClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
            _systemInterface.Clipboard = GetRoutine();
        }


        private void LoadSequencePlugins() {
            _systemInterface.VerifySequenceHardwarePlugins(_sequence);
            var num = 0;
            toolStripDropDownButtonPlugins.DropDownItems.Clear();
            foreach (XmlNode node in _sequence.PlugInData.GetAllPluginData()) {
                //todo refactor
                var item =
                    new ToolStripMenuItem(string.Format("{0} ({1}-{2})", node.Attributes["name"].Value, node.Attributes["from"].Value,
                                                        node.Attributes["to"].Value));
                item.Checked = bool.Parse(node.Attributes["enabled"].Value);
                item.CheckOnClick = true;
                item.CheckedChanged += _pluginCheckHandler;
                item.Tag = num.ToString(CultureInfo.InvariantCulture);
                num++;
                toolStripDropDownButtonPlugins.DropDownItems.Add(item);
            }
            if (toolStripDropDownButtonPlugins.DropDownItems.Count > 0) {
                toolStripDropDownButtonPlugins.DropDownItems.Add("-");
                toolStripDropDownButtonPlugins.DropDownItems.Add("Select all", null, selectAllToolStripMenuItem_Click);
                toolStripDropDownButtonPlugins.DropDownItems.Add("Unselect all", null, unselectAllToolStripMenuItem_Click);
            }
        }


        private void LoadSequenceSorts() {
            toolStripComboBoxChannelOrder.BeginUpdate();
            var item = (string) toolStripComboBoxChannelOrder.Items[0];
            var str2 = (string) toolStripComboBoxChannelOrder.Items[toolStripComboBoxChannelOrder.Items.Count - 1];
            toolStripComboBoxChannelOrder.Items.Clear();
            toolStripComboBoxChannelOrder.Items.Add(item);
            foreach (var order in _sequence.Sorts) {
                toolStripComboBoxChannelOrder.Items.Add(order);
            }
            toolStripComboBoxChannelOrder.Items.Add(str2);
            if (_sequence.LastSort != -1) {
                toolStripComboBoxChannelOrder.SelectedIndex = _sequence.LastSort + 1;
            }
            toolStripComboBoxChannelOrder.EndUpdate();
        }


        private void m_intensityAdjustDialog_VisibleChanged(object sender, EventArgs e) {
            if (!m_intensityAdjustDialog.Visible) {
                int left;
                int num2;
                int num4;
                int num8;
                AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite,"Adjust intensity");
                var delta = m_intensityAdjustDialog.Delta;
                var bottom = _selectedCells.Bottom;
                var right = _selectedCells.Right;
                if (_actualLevels) {
                    for (num8 = _selectedCells.Top; num8 < bottom; num8++) {
                        num2 = _channelOrderMapping[num8];
                        if (_sequence.Channels[num2].Enabled) {
                            left = _selectedCells.Left;
                            while (left < right) {
                                num4 = _sequence.EventValues[num2, left] + delta;
                                num4 = Math.Max(Math.Min(num4, _sequence.MaximumLevel), _sequence.MinimumLevel);
                                _sequence.EventValues[num2, left] = (byte) num4;
                                left++;
                            }
                        }
                    }
                }
                else {
                    for (num8 = _selectedCells.Top; num8 < bottom; num8++) {
                        num2 = _channelOrderMapping[num8];
                        if (_sequence.Channels[num2].Enabled) {
                            for (left = _selectedCells.Left; left < right; left++) {
                                num4 = ((int) Math.Round(_sequence.EventValues[num2, left] * 100f / 255f, MidpointRounding.AwayFromZero)) +
                                       delta;
                                var num5 = (int) Math.Round(num4 / 100f * 255f, MidpointRounding.AwayFromZero);
                                num5 = Math.Max(Math.Min(num5, _sequence.MaximumLevel), _sequence.MinimumLevel);
                                _sequence.EventValues[num2, left] = (byte) num5;
                            }
                        }
                    }
                }
                pictureBoxGrid.Refresh();
            }
            else {
                m_intensityAdjustDialog.LargeDelta = _intensityLargeDelta;
            }
        }


        private void m_positionTimer_Tick(object sender, EventArgs e) {
            int num;
            if (_executionInterface.EngineStatus(_executionContextHandle, out num) == 0) {
                ProgramEnded();
            }
            else {
                var num2 = num / _sequence.EventPeriod;
                if (num2 != _position) {
                    toolStripLabelExecutionPoint.Text = string.Format("{0:d2}:{1:d2}", num / 60000, (num % 60000) / 1000);
                    _previousPosition = _position;
                    _position = num2;
                    if ((_position < hScrollBar1.Value) || (_position > (hScrollBar1.Value + _visibleEventPeriods))) {
                        if (_autoScrolling) {
                            if (_position != -1) {
                                if (_position >= _sequence.TotalEventPeriods) {
                                    _previousPosition = _position = _sequence.TotalEventPeriods - 1;
                                }
                                hScrollBar1.Value = _position;
                                toolStripLabelExecutionPoint.Text = string.Format("{0:d2}:{1:d2}", num / 60000, (num % 60000) / 1000);
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
                        toolStripLabelExecutionPoint.Text = string.Format("{0:d2}:{1:d2}", num / 60000, (num % 60000) / 1000);
                    }
                }
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
            Text = _sequence.Name ?? "Unnamed sequence";
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
                    VixenPlus.SortOrder currentOrder = _sequence.Sorts.CurrentOrder;
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
            var flag = (ModifierKeys & Keys.Shift) == Keys.None;
            if (_preferences.GetBoolean("FlipScrollBehavior")) {
                flag = !flag;
            }
            if (flag) {
                if (e.Delta > 0) {
                    if (vScrollBar1.Value >= (vScrollBar1.Minimum + _mouseWheelVerticalDelta)) {
                        vScrollBar1.Value -= _mouseWheelVerticalDelta;
                    }
                    else {
                        vScrollBar1.Value = vScrollBar1.Minimum;
                    }
                }
                else if (vScrollBar1.Value <= (vScrollBar1.Maximum - (_visibleRowCount + _mouseWheelVerticalDelta))) {
                    vScrollBar1.Value += _mouseWheelVerticalDelta;
                }
                else {
                    vScrollBar1.Value = Math.Max((vScrollBar1.Maximum - _visibleRowCount) + 1, 0);
                }
            }
            else if (e.Delta > 0) {
                if (hScrollBar1.Value >= (hScrollBar1.Minimum + _mouseWheelHorizontalDelta)) {
                    hScrollBar1.Value -= _mouseWheelHorizontalDelta;
                }
                else {
                    hScrollBar1.Value = hScrollBar1.Minimum;
                }
            }
            else if (hScrollBar1.Value <= (hScrollBar1.Maximum - (_visibleEventPeriods + _mouseWheelHorizontalDelta))) {
                hScrollBar1.Value += _mouseWheelHorizontalDelta;
            }
            else {
                hScrollBar1.Value = Math.Max((hScrollBar1.Maximum - _visibleEventPeriods) + 1, 0);
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


        //TODO Refactor
        private void ParseAudioWaveform() {
            string str;
            if ((_sequence.Audio != null) && File.Exists(str = Path.Combine(Paths.AudioPath, _sequence.Audio.FileName))) {
                if (toolStripButtonWaveform.Checked) {
                    var dialog = new VixenEditor.ProgressDialog();
                    dialog.Show();
                    dialog.Message = "Parsing sound data, please wait...";
                    Cursor = Cursors.WaitCursor;
                    try {
                        _waveformPcmData = new uint[_sequence.TotalEventPeriods * _periodPixelWidth];
                        _waveformPixelData = new uint[_sequence.TotalEventPeriods * _periodPixelWidth];
                        var rAW = SOUND_TYPE.RAW;
                        var nONE = SOUND_FORMAT.NONE;
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
                        double num9 = num8 / ((double) _sequence.TotalEventPeriods);
                        double num10 = num9 / ((double) _periodPixelWidth);
                        double num11 = ((double) _sequence.EventPeriod) / ((double) _periodPixelWidth);
                        double num12 = frequency / 1000f;
                        int num14 = 0;
                        int num15 = 0;
                        num14 = 0;
                        num15 = 0;
                        uint num18 = 0;
                        sound.getLength(ref num18, TIMEUNIT.MS);
                        int index = 0;
                        for (double i = 0.0; (index < _waveformPcmData.Length) && (i < num18); i += num11) {
                            int startSample = (int) (i * num12);
                            uint num16 = GetSampleMinMax(startSample, (int) Math.Min(num10, num8 - startSample), sound, bits, channels);
                            num14 = Math.Max(num14, (short) (num16 >> 0x10));
                            num15 = Math.Min(num15, (short) (num16 & 0xffff));
                            _waveformPcmData[index] = num16;
                            index++;
                        }
                        _waveform100PercentAmplitude = _waveformMaxAmplitude = Math.Max(num14, -num15);
                        PcmToPixels(_waveformPcmData, _waveformPixelData);
                        sound.release();
                    }
                    finally {
                        Cursor = Cursors.Default;
                        dialog.Hide();
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
            if (_systemInterface.Clipboard != null) {
                AddUndoItem(new Rectangle(0, _selectedLineIndex, _systemInterface.Clipboard.GetLength(1), 1), UndoOriginalBehavior.Overwrite,"Paste");
                var num2 = Math.Min(_systemInterface.Clipboard.GetLength(1), _sequence.TotalEventPeriods);
                for (int i = 0; i < num2; i++) {
                    _sequence.EventValues[_editingChannelSortedIndex, i] = _systemInterface.Clipboard[0, i];
                }
                IsDirty = true;
                pictureBoxGrid.Refresh();
            }
        }


        private void PasteOver() {
            if (_systemInterface.Clipboard != null) {
                ArrayToCells(_systemInterface.Clipboard);
                pictureBoxGrid.Refresh();
            }
        }


        private void PcmToPixels(uint[] pcmDataValues, uint[] pixelData) {
            var pcmDataLength = pcmDataValues.Length;
            var waveformOffset = _waveformOffset;
            var negativeOffset = -_waveformOffset;
            var amplitudeDivisor = _waveformMaxAmplitude / ((double) Math.Max(waveformOffset, negativeOffset));
            for (var i = 0; i < pcmDataLength; i++) {
                var pcmData = pcmDataValues[i];

                var minAmp = (short) (Math.Min((short) (pcmData >> 16), _waveformMaxAmplitude) / amplitudeDivisor);
                minAmp = Math.Max(minAmp, (short) 0);
                minAmp = (short) Math.Min(minAmp, waveformOffset);

                var maxAmp = (short) (Math.Max((short) (pcmData & 0xffff), -_waveformMaxAmplitude) / amplitudeDivisor);
                maxAmp = Math.Min(maxAmp, (short) 0);
                maxAmp = (short) Math.Max(maxAmp, negativeOffset);

                pixelData[i] = (uint) ((minAmp << 16) | ((ushort) maxAmp));
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
            if (!e.Data.GetDataPresent(typeof (VixenPlus.Channel))) {
                return;
            }

            var point = pictureBoxChannels.PointToClient(new Point(e.X, e.Y));
            var lineIndexAt = GetLineIndexAt(point);
            if (vScrollBar1.Enabled) {
                if ((vScrollBar1.Value > vScrollBar1.Minimum) && ((lineIndexAt - vScrollBar1.Value) < 2)) {
                    vScrollBar1.Value--;
                    Thread.Sleep(50); //todo evaluate how to put in preferences or a constant
                    lineIndexAt = GetLineIndexAt(point);
                }
                else if ((vScrollBar1.Value < vScrollBar1.Maximum) && ((lineIndexAt - vScrollBar1.Value) > (_visibleRowCount - 2))) {
                    vScrollBar1.Value++;
                    Thread.Sleep(50); //todo evaluate how to put in preferences or a constant
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


        private void pictureBoxChannels_GiveFeedback(object sender, GiveFeedbackEventArgs e) {}


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
            if ((((e.Button & MouseButtons.Left) != MouseButtons.None) && (_mouseDownAtInChannels != Point.Empty)) &&
                ((Math.Abs(e.X - _mouseDownAtInChannels.X) > 3) || (Math.Abs(e.Y - _mouseDownAtInChannels.Y) > 3))) {
                DoDragDrop(SelectedChannel, DragDropEffects.Move | DragDropEffects.Copy);
            }
        }


        private void pictureBoxChannels_MouseUp(object sender, MouseEventArgs e) {
            _mouseDownAtInChannels = Point.Empty;
        }


        //major refactor done, seems stable.
        private void pictureBoxChannels_Paint(object sender, PaintEventArgs e) {
            e.Graphics.FillRectangle(_channelBackBrush, e.Graphics.VisibleClipBounds);

            VixenPlus.Channel channel;
            int mappedChannel;
            int channelOffset;

            var height = pictureBoxTime.Height;
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

                    e.Graphics.FillRectangle(isChannelSelected ? SystemBrushes.Highlight : channel.Brush, 0, height, pictureBoxChannels.Width,
                                             _gridRowHeight);

                    if (showNaturalChannelNumbers) {
                        e.Graphics.DrawString(string.Format("{0}:", mappedChannel + 1), _channelNameFont, Brushes.Black, 10f, height + heightAddition);
                    }

                    e.Graphics.DrawString(channel.Name, _channelNameFont, isChannelSelected ? SystemBrushes.HighlightText : Brushes.Black, x,
                                          height + heightAddition);

                    height += _gridRowHeight;
                }
            }
                #endregion
                #region Showing Outputs

            else {
                var width = Math.Min(_gridRowHeight - 4, 11); // 11 came from the height of an arrow which was 11x11
                var heightOffset = (_gridRowHeight - width) >> 1;
                var brush = new SolidBrush(Color.White);

                for (channelOffset = vScrollBar1.Value; (channelOffset >= 0) && (channelOffset < _sequence.ChannelCount); channelOffset++) {

                    mappedChannel = _channelOrderMapping[channelOffset];
                    channel = _sequence.Channels[mappedChannel];

                    brush.Color = Color.FromArgb(192, Color.Gray);

                    e.Graphics.FillRectangle(channel == SelectedChannel && showNaturalChannelNumbers ? SystemBrushes.Highlight : channel.Brush, 0,
                                             height, pictureBoxChannels.Width, _gridRowHeight);

                    if (showNaturalChannelNumbers) {
                        e.Graphics.DrawString(string.Format("{0}:", mappedChannel + 1), _channelNameFont, Brushes.Black, 10f, height + heightAddition);
                    }
                    else {
                        x = 10;
                    }

                    e.Graphics.FillRectangle(brush, x, height + 1, 40, _gridRowHeight - 2);

                    if (toolStripComboBoxRowZoom.SelectedIndex > 4) //todo What is 4? 30%
                    {
                        e.Graphics.DrawRectangle(Pens.Black, x, height + 1, 40, _gridRowHeight - 2);
                    }

                    //e.Graphics.DrawImage(_arrowBitmap, x + 2, height + heightOffset, width, width);
                    e.Graphics.DrawString(String.Format("{0}", channel.OutputChannel + 1), _channelNameFont, Brushes.Black, x + 16,
                                          height + heightAddition);
                    e.Graphics.DrawString(channel.Name, _channelNameFont, Brushes.Black, x + 44, height + heightAddition);

                    height += _gridRowHeight;
                }

                #endregion

                brush.Dispose();
            }
            e.Graphics.FillRectangle(Brushes.White, 0, 0, 5, pictureBoxChannels.Height);
            if (_mouseChannelCaret != -1) {
                e.Graphics.FillRectangle(_channelCaretBrush, 0, ((_mouseChannelCaret - vScrollBar1.Value) * _gridRowHeight) + pictureBoxTime.Height, 5,
                                         _gridRowHeight);
            }
        }


        /*
                private void pictureBoxChannels_QueryContinueDrag(object sender, QueryContinueDragEventArgs keyEvent)
                {
                }
        */


        private void pictureBoxChannels_Resize(object sender, EventArgs e) {
            pictureBoxChannels.Refresh();
        }


        private void pictureBoxGrid_DoubleClick(object sender, EventArgs e) {
            if (_currentlyEditingChannel != null) {
                AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, "Double Click");
                _sequence.EventValues[_editingChannelSortedIndex, _selectedCells.X] =
                    (_sequence.EventValues[_editingChannelSortedIndex, _selectedCells.X] > _sequence.MinimumLevel)
                        ? _sequence.MinimumLevel : _drawingLevel;
                UpdateGrid(_gridGraphics,
                           new Rectangle((_selectedCells.X - hScrollBar1.Value) * _periodPixelWidth,
                                         (_editingChannelSortedIndex - vScrollBar1.Value) * _gridRowHeight, _periodPixelWidth, _gridRowHeight));
            }
        }


        private void pictureBoxGrid_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                _mouseDownInGrid = true;
                lblFollowMouse.Visible = true;
                _mouseDownAtInGrid.X = (e.X / _periodPixelWidth) + hScrollBar1.Value;
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
                    var rect = new Rectangle();
                    rect.X = _selectedCells.X;
                    rect.Y = _selectedCells.Y;
                    rect.Width = ((hScrollBar1.Value + (int) Math.Floor(e.X / ((float) _periodPixelWidth))) - _selectedCells.Left) + 1;
                    if (rect.Width < 0) {
                        rect.Width--;
                    }
                    rect.Height = ((vScrollBar1.Value + (e.Y / _gridRowHeight)) - _selectedCells.Top) + 1;
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
                else if ((((e.Y / _gridRowHeight) + vScrollBar1.Value) < _sequence.ChannelCount) &&
                         (((e.X / _periodPixelWidth) + hScrollBar1.Value) < _sequence.TotalEventPeriods)) {
                    _selectedLineIndex = (e.Y / _gridRowHeight) + vScrollBar1.Value;
                    _editingChannelSortedIndex = _channelOrderMapping[_selectedLineIndex];
                    _currentlyEditingChannel = _sequence.Channels[_editingChannelSortedIndex];
                    _selectedRange.X = hScrollBar1.Value + ((int) Math.Floor(e.X / ((float) _periodPixelWidth)));
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
                UpdateFollowMouse(new Point(_mouseDownAtInGrid.X, _mouseDownAtInGrid.Y));
            }
        }


        private void pictureBoxGrid_MouseLeave(object sender, EventArgs e) {
            toolStripLabelCellIntensity.Text = string.Empty;
            toolStripLabelCurrentCell.Text = string.Empty;
        }


        //TODO Refactor
        private void pictureBoxGrid_MouseMove(object sender, MouseEventArgs e) {
            Rectangle rectangle;
            int cellX = e.X / _periodPixelWidth;
            int cellY = e.Y / _gridRowHeight;
            toolStripLabelCellIntensity.Text = string.Empty;
            toolStripLabelCurrentCell.Text = string.Empty;
            if (cellX < 0) {
                cellX = 0;
            }
            if (cellY < 0) {
                cellY = 0;
            }
            cellX += hScrollBar1.Value;
            cellY += vScrollBar1.Value;
            if (cellX >= _sequence.TotalEventPeriods) {
                cellX = _sequence.TotalEventPeriods - 1;
            }
            if (cellY >= _sequence.ChannelCount) {
                cellY = _sequence.ChannelCount - 1;
            }
            if ((e.Button != MouseButtons.Left) || !_mouseDownInGrid) {
                goto Label_0733;
            }
            if (_lineRect.Left == -1) {
                int num7 = 0;
                if (e.X > pictureBoxGrid.Width) {
                    num7 |= 0x10;
                }
                else if (e.X < 0) {
                    num7 |= 0x1000;
                }
                if (e.Y > pictureBoxGrid.Height) {
                    num7 |= 1;
                }
                else if (e.Y < 0) {
                    num7 |= 0x100;
                }
                switch (num7) {
                    case 0x100:
                        ScrollSelectionUp(cellX);
                        goto Label_0715;

                    case 0x1000:
                        ScrollSelectionLeft(cellX, cellY);
                        goto Label_0715;

                    case 0x1100:
                        ScrollSelectionLeft(cellX, cellY);
                        ScrollSelectionUp(cellX);
                        goto Label_0715;

                    case 0:
                        EraseSelectedRange();
                        if (cellX >= _mouseDownAtInGrid.X) {
                            if (cellX > _mouseDownAtInGrid.X) {
                                _selectedRange.Width = (cellX - _selectedRange.Left) + 1;
                            }
                            else {
                                _selectedRange.Width = 1;
                            }
                        }
                        else {
                            _selectedRange.Width = cellX - _selectedRange.Left;
                        }
                        if (cellY < _mouseDownAtInGrid.Y) {
                            _selectedRange.Height = cellY - _selectedRange.Top;
                        }
                        else if (cellY > _mouseDownAtInGrid.Y) {
                            _selectedRange.Height = (cellY - _selectedRange.Top) + 1;
                        }
                        else {
                            _selectedRange.Height = 1;
                        }
                        _selectedCells = NormalizeRect(_selectedRange);
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
            EraseRectangleEntity(_lineRect);
            if ((ModifierKeys & Keys.Shift) != Keys.None) {
                int num5;
                int num6;
                int num3 = cellX - _mouseDownAtInGrid.X;
                int num4 = cellY - _mouseDownAtInGrid.Y;
                if (num3 >= 0) {
                    if (num4 >= 0) {
                        if (num4 < (num3 >> 1)) {
                            num5 = 4;
                        }
                        else if (num3 < (num4 >> 1)) {
                            num5 = 6;
                        }
                        else {
                            num5 = 5;
                        }
                    }
                    else if (num3 < -(num4 >> 1)) {
                        num5 = 2;
                    }
                    else if (num4 >= -(num3 >> 1)) {
                        num5 = 4;
                    }
                    else {
                        num5 = 3;
                    }
                }
                else if (num4 >= 0) {
                    if (num4 < -(num3 >> 1)) {
                        num5 = 8;
                    }
                    else if (num3 >= -(num4 >> 1)) {
                        num5 = 6;
                    }
                    else {
                        num5 = 7;
                    }
                }
                else if (num4 >= (num3 >> 1)) {
                    num5 = 8;
                }
                else if (num3 >= (num4 >> 1)) {
                    num5 = 2;
                }
                else {
                    num5 = 1;
                }
                num6 = Math.Abs(Math.Abs(num3) < Math.Abs(num4) ? num3 : num4);
                switch (num5) {
                    case 1:
                        _lineRect.Width = -num6;
                        _lineRect.Height = -num6;
                        goto Label_0473;

                    case 2:
                        _lineRect.Width = 0;
                        _lineRect.Height = num4;
                        goto Label_0473;

                    case 3:
                        _lineRect.Width = num6;
                        _lineRect.Height = -num6;
                        goto Label_0473;

                    case 4:
                        _lineRect.Width = num3;
                        _lineRect.Height = 0;
                        goto Label_0473;

                    case 5:
                        _lineRect.Width = num6;
                        _lineRect.Height = num6;
                        goto Label_0473;

                    case 6:
                        _lineRect.Width = 0;
                        _lineRect.Height = num4;
                        goto Label_0473;

                    case 7:
                        _lineRect.Width = -num6;
                        _lineRect.Height = num6;
                        goto Label_0473;

                    case 8:
                        _lineRect.Width = num3;
                        _lineRect.Height = 0;
                        goto Label_0473;
                }
            }
            else {
                if (cellX < _mouseDownAtInGrid.X) {
                    _lineRect.Width = cellX - _lineRect.Left;
                }
                else if (cellX > _mouseDownAtInGrid.X) {
                    _lineRect.Width = cellX - _lineRect.Left;
                }
                else {
                    _lineRect.Width = 0;
                }
                if (cellY < _mouseDownAtInGrid.Y) {
                    _lineRect.Height = cellY - _lineRect.Top;
                }
                else if (cellY > _mouseDownAtInGrid.Y) {
                    _lineRect.Height = cellY - _lineRect.Top;
                }
                else {
                    _lineRect.Height = 0;
                }
            }
            Label_0473:
            InvalidateRect(_lineRect);
            UpdatePositionLabel(NormalizeRect(new Rectangle(_lineRect.X, _lineRect.Y, _lineRect.Width + 1, _lineRect.Height)), true);
            //UpdateFollowMouse(keyEvent.Location);
            goto Label_0733;
            Label_0715:
            _lastCellX = cellX;
            _lastCellY = cellY;
            UpdatePositionLabel(_selectedCells, false);
            //UpdateFollowMouse(keyEvent.Location);
            Label_0733:
            int num8 = 0;
            int num9 = 0;
            int y = 0;
            int num11 = 0;
            if ((cellX != _mouseTimeCaret) || (cellY != _mouseChannelCaret)) {
                num8 = (Math.Min(cellX, _mouseTimeCaret) - hScrollBar1.Value) * _periodPixelWidth;
                num9 = ((Math.Max(cellX, _mouseTimeCaret) - hScrollBar1.Value) + 1) * _periodPixelWidth;
                y = (Math.Min(cellY, _mouseChannelCaret) - vScrollBar1.Value) * _gridRowHeight;
                num11 = ((Math.Max(cellY, _mouseChannelCaret) - vScrollBar1.Value) + 1) * _gridRowHeight;
            }
            if (cellY != _mouseChannelCaret) {
                rectangle = new Rectangle(0, pictureBoxTime.Height + (_gridRowHeight * (_mouseChannelCaret - vScrollBar1.Value)), 5, _gridRowHeight);
                _mouseChannelCaret = -1;
                pictureBoxChannels.Invalidate(rectangle);
                pictureBoxChannels.Update();
                if (cellY < _sequence.ChannelCount) {
                    _mouseChannelCaret = cellY;
                    rectangle.Y = pictureBoxTime.Height + (_gridRowHeight * (_mouseChannelCaret - vScrollBar1.Value));
                    pictureBoxChannels.Invalidate(rectangle);
                    pictureBoxChannels.Update();
                }
                else {
                    _mouseChannelCaret = -1;
                }
            }
            if (cellX != _mouseTimeCaret) {
                rectangle = new Rectangle(_periodPixelWidth * (_mouseTimeCaret - hScrollBar1.Value), 0, _periodPixelWidth, 5);
                _mouseTimeCaret = -1;
                pictureBoxTime.Invalidate(rectangle);
                pictureBoxTime.Update();
                if (cellX < _sequence.TotalEventPeriods) {
                    _mouseTimeCaret = cellX;
                    rectangle.X = _periodPixelWidth * (_mouseTimeCaret - hScrollBar1.Value);
                    pictureBoxTime.Invalidate(rectangle);
                    pictureBoxTime.Update();
                }
                else {
                    _mouseTimeCaret = -1;
                }
            }
            if (num8 != num9) {
                pictureBoxGrid.Invalidate(new Rectangle(num8, 0, num9 - num8, pictureBoxGrid.Height));
                pictureBoxGrid.Update();
                pictureBoxGrid.Invalidate(new Rectangle(0, y, pictureBoxGrid.Width, num11 - y));
                pictureBoxGrid.Update();
            }
            if ((cellX >= 0) && (cellY >= 0)) {
                string str;
                GetCellIntensity(cellX, cellY, out str);
                toolStripLabelCellIntensity.Text = str;
                toolStripLabelCurrentCell.Text = string.Format("{0} , {1}", TimeString(cellX * _sequence.EventPeriod),
                                                               _sequence.Channels[_channelOrderMapping[cellY]].Name);
            }
            UpdateFollowMouse(new Point(e.X, e.Y));
        }


        private void pictureBoxGrid_MouseUp(object sender, MouseEventArgs e) {
            _mouseDownInGrid = false;
            lblFollowMouse.Visible = false;
            if (_lineRect.Left == -1) {
                return;
            }

             if (paintFromClipboardToolStripMenuItem.Checked && (_systemInterface.Clipboard != null)) {
                var rect = NormalizeRect(_lineRect);
                var clipLen = _systemInterface.Clipboard.GetLength(1);
                rect.Width += clipLen;
                EraseRectangleEntity(rect);
                rect.Width++;
                rect.Height++;
                AddUndoItem(rect, UndoOriginalBehavior.Overwrite, "Chase Lines from Clipboard");
                var brush = new byte[clipLen];
                for (var i = 0; i < clipLen; i++) {
                    brush[i] = _systemInterface.Clipboard[0, i];
                }
                BresenhamLine(_lineRect, brush);
            }
            else {
                EraseRectangleEntity(_lineRect);
                var rect = NormalizeRect(_lineRect);
                rect.Width++;
                rect.Height++;
                AddUndoItem(rect, UndoOriginalBehavior.Overwrite, "Chase Lines");
                BresenhamLine(_lineRect);
            }

            _lineRect.X = -1;
            UpdatePositionLabel(_selectedCells, false);
            //UpdateFollowMouse(keyEvent.Location);
        }


        private void pictureBoxGrid_Paint(object sender, PaintEventArgs e) {
            e.Graphics.FillRectangle(_gridBackBrush, e.ClipRectangle);
            if (_sequence.ChannelCount != 0) {
                var startPoint = new Point();
                var endPoint = new Point();

                var cellCount = e.ClipRectangle.Left / _periodPixelWidth;

                startPoint.Y = e.ClipRectangle.Top;
                endPoint.Y = Math.Min(e.ClipRectangle.Bottom, (_sequence.ChannelCount - vScrollBar1.Value) * _gridRowHeight);

                var pixelLimit = Math.Min(e.ClipRectangle.Right, (_sequence.TotalEventPeriods - hScrollBar1.Value) * _periodPixelWidth);
                var startPixelX = 0;
                while (startPixelX < pixelLimit) {
                    startPixelX = _periodPixelWidth * cellCount;
                    startPoint.X = startPixelX;
                    endPoint.X = startPixelX;
                    e.Graphics.DrawLine(Pens.Gray, startPoint, endPoint);
                    cellCount++;
                }
                cellCount = e.ClipRectangle.Top / _gridRowHeight;
                startPoint.X = e.ClipRectangle.Left;
                endPoint.X = Math.Min(e.ClipRectangle.Right, (_sequence.TotalEventPeriods - hScrollBar1.Value) * _periodPixelWidth);
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
                        startPixelX = (_previousPosition - hScrollBar1.Value) * _periodPixelWidth;
                        e.Graphics.DrawLine(Pens.Gray, startPixelX, 0, startPixelX, startPixelY);
                    }
                    startPixelX = (_position - hScrollBar1.Value) * _periodPixelWidth;
                    e.Graphics.DrawLine(Pens.Yellow, startPixelX, 0, startPixelX, startPixelY);
                }
                else {
                    if (_lineRect.Left != -1) {
                        var left = ((_lineRect.Left - hScrollBar1.Value) * _periodPixelWidth) + (_periodPixelWidth >> 1);
                        var top = ((_lineRect.Top - vScrollBar1.Value) * _gridRowHeight) + (_gridRowHeight >> 1);
                        var width = left + (_lineRect.Width * _periodPixelWidth);
                        var height = top + (_lineRect.Height * _gridRowHeight);
                        e.Graphics.DrawLine(Pens.Blue, left, top, width, height);
                    }
                    else {
                        if (_selectedCells.Left > _sequence.TotalEventPeriods) {
                            _selectedCells.Width = 0;
                        }
                        Rectangle range = Rectangle.Intersect(_selectedCells,
                                                              Rectangle.FromLTRB(hScrollBar1.Value, vScrollBar1.Value,
                                                                                 (hScrollBar1.Value + _visibleEventPeriods) + 1,
                                                                                 (vScrollBar1.Value + _visibleRowCount) + 1));
                        if (!range.IsEmpty) {
                            e.Graphics.FillRectangle(_selectionBrush, RangeToRectangle(range));
                        }
                    }
                    if (toolStripButtonToggleCrossHairs.Checked) {
                        var x = ((_mouseTimeCaret - hScrollBar1.Value) * _periodPixelWidth) + ((int) (_periodPixelWidth * 0.5f));
                        var y = ((_mouseChannelCaret - vScrollBar1.Value) * _gridRowHeight) + ((int) (_gridRowHeight * 0.5f));
                        if (((x > e.ClipRectangle.Left) && (x < e.ClipRectangle.Right)) || ((y > e.ClipRectangle.Top) && (y < e.ClipRectangle.Bottom))) {
                            e.Graphics.DrawLine(Pens.Yellow, x, 0, x, Height);
                            e.Graphics.DrawLine(Pens.Yellow, 0, y, Width, y);
                        }
                    }
                }
            }
        }


        private void pictureBoxGrid_Resize(object sender, EventArgs e) {
            if (!_initializing) {
                VScrollCheck();
                HScrollCheck();
                pictureBoxGrid.Refresh();
            }
        }


        private void pictureBoxTime_Paint(object sender, PaintEventArgs e) {
            int x;
            var timePen = Pens.White;
            var timeBrush = Brushes.White;
            var tickPen = Pens.Red;

            e.Graphics.FillRectangle(_timeBackBrush, e.ClipRectangle);
            var topLeftPt = new Point();
            var bottomRightPt = new Point();
            topLeftPt.Y = pictureBoxTime.Height - 20;
            bottomRightPt.Y = pictureBoxTime.Height - 5;
            if ((e.ClipRectangle.Bottom >= topLeftPt.Y) || (e.ClipRectangle.Top <= bottomRightPt.Y)) {
                var rightmosCell = x = e.ClipRectangle.X / _periodPixelWidth;
                for (x *= _periodPixelWidth; (x < e.ClipRectangle.Right) && ((rightmosCell + hScrollBar1.Value) <= _sequence.TotalEventPeriods);
                     x += _periodPixelWidth) {
                    if (rightmosCell != 0) {
                        topLeftPt.X = x;
                        bottomRightPt.X = x;
                        e.Graphics.DrawLine(timePen, topLeftPt, bottomRightPt);
                    }
                    rightmosCell++;
                }
            }
            topLeftPt.Y = pictureBoxTime.Height - 30;
            if ((e.ClipRectangle.Bottom >= topLeftPt.Y) || (e.ClipRectangle.Top <= bottomRightPt.Y)) {
                x = e.ClipRectangle.X;
                var eventPeriod = _sequence.EventPeriod;
                var startMills = (hScrollBar1.Value + (e.ClipRectangle.Left / ((float) _periodPixelWidth))) * eventPeriod;
                var endMills = Math.Min((hScrollBar1.Value + (e.ClipRectangle.Right / _periodPixelWidth)) * eventPeriod, _sequence.Time);
                var eventCount = (float) _sequence.EventsPerSecond;

                var currentMills = (!(0f.Equals(startMills % 1000f))) ? (((int) startMills) / 1000) * 1000 : (int) startMills;

                while ((x < e.ClipRectangle.Right) && (currentMills <= endMills)) {
                    if (currentMills != 0) {
                        x = e.ClipRectangle.Left + ((int) (((currentMills - startMills) / 1000f) * (_periodPixelWidth * eventCount)));
                        topLeftPt.X = x;
                        bottomRightPt.X = x;
                        e.Graphics.DrawLine(timePen, topLeftPt, bottomRightPt);
                        topLeftPt.X++;
                        bottomRightPt.X++;
                        e.Graphics.DrawLine(timePen, topLeftPt, bottomRightPt);
                        var str = currentMills >= 60000
                                      ? string.Format("{0}:{1:d2}", currentMills / 60000, (currentMills % 60000) / 1000)
                                      : string.Format(":{0:d2}", currentMills / 1000);
                        var ef = e.Graphics.MeasureString(str, _timeFont);
                        e.Graphics.DrawString(str, _timeFont, timeBrush, x - (ef.Width / 2f), topLeftPt.Y - ef.Height - 5f);
                    }
                    currentMills += 1000;
                }
            }

            topLeftPt.Y = pictureBoxTime.Height - 35;
            bottomRightPt.Y = pictureBoxTime.Height - 20;

            if (((e.ClipRectangle.Bottom >= topLeftPt.Y) || (e.ClipRectangle.Top <= bottomRightPt.Y)) && (_showPositionMarker && (_position != -1))) {
                x = _periodPixelWidth * (_position - hScrollBar1.Value);
                if (x < pictureBoxTime.Width) {
                    topLeftPt.X = x;
                    bottomRightPt.X = x;
                    e.Graphics.DrawLine(tickPen, topLeftPt, bottomRightPt);
                }
            }
            if (_mouseTimeCaret != -1) {
                e.Graphics.FillRectangle(_channelCaretBrush, (_mouseTimeCaret - hScrollBar1.Value) * _periodPixelWidth, 0, _periodPixelWidth, 5);
            }
            if (toolStripButtonWaveform.Checked) {
                DrawWaveform(e);
            }
        }


        private void DrawWaveform(PaintEventArgs e) {
            var startPosition = hScrollBar1.Value * _periodPixelWidth;
            var endPosition = Math.Min(startPosition + ((_visibleEventPeriods + 1) * _periodPixelWidth), _waveformPixelData.Length);
            var waveformPosition = 0;

            var wavePen = Pens.White;
            while (startPosition < endPosition) {
                e.Graphics.DrawLine(wavePen, waveformPosition, _waveformOffset - (_waveformPixelData[startPosition] >> 16), waveformPosition,
                                    _waveformOffset - ((short) (_waveformPixelData[startPosition] & 0xffff)));
                waveformPosition++;
                startPosition++;
            }
        }


        private void PlaceSparkle(byte[,] valueArray, int row, int startCol, int decayTime, byte minIntensity, byte maxIntensity) {
            var num = (int) (Math.Round(_sequence.EventsPerSecond, MidpointRounding.AwayFromZero) * 0.1);
            var num2 = (int) Math.Round(((float) decayTime) / _sequence.EventPeriod, MidpointRounding.AwayFromZero);
            if ((startCol + num) >= valueArray.GetLength(1)) {
                num = valueArray.GetLength(1) - startCol;
            }
            if (((startCol + num) + num2) >= valueArray.GetLength(1)) {
                num2 = (valueArray.GetLength(1) - startCol) - num;
                if (num2 < 0) {
                    num2 = 0;
                }
            }
            int num3 = 0;
            while (num3 < num) {
                valueArray[row, startCol + num3] = maxIntensity;
                num3++;
            }
            if (num2 != 0) {
                byte num4 = (byte) ((maxIntensity - minIntensity) / num2);
                byte num5 = (byte) (maxIntensity - num4);
                num3 = startCol + num;
                while (--num2 > 0) {
                    valueArray[row, num3++] = num5;
                    num5 = (byte) (num5 - num4);
                }
            }
        }


        private void playAtTheSelectedPointToolStripMenuItem_Click(object sender, EventArgs e) {
            playAtTheSelectedPointToolStripMenuItem.Checked = true;
            playOnlyTheSelectedRangeToolStripMenuItem.Checked = false;
            tsbPlayFrom.ToolTipText = "Play this sequence starting at the selection startPoint (F6)";
        }


        private void playOnlyTheSelectedRangeToolStripMenuItem_Click(object sender, EventArgs e) {
            playOnlyTheSelectedRangeToolStripMenuItem.Checked = true;
            playAtTheSelectedPointToolStripMenuItem.Checked = false;
            tsbPlayFrom.ToolTipText = "Play the selected range of this sequence (F6)";
        }


        private void plugInItem_CheckedChanged(object sender, EventArgs e) {
            var item = (ToolStripMenuItem) sender;
            var attributes = _sequence.PlugInData.GetPlugInData((string) item.Tag).Attributes;
            if (attributes != null) {
                attributes["enabled"].Value = item.Checked.ToString();
            }
            //bool flag = false;
            //foreach (ToolStripItem item2 in toolStripDropDownButtonPlugins.DropDownItems)
            //{
            //    if (item2 is ToolStripMenuItem)
            //    {
            //        flag |= ((ToolStripMenuItem) item2).Checked;
            //    }
            //}
            IsDirty = true;
        }


        private void printDocument_PrintPage(object sender, PrintPageEventArgs e) {
            //Font font = new Font("Arial", 16f, FontStyle.Bold);
            //Font font2 = new Font("Arial", 12f, FontStyle.Bold);
            //Font font3 = new Font("Arial", 10f, FontStyle.Bold);
            //Font font4 = new Font("Arial", 10f);
            //float top = keyEvent.MarginBounds.Top;
            //float height = keyEvent.Graphics.MeasureString("Mg", font4).Height;
            //SolidBrush brush = new SolidBrush(Color.FromArgb(0xe8, 0xe8, 0xe8));
            //List<OutputPlugin> list = new List<OutputPlugin>(_sequence.PlugInData.GetOutputPlugins());
            //List<string> list2 = new List<string>();
            //if (_printingChannelIndex == 0)
            //{
            //    keyEvent.Graphics.DrawString(_sequence.Name, font, Brushes.Black, (float) keyEvent.MarginBounds.Left, top);
            //    top += keyEvent.Graphics.MeasureString("Mg", font).Height;
            //    keyEvent.Graphics.DrawString(DateTime.Today.ToShortDateString(), font2, Brushes.Black, (float) keyEvent.MarginBounds.Left, top);
            //    top += keyEvent.Graphics.MeasureString("Mg", font2).Height;
            //    Pen pen = new Pen(Brushes.Black, 3f);
            //    keyEvent.Graphics.DrawLine(pen, (float) keyEvent.MarginBounds.Left, top, (float) keyEvent.MarginBounds.Right, top);
            //    pen.Dispose();
            //    top += 20f;
            //    keyEvent.Graphics.FillRectangle(brush, (float) keyEvent.MarginBounds.Left, top, (float) keyEvent.MarginBounds.Width, 24f);
            //    keyEvent.Graphics.DrawRectangle(Pens.Black, (float) keyEvent.MarginBounds.Left, top, (float) keyEvent.MarginBounds.Width, 24f);
            //    top += 4f;
            //    keyEvent.Graphics.DrawString("Name", font3, Brushes.Black, (float) (keyEvent.MarginBounds.Left + 10), top);
            //    keyEvent.Graphics.DrawString("Output #", font3, Brushes.Black, (float) (keyEvent.MarginBounds.Left + 0x113), top);
            //    keyEvent.Graphics.DrawString("Controller", font3, Brushes.Black, (float) (keyEvent.MarginBounds.Left + 350), top);
            //    top += 30f;
            //}
            //while ((_printingChannelIndex < _sequence.ChannelCount) && ((top + height) < keyEvent.MarginBounds.Bottom))
            //{
            //    if ((_printingChannelIndex % 2) == 1)
            //    {
            //        keyEvent.Graphics.FillRectangle(brush, (float) keyEvent.MarginBounds.Left, top - 1f, (float) keyEvent.MarginBounds.Width, height);
            //    }
            //    int num3 = _printingChannelList[_printingChannelIndex].OutputChannel + 1;
            //    keyEvent.Graphics.DrawString(_printingChannelList[_printingChannelIndex].Name, font4, Brushes.Black, (float) (keyEvent.MarginBounds.Left + 10), top);
            //    keyEvent.Graphics.DrawString(num3.ToString(), font4, Brushes.Black, (float) (keyEvent.MarginBounds.Left + 0x113), top);
            //    list2.Clear();
            //    foreach (OutputPlugin plugin in list)
            //    {
            //        if ((plugin.Enabled && (plugin.ChannelFrom <= num3)) && (plugin.ChannelTo >= num3))
            //        {
            //            list2.Add(plugin.Name);
            //        }
            //    }
            //    keyEvent.Graphics.DrawString(string.Join(", ", list2.ToArray()), font4, Brushes.Black, (float) (keyEvent.MarginBounds.Left + 350), top);
            //    _printingChannelIndex++;
            //    top += height;
            //}
            //brush.Dispose();
            //font.Dispose();
            //font2.Dispose();
            //font3.Dispose();
            //font4.Dispose();
            //keyEvent.HasMorePages = _printingChannelIndex < _sequence.ChannelCount;
        }


        private void ProgramEnded() {
            m_positionTimer.Stop();
            SetEditingState(true);
            pictureBoxGrid.Refresh();
        }


        private void quarterSpeedToolStripMenuItem_Click(object sender, EventArgs e) {
            SetAudioSpeed(0.25f);
        }


        private void Ramp(int startingLevel, int endingLevel) {
            var originalAction = (startingLevel > endingLevel) ? "Fade" : "Ramp";
            if ((startingLevel != _sequence.MinimumLevel && endingLevel != _sequence.MinimumLevel) ||
                (startingLevel != _sequence.MaximumLevel && endingLevel != _sequence.MaximumLevel)) {
                originalAction = "Partial " + originalAction;
            }
            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, originalAction);
            var bottom = _selectedCells.Bottom;
            var right = _selectedCells.Right;
            var left = _selectedCells.Left;
            for (var top = _selectedCells.Top; top < bottom; top++) {
                var channelOrder = _channelOrderMapping[top];
                if (_sequence.Channels[channelOrder].Enabled) {
                    for (var column = left; column < right; column++) {
                        var computedLevel =
                            (byte) ((((column - left) / ((float) ((right - left) - 1))) * (endingLevel - startingLevel)) + startingLevel);
                        if (computedLevel < _sequence.MinimumLevel) {
                            computedLevel = _sequence.MinimumLevel;
                        }
                        else if (computedLevel > _sequence.MaximumLevel) {
                            computedLevel = _sequence.MaximumLevel;
                        }
                        _sequence.EventValues[channelOrder, column] = computedLevel;
                    }
                }
            }
            _selectionRectangle.Width = 0;
            pictureBoxGrid.Invalidate(SelectionToRectangle());
        }


        private Rectangle RangeToRectangle(Rectangle range) {
            return new Rectangle {
                                     X = ((range.X - hScrollBar1.Value) * _periodPixelWidth) + 1, Y = ((range.Y - vScrollBar1.Value) * _gridRowHeight) + 1,
                                     Width = (range.Width * _periodPixelWidth) - 1, Height = (range.Height * _gridRowHeight) - 1
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
            //TODO: This is always false for a reason right now
            //bool flag = _sequence.Profile != null;
            var flag = false;
            flattenProfileIntoSequenceToolStripMenuItem.Enabled = flag;
            detachSequenceFromItsProfileToolStripMenuItem.Enabled = flag;
            channelOutputMaskToolStripMenuItem.Enabled = !flag;
            ReactEditingStateToProfileAssignment();
            SetOrderArraySize(_sequence.ChannelCount);
            textBoxChannelCount.Text = _sequence.ChannelCount.ToString();
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
            if (_redoStack.Count != 0) {
                var item = (UndoItem) _redoStack.Pop();
                var height = 0;
                var width = 0;
                if (item.Data != null) {
                    height = item.Data.GetLength(0);
                    width = item.Data.GetLength(1);
                }
                toolStripButtonRedo.Enabled = redoToolStripMenuItem.Enabled = _redoStack.Count > 0;
                IsDirty = true;
                var item2 = new UndoItem(item.Location,
                                         GetAffectedBlockData(new Rectangle(item.Location.X, item.Location.Y, item.Data.GetLength(1),
                                                                            item.Data.GetLength(0))), item.Behavior, _sequence, _channelOrderMapping, item.OriginalAction);
                switch (item.Behavior) {
                    case UndoOriginalBehavior.Overwrite:
                        DisjointedOverwrite(item.Location.X, item.Data, item.ReferencedChannels);
                        pictureBoxGrid.Invalidate(new Rectangle((item.Location.X - hScrollBar1.Value) * _periodPixelWidth,
                                                                (item.Location.Y - vScrollBar1.Value) * _gridRowHeight, width * _periodPixelWidth,
                                                                height * _gridRowHeight));
                        break;

                    case UndoOriginalBehavior.Removal:
                        DisjointedRemove(item.Location.X, width, height, item.ReferencedChannels);
                        pictureBoxGrid.Refresh();
                        break;

                    case UndoOriginalBehavior.Insertion:
                        DisjointedInsert(item.Location.X, width, height, item.ReferencedChannels);
                        DisjointedOverwrite(item.Location.X, item.Data, item.ReferencedChannels);
                        pictureBoxGrid.Refresh();
                        break;
                }
                UpdateRedoText();
                _undoStack.Push(item2);
                toolStripButtonUndo.Enabled = undoToolStripMenuItem.Enabled = true;
                UpdateUndoText();
            }
        }


        private void RefreshAll() {
            SetOrderArraySize(_sequence.ChannelCount);
            textBoxChannelCount.Text = _sequence.ChannelCount.ToString();
            textBoxProgramLength.Text = TimeString(_sequence.Time);
            pictureBoxGrid.Refresh();
            pictureBoxChannels.Refresh();
            pictureBoxTime.Refresh();
            VScrollCheck();
            HScrollCheck();
        }


        private void ReorderChannelOutputs() {
            if (_sequence.Profile != null) {
                MessageBox.Show("This sequence is attached to a profile.\nChanges made to the profile's channel outputs will be reflected here.",
                                Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                ChannelOrderDialog dialog = new ChannelOrderDialog(_sequence.OutputChannels, null);
                dialog.Text = "Channel Output Mapping";
                if (dialog.ShowDialog() == DialogResult.OK) {
                    List<VixenPlus.Channel> channelMapping = dialog.ChannelMapping;
                    foreach (VixenPlus.Channel channel in _sequence.Channels) {
                        channel.OutputChannel = channelMapping.IndexOf(channel);
                    }
                    if (_showingOutputs) {
                        pictureBoxChannels.Refresh();
                        pictureBoxGrid.Refresh();
                    }
                    IsDirty = true;
                }
                dialog.Dispose();
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
            VixenEditor.ToolStripManager.LoadSettings(this, _preferences.XmlDoc.DocumentElement, "reset");
            foreach (ToolStripItem item in toolbarsToolStripMenuItem.DropDownItems) {
                if ((item is ToolStripMenuItem) && (item.Tag != null)) {
                    ((ToolStripMenuItem) item).Checked = true;
                }
            }
        }


        public override DialogResult RunWizard(ref EventSequence resultSequence) {
            NewSequenceWizardDialog dialog = new NewSequenceWizardDialog(_systemInterface.UserPreferences);
            if (dialog.ShowDialog() == DialogResult.OK) {
                resultSequence = dialog.Sequence;
                dialog.Dispose();
                return DialogResult.OK;
            }
            dialog.Dispose();
            return DialogResult.Cancel;
        }


        private void saveAsARoutineToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult result;
            var path = string.Empty;
            var dialog = new TextQueryDialog(Vendor.ProductName, "Name of the routine", string.Empty);
            do {
                if ((result = dialog.ShowDialog()) == DialogResult.OK) {
                    path = Path.Combine(Paths.RoutinePath, Path.GetFileNameWithoutExtension(dialog.Response) + ".vir");
                    if (File.Exists(path)) {
                        result = MessageBox.Show("File already exists.  Overwrite?", Vendor.ProductName, MessageBoxButtons.YesNoCancel,
                                                 MessageBoxIcon.Question);
                    }
                }
            } while (result == DialogResult.No);
            dialog.Dispose();
            if (result != DialogResult.Cancel) {
                var writer = new StreamWriter(path);
                var buffer = CellsToArray();
                for (var i = 0; i < buffer.GetLength(0); i++) {
                    for (var j = 0; j < buffer.GetLength(1); j++) {
                        writer.Write(buffer[i, j].ToString(CultureInfo.InvariantCulture) + " ");
                    }
                    writer.WriteLine();
                }
                writer.Close();
                MessageBox.Show(string.Format("Routine \"{0}\" has been saved", Path.GetFileNameWithoutExtension(path)), Vendor.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }


        public override void SaveTo(string filePath) {
            _sequence.SaveTo(filePath);
            IsDirty = false;
        }


        private void saveToolbarPositionsToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripManager.SaveSettings(this, _preferences.XmlDoc.DocumentElement);
            _preferences.Flush();
            MessageBox.Show("Toolbar Settings Saved.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        private void scaleToolStripMenuItem_Click(object sender, EventArgs e) {
            ArithmeticPaste(ArithmeticOperation.Scale);
        }

        //todo cellY should not be passed
        private void ScrollSelectionDown(int cellX, int cellY) {
            var num = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Left, pictureBoxGrid.Bottom)).Y - pictureBoxTime.Height;
            _selectedRange.Width = (cellX + 1) - _selectedRange.Left;
            while (MousePosition.Y > num) {
                cellY = pictureBoxGrid.PointToClient(MousePosition).Y / _periodPixelWidth;
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
            }
        }

        //todo cellX should not be passed
        private void ScrollSelectionLeft(int cellX, int cellY) {
            int x = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Left, pictureBoxGrid.Top)).X;
            while ((MousePosition.X < x) && (hScrollBar1.Value != 0)) {
                _selectedRange.Height = (cellY + 1) - _selectedRange.Top;
                _selectedRange.Width--;
                _selectedCells = NormalizeRect(_selectedRange);
                hScrollBar1.Value--;
            }
        }

        //todo cellX should not be passed
        private void ScrollSelectionRight(int cellX, int cellY) {
            var x = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Right, pictureBoxGrid.Top)).X;
            _selectedRange.Height = (cellY + 1) - _selectedRange.Top;
            while (MousePosition.X > x) {
                cellX = pictureBoxGrid.PointToClient(MousePosition).X / _periodPixelWidth;
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
            }
        }


        private void ScrollSelectionUp(int cellX) {
            var y = pictureBoxGrid.PointToScreen(new Point(pictureBoxGrid.Left, pictureBoxGrid.Top)).Y;
            while ((MousePosition.Y < y) && (vScrollBar1.Value != 0)) {
                _selectedRange.Width = (cellX + 1) - _selectedRange.Left;
                _selectedRange.Height--;
                _selectedCells = NormalizeRect(_selectedRange);
                vScrollBar1.Value--;
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
            if (toolStripDropDownButtonPlugins.DropDownItems.Count > 3) {}
        }


        private Rectangle SelectionToRectangle() {
            return new Rectangle {   X = ((_selectedCells.X - hScrollBar1.Value) * _periodPixelWidth) + 1,
                                     Y = ((_selectedCells.Y - vScrollBar1.Value) * _gridRowHeight) + 1, 
                                     Width = (_selectedCells.Width * _periodPixelWidth) - 1,
                                     Height = (_selectedCells.Height * _gridRowHeight) - 1
                                 };
        }


        private void setAllChannelColorsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_sequence.Profile != null) {
                MessageBox.Show(
                    "This sequence is attached to a profile.\nChanges to the profile affect all sequences attached to it.\n\nIf this is what you want to do, please use the profile manager.",
                    Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                var dialog = new AllChannelsColorDialog(_sequence.Channels);
                if (dialog.ShowDialog() == DialogResult.OK) {
                    var channelCount = _sequence.ChannelCount;
                    var channelColors = dialog.ChannelColors;
                    for (var i = 0; i < channelCount; i++) {
                        _sequence.Channels[i].Color = channelColors[i];
                    }
                }
                dialog.Dispose();
                IsDirty = true;
                pictureBoxChannels.Refresh();
                pictureBoxGrid.Refresh();
            }
        }


        private void SetAudioSpeed(float rate) {
            if ((rate > 0f) && (rate <= 1f)) {
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
                _executionInterface.SetAudioSpeed(_executionContextHandle, rate);
            }
        }


        private void SetChannelCount(int count) {
            if (count != _sequence.ChannelCount) {
                int num;
                bool flag = false;
                int num2 = Math.Min(_sequence.ChannelCount, count);
                for (num = 0; num < num2; num++) {
                    if (_sequence.Channels[num].OutputChannel > (count - 1)) {
                        flag = true;
                        break;
                    }
                }
                if (flag) {
                    if (
                        MessageBox.Show(
                            "With the new channel count, some channels would refer to outputs that no longer exist.\nTo keep the sequence valid, channel outputs would have to be reset.\n\nDo you want to keep the new channel count?",
                            Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes) {
                        textBoxChannelCount.Text = _sequence.ChannelCount.ToString(CultureInfo.InvariantCulture);
                        return;
                    }
                    for (num = 0; num < _sequence.ChannelCount; num++) {
                        _sequence.Channels[num].OutputChannel = num;
                    }
                }
                SetOrderArraySize(count);
                _sequence.ChannelCount = count;
                textBoxChannelCount.Text = count.ToString(CultureInfo.InvariantCulture);
                VScrollCheck();
                pictureBoxChannels.Refresh();
                pictureBoxGrid.Refresh();
                IsDirty = true;
                MessageBox.Show("Channel count has been updated.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }


        private void SetDrawingLevel(byte level) {
            _drawingLevel = level;
            toolStripLabelCurrentDrawingIntensity.Text = _actualLevels
                                                    ? level.ToString(CultureInfo.InvariantCulture)
                                                    : string.Format("{0}%", (int)Math.Round(level * 100f / 255f,MidpointRounding.AwayFromZero));
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
            ReactEditingStateToProfileAssignment();
        }


        private void SetOrderArraySize(int count) {
            if (count < _channelOrderMapping.Count) {
                var list = new List<int>();
                list.AddRange(_channelOrderMapping);
                foreach (var num in list) {
                    if (num >= count) {
                        _channelOrderMapping.Remove(num);
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


        private bool SetProgramTime(int milliseconds) {
            try {
                _sequence.Time = milliseconds;
            }
            catch {
                MessageBox.Show("Cannot set the sequence length.\nThere is audio associated which would exceed that length.", Vendor.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Hand);
                textBoxProgramLength.Text = TimeString(_sequence.Time);
                return false;
            }
            textBoxProgramLength.Text = TimeString(_sequence.Time);
            HScrollCheck();
            pictureBoxTime.Refresh();
            pictureBoxGrid.Refresh();
            return true;
        }


        private void SetVariablePlaybackSpeed(Point dialogScreenCoords) {
            var dialog = new AudioSpeedDialog();
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
            dialog.Dispose();
        }


        private void ShowChannelProperties() {
            var channels = new List<VixenPlus.Channel>();
            channels.AddRange(_sequence.Channels);
            for (var i = 0; i < channels.Count; i++) {
                channels[i] = _sequence.Channels[_channelOrderMapping[i]];
            }
            var dialog = new ChannelPropertyDialog(channels, SelectedChannel, true);
            dialog.ShowDialog();
            dialog.Dispose();
            pictureBoxChannels.Refresh();
            pictureBoxGrid.Refresh();
            IsDirty = true;
        }


        private void sortByChannelNumberToolStripMenuItem_Click(object sender, EventArgs e) {
            _printingChannelList = _sequence.Channels;
            if (printDialog.ShowDialog() == DialogResult.OK) {
                _printDocument.DocumentName = "Vixen channel configuration";
                //_printingChannelIndex = 0;
                _printDocument.Print();
            }
        }


        private void sortByChannelOutputToolStripMenuItem_Click(object sender, EventArgs e) {
            _printingChannelList = new List<VixenPlus.Channel>();
            var collection = new VixenPlus.Channel[_sequence.ChannelCount];
            foreach (var channel in _sequence.Channels) {
                collection[channel.OutputChannel] = channel;
            }
            _printingChannelList.AddRange(collection);
            if (printDialog.ShowDialog() == DialogResult.OK) {
                _printDocument.DocumentName = "Vixen channel configuration";
                //_printingChannelIndex = 0;
                _printDocument.Print();
            }
        }


        private void SparkleGenerator(byte[,] values, params int[] effectParameters) {
            var num = (int) _sequence.EventsPerSecond; // 1000 / _sequence.EventPeriod;
            var maxValue = num - effectParameters[0];
            var decayTime = effectParameters[1];
            for (var i = 0; i < values.GetLength(0); i++) {
                for (var k = 0; k < values.GetLength(1); k++) {
                    values[i, k] = _sequence.MinimumLevel;
                }
            }
            //var eventCount = (int) Math.Round(decayTime / ((float) _sequence.EventPeriod), MidpointRounding.AwayFromZero);
            var length = values.GetLength(0);
            var num8 = values.GetLength(1);
            var numArray = new int[num8];
            var index = 0;
            var random = new Random();
            while (index < num8) {
                numArray[index] = random.Next(length) + 1;
                var num10 = random.Next(maxValue);
                index += Math.Max(num10, 1);
            }
            for (var j = 0; j < numArray.Length; j++) {
                if (numArray[j] != 0) {
                    PlaceSparkle(values, numArray[j] - 1, j, decayTime, (byte) effectParameters[2], (byte) effectParameters[3]);
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
                if (_executionInterface.EngineStatus(_executionContextHandle) != 0) {
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

            var isNotRunning = _executionInterface.EngineStatus(_executionContextHandle) != 1;
            
            // Keys here only work on selected areas if they exist.
            if (!keyEvent.Handled && _selectedCells.Width > 0 ) {
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
            if (e.Control && e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) {
                var index = ((int)e.KeyCode) - (int)Keys.D0;
                e.Handled = true;

                if (e.Shift) {
                    _bookmarks[index] = (_bookmarks[index] == _selectedCells.Left) ? -1 : _selectedCells.Left;
                    pictureBoxTime.Refresh();
                }
                else if (_bookmarks[index] != -1) {
                    hScrollBar1.Value = _bookmarks[index];
                }
            }
        }

        // todo refactor
        private void HandleSpaceAndArrowKeys(KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Space: {
                    if ((SelectableControlFocused() && !pictureBoxChannels.Focused) && !pictureBoxGrid.Focused) {
                        break;
                    }
                    
                    int currentPosition;
                    if (_executionInterface.EngineStatus(_executionContextHandle, out currentPosition) != 1) {
                        
                        var nonZeroCellCount = 0;

                        for (var top = _selectedCells.Top; top < _selectedCells.Bottom; top++) {
                            var channel = _channelOrderMapping[top];
                            for (var left = _selectedCells.Left; left < _selectedCells.Right; left++ ) {
                                if (_sequence.EventValues[channel, left] > _sequence.MinimumLevel) {
                                    nonZeroCellCount++;
                                }
                            }
                        }

                        var selectedCellsCount = _selectedCells.Height * _selectedCells.Width;
                        var level = _drawingLevel;
                        var originalAction = "On";
                        if (nonZeroCellCount == selectedCellsCount) {
                            level = _sequence.MinimumLevel;
                            originalAction = "Off";
                        }

                        AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite, originalAction);

                        for (var top = _selectedCells.Top; top < _selectedCells.Bottom; top++) {
                            var channel = _channelOrderMapping[top];
                            for (var left = _selectedCells.Left; left < _selectedCells.Right; left++) {
                                _sequence.EventValues[channel, left] = level;
                            }
                        }

                        pictureBoxGrid.Invalidate(new Rectangle((_selectedCells.Left - hScrollBar1.Value) * _periodPixelWidth,
                                                                (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight,
                                                                _selectedCells.Width * _periodPixelWidth, _selectedCells.Height * _gridRowHeight));
                        e.Handled = true;
                        break;
                    }
                    var currentEvent = currentPosition / _sequence.EventPeriod;
                    AddUndoItem(new Rectangle(currentEvent, _selectedCells.Top, 1, _selectedCells.Height), UndoOriginalBehavior.Overwrite,"On");
                    
                    for (int i = _selectedCells.Top; i < _selectedCells.Bottom; i++) {
                        var channel = _channelOrderMapping[i];
                        _sequence.EventValues[channel, currentEvent] = _drawingLevel;
                    }
                    pictureBoxGrid.Invalidate(new Rectangle((currentEvent - hScrollBar1.Value) * _periodPixelWidth,
                                                            (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight, _periodPixelWidth,
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
                        pictureBoxGrid.Invalidate(new Rectangle((_selectedCells.Left - hScrollBar1.Value) * _periodPixelWidth,
                                                                (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight,
                                                                (_selectedCells.Width + 1) * _periodPixelWidth,
                                                                _selectedCells.Height * _gridRowHeight));
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
                            pictureBoxGrid.Invalidate(new Rectangle((_selectedCells.Left - hScrollBar1.Value) * _periodPixelWidth,
                                                                    (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight,
                                                                    _selectedCells.Width * _periodPixelWidth,
                                                                    (_selectedCells.Height + 1) * _gridRowHeight));
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
                                pictureBoxGrid.Invalidate(new Rectangle(((_selectedCells.Left - hScrollBar1.Value) - 1) * _periodPixelWidth,
                                                                        (_selectedCells.Top - vScrollBar1.Value) * _gridRowHeight,
                                                                        (_selectedCells.Width + 1) * _periodPixelWidth,
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
                            pictureBoxGrid.Invalidate(new Rectangle((_selectedCells.Left - hScrollBar1.Value) * _periodPixelWidth,
                                                                    ((_selectedCells.Top - vScrollBar1.Value) - 1) * _gridRowHeight,
                                                                    _selectedCells.Width * _periodPixelWidth,
                                                                    (_selectedCells.Height + 1) * _gridRowHeight));
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
            if (((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down)) && e.Control) {
                IntensityAdjustDialogCheck();
                var change = e.Alt ? 1 : _intensityLargeDelta;
                m_intensityAdjustDialog.Delta = (e.KeyCode == Keys.Up) ? change : -change;
                e.Handled = true;
            }
        }


        // refactored and tested
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
                        int num2 = Math.Min(_sequence.ChannelCount - vScrollBar1.Value, _visibleRowCount);
                        int num3 = Math.Min(_sequence.ChannelCount - _selectedCells.Bottom, _visibleRowCount);
                        var newPosition = Math.Min(num2, num3);
                        _selectedRange.Y += newPosition;
                        _selectedCells.Y += newPosition;
                        vScrollBar1.Value += newPosition;
                        e.Handled = true;
                    }
                    break;

                case Keys.End:
                    if (hScrollBar1.Value < _sequence.TotalEventPeriods - _visibleEventPeriods) {
                        int positionFromEnd = _sequence.TotalEventPeriods - _visibleEventPeriods;
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
                        if (e.Alt) {
                            using (var dialog = new DelayedStartDialog()) {
                                if (dialog.ShowDialog() != DialogResult.OK) {
                                    return;
                                }
                            }
                        }
                        toolStripButtonPlay_Click(null, null);
                        e.Handled = true;
                    }
                    break;

                case Keys.F6:
                    if (tsbPlayFrom.Enabled) {
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
                    } else if (_sequence.Profile == null && _sequence.ChannelCount > 0) {
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
                        (MessageBox.Show(string.Format("Delete channel {0}?", SelectedChannel.Name), "Confirm", MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question) == DialogResult.Yes)) {
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
            var panelArray = new ToolStripPanel[] {
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
            var position = 4; //todo this should be resolved dynamically
            foreach (var str in list) {
                var item = new ToolStripMenuItem(str) {Tag = _toolStrips[str], Checked = _toolStrips[str].Visible, CheckOnClick = true};
                item.CheckStateChanged += _toolStripCheckStateChangeHandler;
                toolbarsToolStripMenuItem.DropDownItems.Insert(position++, item);
            }
            _actualLevels = _preferences.GetBoolean("ActualLevels");
            UpdateToolbarMenu();
            UpdateLevelDisplay();
        }


        private void subtractionToolStripMenuItem_Click(object sender, EventArgs e) {
            ArithmeticPaste(ArithmeticOperation.Subtraction);
        }


        private void SyncAudioButton() {
            tsbAudio.Checked = _sequence.Audio != null;
            tsbAudio.ToolTipText = (_sequence.Audio != null) ? _sequence.Audio.Name : "Add audio";
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


        private string TimeString(int milliseconds) {
            return string.Format("{0:d2}:{1:d2}.{2:d3}", milliseconds / 60000, (milliseconds % 60000) / 1000, milliseconds % 1000);
        }


        private void toggleOutputChannelsToolStripMenuItem_Click(object sender, EventArgs e) {
            _showingOutputs = !_showingOutputs;
            pictureBoxChannels.Refresh();
        }


        private void toolStripButtonAudio_Click(object sender, EventArgs e) {
            var audio = _sequence.Audio;
            var integer = _preferences.GetInteger("SoundDevice");
            Cursor = Cursors.WaitCursor;
            var dialog = new AudioDialog(_sequence, _preferences.GetBoolean("EventSequenceAutoSize"), integer);
            Cursor = Cursors.Default;
            var audio2 = _sequence.Audio;
            if (dialog.ShowDialog() == DialogResult.OK) {
                SetProgramTime(_sequence.Time);
                IsDirty = true;
                pictureBoxGrid.Refresh();
            }
            SyncAudioButton();
            IsDirty |= audio2 != _sequence.Audio;
            if (audio != _sequence.Audio) {
                ParseAudioWaveform();
                pictureBoxTime.Refresh();
            }
            dialog.Dispose();
        }


        private void toolStripButtonChangeIntensity_Click(object sender, EventArgs e) {
            var dialog = new DrawingIntensityDialog(_sequence, _drawingLevel, _actualLevels);
            if (dialog.ShowDialog() == DialogResult.OK) {
                SetDrawingLevel(dialog.SelectedIntensity);
            }
            dialog.Dispose();
        }


        private void toolStripButtonChannelOutputMask_Click(object sender, EventArgs e) {
            EditSequenceChannelMask();
        }


        private void toolStripButtonCopy_Click(object sender, EventArgs e) {
            CopyCells();
        }


        private void toolStripButtonCut_Click(object sender, EventArgs e) {
            CopyCells();
            TurnCellsOff();
        }


        private void toolStripButtonDeleteOrder_Click(object sender, EventArgs e) {
            if (
                MessageBox.Show(string.Format("Delete channel order '{0}'?", toolStripComboBoxChannelOrder.Text), Vendor.ProductName,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                _sequence.Sorts.Remove((VixenPlus.SortOrder) toolStripComboBoxChannelOrder.SelectedItem);
                toolStripComboBoxChannelOrder.Items.RemoveAt(toolStripComboBoxChannelOrder.SelectedIndex);
                toolStripButtonDeleteOrder.Enabled = false;
                IsDirty = true;
            }
        }


        private void toolStripButtonFindAndReplace_Click(object sender, EventArgs e) {
            if ((_selectedCells.Width == 0) || (_selectedCells.Height == 0)) {
                MessageBox.Show("There are no cells to search", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                var dialog = new FindAndReplaceDialog(_sequence.MinimumLevel, _sequence.MaximumLevel, _actualLevels);
                if (dialog.ShowDialog() == DialogResult.OK) {
                    int left;
                    int num4;
                    int num5;
                    AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite,"Find and Replace");
                    var findValue = dialog.FindValue;
                    var replaceWithValue = dialog.ReplaceWithValue;
                    if (_actualLevels) {
                        for (num5 = _selectedCells.Top; num5 < _selectedCells.Bottom; num5++) {
                            num4 = _channelOrderMapping[num5];
                            left = _selectedCells.Left;
                            while (left < _selectedCells.Right) {
                                if (_sequence.EventValues[num4, left] == findValue) {
                                    _sequence.EventValues[num4, left] = replaceWithValue;
                                }
                                left++;
                            }
                        }
                    }
                    else {
                        for (num5 = _selectedCells.Top; num5 < _selectedCells.Bottom; num5++) {
                            num4 = _channelOrderMapping[num5];
                            for (left = _selectedCells.Left; left < _selectedCells.Right; left++) {
                                if ((byte) Math.Round(_sequence.EventValues[num4, left] * 100f / 255f, MidpointRounding.AwayFromZero) == findValue) {
                                    _sequence.EventValues[num4, left] =
                                        (byte) Math.Round(replaceWithValue / 100f * 255f, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                    }
                    IsDirty = true;
                    pictureBoxGrid.Refresh();
                }
                dialog.Dispose();
            }
        }


        private void toolStripButtonInsertPaste_Click(object sender, EventArgs e) {
            if (_systemInterface.Clipboard != null) {
                byte[,] clipboard = _systemInterface.Clipboard;
                int length = clipboard.GetLength(0);
                int width = clipboard.GetLength(1);
                int left = _selectedCells.Left;
                int num5 = left + width;
                int num6 = _sequence.TotalEventPeriods - num5;
                for (int i = 0; (i < length) && ((_selectedCells.Top + i) < _sequence.ChannelCount); i++) {
                    int num7 = _channelOrderMapping[_selectedCells.Top + i];
                    for (int j = Math.Min(num6, _sequence.TotalEventPeriods - _selectedCells.Left) - 1; j >= 0; j--) {
                        _sequence.EventValues[num7, num5 + j] = _sequence.EventValues[num7, left + j];
                    }
                }
                PasteOver();
                AddUndoItem(new Rectangle(_selectedCells.X, _selectedCells.Y, width, length), UndoOriginalBehavior.Insertion,"Insert Paste");
            }
        }


        private void toolStripButtonIntensity_Click(object sender, EventArgs e) {
            int result = 0;
            bool flag = false;
            while (!flag) {
                string str;
                TextQueryDialog dialog;
                flag = true;
                if (_actualLevels) {
                    str = "255";
                    if ((_selectedCells.Width == 1) && (_selectedCells.Height == 1)) {
                        str =
                            _sequence.EventValues[_channelOrderMapping[_selectedCells.Top], _selectedCells.Left].ToString(
                                CultureInfo.InvariantCulture);
                    }
                    dialog = new TextQueryDialog(Vendor.ProductName, "What intensity level (0-255)?", str);
                    if (dialog.ShowDialog() != DialogResult.OK) {
                        dialog.Dispose();
                        return;
                    }
                    if (!int.TryParse(dialog.Response, out result)) {
                        MessageBox.Show("Not a valid number.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        flag = false;
                    }
                    if ((result < 0) || (result > 0xff)) {
                        MessageBox.Show("Not a valid value.\nPlease select a value between 0 and 255.", Vendor.ProductName, MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                        flag = false;
                    }
                    dialog.Dispose();
                }
                else {
                    str = "100";
                    if ((_selectedCells.Width == 1) && (_selectedCells.Height == 1)) {
                        str =
                            ((int)
                             Math.Round(
                                 (double) ((_sequence.EventValues[_channelOrderMapping[_selectedCells.Top], _selectedCells.Left] * 100f) / 255f),
                                 MidpointRounding.AwayFromZero)).ToString(CultureInfo.InvariantCulture);
                    }
                    dialog = new TextQueryDialog(Vendor.ProductName, "What % intensity (0-100)?", str);
                    if (dialog.ShowDialog() != DialogResult.OK) {
                        dialog.Dispose();
                        return;
                    }
                    try {
                        result = (int) Math.Round((double) ((Convert.ToSingle(dialog.Response) * 255f) / 100f), MidpointRounding.AwayFromZero);
                        if ((result < 0) || (result > 0xff)) {
                            MessageBox.Show("Not a valid value.\nPlease select a value between 0 and 100.", Vendor.ProductName, MessageBoxButtons.OK,
                                            MessageBoxIcon.Exclamation);
                            flag = false;
                        }
                    }
                    catch {
                        MessageBox.Show("Not a valid number.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        flag = false;
                    }
                    finally {
                        dialog.Dispose();
                    }
                }
            }
            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite,"Intensity");
            int bottom = _selectedCells.Bottom;
            int right = _selectedCells.Right;
            for (int i = _selectedCells.Top; i < bottom; i++) {
                int num5 = _channelOrderMapping[i];
                if (_sequence.Channels[num5].Enabled) {
                    for (int j = _selectedCells.Left; j < right; j++) {
                        _sequence.EventValues[num5, j] = (byte) result;
                    }
                }
            }
            _selectionRectangle.Width = 0;
            pictureBoxGrid.Invalidate(SelectionToRectangle());
        }


        private void toolStripButtonInvert_Click(object sender, EventArgs e) {
            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite,"Invert");
            int bottom = _selectedCells.Bottom;
            int right = _selectedCells.Right;
            for (int i = _selectedCells.Top; i < bottom; i++) {
                int num4 = _channelOrderMapping[i];
                if (_sequence.Channels[num4].Enabled) {
                    for (int j = _selectedCells.Left; j < right; j++) {
                        _sequence.EventValues[num4, j] = (byte) (_sequence.MaximumLevel - _sequence.EventValues[num4, j]);
                    }
                }
            }
            _selectionRectangle.Width = 0;
            pictureBoxGrid.Invalidate(SelectionToRectangle());
        }


        private void toolStripButtonLoop_CheckedChanged(object sender, EventArgs e) {
            _executionInterface.SetLoopState(_executionContextHandle, tsbLoop.Checked);
        }


        private void toolStripButtonMirrorHorizontal_Click(object sender, EventArgs e) {
            var buffer = new byte[_selectedCells.Height,_selectedCells.Width];
            for (var i = 0; i < _selectedCells.Height; i++) {
                var num3 = _channelOrderMapping[_selectedCells.Top + i];
                var num2 = 0;
                var num = _selectedCells.Width - 1;
                while (num >= 0) {
                    buffer[i, num2] = _sequence.EventValues[num3, _selectedCells.Left + num];
                    num--;
                    num2++;
                }
            }
            _systemInterface.Clipboard = buffer;
        }


        private void toolStripButtonMirrorVertical_Click(object sender, EventArgs e) {
            var buffer = new byte[_selectedCells.Height,_selectedCells.Width];
            var num2 = 0;
            var num4 = _selectedCells.Height - 1;
            while (num4 >= 0) {
                var num3 = _channelOrderMapping[_selectedCells.Top + num4];
                for (var i = 0; i < _selectedCells.Width; i++) {
                    buffer[num2, i] = _sequence.EventValues[num3, _selectedCells.Left + i];
                }
                num4--;
                num2++;
            }
            _systemInterface.Clipboard = buffer;
        }


        private void toolStripButtonOff_Click(object sender, EventArgs e) {
            TurnCellsOff();
        }


        private void toolStripButtonOn_Click(object sender, EventArgs e) {
            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite,"On");
            int bottom = _selectedCells.Bottom;
            int right = _selectedCells.Right;
            for (int i = _selectedCells.Top; i < bottom; i++) {
                int num4 = _channelOrderMapping[i];
                if (_sequence.Channels[num4].Enabled) {
                    for (int j = _selectedCells.Left; j < right; j++) {
                        _sequence.EventValues[num4, j] = _drawingLevel;
                    }
                }
            }
            _selectionRectangle.Width = 0;
            pictureBoxGrid.Invalidate(SelectionToRectangle());
        }


        private void toolStripButtonOpaquePaste_Click(object sender, EventArgs e) {
            if (_systemInterface.Clipboard != null) {
                AddUndoItem(
                    new Rectangle(_selectedCells.X, _selectedCells.Y, _systemInterface.Clipboard.GetLength(1),
                                  _systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite,"Opaque Paste");
                PasteOver();
            }
        }


        private void toolStripButtonPartialRampOff_Click(object sender, EventArgs e) {
            var dialog = new RampQueryDialog(_sequence.MinimumLevel, _sequence.MaximumLevel, true, _actualLevels);
            if (dialog.ShowDialog() == DialogResult.OK) {
                Ramp(dialog.StartingLevel, dialog.EndingLevel);
            }
            dialog.Dispose();
        }


        private void toolStripButtonPartialRampOn_Click(object sender, EventArgs e) {
            var dialog = new RampQueryDialog(_sequence.MinimumLevel, _sequence.MaximumLevel, false, _actualLevels);
            if (dialog.ShowDialog() == DialogResult.OK) {
                Ramp(dialog.StartingLevel, dialog.EndingLevel);
            }
            dialog.Dispose();
        }


        private void toolStripButtonPause_Click(object sender, EventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) == 1) {
                m_positionTimer.Stop();
                _executionInterface.ExecutePause(_executionContextHandle);
                SetEditingState(true);
            }
        }


        private void toolStripButtonPlay_Click(object sender, EventArgs e) {
            int sequencePosition;
            var num = _executionInterface.EngineStatus(_executionContextHandle, out sequencePosition);
            if (num != 1) {
                if (num != 2) {
                    Reset();
                }
                if (_executionInterface.ExecutePlay(_executionContextHandle, sequencePosition, 0)) {
                    m_positionTimer.Start();
                    SetEditingState(false);
                }
            }
        }


        private void toolStripButtonPlayPoint_Click(object sender, EventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) != 1) {
                SetEditingState(false);
                if (playOnlyTheSelectedRangeToolStripMenuItem.Checked) {
                    _executionInterface.ExecutePlay(_executionContextHandle, _selectedCells.Left * _sequence.EventPeriod,
                                                    _selectedCells.Right * _sequence.EventPeriod);
                }
                else {
                    _executionInterface.ExecutePlay(_executionContextHandle, _selectedCells.Left * _sequence.EventPeriod,
                                                    _sequence.TotalEventPeriods * _sequence.EventPeriod);
                }
                m_positionTimer.Start();
            }
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
            var dialog = new RandomParametersDialog(_sequence.MinimumLevel, _sequence.MaximumLevel, _actualLevels);
            int maximumLevel = _sequence.MaximumLevel;
            int intensityMax = _sequence.MaximumLevel;
            //var channelOffset = 0;
            Random random = null;
            if (dialog.ShowDialog() == DialogResult.OK) {
                int left;
                int num10;
                var saturationLevel = dialog.SaturationLevel;
                var periodLength = dialog.PeriodLength;
                if (dialog.VaryIntensity) {
                    maximumLevel = dialog.IntensityMin;
                    intensityMax = dialog.IntensityMax;
                    //channelOffset = Math.Abs((int) (intensityMax - maximumLevel));
                    random = new Random();
                }
                AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite,"Random");
                var list = new List<int>();
                var random2 = new Random();
                var top = _selectedCells.Top;
                while (top < _selectedCells.Bottom) {
                    num10 = _channelOrderMapping[top];
                    if (_sequence.Channels[num10].Enabled) {
                        left = _selectedCells.Left;
                        while (left < _selectedCells.Right) {
                            _sequence.EventValues[num10, left] = _sequence.MinimumLevel;
                            left++;
                        }
                    }
                    top++;
                }
                for (left = _selectedCells.Left; left < _selectedCells.Right; left += periodLength) {
                    int num8;
                    if (dialog.UseSaturation) {
                        if (random2.Next(2) > 0) {
                            num8 = (int) Math.Ceiling(_selectedCells.Height * saturationLevel - 0.1);
                        }
                        else {
                            num8 = (int) Math.Floor(_selectedCells.Height * saturationLevel);
                        }
                    }
                    else {
                        num8 = 0;
                        while (num8 == 0) {
                            num8 = random2.Next(_selectedCells.Height + 1);
                        }
                    }
                    list.Clear();
                    for (top = _selectedCells.Top; top < _selectedCells.Bottom; top++) {
                        num10 = _channelOrderMapping[top];
                        list.Add(num10);
                    }
                    var drawingLevel = _drawingLevel;
                    while (num8-- > 0) {
                        var index = random2.Next(list.Count);
                        if (random != null) {
                            drawingLevel = (byte) random.Next(maximumLevel, intensityMax + 1);
                        }
                        for (var i = 0; (i < periodLength) && ((left + i) < _selectedCells.Right); i++) {
                            _sequence.EventValues[list[index], left + i] = drawingLevel;
                        }
                        list.RemoveAt(index);
                    }
                }
                _selectionRectangle.Width = 0;
                pictureBoxGrid.Invalidate(SelectionToRectangle());
            }
            dialog.Dispose();
        }


        private void toolStripButtonRedo_Click(object sender, EventArgs e) {
            Redo();
        }


        private void toolStripButtonRemoveCells_Click(object sender, EventArgs e) {
            if (_selectedCells.Width != 0) {
                int right = _selectedCells.Right;
                int num2 = _sequence.TotalEventPeriods - right;
                AddUndoItem(new Rectangle(_selectedCells.Left, _selectedCells.Top, _selectedCells.Width, _selectedCells.Height),
                            UndoOriginalBehavior.Removal,"Remove");
                for (int i = 0; i < _selectedCells.Height; i++) {
                    int num3 = _channelOrderMapping[_selectedCells.Top + i];
                    int num4 = 0;
                    while (num4 < num2) {
                        _sequence.EventValues[num3, (right - _selectedCells.Width) + num4] = _sequence.EventValues[num3, right + num4];
                        num4++;
                    }
                    for (num4 = (right + num2) - _selectedCells.Width; num4 < _sequence.TotalEventPeriods; num4++) {
                        _sequence.EventValues[num3, num4] = _sequence.MinimumLevel;
                    }
                }
                pictureBoxGrid.Refresh();
            }
        }


        private void toolStripButtonSave_Click(object sender, EventArgs e) {
            _systemInterface.InvokeSave(this);
        }


        private void toolStripButtonSaveOrder_Click(object sender, EventArgs e) {
            VixenPlus.SortOrder item = null;
            var dialog = new TextQueryDialog("New order", "What name would you like to give to this ordering of the channels?", string.Empty);
            var no = DialogResult.No;
            while (no == DialogResult.No) {
                if (dialog.ShowDialog() == DialogResult.Cancel) {
                    dialog.Dispose();
                    return;
                }
                no = DialogResult.Yes;
                foreach (var order2 in _sequence.Sorts) {
                    if (order2.Name == dialog.Response) {
                        if (
                            (no =
                             MessageBox.Show("This name is already in use.\nDo you want to overwrite it?", Vendor.ProductName,
                                             MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) == DialogResult.Cancel) {
                            dialog.Dispose();
                            return;
                        }
                        item = order2;
                        break;
                    }
                }
            }
            if (item != null) {
                item.ChannelIndexes.Clear();
                item.ChannelIndexes.AddRange(_channelOrderMapping);
                toolStripComboBoxChannelOrder.SelectedItem = item;
            }
            else {
                _sequence.Sorts.Add(item = new VixenPlus.SortOrder(dialog.Response, _channelOrderMapping));
                toolStripComboBoxChannelOrder.Items.Insert(toolStripComboBoxChannelOrder.Items.Count - 1, item);
                toolStripComboBoxChannelOrder.SelectedIndex = toolStripComboBoxChannelOrder.Items.Count - 2;
            }
            dialog.Dispose();
            IsDirty = true;
        }


        private void toolStripButtonShimmerDimming_Click(object sender, EventArgs e) {
            var maxFrequency = (int) _sequence.EventsPerSecond; // 1000 / _sequence.EventPeriod;;
            var dialog = new EffectFrequencyDialog("Shimmer (dimming)", maxFrequency, _dimmingShimmerGenerator);
            if (dialog.ShowDialog() == DialogResult.OK) {
                AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite,"Shimmer");
                var bottom = _selectedCells.Bottom;
                var right = _selectedCells.Right;
                var values = new byte[_selectedCells.Height,_selectedCells.Width];
                DimmingShimmerGenerator(values, new[] {dialog.Frequency});
                var top = _selectedCells.Top;
                for (var i = 0; top < bottom; i++) {
                    var num5 = _channelOrderMapping[top];
                    if (_sequence.Channels[num5].Enabled) {
                        var left = _selectedCells.Left;
                        for (var j = 0; left < right; j++) {
                            _sequence.EventValues[num5, left] = values[i, j];
                            left++;
                        }
                    }
                    top++;
                }
                _selectionRectangle.Width = 0;
                pictureBoxGrid.Invalidate(SelectionToRectangle());
            }
            dialog.Dispose();
        }


        private void toolStripButtonSparkle_Click(object sender, EventArgs e) {
            var maxFrequency = (int) _sequence.EventsPerSecond; // 1000 / _sequence.EventPeriod;
            var dialog = new SparkleParamsDialog(maxFrequency, _sparkleGenerator, _sequence.MinimumLevel, _sequence.MaximumLevel, _drawingLevel,
                                                 _actualLevels);
            if (dialog.ShowDialog() == DialogResult.OK) {
                AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite,"Sparkle");
                var bottom = _selectedCells.Bottom;
                var right = _selectedCells.Right;
                var values = new byte[_selectedCells.Height,_selectedCells.Width];
                SparkleGenerator(values, new[] {dialog.Frequency, dialog.DecayTime, dialog.MinimumIntensity, dialog.MaximumIntensity});
                var top = _selectedCells.Top;
                for (var i = 0; top < bottom; i++) {
                    var num5 = _channelOrderMapping[top];
                    if (_sequence.Channels[num5].Enabled) {
                        var left = _selectedCells.Left;
                        for (var j = 0; left < right; j++) {
                            _sequence.EventValues[num5, left] = values[i, j];
                            left++;
                        }
                    }
                    top++;
                }
                _selectionRectangle.Width = 0;
                pictureBoxGrid.Invalidate(SelectionToRectangle());
            }
            dialog.Dispose();
        }


        private void toolStripButtonStop_Click(object sender, EventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) != 0) {
                m_positionTimer.Stop();
                ProgramEnded();
                _executionInterface.ExecuteStop(_executionContextHandle);
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
            toolStripButtonToggleCellText.BackgroundImage = _showCellText
                                                                ? global::Properties.Resources.Ball_Green : global::Properties.Resources.Ball_Red;
            pictureBoxGrid.Refresh();
        }


        private void toolStripButtonToggleCrossHairs_Click(object sender, EventArgs e) {
            // Cant check on click here since the paint method depends on the crosshairs flag, so we set it right away.
            toolStripButtonToggleCrossHairs.Checked = !toolStripButtonToggleCrossHairs.Checked;

            pictureBoxGrid.Invalidate(new Rectangle((_mouseTimeCaret - hScrollBar1.Value) * _periodPixelWidth, 0, _periodPixelWidth,
                                                    pictureBoxGrid.Height));
            pictureBoxGrid.Invalidate(new Rectangle(0, (_mouseChannelCaret - vScrollBar1.Value) * _gridRowHeight, pictureBoxGrid.Width, _gridRowHeight));
            pictureBoxGrid.Update();
        }


        private void toolStripButtonToggleLevels_Click(object sender, EventArgs e) {
            _actualLevels = !_actualLevels;
            _preferences.SetBoolean("ActualLevels", _actualLevels);
            UpdateLevelDisplay();
            pictureBoxGrid.Refresh();
        }


        private void toolStripButtonToggleRamps_Click(object sender, EventArgs e) {
            _showingGradient = !_showingGradient;
            _preferences.SetBoolean("BarLevels", !_showingGradient);
            pictureBoxGrid.Refresh();
        }


        private void toolStripButtonTransparentPaste_Click(object sender, EventArgs e) {
            if (_systemInterface.Clipboard != null) {
                AddUndoItem(
                    new Rectangle(_selectedCells.X, _selectedCells.Y, _systemInterface.Clipboard.GetLength(1),
                                  _systemInterface.Clipboard.GetLength(0)), UndoOriginalBehavior.Overwrite,"Transparent Paste");
                byte[,] clipboard = _systemInterface.Clipboard;
                int length = clipboard.GetLength(0);
                int num3 = clipboard.GetLength(1);
                int minimumLevel = _sequence.MinimumLevel;
                for (int i = 0; (i < length) && ((_selectedCells.Top + i) < _sequence.ChannelCount); i++) {
                    int num4 = _channelOrderMapping[_selectedCells.Top + i];
                    for (int j = 0; (j < num3) && ((_selectedCells.Left + j) < _sequence.TotalEventPeriods); j++) {
                        byte num5 = clipboard[i, j];
                        if (num5 > minimumLevel) {
                            _sequence.EventValues[num4, _selectedCells.Left + j] = num5;
                        }
                    }
                }
                IsDirty = true;
                pictureBoxGrid.Refresh();
            }
        }


        private void toolStripButtonUndo_Click(object sender, EventArgs e) {
            Undo();
        }


        private void toolStripButtonWaveform_Click(object sender, EventArgs e) {
            if (_executionInterface.EngineStatus(_executionContextHandle) != 1) {
                if ((_waveformPixelData == null) || (_waveformPcmData == null)) {
                    ParseAudioWaveform();
                }
                if (toolStripButtonWaveform.Checked) {
                    pictureBoxTime.Height = 120; //TODO  (int)(120 * ((toolStripComboBoxWaveformZoom.SelectedIndex + 1) * .1));
                    splitContainer2.SplitterDistance = 120;
                    splitContainer2.Enabled = true;
                    EnableWaveformButton();
                }
                else {
                    pictureBoxTime.Height = 60;
                    splitContainer2.SplitterDistance = 60;
                    splitContainer2.Enabled = false;
                    toolStripLabelWaveformZoom.Enabled = false;
                    toolStripComboBoxWaveformZoom.Enabled = false;
                }
                pictureBoxTime.Refresh();
                pictureBoxChannels.Refresh();
            }
        }


        private void toolStripComboBoxChannelOrder_SelectedIndexChanged(object sender, EventArgs e) {
            if (toolStripComboBoxChannelOrder.SelectedIndex != -1) {
                if ((_sequence.Profile != null) && (toolStripComboBoxChannelOrder.SelectedIndex == 0)) {
                    toolStripComboBoxChannelOrder.SelectedIndex = -1;
                    MessageBox.Show(
                        "This sequence is attached to a profile.\nChanges to the profile affect all sequences attached to it.\n\nIf this is what you want to do, please use the profile manager.",
                        Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else {
                    if (toolStripComboBoxChannelOrder.SelectedIndex == 0) {
                        if (_sequence.ChannelCount == 0) {
                            MessageBox.Show("There are no channels to reorder.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        toolStripButtonDeleteOrder.Enabled = false;
                        toolStripComboBoxChannelOrder.SelectedIndex = -1;
                        var dialog = new ChannelOrderDialog(_sequence.Channels, _channelOrderMapping);
                        if (dialog.ShowDialog() == DialogResult.OK) {
                            _channelOrderMapping.Clear();
                            foreach (var channel in dialog.ChannelMapping) {
                                _channelOrderMapping.Add(_sequence.Channels.IndexOf(channel));
                            }
                            IsDirty = true;
                        }
                        dialog.Dispose();
                    }
                    else if (toolStripComboBoxChannelOrder.SelectedIndex == (toolStripComboBoxChannelOrder.Items.Count - 1)) {
                        toolStripButtonDeleteOrder.Enabled = false;
                        toolStripComboBoxChannelOrder.SelectedIndex = -1;
                        _channelOrderMapping.Clear();
                        for (int i = 0; i < _sequence.ChannelCount; i++) {
                            _channelOrderMapping.Add(i);
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


        private void toolStripComboBoxWaveformZoom_SelectedIndexChanged(object sender, EventArgs e) {
            if ((_executionInterface.EngineStatus(_executionContextHandle) != 1) &&
                ((toolStripComboBoxWaveformZoom.SelectedIndex != -1) && toolStripComboBoxWaveformZoom.Enabled)) {
                var selectedItem = (string) toolStripComboBoxWaveformZoom.SelectedItem;
                _waveformMaxAmplitude =
                    (int) ((100f / (Convert.ToInt32(selectedItem.Substring(0, selectedItem.Length - 1)))) * _waveform100PercentAmplitude);
                PcmToPixels(_waveformPcmData, _waveformPixelData);
                pictureBoxTime.Refresh();
            }
        }


        private void toolStripDropDownButtonPlugins_Click(object sender, EventArgs e) {
            var dialog = new PluginListDialog(_sequence);
            if (dialog.ShowDialog() == DialogResult.OK) {
                toolStripDropDownButtonPlugins.DropDownItems.Clear();
                int num = 0;
                foreach (object[] objArray in dialog.MappedPluginList) {
                    var item = new ToolStripMenuItem((string) objArray[0])
                               {Checked = (bool) objArray[1], CheckOnClick = true, Tag = num.ToString(CultureInfo.InvariantCulture)};
                    item.CheckedChanged += plugInItem_CheckedChanged;
                    num++;
                    toolStripDropDownButtonPlugins.DropDownItems.Add(item);
                }
                if (toolStripDropDownButtonPlugins.DropDownItems.Count > 0) {
                    toolStripDropDownButtonPlugins.DropDownItems.Add("-");
                    toolStripDropDownButtonPlugins.DropDownItems.Add("Select all", null, selectAllToolStripMenuItem_Click);
                    toolStripDropDownButtonPlugins.DropDownItems.Add("Unselect all", null, unselectAllToolStripMenuItem_Click);
                }
                IsDirty = true;
            }
            dialog.Dispose();
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


        private void TurnCellsOff() {
            AddUndoItem(_selectedCells, UndoOriginalBehavior.Overwrite,"Off");
            int bottom = _selectedCells.Bottom;
            int right = _selectedCells.Right;
            for (int i = _selectedCells.Top; i < bottom; i++) {
                int num4 = _channelOrderMapping[i];
                if (_sequence.Channels[num4].Enabled) {
                    for (int j = _selectedCells.Left; j < right; j++) {
                        _sequence.EventValues[num4, j] = _sequence.MinimumLevel;
                    }
                }
            }
            _selectionRectangle.Width = 0;
            pictureBoxGrid.Invalidate(SelectionToRectangle());
        }


        private void Undo() {
            if (_undoStack.Count != 0) {
                var item = (UndoItem) _undoStack.Pop();
                //var length = 0;
                //var heightAddition = 0;
                //if (item.Data != null)
                //{
                //    length = item.Data.GetLength(0);
                //    heightAddition = item.Data.GetLength(1);
                //}
                toolStripButtonUndo.Enabled = undoToolStripMenuItem.Enabled = _undoStack.Count > 0;
                if (_undoStack.Count > 0) {
                    IsDirty = true;
                }
                var item2 = new UndoItem(item.Location,
                                         GetAffectedBlockData(new Rectangle(item.Location.X, item.Location.Y, item.Data.GetLength(1),
                                                                            item.Data.GetLength(0))), item.Behavior, _sequence, _channelOrderMapping, item.OriginalAction);
                switch (item.Behavior) {
                    case UndoOriginalBehavior.Overwrite:
                        DisjointedOverwrite(item.Location.X, item.Data, item.ReferencedChannels);
                        pictureBoxGrid.Refresh();
                        break;

                    case UndoOriginalBehavior.Removal:
                        DisjointedInsert(item.Location.X, item.Data.GetLength(1), item.Data.GetLength(0), item.ReferencedChannels);
                        DisjointedOverwrite(item.Location.X, item.Data, item.ReferencedChannels);
                        pictureBoxGrid.Refresh();
                        break;

                    case UndoOriginalBehavior.Insertion:
                        DisjointedRemove(item.Location.X, item.Data.GetLength(1), item.Data.GetLength(0), item.ReferencedChannels);
                        pictureBoxGrid.Refresh();
                        break;
                }
                UpdateUndoText();
                _redoStack.Push(item2);
                toolStripButtonRedo.Enabled = redoToolStripMenuItem.Enabled = true;
                UpdateRedoText();
            }
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
            double num = (toolStripComboBoxColumnZoom.SelectedIndex + 1) * 0.1;
            _periodPixelWidth = (int) (_preferences.GetInteger("MaxColumnWidth") * num);
            HScrollCheck();
            ParseAudioWaveform();
            pictureBoxGrid.Refresh();
            pictureBoxTime.Refresh();
        }


        private void UpdateGrid(Graphics g, Rectangle clipRect) {
            if (_sequence.ChannelCount != 0) {
                var fontSize = (_periodPixelWidth <= 20) ? 5 : ((_periodPixelWidth <= 25) ? 6 : ((_periodPixelWidth < 50) ? 8 : 10));
                using (var font = new Font(Font.FontFamily, fontSize)) {
                    using (var brush = new SolidBrush(Color.White)) {
                        var initialX = ((clipRect.X / _periodPixelWidth) * _periodPixelWidth) + 1;
                        int currentY = ((clipRect.Y / _gridRowHeight) * _gridRowHeight) + 1;
                        int startEvent = (clipRect.X / _periodPixelWidth) + hScrollBar1.Value;
                        int channelIndex = (clipRect.Y / _gridRowHeight) + vScrollBar1.Value;

                        while ((currentY < clipRect.Bottom) && (channelIndex < _sequence.ChannelCount)) {
                            var currentChannel = _channelOrderMapping[channelIndex];
                            var channel = _sequence.Channels[currentChannel];
                            var currentX = initialX;

                            for (var currentEventCount = startEvent; (currentX < clipRect.Right) && (currentEventCount < _sequence.TotalEventPeriods);
                                 currentEventCount++) {
                                if (_showingGradient) {
                                    brush.Color = GetGradientColor(_gridBackBrush.Color, channel.Color,
                                                                   _sequence.EventValues[currentChannel, currentEventCount]);
                                    g.FillRectangle(brush, currentX, currentY, _periodPixelWidth - 1, _gridRowHeight - 1);
                                }
                                else {
                                    int height = ((_gridRowHeight - 1) * _sequence.EventValues[currentChannel, currentEventCount]) / 255;
                                    g.FillRectangle(channel.Brush, currentX, ((currentY + _gridRowHeight) - 1) - height, _periodPixelWidth - 1, height);
                                }

                                string cellIntensity;
                                if (_showCellText && (GetCellIntensity(currentEventCount, channelIndex, out cellIntensity) > 0)) {
                                    g.DrawString(cellIntensity, font, Brushes.Black,
                                                 new RectangleF(currentX, currentY, (_periodPixelWidth - 1), (_gridRowHeight - 1)));
                                }
                                currentX += _periodPixelWidth;
                            }
                            currentY += _gridRowHeight;
                            channelIndex++;
                        }
                    }
                }
            }
        }


        private void UpdateLevelDisplay() {
            SetDrawingLevel(_drawingLevel);
            if (_actualLevels) {
                toolStripButtonToggleLevels.Image = global::Properties.Resources.Percent;
                toolStripButtonToggleLevels.Text = toolStripButtonToggleLevels.ToolTipText = "Show intensity levels as percent (0-100%)";
            }
            else {
                toolStripButtonToggleLevels.Image = global::Properties.Resources.number;
                toolStripButtonToggleLevels.Text = toolStripButtonToggleLevels.ToolTipText = "Show actual intensity levels (0-255)";
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
            int milliseconds = rect.Left * _sequence.EventPeriod;
            string str = TimeString(milliseconds);
            if (rect.Width > 1) {
                int num2 = (rect.Right - 1) * _sequence.EventPeriod;
                string str2 = TimeString(num2);
                labelPosition.Text = string.Format("{0} - {1}\n({2})", str, str2, TimeString(num2 - milliseconds));
            }
            else if (((rect.Width == 0) && zeroWidthIsValid) || (rect.Width == 1)) {
                labelPosition.Text = str;
            }
            else {
                labelPosition.Text = string.Empty;
            }
        }


        private void UpdateFollowMouse(Point mousePoint) {
            var rowCount = _selectedCells.Height;
            lblFollowMouse.Text = labelPosition.Text + Environment.NewLine + rowCount + " Channel" + ((rowCount == 1) ? "" : "s");
            mousePoint.X -= lblFollowMouse.Size.Width;
            mousePoint.Y += 24;
            lblFollowMouse.Location = mousePoint;

        }


        private void UpdateProgress() {
            int x = (_previousPosition - hScrollBar1.Value) * _periodPixelWidth;
            pictureBoxTime.Invalidate(new Rectangle(x, pictureBoxTime.Height - 0x23, _periodPixelWidth + _periodPixelWidth, 15));
            pictureBoxGrid.Invalidate(new Rectangle(x, 0, _periodPixelWidth + _periodPixelWidth, pictureBoxGrid.Height));
        }


        private void UpdateRedoText() {
            if (_redoStack.Count > 0) {
                toolStripButtonRedo.ToolTipText = redoToolStripMenuItem.Text = "Redo: " + ((UndoItem) _redoStack.Peek()).ToString();
            }
            else {
                toolStripButtonRedo.ToolTipText = redoToolStripMenuItem.Text = "Redo";
            }
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
            }

            var num = (toolStripComboBoxRowZoom.SelectedIndex + 1) * 0.1;
            _gridRowHeight = (int) (_preferences.GetInteger("MaxRowHeight") * num);
            VScrollCheck();
            pictureBoxGrid.Refresh();
            pictureBoxChannels.Refresh();
        }


        private void UpdateUndoText() {
            if (_undoStack.Count > 0) {
                toolStripButtonUndo.ToolTipText = undoToolStripMenuItem.Text = "Undo: " + ((UndoItem) _undoStack.Peek()).ToString();
            }
            else {
                toolStripButtonUndo.ToolTipText = undoToolStripMenuItem.Text = "Undo";
            }
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
            get { return ".vix"; }
        }

        public override string FileTypeDescription {
            get { return "Vixen/Vixen+ sequence"; }
        }

        private VixenPlus.Channel SelectedChannel {
            get { return _selectedChannel; }
            set {
                if (_selectedChannel != value) {
                    VixenPlus.Channel selectedChannel = _selectedChannel;
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

        /*
                private delegate void ToolStripUpdateDelegate(int seconds);
        */


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

        //private void splitContainer2_SplitterMoving(object sender, SplitterCancelEventArgs keyEvent)
        //{
        //    System.Diagnostics.Debug.Print(String.Format("x:{0}, y:{1}, sx:{2}, sy:{3}",keyEvent.MouseCursorX, keyEvent.MouseCursorY, keyEvent.SplitX, keyEvent.SplitY));
        //}

        //private void splitContainer2_StyleChanged(object sender, EventArgs keyEvent)
        //{
        //    keyEvent.
        //}

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e) {
            pictureBoxTime.Height = e.SplitY;
            pictureBoxChannels.Refresh();
            pictureBoxGrid.Refresh();
        }

    }
}