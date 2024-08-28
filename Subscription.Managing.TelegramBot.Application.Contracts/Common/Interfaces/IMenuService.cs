namespace Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;

public interface IMenuService
{
    Task OnMessage(Message msg, UpdateType type);
    Task ShowMainMenu(Chat chat);
    Task ServicesCallback(CallbackQuery callbackQuery);
}
