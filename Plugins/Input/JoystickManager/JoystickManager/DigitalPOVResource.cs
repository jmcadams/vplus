namespace JoystickManager
{
    using System;

    public class DigitalPOVResource : POVResource
    {
        private Direction m_direction;

        public DigitalPOVResource(Joystick owningDevice, int index, Direction direction) : base(owningDevice, index)
        {
            this.m_direction = direction;
        }

        public override string Name
        {
            get
            {
                return string.Format("{0} [{1}]", base.Name, this.m_direction);
            }
        }

        public override int Value
        {
            get
            {
                return ((base.InternalValue == this.m_direction) ? 0xff : 0);
            }
        }

        public enum Direction
        {
            E = 0x2328,
            N = 0,
            S = 0x4650,
            W = 0x6978
        }
    }
}

