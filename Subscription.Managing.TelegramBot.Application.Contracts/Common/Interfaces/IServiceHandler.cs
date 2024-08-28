namespace Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;

public interface IServiceHandler
{
    Task HandleServiceAction(CallbackQuery callbackQuery, int serviceId);
}
