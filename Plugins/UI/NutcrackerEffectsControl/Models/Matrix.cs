using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus;
using VixenPlus.Annotations;

namespace NutcrackerEffectsControl.Models {
    [UsedImplicitly]
    public partial class Matrix : UserControl, INutcrackerModel {

        private int _rows;
        private int _cols;
        private NutcrackerNodes[,] _nodes;
        private const int XyOffset = 5; 

        public Matrix() {
            InitializeComponent();
        }

        public string EffectName {
            get { return "Matrix"; }
        }

        public string Notes {
            get { return string.Empty; }
        }

/*
        public XmlElement Settings { get; set; }
*/

/*
        public NutcrackerNodes[,] InitializeNodes(Rectangle rect) {
            InitMatrix();
            return _nodes;
        }
*/


        public bool IsLtoR { private get; set; }


        private void DrawPreview() {
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

        private void InitMatrix() {
            var strandsPerString = (int)nudStrandCount.Value;

            var numStrands = _cols;
            var pixelsPerStrand = _rows;
            var xFactor = (pbPreview.Width - (XyOffset * 2)) / _cols;
            var yFactor = (pbPreview.Height - (XyOffset * 2)) / _rows;
            var index = 0;
            for (var strand = 0; strand < numStrands; strand++) {
                var segmentnum = strand % strandsPerString;
                for (var pixel = 0; pixel < pixelsPerStrand; pixel++) {
                    var y = index % _rows;
                    var x = index / _rows;
                    var ptX = IsLtoR ? strand : numStrands - strand - 1;
                    var ptY = (segmentnum % 2 != 0) ? pixel : pixelsPerStrand - pixel - 1;
                    _nodes[y, x].Model = new Point(ptX * xFactor, ptY * yFactor );
                    index++;
                }
            }
        }


        private void control_ValueChanged(object sender, EventArgs e) {
            ResetNodes();
            InitMatrix();
            DrawPreview();
        }


        private void ResetNodes() {
            var nodesPerString = (int) nudNodeCount.Value / (int) nudStrandCount.Value;
            var totalStringCount = (int) nudStringCount.Value * (int) nudStrandCount.Value;
            _rows = rbVertical.Checked ? nodesPerString : totalStringCount;
            _cols = rbVertical.Checked ? totalStringCount : nodesPerString;
            _nodes = new NutcrackerNodes[_rows,_cols];
            for (var row = 0; row < _rows; row++) {
                for (var col = 0; col < _cols; col++) {
                    _nodes[row, col] = new NutcrackerNodes();
                }
            }
        }
    }
}
