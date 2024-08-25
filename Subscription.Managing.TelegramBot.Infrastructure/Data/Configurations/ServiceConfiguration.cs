namespace Subscription.Managing.TelegramBot.Infrastructure.Data.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(t => t.Name).IsUnique();

        builder.Property(t => t.Description)
            .HasMaxLength(500)
            .IsRequired();
    }
}
