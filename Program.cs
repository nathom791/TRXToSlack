using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace TRXToSlack
{
    public static class Program
    {
        private static readonly SlackMessages slack = new();

        public static string FileName { get; set; }
        public static string SettingsFileName { get; set; }

        private static async Task Main(string[] args)
        {
            //FileName = Console.ReadLine();
            //SettingsFileName = Console.ReadLine();
            FileName = args[0];
            SettingsFileName = args[1];

            try
            {
                await slack.SendToChannel(ParseTRX(FileName), SettingsFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static TestResults ParseTRX(string fileName)
        {
            TestResults testResults = new();

            var filename = fileName;
            var currentDir = Directory.GetCurrentDirectory();
            var resultsFilePath = Path.Combine(currentDir, filename);

            XmlDocument xmlDoc = new();
            xmlDoc.Load(resultsFilePath);

            XmlNodeList counters = xmlDoc.GetElementsByTagName("Counters");
            testResults.FileName = fileName;
            testResults.Total = counters[0].Attributes["total"].Value;
            testResults.Passed = counters[0].Attributes["passed"].Value;
            testResults.Failed = counters[0].Attributes["failed"].Value;

            XmlNodeList times = xmlDoc.GetElementsByTagName("Times");
            testResults.Start = times[0].Attributes["start"].Value;
            testResults.Finish = times[0].Attributes["finish"].Value;

            return testResults;
        }
    }
}