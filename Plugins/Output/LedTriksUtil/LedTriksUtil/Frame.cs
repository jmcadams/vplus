namespace LedTriksUtil
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Vixen;

    public class Frame
    {
        private List<uint> m_cells;
        private int m_length;

        public Frame(int length)
        {
            this.m_cells = new List<uint>();
            this.m_length = length;
        }

        public Frame(XmlNode frameNode)
        {
            this.m_length = int.Parse(frameNode.Attributes["length"].Value);
            this.m_cells = new List<uint>();
            byte[] buffer = Convert.FromBase64String(frameNode.InnerText);
            for (int i = 0; i < buffer.Length; i += 4)
            {
                this.m_cells.Add(BitConverter.ToUInt32(buffer, i));
            }
        }

        public Frame Clone()
        {
            Frame frame = new Frame(this.m_length);
            frame.m_cells = new List<uint>();
            frame.m_cells.AddRange(this.m_cells);
            return frame;
        }

        public Frame MergeWith(Frame frame)
        {
            Frame frame2 = new Frame(Math.Max(this.m_length, frame.m_length));
            frame2.m_cells.AddRange(this.m_cells);
            foreach (uint num in frame.Cells)
            {
                if (!frame2.m_cells.Contains(num))
                {
                    frame2.m_cells.Add(num);
                }
            }
            return frame2;
        }

        public void SaveToXml(XmlNode parentNode)
        {
            XmlNode node = Xml.SetNewValue(parentNode, "Frame", string.Empty);
            Xml.SetAttribute(node, "length", this.m_length.ToString());
            List<byte> list = new List<byte>();
            foreach (uint num in this.m_cells)
            {
                list.AddRange(BitConverter.GetBytes(num));
            }
            node.InnerText = Convert.ToBase64String(list.ToArray());
        }

        public List<uint> Cells
        {
            get
            {
                return this.m_cells;
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

