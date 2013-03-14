namespace MIDIReader
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class MIDITrack
    {
        private int m_currentTime;
        private List<MIDIEvent> m_events;
        private static int m_microsPerQuarter = 0;
        private MThd m_mthd;
        private int m_readCursor;
        private static int m_timeSignatureDenominator = 4;

        public MIDITrack(MTrk mtrk, MThd mthd)
        {
            MIDIEvent event2;
            this.m_readCursor = 0;
            this.m_currentTime = 0;
            this.m_mthd = mthd;
            this.m_events = new List<MIDIEvent>();
            int offset = 0;
            for (byte i = 0; offset < mtrk.Data.Length; i = (byte) ((((byte) event2.EventType) << 4) | event2.Channel))
            {
                this.m_events.Add(event2 = new MIDIEvent(mtrk.Data, ref offset, i));
            }
        }

        private void CalcDeltaTicksPerMs()
        {
            if (m_microsPerQuarter != 0)
            {
                int num = 2 ^ (m_timeSignatureDenominator - 2);
                double num2 = 1.0 / ((double) num);
            }
        }

        private int DeltaTicksToMilliseconds(int deltaTicks)
        {
            return (int) ((((float) deltaTicks) / ((float) this.m_mthd.PulsesPerQuarterNote)) * (m_microsPerQuarter / 0x3e8));
        }

        public void DumpTo(StreamWriter writer, string format)
        {
            int deltaTicks = 0;
            foreach (MIDIEvent event2 in this.m_events)
            {
                deltaTicks += event2.DeltaTime;
                writer.WriteLine(format, new object[] { deltaTicks, this.DeltaTicksToMilliseconds(deltaTicks), event2.EventType.ToString(), string.Format("{0}, {1}", event2.Param1, event2.Param2) });
            }
        }

        public List<MIDIEvent> GetEventsOfType(MIDIEventType eventType)
        {
            List<MIDIEvent> list = new List<MIDIEvent>();
            foreach (MIDIEvent event2 in this.m_events)
            {
                if (event2.EventType == eventType)
                {
                    list.Add(event2);
                }
            }
            return list;
        }

        private int MillisecondsToDeltaTicks(int milliseconds)
        {
            if (milliseconds == 0)
            {
                return 0;
            }
            return (int) ((1.0 / ((double) this.DeltaTicksToMilliseconds(1))) * milliseconds);
        }

        public void ReadAllEvents()
        {
            int milliseconds = 0xea60;
            byte[] noteStates = new byte[0x58];
            while (this.m_readCursor < this.m_events.Count)
            {
                this.ReadTo(milliseconds, noteStates);
                milliseconds += 0xea60;
            }
        }

        public void ReadTo(int milliseconds, byte[] noteStates)
        {
            if (this.m_readCursor >= this.m_events.Count)
            {
                for (int i = 0; i < noteStates.Length; i++)
                {
                    noteStates[i] = 0;
                }
            }
            else
            {
                while (this.DeltaTicksToMilliseconds(this.m_currentTime) <= milliseconds)
                {
                    MIDIEvent event2 = this.m_events[this.m_readCursor];
                    switch (event2.EventType)
                    {
                        case MIDIEventType.NoteOff:
                            noteStates[event2.Param1 - 0x15] = 0;
                            goto Label_00DE;

                        case MIDIEventType.NoteOn:
                            noteStates[event2.Param1 - 0x15] = event2.Param2;
                            goto Label_00DE;

                        case MIDIEventType.MetaEvent:
                            switch (((MIDIMetaEventType) event2.MetaEventType))
                            {
                                case MIDIMetaEventType.SetTempo:
                                    m_microsPerQuarter = ((event2.MetaEventData[0] << 0x10) | (event2.MetaEventData[1] << 8)) | event2.MetaEventData[2];
                                    break;

                                case MIDIMetaEventType.TimeSignature:
                                    goto Label_00CD;
                            }
                            goto Label_00DE;
                    }
                    goto Label_00DE;
                Label_00CD:
                    m_timeSignatureDenominator = event2.MetaEventData[1];
                Label_00DE:
                    if (++this.m_readCursor < this.m_events.Count)
                    {
                        this.m_currentTime += this.m_events[this.m_readCursor].DeltaTime;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void Reset()
        {
            this.m_readCursor = 0;
            this.m_currentTime = 0;
        }

        private int TranslateTime(int deltaTimeTicks)
        {
            return (int) ((((double) deltaTimeTicks) / ((double) this.m_mthd.PulsesPerQuarterNote)) * (((double) m_microsPerQuarter) / 1000.0));
        }

        public int MicrosPerQuarter
        {
            get
            {
                return m_microsPerQuarter;
            }
        }
    }
}

