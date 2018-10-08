using TfsNotifications.Infrastructure.Options;
using TfsNotifications.Models.Requests;

namespace TfsNotifications.Interfaces
{
    public interface IMapper
    {
        TransmittionRequest Map(Notifications.Endpoint endpoint, NotificationRequest request);

        TransmittionRequest Map(Notifications.Endpoint endpoint, WebHook request);
    }
}