namespace Subscription.Managing.TelegramBot.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceDetailController : ControllerBase
    {
        private readonly IMediator mediator;

        public ServiceDetailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataSource = await mediator.Send(new GetlAllServiceDetailsQuery());
            return Ok(dataSource.AsQueryable());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetforTelegram(int id)
        {
            return Ok(await mediator.Send(new GetServiceDetailQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateServiceDetailCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(uint id, [FromBody] UpdateServiceDetailCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest($"Check request: {id} not equals {command.Id}");
            }
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await mediator.Send(new DeleteServiceDetailCommand(id));
            return NoContent();
        }
    }
}