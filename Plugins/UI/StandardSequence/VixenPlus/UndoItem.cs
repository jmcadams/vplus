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
            for (int i = 0; i < ReferencedChannels.Length; i++)
            {
                ReferencedChannels[i] = currentOrder[location.Y + i];
            }
        }

        public override string ToString()
        {
            var num = Location.X * _sequence.EventPeriod;
            return string.Format("{0} of {1} x {2} cells at {3}:{4:d2}.{5:d3}", new object[] { OriginalAction, Data.GetLength(1), Data.GetLength(0), num / 60000, (num % 60000) / 1000, num % 1000 });
        }


        public UndoOriginalBehavior Behavior { get; private set; }

        public byte[,] Data { get; private set; }

        public Point Location { get; private set; }

        public int[] ReferencedChannels { get; private set; }

        public string OriginalAction { get; private set; }
    }
}

