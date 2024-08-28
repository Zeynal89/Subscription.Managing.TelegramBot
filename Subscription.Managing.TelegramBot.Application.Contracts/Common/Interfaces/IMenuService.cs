namespace Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;

public interface IMenuService
{
    Task ShowMainMenu(Chat chat);
    Task ServicesCallback(CallbackQuery callbackQuery);
}
