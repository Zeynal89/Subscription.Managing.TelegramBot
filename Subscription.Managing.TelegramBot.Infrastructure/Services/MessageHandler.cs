namespace Subscription.Managing.TelegramBot.Infrastructure.Services;

public class MessageHandler : IMessageHandler
{
    private readonly IMenuService menuService;

    public MessageHandler(IMenuService menuService)
    {
        this.menuService = menuService;
    }

    public async Task OnMessage(Message msg, UpdateType type)
    {
        await menuService.ShowMainMenu(msg.Chat);
    }
}
