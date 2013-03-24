namespace VixenPlus
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    internal class UndoItem
    {
        private byte[,] m_data;
        private Point m_location;
        private int[] m_referencedChannels;
        private EventSequence m_sequence;
        private UndoOriginalBehavior m_undoBehavior;

        public UndoItem(Point location, byte[,] data, UndoOriginalBehavior undoBehavior, EventSequence sequence, List<int> currentOrder)
        {
            this.m_location = location;
            this.m_data = data;
            this.m_undoBehavior = undoBehavior;
            this.m_sequence = sequence;
            this.m_referencedChannels = new int[data.GetLength(0)];
            for (int i = 0; i < this.m_referencedChannels.Length; i++)
            {
                this.m_referencedChannels[i] = currentOrder[location.Y + i];
            }
        }

        public override string ToString()
        {
            int num = this.m_location.X * this.m_sequence.EventPeriod;
            return string.Format("{0} of {1} x {2} cells at {3}:{4:d2}.{5:d3}", new object[] { this.m_undoBehavior, this.m_data.GetLength(1), this.m_data.GetLength(0), num / 0xea60, (num % 0xea60) / 0x3e8, num % 0x3e8 });
        }

        public UndoOriginalBehavior Behavior
        {
            get
            {
                return this.m_undoBehavior;
            }
        }

        public byte[,] Data
        {
            get
            {
                return this.m_data;
            }
        }

        public Point Location
        {
            get
            {
                return this.m_location;
            }
        }

        public int[] ReferencedChannels
        {
            get
            {
                return this.m_referencedChannels;
            }
        }
    }
}

