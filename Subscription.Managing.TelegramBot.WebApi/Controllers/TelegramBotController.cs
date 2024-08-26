namespace Subscription.Managing.TelegramBot.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TelegramBotController : ControllerBase
{
    private readonly ITelegramBotService _telegramBotService;

    public TelegramBotController(ITelegramBotService telegramBotService)
    {
        _telegramBotService = telegramBotService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
}