using System;
using System.Reflection;
namespace OopExamAutomation.Engine
{
    public static class TypeExtensions
    {
        public static bool ConstructorThrowsException(this Type type, params object[] args)
        {
            try
            {
                var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                Activator.CreateInstance(type, flags, null, args, null);
            }
            catch
            {
                return true;
            }

            return false;
        }

        public static bool MethodThrowsException(this Type type, object obj, string methodName, params object[] args)
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

            try
            {
                method.Invoke(obj, args);
            }
            catch(Exception exception)
            {
                return true;
            }

            return false;
        }

        public static object GetInstance(this Type type, params object[] args)
        {
            try
            {
                var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                return Activator.CreateInstance(type, flags, null, args, null);
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

        public static bool CheckMethodValue<T>(this Type type, object obj, string methodName, T expectedValue, params object[] args)
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
                result = method.Invoke(obj, args);
            }
            catch(Exception ex)
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
