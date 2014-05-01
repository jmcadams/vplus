using System;
using System.Drawing;
using System.Windows.Forms;

using Nutcracker.Effects;

using NutcrackerEffects.Models;

using VixenPlus.Annotations;

namespace Nutcracker.Models {
    [UsedImplicitly]
    public partial class Arch : UserControl, INutcrackerModel {

        private int _rows;
        private int _cols; 
        private NutcrackerNodes[,] _nodes;
        private const int XyOffset = 2; 

        public Arch() {
            InitializeComponent();
        }


        public string EffectName {
            get { return "Arch"; }
        }

        public string Notes {
            get { return "Define up to 10 arches"; }
        }

/*
        public XmlElement Settings { get; set; }
*/


/*
        public NutcrackerNodes[,] InitializeNodes(Rectangle rect) {
            throw new NotImplementedException();
        }
*/


        public bool IsLtoR { private get; set; }


        private void DrawPreview(Control ctrl) {
            using (var g = ctrl.CreateGraphics()) {
                g.Clear(Color.Black);
                var b = new Bitmap(ctrl.Width, ctrl.Height, g);
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

// Angles are expressed as a number between 0 and 1.  .25 = 90 degrees.
// If you prefer using degrees, write 90 degrees like so "90/360".
//function drawArc(centerX, centerY, radius, startAngle, arcAngle, steps){
/*
    private void DrawArc (int centerX, int centerY, double radius, double startAngle, double arcAngle, int steps){
        //
    // For convenience, store the number of radians in a full circle.
    const double twoPi = 2 * Math.PI;
    //
    // To determine the size of the angle between each point on the
    // arc, divide the overall angle by the total number of points.
    var angleStep = arcAngle/steps;
    //
    // Determine coordinates of first point using basic circle math.
    var xx = centerX + Math.Cos(startAngle * twoPi) * radius;
    var yy = centerY + Math.Sin(startAngle * twoPi) * radius;
    //
    // Move to the first point.
    //moveTo(xx, yy);
    //
    // Draw a line to each point on the arc.
    //for(var i=1; i<=steps; i++){
        //
        // Increment the angle by "angleStep".
        //var angle = startAngle + i * angleStep;
        //
        // Determine next point's coordinates using basic circle math.
        //xx = centerX + Math.Cos(angle * twoPi) * radius;
        //yy = centerY + Math.Sin(angle * twoPi) * radius;
        //
        // Draw a line to the next point.
        //lineTo(xx, yy);
    //}
}
*/
//
// Set a line style so we can see what we are drawing.
// lineStyle(0, 0xFF0000);
//
// Draw an arc with a center of (250, 250) and a radius of 200
// that starts at an angle of 45 degrees then rotates counter-
// clockwise 90 degrees.  We'll span the arc with 20 evenly spaced points.
// drawArc(250, 250, 200, 45/360, -90/360, 20);
//

        // Set screen coordinates for arches
        private void SetArchCoord(Control ctrl) {
            var radius = ((ctrl.Size.Width / 2) - (XyOffset * 2)) / _cols;
            const double twoPi = Math.PI * 2;
            var angleStep = 0.5d / (_rows - 1);

            var centerX = radius;
            var centerY = ctrl.Size.Height - (radius / 2) + XyOffset;

            //var idx = IsLtoR ? 1 : -1;
            //SetRenderSize(parm2,NodeCount*2);
            for (var c = 0; c < _cols; c++) {
                for (var ns = 0; ns < _rows; ns++) {
                    var angle = 0.5d + ns * angleStep;
                    var ptX = centerX + (int) Math.Floor(Math.Cos(angle * twoPi) * radius);
                    var ptY = centerY + (int) Math.Floor(Math.Sin(angle * twoPi) * radius);
                    //Debug.Print("X:{0:000} Y:{1:000} A:{2}", ptX, ptY, angle);
                    _nodes[ns, c].Model = new Point(ptX, ptY);
                }
                centerX += radius * 2;
            }
        }


        private void control_ValueChanged(object sender, EventArgs e)
        {
            ResetNodes();
            SetArchCoord(pbPreview);
            DrawPreview(pbPreview);
        }
    }
}
