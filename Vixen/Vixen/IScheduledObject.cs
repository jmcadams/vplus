namespace Vixen
{
    using System;

    internal interface IScheduledObject : IExecutable, IMaskable, IDisposable
    {
        int Length { get; }
    }
}

