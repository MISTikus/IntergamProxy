using RestSharp;
using System.Threading.Tasks;
using TfsNotifications.Interfaces;
using TfsNotifications.Models.Requests;
using TfsNotifications.Models.Responses;

namespace TfsNotifications.Transmisson
{
    public class NotificationTransmitter : INotificationTransmitter
    {
        public async Task<TransmissionResponse> Send(TransmittionRequest request)
        {
            var client = new RestClient(request.Url);
            var req = new RestRequest(Method.POST);
            req.AddParameter("application/json", request.Text, ParameterType.RequestBody);

            var resp = await client.ExecuteAsync(req);
            return new TransmissionResponse
            {
                IsSuccessful = (int) resp.StatusCode < 400,
                Error = resp.Content
            };
        }
    }

    public static class RestClientExtensions
    {
        public static async Task<RestResponse> ExecuteAsync(this RestClient client, RestRequest request)
        {
            var taskCompletion = new TaskCompletionSource<IRestResponse>();
            var handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));
            return (RestResponse) (await taskCompletion.Task);
        }
    }
}