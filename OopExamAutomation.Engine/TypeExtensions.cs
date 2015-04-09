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

        public static bool MethodThrowsArgumentException(this Type type, object obj, string methodName, params object[] args)
        {
            if (obj == null)
            {
                return false;
            }

            MethodInfo method = null;
            try
            {
                method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }
            catch (AmbiguousMatchException)
            {
                return false;
            }

            if (method == null)
            {
                return false;
            }

            try
            {
                method.Invoke(obj, args);
            }
            catch (TargetInvocationException ex)
            {
                return ex.InnerException is ArgumentException;
            }
            catch
            {
                return false;
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

        public static void InvokeMethod(this Type type, object obj, string methodName, params object[] args)
        {
            MethodInfo method = null;
            try
            {
                method = type.GetMethod(methodName);
            }
            catch (AmbiguousMatchException)
            {
                return;
            }

            if (method == null)
            {
                return;
            }

            try
            {
                method.Invoke(obj, args);
            }
            catch (Exception)
            {
                return;
            }
        }

        public static object GetMethodValue(this Type type, object obj, string methodName, params object[] args)
        {
            MethodInfo method = null;
            try
            {
                method = type.GetMethod(methodName);
            }
            catch (AmbiguousMatchException)
            {
                return null;
            }

            if (method == null)
            {
                return null;
            }

            object val = null;
            try
            {
                val = method.Invoke(obj, args);
            }
            catch (Exception)
            {
                return null;
            }

            return val;
        }

        public static bool CheckMethodValue<T>(this Type type, object obj, string methodName, T expectedValue, params object[] args)
        {
            if (obj == null)
            {
                return false;
            }

            MethodInfo method = null;
            try
            {
                method = type.GetMethod(methodName);
            }
            catch (AmbiguousMatchException)
            {
                return false;
            }

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
