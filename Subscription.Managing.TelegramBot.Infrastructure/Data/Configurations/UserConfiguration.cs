namespace Subscription.Managing.TelegramBot.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(t => t.Username).IsRequired();
        builder.HasIndex(t => t.Username).IsUnique();
    }
}
