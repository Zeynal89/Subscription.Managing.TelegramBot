namespace Subscription.Managing.TelegramBot.Infrastructure.Services;

public class TelegramBotService : ITelegramBotService
{
    private readonly TelegramBotClient bot;
    private readonly IMessageHandler messageHandler;
    private readonly ICallbackHandler callbackHandler;

    public TelegramBotService(TelegramBotClient bot, IMessageHandler messageHandler, ICallbackHandler callbackHandler)
    {
        this.bot = bot;
        this.messageHandler = messageHandler;
        this.callbackHandler = callbackHandler;

        bot.OnMessage += this.messageHandler.OnMessage;
        bot.OnUpdate += this.callbackHandler.OnUpdate;
    }

    public async Task StartAsync()
    {
        await bot.DropPendingUpdatesAsync();
    }

    public async Task SendMessageAsync(Chat chat, string message, IReplyMarkup replyMarkup)
    {
        await bot.SendTextMessageAsync(chat, message, replyMarkup: replyMarkup);
    }
}
