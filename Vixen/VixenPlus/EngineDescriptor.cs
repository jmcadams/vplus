using System;

namespace VixenPlus
{
	internal class EngineDescriptor
	{
		private readonly string m_name;
		private readonly Type m_type;

		public EngineDescriptor(IEngine2 engineInstance)
		{
			m_name = engineInstance.Name;
			m_type = engineInstance.GetType();
			engineInstance.Dispose();
		}

		public string Name
		{
			get { return m_name; }
		}

		public Type Type
		{
			get { return m_type; }
		}
	}
}