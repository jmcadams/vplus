namespace Keyboard
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [StructLayout(LayoutKind.Sequential)]
    internal struct KeyInput
    {
        public Keys Key;
        public Keyboard.Inputs.InputType InputType;
        public bool IsIterator;
    }
}

