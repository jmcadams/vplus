using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nutcracker
{
    sealed partial class NutcrackerControlDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(NutcrackerControlDialog));
            this.gbEffect2 = new GroupBox();
            this.gbEffect1 = new GroupBox();
            this.pbPreview = new PictureBox();
            this.gbSettings = new GroupBox();
            this.gbModel = new GroupBox();
            this.btnModelEdit = new Button();
            this.btnModelRemove = new Button();
            this.tbSummary = new TextBox();
            this.progressBar = new ProgressBar();
            this.lblRenderInfo = new Label();
            this.cbModels = new ComboBox();
            this.lblPresets = new Label();
            this.cbEffectsPresets = new ComboBox();
            this.tbSparkles = new TrackBar();
            this.lblSparkles = new Label();
            this.lblStatsMs = new Label();
            this.gbLayer = new GroupBox();
            this.rbAverage = new RadioButton();
            this.rbLayer = new RadioButton();
            this.rbUnmask2 = new RadioButton();
            this.rbUnmask1 = new RadioButton();
            this.rbMask2 = new RadioButton();
            this.rbMask1 = new RadioButton();
            this.rbEffect2 = new RadioButton();
            this.rbEffect1 = new RadioButton();
            this.btnOK = new Button();
            this.btnPlayStop = new Button();
            this.lblStatsFps = new Label();
            this.gbRenderTo = new GroupBox();
            this.cbChannels = new ComboBox();
            this.lblChannels = new Label();
            this.lblStartEventTime = new Label();
            this.nudStartEvent = new NumericUpDown();
            this.lblStartEvent = new Label();
            this.lblEventCountTime = new Label();
            this.nudEventCount = new NumericUpDown();
            this.lblEventCount = new Label();
            this.rbSpecificPoint = new RadioButton();
            this.rbCurrentSelection = new RadioButton();
            this.rbRoutine = new RadioButton();
            this.rbClipboard = new RadioButton();
            this.chkBoxEnableRawPreview = new CheckBox();
            this.pbRawPreview = new PictureBox();
            this.btnLightsOff = new Button();
            this.cbRender = new CheckBox();
            this.btnCancel = new Button();
            this.timerRender = new Timer(this.components);
            this.toolTip = new ToolTip(this.components);
            this.nutcrackerEffectControl1 = new NutcrackerEffectControl();
            this.nutcrackerEffectControl2 = new NutcrackerEffectControl();
            this.gbEffect2.SuspendLayout();
            this.gbEffect1.SuspendLayout();
            ((ISupportInitialize)(this.pbPreview)).BeginInit();
            this.gbSettings.SuspendLayout();
            this.gbModel.SuspendLayout();
            ((ISupportInitialize)(this.tbSparkles)).BeginInit();
            this.gbLayer.SuspendLayout();
            this.gbRenderTo.SuspendLayout();
            ((ISupportInitialize)(this.nudStartEvent)).BeginInit();
            ((ISupportInitialize)(this.nudEventCount)).BeginInit();
            ((ISupportInitialize)(this.pbRawPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // gbEffect2
            // 
            this.gbEffect2.Controls.Add(this.nutcrackerEffectControl2);
            this.gbEffect2.Location = new Point(788, 304);
            this.gbEffect2.Name = "gbEffect2";
            this.gbEffect2.Size = new Size(384, 250);
            this.gbEffect2.TabIndex = 2;
            this.gbEffect2.TabStop = false;
            this.gbEffect2.Text = "Effect 2";
            // 
            // gbEffect1
            // 
            this.gbEffect1.Controls.Add(this.nutcrackerEffectControl1);
            this.gbEffect1.Location = new Point(398, 304);
            this.gbEffect1.Name = "gbEffect1";
            this.gbEffect1.Size = new Size(384, 250);
            this.gbEffect1.TabIndex = 1;
            this.gbEffect1.TabStop = false;
            this.gbEffect1.Text = "Effect 1";
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = Color.Black;
            this.pbPreview.Location = new Point(12, 12);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new Size(380, 538);
            this.pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbPreview.TabIndex = 2;
            this.pbPreview.TabStop = false;
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.gbModel);
            this.gbSettings.Controls.Add(this.lblPresets);
            this.gbSettings.Controls.Add(this.cbEffectsPresets);
            this.gbSettings.Controls.Add(this.tbSparkles);
            this.gbSettings.Controls.Add(this.lblSparkles);
            this.gbSettings.Controls.Add(this.lblStatsMs);
            this.gbSettings.Controls.Add(this.gbLayer);
            this.gbSettings.Controls.Add(this.btnOK);
            this.gbSettings.Controls.Add(this.btnPlayStop);
            this.gbSettings.Controls.Add(this.lblStatsFps);
            this.gbSettings.Controls.Add(this.gbRenderTo);
            this.gbSettings.Controls.Add(this.chkBoxEnableRawPreview);
            this.gbSettings.Controls.Add(this.pbRawPreview);
            this.gbSettings.Controls.Add(this.btnLightsOff);
            this.gbSettings.Controls.Add(this.cbRender);
            this.gbSettings.Controls.Add(this.btnCancel);
            this.gbSettings.Location = new Point(398, 12);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new Size(774, 286);
            this.gbSettings.TabIndex = 0;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Nutcracker Settings  -  Original Concept by: Sean Meighan and Matt Brown";
            // 
            // gbModel
            // 
            this.gbModel.Controls.Add(this.btnModelEdit);
            this.gbModel.Controls.Add(this.btnModelRemove);
            this.gbModel.Controls.Add(this.tbSummary);
            this.gbModel.Controls.Add(this.progressBar);
            this.gbModel.Controls.Add(this.lblRenderInfo);
            this.gbModel.Controls.Add(this.cbModels);
            this.gbModel.Location = new Point(6, 19);
            this.gbModel.Name = "gbModel";
            this.gbModel.Size = new Size(239, 232);
            this.gbModel.TabIndex = 25;
            this.gbModel.TabStop = false;
            this.gbModel.Text = "Models";
            // 
            // btnModelEdit
            // 
            this.btnModelEdit.BackgroundImage = ((Image)(resources.GetObject("btnModelEdit.BackgroundImage")));
            this.btnModelEdit.BackgroundImageLayout = ImageLayout.Center;
            this.btnModelEdit.Location = new Point(181, 18);
            this.btnModelEdit.Name = "btnModelEdit";
            this.btnModelEdit.Size = new Size(23, 23);
            this.btnModelEdit.TabIndex = 25;
            this.toolTip.SetToolTip(this.btnModelEdit, "Edit Model");
            this.btnModelEdit.UseVisualStyleBackColor = true;
            this.btnModelEdit.Visible = false;
            this.btnModelEdit.Click += new EventHandler(this.btnModelEdit_Click);
            // 
            // btnModelRemove
            // 
            this.btnModelRemove.BackgroundImage = ((Image)(resources.GetObject("btnModelRemove.BackgroundImage")));
            this.btnModelRemove.BackgroundImageLayout = ImageLayout.Center;
            this.btnModelRemove.Location = new Point(210, 18);
            this.btnModelRemove.Name = "btnModelRemove";
            this.btnModelRemove.Size = new Size(23, 23);
            this.btnModelRemove.TabIndex = 24;
            this.toolTip.SetToolTip(this.btnModelRemove, "Delete Model");
            this.btnModelRemove.UseVisualStyleBackColor = true;
            this.btnModelRemove.Visible = false;
            this.btnModelRemove.Click += new EventHandler(this.btnModelRemove_Click);
            // 
            // tbSummary
            // 
            this.tbSummary.Location = new Point(6, 62);
            this.tbSummary.Multiline = true;
            this.tbSummary.Name = "tbSummary";
            this.tbSummary.Size = new Size(227, 133);
            this.tbSummary.TabIndex = 16;
            // 
            // progressBar
            // 
            this.progressBar.Location = new Point(6, 201);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(227, 25);
            this.progressBar.TabIndex = 17;
            this.progressBar.Visible = false;
            // 
            // lblRenderInfo
            // 
            this.lblRenderInfo.AutoSize = true;
            this.lblRenderInfo.Location = new Point(6, 46);
            this.lblRenderInfo.Name = "lblRenderInfo";
            this.lblRenderInfo.Size = new Size(114, 13);
            this.lblRenderInfo.TabIndex = 15;
            this.lblRenderInfo.Text = "Rendering Information:";
            // 
            // cbModels
            // 
            this.cbModels.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbModels.FormattingEnabled = true;
            this.cbModels.Location = new Point(9, 19);
            this.cbModels.Name = "cbModels";
            this.cbModels.Size = new Size(166, 21);
            this.cbModels.TabIndex = 2;
            this.cbModels.SelectedIndexChanged += new EventHandler(this.cbModels_SelectedIndexChanged);
            // 
            // lblPresets
            // 
            this.lblPresets.AutoSize = true;
            this.lblPresets.Location = new Point(360, 262);
            this.lblPresets.Name = "lblPresets";
            this.lblPresets.Size = new Size(76, 13);
            this.lblPresets.TabIndex = 23;
            this.lblPresets.Text = "Effect Presets:";
            // 
            // cbEffectsPresets
            // 
            this.cbEffectsPresets.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbEffectsPresets.FormattingEnabled = true;
            this.cbEffectsPresets.Location = new Point(442, 259);
            this.cbEffectsPresets.Name = "cbEffectsPresets";
            this.cbEffectsPresets.Size = new Size(164, 21);
            this.cbEffectsPresets.TabIndex = 22;
            this.cbEffectsPresets.SelectedIndexChanged += new EventHandler(this.cbEffectsPresets_SelectedIndexChanged);
            // 
            // tbSparkles
            // 
            this.tbSparkles.AutoSize = false;
            this.tbSparkles.Location = new Point(60, 257);
            this.tbSparkles.Maximum = 100;
            this.tbSparkles.Name = "tbSparkles";
            this.tbSparkles.Size = new Size(148, 23);
            this.tbSparkles.TabIndex = 3;
            this.tbSparkles.TickStyle = TickStyle.None;
            // 
            // lblSparkles
            // 
            this.lblSparkles.AutoSize = true;
            this.lblSparkles.Location = new Point(6, 262);
            this.lblSparkles.Name = "lblSparkles";
            this.lblSparkles.Size = new Size(48, 13);
            this.lblSparkles.TabIndex = 18;
            this.lblSparkles.Text = "Sparkles";
            // 
            // lblStatsMs
            // 
            this.lblStatsMs.AutoSize = true;
            this.lblStatsMs.Location = new Point(645, 176);
            this.lblStatsMs.Name = "lblStatsMs";
            this.lblStatsMs.Size = new Size(29, 13);
            this.lblStatsMs.TabIndex = 19;
            this.lblStatsMs.Text = "0 ms";
            // 
            // gbLayer
            // 
            this.gbLayer.Controls.Add(this.rbAverage);
            this.gbLayer.Controls.Add(this.rbLayer);
            this.gbLayer.Controls.Add(this.rbUnmask2);
            this.gbLayer.Controls.Add(this.rbUnmask1);
            this.gbLayer.Controls.Add(this.rbMask2);
            this.gbLayer.Controls.Add(this.rbMask1);
            this.gbLayer.Controls.Add(this.rbEffect2);
            this.gbLayer.Controls.Add(this.rbEffect1);
            this.gbLayer.Location = new Point(486, 19);
            this.gbLayer.Name = "gbLayer";
            this.gbLayer.Size = new Size(141, 232);
            this.gbLayer.TabIndex = 12;
            this.gbLayer.TabStop = false;
            this.gbLayer.Text = "Layer Method";
            // 
            // rbAverage
            // 
            this.rbAverage.AutoSize = true;
            this.rbAverage.Location = new Point(6, 178);
            this.rbAverage.Name = "rbAverage";
            this.rbAverage.Size = new Size(123, 17);
            this.rbAverage.TabIndex = 7;
            this.rbAverage.Text = "Average Effect 1 && 2";
            this.rbAverage.UseVisualStyleBackColor = true;
            this.rbAverage.CheckedChanged += new EventHandler(this.EffectLayerChanged);
            // 
            // rbLayer
            // 
            this.rbLayer.AutoSize = true;
            this.rbLayer.Location = new Point(6, 155);
            this.rbLayer.Name = "rbLayer";
            this.rbLayer.Size = new Size(109, 17);
            this.rbLayer.TabIndex = 6;
            this.rbLayer.Text = "Layer Effect 1 && 2";
            this.rbLayer.UseVisualStyleBackColor = true;
            this.rbLayer.CheckedChanged += new EventHandler(this.EffectLayerChanged);
            // 
            // rbUnmask2
            // 
            this.rbUnmask2.AutoSize = true;
            this.rbUnmask2.Location = new Point(6, 132);
            this.rbUnmask2.Name = "rbUnmask2";
            this.rbUnmask2.Size = new Size(114, 17);
            this.rbUnmask2.TabIndex = 5;
            this.rbUnmask2.Text = "Effect 2 is Unmask";
            this.rbUnmask2.UseVisualStyleBackColor = true;
            this.rbUnmask2.CheckedChanged += new EventHandler(this.EffectLayerChanged);
            // 
            // rbUnmask1
            // 
            this.rbUnmask1.AutoSize = true;
            this.rbUnmask1.Location = new Point(6, 109);
            this.rbUnmask1.Name = "rbUnmask1";
            this.rbUnmask1.Size = new Size(114, 17);
            this.rbUnmask1.TabIndex = 4;
            this.rbUnmask1.Text = "Effect 1 is Unmask";
            this.rbUnmask1.UseVisualStyleBackColor = true;
            this.rbUnmask1.CheckedChanged += new EventHandler(this.EffectLayerChanged);
            // 
            // rbMask2
            // 
            this.rbMask2.AutoSize = true;
            this.rbMask2.Location = new Point(6, 86);
            this.rbMask2.Name = "rbMask2";
            this.rbMask2.Size = new Size(101, 17);
            this.rbMask2.TabIndex = 3;
            this.rbMask2.Text = "Effect 2 is Mask";
            this.rbMask2.UseVisualStyleBackColor = true;
            this.rbMask2.CheckedChanged += new EventHandler(this.EffectLayerChanged);
            // 
            // rbMask1
            // 
            this.rbMask1.AutoSize = true;
            this.rbMask1.Location = new Point(6, 65);
            this.rbMask1.Name = "rbMask1";
            this.rbMask1.Size = new Size(101, 17);
            this.rbMask1.TabIndex = 2;
            this.rbMask1.Text = "Effect 1 is Mask";
            this.rbMask1.UseVisualStyleBackColor = true;
            this.rbMask1.CheckedChanged += new EventHandler(this.EffectLayerChanged);
            // 
            // rbEffect2
            // 
            this.rbEffect2.AutoSize = true;
            this.rbEffect2.Location = new Point(6, 42);
            this.rbEffect2.Name = "rbEffect2";
            this.rbEffect2.Size = new Size(62, 17);
            this.rbEffect2.TabIndex = 1;
            this.rbEffect2.Text = "Effect 2";
            this.rbEffect2.UseVisualStyleBackColor = true;
            this.rbEffect2.CheckedChanged += new EventHandler(this.EffectLayerChanged);
            // 
            // rbEffect1
            // 
            this.rbEffect1.AutoSize = true;
            this.rbEffect1.Checked = true;
            this.rbEffect1.Location = new Point(6, 19);
            this.rbEffect1.Name = "rbEffect1";
            this.rbEffect1.Size = new Size(62, 17);
            this.rbEffect1.TabIndex = 0;
            this.rbEffect1.TabStop = true;
            this.rbEffect1.Text = "Effect 1";
            this.rbEffect1.UseVisualStyleBackColor = true;
            this.rbEffect1.CheckedChanged += new EventHandler(this.EffectLayerChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(612, 257);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Okay";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            // 
            // btnPlayStop
            // 
            this.btnPlayStop.Location = new Point(214, 257);
            this.btnPlayStop.Name = "btnPlayStop";
            this.btnPlayStop.Size = new Size(90, 23);
            this.btnPlayStop.TabIndex = 4;
            this.btnPlayStop.Text = "Play Effects";
            this.btnPlayStop.UseVisualStyleBackColor = true;
            this.btnPlayStop.Click += new EventHandler(this.btnPlayStop_Click);
            // 
            // lblStatsFps
            // 
            this.lblStatsFps.Location = new Point(693, 176);
            this.lblStatsFps.Name = "lblStatsFps";
            this.lblStatsFps.Size = new Size(75, 13);
            this.lblStatsFps.TabIndex = 20;
            this.lblStatsFps.Text = "0.00 FPS";
            this.lblStatsFps.TextAlign = ContentAlignment.TopRight;
            // 
            // gbRenderTo
            // 
            this.gbRenderTo.Controls.Add(this.cbChannels);
            this.gbRenderTo.Controls.Add(this.lblChannels);
            this.gbRenderTo.Controls.Add(this.lblStartEventTime);
            this.gbRenderTo.Controls.Add(this.nudStartEvent);
            this.gbRenderTo.Controls.Add(this.lblStartEvent);
            this.gbRenderTo.Controls.Add(this.lblEventCountTime);
            this.gbRenderTo.Controls.Add(this.nudEventCount);
            this.gbRenderTo.Controls.Add(this.lblEventCount);
            this.gbRenderTo.Controls.Add(this.rbSpecificPoint);
            this.gbRenderTo.Controls.Add(this.rbCurrentSelection);
            this.gbRenderTo.Controls.Add(this.rbRoutine);
            this.gbRenderTo.Controls.Add(this.rbClipboard);
            this.gbRenderTo.Location = new Point(251, 19);
            this.gbRenderTo.Name = "gbRenderTo";
            this.gbRenderTo.Size = new Size(229, 232);
            this.gbRenderTo.TabIndex = 11;
            this.gbRenderTo.TabStop = false;
            this.gbRenderTo.Text = "Render Effects To";
            // 
            // cbChannels
            // 
            this.cbChannels.DrawMode = DrawMode.OwnerDrawFixed;
            this.cbChannels.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbChannels.FormattingEnabled = true;
            this.cbChannels.Location = new Point(59, 138);
            this.cbChannels.Name = "cbChannels";
            this.cbChannels.Size = new Size(164, 21);
            this.cbChannels.TabIndex = 11;
            this.cbChannels.DrawItem += new DrawItemEventHandler(this.cbChannels_DrawItem);
            // 
            // lblChannels
            // 
            this.lblChannels.AutoSize = true;
            this.lblChannels.Location = new Point(7, 141);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new Size(46, 13);
            this.lblChannels.TabIndex = 10;
            this.lblChannels.Text = "Channel";
            // 
            // lblStartEventTime
            // 
            this.lblStartEventTime.AutoSize = true;
            this.lblStartEventTime.Location = new Point(161, 167);
            this.lblStartEventTime.Name = "lblStartEventTime";
            this.lblStartEventTime.Size = new Size(55, 13);
            this.lblStartEventTime.TabIndex = 7;
            this.lblStartEventTime.Text = "00:00.000";
            this.lblStartEventTime.Visible = false;
            // 
            // nudStartEvent
            // 
            this.nudStartEvent.Location = new Point(104, 165);
            this.nudStartEvent.Name = "nudStartEvent";
            this.nudStartEvent.Size = new Size(51, 20);
            this.nudStartEvent.TabIndex = 4;
            this.nudStartEvent.Visible = false;
            this.nudStartEvent.ValueChanged += new EventHandler(this.nudStartEvent_ValueChanged);
            // 
            // lblStartEvent
            // 
            this.lblStartEvent.AutoSize = true;
            this.lblStartEvent.Location = new Point(38, 167);
            this.lblStartEvent.Name = "lblStartEvent";
            this.lblStartEvent.Size = new Size(60, 13);
            this.lblStartEvent.TabIndex = 6;
            this.lblStartEvent.Text = "Start Event";
            this.lblStartEvent.Visible = false;
            // 
            // lblEventCountTime
            // 
            this.lblEventCountTime.AutoSize = true;
            this.lblEventCountTime.Location = new Point(161, 193);
            this.lblEventCountTime.Name = "lblEventCountTime";
            this.lblEventCountTime.Size = new Size(55, 13);
            this.lblEventCountTime.TabIndex = 9;
            this.lblEventCountTime.Text = "00:00.000";
            this.lblEventCountTime.Visible = false;
            // 
            // nudEventCount
            // 
            this.nudEventCount.Location = new Point(104, 191);
            this.nudEventCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEventCount.Name = "nudEventCount";
            this.nudEventCount.Size = new Size(51, 20);
            this.nudEventCount.TabIndex = 5;
            this.nudEventCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEventCount.Visible = false;
            this.nudEventCount.ValueChanged += new EventHandler(this.nudEventCount_ValueChanged);
            // 
            // lblEventCount
            // 
            this.lblEventCount.AutoSize = true;
            this.lblEventCount.Location = new Point(6, 193);
            this.lblEventCount.Name = "lblEventCount";
            this.lblEventCount.Size = new Size(92, 13);
            this.lblEventCount.TabIndex = 8;
            this.lblEventCount.Text = "Number of Events";
            this.lblEventCount.Visible = false;
            // 
            // rbSpecificPoint
            // 
            this.rbSpecificPoint.AutoSize = true;
            this.rbSpecificPoint.Location = new Point(6, 86);
            this.rbSpecificPoint.Name = "rbSpecificPoint";
            this.rbSpecificPoint.Size = new Size(154, 17);
            this.rbSpecificPoint.TabIndex = 3;
            this.rbSpecificPoint.Text = "Specific Point In Sequence";
            this.rbSpecificPoint.UseVisualStyleBackColor = true;
            this.rbSpecificPoint.CheckedChanged += new EventHandler(this.RenderToChanged);
            // 
            // rbCurrentSelection
            // 
            this.rbCurrentSelection.AutoSize = true;
            this.rbCurrentSelection.Location = new Point(6, 63);
            this.rbCurrentSelection.Name = "rbCurrentSelection";
            this.rbCurrentSelection.Size = new Size(158, 17);
            this.rbCurrentSelection.TabIndex = 2;
            this.rbCurrentSelection.Text = "Current Sequence Selection";
            this.rbCurrentSelection.UseVisualStyleBackColor = true;
            this.rbCurrentSelection.CheckedChanged += new EventHandler(this.RenderToChanged);
            // 
            // rbRoutine
            // 
            this.rbRoutine.AutoSize = true;
            this.rbRoutine.Location = new Point(6, 19);
            this.rbRoutine.Name = "rbRoutine";
            this.rbRoutine.Size = new Size(136, 17);
            this.rbRoutine.TabIndex = 0;
            this.rbRoutine.Text = "Vixen+ Routine (.vir file)";
            this.rbRoutine.UseVisualStyleBackColor = true;
            this.rbRoutine.CheckedChanged += new EventHandler(this.RenderToChanged);
            // 
            // rbClipboard
            // 
            this.rbClipboard.AutoSize = true;
            this.rbClipboard.Checked = true;
            this.rbClipboard.Location = new Point(6, 42);
            this.rbClipboard.Name = "rbClipboard";
            this.rbClipboard.Size = new Size(69, 17);
            this.rbClipboard.TabIndex = 1;
            this.rbClipboard.TabStop = true;
            this.rbClipboard.Text = "Clipboard";
            this.rbClipboard.UseVisualStyleBackColor = true;
            this.rbClipboard.CheckedChanged += new EventHandler(this.RenderToChanged);
            // 
            // chkBoxEnableRawPreview
            // 
            this.chkBoxEnableRawPreview.AutoSize = true;
            this.chkBoxEnableRawPreview.Checked = true;
            this.chkBoxEnableRawPreview.CheckState = CheckState.Checked;
            this.chkBoxEnableRawPreview.Location = new Point(648, 152);
            this.chkBoxEnableRawPreview.Name = "chkBoxEnableRawPreview";
            this.chkBoxEnableRawPreview.Size = new Size(125, 17);
            this.chkBoxEnableRawPreview.TabIndex = 6;
            this.chkBoxEnableRawPreview.Text = "Enable Raw Preview";
            this.chkBoxEnableRawPreview.UseVisualStyleBackColor = true;
            // 
            // pbRawPreview
            // 
            this.pbRawPreview.BackColor = Color.Black;
            this.pbRawPreview.Location = new Point(648, 25);
            this.pbRawPreview.Name = "pbRawPreview";
            this.pbRawPreview.Size = new Size(120, 120);
            this.pbRawPreview.TabIndex = 11;
            this.pbRawPreview.TabStop = false;
            // 
            // btnLightsOff
            // 
            this.btnLightsOff.Enabled = false;
            this.btnLightsOff.Location = new Point(648, 205);
            this.btnLightsOff.Name = "btnLightsOff";
            this.btnLightsOff.Size = new Size(75, 23);
            this.btnLightsOff.TabIndex = 7;
            this.btnLightsOff.Text = "Lights Off";
            this.btnLightsOff.UseVisualStyleBackColor = true;
            this.btnLightsOff.Visible = false;
            // 
            // cbRender
            // 
            this.cbRender.AutoSize = true;
            this.cbRender.Enabled = false;
            this.cbRender.Location = new Point(648, 234);
            this.cbRender.Name = "cbRender";
            this.cbRender.Size = new Size(101, 17);
            this.cbRender.TabIndex = 8;
            this.cbRender.Text = "Output to Lights";
            this.cbRender.UseVisualStyleBackColor = true;
            this.cbRender.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(693, 257);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // timerRender
            // 
            this.timerRender.Interval = 50;
            this.timerRender.Tick += new EventHandler(this.timerRender_Tick);
            // 
            // nutcrackerEffectControl1
            // 
            this.nutcrackerEffectControl1.Location = new Point(7, 20);
            this.nutcrackerEffectControl1.Name = "nutcrackerEffectControl1";
            this.nutcrackerEffectControl1.Size = new Size(371, 225);
            this.nutcrackerEffectControl1.Speed = 1;
            this.nutcrackerEffectControl1.TabIndex = 0;
            this.nutcrackerEffectControl1.ControlChanged += new NutcrackerEffectControl.ControlChangedHandler(this.ControlChanged1);
            // 
            // nutcrackerEffectControl2
            // 
            this.nutcrackerEffectControl2.Location = new Point(7, 20);
            this.nutcrackerEffectControl2.Name = "nutcrackerEffectControl2";
            this.nutcrackerEffectControl2.Size = new Size(371, 225);
            this.nutcrackerEffectControl2.Speed = 1;
            this.nutcrackerEffectControl2.TabIndex = 0;
            this.nutcrackerEffectControl2.ControlChanged += new NutcrackerEffectControl.ControlChangedHandler(this.ControlChanged2);
            // 
            // NutcrackerControlDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1184, 562);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.gbEffect1);
            this.Controls.Add(this.gbEffect2);
            this.Controls.Add(this.pbPreview);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NutcrackerControlDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Generate Nutcracker Effect";
            this.FormClosing += new FormClosingEventHandler(this.NutcrackerControlDialog_FormClosing);
            this.gbEffect2.ResumeLayout(false);
            this.gbEffect1.ResumeLayout(false);
            ((ISupportInitialize)(this.pbPreview)).EndInit();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.gbModel.ResumeLayout(false);
            this.gbModel.PerformLayout();
            ((ISupportInitialize)(this.tbSparkles)).EndInit();
            this.gbLayer.ResumeLayout(false);
            this.gbLayer.PerformLayout();
            this.gbRenderTo.ResumeLayout(false);
            this.gbRenderTo.PerformLayout();
            ((ISupportInitialize)(this.nudStartEvent)).EndInit();
            ((ISupportInitialize)(this.nudEventCount)).EndInit();
            ((ISupportInitialize)(this.pbRawPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbEffect2;
        private GroupBox gbEffect1;
        private PictureBox pbPreview;
        private GroupBox gbSettings;
        private Button btnOK;
        private Button btnCancel;
        private CheckBox cbRender;
        private NutcrackerEffectControl nutcrackerEffectControl2;
        private NutcrackerEffectControl nutcrackerEffectControl1;
        private GroupBox gbLayer;
        private RadioButton rbAverage;
        private RadioButton rbLayer;
        private RadioButton rbUnmask2;
        private RadioButton rbUnmask1;
        private RadioButton rbMask2;
        private RadioButton rbMask1;
        private RadioButton rbEffect2;
        private RadioButton rbEffect1;
        private Button btnLightsOff;
        private Timer timerRender;
        private ComboBox cbModels;
        private Button btnPlayStop;
        private CheckBox chkBoxEnableRawPreview;
        private PictureBox pbRawPreview;
        private Label lblStatsFps;
        private GroupBox gbRenderTo;
        private Label lblEventCountTime;
        private NumericUpDown nudEventCount;
        private Label lblEventCount;
        private RadioButton rbSpecificPoint;
        private RadioButton rbCurrentSelection;
        private RadioButton rbRoutine;
        private RadioButton rbClipboard;
        private Label lblStartEventTime;
        private NumericUpDown nudStartEvent;
        private Label lblStartEvent;
        private TextBox tbSummary;
        private Label lblStatsMs;
        private TrackBar tbSparkles;
        private Label lblSparkles;
        private Label lblRenderInfo;
        private ProgressBar progressBar;
        private ComboBox cbEffectsPresets;
        private Label lblPresets;
        private Button btnModelRemove;
        private GroupBox gbModel;
        private Button btnModelEdit;
        private ToolTip toolTip;
        private ComboBox cbChannels;
        private Label lblChannels;
    }
}