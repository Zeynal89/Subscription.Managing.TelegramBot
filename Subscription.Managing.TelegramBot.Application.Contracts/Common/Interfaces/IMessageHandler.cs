namespace Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;

public interface IMessageHandler
{
    Task OnMessage(Message msg, UpdateType type);
}
