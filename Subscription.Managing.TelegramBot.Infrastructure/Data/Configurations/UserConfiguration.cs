namespace Subscription.Managing.TelegramBot.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<Domain.Entities.User>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.User> builder)
    {
        builder.Property(t => t.Username).IsRequired();
        builder.HasIndex(t => t.Username).IsUnique();
    }
}
