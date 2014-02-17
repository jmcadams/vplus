using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using CommonControls.Properties;

namespace CommonControls {
    // Cyotek Color Picker controls library
    // Copyright © 2013 Cyotek. All Rights Reserved.
    // http://cyotek.com/blog/tag/colorpicker

    // If you use this code in your applications, donations or attribution are welcome

    /// <summary>
    /// Represents a grid control, which displays a collection of colors using different styles.
    /// </summary>
    [DefaultProperty("Color")]
    [DefaultEvent("ColorChanged")]
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class ColorGrid : Control, IColorEditor {
        #region Constants

        private const int InvalidIndex = -1;

        #endregion

        #region Instance Fields

        private readonly Image _cellBackground;

        private readonly TextureBrush _cellBackgroundBrush;

        private bool _autoAddColors;

        private Color _cellBorderColor;

        private ColorCellBorderStyle _cellBorderStyle;

        private Size _cellSize;

        private Color _color;

        private ColorCollection _colors;

        private int _columns;

        private ColorCollection _customColors;

        private ColorEditingMode _editMode;

        private int _hotIndex;

        private ColorPalette _palette;

        private ColorGridSelectedCellStyle _selectedCellStyle;

        private bool _showCustomColors;

        private bool _showToolTips;

        private Size _spacing;

        //private ToolTip _toolTip;

        private int _updateCount;

        #endregion

        #region Constructors

        //public ColorGrid(IEnumerable<Color> colors) : this(new ColorCollection(colors)) {}

        //public ColorGrid(ColorCollection colors) : this(colors, new ColorCollection(Enumerable.Repeat(Color.White, 32))) {}

        public ColorGrid(ColorPalette palette) : this(null, new ColorCollection(Enumerable.Repeat(Color.White, 32)), palette) {}

        //public ColorGrid(ColorCollection colors, ColorCollection customColors) : this(colors, customColors, ColorPalette.None) {}

        public ColorGrid() : this(ColorPalette.Named) {}


        protected ColorGrid(ColorCollection colors, ColorCollection customColors, ColorPalette palette) {
            _cellBackground = new Bitmap(Resources.cellbackground);
            _cellBackgroundBrush = new TextureBrush(_cellBackground, WrapMode.Tile);
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable |
                ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
            UpdateStyles();
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            HotIndex = InvalidIndex;
            ColorRegions = new Dictionary<int, Rectangle>();
            if (Palette != ColorPalette.None)
                Colors = colors;
            else
                Palette = palette;
            CustomColors = customColors;
            ShowCustomColors = true;
            CellSize = new Size(12, 12);
            Spacing = new Size(3, 3);
            Columns = 16;
            AutoSize = true;
            Padding = new Padding(5);
            AutoAddColors = true;
            CellBorderColor = SystemColors.ButtonShadow;
            ShowToolTips = true;
            SeparatorHeight = 8;
            EditMode = ColorEditingMode.CustomOnly;
            Color = Color.Black;
            CellBorderStyle = ColorCellBorderStyle.FixedSingle;
            SelectedCellStyle = ColorGridSelectedCellStyle.Zoomed;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        #endregion

        #region Events

        ///// <summary>
        ///// Occurs when the AutoAddColors property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler AutoAddColorsChanged;

        ///// <summary>
        ///// Occurs when the CellBorderColor property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler CellBorderColorChanged;

        ///// <summary>
        ///// Occurs when the CellBorderStyle property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler CellBorderStyleChanged;

        ///// <summary>
        ///// Occurs when the CellSize property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler CellSizeChanged;

        /// <summary>
        /// Occurs when the Color property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler ColorChanged;

        ///// <summary>
        ///// Occurs when the Colors property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler ColorsChanged;

        ///// <summary>
        ///// Occurs when the Columns property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler ColumnsChanged;

        ///// <summary>
        ///// Occurs when the CustomColors property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler CustomColorsChanged;

        ///// <summary>
        ///// Occurs when the EditMode property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler EditModeChanged;

        ///// <summary>
        ///// Occurs when the HotIndex property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler HotIndexChanged;

        /// <summary>
        /// Occurs when the Palette property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler PaletteChanged;

        ///// <summary>
        ///// Occurs when the SelectedCellStyle property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler SelectedCellStyleChanged;

        ///// <summary>
        ///// Occurs when the ShowCustomColors property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler ShowCustomColorsChanged;

        ///// <summary>
        ///// Occurs when the ShowToolTips property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler ShowToolTipsChanged;

        ///// <summary>
        ///// Occurs when the Spacing property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //public event EventHandler SpacingChanged;

        #endregion

        #region Overridden Properties

        [Browsable(true)]
        [DefaultValue(true)]
        public override bool AutoSize {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Font Font {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #endregion

        #region Overridden Members

        public override Size GetPreferredSize(Size proposedSize) {
            return AutoSize ? GetAutoSize() : base.GetPreferredSize(proposedSize);
        }


        protected override void Dispose(bool disposing) {
            if (disposing) {
                //if (_toolTip != null)
                //    _toolTip.Dispose();

                if (_cellBackground != null)
                    _cellBackground.Dispose();

                if (_cellBackgroundBrush != null)
                    _cellBackgroundBrush.Dispose();
            }

            base.Dispose(disposing);
        }


        protected override bool IsInputKey(Keys keyData) {
            bool result;

            if (keyData == Keys.Left || keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Right || keyData == Keys.Enter ||
                keyData == Keys.Home || keyData == Keys.End)
                result = true;
            else
                result = base.IsInputKey(keyData);

            return result;
        }


        protected override void OnAutoSizeChanged(EventArgs e) {
            base.OnAutoSizeChanged(e);

            if (AutoSize)
                SizeToFit();
        }


        protected override void OnGotFocus(EventArgs e) {
            base.OnGotFocus(e);

            Invalidate();
        }


        protected override void OnKeyDown(KeyEventArgs e) {
            bool processed;

            var lastStandardRowOffset = (PrimaryRows * Columns) - Colors.Count;
            var lastStandardRowLastColumn = Columns - lastStandardRowOffset;
            var cellLocation = GetCell(ColorIndex);
            var column = cellLocation.X;
            var row = cellLocation.Y;

            switch (e.KeyData) {
                case Keys.Down:
                    row++;
                    if (row == PrimaryRows - 1 && column >= lastStandardRowLastColumn)
                        column = lastStandardRowLastColumn - 1;
                    processed = true;
                    break;
                case Keys.Up:
                    row--;
                    if (row == PrimaryRows - 1 && column > lastStandardRowLastColumn)
                        column = lastStandardRowLastColumn - 1;
                    processed = true;
                    break;
                case Keys.Left:
                    column--;
                    if (column < 0) {
                        column = Columns - 1;
                        row--;
                        if (row == PrimaryRows - 1)
                            column = Columns - (lastStandardRowOffset + 1);
                    }
                    processed = true;
                    break;
                case Keys.Right:
                    column++;
                    if (row == PrimaryRows - 1 && column >= Columns - lastStandardRowOffset || column >= Columns) {
                        column = 0;
                        row++;
                    }
                    processed = true;
                    break;
                case Keys.Home:
                    column = 0;
                    row = 0;
                    processed = true;
                    break;
                case Keys.End:
                    column = Columns - 1;
                    row = PrimaryRows + CustomRows - 1;
                    processed = true;
                    break;
                default:
                    processed = false;
                    break;
            }

            if (processed) {
                var index = GetCellIndex(column, row);
                if (index != InvalidIndex) {
                    // setting the Color property will automatically set the ColorIndex to the index of the first match
                    // which is not exactly what you want when navigating the grid with the keyboard! Disable painting
                    // so that no flicker is displayed if the index flicks between two values with a forced repaint inbetween
                    BeginUpdate();
                    Color = GetColor(index);
                    ColorIndex = index;
                    EndUpdate();
                }

                e.Handled = true;
            }

            base.OnKeyDown(e);
        }


        //protected override void OnKeyUp(KeyEventArgs e) {
        //    if (e.KeyData == Keys.Enter && ColorIndex != InvalidIndex) {
        //        ColorSource source;

        //        source = GetColorSource(ColorIndex);
        //        if (source == ColorSource.Custom && EditMode != ColorEditingMode.None ||
        //            source == ColorSource.Standard && EditMode == ColorEditingMode.Both) {
        //            e.Handled = true;
        //            EditColor(ColorIndex);
        //        }
        //    }

        //    base.OnKeyUp(e);
        //}


        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);

            Invalidate();
        }


        //protected override void OnMouseDoubleClick(MouseEventArgs e) {
        //    ColorHitTestInfo hitTest;

        //    base.OnMouseDoubleClick(e);

        //    hitTest = HitTest(e.Location);

        //    if (hitTest.Source == ColorSource.Custom && EditMode != ColorEditingMode.None ||
        //        hitTest.Source == ColorSource.Standard && EditMode == ColorEditingMode.Both)
        //        EditColor(hitTest.Index);
        //}


        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);

            if (!Focused && TabStop)
                Focus();

            ProcessMouseClick(e);
        }


        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);

            HotIndex = InvalidIndex;
        }


        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);

            HotIndex = HitTest(e.Location).Index;

            ProcessMouseClick(e);
        }


        protected override void OnPaddingChanged(EventArgs e) {
            base.OnPaddingChanged(e);

            RefreshColors();

            Invalidate();
        }


        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            if (!AllowPainting) {
                return;
            }

            base.OnPaintBackground(e);
            // HACK: Easiest way of supporting things like BackgroundImage, BackgroundImageLayout etc as the PaintBackground event is no longer being called

            // draw cells for all current colors
            for (var i = 0; i < Colors.Count; i++)
                PaintCell(e, i, Colors[i], ColorRegions[i]);

            if (CustomColors.Count != 0 && ShowCustomColors) {
                // draw a separator
                PaintSeparator(e);

                // and the custom colors
                for (var i = 0; i < CustomColors.Count; i++)
                    PaintCell(e, Colors.Count + i, CustomColors[i], ColorRegions[Colors.Count + i]);
            }

            // draw the selected color
            if (SelectedCellStyle != ColorGridSelectedCellStyle.None && ColorIndex >= 0)
                PaintSelectedCell(e, Color, ColorRegions[ColorIndex]);
        }


        protected override void OnResize(EventArgs e) {
            RefreshColors();

            base.OnResize(e);
        }

        #endregion

        #region Properties

        [Category("Behavior")]
        [DefaultValue(true)]
        public virtual bool AutoAddColors {
            get { return _autoAddColors; }
            set {
                if (AutoAddColors == value) {
                    return;
                }
                _autoAddColors = value;

                //OnAutoAddColorsChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "ButtonShadow")]
        public virtual Color CellBorderColor {
            get { return _cellBorderColor; }
            set {
                if (CellBorderColor == value) {
                    return;
                }
                _cellBorderColor = value;

                //OnCellBorderColorChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(ColorCellBorderStyle), "FixedSingle")]
        public virtual ColorCellBorderStyle CellBorderStyle {
            get { return _cellBorderStyle; }
            set {
                if (CellBorderStyle == value) {
                    return;
                }
                _cellBorderStyle = value;

                //OnCellBorderStyleChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Size), "12, 12")]
        public virtual Size CellSize {
            get { return _cellSize; }
            set {
                if (CellSize == value) {
                    return;
                }
                _cellSize = value;

                //OnCellSizeChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof (Color), "Black")]
        public virtual Color Color {
            get { return _color; }
            set {
                _color = value;

                var newIndex = GetColorIndex(value);

                if (newIndex == InvalidIndex)
                    newIndex = AddCustomColor(value);

                ColorIndex = newIndex;

                OnColorChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ColorCollection Colors {
            get { return _colors; }
            set {
                if (Colors == value) {
                    return;
                }
                _colors = value;

                //OnColorsChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DefaultValue(16)]
        public virtual int Columns {
            get { return _columns; }
            set {
                if (Columns == value) {
                    return;
                }
                _columns = value;
                CalculateRows();

                //OnColumnsChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ColorCollection CustomColors {
            get { return _customColors; }
            set {
                if (CustomColors == value) {
                    return;
                }
                _customColors = value;

                //OnCustomColorsChanged(EventArgs.Empty);
            }
        }

        [Category("Behavior")]
        [DefaultValue(typeof (ColorEditingMode), "Custom")]
        public virtual ColorEditingMode EditMode {
            get { return _editMode; }
            set {
                if (EditMode == value) {
                    return;
                }
                _editMode = value;

                //OnEditModeChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int HotIndex {
            get { return _hotIndex; }
            set {
                if (HotIndex == value) {
                    return;
                }
                _hotIndex = value;

                //OnHotIndexChanged(EventArgs.Empty);
            }
        }

        [DefaultValue(typeof (Padding), "5, 5, 5, 5")]
        public new Padding Padding {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        [Category("Appearance")]
        [DefaultValue(typeof (ColorPalette), "Named")]
        public virtual ColorPalette Palette {
            get { return _palette; }
            set {
                if (Palette == value) {
                    return;
                }
                _palette = value;

                OnPaletteChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof (ColorGridSelectedCellStyle), "Zoomed")]
        public virtual ColorGridSelectedCellStyle SelectedCellStyle {
            get { return _selectedCellStyle; }
            set {
                if (SelectedCellStyle == value) {
                    return;
                }
                _selectedCellStyle = value;

                //OnSelectedCellStyleChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DefaultValue(true)]
        public virtual bool ShowCustomColors {
            get { return _showCustomColors; }
            set {
                if (ShowCustomColors == value) {
                    return;
                }
                _showCustomColors = value;

                //OnShowCustomColorsChanged(EventArgs.Empty);
            }
        }

        [Category("Behavior")]
        [DefaultValue(true)]
        public virtual bool ShowToolTips {
            get { return _showToolTips; }
            set {
                if (ShowToolTips == value) {
                    return;
                }
                _showToolTips = value;

                //OnShowToolTipsChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof (Size), "3, 3")]
        public virtual Size Spacing {
            get { return _spacing; }
            set {
                if (Spacing == value) {
                    return;
                }
                _spacing = value;

                //OnSpacingChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether painting of the control is allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if painting of the control is allowed; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool AllowPainting {
            get { return _updateCount == 0; }
        }

        protected int ColorIndex { get; set; }

        protected IDictionary<int, Rectangle> ColorRegions { get; set; }

        protected int CustomRows { get; set; }

        //protected bool LockUpdates { get; set; }

        protected int PrimaryRows { get; set; }

        protected int SeparatorHeight { get; set; }

        #endregion

        #region Members

        public virtual int AddCustomColor(Color value) {
            var newIndex = GetColorIndex(value);

            if (newIndex != InvalidIndex) {
                return newIndex;
            }

            if (AutoAddColors)
                CustomColors.Add(value);
            else {
                if (CustomColors == null) {
                    CustomColors = new ColorCollection {value};
                }
                else
                    CustomColors[0] = value;

                newIndex = GetColorIndex(value);
            }

            RefreshColors();

            return newIndex;
        }


        /// <summary>
        ///   Disables any redrawing of the image box
        /// </summary>
        public virtual void BeginUpdate() {
            _updateCount++;
        }


        /// <summary>
        ///   Enables the redrawing of the image box
        /// </summary>
        public virtual void EndUpdate() {
            if (_updateCount > 0)
                _updateCount--;

            if (AllowPainting)
                Invalidate();
        }


        public Color GetColor(int index) {
            Color result;

            var colorCount = Colors != null ? Colors.Count : 0;
            var customColorCount = CustomColors != null ? CustomColors.Count : 0;

            if (index < 0 || index > (colorCount + customColorCount))
                result = Color.Empty;
            else
                // ReSharper disablePossibleNullReferenceException
                result = index > colorCount - 1 ? CustomColors[index - colorCount] : Colors[index];
                // ReSharper enable PossibleNullReferenceException

            return result;
        }


        public ColorSource GetColorSource(int colorIndex) {
            ColorSource result;
            var colorCount = Colors != null ? Colors.Count : 0;
            var customColorCount = CustomColors != null ? CustomColors.Count : 0;

            if (colorCount < 0 || colorIndex > (colorCount + customColorCount))
                result = ColorSource.None;
            else
                result = colorIndex > colorCount - 1 ? ColorSource.Custom : ColorSource.Standard;

            return result;
        }


        //public ColorSource GetColorSource(Color color) {
        //    ColorSource result;

        //    var index = Colors.IndexOf(color);
        //    if (index != InvalidIndex)
        //        result = ColorSource.Standard;
        //    else {
        //        index = CustomColors.IndexOf(color);
        //        result = index != InvalidIndex ? ColorSource.Custom : ColorSource.None;
        //    }

        //    return result;
        //}


        public ColorHitTestInfo HitTest(Point point) {
            var result = new ColorHitTestInfo();
            var colorIndex = InvalidIndex;

            foreach (var pair in ColorRegions.Where(pair => pair.Value.Contains(point))) {
                colorIndex = pair.Key;
                break;
            }

            result.Index = colorIndex;
            if (colorIndex != InvalidIndex) {
                result.Color = colorIndex < (Colors.Count + CustomColors.Count) ? GetColor(colorIndex) : Color.White;
                result.Source = GetColorSource(colorIndex);
            }
            else
                result.Source = ColorSource.None;

            return result;
        }


        protected virtual void CalculateRows() {
            var primaryRows = GetRows(Colors != null ? Colors.Count : 0);
            if (primaryRows == 0)
                primaryRows = 1;

            var customRows = ShowCustomColors ? GetRows(CustomColors != null ? CustomColors.Count : 0) : 0;

            PrimaryRows = primaryRows;
            CustomRows = customRows;
        }


        protected void DefineColorRegions(ColorCollection colors, int rangeStart, int offset) {
            if (colors == null) {
                return;
            }
            var rows = GetRows(colors.Count);
            var index = 0;

            for (var row = 0; row < rows; row++) {
                for (var column = 0; column < Columns; column++) {
                    if (index < colors.Count)
                        ColorRegions.Add(rangeStart + index,
                            new Rectangle(Padding.Left + (column * (CellSize.Width + Spacing.Width)),
                                offset + (row * (CellSize.Height + Spacing.Height)), CellSize.Width, CellSize.Height));

                    index++;
                }
            }
        }


        //protected virtual void EditColor(int colorIndex) {
        //    using (ColorPickerDialog dialog = new ColorPickerDialog()) {
        //        dialog.Color = GetColor(colorIndex);
        //        if (dialog.ShowDialog(this) == DialogResult.OK) {
        //            BeginUpdate();
        //            SetColor(colorIndex, dialog.Color);
        //            Color = dialog.Color;
        //            EndUpdate();
        //        }
        //    }
        //}


        protected Size GetAutoSize() {
            var offset = CustomRows != 0 ? SeparatorHeight : 0;

            return new Size(((CellSize.Width + Spacing.Width) * Columns) + Padding.Horizontal - Spacing.Width,
                ((CellSize.Height + Spacing.Height) * (PrimaryRows + CustomRows)) + offset + Padding.Vertical - Spacing.Height);
        }


        protected virtual int GetCellIndex(int column, int row) {
            int result;

            if (column >= 0 && column < Columns && row >= 0 && row < (PrimaryRows + CustomRows)) {
                var lastStandardRowOffset = (PrimaryRows * Columns) - Colors.Count;
                result = row * Columns + column;
                if (row == PrimaryRows - 1 && column >= (Columns - lastStandardRowOffset))
                    result -= lastStandardRowOffset;
                if (row >= PrimaryRows)
                    result -= lastStandardRowOffset;

                if (result > (Colors.Count + CustomColors.Count) - 1)
                    result = InvalidIndex;
            }
            else
                result = InvalidIndex;

            return result;
        }


        protected virtual int GetColorIndex(Color value) {
            var index = Colors != null ? Colors.IndexOf(value) : InvalidIndex;
            if (index != InvalidIndex || CustomColors == null) {
                return index;
            }

            index = CustomColors.IndexOf(value);
            if (index != InvalidIndex && null != Colors)
                index += Colors.Count;

            return index;
        }


        protected virtual ColorCollection GetPredefinedPalette() {
            return ColorPalettes.GetPalette(Palette);
        }


        protected int GetRows(int count) {
            int rows;

            if (count != 0 && Columns > 0) {
                rows = count / Columns;
                if ((count % Columns) != 0)
                    rows++;
            }
            else
                rows = 0;

            return rows;
        }


        ///// <summary>
        ///// Raises the <see cref="AutoAddColorsChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnAutoAddColorsChanged(EventArgs e) {
        //    var handler = AutoAddColorsChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        ///// <summary>
        ///// Raises the <see cref="CellBorderColorChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnCellBorderColorChanged(EventArgs e) {
        //    Invalidate();

        //    var handler = CellBorderColorChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        ///// <summary>
        ///// Raises the <see cref="CellBorderStyleChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnCellBorderStyleChanged(EventArgs e) {
        //    Invalidate();

        //    var handler = CellBorderStyleChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        ///// <summary>
        ///// Raises the <see cref="CellSizeChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnCellSizeChanged(EventArgs e) {
        //    if (AutoSize)
        //        SizeToFit();
        //    Invalidate();

        //    var handler = CellSizeChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        /// <summary>
        /// Raises the <see cref="ColorChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnColorChanged(EventArgs e) {
            if (AllowPainting)
                Refresh();

            var handler = ColorChanged;

            if (handler != null)
                handler(this, e);
        }


        ///// <summary>
        ///// Raises the <see cref="ColorsChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnColorsChanged(EventArgs e) {
        //    if (Colors != null)
        //        Colors.CollectionChanged += ColorsCollectionChangedHandler;
        //    RefreshColors();

        //    var handler = ColorsChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        ///// <summary>
        ///// Raises the <see cref="ColumnsChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnColumnsChanged(EventArgs e) {
        //    RefreshColors();

        //    var handler = ColumnsChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        ///// <summary>
        ///// Raises the <see cref="CustomColorsChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnCustomColorsChanged(EventArgs e) {
        //    if (CustomColors != null)
        //        CustomColors.CollectionChanged += ColorsCollectionChangedHandler;
        //    RefreshColors();

        //    var handler = CustomColorsChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        ///// <summary>
        ///// Raises the <see cref="EditModeChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnEditModeChanged(EventArgs e) {
        //    var handler = EditModeChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        ///// <summary>
        ///// Raises the <see cref="HotIndexChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnHotIndexChanged(EventArgs e) {
        //    SetToolTip();
        //    Refresh();

        //    var handler = HotIndexChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        /// <summary>
        /// Raises the <see cref="PaletteChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnPaletteChanged(EventArgs e) {
            Colors = GetPredefinedPalette();

            var handler = PaletteChanged;

            if (handler != null)
                handler(this, e);
        }


        ///// <summary>
        ///// Raises the <see cref="SelectedCellStyleChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnSelectedCellStyleChanged(EventArgs e) {
        //    Invalidate();

        //    var handler = SelectedCellStyleChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        ///// <summary>
        ///// Raises the <see cref="ShowCustomColorsChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnShowCustomColorsChanged(EventArgs e) {
        //    RefreshColors();

        //    var handler = ShowCustomColorsChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        ///// <summary>
        ///// Raises the <see cref="ShowToolTipsChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnShowToolTipsChanged(EventArgs e) {
        //    if (ShowToolTips)
        //        _toolTip = new ToolTip();
        //    else if (_toolTip != null) {
        //        _toolTip.Dispose();
        //        _toolTip = null;
        //    }

        //    var handler = ShowToolTipsChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        ///// <summary>
        ///// Raises the <see cref="SpacingChanged" /> event.
        ///// </summary>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //protected virtual void OnSpacingChanged(EventArgs e) {
        //    if (AutoSize) {
        //        SizeToFit();
        //    }
        //    Invalidate();

        //    var handler = SpacingChanged;

        //    if (handler != null)
        //        handler(this, e);
        //}


        protected virtual void PaintCell(PaintEventArgs e, int cellIndex, Color color, Rectangle bounds) {
            if (color.A != 255)
                e.Graphics.FillRectangle(_cellBackgroundBrush, bounds);

            using (Brush brush = new SolidBrush(color))
                e.Graphics.FillRectangle(brush, bounds);

            if (CellBorderStyle == ColorCellBorderStyle.FixedSingle) {
                using (var pen = new Pen(CellBorderColor))
                    e.Graphics.DrawRectangle(pen, bounds.Left, bounds.Top, bounds.Width - 1, bounds.Height - 1);
            }

            if (HotIndex == InvalidIndex || HotIndex != cellIndex) {
                return;
            }
            e.Graphics.DrawRectangle(Pens.Black, bounds.Left, bounds.Top, bounds.Width - 1, bounds.Height - 1);
            e.Graphics.DrawRectangle(Pens.White, bounds.Left + 1, bounds.Top + 1, bounds.Width - 3, bounds.Height - 3);
        }


        protected virtual void PaintSelectedCell(PaintEventArgs e, Color color, Rectangle bounds) {
            switch (SelectedCellStyle) {
                case ColorGridSelectedCellStyle.Standard:
                    if (Focused)
                        ControlPaint.DrawFocusRectangle(e.Graphics, bounds);
                    else
                        e.Graphics.DrawRectangle(Pens.Black, bounds.Left, bounds.Top, bounds.Width - 1, bounds.Height - 1);
                    break;
                case ColorGridSelectedCellStyle.Zoomed:
                    // make the cell larger by half
                    if (SelectedCellStyle == ColorGridSelectedCellStyle.Zoomed)
                        bounds.Inflate((CellSize.Width / 2) - 1, (CellSize.Height / 2) - 1);

                    // fill the inner
                    e.Graphics.FillRectangle(Brushes.White, bounds);
                    if (SelectedCellStyle == ColorGridSelectedCellStyle.Zoomed)
                        bounds.Inflate(-3, -3);
                    if (color.A != 255)
                        e.Graphics.FillRectangle(_cellBackgroundBrush, bounds);

                    using (Brush brush = new SolidBrush(color))
                        e.Graphics.FillRectangle(brush, bounds);
                    // draw a border
                    if (Focused) {
                        bounds = new Rectangle(bounds.Left - 2, bounds.Top - 2, bounds.Width + 4, bounds.Height + 4);
                        ControlPaint.DrawFocusRectangle(e.Graphics, bounds);
                    }
                    else {
                        bounds = new Rectangle(bounds.Left - 2, bounds.Top - 2, bounds.Width + 3, bounds.Height + 3);

                        using (var pen = new Pen(CellBorderColor)) {
                            e.Graphics.DrawRectangle(pen, bounds);
                        }
                    }
                    break;
            }
        }


        protected virtual void PaintSeparator(PaintEventArgs e) {
            var x1 = Padding.Left;
            var x2 = ClientSize.Width - Padding.Right;
            var y1 = (SeparatorHeight / 2) + Padding.Top + (PrimaryRows * (CellSize.Height + Spacing.Height)) + 1 - Spacing.Height;
            var y2 = y1;

            using (var pen = new Pen(CellBorderColor)) {
                e.Graphics.DrawLine(pen, x1, y1, x2, y2);
            }
        }


        protected virtual void ProcessMouseClick(MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) {
                return;
            }

            var hitTest = HitTest(e.Location);

            if (hitTest.Source == ColorSource.None) {
                return;
            }

            Color = hitTest.Color;
            ColorIndex = hitTest.Index;
            Invalidate();
        }


        protected virtual void RefreshColors() {
            if (!AllowPainting) {
                return;
            }

            CalculateRows();
            if (AutoSize)
                SizeToFit();

            ColorRegions.Clear();

            if (Colors == null) {
                return;
            }

            DefineColorRegions(Colors, 0, Padding.Top);
            DefineColorRegions(CustomColors, Colors.Count, Padding.Top + SeparatorHeight + ((CellSize.Height + Spacing.Height) * PrimaryRows));

            ColorIndex = GetColorIndex(Color);

            if (!Color.IsEmpty && ColorIndex == InvalidIndex)
                AddCustomColor(Color);

            Invalidate();
        }


        //protected virtual void SetColor(int colorIndex, Color color) {
        //    int colorCount;

        //    colorCount = Colors.Count;
        //    if (colorIndex < 0 || colorIndex > (colorCount + CustomColors.Count))
        //        throw new ArgumentOutOfRangeException("colorIndex");

        //    if (colorIndex > colorCount - 1)
        //        CustomColors[colorIndex - colorCount] = color;
        //    else
        //        Colors[colorIndex] = color;
        //}


        //protected virtual void SetToolTip() {
        //    if (ShowToolTips)
        //        _toolTip.SetToolTip(this, HotIndex != InvalidIndex ? GetColor(HotIndex).Name : null);
        //}


        private Point GetCell(int index) {
            int row;
            int column;

            if (index >= Colors.Count) {
                // custom color
                index -= Colors.Count;
                row = index / Columns;
                column = index - (row * Columns);
                row += PrimaryRows;
            }
            else {
                // normal row
                row = index / Columns;
                column = index - (row * Columns);
            }

            return new Point(column, row);
        }


        private void SizeToFit() {
            Size = GetAutoSize();
        }

        #endregion

        #region Event Handlers

        //private void ColorsCollectionChangedHandler(object sender, ColorCollectionEventArgs e) {
        //    RefreshColors();
        //}

        #endregion
    }

    // ReSharper restore MemberCanBePrivate.Global
    // ReSharper restore MemberCanBeProtected.Global

}