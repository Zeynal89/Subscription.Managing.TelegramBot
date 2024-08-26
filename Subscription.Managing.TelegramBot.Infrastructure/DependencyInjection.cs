using Subscription.Managing.TelegramBot.Infrastructure.Services;

namespace Subscription.Managing.TelegramBot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlite(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();

        var token = configuration["TelegramBot:Token"];
        services.AddSingleton<ITelegramBotService>(new TelegramBotService(token));

        services.AddSingleton(TimeProvider.System);
        return services;
    }
}

