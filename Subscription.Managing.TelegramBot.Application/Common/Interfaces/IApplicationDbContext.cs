namespace Subscription.Managing.TelegramBot.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    /// <summary>
    /// Установка Quary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}
