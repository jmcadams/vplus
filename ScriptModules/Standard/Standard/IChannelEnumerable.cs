namespace Standard
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IChannelEnumerable : IEnumerable<VixenChannel>, IEnumerable
    {
        int Count { get; }

        VixenChannel this[int index] { get; set; }
    }
}

