namespace MIDIReader
{
    using System;

    internal static class Helpers
    {
        public static int ReadVarLength(byte[] bytes, ref int offset)
        {
            int num;
            if (((num = bytes[offset++]) & 0x80) > 0)
            {
                byte num2;
                num &= 0x7f;
                do
                {
                    num = (num << 7) + ((num2 = bytes[offset++]) & 0x7f);
                }
                while ((num2 & 0x80) > 0);
            }
            return num;
        }
    }
}

