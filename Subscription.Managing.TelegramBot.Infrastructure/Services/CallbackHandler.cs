namespace Subscription.Managing.TelegramBot.Infrastructure.Services
{
    public class CallbackHandler : ICallbackHandler
    {
        private readonly IMenuService menuService;
        private readonly IMapper mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly TelegramBotClient bot;

        public CallbackHandler(IMenuService menuService, TelegramBotClient bot, IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            this.menuService = menuService;
            this.bot = bot;
            this.mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public async Task OnUpdate(Update update)
        {
            if (update.CallbackQuery != null)
            {
                await OnCallbackQuery(update.CallbackQuery);
            }
        }

        private async Task OnCallbackQuery(CallbackQuery callbackQuery)
        {
            var userId = callbackQuery.From.Id;
            var callbackData = callbackQuery.Data;

            using var scope = _scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            switch (callbackData)
            {
                case "Личный кабинет":
                    //await HandlePersonalAccountCallback(callbackQuery);
                    break;

                case "Назад":
                    //await HandleBackCallback(callbackQuery);
                    break;

                default:
                    if (callbackData.Contains('_'))
                    {
                        await HandleSubscriptionAction(callbackQuery, callbackData, userId, dbContext);
                    }
                    else if (int.TryParse(callbackData, out int serviceId))
                    {
                        await HandleServiceAction(callbackQuery, serviceId, dbContext);
                    }
                    else
                    {
                        await menuService.ServicesCallback(callbackQuery);
                    }
                    break;
            }
        }


        private async Task HandleSubscriptionAction(CallbackQuery callbackQuery, string callbackData, long userId, ApplicationDbContext dbContext)
        {
            var (action, serviceDetailId, oldServiceDetailId) = ParseSubscriptionCallbackData(callbackData);

            var serviceDetail = await dbContext.Set<ServiceDetail>().FirstOrDefaultAsync(p => p.Id == serviceDetailId);
            var responseMessage = await ProcessSubscriptionAction(action, callbackQuery, serviceDetailId, oldServiceDetailId, userId, serviceDetail, dbContext);

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

        private async Task<string> ProcessSubscriptionAction(string action, CallbackQuery callbackQuery, int serviceDetailId, int oldServiceDetailId, long userId, ServiceDetail serviceDetail, ApplicationDbContext dbContext)
        {
            switch (action)
            {
                case "resume":
                    await ResumeSubscription(serviceDetailId, userId, dbContext);
                    await ShowServiceDetails(callbackQuery, serviceDetail.ServiceId, dbContext);
                    return "Подписка возобновлена.";

                case "stop":
                    await StopSubscription(serviceDetailId, userId, dbContext);
                    return "Подписка остановлена.";

                case "change":
                    await ShowServiceDetails(callbackQuery, serviceDetail.ServiceId, dbContext, true, serviceDetail.Id);
                    return "Изменение подписки.";

                case "isChanged":
                    await ChangeSubscription(serviceDetailId, oldServiceDetailId, userId, dbContext);
                    return "Подписка изменена.";

                default:
                    return "Неизвестное действие.";
            }
        }

        private async Task ResumeSubscription(int serviceDetailId, long userId, ApplicationDbContext dbContext)
        {
            var userSubscription = await dbContext.Set<UserSubscription>().FirstOrDefaultAsync(y => y.ServiceDetailId == serviceDetailId && y.UserId == userId && y.EndDate >= DateTime.Now);
            userSubscription?.ChangeUserSubscriptionStatus();
            await dbContext.SaveChangesAsync();
        }

        private async Task StopSubscription(int serviceDetailId, long userId, ApplicationDbContext dbContext)
        {
            var userSubscription = await dbContext.Set<UserSubscription>().FirstOrDefaultAsync(y => y.ServiceDetailId == serviceDetailId && y.UserId == userId && y.EndDate >= DateTime.Now);
            userSubscription?.ChangeUserSubscriptionStatus();
            await dbContext.SaveChangesAsync();
        }

        private async Task ChangeSubscription(int serviceDetailId, int oldServiceDetailId, long userId, ApplicationDbContext dbContext)
        {
            var userSubscription = await dbContext.Set<UserSubscription>()
                                                    .Include(p=>p.ServiceDetail)
                                                        .ThenInclude(p => p.Service)
                                                    .FirstOrDefaultAsync(y => y.ServiceDetailId == oldServiceDetailId && y.UserId == userId && y.EndDate >= DateTime.Now);

            var newServiceDetail = await dbContext.Set<ServiceDetail>().FirstOrDefaultAsync(p => p.Id == serviceDetailId);
            userSubscription.UpdateSubscription(newServiceDetail.Id, newServiceDetail.Duration.GetEndDate());
            dbContext.Set<UserSubscription>().Update(userSubscription);
            await dbContext.SaveChangesAsync();
        }

        private async Task HandleServiceAction(CallbackQuery callbackQuery, int serviceId, ApplicationDbContext dbContext)
        {
            var user = await GetOrCreateUser(callbackQuery.From, dbContext);

            if (callbackQuery.Message.Text == "Сервисы:")
            {
                var serviceDetail = await dbContext.Set<ServiceDetail>().FirstOrDefaultAsync(p => p.ServiceId == serviceId);
                var userSubscription = await dbContext.Set<UserSubscription>()
                                                .Include(p => p.ServiceDetail)
                                                    .ThenInclude(p=>p.Service)
                                                .FirstOrDefaultAsync(p => p.ServiceDetailId == serviceDetail.Id && p.UserId == user.Id && p.EndDate >= DateTime.Now);

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

        private async Task<Domain.Entities.User> GetOrCreateUser(Telegram.Bot.Types.User profile, ApplicationDbContext dbContext)
        {
            var user = await dbContext.Set<Domain.Entities.User>().FirstOrDefaultAsync(p => p.Username == profile.Username);
            if (user == null)
            {
                user = new Domain.Entities.User(profile.Id, profile.FirstName, profile.LastName, profile.Username);
                await dbContext.Set<Domain.Entities.User>().AddAsync(user);
                await dbContext.SaveChangesAsync();
            }
            return user;
        }

        private async Task ProcessServiceAction(CallbackQuery callbackQuery, int serviceId, Domain.Entities.User user, ApplicationDbContext dbContext)
        {
            var userSubscription = await dbContext.Set<UserSubscription>()
                                                    .Include(p => p.ServiceDetail)
                                                        .ThenInclude(p => p.Service)
                                                    .FirstOrDefaultAsync(p => p.ServiceDetailId == serviceId);
            if (userSubscription == null)
            {
                await CreateNewSubscription(serviceId, user, dbContext);
                await bot.AnswerCallbackQueryAsync(callbackQuery.Id, "Ваш платеж прошел успешно.");
            }

            var userSubscriptionDto = mapper.Map<UserSubscriptionDto>(userSubscription);
            await ShowServiceSubscriptionManagement(callbackQuery, userSubscriptionDto);
        }

        private async Task CreateNewSubscription(int serviceId, Domain.Entities.User user, ApplicationDbContext dbContext)
        {
            var serviceDetail = await dbContext.Set<ServiceDetail>().FirstOrDefaultAsync(p => p.Id == serviceId);
            var userSubscription = new UserSubscription
            {
                UserId = user.Id,
                ServiceDetailId = serviceId,
                StartDate = DateTime.Now,
                EndDate = serviceDetail.Duration.GetEndDate(),
                UserSubscriptionStatus = UserSubscriptionStatus.Active
            };
            await dbContext.Set<UserSubscription>().AddAsync(userSubscription);
            await dbContext.SaveChangesAsync();
        }

        public async Task ShowServiceDetails(CallbackQuery callbackQuery, int serviceId, ApplicationDbContext dbContext, bool isChanged = false, int oldServiceDetailId = default)
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

        async Task ShowServiceSubscriptionManagement(CallbackQuery callbackQuery, UserSubscriptionDto userSubscription)
        {
            var detailMessage = $"Названия: {userSubscription.ServiceDetail.ServiceDto.Name}\n" +
                                $"Описания: {userSubscription.ServiceDetail.ServiceDto.Description}\n" +
                                $"Ссылка на Telegram бота: {userSubscription.ServiceDetail.ServiceDto.BotLink}\n" +
                                $"Период подписки: {userSubscription.ServiceDetail.Duration}\n" +
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
    }
}
