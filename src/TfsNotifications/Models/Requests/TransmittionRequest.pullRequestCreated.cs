using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TfsNotifications.Extensions;

namespace TfsNotifications.Models.Requests
{
    public class PullRequestCreatedTransmittionRequest : TransmittionRequest
    {
        private const string destinationTitle = "Целевая платформа";
        private const string branchTitle = "Источник";

        public PullRequestCreatedTransmittionRequest(string url, string templateFileName, NotificationRequest request) : base(url)
        {
            var data = request.Attachments.First();
            var destination = data.Fields.FirstOrDefault(x => x.Title == destinationTitle)?.Value;
            var branch = data.Fields.FirstOrDefault(x => x.Title == branchTitle)?.Value;

            var re = new Regex(@"(?<author>[А-Яа-я]+\s[А-Яа-я]+).*<(?<url>[^|]+)\|[\D]*(?<number>\d+)>\s\((?<name>[\w\s\[\]\:]*)\).*\|(?<project>\w+)>");
            var match = re.Match(data.PreText);
            var author = match.Groups["author"]?.Value;
            var buildUrl = match.Groups["url"]?.Value;
            var number = match.Groups["number"]?.Value;
            var name = match.Groups["name"]?.Value;
            var project = match.Groups["project"]?.Value;

            if (!File.Exists(templateFileName))
                throw new ArgumentException($"Template file does not exists: '{templateFileName}'");

            var template = File.ReadAllText(templateFileName);
            var text = template.ReplaceAll(new[] {
                ("{author}", author),
                ("{url}", buildUrl),
                ("{number}", number),
                ("{name}", name),
                ("{project}", project),
                ("{branch}", branch),
                ("{destination}", destination),
            });

            Text = text;
        }

        public PullRequestCreatedTransmittionRequest(string url, string templateFileName, WebHook request) : base(url)
        {
            var destination = request.Resource.TargetBranch;
            var branch = request.Resource.SourceBranch;

            var re = new Regex(@"(?<author>[A-Za-zА-Яа-я]+\s[A-Za-zА-Яа-я]+)\s(?<reason>[A-Za-zа-яА-Я]+)\s\[\D+(?<number>\d+)\]\((?<url>.*)\)\s\((?<name>.*)\).*\[.*\].*");
            var match = re.Match(request.Message.Markdown);
            var author = match.Groups["author"]?.Value;
            var buildUrl = match.Groups["url"]?.Value;
            var number = match.Groups["number"]?.Value;
            var name = match.Groups["name"]?.Value;
            var project = request.Resource.Repository.Project.Name;

            if (!File.Exists(templateFileName))
                throw new ArgumentException($"Template file does not exists: '{templateFileName}'");

            var template = File.ReadAllText(templateFileName);
            var text = template.ReplaceAll(new[] {
                ("{author}", author),
                ("{url}", buildUrl),
                ("{number}", number),
                ("{name}", name),
                ("{project}", project),
                ("{branch}", branch),
                ("{destination}", destination),
            });

            Text = text;
        }
    }
}