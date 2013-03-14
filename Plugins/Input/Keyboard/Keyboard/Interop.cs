namespace Keyboard
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public abstract class Interop
    {
        protected Interop()
        {
        }

        public abstract class Keyboard
        {
            protected Keyboard()
            {
            }

            [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
            private static extern short GetKeyState(int keyCode);
            private static KeyStates GetKeyState(Keys key)
            {
                KeyStates none = KeyStates.None;
                short keyState = GetKeyState((int) key);
                if ((keyState & 0x8000) == 0x8000)
                {
                    none |= KeyStates.Down;
                }
                if ((keyState & 1) == 1)
                {
                    none |= KeyStates.Toggled;
                }
                return none;
            }

            public static bool IsKeyDown(Keys key)
            {
                return (KeyStates.Down == (GetKeyState(key) & KeyStates.Down));
            }

            public static bool IsKeyToggled(Keys key)
            {
                return (KeyStates.Toggled == (GetKeyState(key) & KeyStates.Toggled));
            }

            [Flags]
            private enum KeyStates
            {
                None,
                Down,
                Toggled
            }
        }
    }
}

