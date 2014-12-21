using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VixenPlusCommon;

namespace VixenEditor{

    public partial class StandardSequence{
        private IContainer components;
        private ToolStripButton toolStripButtonWaveform;
        private ToolStripButton newSeqTsb;
        private ToolStripButton mapperTsb;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton openSequenceTsb;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox textBoxChannelCount;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripLabel toolStripLabel2;
        private ToolStripTextBox textBoxProgramLength;
        private SplitContainer splitContainer2;
        private PictureBox pictureBoxTime;
        private SelectablePictureBox pictureBoxGrid;
        private VScrollBar vScrollBar1;
        private HScrollBar hScrollBar1;
        private ToolStripMenuItem toolbarsToolStripMenuItem;
        private ToolStripMenuItem lockToolbarToolStripMenuItem;
        private ToolStripMenuItem toolbarIconSizeToolStripMenuItem;
        private ToolStripMenuItem smallToolStripMenuItem;
        private ToolStripMenuItem mediumToolStripMenuItem;
        private ToolStripMenuItem largeToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem15;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem resetAllToolbarsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator19;
        private ToolStripLabel profileToolStripLabel;
        private TransparentLabel lblFollowMouse;
        private ToolStripButton tsbPlayRange;
        private ToolStripButton tsbPlayPoint;
        private ToolStripMenuItem selectNoneToolStripMenuItem;
        private ToolStripMenuItem tsmProfiles;
        private ComboBox cbGroups;

        protected override void Dispose(bool disposing) {
            if (_gridLinePen != null) {
                _gridLinePen.Dispose();
            }
            if (_crosshairPen != null) {
                _crosshairPen.Dispose();
            }
            if (_waveformPen != null) {
                _waveformPen.Dispose();
            }
            if (_waveformZeroLinePen != null) {
                _waveformZeroLinePen.Dispose();
            }

            if (_channelBackBrush != null) {
                _channelBackBrush.Dispose();
            }
            if (_waveformBackBrush != null) {
                _waveformBackBrush.Dispose();
            }
            if (_gridBackBrush != null) {
                _gridBackBrush.Dispose();
            }
            if (_gridLineBrush != null) {
                _gridLineBrush.Dispose();
            }
            if (_waveformZeroLineBrush != null) {
                _waveformZeroLineBrush.Dispose();
            }
            if (_waveformBrush != null) {
                _waveformBrush.Dispose();
            }
            if (_crosshairBrush != null) {
                _crosshairBrush.Dispose();
            }
            if (_mouseCaretBrush != null) {
                _mouseCaretBrush.Dispose();
            }
            if (_selectionBrush != null) {
                _selectionBrush.Dispose();
            }
            if (_positionBrush != null) {
                _positionBrush.Dispose();
            }

            if (_gridGraphics != null) {
                _gridGraphics.Dispose();
            }

            _channelNameFont.Dispose();
            _channelStrikeoutFont.Dispose();
            _timeFont.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private ContextMenuStrip contextMenuChannels;
        private ContextMenuStrip contextMenuGrid;
        private ContextMenuStrip contextMenuTime;
        private Label labelPosition;
        private MenuStrip menuStrip;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog;
        private SelectablePictureBox pictureBoxChannels;
        private SplitContainer splitContainer1;
        private Timer positionTimer;
        private ToolStrip toolStripDisplaySettings;
        private ToolStrip toolStripEditing;
        private ToolStrip toolStripEffect;
        private ToolStrip toolStripExecutionControl;
        private ToolStrip toolStripSequenceSettings;
        private ToolStrip toolStripText;
        private ToolStripButton tsbAudio;
        private ToolStripButton toolStripButtonChangeIntensity;
        private ToolStripButton toolStripButtonChannelOutputMask;
        private ToolStripButton toolStripButtonCopy;
        private ToolStripButton toolStripButtonCut;
        private ToolStripButton toolStripButtonFindAndReplace;
        private ToolStripButton toolStripButtonInsertPaste;
        private ToolStripButton toolStripButtonIntensity;
        private ToolStripButton toolStripButtonInvert;
        private ToolStripButton tsbLoop;
        private ToolStripButton toolStripButtonMirrorHorizontal;
        private ToolStripButton toolStripButtonMirrorVertical;
        private ToolStripButton toolStripButtonOff;
        private ToolStripButton toolStripButtonOn;
        private ToolStripButton toolStripButtonOpaquePaste;
        private ToolStripButton toolStripButtonPartialRampOff;
        private ToolStripButton toolStripButtonPartialRampOn;
        private ToolStripButton tsbPause;
        private ToolStripButton tsbPlay;
        private ToolStripButton SpeedHalfTsb;
        private ToolStripButton SpeedNormalTsb;
        private ToolStripButton SpeedQtrTsb;
        private ToolStripButton SpeedThreeQtrTsb;
        private ToolStripButton SpeedVariableTsb;
        private ToolStripButton toolStripButtonRampOff;
        private ToolStripButton toolStripButtonRampOn;
        private ToolStripButton toolStripButtonRandom;
        private ToolStripButton toolStripButtonRedo;
        private ToolStripButton toolStripButtonRemoveCells;
        private ToolStripButton tbsSave;
        private ToolStripButton toolStripButtonShimmerDimming;
        private ToolStripButton toolStripButtonSparkle;
        private ToolStripButton tsbStop;
        private ToolStripButton tbsTestChannels;
        private ToolStripButton tbsTestConsole;
        private ToolStripButton toolStripButtonToggleCellText;
        private ToolStripButton toolStripButtonToggleCrossHairs;
        private ToolStripButton toolStripButtonToggleLevels;
        private ToolStripButton toolStripButtonToggleRamps;
        private ToolStripButton toolStripButtonTransparentPaste;
        private ToolStripButton toolStripButtonUndo;
        private ToolStripButton tsbSaveAs;
        private ToolStripComboBox toolStripComboBoxColumnZoom;
        private ToolStripComboBox toolStripComboBoxRowZoom;
        private ToolStripContainer toolStripContainer1;
        private ToolStripLabel toolStripLabel10;
        private ToolStripLabel toolStripLabel4;
        private ToolStripLabel toolStripLabel5;
        private ToolStripLabel toolStripLabel6;
        private ToolStripLabel toolStripLabel8;
        private ToolStripLabel toolStripLabelCellIntensity;
        private ToolStripLabel toolStripLabelCurrentCell;
        private ToolStripLabel toolStripLabelCurrentDrawingIntensity;
        private ToolStripLabel toolStripLabelExecutionPoint;
        private ToolStripMenuItem aNDToolStripMenuItem1;
        private ToolStripMenuItem aNDToolStripMenuItem;
        private ToolStripMenuItem additionToolStripMenuItem1;
        private ToolStripMenuItem additionToolStripMenuItem;
        private ToolStripMenuItem allChannelsToFullIntensityForThisEventToolStripMenuItem;
        private ToolStripMenuItem allEventsToFullIntensityToolStripMenuItem;
        private ToolStripMenuItem arithmeticPasteToolStripMenuItem;
        private ToolStripMenuItem attachSequenceToToolStripMenuItem;
        private ToolStripMenuItem audioToolStripMenuItem1;
        private ToolStripMenuItem booleanPasteToolStripMenuItem1;
        private ToolStripMenuItem booleanPasteToolStripMenuItem;
        private ToolStripMenuItem channelOutputMaskToolStripMenuItem;
        private ToolStripMenuItem channelPropertiesToolStripMenuItem;
        private ToolStripMenuItem chaseLinesToolStripMenuItem;
        private ToolStripMenuItem clearAllChannelsForThisEventToolStripMenuItem;
        private ToolStripMenuItem clearAllToolStripMenuItem;
        private ToolStripMenuItem clearChannelEventsToolStripMenuItem;
        private ToolStripMenuItem copyChannelEventsToClipboardToolStripMenuItem;
        private ToolStripMenuItem copyChannelToolStripMenuItem1;
        private ToolStripMenuItem copyToolStripMenuItem1;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem createFromSequenceToolStripMenuItem;
        private ToolStripMenuItem currentProgramsSettingsToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem1;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem detachSequenceFromItsProfileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem effectsToolStripMenuItem;
        private ToolStripMenuItem findAndReplaceToolStripMenuItem1;
        private ToolStripMenuItem findAndReplaceToolStripMenuItem;
        private ToolStripMenuItem flattenProfileIntoSequenceToolStripMenuItem;
        private ToolStripMenuItem insertPasteToolStripMenuItem1;
        private ToolStripMenuItem insertPasteToolStripMenuItem;
        private ToolStripMenuItem invertToolStripMenuItem1;
        private ToolStripMenuItem invertToolStripMenuItem;
        private ToolStripMenuItem loadARoutineToolStripMenuItem;
        private ToolStripMenuItem loadRoutineToClipboardToolStripMenuItem;
        private ToolStripMenuItem maxToolStripMenuItem1;
        private ToolStripMenuItem maxToolStripMenuItem;
        private ToolStripMenuItem minToolStripMenuItem1;
        private ToolStripMenuItem minToolStripMenuItem;
        private ToolStripMenuItem mirrorHorizontallyToolStripMenuItem1;
        private ToolStripMenuItem mirrorHorizontallyToolStripMenuItem;
        private ToolStripMenuItem mirrorVerticallyToolStripMenuItem1;
        private ToolStripMenuItem mirrorVerticallyToolStripMenuItem;
        private ToolStripMenuItem nANDToolStripMenuItem1;
        private ToolStripMenuItem nANDToolStripMenuItem;
        private ToolStripMenuItem nORToolStripMenuItem1;
        private ToolStripMenuItem nORToolStripMenuItem;
        private ToolStripMenuItem normalToolStripMenuItem;
        private ToolStripMenuItem oRToolStripMenuItem1;
        private ToolStripMenuItem oRToolStripMenuItem;
        private ToolStripMenuItem offToolStripMenuItem1;
        private ToolStripMenuItem offToolStripMenuItem;
        private ToolStripMenuItem onToolStripMenuItem1;
        private ToolStripMenuItem onToolStripMenuItem;
        private ToolStripMenuItem opaquePasteToolStripMenuItem1;
        private ToolStripMenuItem opaquePasteToolStripMenuItem;
        private ToolStripMenuItem paintFromClipboardToolStripMenuItem;
        private ToolStripMenuItem partialRampOffToolStripMenuItem1;
        private ToolStripMenuItem partialRampOffToolStripMenuItem;
        private ToolStripMenuItem partialRampOnToolStripMenuItem1;
        private ToolStripMenuItem partialRampOnToolStripMenuItem;
        private ToolStripMenuItem pasteFullChannelEventsFromClipboardToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem1;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem programToolStripMenuItem;
        private ToolStripMenuItem rampOffToolStripMenuItem1;
        private ToolStripMenuItem rampOffToolStripMenuItem;
        private ToolStripMenuItem rampOnToolStripMenuItem1;
        private ToolStripMenuItem rampOnToolStripMenuItem;
        private ToolStripMenuItem randomToolStripMenuItem1;
        private ToolStripMenuItem randomToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem removeCellsToolStripMenuItem1;
        private ToolStripMenuItem removeCellsToolStripMenuItem;
        private ToolStripMenuItem reorderChannelOutputsToolStripMenuItem;
        private ToolStripMenuItem saveAsARoutineToolStripMenuItem;
        private ToolStripMenuItem scaleToolStripMenuItem1;
        private ToolStripMenuItem scaleToolStripMenuItem;
        private ToolStripMenuItem setAllChannelColorsToolStripMenuItem;
        private ToolStripMenuItem setIntensityToolStripMenuItem1;
        private ToolStripMenuItem setIntensityToolStripMenuItem;
        private ToolStripMenuItem shimmerToolStripMenuItem1;
        private ToolStripMenuItem shimmerToolStripMenuItem;
        private ToolStripMenuItem sparkleToolStripMenuItem1;
        private ToolStripMenuItem sparkleToolStripMenuItem;
        private ToolStripMenuItem subtractionToolStripMenuItem1;
        private ToolStripMenuItem subtractionToolStripMenuItem;
        private ToolStripMenuItem toggleOutputChannelsToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItemPasteAnd;
        private ToolStripMenuItem toolStripMenuItemPasteNand;
        private ToolStripMenuItem toolStripMenuItemPasteNor;
        private ToolStripMenuItem toolStripMenuItemPasteOr;
        private ToolStripMenuItem toolStripMenuItemPasteXnor;
        private ToolStripMenuItem toolStripMenuItemPasteXor;
        private ToolStripMenuItem transparentPasteToolStripMenuItem1;
        private ToolStripMenuItem transparentPasteToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem xNORToolStripMenuItem1;
        private ToolStripMenuItem xNORToolStripMenuItem;
        private ToolStripMenuItem xORToolStripMenuItem1;
        private ToolStripMenuItem xORToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem10;
        private ToolStripSeparator toolStripMenuItem11;
        private ToolStripSeparator toolStripMenuItem12;
        private ToolStripSeparator toolStripMenuItem14;
        private ToolStripSeparator toolStripMenuItem16;
        private ToolStripSeparator toolStripMenuItem17;
        private ToolStripSeparator toolStripMenuItem18;
        private ToolStripSeparator toolStripMenuItem19;
        private ToolStripSeparator toolStripMenuItem20;
        private ToolStripSeparator toolStripMenuItem21;
        private ToolStripSeparator toolStripMenuItem23;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripSeparator toolStripMenuItem9;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripSeparator toolStripSeparator14;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripSeparator toolStripSeparator16;
        private ToolStripSeparator toolStripSeparator17;
        private ToolStripSeparator toolStripSeparator18;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSplitButton toolStripSplitButtonArithmeticPaste;
        private ToolStripSplitButton toolStripSplitButtonBooleanPaste;

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(StandardSequence));
            this.menuStrip = new MenuStrip();
            this.programToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.audioToolStripMenuItem1 = new ToolStripMenuItem();
            this.channelOutputMaskToolStripMenuItem = new ToolStripMenuItem();
            this.currentProgramsSettingsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem19 = new ToolStripSeparator();
            this.editToolStripMenuItem = new ToolStripMenuItem();
            this.undoToolStripMenuItem = new ToolStripMenuItem();
            this.redoToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem7 = new ToolStripSeparator();
            this.selectAlltoolStripMenuItem = new ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new ToolStripMenuItem();
            this.cutToolStripMenuItem = new ToolStripMenuItem();
            this.copyToolStripMenuItem = new ToolStripMenuItem();
            this.pasteToolStripMenuItem = new ToolStripMenuItem();
            this.opaquePasteToolStripMenuItem = new ToolStripMenuItem();
            this.transparentPasteToolStripMenuItem = new ToolStripMenuItem();
            this.booleanPasteToolStripMenuItem = new ToolStripMenuItem();
            this.oRToolStripMenuItem = new ToolStripMenuItem();
            this.aNDToolStripMenuItem = new ToolStripMenuItem();
            this.xORToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem16 = new ToolStripSeparator();
            this.nORToolStripMenuItem = new ToolStripMenuItem();
            this.nANDToolStripMenuItem = new ToolStripMenuItem();
            this.xNORToolStripMenuItem = new ToolStripMenuItem();
            this.insertPasteToolStripMenuItem = new ToolStripMenuItem();
            this.pastePreviewToolStripMenuItem = new ToolStripMenuItem();
            this.tsmiPpOpaque = new ToolStripMenuItem();
            this.tsmiPpTransparent = new ToolStripMenuItem();
            this.tsmiPpOR = new ToolStripMenuItem();
            this.tsmiPpAND = new ToolStripMenuItem();
            this.tsmiPpXOR = new ToolStripMenuItem();
            this.tsmiPpNOR = new ToolStripMenuItem();
            this.tsmiPpNAND = new ToolStripMenuItem();
            this.tsmiPpXNOR = new ToolStripMenuItem();
            this.tsmiPpAdd = new ToolStripMenuItem();
            this.tsmiPpSubtract = new ToolStripMenuItem();
            this.tsmiPpScale = new ToolStripMenuItem();
            this.tsmiPpMin = new ToolStripMenuItem();
            this.tsmiPpMax = new ToolStripMenuItem();
            this.tsmiPpInsert = new ToolStripMenuItem();
            this.removeCellsToolStripMenuItem1 = new ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem18 = new ToolStripSeparator();
            this.findAndReplaceToolStripMenuItem = new ToolStripMenuItem();
            this.viewToolStripMenuItem = new ToolStripMenuItem();
            this.tsmProfiles = new ToolStripMenuItem();
            this.createFromSequenceToolStripMenuItem = new ToolStripMenuItem();
            this.attachSequenceToToolStripMenuItem = new ToolStripMenuItem();
            this.detachSequenceFromItsProfileToolStripMenuItem = new ToolStripMenuItem();
            this.flattenProfileIntoSequenceToolStripMenuItem = new ToolStripMenuItem();
            this.toolbarsToolStripMenuItem = new ToolStripMenuItem();
            this.lockToolbarToolStripMenuItem = new ToolStripMenuItem();
            this.toolbarIconSizeToolStripMenuItem = new ToolStripMenuItem();
            this.smallToolStripMenuItem = new ToolStripMenuItem();
            this.mediumToolStripMenuItem = new ToolStripMenuItem();
            this.largeToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem15 = new ToolStripSeparator();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.resetAllToolbarsToolStripMenuItem = new ToolStripMenuItem();
            this.effectsToolStripMenuItem = new ToolStripMenuItem();
            this.onToolStripMenuItem1 = new ToolStripMenuItem();
            this.offToolStripMenuItem1 = new ToolStripMenuItem();
            this.toolStripMenuItem8 = new ToolStripSeparator();
            this.rampOnToolStripMenuItem1 = new ToolStripMenuItem();
            this.rampOffToolStripMenuItem1 = new ToolStripMenuItem();
            this.toolStripMenuItem9 = new ToolStripSeparator();
            this.partialRampOnToolStripMenuItem1 = new ToolStripMenuItem();
            this.partialRampOffToolStripMenuItem1 = new ToolStripMenuItem();
            this.toolStripMenuItem10 = new ToolStripSeparator();
            this.setIntensityToolStripMenuItem = new ToolStripMenuItem();
            this.mirrorVerticallyToolStripMenuItem1 = new ToolStripMenuItem();
            this.mirrorHorizontallyToolStripMenuItem1 = new ToolStripMenuItem();
            this.invertToolStripMenuItem1 = new ToolStripMenuItem();
            this.randomToolStripMenuItem = new ToolStripMenuItem();
            this.shimmerToolStripMenuItem1 = new ToolStripMenuItem();
            this.sparkleToolStripMenuItem1 = new ToolStripMenuItem();
            this.toolStripMenuItem20 = new ToolStripSeparator();
            this.chaseLinesToolStripMenuItem = new ToolStripMenuItem();
            this.normalToolStripMenuItem = new ToolStripMenuItem();
            this.paintFromClipboardToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripContainer1 = new ToolStripContainer();
            this.splitContainer1 = new SplitContainer();
            this.cbGroups = new ComboBox();
            this.labelPosition = new Label();
            this.pictureBoxChannels = new SelectablePictureBox();
            this.contextMenuChannels = new ContextMenuStrip(this.components);
            this.toggleOutputChannelsToolStripMenuItem = new ToolStripMenuItem();
            this.reorderChannelOutputsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem5 = new ToolStripSeparator();
            this.clearChannelEventsToolStripMenuItem = new ToolStripMenuItem();
            this.allEventsToFullIntensityToolStripMenuItem = new ToolStripMenuItem();
            this.copyChannelToolStripMenuItem1 = new ToolStripMenuItem();
            this.setAllChannelColorsToolStripMenuItem = new ToolStripMenuItem();
            this.selectAllEventsMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem23 = new ToolStripSeparator();
            this.copyChannelEventsToClipboardToolStripMenuItem = new ToolStripMenuItem();
            this.pasteFullChannelEventsFromClipboardToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem14 = new ToolStripSeparator();
            this.channelPropertiesToolStripMenuItem = new ToolStripMenuItem();
            this.splitContainer2 = new SplitContainer();
            this.pictureBoxTime = new PictureBox();
            this.contextMenuTime = new ContextMenuStrip(this.components);
            this.clearAllChannelsForThisEventToolStripMenuItem = new ToolStripMenuItem();
            this.allChannelsToFullIntensityForThisEventToolStripMenuItem = new ToolStripMenuItem();
            this.selectAllChannelsForPeriod = new ToolStripMenuItem();
            this.lblFollowMouse = new TransparentLabel(this.components);
            this.pictureBoxGrid = new SelectablePictureBox();
            this.contextMenuGrid = new ContextMenuStrip(this.components);
            this.onToolStripMenuItem = new ToolStripMenuItem();
            this.offToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem2 = new ToolStripSeparator();
            this.rampOnToolStripMenuItem = new ToolStripMenuItem();
            this.rampOffToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem3 = new ToolStripSeparator();
            this.partialRampOnToolStripMenuItem = new ToolStripMenuItem();
            this.partialRampOffToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem6 = new ToolStripSeparator();
            this.cutToolStripMenuItem1 = new ToolStripMenuItem();
            this.copyToolStripMenuItem1 = new ToolStripMenuItem();
            this.opaquePasteToolStripMenuItem1 = new ToolStripMenuItem();
            this.transparentPasteToolStripMenuItem1 = new ToolStripMenuItem();
            this.pasteToolStripMenuItem1 = new ToolStripMenuItem();
            this.booleanPasteToolStripMenuItem1 = new ToolStripMenuItem();
            this.oRToolStripMenuItem1 = new ToolStripMenuItem();
            this.aNDToolStripMenuItem1 = new ToolStripMenuItem();
            this.xORToolStripMenuItem1 = new ToolStripMenuItem();
            this.toolStripMenuItem17 = new ToolStripSeparator();
            this.nORToolStripMenuItem1 = new ToolStripMenuItem();
            this.nANDToolStripMenuItem1 = new ToolStripMenuItem();
            this.xNORToolStripMenuItem1 = new ToolStripMenuItem();
            this.insertPasteToolStripMenuItem1 = new ToolStripMenuItem();
            this.arithmeticPasteToolStripMenuItem = new ToolStripMenuItem();
            this.additionToolStripMenuItem1 = new ToolStripMenuItem();
            this.subtractionToolStripMenuItem1 = new ToolStripMenuItem();
            this.scaleToolStripMenuItem1 = new ToolStripMenuItem();
            this.minToolStripMenuItem1 = new ToolStripMenuItem();
            this.maxToolStripMenuItem1 = new ToolStripMenuItem();
            this.removeCellsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem12 = new ToolStripSeparator();
            this.findAndReplaceToolStripMenuItem1 = new ToolStripMenuItem();
            this.toolStripMenuItem21 = new ToolStripSeparator();
            this.setIntensityToolStripMenuItem1 = new ToolStripMenuItem();
            this.mirrorVerticallyToolStripMenuItem = new ToolStripMenuItem();
            this.mirrorHorizontallyToolStripMenuItem = new ToolStripMenuItem();
            this.invertToolStripMenuItem = new ToolStripMenuItem();
            this.randomToolStripMenuItem1 = new ToolStripMenuItem();
            this.shimmerToolStripMenuItem = new ToolStripMenuItem();
            this.sparkleToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem11 = new ToolStripSeparator();
            this.saveAsARoutineToolStripMenuItem = new ToolStripMenuItem();
            this.loadARoutineToolStripMenuItem = new ToolStripMenuItem();
            this.loadRoutineToClipboardToolStripMenuItem = new ToolStripMenuItem();
            this.vScrollBar1 = new VScrollBar();
            this.hScrollBar1 = new HScrollBar();
            this.toolStripSequenceSettings = new ToolStrip();
            this.newSeqTsb = new ToolStripButton();
            this.openSequenceTsb = new ToolStripButton();
            this.tbsSave = new ToolStripButton();
            this.tsbSaveAs = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.mapperTsb = new ToolStripButton();
            this.toolStripSeparator8 = new ToolStripSeparator();
            this.tbsTestChannels = new ToolStripButton();
            this.tbsTestConsole = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.tsbAudio = new ToolStripButton();
            this.toolStripButtonWaveform = new ToolStripButton();
            this.toolStripSeparator13 = new ToolStripSeparator();
            this.toolStripButtonChannelOutputMask = new ToolStripButton();
            this.toolStripSeparator19 = new ToolStripSeparator();
            this.profileToolStripLabel = new ToolStripLabel();
            this.toolStripExecutionControl = new ToolStrip();
            this.tsbPlay = new ToolStripButton();
            this.tsbPlayPoint = new ToolStripButton();
            this.tsbPlayRange = new ToolStripButton();
            this.tsbPause = new ToolStripButton();
            this.tsbStop = new ToolStripButton();
            this.tsbLoop = new ToolStripButton();
            this.toolStripSeparator10 = new ToolStripSeparator();
            this.SpeedQtrTsb = new ToolStripButton();
            this.SpeedHalfTsb = new ToolStripButton();
            this.SpeedThreeQtrTsb = new ToolStripButton();
            this.SpeedNormalTsb = new ToolStripButton();
            this.SpeedVariableTsb = new ToolStripButton();
            this.toolStripEffect = new ToolStrip();
            this.toolStripButtonOn = new ToolStripButton();
            this.toolStripButtonOff = new ToolStripButton();
            this.toolStripButtonRampOn = new ToolStripButton();
            this.toolStripButtonRampOff = new ToolStripButton();
            this.toolStripButtonPartialRampOn = new ToolStripButton();
            this.toolStripButtonPartialRampOff = new ToolStripButton();
            this.toolStripButtonToggleRamps = new ToolStripButton();
            this.toolStripButtonIntensity = new ToolStripButton();
            this.toolStripButtonMirrorVertical = new ToolStripButton();
            this.toolStripButtonMirrorHorizontal = new ToolStripButton();
            this.toolStripButtonInvert = new ToolStripButton();
            this.toolStripButtonRandom = new ToolStripButton();
            this.toolStripButtonSparkle = new ToolStripButton();
            this.toolStripButtonShimmerDimming = new ToolStripButton();
            this.tsbNutcracker = new ToolStripButton();
            this.toolStripSeparator16 = new ToolStripSeparator();
            this.toolStripButtonToggleLevels = new ToolStripButton();
            this.toolStripButtonToggleCellText = new ToolStripButton();
            this.toolStripButtonChangeIntensity = new ToolStripButton();
            this.toolStripEditing = new ToolStrip();
            this.toolStripButtonCut = new ToolStripButton();
            this.toolStripButtonCopy = new ToolStripButton();
            this.toolStripButtonOpaquePaste = new ToolStripButton();
            this.toolStripButtonTransparentPaste = new ToolStripButton();
            this.toolStripSplitButtonBooleanPaste = new ToolStripSplitButton();
            this.toolStripMenuItemPasteOr = new ToolStripMenuItem();
            this.toolStripMenuItemPasteAnd = new ToolStripMenuItem();
            this.toolStripMenuItemPasteXor = new ToolStripMenuItem();
            this.toolStripSeparator14 = new ToolStripSeparator();
            this.toolStripMenuItemPasteNor = new ToolStripMenuItem();
            this.toolStripMenuItemPasteNand = new ToolStripMenuItem();
            this.toolStripMenuItemPasteXnor = new ToolStripMenuItem();
            this.toolStripSplitButtonArithmeticPaste = new ToolStripSplitButton();
            this.additionToolStripMenuItem = new ToolStripMenuItem();
            this.subtractionToolStripMenuItem = new ToolStripMenuItem();
            this.scaleToolStripMenuItem = new ToolStripMenuItem();
            this.minToolStripMenuItem = new ToolStripMenuItem();
            this.maxToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripButtonInsertPaste = new ToolStripButton();
            this.tsPreviewLabel = new ToolStripLabel();
            this.cbPastePreview = new ToolStripComboBox();
            this.toolStripButtonRemoveCells = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.toolStripButtonFindAndReplace = new ToolStripButton();
            this.toolStripSeparator15 = new ToolStripSeparator();
            this.toolStripButtonUndo = new ToolStripButton();
            this.toolStripButtonRedo = new ToolStripButton();
            this.toolStripDisplaySettings = new ToolStrip();
            this.toolStripButtonToggleCrossHairs = new ToolStripButton();
            this.toolStripLabel4 = new ToolStripLabel();
            this.toolStripComboBoxColumnZoom = new ToolStripComboBox();
            this.toolStripLabel5 = new ToolStripLabel();
            this.toolStripComboBoxRowZoom = new ToolStripComboBox();
            this.toolStripSeparator6 = new ToolStripSeparator();
            this.toolStripText = new ToolStrip();
            this.toolStripLabel6 = new ToolStripLabel();
            this.toolStripLabelExecutionPoint = new ToolStripLabel();
            this.toolStripSeparator11 = new ToolStripSeparator();
            this.toolStripLabel10 = new ToolStripLabel();
            this.toolStripLabelCurrentDrawingIntensity = new ToolStripLabel();
            this.toolStripSeparator18 = new ToolStripSeparator();
            this.toolStripLabel8 = new ToolStripLabel();
            this.toolStripLabelCellIntensity = new ToolStripLabel();
            this.toolStripSeparator17 = new ToolStripSeparator();
            this.toolStripLabelCurrentCell = new ToolStripLabel();
            this.toolStripSeparator9 = new ToolStripSeparator();
            this.toolStripLabel1 = new ToolStripLabel();
            this.textBoxChannelCount = new ToolStripTextBox();
            this.toolStripSeparator12 = new ToolStripSeparator();
            this.toolStripLabel2 = new ToolStripLabel();
            this.textBoxProgramLength = new ToolStripTextBox();
            this.openFileDialog1 = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.positionTimer = new Timer(this.components);
            this.menuStrip.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((ISupportInitialize)(this.pictureBoxChannels)).BeginInit();
            this.contextMenuChannels.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((ISupportInitialize)(this.pictureBoxTime)).BeginInit();
            this.contextMenuTime.SuspendLayout();
            ((ISupportInitialize)(this.pictureBoxGrid)).BeginInit();
            this.contextMenuGrid.SuspendLayout();
            this.toolStripSequenceSettings.SuspendLayout();
            this.toolStripExecutionControl.SuspendLayout();
            this.toolStripEffect.SuspendLayout();
            this.toolStripEditing.SuspendLayout();
            this.toolStripDisplaySettings.SuspendLayout();
            this.toolStripText.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
            this.programToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.effectsToolStripMenuItem});
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(976, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            this.menuStrip.Visible = false;
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.toolStripSeparator4,
            this.audioToolStripMenuItem1,
            this.channelOutputMaskToolStripMenuItem,
            this.currentProgramsSettingsToolStripMenuItem,
            this.toolStripMenuItem19});
            this.programToolStripMenuItem.Enabled = false;
            this.programToolStripMenuItem.MergeAction = MergeAction.MatchOnly;
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new Size(37, 20);
            this.programToolStripMenuItem.Text = "File";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.MergeAction = MergeAction.Insert;
            this.toolStripSeparator4.MergeIndex = 5;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(185, 6);
            // 
            // audioToolStripMenuItem1
            // 
            this.audioToolStripMenuItem1.MergeAction = MergeAction.Insert;
            this.audioToolStripMenuItem1.MergeIndex = 6;
            this.audioToolStripMenuItem1.Name = "audioToolStripMenuItem1";
            this.audioToolStripMenuItem1.Size = new Size(188, 22);
            this.audioToolStripMenuItem1.Text = "Audio";
            this.audioToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonAudio_Click);
            // 
            // channelOutputMaskToolStripMenuItem
            // 
            this.channelOutputMaskToolStripMenuItem.MergeAction = MergeAction.Insert;
            this.channelOutputMaskToolStripMenuItem.MergeIndex = 7;
            this.channelOutputMaskToolStripMenuItem.Name = "channelOutputMaskToolStripMenuItem";
            this.channelOutputMaskToolStripMenuItem.Size = new Size(188, 22);
            this.channelOutputMaskToolStripMenuItem.Text = "Channel output mask";
            this.channelOutputMaskToolStripMenuItem.Click += new EventHandler(this.channelOutputMaskToolStripMenuItem_Click);
            // 
            // currentProgramsSettingsToolStripMenuItem
            // 
            this.currentProgramsSettingsToolStripMenuItem.MergeAction = MergeAction.Insert;
            this.currentProgramsSettingsToolStripMenuItem.MergeIndex = 8;
            this.currentProgramsSettingsToolStripMenuItem.Name = "currentProgramsSettingsToolStripMenuItem";
            this.currentProgramsSettingsToolStripMenuItem.Size = new Size(188, 22);
            this.currentProgramsSettingsToolStripMenuItem.Text = "Settings";
            this.currentProgramsSettingsToolStripMenuItem.Click += new EventHandler(this.currentProgramsSettingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem19
            // 
            this.toolStripMenuItem19.MergeAction = MergeAction.Insert;
            this.toolStripMenuItem19.MergeIndex = 9;
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            this.toolStripMenuItem19.Size = new Size(185, 6);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripMenuItem7,
            this.selectAlltoolStripMenuItem,
            this.selectNoneToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.pastePreviewToolStripMenuItem,
            this.removeCellsToolStripMenuItem1,
            this.clearAllToolStripMenuItem,
            this.toolStripMenuItem18,
            this.findAndReplaceToolStripMenuItem});
            this.editToolStripMenuItem.MergeAction = MergeAction.Insert;
            this.editToolStripMenuItem.MergeIndex = 1;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.Z)));
            this.undoToolStripMenuItem.Size = new Size(201, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new EventHandler(this.toolStripButtonUndo_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.Y)));
            this.redoToolStripMenuItem.Size = new Size(201, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new EventHandler(this.toolStripButtonRedo_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new Size(198, 6);
            // 
            // selectAlltoolStripMenuItem
            // 
            this.selectAlltoolStripMenuItem.Name = "selectAlltoolStripMenuItem";
            this.selectAlltoolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.A)));
            this.selectAlltoolStripMenuItem.Size = new Size(201, 22);
            this.selectAlltoolStripMenuItem.Text = "Select &All";
            this.selectAlltoolStripMenuItem.Click += new EventHandler(this.selectAlltoolStripMenuItem_Click);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.N)));
            this.selectNoneToolStripMenuItem.Size = new Size(201, 22);
            this.selectNoneToolStripMenuItem.Text = "Select &None";
            this.selectNoneToolStripMenuItem.Click += new EventHandler(this.selectNoneToolStripMenuItem_Click);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.X)));
            this.cutToolStripMenuItem.Size = new Size(201, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new EventHandler(this.toolStripButtonCut_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.C)));
            this.copyToolStripMenuItem.Size = new Size(201, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new EventHandler(this.toolStripButtonCopy_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.opaquePasteToolStripMenuItem,
            this.transparentPasteToolStripMenuItem,
            this.booleanPasteToolStripMenuItem,
            this.insertPasteToolStripMenuItem});
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new Size(201, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // opaquePasteToolStripMenuItem
            // 
            this.opaquePasteToolStripMenuItem.Name = "opaquePasteToolStripMenuItem";
            this.opaquePasteToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.V)));
            this.opaquePasteToolStripMenuItem.Size = new Size(188, 22);
            this.opaquePasteToolStripMenuItem.Text = "Opaque paste";
            this.opaquePasteToolStripMenuItem.Click += new EventHandler(this.toolStripButtonOpaquePaste_Click);
            // 
            // transparentPasteToolStripMenuItem
            // 
            this.transparentPasteToolStripMenuItem.Name = "transparentPasteToolStripMenuItem";
            this.transparentPasteToolStripMenuItem.Size = new Size(188, 22);
            this.transparentPasteToolStripMenuItem.Text = "Transparent paste";
            this.transparentPasteToolStripMenuItem.Click += new EventHandler(this.toolStripButtonTransparentPaste_Click);
            // 
            // booleanPasteToolStripMenuItem
            // 
            this.booleanPasteToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.oRToolStripMenuItem,
            this.aNDToolStripMenuItem,
            this.xORToolStripMenuItem,
            this.toolStripMenuItem16,
            this.nORToolStripMenuItem,
            this.nANDToolStripMenuItem,
            this.xNORToolStripMenuItem});
            this.booleanPasteToolStripMenuItem.Name = "booleanPasteToolStripMenuItem";
            this.booleanPasteToolStripMenuItem.Size = new Size(188, 22);
            this.booleanPasteToolStripMenuItem.Text = "Boolean paste";
            // 
            // oRToolStripMenuItem
            // 
            this.oRToolStripMenuItem.Name = "oRToolStripMenuItem";
            this.oRToolStripMenuItem.Size = new Size(172, 22);
            this.oRToolStripMenuItem.Text = "OR";
            this.oRToolStripMenuItem.Click += new EventHandler(this.toolStripMenuItemPasteOr_Click);
            // 
            // aNDToolStripMenuItem
            // 
            this.aNDToolStripMenuItem.Name = "aNDToolStripMenuItem";
            this.aNDToolStripMenuItem.Size = new Size(172, 22);
            this.aNDToolStripMenuItem.Text = "AND";
            this.aNDToolStripMenuItem.Click += new EventHandler(this.toolStripMenuItemPasteAnd_Click);
            // 
            // xORToolStripMenuItem
            // 
            this.xORToolStripMenuItem.Name = "xORToolStripMenuItem";
            this.xORToolStripMenuItem.Size = new Size(172, 22);
            this.xORToolStripMenuItem.Text = "XOR";
            this.xORToolStripMenuItem.Click += new EventHandler(this.toolStripMenuItemPasteXor_Click);
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new Size(169, 6);
            // 
            // nORToolStripMenuItem
            // 
            this.nORToolStripMenuItem.Name = "nORToolStripMenuItem";
            this.nORToolStripMenuItem.Size = new Size(172, 22);
            this.nORToolStripMenuItem.Text = "NOR (NOT OR)";
            this.nORToolStripMenuItem.Click += new EventHandler(this.toolStripMenuItemPasteNor_Click);
            // 
            // nANDToolStripMenuItem
            // 
            this.nANDToolStripMenuItem.Name = "nANDToolStripMenuItem";
            this.nANDToolStripMenuItem.Size = new Size(172, 22);
            this.nANDToolStripMenuItem.Text = "NAND (NOT AND)";
            this.nANDToolStripMenuItem.Click += new EventHandler(this.toolStripMenuItemPasteNand_Click);
            // 
            // xNORToolStripMenuItem
            // 
            this.xNORToolStripMenuItem.Name = "xNORToolStripMenuItem";
            this.xNORToolStripMenuItem.Size = new Size(172, 22);
            this.xNORToolStripMenuItem.Text = "XNOR (NOT XOR)";
            this.xNORToolStripMenuItem.Click += new EventHandler(this.toolStripMenuItemPasteXnor_Click);
            // 
            // insertPasteToolStripMenuItem
            // 
            this.insertPasteToolStripMenuItem.Name = "insertPasteToolStripMenuItem";
            this.insertPasteToolStripMenuItem.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift)
                        | Keys.V)));
            this.insertPasteToolStripMenuItem.Size = new Size(188, 22);
            this.insertPasteToolStripMenuItem.ToolTipText = "Insert paste (Ctrl-Shift-V)";
            this.insertPasteToolStripMenuItem.Click += new EventHandler(this.toolStripButtonInsertPaste_Click);
            // 
            // pastePreviewToolStripMenuItem
            // 
            this.pastePreviewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.tsmiPpOpaque,
            this.tsmiPpTransparent,
            this.tsmiPpOR,
            this.tsmiPpAND,
            this.tsmiPpXOR,
            this.tsmiPpNOR,
            this.tsmiPpNAND,
            this.tsmiPpXNOR,
            this.tsmiPpAdd,
            this.tsmiPpSubtract,
            this.tsmiPpScale,
            this.tsmiPpMin,
            this.tsmiPpMax,
            this.tsmiPpInsert});
            this.pastePreviewToolStripMenuItem.Name = "pastePreviewToolStripMenuItem";
            this.pastePreviewToolStripMenuItem.Size = new Size(201, 22);
            this.pastePreviewToolStripMenuItem.Text = "Paste Preview";
            // 
            // tsmiPpOpaque
            // 
            this.tsmiPpOpaque.Checked = true;
            this.tsmiPpOpaque.CheckState = CheckState.Checked;
            this.tsmiPpOpaque.Name = "tsmiPpOpaque";
            this.tsmiPpOpaque.Size = new Size(167, 22);
            this.tsmiPpOpaque.Text = "Opaque (Normal)";
            this.tsmiPpOpaque.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpTransparent
            // 
            this.tsmiPpTransparent.Name = "tsmiPpTransparent";
            this.tsmiPpTransparent.Size = new Size(167, 22);
            this.tsmiPpTransparent.Text = "Transparent";
            this.tsmiPpTransparent.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpOR
            // 
            this.tsmiPpOR.Name = "tsmiPpOR";
            this.tsmiPpOR.Size = new Size(167, 22);
            this.tsmiPpOR.Text = "Boolean (OR)";
            this.tsmiPpOR.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpAND
            // 
            this.tsmiPpAND.Name = "tsmiPpAND";
            this.tsmiPpAND.Size = new Size(167, 22);
            this.tsmiPpAND.Text = "Boolean (AND)";
            this.tsmiPpAND.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpXOR
            // 
            this.tsmiPpXOR.Name = "tsmiPpXOR";
            this.tsmiPpXOR.Size = new Size(167, 22);
            this.tsmiPpXOR.Text = "Boolean (XOR)";
            this.tsmiPpXOR.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpNOR
            // 
            this.tsmiPpNOR.Name = "tsmiPpNOR";
            this.tsmiPpNOR.Size = new Size(167, 22);
            this.tsmiPpNOR.Text = "Boolean (NOR)";
            this.tsmiPpNOR.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpNAND
            // 
            this.tsmiPpNAND.Name = "tsmiPpNAND";
            this.tsmiPpNAND.Size = new Size(167, 22);
            this.tsmiPpNAND.Text = "Boolean (NAND)";
            this.tsmiPpNAND.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpXNOR
            // 
            this.tsmiPpXNOR.Name = "tsmiPpXNOR";
            this.tsmiPpXNOR.Size = new Size(167, 22);
            this.tsmiPpXNOR.Text = "Boolean (XNOR)";
            this.tsmiPpXNOR.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpAdd
            // 
            this.tsmiPpAdd.Name = "tsmiPpAdd";
            this.tsmiPpAdd.Size = new Size(167, 22);
            this.tsmiPpAdd.Text = "Addition";
            this.tsmiPpAdd.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpSubtract
            // 
            this.tsmiPpSubtract.Name = "tsmiPpSubtract";
            this.tsmiPpSubtract.Size = new Size(167, 22);
            this.tsmiPpSubtract.Text = "Subtraction";
            this.tsmiPpSubtract.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpScale
            // 
            this.tsmiPpScale.Name = "tsmiPpScale";
            this.tsmiPpScale.Size = new Size(167, 22);
            this.tsmiPpScale.Text = "Scale";
            this.tsmiPpScale.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpMin
            // 
            this.tsmiPpMin.Name = "tsmiPpMin";
            this.tsmiPpMin.Size = new Size(167, 22);
            this.tsmiPpMin.Text = "Min";
            this.tsmiPpMin.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpMax
            // 
            this.tsmiPpMax.Name = "tsmiPpMax";
            this.tsmiPpMax.Size = new Size(167, 22);
            this.tsmiPpMax.Text = "Max";
            this.tsmiPpMax.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // tsmiPpInsert
            // 
            this.tsmiPpInsert.Name = "tsmiPpInsert";
            this.tsmiPpInsert.Size = new Size(167, 22);
            this.tsmiPpInsert.Text = "Insert";
            this.tsmiPpInsert.Click += new EventHandler(this.pastePreviewItem_Click);
            // 
            // removeCellsToolStripMenuItem1
            // 
            this.removeCellsToolStripMenuItem1.Name = "removeCellsToolStripMenuItem1";
            this.removeCellsToolStripMenuItem1.Size = new Size(201, 22);
            this.removeCellsToolStripMenuItem1.Text = "Remove Cells";
            this.removeCellsToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonRemoveCells_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.Delete)));
            this.clearAllToolStripMenuItem.Size = new Size(201, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All Cells";
            this.clearAllToolStripMenuItem.Click += new EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Size = new Size(198, 6);
            // 
            // findAndReplaceToolStripMenuItem
            // 
            this.findAndReplaceToolStripMenuItem.Name = "findAndReplaceToolStripMenuItem";
            this.findAndReplaceToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.F)));
            this.findAndReplaceToolStripMenuItem.Size = new Size(201, 22);
            this.findAndReplaceToolStripMenuItem.Text = "Find and replace";
            this.findAndReplaceToolStripMenuItem.Click += new EventHandler(this.toolStripButtonFindAndReplace_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.tsmProfiles,
            this.toolbarsToolStripMenuItem});
            this.viewToolStripMenuItem.Enabled = false;
            this.viewToolStripMenuItem.MergeAction = MergeAction.MatchOnly;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // tsmProfiles
            // 
            this.tsmProfiles.DropDownItems.AddRange(new ToolStripItem[] {
            this.createFromSequenceToolStripMenuItem,
            this.attachSequenceToToolStripMenuItem,
            this.detachSequenceFromItsProfileToolStripMenuItem,
            this.flattenProfileIntoSequenceToolStripMenuItem});
            this.tsmProfiles.MergeAction = MergeAction.MatchOnly;
            this.tsmProfiles.Name = "tsmProfiles";
            this.tsmProfiles.Size = new Size(120, 22);
            this.tsmProfiles.Text = "Profiles";
            // 
            // createFromSequenceToolStripMenuItem
            // 
            this.createFromSequenceToolStripMenuItem.Name = "createFromSequenceToolStripMenuItem";
            this.createFromSequenceToolStripMenuItem.Size = new Size(245, 22);
            this.createFromSequenceToolStripMenuItem.Text = "Create profile from sequence";
            this.createFromSequenceToolStripMenuItem.Click += new EventHandler(this.createFromSequenceToolStripMenuItem_Click);
            // 
            // attachSequenceToToolStripMenuItem
            // 
            this.attachSequenceToToolStripMenuItem.Name = "attachSequenceToToolStripMenuItem";
            this.attachSequenceToToolStripMenuItem.Size = new Size(245, 22);
            this.attachSequenceToToolStripMenuItem.Text = "Attach sequence to profile";
            this.attachSequenceToToolStripMenuItem.Click += new EventHandler(this.attachSequenceToToolStripMenuItem_Click);
            // 
            // detachSequenceFromItsProfileToolStripMenuItem
            // 
            this.detachSequenceFromItsProfileToolStripMenuItem.Enabled = false;
            this.detachSequenceFromItsProfileToolStripMenuItem.Name = "detachSequenceFromItsProfileToolStripMenuItem";
            this.detachSequenceFromItsProfileToolStripMenuItem.Size = new Size(245, 22);
            this.detachSequenceFromItsProfileToolStripMenuItem.Text = "Detach sequence from its profile";
            this.detachSequenceFromItsProfileToolStripMenuItem.Click += new EventHandler(this.detachSequenceFromItsProfileToolStripMenuItem_Click);
            // 
            // flattenProfileIntoSequenceToolStripMenuItem
            // 
            this.flattenProfileIntoSequenceToolStripMenuItem.Enabled = false;
            this.flattenProfileIntoSequenceToolStripMenuItem.Name = "flattenProfileIntoSequenceToolStripMenuItem";
            this.flattenProfileIntoSequenceToolStripMenuItem.Size = new Size(245, 22);
            this.flattenProfileIntoSequenceToolStripMenuItem.Text = "Flatten profile into sequence";
            this.flattenProfileIntoSequenceToolStripMenuItem.Click += new EventHandler(this.flattenProfileIntoSequenceToolStripMenuItem_Click);
            // 
            // toolbarsToolStripMenuItem
            // 
            this.toolbarsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.lockToolbarToolStripMenuItem,
            this.toolbarIconSizeToolStripMenuItem,
            this.toolStripMenuItem15,
            this.toolStripSeparator7,
            this.resetAllToolbarsToolStripMenuItem});
            this.toolbarsToolStripMenuItem.MergeAction = MergeAction.Insert;
            this.toolbarsToolStripMenuItem.MergeIndex = 6;
            this.toolbarsToolStripMenuItem.Name = "toolbarsToolStripMenuItem";
            this.toolbarsToolStripMenuItem.Size = new Size(120, 22);
            this.toolbarsToolStripMenuItem.Text = "Toolbars";
            // 
            // lockToolbarToolStripMenuItem
            // 
            this.lockToolbarToolStripMenuItem.CheckOnClick = true;
            this.lockToolbarToolStripMenuItem.Name = "lockToolbarToolStripMenuItem";
            this.lockToolbarToolStripMenuItem.Size = new Size(164, 22);
            this.lockToolbarToolStripMenuItem.Text = "Lock Position";
            this.lockToolbarToolStripMenuItem.Click += new EventHandler(this.lockToolbarToolStripMenuItem_Click);
            // 
            // toolbarIconSizeToolStripMenuItem
            // 
            this.toolbarIconSizeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.smallToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.largeToolStripMenuItem});
            this.toolbarIconSizeToolStripMenuItem.Name = "toolbarIconSizeToolStripMenuItem";
            this.toolbarIconSizeToolStripMenuItem.Size = new Size(164, 22);
            this.toolbarIconSizeToolStripMenuItem.Text = "Toolbar Icon Size";
            // 
            // smallToolStripMenuItem
            // 
            this.smallToolStripMenuItem.Checked = true;
            this.smallToolStripMenuItem.CheckOnClick = true;
            this.smallToolStripMenuItem.CheckState = CheckState.Checked;
            this.smallToolStripMenuItem.Name = "smallToolStripMenuItem";
            this.smallToolStripMenuItem.Size = new Size(119, 22);
            this.smallToolStripMenuItem.Text = "Small";
            this.smallToolStripMenuItem.Click += new EventHandler(this.smallToolStripMenuItem_Click);
            // 
            // mediumToolStripMenuItem
            // 
            this.mediumToolStripMenuItem.CheckOnClick = true;
            this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            this.mediumToolStripMenuItem.Size = new Size(119, 22);
            this.mediumToolStripMenuItem.Text = "Medium";
            this.mediumToolStripMenuItem.Click += new EventHandler(this.mediumToolStripMenuItem_Click);
            // 
            // largeToolStripMenuItem
            // 
            this.largeToolStripMenuItem.CheckOnClick = true;
            this.largeToolStripMenuItem.Name = "largeToolStripMenuItem";
            this.largeToolStripMenuItem.Size = new Size(119, 22);
            this.largeToolStripMenuItem.Text = "Large";
            this.largeToolStripMenuItem.Click += new EventHandler(this.largeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new Size(161, 6);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(161, 6);
            // 
            // resetAllToolbarsToolStripMenuItem
            // 
            this.resetAllToolbarsToolStripMenuItem.Name = "resetAllToolbarsToolStripMenuItem";
            this.resetAllToolbarsToolStripMenuItem.Size = new Size(164, 22);
            this.resetAllToolbarsToolStripMenuItem.Text = "Reset all toolbars";
            this.resetAllToolbarsToolStripMenuItem.Click += new EventHandler(this.resetAllToolbarsToolStripMenuItem_Click);
            // 
            // effectsToolStripMenuItem
            // 
            this.effectsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.onToolStripMenuItem1,
            this.offToolStripMenuItem1,
            this.toolStripMenuItem8,
            this.rampOnToolStripMenuItem1,
            this.rampOffToolStripMenuItem1,
            this.toolStripMenuItem9,
            this.partialRampOnToolStripMenuItem1,
            this.partialRampOffToolStripMenuItem1,
            this.toolStripMenuItem10,
            this.setIntensityToolStripMenuItem,
            this.mirrorVerticallyToolStripMenuItem1,
            this.mirrorHorizontallyToolStripMenuItem1,
            this.invertToolStripMenuItem1,
            this.randomToolStripMenuItem,
            this.shimmerToolStripMenuItem1,
            this.sparkleToolStripMenuItem1,
            this.toolStripMenuItem20,
            this.chaseLinesToolStripMenuItem});
            this.effectsToolStripMenuItem.MergeAction = MergeAction.Insert;
            this.effectsToolStripMenuItem.MergeIndex = 3;
            this.effectsToolStripMenuItem.Name = "effectsToolStripMenuItem";
            this.effectsToolStripMenuItem.Size = new Size(54, 20);
            this.effectsToolStripMenuItem.Text = "Effects";
            // 
            // onToolStripMenuItem1
            // 
            this.onToolStripMenuItem1.Name = "onToolStripMenuItem1";
            this.onToolStripMenuItem1.Size = new Size(174, 22);
            this.onToolStripMenuItem1.Text = "On";
            this.onToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonOn_Click);
            // 
            // offToolStripMenuItem1
            // 
            this.offToolStripMenuItem1.Name = "offToolStripMenuItem1";
            this.offToolStripMenuItem1.Size = new Size(174, 22);
            this.offToolStripMenuItem1.Text = "Off";
            this.offToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonOff_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new Size(171, 6);
            // 
            // rampOnToolStripMenuItem1
            // 
            this.rampOnToolStripMenuItem1.Name = "rampOnToolStripMenuItem1";
            this.rampOnToolStripMenuItem1.ShortcutKeyDisplayString = "";
            this.rampOnToolStripMenuItem1.Size = new Size(174, 22);
            this.rampOnToolStripMenuItem1.Text = "Ramp On";
            this.rampOnToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonRampOn_Click);
            // 
            // rampOffToolStripMenuItem1
            // 
            this.rampOffToolStripMenuItem1.Name = "rampOffToolStripMenuItem1";
            this.rampOffToolStripMenuItem1.Size = new Size(174, 22);
            this.rampOffToolStripMenuItem1.Text = "Ramp Off";
            this.rampOffToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonRampOff_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new Size(171, 6);
            // 
            // partialRampOnToolStripMenuItem1
            // 
            this.partialRampOnToolStripMenuItem1.Name = "partialRampOnToolStripMenuItem1";
            this.partialRampOnToolStripMenuItem1.Size = new Size(174, 22);
            this.partialRampOnToolStripMenuItem1.Text = "Partial Ramp On";
            this.partialRampOnToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonPartialRampOn_Click);
            // 
            // partialRampOffToolStripMenuItem1
            // 
            this.partialRampOffToolStripMenuItem1.Name = "partialRampOffToolStripMenuItem1";
            this.partialRampOffToolStripMenuItem1.Size = new Size(174, 22);
            this.partialRampOffToolStripMenuItem1.Text = "Partial Ramp Off";
            this.partialRampOffToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonPartialRampOff_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new Size(171, 6);
            // 
            // setIntensityToolStripMenuItem
            // 
            this.setIntensityToolStripMenuItem.Name = "setIntensityToolStripMenuItem";
            this.setIntensityToolStripMenuItem.Size = new Size(174, 22);
            this.setIntensityToolStripMenuItem.Text = "Set Intensity";
            this.setIntensityToolStripMenuItem.Click += new EventHandler(this.toolStripButtonIntensity_Click);
            // 
            // mirrorVerticallyToolStripMenuItem1
            // 
            this.mirrorVerticallyToolStripMenuItem1.Name = "mirrorVerticallyToolStripMenuItem1";
            this.mirrorVerticallyToolStripMenuItem1.Size = new Size(174, 22);
            this.mirrorVerticallyToolStripMenuItem1.Text = "Mirror Vertically";
            this.mirrorVerticallyToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonMirrorVertical_Click);
            // 
            // mirrorHorizontallyToolStripMenuItem1
            // 
            this.mirrorHorizontallyToolStripMenuItem1.Name = "mirrorHorizontallyToolStripMenuItem1";
            this.mirrorHorizontallyToolStripMenuItem1.Size = new Size(174, 22);
            this.mirrorHorizontallyToolStripMenuItem1.Text = "Mirror Horizontally";
            this.mirrorHorizontallyToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonMirrorHorizontal_Click);
            // 
            // invertToolStripMenuItem1
            // 
            this.invertToolStripMenuItem1.Name = "invertToolStripMenuItem1";
            this.invertToolStripMenuItem1.Size = new Size(174, 22);
            this.invertToolStripMenuItem1.Text = "Invert";
            this.invertToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonInvert_Click);
            // 
            // randomToolStripMenuItem
            // 
            this.randomToolStripMenuItem.Name = "randomToolStripMenuItem";
            this.randomToolStripMenuItem.Size = new Size(174, 22);
            this.randomToolStripMenuItem.Text = "Random";
            this.randomToolStripMenuItem.Click += new EventHandler(this.toolStripButtonRandom_Click);
            // 
            // shimmerToolStripMenuItem1
            // 
            this.shimmerToolStripMenuItem1.Name = "shimmerToolStripMenuItem1";
            this.shimmerToolStripMenuItem1.Size = new Size(174, 22);
            this.shimmerToolStripMenuItem1.Text = "Shimmer";
            this.shimmerToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonShimmerDimming_Click);
            // 
            // sparkleToolStripMenuItem1
            // 
            this.sparkleToolStripMenuItem1.Name = "sparkleToolStripMenuItem1";
            this.sparkleToolStripMenuItem1.Size = new Size(174, 22);
            this.sparkleToolStripMenuItem1.Text = "Sparkle";
            this.sparkleToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonSparkle_Click);
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new Size(171, 6);
            // 
            // chaseLinesToolStripMenuItem
            // 
            this.chaseLinesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.normalToolStripMenuItem,
            this.paintFromClipboardToolStripMenuItem});
            this.chaseLinesToolStripMenuItem.Name = "chaseLinesToolStripMenuItem";
            this.chaseLinesToolStripMenuItem.Size = new Size(174, 22);
            this.chaseLinesToolStripMenuItem.Text = "Chase lines";
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Checked = true;
            this.normalToolStripMenuItem.CheckState = CheckState.Checked;
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new Size(185, 22);
            this.normalToolStripMenuItem.Text = "Normal";
            this.normalToolStripMenuItem.Click += new EventHandler(this.normalToolStripMenuItem_Click);
            // 
            // paintFromClipboardToolStripMenuItem
            // 
            this.paintFromClipboardToolStripMenuItem.Name = "paintFromClipboardToolStripMenuItem";
            this.paintFromClipboardToolStripMenuItem.Size = new Size(185, 22);
            this.paintFromClipboardToolStripMenuItem.Text = "Paint from Clipboard";
            this.paintFromClipboardToolStripMenuItem.Click += new EventHandler(this.paintFromClipboardToolStripMenuItem_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new Size(976, 243);
            this.toolStripContainer1.Dock = DockStyle.Fill;
            this.toolStripContainer1.Location = new Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new Size(976, 539);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripSequenceSettings);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripExecutionControl);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripEffect);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripEditing);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripDisplaySettings);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripText);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cbGroups);
            this.splitContainer1.Panel1.Controls.Add(this.labelPosition);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBoxChannels);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new Size(976, 243);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 20;
            this.splitContainer1.SplitterMoving += new SplitterCancelEventHandler(this.splitContainer1_SplitterMoving);
            this.splitContainer1.SplitterMoved += new SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // cbGroups
            // 
            this.cbGroups.DrawMode = DrawMode.OwnerDrawFixed;
            this.cbGroups.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbGroups.Location = new Point(0, 39);
            this.cbGroups.Name = "cbGroups";
            this.cbGroups.Size = new Size(172, 21);
            this.cbGroups.TabIndex = 13;
            this.cbGroups.DrawItem += new DrawItemEventHandler(this.cbGroups_DrawItem);
            this.cbGroups.SelectedIndexChanged += new EventHandler(this.cbGroups_SelectedIndexChanged);
            // 
            // labelPosition
            // 
            this.labelPosition.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.labelPosition.BackColor = Color.Transparent;
            this.labelPosition.Location = new Point(12, 9);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new Size(132, 31);
            this.labelPosition.TabIndex = 12;
            this.labelPosition.Text = "Position Label";
            // 
            // pictureBoxChannels
            // 
            this.pictureBoxChannels.BackColor = Color.White;
            this.pictureBoxChannels.ContextMenuStrip = this.contextMenuChannels;
            this.pictureBoxChannels.Dock = DockStyle.Fill;
            this.pictureBoxChannels.Location = new Point(0, 0);
            this.pictureBoxChannels.Name = "pictureBoxChannels";
            this.pictureBoxChannels.Size = new Size(175, 243);
            this.pictureBoxChannels.TabIndex = 11;
            this.pictureBoxChannels.TabStop = false;
            this.pictureBoxChannels.DragDrop += new DragEventHandler(this.pictureBoxChannels_DragDrop);
            this.pictureBoxChannels.DragOver += new DragEventHandler(this.pictureBoxChannels_DragOver);
            this.pictureBoxChannels.Paint += new PaintEventHandler(this.pictureBoxChannels_Paint);
            this.pictureBoxChannels.MouseDoubleClick += new MouseEventHandler(this.pictureBoxChannels_MouseDoubleClick);
            this.pictureBoxChannels.MouseDown += new MouseEventHandler(this.pictureBoxChannels_MouseDown);
            this.pictureBoxChannels.MouseMove += new MouseEventHandler(this.pictureBoxChannels_MouseMove);
            this.pictureBoxChannels.MouseUp += new MouseEventHandler(this.pictureBoxChannels_MouseUp);
            this.pictureBoxChannels.Resize += new EventHandler(this.pictureBoxChannels_Resize);
            // 
            // contextMenuChannels
            // 
            this.contextMenuChannels.Items.AddRange(new ToolStripItem[] {
            this.toggleOutputChannelsToolStripMenuItem,
            this.reorderChannelOutputsToolStripMenuItem,
            this.toolStripMenuItem5,
            this.clearChannelEventsToolStripMenuItem,
            this.allEventsToFullIntensityToolStripMenuItem,
            this.copyChannelToolStripMenuItem1,
            this.setAllChannelColorsToolStripMenuItem,
            this.selectAllEventsMenuItem,
            this.toolStripMenuItem23,
            this.copyChannelEventsToClipboardToolStripMenuItem,
            this.pasteFullChannelEventsFromClipboardToolStripMenuItem,
            this.toolStripMenuItem14,
            this.channelPropertiesToolStripMenuItem});
            this.contextMenuChannels.Name = "contextMenuChannels";
            this.contextMenuChannels.Size = new Size(287, 242);
            this.contextMenuChannels.Opening += new CancelEventHandler(this.contextMenuChannels_Opening);
            // 
            // toggleOutputChannelsToolStripMenuItem
            // 
            this.toggleOutputChannelsToolStripMenuItem.Name = "toggleOutputChannelsToolStripMenuItem";
            this.toggleOutputChannelsToolStripMenuItem.Size = new Size(286, 22);
            this.toggleOutputChannelsToolStripMenuItem.Text = "Toggle channel outputs";
            this.toggleOutputChannelsToolStripMenuItem.Click += new EventHandler(this.toggleOutputChannelsToolStripMenuItem_Click);
            // 
            // reorderChannelOutputsToolStripMenuItem
            // 
            this.reorderChannelOutputsToolStripMenuItem.Name = "reorderChannelOutputsToolStripMenuItem";
            this.reorderChannelOutputsToolStripMenuItem.Size = new Size(286, 22);
            this.reorderChannelOutputsToolStripMenuItem.Text = "Reorder channel outputs";
            this.reorderChannelOutputsToolStripMenuItem.Click += new EventHandler(this.reorderChannelOutputsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new Size(283, 6);
            // 
            // clearChannelEventsToolStripMenuItem
            // 
            this.clearChannelEventsToolStripMenuItem.Name = "clearChannelEventsToolStripMenuItem";
            this.clearChannelEventsToolStripMenuItem.Size = new Size(286, 22);
            this.clearChannelEventsToolStripMenuItem.Text = "Clear channel events";
            this.clearChannelEventsToolStripMenuItem.Click += new EventHandler(this.clearChannelEventsToolStripMenuItem_Click);
            // 
            // allEventsToFullIntensityToolStripMenuItem
            // 
            this.allEventsToFullIntensityToolStripMenuItem.Name = "allEventsToFullIntensityToolStripMenuItem";
            this.allEventsToFullIntensityToolStripMenuItem.Size = new Size(286, 22);
            this.allEventsToFullIntensityToolStripMenuItem.Text = "All events to full intensity";
            this.allEventsToFullIntensityToolStripMenuItem.Click += new EventHandler(this.allEventsToFullIntensityToolStripMenuItem_Click);
            // 
            // copyChannelToolStripMenuItem1
            // 
            this.copyChannelToolStripMenuItem1.Name = "copyChannelToolStripMenuItem1";
            this.copyChannelToolStripMenuItem1.Size = new Size(286, 22);
            this.copyChannelToolStripMenuItem1.Text = "Copy channel...";
            this.copyChannelToolStripMenuItem1.Click += new EventHandler(this.copyChannelToolStripMenuItem_Click);
            // 
            // setAllChannelColorsToolStripMenuItem
            // 
            this.setAllChannelColorsToolStripMenuItem.Name = "setAllChannelColorsToolStripMenuItem";
            this.setAllChannelColorsToolStripMenuItem.Size = new Size(286, 22);
            this.setAllChannelColorsToolStripMenuItem.Text = "Set all channel colors";
            this.setAllChannelColorsToolStripMenuItem.Click += new EventHandler(this.setAllChannelColorsToolStripMenuItem_Click);
            // 
            // selectAllEventsMenuItem
            // 
            this.selectAllEventsMenuItem.Name = "selectAllEventsMenuItem";
            this.selectAllEventsMenuItem.Size = new Size(286, 22);
            this.selectAllEventsMenuItem.Text = "Select All Channel Events";
            this.selectAllEventsMenuItem.Click += new EventHandler(this.selectAllEventsMenuItem_Click);
            // 
            // toolStripMenuItem23
            // 
            this.toolStripMenuItem23.Name = "toolStripMenuItem23";
            this.toolStripMenuItem23.Size = new Size(283, 6);
            // 
            // copyChannelEventsToClipboardToolStripMenuItem
            // 
            this.copyChannelEventsToClipboardToolStripMenuItem.Name = "copyChannelEventsToClipboardToolStripMenuItem";
            this.copyChannelEventsToClipboardToolStripMenuItem.Size = new Size(286, 22);
            this.copyChannelEventsToClipboardToolStripMenuItem.Text = "Copy full channel events to clipboard";
            this.copyChannelEventsToClipboardToolStripMenuItem.Click += new EventHandler(this.copyChannelEventsToClipboardToolStripMenuItem_Click);
            // 
            // pasteFullChannelEventsFromClipboardToolStripMenuItem
            // 
            this.pasteFullChannelEventsFromClipboardToolStripMenuItem.Name = "pasteFullChannelEventsFromClipboardToolStripMenuItem";
            this.pasteFullChannelEventsFromClipboardToolStripMenuItem.Size = new Size(286, 22);
            this.pasteFullChannelEventsFromClipboardToolStripMenuItem.Text = "Paste full channel events from clipboard";
            this.pasteFullChannelEventsFromClipboardToolStripMenuItem.Click += new EventHandler(this.pasteFullChannelEventsFromClipboardToolStripMenuItem_Click);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new Size(283, 6);
            // 
            // channelPropertiesToolStripMenuItem
            // 
            this.channelPropertiesToolStripMenuItem.Name = "channelPropertiesToolStripMenuItem";
            this.channelPropertiesToolStripMenuItem.Size = new Size(286, 22);
            this.channelPropertiesToolStripMenuItem.Text = "Channel properties";
            this.channelPropertiesToolStripMenuItem.Click += new EventHandler(this.channelPropertiesToolStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = DockStyle.Fill;
            this.splitContainer2.FixedPanel = FixedPanel.Panel1;
            this.splitContainer2.Location = new Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pictureBoxTime);
            this.splitContainer2.Panel1MinSize = 60;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lblFollowMouse);
            this.splitContainer2.Panel2.Controls.Add(this.pictureBoxGrid);
            this.splitContainer2.Panel2.Controls.Add(this.vScrollBar1);
            this.splitContainer2.Panel2.Controls.Add(this.hScrollBar1);
            this.splitContainer2.Size = new Size(797, 243);
            this.splitContainer2.SplitterDistance = 60;
            this.splitContainer2.TabIndex = 5;
            this.splitContainer2.TabStop = false;
            this.splitContainer2.SplitterMoving += new SplitterCancelEventHandler(this.splitContainer2_SplitterMoving);
            this.splitContainer2.SplitterMoved += new SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // pictureBoxTime
            // 
            this.pictureBoxTime.BackColor = Color.White;
            this.pictureBoxTime.ContextMenuStrip = this.contextMenuTime;
            this.pictureBoxTime.Dock = DockStyle.Top;
            this.pictureBoxTime.Location = new Point(0, 0);
            this.pictureBoxTime.Name = "pictureBoxTime";
            this.pictureBoxTime.Size = new Size(797, 60);
            this.pictureBoxTime.TabIndex = 1;
            this.pictureBoxTime.TabStop = false;
            this.pictureBoxTime.Paint += new PaintEventHandler(this.pictureBoxTime_Paint);
            this.pictureBoxTime.DoubleClick += new EventHandler(this.pictureBoxTime_DoubleClick);
            this.pictureBoxTime.Resize += new EventHandler(this.pictureBoxChannels_Resize);
            // 
            // contextMenuTime
            // 
            this.contextMenuTime.Items.AddRange(new ToolStripItem[] {
            this.clearAllChannelsForThisEventToolStripMenuItem,
            this.allChannelsToFullIntensityForThisEventToolStripMenuItem,
            this.selectAllChannelsForPeriod});
            this.contextMenuTime.Name = "contextMenuTime";
            this.contextMenuTime.Size = new Size(293, 70);
            this.contextMenuTime.Opening += new CancelEventHandler(this.contextMenuTime_Opening);
            // 
            // clearAllChannelsForThisEventToolStripMenuItem
            // 
            this.clearAllChannelsForThisEventToolStripMenuItem.Name = "clearAllChannelsForThisEventToolStripMenuItem";
            this.clearAllChannelsForThisEventToolStripMenuItem.Size = new Size(292, 22);
            this.clearAllChannelsForThisEventToolStripMenuItem.Text = "Clear all channels for this event";
            this.clearAllChannelsForThisEventToolStripMenuItem.Click += new EventHandler(this.clearAllChannelsForThisEventToolStripMenuItem_Click);
            // 
            // allChannelsToFullIntensityForThisEventToolStripMenuItem
            // 
            this.allChannelsToFullIntensityForThisEventToolStripMenuItem.Name = "allChannelsToFullIntensityForThisEventToolStripMenuItem";
            this.allChannelsToFullIntensityForThisEventToolStripMenuItem.Size = new Size(292, 22);
            this.allChannelsToFullIntensityForThisEventToolStripMenuItem.Text = "All channels to full intensity for this event";
            this.allChannelsToFullIntensityForThisEventToolStripMenuItem.Click += new EventHandler(this.allChannelsToFullIntensityForThisEventToolStripMenuItem_Click);
            // 
            // selectAllChannelsForPeriod
            // 
            this.selectAllChannelsForPeriod.Name = "selectAllChannelsForPeriod";
            this.selectAllChannelsForPeriod.Size = new Size(292, 22);
            this.selectAllChannelsForPeriod.Text = "Select All Events for this Period";
            this.selectAllChannelsForPeriod.Click += new EventHandler(this.selectAllChannelsForPeriod_Click);
            // 
            // lblFollowMouse
            // 
            this.lblFollowMouse.AutoSize = true;
            this.lblFollowMouse.Location = new Point(3, 0);
            this.lblFollowMouse.Name = "lblFollowMouse";
            this.lblFollowMouse.Size = new Size(95, 13);
            this.lblFollowMouse.TabIndex = 8;
            this.lblFollowMouse.Text = "FollowMouseLabel";
            this.lblFollowMouse.TextAlign = ContentAlignment.BottomRight;
            this.lblFollowMouse.Visible = false;
            // 
            // pictureBoxGrid
            // 
            this.pictureBoxGrid.BackColor = Color.White;
            this.pictureBoxGrid.ContextMenuStrip = this.contextMenuGrid;
            this.pictureBoxGrid.Dock = DockStyle.Fill;
            this.pictureBoxGrid.Location = new Point(0, 0);
            this.pictureBoxGrid.Name = "pictureBoxGrid";
            this.pictureBoxGrid.Size = new Size(780, 162);
            this.pictureBoxGrid.TabIndex = 5;
            this.pictureBoxGrid.TabStop = false;
            this.pictureBoxGrid.Paint += new PaintEventHandler(this.pictureBoxGrid_Paint);
            this.pictureBoxGrid.DoubleClick += new EventHandler(this.pictureBoxGrid_DoubleClick);
            this.pictureBoxGrid.MouseDown += new MouseEventHandler(this.pictureBoxGrid_MouseDown);
            this.pictureBoxGrid.MouseLeave += new EventHandler(this.pictureBoxGrid_MouseLeave);
            this.pictureBoxGrid.MouseMove += new MouseEventHandler(this.pictureBoxGrid_MouseMove);
            this.pictureBoxGrid.MouseUp += new MouseEventHandler(this.pictureBoxGrid_MouseUp);
            this.pictureBoxGrid.Resize += new EventHandler(this.pictureBoxGrid_Resize);
            // 
            // contextMenuGrid
            // 
            this.contextMenuGrid.Items.AddRange(new ToolStripItem[] {
            this.onToolStripMenuItem,
            this.offToolStripMenuItem,
            this.toolStripMenuItem2,
            this.rampOnToolStripMenuItem,
            this.rampOffToolStripMenuItem,
            this.toolStripMenuItem3,
            this.partialRampOnToolStripMenuItem,
            this.partialRampOffToolStripMenuItem,
            this.toolStripMenuItem6,
            this.cutToolStripMenuItem1,
            this.copyToolStripMenuItem1,
            this.opaquePasteToolStripMenuItem1,
            this.transparentPasteToolStripMenuItem1,
            this.pasteToolStripMenuItem1,
            this.removeCellsToolStripMenuItem,
            this.toolStripMenuItem12,
            this.findAndReplaceToolStripMenuItem1,
            this.toolStripMenuItem21,
            this.setIntensityToolStripMenuItem1,
            this.mirrorVerticallyToolStripMenuItem,
            this.mirrorHorizontallyToolStripMenuItem,
            this.invertToolStripMenuItem,
            this.randomToolStripMenuItem1,
            this.shimmerToolStripMenuItem,
            this.sparkleToolStripMenuItem,
            this.toolStripMenuItem11,
            this.saveAsARoutineToolStripMenuItem,
            this.loadARoutineToolStripMenuItem,
            this.loadRoutineToClipboardToolStripMenuItem});
            this.contextMenuGrid.Name = "contextMenuGrid";
            this.contextMenuGrid.Size = new Size(209, 546);
            this.contextMenuGrid.Opening += new CancelEventHandler(this.contextMenuGrid_Opening);
            // 
            // onToolStripMenuItem
            // 
            this.onToolStripMenuItem.Name = "onToolStripMenuItem";
            this.onToolStripMenuItem.Size = new Size(208, 22);
            this.onToolStripMenuItem.Text = "On";
            this.onToolStripMenuItem.Click += new EventHandler(this.toolStripButtonOn_Click);
            // 
            // offToolStripMenuItem
            // 
            this.offToolStripMenuItem.Name = "offToolStripMenuItem";
            this.offToolStripMenuItem.Size = new Size(208, 22);
            this.offToolStripMenuItem.Text = "Off";
            this.offToolStripMenuItem.Click += new EventHandler(this.toolStripButtonOff_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(205, 6);
            // 
            // rampOnToolStripMenuItem
            // 
            this.rampOnToolStripMenuItem.Name = "rampOnToolStripMenuItem";
            this.rampOnToolStripMenuItem.Size = new Size(208, 22);
            this.rampOnToolStripMenuItem.Text = "Ramp on";
            this.rampOnToolStripMenuItem.Click += new EventHandler(this.toolStripButtonRampOn_Click);
            // 
            // rampOffToolStripMenuItem
            // 
            this.rampOffToolStripMenuItem.Name = "rampOffToolStripMenuItem";
            this.rampOffToolStripMenuItem.Size = new Size(208, 22);
            this.rampOffToolStripMenuItem.Text = "Ramp off";
            this.rampOffToolStripMenuItem.Click += new EventHandler(this.toolStripButtonRampOff_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new Size(205, 6);
            // 
            // partialRampOnToolStripMenuItem
            // 
            this.partialRampOnToolStripMenuItem.Name = "partialRampOnToolStripMenuItem";
            this.partialRampOnToolStripMenuItem.Size = new Size(208, 22);
            this.partialRampOnToolStripMenuItem.Text = "Partial ramp on";
            this.partialRampOnToolStripMenuItem.Click += new EventHandler(this.toolStripButtonPartialRampOn_Click);
            // 
            // partialRampOffToolStripMenuItem
            // 
            this.partialRampOffToolStripMenuItem.Name = "partialRampOffToolStripMenuItem";
            this.partialRampOffToolStripMenuItem.Size = new Size(208, 22);
            this.partialRampOffToolStripMenuItem.Text = "Partial ramp off";
            this.partialRampOffToolStripMenuItem.Click += new EventHandler(this.toolStripButtonPartialRampOff_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new Size(205, 6);
            // 
            // cutToolStripMenuItem1
            // 
            this.cutToolStripMenuItem1.Name = "cutToolStripMenuItem1";
            this.cutToolStripMenuItem1.Size = new Size(208, 22);
            this.cutToolStripMenuItem1.Text = "Cut";
            this.cutToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonCut_Click);
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.Size = new Size(208, 22);
            this.copyToolStripMenuItem1.Text = "Copy";
            this.copyToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonCopy_Click);
            // 
            // opaquePasteToolStripMenuItem1
            // 
            this.opaquePasteToolStripMenuItem1.Name = "opaquePasteToolStripMenuItem1";
            this.opaquePasteToolStripMenuItem1.Size = new Size(208, 22);
            this.opaquePasteToolStripMenuItem1.Text = "Opaque paste";
            this.opaquePasteToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonOpaquePaste_Click);
            // 
            // transparentPasteToolStripMenuItem1
            // 
            this.transparentPasteToolStripMenuItem1.Name = "transparentPasteToolStripMenuItem1";
            this.transparentPasteToolStripMenuItem1.Size = new Size(208, 22);
            this.transparentPasteToolStripMenuItem1.Text = "Transparent paste";
            this.transparentPasteToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonTransparentPaste_Click);
            // 
            // pasteToolStripMenuItem1
            // 
            this.pasteToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] {
            this.booleanPasteToolStripMenuItem1,
            this.insertPasteToolStripMenuItem1,
            this.arithmeticPasteToolStripMenuItem});
            this.pasteToolStripMenuItem1.Name = "pasteToolStripMenuItem1";
            this.pasteToolStripMenuItem1.Size = new Size(208, 22);
            this.pasteToolStripMenuItem1.Text = "More paste";
            // 
            // booleanPasteToolStripMenuItem1
            // 
            this.booleanPasteToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] {
            this.oRToolStripMenuItem1,
            this.aNDToolStripMenuItem1,
            this.xORToolStripMenuItem1,
            this.toolStripMenuItem17,
            this.nORToolStripMenuItem1,
            this.nANDToolStripMenuItem1,
            this.xNORToolStripMenuItem1});
            this.booleanPasteToolStripMenuItem1.Name = "booleanPasteToolStripMenuItem1";
            this.booleanPasteToolStripMenuItem1.Size = new Size(161, 22);
            this.booleanPasteToolStripMenuItem1.Text = "Boolean paste";
            // 
            // oRToolStripMenuItem1
            // 
            this.oRToolStripMenuItem1.Name = "oRToolStripMenuItem1";
            this.oRToolStripMenuItem1.Size = new Size(172, 22);
            this.oRToolStripMenuItem1.Text = "OR";
            this.oRToolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItemPasteOr_Click);
            // 
            // aNDToolStripMenuItem1
            // 
            this.aNDToolStripMenuItem1.Name = "aNDToolStripMenuItem1";
            this.aNDToolStripMenuItem1.Size = new Size(172, 22);
            this.aNDToolStripMenuItem1.Text = "AND";
            this.aNDToolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItemPasteAnd_Click);
            // 
            // xORToolStripMenuItem1
            // 
            this.xORToolStripMenuItem1.Name = "xORToolStripMenuItem1";
            this.xORToolStripMenuItem1.Size = new Size(172, 22);
            this.xORToolStripMenuItem1.Text = "XOR";
            this.xORToolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItemPasteXor_Click);
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new Size(169, 6);
            // 
            // nORToolStripMenuItem1
            // 
            this.nORToolStripMenuItem1.Name = "nORToolStripMenuItem1";
            this.nORToolStripMenuItem1.Size = new Size(172, 22);
            this.nORToolStripMenuItem1.Text = "NOR (NOT OR)";
            this.nORToolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItemPasteNor_Click);
            // 
            // nANDToolStripMenuItem1
            // 
            this.nANDToolStripMenuItem1.Name = "nANDToolStripMenuItem1";
            this.nANDToolStripMenuItem1.Size = new Size(172, 22);
            this.nANDToolStripMenuItem1.Text = "NAND (NOT AND)";
            this.nANDToolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItemPasteNand_Click);
            // 
            // xNORToolStripMenuItem1
            // 
            this.xNORToolStripMenuItem1.Name = "xNORToolStripMenuItem1";
            this.xNORToolStripMenuItem1.Size = new Size(172, 22);
            this.xNORToolStripMenuItem1.Text = "XNOR (NOT XOR)";
            this.xNORToolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItemPasteXnor_Click);
            // 
            // insertPasteToolStripMenuItem1
            // 
            this.insertPasteToolStripMenuItem1.Name = "insertPasteToolStripMenuItem1";
            this.insertPasteToolStripMenuItem1.Size = new Size(161, 22);
            this.insertPasteToolStripMenuItem1.Text = "Insert Paste";
            this.insertPasteToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonInsertPaste_Click);
            // 
            // arithmeticPasteToolStripMenuItem
            // 
            this.arithmeticPasteToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.additionToolStripMenuItem1,
            this.subtractionToolStripMenuItem1,
            this.scaleToolStripMenuItem1,
            this.minToolStripMenuItem1,
            this.maxToolStripMenuItem1});
            this.arithmeticPasteToolStripMenuItem.Name = "arithmeticPasteToolStripMenuItem";
            this.arithmeticPasteToolStripMenuItem.Size = new Size(161, 22);
            this.arithmeticPasteToolStripMenuItem.Text = "Arithmetic Paste";
            // 
            // additionToolStripMenuItem1
            // 
            this.additionToolStripMenuItem1.Name = "additionToolStripMenuItem1";
            this.additionToolStripMenuItem1.Size = new Size(135, 22);
            this.additionToolStripMenuItem1.Text = "Addition";
            this.additionToolStripMenuItem1.Click += new EventHandler(this.additionToolStripMenuItem_Click);
            // 
            // subtractionToolStripMenuItem1
            // 
            this.subtractionToolStripMenuItem1.Name = "subtractionToolStripMenuItem1";
            this.subtractionToolStripMenuItem1.Size = new Size(135, 22);
            this.subtractionToolStripMenuItem1.Text = "Subtraction";
            this.subtractionToolStripMenuItem1.Click += new EventHandler(this.subtractionToolStripMenuItem_Click);
            // 
            // scaleToolStripMenuItem1
            // 
            this.scaleToolStripMenuItem1.Name = "scaleToolStripMenuItem1";
            this.scaleToolStripMenuItem1.Size = new Size(135, 22);
            this.scaleToolStripMenuItem1.Text = "Scale";
            this.scaleToolStripMenuItem1.Click += new EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // minToolStripMenuItem1
            // 
            this.minToolStripMenuItem1.Name = "minToolStripMenuItem1";
            this.minToolStripMenuItem1.Size = new Size(135, 22);
            this.minToolStripMenuItem1.Text = "Min";
            this.minToolStripMenuItem1.Click += new EventHandler(this.minToolStripMenuItem_Click);
            // 
            // maxToolStripMenuItem1
            // 
            this.maxToolStripMenuItem1.Name = "maxToolStripMenuItem1";
            this.maxToolStripMenuItem1.Size = new Size(135, 22);
            this.maxToolStripMenuItem1.Text = "Max";
            this.maxToolStripMenuItem1.Click += new EventHandler(this.maxToolStripMenuItem_Click);
            // 
            // removeCellsToolStripMenuItem
            // 
            this.removeCellsToolStripMenuItem.Name = "removeCellsToolStripMenuItem";
            this.removeCellsToolStripMenuItem.Size = new Size(208, 22);
            this.removeCellsToolStripMenuItem.Text = "Remove cells";
            this.removeCellsToolStripMenuItem.Click += new EventHandler(this.toolStripButtonRemoveCells_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new Size(205, 6);
            // 
            // findAndReplaceToolStripMenuItem1
            // 
            this.findAndReplaceToolStripMenuItem1.Name = "findAndReplaceToolStripMenuItem1";
            this.findAndReplaceToolStripMenuItem1.ShortcutKeys = ((Keys)((Keys.Control | Keys.F)));
            this.findAndReplaceToolStripMenuItem1.Size = new Size(208, 22);
            this.findAndReplaceToolStripMenuItem1.Text = "Find and replace";
            this.findAndReplaceToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonFindAndReplace_Click);
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            this.toolStripMenuItem21.Size = new Size(205, 6);
            // 
            // setIntensityToolStripMenuItem1
            // 
            this.setIntensityToolStripMenuItem1.Name = "setIntensityToolStripMenuItem1";
            this.setIntensityToolStripMenuItem1.Size = new Size(208, 22);
            this.setIntensityToolStripMenuItem1.Text = "Set intensity";
            this.setIntensityToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonIntensity_Click);
            // 
            // mirrorVerticallyToolStripMenuItem
            // 
            this.mirrorVerticallyToolStripMenuItem.Name = "mirrorVerticallyToolStripMenuItem";
            this.mirrorVerticallyToolStripMenuItem.Size = new Size(208, 22);
            this.mirrorVerticallyToolStripMenuItem.Text = "Mirror vertically";
            this.mirrorVerticallyToolStripMenuItem.Click += new EventHandler(this.toolStripButtonMirrorVertical_Click);
            // 
            // mirrorHorizontallyToolStripMenuItem
            // 
            this.mirrorHorizontallyToolStripMenuItem.Name = "mirrorHorizontallyToolStripMenuItem";
            this.mirrorHorizontallyToolStripMenuItem.Size = new Size(208, 22);
            this.mirrorHorizontallyToolStripMenuItem.Text = "Mirror horizontally";
            this.mirrorHorizontallyToolStripMenuItem.Click += new EventHandler(this.toolStripButtonMirrorHorizontal_Click);
            // 
            // invertToolStripMenuItem
            // 
            this.invertToolStripMenuItem.Name = "invertToolStripMenuItem";
            this.invertToolStripMenuItem.Size = new Size(208, 22);
            this.invertToolStripMenuItem.Text = "Invert";
            this.invertToolStripMenuItem.Click += new EventHandler(this.toolStripButtonInvert_Click);
            // 
            // randomToolStripMenuItem1
            // 
            this.randomToolStripMenuItem1.Name = "randomToolStripMenuItem1";
            this.randomToolStripMenuItem1.Size = new Size(208, 22);
            this.randomToolStripMenuItem1.Text = "Random";
            this.randomToolStripMenuItem1.Click += new EventHandler(this.toolStripButtonRandom_Click);
            // 
            // shimmerToolStripMenuItem
            // 
            this.shimmerToolStripMenuItem.Name = "shimmerToolStripMenuItem";
            this.shimmerToolStripMenuItem.Size = new Size(208, 22);
            this.shimmerToolStripMenuItem.Text = "Shimmer";
            this.shimmerToolStripMenuItem.Click += new EventHandler(this.toolStripButtonShimmerDimming_Click);
            // 
            // sparkleToolStripMenuItem
            // 
            this.sparkleToolStripMenuItem.Name = "sparkleToolStripMenuItem";
            this.sparkleToolStripMenuItem.Size = new Size(208, 22);
            this.sparkleToolStripMenuItem.Text = "Sparkle";
            this.sparkleToolStripMenuItem.Click += new EventHandler(this.toolStripButtonSparkle_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new Size(205, 6);
            // 
            // saveAsARoutineToolStripMenuItem
            // 
            this.saveAsARoutineToolStripMenuItem.Name = "saveAsARoutineToolStripMenuItem";
            this.saveAsARoutineToolStripMenuItem.Size = new Size(208, 22);
            this.saveAsARoutineToolStripMenuItem.Text = "Save as a routine";
            this.saveAsARoutineToolStripMenuItem.Click += new EventHandler(this.saveAsARoutineToolStripMenuItem_Click);
            // 
            // loadARoutineToolStripMenuItem
            // 
            this.loadARoutineToolStripMenuItem.Name = "loadARoutineToolStripMenuItem";
            this.loadARoutineToolStripMenuItem.Size = new Size(208, 22);
            this.loadARoutineToolStripMenuItem.Text = "Load a routine";
            this.loadARoutineToolStripMenuItem.Click += new EventHandler(this.loadARoutineToolStripMenuItem_Click);
            // 
            // loadRoutineToClipboardToolStripMenuItem
            // 
            this.loadRoutineToClipboardToolStripMenuItem.Name = "loadRoutineToClipboardToolStripMenuItem";
            this.loadRoutineToClipboardToolStripMenuItem.Size = new Size(208, 22);
            this.loadRoutineToClipboardToolStripMenuItem.Text = "Load routine to clipboard";
            this.loadRoutineToClipboardToolStripMenuItem.Click += new EventHandler(this.loadRoutineToClipboardToolStripMenuItem_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = DockStyle.Right;
            this.vScrollBar1.Location = new Point(780, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new Size(17, 162);
            this.vScrollBar1.TabIndex = 4;
            this.vScrollBar1.ValueChanged += new EventHandler(this.vScrollBar1_ValueChanged);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = DockStyle.Bottom;
            this.hScrollBar1.Location = new Point(0, 162);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new Size(797, 17);
            this.hScrollBar1.TabIndex = 3;
            this.hScrollBar1.ValueChanged += new EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // toolStripSequenceSettings
            // 
            this.toolStripSequenceSettings.Dock = DockStyle.None;
            this.toolStripSequenceSettings.ImageScalingSize = new Size(48, 48);
            this.toolStripSequenceSettings.Items.AddRange(new ToolStripItem[] {
            this.newSeqTsb,
            this.openSequenceTsb,
            this.tbsSave,
            this.tsbSaveAs,
            this.toolStripSeparator3,
            this.mapperTsb,
            this.toolStripSeparator8,
            this.tbsTestChannels,
            this.tbsTestConsole,
            this.toolStripSeparator1,
            this.tsbAudio,
            this.toolStripButtonWaveform,
            this.toolStripSeparator13,
            this.toolStripButtonChannelOutputMask,
            this.toolStripSeparator19,
            this.profileToolStripLabel});
            this.toolStripSequenceSettings.Location = new Point(3, 0);
            this.toolStripSequenceSettings.Name = "toolStripSequenceSettings";
            this.toolStripSequenceSettings.Size = new Size(672, 55);
            this.toolStripSequenceSettings.TabIndex = 1;
            this.toolStripSequenceSettings.Text = "Sequence settings";
            // 
            // newSeqTsb
            // 
            this.newSeqTsb.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.newSeqTsb.Image = ((Image)(resources.GetObject("newSeqTsb.Image")));
            this.newSeqTsb.ImageTransparentColor = Color.Magenta;
            this.newSeqTsb.Name = "newSeqTsb";
            this.newSeqTsb.Size = new Size(52, 52);
            this.newSeqTsb.ToolTipText = "New Sequence";
            this.newSeqTsb.Click += new EventHandler(this.newSeqTsb_Click);
            // 
            // openSequenceTsb
            // 
            this.openSequenceTsb.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.openSequenceTsb.Image = ((Image)(resources.GetObject("openSequenceTsb.Image")));
            this.openSequenceTsb.ImageTransparentColor = Color.Magenta;
            this.openSequenceTsb.Name = "openSequenceTsb";
            this.openSequenceTsb.Size = new Size(52, 52);
            this.openSequenceTsb.ToolTipText = "Open Sequence";
            this.openSequenceTsb.Click += new EventHandler(this.openSequenceTsb_Click);
            // 
            // tbsSave
            // 
            this.tbsSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tbsSave.Enabled = false;
            this.tbsSave.Image = ((Image)(resources.GetObject("tbsSave.Image")));
            this.tbsSave.ImageTransparentColor = Color.Magenta;
            this.tbsSave.Name = "tbsSave";
            this.tbsSave.Size = new Size(52, 52);
            this.tbsSave.ToolTipText = "Save Sequence";
            this.tbsSave.Click += new EventHandler(this.toolStripButtonSave_Click);
            // 
            // tsbSaveAs
            // 
            this.tsbSaveAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbSaveAs.Image = ((Image)(resources.GetObject("tsbSaveAs.Image")));
            this.tsbSaveAs.ImageTransparentColor = Color.Magenta;
            this.tsbSaveAs.Name = "tsbSaveAs";
            this.tsbSaveAs.Size = new Size(52, 52);
            this.tsbSaveAs.ToolTipText = "Save Sequence As (Shft-Ctrl-S)";
            this.tsbSaveAs.Click += new EventHandler(this.tsbSaveAs_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 55);
            // 
            // mapperTsb
            // 
            this.mapperTsb.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.mapperTsb.Image = ((Image)(resources.GetObject("mapperTsb.Image")));
            this.mapperTsb.ImageTransparentColor = Color.Magenta;
            this.mapperTsb.Name = "mapperTsb";
            this.mapperTsb.Size = new Size(52, 52);
            this.mapperTsb.ToolTipText = "Channel Mapper";
            this.mapperTsb.Click += new EventHandler(this.mapperTsb_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new Size(6, 55);
            // 
            // tbsTestChannels
            // 
            this.tbsTestChannels.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tbsTestChannels.Image = ((Image)(resources.GetObject("tbsTestChannels.Image")));
            this.tbsTestChannels.ImageTransparentColor = Color.Magenta;
            this.tbsTestChannels.Name = "tbsTestChannels";
            this.tbsTestChannels.Size = new Size(52, 52);
            this.tbsTestChannels.ToolTipText = "Test channels";
            this.tbsTestChannels.Click += new EventHandler(this.toolStripButtonTestChannels_Click);
            // 
            // tbsTestConsole
            // 
            this.tbsTestConsole.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tbsTestConsole.Image = ((Image)(resources.GetObject("tbsTestConsole.Image")));
            this.tbsTestConsole.ImageTransparentColor = Color.Magenta;
            this.tbsTestConsole.Name = "tbsTestConsole";
            this.tbsTestConsole.Size = new Size(52, 52);
            this.tbsTestConsole.ToolTipText = "Test console";
            this.tbsTestConsole.Click += new EventHandler(this.toolStripButtonTestConsole_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 55);
            // 
            // tsbAudio
            // 
            this.tsbAudio.BackColor = SystemColors.Control;
            this.tsbAudio.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbAudio.Image = ((Image)(resources.GetObject("tsbAudio.Image")));
            this.tsbAudio.ImageTransparentColor = Color.Transparent;
            this.tsbAudio.Name = "tsbAudio";
            this.tsbAudio.Size = new Size(52, 52);
            this.tsbAudio.ToolTipText = "Add or change audio...";
            this.tsbAudio.Click += new EventHandler(this.toolStripButtonAudio_Click);
            // 
            // toolStripButtonWaveform
            // 
            this.toolStripButtonWaveform.CheckOnClick = true;
            this.toolStripButtonWaveform.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWaveform.Enabled = false;
            this.toolStripButtonWaveform.Image = ((Image)(resources.GetObject("toolStripButtonWaveform.Image")));
            this.toolStripButtonWaveform.ImageTransparentColor = Color.White;
            this.toolStripButtonWaveform.Name = "toolStripButtonWaveform";
            this.toolStripButtonWaveform.Size = new Size(52, 52);
            this.toolStripButtonWaveform.ToolTipText = "Show Audio Waveform";
            this.toolStripButtonWaveform.Click += new EventHandler(this.toolStripButtonWaveform_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new Size(6, 55);
            // 
            // toolStripButtonChannelOutputMask
            // 
            this.toolStripButtonChannelOutputMask.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonChannelOutputMask.Image = ((Image)(resources.GetObject("toolStripButtonChannelOutputMask.Image")));
            this.toolStripButtonChannelOutputMask.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonChannelOutputMask.Name = "toolStripButtonChannelOutputMask";
            this.toolStripButtonChannelOutputMask.Size = new Size(52, 52);
            this.toolStripButtonChannelOutputMask.ToolTipText = "Enable/disable channels for this sequence";
            this.toolStripButtonChannelOutputMask.Click += new EventHandler(this.toolStripButtonChannelOutputMask_Click);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new Size(6, 55);
            // 
            // profileToolStripLabel
            // 
            this.profileToolStripLabel.AutoSize = false;
            this.profileToolStripLabel.Name = "profileToolStripLabel";
            this.profileToolStripLabel.Size = new Size(110, 52);
            this.profileToolStripLabel.Text = "Profile";
            this.profileToolStripLabel.ToolTipText = "Click to Edit Profile";
            this.profileToolStripLabel.Click += new EventHandler(this.profileToolStripLabel_Click);
            // 
            // toolStripExecutionControl
            // 
            this.toolStripExecutionControl.Dock = DockStyle.None;
            this.toolStripExecutionControl.ImageScalingSize = new Size(48, 48);
            this.toolStripExecutionControl.Items.AddRange(new ToolStripItem[] {
            this.tsbPlay,
            this.tsbPlayPoint,
            this.tsbPlayRange,
            this.tsbPause,
            this.tsbStop,
            this.tsbLoop,
            this.toolStripSeparator10,
            this.SpeedQtrTsb,
            this.SpeedHalfTsb,
            this.SpeedThreeQtrTsb,
            this.SpeedNormalTsb,
            this.SpeedVariableTsb});
            this.toolStripExecutionControl.Location = new Point(3, 55);
            this.toolStripExecutionControl.Name = "toolStripExecutionControl";
            this.toolStripExecutionControl.Size = new Size(590, 55);
            this.toolStripExecutionControl.TabIndex = 2;
            this.toolStripExecutionControl.Text = "Execution control";
            // 
            // tsbPlay
            // 
            this.tsbPlay.BackgroundImageLayout = ImageLayout.Center;
            this.tsbPlay.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbPlay.Image = ((Image)(resources.GetObject("tsbPlay.Image")));
            this.tsbPlay.ImageTransparentColor = Color.Magenta;
            this.tsbPlay.Name = "tsbPlay";
            this.tsbPlay.Size = new Size(52, 52);
            this.tsbPlay.ToolTipText = "Play this sequence (F5)";
            this.tsbPlay.Click += new EventHandler(this.toolStripButtonPlay_Click);
            // 
            // tsbPlayPoint
            // 
            this.tsbPlayPoint.BackgroundImageLayout = ImageLayout.Center;
            this.tsbPlayPoint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbPlayPoint.Enabled = false;
            this.tsbPlayPoint.Image = ((Image)(resources.GetObject("tsbPlayPoint.Image")));
            this.tsbPlayPoint.ImageTransparentColor = Color.Magenta;
            this.tsbPlayPoint.Name = "tsbPlayPoint";
            this.tsbPlayPoint.Size = new Size(52, 52);
            this.tsbPlayPoint.ToolTipText = "Play from point (F6)";
            this.tsbPlayPoint.Click += new EventHandler(this.toolStripButtonPlayPoint_Click);
            // 
            // tsbPlayRange
            // 
            this.tsbPlayRange.BackgroundImageLayout = ImageLayout.Center;
            this.tsbPlayRange.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbPlayRange.Enabled = false;
            this.tsbPlayRange.Image = ((Image)(resources.GetObject("tsbPlayRange.Image")));
            this.tsbPlayRange.ImageTransparentColor = Color.Magenta;
            this.tsbPlayRange.Name = "tsbPlayRange";
            this.tsbPlayRange.Size = new Size(52, 52);
            this.tsbPlayRange.ToolTipText = "Play Range (Alt-F6)";
            this.tsbPlayRange.Click += new EventHandler(this.toolStripButtonPlayRange_Click);
            // 
            // tsbPause
            // 
            this.tsbPause.BackgroundImageLayout = ImageLayout.Center;
            this.tsbPause.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbPause.Enabled = false;
            this.tsbPause.Image = ((Image)(resources.GetObject("tsbPause.Image")));
            this.tsbPause.ImageTransparentColor = Color.Magenta;
            this.tsbPause.Name = "tsbPause";
            this.tsbPause.Size = new Size(52, 52);
            this.tsbPause.ToolTipText = "Pause (F7)";
            this.tsbPause.Click += new EventHandler(this.toolStripButtonPause_Click);
            // 
            // tsbStop
            // 
            this.tsbStop.BackgroundImageLayout = ImageLayout.Center;
            this.tsbStop.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbStop.Enabled = false;
            this.tsbStop.Image = ((Image)(resources.GetObject("tsbStop.Image")));
            this.tsbStop.ImageTransparentColor = Color.Magenta;
            this.tsbStop.Name = "tsbStop";
            this.tsbStop.Size = new Size(52, 52);
            this.tsbStop.ToolTipText = "Stop playing (F8)";
            this.tsbStop.Click += new EventHandler(this.toolStripButtonStop_Click);
            // 
            // tsbLoop
            // 
            this.tsbLoop.BackgroundImageLayout = ImageLayout.Center;
            this.tsbLoop.CheckOnClick = true;
            this.tsbLoop.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbLoop.Image = ((Image)(resources.GetObject("tsbLoop.Image")));
            this.tsbLoop.ImageTransparentColor = Color.Transparent;
            this.tsbLoop.Name = "tsbLoop";
            this.tsbLoop.Size = new Size(52, 52);
            this.tsbLoop.ToolTipText = "Loop this sequence";
            this.tsbLoop.CheckedChanged += new EventHandler(this.toolStripButtonLoop_CheckedChanged);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new Size(6, 55);
            // 
            // SpeedQtrTsb
            // 
            this.SpeedQtrTsb.BackgroundImageLayout = ImageLayout.Center;
            this.SpeedQtrTsb.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.SpeedQtrTsb.Image = ((Image)(resources.GetObject("SpeedQtrTsb.Image")));
            this.SpeedQtrTsb.ImageTransparentColor = Color.Magenta;
            this.SpeedQtrTsb.Name = "SpeedQtrTsb";
            this.SpeedQtrTsb.Size = new Size(52, 52);
            this.SpeedQtrTsb.ToolTipText = "Play at 1/4 of normal speed";
            this.SpeedQtrTsb.Click += new EventHandler(this.toolStripButtonPlaySpeedQuarter_Click);
            // 
            // SpeedHalfTsb
            // 
            this.SpeedHalfTsb.BackgroundImageLayout = ImageLayout.Center;
            this.SpeedHalfTsb.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.SpeedHalfTsb.Image = ((Image)(resources.GetObject("SpeedHalfTsb.Image")));
            this.SpeedHalfTsb.ImageTransparentColor = Color.Magenta;
            this.SpeedHalfTsb.Name = "SpeedHalfTsb";
            this.SpeedHalfTsb.Size = new Size(52, 52);
            this.SpeedHalfTsb.ToolTipText = "Play at 1/2 of normal speed";
            this.SpeedHalfTsb.Click += new EventHandler(this.toolStripButtonPlaySpeedHalf_Click);
            // 
            // SpeedThreeQtrTsb
            // 
            this.SpeedThreeQtrTsb.BackgroundImageLayout = ImageLayout.Center;
            this.SpeedThreeQtrTsb.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.SpeedThreeQtrTsb.Image = ((Image)(resources.GetObject("SpeedThreeQtrTsb.Image")));
            this.SpeedThreeQtrTsb.ImageTransparentColor = Color.Magenta;
            this.SpeedThreeQtrTsb.Name = "SpeedThreeQtrTsb";
            this.SpeedThreeQtrTsb.Size = new Size(52, 52);
            this.SpeedThreeQtrTsb.ToolTipText = "Play at 3/4 of normal speed";
            this.SpeedThreeQtrTsb.Click += new EventHandler(this.toolStripButtonPlaySpeedThreeQuarters_Click);
            // 
            // SpeedNormalTsb
            // 
            this.SpeedNormalTsb.BackgroundImage = ((Image)(resources.GetObject("SpeedNormalTsb.BackgroundImage")));
            this.SpeedNormalTsb.BackgroundImageLayout = ImageLayout.Center;
            this.SpeedNormalTsb.Checked = true;
            this.SpeedNormalTsb.CheckState = CheckState.Checked;
            this.SpeedNormalTsb.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.SpeedNormalTsb.Image = ((Image)(resources.GetObject("SpeedNormalTsb.Image")));
            this.SpeedNormalTsb.ImageTransparentColor = Color.Magenta;
            this.SpeedNormalTsb.Name = "SpeedNormalTsb";
            this.SpeedNormalTsb.Size = new Size(52, 52);
            this.SpeedNormalTsb.ToolTipText = "Play at normal speed";
            this.SpeedNormalTsb.Click += new EventHandler(this.toolStripButtonPlaySpeedNormal_Click);
            // 
            // SpeedVariableTsb
            // 
            this.SpeedVariableTsb.BackgroundImageLayout = ImageLayout.Center;
            this.SpeedVariableTsb.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.SpeedVariableTsb.Image = ((Image)(resources.GetObject("SpeedVariableTsb.Image")));
            this.SpeedVariableTsb.ImageTransparentColor = Color.Magenta;
            this.SpeedVariableTsb.Name = "SpeedVariableTsb";
            this.SpeedVariableTsb.Size = new Size(52, 52);
            this.SpeedVariableTsb.ToolTipText = "Variable playback speed";
            this.SpeedVariableTsb.Click += new EventHandler(this.toolStripButtonPlaySpeedVariable_Click);
            // 
            // toolStripEffect
            // 
            this.toolStripEffect.BackgroundImageLayout = ImageLayout.Center;
            this.toolStripEffect.Dock = DockStyle.None;
            this.toolStripEffect.ImageScalingSize = new Size(48, 48);
            this.toolStripEffect.Items.AddRange(new ToolStripItem[] {
            this.toolStripButtonOn,
            this.toolStripButtonOff,
            this.toolStripButtonRampOn,
            this.toolStripButtonRampOff,
            this.toolStripButtonPartialRampOn,
            this.toolStripButtonPartialRampOff,
            this.toolStripButtonToggleRamps,
            this.toolStripButtonIntensity,
            this.toolStripButtonMirrorVertical,
            this.toolStripButtonMirrorHorizontal,
            this.toolStripButtonInvert,
            this.toolStripButtonRandom,
            this.toolStripButtonSparkle,
            this.toolStripButtonShimmerDimming,
            this.tsbNutcracker,
            this.toolStripSeparator16,
            this.toolStripButtonToggleLevels,
            this.toolStripButtonToggleCellText,
            this.toolStripButtonChangeIntensity});
            this.toolStripEffect.Location = new Point(3, 110);
            this.toolStripEffect.Name = "toolStripEffect";
            this.toolStripEffect.Size = new Size(954, 55);
            this.toolStripEffect.TabIndex = 3;
            this.toolStripEffect.Text = "Effects";
            // 
            // toolStripButtonOn
            // 
            this.toolStripButtonOn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOn.Image = ((Image)(resources.GetObject("toolStripButtonOn.Image")));
            this.toolStripButtonOn.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonOn.Name = "toolStripButtonOn";
            this.toolStripButtonOn.Size = new Size(52, 52);
            this.toolStripButtonOn.ToolTipText = "On";
            this.toolStripButtonOn.Click += new EventHandler(this.toolStripButtonOn_Click);
            // 
            // toolStripButtonOff
            // 
            this.toolStripButtonOff.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOff.Image = ((Image)(resources.GetObject("toolStripButtonOff.Image")));
            this.toolStripButtonOff.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonOff.Name = "toolStripButtonOff";
            this.toolStripButtonOff.Size = new Size(52, 52);
            this.toolStripButtonOff.ToolTipText = "Off";
            this.toolStripButtonOff.Click += new EventHandler(this.toolStripButtonOff_Click);
            // 
            // toolStripButtonRampOn
            // 
            this.toolStripButtonRampOn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRampOn.Image = ((Image)(resources.GetObject("toolStripButtonRampOn.Image")));
            this.toolStripButtonRampOn.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonRampOn.Name = "toolStripButtonRampOn";
            this.toolStripButtonRampOn.Size = new Size(52, 52);
            this.toolStripButtonRampOn.ToolTipText = "Ramp on (R)";
            this.toolStripButtonRampOn.Click += new EventHandler(this.toolStripButtonRampOn_Click);
            // 
            // toolStripButtonRampOff
            // 
            this.toolStripButtonRampOff.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRampOff.Image = ((Image)(resources.GetObject("toolStripButtonRampOff.Image")));
            this.toolStripButtonRampOff.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonRampOff.Name = "toolStripButtonRampOff";
            this.toolStripButtonRampOff.Size = new Size(52, 52);
            this.toolStripButtonRampOff.ToolTipText = "Ramp off (F)";
            this.toolStripButtonRampOff.Click += new EventHandler(this.toolStripButtonRampOff_Click);
            // 
            // toolStripButtonPartialRampOn
            // 
            this.toolStripButtonPartialRampOn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPartialRampOn.Image = ((Image)(resources.GetObject("toolStripButtonPartialRampOn.Image")));
            this.toolStripButtonPartialRampOn.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonPartialRampOn.Name = "toolStripButtonPartialRampOn";
            this.toolStripButtonPartialRampOn.Size = new Size(52, 52);
            this.toolStripButtonPartialRampOn.ToolTipText = "Partial ramp on (Shift-R)";
            this.toolStripButtonPartialRampOn.Click += new EventHandler(this.toolStripButtonPartialRampOn_Click);
            // 
            // toolStripButtonPartialRampOff
            // 
            this.toolStripButtonPartialRampOff.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPartialRampOff.Image = ((Image)(resources.GetObject("toolStripButtonPartialRampOff.Image")));
            this.toolStripButtonPartialRampOff.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonPartialRampOff.Name = "toolStripButtonPartialRampOff";
            this.toolStripButtonPartialRampOff.Size = new Size(52, 52);
            this.toolStripButtonPartialRampOff.ToolTipText = "Partial ramp off (Shift-F)";
            this.toolStripButtonPartialRampOff.Click += new EventHandler(this.toolStripButtonPartialRampOff_Click);
            // 
            // toolStripButtonToggleRamps
            // 
            this.toolStripButtonToggleRamps.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonToggleRamps.Image = ((Image)(resources.GetObject("toolStripButtonToggleRamps.Image")));
            this.toolStripButtonToggleRamps.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonToggleRamps.Name = "toolStripButtonToggleRamps";
            this.toolStripButtonToggleRamps.Size = new Size(52, 52);
            this.toolStripButtonToggleRamps.ToolTipText = "Toggle between gradient and bar ramps";
            this.toolStripButtonToggleRamps.Click += new EventHandler(this.toolStripButtonToggleRamps_Click);
            // 
            // toolStripButtonIntensity
            // 
            this.toolStripButtonIntensity.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonIntensity.Image = ((Image)(resources.GetObject("toolStripButtonIntensity.Image")));
            this.toolStripButtonIntensity.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonIntensity.Name = "toolStripButtonIntensity";
            this.toolStripButtonIntensity.Size = new Size(52, 52);
            this.toolStripButtonIntensity.ToolTipText = "Intensity (I)";
            this.toolStripButtonIntensity.Click += new EventHandler(this.toolStripButtonIntensity_Click);
            // 
            // toolStripButtonMirrorVertical
            // 
            this.toolStripButtonMirrorVertical.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMirrorVertical.Image = ((Image)(resources.GetObject("toolStripButtonMirrorVertical.Image")));
            this.toolStripButtonMirrorVertical.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonMirrorVertical.Name = "toolStripButtonMirrorVertical";
            this.toolStripButtonMirrorVertical.Size = new Size(52, 52);
            this.toolStripButtonMirrorVertical.ToolTipText = "Mirror vertically (V)";
            this.toolStripButtonMirrorVertical.Click += new EventHandler(this.toolStripButtonMirrorVertical_Click);
            // 
            // toolStripButtonMirrorHorizontal
            // 
            this.toolStripButtonMirrorHorizontal.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMirrorHorizontal.Image = ((Image)(resources.GetObject("toolStripButtonMirrorHorizontal.Image")));
            this.toolStripButtonMirrorHorizontal.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonMirrorHorizontal.Name = "toolStripButtonMirrorHorizontal";
            this.toolStripButtonMirrorHorizontal.Size = new Size(52, 52);
            this.toolStripButtonMirrorHorizontal.ToolTipText = "Mirror horizontally (H)";
            this.toolStripButtonMirrorHorizontal.Click += new EventHandler(this.toolStripButtonMirrorHorizontal_Click);
            // 
            // toolStripButtonInvert
            // 
            this.toolStripButtonInvert.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInvert.Image = ((Image)(resources.GetObject("toolStripButtonInvert.Image")));
            this.toolStripButtonInvert.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonInvert.Name = "toolStripButtonInvert";
            this.toolStripButtonInvert.Size = new Size(52, 52);
            this.toolStripButtonInvert.ToolTipText = "Invert (T)";
            this.toolStripButtonInvert.Click += new EventHandler(this.toolStripButtonInvert_Click);
            // 
            // toolStripButtonRandom
            // 
            this.toolStripButtonRandom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRandom.Image = ((Image)(resources.GetObject("toolStripButtonRandom.Image")));
            this.toolStripButtonRandom.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonRandom.Name = "toolStripButtonRandom";
            this.toolStripButtonRandom.Size = new Size(52, 52);
            this.toolStripButtonRandom.ToolTipText = "Random (A)";
            this.toolStripButtonRandom.Click += new EventHandler(this.toolStripButtonRandom_Click);
            // 
            // toolStripButtonSparkle
            // 
            this.toolStripButtonSparkle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSparkle.Image = ((Image)(resources.GetObject("toolStripButtonSparkle.Image")));
            this.toolStripButtonSparkle.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonSparkle.Name = "toolStripButtonSparkle";
            this.toolStripButtonSparkle.Size = new Size(52, 52);
            this.toolStripButtonSparkle.ToolTipText = "Sparkle (S)";
            this.toolStripButtonSparkle.Click += new EventHandler(this.toolStripButtonSparkle_Click);
            // 
            // toolStripButtonShimmerDimming
            // 
            this.toolStripButtonShimmerDimming.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShimmerDimming.Image = ((Image)(resources.GetObject("toolStripButtonShimmerDimming.Image")));
            this.toolStripButtonShimmerDimming.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonShimmerDimming.Name = "toolStripButtonShimmerDimming";
            this.toolStripButtonShimmerDimming.Size = new Size(52, 52);
            this.toolStripButtonShimmerDimming.ToolTipText = "Shimmer (E)";
            this.toolStripButtonShimmerDimming.Click += new EventHandler(this.toolStripButtonShimmerDimming_Click);
            // 
            // tsbNutcracker
            // 
            this.tsbNutcracker.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbNutcracker.DoubleClickEnabled = true;
            this.tsbNutcracker.Image = ((Image)(resources.GetObject("tsbNutcracker.Image")));
            this.tsbNutcracker.ImageTransparentColor = Color.Magenta;
            this.tsbNutcracker.Name = "tsbNutcracker";
            this.tsbNutcracker.Size = new Size(52, 52);
            this.tsbNutcracker.ToolTipText = "Nutcracker (N)";
            this.tsbNutcracker.Click += new EventHandler(this.tsbNutcracker_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new Size(6, 55);
            // 
            // toolStripButtonToggleLevels
            // 
            this.toolStripButtonToggleLevels.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonToggleLevels.Image = ((Image)(resources.GetObject("toolStripButtonToggleLevels.Image")));
            this.toolStripButtonToggleLevels.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonToggleLevels.Name = "toolStripButtonToggleLevels";
            this.toolStripButtonToggleLevels.Size = new Size(52, 52);
            this.toolStripButtonToggleLevels.ToolTipText = "Toggle Between Percent and Actual Levels Text";
            this.toolStripButtonToggleLevels.Click += new EventHandler(this.toolStripButtonToggleLevels_Click);
            // 
            // toolStripButtonToggleCellText
            // 
            this.toolStripButtonToggleCellText.BackgroundImageLayout = ImageLayout.Center;
            this.toolStripButtonToggleCellText.CheckOnClick = true;
            this.toolStripButtonToggleCellText.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonToggleCellText.Image = ((Image)(resources.GetObject("toolStripButtonToggleCellText.Image")));
            this.toolStripButtonToggleCellText.ImageTransparentColor = Color.Transparent;
            this.toolStripButtonToggleCellText.Name = "toolStripButtonToggleCellText";
            this.toolStripButtonToggleCellText.Size = new Size(52, 52);
            this.toolStripButtonToggleCellText.ToolTipText = "Show Levels Text";
            this.toolStripButtonToggleCellText.Click += new EventHandler(this.toolStripButtonToggleCellText_Click);
            // 
            // toolStripButtonChangeIntensity
            // 
            this.toolStripButtonChangeIntensity.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonChangeIntensity.Image = ((Image)(resources.GetObject("toolStripButtonChangeIntensity.Image")));
            this.toolStripButtonChangeIntensity.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonChangeIntensity.Name = "toolStripButtonChangeIntensity";
            this.toolStripButtonChangeIntensity.Size = new Size(52, 52);
            this.toolStripButtonChangeIntensity.ToolTipText = "Change Drawing Intesity";
            this.toolStripButtonChangeIntensity.Click += new EventHandler(this.toolStripButtonChangeIntensity_Click);
            // 
            // toolStripEditing
            // 
            this.toolStripEditing.Dock = DockStyle.None;
            this.toolStripEditing.ImageScalingSize = new Size(48, 48);
            this.toolStripEditing.Items.AddRange(new ToolStripItem[] {
            this.toolStripButtonCut,
            this.toolStripButtonCopy,
            this.toolStripButtonOpaquePaste,
            this.toolStripButtonTransparentPaste,
            this.toolStripSplitButtonBooleanPaste,
            this.toolStripSplitButtonArithmeticPaste,
            this.toolStripButtonInsertPaste,
            this.tsPreviewLabel,
            this.cbPastePreview,
            this.toolStripButtonRemoveCells,
            this.toolStripSeparator2,
            this.toolStripButtonFindAndReplace,
            this.toolStripSeparator15,
            this.toolStripButtonUndo,
            this.toolStripButtonRedo});
            this.toolStripEditing.Location = new Point(3, 165);
            this.toolStripEditing.Name = "toolStripEditing";
            this.toolStripEditing.Size = new Size(825, 55);
            this.toolStripEditing.TabIndex = 6;
            this.toolStripEditing.Text = "Editing";
            // 
            // toolStripButtonCut
            // 
            this.toolStripButtonCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCut.Image = ((Image)(resources.GetObject("toolStripButtonCut.Image")));
            this.toolStripButtonCut.ImageTransparentColor = Color.White;
            this.toolStripButtonCut.Name = "toolStripButtonCut";
            this.toolStripButtonCut.Size = new Size(52, 52);
            this.toolStripButtonCut.ToolTipText = "Cut (Ctrl-X)";
            this.toolStripButtonCut.Click += new EventHandler(this.toolStripButtonCut_Click);
            // 
            // toolStripButtonCopy
            // 
            this.toolStripButtonCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCopy.Image = ((Image)(resources.GetObject("toolStripButtonCopy.Image")));
            this.toolStripButtonCopy.ImageTransparentColor = Color.White;
            this.toolStripButtonCopy.Name = "toolStripButtonCopy";
            this.toolStripButtonCopy.Size = new Size(52, 52);
            this.toolStripButtonCopy.ToolTipText = "Copy (Ctrl-C)";
            this.toolStripButtonCopy.Click += new EventHandler(this.toolStripButtonCopy_Click);
            // 
            // toolStripButtonOpaquePaste
            // 
            this.toolStripButtonOpaquePaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpaquePaste.Image = ((Image)(resources.GetObject("toolStripButtonOpaquePaste.Image")));
            this.toolStripButtonOpaquePaste.ImageTransparentColor = Color.White;
            this.toolStripButtonOpaquePaste.Name = "toolStripButtonOpaquePaste";
            this.toolStripButtonOpaquePaste.Size = new Size(52, 52);
            this.toolStripButtonOpaquePaste.ToolTipText = "Paste (Ctrl-V)";
            this.toolStripButtonOpaquePaste.Click += new EventHandler(this.toolStripButtonOpaquePaste_Click);
            // 
            // toolStripButtonTransparentPaste
            // 
            this.toolStripButtonTransparentPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTransparentPaste.Image = ((Image)(resources.GetObject("toolStripButtonTransparentPaste.Image")));
            this.toolStripButtonTransparentPaste.ImageTransparentColor = Color.White;
            this.toolStripButtonTransparentPaste.Name = "toolStripButtonTransparentPaste";
            this.toolStripButtonTransparentPaste.Size = new Size(52, 52);
            this.toolStripButtonTransparentPaste.ToolTipText = "Transparent paste";
            this.toolStripButtonTransparentPaste.Click += new EventHandler(this.toolStripButtonTransparentPaste_Click);
            // 
            // toolStripSplitButtonBooleanPaste
            // 
            this.toolStripSplitButtonBooleanPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonBooleanPaste.DropDownItems.AddRange(new ToolStripItem[] {
            this.toolStripMenuItemPasteOr,
            this.toolStripMenuItemPasteAnd,
            this.toolStripMenuItemPasteXor,
            this.toolStripSeparator14,
            this.toolStripMenuItemPasteNor,
            this.toolStripMenuItemPasteNand,
            this.toolStripMenuItemPasteXnor});
            this.toolStripSplitButtonBooleanPaste.Image = ((Image)(resources.GetObject("toolStripSplitButtonBooleanPaste.Image")));
            this.toolStripSplitButtonBooleanPaste.ImageTransparentColor = Color.White;
            this.toolStripSplitButtonBooleanPaste.Name = "toolStripSplitButtonBooleanPaste";
            this.toolStripSplitButtonBooleanPaste.Size = new Size(64, 52);
            this.toolStripSplitButtonBooleanPaste.ToolTipText = "Boolean paste";
            // 
            // toolStripMenuItemPasteOr
            // 
            this.toolStripMenuItemPasteOr.Name = "toolStripMenuItemPasteOr";
            this.toolStripMenuItemPasteOr.Size = new Size(172, 22);
            this.toolStripMenuItemPasteOr.Text = "OR";
            this.toolStripMenuItemPasteOr.ToolTipText = "OR";
            this.toolStripMenuItemPasteOr.Click += new EventHandler(this.toolStripMenuItemPasteOr_Click);
            // 
            // toolStripMenuItemPasteAnd
            // 
            this.toolStripMenuItemPasteAnd.Name = "toolStripMenuItemPasteAnd";
            this.toolStripMenuItemPasteAnd.Size = new Size(172, 22);
            this.toolStripMenuItemPasteAnd.Text = "AND";
            this.toolStripMenuItemPasteAnd.ToolTipText = "AND";
            this.toolStripMenuItemPasteAnd.Click += new EventHandler(this.toolStripMenuItemPasteAnd_Click);
            // 
            // toolStripMenuItemPasteXor
            // 
            this.toolStripMenuItemPasteXor.Name = "toolStripMenuItemPasteXor";
            this.toolStripMenuItemPasteXor.Size = new Size(172, 22);
            this.toolStripMenuItemPasteXor.Text = "XOR";
            this.toolStripMenuItemPasteXor.ToolTipText = "XOR";
            this.toolStripMenuItemPasteXor.Click += new EventHandler(this.toolStripMenuItemPasteXor_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new Size(169, 6);
            // 
            // toolStripMenuItemPasteNor
            // 
            this.toolStripMenuItemPasteNor.Name = "toolStripMenuItemPasteNor";
            this.toolStripMenuItemPasteNor.Size = new Size(172, 22);
            this.toolStripMenuItemPasteNor.Text = "NOR (NOT OR)";
            this.toolStripMenuItemPasteNor.ToolTipText = "NOR (NOT OR)";
            this.toolStripMenuItemPasteNor.Click += new EventHandler(this.toolStripMenuItemPasteNor_Click);
            // 
            // toolStripMenuItemPasteNand
            // 
            this.toolStripMenuItemPasteNand.Name = "toolStripMenuItemPasteNand";
            this.toolStripMenuItemPasteNand.Size = new Size(172, 22);
            this.toolStripMenuItemPasteNand.Text = "NAND (NOT AND)";
            this.toolStripMenuItemPasteNand.ToolTipText = "NAND (NOT AND)";
            this.toolStripMenuItemPasteNand.Click += new EventHandler(this.toolStripMenuItemPasteNand_Click);
            // 
            // toolStripMenuItemPasteXnor
            // 
            this.toolStripMenuItemPasteXnor.Name = "toolStripMenuItemPasteXnor";
            this.toolStripMenuItemPasteXnor.Size = new Size(172, 22);
            this.toolStripMenuItemPasteXnor.Text = "XNOR (NOT XOR)";
            this.toolStripMenuItemPasteXnor.ToolTipText = "XNOR (NOT XOR)";
            this.toolStripMenuItemPasteXnor.Click += new EventHandler(this.toolStripMenuItemPasteXnor_Click);
            // 
            // toolStripSplitButtonArithmeticPaste
            // 
            this.toolStripSplitButtonArithmeticPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonArithmeticPaste.DropDownItems.AddRange(new ToolStripItem[] {
            this.additionToolStripMenuItem,
            this.subtractionToolStripMenuItem,
            this.scaleToolStripMenuItem,
            this.minToolStripMenuItem,
            this.maxToolStripMenuItem});
            this.toolStripSplitButtonArithmeticPaste.Image = ((Image)(resources.GetObject("toolStripSplitButtonArithmeticPaste.Image")));
            this.toolStripSplitButtonArithmeticPaste.ImageTransparentColor = Color.White;
            this.toolStripSplitButtonArithmeticPaste.Name = "toolStripSplitButtonArithmeticPaste";
            this.toolStripSplitButtonArithmeticPaste.Size = new Size(64, 52);
            this.toolStripSplitButtonArithmeticPaste.ToolTipText = "Arithmetic paste";
            // 
            // additionToolStripMenuItem
            // 
            this.additionToolStripMenuItem.Name = "additionToolStripMenuItem";
            this.additionToolStripMenuItem.Size = new Size(135, 22);
            this.additionToolStripMenuItem.Text = "Addition";
            this.additionToolStripMenuItem.ToolTipText = "Pasted values are added to destination values";
            this.additionToolStripMenuItem.Click += new EventHandler(this.additionToolStripMenuItem_Click);
            // 
            // subtractionToolStripMenuItem
            // 
            this.subtractionToolStripMenuItem.Name = "subtractionToolStripMenuItem";
            this.subtractionToolStripMenuItem.Size = new Size(135, 22);
            this.subtractionToolStripMenuItem.Text = "Subtraction";
            this.subtractionToolStripMenuItem.ToolTipText = "Pasted values are subtracted from destination values";
            this.subtractionToolStripMenuItem.Click += new EventHandler(this.subtractionToolStripMenuItem_Click);
            // 
            // scaleToolStripMenuItem
            // 
            this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
            this.scaleToolStripMenuItem.Size = new Size(135, 22);
            this.scaleToolStripMenuItem.Text = "Scale";
            this.scaleToolStripMenuItem.ToolTipText = "Pasted values are used to scale the destination values";
            this.scaleToolStripMenuItem.Click += new EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // minToolStripMenuItem
            // 
            this.minToolStripMenuItem.Name = "minToolStripMenuItem";
            this.minToolStripMenuItem.Size = new Size(135, 22);
            this.minToolStripMenuItem.Text = "Min";
            this.minToolStripMenuItem.ToolTipText = "Results in the lowest of the pasted and destination values";
            this.minToolStripMenuItem.Click += new EventHandler(this.minToolStripMenuItem_Click);
            // 
            // maxToolStripMenuItem
            // 
            this.maxToolStripMenuItem.Name = "maxToolStripMenuItem";
            this.maxToolStripMenuItem.Size = new Size(135, 22);
            this.maxToolStripMenuItem.Text = "Max";
            this.maxToolStripMenuItem.ToolTipText = "Results in the highest of the pasted and destination values";
            this.maxToolStripMenuItem.Click += new EventHandler(this.maxToolStripMenuItem_Click);
            // 
            // toolStripButtonInsertPaste
            // 
            this.toolStripButtonInsertPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInsertPaste.Image = ((Image)(resources.GetObject("toolStripButtonInsertPaste.Image")));
            this.toolStripButtonInsertPaste.ImageTransparentColor = Color.White;
            this.toolStripButtonInsertPaste.Name = "toolStripButtonInsertPaste";
            this.toolStripButtonInsertPaste.Size = new Size(52, 52);
            this.toolStripButtonInsertPaste.ToolTipText = "Insert paste";
            this.toolStripButtonInsertPaste.Click += new EventHandler(this.toolStripButtonInsertPaste_Click);
            // 
            // tsPreviewLabel
            // 
            this.tsPreviewLabel.Name = "tsPreviewLabel";
            this.tsPreviewLabel.Size = new Size(82, 52);
            this.tsPreviewLabel.Text = "Paste Preview:";
            this.tsPreviewLabel.ToolTipText = "Paste Preview (F3)";
            // 
            // cbPastePreview
            // 
            this.cbPastePreview.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbPastePreview.FlatStyle = FlatStyle.Standard;
            this.cbPastePreview.Items.AddRange(new object[] {
            "Opaque (Normal)",
            "Transparent",
            "Boolean (OR)",
            "Boolean (AND)",
            "Boolean (XOR)",
            "Boolean (NOR)",
            "Boolean (NAND)",
            "Boolean (XNOR)",
            "Addition",
            "Subtraction",
            "Scale",
            "Min",
            "Max",
            "Insert"});
            this.cbPastePreview.Name = "cbPastePreview";
            this.cbPastePreview.Size = new Size(121, 55);
            this.cbPastePreview.ToolTipText = "Selects paste preview (F3)";
            this.cbPastePreview.SelectedIndexChanged += new EventHandler(this.pastePreviewItemChanged_Click);
            // 
            // toolStripButtonRemoveCells
            // 
            this.toolStripButtonRemoveCells.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveCells.Image = ((Image)(resources.GetObject("toolStripButtonRemoveCells.Image")));
            this.toolStripButtonRemoveCells.ImageTransparentColor = Color.White;
            this.toolStripButtonRemoveCells.Name = "toolStripButtonRemoveCells";
            this.toolStripButtonRemoveCells.Size = new Size(52, 52);
            this.toolStripButtonRemoveCells.ToolTipText = "Remove cells";
            this.toolStripButtonRemoveCells.Click += new EventHandler(this.toolStripButtonRemoveCells_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 55);
            // 
            // toolStripButtonFindAndReplace
            // 
            this.toolStripButtonFindAndReplace.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFindAndReplace.Image = ((Image)(resources.GetObject("toolStripButtonFindAndReplace.Image")));
            this.toolStripButtonFindAndReplace.ImageTransparentColor = Color.White;
            this.toolStripButtonFindAndReplace.Name = "toolStripButtonFindAndReplace";
            this.toolStripButtonFindAndReplace.Size = new Size(52, 52);
            this.toolStripButtonFindAndReplace.ToolTipText = "Find and replace (Ctrl-F)";
            this.toolStripButtonFindAndReplace.Click += new EventHandler(this.toolStripButtonFindAndReplace_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new Size(6, 55);
            // 
            // toolStripButtonUndo
            // 
            this.toolStripButtonUndo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUndo.Enabled = false;
            this.toolStripButtonUndo.Image = ((Image)(resources.GetObject("toolStripButtonUndo.Image")));
            this.toolStripButtonUndo.ImageTransparentColor = Color.White;
            this.toolStripButtonUndo.Name = "toolStripButtonUndo";
            this.toolStripButtonUndo.Size = new Size(52, 52);
            this.toolStripButtonUndo.ToolTipText = "Nothing to Undo";
            this.toolStripButtonUndo.Click += new EventHandler(this.toolStripButtonUndo_Click);
            // 
            // toolStripButtonRedo
            // 
            this.toolStripButtonRedo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRedo.Enabled = false;
            this.toolStripButtonRedo.Image = ((Image)(resources.GetObject("toolStripButtonRedo.Image")));
            this.toolStripButtonRedo.ImageTransparentColor = Color.White;
            this.toolStripButtonRedo.Name = "toolStripButtonRedo";
            this.toolStripButtonRedo.Size = new Size(52, 52);
            this.toolStripButtonRedo.ToolTipText = "Nothing to Redo";
            this.toolStripButtonRedo.Click += new EventHandler(this.toolStripButtonRedo_Click);
            // 
            // toolStripDisplaySettings
            // 
            this.toolStripDisplaySettings.Dock = DockStyle.None;
            this.toolStripDisplaySettings.ImageScalingSize = new Size(48, 48);
            this.toolStripDisplaySettings.Items.AddRange(new ToolStripItem[] {
            this.toolStripButtonToggleCrossHairs,
            this.toolStripLabel4,
            this.toolStripComboBoxColumnZoom,
            this.toolStripLabel5,
            this.toolStripComboBoxRowZoom,
            this.toolStripSeparator6});
            this.toolStripDisplaySettings.Location = new Point(3, 220);
            this.toolStripDisplaySettings.Name = "toolStripDisplaySettings";
            this.toolStripDisplaySettings.Size = new Size(326, 51);
            this.toolStripDisplaySettings.TabIndex = 5;
            this.toolStripDisplaySettings.Text = "Display settings";
            // 
            // toolStripButtonToggleCrossHairs
            // 
            this.toolStripButtonToggleCrossHairs.AutoSize = false;
            this.toolStripButtonToggleCrossHairs.BackgroundImageLayout = ImageLayout.Center;
            this.toolStripButtonToggleCrossHairs.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonToggleCrossHairs.Image = ((Image)(resources.GetObject("toolStripButtonToggleCrossHairs.Image")));
            this.toolStripButtonToggleCrossHairs.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonToggleCrossHairs.MergeAction = MergeAction.Insert;
            this.toolStripButtonToggleCrossHairs.MergeIndex = 0;
            this.toolStripButtonToggleCrossHairs.Name = "toolStripButtonToggleCrossHairs";
            this.toolStripButtonToggleCrossHairs.Size = new Size(48, 48);
            this.toolStripButtonToggleCrossHairs.ToolTipText = "Toggle cross-hairs (X)";
            this.toolStripButtonToggleCrossHairs.Click += new EventHandler(this.toolStripButtonToggleCrossHairs_Click);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new Size(83, 48);
            this.toolStripLabel4.Text = "Column zoom";
            // 
            // toolStripComboBoxColumnZoom
            // 
            this.toolStripComboBoxColumnZoom.AutoSize = false;
            this.toolStripComboBoxColumnZoom.DropDownStyle = ComboBoxStyle.DropDownList;
            this.toolStripComboBoxColumnZoom.DropDownWidth = 55;
            this.toolStripComboBoxColumnZoom.FlatStyle = FlatStyle.Standard;
            this.toolStripComboBoxColumnZoom.Items.AddRange(new object[] {
            "20%",
            "25%",
            "30%",
            "35%",
            "40%",
            "45%",
            "50%",
            "55%",
            "60%",
            "65%",
            "70%",
            "75%",
            "80%",
            "85%",
            "90%",
            "95%",
            "100%"});
            this.toolStripComboBoxColumnZoom.Name = "toolStripComboBoxColumnZoom";
            this.toolStripComboBoxColumnZoom.Size = new Size(55, 23);
            this.toolStripComboBoxColumnZoom.ToolTipText = "Change column size";
            this.toolStripComboBoxColumnZoom.SelectedIndexChanged += new EventHandler(this.toolStripComboBoxColumnZoom_SelectedIndexChanged);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new Size(63, 48);
            this.toolStripLabel5.Text = "Row zoom";
            // 
            // toolStripComboBoxRowZoom
            // 
            this.toolStripComboBoxRowZoom.AutoSize = false;
            this.toolStripComboBoxRowZoom.DropDownStyle = ComboBoxStyle.DropDownList;
            this.toolStripComboBoxRowZoom.DropDownWidth = 55;
            this.toolStripComboBoxRowZoom.FlatStyle = FlatStyle.Standard;
            this.toolStripComboBoxRowZoom.Items.AddRange(new object[] {
            "20%",
            "25%",
            "30%",
            "35%",
            "40%",
            "45%",
            "50%",
            "55%",
            "60%",
            "65%",
            "70%",
            "75%",
            "80%",
            "85%",
            "90%",
            "95%",
            "100%"});
            this.toolStripComboBoxRowZoom.Name = "toolStripComboBoxRowZoom";
            this.toolStripComboBoxRowZoom.Size = new Size(55, 23);
            this.toolStripComboBoxRowZoom.ToolTipText = "Change row size";
            this.toolStripComboBoxRowZoom.SelectedIndexChanged += new EventHandler(this.toolStripComboBoxRowZoom_SelectedIndexChanged);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new Size(6, 51);
            // 
            // toolStripText
            // 
            this.toolStripText.Dock = DockStyle.None;
            this.toolStripText.Items.AddRange(new ToolStripItem[] {
            this.toolStripLabel6,
            this.toolStripLabelExecutionPoint,
            this.toolStripSeparator11,
            this.toolStripLabel10,
            this.toolStripLabelCurrentDrawingIntensity,
            this.toolStripSeparator18,
            this.toolStripLabel8,
            this.toolStripLabelCellIntensity,
            this.toolStripSeparator17,
            this.toolStripLabelCurrentCell,
            this.toolStripSeparator9,
            this.toolStripLabel1,
            this.textBoxChannelCount,
            this.toolStripSeparator12,
            this.toolStripLabel2,
            this.textBoxProgramLength});
            this.toolStripText.Location = new Point(3, 271);
            this.toolStripText.Name = "toolStripText";
            this.toolStripText.Size = new Size(942, 25);
            this.toolStripText.TabIndex = 7;
            this.toolStripText.Text = "Text";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new Size(76, 22);
            this.toolStripLabel6.Text = "Executing At:";
            // 
            // toolStripLabelExecutionPoint
            // 
            this.toolStripLabelExecutionPoint.AutoSize = false;
            this.toolStripLabelExecutionPoint.Name = "toolStripLabelExecutionPoint";
            this.toolStripLabelExecutionPoint.Size = new Size(85, 22);
            this.toolStripLabelExecutionPoint.Text = "00:000.000";
            this.toolStripLabelExecutionPoint.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new Size(6, 25);
            // 
            // toolStripLabel10
            // 
            this.toolStripLabel10.Name = "toolStripLabel10";
            this.toolStripLabel10.Size = new Size(64, 22);
            this.toolStripLabel10.Text = "Drawing at";
            // 
            // toolStripLabelCurrentDrawingIntensity
            // 
            this.toolStripLabelCurrentDrawingIntensity.AutoSize = false;
            this.toolStripLabelCurrentDrawingIntensity.Name = "toolStripLabelCurrentDrawingIntensity";
            this.toolStripLabelCurrentDrawingIntensity.Size = new Size(46, 22);
            this.toolStripLabelCurrentDrawingIntensity.Text = "100%";
            this.toolStripLabelCurrentDrawingIntensity.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new Size(6, 25);
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new Size(78, 22);
            this.toolStripLabel8.Text = "Cell intensity:";
            // 
            // toolStripLabelCellIntensity
            // 
            this.toolStripLabelCellIntensity.AutoSize = false;
            this.toolStripLabelCellIntensity.Name = "toolStripLabelCellIntensity";
            this.toolStripLabelCellIntensity.Size = new Size(45, 22);
            this.toolStripLabelCellIntensity.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new Size(6, 25);
            // 
            // toolStripLabelCurrentCell
            // 
            this.toolStripLabelCurrentCell.AutoSize = false;
            this.toolStripLabelCurrentCell.Name = "toolStripLabelCurrentCell";
            this.toolStripLabelCurrentCell.Size = new Size(200, 15);
            this.toolStripLabelCurrentCell.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(56, 22);
            this.toolStripLabel1.Text = "Channels";
            // 
            // textBoxChannelCount
            // 
            this.textBoxChannelCount.MaxLength = 5;
            this.textBoxChannelCount.Name = "textBoxChannelCount";
            this.textBoxChannelCount.Size = new Size(40, 25);
            this.textBoxChannelCount.Text = "0";
            this.textBoxChannelCount.KeyPress += new KeyPressEventHandler(this.textBoxChannelCount_KeyPress);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new Size(131, 22);
            this.toolStripLabel2.Text = "Sequence time (mm:ss)";
            // 
            // textBoxProgramLength
            // 
            this.textBoxProgramLength.Name = "textBoxProgramLength";
            this.textBoxProgramLength.Size = new Size(75, 25);
            this.textBoxProgramLength.Text = "00:00";
            this.textBoxProgramLength.KeyPress += new KeyPressEventHandler(this.textBoxProgramLength_KeyPress);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "vix";
            this.saveFileDialog.Filter = "Vixen Event Sequence | *.vix";
            this.saveFileDialog.Title = "Save As";
            // 
            // positionTimer
            // 
            this.positionTimer.Interval = 1;
            this.positionTimer.Tick += new EventHandler(this.positionTimer_Tick);
            // 
            // StandardSequence
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.ClientSize = new Size(976, 539);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.menuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "StandardSequence";
            this.Activated += new EventHandler(this.StandardSequence_Activated);
            this.Deactivate += new EventHandler(this.StandardSequence_Deactivate);
            this.FormClosing += new FormClosingEventHandler(this.StandardSequence_FormClosing);
            this.Load += new EventHandler(this.StandardSequence_Load);
            this.KeyDown += new KeyEventHandler(this.StandardSequence_KeyDown);
            this.KeyUp += new KeyEventHandler(this.StandardSequence_KeyUp);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((ISupportInitialize)(this.pictureBoxChannels)).EndInit();
            this.contextMenuChannels.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            ((ISupportInitialize)(this.pictureBoxTime)).EndInit();
            this.contextMenuTime.ResumeLayout(false);
            ((ISupportInitialize)(this.pictureBoxGrid)).EndInit();
            this.contextMenuGrid.ResumeLayout(false);
            this.toolStripSequenceSettings.ResumeLayout(false);
            this.toolStripSequenceSettings.PerformLayout();
            this.toolStripExecutionControl.ResumeLayout(false);
            this.toolStripExecutionControl.PerformLayout();
            this.toolStripEffect.ResumeLayout(false);
            this.toolStripEffect.PerformLayout();
            this.toolStripEditing.ResumeLayout(false);
            this.toolStripEditing.PerformLayout();
            this.toolStripDisplaySettings.ResumeLayout(false);
            this.toolStripDisplaySettings.PerformLayout();
            this.toolStripText.ResumeLayout(false);
            this.toolStripText.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private ToolStripButton tsbNutcracker;
        private ToolStripLabel tsPreviewLabel;
        private ToolStripComboBox cbPastePreview;
        private ToolStripMenuItem pastePreviewToolStripMenuItem;
        private ToolStripMenuItem tsmiPpOpaque;
        private ToolStripMenuItem tsmiPpOR;
        private ToolStripMenuItem tsmiPpAND;
        private ToolStripMenuItem tsmiPpXOR;
        private ToolStripMenuItem tsmiPpNOR;
        private ToolStripMenuItem tsmiPpNAND;
        private ToolStripMenuItem tsmiPpXNOR;
        private ToolStripMenuItem tsmiPpAdd;
        private ToolStripMenuItem tsmiPpSubtract;
        private ToolStripMenuItem tsmiPpScale;
        private ToolStripMenuItem tsmiPpMin;
        private ToolStripMenuItem tsmiPpMax;
        private ToolStripMenuItem tsmiPpInsert;
        private ToolStripMenuItem tsmiPpTransparent;
        private ToolStripMenuItem selectAllEventsMenuItem;
        private ToolStripMenuItem selectAllChannelsForPeriod;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem selectAlltoolStripMenuItem;

    }
}
