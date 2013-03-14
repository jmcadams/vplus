namespace Vixen
{
    using System;

    public interface IPlugIn
    {
        string ToString();

        string Author { get; }

        string Description { get; }

        string Name { get; }
    }
}

