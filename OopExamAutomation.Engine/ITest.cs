namespace OopExamAutomation.Engine
{
    using System.Reflection;

    public interface ITest
    {
        string Name { get; }

        TestResult CalculateResult(Assembly assembly);
    }
}
