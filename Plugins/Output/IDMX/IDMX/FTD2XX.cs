namespace IDMX
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class FTD2XX
    {
        public const uint FT_BAUD_115200 = 0x1c200;
        public const uint FT_BAUD_1200 = 0x4b0;
        public const uint FT_BAUD_14400 = 0x3840;
        public const uint FT_BAUD_19200 = 0x4b00;
        public const uint FT_BAUD_230400 = 0x38400;
        public const uint FT_BAUD_2400 = 0x960;
        public const uint FT_BAUD_300 = 300;
        public const uint FT_BAUD_38400 = 0x9600;
        public const uint FT_BAUD_460800 = 0x70800;
        public const uint FT_BAUD_4800 = 0x12c0;
        public const uint FT_BAUD_57600 = 0xe100;
        public const uint FT_BAUD_600 = 600;
        public const uint FT_BAUD_921600 = 0xe1000;
        public const uint FT_BAUD_9600 = 0x2580;
        public const byte FT_BITS_5 = 5;
        public const byte FT_BITS_6 = 6;
        public const byte FT_BITS_7 = 7;
        public const byte FT_BITS_8 = 8;
        public const uint FT_EVENT_MODEM_STATUS = 2;
        public const uint FT_EVENT_RXCHAR = 1;
        public const ushort FT_FLOW_DTR_DSR = 0x200;
        public const ushort FT_FLOW_NONE = 0;
        public const ushort FT_FLOW_RTS_CTS = 0x100;
        public const ushort FT_FLOW_XON_XOFF = 0x400;
        public const uint FT_LIST_ALL = 0x20000000;
        public const uint FT_LIST_BY_INDEX = 0x40000000;
        public const uint FT_LIST_NUMBER_ONLY = 0x80000000;
        public const uint FT_OPEN_BY_DESCRIPTION = 2;
        public const uint FT_OPEN_BY_SERIAL_NUMBER = 1;
        public const byte FT_PARITY_EVEN = 2;
        public const byte FT_PARITY_MARK = 3;
        public const byte FT_PARITY_NONE = 0;
        public const byte FT_PARITY_ODD = 1;
        public const byte FT_PARITY_SPACE = 4;
        public const byte FT_PURGE_RX = 1;
        public const byte FT_PURGE_TX = 2;
        public const byte FT_STOP_BITS_1 = 0;
        public const byte FT_STOP_BITS_1_5 = 1;
        public const byte FT_STOP_BITS_2 = 2;

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_Close(IntPtr ftHandle);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_ListDevices(uint deviceIndex, [MarshalAs(UnmanagedType.LPStr)] StringBuilder deviceName, uint Flags);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_ListDevices(ref uint numDevices, ref uint notUsed, uint Flags);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_Open(int deviceIndex, ref IntPtr ftHandle);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_Read(IntPtr ftHandle, byte[] buffer, uint bytesToRead, ref uint bytesReturned);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_ResetDevice(IntPtr ftHandle);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetBaudRate(IntPtr ftHandle, uint baudRate);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetBreakOff(IntPtr ftHandle);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetBreakOn(IntPtr ftHandle);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetDataCharacteristics(IntPtr ftHandle, byte wordLength, byte stopBits, byte parity);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetFlowControl(IntPtr ftHandle, ushort flowControl, byte xOn, byte xOff);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetTimeouts(IntPtr ftHandle, uint readTimeout, uint writeTimeout);
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_Write(IntPtr ftHandle, byte[] buffer, uint bytesToWrite, ref uint bytesWritten);

        public enum FT_STATUS : uint
        {
            FT_DEVICE_NOT_FOUND = 2,
            FT_DEVICE_NOT_OPENED = 3,
            FT_DEVICE_NOT_OPENED_FOR_ERASE = 8,
            FT_DEVICE_NOT_OPENED_FOR_WRITE = 9,
            FT_EEPROM_ERASE_FAILED = 13,
            FT_EEPROM_NOT_PRESENT = 14,
            FT_EEPROM_NOT_PROGRAMMED = 15,
            FT_EEPROM_READ_FAILED = 11,
            FT_EEPROM_WRITE_FAILED = 12,
            FT_FAILED_TO_WRITE_DEVICE = 10,
            FT_INSUFFICIENT_RESOURCES = 5,
            FT_INVALID_ARGS = 0x10,
            FT_INVALID_BAUD_RATE = 7,
            FT_INVALID_HANDLE = 1,
            FT_INVALID_PARAMETER = 6,
            FT_IO_ERROR = 4,
            FT_OK = 0,
            FT_OTHER_ERROR = 0x11
        }
    }
}

