namespace Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;

public interface ISubscriptionService
{
    Task HandleSubscriptionAction(CallbackQuery callbackQuery, string callbackData);
}
