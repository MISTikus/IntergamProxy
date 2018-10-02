using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TfsNotifications.Extensions;

namespace TfsNotifications.Models.Requests
{
    public class PullRequestApprovedTransmittionRequest : TransmittionRequest
    {
        private const string approveAction = "утвердили";
        private const string waitAction = "ожидает действие автора в";
        private const string remove = "поставили оценку";
        private const string reject = "отклонили";
        private const string suggestions = "код утвержден с предложениями в";

        public PullRequestApprovedTransmittionRequest(string url, string templateFileName, NotificationRequest request) : base(url)
        {
            var data = request.Attachments.First();

            var re = new Regex(@"(?<author>[А-Яа-яA-Za-z]+\s[А-Яа-яA-Za-z]+)\W?\s(?<action>.*)\s<(?<url>[^|]+)\|[\D]*(?<number>\d+)>\s\((?<name>.*)\).*\|(?<project>\w+)>");
            var match = re.Match(data.PreText);
            var author = match.Groups["author"]?.Value;
            var action = match.Groups["action"]?.Value;
            var buildUrl = match.Groups["url"]?.Value;
            var number = match.Groups["number"]?.Value;
            var name = match.Groups["name"]?.Value;
            var project = match.Groups["project"]?.Value;
            var actionData = GetAction(action);

            if (!File.Exists(templateFileName))
                throw new ArgumentException($"Template file does not exists: '{templateFileName}'");

            var template = File.ReadAllText(templateFileName);
            var text = template.ReplaceAll(new[] {
                ("{author}", author),
                ("{action}", actionData.action),
                ("{icon}", actionData.icon),
                ("{url}", buildUrl),
                ("{number}", number),
                ("{name}", name),
                ("{project}", project)
            });

            Text = text;
        }

        public PullRequestApprovedTransmittionRequest(string url, string templateFileName, WebHook request) : base(url)
        {
            var re = new Regex(@"(?<author>[A-Za-zА-Яа-я]+\s[A-Za-zА-Яа-я]+)\W?\s(?<action>.*)\s\[\D+(?<number>\d+)\]\((?<url>.*)\)\s\((?<name>.*)\).*\[.*\].*");
            var match = re.Match(request.Message.Markdown);
            var author = match.Groups["author"]?.Value;
            var action = match.Groups["action"]?.Value;
            var buildUrl = match.Groups["url"]?.Value;
            var number = match.Groups["number"]?.Value;
            var name = match.Groups["name"]?.Value;
            var project = request.Resource.Repository.Project.Name;
            var actionData = GetAction(action);

            if (!File.Exists(templateFileName))
                throw new ArgumentException($"Template file does not exists: '{templateFileName}'");

            var template = File.ReadAllText(templateFileName);
            var text = template.ReplaceAll(new[] {
                ("{author}", author),
                ("{action}", actionData.action),
                ("{icon}", actionData.icon),
                ("{url}", buildUrl),
                ("{number}", number),
                ("{name}", name),
                ("{project}", project)
            });

            Text = text;
        }

        private (string action, string icon) GetAction(string action)
        {
            switch (action)
            {
                case approveAction:
                    return ("approved", "✔️");

                case waitAction:
                    return ("waiting for author on", "❔");

                case reject:
                    return ("rejected", "❌");

                case remove:
                    return ("removed approve from", "➖");

                case suggestions:
                    return ("approved with suggestions", "✔️");

                default:
                    return ("approved", "✔️");
            }
        }
    }
}