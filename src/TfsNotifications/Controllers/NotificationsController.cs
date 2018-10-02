using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TfsNotifications.Interfaces;

namespace TfsNotifications.Controllers
{
    [Route("notify")]
    [Produces("application/json")]
    public class NotificationsController : Controller
    {
        private readonly Dictionary<string, Infrastructure.Options.Notifications.Endpoint> options;
        private readonly IMapper mapper;
        private readonly INotificationTransmitter transmitter;

        public NotificationsController(IOptions<Infrastructure.Options.Notifications> options, IMapper mapper, INotificationTransmitter transmitter)
        {
            this.options = options.Value.Endpoints.ToDictionary(k => k.Name.ToLower(), v => v);
            this.mapper = mapper;
            this.transmitter = transmitter;
        }

        [HttpPost, Route("slack/{endpoint}")]
        public async Task<IActionResult> NotifySlack(
            [FromRoute]string endpoint,
            [FromBody]Models.Requests.NotificationRequest request)
        {
            var mappedRequest = mapper.Map(options[endpoint.ToLower()], request);
            var response = await transmitter.Send(mappedRequest);
            if (!response.IsSuccessful)
                return BadRequest(response.Error);
            return Ok();
        }

        [HttpPost, Route("webhook/{endpoint}")]
        public async Task<IActionResult> NotifyWebHook(
            [FromRoute]string endpoint,
            [FromBody]Models.Requests.WebHook request)
        {
            var mappedRequest = mapper.Map(options[endpoint.ToLower()], request);
            var response = await transmitter.Send(mappedRequest);
            if (!response.IsSuccessful)
                return BadRequest(response.Error);
            return Ok();
        }
    }
}