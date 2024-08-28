namespace Subscription.Managing.TelegramBot.Infrastructure.Services;

public class CallbackHandler : ICallbackHandler
{
    private readonly IMenuService menuService;
    private readonly ISubscriptionService subscriptionService;
    private readonly IServiceHandler serviceHandler;

    public CallbackHandler(
        IMenuService menuService,
        ISubscriptionService subscriptionService,
        IServiceHandler serviceHandler)
    {
        this.menuService = menuService;
        this.subscriptionService = subscriptionService;
        this.serviceHandler = serviceHandler;
    }

    public async Task OnUpdate(Update update)
    {
        if (update.CallbackQuery != null)
        {
            await HandleCallbackQuery(update.CallbackQuery);
        }
    }

    private async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        var callbackData = callbackQuery.Data;

        if (callbackQuery.Data == "Личный кабинет")
        {
            //await ShowPersonalAccountMenu(callbackQuery);
        }
        else if (callbackQuery.Data == "Назад")
        {
            await menuService.ServicesCallback(callbackQuery);
        }
        else if (callbackData.Contains('_'))
        {
            await subscriptionService.HandleSubscriptionAction(callbackQuery, callbackData);
        }
        else if (int.TryParse(callbackData, out int serviceId))
        {
            await serviceHandler.HandleServiceAction(callbackQuery, serviceId);
        }
        else
        {
            await menuService.ServicesCallback(callbackQuery);
        }
    }
}
