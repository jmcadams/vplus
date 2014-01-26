public interface IVixenMDI
{
    EventSequence Sequence { get; set; }
    void Notify(Notification notification, object data);
}