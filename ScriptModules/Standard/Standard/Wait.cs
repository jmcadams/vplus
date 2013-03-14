namespace Standard
{
    using System;

    public class Wait : Modifier
    {
        public override uint Type
        {
            get
            {
                return 0x300000;
            }
        }
    }
}

