using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace CommonControls {
    internal class ToolboxCategoryTypeConverter : TypeConverter {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return ((destinationType == typeof (InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }


        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (destinationType == typeof (InstanceDescriptor)) {
                var constructor = value.GetType().GetConstructor(new[] {typeof (string), typeof (string)});
                var category = (ToolboxCategory) value;
                return new InstanceDescriptor(constructor, new object[] {category.Name, category.Description}, false);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
