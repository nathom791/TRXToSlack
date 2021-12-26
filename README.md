# TRXToSlack
Parse results from a dotnet test .trx result file and post it to Slack using SlackBotMessages: https://github.com/prjseal/SlackBotMessages

This is useful if using something like "dotnet test -l:trx" to run tests in a pipeline from Azure, and you want to post the test results to Slack.
https://imgur.com/EqMqYAa

Installation: 
Clone the repo and run "dotnet build"

Modify the target config json file to contain your webhookUrl from Slack. Sample json file is included in the repo, "config.json"

Run "trxtoslack.exe {anyNamedTestResults.trx} {anyNamedConfig.json}"
