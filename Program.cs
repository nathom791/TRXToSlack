using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace TRXToSlack
{
    public class Program
    {
        public static string fileName;
        public static string SettingsFileName;
        private static SlackAPI slack = new SlackAPI();

        private static async Task Main(string[] args)
        {
            //fileName = Console.ReadLine();
            //SettingsFileName = Console.ReadLine();
            fileName = args[0];
            SettingsFileName = args[1];

            try
            {
                await slack.SendToChannel(ParseTRX(fileName), SettingsFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static TestResults ParseTRX(string fileName)
        {
            TestResults testResults = new TestResults();

            var filename = fileName;
            var currentDir = Directory.GetCurrentDirectory();
            var resultsFilePath = Path.Combine(currentDir, filename);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(resultsFilePath);

            XmlNodeList counters = xmlDoc.GetElementsByTagName("Counters");
            testResults.FileName = fileName;
            testResults.Total = counters[0].Attributes["total"].Value;
            testResults.Passed = counters[0].Attributes["passed"].Value;
            testResults.Failed = counters[0].Attributes["failed"].Value;

            return testResults;
        }
    }
}