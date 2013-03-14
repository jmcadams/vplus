namespace Prop2SeqGen
{
    using System;
    using System.Collections.Generic;

    internal class EventTableRecord
    {
        public ushort EventPeriodCount;
        public ushort Value;

        public EventTableRecord(ushort value)
        {
            this.Value = value;
            this.EventPeriodCount = 1;
        }

        public string[] GetRecordStrings()
        {
            List<string> list = new List<string>();
            for (int i = this.EventPeriodCount; i > 0; i -= 0xff)
            {
                if ((i > 0xff) && ((this.Value & 0x8000) != 0))
                {
                    list.Add(string.Format("  DATA  Word %{0}, {1}", Convert.ToString((int) (this.Value & 0x7fff), 2).PadLeft(0x10, '0'), Math.Min(0xff, i)));
                }
                else
                {
                    list.Add(string.Format("  DATA  Word %{0}, {1}", Convert.ToString((int) this.Value, 2).PadLeft(0x10, '0'), Math.Min(0xff, i)));
                }
            }
            return list.ToArray();
        }
    }
}

