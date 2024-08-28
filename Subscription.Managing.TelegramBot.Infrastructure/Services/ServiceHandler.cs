namespace Subscription.Managing.TelegramBot.Infrastructure.Services;

public class ServiceHandler : IServiceHandler
{
    private readonly TelegramBotClient bot;
    private readonly IMapper mapper;
    private readonly IServiceScopeFactory _scopeFactory;

    public ServiceHandler(
        TelegramBotClient bot,
        IMapper mapper,
        IServiceScopeFactory scopeFactory)
    {
        this.bot = bot;
        this.mapper = mapper;
        _scopeFactory = scopeFactory;
    }

    public async Task HandleServiceAction(CallbackQuery callbackQuery, int serviceId)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = await GetOrCreateUser(callbackQuery.From, dbContext);

        if (callbackQuery.Message.Text == "Сервисы:")
        {
            var serviceDetail = await dbContext.Set<ServiceDetail>().FirstOrDefaultAsync(p => p.ServiceId == serviceId);
            var userSubscription = await dbContext.Set<UserSubscription>()
                .Include(p => p.ServiceDetail)
                .ThenInclude(p => p.Service)
                .FirstOrDefaultAsync(p => p.ServiceDetailId == serviceDetail.Id && p.EndDate >= DateTime.Now);

            var userSubscriptionDto = mapper.Map<UserSubscriptionDto>(userSubscription);
            if (userSubscriptionDto != null)
            {
                await ShowServiceSubscriptionManagement(callbackQuery, userSubscriptionDto);
            }
            else
            {
                await ShowServiceDetails(callbackQuery, serviceId, dbContext);
            }
        }
        else
        {
            await ProcessServiceAction(callbackQuery, serviceId, user, dbContext);
        }
    }

    private async Task ProcessServiceAction(CallbackQuery callbackQuery, int serviceId, Domain.Entities.User user, ApplicationDbContext dbContext)
    {
        var userSubscription = await dbContext.Set<UserSubscription>()
            .Include(p => p.ServiceDetail)
            .ThenInclude(p => p.Service)
            .FirstOrDefaultAsync(p => p.ServiceDetailId == serviceId && p.EndDate >= DateTime.Now);

        if (userSubscription == null)
        {
            userSubscription = await CreateNewSubscription(serviceId, user, dbContext);
            await bot.AnswerCallbackQueryAsync(callbackQuery.Id, "Ваш платеж прошел успешно.");
        }

        var userSubscriptionDto = mapper.Map<UserSubscriptionDto>(userSubscription);
        await ShowServiceSubscriptionManagement(callbackQuery, userSubscriptionDto);
    }

    private async Task<UserSubscription> CreateNewSubscription(int serviceId, Domain.Entities.User user, ApplicationDbContext dbContext)
    {
        var serviceDetail = await dbContext.Set<ServiceDetail>().FirstOrDefaultAsync(p => p.Id == serviceId);
        var userSubscription = new UserSubscription(user.Id, serviceDetail.Id, serviceDetail.Duration.GetEndDate());
        
        await dbContext.Set<UserSubscription>().AddAsync(userSubscription);
        await dbContext.SaveChangesAsync();
        return userSubscription;
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
            var duration = GetDuration(serviceDetail.Duration);
            var detailButtons = $"Период: {duration}, Цена: ${serviceDetail.Cost:00}";
            var buttonCallbackData = isChanged ? $"isChanged_{serviceDetail.Id}_{oldServiceDetailId}" : serviceDetail.Id.ToString();

            markup.AddNewRow();
            markup.AddButton(detailButtons, buttonCallbackData);
        }

        markup.AddNewRow();
        markup.AddButton("Назад");

        return markup;
    }

    private async Task ShowServiceSubscriptionManagement(CallbackQuery callbackQuery, UserSubscriptionDto userSubscription)
    {
        var detailMessage = $"Названия: {userSubscription.ServiceDetail.ServiceDto.Name}\n" +
                            $"Описания: {userSubscription.ServiceDetail.ServiceDto.Description}\n" +
                            $"Ссылка на Telegram бота: {userSubscription.ServiceDetail.ServiceDto.BotLink}\n" +
                            $"Период подписки: {GetDuration(userSubscription.ServiceDetail.Duration)}\n" +
                            $"Цена: ${userSubscription.ServiceDetail.Cost:00}\n" +
                            $"Текущий статус: {userSubscription.UserSubscriptionStatus}\n" +
                            $"Дата начала подписки: {userSubscription.StartDate}\n" +
                            $"Дата следующей оплаты подписки {userSubscription.EndDate}";

        var mainMenuMarkup = CreateSubscriptionManagementMarkup(userSubscription.ServiceDetail.Id.ToString(), userSubscription.UserSubscriptionStatus);

        await bot.SendTextMessageAsync(callbackQuery.Message.Chat, detailMessage, replyMarkup: mainMenuMarkup);
    }

    private InlineKeyboardMarkup CreateSubscriptionManagementMarkup(string serviceDetailId, UserSubscriptionStatus status)
    {
        var markup = new InlineKeyboardMarkup();

        if (status == UserSubscriptionStatus.Stopped)
        {
            markup.AddNewRow();
            markup.AddButton("Возобновить", $"resume_{serviceDetailId}");
        }
        else
        {
            markup.AddNewRow();
            markup.AddButton("Остановить", $"stop_{serviceDetailId}");
            markup.AddNewRow();
            markup.AddButton("Изменить", $"change_{serviceDetailId}");
        }

        return markup;
    }

    private async Task<Domain.Entities.User> GetOrCreateUser(Telegram.Bot.Types.User profile, ApplicationDbContext dbContext)
    {
        var user = await dbContext.Set<Domain.Entities.User>()
            .FirstOrDefaultAsync(p => p.Username == profile.Username);

        if (user == null)
        {
            user = new Domain.Entities.User(profile.Id, profile.FirstName, profile.LastName, profile.Username);
            await dbContext.Set<Domain.Entities.User>().AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        return user;
    }

    private string GetDuration(Duration duration)
    {
        return ((DescriptionAttribute)duration.GetType().GetField(duration.ToString()).GetCustomAttribute(typeof(DescriptionAttribute)))?.Description;
    }
}