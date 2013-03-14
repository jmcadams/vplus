namespace LedTriks
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyState
    {
        private const int ShiftMask = 4;
        private const int CtrlMask = 8;
        private const int AltMask = 0x20;
        private const int LButtonMask = 1;
        private const int MButtonMask = 0x10;
        private const int RButtonMask = 2;
        private int m_KeyState;
        public KeyState(int keyState)
        {
            this.m_KeyState = keyState;
        }

        public KeyState(KeyState keyState)
        {
            this.m_KeyState = keyState.m_KeyState;
        }

        public int State
        {
            get
            {
                return this.m_KeyState;
            }
            set
            {
                this.m_KeyState = value;
            }
        }
        private void SetMaskedVal(bool Value, int Mask)
        {
            if (Value)
            {
                this.m_KeyState |= Mask;
            }
            else
            {
                this.m_KeyState &= ~Mask;
            }
        }

        public bool Shift
        {
            get
            {
                return IsShift(this.m_KeyState);
            }
            set
            {
                this.SetMaskedVal(value, 4);
            }
        }
        public bool Ctrl
        {
            get
            {
                return IsCtrl(this.m_KeyState);
            }
            set
            {
                this.SetMaskedVal(value, 8);
            }
        }
        public bool Alt
        {
            get
            {
                return IsAlt(this.m_KeyState);
            }
            set
            {
                this.SetMaskedVal(value, 0x20);
            }
        }
        public bool LButton
        {
            get
            {
                return IsLButton(this.m_KeyState);
            }
            set
            {
                this.SetMaskedVal(value, 1);
            }
        }
        public bool RButton
        {
            get
            {
                return IsRButton(this.m_KeyState);
            }
            set
            {
                this.SetMaskedVal(value, 2);
            }
        }
        public bool MButton
        {
            get
            {
                return IsMButton(this.m_KeyState);
            }
            set
            {
                this.SetMaskedVal(value, 0x10);
            }
        }
        private static bool IsShift(int keyState)
        {
            return Convert.ToBoolean((int) (keyState & 4));
        }

        private static bool IsCtrl(int keyState)
        {
            return Convert.ToBoolean((int) (keyState & 8));
        }

        private static bool IsAlt(int keyState)
        {
            return Convert.ToBoolean((int) (keyState & 0x20));
        }

        private static bool IsLButton(int keyState)
        {
            return Convert.ToBoolean((int) (keyState & 1));
        }

        private static bool IsRButton(int keyState)
        {
            return Convert.ToBoolean((int) (keyState & 2));
        }

        private static bool IsMButton(int keyState)
        {
            return Convert.ToBoolean((int) (keyState & 0x10));
        }

        public static bool IsShiftPressed(DragEventArgs e)
        {
            return IsShift(e.KeyState);
        }

        public static bool IsShiftPressed(QueryContinueDragEventArgs e)
        {
            return IsShift(e.KeyState);
        }

        public static bool IsCtrlPressed(DragEventArgs e)
        {
            return IsCtrl(e.KeyState);
        }

        public static bool IsCtrlPressed(QueryContinueDragEventArgs e)
        {
            return IsCtrl(e.KeyState);
        }

        public static bool IsAltPressed(DragEventArgs e)
        {
            return IsAlt(e.KeyState);
        }

        public static bool IsAltPressed(QueryContinueDragEventArgs e)
        {
            return IsAlt(e.KeyState);
        }

        public static bool IsLButtonPressed(DragEventArgs e)
        {
            return IsLButton(e.KeyState);
        }

        public static bool IsLButtonPressed(QueryContinueDragEventArgs e)
        {
            return IsLButton(e.KeyState);
        }

        public static bool IsRButtonPressed(DragEventArgs e)
        {
            return IsRButton(e.KeyState);
        }

        public static bool IsRButtonPressed(QueryContinueDragEventArgs e)
        {
            return IsRButton(e.KeyState);
        }

        public static bool IsMButtonPressed(DragEventArgs e)
        {
            return IsMButton(e.KeyState);
        }

        public static bool IsMButtonPressed(QueryContinueDragEventArgs e)
        {
            return false;
        }
    }
}

