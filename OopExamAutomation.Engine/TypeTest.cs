namespace OopExamAutomation.Engine
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class TypeTest : ITest
    {
        private readonly decimal maxPoints;

        private string typeName;

        private readonly Func<Type, bool> predicate;

        public TypeTest(string name, decimal maxPoints, string typeName, Func<Type, bool> predicate)
        {
            this.Name = name;
            this.maxPoints = maxPoints;
            this.typeName = typeName;
            this.predicate = predicate;
        }

        public TestResult CalculateResult(Assembly assembly)
        {
            var type = assembly.GetTypes().FirstOrDefault(x => x.Name == this.typeName);
            if (type == null)
            {
                return new TestResult(0, "Type not found!");
            }

            if (this.predicate(type))
            {
                return new TestResult(maxPoints, "OK");
            }
            else
            {
                return new TestResult(0, "FAIL");
            }
        }

        public string Name { get; private set; }
    }
}
