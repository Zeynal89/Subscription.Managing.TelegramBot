namespace Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;

public interface ITelegramBotService
{
    Task StartAsync();
    Task SendMessageAsync(Chat chat, string message, IReplyMarkup replyMarkup);
}
