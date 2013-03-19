namespace JoystickManager
{
    using System;

    public class AxisResource : ResourceObject
    {
        private Joystick.Axis m_axis;

        public AxisResource(Joystick owningDevice, Joystick.Axis axis) : base(owningDevice)
        {
            this.m_axis = axis;
        }

        public override string Name
        {
            get
            {
                return ("Axis " + this.m_axis);
            }
        }

        public override int Value
        {
            get
            {
                switch (this.m_axis)
                {
                    case Joystick.Axis.X:
                        return base.OwningDevice.Device.CurrentJoystickState.X;

                    case Joystick.Axis.Y:
                        return base.OwningDevice.Device.CurrentJoystickState.Y;

                    case Joystick.Axis.Z:
                        return base.OwningDevice.Device.CurrentJoystickState.Z;

                    case Joystick.Axis.rX:
                        return base.OwningDevice.Device.CurrentJoystickState.Rx;

                    case Joystick.Axis.rY:
                        return base.OwningDevice.Device.CurrentJoystickState.Ry;

                    case Joystick.Axis.rZ:
                        return base.OwningDevice.Device.CurrentJoystickState.Rz;
                }
                return -1;
            }
        }
    }
}

