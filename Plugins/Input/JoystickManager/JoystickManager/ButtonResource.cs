namespace JoystickManager
{
    using System;

    public class ButtonResource : ResourceObject
    {
        public int ButtonIndex;

        public ButtonResource(Joystick owningDevice, int buttonIndex) : base(owningDevice)
        {
            this.ButtonIndex = buttonIndex;
        }

        public override string Name
        {
            get
            {
                return ("Button " + this.ButtonIndex);
            }
        }

        public bool Pressed
        {
            get
            {
                return (base.OwningDevice.Device.get_CurrentJoystickState().GetButtons()[this.ButtonIndex] >= 0x80);
            }
        }

        public override int Value
        {
            get
            {
                return (this.Pressed ? 0xff : 0);
            }
        }
    }
}

