using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Tree {
    public partial class Tree : UserControl, INutcrackerModel {

        private int _rows;
        private int _cols;
        private NutcrackerNodes[,] _nodes;
        private const int XyOffset = 5; 

        public Tree() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Tree"; }
        }

        public string Notes {
            get { return string.Empty; }
        }

        public XmlElement Settings { get; set; }


        public NutcrackerNodes[,] InitializeNodes(Rectangle rect) {
            InitializePreview(rect);
            return _nodes;
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


        private void InitializePreview(Rectangle previewRect) {
            InitMatrix();
            var degrees = rb360.Checked ? 360 : rb270.Checked ? 270 : rb180.Checked ? 180 : 90;
            if (_cols < 2 || _rows < 1) return;
            var factor = previewRect.Height / _rows;
            var renderWi = previewRect.Width / 2;
            var radians = 2.0 * Math.PI * degrees / 360.0;
            var radius = renderWi * 0.8;
            var startAngle = -radians / 2.0;
            var angleIncr = radians / (_cols - 1);
            for (var row = 0; row < _rows; row++) {
                for (var col = 0; col < _cols; col++) {
                    var angle = startAngle + _nodes[_rows - 1 - row, col].BufX * angleIncr;
                    var x0 = radius * Math.Sin(angle);
                    var x = (int) Math.Floor(x0 * (1.0 - (double) (_nodes[row, col].BufY) / _rows) + 0.5) + renderWi;
                    var y = _nodes[_rows - 1 - row, col].BufY * factor;
                    _nodes[_rows - 1 - row, col].Model = new Point(x, y);
                }
            }
        }


        private void InitMatrix() {
            var strandsPerString = (int) nudStrandCount.Value;

            var numStrands = _cols;
            var pixelsPerStrand = _rows;
            var index = 0;
            for (var strand = 0; strand < numStrands; strand++) {
                var segmentnum = strand % strandsPerString;
                for (var pixel = 0; pixel < pixelsPerStrand; pixel++) {
                    var y = index % _rows;
                    var x = index / _rows;
                    _nodes[y, x].BufX = IsLtoR ? strand : numStrands - strand - 1;
                    _nodes[y, x].BufY = (segmentnum % 2 != 0) ? pixel : pixelsPerStrand - pixel - 1;
                    index++;
                }
            }
        }


        private void control_ValueChanged(object sender, EventArgs e) {
            ResetNodes();
            var rect = pbPreview.DisplayRectangle;
            rect.Offset(-XyOffset * 2, -XyOffset * 2);
            InitializePreview(rect);
            DrawPreview();
        }


        private void ResetNodes() {
            _rows = (int) nudNodeCount.Value / (int) nudStrandCount.Value;
            _cols = (int) nudStringCount.Value * (int) nudStrandCount.Value;
            _nodes = new NutcrackerNodes[_rows,_cols];
            for (var row = 0; row < _rows; row++) {
                for (var col = 0; col < _cols; col++) {
                    _nodes[row, col] = new NutcrackerNodes();
                }
            }
        }
    }
}
