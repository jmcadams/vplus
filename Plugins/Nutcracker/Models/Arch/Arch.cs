using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using VixenPlus;

namespace Arch {
    public partial class Arch : UserControl, INutcrackerModel {

        private int _rows;
        private int _cols; 
        private NutcrackerNodes[,] _nodes;
        private const int XyOffset = 5; 

        public Arch() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Arch"; }
        }

        public string Notes {
            get { return "Broken!"; }
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
                        b.SetPixel(_nodes[row, col].Model.X, _nodes[row, col].Model.Y, Color.White);
                    }
                }
                g.DrawImage(b, XyOffset, XyOffset);
            }
        }

        //private void InitLine() {
        //    var idx = 0;
        //    var archCount = (int) nudArchCount.Value; //1
        //    var nodeCount = (int) nudNodeCount.Value; //2
        //    for (var ns = 0; ns < archCount; ns++) {
        //        for (var x = 0; x < nodeCount; x++) {
        //            _nodes[0, idx].Model = new Point(IsLtoR ? x : nodeCount - x - 1, 0);
        //            idx++;
        //        }
        //    }
        //}


        private void ResetNodes() {
            _rows = (int)nudNodeCount.Value; //2
            _cols = (int)nudArchCount.Value; //1
            _nodes = new NutcrackerNodes[_rows, _cols];
            for (var row = 0; row < _rows; row++) {
                for (var col = 0; col < _cols; col++) {
                    _nodes[row, col] = new NutcrackerNodes {Model = new Point(IsLtoR ? col : _cols - col - 1, 0)};
                }
            }
        }


        // Set screen coordinates for arches
        private void SetArchCoord() {
            var nodeCount = _cols * _rows;

            //var idx = IsLtoR ? 0 : nodeCount - 1;
            var incr = IsLtoR ? 1 : -1;
            //SetRenderSize(parm2,NodeCount*2);
            double midpt = _rows;
            var angleIncr = Math.PI / _rows;
            for (var ns = 0; ns < _cols; ns++) {
                var angle = -1.0 * Math.PI / 2.0;
                var xoffset = ns * _rows * 2 - nodeCount;
                for (var x = 0; x < _rows; x++) {
                    var ptX = xoffset + (int)Math.Floor(midpt * Math.Sin(angle) + midpt) + nodeCount;
                    var ptY = (int) Math.Floor(midpt * Math.Cos(angle) + 0.5);
                    System.Diagnostics.Debug.Print("X:{0}, Y:{1}", ptX, ptY);
                    _nodes[x, ns].Model = new Point(ptX, ptY);
                    angle += angleIncr;
                    //idx+=incr;
                }
            }
        }


        private void control_ValueChanged(object sender, System.EventArgs e)
        {
            ResetNodes();
            SetArchCoord();
            DrawPreview();
        }
    }
}
