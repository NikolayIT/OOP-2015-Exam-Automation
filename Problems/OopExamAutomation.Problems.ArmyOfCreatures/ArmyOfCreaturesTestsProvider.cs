namespace OopExamAutomation.Problems.ArmyOfCreatures
{
    using System.Collections.Generic;
    using System.Linq;

    using OopExamAutomation.Engine;
    using System;
    using System.Reflection;

    public class ArmyOfCreaturesTestsProvider : ITestsProvider
    {
        public IEnumerable<ITest> GetTests()
        {
            var list = new List<ITest>();

            AddGoblinChecks(list);
            AddAncientBehemothChecks(list);
            AddWolfRaiderChecks(list);
            AddGriffinChecks(list);
            AddCyclopsKingChecks(list);

            AddDoubleDamageChecks(list);
            AddAttackWhenSkipChecks(list);
            AddDoubleAttackWhenAttackingChecks(list);

            AddWorkingWithThreeArmiesChecks(list);
            AddFactoryChecks(list);

            return list;
        }

        private void AddWorkingWithThreeArmiesChecks(IList<ITest> list)
        {
            // TODO: Check working with 3 armies
        }

        private void AddFactoryChecks(IList<ITest> list)
        {
            // TODO: Check factory
        }

        private void AddDoubleDamageChecks(IList<ITest> list)
        {
            // Common requirements
            list.Add(new PredicateTest("Added class DoubleDamage", 0.25M, assembly => assembly.GetTypes().Any(t => t.Name == "DoubleDamage")));
            list.Add(new TypeTest("Class DoubleDamage has only 1 constructor", 0.25M, "DoubleDamage", type => type.GetConstructors().Count() == 1));
            list.Add(new TypeTest("DoubleDamage constructor doesn't throw exception when rounds is 10", 0.25M, "DoubleDamage", type => !type.ConstructorThrowsException(10)));
            list.Add(new TypeTest("Class DoubleDamage correct ToString() method", 1M, "DoubleDamage", type => type.CheckMethodValue(type.GetInstance(10), "ToString", "DoubleDamage(10)")));

            // Logic
            list.Add(new TypeTest("DoubleDamage doubles the damage", 1.5M, "DoubleDamage", type =>
            {
                var obj = type.GetInstance(2);
                var creaturesInBattle = CreateCreaturesInBattleObject(type.Assembly);
                return type.CheckMethodValue(obj, "ChangeDamageWhenAttacking", 100M, creaturesInBattle, creaturesInBattle, 50M);
            }));
            list.Add(new TypeTest("DoubleDamage expires after few rounds", 1.5M, "DoubleDamage", type =>
            {
                var obj = type.GetInstance(2);
                var creaturesInBattle = CreateCreaturesInBattleObject(type.Assembly);
                type.CheckMethodValue(obj, "ChangeDamageWhenAttacking", 100M, creaturesInBattle, creaturesInBattle, 50M);
                type.CheckMethodValue(obj, "ChangeDamageWhenAttacking", 100M, creaturesInBattle, creaturesInBattle, 50M);
                return type.CheckMethodValue(obj, "ChangeDamageWhenAttacking", 50M, creaturesInBattle, creaturesInBattle, 50M);
            }));

            // Validations
            list.Add(new TypeTest("DoubleDamage constructor throws an exception when given 0 rounds", 1.5M, "DoubleDamage", type => type.ConstructorThrowsException(0)));
            list.Add(new TypeTest("DoubleDamage constructor throws an exception when given -1 rounds", 1M, "DoubleDamage", type => type.ConstructorThrowsException(-1)));
            list.Add(new TypeTest("DoubleDamage constructor throws an exception when given 11 rounds", 2M, "DoubleDamage", type => type.ConstructorThrowsException(11)));
            
            // Null checks
            list.Add(new TypeTest("DoubleDamage ChangeDamageWhenAttacking throws argument exception when given null attackerWithSpecialty", 1M, "DoubleDamage",
                type => type.MethodThrowsArgumentException(type.GetInstance(5), "ChangeDamageWhenAttacking", null, CreateCreaturesInBattleObject(type.Assembly), 0M)));
            list.Add(new TypeTest("DoubleDamage ChangeDamageWhenAttacking throws argument exception when given null defender", 1M, "DoubleDamage",
                type => type.MethodThrowsArgumentException(type.GetInstance(5), "ChangeDamageWhenAttacking", CreateCreaturesInBattleObject(type.Assembly), null, 0M)));
        }

        private void AddAttackWhenSkipChecks(IList<ITest> list)
        {
            // Common requirements
            list.Add(new PredicateTest("Added class AddAttackWhenSkip", 0.25M, assembly => assembly.GetTypes().Any(t => t.Name == "AddAttackWhenSkip")));
            list.Add(new TypeTest("Class AddAttackWhenSkip has only 1 constructor", 0.25M, "AddAttackWhenSkip", type => type.GetConstructors().Count() == 1));
            list.Add(new TypeTest("AddAttackWhenSkip constructor doesn't throw exception when attackToAdd is 10", 0.25M, "AddAttackWhenSkip", type => !type.ConstructorThrowsException(10)));
            list.Add(new TypeTest("Class AddAttackWhenSkip correct ToString() method", 1M, "AddAttackWhenSkip", type => type.CheckMethodValue(type.GetInstance(10), "ToString", "AddAttackWhenSkip(10)")));

            // Check AddAttackWhenSkip logic
            list.Add(new TypeTest("AddAttackWhenSkip ApplyOnSkip adds attack", 1.5M, "AddAttackWhenSkip", type =>
            {
                var obj = type.GetInstance(7);
                var creaturesInBattle = CreateCreaturesInBattleObject(type.Assembly, "Angel");
                type.InvokeMethod(obj, "ApplyOnSkip", creaturesInBattle);
                return creaturesInBattle.GetPropertyValue<int>("PermanentAttack").Equals(27);
            }));

            // Null checks
            list.Add(new TypeTest("AddAttackWhenSkip ApplyOnSkip throws argument exception when given null skipCreature", 1M, "AddAttackWhenSkip",
                type => type.MethodThrowsArgumentException(type.GetInstance(5), "ApplyOnSkip", new object[] { null })));

            // Validations
            list.Add(new TypeTest("AddAttackWhenSkip constructor throws an exception when given 0 attackToAdd", 1.5M, "AddAttackWhenSkip", type => type.ConstructorThrowsException(0)));
            list.Add(new TypeTest("AddAttackWhenSkip constructor throws an exception when given -1 attackToAdd", 1M, "AddAttackWhenSkip", type => type.ConstructorThrowsException(-1)));
            list.Add(new TypeTest("AddAttackWhenSkip constructor throws an exception when given 11 attackToAdd", 2M, "AddAttackWhenSkip", type => type.ConstructorThrowsException(11)));
        }

        private void AddDoubleAttackWhenAttackingChecks(IList<ITest> list)
        {
            list.Add(new PredicateTest("Added class DoubleAttackWhenAttacking", 0.25M, assembly => assembly.GetTypes().Any(t => t.Name == "DoubleAttackWhenAttacking")));
            list.Add(new TypeTest("Class DoubleAttackWhenAttacking has only 1 constructor", 0.25M, "DoubleAttackWhenAttacking", type => type.GetConstructors().Count() == 1));
            list.Add(new TypeTest("Class DoubleAttackWhenAttacking correct ToString() method", 1M, "DoubleAttackWhenAttacking", type => type.CheckMethodValue(type.GetInstance(10), "ToString", "DoubleAttackWhenAttacking(10)")));
            // TODO: Check DoubleAttackWhenAttacking logic
            // TODO: Null checks
            // TODO: Check validations
        }

        private void AddGoblinChecks(IList<ITest> list)
        {
            AddBaseCreatureChecks(list, "Goblin", 4, 2, 5, 1.5M, 0);
        }

        private void AddAncientBehemothChecks(IList<ITest> list)
        {
            AddBaseCreatureChecks(list, "AncientBehemoth", 19, 19, 300, 40, 2);
            AddCheckIfCreatureHasSpecialty(list, "AncientBehemoth", "ReduceEnemyDefenseByPercentage");
            AddCheckIfCreatureHasSpecialty(list, "AncientBehemoth", "DoubleDefenseWhenDefending");
        }

        private void AddWolfRaiderChecks(IList<ITest> list)
        {
            AddBaseCreatureChecks(list, "WolfRaider", 8, 5, 10, 3.5M, 1);
            AddCheckIfCreatureHasSpecialty(list, "WolfRaider", "DoubleDamage");
        }

        private void AddGriffinChecks(IList<ITest> list)
        {
            AddBaseCreatureChecks(list, "Griffin", 8, 8, 25, 4.5M, 3);
            AddCheckIfCreatureHasSpecialty(list, "Griffin", "DoubleDefenseWhenDefending");
            AddCheckIfCreatureHasSpecialty(list, "Griffin", "AddDefenseWhenSkip");
            AddCheckIfCreatureHasSpecialty(list, "Griffin", "Hate");
        }

        private void AddCyclopsKingChecks(IList<ITest> list)
        {
            AddBaseCreatureChecks(list, "CyclopsKing", 17, 13, 70, 18, 3);
            AddCheckIfCreatureHasSpecialty(list, "CyclopsKing", "AddAttackWhenSkip");
            AddCheckIfCreatureHasSpecialty(list, "CyclopsKing", "DoubleAttackWhenAttacking");
            AddCheckIfCreatureHasSpecialty(list, "CyclopsKing", "DoubleDamage");
        }

        private void AddBaseCreatureChecks(IList<ITest> list, string creatureName, int attack, int defense, int healthPoints, decimal damage, int specialtiesCount)
        {
            // Total: 2.25
            list.Add(new PredicateTest("Added class " + creatureName, 0.25M, assembly => assembly.GetTypes().Any(t => t.Name == creatureName)));
            list.Add(new PredicateTest(creatureName + " is a creature", 0.5M, assembly => assembly.GetTypes().Any(t => t.Name == creatureName && t.BaseType.Name == "Creature")));
            list.Add(new TypeTest(creatureName + " has " + attack + " attack", 0.25M, creatureName, type => type.CheckPropertyValue("Attack", attack)));
            list.Add(new TypeTest(creatureName + " has " + defense + " defense", 0.25M, creatureName, type => type.CheckPropertyValue("Defense", defense)));
            list.Add(new TypeTest(creatureName + " has " + healthPoints + " health points", 0.25M, creatureName, type => type.CheckPropertyValue("HealthPoints", healthPoints)));
            list.Add(new TypeTest(creatureName + " has " + damage + " damage", 0.25M, creatureName, type => type.CheckPropertyValue("Damage", damage)));
            list.Add(new TypeTest(creatureName + " has " + specialtiesCount + " specialties", 0.5M, creatureName, type => type.CheckPropertyValue<IEnumerable<object>>("Specialties", v => v.Count() == specialtiesCount)));
        }

        private void AddCheckIfCreatureHasSpecialty(IList<ITest> list, string creatureName, string specialtyName)
        {
            list.Add(new TypeTest(creatureName + " has " + specialtyName, 0.75M, creatureName, type => type.CheckPropertyValue<IEnumerable<object>>("Specialties", v => v.Any(i => i.GetType().Name == specialtyName))));
        }

        private object CreateCreaturesInBattleObject(Assembly assembly, string creatureTypeName = "Angel")
        {
            var creature = assembly.GetTypeByName(creatureTypeName).GetInstance();
            var creaturesInBattle = assembly.GetTypeByName("CreaturesInBattle").GetInstance(creature, 1);
            return creaturesInBattle;
        }
    }
}
