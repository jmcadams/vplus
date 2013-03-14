namespace Vixen
{
    using System;

    public interface VixenMDI
    {
        void Notify(Notification notification, object data);

        EventSequence Sequence { get; set; }
    }
}

