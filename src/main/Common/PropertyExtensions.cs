using System.ComponentModel;
using System.Reflection;

namespace ei8.Avatar.Installer.Common
{
    public static class PropertyExtensions
    {
        /// <summary>
        /// Wraps <see cref="PropertyInfo.SetValue"/> to automatically invoke the appropriate <see cref="TypeConverter"/> for the passed value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetValueFromString(this PropertyInfo property, object obj, string value)
        {
            if (property.PropertyType == typeof(string))
                property.SetValue(obj, value);

            var converter = TypeDescriptor.GetConverter(property.PropertyType);

            if (converter != null)
            {
                var convertedValue = converter.ConvertFrom(value);
                property.SetValue(obj, convertedValue);
            }
        }
    }
}
