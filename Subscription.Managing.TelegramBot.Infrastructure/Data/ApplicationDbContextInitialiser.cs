using Subscription.Managing.TelegramBot.Domain.Shared.Enums;

namespace Subscription.Managing.TelegramBot.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.Services.Any())
        {
            _context.Services.AddRange(
                 new Service
                 {
                     Id = 1,
                     Name = "c# tutorial",
                     Description = "Teaching c# for beginners",
                     Status = ServiceStatus.Active,
                     EndDate = DateTime.Now.AddYears(2),
                     BotLink = "https://t.me/ExampleBot1"
                 },
            new Service
            {
                Id = 2,
                Name = "c# advanced",
                Description = "Teaching c# for developers",
                Status = ServiceStatus.Active,
                EndDate = DateTime.Now.AddYears(2),
                BotLink = "https://t.me/ExampleBot2"
            },
            new Service
            {
                Id = 3,
                Name = "c# from zero to hero",
                Description = "Teaching c# for beginners and masters",
                Status = ServiceStatus.Active,
                EndDate = DateTime.Now.AddYears(2),
                BotLink = "https://t.me/ExampleBot3"
            });
            await _context.SaveChangesAsync();

            _context.ServiceDetails.AddRange(
new ServiceDetail
{
    Id = 1,
    Duration = Duration.Week,
    Cost = 100.56,
    ServiceId = 1,
},
            new ServiceDetail
            {
                Id = 2,
                Duration = Duration.TwoWeek,
                Cost = 150,
                ServiceId = 1,
            },
            new ServiceDetail
            {
                Id = 3,
                Duration = Duration.Month,
                Cost = 200,
                ServiceId = 1,
            },
             new ServiceDetail
             {
                 Id = 4,
                 Duration = Duration.ThreeMonth,
                 Cost = 250,
                 ServiceId = 1,
             },
            new ServiceDetail
            {
                Id = 5,
                Duration = Duration.SixMonth,
                Cost = 300,
                ServiceId = 1,
            },
            new ServiceDetail
            {
                Id = 6,
                Duration = Duration.Year,
                Cost = 350,
                ServiceId = 1,
            },

            new ServiceDetail
            {
                Id = 7,
                Duration = Duration.Week,
                Cost = 10,
                ServiceId = 2,
            },
            new ServiceDetail
            {
                Id = 8,
                Duration = Duration.TwoWeek,
                Cost = 20,
                ServiceId = 2,
            },
            new ServiceDetail
            {
                Id = 9,
                Duration = Duration.Month,
                Cost = 30,
                ServiceId = 2,
            },
             new ServiceDetail
             {
                 Id = 10,
                 Duration = Duration.ThreeMonth,
                 Cost = 40,
                 ServiceId = 2,
             },
            new ServiceDetail
            {
                Id = 11,
                Duration = Duration.SixMonth,
                Cost = 50,
                ServiceId = 2,
            },
            new ServiceDetail
            {
                Id = 12,
                Duration = Duration.Year,
                Cost = 60,
                ServiceId = 2,
            },

            new ServiceDetail
            {
                Id = 13,
                Duration = Duration.Week,
                Cost = 70,
                ServiceId = 3,
            },
            new ServiceDetail
            {
                Id = 14,
                Duration = Duration.TwoWeek,
                Cost = 80,
                ServiceId = 3,
            },
            new ServiceDetail
            {
                Id = 15,
                Duration = Duration.Month,
                Cost = 30,
                ServiceId = 3,
            },
             new ServiceDetail
             {
                 Id = 16,
                 Duration = Duration.ThreeMonth,
                 Cost = 40,
                 ServiceId = 3,
             },
            new ServiceDetail
            {
                Id = 17,
                Duration = Duration.SixMonth,
                Cost = 50,
                ServiceId = 3,
            },
            new ServiceDetail
            {
                Id = 18,
                Duration = Duration.Year,
                Cost = 60,
                ServiceId = 3,
            });
            await _context.SaveChangesAsync();
        }
    }
}
