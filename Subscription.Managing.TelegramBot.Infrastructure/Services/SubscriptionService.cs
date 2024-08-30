namespace Subscription.Managing.TelegramBot.Infrastructure.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly TelegramBotClient bot;
    private readonly IServiceScopeFactory _scopeFactory;

    public SubscriptionService(TelegramBotClient bot, IServiceScopeFactory scopeFactory)
    {
        this.bot = bot;
        _scopeFactory = scopeFactory;
    }

    public async Task HandleSubscriptionAction(CallbackQuery callbackQuery, string callbackData)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var (action, serviceDetailId, oldServiceDetailId) = ParseSubscriptionCallbackData(callbackData);

        var serviceDetail = await dbContext.Set<ServiceDetail>().FirstOrDefaultAsync(p => p.Id == serviceDetailId);
        var responseMessage = await ProcessSubscriptionAction(action, callbackQuery, serviceDetailId, oldServiceDetailId, serviceDetail, dbContext);

        await bot.AnswerCallbackQueryAsync(callbackQuery.Id, responseMessage);
    }

    private (string action, int serviceDetailId, int oldServiceDetailId) ParseSubscriptionCallbackData(string callbackData)
    {
        var parts = callbackData.Split('_');
        var action = parts[0];
        var serviceDetailId = int.Parse(parts[1]);
        var oldServiceDetailId = parts.Length > 2 ? int.Parse(parts[2]) : 0;
        return (action, serviceDetailId, oldServiceDetailId);
    }

    private async Task<string> ProcessSubscriptionAction(string action, CallbackQuery callbackQuery, int serviceDetailId, int oldServiceDetailId, ServiceDetail serviceDetail, ApplicationDbContext dbContext)
    {
        var userId = callbackQuery.From.Id;
        switch (action)
        {
            case "resume":
                await ResumeSubscription(userId, serviceDetailId, dbContext);
                await ShowServiceDetails(callbackQuery, serviceDetail.ServiceId, dbContext);
                return "Подписка возобновлена.";

            case "stop":
                await StopSubscription(userId, serviceDetailId, dbContext);
                return "Подписка остановлена.";

            case "change":
                await ShowServiceDetails(callbackQuery, serviceDetail.ServiceId, dbContext, true, serviceDetail.Id);
                return "Изменение подписки.";

            case "isChanged":
                await ChangeSubscription(userId, serviceDetailId, oldServiceDetailId, dbContext);
                return "Подписка изменена.";

            default:
                return "Неизвестное действие.";
        }
    }

    private async Task ResumeSubscription(long userId, int serviceDetailId, ApplicationDbContext dbContext)
    {
        var userSubscription = await dbContext.Set<UserSubscription>()
            .FirstOrDefaultAsync(y => y.UserId == userId && y.ServiceDetailId == serviceDetailId && y.EndDate >= DateTime.Now);
        userSubscription?.ResumeSubscriptionStatus();
        await dbContext.SaveChangesAsync();
    }

    private async Task StopSubscription(long userId, int serviceDetailId, ApplicationDbContext dbContext)
    {
        var userSubscription = await dbContext.Set<UserSubscription>()
            .FirstOrDefaultAsync(y => y.UserId == userId && y.ServiceDetailId == serviceDetailId && y.EndDate >= DateTime.Now);
        userSubscription?.StopSubscriptionStatus();
        await dbContext.SaveChangesAsync();
    }

    private async Task ChangeSubscription(long userId, int serviceDetailId, int oldServiceDetailId, ApplicationDbContext dbContext)
    {
        var userSubscription = await dbContext.Set<UserSubscription>()
            .Include(p => p.ServiceDetail)
            .ThenInclude(p => p.Service)
            .FirstOrDefaultAsync(y => y.UserId == userId && y.ServiceDetailId == oldServiceDetailId && y.EndDate >= DateTime.Now);

        var newServiceDetail = await dbContext.Set<ServiceDetail>().FirstOrDefaultAsync(p => p.Id == serviceDetailId);
        userSubscription.UpdateSubscription(newServiceDetail.Id, newServiceDetail.Duration.GetEndDate());
        dbContext.Set<UserSubscription>().Update(userSubscription);
        await dbContext.SaveChangesAsync();
    }

    private async Task ShowServiceDetails(CallbackQuery callbackQuery, int serviceId, ApplicationDbContext dbContext, bool isChanged = false, int oldServiceDetailId = default)
    {
        var service = await dbContext.Set<Service>().FirstOrDefaultAsync(p => p.Id == serviceId);
        var servicesDetail = dbContext.Set<ServiceDetail>().Where(sd => sd.ServiceId == serviceId).ToList();

        var detailMessage = $"Названия: {service.Name}\nОписания: {service.Description}";
        var mainMenuMarkup = CreateServiceDetailsMarkup(servicesDetail, isChanged, oldServiceDetailId);

        await bot.SendTextMessageAsync(callbackQuery.Message.Chat, detailMessage, replyMarkup: mainMenuMarkup);
    }

    private InlineKeyboardMarkup CreateServiceDetailsMarkup(List<ServiceDetail> servicesDetail, bool isChanged, int oldServiceDetailId)
    {
        var markup = new InlineKeyboardMarkup();

        foreach (var serviceDetail in servicesDetail.Where(p => p.Id != oldServiceDetailId))
        {
            var duration = ((DescriptionAttribute)serviceDetail.Duration.GetType().GetField(serviceDetail.Duration.ToString()).GetCustomAttribute(typeof(DescriptionAttribute)))?.Description;
            var detailButtons = $"Период: {duration}, Цена: ${serviceDetail.Cost:00}";
            var buttonCallbackData = isChanged ? $"isChanged_{serviceDetail.Id}_{oldServiceDetailId}" : serviceDetail.Id.ToString();

            markup.AddNewRow();
            markup.AddButton(detailButtons, buttonCallbackData);
        }

        markup.AddNewRow();
        markup.AddButton("Назад");

        return markup;
    }
}