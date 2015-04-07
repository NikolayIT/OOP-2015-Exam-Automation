namespace OopExamAutomation.Engine
{
    using System;

    using OJS.Workers.Common;
    using OJS.Workers.Compilers;

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
                result = compiler.Compile(this.csharpCompilerPath, fileToCompile, "/optimize+ /nologo /reference:System.Numerics.dll");
            }
            else if (fileToCompile.EndsWith(".zip"))
            {
                var compiler = new MsBuildCompiler();
                result = compiler.Compile(this.msbuildCompilerPath, fileToCompile, "/t:rebuild /p:Configuration=Release,Optimize=true /verbosity:quiet /nologo");
            }
            else
            {
                throw new ArgumentException("Invalid file extension", "fileToCompile");
            }

            return new FileCompileResult(result);
        }
    }
}
