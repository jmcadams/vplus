using System.Collections.Generic;
using System.Drawing;

namespace VixenPlus
{
    internal class UndoItem
    {
        private readonly byte[,] _undoData;
        private Point _location;
        private readonly int[] _referencedChannels;
        private readonly EventSequence _sequence;
        private readonly UndoOriginalBehavior _undoBehavior;

        public UndoItem(Point location, byte[,] data, UndoOriginalBehavior undoBehavior, EventSequence sequence, List<int> currentOrder)
        {
            _location = location;
            _undoData = data;
            _undoBehavior = undoBehavior;
            _sequence = sequence;
            _referencedChannels = new int[data.GetLength(0)];
            for (int i = 0; i < _referencedChannels.Length; i++)
            {
                _referencedChannels[i] = currentOrder[location.Y + i];
            }
        }

        public override string ToString()
        {
            int num = _location.X * _sequence.EventPeriod;
            return string.Format("{0} of {1} x {2} cells at {3}:{4:d2}.{5:d3}", new object[] { _undoBehavior, _undoData.GetLength(1), _undoData.GetLength(0), num / 0xea60, (num % 0xea60) / 0x3e8, num % 0x3e8 });
        }

        public UndoOriginalBehavior Behavior
        {
            get
            {
                return _undoBehavior;
            }
        }

        public byte[,] Data
        {
            get
            {
                return _undoData;
            }
        }

        public Point Location
        {
            get
            {
                return _location;
            }
        }

        public int[] ReferencedChannels
        {
            get
            {
                return _referencedChannels;
            }
        }
    }
}

