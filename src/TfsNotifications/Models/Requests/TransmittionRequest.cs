namespace TfsNotifications.Models.Requests
{
    public class TransmittionRequest
    {
        public string Url { get; set; }
        public string Text { get; set; }

        public TransmittionRequest(string url) => Url = url;
    }
}