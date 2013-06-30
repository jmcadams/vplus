using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using VixenPlus;

namespace WindowFrame {
    public partial class WindowFrame : UserControl, INutcrackerModel {

        private int _rows;
        private int _cols;
        private NutcrackerNodes[,] _nodes;
        private const int XyOffset = 5; 

        public WindowFrame() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Window Frame"; }
        }

        public string Notes {
            get { return string.Empty; }
        }

        public XmlElement Settings { get; set; }


        public NutcrackerNodes[,] InitializeNodes(Rectangle rect) {
            throw new System.NotImplementedException();
        }


        public bool IsLtoR { get; set; }


        public void DrawPreview() {
            using (var g = pbPreview.CreateGraphics()) {
                g.Clear(Color.Black);
                var b = new Bitmap(pbPreview.Width, pbPreview.Height, g);
                for (var row = 0; row < _rows; row++) {
                    for (var col = 0; col < _cols; col++) {
                        b.SetPixel(_nodes[row, col].Model.X, _rows - 1 - _nodes[row, col].Model.Y, Color.White);
                    }
                }
                g.DrawImage(b, XyOffset, XyOffset);
            }
        }

        private void InitFrame() {
            var top = (int)nudTopCount.Value;
            var sides = (int)nudSideCount.Value;
            var bottom = (int)nudBottomCount.Value;

            _cols = Math.Max(top, bottom) + 2;
            _rows = sides;
            _nodes = new NutcrackerNodes[_rows, _cols];
            for (var row = 0; row < _rows; row++) {
                for (var col = 0; col < _cols; col++) {
                    _nodes[row, col] = new NutcrackerNodes();
                }
            }

            int x,y;
            //SetNodeCount(top+2*sides+bottom);
            if (top >= bottom) {
                // first node is bottom left and we count up the left side, across the top, and down the right
                var frameWidth = top + 2; // allow for left/right columns

                // up side 1
                x = IsLtoR ? 0 : frameWidth - 1;
                for (y = 0; y < sides; y++) {
                    _nodes[y, x].Model = new Point(x, y);
                }
                // across top
                y = sides - 1;
                for (x = 0; x < top; x++) {
                    _nodes[y, x].Model = new Point(IsLtoR ? x + 1 : top - x, y);
                }
                // down side 2
                x = IsLtoR ? frameWidth - 1 : 0;
                for (y = sides - 1; y >= 0; y--) {
                    _nodes[y, x].Model = new Point(x, y);
                }
                // across bottom
                y = 0;
                for (x = 0; x < bottom; x++) {
                    _nodes[y, x].Model = new Point(IsLtoR ? top - x : x + 1, y);
                }
            }
            else {
                // first node is top left and we count down the left side, across the bottom, and up the right
                var frameWidth = bottom + 2;

                // down side 1
                x = IsLtoR ? 0 : frameWidth - 1;
                for (y = sides - 1; y >= 0; y--) {
                    _nodes[y, x].Model = new Point(x, y);
                }
                // across bottom
                y = 0;
                for (x = 0; x < bottom; x++) {
                    _nodes[y, x].Model = new Point(IsLtoR ? x + 1 : bottom - x, y);
                }
                // up side 2
                x = IsLtoR ? frameWidth - 1 : 0;
                for (y = 0; y < sides; y++) {
                    _nodes[y, x].Model = new Point(x, y);
                }
                // across top
                y = sides - 1;
                for (x = 0; x < top; x++) {
                    _nodes[y, x].Model = new Point(IsLtoR ? bottom - x : x + 1, y);
                }
            }
        }

        private void control_ValueChanged(object sender, System.EventArgs e) {
            InitFrame();
            DrawPreview();
        }
        /*
         // initialize screen coordinates
void ModelClass::CopyBufCoord2ScreenCoord()
{
    size_t NodeCount=GetNodeCount();
    int xoffset=RenderWi/2;
    for(size_t i=0; i<NodeCount; i++)
    {
        Nodes[i].screenX = Nodes[i].bufX - xoffset;
        Nodes[i].screenY = Nodes[i].bufY;
    }
}
         */
    }
}
