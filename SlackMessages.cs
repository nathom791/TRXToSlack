using Microsoft.Extensions.Configuration;
using SlackBotMessages;
using SlackBotMessages.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TRXToSlack
{
    internal class SlackMessages
    {
        private static readonly Settings settings = new();
        private static readonly string workingDir = Environment.CurrentDirectory;

        public async Task SendToChannel(TestResults results, string settingsFileName)
        {
            SetConfig(settingsFileName);

            var WebHookUrl = settings.webhookUrl;
            var client = new SbmClient(WebHookUrl);
            string duration = TestResults.GetDuration(results);

            string moodColor = "good";
            decimal percentPassed = TestResults.GetPercentPassed(results);
            string skipped = (int.Parse(results.Total) - (int.Parse(results.Passed) + int.Parse(results.Failed))).ToString();


            if (percentPassed > 85)
            {
                moodColor = "good";
            }
            else if (percentPassed > 65 && percentPassed <= 85)
            {
                moodColor = "warning";
            }
            else if (percentPassed <= 65)
            {
                moodColor = "danger";
            }

            string fileName = Program.FileName;
            int index = fileName.IndexOf(".");
            if (index >= 0)
                fileName = fileName[..index];

           fileName = fileName[(fileName.LastIndexOf('\\') + 1)..];

            var message = new Message(fileName)
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                       Color = moodColor,
                       Fields = new List<Field>
                    {
                        new Field
                        {
                            Title = "Total",
                            Value = results.Total + $" -  {percentPassed}%",
                            Short = true
                        },
                        new Field
                        {
                            Title = "Skipped",
                            Value = skipped,
                            Short = true
                        },
                        new Field
                        {
                            Title = "Passed",
                            Value = results.Passed,
                            Short = true
                        },
                        new Field
                        {
                            Title = "Failed",
                            Value = results.Failed,
                            Short = true
                        },
                        new Field
                        {
                            Title = "Duration",
                            Value = $"{duration} minutes",
                            Short = true
                        }
                    }
                    }
                }
            };

            await client.SendAsync(message);
        }

        public static void SetConfig(string settingsFileName)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(workingDir, settingsFileName));

            var root = builder.Build();

            settings.webhookUrl = root.GetSection("webhookUrl").Value;
        }
    }
}