namespace OopExamAutomation.Engine
{
    using System;

    using OJS.Workers.Common;
    using OJS.Workers.Compilers;
    using System.IO;

    public class DotNetFileCompiler : IFileCompiler
    {
        private string csharpCompilerPath;
        private string msbuildCompilerPath;

        public DotNetFileCompiler(string csharpCompilerPath, string msbuildCompilerPath)
        {
            this.csharpCompilerPath = csharpCompilerPath;
            this.msbuildCompilerPath = msbuildCompilerPath;
        }

        public FileCompileResult Compile(string fileToCompile)
        {
            CompileResult result;
            if (fileToCompile.EndsWith(".cs"))
            {
                var compiler = new CSharpCompiler();
                var copiedFileName = fileToCompile + ".cs";
                File.Copy(fileToCompile, copiedFileName);
                result = compiler.Compile(this.csharpCompilerPath, copiedFileName, "/optimize+ /nologo /reference:System.Numerics.dll");
            }
            else if (fileToCompile.EndsWith(".zip"))
            {
                var compiler = new MsBuildCompiler();
                var copiedFileName = fileToCompile + ".zip";
                File.Copy(fileToCompile, copiedFileName);
                result = compiler.Compile(this.msbuildCompilerPath, copiedFileName, "/t:rebuild /p:Configuration=Release,Optimize=true /verbosity:quiet /nologo");
            }
            else
            {
                throw new ArgumentException("Invalid file extension", "fileToCompile");
            }

            return new FileCompileResult(result);
        }
    }
}
