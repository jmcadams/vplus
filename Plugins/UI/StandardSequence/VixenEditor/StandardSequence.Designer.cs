using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace VixenEditor{

	public partial class StandardSequence{
		private IContainer components;

		#region Windows Form Designer generated code
		private ToolStripMenuItem additionToolStripMenuItem;
		private ToolStripMenuItem additionToolStripMenuItem1;
		private ToolStripMenuItem allChannelsToFullIntensityForThisEventToolStripMenuItem;
		private ToolStripMenuItem allEventsToFullIntensityToolStripMenuItem;
		private ToolStripMenuItem aNDToolStripMenuItem;
		private ToolStripMenuItem aNDToolStripMenuItem1;
		private ToolStripMenuItem arithmeticPasteToolStripMenuItem;
		private ToolStripMenuItem attachSequenceToToolStripMenuItem;
		private ToolStripMenuItem audioSpeedToolStripMenuItem;
		private ToolStripMenuItem audioToolStripMenuItem1;
		private ToolStripMenuItem booleanPasteToolStripMenuItem;
		private ToolStripMenuItem booleanPasteToolStripMenuItem1;
		private ToolStripMenuItem channelOutputMaskToolStripMenuItem;
		private ToolStripMenuItem channelPropertiesToolStripMenuItem;
		private ToolStripMenuItem chaseLinesToolStripMenuItem;
		private ToolStripMenuItem clearAllChannelsForThisEventToolStripMenuItem;
		private ToolStripMenuItem clearAllToolStripMenuItem;
		private ToolStripMenuItem clearChannelEventsToolStripMenuItem;
		private ColorDialog colorDialog1;
		private ContextMenuStrip contextMenuChannels;
		private ContextMenuStrip contextMenuGrid;
		private ContextMenuStrip contextMenuTime;
		private ToolStripMenuItem copyChannelEventsToClipboardToolStripMenuItem;
		private ToolStripMenuItem copyChannelToolStripMenuItem;
		private ToolStripMenuItem copyChannelToolStripMenuItem1;
		private ToolStripMenuItem copyToolStripMenuItem;
		private ToolStripMenuItem copyToolStripMenuItem1;
		private ToolStripMenuItem createFromSequenceToolStripMenuItem;
		private ToolStripMenuItem currentProgramsSettingsToolStripMenuItem;
		private ToolStripMenuItem cutToolStripMenuItem;
		private ToolStripMenuItem cutToolStripMenuItem1;
		private ToolStripMenuItem detachSequenceFromItsProfileToolStripMenuItem;
		private ToolStripMenuItem editToolStripMenuItem;
		private ToolStripMenuItem effectsToolStripMenuItem;
		private ToolStripMenuItem exportChannelNamesListToolStripMenuItem;
		private ToolStripMenuItem findAndReplaceToolStripMenuItem;
		private ToolStripMenuItem findAndReplaceToolStripMenuItem1;
		private ToolStripMenuItem flattenProfileIntoSequenceToolStripMenuItem;
		private HScrollBar hScrollBar1;
		private ToolStripMenuItem importChannelNamesListToolStripMenuItem;
		private ToolStripMenuItem insertPasteToolStripMenuItem;
		private ToolStripMenuItem insertPasteToolStripMenuItem1;
		private ToolStripMenuItem invertToolStripMenuItem;
		private ToolStripMenuItem invertToolStripMenuItem1;
		private Label labelPosition;
		private ToolStripMenuItem loadARoutineToolStripMenuItem;
		private ToolStripMenuItem loadRoutineToClipboardToolStripMenuItem;
		private IntensityAdjustDialog m_intensityAdjustDialog;
		private Control m_lastSelectableControl;
		private System.Windows.Forms.Timer m_positionTimer;
		private ToolStripMenuItem maxToolStripMenuItem;
		private ToolStripMenuItem maxToolStripMenuItem1;
		private MenuStrip menuStrip;
		private ToolStripMenuItem minToolStripMenuItem;
		private ToolStripMenuItem minToolStripMenuItem1;
		private ToolStripMenuItem mirrorHorizontallyToolStripMenuItem;
		private ToolStripMenuItem mirrorHorizontallyToolStripMenuItem1;
		private ToolStripMenuItem mirrorVerticallyToolStripMenuItem;
		private ToolStripMenuItem mirrorVerticallyToolStripMenuItem1;
		private ToolStripMenuItem nANDToolStripMenuItem;
		private ToolStripMenuItem nANDToolStripMenuItem1;
		private ToolStripMenuItem normalToolStripMenuItem;
		private ToolStripMenuItem normalToolStripMenuItem1;
		private ToolStripMenuItem nORToolStripMenuItem;
		private ToolStripMenuItem nORToolStripMenuItem1;
		private ToolStripMenuItem offToolStripMenuItem;
		private ToolStripMenuItem offToolStripMenuItem1;
		private ToolStripMenuItem onToolStripMenuItem;
		private ToolStripMenuItem onToolStripMenuItem1;
		private ToolStripMenuItem opaquePasteToolStripMenuItem;
		private ToolStripMenuItem opaquePasteToolStripMenuItem1;
		private OpenFileDialog openFileDialog1;
		private ToolStripMenuItem oRToolStripMenuItem;
		private ToolStripMenuItem oRToolStripMenuItem1;
		private ToolStripMenuItem otherToolStripMenuItem;
		private ToolStripMenuItem paintFromClipboardToolStripMenuItem;
		private ToolStripMenuItem partialRampOffToolStripMenuItem;
		private ToolStripMenuItem partialRampOffToolStripMenuItem1;
		private ToolStripMenuItem partialRampOnToolStripMenuItem;
		private ToolStripMenuItem partialRampOnToolStripMenuItem1;
		private ToolStripMenuItem pasteFullChannelEventsFromClipboardToolStripMenuItem;
		private ToolStripMenuItem pasteToolStripMenuItem;
		private ToolStripMenuItem pasteToolStripMenuItem1;
		private SelectablePictureBox pictureBoxChannels;
		private SelectablePictureBox pictureBoxGrid;
		private PictureBox pictureBoxLevelNumber;
		private PictureBox pictureBoxLevelPercent;
		private PictureBox pictureBoxOutputArrow;
		private PictureBox pictureBoxTime;
		private ToolStripMenuItem playAtTheSelectedPointToolStripMenuItem;
		private ToolStripMenuItem playOnlyTheSelectedRangeToolStripMenuItem;
		private ToolStripMenuItem printChannelConfigurationToolStripMenuItem;
		private PrintDialog printDialog;
		private PrintPreviewDialog printPreviewDialog;
		private ToolStripMenuItem profilesToolStripMenuItem;
		private ToolStripMenuItem programToolStripMenuItem;
		private ToolStripMenuItem rampOffToolStripMenuItem;
		private ToolStripMenuItem rampOffToolStripMenuItem1;
		private ToolStripMenuItem rampOnToolStripMenuItem;
		private ToolStripMenuItem rampOnToolStripMenuItem1;
		private ToolStripMenuItem randomToolStripMenuItem;
		private ToolStripMenuItem randomToolStripMenuItem1;
		private ToolStripMenuItem redoToolStripMenuItem;
		private ToolStripMenuItem removeCellsToolStripMenuItem;
		private ToolStripMenuItem removeCellsToolStripMenuItem1;
		private ToolStripMenuItem reorderChannelOutputsToolStripMenuItem;
		private ToolStripMenuItem resetAllToolbarsToolStripMenuItem;
		private ToolStripMenuItem saveAsARoutineToolStripMenuItem;
		private SaveFileDialog saveFileDialog;
		private ToolStripMenuItem saveToolbarPositionsToolStripMenuItem;
		private ToolStripMenuItem scaleToolStripMenuItem;
		private ToolStripMenuItem scaleToolStripMenuItem1;
		private ToolStripMenuItem setAllChannelColorsToolStripMenuItem;
		private ToolStripMenuItem setIntensityToolStripMenuItem;
		private ToolStripMenuItem setIntensityToolStripMenuItem1;
		private ToolStripMenuItem shimmerToolStripMenuItem;
		private ToolStripMenuItem shimmerToolStripMenuItem1;
		private ToolStripMenuItem sortByChannelNumberToolStripMenuItem;
		private ToolStripMenuItem sortByChannelOutputToolStripMenuItem;
		private ToolStripMenuItem sparkleToolStripMenuItem;
		private ToolStripMenuItem sparkleToolStripMenuItem1;
		private SplitContainer splitContainer1;
		private ToolStripMenuItem subtractionToolStripMenuItem;
		private ToolStripMenuItem subtractionToolStripMenuItem1;
		private ToolStripTextBox textBoxChannelCount;
		private ToolStripTextBox textBoxProgramLength;
		private ToolStripMenuItem toggleOutputChannelsToolStripMenuItem;
		private ToolStripMenuItem toolbarsToolStripMenuItem;
		private ToolStripButton toolStripButtonAudio;
		private ToolStripButton toolStripButtonChangeIntensity;
		private ToolStripButton toolStripButtonChannelOutputMask;
		private ToolStripButton toolStripButtonCopy;
		private ToolStripButton toolStripButtonCut;
		private ToolStripButton toolStripButtonDeleteOrder;
		private ToolStripButton toolStripButtonFindAndReplace;
		private ToolStripButton toolStripButtonInsertPaste;
		private ToolStripButton toolStripButtonIntensity;
		private ToolStripButton toolStripButtonInvert;
		private ToolStripButton toolStripButtonLoop;
		private ToolStripButton toolStripButtonMirrorHorizontal;
		private ToolStripButton toolStripButtonMirrorVertical;
		private ToolStripButton toolStripButtonOff;
		private ToolStripButton toolStripButtonOn;
		private ToolStripButton toolStripButtonOpaquePaste;
		private ToolStripButton toolStripButtonPartialRampOff;
		private ToolStripButton toolStripButtonPartialRampOn;
		private ToolStripButton toolStripButtonPause;
		private ToolStripButton toolStripButtonPlay;
		private ToolStripSplitButton toolStripButtonPlayPoint;
		private ToolStripButton toolStripButtonPlaySpeedHalf;
		private ToolStripButton toolStripButtonPlaySpeedNormal;
		private ToolStripButton toolStripButtonPlaySpeedQuarter;
		private ToolStripButton toolStripButtonPlaySpeedThreeQuarters;
		private ToolStripButton toolStripButtonPlaySpeedVariable;
		private ToolStripButton toolStripButtonRampOff;
		private ToolStripButton toolStripButtonRampOn;
		private ToolStripButton toolStripButtonRandom;
		private ToolStripButton toolStripButtonRedo;
		private ToolStripButton toolStripButtonRemoveCells;
		private ToolStripButton toolStripButtonSave;
		private ToolStripButton toolStripButtonSaveOrder;
		private ToolStripButton toolStripButtonShimmerDimming;
		private ToolStripButton toolStripButtonSparkle;
		private ToolStripButton toolStripButtonStop;
		private ToolStripButton toolStripButtonTestChannels;
		private ToolStripButton toolStripButtonTestConsole;
		private ToolStripButton toolStripButtonToggleCellText;
		private ToolStripButton toolStripButtonToggleCrossHairs;
		private ToolStripButton toolStripButtonToggleLevels;
		private ToolStripButton toolStripButtonToggleRamps;
		private ToolStripButton toolStripButtonTransparentPaste;
		private ToolStripButton toolStripButtonUndo;
		private ToolStripButton toolStripButtonWaveform;
		private ToolStripComboBox toolStripComboBoxChannelOrder;
		private ToolStripComboBox toolStripComboBoxColumnZoom;
		private ToolStripComboBox toolStripComboBoxRowZoom;
		private ToolStripComboBox toolStripComboBoxWaveformZoom;
		private ToolStripContainer toolStripContainer1;
		private ToolStrip toolStripDisplaySettings;
		private ToolStripDropDownButton toolStripDropDownButtonPlugins;
		private ToolStrip toolStripEditing;
		private ToolStrip toolStripEffect;
		private ToolStrip toolStripExecutionControl;
		private ToolStripLabel toolStripLabel1;
		private ToolStripLabel toolStripLabel10;
		private ToolStripLabel toolStripLabel2;
		private ToolStripLabel toolStripLabel3;
		private ToolStripLabel toolStripLabel4;
		private ToolStripLabel toolStripLabel5;
		private ToolStripLabel toolStripLabel6;
		private ToolStripLabel toolStripLabel8;
		private ToolStripLabel toolStripLabelCellIntensity;
		private ToolStripLabel toolStripLabelCurrentCell;
		private ToolStripLabel toolStripLabelCurrentDrawingIntensity;
		private ToolStripLabel toolStripLabelCurrentIntensity;
		private ToolStripLabel toolStripLabelExecutionPoint;
		private ToolStripLabel toolStripLabelIntensity;
		private ToolStripLabel toolStripLabelProgess;
		private ToolStripLabel toolStripLabelWaveformZoom;
		private ToolStripSeparator toolStripMenuItem10;
		private ToolStripSeparator toolStripMenuItem11;
		private ToolStripSeparator toolStripMenuItem12;
		private ToolStripSeparator toolStripMenuItem13;
		private ToolStripSeparator toolStripMenuItem14;
		private ToolStripSeparator toolStripMenuItem15;
		private ToolStripSeparator toolStripMenuItem16;
		private ToolStripSeparator toolStripMenuItem17;
		private ToolStripSeparator toolStripMenuItem18;
		private ToolStripSeparator toolStripMenuItem19;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripSeparator toolStripMenuItem20;
		private ToolStripSeparator toolStripMenuItem21;
		private ToolStripSeparator toolStripMenuItem22;
		private ToolStripSeparator toolStripMenuItem23;
		private ToolStripSeparator toolStripMenuItem24;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripSeparator toolStripMenuItem4;
		private ToolStripSeparator toolStripMenuItem5;
		private ToolStripSeparator toolStripMenuItem6;
		private ToolStripSeparator toolStripMenuItem7;
		private ToolStripSeparator toolStripMenuItem8;
		private ToolStripSeparator toolStripMenuItem9;
		private ToolStripMenuItem toolStripMenuItemPasteAnd;
		private ToolStripMenuItem toolStripMenuItemPasteNand;
		private ToolStripMenuItem toolStripMenuItemPasteNor;
		private ToolStripMenuItem toolStripMenuItemPasteOr;
		private ToolStripMenuItem toolStripMenuItemPasteXnor;
		private ToolStripMenuItem toolStripMenuItemPasteXor;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator toolStripSeparator10;
		private ToolStripSeparator toolStripSeparator11;
		private ToolStripSeparator toolStripSeparator13;
		private ToolStripSeparator toolStripSeparator14;
		private ToolStripSeparator toolStripSeparator15;
		private ToolStripSeparator toolStripSeparator16;
		private ToolStripSeparator toolStripSeparator17;
		private ToolStripSeparator toolStripSeparator18;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripSeparator toolStripSeparator5;
		private ToolStripSeparator toolStripSeparator6;
		private ToolStripSeparator toolStripSeparator7;
		private ToolStripSeparator toolStripSeparator8;
		private ToolStripSeparator toolStripSeparator9;
		private ToolStrip toolStripSequenceSettings;
		private ToolStripSplitButton toolStripSplitButtonArithmeticPaste;
		private ToolStripSplitButton toolStripSplitButtonBooleanPaste;
		private ToolStrip toolStripText;
		private ToolStrip toolStripVisualizer;
		private ToolStripMenuItem transparentPasteToolStripMenuItem;
		private ToolStripMenuItem transparentPasteToolStripMenuItem1;
		private ToolStripMenuItem undoToolStripMenuItem;
		private VScrollBar vScrollBar1;
		private ToolStripMenuItem xNORToolStripMenuItem;
		private ToolStripMenuItem xNORToolStripMenuItem1;
		private ToolStripMenuItem xORToolStripMenuItem;
		private ToolStripMenuItem xORToolStripMenuItem1;
		private ToolStripMenuItem xToolStripMenuItem;
		private ToolStripMenuItem xToolStripMenuItem1;
		private ToolStripMenuItem xToolStripMenuItem2;

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandardSequence));
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportChannelNamesListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importChannelNamesListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printChannelConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sortByChannelNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sortByChannelOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.audioToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.channelOutputMaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.currentProgramsSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripSeparator();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.opaquePasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.transparentPasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.booleanPasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.oRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aNDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xORToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
			this.nORToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nANDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xNORToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.insertPasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeCellsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
			this.findAndReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.copyChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripSeparator();
			this.audioSpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.xToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.normalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
			this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.effectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.onToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.offToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.rampOnToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.rampOffToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.partialRampOnToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.partialRampOffToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.setIntensityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mirrorVerticallyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mirrorHorizontallyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.invertToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.randomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.shimmerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.sparkleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripSeparator();
			this.chaseLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.paintFromClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.profilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.createFromSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.attachSequenceToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.detachSequenceFromItsProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.flattenProfileIntoSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolbarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
			this.resetAllToolbarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolbarPositionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.pictureBoxLevelNumber = new System.Windows.Forms.PictureBox();
			this.pictureBoxLevelPercent = new System.Windows.Forms.PictureBox();
			this.pictureBoxOutputArrow = new System.Windows.Forms.PictureBox();
			this.labelPosition = new System.Windows.Forms.Label();
			this.pictureBoxChannels = new VixenEditor.SelectablePictureBox();
			this.contextMenuChannels = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toggleOutputChannelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reorderChannelOutputsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.clearChannelEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allEventsToFullIntensityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyChannelToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.setAllChannelColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
			this.copyChannelEventsToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteFullChannelEventsFromClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
			this.channelPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pictureBoxGrid = new VixenEditor.SelectablePictureBox();
			this.contextMenuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.onToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.rampOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rampOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.partialRampOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.partialRampOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.opaquePasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.transparentPasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.booleanPasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.oRToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.aNDToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.xORToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
			this.nORToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.nANDToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.xNORToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.insertPasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.arithmeticPasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.additionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.subtractionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.scaleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.minToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.maxToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.removeCellsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
			this.findAndReplaceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
			this.setIntensityToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.mirrorVerticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mirrorHorizontallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.invertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.randomToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.shimmerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sparkleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
			this.saveAsARoutineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadARoutineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadRoutineToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.pictureBoxTime = new System.Windows.Forms.PictureBox();
			this.contextMenuTime = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.clearAllChannelsForThisEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allChannelsToFullIntensityForThisEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSequenceSettings = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.textBoxChannelCount = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripButtonTestChannels = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonTestConsole = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.textBoxProgramLength = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonAudio = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonChannelOutputMask = new System.Windows.Forms.ToolStripButton();
			this.toolStripExecutionControl = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButtonPlugins = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonPlay = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPlayPoint = new System.Windows.Forms.ToolStripSplitButton();
			this.playAtTheSelectedPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.playOnlyTheSelectedRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripButtonPause = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
			this.toolStripLabelProgess = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonLoop = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonPlaySpeedQuarter = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPlaySpeedHalf = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPlaySpeedThreeQuarters = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPlaySpeedNormal = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPlaySpeedVariable = new System.Windows.Forms.ToolStripButton();
			this.toolStripLabelIntensity = new System.Windows.Forms.ToolStripLabel();
			this.toolStripEffect = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonOn = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonOff = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonRampOn = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonRampOff = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPartialRampOn = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPartialRampOff = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonToggleRamps = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonIntensity = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonMirrorVertical = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonMirrorHorizontal = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonInvert = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonRandom = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSparkle = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonShimmerDimming = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonToggleLevels = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonToggleCellText = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonChangeIntensity = new System.Windows.Forms.ToolStripButton();
			this.toolStripLabelCurrentIntensity = new System.Windows.Forms.ToolStripLabel();
			this.toolStripEditing = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonCut = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonCopy = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonOpaquePaste = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonTransparentPaste = new System.Windows.Forms.ToolStripButton();
			this.toolStripSplitButtonBooleanPaste = new System.Windows.Forms.ToolStripSplitButton();
			this.toolStripMenuItemPasteOr = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemPasteAnd = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemPasteXor = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItemPasteNor = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemPasteNand = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemPasteXnor = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSplitButtonArithmeticPaste = new System.Windows.Forms.ToolStripSplitButton();
			this.additionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.subtractionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.scaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.minToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.maxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripButtonInsertPaste = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonRemoveCells = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonFindAndReplace = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonRedo = new System.Windows.Forms.ToolStripButton();
			this.toolStripText = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripLabelExecutionPoint = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel10 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripLabelCurrentDrawingIntensity = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripLabelCellIntensity = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabelCurrentCell = new System.Windows.Forms.ToolStripLabel();
			this.toolStripDisplaySettings = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonToggleCrossHairs = new System.Windows.Forms.ToolStripButton();
			this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripComboBoxColumnZoom = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripComboBoxRowZoom = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripComboBoxChannelOrder = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripButtonSaveOrder = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonDeleteOrder = new System.Windows.Forms.ToolStripButton();
			this.toolStripVisualizer = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonWaveform = new System.Windows.Forms.ToolStripButton();
			this.toolStripLabelWaveformZoom = new System.Windows.Forms.ToolStripLabel();
			this.toolStripComboBoxWaveformZoom = new System.Windows.Forms.ToolStripComboBox();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.printDocument = new System.Drawing.Printing.PrintDocument();
			this.printDialog = new System.Windows.Forms.PrintDialog();
			this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
			this.m_positionTimer = new System.Windows.Forms.Timer(this.components);
			this.menuStrip.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLevelNumber)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLevelPercent)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutputArrow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxChannels)).BeginInit();
			this.contextMenuChannels.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxGrid)).BeginInit();
			this.contextMenuGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxTime)).BeginInit();
			this.contextMenuTime.SuspendLayout();
			this.toolStripSequenceSettings.SuspendLayout();
			this.toolStripExecutionControl.SuspendLayout();
			this.toolStripEffect.SuspendLayout();
			this.toolStripEditing.SuspendLayout();
			this.toolStripText.SuspendLayout();
			this.toolStripDisplaySettings.SuspendLayout();
			this.toolStripVisualizer.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem,
            this.editToolStripMenuItem,
            this.effectsToolStripMenuItem,
            this.profilesToolStripMenuItem,
            this.toolbarsToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(792, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			this.menuStrip.Visible = false;
			// 
			// programToolStripMenuItem
			// 
			this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportChannelNamesListToolStripMenuItem,
            this.importChannelNamesListToolStripMenuItem,
            this.printChannelConfigurationToolStripMenuItem,
            this.toolStripMenuItem13,
            this.audioToolStripMenuItem1,
            this.channelOutputMaskToolStripMenuItem,
            this.currentProgramsSettingsToolStripMenuItem,
            this.toolStripMenuItem19});
			this.programToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
			this.programToolStripMenuItem.Name = "programToolStripMenuItem";
			this.programToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
			this.programToolStripMenuItem.Text = "Sequence";
			// 
			// exportChannelNamesListToolStripMenuItem
			// 
			this.exportChannelNamesListToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.exportChannelNamesListToolStripMenuItem.MergeIndex = 6;
			this.exportChannelNamesListToolStripMenuItem.Name = "exportChannelNamesListToolStripMenuItem";
			this.exportChannelNamesListToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.exportChannelNamesListToolStripMenuItem.Text = "Export channel names list";
			this.exportChannelNamesListToolStripMenuItem.Click += new System.EventHandler(this.exportChannelNamesListToolStripMenuItem_Click);
			// 
			// importChannelNamesListToolStripMenuItem
			// 
			this.importChannelNamesListToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.importChannelNamesListToolStripMenuItem.MergeIndex = 7;
			this.importChannelNamesListToolStripMenuItem.Name = "importChannelNamesListToolStripMenuItem";
			this.importChannelNamesListToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.importChannelNamesListToolStripMenuItem.Text = "Import channel names list";
			this.importChannelNamesListToolStripMenuItem.Click += new System.EventHandler(this.importChannelNamesListToolStripMenuItem_Click);
			// 
			// printChannelConfigurationToolStripMenuItem
			// 
			this.printChannelConfigurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortByChannelNumberToolStripMenuItem,
            this.sortByChannelOutputToolStripMenuItem});
			this.printChannelConfigurationToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.printChannelConfigurationToolStripMenuItem.MergeIndex = 8;
			this.printChannelConfigurationToolStripMenuItem.Name = "printChannelConfigurationToolStripMenuItem";
			this.printChannelConfigurationToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.printChannelConfigurationToolStripMenuItem.Text = "Print channel configuration";
			// 
			// sortByChannelNumberToolStripMenuItem
			// 
			this.sortByChannelNumberToolStripMenuItem.Name = "sortByChannelNumberToolStripMenuItem";
			this.sortByChannelNumberToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.sortByChannelNumberToolStripMenuItem.Text = "Sort by natural channel order";
			this.sortByChannelNumberToolStripMenuItem.Click += new System.EventHandler(this.sortByChannelNumberToolStripMenuItem_Click);
			// 
			// sortByChannelOutputToolStripMenuItem
			// 
			this.sortByChannelOutputToolStripMenuItem.Name = "sortByChannelOutputToolStripMenuItem";
			this.sortByChannelOutputToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.sortByChannelOutputToolStripMenuItem.Text = "Sort by channel output";
			this.sortByChannelOutputToolStripMenuItem.Click += new System.EventHandler(this.sortByChannelOutputToolStripMenuItem_Click);
			// 
			// toolStripMenuItem13
			// 
			this.toolStripMenuItem13.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.toolStripMenuItem13.MergeIndex = 9;
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			this.toolStripMenuItem13.Size = new System.Drawing.Size(216, 6);
			// 
			// audioToolStripMenuItem1
			// 
			this.audioToolStripMenuItem1.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.audioToolStripMenuItem1.MergeIndex = 10;
			this.audioToolStripMenuItem1.Name = "audioToolStripMenuItem1";
			this.audioToolStripMenuItem1.Size = new System.Drawing.Size(219, 22);
			this.audioToolStripMenuItem1.Text = "Audio";
			this.audioToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonAudio_Click);
			// 
			// channelOutputMaskToolStripMenuItem
			// 
			this.channelOutputMaskToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.channelOutputMaskToolStripMenuItem.MergeIndex = 11;
			this.channelOutputMaskToolStripMenuItem.Name = "channelOutputMaskToolStripMenuItem";
			this.channelOutputMaskToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.channelOutputMaskToolStripMenuItem.Text = "Channel output mask";
			this.channelOutputMaskToolStripMenuItem.Click += new System.EventHandler(this.channelOutputMaskToolStripMenuItem_Click);
			// 
			// currentProgramsSettingsToolStripMenuItem
			// 
			this.currentProgramsSettingsToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.currentProgramsSettingsToolStripMenuItem.MergeIndex = 12;
			this.currentProgramsSettingsToolStripMenuItem.Name = "currentProgramsSettingsToolStripMenuItem";
			this.currentProgramsSettingsToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
			this.currentProgramsSettingsToolStripMenuItem.Text = "Settings";
			this.currentProgramsSettingsToolStripMenuItem.Click += new System.EventHandler(this.currentProgramsSettingsToolStripMenuItem_Click);
			// 
			// toolStripMenuItem19
			// 
			this.toolStripMenuItem19.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.toolStripMenuItem19.MergeIndex = 13;
			this.toolStripMenuItem19.Name = "toolStripMenuItem19";
			this.toolStripMenuItem19.Size = new System.Drawing.Size(216, 6);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripMenuItem7,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.removeCellsToolStripMenuItem1,
            this.clearAllToolStripMenuItem,
            this.toolStripMenuItem18,
            this.findAndReplaceToolStripMenuItem,
            this.toolStripMenuItem4,
            this.copyChannelToolStripMenuItem,
            this.toolStripMenuItem22,
            this.audioSpeedToolStripMenuItem});
			this.editToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.editToolStripMenuItem.MergeIndex = 2;
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Enabled = false;
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.undoToolStripMenuItem.Text = "Undo";
			this.undoToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonUndo_Click);
			// 
			// redoToolStripMenuItem
			// 
			this.redoToolStripMenuItem.Enabled = false;
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.redoToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.redoToolStripMenuItem.Text = "Redo";
			this.redoToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonRedo_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(198, 6);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.cutToolStripMenuItem.Text = "Cut";
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonCut_Click);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opaquePasteToolStripMenuItem,
            this.transparentPasteToolStripMenuItem,
            this.booleanPasteToolStripMenuItem,
            this.insertPasteToolStripMenuItem});
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			// 
			// opaquePasteToolStripMenuItem
			// 
			this.opaquePasteToolStripMenuItem.Name = "opaquePasteToolStripMenuItem";
			this.opaquePasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.opaquePasteToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.opaquePasteToolStripMenuItem.Text = "Opaque paste";
			this.opaquePasteToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonOpaquePaste_Click);
			// 
			// transparentPasteToolStripMenuItem
			// 
			this.transparentPasteToolStripMenuItem.Name = "transparentPasteToolStripMenuItem";
			this.transparentPasteToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.transparentPasteToolStripMenuItem.Text = "Transparent paste";
			this.transparentPasteToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonTransparentPaste_Click);
			// 
			// booleanPasteToolStripMenuItem
			// 
			this.booleanPasteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oRToolStripMenuItem,
            this.aNDToolStripMenuItem,
            this.xORToolStripMenuItem,
            this.toolStripMenuItem16,
            this.nORToolStripMenuItem,
            this.nANDToolStripMenuItem,
            this.xNORToolStripMenuItem});
			this.booleanPasteToolStripMenuItem.Name = "booleanPasteToolStripMenuItem";
			this.booleanPasteToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.booleanPasteToolStripMenuItem.Text = "Boolean paste";
			// 
			// oRToolStripMenuItem
			// 
			this.oRToolStripMenuItem.Name = "oRToolStripMenuItem";
			this.oRToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.oRToolStripMenuItem.Text = "OR";
			this.oRToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItemPasteOr_Click);
			// 
			// aNDToolStripMenuItem
			// 
			this.aNDToolStripMenuItem.Name = "aNDToolStripMenuItem";
			this.aNDToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.aNDToolStripMenuItem.Text = "AND";
			this.aNDToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItemPasteAnd_Click);
			// 
			// xORToolStripMenuItem
			// 
			this.xORToolStripMenuItem.Name = "xORToolStripMenuItem";
			this.xORToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.xORToolStripMenuItem.Text = "XOR";
			this.xORToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItemPasteXor_Click);
			// 
			// toolStripMenuItem16
			// 
			this.toolStripMenuItem16.Name = "toolStripMenuItem16";
			this.toolStripMenuItem16.Size = new System.Drawing.Size(169, 6);
			// 
			// nORToolStripMenuItem
			// 
			this.nORToolStripMenuItem.Name = "nORToolStripMenuItem";
			this.nORToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.nORToolStripMenuItem.Text = "NOR (NOT OR)";
			this.nORToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItemPasteNor_Click);
			// 
			// nANDToolStripMenuItem
			// 
			this.nANDToolStripMenuItem.Name = "nANDToolStripMenuItem";
			this.nANDToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.nANDToolStripMenuItem.Text = "NAND (NOT AND)";
			this.nANDToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItemPasteNand_Click);
			// 
			// xNORToolStripMenuItem
			// 
			this.xNORToolStripMenuItem.Name = "xNORToolStripMenuItem";
			this.xNORToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.xNORToolStripMenuItem.Text = "XNOR (NOT XOR)";
			this.xNORToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItemPasteXnor_Click);
			// 
			// insertPasteToolStripMenuItem
			// 
			this.insertPasteToolStripMenuItem.Name = "insertPasteToolStripMenuItem";
			this.insertPasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.V)));
			this.insertPasteToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.insertPasteToolStripMenuItem.Text = "Insert paste";
			this.insertPasteToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonInsertPaste_Click);
			// 
			// removeCellsToolStripMenuItem1
			// 
			this.removeCellsToolStripMenuItem1.Name = "removeCellsToolStripMenuItem1";
			this.removeCellsToolStripMenuItem1.Size = new System.Drawing.Size(201, 22);
			this.removeCellsToolStripMenuItem1.Text = "Remove Cells";
			this.removeCellsToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonRemoveCells_Click);
			// 
			// clearAllToolStripMenuItem
			// 
			this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
			this.clearAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.A)));
			this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.clearAllToolStripMenuItem.Text = "Clear all";
			this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
			// 
			// toolStripMenuItem18
			// 
			this.toolStripMenuItem18.Name = "toolStripMenuItem18";
			this.toolStripMenuItem18.Size = new System.Drawing.Size(198, 6);
			// 
			// findAndReplaceToolStripMenuItem
			// 
			this.findAndReplaceToolStripMenuItem.Name = "findAndReplaceToolStripMenuItem";
			this.findAndReplaceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.findAndReplaceToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.findAndReplaceToolStripMenuItem.Text = "Find and replace";
			this.findAndReplaceToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonFindAndReplace_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(198, 6);
			// 
			// copyChannelToolStripMenuItem
			// 
			this.copyChannelToolStripMenuItem.Name = "copyChannelToolStripMenuItem";
			this.copyChannelToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.copyChannelToolStripMenuItem.Text = "Copy channel";
			this.copyChannelToolStripMenuItem.Click += new System.EventHandler(this.copyChannelToolStripMenuItem_Click);
			// 
			// toolStripMenuItem22
			// 
			this.toolStripMenuItem22.Name = "toolStripMenuItem22";
			this.toolStripMenuItem22.Size = new System.Drawing.Size(198, 6);
			// 
			// audioSpeedToolStripMenuItem
			// 
			this.audioSpeedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xToolStripMenuItem,
            this.xToolStripMenuItem1,
            this.xToolStripMenuItem2,
            this.normalToolStripMenuItem1,
            this.toolStripMenuItem24,
            this.otherToolStripMenuItem});
			this.audioSpeedToolStripMenuItem.Name = "audioSpeedToolStripMenuItem";
			this.audioSpeedToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.audioSpeedToolStripMenuItem.Text = "Audio speed";
			// 
			// xToolStripMenuItem
			// 
			this.xToolStripMenuItem.Name = "xToolStripMenuItem";
			this.xToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
			this.xToolStripMenuItem.Text = "1/4x";
			this.xToolStripMenuItem.Click += new System.EventHandler(this.quarterSpeedToolStripMenuItem_Click);
			// 
			// xToolStripMenuItem1
			// 
			this.xToolStripMenuItem1.Name = "xToolStripMenuItem1";
			this.xToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
			this.xToolStripMenuItem1.Text = "1/2x";
			this.xToolStripMenuItem1.Click += new System.EventHandler(this.halfSpeedToolStripMenuItem_Click);
			// 
			// xToolStripMenuItem2
			// 
			this.xToolStripMenuItem2.Name = "xToolStripMenuItem2";
			this.xToolStripMenuItem2.Size = new System.Drawing.Size(114, 22);
			this.xToolStripMenuItem2.Text = "3/4x";
			this.xToolStripMenuItem2.Click += new System.EventHandler(this.xToolStripMenuItem2_Click);
			// 
			// normalToolStripMenuItem1
			// 
			this.normalToolStripMenuItem1.Checked = true;
			this.normalToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.normalToolStripMenuItem1.Name = "normalToolStripMenuItem1";
			this.normalToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
			this.normalToolStripMenuItem1.Text = "Normal";
			this.normalToolStripMenuItem1.Click += new System.EventHandler(this.normalSpeedToolStripMenuItem_Click);
			// 
			// toolStripMenuItem24
			// 
			this.toolStripMenuItem24.Name = "toolStripMenuItem24";
			this.toolStripMenuItem24.Size = new System.Drawing.Size(111, 6);
			// 
			// otherToolStripMenuItem
			// 
			this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
			this.otherToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
			this.otherToolStripMenuItem.Text = "Other";
			this.otherToolStripMenuItem.Click += new System.EventHandler(this.otherToolStripMenuItem_Click);
			// 
			// effectsToolStripMenuItem
			// 
			this.effectsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
			this.effectsToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.effectsToolStripMenuItem.MergeIndex = 3;
			this.effectsToolStripMenuItem.Name = "effectsToolStripMenuItem";
			this.effectsToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.effectsToolStripMenuItem.Text = "Effects";
			// 
			// onToolStripMenuItem1
			// 
			this.onToolStripMenuItem1.Name = "onToolStripMenuItem1";
			this.onToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.onToolStripMenuItem1.Text = "On";
			this.onToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonOn_Click);
			// 
			// offToolStripMenuItem1
			// 
			this.offToolStripMenuItem1.Name = "offToolStripMenuItem1";
			this.offToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.offToolStripMenuItem1.Text = "Off";
			this.offToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonOff_Click);
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(171, 6);
			// 
			// rampOnToolStripMenuItem1
			// 
			this.rampOnToolStripMenuItem1.Name = "rampOnToolStripMenuItem1";
			this.rampOnToolStripMenuItem1.ShortcutKeyDisplayString = "";
			this.rampOnToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.rampOnToolStripMenuItem1.Text = "Ramp On";
			this.rampOnToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonRampOn_Click);
			// 
			// rampOffToolStripMenuItem1
			// 
			this.rampOffToolStripMenuItem1.Name = "rampOffToolStripMenuItem1";
			this.rampOffToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.rampOffToolStripMenuItem1.Text = "Ramp Off";
			this.rampOffToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonRampOff_Click);
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Size = new System.Drawing.Size(171, 6);
			// 
			// partialRampOnToolStripMenuItem1
			// 
			this.partialRampOnToolStripMenuItem1.Name = "partialRampOnToolStripMenuItem1";
			this.partialRampOnToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.partialRampOnToolStripMenuItem1.Text = "Partial Ramp On";
			this.partialRampOnToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonPartialRampOn_Click);
			// 
			// partialRampOffToolStripMenuItem1
			// 
			this.partialRampOffToolStripMenuItem1.Name = "partialRampOffToolStripMenuItem1";
			this.partialRampOffToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.partialRampOffToolStripMenuItem1.Text = "Partial Ramp Off";
			this.partialRampOffToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonPartialRampOff_Click);
			// 
			// toolStripMenuItem10
			// 
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			this.toolStripMenuItem10.Size = new System.Drawing.Size(171, 6);
			// 
			// setIntensityToolStripMenuItem
			// 
			this.setIntensityToolStripMenuItem.Name = "setIntensityToolStripMenuItem";
			this.setIntensityToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
			this.setIntensityToolStripMenuItem.Text = "Set Intensity";
			this.setIntensityToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonIntensity_Click);
			// 
			// mirrorVerticallyToolStripMenuItem1
			// 
			this.mirrorVerticallyToolStripMenuItem1.Name = "mirrorVerticallyToolStripMenuItem1";
			this.mirrorVerticallyToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.mirrorVerticallyToolStripMenuItem1.Text = "Mirror Vertically";
			this.mirrorVerticallyToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonMirrorVertical_Click);
			// 
			// mirrorHorizontallyToolStripMenuItem1
			// 
			this.mirrorHorizontallyToolStripMenuItem1.Name = "mirrorHorizontallyToolStripMenuItem1";
			this.mirrorHorizontallyToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.mirrorHorizontallyToolStripMenuItem1.Text = "Mirror Horizontally";
			this.mirrorHorizontallyToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonMirrorHorizontal_Click);
			// 
			// invertToolStripMenuItem1
			// 
			this.invertToolStripMenuItem1.Name = "invertToolStripMenuItem1";
			this.invertToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.invertToolStripMenuItem1.Text = "Invert";
			this.invertToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonInvert_Click);
			// 
			// randomToolStripMenuItem
			// 
			this.randomToolStripMenuItem.Name = "randomToolStripMenuItem";
			this.randomToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
			this.randomToolStripMenuItem.Text = "Random";
			this.randomToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonRandom_Click);
			// 
			// shimmerToolStripMenuItem1
			// 
			this.shimmerToolStripMenuItem1.Name = "shimmerToolStripMenuItem1";
			this.shimmerToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.shimmerToolStripMenuItem1.Text = "Shimmer";
			this.shimmerToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonShimmerDimming_Click);
			// 
			// sparkleToolStripMenuItem1
			// 
			this.sparkleToolStripMenuItem1.Name = "sparkleToolStripMenuItem1";
			this.sparkleToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
			this.sparkleToolStripMenuItem1.Text = "Sparkle";
			this.sparkleToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonSparkle_Click);
			// 
			// toolStripMenuItem20
			// 
			this.toolStripMenuItem20.Name = "toolStripMenuItem20";
			this.toolStripMenuItem20.Size = new System.Drawing.Size(171, 6);
			// 
			// chaseLinesToolStripMenuItem
			// 
			this.chaseLinesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.normalToolStripMenuItem,
            this.paintFromClipboardToolStripMenuItem});
			this.chaseLinesToolStripMenuItem.Name = "chaseLinesToolStripMenuItem";
			this.chaseLinesToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
			this.chaseLinesToolStripMenuItem.Text = "Chase lines";
			// 
			// normalToolStripMenuItem
			// 
			this.normalToolStripMenuItem.Checked = true;
			this.normalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
			this.normalToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.normalToolStripMenuItem.Text = "Normal";
			this.normalToolStripMenuItem.Click += new System.EventHandler(this.normalToolStripMenuItem_Click);
			// 
			// paintFromClipboardToolStripMenuItem
			// 
			this.paintFromClipboardToolStripMenuItem.Name = "paintFromClipboardToolStripMenuItem";
			this.paintFromClipboardToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.paintFromClipboardToolStripMenuItem.Text = "Paint from Clipboard";
			this.paintFromClipboardToolStripMenuItem.Click += new System.EventHandler(this.paintFromClipboardToolStripMenuItem_Click);
			// 
			// profilesToolStripMenuItem
			// 
			this.profilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createFromSequenceToolStripMenuItem,
            this.attachSequenceToToolStripMenuItem,
            this.detachSequenceFromItsProfileToolStripMenuItem,
            this.flattenProfileIntoSequenceToolStripMenuItem});
			this.profilesToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
			this.profilesToolStripMenuItem.Name = "profilesToolStripMenuItem";
			this.profilesToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
			this.profilesToolStripMenuItem.Text = "Profiles";
			// 
			// createFromSequenceToolStripMenuItem
			// 
			this.createFromSequenceToolStripMenuItem.Name = "createFromSequenceToolStripMenuItem";
			this.createFromSequenceToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
			this.createFromSequenceToolStripMenuItem.Text = "Create profile from sequence";
			this.createFromSequenceToolStripMenuItem.Click += new System.EventHandler(this.createFromSequenceToolStripMenuItem_Click);
			// 
			// attachSequenceToToolStripMenuItem
			// 
			this.attachSequenceToToolStripMenuItem.Name = "attachSequenceToToolStripMenuItem";
			this.attachSequenceToToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
			this.attachSequenceToToolStripMenuItem.Text = "Attach sequence to profile";
			this.attachSequenceToToolStripMenuItem.Click += new System.EventHandler(this.attachSequenceToToolStripMenuItem_Click);
			// 
			// detachSequenceFromItsProfileToolStripMenuItem
			// 
			this.detachSequenceFromItsProfileToolStripMenuItem.Enabled = false;
			this.detachSequenceFromItsProfileToolStripMenuItem.Name = "detachSequenceFromItsProfileToolStripMenuItem";
			this.detachSequenceFromItsProfileToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
			this.detachSequenceFromItsProfileToolStripMenuItem.Text = "Detach sequence from its profile";
			this.detachSequenceFromItsProfileToolStripMenuItem.Click += new System.EventHandler(this.detachSequenceFromItsProfileToolStripMenuItem_Click);
			// 
			// flattenProfileIntoSequenceToolStripMenuItem
			// 
			this.flattenProfileIntoSequenceToolStripMenuItem.Enabled = false;
			this.flattenProfileIntoSequenceToolStripMenuItem.Name = "flattenProfileIntoSequenceToolStripMenuItem";
			this.flattenProfileIntoSequenceToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
			this.flattenProfileIntoSequenceToolStripMenuItem.Text = "Flatten profile into sequence";
			this.flattenProfileIntoSequenceToolStripMenuItem.Click += new System.EventHandler(this.flattenProfileIntoSequenceToolStripMenuItem_Click);
			// 
			// toolbarsToolStripMenuItem
			// 
			this.toolbarsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem15,
            this.resetAllToolbarsToolStripMenuItem,
            this.saveToolbarPositionsToolStripMenuItem});
			this.toolbarsToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.toolbarsToolStripMenuItem.MergeIndex = 8;
			this.toolbarsToolStripMenuItem.Name = "toolbarsToolStripMenuItem";
			this.toolbarsToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
			this.toolbarsToolStripMenuItem.Text = "Toolbars";
			// 
			// toolStripMenuItem15
			// 
			this.toolStripMenuItem15.Name = "toolStripMenuItem15";
			this.toolStripMenuItem15.Size = new System.Drawing.Size(187, 6);
			// 
			// resetAllToolbarsToolStripMenuItem
			// 
			this.resetAllToolbarsToolStripMenuItem.Name = "resetAllToolbarsToolStripMenuItem";
			this.resetAllToolbarsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.resetAllToolbarsToolStripMenuItem.Text = "Reset all toolbars";
			this.resetAllToolbarsToolStripMenuItem.Click += new System.EventHandler(this.resetAllToolbarsToolStripMenuItem_Click);
			// 
			// saveToolbarPositionsToolStripMenuItem
			// 
			this.saveToolbarPositionsToolStripMenuItem.Name = "saveToolbarPositionsToolStripMenuItem";
			this.saveToolbarPositionsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.saveToolbarPositionsToolStripMenuItem.Text = "Save toolbar positions";
			this.saveToolbarPositionsToolStripMenuItem.Click += new System.EventHandler(this.saveToolbarPositionsToolStripMenuItem_Click);
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(740, 269);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size(740, 444);
			this.toolStripContainer1.TabIndex = 1;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripSequenceSettings);
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripExecutionControl);
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripEffect);
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripEditing);
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripText);
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripVisualizer);
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripDisplaySettings);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.pictureBoxLevelNumber);
			this.splitContainer1.Panel1.Controls.Add(this.pictureBoxLevelPercent);
			this.splitContainer1.Panel1.Controls.Add(this.pictureBoxOutputArrow);
			this.splitContainer1.Panel1.Controls.Add(this.labelPosition);
			this.splitContainer1.Panel1.Controls.Add(this.pictureBoxChannels);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pictureBoxGrid);
			this.splitContainer1.Panel2.Controls.Add(this.hScrollBar1);
			this.splitContainer1.Panel2.Controls.Add(this.vScrollBar1);
			this.splitContainer1.Panel2.Controls.Add(this.pictureBoxTime);
			this.splitContainer1.Size = new System.Drawing.Size(740, 269);
			this.splitContainer1.SplitterDistance = 139;
			this.splitContainer1.TabIndex = 20;
			// 
			// pictureBoxLevelNumber
			// 
			this.pictureBoxLevelNumber.Location = new System.Drawing.Point(15, 60);
			this.pictureBoxLevelNumber.Name = "pictureBoxLevelNumber";
			this.pictureBoxLevelNumber.Size = new System.Drawing.Size(16, 16);
			this.pictureBoxLevelNumber.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBoxLevelNumber.TabIndex = 21;
			this.pictureBoxLevelNumber.TabStop = false;
			this.pictureBoxLevelNumber.Visible = false;
			// 
			// pictureBoxLevelPercent
			// 
			this.pictureBoxLevelPercent.Location = new System.Drawing.Point(15, 29);
			this.pictureBoxLevelPercent.Name = "pictureBoxLevelPercent";
			this.pictureBoxLevelPercent.Size = new System.Drawing.Size(16, 16);
			this.pictureBoxLevelPercent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBoxLevelPercent.TabIndex = 20;
			this.pictureBoxLevelPercent.TabStop = false;
			this.pictureBoxLevelPercent.Visible = false;
			// 
			// pictureBoxOutputArrow
			// 
			this.pictureBoxOutputArrow.Location = new System.Drawing.Point(15, 9);
			this.pictureBoxOutputArrow.Name = "pictureBoxOutputArrow";
			this.pictureBoxOutputArrow.Size = new System.Drawing.Size(11, 11);
			this.pictureBoxOutputArrow.TabIndex = 19;
			this.pictureBoxOutputArrow.TabStop = false;
			this.pictureBoxOutputArrow.Visible = false;
			// 
			// labelPosition
			// 
			this.labelPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelPosition.BackColor = System.Drawing.Color.White;
			this.labelPosition.Location = new System.Drawing.Point(12, 9);
			this.labelPosition.Name = "labelPosition";
			this.labelPosition.Size = new System.Drawing.Size(124, 31);
			this.labelPosition.TabIndex = 12;
			// 
			// pictureBoxChannels
			// 
			this.pictureBoxChannels.BackColor = System.Drawing.Color.White;
			this.pictureBoxChannels.ContextMenuStrip = this.contextMenuChannels;
			this.pictureBoxChannels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxChannels.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxChannels.Name = "pictureBoxChannels";
			this.pictureBoxChannels.Size = new System.Drawing.Size(139, 269);
			this.pictureBoxChannels.TabIndex = 11;
			this.pictureBoxChannels.TabStop = false;
			this.pictureBoxChannels.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBoxChannels_DragDrop);
			this.pictureBoxChannels.DragOver += new System.Windows.Forms.DragEventHandler(this.pictureBoxChannels_DragOver);
			this.pictureBoxChannels.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.pictureBoxChannels_GiveFeedback);
			this.pictureBoxChannels.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxChannels_Paint);
			this.pictureBoxChannels.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.pictureBoxChannels_QueryContinueDrag);
			this.pictureBoxChannels.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxChannels_MouseDoubleClick);
			this.pictureBoxChannels.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxChannels_MouseDown);
			this.pictureBoxChannels.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxChannels_MouseMove);
			this.pictureBoxChannels.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxChannels_MouseUp);
			this.pictureBoxChannels.Resize += new System.EventHandler(this.pictureBoxChannels_Resize);
			// 
			// contextMenuChannels
			// 
			this.contextMenuChannels.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleOutputChannelsToolStripMenuItem,
            this.reorderChannelOutputsToolStripMenuItem,
            this.toolStripMenuItem5,
            this.clearChannelEventsToolStripMenuItem,
            this.allEventsToFullIntensityToolStripMenuItem,
            this.copyChannelToolStripMenuItem1,
            this.setAllChannelColorsToolStripMenuItem,
            this.toolStripMenuItem23,
            this.copyChannelEventsToClipboardToolStripMenuItem,
            this.pasteFullChannelEventsFromClipboardToolStripMenuItem,
            this.toolStripMenuItem14,
            this.channelPropertiesToolStripMenuItem});
			this.contextMenuChannels.Name = "contextMenuChannels";
			this.contextMenuChannels.Size = new System.Drawing.Size(287, 220);
			this.contextMenuChannels.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuChannels_Opening);
			// 
			// toggleOutputChannelsToolStripMenuItem
			// 
			this.toggleOutputChannelsToolStripMenuItem.Name = "toggleOutputChannelsToolStripMenuItem";
			this.toggleOutputChannelsToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
			this.toggleOutputChannelsToolStripMenuItem.Text = "Toggle channel outputs";
			this.toggleOutputChannelsToolStripMenuItem.Click += new System.EventHandler(this.toggleOutputChannelsToolStripMenuItem_Click);
			// 
			// reorderChannelOutputsToolStripMenuItem
			// 
			this.reorderChannelOutputsToolStripMenuItem.Name = "reorderChannelOutputsToolStripMenuItem";
			this.reorderChannelOutputsToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
			this.reorderChannelOutputsToolStripMenuItem.Text = "Reorder channel outputs";
			this.reorderChannelOutputsToolStripMenuItem.Click += new System.EventHandler(this.reorderChannelOutputsToolStripMenuItem_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(283, 6);
			// 
			// clearChannelEventsToolStripMenuItem
			// 
			this.clearChannelEventsToolStripMenuItem.Name = "clearChannelEventsToolStripMenuItem";
			this.clearChannelEventsToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
			this.clearChannelEventsToolStripMenuItem.Text = "Clear channel events";
			this.clearChannelEventsToolStripMenuItem.Click += new System.EventHandler(this.clearChannelEventsToolStripMenuItem_Click);
			// 
			// allEventsToFullIntensityToolStripMenuItem
			// 
			this.allEventsToFullIntensityToolStripMenuItem.Name = "allEventsToFullIntensityToolStripMenuItem";
			this.allEventsToFullIntensityToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
			this.allEventsToFullIntensityToolStripMenuItem.Text = "All events to full intensity";
			this.allEventsToFullIntensityToolStripMenuItem.Click += new System.EventHandler(this.allEventsToFullIntensityToolStripMenuItem_Click);
			// 
			// copyChannelToolStripMenuItem1
			// 
			this.copyChannelToolStripMenuItem1.Name = "copyChannelToolStripMenuItem1";
			this.copyChannelToolStripMenuItem1.Size = new System.Drawing.Size(286, 22);
			this.copyChannelToolStripMenuItem1.Text = "Copy channel...";
			this.copyChannelToolStripMenuItem1.Click += new System.EventHandler(this.copyChannelToolStripMenuItem_Click);
			// 
			// setAllChannelColorsToolStripMenuItem
			// 
			this.setAllChannelColorsToolStripMenuItem.Name = "setAllChannelColorsToolStripMenuItem";
			this.setAllChannelColorsToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
			this.setAllChannelColorsToolStripMenuItem.Text = "Set all channel colors";
			this.setAllChannelColorsToolStripMenuItem.Click += new System.EventHandler(this.setAllChannelColorsToolStripMenuItem_Click);
			// 
			// toolStripMenuItem23
			// 
			this.toolStripMenuItem23.Name = "toolStripMenuItem23";
			this.toolStripMenuItem23.Size = new System.Drawing.Size(283, 6);
			// 
			// copyChannelEventsToClipboardToolStripMenuItem
			// 
			this.copyChannelEventsToClipboardToolStripMenuItem.Name = "copyChannelEventsToClipboardToolStripMenuItem";
			this.copyChannelEventsToClipboardToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
			this.copyChannelEventsToClipboardToolStripMenuItem.Text = "Copy full channel events to clipboard";
			this.copyChannelEventsToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyChannelEventsToClipboardToolStripMenuItem_Click);
			// 
			// pasteFullChannelEventsFromClipboardToolStripMenuItem
			// 
			this.pasteFullChannelEventsFromClipboardToolStripMenuItem.Name = "pasteFullChannelEventsFromClipboardToolStripMenuItem";
			this.pasteFullChannelEventsFromClipboardToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
			this.pasteFullChannelEventsFromClipboardToolStripMenuItem.Text = "Paste full channel events from clipboard";
			this.pasteFullChannelEventsFromClipboardToolStripMenuItem.Click += new System.EventHandler(this.pasteFullChannelEventsFromClipboardToolStripMenuItem_Click);
			// 
			// toolStripMenuItem14
			// 
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			this.toolStripMenuItem14.Size = new System.Drawing.Size(283, 6);
			// 
			// channelPropertiesToolStripMenuItem
			// 
			this.channelPropertiesToolStripMenuItem.Name = "channelPropertiesToolStripMenuItem";
			this.channelPropertiesToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
			this.channelPropertiesToolStripMenuItem.Text = "Channel properties";
			this.channelPropertiesToolStripMenuItem.Click += new System.EventHandler(this.channelPropertiesToolStripMenuItem_Click);
			// 
			// pictureBoxGrid
			// 
			this.pictureBoxGrid.BackColor = System.Drawing.Color.White;
			this.pictureBoxGrid.ContextMenuStrip = this.contextMenuGrid;
			this.pictureBoxGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxGrid.Location = new System.Drawing.Point(0, 60);
			this.pictureBoxGrid.Name = "pictureBoxGrid";
			this.pictureBoxGrid.Size = new System.Drawing.Size(580, 192);
			this.pictureBoxGrid.TabIndex = 3;
			this.pictureBoxGrid.TabStop = false;
			this.pictureBoxGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxGrid_Paint);
			this.pictureBoxGrid.DoubleClick += new System.EventHandler(this.pictureBoxGrid_DoubleClick);
			this.pictureBoxGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxGrid_MouseDown);
			this.pictureBoxGrid.MouseLeave += new System.EventHandler(this.pictureBoxGrid_MouseLeave);
			this.pictureBoxGrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxGrid_MouseMove);
			this.pictureBoxGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxGrid_MouseUp);
			this.pictureBoxGrid.Resize += new System.EventHandler(this.pictureBoxGrid_Resize);
			// 
			// contextMenuGrid
			// 
			this.contextMenuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
			this.contextMenuGrid.Size = new System.Drawing.Size(209, 546);
			this.contextMenuGrid.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuGrid_Opening);
			// 
			// onToolStripMenuItem
			// 
			this.onToolStripMenuItem.Name = "onToolStripMenuItem";
			this.onToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.onToolStripMenuItem.Text = "On";
			this.onToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonOn_Click);
			// 
			// offToolStripMenuItem
			// 
			this.offToolStripMenuItem.Name = "offToolStripMenuItem";
			this.offToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.offToolStripMenuItem.Text = "Off";
			this.offToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonOff_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(205, 6);
			// 
			// rampOnToolStripMenuItem
			// 
			this.rampOnToolStripMenuItem.Name = "rampOnToolStripMenuItem";
			this.rampOnToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.rampOnToolStripMenuItem.Text = "Ramp on";
			this.rampOnToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonRampOn_Click);
			// 
			// rampOffToolStripMenuItem
			// 
			this.rampOffToolStripMenuItem.Name = "rampOffToolStripMenuItem";
			this.rampOffToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.rampOffToolStripMenuItem.Text = "Ramp off";
			this.rampOffToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonRampOff_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(205, 6);
			// 
			// partialRampOnToolStripMenuItem
			// 
			this.partialRampOnToolStripMenuItem.Name = "partialRampOnToolStripMenuItem";
			this.partialRampOnToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.partialRampOnToolStripMenuItem.Text = "Partial ramp on";
			this.partialRampOnToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonPartialRampOn_Click);
			// 
			// partialRampOffToolStripMenuItem
			// 
			this.partialRampOffToolStripMenuItem.Name = "partialRampOffToolStripMenuItem";
			this.partialRampOffToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.partialRampOffToolStripMenuItem.Text = "Partial ramp off";
			this.partialRampOffToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonPartialRampOff_Click);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(205, 6);
			// 
			// cutToolStripMenuItem1
			// 
			this.cutToolStripMenuItem1.Name = "cutToolStripMenuItem1";
			this.cutToolStripMenuItem1.Size = new System.Drawing.Size(208, 22);
			this.cutToolStripMenuItem1.Text = "Cut";
			this.cutToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonCut_Click);
			// 
			// copyToolStripMenuItem1
			// 
			this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
			this.copyToolStripMenuItem1.Size = new System.Drawing.Size(208, 22);
			this.copyToolStripMenuItem1.Text = "Copy";
			this.copyToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
			// 
			// opaquePasteToolStripMenuItem1
			// 
			this.opaquePasteToolStripMenuItem1.Name = "opaquePasteToolStripMenuItem1";
			this.opaquePasteToolStripMenuItem1.Size = new System.Drawing.Size(208, 22);
			this.opaquePasteToolStripMenuItem1.Text = "Opaque paste";
			this.opaquePasteToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonOpaquePaste_Click);
			// 
			// transparentPasteToolStripMenuItem1
			// 
			this.transparentPasteToolStripMenuItem1.Name = "transparentPasteToolStripMenuItem1";
			this.transparentPasteToolStripMenuItem1.Size = new System.Drawing.Size(208, 22);
			this.transparentPasteToolStripMenuItem1.Text = "Transparent paste";
			this.transparentPasteToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonTransparentPaste_Click);
			// 
			// pasteToolStripMenuItem1
			// 
			this.pasteToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.booleanPasteToolStripMenuItem1,
            this.insertPasteToolStripMenuItem1,
            this.arithmeticPasteToolStripMenuItem});
			this.pasteToolStripMenuItem1.Name = "pasteToolStripMenuItem1";
			this.pasteToolStripMenuItem1.Size = new System.Drawing.Size(208, 22);
			this.pasteToolStripMenuItem1.Text = "More paste";
			// 
			// booleanPasteToolStripMenuItem1
			// 
			this.booleanPasteToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oRToolStripMenuItem1,
            this.aNDToolStripMenuItem1,
            this.xORToolStripMenuItem1,
            this.toolStripMenuItem17,
            this.nORToolStripMenuItem1,
            this.nANDToolStripMenuItem1,
            this.xNORToolStripMenuItem1});
			this.booleanPasteToolStripMenuItem1.Name = "booleanPasteToolStripMenuItem1";
			this.booleanPasteToolStripMenuItem1.Size = new System.Drawing.Size(161, 22);
			this.booleanPasteToolStripMenuItem1.Text = "Boolean paste";
			// 
			// oRToolStripMenuItem1
			// 
			this.oRToolStripMenuItem1.Name = "oRToolStripMenuItem1";
			this.oRToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
			this.oRToolStripMenuItem1.Text = "OR";
			this.oRToolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItemPasteOr_Click);
			// 
			// aNDToolStripMenuItem1
			// 
			this.aNDToolStripMenuItem1.Name = "aNDToolStripMenuItem1";
			this.aNDToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
			this.aNDToolStripMenuItem1.Text = "AND";
			this.aNDToolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItemPasteAnd_Click);
			// 
			// xORToolStripMenuItem1
			// 
			this.xORToolStripMenuItem1.Name = "xORToolStripMenuItem1";
			this.xORToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
			this.xORToolStripMenuItem1.Text = "XOR";
			this.xORToolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItemPasteXor_Click);
			// 
			// toolStripMenuItem17
			// 
			this.toolStripMenuItem17.Name = "toolStripMenuItem17";
			this.toolStripMenuItem17.Size = new System.Drawing.Size(169, 6);
			// 
			// nORToolStripMenuItem1
			// 
			this.nORToolStripMenuItem1.Name = "nORToolStripMenuItem1";
			this.nORToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
			this.nORToolStripMenuItem1.Text = "NOR (NOT OR)";
			this.nORToolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItemPasteNor_Click);
			// 
			// nANDToolStripMenuItem1
			// 
			this.nANDToolStripMenuItem1.Name = "nANDToolStripMenuItem1";
			this.nANDToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
			this.nANDToolStripMenuItem1.Text = "NAND (NOT AND)";
			this.nANDToolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItemPasteNand_Click);
			// 
			// xNORToolStripMenuItem1
			// 
			this.xNORToolStripMenuItem1.Name = "xNORToolStripMenuItem1";
			this.xNORToolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
			this.xNORToolStripMenuItem1.Text = "XNOR (NOT XOR)";
			this.xNORToolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItemPasteXnor_Click);
			// 
			// insertPasteToolStripMenuItem1
			// 
			this.insertPasteToolStripMenuItem1.Name = "insertPasteToolStripMenuItem1";
			this.insertPasteToolStripMenuItem1.Size = new System.Drawing.Size(161, 22);
			this.insertPasteToolStripMenuItem1.Text = "Insert Paste";
			this.insertPasteToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonInsertPaste_Click);
			// 
			// arithmeticPasteToolStripMenuItem
			// 
			this.arithmeticPasteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.additionToolStripMenuItem1,
            this.subtractionToolStripMenuItem1,
            this.scaleToolStripMenuItem1,
            this.minToolStripMenuItem1,
            this.maxToolStripMenuItem1});
			this.arithmeticPasteToolStripMenuItem.Name = "arithmeticPasteToolStripMenuItem";
			this.arithmeticPasteToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.arithmeticPasteToolStripMenuItem.Text = "Arithmetic Paste";
			// 
			// additionToolStripMenuItem1
			// 
			this.additionToolStripMenuItem1.Name = "additionToolStripMenuItem1";
			this.additionToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
			this.additionToolStripMenuItem1.Text = "Addition";
			this.additionToolStripMenuItem1.Click += new System.EventHandler(this.additionToolStripMenuItem_Click);
			// 
			// subtractionToolStripMenuItem1
			// 
			this.subtractionToolStripMenuItem1.Name = "subtractionToolStripMenuItem1";
			this.subtractionToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
			this.subtractionToolStripMenuItem1.Text = "Subtraction";
			this.subtractionToolStripMenuItem1.Click += new System.EventHandler(this.subtractionToolStripMenuItem_Click);
			// 
			// scaleToolStripMenuItem1
			// 
			this.scaleToolStripMenuItem1.Name = "scaleToolStripMenuItem1";
			this.scaleToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
			this.scaleToolStripMenuItem1.Text = "Scale";
			this.scaleToolStripMenuItem1.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
			// 
			// minToolStripMenuItem1
			// 
			this.minToolStripMenuItem1.Name = "minToolStripMenuItem1";
			this.minToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
			this.minToolStripMenuItem1.Text = "Min";
			this.minToolStripMenuItem1.Click += new System.EventHandler(this.minToolStripMenuItem_Click);
			// 
			// maxToolStripMenuItem1
			// 
			this.maxToolStripMenuItem1.Name = "maxToolStripMenuItem1";
			this.maxToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
			this.maxToolStripMenuItem1.Text = "Max";
			this.maxToolStripMenuItem1.Click += new System.EventHandler(this.maxToolStripMenuItem_Click);
			// 
			// removeCellsToolStripMenuItem
			// 
			this.removeCellsToolStripMenuItem.Name = "removeCellsToolStripMenuItem";
			this.removeCellsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.removeCellsToolStripMenuItem.Text = "Remove cells";
			this.removeCellsToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonRemoveCells_Click);
			// 
			// toolStripMenuItem12
			// 
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			this.toolStripMenuItem12.Size = new System.Drawing.Size(205, 6);
			// 
			// findAndReplaceToolStripMenuItem1
			// 
			this.findAndReplaceToolStripMenuItem1.Name = "findAndReplaceToolStripMenuItem1";
			this.findAndReplaceToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.findAndReplaceToolStripMenuItem1.Size = new System.Drawing.Size(208, 22);
			this.findAndReplaceToolStripMenuItem1.Text = "Find and replace";
			this.findAndReplaceToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonFindAndReplace_Click);
			// 
			// toolStripMenuItem21
			// 
			this.toolStripMenuItem21.Name = "toolStripMenuItem21";
			this.toolStripMenuItem21.Size = new System.Drawing.Size(205, 6);
			// 
			// setIntensityToolStripMenuItem1
			// 
			this.setIntensityToolStripMenuItem1.Name = "setIntensityToolStripMenuItem1";
			this.setIntensityToolStripMenuItem1.Size = new System.Drawing.Size(208, 22);
			this.setIntensityToolStripMenuItem1.Text = "Set intensity";
			this.setIntensityToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonIntensity_Click);
			// 
			// mirrorVerticallyToolStripMenuItem
			// 
			this.mirrorVerticallyToolStripMenuItem.Name = "mirrorVerticallyToolStripMenuItem";
			this.mirrorVerticallyToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.mirrorVerticallyToolStripMenuItem.Text = "Mirror vertically";
			this.mirrorVerticallyToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonMirrorVertical_Click);
			// 
			// mirrorHorizontallyToolStripMenuItem
			// 
			this.mirrorHorizontallyToolStripMenuItem.Name = "mirrorHorizontallyToolStripMenuItem";
			this.mirrorHorizontallyToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.mirrorHorizontallyToolStripMenuItem.Text = "Mirror horizontally";
			this.mirrorHorizontallyToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonMirrorHorizontal_Click);
			// 
			// invertToolStripMenuItem
			// 
			this.invertToolStripMenuItem.Name = "invertToolStripMenuItem";
			this.invertToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.invertToolStripMenuItem.Text = "Invert";
			this.invertToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonInvert_Click);
			// 
			// randomToolStripMenuItem1
			// 
			this.randomToolStripMenuItem1.Name = "randomToolStripMenuItem1";
			this.randomToolStripMenuItem1.Size = new System.Drawing.Size(208, 22);
			this.randomToolStripMenuItem1.Text = "Random";
			this.randomToolStripMenuItem1.Click += new System.EventHandler(this.toolStripButtonRandom_Click);
			// 
			// shimmerToolStripMenuItem
			// 
			this.shimmerToolStripMenuItem.Name = "shimmerToolStripMenuItem";
			this.shimmerToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.shimmerToolStripMenuItem.Text = "Shimmer";
			this.shimmerToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonShimmerDimming_Click);
			// 
			// sparkleToolStripMenuItem
			// 
			this.sparkleToolStripMenuItem.Name = "sparkleToolStripMenuItem";
			this.sparkleToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.sparkleToolStripMenuItem.Text = "Sparkle";
			this.sparkleToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonSparkle_Click);
			// 
			// toolStripMenuItem11
			// 
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			this.toolStripMenuItem11.Size = new System.Drawing.Size(205, 6);
			// 
			// saveAsARoutineToolStripMenuItem
			// 
			this.saveAsARoutineToolStripMenuItem.Name = "saveAsARoutineToolStripMenuItem";
			this.saveAsARoutineToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.saveAsARoutineToolStripMenuItem.Text = "Save as a routine";
			this.saveAsARoutineToolStripMenuItem.Click += new System.EventHandler(this.saveAsARoutineToolStripMenuItem_Click);
			// 
			// loadARoutineToolStripMenuItem
			// 
			this.loadARoutineToolStripMenuItem.Name = "loadARoutineToolStripMenuItem";
			this.loadARoutineToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.loadARoutineToolStripMenuItem.Text = "Load a routine";
			this.loadARoutineToolStripMenuItem.Click += new System.EventHandler(this.loadARoutineToolStripMenuItem_Click);
			// 
			// loadRoutineToClipboardToolStripMenuItem
			// 
			this.loadRoutineToClipboardToolStripMenuItem.Name = "loadRoutineToClipboardToolStripMenuItem";
			this.loadRoutineToClipboardToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
			this.loadRoutineToClipboardToolStripMenuItem.Text = "Load routine to clipboard";
			this.loadRoutineToClipboardToolStripMenuItem.Click += new System.EventHandler(this.loadRoutineToClipboardToolStripMenuItem_Click);
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.hScrollBar1.Location = new System.Drawing.Point(0, 252);
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(580, 17);
			this.hScrollBar1.TabIndex = 2;
			this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
			this.vScrollBar1.Location = new System.Drawing.Point(580, 60);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(17, 209);
			this.vScrollBar1.TabIndex = 1;
			this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
			// 
			// pictureBoxTime
			// 
			this.pictureBoxTime.BackColor = System.Drawing.Color.White;
			this.pictureBoxTime.ContextMenuStrip = this.contextMenuTime;
			this.pictureBoxTime.Dock = System.Windows.Forms.DockStyle.Top;
			this.pictureBoxTime.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxTime.Name = "pictureBoxTime";
			this.pictureBoxTime.Size = new System.Drawing.Size(597, 60);
			this.pictureBoxTime.TabIndex = 0;
			this.pictureBoxTime.TabStop = false;
			this.pictureBoxTime.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxTime_Paint);
			// 
			// contextMenuTime
			// 
			this.contextMenuTime.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllChannelsForThisEventToolStripMenuItem,
            this.allChannelsToFullIntensityForThisEventToolStripMenuItem});
			this.contextMenuTime.Name = "contextMenuTime";
			this.contextMenuTime.Size = new System.Drawing.Size(293, 48);
			this.contextMenuTime.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuTime_Opening);
			// 
			// clearAllChannelsForThisEventToolStripMenuItem
			// 
			this.clearAllChannelsForThisEventToolStripMenuItem.Name = "clearAllChannelsForThisEventToolStripMenuItem";
			this.clearAllChannelsForThisEventToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
			this.clearAllChannelsForThisEventToolStripMenuItem.Text = "Clear all channels for this event";
			this.clearAllChannelsForThisEventToolStripMenuItem.Click += new System.EventHandler(this.clearAllChannelsForThisEventToolStripMenuItem_Click);
			// 
			// allChannelsToFullIntensityForThisEventToolStripMenuItem
			// 
			this.allChannelsToFullIntensityForThisEventToolStripMenuItem.Name = "allChannelsToFullIntensityForThisEventToolStripMenuItem";
			this.allChannelsToFullIntensityForThisEventToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
			this.allChannelsToFullIntensityForThisEventToolStripMenuItem.Text = "All channels to full intensity for this event";
			this.allChannelsToFullIntensityForThisEventToolStripMenuItem.Click += new System.EventHandler(this.allChannelsToFullIntensityForThisEventToolStripMenuItem_Click);
			// 
			// toolStripSequenceSettings
			// 
			this.toolStripSequenceSettings.AllowItemReorder = true;
			this.toolStripSequenceSettings.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripSequenceSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripSeparator8,
            this.toolStripLabel1,
            this.textBoxChannelCount,
            this.toolStripButtonTestChannels,
            this.toolStripButtonTestConsole,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.textBoxProgramLength,
            this.toolStripSeparator3,
            this.toolStripButtonAudio,
            this.toolStripSeparator13,
            this.toolStripButtonChannelOutputMask});
			this.toolStripSequenceSettings.Location = new System.Drawing.Point(3, 0);
			this.toolStripSequenceSettings.Name = "toolStripSequenceSettings";
			this.toolStripSequenceSettings.Size = new System.Drawing.Size(457, 25);
			this.toolStripSequenceSettings.TabIndex = 1;
			this.toolStripSequenceSettings.Text = "Sequence settings";
			// 
			// toolStripButtonSave
			// 
			this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSave.Enabled = false;
			this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSave.Name = "toolStripButtonSave";
			this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonSave.Text = "toolStripButton1";
			this.toolStripButtonSave.ToolTipText = "Save this event sequence";
			this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(56, 22);
			this.toolStripLabel1.Text = "Channels";
			// 
			// textBoxChannelCount
			// 
			this.textBoxChannelCount.MaxLength = 5;
			this.textBoxChannelCount.Name = "textBoxChannelCount";
			this.textBoxChannelCount.Size = new System.Drawing.Size(40, 25);
			this.textBoxChannelCount.Text = "0";
			this.textBoxChannelCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxChannelCount_KeyPress);
			// 
			// toolStripButtonTestChannels
			// 
			this.toolStripButtonTestChannels.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonTestChannels.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonTestChannels.Name = "toolStripButtonTestChannels";
			this.toolStripButtonTestChannels.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonTestChannels.Text = "toolStripButton1";
			this.toolStripButtonTestChannels.ToolTipText = "Test channels";
			this.toolStripButtonTestChannels.Click += new System.EventHandler(this.toolStripButtonTestChannels_Click);
			// 
			// toolStripButtonTestConsole
			// 
			this.toolStripButtonTestConsole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonTestConsole.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonTestConsole.Name = "toolStripButtonTestConsole";
			this.toolStripButtonTestConsole.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonTestConsole.Text = "Test console";
			this.toolStripButtonTestConsole.Click += new System.EventHandler(this.toolStripButtonTestConsole_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(131, 22);
			this.toolStripLabel2.Text = "Sequence time (mm:ss)";
			// 
			// textBoxProgramLength
			// 
			this.textBoxProgramLength.Name = "textBoxProgramLength";
			this.textBoxProgramLength.Size = new System.Drawing.Size(75, 25);
			this.textBoxProgramLength.Text = "00:00";
			this.textBoxProgramLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxProgramLength_KeyPress);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonAudio
			// 
			this.toolStripButtonAudio.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonAudio.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonAudio.Name = "toolStripButtonAudio";
			this.toolStripButtonAudio.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonAudio.Text = "Add audio or events";
			this.toolStripButtonAudio.Click += new System.EventHandler(this.toolStripButtonAudio_Click);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonChannelOutputMask
			// 
			this.toolStripButtonChannelOutputMask.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonChannelOutputMask.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonChannelOutputMask.Name = "toolStripButtonChannelOutputMask";
			this.toolStripButtonChannelOutputMask.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonChannelOutputMask.Text = "Channel Output Mask";
			this.toolStripButtonChannelOutputMask.ToolTipText = "Enable/disable channels for this sequence";
			this.toolStripButtonChannelOutputMask.Click += new System.EventHandler(this.toolStripButtonChannelOutputMask_Click);
			// 
			// toolStripExecutionControl
			// 
			this.toolStripExecutionControl.AllowItemReorder = true;
			this.toolStripExecutionControl.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripExecutionControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButtonPlugins,
            this.toolStripSeparator4,
            this.toolStripButtonPlay,
            this.toolStripButtonPlayPoint,
            this.toolStripButtonPause,
            this.toolStripButtonStop,
            this.toolStripLabelProgess,
            this.toolStripSeparator5,
            this.toolStripButtonLoop,
            this.toolStripSeparator10,
            this.toolStripButtonPlaySpeedQuarter,
            this.toolStripButtonPlaySpeedHalf,
            this.toolStripButtonPlaySpeedThreeQuarters,
            this.toolStripButtonPlaySpeedNormal,
            this.toolStripButtonPlaySpeedVariable,
            this.toolStripLabelIntensity});
			this.toolStripExecutionControl.Location = new System.Drawing.Point(3, 25);
			this.toolStripExecutionControl.Name = "toolStripExecutionControl";
			this.toolStripExecutionControl.Size = new System.Drawing.Size(363, 25);
			this.toolStripExecutionControl.TabIndex = 2;
			this.toolStripExecutionControl.Text = "Execution control";
			// 
			// toolStripDropDownButtonPlugins
			// 
			this.toolStripDropDownButtonPlugins.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButtonPlugins.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButtonPlugins.Name = "toolStripDropDownButtonPlugins";
			this.toolStripDropDownButtonPlugins.Size = new System.Drawing.Size(110, 22);
			this.toolStripDropDownButtonPlugins.Text = "Attached Plugins";
			this.toolStripDropDownButtonPlugins.Click += new System.EventHandler(this.toolStripDropDownButtonPlugins_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonPlay
			// 
			this.toolStripButtonPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPlay.Name = "toolStripButtonPlay";
			this.toolStripButtonPlay.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPlay.Text = "Play this sequence (F5)";
			this.toolStripButtonPlay.Click += new System.EventHandler(this.toolStripButtonPlay_Click);
			// 
			// toolStripButtonPlayPoint
			// 
			this.toolStripButtonPlayPoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPlayPoint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playAtTheSelectedPointToolStripMenuItem,
            this.playOnlyTheSelectedRangeToolStripMenuItem});
			this.toolStripButtonPlayPoint.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPlayPoint.Name = "toolStripButtonPlayPoint";
			this.toolStripButtonPlayPoint.Size = new System.Drawing.Size(16, 22);
			this.toolStripButtonPlayPoint.Text = "Play this sequence starting at the selection point (F6)";
			this.toolStripButtonPlayPoint.ToolTipText = "Play this sequence starting at the selection point (F6)";
			this.toolStripButtonPlayPoint.Click += new System.EventHandler(this.toolStripButtonPlayPoint_Click);
			// 
			// playAtTheSelectedPointToolStripMenuItem
			// 
			this.playAtTheSelectedPointToolStripMenuItem.Checked = true;
			this.playAtTheSelectedPointToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.playAtTheSelectedPointToolStripMenuItem.Name = "playAtTheSelectedPointToolStripMenuItem";
			this.playAtTheSelectedPointToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
			this.playAtTheSelectedPointToolStripMenuItem.Text = "Play at the selected point";
			this.playAtTheSelectedPointToolStripMenuItem.Click += new System.EventHandler(this.playAtTheSelectedPointToolStripMenuItem_Click);
			// 
			// playOnlyTheSelectedRangeToolStripMenuItem
			// 
			this.playOnlyTheSelectedRangeToolStripMenuItem.Name = "playOnlyTheSelectedRangeToolStripMenuItem";
			this.playOnlyTheSelectedRangeToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
			this.playOnlyTheSelectedRangeToolStripMenuItem.Text = "Play only the selected range";
			this.playOnlyTheSelectedRangeToolStripMenuItem.Click += new System.EventHandler(this.playOnlyTheSelectedRangeToolStripMenuItem_Click);
			// 
			// toolStripButtonPause
			// 
			this.toolStripButtonPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPause.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPause.Name = "toolStripButtonPause";
			this.toolStripButtonPause.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPause.Text = "Pause (F7)";
			this.toolStripButtonPause.Click += new System.EventHandler(this.toolStripButtonPause_Click);
			// 
			// toolStripButtonStop
			// 
			this.toolStripButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonStop.Name = "toolStripButtonStop";
			this.toolStripButtonStop.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonStop.Text = "Stop playing (F8)";
			this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
			// 
			// toolStripLabelProgess
			// 
			this.toolStripLabelProgess.Name = "toolStripLabelProgess";
			this.toolStripLabelProgess.Size = new System.Drawing.Size(34, 22);
			this.toolStripLabelProgess.Text = "00:00";
			this.toolStripLabelProgess.Visible = false;
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonLoop
			// 
			this.toolStripButtonLoop.CheckOnClick = true;
			this.toolStripButtonLoop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonLoop.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonLoop.Name = "toolStripButtonLoop";
			this.toolStripButtonLoop.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonLoop.Text = "Loop this sequence";
			this.toolStripButtonLoop.CheckedChanged += new System.EventHandler(this.toolStripButtonLoop_CheckedChanged);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonPlaySpeedQuarter
			// 
			this.toolStripButtonPlaySpeedQuarter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPlaySpeedQuarter.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPlaySpeedQuarter.Name = "toolStripButtonPlaySpeedQuarter";
			this.toolStripButtonPlaySpeedQuarter.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPlaySpeedQuarter.Text = "Play at 1/4 of normal speed";
			this.toolStripButtonPlaySpeedQuarter.Click += new System.EventHandler(this.toolStripButtonPlaySpeedQuarter_Click);
			// 
			// toolStripButtonPlaySpeedHalf
			// 
			this.toolStripButtonPlaySpeedHalf.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPlaySpeedHalf.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPlaySpeedHalf.Name = "toolStripButtonPlaySpeedHalf";
			this.toolStripButtonPlaySpeedHalf.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPlaySpeedHalf.Text = "Play at 1/2 of normal speed";
			this.toolStripButtonPlaySpeedHalf.Click += new System.EventHandler(this.toolStripButtonPlaySpeedHalf_Click);
			// 
			// toolStripButtonPlaySpeedThreeQuarters
			// 
			this.toolStripButtonPlaySpeedThreeQuarters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPlaySpeedThreeQuarters.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPlaySpeedThreeQuarters.Name = "toolStripButtonPlaySpeedThreeQuarters";
			this.toolStripButtonPlaySpeedThreeQuarters.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPlaySpeedThreeQuarters.Text = "Play at 3/4 of normal speed";
			this.toolStripButtonPlaySpeedThreeQuarters.Click += new System.EventHandler(this.toolStripButtonPlaySpeedThreeQuarters_Click);
			// 
			// toolStripButtonPlaySpeedNormal
			// 
			this.toolStripButtonPlaySpeedNormal.Checked = true;
			this.toolStripButtonPlaySpeedNormal.CheckState = System.Windows.Forms.CheckState.Checked;
			this.toolStripButtonPlaySpeedNormal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPlaySpeedNormal.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPlaySpeedNormal.Name = "toolStripButtonPlaySpeedNormal";
			this.toolStripButtonPlaySpeedNormal.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPlaySpeedNormal.Text = "Play at normal speed";
			this.toolStripButtonPlaySpeedNormal.Click += new System.EventHandler(this.toolStripButtonPlaySpeedNormal_Click);
			// 
			// toolStripButtonPlaySpeedVariable
			// 
			this.toolStripButtonPlaySpeedVariable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPlaySpeedVariable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPlaySpeedVariable.Name = "toolStripButtonPlaySpeedVariable";
			this.toolStripButtonPlaySpeedVariable.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPlaySpeedVariable.Text = "Use a trackbar to adjust the playback speed";
			this.toolStripButtonPlaySpeedVariable.Click += new System.EventHandler(this.toolStripButtonPlaySpeedVariable_Click);
			// 
			// toolStripLabelIntensity
			// 
			this.toolStripLabelIntensity.AutoSize = false;
			this.toolStripLabelIntensity.Name = "toolStripLabelIntensity";
			this.toolStripLabelIntensity.Size = new System.Drawing.Size(75, 22);
			this.toolStripLabelIntensity.Visible = false;
			// 
			// toolStripEffect
			// 
			this.toolStripEffect.AllowItemReorder = true;
			this.toolStripEffect.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripEffect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOn,
            this.toolStripButtonOff,
            this.toolStripSeparator7,
            this.toolStripButtonRampOn,
            this.toolStripButtonRampOff,
            this.toolStripButtonPartialRampOn,
            this.toolStripButtonPartialRampOff,
            this.toolStripButtonToggleRamps,
            this.toolStripSeparator9,
            this.toolStripButtonIntensity,
            this.toolStripButtonMirrorVertical,
            this.toolStripButtonMirrorHorizontal,
            this.toolStripButtonInvert,
            this.toolStripButtonRandom,
            this.toolStripButtonSparkle,
            this.toolStripButtonShimmerDimming,
            this.toolStripSeparator16,
            this.toolStripButtonToggleLevels,
            this.toolStripButtonToggleCellText,
            this.toolStripButtonChangeIntensity,
            this.toolStripLabelCurrentIntensity});
			this.toolStripEffect.Location = new System.Drawing.Point(3, 50);
			this.toolStripEffect.Name = "toolStripEffect";
			this.toolStripEffect.Size = new System.Drawing.Size(421, 25);
			this.toolStripEffect.TabIndex = 3;
			this.toolStripEffect.Text = "Effects";
			// 
			// toolStripButtonOn
			// 
			this.toolStripButtonOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonOn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonOn.Name = "toolStripButtonOn";
			this.toolStripButtonOn.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonOn.Text = "On";
			this.toolStripButtonOn.Click += new System.EventHandler(this.toolStripButtonOn_Click);
			// 
			// toolStripButtonOff
			// 
			this.toolStripButtonOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonOff.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonOff.Name = "toolStripButtonOff";
			this.toolStripButtonOff.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonOff.Text = "Off";
			this.toolStripButtonOff.Click += new System.EventHandler(this.toolStripButtonOff_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonRampOn
			// 
			this.toolStripButtonRampOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRampOn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonRampOn.Name = "toolStripButtonRampOn";
			this.toolStripButtonRampOn.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonRampOn.Text = "Ramp on (R)";
			this.toolStripButtonRampOn.Click += new System.EventHandler(this.toolStripButtonRampOn_Click);
			// 
			// toolStripButtonRampOff
			// 
			this.toolStripButtonRampOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRampOff.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonRampOff.Name = "toolStripButtonRampOff";
			this.toolStripButtonRampOff.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonRampOff.Text = "Ramp off (F)";
			this.toolStripButtonRampOff.Click += new System.EventHandler(this.toolStripButtonRampOff_Click);
			// 
			// toolStripButtonPartialRampOn
			// 
			this.toolStripButtonPartialRampOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPartialRampOn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPartialRampOn.Name = "toolStripButtonPartialRampOn";
			this.toolStripButtonPartialRampOn.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPartialRampOn.Text = "Partial ramp on (Shift-R)";
			this.toolStripButtonPartialRampOn.Click += new System.EventHandler(this.toolStripButtonPartialRampOn_Click);
			// 
			// toolStripButtonPartialRampOff
			// 
			this.toolStripButtonPartialRampOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPartialRampOff.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonPartialRampOff.Name = "toolStripButtonPartialRampOff";
			this.toolStripButtonPartialRampOff.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonPartialRampOff.Text = "Partial ramp off (Shift-F)";
			this.toolStripButtonPartialRampOff.Click += new System.EventHandler(this.toolStripButtonPartialRampOff_Click);
			// 
			// toolStripButtonToggleRamps
			// 
			this.toolStripButtonToggleRamps.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonToggleRamps.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonToggleRamps.Name = "toolStripButtonToggleRamps";
			this.toolStripButtonToggleRamps.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonToggleRamps.Text = "Toggle between gradient and bar ramps";
			this.toolStripButtonToggleRamps.Click += new System.EventHandler(this.toolStripButtonToggleRamps_Click);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonIntensity
			// 
			this.toolStripButtonIntensity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonIntensity.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonIntensity.Name = "toolStripButtonIntensity";
			this.toolStripButtonIntensity.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonIntensity.Text = "Intensity (I)";
			this.toolStripButtonIntensity.Click += new System.EventHandler(this.toolStripButtonIntensity_Click);
			// 
			// toolStripButtonMirrorVertical
			// 
			this.toolStripButtonMirrorVertical.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonMirrorVertical.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonMirrorVertical.Name = "toolStripButtonMirrorVertical";
			this.toolStripButtonMirrorVertical.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonMirrorVertical.Text = "Mirror vertically (V)";
			this.toolStripButtonMirrorVertical.Click += new System.EventHandler(this.toolStripButtonMirrorVertical_Click);
			// 
			// toolStripButtonMirrorHorizontal
			// 
			this.toolStripButtonMirrorHorizontal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonMirrorHorizontal.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonMirrorHorizontal.Name = "toolStripButtonMirrorHorizontal";
			this.toolStripButtonMirrorHorizontal.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonMirrorHorizontal.Text = "Mirror horizontally (H)";
			this.toolStripButtonMirrorHorizontal.Click += new System.EventHandler(this.toolStripButtonMirrorHorizontal_Click);
			// 
			// toolStripButtonInvert
			// 
			this.toolStripButtonInvert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonInvert.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonInvert.Name = "toolStripButtonInvert";
			this.toolStripButtonInvert.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonInvert.Text = "Invert (T)";
			this.toolStripButtonInvert.Click += new System.EventHandler(this.toolStripButtonInvert_Click);
			// 
			// toolStripButtonRandom
			// 
			this.toolStripButtonRandom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRandom.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonRandom.Name = "toolStripButtonRandom";
			this.toolStripButtonRandom.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonRandom.Text = "Random (A)";
			this.toolStripButtonRandom.Click += new System.EventHandler(this.toolStripButtonRandom_Click);
			// 
			// toolStripButtonSparkle
			// 
			this.toolStripButtonSparkle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSparkle.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSparkle.Name = "toolStripButtonSparkle";
			this.toolStripButtonSparkle.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonSparkle.Text = "Sparkle (S)";
			this.toolStripButtonSparkle.Click += new System.EventHandler(this.toolStripButtonSparkle_Click);
			// 
			// toolStripButtonShimmerDimming
			// 
			this.toolStripButtonShimmerDimming.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonShimmerDimming.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonShimmerDimming.Name = "toolStripButtonShimmerDimming";
			this.toolStripButtonShimmerDimming.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonShimmerDimming.Text = "Shimmer (E)";
			this.toolStripButtonShimmerDimming.Click += new System.EventHandler(this.toolStripButtonShimmerDimming_Click);
			// 
			// toolStripSeparator16
			// 
			this.toolStripSeparator16.Name = "toolStripSeparator16";
			this.toolStripSeparator16.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonToggleLevels
			// 
			this.toolStripButtonToggleLevels.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonToggleLevels.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonToggleLevels.Name = "toolStripButtonToggleLevels";
			this.toolStripButtonToggleLevels.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonToggleLevels.Text = "Show actual intensity levels (0-255)";
			this.toolStripButtonToggleLevels.Click += new System.EventHandler(this.toolStripButtonToggleLevels_Click);
			// 
			// toolStripButtonToggleCellText
			// 
			this.toolStripButtonToggleCellText.CheckOnClick = true;
			this.toolStripButtonToggleCellText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonToggleCellText.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonToggleCellText.Name = "toolStripButtonToggleCellText";
			this.toolStripButtonToggleCellText.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonToggleCellText.Text = "Display/hide each cell\'s intensity value within the cell";
			this.toolStripButtonToggleCellText.Click += new System.EventHandler(this.toolStripButtonToggleCellText_Click);
			// 
			// toolStripButtonChangeIntensity
			// 
			this.toolStripButtonChangeIntensity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonChangeIntensity.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonChangeIntensity.Name = "toolStripButtonChangeIntensity";
			this.toolStripButtonChangeIntensity.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonChangeIntensity.Text = "Change drawing intensity";
			this.toolStripButtonChangeIntensity.Click += new System.EventHandler(this.toolStripButtonChangeIntensity_Click);
			// 
			// toolStripLabelCurrentIntensity
			// 
			this.toolStripLabelCurrentIntensity.Name = "toolStripLabelCurrentIntensity";
			this.toolStripLabelCurrentIntensity.Size = new System.Drawing.Size(146, 22);
			this.toolStripLabelCurrentIntensity.Text = "Currently drawing at 100%";
			this.toolStripLabelCurrentIntensity.Visible = false;
			// 
			// toolStripEditing
			// 
			this.toolStripEditing.AllowItemReorder = true;
			this.toolStripEditing.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripEditing.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonCut,
            this.toolStripButtonCopy,
            this.toolStripButtonOpaquePaste,
            this.toolStripButtonTransparentPaste,
            this.toolStripSplitButtonBooleanPaste,
            this.toolStripSplitButtonArithmeticPaste,
            this.toolStripButtonInsertPaste,
            this.toolStripButtonRemoveCells,
            this.toolStripSeparator2,
            this.toolStripButtonFindAndReplace,
            this.toolStripSeparator15,
            this.toolStripButtonUndo,
            this.toolStripButtonRedo});
			this.toolStripEditing.Location = new System.Drawing.Point(3, 75);
			this.toolStripEditing.Name = "toolStripEditing";
			this.toolStripEditing.Size = new System.Drawing.Size(263, 25);
			this.toolStripEditing.TabIndex = 6;
			this.toolStripEditing.Text = "Editing";
			// 
			// toolStripButtonCut
			// 
			this.toolStripButtonCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonCut.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonCut.Name = "toolStripButtonCut";
			this.toolStripButtonCut.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonCut.Text = "Cut";
			this.toolStripButtonCut.Click += new System.EventHandler(this.toolStripButtonCut_Click);
			// 
			// toolStripButtonCopy
			// 
			this.toolStripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonCopy.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonCopy.Name = "toolStripButtonCopy";
			this.toolStripButtonCopy.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonCopy.Text = "Copy";
			this.toolStripButtonCopy.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
			// 
			// toolStripButtonOpaquePaste
			// 
			this.toolStripButtonOpaquePaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonOpaquePaste.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonOpaquePaste.Name = "toolStripButtonOpaquePaste";
			this.toolStripButtonOpaquePaste.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonOpaquePaste.Text = "Opaque paste";
			this.toolStripButtonOpaquePaste.Click += new System.EventHandler(this.toolStripButtonOpaquePaste_Click);
			// 
			// toolStripButtonTransparentPaste
			// 
			this.toolStripButtonTransparentPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonTransparentPaste.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonTransparentPaste.Name = "toolStripButtonTransparentPaste";
			this.toolStripButtonTransparentPaste.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonTransparentPaste.Text = "Transparent paste";
			this.toolStripButtonTransparentPaste.Click += new System.EventHandler(this.toolStripButtonTransparentPaste_Click);
			// 
			// toolStripSplitButtonBooleanPaste
			// 
			this.toolStripSplitButtonBooleanPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSplitButtonBooleanPaste.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemPasteOr,
            this.toolStripMenuItemPasteAnd,
            this.toolStripMenuItemPasteXor,
            this.toolStripSeparator14,
            this.toolStripMenuItemPasteNor,
            this.toolStripMenuItemPasteNand,
            this.toolStripMenuItemPasteXnor});
			this.toolStripSplitButtonBooleanPaste.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripSplitButtonBooleanPaste.Name = "toolStripSplitButtonBooleanPaste";
			this.toolStripSplitButtonBooleanPaste.Size = new System.Drawing.Size(16, 22);
			this.toolStripSplitButtonBooleanPaste.Text = "Boolean paste";
			// 
			// toolStripMenuItemPasteOr
			// 
			this.toolStripMenuItemPasteOr.Name = "toolStripMenuItemPasteOr";
			this.toolStripMenuItemPasteOr.Size = new System.Drawing.Size(172, 22);
			this.toolStripMenuItemPasteOr.Text = "OR";
			this.toolStripMenuItemPasteOr.ToolTipText = "OR";
			this.toolStripMenuItemPasteOr.Click += new System.EventHandler(this.toolStripMenuItemPasteOr_Click);
			// 
			// toolStripMenuItemPasteAnd
			// 
			this.toolStripMenuItemPasteAnd.Name = "toolStripMenuItemPasteAnd";
			this.toolStripMenuItemPasteAnd.Size = new System.Drawing.Size(172, 22);
			this.toolStripMenuItemPasteAnd.Text = "AND";
			this.toolStripMenuItemPasteAnd.ToolTipText = "AND";
			this.toolStripMenuItemPasteAnd.Click += new System.EventHandler(this.toolStripMenuItemPasteAnd_Click);
			// 
			// toolStripMenuItemPasteXor
			// 
			this.toolStripMenuItemPasteXor.Name = "toolStripMenuItemPasteXor";
			this.toolStripMenuItemPasteXor.Size = new System.Drawing.Size(172, 22);
			this.toolStripMenuItemPasteXor.Text = "XOR";
			this.toolStripMenuItemPasteXor.ToolTipText = "XOR";
			this.toolStripMenuItemPasteXor.Click += new System.EventHandler(this.toolStripMenuItemPasteXor_Click);
			// 
			// toolStripSeparator14
			// 
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			this.toolStripSeparator14.Size = new System.Drawing.Size(169, 6);
			// 
			// toolStripMenuItemPasteNor
			// 
			this.toolStripMenuItemPasteNor.Name = "toolStripMenuItemPasteNor";
			this.toolStripMenuItemPasteNor.Size = new System.Drawing.Size(172, 22);
			this.toolStripMenuItemPasteNor.Text = "NOR (NOT OR)";
			this.toolStripMenuItemPasteNor.ToolTipText = "NOR (NOT OR)";
			this.toolStripMenuItemPasteNor.Click += new System.EventHandler(this.toolStripMenuItemPasteNor_Click);
			// 
			// toolStripMenuItemPasteNand
			// 
			this.toolStripMenuItemPasteNand.Name = "toolStripMenuItemPasteNand";
			this.toolStripMenuItemPasteNand.Size = new System.Drawing.Size(172, 22);
			this.toolStripMenuItemPasteNand.Text = "NAND (NOT AND)";
			this.toolStripMenuItemPasteNand.ToolTipText = "NAND (NOT AND)";
			this.toolStripMenuItemPasteNand.Click += new System.EventHandler(this.toolStripMenuItemPasteNand_Click);
			// 
			// toolStripMenuItemPasteXnor
			// 
			this.toolStripMenuItemPasteXnor.Name = "toolStripMenuItemPasteXnor";
			this.toolStripMenuItemPasteXnor.Size = new System.Drawing.Size(172, 22);
			this.toolStripMenuItemPasteXnor.Text = "XNOR (NOT XOR)";
			this.toolStripMenuItemPasteXnor.ToolTipText = "XNOR (NOT XOR)";
			this.toolStripMenuItemPasteXnor.Click += new System.EventHandler(this.toolStripMenuItemPasteXnor_Click);
			// 
			// toolStripSplitButtonArithmeticPaste
			// 
			this.toolStripSplitButtonArithmeticPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSplitButtonArithmeticPaste.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.additionToolStripMenuItem,
            this.subtractionToolStripMenuItem,
            this.scaleToolStripMenuItem,
            this.minToolStripMenuItem,
            this.maxToolStripMenuItem});
			this.toolStripSplitButtonArithmeticPaste.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripSplitButtonArithmeticPaste.Name = "toolStripSplitButtonArithmeticPaste";
			this.toolStripSplitButtonArithmeticPaste.Size = new System.Drawing.Size(16, 22);
			this.toolStripSplitButtonArithmeticPaste.Text = "Arithmetic paste";
			// 
			// additionToolStripMenuItem
			// 
			this.additionToolStripMenuItem.Name = "additionToolStripMenuItem";
			this.additionToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.additionToolStripMenuItem.Text = "Addition";
			this.additionToolStripMenuItem.ToolTipText = "Pasted values are added to destination values";
			this.additionToolStripMenuItem.Click += new System.EventHandler(this.additionToolStripMenuItem_Click);
			// 
			// subtractionToolStripMenuItem
			// 
			this.subtractionToolStripMenuItem.Name = "subtractionToolStripMenuItem";
			this.subtractionToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.subtractionToolStripMenuItem.Text = "Subtraction";
			this.subtractionToolStripMenuItem.ToolTipText = "Pasted values are subtracted from destination values";
			this.subtractionToolStripMenuItem.Click += new System.EventHandler(this.subtractionToolStripMenuItem_Click);
			// 
			// scaleToolStripMenuItem
			// 
			this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
			this.scaleToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.scaleToolStripMenuItem.Text = "Scale";
			this.scaleToolStripMenuItem.ToolTipText = "Pasted values are used to scale the destination values";
			this.scaleToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
			// 
			// minToolStripMenuItem
			// 
			this.minToolStripMenuItem.Name = "minToolStripMenuItem";
			this.minToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.minToolStripMenuItem.Text = "Min";
			this.minToolStripMenuItem.ToolTipText = "Results in the lowest of the pasted and destination values";
			this.minToolStripMenuItem.Click += new System.EventHandler(this.minToolStripMenuItem_Click);
			// 
			// maxToolStripMenuItem
			// 
			this.maxToolStripMenuItem.Name = "maxToolStripMenuItem";
			this.maxToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.maxToolStripMenuItem.Text = "Max";
			this.maxToolStripMenuItem.ToolTipText = "Results in the highest of the pasted and destination values";
			this.maxToolStripMenuItem.Click += new System.EventHandler(this.maxToolStripMenuItem_Click);
			// 
			// toolStripButtonInsertPaste
			// 
			this.toolStripButtonInsertPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonInsertPaste.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonInsertPaste.Name = "toolStripButtonInsertPaste";
			this.toolStripButtonInsertPaste.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonInsertPaste.Text = "Insert paste";
			this.toolStripButtonInsertPaste.Click += new System.EventHandler(this.toolStripButtonInsertPaste_Click);
			// 
			// toolStripButtonRemoveCells
			// 
			this.toolStripButtonRemoveCells.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRemoveCells.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonRemoveCells.Name = "toolStripButtonRemoveCells";
			this.toolStripButtonRemoveCells.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonRemoveCells.Text = "Remove cells";
			this.toolStripButtonRemoveCells.Click += new System.EventHandler(this.toolStripButtonRemoveCells_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonFindAndReplace
			// 
			this.toolStripButtonFindAndReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonFindAndReplace.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonFindAndReplace.Name = "toolStripButtonFindAndReplace";
			this.toolStripButtonFindAndReplace.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonFindAndReplace.Text = "Find and replace";
			this.toolStripButtonFindAndReplace.Click += new System.EventHandler(this.toolStripButtonFindAndReplace_Click);
			// 
			// toolStripSeparator15
			// 
			this.toolStripSeparator15.Name = "toolStripSeparator15";
			this.toolStripSeparator15.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonUndo
			// 
			this.toolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonUndo.Enabled = false;
			this.toolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonUndo.Name = "toolStripButtonUndo";
			this.toolStripButtonUndo.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonUndo.Text = "Undo";
			this.toolStripButtonUndo.ToolTipText = "Undo";
			this.toolStripButtonUndo.Click += new System.EventHandler(this.toolStripButtonUndo_Click);
			// 
			// toolStripButtonRedo
			// 
			this.toolStripButtonRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRedo.Enabled = false;
			this.toolStripButtonRedo.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonRedo.Name = "toolStripButtonRedo";
			this.toolStripButtonRedo.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonRedo.Text = "Redo";
			this.toolStripButtonRedo.ToolTipText = "Redo";
			this.toolStripButtonRedo.Click += new System.EventHandler(this.toolStripButtonRedo_Click);
			// 
			// toolStripText
			// 
			this.toolStripText.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripText.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.toolStripLabelExecutionPoint,
            this.toolStripSeparator11,
            this.toolStripLabel10,
            this.toolStripLabelCurrentDrawingIntensity,
            this.toolStripSeparator18,
            this.toolStripLabel8,
            this.toolStripLabelCellIntensity,
            this.toolStripSeparator17,
            this.toolStripLabelCurrentCell});
			this.toolStripText.Location = new System.Drawing.Point(3, 100);
			this.toolStripText.Name = "toolStripText";
			this.toolStripText.Size = new System.Drawing.Size(737, 25);
			this.toolStripText.TabIndex = 7;
			this.toolStripText.Text = "Text";
			// 
			// toolStripLabel6
			// 
			this.toolStripLabel6.Name = "toolStripLabel6";
			this.toolStripLabel6.Size = new System.Drawing.Size(92, 22);
			this.toolStripLabel6.Text = "Execution point:";
			// 
			// toolStripLabelExecutionPoint
			// 
			this.toolStripLabelExecutionPoint.AutoSize = false;
			this.toolStripLabelExecutionPoint.Name = "toolStripLabelExecutionPoint";
			this.toolStripLabelExecutionPoint.Size = new System.Drawing.Size(86, 22);
			this.toolStripLabelExecutionPoint.Text = "00:00";
			this.toolStripLabelExecutionPoint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel10
			// 
			this.toolStripLabel10.Name = "toolStripLabel10";
			this.toolStripLabel10.Size = new System.Drawing.Size(115, 22);
			this.toolStripLabel10.Text = "Currently drawing at";
			// 
			// toolStripLabelCurrentDrawingIntensity
			// 
			this.toolStripLabelCurrentDrawingIntensity.AutoSize = false;
			this.toolStripLabelCurrentDrawingIntensity.Name = "toolStripLabelCurrentDrawingIntensity";
			this.toolStripLabelCurrentDrawingIntensity.Size = new System.Drawing.Size(86, 22);
			this.toolStripLabelCurrentDrawingIntensity.Text = "100%";
			this.toolStripLabelCurrentDrawingIntensity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripSeparator18
			// 
			this.toolStripSeparator18.Name = "toolStripSeparator18";
			this.toolStripSeparator18.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel8
			// 
			this.toolStripLabel8.Name = "toolStripLabel8";
			this.toolStripLabel8.Size = new System.Drawing.Size(78, 22);
			this.toolStripLabel8.Text = "Cell intensity:";
			// 
			// toolStripLabelCellIntensity
			// 
			this.toolStripLabelCellIntensity.AutoSize = false;
			this.toolStripLabelCellIntensity.Name = "toolStripLabelCellIntensity";
			this.toolStripLabelCellIntensity.Size = new System.Drawing.Size(86, 22);
			this.toolStripLabelCellIntensity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripSeparator17
			// 
			this.toolStripSeparator17.Name = "toolStripSeparator17";
			this.toolStripSeparator17.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabelCurrentCell
			// 
			this.toolStripLabelCurrentCell.AutoSize = false;
			this.toolStripLabelCurrentCell.Name = "toolStripLabelCurrentCell";
			this.toolStripLabelCurrentCell.Size = new System.Drawing.Size(200, 15);
			this.toolStripLabelCurrentCell.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripDisplaySettings
			// 
			this.toolStripDisplaySettings.AllowItemReorder = true;
			this.toolStripDisplaySettings.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripDisplaySettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonToggleCrossHairs,
            this.toolStripLabel4,
            this.toolStripComboBoxColumnZoom,
            this.toolStripLabel5,
            this.toolStripComboBoxRowZoom,
            this.toolStripSeparator6,
            this.toolStripLabel3,
            this.toolStripComboBoxChannelOrder,
            this.toolStripButtonSaveOrder,
            this.toolStripButtonDeleteOrder});
			this.toolStripDisplaySettings.Location = new System.Drawing.Point(3, 150);
			this.toolStripDisplaySettings.Name = "toolStripDisplaySettings";
			this.toolStripDisplaySettings.Size = new System.Drawing.Size(531, 25);
			this.toolStripDisplaySettings.TabIndex = 5;
			this.toolStripDisplaySettings.Text = "Display settings";
			// 
			// toolStripButtonToggleCrossHairs
			// 
			this.toolStripButtonToggleCrossHairs.CheckOnClick = true;
			this.toolStripButtonToggleCrossHairs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonToggleCrossHairs.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonToggleCrossHairs.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.toolStripButtonToggleCrossHairs.MergeIndex = 0;
			this.toolStripButtonToggleCrossHairs.Name = "toolStripButtonToggleCrossHairs";
			this.toolStripButtonToggleCrossHairs.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonToggleCrossHairs.Text = "Toggle cross-hairs";
			this.toolStripButtonToggleCrossHairs.Click += new System.EventHandler(this.toolStripButtonToggleCrossHairs_Click);
			// 
			// toolStripLabel4
			// 
			this.toolStripLabel4.Name = "toolStripLabel4";
			this.toolStripLabel4.Size = new System.Drawing.Size(83, 22);
			this.toolStripLabel4.Text = "Column zoom";
			// 
			// toolStripComboBoxColumnZoom
			// 
			this.toolStripComboBoxColumnZoom.AutoSize = false;
			this.toolStripComboBoxColumnZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripComboBoxColumnZoom.DropDownWidth = 55;
			this.toolStripComboBoxColumnZoom.Items.AddRange(new object[] {
            "10%",
            "20%",
            "30%",
            "40%",
            "50%",
            "60%",
            "70%",
            "80%",
            "90%",
            "100%"});
			this.toolStripComboBoxColumnZoom.Name = "toolStripComboBoxColumnZoom";
			this.toolStripComboBoxColumnZoom.Size = new System.Drawing.Size(55, 23);
			this.toolStripComboBoxColumnZoom.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxColumnZoom_SelectedIndexChanged);
			// 
			// toolStripLabel5
			// 
			this.toolStripLabel5.Name = "toolStripLabel5";
			this.toolStripLabel5.Size = new System.Drawing.Size(63, 22);
			this.toolStripLabel5.Text = "Row zoom";
			// 
			// toolStripComboBoxRowZoom
			// 
			this.toolStripComboBoxRowZoom.AutoSize = false;
			this.toolStripComboBoxRowZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripComboBoxRowZoom.DropDownWidth = 55;
			this.toolStripComboBoxRowZoom.Items.AddRange(new object[] {
            "10%",
            "20%",
            "30%",
            "40%",
            "50%",
            "60%",
            "70%",
            "80%",
            "90%",
            "100%"});
			this.toolStripComboBoxRowZoom.Name = "toolStripComboBoxRowZoom";
			this.toolStripComboBoxRowZoom.Size = new System.Drawing.Size(55, 23);
			this.toolStripComboBoxRowZoom.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxRowZoom_SelectedIndexChanged);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel3
			// 
			this.toolStripLabel3.Name = "toolStripLabel3";
			this.toolStripLabel3.Size = new System.Drawing.Size(82, 22);
			this.toolStripLabel3.Text = "Channel order";
			// 
			// toolStripComboBoxChannelOrder
			// 
			this.toolStripComboBoxChannelOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripComboBoxChannelOrder.DropDownWidth = 120;
			this.toolStripComboBoxChannelOrder.Items.AddRange(new object[] {
            "Define new order...",
            "Restore natural order..."});
			this.toolStripComboBoxChannelOrder.Name = "toolStripComboBoxChannelOrder";
			this.toolStripComboBoxChannelOrder.Size = new System.Drawing.Size(100, 25);
			this.toolStripComboBoxChannelOrder.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxChannelOrder_SelectedIndexChanged);
			// 
			// toolStripButtonSaveOrder
			// 
			this.toolStripButtonSaveOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSaveOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSaveOrder.Name = "toolStripButtonSaveOrder";
			this.toolStripButtonSaveOrder.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonSaveOrder.Text = "Save the current channel order";
			this.toolStripButtonSaveOrder.ToolTipText = "Save the current channel order";
			this.toolStripButtonSaveOrder.Click += new System.EventHandler(this.toolStripButtonSaveOrder_Click);
			// 
			// toolStripButtonDeleteOrder
			// 
			this.toolStripButtonDeleteOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonDeleteOrder.Enabled = false;
			this.toolStripButtonDeleteOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonDeleteOrder.Name = "toolStripButtonDeleteOrder";
			this.toolStripButtonDeleteOrder.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonDeleteOrder.Text = "Delete the current channel order";
			this.toolStripButtonDeleteOrder.Click += new System.EventHandler(this.toolStripButtonDeleteOrder_Click);
			// 
			// toolStripVisualizer
			// 
			this.toolStripVisualizer.AllowItemReorder = true;
			this.toolStripVisualizer.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripVisualizer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonWaveform,
            this.toolStripLabelWaveformZoom,
            this.toolStripComboBoxWaveformZoom});
			this.toolStripVisualizer.Location = new System.Drawing.Point(3, 125);
			this.toolStripVisualizer.Name = "toolStripVisualizer";
			this.toolStripVisualizer.Size = new System.Drawing.Size(181, 25);
			this.toolStripVisualizer.TabIndex = 4;
			this.toolStripVisualizer.Text = "Audio visualizer";
			// 
			// toolStripButtonWaveform
			// 
			this.toolStripButtonWaveform.CheckOnClick = true;
			this.toolStripButtonWaveform.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonWaveform.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripButtonWaveform.Name = "toolStripButtonWaveform";
			this.toolStripButtonWaveform.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonWaveform.Text = "Audio visualizer";
			this.toolStripButtonWaveform.Click += new System.EventHandler(this.toolStripButtonWaveform_Click);
			// 
			// toolStripLabelWaveformZoom
			// 
			this.toolStripLabelWaveformZoom.Enabled = false;
			this.toolStripLabelWaveformZoom.Name = "toolStripLabelWaveformZoom";
			this.toolStripLabelWaveformZoom.Size = new System.Drawing.Size(89, 22);
			this.toolStripLabelWaveformZoom.Text = "Visualizer zoom";
			// 
			// toolStripComboBoxWaveformZoom
			// 
			this.toolStripComboBoxWaveformZoom.AutoSize = false;
			this.toolStripComboBoxWaveformZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripComboBoxWaveformZoom.DropDownWidth = 55;
			this.toolStripComboBoxWaveformZoom.Enabled = false;
			this.toolStripComboBoxWaveformZoom.Items.AddRange(new object[] {
            "10%",
            "20%",
            "30%",
            "40%",
            "50%",
            "60%",
            "70%",
            "80%",
            "90%",
            "100%",
            "110%",
            "120%",
            "130%",
            "140%",
            "150%",
            "160%",
            "170%",
            "180%",
            "190%",
            "200%",
            "210%",
            "220%",
            "230%",
            "240%",
            "250%",
            "260%",
            "270%",
            "280%",
            "290%",
            "300%",
            "310%",
            "320%",
            "330%",
            "340%",
            "350%",
            "360%",
            "370%",
            "380%",
            "390%",
            "400%"});
			this.toolStripComboBoxWaveformZoom.MaxDropDownItems = 10;
			this.toolStripComboBoxWaveformZoom.Name = "toolStripComboBoxWaveformZoom";
			this.toolStripComboBoxWaveformZoom.Size = new System.Drawing.Size(55, 23);
			this.toolStripComboBoxWaveformZoom.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxWaveformZoom_SelectedIndexChanged);
			// 
			// colorDialog1
			// 
			this.colorDialog1.AnyColor = true;
			this.colorDialog1.FullOpen = true;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "vix";
			this.saveFileDialog.Filter = "Vixen Event Sequence | *.vix";
			this.saveFileDialog.Title = "Save As";
			// 
			// printDocument
			// 
			this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
			// 
			// printDialog
			// 
			this.printDialog.AllowPrintToFile = false;
			this.printDialog.Document = this.printDocument;
			this.printDialog.UseEXDialog = true;
			// 
			// printPreviewDialog
			// 
			this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
			this.printPreviewDialog.Document = this.printDocument;
			this.printPreviewDialog.Enabled = true;
			this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
			this.printPreviewDialog.Name = "printPreviewDialog";
			this.printPreviewDialog.Visible = false;
			// 
			// m_positionTimer
			// 
			this.m_positionTimer.Interval = 1;
			this.m_positionTimer.Tick += new System.EventHandler(this.m_positionTimer_Tick);
			// 
			// StandardSequence
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(740, 444);
			this.Controls.Add(this.toolStripContainer1);
			this.Controls.Add(this.menuStrip);
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip;
			this.Name = "StandardSequence";
			this.Activated += new System.EventHandler(this.StandardSequence_Activated);
			this.Deactivate += new System.EventHandler(this.StandardSequence_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StandardSequence_FormClosing);
			this.Load += new System.EventHandler(this.StandardSequence_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StandardSequence_KeyDown);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLevelNumber)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLevelPercent)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutputArrow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxChannels)).EndInit();
			this.contextMenuChannels.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxGrid)).EndInit();
			this.contextMenuGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxTime)).EndInit();
			this.contextMenuTime.ResumeLayout(false);
			this.toolStripSequenceSettings.ResumeLayout(false);
			this.toolStripSequenceSettings.PerformLayout();
			this.toolStripExecutionControl.ResumeLayout(false);
			this.toolStripExecutionControl.PerformLayout();
			this.toolStripEffect.ResumeLayout(false);
			this.toolStripEffect.PerformLayout();
			this.toolStripEditing.ResumeLayout(false);
			this.toolStripEditing.PerformLayout();
			this.toolStripText.ResumeLayout(false);
			this.toolStripText.PerformLayout();
			this.toolStripDisplaySettings.ResumeLayout(false);
			this.toolStripDisplaySettings.PerformLayout();
			this.toolStripVisualizer.ResumeLayout(false);
			this.toolStripVisualizer.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			if (this.m_channelBackBrush != null)
			{
				this.m_channelBackBrush.Dispose();
			}
			if (this.m_timeBackBrush != null)
			{
				this.m_timeBackBrush.Dispose();
			}
			if (this.m_gridBackBrush != null)
			{
				this.m_gridBackBrush.Dispose();
			}
			this.m_channelNameFont.Dispose();
			this.m_timeFont.Dispose();
			this.m_selectionBrush.Dispose();
			this.m_positionBrush.Dispose();
			this.m_channelCaretBrush.Dispose();
			if (this.m_arrowBitmap != null)
			{
				this.m_arrowBitmap.Dispose();
			}
			if (this.m_gridGraphics != null)
			{
				this.m_gridGraphics.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
