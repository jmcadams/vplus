namespace VixenPlus{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	internal sealed partial class VixenPlusForm{
		private IContainer components;

		#region Windows Form Designer generated code
		private ToolStripMenuItem aboutToolStripMenuItem1;
		private ToolStripMenuItem cascadeToolStripMenuItem;
		private ToolStripMenuItem channelDimmingCurvesToolStripMenuItem;
		private ToolStripMenuItem checkForUpdatesToolStripMenuItem;
		private ToolStripMenuItem contentsToolStripMenuItem;
		private ToolStripMenuItem copyASequenceToolStripMenuItem;
		private ToolStripMenuItem copyChannelColorsToolStripMenuItem;
		private ToolStripMenuItem copyPluginaddinDataToolStripMenuItem;
		private ToolStripMenuItem diagnosticsToolStripMenuItem;
		private HelpProvider helpProvider;
		private ToolStripMenuItem helpToolStripMenuItem;
		private ToolStripMenuItem manageToolStripMenuItem;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem musicPlayerToolStripMenuItem;
		private ToolStripMenuItem newLightingProgramToolStripMenuItem;
		private ToolStripMenuItem onlineSupportForumToolStripMenuItem;
		private ToolStripMenuItem openALightingProgramToolStripMenuItem;
		private OpenFileDialog openFileDialog1;
		private ToolStripMenuItem preferencesToolStripMenuItem;
		private ToolStripMenuItem viewToolStripMenuItem;
		private ToolStripMenuItem programToolStripMenuItem;
		private ToolStripMenuItem recentToolStripMenuItem;
		private ToolStripMenuItem saveAsToolStripMenuItem;
		private SaveFileDialog saveFileDialog1;
		private ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.Timer scheduleTimer;
		private ToolStripMenuItem setBackgroundSequenceToolStripMenuItem;
		private ToolStripMenuItem setSoundDeviceToolStripMenuItem;
		private System.Windows.Forms.Timer shutdownTimer;
		private StatusStrip statusStrip;
		private ToolStripMenuItem tileToolStripMenuItem;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripSeparator toolStripMenuItem4;
		private ToolStripSeparator toolStripMenuItem6;
		private ToolStripSeparator toolStripMenuItem8;
		private ToolStripProgressBar toolStripProgressBarBackgroundSequenceRunning;
		private ToolStripStatusLabel toolStripStatusLabelMusic;
		private ToolStripMenuItem turnOnQueryServerToolStripMenuItem;
		private ToolStripMenuItem utilityToolStripMenuItem;
		private ToolStripMenuItem visualChannelLayoutToolStripMenuItem;
		private ToolStripMenuItem windowsToolStripMenuItem;

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newLightingProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openALightingProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.setBackgroundSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channelDimmingCurvesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyASequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyChannelColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPluginaddinDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.setSoundDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.musicPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualChannelLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diagnosticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOnQueryServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineSupportForumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.scheduleTimer = new System.Windows.Forms.Timer(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBarBackgroundSequenceRunning = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelMusic = new System.Windows.Forms.ToolStripStatusLabel();
            this.shutdownTimer = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.utilityToolStripMenuItem,
            this.windowsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MdiWindowListItem = this.windowsToolStripMenuItem;
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(776, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newLightingProgramToolStripMenuItem,
            this.openALightingProgramToolStripMenuItem,
            this.toolStripMenuItem2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem8,
            this.setBackgroundSequenceToolStripMenuItem,
            this.channelDimmingCurvesToolStripMenuItem,
            this.toolStripMenuItem6,
            this.recentToolStripMenuItem});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.programToolStripMenuItem.Text = "File";
            this.programToolStripMenuItem.DropDownOpening += new System.EventHandler(this.programToolStripMenuItem_DropDownOpening);
            // 
            // newLightingProgramToolStripMenuItem
            // 
            this.newLightingProgramToolStripMenuItem.Name = "newLightingProgramToolStripMenuItem";
            this.newLightingProgramToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.newLightingProgramToolStripMenuItem.Text = "New event sequence";
            // 
            // openALightingProgramToolStripMenuItem
            // 
            this.openALightingProgramToolStripMenuItem.Name = "openALightingProgramToolStripMenuItem";
            this.openALightingProgramToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openALightingProgramToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.openALightingProgramToolStripMenuItem.Text = "Open an event sequence";
            this.openALightingProgramToolStripMenuItem.Click += new System.EventHandler(this.openALightingProgramToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(244, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(244, 6);
            // 
            // setBackgroundSequenceToolStripMenuItem
            // 
            this.setBackgroundSequenceToolStripMenuItem.Name = "setBackgroundSequenceToolStripMenuItem";
            this.setBackgroundSequenceToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.setBackgroundSequenceToolStripMenuItem.Text = "Set background sequence";
            this.setBackgroundSequenceToolStripMenuItem.Visible = false;
            this.setBackgroundSequenceToolStripMenuItem.Click += new System.EventHandler(this.setBackgroundSequenceToolStripMenuItem_Click);
            // 
            // channelDimmingCurvesToolStripMenuItem
            // 
            this.channelDimmingCurvesToolStripMenuItem.Name = "channelDimmingCurvesToolStripMenuItem";
            this.channelDimmingCurvesToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.channelDimmingCurvesToolStripMenuItem.Text = "Channel dimming curves";
            this.channelDimmingCurvesToolStripMenuItem.Visible = false;
            this.channelDimmingCurvesToolStripMenuItem.Click += new System.EventHandler(this.channelDimmingCurvesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(244, 6);
            this.toolStripMenuItem6.Visible = false;
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.recentToolStripMenuItem.Text = "Recent";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // manageToolStripMenuItem
            // 
            this.manageToolStripMenuItem.Name = "manageToolStripMenuItem";
            this.manageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.manageToolStripMenuItem.Text = "Manage";
            this.manageToolStripMenuItem.Click += new System.EventHandler(this.manageToolStripMenuItem_Click);
            // 
            // utilityToolStripMenuItem
            // 
            this.utilityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyASequenceToolStripMenuItem,
            this.copyChannelColorsToolStripMenuItem,
            this.copyPluginaddinDataToolStripMenuItem,
            this.toolStripMenuItem4,
            this.setSoundDeviceToolStripMenuItem,
            this.musicPlayerToolStripMenuItem,
            this.visualChannelLayoutToolStripMenuItem,
            this.diagnosticsToolStripMenuItem,
            this.turnOnQueryServerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.preferencesToolStripMenuItem});
            this.utilityToolStripMenuItem.Name = "utilityToolStripMenuItem";
            this.utilityToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.utilityToolStripMenuItem.Text = "Tools";
            // 
            // copyASequenceToolStripMenuItem
            // 
            this.copyASequenceToolStripMenuItem.Name = "copyASequenceToolStripMenuItem";
            this.copyASequenceToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.copyASequenceToolStripMenuItem.Text = "Copy sequence event data";
            this.copyASequenceToolStripMenuItem.Visible = false;
            this.copyASequenceToolStripMenuItem.Click += new System.EventHandler(this.copyASequenceToolStripMenuItem_Click);
            // 
            // copyChannelColorsToolStripMenuItem
            // 
            this.copyChannelColorsToolStripMenuItem.Name = "copyChannelColorsToolStripMenuItem";
            this.copyChannelColorsToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.copyChannelColorsToolStripMenuItem.Text = "Copy channel colors";
            this.copyChannelColorsToolStripMenuItem.Visible = false;
            this.copyChannelColorsToolStripMenuItem.Click += new System.EventHandler(this.copyChannelColorsToolStripMenuItem_Click);
            // 
            // copyPluginaddinDataToolStripMenuItem
            // 
            this.copyPluginaddinDataToolStripMenuItem.Name = "copyPluginaddinDataToolStripMenuItem";
            this.copyPluginaddinDataToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.copyPluginaddinDataToolStripMenuItem.Text = "Copy data";
            this.copyPluginaddinDataToolStripMenuItem.Visible = false;
            this.copyPluginaddinDataToolStripMenuItem.Click += new System.EventHandler(this.copyPluginaddinDataToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(259, 6);
            this.toolStripMenuItem4.Visible = false;
            // 
            // setSoundDeviceToolStripMenuItem
            // 
            this.setSoundDeviceToolStripMenuItem.Name = "setSoundDeviceToolStripMenuItem";
            this.setSoundDeviceToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.setSoundDeviceToolStripMenuItem.Text = "Set application default audio device";
            this.setSoundDeviceToolStripMenuItem.Click += new System.EventHandler(this.setSoundDeviceToolStripMenuItem_Click);
            // 
            // musicPlayerToolStripMenuItem
            // 
            this.musicPlayerToolStripMenuItem.Name = "musicPlayerToolStripMenuItem";
            this.musicPlayerToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.musicPlayerToolStripMenuItem.Text = "Music player";
            this.musicPlayerToolStripMenuItem.Click += new System.EventHandler(this.musicPlayerToolStripMenuItem_Click);
            // 
            // visualChannelLayoutToolStripMenuItem
            // 
            this.visualChannelLayoutToolStripMenuItem.Name = "visualChannelLayoutToolStripMenuItem";
            this.visualChannelLayoutToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.visualChannelLayoutToolStripMenuItem.Text = "Visual channel layout";
            this.visualChannelLayoutToolStripMenuItem.Visible = false;
            this.visualChannelLayoutToolStripMenuItem.Click += new System.EventHandler(this.visualChannelLayoutToolStripMenuItem_Click);
            // 
            // diagnosticsToolStripMenuItem
            // 
            this.diagnosticsToolStripMenuItem.Name = "diagnosticsToolStripMenuItem";
            this.diagnosticsToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.diagnosticsToolStripMenuItem.Text = "Diagnostics";
            this.diagnosticsToolStripMenuItem.Visible = false;
            this.diagnosticsToolStripMenuItem.Click += new System.EventHandler(this.diagnosticsToolStripMenuItem_Click);
            // 
            // turnOnQueryServerToolStripMenuItem
            // 
            this.turnOnQueryServerToolStripMenuItem.Name = "turnOnQueryServerToolStripMenuItem";
            this.turnOnQueryServerToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.turnOnQueryServerToolStripMenuItem.Text = "Query server";
            this.turnOnQueryServerToolStripMenuItem.Visible = false;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(259, 6);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.preferencesToolStripMenuItem.MergeIndex = 99;
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tileToolStripMenuItem,
            this.cascadeToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.windowsToolStripMenuItem.Text = "Windows";
            // 
            // tileToolStripMenuItem
            // 
            this.tileToolStripMenuItem.Name = "tileToolStripMenuItem";
            this.tileToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.tileToolStripMenuItem.Text = "Tile";
            this.tileToolStripMenuItem.Click += new System.EventHandler(this.tileToolStripMenuItem_Click);
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.cascadeToolStripMenuItem.Text = "Cascade";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.cascadeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.onlineSupportForumToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.toolStripMenuItem3,
            this.aboutToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Enabled = false;
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.ShortcutKeyDisplayString = "F1";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.contentsToolStripMenuItem.Text = "Contents";
            this.contentsToolStripMenuItem.Visible = false;
            // 
            // onlineSupportForumToolStripMenuItem
            // 
            this.onlineSupportForumToolStripMenuItem.Name = "onlineSupportForumToolStripMenuItem";
            this.onlineSupportForumToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.onlineSupportForumToolStripMenuItem.Text = "Online support forum";
            this.onlineSupportForumToolStripMenuItem.Click += new System.EventHandler(this.onlineSupportForumToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            this.checkForUpdatesToolStripMenuItem.Visible = false;
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(186, 6);
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(189, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "vix";
            this.openFileDialog1.Filter = "Vixen event sequence | *.vix";
            this.openFileDialog1.InitialDirectory = "Sequences";
            // 
            // scheduleTimer
            // 
            this.scheduleTimer.Interval = 10000;
            this.scheduleTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBarBackgroundSequenceRunning,
            this.toolStripStatusLabelMusic});
            this.statusStrip.Location = new System.Drawing.Point(0, 467);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.ShowItemToolTips = true;
            this.statusStrip.Size = new System.Drawing.Size(776, 22);
            this.statusStrip.TabIndex = 3;
            // 
            // toolStripProgressBarBackgroundSequenceRunning
            // 
            this.toolStripProgressBarBackgroundSequenceRunning.Enabled = false;
            this.toolStripProgressBarBackgroundSequenceRunning.Name = "toolStripProgressBarBackgroundSequenceRunning";
            this.toolStripProgressBarBackgroundSequenceRunning.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBarBackgroundSequenceRunning.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBarBackgroundSequenceRunning.Visible = false;
            // 
            // toolStripStatusLabelMusic
            // 
            this.toolStripStatusLabelMusic.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripStatusLabelMusic.Name = "toolStripStatusLabelMusic";
            this.toolStripStatusLabelMusic.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabelMusic.Text = "   ";
            this.toolStripStatusLabelMusic.ToolTipText = "Background music is playing";
            this.toolStripStatusLabelMusic.Visible = false;
            // 
            // shutdownTimer
            // 
            this.shutdownTimer.Interval = 10000;
            this.shutdownTimer.Tick += new System.EventHandler(this.shutdownTimer_Tick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.SupportMultiDottedExtensions = true;
            // 
            // VixenPlusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 489);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.TableOfContents);
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VixenPlusForm";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VixenPlus";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.MdiChildActivate += new System.EventHandler(this.Form1_MdiChildActivate);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
