namespace Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;

public interface ICallbackHandler
{
    Task OnUpdate(Update update);
}
