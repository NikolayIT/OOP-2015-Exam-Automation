namespace OopExamAutomation.Engine
{
    public interface IFileCompiler
    {
        FileCompileResult Compile(string fileToCompile);
    }
}
