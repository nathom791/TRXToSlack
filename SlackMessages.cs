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
        private static Settings settings = new Settings();
        private static string workingDir = Environment.CurrentDirectory;

        public async Task SendToChannel(TestResults results, string settingsFileName)
        {
            SetConfig(settingsFileName);

            var WebHookUrl = settings.webhookUrl;
            var client = new SbmClient(WebHookUrl);

            string moodColor = "good";
            decimal percentPassed = results.percentPassed(results);

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

            var message = new Message(Program.fileName.Trim('.'))
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
                            Value = results.Total + $" - {percentPassed}%",
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
                        }
                    }
                    }
                }
            };

            await client.SendAsync(message);
        }

        public void SetConfig(string settingsFileName)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(workingDir, settingsFileName));

            var root = builder.Build();

            settings.webhookUrl = root.GetSection("webhookUrl").Value;
        }
    }
}