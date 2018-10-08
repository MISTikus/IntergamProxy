using System;
using System.Diagnostics;
using System.IO;
using TfsNotifications.Infrastructure.Options;
using TfsNotifications.Interfaces;
using TfsNotifications.Models.Requests;

namespace TfsNotifications.Transmisson
{
    public class Mapper : IMapper
    {
        public TransmittionRequest Map(Notifications.Endpoint endpoint, NotificationRequest request)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            var templateFileName = Path.GetFullPath(Path.Combine(pathToContentRoot, "Templates", endpoint.Template));

            switch (endpoint.Type)
            {
                case Notifications.EndpointType.Build:
                    return new BuildTransmittionRequest(endpoint.Url, templateFileName, request);

                case Notifications.EndpointType.PullRequestCreated:
                    return new PullRequestCreatedTransmittionRequest(endpoint.Url, templateFileName, request);

                case Notifications.EndpointType.PullRequestApproved:
                    return new PullRequestApprovedTransmittionRequest(endpoint.Url, templateFileName, request);

                default:
                    throw new ArgumentException($"No idea how is this happens... Type is '{endpoint.Type}'.");
            }
        }

        public TransmittionRequest Map(Notifications.Endpoint endpoint, WebHook request)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            var templateFileName = Path.GetFullPath(Path.Combine(pathToContentRoot, "Templates", endpoint.Template));

            switch (endpoint.Type)
            {
                case Notifications.EndpointType.Build:
                    return new BuildTransmittionRequest(endpoint.Url, templateFileName, request);

                case Notifications.EndpointType.PullRequestCreated:
                    return new PullRequestCreatedTransmittionRequest(endpoint.Url, templateFileName, request);

                case Notifications.EndpointType.PullRequestApproved:
                    return new PullRequestApprovedTransmittionRequest(endpoint.Url, templateFileName, request);

                default:
                    throw new ArgumentException($"No idea how is this happens... Type is '{endpoint.Type}'.");
            }
        }
    }
}