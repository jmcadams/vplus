namespace Standard
{
    using System;

    public interface IModifier
    {
        uint Type { get; }

        uint TypeValue { get; }

        int Value { get; }
    }
}

