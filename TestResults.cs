using System;

namespace TRXToSlack
{
    public class TestResults
    {
        public string FileName { get; set; }
        public string Total { get; set; }
        public string Passed { get; set; }
        public string Failed { get; set; }
        public string Start { get; set; }
        public string Finish { get; set; }

        public static string GetDuration(TestResults result)
        {
            DateTime start = DateTime.Parse(result.Start);
            DateTime finish = DateTime.Parse(result.Finish);

            decimal duration = (decimal)finish.Subtract(start).TotalMinutes;
            decimal durationRounded = decimal.Round(duration, 2, MidpointRounding.AwayFromZero);
            return durationRounded.ToString();
        }

        public static decimal GetPercentPassed(TestResults result)
        {
            decimal passed = decimal.Parse(result.Passed);
            decimal failed = decimal.Parse(result.Failed);
            decimal total = decimal.Parse(result.Total);

            decimal temp = passed + failed;
            decimal skipped = total - temp;
            decimal totalMinusSkipped = total - skipped;
            decimal percentageUnrounded = (totalMinusSkipped / total) * 100;
            decimal percentage = decimal.Round(percentageUnrounded, 2, MidpointRounding.AwayFromZero);
            return percentage;
        }
    }
}