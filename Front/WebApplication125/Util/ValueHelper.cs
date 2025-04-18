using System.Reflection;

namespace WEB.Util
{
    public class ValueHelper
    {
        public static void PreserveOriginalValues<T>(T updatedEntity, T originalEntity) where T : class
        {
            if (updatedEntity == null || originalEntity == null)
            {
                return;
            }

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // Check if the property has its default value in the updated entity
                object updatedValue = property.GetValue(updatedEntity);
                object defaultValue = GetDefaultValue(property.PropertyType);
                object originalValue = property.GetValue(originalEntity);

                // If the updated value is the default, it means it wasn't explicitly set in the form
                if (Equals(updatedValue, defaultValue) && !Equals(originalValue, defaultValue))
                {
                    property.SetValue(updatedEntity, originalValue);
                }
            }
        }

        private static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
