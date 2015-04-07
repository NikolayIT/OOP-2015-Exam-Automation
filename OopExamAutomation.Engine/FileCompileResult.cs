namespace OopExamAutomation.Engine
{
    using OJS.Workers.Common;
    
    public class FileCompileResult
    {
        public FileCompileResult(CompileResult compileResult)
        {
            this.CompilerComment = compileResult.CompilerComment;
            this.IsCompiledSuccessfully = compileResult.IsCompiledSuccessfully;
            this.OutputFile = compileResult.OutputFile;
        }

        public string CompilerComment { get; set; }

        public bool IsCompiledSuccessfully { get; set; }

        public string OutputFile { get; set; }
    }
}
