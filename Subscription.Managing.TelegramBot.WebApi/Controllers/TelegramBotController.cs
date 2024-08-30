namespace Subscription.Managing.TelegramBot.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TelegramBotController : ControllerBase
{
    private readonly ITelegramBotService telegramBotService;

    public TelegramBotController(ITelegramBotService telegramBotService)
    {
        this.telegramBotService = telegramBotService;
    }

    [HttpGet]
    public async Task<IActionResult> StartAsync()
    {
        await telegramBotService.StartAsync();
        return Ok();
    }
}