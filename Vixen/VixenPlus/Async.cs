using System;

namespace VixenPlus
{
	internal class Async : IObjectQuery
	{
		public bool UsesInputPlugin(InputPlugin plugin)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool UsesOutputPlugin(IOutputPlugIn plugin)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool UsesProgram(SequenceProgram program)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool UsesSequence(EventSequence sequence)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void HardwareUpdate(byte[] values)
		{
		}

		public void Initialize(EventSequence sequence)
		{
		}

		public void Initialize(Profile profile)
		{
		}

		public void Initialize(SequenceProgram program)
		{
		}
	}
}