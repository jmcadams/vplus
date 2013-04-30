using System.Collections.Generic;
using System.Drawing;
using CommonUtils;

namespace VixenPlus {
    internal class UndoItem {
        private readonly EventSequence _sequence;


        public UndoItem(Point location, byte[,] data, UndoOriginalBehavior undoBehavior, EventSequence sequence, IList<int> currentOrder,
                        string originalAction) {
            Location = location;
            Data = data;
            Behavior = undoBehavior;
            OriginalAction = originalAction;
            _sequence = sequence;
            ReferencedChannels = new int[data.GetLength(0)];
            for (var i = 0; i < ReferencedChannels.Length; i++) {
                ReferencedChannels[i] = currentOrder[location.Y + i];
            }
        }


        public override string ToString() {
            var formattedTime = Utils.TimeFormatWithMills(Location.X * _sequence.EventPeriod);
            var columns = Data.GetLength(Utils.IndexColsOrWidth);
            var rows = Data.GetLength(Utils.IndexRowsOrHeight);

            return (columns > 1 || rows > 1)
                       ? string.Format("{0} of {1} x {2} cells at {3}", OriginalAction, columns, rows, formattedTime)
                       : string.Format("{0} {1}", OriginalAction, formattedTime);
        }


        public UndoOriginalBehavior Behavior { get; private set; }

        public byte[,] Data { get; private set; }

        public Point Location { get; private set; }

        public int[] ReferencedChannels { get; private set; }

        public string OriginalAction { get; private set; }
    }
}