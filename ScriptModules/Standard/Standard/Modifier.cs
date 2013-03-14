namespace Standard
{
    using System;

    public abstract class Modifier : IModifier
    {
        public static uint MODIFIER_TYPE = 0xfff00000;
        public static int MODIFIER_VALUE = 0xfffff;
        protected int value;

        protected Modifier()
        {
        }

        public abstract uint Type { get; }

        public uint TypeValue
        {
            get
            {
                return (this.Type | ((uint) this.Value));
            }
        }

        public virtual int Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

