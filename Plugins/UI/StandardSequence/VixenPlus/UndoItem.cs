using System.Collections.Generic;
using System.Linq;

using CommonUtils;

namespace VixenEditor.VixenPlus {
    internal class UndoItem {
        private readonly int _eventPeriod;


        public UndoItem(int columnOffset, byte[,] data, UndoOriginalBehavior undoBehavior, int eventPeriod, IEnumerable<Channel> currentOrder,
                        string originalAction) {
            ColumnOffset = columnOffset;
            Data = data;
            Behavior = undoBehavior;
            OriginalAction = originalAction;
            _eventPeriod = eventPeriod;
            ReferencedChannels = currentOrder.ToArray();
        }


        public override string ToString() {
            var formattedTime = (ColumnOffset * _eventPeriod).FormatFull();
            var columns = Data.GetLength(Utils.IndexColsOrWidth);
            var rows = Data.GetLength(Utils.IndexRowsOrHeight);

            return (columns > 1 || rows > 1)
                       ? string.Format("{0} of {1} x {2} cells at {3}", OriginalAction, columns, rows, formattedTime)
                       : string.Format("{0} {1}", OriginalAction, formattedTime);
        }


        public UndoOriginalBehavior Behavior { get; private set; }

        public byte[,] Data { get; private set; }

        public int ColumnOffset { get; private set; }

        public Channel[] ReferencedChannels { get; private set; }

        public string OriginalAction { get; private set; }
    }
}
