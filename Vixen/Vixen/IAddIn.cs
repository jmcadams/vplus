namespace Vixen
{
    using System;

    public interface IAddIn : ILoadable, IPlugIn
    {
        bool Execute(EventSequence sequence);
    }
}

