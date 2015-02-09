using System.ComponentModel;
using System.Windows.Forms;

partial class RGBTreeSetupDialog
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
        if (this._mBrush != null)
            this._mBrush.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.btn_setupOK = new System.Windows.Forms.Button();
        this.controlPanel = new System.Windows.Forms.Panel();
        this.lbl_direction = new System.Windows.Forms.Label();
        this.comboDirection = new System.Windows.Forms.ComboBox();
        this.comboClock = new System.Windows.Forms.ComboBox();
        this.tb_Requested = new System.Windows.Forms.TextBox();
        this.tb_Available = new System.Windows.Forms.TextBox();
        this.lbl_Requested = new System.Windows.Forms.Label();
        this.lbl_available = new System.Windows.Forms.Label();
        this.group_Type = new System.Windows.Forms.GroupBox();
        this.rb_updown_180 = new System.Windows.Forms.RadioButton();
        this.rb_updown_next = new System.Windows.Forms.RadioButton();
        this.rb_bottom_top = new System.Windows.Forms.RadioButton();
        this.track_posY = new System.Windows.Forms.TrackBar();
        this.tb_posY = new System.Windows.Forms.TextBox();
        this.lbl_posY = new System.Windows.Forms.Label();
        this.lbl_posX = new System.Windows.Forms.Label();
        this.tb_posX = new System.Windows.Forms.TextBox();
        this.track_posX = new System.Windows.Forms.TrackBar();
        this.track_PixelSize = new System.Windows.Forms.TrackBar();
        this.lbl_pixelSize = new System.Windows.Forms.Label();
        this.tb_pixelSize = new System.Windows.Forms.TextBox();
        this.btn_Create = new System.Windows.Forms.Button();
        this.tb_noPixels = new System.Windows.Forms.TextBox();
        this.tb_noStrings = new System.Windows.Forms.TextBox();
        this.lbl_startPos = new System.Windows.Forms.Label();
        this.lbl_noPixels = new System.Windows.Forms.Label();
        this.lbl_noStrings = new System.Windows.Forms.Label();
        this.btn_Cancel = new System.Windows.Forms.Button();
        this.track_Height = new System.Windows.Forms.TrackBar();
        this.track_Ellipse_X = new System.Windows.Forms.TrackBar();
        this.track_Ellipse_Y = new System.Windows.Forms.TrackBar();
        this.panel1 = new System.Windows.Forms.Panel();
        this.panel3 = new System.Windows.Forms.Panel();
        this.panel4 = new System.Windows.Forms.Panel();
        this.textBox4 = new System.Windows.Forms.TextBox();
        this.textBox3 = new System.Windows.Forms.TextBox();
        this.textBox2 = new System.Windows.Forms.TextBox();
        this.panel2 = new System.Windows.Forms.Panel();
        this.tb_Ellipse_Y = new System.Windows.Forms.TextBox();
        this.tb_Ellipse_X = new System.Windows.Forms.TextBox();
        this.tb_Height = new System.Windows.Forms.TextBox();
        this.pictureBox = new System.Windows.Forms.PictureBox();
        this.controlPanel.SuspendLayout();
        this.group_Type.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.track_posY)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.track_posX)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.track_PixelSize)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.track_Height)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.track_Ellipse_X)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.track_Ellipse_Y)).BeginInit();
        this.panel1.SuspendLayout();
        this.panel3.SuspendLayout();
        this.panel4.SuspendLayout();
        this.panel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
        this.SuspendLayout();
        // 
        // btn_setupOK
        // 
        this.btn_setupOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.btn_setupOK.Location = new System.Drawing.Point(91, 611);
        this.btn_setupOK.Name = "btn_setupOK";
        this.btn_setupOK.Size = new System.Drawing.Size(75, 23);
        this.btn_setupOK.TabIndex = 0;
        this.btn_setupOK.Text = "OK";
        this.btn_setupOK.UseVisualStyleBackColor = true;
        this.btn_setupOK.Click += new System.EventHandler(this.btn_setupOK_Click);
        // 
        // controlPanel
        // 
        this.controlPanel.BackColor = System.Drawing.SystemColors.Control;
        this.controlPanel.Controls.Add(this.lbl_direction);
        this.controlPanel.Controls.Add(this.comboDirection);
        this.controlPanel.Controls.Add(this.comboClock);
        this.controlPanel.Controls.Add(this.tb_Requested);
        this.controlPanel.Controls.Add(this.tb_Available);
        this.controlPanel.Controls.Add(this.lbl_Requested);
        this.controlPanel.Controls.Add(this.lbl_available);
        this.controlPanel.Controls.Add(this.group_Type);
        this.controlPanel.Controls.Add(this.track_posY);
        this.controlPanel.Controls.Add(this.tb_posY);
        this.controlPanel.Controls.Add(this.lbl_posY);
        this.controlPanel.Controls.Add(this.lbl_posX);
        this.controlPanel.Controls.Add(this.tb_posX);
        this.controlPanel.Controls.Add(this.track_posX);
        this.controlPanel.Controls.Add(this.track_PixelSize);
        this.controlPanel.Controls.Add(this.lbl_pixelSize);
        this.controlPanel.Controls.Add(this.tb_pixelSize);
        this.controlPanel.Controls.Add(this.btn_Create);
        this.controlPanel.Controls.Add(this.tb_noPixels);
        this.controlPanel.Controls.Add(this.tb_noStrings);
        this.controlPanel.Controls.Add(this.lbl_startPos);
        this.controlPanel.Controls.Add(this.lbl_noPixels);
        this.controlPanel.Controls.Add(this.lbl_noStrings);
        this.controlPanel.Controls.Add(this.btn_Cancel);
        this.controlPanel.Controls.Add(this.btn_setupOK);
        this.controlPanel.Dock = System.Windows.Forms.DockStyle.Right;
        this.controlPanel.Location = new System.Drawing.Point(987, 0);
        this.controlPanel.Name = "controlPanel";
        this.controlPanel.Size = new System.Drawing.Size(180, 637);
        this.controlPanel.TabIndex = 2;
        // 
        // lbl_direction
        // 
        this.lbl_direction.AutoSize = true;
        this.lbl_direction.Location = new System.Drawing.Point(15, 110);
        this.lbl_direction.Name = "lbl_direction";
        this.lbl_direction.Size = new System.Drawing.Size(49, 13);
        this.lbl_direction.TabIndex = 28;
        this.lbl_direction.Text = "Direction";
        // 
        // comboDirection
        // 
        this.comboDirection.FormattingEnabled = true;
        this.comboDirection.Items.AddRange(new object[] {
            "CCW",
            "CW"});
        this.comboDirection.Location = new System.Drawing.Point(111, 107);
        this.comboDirection.Name = "comboDirection";
        this.comboDirection.Size = new System.Drawing.Size(55, 21);
        this.comboDirection.TabIndex = 27;
        this.comboDirection.SelectedIndexChanged += new System.EventHandler(this.comboDirection_SelectedIndexChanged);
        // 
        // comboClock
        // 
        this.comboClock.FormattingEnabled = true;
        this.comboClock.Items.AddRange(new object[] {
            "12:00",
            "3:00",
            "6:00",
            "9:00"});
        this.comboClock.Location = new System.Drawing.Point(111, 80);
        this.comboClock.Name = "comboClock";
        this.comboClock.Size = new System.Drawing.Size(55, 21);
        this.comboClock.TabIndex = 26;
        this.comboClock.SelectedIndexChanged += new System.EventHandler(this.comboClock_SelectedIndexChanged);
        // 
        // tb_Requested
        // 
        this.tb_Requested.Location = new System.Drawing.Point(90, 560);
        this.tb_Requested.Name = "tb_Requested";
        this.tb_Requested.ReadOnly = true;
        this.tb_Requested.Size = new System.Drawing.Size(76, 20);
        this.tb_Requested.TabIndex = 25;
        // 
        // tb_Available
        // 
        this.tb_Available.Location = new System.Drawing.Point(90, 529);
        this.tb_Available.Name = "tb_Available";
        this.tb_Available.ReadOnly = true;
        this.tb_Available.Size = new System.Drawing.Size(76, 20);
        this.tb_Available.TabIndex = 24;
        // 
        // lbl_Requested
        // 
        this.lbl_Requested.AutoSize = true;
        this.lbl_Requested.Location = new System.Drawing.Point(15, 563);
        this.lbl_Requested.Name = "lbl_Requested";
        this.lbl_Requested.Size = new System.Drawing.Size(59, 13);
        this.lbl_Requested.TabIndex = 23;
        this.lbl_Requested.Text = "Requested";
        // 
        // lbl_available
        // 
        this.lbl_available.AutoSize = true;
        this.lbl_available.Location = new System.Drawing.Point(15, 532);
        this.lbl_available.Name = "lbl_available";
        this.lbl_available.Size = new System.Drawing.Size(69, 13);
        this.lbl_available.TabIndex = 22;
        this.lbl_available.Text = "Available Ch.";
        // 
        // group_Type
        // 
        this.group_Type.Controls.Add(this.rb_updown_180);
        this.group_Type.Controls.Add(this.rb_updown_next);
        this.group_Type.Controls.Add(this.rb_bottom_top);
        this.group_Type.Location = new System.Drawing.Point(18, 373);
        this.group_Type.Name = "group_Type";
        this.group_Type.Size = new System.Drawing.Size(148, 100);
        this.group_Type.TabIndex = 21;
        this.group_Type.TabStop = false;
        this.group_Type.Text = "Tree Type";
        // 
        // rb_updown_180
        // 
        this.rb_updown_180.AutoSize = true;
        this.rb_updown_180.Location = new System.Drawing.Point(7, 68);
        this.rb_updown_180.Name = "rb_updown_180";
        this.rb_updown_180.Size = new System.Drawing.Size(91, 17);
        this.rb_updown_180.TabIndex = 2;
        this.rb_updown_180.Tag = "rb_updown_180";
        this.rb_updown_180.Text = "Up Down 180";
        this.rb_updown_180.UseVisualStyleBackColor = true;
        // 
        // rb_updown_next
        // 
        this.rb_updown_next.AutoSize = true;
        this.rb_updown_next.Location = new System.Drawing.Point(7, 44);
        this.rb_updown_next.Name = "rb_updown_next";
        this.rb_updown_next.Size = new System.Drawing.Size(95, 17);
        this.rb_updown_next.TabIndex = 1;
        this.rb_updown_next.Tag = "rb_updown_next";
        this.rb_updown_next.Text = "Up Down Next";
        this.rb_updown_next.UseVisualStyleBackColor = true;
        // 
        // rb_bottom_top
        // 
        this.rb_bottom_top.AutoSize = true;
        this.rb_bottom_top.Location = new System.Drawing.Point(7, 20);
        this.rb_bottom_top.Name = "rb_bottom_top";
        this.rb_bottom_top.Size = new System.Drawing.Size(92, 17);
        this.rb_bottom_top.TabIndex = 0;
        this.rb_bottom_top.Tag = "rb_bottom_top";
        this.rb_bottom_top.Text = "Bottom to Top";
        this.rb_bottom_top.UseVisualStyleBackColor = true;
        // 
        // track_posY
        // 
        this.track_posY.Location = new System.Drawing.Point(18, 321);
        this.track_posY.Minimum = 1;
        this.track_posY.Name = "track_posY";
        this.track_posY.Size = new System.Drawing.Size(148, 45);
        this.track_posY.TabIndex = 20;
        this.track_posY.Value = 1;
        this.track_posY.Scroll += new System.EventHandler(this.track_posY_Scroll);
        // 
        // tb_posY
        // 
        this.tb_posY.Location = new System.Drawing.Point(111, 285);
        this.tb_posY.Name = "tb_posY";
        this.tb_posY.ReadOnly = true;
        this.tb_posY.Size = new System.Drawing.Size(55, 20);
        this.tb_posY.TabIndex = 19;
        this.tb_posY.Text = "1";
        // 
        // lbl_posY
        // 
        this.lbl_posY.AutoSize = true;
        this.lbl_posY.Location = new System.Drawing.Point(15, 293);
        this.lbl_posY.Name = "lbl_posY";
        this.lbl_posY.Size = new System.Drawing.Size(57, 13);
        this.lbl_posY.TabIndex = 18;
        this.lbl_posY.Text = "Y Posittion";
        // 
        // lbl_posX
        // 
        this.lbl_posX.AutoSize = true;
        this.lbl_posX.Location = new System.Drawing.Point(15, 222);
        this.lbl_posX.Name = "lbl_posX";
        this.lbl_posX.Size = new System.Drawing.Size(53, 13);
        this.lbl_posX.TabIndex = 17;
        this.lbl_posX.Text = "X position";
        // 
        // tb_posX
        // 
        this.tb_posX.Location = new System.Drawing.Point(111, 219);
        this.tb_posX.Name = "tb_posX";
        this.tb_posX.ReadOnly = true;
        this.tb_posX.Size = new System.Drawing.Size(55, 20);
        this.tb_posX.TabIndex = 16;
        this.tb_posX.Text = "1";
        // 
        // track_posX
        // 
        this.track_posX.Location = new System.Drawing.Point(18, 245);
        this.track_posX.Minimum = 1;
        this.track_posX.Name = "track_posX";
        this.track_posX.Size = new System.Drawing.Size(148, 45);
        this.track_posX.TabIndex = 15;
        this.track_posX.Tag = "X";
        this.track_posX.Value = 1;
        this.track_posX.Scroll += new System.EventHandler(this.track_posX_Scroll);
        // 
        // track_PixelSize
        // 
        this.track_PixelSize.Location = new System.Drawing.Point(18, 174);
        this.track_PixelSize.Minimum = 1;
        this.track_PixelSize.Name = "track_PixelSize";
        this.track_PixelSize.Size = new System.Drawing.Size(148, 45);
        this.track_PixelSize.TabIndex = 14;
        this.track_PixelSize.Value = 1;
        this.track_PixelSize.Scroll += new System.EventHandler(this.track_PixelSize_Scroll);
        // 
        // lbl_pixelSize
        // 
        this.lbl_pixelSize.AutoSize = true;
        this.lbl_pixelSize.Location = new System.Drawing.Point(15, 151);
        this.lbl_pixelSize.Name = "lbl_pixelSize";
        this.lbl_pixelSize.Size = new System.Drawing.Size(52, 13);
        this.lbl_pixelSize.TabIndex = 13;
        this.lbl_pixelSize.Text = "Pixel Size";
        // 
        // tb_pixelSize
        // 
        this.tb_pixelSize.Location = new System.Drawing.Point(111, 148);
        this.tb_pixelSize.Name = "tb_pixelSize";
        this.tb_pixelSize.ReadOnly = true;
        this.tb_pixelSize.Size = new System.Drawing.Size(55, 20);
        this.tb_pixelSize.TabIndex = 12;
        this.tb_pixelSize.Text = "1";
        // 
        // btn_Create
        // 
        this.btn_Create.Location = new System.Drawing.Point(56, 479);
        this.btn_Create.Name = "btn_Create";
        this.btn_Create.Size = new System.Drawing.Size(75, 23);
        this.btn_Create.TabIndex = 8;
        this.btn_Create.Text = "Create";
        this.btn_Create.UseVisualStyleBackColor = true;
        this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
        // 
        // tb_noPixels
        // 
        this.tb_noPixels.Location = new System.Drawing.Point(111, 50);
        this.tb_noPixels.Name = "tb_noPixels";
        this.tb_noPixels.Size = new System.Drawing.Size(55, 20);
        this.tb_noPixels.TabIndex = 6;
        this.tb_noPixels.Text = "1";
        this.tb_noPixels.TextChanged += new System.EventHandler(this.tb_noPixels_TextChanged);
        // 
        // tb_noStrings
        // 
        this.tb_noStrings.Location = new System.Drawing.Point(111, 21);
        this.tb_noStrings.Name = "tb_noStrings";
        this.tb_noStrings.Size = new System.Drawing.Size(55, 20);
        this.tb_noStrings.TabIndex = 5;
        this.tb_noStrings.Text = "1";
        this.tb_noStrings.TextChanged += new System.EventHandler(this.tb_noStrings_TextChanged);
        // 
        // lbl_startPos
        // 
        this.lbl_startPos.AutoSize = true;
        this.lbl_startPos.Location = new System.Drawing.Point(15, 83);
        this.lbl_startPos.Name = "lbl_startPos";
        this.lbl_startPos.Size = new System.Drawing.Size(69, 13);
        this.lbl_startPos.TabIndex = 4;
        this.lbl_startPos.Text = "Start Position";
        // 
        // lbl_noPixels
        // 
        this.lbl_noPixels.AutoSize = true;
        this.lbl_noPixels.Location = new System.Drawing.Point(12, 53);
        this.lbl_noPixels.Name = "lbl_noPixels";
        this.lbl_noPixels.Size = new System.Drawing.Size(88, 13);
        this.lbl_noPixels.TabIndex = 3;
        this.lbl_noPixels.Text = "Number Of Pixels";
        // 
        // lbl_noStrings
        // 
        this.lbl_noStrings.AutoSize = true;
        this.lbl_noStrings.Location = new System.Drawing.Point(12, 28);
        this.lbl_noStrings.Name = "lbl_noStrings";
        this.lbl_noStrings.Size = new System.Drawing.Size(93, 13);
        this.lbl_noStrings.TabIndex = 2;
        this.lbl_noStrings.Text = "Number Of Strings";
        // 
        // btn_Cancel
        // 
        this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btn_Cancel.Location = new System.Drawing.Point(10, 611);
        this.btn_Cancel.Name = "btn_Cancel";
        this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
        this.btn_Cancel.TabIndex = 1;
        this.btn_Cancel.Text = "Cancel";
        this.btn_Cancel.UseVisualStyleBackColor = true;
        // 
        // track_Height
        // 
        this.track_Height.Dock = System.Windows.Forms.DockStyle.Right;
        this.track_Height.Location = new System.Drawing.Point(90, 0);
        this.track_Height.Maximum = 100;
        this.track_Height.Minimum = 2;
        this.track_Height.Name = "track_Height";
        this.track_Height.Orientation = System.Windows.Forms.Orientation.Vertical;
        this.track_Height.Size = new System.Drawing.Size(45, 591);
        this.track_Height.TabIndex = 3;
        this.track_Height.TickStyle = System.Windows.Forms.TickStyle.Both;
        this.track_Height.Value = 20;
        this.track_Height.Scroll += new System.EventHandler(this.track_Height_Scroll);
        // 
        // track_Ellipse_X
        // 
        this.track_Ellipse_X.Dock = System.Windows.Forms.DockStyle.Right;
        this.track_Ellipse_X.Location = new System.Drawing.Point(45, 0);
        this.track_Ellipse_X.Maximum = 60;
        this.track_Ellipse_X.Minimum = 1;
        this.track_Ellipse_X.Name = "track_Ellipse_X";
        this.track_Ellipse_X.Orientation = System.Windows.Forms.Orientation.Vertical;
        this.track_Ellipse_X.Size = new System.Drawing.Size(45, 591);
        this.track_Ellipse_X.TabIndex = 5;
        this.track_Ellipse_X.TickStyle = System.Windows.Forms.TickStyle.Both;
        this.track_Ellipse_X.Value = 24;
        this.track_Ellipse_X.Scroll += new System.EventHandler(this.track_Ellipse_X_Scroll);
        // 
        // track_Ellipse_Y
        // 
        this.track_Ellipse_Y.Dock = System.Windows.Forms.DockStyle.Right;
        this.track_Ellipse_Y.Location = new System.Drawing.Point(0, 0);
        this.track_Ellipse_Y.Maximum = 30;
        this.track_Ellipse_Y.Minimum = 1;
        this.track_Ellipse_Y.Name = "track_Ellipse_Y";
        this.track_Ellipse_Y.Orientation = System.Windows.Forms.Orientation.Vertical;
        this.track_Ellipse_Y.Size = new System.Drawing.Size(45, 591);
        this.track_Ellipse_Y.TabIndex = 6;
        this.track_Ellipse_Y.TickStyle = System.Windows.Forms.TickStyle.Both;
        this.track_Ellipse_Y.Value = 8;
        this.track_Ellipse_Y.Scroll += new System.EventHandler(this.track_Ellipse_Y_Scroll);
        // 
        // panel1
        // 
        this.panel1.Controls.Add(this.panel3);
        this.panel1.Controls.Add(this.panel4);
        this.panel1.Controls.Add(this.panel2);
        this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
        this.panel1.Location = new System.Drawing.Point(852, 0);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(135, 637);
        this.panel1.TabIndex = 9;
        // 
        // panel3
        // 
        this.panel3.Controls.Add(this.track_Ellipse_Y);
        this.panel3.Controls.Add(this.track_Ellipse_X);
        this.panel3.Controls.Add(this.track_Height);
        this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel3.Location = new System.Drawing.Point(0, 23);
        this.panel3.Name = "panel3";
        this.panel3.Size = new System.Drawing.Size(135, 591);
        this.panel3.TabIndex = 1;
        // 
        // panel4
        // 
        this.panel4.Controls.Add(this.textBox4);
        this.panel4.Controls.Add(this.textBox3);
        this.panel4.Controls.Add(this.textBox2);
        this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panel4.Location = new System.Drawing.Point(0, 614);
        this.panel4.Name = "panel4";
        this.panel4.Size = new System.Drawing.Size(135, 23);
        this.panel4.TabIndex = 1;
        // 
        // textBox4
        // 
        this.textBox4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
        this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.textBox4.Dock = System.Windows.Forms.DockStyle.Right;
        this.textBox4.ForeColor = System.Drawing.SystemColors.MenuBar;
        this.textBox4.Location = new System.Drawing.Point(0, 0);
        this.textBox4.Name = "textBox4";
        this.textBox4.ReadOnly = true;
        this.textBox4.Size = new System.Drawing.Size(45, 20);
        this.textBox4.TabIndex = 2;
        this.textBox4.Text = "ValueY";
        this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // textBox3
        // 
        this.textBox3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
        this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.textBox3.Dock = System.Windows.Forms.DockStyle.Right;
        this.textBox3.ForeColor = System.Drawing.SystemColors.MenuBar;
        this.textBox3.Location = new System.Drawing.Point(45, 0);
        this.textBox3.Name = "textBox3";
        this.textBox3.ReadOnly = true;
        this.textBox3.Size = new System.Drawing.Size(45, 20);
        this.textBox3.TabIndex = 1;
        this.textBox3.Text = "ValueX";
        this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // textBox2
        // 
        this.textBox2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
        this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.textBox2.Dock = System.Windows.Forms.DockStyle.Right;
        this.textBox2.ForeColor = System.Drawing.SystemColors.MenuBar;
        this.textBox2.Location = new System.Drawing.Point(90, 0);
        this.textBox2.Name = "textBox2";
        this.textBox2.ReadOnly = true;
        this.textBox2.Size = new System.Drawing.Size(45, 20);
        this.textBox2.TabIndex = 0;
        this.textBox2.Text = "Height";
        this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // panel2
        // 
        this.panel2.Controls.Add(this.tb_Ellipse_Y);
        this.panel2.Controls.Add(this.tb_Ellipse_X);
        this.panel2.Controls.Add(this.tb_Height);
        this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
        this.panel2.Location = new System.Drawing.Point(0, 0);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(135, 23);
        this.panel2.TabIndex = 0;
        // 
        // tb_Ellipse_Y
        // 
        this.tb_Ellipse_Y.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
        this.tb_Ellipse_Y.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.tb_Ellipse_Y.Dock = System.Windows.Forms.DockStyle.Right;
        this.tb_Ellipse_Y.ForeColor = System.Drawing.SystemColors.MenuBar;
        this.tb_Ellipse_Y.Location = new System.Drawing.Point(0, 0);
        this.tb_Ellipse_Y.Name = "tb_Ellipse_Y";
        this.tb_Ellipse_Y.ReadOnly = true;
        this.tb_Ellipse_Y.Size = new System.Drawing.Size(45, 20);
        this.tb_Ellipse_Y.TabIndex = 10;
        this.tb_Ellipse_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // tb_Ellipse_X
        // 
        this.tb_Ellipse_X.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
        this.tb_Ellipse_X.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.tb_Ellipse_X.Dock = System.Windows.Forms.DockStyle.Right;
        this.tb_Ellipse_X.ForeColor = System.Drawing.SystemColors.MenuBar;
        this.tb_Ellipse_X.Location = new System.Drawing.Point(45, 0);
        this.tb_Ellipse_X.Name = "tb_Ellipse_X";
        this.tb_Ellipse_X.ReadOnly = true;
        this.tb_Ellipse_X.Size = new System.Drawing.Size(45, 20);
        this.tb_Ellipse_X.TabIndex = 11;
        this.tb_Ellipse_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // tb_Height
        // 
        this.tb_Height.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
        this.tb_Height.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.tb_Height.Dock = System.Windows.Forms.DockStyle.Right;
        this.tb_Height.ForeColor = System.Drawing.SystemColors.MenuBar;
        this.tb_Height.Location = new System.Drawing.Point(90, 0);
        this.tb_Height.Name = "tb_Height";
        this.tb_Height.ReadOnly = true;
        this.tb_Height.Size = new System.Drawing.Size(45, 20);
        this.tb_Height.TabIndex = 12;
        this.tb_Height.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // pictureBox
        // 
        this.pictureBox.BackColor = System.Drawing.Color.Transparent;
        this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pictureBox.Location = new System.Drawing.Point(0, 0);
        this.pictureBox.Name = "pictureBox";
        this.pictureBox.Size = new System.Drawing.Size(852, 637);
        this.pictureBox.TabIndex = 10;
        this.pictureBox.TabStop = false;
        this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pb_previewArea_Paint);
        // 
        // RGBTreeSetupDialog
        // 
        this.AcceptButton = this.btn_setupOK;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.Black;
        this.ClientSize = new System.Drawing.Size(1167, 637);
        this.Controls.Add(this.pictureBox);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.controlPanel);
        this.Name = "RGBTreeSetupDialog";
        this.Text = "RGB Tree Setup";
        this.Resize += new System.EventHandler(this.form_Setup_Resize);
        this.controlPanel.ResumeLayout(false);
        this.controlPanel.PerformLayout();
        this.group_Type.ResumeLayout(false);
        this.group_Type.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.track_posY)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.track_posX)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.track_PixelSize)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.track_Height)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.track_Ellipse_X)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.track_Ellipse_Y)).EndInit();
        this.panel1.ResumeLayout(false);
        this.panel3.ResumeLayout(false);
        this.panel3.PerformLayout();
        this.panel4.ResumeLayout(false);
        this.panel4.PerformLayout();
        this.panel2.ResumeLayout(false);
        this.panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
        this.ResumeLayout(false);

    }

    #endregion

    private Button btn_setupOK;
    private Panel controlPanel;
    private Button btn_Cancel;
    private TextBox tb_noPixels;
    private TextBox tb_noStrings;
    private Label lbl_startPos;
    private Label lbl_noPixels;
    private Label lbl_noStrings;
    private Button btn_Create;
    private TextBox tb_pixelSize;
    private Label lbl_pixelSize;
    private TrackBar track_PixelSize;
    private TrackBar track_posX;
    private TextBox tb_posX;
    private Label lbl_posX;
    private Label lbl_posY;
    private TextBox tb_posY;
    private TrackBar track_posY;
    private GroupBox group_Type;
    private RadioButton rb_updown_180;
    private RadioButton rb_updown_next;
    private RadioButton rb_bottom_top;
    private TextBox tb_Requested;
    private TextBox tb_Available;
    private Label lbl_Requested;
    private Label lbl_available;
    private TrackBar track_Height;
    private TrackBar track_Ellipse_X;
    private TrackBar track_Ellipse_Y;
    private Panel panel1;
    private Panel panel3;
    private Panel panel2;
    private TextBox tb_Ellipse_Y;
    private TextBox tb_Ellipse_X;
    private TextBox tb_Height;
    private Panel panel4;
    private TextBox textBox4;
    private TextBox textBox3;
    private TextBox textBox2;
    private ComboBox comboClock;
    private Label lbl_direction;
    private ComboBox comboDirection;
    private PictureBox pictureBox;
}