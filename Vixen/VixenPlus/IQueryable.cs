namespace Vixen
{
    using System;

    internal interface IQueryable
    {
        string QueryInstance(int index);

        int Count { get; }
    }
}

