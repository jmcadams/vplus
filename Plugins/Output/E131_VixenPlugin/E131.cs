//=====================================================================
//
//	E131.cs - common E1.31 classes
//
//		written from the ground up based on the protocol specifications
//		from E1.31 published documentation available. in no way is
//		this claimed to be an official implementation endorsed or
//		certified in any way.
//
//		mainly a collection of classes used to build, send, receive,
//		and manipulate the E1.31 protocol in a logical vs. physical
//		format. the three layers are represented as C# classes without
//		concerns of actual transport formatting. the PhyBuffer member
//		converts the values to/from a byte array for transport in
//		network normal byte ordering and packing.
//
//		this is not the most efficient way. it tries to be object
//		oriented instead of efficient to cleanly implement the
//		protocol.
//
//		there are other ways through interop marshalling that may
//		be more efficient for the 'conversion' between logical vs.
//		physical but they may then be less efficient for manipulation
//		of the resultant data within C# with its System.Object based
//		reference variable etc. and there is always the big-endian
//		vs. little-endian conversions to take into account.
//
//		however once built, if a copy of the phybuffer is preserved,
//		it only needs to be 'patched' in two places (sequence # and slots)
//		and reused to send another packet of the same format.
//
//		version 1.0.0.0 - 1 june 2010
//      version 1.1.0.0 - 1 July 2013 - John McAdams for Vixen+
//
//=====================================================================

//=====================================================================
//
// Copyright (c) 2010 Joshua 1 Systems Inc. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are
// permitted provided that the following conditions are met:
//
//    1. Redistributions of source code must retain the above copyright notice, this list of
//       conditions and the following disclaimer.
//
//    2. Redistributions in binary form must reproduce the above copyright notice, this list
//       of conditions and the following disclaimer in the documentation and/or other materials
//       provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY JOSHUA 1 SYSTEMS INC. "AS IS" AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
// ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
// ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// The views and conclusions contained in the software and documentation are those of the
// authors and should not be interpreted as representing official policies, either expressed
// or implied, of Joshua 1 Systems Inc.
//
//=====================================================================

using System;
using System.Linq;
using System.Text;

namespace E131_VixenPlugin
{
    //-----------------------------------------------------------------
    //
    //	E131Base - a collection of common functions and constants
    //
    //-----------------------------------------------------------------
    
    public class E131Base
    {
        virtual public byte[] PhyBuffer
        {
            get
            {
                return new byte[0];
            }

// ReSharper disable ValueParameterNotUsed
            set { }
// ReSharper restore ValueParameterNotUsed
        }

        override public string ToString()
        {
            var	txt = string.Empty;

            return PhyBuffer.Aggregate(txt, (current, val) => current + (val.ToString("X2") + ' '));
        }

        protected void UInt16ToBfrSwapped(UInt16 value, byte[] bfr, int offset)
        {
            bfr[offset]   = (byte) ((value & 0xff00) >> 8);
            bfr[offset+1] = (byte) (value & 0x00ff);
        }

        protected UInt16 BfrToUInt16Swapped(byte[] bfr, int offset)
        {
            return (UInt16) ((bfr[offset] << 8) | bfr[offset+1]);
        }

        protected void UInt32ToBfrSwapped(UInt32 value, byte[] bfr, int offset)
        {
            bfr[offset]   = (byte) ((value & 0xff000000) >> 24);
            bfr[offset+1] = (byte) ((value & 0x00ff0000) >> 16);
            bfr[offset+2] = (byte) ((value & 0x0000ff00) >>  8);
            bfr[offset+3] = (byte) ((value & 0x000000ff));
        }

        protected UInt32 BfrToUInt32Swapped(byte[] bfr, int offset)
        {
            return (((UInt32) bfr[offset]) << 24) | (((UInt32) bfr[offset+1]) << 16) | (((UInt32) bfr[offset+2]) << 8) | bfr[offset+3];
        }

        protected void StringToBfr(string value, byte[] bfr, int offset, int length)
        {
            var	val = new UTF8Encoding();

            var valBytes = val.GetBytes(value);

            if (valBytes.Length >= length) Array.Copy(valBytes, 0, bfr, offset, length);
            else
            {
                Array.Copy(valBytes, 0, bfr, offset, valBytes.Length);

                offset += valBytes.Length;
                length -= valBytes.Length;

                while (length-- > 0) bfr[offset++] = 0;
            }
        }

        protected string BfrToString(byte[] bfr, int offset, int length)
        {
            return new UTF8Encoding().GetString(bfr, offset, length);
        }

        protected void GUIDToBfr(Guid value, byte[] bfr, int offset)
        {
            var valBytes = value.ToByteArray();

            Array.Copy(valBytes, 0, bfr, offset, valBytes.Length);
        }

        protected Guid BfrToGuid(byte[] bfr, int offset)
        {
            var	valBytes = new byte[16];
            Array.Copy(bfr, offset, valBytes, 0, valBytes.Length);

            return new Guid(valBytes);
        }
    }

    //-----------------------------------------------------------------
    //
    //	E131Root - E1.31 Root Layer
    //
    //-----------------------------------------------------------------
    
    public class E131Root : E131Base
    {
        public UInt16	PreambleSize;		// RLP Preamble Size (0x0010)
        public UInt16	PostambleSize;		// RLP Postamble Size (0x0000)
        public string	AcnPacketID;		// Identifies Packet (ASC-E1.17)
        public UInt16	FlagsLength;		// PDU Flags/Length
        public UInt32	Vector;				// Vector (0x00000004)
        public Guid		SendCid;			// Sender's CID

        public bool		Malformed = true;	// malformed packet (length error)

        // note offsets are byte locations within the layer - not within the packet
    
        public const int	PreamblesizeOffset		=   0;
        public const int	PostamblesizeOffset	=   2;
        public const int	AcnpacketidOffset		=   4;
        public const int	AcnpacketidSize		=  12;
        public const int	FlagslengthOffset		=  16;
        public const int	VectorOffset			=  18;
        public const int	SendercidOffset		=  22;

        public const int	PhybufferSize		= 38;
        public const int	PduSize			= PhybufferSize - FlagslengthOffset;

        public E131Root()
        {
        }

        public E131Root(UInt16 length, Guid guid)
        {
            PreambleSize	= 0x0010;
            PostambleSize	= 0x0000;
            AcnPacketID		= "ASC-E1.17";
            FlagsLength		= (UInt16) (0x7000 | length);
            Vector			= 0x00000004;
            SendCid			= guid;
        }

        public E131Root(byte[] bfr, int offset)
        {
            FromBfr(bfr, offset);
        }

        public UInt16	Length
        {
            get
            {
                return (UInt16) (FlagsLength & 0x0fff);
            }

            set
            {
                FlagsLength = (UInt16) (0x7000 | value);
            }
        }
    
        override public byte[] PhyBuffer
        {
            get
            {
                var	bfr = new byte[PhybufferSize];

                ToBfr(bfr, 0);

                return bfr;
            }

            set
            {
                FromBfr(value, 0);
            }
        }

        public void FromBfr(byte[] bfr, int offset)
        {
            PreambleSize	= BfrToUInt16Swapped(bfr, offset + PreamblesizeOffset);
            PostambleSize	= BfrToUInt16Swapped(bfr, offset + PostamblesizeOffset);
            AcnPacketID		= BfrToString(bfr, offset + AcnpacketidOffset, AcnpacketidSize);
            FlagsLength		= BfrToUInt16Swapped(bfr, offset + FlagslengthOffset);
            Vector			= BfrToUInt32Swapped(bfr, offset + VectorOffset);
            SendCid			= BfrToGuid(bfr, offset + SendercidOffset);

            Malformed = true;

            if (Length != bfr.Length - FlagslengthOffset) return;

            Malformed = false;
        }

        public void ToBfr(byte[] bfr, int offset)
        {
            UInt16ToBfrSwapped(PreambleSize, bfr, offset + PreamblesizeOffset);
            UInt16ToBfrSwapped(PostambleSize, bfr, offset + PostamblesizeOffset);
            StringToBfr(AcnPacketID, bfr, offset + AcnpacketidOffset, AcnpacketidSize);
            UInt16ToBfrSwapped(FlagsLength, bfr, offset + FlagslengthOffset);
            UInt32ToBfrSwapped(Vector, bfr, offset + VectorOffset);
            GUIDToBfr(SendCid, bfr, offset + SendercidOffset);
        }
    }

    //-----------------------------------------------------------------
    //
    //	E131Framing - E1.31 Framing Layer
    //
    //-----------------------------------------------------------------
    
    public class E131Framing : E131Base
    {
        public UInt16	FlagsLength;		// PDU Flags/Length
        public UInt32	Vector;				// Vector (0x00000002)
        public string	SourceName;			// Source Name
        public byte		Priority;			// Data Priority
        public UInt16	Reserved;			// reserved (0)
        public byte		SequenceNumber;		// Packet Sequence Number
        public byte		Options;			// Options Flags
        public UInt16	Universe;			// Universe Number

        public bool		Malformed = true;	// malformed packet (length error)

        public const int	PhybufferSize		= 77;
        public const int	PduSize			= PhybufferSize;

        // note offsets are byte locations within the layer - not within the packet
    
        public const int	FlagslengthOffset		=   0;
        public const int	VectorOffset			=   2;
        public const int	SourcenameOffset		=   6;
        public const int	SourcenameSize			=  64;
        public const int	PriorityOffset			=  70;
        public const int	ReservedOffset		=  71;
        public const int	SequencenumberOffset	=  73;
        public const int	OptionsOffset			=  74;
        public const int	UniverseOffset			=  75;

        public E131Framing()
        {
        }

        public E131Framing(UInt16 length, string source, byte sequence, UInt16 univ)
        {
            FlagsLength		= (UInt16) (0x7000 | length);
            Vector			= 0x00000002;
            SourceName		= source;
            Priority		= 100;
            Reserved		= 0;
            SequenceNumber	= sequence;
            Options			= 0;
            Universe		= univ;
        }

        public E131Framing(byte[] bfr, int offset)
        {
            FromBfr(bfr, offset);
        }

        public UInt16	Length
        {
            get
            {
                return (UInt16) (FlagsLength & 0x0fff);
            }

            set
            {
                FlagsLength = (UInt16) (0x7000 | value);
            }
        }
    
        override public byte[] PhyBuffer
        {
            get
            {
                var	bfr = new byte[PhybufferSize];

                ToBfr(bfr, 0);

                return bfr;
            }

            set
            {
                FromBfr(value, 0);
            }
        }

        public void FromBfr(byte[] bfr, int offset)
        {
            FlagsLength		= BfrToUInt16Swapped(bfr, offset + FlagslengthOffset);
            Vector			= BfrToUInt32Swapped(bfr, offset + VectorOffset);
            SourceName		= BfrToString(bfr, offset + SourcenameOffset, SourcenameSize);
            Priority		= bfr[offset + PriorityOffset];
            Reserved		= BfrToUInt16Swapped(bfr, offset + ReservedOffset);
            SequenceNumber	= bfr[offset + SequencenumberOffset];
            Options			= bfr[offset + OptionsOffset];
            Universe		= BfrToUInt16Swapped(bfr, offset + UniverseOffset);

            Malformed = true;

            if (Length != bfr.Length - E131Root.PhybufferSize) return;

            Malformed = false;
        }

        public void ToBfr(byte[] bfr, int offset)
        {
            UInt16ToBfrSwapped(FlagsLength, bfr, offset + FlagslengthOffset);
            UInt32ToBfrSwapped(Vector, bfr, offset + VectorOffset);
            StringToBfr(SourceName, bfr, offset + SourcenameOffset, SourcenameSize);
            bfr[offset + PriorityOffset] = Priority;
            UInt16ToBfrSwapped(Reserved, bfr, offset + ReservedOffset);
            bfr[offset + SequencenumberOffset] = SequenceNumber;
            bfr[offset + OptionsOffset] = Options;
            UInt16ToBfrSwapped(Universe, bfr, offset + UniverseOffset);
        }
    }

    //-----------------------------------------------------------------
    //
    //	E131DMP - E1.31 DMP Layer
    //
    //-----------------------------------------------------------------
    
    public class E131Dmp : E131Base
    {
        public UInt16	FlagsLength;		// DMP PDU Flags/Length
        public byte		Vector;				// DMP Vector (0x02)
        public byte		AddrTypeDataType;	// Address Type / Data Type (0xa1)
        public UInt16	FirstPropertyAddr;	// DMX Start At DMP 0 (0x0000)
        public UInt16	AddrIncrement;		// Property Size (0x0001)
        public UInt16	PropertyValueCnt;	// Property Value Count
        public byte[]	PropertyValues;		// Property Values

        public bool		Malformed = true;	// malformed packet (length error)

        public const int	PhybufferBase		= 10;
        public const int	PduBase			= PhybufferBase;

        public const int	FlagslengthOffset			=   0;
        public const int	VectorOffset				=   2;
        public const int	AddrtypedatatypeOffset		=   3;
        public const int	FirstpropertyaddrOffset	=   4;
        public const int	AddrincrementOffset		=   6;
        public const int	PropertyvaluecntOffset		=   8;
        public const int	PropertyvaluesOffset		=  10;

        public E131Dmp()
        {
        }

        public E131Dmp(byte[] values, int offset, int slots)
        {
            FlagsLength			= (UInt16) (0x7000 | (PduBase + 1 + slots));
            Vector				= 0x02;
            AddrTypeDataType	= 0xa1;
            FirstPropertyAddr	= 0x0000;
            AddrIncrement		= 0x0001;
            PropertyValueCnt	= (UInt16) (slots + 1);
            PropertyValues		= new byte[slots + 1];
            PropertyValues[0]   = 0;
            Array.Copy(values, offset, PropertyValues, 1, slots);
        }

        public E131Dmp(byte[] bfr, int offset)
        {
            FromBfr(bfr, offset);
        }

        public UInt16 PhyLength
        {
            get
            {
                return (UInt16) (PhybufferBase + PropertyValueCnt);
            }
        }

        public UInt16	Length
        {
            get
            {
                return (UInt16) (FlagsLength & 0x0fff);
            }

            set
            {
                FlagsLength = (UInt16) (0x7000 | value);
            }
        }
    
        override public byte[] PhyBuffer
        {
            get
            {
                var	bfr = new byte[PhyLength];

                ToBfr(bfr, 0);

                return bfr;
            }

            set
            {
                FromBfr(value, 0);
            }
        }

        public void FromBfr(byte[] bfr, int offset)
        {
            FlagsLength			= BfrToUInt16Swapped(bfr, offset + FlagslengthOffset);
            Vector				= bfr[offset + VectorOffset];
            AddrTypeDataType	= bfr[offset + AddrtypedatatypeOffset];
            FirstPropertyAddr	= BfrToUInt16Swapped(bfr, offset + FirstpropertyaddrOffset);
            AddrIncrement		= BfrToUInt16Swapped(bfr, offset + AddrincrementOffset);
            PropertyValueCnt	= BfrToUInt16Swapped(bfr, offset + PropertyvaluecntOffset);
            PropertyValues		= new byte[PropertyValueCnt];

            Malformed = true;

            Array.Copy(bfr, offset + PropertyvaluesOffset, PropertyValues, 0, PropertyValueCnt);

            Malformed = false;
        }

        public void ToBfr(byte[] bfr, int offset)
        {
            UInt16ToBfrSwapped(FlagsLength, bfr, offset + FlagslengthOffset);
            bfr[offset + VectorOffset] = Vector;
            bfr[offset + AddrtypedatatypeOffset] = AddrTypeDataType;
            UInt16ToBfrSwapped(FirstPropertyAddr, bfr, offset + FirstpropertyaddrOffset);
            UInt16ToBfrSwapped(AddrIncrement, bfr, offset + AddrincrementOffset);
            UInt16ToBfrSwapped(PropertyValueCnt, bfr, offset + PropertyvaluecntOffset);
            Array.Copy(PropertyValues, 0, bfr, offset + PropertyvaluesOffset, PropertyValueCnt);
        }
    }

    //-----------------------------------------------------------------
    //
    //	E131Pkt - E1.31 The Packet
    //
    //-----------------------------------------------------------------
    
    public class E131Pkt : E131Base
    {
        public E131Root		E131Root;			// root layer
        public E131Framing	E131Framing;		// framing layer
        public E131Dmp		E131Dmp;			// dmp layer

        public bool			Malformed = true;	// malformed packet (length error)

        const int	RootOffset			=   0;
        const int	FramingOffset		= E131Root.PhybufferSize;
        const int	DmpOffset			= E131Root.PhybufferSize + E131Framing.PhybufferSize;

        public E131Pkt()
        {
        }

        public E131Pkt(Guid guid, string source, byte sequence, UInt16 universe, byte[] values, int offset, int slots)
        {
            E131Dmp		= new E131Dmp(values, offset, slots);
            E131Framing	= new E131Framing((UInt16) (E131Framing.PhybufferSize + E131Dmp.Length), source, sequence, universe);
            E131Root	= new E131Root((UInt16) (E131Root.PduSize + E131Framing.Length), guid);
        }

        public E131Pkt(byte[] bfr)
        {
            PhyBuffer = bfr;
        }

        public UInt16 PhyLength
        {
            get
            {
                return (UInt16) (E131Root.PhybufferSize + E131Framing.Length);
            }
        }

        override public sealed byte[] PhyBuffer
        {
            get
            {
                var	bfr = new byte[PhyLength];

                E131Root.ToBfr(bfr, RootOffset);
                E131Framing.ToBfr(bfr, FramingOffset);
                E131Dmp.ToBfr(bfr, DmpOffset);

                return bfr;
            }

            set
            {
                Malformed = true;		// assume malformed

                if (value.Length < E131Root.PhybufferSize + E131Framing.PhybufferSize + E131Dmp.PhybufferBase) return;

                E131Root = new E131Root(value, RootOffset);
                if (E131Root.Malformed) return;

                E131Framing = new E131Framing(value, FramingOffset);
                if (E131Root.Malformed) return;

                E131Dmp = new E131Dmp(value, DmpOffset);
                if (E131Dmp.Malformed) return;

                Malformed = false;
            }
        }

        //-------------------------------------------------------------
        //
        //	CompareSlots() - compare a new event buffer against current
        //					 slots
        //
        //		this is a static function to work on prebuilt packets.
        //		it is embedded in the E131Pkt class to keep it with
        //		the constants and rules that were used to build the
        //		original packet.
        //
        //-------------------------------------------------------------

        static public bool CompareSlots(byte[] phyBuffer, byte[] values, int offset, int slots)
        {
            var	idx = E131Root.PhybufferSize + E131Framing.PhybufferSize + E131Dmp.PropertyvaluesOffset + 1;

            while (slots-- > 0)
            {
                if (phyBuffer[idx++] != values[offset++]) return false;
            }

            return true;
        }

        //-------------------------------------------------------------
        //
        //	CopySlotsSeqNum() - copy a new sequence # and slots into
        //						an existing packet buffer
        //
        //		this is a static function to work on prebuilt packets.
        //		it is embedded in the E131Pkt class to keep it with
        //		the constants and rules that were used to build the
        //		original packet.
        //
        //-------------------------------------------------------------

        static public void CopySeqNumSlots(byte[] phyBuffer, byte[] values, int offset, int slots, byte seqNum)
        {
            const int idx = E131Root.PhybufferSize + E131Framing.PhybufferSize + E131Dmp.PropertyvaluesOffset + 1;

            Array.Copy(values, offset, phyBuffer, idx, slots);

            phyBuffer[E131Root.PhybufferSize + E131Framing.SequencenumberOffset] = seqNum;
        }
    }
}
