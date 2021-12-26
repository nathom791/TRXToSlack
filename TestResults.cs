namespace TRXToSlack
{
    public class TestResults
    {
        public string FileName { get; set; }
        public string Total { get; set; }
        public string Passed { get; set; }
        public string Failed { get; set; }

        public decimal percentPassed(TestResults result)
        {
            decimal passed = decimal.Parse(result.Passed);
            decimal total = decimal.Parse(result.Total);

            return (passed / total) * 100;
        }
    }
}