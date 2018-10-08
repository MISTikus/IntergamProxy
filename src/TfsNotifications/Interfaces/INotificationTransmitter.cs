using System.Threading.Tasks;
using TfsNotifications.Models.Requests;
using TfsNotifications.Models.Responses;

namespace TfsNotifications.Interfaces
{
    public interface INotificationTransmitter
    {
        Task<TransmissionResponse> Send(TransmittionRequest request);
    }
}