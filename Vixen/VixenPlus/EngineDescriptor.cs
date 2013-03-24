using System;

namespace VixenPlus
{
	internal class EngineDescriptor
	{
		private readonly string _name;
		private readonly Type _type;

		public EngineDescriptor(IEngine2 engineInstance)
		{
			_name = engineInstance.Name;
			_type = engineInstance.GetType();
			engineInstance.Dispose();
		}

		public string Name
		{
			get { return _name; }
		}

		public Type Type
		{
			get { return _type; }
		}
	}
}