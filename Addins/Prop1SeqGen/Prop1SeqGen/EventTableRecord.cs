namespace Prop1SeqGen
{
    using System;
    using System.Collections.Generic;

    internal class EventTableRecord
    {
        public int EventPeriodCount;
        public byte Value;

        public EventTableRecord(byte value)
        {
            this.Value = value;
            this.EventPeriodCount = 1;
        }

        public string[] GetRecordStrings()
        {
            List<string> list = new List<string>();
            for (int i = this.EventPeriodCount; i > 0; i -= 0xff)
            {
                if ((i > 0xff) && ((this.Value & 0x80) != 0))
                {
                    list.Add(string.Format("  EEPROM (%{0}, {1})", Convert.ToString((int) (this.Value & 0x7f), 2).PadLeft(8, '0'), Math.Min(0xff, i)));
                }
                else
                {
                    list.Add(string.Format("  EEPROM (%{0}, {1})", Convert.ToString(this.Value, 2).PadLeft(8, '0'), Math.Min(0xff, i)));
                }
            }
            return list.ToArray();
        }
    }
}

