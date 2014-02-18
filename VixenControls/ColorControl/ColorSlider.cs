using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

using CommonControls.Annotations;

namespace CommonControls {
    // Cyotek Color Picker controls library
    // Copyright © 2013 Cyotek. All Rights Reserved.
    // http://cyotek.com/blog/tag/colorpicker

    // If you use this code in your applications, donations or attribution are welcome

    /// <summary>
    ///     Represents a control for selecting a value from a scale
    /// </summary>
    [DefaultValue("Value")]
    [DefaultEvent("ValueChanged")]
    [ToolboxItem(false)]
    public class ColorSlider : Control {
        #region Instance Fields

        private Rectangle _barBounds;

        private Padding _barPadding;

        private ColorCollection _customColors;

        private int _largeChange;

        private float _maximum;

        private float _minimum;

        private Color _nubColor;

        private Size _nubSize;

        private int _smallChange;

        private float _value;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorSlider" /> class.
        /// </summary>
        public ColorSlider() {
            SetStyle(
                ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            UpdateStyles();
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Minimum = 0;
            Maximum = 100;
            NubSize = new Size(8, 8);
            NubColor = Color.Black;
            SmallChange = 1;
            LargeChange = 8;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when the BarBounds property value changes
        /// </summary>
        [Category("Property Changed")]
        [UsedImplicitly]
        public event EventHandler BarBoundsChanged;

        /// <summary>
        ///     Occurs when the BarPadding property value changes
        /// </summary>
        [Category("Property Changed")]
        [UsedImplicitly]
        public event EventHandler BarPaddingChanged;

        ///// <summary>
        /////     Occurs when the Style property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //[UsedImplicitly]
        //public event EventHandler BarStyleChanged;

        /// <summary>
        ///     Occurs when the CustomColors property value changes
        /// </summary>
        [Category("Property Changed")]
        [UsedImplicitly]
        public event EventHandler CustomColorsChanged;

        /// <summary>
        ///     Occurs when the LargeChange property value changes
        /// </summary>
        [Category("Property Changed")]
        [UsedImplicitly]
        public event EventHandler LargeChangeChanged;

        /// <summary>
        ///     Occurs when the Maximum property value changes
        /// </summary>
        [Category("Property Changed")]
        [UsedImplicitly]
        public event EventHandler MaximumChanged;

        /// <summary>
        ///     Occurs when the Minimum property value changes
        /// </summary>
        [Category("Property Changed")]
        [UsedImplicitly]
        public event EventHandler MinimumChanged;

        /// <summary>
        ///     Occurs when the NubColor property value changes
        /// </summary>
        [Category("Property Changed")]
        [UsedImplicitly]
        public event EventHandler NubColorChanged;

        /// <summary>
        ///     Occurs when the NubSize property value changes
        /// </summary>
        [Category("Property Changed")]
        [UsedImplicitly]
        public event EventHandler NubSizeChanged;

        ///// <summary>
        /////     Occurs when the NubStyle property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //[UsedImplicitly]
        //public event EventHandler NubStyleChanged;

        ///// <summary>
        /////     Occurs when the SliderStyle property value changes
        ///// </summary>
        //[Category("Property Changed")]
        //[UsedImplicitly]
        //public event EventHandler SliderStyleChanged;

        /// <summary>
        ///     Occurs when the SmallChange property value changes
        /// </summary>
        [Category("Property Changed")]
        [UsedImplicitly]
        public event EventHandler SmallChangeChanged;

        /// <summary>
        ///     Occurs when the Percent property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler ValueChanged;

        #endregion

        #region Overridden Properties

        /// <summary>
        ///     Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value>The font.</value>
        /// <returns>
        ///     The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the
        ///     value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.
        /// </returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Font Font {
            get { return base.Font; }
            set { base.Font = value; }
        }

        /// <summary>
        ///     Gets or sets the foreground color of the control.
        /// </summary>
        /// <value>The color of the fore.</value>
        /// <returns>
        ///     The foreground <see cref="T:System.Drawing.Color" /> of the control. The default is the value of the
        ///     <see cref="P:System.Windows.Forms.Control.DefaultForeColor" /> property.
        /// </returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        /// <summary>
        ///     Gets or sets the text associated with this control.
        /// </summary>
        /// <value>The text.</value>
        /// <returns>The text associated with this control.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #endregion

        /// <summary>
        ///     Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" /> and its child controls
        ///     and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing) {
            if (disposing && SelectionGlyph != null) {
                SelectionGlyph.Dispose();
            }

            base.Dispose(disposing);
        }


        /// <summary>
        ///     Determines whether the specified key is a regular input key or a special key that requires preprocessing.
        /// </summary>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values.</param>
        /// <returns>true if the specified key is a regular input key; otherwise, false.</returns>
        protected override bool IsInputKey(Keys keyData) {
            bool result;

            if ((keyData & Keys.Left) == Keys.Left || (keyData & Keys.Up) == Keys.Up || (keyData & Keys.Down) == Keys.Down ||
                (keyData & Keys.Right) == Keys.Right || (keyData & Keys.PageUp) == Keys.PageUp || (keyData & Keys.PageDown) == Keys.PageDown ||
                (keyData & Keys.Home) == Keys.Home || (keyData & Keys.End) == Keys.End) {
                result = true;
            }
            else {
                result = base.IsInputKey(keyData);
            }

            return result;
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e) {
            base.OnGotFocus(e);

            Invalidate();
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e) {
            var step = e.Shift ? LargeChange : SmallChange;
            var value = Value;

            switch (e.KeyCode) {
                case Keys.Right:
                case Keys.Up:
                    value += step;
                    break;
                case Keys.Left:
                case Keys.Down:
                    value -= step;
                    break;
                case Keys.PageDown:
                    value += LargeChange;
                    break;
                case Keys.PageUp:
                    value -= LargeChange;
                    break;
                case Keys.Home:
                    value = Minimum;
                    break;
                case Keys.End:
                    value = Maximum;
                    break;
            }

            if (value < Minimum) {
                value = Minimum;
            }

            if (value > Maximum) {
                value = Maximum;
            }

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (value != Value)
                // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                Value = value;

                e.Handled = true;
            }

            base.OnKeyDown(e);
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);

            Invalidate();
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);

            if (!Focused && TabStop) {
                Focus();
            }

            if (e.Button == MouseButtons.Left) {
                PointToValue(e.Location);
            }
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left) {
                PointToValue(e.Location);
            }
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.PaddingChanged" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnPaddingChanged(EventArgs e) {
            base.OnPaddingChanged(e);

            DefineBar();
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            PaintBar(e);
            PaintAdornments(e);
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            DefineBar();
        }

        #region Properties

        /// <summary>
        ///     Gets or sets the location and size of the color bar.
        /// </summary>
        /// <value>The location and size of the color bar.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Rectangle BarBounds {
            get { return _barBounds; }
            private set {
                if (BarBounds == value) {
                    return;
                }
                _barBounds = value;

                OnBarBoundsChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the bar padding.
        /// </summary>
        /// <value>The bar padding.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private Padding BarPadding {
            get { return _barPadding; }
            set {
                if (BarPadding == value) {
                    return;
                }
                _barPadding = value;

                OnBarPaddingChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the color range used by the custom bar style.
        /// </summary>
        /// <value>The custom colors.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected ColorCollection CustomColors {
            private get { return _customColors; }
            set {
                if (CustomColors == value) {
                    return;
                }
                _customColors = value;

                OnCustomColorsChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets a value to be added to or subtracted from the <see cref="Value" /> property when the selection is
        ///     moved a large distance.
        /// </summary>
        /// <value>A numeric value. The default value is 10.</value>
        [Category("Behavior")]
        [DefaultValue(10)]
        private int LargeChange {
            get { return _largeChange; }
            set {
                if (LargeChange == value) {
                    return;
                }
                _largeChange = value;

                OnLargeChangeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the upper limit of values of the selection range.
        /// </summary>
        /// <value>A numeric value. The default value is 100.</value>
        [Category("Behavior")]
        [DefaultValue(100F)]
        protected virtual float Maximum {
            get { return _maximum; }
            set {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Maximum == value) {
                    return;
                }
                _maximum = value;

                OnMaximumChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the lower limit of values of the selection range.
        /// </summary>
        /// <value>A numeric value. The default value is 0.</value>
        [Category("Behavior")]
        [DefaultValue(0F)]
        protected virtual float Minimum {
            get { return _minimum; }
            set {
                // ReSharper disable CompareOfFloatsByEqualityOperator
                if (Minimum == value) {
                    return;
                }
                _minimum = value;

                OnMinimumChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the color of the selection nub.
        /// </summary>
        /// <value>The color of the nub.</value>
        [Category("Appearance")]
        [DefaultValue(typeof (Color), "Black")]
        // ReSharper disable once VirtualMemberNeverOverriden.Global
        public virtual Color NubColor {
            // ReSharper disable once MemberCanBeProtected.Global
            get { return _nubColor; }
            set {
                if (NubColor == value) {
                    return;
                }
                _nubColor = value;

                OnNubColorChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the size of the selection nub.
        /// </summary>
        /// <value>The size of the nub.</value>
        [Category("Appearance")]
        [DefaultValue(typeof (Size), "8, 8")]
        private Size NubSize {
            get { return _nubSize; }
            set {
                if (NubSize == value) {
                    return;
                }
                _nubSize = value;

                OnNubSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the value to be added to or subtracted from the <see cref="Value" /> property when the selection is
        ///     moved a small distance.
        /// </summary>
        /// <value>A numeric value. The default value is 1.</value>
        [Category("Behavior")]
        [DefaultValue(1)]
        private int SmallChange {
            get { return _smallChange; }
            set {
                if (SmallChange == value) {
                    return;
                }
                _smallChange = value;

                OnSmallChangeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets a numeric value that represents the current position of the selection numb on the color slider
        ///     control.
        /// </summary>
        /// <value>
        ///     A numeric value that is within the <see cref="Minimum" /> and <see cref="Maximum" /> range. The default value is
        ///     0.
        /// </value>
        [Category("Appearance")]
        [DefaultValue(0F)]
        public virtual float Value {
            get { return _value; }
            set {
                value = value < Minimum ? Minimum : value > Maximum ? Maximum : value;

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Value == value) {
                    return;
                }

                _value = value;

                OnValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the selection glyph.
        /// </summary>
        /// <value>The selection glyph.</value>
        private Image SelectionGlyph { get; set; }

        #endregion

        #region Members

        /// <summary>
        ///     Creates the selection nub glyph.
        /// </summary>
        /// <returns>Image.</returns>
        private Image CreateNubGlyph() {
            var firstCorner = new Point(0, NubSize.Height);
            var lastCorner = new Point(NubSize.Width, NubSize.Height);
            var tipCorner = new Point(NubSize.Width / 2, 0);
            var outer = new[] {firstCorner, lastCorner, tipCorner};

            var image = new Bitmap(NubSize.Width + 1, NubSize.Height + 1, PixelFormat.Format32bppArgb);

            using (var g = Graphics.FromImage(image)) {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Brush brush = new SolidBrush(NubColor)) {
                    g.FillPolygon(brush, outer);
                }
            }

            return image;
        }


        /// <summary>
        ///     Defines the bar bounds and padding.
        /// </summary>
        private void DefineBar() {
            if (SelectionGlyph != null) {
                SelectionGlyph.Dispose();
            }

            BarPadding = GetBarPadding();
            BarBounds = GetBarBounds();
            SelectionGlyph = CreateNubGlyph();
        }


        /// <summary>
        ///     Gets the bar bounds.
        /// </summary>
        /// <returns>Rectangle.</returns>
        private Rectangle GetBarBounds() {
            var clientRectangle = ClientRectangle;
            var padding = BarPadding + Padding;

            return new Rectangle(clientRectangle.Left + padding.Left, clientRectangle.Top + padding.Top, clientRectangle.Width - padding.Horizontal,
                clientRectangle.Height - padding.Vertical);
        }


        /// <summary>
        ///     Gets the bar padding.
        /// </summary>
        /// <returns>Padding.</returns>
        private Padding GetBarPadding() {
            const int top = 0;

            var bottom = NubSize.Height + 1;
            var left = (NubSize.Width / 2) + 1;
            var right = left;

            return new Padding(left, top, right, bottom);
        }


        /// <summary>
        ///     Raises the <see cref="BarBoundsChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnBarBoundsChanged(EventArgs e) {
            var handler = BarBoundsChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Raises the <see cref="BarPaddingChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnBarPaddingChanged(EventArgs e) {
            Invalidate();

            var handler = BarPaddingChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Raises the <see cref="CustomColorsChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnCustomColorsChanged(EventArgs e) {
            Invalidate();

            var handler = CustomColorsChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Raises the <see cref="LargeChangeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnLargeChangeChanged(EventArgs e) {
            var handler = LargeChangeChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Raises the <see cref="MaximumChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnMaximumChanged(EventArgs e) {
            var handler = MaximumChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Raises the <see cref="MinimumChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnMinimumChanged(EventArgs e) {
            Invalidate();

            var handler = MinimumChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Raises the <see cref="NubColorChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnNubColorChanged(EventArgs e) {
            Invalidate();

            var handler = NubColorChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Raises the <see cref="NubSizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnNubSizeChanged(EventArgs e) {
            DefineBar();
            Invalidate();

            var handler = NubSizeChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Raises the <see cref="SmallChangeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnSmallChangeChanged(EventArgs e) {
            var handler = SmallChangeChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Raises the <see cref="ValueChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnValueChanged(EventArgs e) {
            Refresh();

            var handler = ValueChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Paints control adornments.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs" /> instance containing the event data.</param>
        private void PaintAdornments(PaintEventArgs e) {
            // drag nub
            if (SelectionGlyph != null) {
                e.Graphics.DrawImage(SelectionGlyph, ValueToPoint(Value).X - NubSize.Width / 2, BarBounds.Bottom);
            }

            // focus
            if (Focused) {
                ControlPaint.DrawFocusRectangle(e.Graphics, Rectangle.Inflate(BarBounds, -2, -2));
            }
        }


        /// <summary>
        ///     Paints the bar.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs" /> instance containing the event data.</param>
        protected virtual void PaintBar(PaintEventArgs e) {
            if (BarBounds.Height <= 0 || BarBounds.Width <= 0) {
                return;
            }

            var blend = new ColorBlend();
            if (CustomColors != null && CustomColors.Count > 0) {
                blend.Colors = CustomColors.ToArray();
                blend.Positions =
                    Enumerable.Range(0, CustomColors.Count).Select(
                        i => i == 0 ? 0 : i == CustomColors.Count - 1 ? 1 : (float) (1.0D / CustomColors.Count) * i).ToArray();
            }

            const int angle = 0;
            // HACK: Inflating the brush rectangle by 1 seems to get rid of a odd issue where the last color is drawn on the first pixel
            using (var brush = new LinearGradientBrush(Rectangle.Inflate(BarBounds, 1, 1), Color.Empty, Color.Empty, angle, false)) {
                brush.InterpolationColors = blend;
                e.Graphics.FillRectangle(brush, BarBounds);
            }
        }


        /// <summary>
        ///     Computes the location of the specified client point into value coordinates.
        /// </summary>
        /// <param name="location">The client coordinate <see cref="Point" /> to convert.</param>
        private void PointToValue(Point location) {
            location.X += ClientRectangle.X - BarBounds.X;
            location.Y += ClientRectangle.Y - BarBounds.Y;

            var value = Minimum + (location.X / (float) BarBounds.Width * (Minimum + Maximum));

            Value = value < Minimum ? Minimum : value > Maximum ? Maximum : value;
        }


        /// <summary>
        ///     Computes the location of the value point into client coordinates.
        /// </summary>
        /// <param name="value">The value coordinate <see cref="Point" /> to convert.</param>
        /// <returns>A <see cref="Point" /> that represents the converted <see cref="Point" />, value, in client coordinates.</returns>
        private Point ValueToPoint(float value) {
            var padding = BarPadding + Padding;
            return new Point((int) ((BarBounds.Width / Maximum) * value) + padding.Left, padding.Top);
        }

        #endregion
    }
}
