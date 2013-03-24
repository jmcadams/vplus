using System;

namespace VixenPlus
{
	internal interface IScheduledObject : IExecutable, IMaskable, IDisposable
	{
		int Length { get; }
	}
}