using System.Text;
using Telegram.Bot;

namespace Subscription.Managing.TelegramBot.Infrastructure.Services;

public class MenuService : IMenuService
{
    private readonly TelegramBotClient bot;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly IMapper mapper;

    public MenuService(TelegramBotClient bot, IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        this.bot = bot;
        this.scopeFactory = scopeFactory;
        this.mapper = mapper;
    }

    public async Task OnMessage(Message msg, UpdateType type)
    {
        await ShowMainMenu(msg.Chat);
    }

    public async Task ShowMainMenu(Chat chat)
    {
        var mainMenuMarkup = new InlineKeyboardMarkup()
            .AddNewRow()
                .AddButton("Сервисы")
                .AddButton("Личный кабинет");
        await bot.SendTextMessageAsync(chat, "Главное меню:", replyMarkup: mainMenuMarkup);
    }

    public async Task ServicesCallback(CallbackQuery callbackQuery)
    {
        using var scope = scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var services = await dbContext.Set<Service>().Where(p => p.Status == ServiceStatus.Active).ToListAsync();

        var mainMenuMarkup = new InlineKeyboardMarkup();

        foreach (var item in services)
        {
            mainMenuMarkup.AddNewRow();
            mainMenuMarkup.AddButton(item.Name, item.Id.ToString());
        }

        mainMenuMarkup.AddNewRow();
        mainMenuMarkup.AddButton("Назад");

        await bot.SendTextMessageAsync(callbackQuery.Message!.Chat, $"{callbackQuery.Data}:", replyMarkup: mainMenuMarkup);
    }

    public async Task ShowPersonalAccountMenu(CallbackQuery callbackQuery)
    {
        using var scope = scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userSubscriptions = await dbContext.Set<UserSubscription>()
                           .Include(p => p.ServiceDetail)
                           .ThenInclude(p => p.Service)
                           .Where(p => p.UserId == callbackQuery.From.Id && p.EndDate >= DateTime.Now).ToListAsync();

        var userSubscriptionsDto = mapper.Map<List<UserSubscriptionDto>>(userSubscriptions);

        StringBuilder personalAccountInfo = new StringBuilder();

        int count = 0;
        foreach (var userSubscription in userSubscriptionsDto)
        {
            personalAccountInfo.AppendLine($"{++count}. Названия: {userSubscription.ServiceDetail.ServiceDto.Name}\n" +
                                           $"Ссылка на Telegram бота: {userSubscription.ServiceDetail.ServiceDto.BotLink}\n" +
                                           $"Текущий статус: {userSubscription.UserSubscriptionStatus}\n" +
                                           $"Дата окончания подписки {userSubscription.EndDate}");
        }

        await bot.SendTextMessageAsync(callbackQuery.Message.Chat, personalAccountInfo.ToString());
    }
}
