namespace OopExamAutomation.Console
{
    using System;
    using System.Linq;

    public class TestSolutionResult
    {
        public TestSolutionResult(string directoryName)
        {
            this.ParseFileName(directoryName);
        }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string StudentsNumber { get; set; }

        public decimal Points { get; set; }

        public string Comment { get; set; }

        public override string ToString()
        {
            return string.Format("{3:00.00},{0},{2},\"{4}\"",
                this.UserName,
                this.Email,
                this.StudentsNumber,
                this.Points,
                this.Comment.Replace("\"", "\"\""));
        }

        private void ParseFileName(string directoryName)
        {
            var fileNameParts = directoryName.Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim(' ', ']')).ToList();

            this.UserName = fileNameParts[0];
            this.Email = fileNameParts[1];
            this.StudentsNumber = fileNameParts[2];
        }
    }
}
