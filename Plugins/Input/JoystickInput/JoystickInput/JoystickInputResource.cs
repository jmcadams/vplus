namespace JoystickInput
{
	using JoystickManager;
	using System;
	using VixenPlus;

	internal class JoystickInputResource : Input
	{
		private ResourceObject m_resource;

		public JoystickInputResource(InputPlugin pluginOwner, ResourceObject resource, bool isIterator) : base(pluginOwner, resource.Name, isIterator)
		{
			this.m_resource = resource;
		}

		public JoystickInputResource(InputPlugin pluginOwner, ResourceObject resource, string inputName, bool isIterator) : base(pluginOwner, inputName, isIterator)
		{
			this.m_resource = resource;
		}

		public override byte GetValue()
		{
			return (byte) this.m_resource.Value;
		}

		public bool IsAxis
		{
			get
			{
				return (this.m_resource is AxisResource);
			}
		}

		public bool IsButton
		{
			get
			{
				return (this.m_resource is ButtonResource);
			}
		}

		public bool IsDigitalPOV
		{
			get
			{
				return (this.m_resource is DigitalPOVResource);
			}
		}

		public bool IsPOV
		{
			get
			{
				return (this.m_resource is POVResource);
			}
		}

		public override bool Changed {
			get { return true; }
		}
	}
}

