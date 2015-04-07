namespace OopExamAutomation.Engine
{
    using System;
    using System.Reflection;

    public class PredicateTest : ITest
    {
        private readonly decimal maxPoints;

        private readonly Func<Assembly, bool> predicate;

        public PredicateTest(string name, decimal maxPoints, Func<Assembly, bool> predicate)
        {
            this.Name = name;
            this.maxPoints = maxPoints;
            this.predicate = predicate;
        }

        public TestResult CalculateResult(Assembly assembly)
        {
            if (this.predicate(assembly))
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