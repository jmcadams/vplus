using System.Drawing;

namespace VixenPlusCommon {
    public static class SparkleTree {
        public static void DrawTrees(Graphics g, byte[,] effectValues, int tickCount) {
            using (var solidBrush = new SolidBrush(Color.Black)) {
                solidBrush.Color = Color.FromArgb(effectValues[0, tickCount], Color.Red);
                g.FillPolygon(solidBrush, new[] {new Point(22, 36), new Point(37, 91), new Point(7, 91)});

                solidBrush.Color = Color.FromArgb(effectValues[1, tickCount], Color.Green);
                g.FillPolygon(solidBrush, new[] {new Point(67, 36), new Point(82, 91), new Point(52, 91)});

                solidBrush.Color = Color.FromArgb(effectValues[2, tickCount], Color.Blue);
                g.FillPolygon(solidBrush, new[] {new Point(112, 36), new Point(127, 91), new Point(97, 91)});

                solidBrush.Color = Color.FromArgb(effectValues[3, tickCount], Color.White);
                g.FillPolygon(solidBrush, new[] {new Point(157, 36), new Point(172, 91), new Point(142, 91)});
            }
        }
    }
}
