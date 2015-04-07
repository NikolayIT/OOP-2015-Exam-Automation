namespace OopExamAutomation.Engine
{
    using System.Collections.Generic;

    public interface ITestsProvider
    {
        IEnumerable<ITest> GetTests();
    }
}
