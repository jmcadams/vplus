namespace Standard
{
    using System;

    public class For : TimeModifier
    {
        public For(int value) : base(value)
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

