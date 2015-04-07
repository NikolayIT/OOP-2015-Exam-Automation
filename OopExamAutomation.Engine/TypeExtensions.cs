using System;
namespace OopExamAutomation.Engine
{
    public static class TypeExtensions
    {
        public static object GetInstance(this Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }

        public static T GetPropertyValue<T>(this object source, string property)
        {
            try
            {
                var value = source.GetType().GetProperty(property).GetValue(source, null);
                return (T)value;
            }
            catch
            {
                return default(T);
            }
        }

        public static bool CheckPropertyValue<T>(this Type type, string propertyName, T value)
        {
            var obj = type.GetInstance();
            if (obj == null) return false;
            return obj.GetPropertyValue<T>(propertyName).Equals(value);
        }

        public static bool CheckPropertyValue<T>(this Type type, string propertyName, Func<T, bool> checkValue)
        {
            var obj = type.GetInstance();
            if (obj == null) return false;
            var propertyValue = obj.GetPropertyValue<T>(propertyName);
            if (propertyValue == null) return false;
            return checkValue(propertyValue);
        }
    }
}
