using System;

namespace VixenPlus
{
	[AttributeUsage(AttributeTargets.Class)]
	internal class ConcreteClassAttribute : Attribute
	{
		private readonly Type _concreteType;

		public ConcreteClassAttribute(Type concreteType)
		{
			_concreteType = concreteType;
		}

		public Type ConcreteType
		{
			get { return _concreteType; }
		}
	}
}