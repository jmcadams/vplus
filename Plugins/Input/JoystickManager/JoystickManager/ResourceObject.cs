namespace JoystickManager
{
    using System;

    public abstract class ResourceObject
    {
        private Joystick m_owningDevice;

        public ResourceObject(Joystick owningDevice)
        {
            this.m_owningDevice = owningDevice;
        }

        public abstract string Name { get; }

        protected Joystick OwningDevice
        {
            get
            {
                return this.m_owningDevice;
            }
        }

        public string QualifiedName
        {
            get
            {
                return string.Format("{0} - {1}", this.m_owningDevice.Name, this.Name);
            }
        }

        public abstract int Value { get; }
    }
}

