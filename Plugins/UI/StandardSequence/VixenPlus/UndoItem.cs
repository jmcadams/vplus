using System.Collections.Generic;
using System.Drawing;

namespace VixenPlus
{
    internal class UndoItem
    {
        private readonly EventSequence _sequence;


        public UndoItem(Point location, byte[,] data, UndoOriginalBehavior undoBehavior, EventSequence sequence, List<int> currentOrder, string originalAction)
        {
            Location = location;
            Data = data;
            Behavior = undoBehavior;
            OriginalAction = originalAction;
            _sequence = sequence;
            ReferencedChannels = new int[data.GetLength(0)];
            for (var i = 0; i < ReferencedChannels.Length; i++)
            {
                ReferencedChannels[i] = currentOrder[location.Y + i];
            }
        }


        public override string ToString()
        {
            var mills = Location.X * _sequence.EventPeriod;
            var formattedTime = string.Format("at {0}:{1:d2}.{2:d3}", mills / 60000, (mills % 60000) / 1000, mills % 1000);
            
            var width = Data.GetLength(1);
            var length = Data.GetLength(0);
            
            return (width > 1 || length > 1)
                ? string.Format("{0} of {1} x {2} cells {3}", OriginalAction, width, length, formattedTime)
                : string.Format("{0} {1}",  OriginalAction,  formattedTime );
        }


        public UndoOriginalBehavior Behavior { get; private set; }

        public byte[,] Data { get; private set; }

        public Point Location { get; private set; }

        public int[] ReferencedChannels { get; private set; }

        public string OriginalAction { get; private set; }
    }
}

