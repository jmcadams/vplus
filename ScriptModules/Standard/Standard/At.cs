namespace Standard
{
    using System;

    public class At : Modifier
    {
        public At(int level)
        {
            level = Math.Min(level, 100);
            level = Math.Max(level, 0);
            base.value = level;
        }

        public override uint Type
        {
            get
            {
                return 0x100000;
            }
        }
    }
}

