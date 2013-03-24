using System.ComponentModel;

namespace VixenPlus
{
	internal class GeneralConcreteClassProvider : TypeDescriptionProvider
	{
		//private Type _abstractType;
		//private Type _concreteType;

		//public GeneralConcreteClassProvider() : base(TypeDescriptor.GetProvider(typeof(System.Windows.Forms.Form)))
		//{
		//}

		//public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
		//{
		//    this.EnsureTypes(objectType);
		//    if (objectType == this._abstractType)
		//    {
		//        objectType = this._concreteType;
		//    }
		//    return base.CreateInstance(provider, objectType, argTypes, args);
		//}

		//private void EnsureTypes(Type objectType)
		//{
		//    if (this._abstractType == null)
		//    {
		//        for (Type type = objectType; ((this._abstractType == null) && (type != null)) && !(type == typeof(object)); type = type.BaseType)
		//        {
		//            object[] customAttributes = type.GetCustomAttributes(typeof(ConcreteClassAttribute), false);
		//            int index = 0;
		//            while (index < customAttributes.Length)
		//            {
		//                ConcreteClassAttribute attribute = (ConcreteClassAttribute) customAttributes[index];
		//                this._abstractType = type;
		//                this._concreteType = attribute.ConcreteType;
		//                break;
		//            }
		//        }
		//        if (this._abstractType == null)
		//        {
		//            throw new InvalidOperationException(string.Format("No ConcreteClassAttribute was found on {0} or any of its subtypes.", objectType));
		//        }
		//    }
		//}

		//public override Type GetReflectionType(Type objectType, object instance)
		//{
		//    this.EnsureTypes(objectType);
		//    if (objectType == this._abstractType)
		//    {
		//        return this._concreteType;
		//    }
		//    return base.GetReflectionType(objectType, instance);
		//}
	}
}