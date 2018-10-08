using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TfsNotifications.Models.Requests
{
    public class WebHook
    {
        public Guid Id { get; set; }
        public Guid SubscriptionId { get; set; }
        public int NotificationId { get; set; }
        public string EventType { get; set; }
        public string PublisherId { get; set; }
        public WebHookMessage DetailedMessage { get; set; }
        public WebHookMessage Message { get; set; }
        public WebHookResource Resource { get; set; }
        public TfsUser CreatedBy { get; set; }

        public class WebHookMessage
        {
            public string Markdown { get; set; }
        }

        public class WebHookResource
        {
            public string Uri { get; set; }
            public int Id { get; set; }
            public string BuildNumber { get; set; }
            public string Url { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime FinishTime { get; set; }
            public string Reason { get; set; }
            public string Status { get; set; }
            public TfsUser LastChangedBy { get; set; }
            public bool RetainIndefinitely { get; set; }
            public BuildDefenition Definition { get; set; }
            public IEnumerable<BuildRequest> Requests { get; set; }

            // PR
            public int PullRequestId { get; set; }

            public string Title { get; set; }

            [JsonProperty(PropertyName = "sourceRefName")]
            public string SourceBranch { get; set; }

            [JsonProperty(PropertyName = "targetRefName")]
            public string TargetBranch { get; set; }

            public WebHookRepository Repository { get; set; }
        }

        public class TfsUser
        {
            public Guid Id { get; set; }
            public string DisplayName { get; set; }
            public string UniqueName { get; set; }
            public string Url { get; set; }
            public string ImageUrl { get; set; }
        }

        public class BuildDefenition
        {
            public int Id { get; set; }
            public string DefinitionType { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
        }

        public class BuildRequest
        {
            public int Id { get; set; }
            public string Url { get; set; }
            public TfsUser RequestedFor { get; set; }
        }

        public class WebHookRepository
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string RemoteUrl { get; set; }
            public string Url { get; set; }
            public WebHookProject Project { get; set; }
        }

        public class WebHookProject
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}