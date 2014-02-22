using System;
using System.Drawing;

namespace VixenPlusCommon {
    public class ColorEventArgs : EventArgs {
        private readonly Color _color;


        public ColorEventArgs(Color value) {
            _color = value;
        }


        public Color Color {
            get { return _color; }
        }
    }
}

