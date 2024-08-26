using Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;

namespace Subscription.Managing.TelegramBot.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Service> Services { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ServiceDetail> ServiceDetails { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<Service>()
            .HasKey(p => p.Id);

        builder.Entity<ServiceDetail>()
            .HasOne(s => s.Service)
            .WithMany(sd => sd.ServiceDetails)
            .HasForeignKey(fk => fk.ServiceId);

        builder.Entity<User>()
            .HasKey(u => u.Id);

        builder.Entity<UserSubscription>()
            .HasOne(s => s.User)
            .WithMany(sd => sd.UserSubscriptions)
            .HasForeignKey(fk => fk.UserId);

        builder.Entity<UserSubscription>()
            .HasOne(s => s.ServiceDetail)
            .WithMany(sd => sd.UserSubscriptions)
            .HasForeignKey(fk => fk.ServiceDetailId);
    }
}
