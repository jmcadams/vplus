namespace Vixen
{
    using System;

    internal class LoadedObject
    {
        public ILoadable Instance;
        public string InterfaceImplemented;
        public Type ObjectType;

        public LoadedObject(Type type, string interfaceImplemented)
        {
            this.ObjectType = type;
            this.InterfaceImplemented = interfaceImplemented;
            this.Instance = null;
        }
    }
}

