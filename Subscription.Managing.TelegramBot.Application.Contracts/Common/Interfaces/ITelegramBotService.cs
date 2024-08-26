namespace Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;

public interface ITelegramBotService
{
    Task SendMessageAsync(long chatId, string message);
}
