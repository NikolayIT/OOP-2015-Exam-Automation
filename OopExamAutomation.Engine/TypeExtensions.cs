using System;
namespace OopExamAutomation.Engine
{
    public static class TypeExtensions
    {
        public static object GetInstance(this Type type, params object[] args)
        {
            try
            {
                return Activator.CreateInstance(type, args);
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

        public static bool CheckMethodValue<T>(this Type type, object obj, string methodName, T expectedValue)
        {
            if (obj == null)
            {
                return false;
            }

            var method = type.GetMethod(methodName);
            if (methodName == null)
            {
                return false;
            }

            object result;
            try
            {
                result = method.Invoke(obj, new object[0]);
            }
            catch
            {
                return false;
            }

            if (result == null)
            {
                return false;
            }

            return result.Equals(expectedValue);
        }

        public static bool CheckPropertyValue<T>(this Type type, string propertyName, T expectedValue)
        {
            var obj = type.GetInstance();
            if (obj == null) return false;
            return obj.GetPropertyValue<T>(propertyName).Equals(expectedValue);
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
