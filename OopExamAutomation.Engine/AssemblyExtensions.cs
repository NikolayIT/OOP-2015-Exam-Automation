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
    }
}
