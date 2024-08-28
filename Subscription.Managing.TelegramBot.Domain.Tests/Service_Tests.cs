using System.Xml.Linq;

namespace Subscription.Managing.TelegramBot.Domain.Tests;

public class Service_Tests
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
            EndDate = DateTime.Now.AddMonths(1),
            BotLink = "https://t.me/testbot"
        };

        // Act & Assert
        Assert.Equal(name, service.Name);
        Assert.Equal(description, service.Description);
        Assert.Equal(ServiceStatus.Active, service.Status);
        Assert.Equal(DateTime.Now.AddMonths(1).Date, service.EndDate.Date);
        Assert.Equal("https://t.me/testbot", service.BotLink);
    }

    [Fact]
    public void ServiceDetails_ShouldInitializeAsEmptyList()
    {
        // Arrange
        string name = "Test Service";
        string description = "Service Description";

        var service = new Service
        {
            Name = name,
            Description = description,
            Status = ServiceStatus.Active, 
            EndDate = DateTime.Now.AddMonths(1),
            BotLink = "https://t.me/testbot"
        };

        // Act
        var serviceDetails = service.ServiceDetails;

        // Assert
        Assert.Null(serviceDetails);
    }
}