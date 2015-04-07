namespace OopExamAutomation.Engine
{
    public class TestResult
    {
        public TestResult(decimal points, string textResult)
        {
            this.Points = points;
            this.TextResult = textResult;
        }

        public decimal Points { get; private set; }

        public string TextResult { get; private set; }
    }
}
