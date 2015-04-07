namespace OopExamAutomation.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using OopExamAutomation.Engine;
    using OopExamAutomation.Problems.ArmyOfCreatures;

    public static class Program
    {
        private const string CSharpCompilerPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
        private const string MSBuildCompilerPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe";

        private static string workingDirectory = @"C:\OOP\";
        private static string solutionsFolder = workingDirectory + @"\solutions\";
        private static string reportsDirectory = workingDirectory + @"\reports\";
        private static string outputFile = workingDirectory + @"\results.csv";

        private static IFileCompiler fileCompiler = new DotNetFileCompiler(CSharpCompilerPath, MSBuildCompilerPath);
        private static ITestsProvider testsProvider = new ArmyOfCreaturesTestsProvider();

        public static void Main()
        {
            IEnumerable<ITest> tests = testsProvider.GetTests();

            var assembly = Assembly.LoadFile(@"C:\Temp\OOP\Solution\ArmyOfCreatures\bin\Debug\ArmyOfCreatures.exe");

            var totalPoints = RunTests(tests, assembly, Console.Out);
            Console.WriteLine(totalPoints);
            // RunTests
        }

        private static decimal RunTests(IEnumerable<ITest> tests, Assembly assembly, TextWriter output)
        {
            decimal totalPoints = 0;

            foreach (var test in tests)
            {
                var testPoints = RunTest(test, assembly, output);
                totalPoints += testPoints;
            }

            return totalPoints;
        }

        private static decimal RunTest(ITest test, Assembly assembly, TextWriter output)
        {
            var result = test.CalculateResult(assembly);
            output.WriteLine("{0} ({1} {2}) - {3}", test.Name, result.Points, result.Points == 1 ? "point" : "points", result.TextResult);
            return result.Points;
        }
    }
}
