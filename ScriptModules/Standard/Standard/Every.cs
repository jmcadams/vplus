namespace Standard
{
    using System;

    public class Every : TimeModifier
    {
        public Every(int value) : base(value)
        {
        }

        public override uint Type
        {
            get
            {
                return 0x400000;
            }
        }
    }
}

