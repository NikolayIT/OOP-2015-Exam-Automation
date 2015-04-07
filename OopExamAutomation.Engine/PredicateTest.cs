namespace OopExamAutomation.Engine
{
    using System;
    using System.Reflection;

    public class PredicateTest : ITest
    {
        private readonly decimal maxPoints;

        private readonly Func<Assembly, bool> predicate;

        public PredicateTest(decimal maxPoints, Func<Assembly, bool> predicate)
        {
            this.maxPoints = maxPoints;
            this.predicate = predicate;
        }

        public decimal CalculatePoints(Assembly assembly)
        {
            if (this.predicate(assembly))
            {
                return this.maxPoints;
            }
            else
            {
                return 0;
            }
        }
    }
}