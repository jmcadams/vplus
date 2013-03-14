namespace Standard
{
    using System;

    public class Over : TimeModifier
    {
        public Over(int value) : base(value)
        {
        }

        public override uint Type
        {
            get
            {
                return 0x200000;
            }
        }
    }
}

