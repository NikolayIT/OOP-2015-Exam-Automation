using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OopExamAutomation.Engine
{
    public static class AssemblyExtensions
    {
        public static Type GetTypeByName(this Assembly assembly, string typeName)
        {
            return assembly.GetTypes().FirstOrDefault(x => x.Name == typeName);
        }

        public static Type GetInterface(this Assembly assembly, string interfaceName)
        {
            return assembly.GetTypes().FirstOrDefault(t => t.Name.Contains(interfaceName) && t.IsInterface);
        }

        public static Type GetAbstractClass(this Assembly assembly, string abstractClassName)
        {
            return assembly.GetTypes().FirstOrDefault(t => t.Name.Contains(abstractClassName) && t.IsAbstract);
        }

        public static bool HasClassImplementingInterface(this Assembly assembly, string interfaceName)
        {
            return assembly.GetTypes().Any(t => t.IsClass && assembly.GetInterface(interfaceName).IsAssignableFrom(t));
        }

        public static Type GetClassImplementingInterface(this Assembly assembly, string interfaceName)
        {
            return assembly.GetTypes().FirstOrDefault(t => t.IsClass && assembly.GetInterface(interfaceName).IsAssignableFrom(t));
        }

        public static bool HasClassImplementingInterfaceWithPrivateProperty(this Assembly assembly, string interfaceName, string propertyName)
        {
            var property = assembly.GetClassImplementingInterface(interfaceName).GetProperties().FirstOrDefault(pr => pr.Name == propertyName);
            if (!property.CanWrite)
            {
                return true;
            }

            if (!property.GetSetMethod(true).IsPublic)
            {
                return true;
            }

            return false;
        }
    }
}
