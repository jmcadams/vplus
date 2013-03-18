namespace LedTriksUtil
{
    using System;

    internal class ParsedFrame
    {
        private const int BOARD_HEIGHT = 0x10;
        private const int BOARD_WIDTH = 0x30;
        private byte[][,] m_boardCells;
        private int m_boardLayoutHeight;
        private int m_boardLayoutWidth;
        private int m_length;

        public ParsedFrame(int length, int boardLayoutWidth, int boardLayoutHeight)
        {
            this.Init(length, boardLayoutWidth, boardLayoutHeight);
        }

        public void AddCell(uint xy)
        {
            ushort num = (ushort) (xy >> 0x10);
            ushort num2 = (ushort) (xy & 0xffff);
            int num3 = num / 0x30;
            int num4 = num2 / 0x10;
            if ((num3 < this.m_boardLayoutWidth) && (num4 < this.m_boardLayoutHeight))
            {
                int index = (num4 * this.m_boardLayoutWidth) + num3;
                this.m_boardCells[index][num2 % 0x10, num % 0x30] = 1;
            }
        }

        public byte CellData(int rowIndex, int columnIndex)
        {
            byte num = 0;
            int length = this.m_boardCells.Length;
            for (int i = 0; i < length; i++)
            {
                num = (byte) (num | ((byte) (this.m_boardCells[i][rowIndex, columnIndex] << i)));
            }
            return num;
        }

        private void Init(int length, int boardLayoutWidth, int boardLayoutHeight)
        {
            this.m_length = length;
            this.m_boardLayoutWidth = boardLayoutWidth;
            this.m_boardLayoutHeight = boardLayoutHeight;
            int num = this.m_boardLayoutWidth * this.m_boardLayoutHeight;
            this.m_boardCells = new byte[num][,];
            for (int i = 0; i < num; i++)
            {
                this.m_boardCells[i] = new byte[0x10, 0x30];
            }
        }

        public ParsedFrame MergeWith(ParsedFrame frame)
        {
            ParsedFrame frame2 = new ParsedFrame(Math.Max(this.m_length, frame.m_length), Math.Min(this.m_boardLayoutWidth, frame.m_boardLayoutWidth), Math.Min(this.m_boardLayoutHeight, frame.m_boardLayoutHeight));
            int length = frame2.m_boardCells.Length;
            for (int i = 0; i < length; i++)
            {
                byte[,] buffer = this.m_boardCells[i];
                byte[,] buffer2 = frame.m_boardCells[i];
                byte[,] buffer3 = frame2.m_boardCells[i];
                int num4 = buffer3.GetLength(0);
                int num3 = buffer3.GetLength(1);
                for (int j = 0; j < num4; j++)
                {
                    for (int k = 0; k < num3; k++)
                    {
                        buffer3[j, k] = (byte) (buffer[j, k] | buffer2[j, k]);
                    }
                }
            }
            return frame2;
        }

        public static ParsedFrame ParseFrame(Frame frame, int boardLayoutWidth, int boardLayoutHeight)
        {
            ParsedFrame frame2 = new ParsedFrame(frame.Length, boardLayoutWidth, boardLayoutHeight);
            foreach (uint num in frame.Cells)
            {
                frame2.AddCell(num);
            }
            return frame2;
        }

        public int BoardCount
        {
            get
            {
                return (this.m_boardLayoutWidth * this.m_boardLayoutHeight);
            }
        }

        public int Length
        {
            get
            {
                return this.m_length;
            }
            set
            {
                this.m_length = value;
            }
        }
    }
}

