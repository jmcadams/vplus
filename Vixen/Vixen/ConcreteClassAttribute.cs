namespace Vixen
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    internal class ConcreteClassAttribute : Attribute
    {
        private Type _concreteType;

        public ConcreteClassAttribute(Type concreteType)
        {
            this._concreteType = concreteType;
        }

        public Type ConcreteType
        {
            get
            {
                return this._concreteType;
            }
        }
    }
}

