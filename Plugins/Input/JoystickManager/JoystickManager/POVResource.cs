namespace JoystickManager
{
    using System;

    public class POVResource : ResourceObject
    {
        public int Index;

        public POVResource(Joystick owningDevice, int index) : base(owningDevice)
        {
            this.Index = index;
        }

        protected int InternalValue
        {
            get
            {
                return base.OwningDevice.Device.CurrentJoystickState.GetPointOfView()[this.Index];
            }
        }

        public override string Name
        {
            get
            {
                return ("POV " + this.Index);
            }
        }

        public override int Value
        {
            get
            {
                return (int) ((((float) this.InternalValue) / 360f) * 255f);
            }
        }
    }
}

