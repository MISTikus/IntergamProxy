using System.Collections.Generic;

namespace TfsNotifications.Infrastructure.Options
{
    public class Notifications
    {
        public IEnumerable<Endpoint> Endpoints { get; set; }

        public class Endpoint
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public string Template { get; set; }
            public EndpointType Type { get; set; }
        }

        public enum EndpointType
        {
            Build,
            PullRequestCreated,
            PullRequestApproved
        }
    }
}