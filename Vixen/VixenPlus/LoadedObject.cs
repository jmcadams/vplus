using System;

namespace VixenPlus
{
	internal class LoadedObject
	{
		public ILoadable Instance;
		public string InterfaceImplemented;
		public Type ObjectType;

		public LoadedObject(Type type, string interfaceImplemented)
		{
			ObjectType = type;
			InterfaceImplemented = interfaceImplemented;
			Instance = null;
		}
	}
}