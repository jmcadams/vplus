namespace VixenPlus
{
	public interface VixenMDI
	{
		EventSequence Sequence { get; set; }
		void Notify(Notification notification, object data);
	}
}