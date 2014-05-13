using System.Windows.Forms;

namespace Nutcracker
{
    sealed partial class NutcrackerControlDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NutcrackerControlDialog));
            this.gbEffect2 = new System.Windows.Forms.GroupBox();
            this.gbEffect1 = new System.Windows.Forms.GroupBox();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.gbModel = new System.Windows.Forms.GroupBox();
            this.btnModelEdit = new System.Windows.Forms.Button();
            this.btnModelRemove = new System.Windows.Forms.Button();
            this.tbSummary = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblRenderInfo = new System.Windows.Forms.Label();
            this.cbModels = new System.Windows.Forms.ComboBox();
            this.lblPresets = new System.Windows.Forms.Label();
            this.cbEffectsPresets = new System.Windows.Forms.ComboBox();
            this.tbSparkles = new System.Windows.Forms.TrackBar();
            this.lblSparkles = new System.Windows.Forms.Label();
            this.lblStatsMs = new System.Windows.Forms.Label();
            this.gbLayer = new System.Windows.Forms.GroupBox();
            this.rbAverage = new System.Windows.Forms.RadioButton();
            this.rbLayer = new System.Windows.Forms.RadioButton();
            this.rbUnmask2 = new System.Windows.Forms.RadioButton();
            this.rbUnmask1 = new System.Windows.Forms.RadioButton();
            this.rbMask2 = new System.Windows.Forms.RadioButton();
            this.rbMask1 = new System.Windows.Forms.RadioButton();
            this.rbEffect2 = new System.Windows.Forms.RadioButton();
            this.rbEffect1 = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnPlayStop = new System.Windows.Forms.Button();
            this.lblStatsFps = new System.Windows.Forms.Label();
            this.gbRenderTo = new System.Windows.Forms.GroupBox();
            this.lblStartEventTime = new System.Windows.Forms.Label();
            this.nudStartEvent = new System.Windows.Forms.NumericUpDown();
            this.lblStartEvent = new System.Windows.Forms.Label();
            this.lblEventCountTime = new System.Windows.Forms.Label();
            this.nudEventCount = new System.Windows.Forms.NumericUpDown();
            this.lblEventCount = new System.Windows.Forms.Label();
            this.rbSpecificPoint = new System.Windows.Forms.RadioButton();
            this.rbCurrentSelection = new System.Windows.Forms.RadioButton();
            this.rbRoutine = new System.Windows.Forms.RadioButton();
            this.rbClipboard = new System.Windows.Forms.RadioButton();
            this.chkBoxEnableRawPreview = new System.Windows.Forms.CheckBox();
            this.pbRawPreview = new System.Windows.Forms.PictureBox();
            this.btnLightsOff = new System.Windows.Forms.Button();
            this.cbRender = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.timerRender = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblChannels = new System.Windows.Forms.Label();
            this.cbChannels = new System.Windows.Forms.ComboBox();
            this.nutcrackerEffectControl1 = new Nutcracker.NutcrackerEffectControl();
            this.nutcrackerEffectControl2 = new Nutcracker.NutcrackerEffectControl();
            this.gbEffect2.SuspendLayout();
            this.gbEffect1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.gbSettings.SuspendLayout();
            this.gbModel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSparkles)).BeginInit();
            this.gbLayer.SuspendLayout();
            this.gbRenderTo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartEvent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEventCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRawPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // gbEffect2
            // 
            this.gbEffect2.Controls.Add(this.nutcrackerEffectControl2);
            this.gbEffect2.Location = new System.Drawing.Point(788, 304);
            this.gbEffect2.Name = "gbEffect2";
            this.gbEffect2.Size = new System.Drawing.Size(384, 250);
            this.gbEffect2.TabIndex = 2;
            this.gbEffect2.TabStop = false;
            this.gbEffect2.Text = "Effect 2";
            // 
            // gbEffect1
            // 
            this.gbEffect1.Controls.Add(this.nutcrackerEffectControl1);
            this.gbEffect1.Location = new System.Drawing.Point(398, 304);
            this.gbEffect1.Name = "gbEffect1";
            this.gbEffect1.Size = new System.Drawing.Size(384, 250);
            this.gbEffect1.TabIndex = 1;
            this.gbEffect1.TabStop = false;
            this.gbEffect1.Text = "Effect 1";
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.Black;
            this.pbPreview.Location = new System.Drawing.Point(12, 12);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(380, 538);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
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
            this.gbSettings.Location = new System.Drawing.Point(398, 12);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(774, 286);
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
            this.gbModel.Location = new System.Drawing.Point(6, 19);
            this.gbModel.Name = "gbModel";
            this.gbModel.Size = new System.Drawing.Size(239, 232);
            this.gbModel.TabIndex = 25;
            this.gbModel.TabStop = false;
            this.gbModel.Text = "Models";
            // 
            // btnModelEdit
            // 
            this.btnModelEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnModelEdit.BackgroundImage")));
            this.btnModelEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnModelEdit.Location = new System.Drawing.Point(181, 18);
            this.btnModelEdit.Name = "btnModelEdit";
            this.btnModelEdit.Size = new System.Drawing.Size(23, 23);
            this.btnModelEdit.TabIndex = 25;
            this.toolTip.SetToolTip(this.btnModelEdit, "Edit Model");
            this.btnModelEdit.UseVisualStyleBackColor = true;
            this.btnModelEdit.Visible = false;
            this.btnModelEdit.Click += new System.EventHandler(this.btnModelEdit_Click);
            // 
            // btnModelRemove
            // 
            this.btnModelRemove.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnModelRemove.BackgroundImage")));
            this.btnModelRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnModelRemove.Location = new System.Drawing.Point(210, 18);
            this.btnModelRemove.Name = "btnModelRemove";
            this.btnModelRemove.Size = new System.Drawing.Size(23, 23);
            this.btnModelRemove.TabIndex = 24;
            this.toolTip.SetToolTip(this.btnModelRemove, "Delete Model");
            this.btnModelRemove.UseVisualStyleBackColor = true;
            this.btnModelRemove.Visible = false;
            this.btnModelRemove.Click += new System.EventHandler(this.btnModelRemove_Click);
            // 
            // tbSummary
            // 
            this.tbSummary.Location = new System.Drawing.Point(6, 62);
            this.tbSummary.Multiline = true;
            this.tbSummary.Name = "tbSummary";
            this.tbSummary.Size = new System.Drawing.Size(227, 133);
            this.tbSummary.TabIndex = 16;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 203);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(227, 23);
            this.progressBar.TabIndex = 17;
            this.progressBar.Visible = false;
            // 
            // lblRenderInfo
            // 
            this.lblRenderInfo.AutoSize = true;
            this.lblRenderInfo.Location = new System.Drawing.Point(6, 46);
            this.lblRenderInfo.Name = "lblRenderInfo";
            this.lblRenderInfo.Size = new System.Drawing.Size(114, 13);
            this.lblRenderInfo.TabIndex = 15;
            this.lblRenderInfo.Text = "Rendering Information:";
            // 
            // cbModels
            // 
            this.cbModels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModels.FormattingEnabled = true;
            this.cbModels.Location = new System.Drawing.Point(9, 19);
            this.cbModels.Name = "cbModels";
            this.cbModels.Size = new System.Drawing.Size(166, 21);
            this.cbModels.TabIndex = 2;
            this.cbModels.SelectedIndexChanged += new System.EventHandler(this.cbModels_SelectedIndexChanged);
            // 
            // lblPresets
            // 
            this.lblPresets.AutoSize = true;
            this.lblPresets.Location = new System.Drawing.Point(360, 262);
            this.lblPresets.Name = "lblPresets";
            this.lblPresets.Size = new System.Drawing.Size(76, 13);
            this.lblPresets.TabIndex = 23;
            this.lblPresets.Text = "Effect Presets:";
            // 
            // cbEffectsPresets
            // 
            this.cbEffectsPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEffectsPresets.FormattingEnabled = true;
            this.cbEffectsPresets.Location = new System.Drawing.Point(442, 259);
            this.cbEffectsPresets.Name = "cbEffectsPresets";
            this.cbEffectsPresets.Size = new System.Drawing.Size(164, 21);
            this.cbEffectsPresets.TabIndex = 22;
            this.cbEffectsPresets.SelectedIndexChanged += new System.EventHandler(this.cbEffectsPresets_SelectedIndexChanged);
            // 
            // tbSparkles
            // 
            this.tbSparkles.AutoSize = false;
            this.tbSparkles.Location = new System.Drawing.Point(60, 257);
            this.tbSparkles.Maximum = 100;
            this.tbSparkles.Name = "tbSparkles";
            this.tbSparkles.Size = new System.Drawing.Size(148, 23);
            this.tbSparkles.TabIndex = 3;
            this.tbSparkles.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblSparkles
            // 
            this.lblSparkles.AutoSize = true;
            this.lblSparkles.Location = new System.Drawing.Point(6, 262);
            this.lblSparkles.Name = "lblSparkles";
            this.lblSparkles.Size = new System.Drawing.Size(48, 13);
            this.lblSparkles.TabIndex = 18;
            this.lblSparkles.Text = "Sparkles";
            // 
            // lblStatsMs
            // 
            this.lblStatsMs.AutoSize = true;
            this.lblStatsMs.Location = new System.Drawing.Point(645, 176);
            this.lblStatsMs.Name = "lblStatsMs";
            this.lblStatsMs.Size = new System.Drawing.Size(29, 13);
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
            this.gbLayer.Location = new System.Drawing.Point(486, 19);
            this.gbLayer.Name = "gbLayer";
            this.gbLayer.Size = new System.Drawing.Size(141, 232);
            this.gbLayer.TabIndex = 12;
            this.gbLayer.TabStop = false;
            this.gbLayer.Text = "Layer Method";
            // 
            // rbAverage
            // 
            this.rbAverage.AutoSize = true;
            this.rbAverage.Location = new System.Drawing.Point(6, 178);
            this.rbAverage.Name = "rbAverage";
            this.rbAverage.Size = new System.Drawing.Size(123, 17);
            this.rbAverage.TabIndex = 7;
            this.rbAverage.Text = "Average Effect 1 && 2";
            this.rbAverage.UseVisualStyleBackColor = true;
            this.rbAverage.CheckedChanged += new System.EventHandler(this.EffectLayerChanged);
            // 
            // rbLayer
            // 
            this.rbLayer.AutoSize = true;
            this.rbLayer.Location = new System.Drawing.Point(6, 155);
            this.rbLayer.Name = "rbLayer";
            this.rbLayer.Size = new System.Drawing.Size(109, 17);
            this.rbLayer.TabIndex = 6;
            this.rbLayer.Text = "Layer Effect 1 && 2";
            this.rbLayer.UseVisualStyleBackColor = true;
            this.rbLayer.CheckedChanged += new System.EventHandler(this.EffectLayerChanged);
            // 
            // rbUnmask2
            // 
            this.rbUnmask2.AutoSize = true;
            this.rbUnmask2.Location = new System.Drawing.Point(6, 132);
            this.rbUnmask2.Name = "rbUnmask2";
            this.rbUnmask2.Size = new System.Drawing.Size(114, 17);
            this.rbUnmask2.TabIndex = 5;
            this.rbUnmask2.Text = "Effect 2 is Unmask";
            this.rbUnmask2.UseVisualStyleBackColor = true;
            this.rbUnmask2.CheckedChanged += new System.EventHandler(this.EffectLayerChanged);
            // 
            // rbUnmask1
            // 
            this.rbUnmask1.AutoSize = true;
            this.rbUnmask1.Location = new System.Drawing.Point(6, 109);
            this.rbUnmask1.Name = "rbUnmask1";
            this.rbUnmask1.Size = new System.Drawing.Size(114, 17);
            this.rbUnmask1.TabIndex = 4;
            this.rbUnmask1.Text = "Effect 1 is Unmask";
            this.rbUnmask1.UseVisualStyleBackColor = true;
            this.rbUnmask1.CheckedChanged += new System.EventHandler(this.EffectLayerChanged);
            // 
            // rbMask2
            // 
            this.rbMask2.AutoSize = true;
            this.rbMask2.Location = new System.Drawing.Point(6, 86);
            this.rbMask2.Name = "rbMask2";
            this.rbMask2.Size = new System.Drawing.Size(101, 17);
            this.rbMask2.TabIndex = 3;
            this.rbMask2.Text = "Effect 2 is Mask";
            this.rbMask2.UseVisualStyleBackColor = true;
            this.rbMask2.CheckedChanged += new System.EventHandler(this.EffectLayerChanged);
            // 
            // rbMask1
            // 
            this.rbMask1.AutoSize = true;
            this.rbMask1.Location = new System.Drawing.Point(6, 65);
            this.rbMask1.Name = "rbMask1";
            this.rbMask1.Size = new System.Drawing.Size(101, 17);
            this.rbMask1.TabIndex = 2;
            this.rbMask1.Text = "Effect 1 is Mask";
            this.rbMask1.UseVisualStyleBackColor = true;
            this.rbMask1.CheckedChanged += new System.EventHandler(this.EffectLayerChanged);
            // 
            // rbEffect2
            // 
            this.rbEffect2.AutoSize = true;
            this.rbEffect2.Location = new System.Drawing.Point(6, 42);
            this.rbEffect2.Name = "rbEffect2";
            this.rbEffect2.Size = new System.Drawing.Size(62, 17);
            this.rbEffect2.TabIndex = 1;
            this.rbEffect2.Text = "Effect 2";
            this.rbEffect2.UseVisualStyleBackColor = true;
            this.rbEffect2.CheckedChanged += new System.EventHandler(this.EffectLayerChanged);
            // 
            // rbEffect1
            // 
            this.rbEffect1.AutoSize = true;
            this.rbEffect1.Checked = true;
            this.rbEffect1.Location = new System.Drawing.Point(6, 19);
            this.rbEffect1.Name = "rbEffect1";
            this.rbEffect1.Size = new System.Drawing.Size(62, 17);
            this.rbEffect1.TabIndex = 0;
            this.rbEffect1.TabStop = true;
            this.rbEffect1.Text = "Effect 1";
            this.rbEffect1.UseVisualStyleBackColor = true;
            this.rbEffect1.CheckedChanged += new System.EventHandler(this.EffectLayerChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(612, 257);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Okay";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnPlayStop
            // 
            this.btnPlayStop.Location = new System.Drawing.Point(214, 257);
            this.btnPlayStop.Name = "btnPlayStop";
            this.btnPlayStop.Size = new System.Drawing.Size(90, 23);
            this.btnPlayStop.TabIndex = 4;
            this.btnPlayStop.Text = "Play Effects";
            this.btnPlayStop.UseVisualStyleBackColor = true;
            this.btnPlayStop.Click += new System.EventHandler(this.btnPlayStop_Click);
            // 
            // lblStatsFps
            // 
            this.lblStatsFps.Location = new System.Drawing.Point(693, 176);
            this.lblStatsFps.Name = "lblStatsFps";
            this.lblStatsFps.Size = new System.Drawing.Size(75, 13);
            this.lblStatsFps.TabIndex = 20;
            this.lblStatsFps.Text = "0.00 FPS";
            this.lblStatsFps.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.gbRenderTo.Location = new System.Drawing.Point(251, 19);
            this.gbRenderTo.Name = "gbRenderTo";
            this.gbRenderTo.Size = new System.Drawing.Size(229, 232);
            this.gbRenderTo.TabIndex = 11;
            this.gbRenderTo.TabStop = false;
            this.gbRenderTo.Text = "Render Effects To";
            // 
            // lblStartEventTime
            // 
            this.lblStartEventTime.AutoSize = true;
            this.lblStartEventTime.Location = new System.Drawing.Point(161, 167);
            this.lblStartEventTime.Name = "lblStartEventTime";
            this.lblStartEventTime.Size = new System.Drawing.Size(55, 13);
            this.lblStartEventTime.TabIndex = 7;
            this.lblStartEventTime.Text = "00:00.000";
            this.lblStartEventTime.Visible = false;
            // 
            // nudStartEvent
            // 
            this.nudStartEvent.Location = new System.Drawing.Point(104, 165);
            this.nudStartEvent.Name = "nudStartEvent";
            this.nudStartEvent.Size = new System.Drawing.Size(51, 20);
            this.nudStartEvent.TabIndex = 4;
            this.nudStartEvent.Visible = false;
            this.nudStartEvent.ValueChanged += new System.EventHandler(this.nudStartEvent_ValueChanged);
            // 
            // lblStartEvent
            // 
            this.lblStartEvent.AutoSize = true;
            this.lblStartEvent.Location = new System.Drawing.Point(38, 167);
            this.lblStartEvent.Name = "lblStartEvent";
            this.lblStartEvent.Size = new System.Drawing.Size(60, 13);
            this.lblStartEvent.TabIndex = 6;
            this.lblStartEvent.Text = "Start Event";
            this.lblStartEvent.Visible = false;
            // 
            // lblEventCountTime
            // 
            this.lblEventCountTime.AutoSize = true;
            this.lblEventCountTime.Location = new System.Drawing.Point(161, 193);
            this.lblEventCountTime.Name = "lblEventCountTime";
            this.lblEventCountTime.Size = new System.Drawing.Size(55, 13);
            this.lblEventCountTime.TabIndex = 9;
            this.lblEventCountTime.Text = "00:00.000";
            this.lblEventCountTime.Visible = false;
            // 
            // nudEventCount
            // 
            this.nudEventCount.Location = new System.Drawing.Point(104, 191);
            this.nudEventCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEventCount.Name = "nudEventCount";
            this.nudEventCount.Size = new System.Drawing.Size(51, 20);
            this.nudEventCount.TabIndex = 5;
            this.nudEventCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEventCount.Visible = false;
            this.nudEventCount.ValueChanged += new System.EventHandler(this.nudEventCount_ValueChanged);
            // 
            // lblEventCount
            // 
            this.lblEventCount.AutoSize = true;
            this.lblEventCount.Location = new System.Drawing.Point(6, 193);
            this.lblEventCount.Name = "lblEventCount";
            this.lblEventCount.Size = new System.Drawing.Size(92, 13);
            this.lblEventCount.TabIndex = 8;
            this.lblEventCount.Text = "Number of Events";
            this.lblEventCount.Visible = false;
            // 
            // rbSpecificPoint
            // 
            this.rbSpecificPoint.AutoSize = true;
            this.rbSpecificPoint.Location = new System.Drawing.Point(6, 86);
            this.rbSpecificPoint.Name = "rbSpecificPoint";
            this.rbSpecificPoint.Size = new System.Drawing.Size(154, 17);
            this.rbSpecificPoint.TabIndex = 3;
            this.rbSpecificPoint.Text = "Specific Point In Sequence";
            this.rbSpecificPoint.UseVisualStyleBackColor = true;
            this.rbSpecificPoint.CheckedChanged += new System.EventHandler(this.RenderToChanged);
            // 
            // rbCurrentSelection
            // 
            this.rbCurrentSelection.AutoSize = true;
            this.rbCurrentSelection.Location = new System.Drawing.Point(6, 63);
            this.rbCurrentSelection.Name = "rbCurrentSelection";
            this.rbCurrentSelection.Size = new System.Drawing.Size(158, 17);
            this.rbCurrentSelection.TabIndex = 2;
            this.rbCurrentSelection.Text = "Current Sequence Selection";
            this.rbCurrentSelection.UseVisualStyleBackColor = true;
            this.rbCurrentSelection.CheckedChanged += new System.EventHandler(this.RenderToChanged);
            // 
            // rbRoutine
            // 
            this.rbRoutine.AutoSize = true;
            this.rbRoutine.Location = new System.Drawing.Point(6, 19);
            this.rbRoutine.Name = "rbRoutine";
            this.rbRoutine.Size = new System.Drawing.Size(136, 17);
            this.rbRoutine.TabIndex = 0;
            this.rbRoutine.Text = "Vixen+ Routine (.vir file)";
            this.rbRoutine.UseVisualStyleBackColor = true;
            this.rbRoutine.CheckedChanged += new System.EventHandler(this.RenderToChanged);
            // 
            // rbClipboard
            // 
            this.rbClipboard.AutoSize = true;
            this.rbClipboard.Checked = true;
            this.rbClipboard.Location = new System.Drawing.Point(6, 42);
            this.rbClipboard.Name = "rbClipboard";
            this.rbClipboard.Size = new System.Drawing.Size(69, 17);
            this.rbClipboard.TabIndex = 1;
            this.rbClipboard.TabStop = true;
            this.rbClipboard.Text = "Clipboard";
            this.rbClipboard.UseVisualStyleBackColor = true;
            this.rbClipboard.CheckedChanged += new System.EventHandler(this.RenderToChanged);
            // 
            // chkBoxEnableRawPreview
            // 
            this.chkBoxEnableRawPreview.AutoSize = true;
            this.chkBoxEnableRawPreview.Checked = true;
            this.chkBoxEnableRawPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxEnableRawPreview.Location = new System.Drawing.Point(648, 152);
            this.chkBoxEnableRawPreview.Name = "chkBoxEnableRawPreview";
            this.chkBoxEnableRawPreview.Size = new System.Drawing.Size(125, 17);
            this.chkBoxEnableRawPreview.TabIndex = 6;
            this.chkBoxEnableRawPreview.Text = "Enable Raw Preview";
            this.chkBoxEnableRawPreview.UseVisualStyleBackColor = true;
            // 
            // pbRawPreview
            // 
            this.pbRawPreview.BackColor = System.Drawing.Color.Black;
            this.pbRawPreview.Location = new System.Drawing.Point(648, 25);
            this.pbRawPreview.Name = "pbRawPreview";
            this.pbRawPreview.Size = new System.Drawing.Size(120, 120);
            this.pbRawPreview.TabIndex = 11;
            this.pbRawPreview.TabStop = false;
            // 
            // btnLightsOff
            // 
            this.btnLightsOff.Enabled = false;
            this.btnLightsOff.Location = new System.Drawing.Point(648, 205);
            this.btnLightsOff.Name = "btnLightsOff";
            this.btnLightsOff.Size = new System.Drawing.Size(75, 23);
            this.btnLightsOff.TabIndex = 7;
            this.btnLightsOff.Text = "Lights Off";
            this.btnLightsOff.UseVisualStyleBackColor = true;
            this.btnLightsOff.Visible = false;
            // 
            // cbRender
            // 
            this.cbRender.AutoSize = true;
            this.cbRender.Enabled = false;
            this.cbRender.Location = new System.Drawing.Point(648, 234);
            this.cbRender.Name = "cbRender";
            this.cbRender.Size = new System.Drawing.Size(101, 17);
            this.cbRender.TabIndex = 8;
            this.cbRender.Text = "Output to Lights";
            this.cbRender.UseVisualStyleBackColor = true;
            this.cbRender.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(693, 257);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // timerRender
            // 
            this.timerRender.Interval = 50;
            this.timerRender.Tick += new System.EventHandler(this.timerRender_Tick);
            // 
            // lblChannels
            // 
            this.lblChannels.AutoSize = true;
            this.lblChannels.Location = new System.Drawing.Point(7, 141);
            this.lblChannels.Name = "lblChannels";
            this.lblChannels.Size = new System.Drawing.Size(46, 13);
            this.lblChannels.TabIndex = 10;
            this.lblChannels.Text = "Channel";
            // 
            // cbChannels
            // 
            this.cbChannels.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChannels.FormattingEnabled = true;
            this.cbChannels.Location = new System.Drawing.Point(59, 138);
            this.cbChannels.Name = "cbChannels";
            this.cbChannels.Size = new System.Drawing.Size(164, 21);
            this.cbChannels.TabIndex = 11;
            this.cbChannels.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbChannels_DrawItem);
            // 
            // nutcrackerEffectControl1
            // 
            this.nutcrackerEffectControl1.Location = new System.Drawing.Point(7, 20);
            this.nutcrackerEffectControl1.Name = "nutcrackerEffectControl1";
            this.nutcrackerEffectControl1.Size = new System.Drawing.Size(371, 225);
            this.nutcrackerEffectControl1.Speed = 1;
            this.nutcrackerEffectControl1.TabIndex = 0;
            this.nutcrackerEffectControl1.ControlChanged += new Nutcracker.NutcrackerEffectControl.ControlChangedHandler(this.ControlChanged1);
            // 
            // nutcrackerEffectControl2
            // 
            this.nutcrackerEffectControl2.Location = new System.Drawing.Point(7, 20);
            this.nutcrackerEffectControl2.Name = "nutcrackerEffectControl2";
            this.nutcrackerEffectControl2.Size = new System.Drawing.Size(371, 225);
            this.nutcrackerEffectControl2.Speed = 1;
            this.nutcrackerEffectControl2.TabIndex = 0;
            this.nutcrackerEffectControl2.ControlChanged += new Nutcracker.NutcrackerEffectControl.ControlChangedHandler(this.ControlChanged2);
            // 
            // NutcrackerControlDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 562);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.gbEffect1);
            this.Controls.Add(this.gbEffect2);
            this.Controls.Add(this.pbPreview);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NutcrackerControlDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate Nutcracker Effect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NutcrackerControlDialog_FormClosing);
            this.gbEffect2.ResumeLayout(false);
            this.gbEffect1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.gbModel.ResumeLayout(false);
            this.gbModel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSparkles)).EndInit();
            this.gbLayer.ResumeLayout(false);
            this.gbLayer.PerformLayout();
            this.gbRenderTo.ResumeLayout(false);
            this.gbRenderTo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartEvent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEventCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRawPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEffect2;
        private System.Windows.Forms.GroupBox gbEffect1;
        private System.Windows.Forms.PictureBox pbPreview;
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