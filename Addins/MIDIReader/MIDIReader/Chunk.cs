namespace MIDIReader
{
    using System;
    using System.IO;

    internal class Chunk
    {
        private byte[] m_data;
        private int m_size;
        private string m_stringID;

        public Chunk(string id, Stream fileStream)
        {
            byte[] buffer = new byte[4];
            this.m_stringID = id;
            fileStream.Read(buffer, 0, 4);
            this.m_size = this.BigToLittleEndian32(buffer);
            this.m_data = new byte[this.m_size];
            fileStream.Read(this.m_data, 0, this.m_size);
        }

        protected short BigToLittleEndian16(byte[] values)
        {
            return this.BigToLittleEndian16(values, 0);
        }

        protected short BigToLittleEndian16(byte[] values, int startIndex)
        {
            Array.Reverse(values, startIndex, 2);
            return BitConverter.ToInt16(values, startIndex);
        }

        protected int BigToLittleEndian32(byte[] values)
        {
            return this.BigToLittleEndian32(values, 0);
        }

        protected int BigToLittleEndian32(byte[] values, int startIndex)
        {
            Array.Reverse(values, startIndex, 4);
            return BitConverter.ToInt32(values, startIndex);
        }

        public byte[] Data
        {
            get
            {
                return this.m_data;
            }
        }

        public string ID
        {
            get
            {
                return this.m_stringID;
            }
        }
    }
}

