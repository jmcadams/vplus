using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using VixenPlus;

namespace Matrix2 {
    public partial class Matrix : UserControl, INutcrackerModel {

        private int _rows;
        private int _cols;
        private NutcrackerNodes[,] _nodes;
        private bool _isLtoR;
        private readonly Timer _timer = new Timer { Interval = 20 };

        public Matrix() {
            InitializeComponent();
            _timer.Tick += control_ValueChanged;
        }

        public string EffectName {
            get { return "Matrix"; }
        }

        public string Notes {
            get { return string.Empty; }
        }

        public XmlElement Settings { get; set; }

        public NutcrackerNodes[,] InitializeNodes(Rectangle rect) {
            InitMatrix();
            return _nodes;
        }


        public bool SetDirection {
            set { _isLtoR = value; }
        }


        public void DrawPreview() {
            using (var g = pbPreview.CreateGraphics()) {
                g.Clear(Color.Black);
                var b = new Bitmap(pbPreview.Width, pbPreview.Height, g);
                for (var row = 0; row < _rows; row++) {
                    for (var col = 0; col < _cols; col++) {
                        b.SetPixel(_nodes[row, col].Model.X, _nodes[row, col].Model.Y, Color.White);
                    }
                }
                g.DrawImage(b, 0, 0);
            }
        }

        /*
        // initialize buffer coordinates
        // parm1=NumStrings
        // parm2=PixelsPerString
        // parm3=StrandsPerString
        void ModelClass::InitHMatrix()
        {
            int y,x,idx,stringnum,segmentnum;
            int NumStrands=parm1*parm3;
            int PixelsPerStrand=parm2/parm3;
            SetBufferSize(NumStrands,PixelsPerStrand);
            SetNodeCount(parm1*parm2);
            SetRenderSize(NumStrands,PixelsPerStrand);

            // create output mapping
            for (y=0; y < NumStrands; y++)
            {
                stringnum=y / parm3;
                segmentnum=y % parm3;
                for(x=0; x<PixelsPerStrand; x++)
                {
                    idx=stringnum * parm2 + segmentnum * PixelsPerStrand + x;
                    Nodes[idx].bufX=IsLtoR != (segmentnum % 2 == 0) ? PixelsPerStrand-x-1 : x;
                    Nodes[idx].bufY=y;
                    Nodes[idx].StringNum=stringnum;
                }
            }
        }
          
        void ModelClass::InitVMatrix()
            {
                int y,x,idx,stringnum,segmentnum;
                int NumStrands=parm1*parm3;
                int PixelsPerStrand=parm2/parm3;
                SetBufferSize(PixelsPerStrand,NumStrands);
                SetNodeCount(parm1*parm2);
                SetRenderSize(PixelsPerStrand,NumStrands);

                // create output mapping
                for (x=0; x < NumStrands; x++)
                {
                    stringnum=x / parm3;
                    segmentnum=x % parm3;
                    for(y=0; y < PixelsPerStrand; y++)
                    {
                        idx=stringnum * parm2 + segmentnum * PixelsPerStrand + y;
                        Nodes[idx].bufX=IsLtoR ? x : NumStrands-x-1;
                        Nodes[idx].bufY=(segmentnum % 2 == 0) ? y : PixelsPerStrand-y-1;
                        Nodes[idx].StringNum=stringnum;
                    }
                }
            }
        */

        private void InitMatrix() {
            var strandsPerString = (int)nudStrandCount.Value;

            var numStrands = _cols;
            var pixelsPerStrand = _rows;
            var xFactor = pbPreview.Width / _cols;
            var yFactor = pbPreview.Height / _rows;
            var index = 0;
            for (var strand = 0; strand < numStrands; strand++) {
                var segmentnum = strand % strandsPerString;
                for (var pixel = 0; pixel < pixelsPerStrand; pixel++) {
                    var y = index % _rows;
                    var x = index / _rows;
                    var ptX = _isLtoR ? strand : numStrands - strand - 1;
                    var ptY = (segmentnum % 2 != 0) ? pixel : pixelsPerStrand - pixel - 1;
                    _nodes[y, x].Model = new Point(ptX * xFactor, ptY * yFactor );
                    index++;
                }
            }
        }


        private void control_ValueChanged(object sender, EventArgs e) {
            //This is a hack to initially display the rendering since there isn't an after show event! - UGH!
            if (_timer.Enabled) {
                _timer.Stop();
            }
            var nodesPerString = (int)nudNodeCount.Value / (int)nudStrandCount.Value;
            var totalStringCount = (int)nudStringCount.Value * (int)nudStrandCount.Value;
            _rows = rbVertical.Checked ? nodesPerString : totalStringCount;
            _cols = rbVertical.Checked ? totalStringCount : nodesPerString;
            _nodes = new NutcrackerNodes[_rows, _cols];
            for (var row = 0; row < _rows; row++) {
                for (var col = 0; col < _cols; col++) {
                    _nodes[row, col] = new NutcrackerNodes();
                }
            }
            InitMatrix();
            DrawPreview();
        }

        private void Matrix_VisibleChanged(object sender, EventArgs e) {
            _timer.Start();
        }
    }
}
