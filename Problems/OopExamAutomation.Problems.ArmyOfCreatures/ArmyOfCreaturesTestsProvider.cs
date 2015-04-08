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
            // Class exists
            list.Add(new PredicateTest("BattleManager is extended", 3M, assembly => assembly.GetTypes().Any(type => type.BaseType != null && type.BaseType.Name == "BattleManager")));
            
            // Null checks
            list.Add(new PredicateTest("In extended BattleManager AddCreaturesByIdentifier throws exception when creatureIdentifier is null", 4M, assembly =>
            {
                var instance = CreateBattleManagerObject(assembly);
                if (instance == null)
                {
                    return false;
                }

                return instance.GetType().MethodThrowsArgumentException(instance, "AddCreaturesByIdentifier", null, CreateCreaturesInBattleObject(assembly));
            }));
            list.Add(new PredicateTest("In extended BattleManager AddCreaturesByIdentifier throws exception when creaturesInBattle is null", 4M, assembly =>
            {
                var instance = CreateBattleManagerObject(assembly);
                if (instance == null)
                {
                    return false;
                }

                return instance.GetType().MethodThrowsArgumentException(instance, "AddCreaturesByIdentifier", CreateCreatureIdentifierObject(assembly), null);
            }));
            list.Add(new PredicateTest("In extended BattleManager GetByIdentifier throws exception when identifier is null", 4M, assembly =>
            {
                var instance = CreateBattleManagerObject(assembly);
                if (instance == null)
                {
                    return false;
                }

                return instance.GetType().MethodThrowsArgumentException(instance, "GetByIdentifier", new object[] { null });
            }));
        }

        private object CreateCreatureIdentifierObject(Assembly assembly, int armyId = 1)
        {
            var type = assembly.GetTypeByName("CreatureIdentifier");
            if (type == null)
            {
                return null;
            }

            return type.GetInstance("Angel", armyId);
        }

        private object CreateBattleManagerObject(Assembly assembly)
        {
            var type = assembly.GetTypes().FirstOrDefault(t => t.BaseType != null && t.BaseType.Name == "BattleManager");
            if (type == null)
            {
                return null;
            }

            var factory = assembly.GetTypeByName("CreaturesFactory").GetInstance();
            var logger = assembly.GetTypeByName("ConsoleLogger").GetInstance();

            var instance = type.GetInstance(factory, logger);
            return instance;
        }

        private void AddFactoryChecks(IList<ITest> list)
        {
            list.Add(new PredicateTest("CreaturesFactory is extended", 3M, assembly => assembly.GetTypes().Any(type => type.BaseType != null && type.BaseType.Name == "CreaturesFactory")));
            list.Add(new PredicateTest("CreaturesFactory CreateCreature throws exception when given invalid data", 2M, assembly =>
            {
                var type = assembly.GetTypes().FirstOrDefault(t => t.BaseType != null && t.BaseType.Name == "CreaturesFactory");
                if (type == null)
                {
                    return false;
                }

                return type.MethodThrowsArgumentException(type.GetInstance(), "CreateCreature", "Invalid");
            }));

            list.Add(new PredicateTest("CreaturesFactory CreateCreature works as expected (AncientBehemoth)", 0.5M, assembly => CheckCreateCreatureMethod(assembly, "AncientBehemoth")));
            list.Add(new PredicateTest("CreaturesFactory CreateCreature works as expected (CyclopsKing)", 0.5M, assembly => CheckCreateCreatureMethod(assembly, "CyclopsKing")));
            list.Add(new PredicateTest("CreaturesFactory CreateCreature works as expected (Goblin)", 0.5M, assembly => CheckCreateCreatureMethod(assembly, "Goblin")));
            list.Add(new PredicateTest("CreaturesFactory CreateCreature works as expected (Griffin)", 0.5M, assembly => CheckCreateCreatureMethod(assembly, "Griffin")));
            list.Add(new PredicateTest("CreaturesFactory CreateCreature works as expected (WolfRaider)", 0.5M, assembly => CheckCreateCreatureMethod(assembly, "WolfRaider")));
            list.Add(new PredicateTest("CreaturesFactory CreateCreature works as expected (Angel)", 0.25M, assembly => CheckCreateCreatureMethod(assembly, "Angel")));
            list.Add(new PredicateTest("CreaturesFactory CreateCreature works as expected (ArchDevil)", 0.25M, assembly => CheckCreateCreatureMethod(assembly, "ArchDevil")));
        }

        private void AddDoubleDamageChecks(IList<ITest> list)
        {
            // Common requirements
            list.Add(new PredicateTest("Added class DoubleDamage", 0.50M, assembly => assembly.GetTypes().Any(t => t.Name == "DoubleDamage")));
            list.Add(new TypeTest("Class DoubleDamage has only 1 constructor", 0.25M, "DoubleDamage", type => type.GetConstructors().Count() == 1));
            list.Add(new TypeTest("DoubleDamage constructor doesn't throw exception when rounds is 10", 0.25M, "DoubleDamage", type => !type.ConstructorThrowsException(10)));
            list.Add(new TypeTest("Class DoubleDamage has correct ToString() method", 1M, "DoubleDamage", type => type.CheckMethodValue(type.GetInstance(10), "ToString", "DoubleDamage(10)")));

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
            list.Add(new TypeTest("DoubleDamage constructor throws an exception when given 0 rounds", 2M, "DoubleDamage", type => type.ConstructorThrowsException(0)));
            list.Add(new TypeTest("DoubleDamage constructor throws an exception when given -1 rounds", 2M, "DoubleDamage", type => type.ConstructorThrowsException(-1)));
            list.Add(new TypeTest("DoubleDamage constructor throws an exception when given 11 rounds", 4M, "DoubleDamage", type => type.ConstructorThrowsException(11)));
            
            // Null checks
            list.Add(new TypeTest("DoubleDamage ChangeDamageWhenAttacking throws argument exception when given null attackerWithSpecialty", 4M, "DoubleDamage",
                type => type.MethodThrowsArgumentException(type.GetInstance(5), "ChangeDamageWhenAttacking", null, CreateCreaturesInBattleObject(type.Assembly), 0M)));
            list.Add(new TypeTest("DoubleDamage ChangeDamageWhenAttacking throws argument exception when given null defender", 4M, "DoubleDamage",
                type => type.MethodThrowsArgumentException(type.GetInstance(5), "ChangeDamageWhenAttacking", CreateCreaturesInBattleObject(type.Assembly), null, 0M)));
        }

        private void AddAttackWhenSkipChecks(IList<ITest> list)
        {
            // Common requirements
            list.Add(new PredicateTest("Added class AddAttackWhenSkip", 0.50M, assembly => assembly.GetTypes().Any(t => t.Name == "AddAttackWhenSkip")));
            list.Add(new TypeTest("Class AddAttackWhenSkip has only 1 constructor", 0.25M, "AddAttackWhenSkip", type => type.GetConstructors().Count() == 1));
            list.Add(new TypeTest("AddAttackWhenSkip constructor doesn't throw exception when attackToAdd is 10", 0.25M, "AddAttackWhenSkip", type => !type.ConstructorThrowsException(10)));
            list.Add(new TypeTest("Class AddAttackWhenSkip has correct ToString() method", 1M, "AddAttackWhenSkip", type => type.CheckMethodValue(type.GetInstance(10), "ToString", "AddAttackWhenSkip(10)")));

            // Check AddAttackWhenSkip logic
            list.Add(new TypeTest("AddAttackWhenSkip ApplyOnSkip adds attack", 1.5M, "AddAttackWhenSkip", type =>
            {
                var obj = type.GetInstance(7);
                var creaturesInBattle = CreateCreaturesInBattleObject(type.Assembly, "Angel");
                type.InvokeMethod(obj, "ApplyOnSkip", creaturesInBattle);
                return creaturesInBattle.GetPropertyValue<int>("PermanentAttack").Equals(27);
            }));

            // Null checks
            list.Add(new TypeTest("AddAttackWhenSkip ApplyOnSkip throws argument exception when given null skipCreature", 4M, "AddAttackWhenSkip",
                type => type.MethodThrowsArgumentException(type.GetInstance(5), "ApplyOnSkip", new object[] { null })));

            // Validations
            list.Add(new TypeTest("AddAttackWhenSkip constructor throws an exception when given 0 attackToAdd", 2M, "AddAttackWhenSkip", type => type.ConstructorThrowsException(0)));
            list.Add(new TypeTest("AddAttackWhenSkip constructor throws an exception when given -1 attackToAdd", 2M, "AddAttackWhenSkip", type => type.ConstructorThrowsException(-1)));
            list.Add(new TypeTest("AddAttackWhenSkip constructor throws an exception when given 11 attackToAdd", 4M, "AddAttackWhenSkip", type => type.ConstructorThrowsException(11)));
        }

        private void AddDoubleAttackWhenAttackingChecks(IList<ITest> list)
        {
            // Common requirements
            list.Add(new PredicateTest("Added class DoubleAttackWhenAttacking", 0.50M, assembly => assembly.GetTypes().Any(t => t.Name == "DoubleAttackWhenAttacking")));
            list.Add(new TypeTest("Class DoubleAttackWhenAttacking has only 1 constructor", 0.25M, "DoubleAttackWhenAttacking", type => type.GetConstructors().Count() == 1));
            list.Add(new TypeTest("DoubleAttackWhenAttacking constructor doesn't throw exception when rounds is 10", 0.25M, "DoubleAttackWhenAttacking", type => !type.ConstructorThrowsException(10)));
            list.Add(new TypeTest("Class DoubleAttackWhenAttacking has correct ToString() method", 1M, "DoubleAttackWhenAttacking", type => type.CheckMethodValue(type.GetInstance(10), "ToString", "DoubleAttackWhenAttacking(10)")));

            // Check DoubleAttackWhenAttacking logic
            list.Add(new TypeTest("DoubleAttackWhenAttacking ApplyWhenAttacking doubles the attack", 1.5M, "DoubleAttackWhenAttacking", type =>
            {
                var obj = type.GetInstance(5);
                var creaturesInBattle = CreateCreaturesInBattleObject(type.Assembly, "Angel");
                type.InvokeMethod(obj, "ApplyWhenAttacking", creaturesInBattle, creaturesInBattle);
                return creaturesInBattle.GetPropertyValue<int>("CurrentAttack").Equals(40);
            }));
            list.Add(new TypeTest("DoubleAttackWhenAttacking expires after few rounds", 1.5M, "DoubleAttackWhenAttacking", type =>
            {
                var obj = type.GetInstance(1);
                var creaturesInBattle = CreateCreaturesInBattleObject(type.Assembly);
                type.InvokeMethod(obj, "ApplyWhenAttacking", creaturesInBattle, creaturesInBattle);
                if (!creaturesInBattle.GetPropertyValue<int>("CurrentAttack").Equals(40))
                {
                    return false;
                }

                creaturesInBattle = CreateCreaturesInBattleObject(type.Assembly);
                type.InvokeMethod(obj, "ApplyWhenAttacking", creaturesInBattle, creaturesInBattle);
                return creaturesInBattle.GetPropertyValue<int>("CurrentAttack").Equals(20);
            }));

            // Null checks
            list.Add(new TypeTest("DoubleAttackWhenAttacking ApplyWhenAttacking throws argument exception when given null attackerWithSpecialty", 4M, "DoubleAttackWhenAttacking",
                type => type.MethodThrowsArgumentException(type.GetInstance(5), "ApplyWhenAttacking", new object[] { null, CreateCreaturesInBattleObject(type.Assembly) })));
            list.Add(new TypeTest("DoubleAttackWhenAttacking ApplyWhenAttacking throws argument exception when given null defender", 4M, "DoubleAttackWhenAttacking",
                type => type.MethodThrowsArgumentException(type.GetInstance(5), "ApplyWhenAttacking", new object[] { CreateCreaturesInBattleObject(type.Assembly), null })));

            // Validations
            list.Add(new TypeTest("DoubleAttackWhenAttacking constructor throws an exception when given 0 rounds", 2M, "DoubleAttackWhenAttacking", type => type.ConstructorThrowsException(0)));
            list.Add(new TypeTest("DoubleAttackWhenAttacking constructor throws an exception when given -1 rounds", 2M, "DoubleAttackWhenAttacking", type => type.ConstructorThrowsException(-1)));
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
            // Total: 2.25 points
            list.Add(new PredicateTest("Added class " + creatureName, 0.25M, assembly => assembly.GetTypes().Any(t => t.Name == creatureName)));
            list.Add(new PredicateTest(creatureName + " is a creature", 0.60M, assembly => assembly.GetTypes().Any(t => t.Name == creatureName && t.BaseType.Name == "Creature")));
            list.Add(new TypeTest(creatureName + " has " + attack + " attack", 0.25M, creatureName, type => type.CheckPropertyValue("Attack", attack)));
            list.Add(new TypeTest(creatureName + " has " + defense + " defense", 0.25M, creatureName, type => type.CheckPropertyValue("Defense", defense)));
            list.Add(new TypeTest(creatureName + " has " + healthPoints + " health points", 0.25M, creatureName, type => type.CheckPropertyValue("HealthPoints", healthPoints)));
            list.Add(new TypeTest(creatureName + " has " + damage + " damage", 0.25M, creatureName, type => type.CheckPropertyValue("Damage", damage)));
            list.Add(new TypeTest(creatureName + " has " + specialtiesCount + " specialties", 0.40M, creatureName, type => type.CheckPropertyValue<IEnumerable<object>>("Specialties", v => v.Count() == specialtiesCount)));
        }

        private void AddCheckIfCreatureHasSpecialty(IList<ITest> list, string creatureName, string specialtyName)
        {
            list.Add(new TypeTest(creatureName + " has " + specialtyName, 0.25M, creatureName, type => type.CheckPropertyValue<IEnumerable<object>>("Specialties", v => v.Any(i => i.GetType().Name == specialtyName))));
        }

        private object CreateCreaturesInBattleObject(Assembly assembly, string creatureTypeName = "Angel")
        {
            var creature = assembly.GetTypeByName(creatureTypeName).GetInstance();
            var creaturesInBattle = assembly.GetTypeByName("CreaturesInBattle").GetInstance(creature, 1);
            return creaturesInBattle;
        }

        private bool CheckCreateCreatureMethod(Assembly assembly, string typeName)
        {
            var type = assembly.GetTypes().FirstOrDefault(t => t.BaseType != null && t.BaseType.Name == "CreaturesFactory");
            if (type == null)
            {
                return false;
            }

            var result = type.GetMethodValue(type.GetInstance(), "CreateCreature", typeName);
            if (result == null)
            {
                return false;
            }

            return result.GetType().Name == typeName;
        }
    }
}
