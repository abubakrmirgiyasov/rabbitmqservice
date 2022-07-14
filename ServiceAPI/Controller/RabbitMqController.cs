using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ServiceAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private readonly IMessageBusWrapper _bus;

        public RabbitMqController(IMessageBusWrapper bus)
        {
            _bus = bus;
        }

        [HttpGet("[action]/{id}")]
        public IActionResult Received(Guid id)
        {
            try
            {
                var result = _bus.Received(id);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("[action]")]
        public IActionResult Send(Service service)
        {
            try
            {
                var id = _bus.Send(service);

                return Ok(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
