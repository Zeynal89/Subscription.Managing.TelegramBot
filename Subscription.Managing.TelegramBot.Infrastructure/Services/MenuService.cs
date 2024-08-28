using Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;

namespace Subscription.Managing.TelegramBot.Infrastructure.Services
{
    public class MenuService : IMenuService
    {
        private readonly TelegramBotClient bot;
        private readonly IServiceScopeFactory _scopeFactory;

        public MenuService(TelegramBotClient bot, IServiceScopeFactory scopeFactory)
        {
            this.bot = bot;
            this._scopeFactory = scopeFactory;
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
            using var scope = _scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var services = await dbContext.Set<Service>().ToListAsync();

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
    }
}
