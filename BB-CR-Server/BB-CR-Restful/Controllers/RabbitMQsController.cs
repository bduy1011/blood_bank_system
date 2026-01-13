using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace BB.CR.Rest.Controllers
{
#if DEBUG
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQsController(IPublishEndpoint _publishEndpoint) : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint = _publishEndpoint;

        [HttpGet("{param}")]
        public async Task<IActionResult> PostAsync(string param)
        {
            await publishEndpoint.Publish<string>(param).ConfigureAwait(false);
            return Ok();
        }
    }
#endif
}
