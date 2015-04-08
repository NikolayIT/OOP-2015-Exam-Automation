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
        private const string CSharpCompilerPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
        private const string MSBuildCompilerPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe";

        private const string WorkingDirectory = @"C:\Temp\OOP\Solutions";
        private const string ReportsDirectory = @"C:\Temp\OOP\Reports";
        private const string ResultsOutputFile = @"C:\Temp\OOP\Results.csv";
        private const string ProblemName = @"2. Army of Creatures";

        private static IFileCompiler fileCompiler = new DotNetFileCompiler(CSharpCompilerPath, MSBuildCompilerPath);
        private static ITestsProvider testsProvider = new ArmyOfCreaturesTestsProvider();

        public static void Main()
        {
            Directory.CreateDirectory(ReportsDirectory);

            IEnumerable<ITest> tests = testsProvider.GetTests();

            var directories = Directory.GetDirectories(WorkingDirectory);
            using (var reportFile = new StreamWriter(ResultsOutputFile))
            {
                foreach (var directory in directories)
                {
                    var testSolutionResult = TestSolution(directory, tests);
                    Console.WriteLine(testSolutionResult);
                    reportFile.WriteLine(testSolutionResult);
                    reportFile.Flush();
                }
            }
        }

        private static TestSolutionResult TestSolution(string directory, IEnumerable<ITest> tests)
        {
            TestSolutionResult result = new TestSolutionResult(new DirectoryInfo(directory).Name);

            var solutionFilePath = Directory.GetFiles(directory).OrderByDescending(fileName => fileName.Length).FirstOrDefault(fileName => fileName.Contains(ProblemName) && (fileName.EndsWith(".cs") || fileName.EndsWith(".zip")));
            if (solutionFilePath == null)
            {
                result.Points = 0;
                result.Comment = "Solution file not found!";
                return result;
            }

            FileCompileResult compileResult = null;
            // AppDomain dom = AppDomain.CreateDomain("some");
            try
            {
                using (StreamWriter report = new StreamWriter(string.Format("{0}\\{1} [{2}] [{3}].txt", ReportsDirectory, result.UserName, result.Email, result.StudentsNumber)))
                {
                    compileResult = fileCompiler.Compile(solutionFilePath);
                    if (!compileResult.IsCompiledSuccessfully)
                    {
                        report.WriteLine("Compilation failed!");
                        report.WriteLine(compileResult.CompilerComment);
                        report.WriteLine();

                        result.Points = 0;
                        result.Comment = "Compilation failed!";
                        return result;
                    }

                    if (!string.IsNullOrWhiteSpace(compileResult.CompilerComment))
                    {
                        report.WriteLine("Compiler comment:");
                        report.WriteLine(compileResult.CompilerComment);
                    }
                    else
                    {
                        // No warnings? +5 points
                        result.Points += 5;
                        report.WriteLine("No warnings. +5 points.");
                    }

                    report.WriteLine();

                    // var assembly = dom.Load(new AssemblyName { CodeBase = compileResult.OutputFile });
                    var assembly = Assembly.LoadFile(compileResult.OutputFile);

                    var totalPoints = RunTests(tests, assembly, report);
                    report.WriteLine();

                    result.Comment = string.Empty;
                    result.Points += totalPoints;
                    report.WriteLine("Total points: " + result.Points);

                    return result;
                }
            }
            catch(Exception exception)
            {
                result.Points = 0;
                result.Comment = exception.ToString();
                return result;
            }
            finally
            {
                // AppDomain.Unload(dom);
                // if (compileResult != null && compileResult.OutputFile != null)
                // {
                //    File.Delete(compileResult.OutputFile);
                // }
            }
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
