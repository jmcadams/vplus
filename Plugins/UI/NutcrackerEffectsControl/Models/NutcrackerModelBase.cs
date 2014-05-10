using System;
using System.Drawing;
using System.Windows.Forms;

using Nutcracker.Effects;

namespace Nutcracker.Models {
    public class NutcrackerModelBase : UserControl {
        public const string TypeName = "NutcrackerModelBase";
        protected const int XyOffset = 2;

        public virtual string EffectName {
            get { return string.Empty; }
        }

        public virtual string Notes { get { return string.Empty; } }
        public bool IsLtoR { protected get; set; }

        internal int Rows;
        internal int Cols;
        internal NutcrackerNodes[,] Nodes;

        internal virtual void ResetNodes() {
            Nodes = new NutcrackerNodes[Rows, Cols];
            for (var row = 0; row < Rows; row++) {
                for (var col = 0; col < Cols; col++) {
                    Nodes[row, col] = new NutcrackerNodes();
                }
            }
        }


        internal void Draw(Control c, Graphics g) {
            var b = new Bitmap(c.Width, c.Height, g);
            for (var row = 0; row < Rows; row++) {
                for (var col = 0; col < Cols; col++) {
                    b.SetPixel(Nodes[row, col].Model.X, Nodes[row, col].Model.Y, Color.White);
                }
            }
            g.DrawImage(b, XyOffset, XyOffset);
        }
    }
}