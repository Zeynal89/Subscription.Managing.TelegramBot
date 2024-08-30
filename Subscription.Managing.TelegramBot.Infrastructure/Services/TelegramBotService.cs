namespace Subscription.Managing.TelegramBot.Infrastructure.Services;

public class TelegramBotService : ITelegramBotService
{
    private readonly TelegramBotClient bot;
    private readonly ICallbackHandler callbackHandler;
    private readonly IMenuService menuService;

    public TelegramBotService(TelegramBotClient bot, ICallbackHandler callbackHandler, IMenuService menuService)
    {
        this.bot = bot;
        this.callbackHandler = callbackHandler;
        this.menuService = menuService;

        bot.OnMessage += this.menuService.OnMessage;
        bot.OnUpdate += this.callbackHandler.OnUpdate;
    }

    public async Task StartAsync()
    {
        await bot.DropPendingUpdatesAsync();
    }
}
