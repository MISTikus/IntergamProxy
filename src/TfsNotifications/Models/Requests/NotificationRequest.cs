using Newtonsoft.Json;
using System.Collections.Generic;

namespace TfsNotifications.Models.Requests
{
    public class NotificationRequest
    {
        public IEnumerable<Notification> Attachments { get; set; }

        public class Notification
        {
            public string Color { get; set; }

            public IEnumerable<Field> Fields { get; set; }

            [JsonProperty(PropertyName = "pretext")]
            public string PreText { get; set; }

            [JsonProperty(PropertyName = "mrkdwn_in")]
            public IEnumerable<string> MarkdownArray { get; set; }

            public string Fallback { get; set; }

            public class Field
            {
                public string Title { get; set; }

                public string Value { get; set; }

                [JsonProperty(PropertyName = "short")]
                public bool IsShort { get; set; }
            }
        }
    }
}