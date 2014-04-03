using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using VixenPlusCommon.Annotations;

namespace VixenPlusCommon {
    // Cyotek Color Picker controls library
    // Copyright © 2013 Cyotek. All Rights Reserved.
    // http://cyotek.com/blog/tag/colorpicker

    // If you use this code in your applications, donations or attribution are welcome

    public sealed class RgbaColorSlider : ColorSlider {
        #region Instance Fields

        private readonly Image _cellBackground;

        private readonly TextureBrush _cellBackgroundBrush;

        private RgbaChannel _channel;

        private Color _color;

        #endregion

        #region Constructors

        public RgbaColorSlider() {
            _cellBackground = new Bitmap(Resources.cellbackground);
            _cellBackgroundBrush = new TextureBrush(_cellBackground, WrapMode.Tile);
            //BarStyle = ColorBarStyle.Custom;
            Maximum = 255;
            Color = Color.Black;
            CreateScale();
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when the Channel property value changes
        /// </summary>
        [Category("Property Changed")]
        // ReSharper disable once EventNeverSubscribedTo.Global
        public event EventHandler ChannelChanged; // todo Keep or remove>

        /// <summary>
        ///     Occurs when the Color property value changes
        /// </summary>
        [Category("Property Changed")]
        // ReSharper disable once EventNeverSubscribedTo.Global
        public event EventHandler ColorChanged; // todo Keep or remove?

        #endregion

        #region Overridden Properties

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public override ColorBarStyle BarStyle {
        //    get { return base.BarStyle; }
        //    set { base.BarStyle = value; }
        //}

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public override Color Color1 {
        //    get { return base.Color1; }
        //    set { base.Color1 = value; }
        //}

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public override Color Color2 {
        //    get { return base.Color2; }
        //    set { base.Color2 = value; }
        //}

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public override Color Color3 {
        //    get { return base.Color3; }
        //    set { base.Color3 = value; }
        //}

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override float Maximum {
            get { return base.Maximum; }
            set { base.Maximum = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override float Minimum {
            get { return base.Minimum; }
            set { base.Minimum = value; }
        }

        public override float Value {
            get { return base.Value; }
            set { base.Value = (int) value; }
        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (_cellBackground != null) {
                    _cellBackground.Dispose();
                }

                if (_cellBackgroundBrush != null) {
                    _cellBackgroundBrush.Dispose();
                }
            }

            base.Dispose(disposing);
        }


        protected override void PaintBar(PaintEventArgs e) {
            e.Graphics.FillRectangle(_cellBackgroundBrush, BarBounds);

            base.PaintBar(e);
        }

        #region Properties

        [Category("Appearance")]
        [DefaultValue(typeof (RgbaChannel), "Red")]
        public RgbaChannel Channel {
// ReSharper disable MemberCanBePrivate.Global
            get { return _channel; }
// ReSharper restore MemberCanBePrivate.Global
            set {
                if (Channel == value) {
                    return;
                }

                _channel = value;
                OnChannelChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof (Color), "Black")]
        public Color Color {
            private get { return _color; }
            set {
                if (Color == value) {
                    return;
                }

                _color = value;
                OnColorChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region Members

        private void CreateScale() {
            CustomColors =
                new ColorCollection(
                    Enumerable.Range(0, 254)
                        .Select(
                            i =>
                                Color.FromArgb(Channel == RgbaChannel.Alpha ? i : Color.A,
                                    Channel == RgbaChannel.Red ? i : Color.R, Channel == RgbaChannel.Green ? i : Color.G,
                                    Channel == RgbaChannel.Blue ? i : Color.B)));
        }


        /// <summary>
        ///     Raises the <see cref="ChannelChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnChannelChanged(EventArgs e) {
            CreateScale();

            var handler = ChannelChanged;

            if (handler != null) {
                handler(this, e);
            }
        }


        /// <summary>
        ///     Raises the <see cref="ColorChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnColorChanged(EventArgs e) {
            CreateScale();
            Invalidate();

            var handler = ColorChanged;

            if (handler != null) {
                handler(this, e);
            }
        }

        #endregion
    }
}
