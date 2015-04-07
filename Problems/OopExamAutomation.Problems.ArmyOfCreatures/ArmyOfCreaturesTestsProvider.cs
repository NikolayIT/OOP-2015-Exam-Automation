namespace OopExamAutomation.Problems.ArmyOfCreatures
{
    using System.Collections.Generic;
    using System.Linq;

    using OopExamAutomation.Engine;

    public class ArmyOfCreaturesTestsProvider : ITestsProvider
    {
        public IEnumerable<ITest> GetTests()
        {
            return new List<ITest>()
            {
                new PredicateTest("Added class Goblin", 0.5M, assembly => assembly.GetTypes().Any(t => t.Name == "Goblin" && t.BaseType.Name == "Creature")),
            };
        }
    }
}
