namespace VixenControls
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Drawing;
    using System.Globalization;
    using System.Reflection;

    internal class ToolboxItemTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                ConstructorInfo constructor = value.GetType().GetConstructor(new Type[] { typeof(string), typeof(string), typeof(System.Drawing.Image) });
                ToolboxItem item = (ToolboxItem) value;
                return new InstanceDescriptor(constructor, new object[] { item.Name, item.Description, item.Image });
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

