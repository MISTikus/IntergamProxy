using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TfsNotifications.Extensions;

namespace TfsNotifications.Models.Requests
{
    public class BuildTransmittionRequest : TransmittionRequest
    {
        private const string builderTitle = "Автор запроса";
        private const string durationTitle = "Длительность";
        private const string buildNameTitle = "Определение сборки";

        public BuildTransmittionRequest(string url, string templateFileName, NotificationRequest request) : base(url)
        {
            var data = request.Attachments.First();
            var builder = data.Fields.FirstOrDefault(x => x.Title == builderTitle)?.Value;
            var duration = data.Fields.FirstOrDefault(x => x.Title == durationTitle)?.Value;
            var buildName = data.Fields.FirstOrDefault(x => x.Title == buildNameTitle)?.Value;

            var re = new Regex(@".*<(?<url>[^|]*)\|(?<number>.*)>.*");
            var match = re.Match(data.PreText);
            var buildUrl = match.Groups["url"]?.Value;
            var buildNumber = match.Groups["number"]?.Value;

            if (!File.Exists(templateFileName))
                throw new ArgumentException($"Template file does not exists: '{templateFileName}'");

            var template = File.ReadAllText(templateFileName);
            var text = template.ReplaceAll(new[] {
                ("{name}", buildName),
                ("{number}", buildNumber),
                ("{url}", buildUrl),
                ("{builderName}", builder),
                ("{duration}", duration)
            });

            Text = text;
        }

        public BuildTransmittionRequest(string url, string templateFileName, WebHook request) : base(url)
        {
            var buildName = request.Resource.Definition.Name;
            var builder = request.Resource.Requests.FirstOrDefault().RequestedFor.DisplayName;
            var duration = (request.Resource.FinishTime - request.Resource.StartTime).ToString(@"hh\:mm\:ss");

            var re = new Regex(@".*\[(?<number>.*)\]\((?<url>.*)\)");
            var match = re.Match(request.Message.Markdown);
            var buildUrl = match.Groups["url"]?.Value;
            var buildNumber = match.Groups["number"]?.Value;

            if (!File.Exists(templateFileName))
                throw new ArgumentException($"Template file does not exists: '{templateFileName}'");

            var template = File.ReadAllText(templateFileName);
            var text = template.ReplaceAll(new[] {
                ("{name}", buildName),
                ("{number}", buildNumber),
                ("{url}", buildUrl),
                ("{builderName}", builder),
                ("{duration}", duration)
            });

            Text = text;
        }
    }
}