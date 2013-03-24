using System;

namespace Vixen
{
	internal interface IScheduledObject : IExecutable, IMaskable, IDisposable
	{
		int Length { get; }
	}
}