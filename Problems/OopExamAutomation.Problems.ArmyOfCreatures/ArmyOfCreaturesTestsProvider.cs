namespace OopExamAutomation.Problems.ArmyOfCreatures
{
    using System.Collections.Generic;
    using System.Linq;

    using OopExamAutomation.Engine;
    using System;

    public class ArmyOfCreaturesTestsProvider : ITestsProvider
    {
        public IEnumerable<ITest> GetTests()
        {
            var list = new List<ITest>();

            AddGoblinChecks(list);
            AddAncientBehemothChecks(list);

            return list;
        }

        private void AddBaseCreatureChecks(IList<ITest> list, string creatureName, int attack, int defense, int healthPoints, decimal damage, int specialtiesCount)
        {
            list.Add(new PredicateTest("Added class " + creatureName, 0.25M, assembly => assembly.GetTypes().Any(t => t.Name == creatureName)));
            list.Add(new PredicateTest(creatureName + " is a creature", 0.5M, assembly => assembly.GetTypes().Any(t => t.Name == creatureName && t.BaseType.Name == "Creature")));
            list.Add(new TypeTest(creatureName + " has " + attack + " attack", 0.25M, creatureName, type => type.CheckPropertyValue("Attack", attack)));
            list.Add(new TypeTest(creatureName + " has " + defense + " defense", 0.25M, creatureName, type => type.CheckPropertyValue("Defense", defense)));
            list.Add(new TypeTest(creatureName + " has " + healthPoints + " health points", 0.25M, creatureName, type => type.CheckPropertyValue("HealthPoints", healthPoints)));
            list.Add(new TypeTest(creatureName + " has " + damage + " damage", 0.25M, creatureName, type => type.CheckPropertyValue("Damage", damage)));
            list.Add(new TypeTest(creatureName + " has " + specialtiesCount + " specialties", 0.25M, creatureName, type => type.CheckPropertyValue<IEnumerable<object>>("Specialties", v => v.Count() == specialtiesCount)));
        }

        private void AddGoblinChecks(IList<ITest> list)
        {
            AddBaseCreatureChecks(list, "Goblin", 4, 2, 5, 1.5M, 0);
        }

        private void AddAncientBehemothChecks(IList<ITest> list)
        {
            AddBaseCreatureChecks(list, "AncientBehemoth", 19, 19, 300, 40, 2);
        }
    }
}
