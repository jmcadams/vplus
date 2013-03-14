namespace MIDIReader
{
    using System;
    using System.Collections.Generic;

    internal class MIDIEvent
    {
        public byte Channel;
        public int DeltaTime;
        public MIDIEventType EventType;
        public byte[] MetaEventData;
        public int MetaEventDataLength;
        public byte MetaEventType;
        public byte Param1;
        public byte Param2;
        public List<byte[]> SysExData;

        public MIDIEvent(byte[] bytes, ref int offset, byte lastEvent)
        {
            byte num;
            this.DeltaTime = Helpers.ReadVarLength(bytes, ref offset);
            if ((bytes[offset] & 0x80) == 0)
            {
                num = lastEvent;
            }
            else
            {
                num = bytes[offset++];
            }
            if (num == 0xff)
            {
                this.EventType = MIDIEventType.MetaEvent;
                this.MetaEventType = bytes[offset++];
                this.MetaEventDataLength = Helpers.ReadVarLength(bytes, ref offset);
                this.MetaEventData = new byte[this.MetaEventDataLength];
                Array.Copy(bytes, offset, this.MetaEventData, 0, this.MetaEventDataLength);
                offset += this.MetaEventDataLength;
            }
            else
            {
                int num2;
                byte[] buffer;
                if (num == 240)
                {
                    this.EventType = MIDIEventType.SysExEvent;
                    this.SysExData = new List<byte[]>();
                    num2 = Helpers.ReadVarLength(bytes, ref offset);
                    buffer = new byte[num2];
                    Array.Copy(bytes, offset, buffer, 0, num2);
                    offset += num2;
                    this.SysExData.Add(buffer);
                    while (buffer[num2 - 1] != 0xf7)
                    {
                        offset++;
                        num2 = Helpers.ReadVarLength(bytes, ref offset);
                        buffer = new byte[num2];
                        Array.Copy(bytes, offset, buffer, 0, num2);
                        offset += num2;
                        this.SysExData.Add(buffer);
                    }
                }
                else if (num == 0xf7)
                {
                    this.EventType = MIDIEventType.AuthorizationSysExEvent;
                    this.SysExData = new List<byte[]>();
                    num2 = Helpers.ReadVarLength(bytes, ref offset);
                    buffer = new byte[num2];
                    Array.Copy(bytes, offset, buffer, 0, num2);
                    offset += num2;
                    this.SysExData.Add(buffer);
                }
                else
                {
                    this.EventType = (MIDIEventType) (num >> 4);
                    this.Channel = (byte) (num & 15);
                    this.Param1 = bytes[offset++];
                    if ((this.EventType != MIDIEventType.ProgramChange) && (this.EventType != MIDIEventType.ChannelAftertouch))
                    {
                        this.Param2 = bytes[offset++];
                    }
                }
            }
        }
    }
}

