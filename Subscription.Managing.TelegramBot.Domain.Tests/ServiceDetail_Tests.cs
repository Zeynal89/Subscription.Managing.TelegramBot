
namespace Subscription.Managing.TelegramBot.Domain.Tests;

public class ServiceDetail_Tests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        string name = "Test Service";
        string description = "Service Description";
        var service = new Service
        {
            Name = name,
            Description = description,
            Status = ServiceStatus.Active,
            BotLink = "https://t.me/testbot"
        }; 

        var serviceDetail = new ServiceDetail
        {
            Duration = Duration.Week, 
            Cost = 9.99,
            Service = service
        };

        // Act & Assert
        Assert.Equal(Duration.Week, serviceDetail.Duration);
        Assert.Equal(9.99, serviceDetail.Cost);
        Assert.Equal(service, serviceDetail.Service);
    }

    [Fact]
    public void UserSubscriptions_ShouldInitializeAsEmptyList()
    {
        // Arrange
        string name = "Test Service";
        string description = "Service Description";
        var service = new Service
        {
            Name = name,
            Description = description,
            Status = ServiceStatus.Active,
            BotLink = "https://t.me/testbot"
        }; 

        var serviceDetail = new ServiceDetail
        {
            Service = service
        };

        // Act
        var userSubscriptions = serviceDetail.UserSubscriptions;

        // Assert
        Assert.Null(userSubscriptions);
    }
}