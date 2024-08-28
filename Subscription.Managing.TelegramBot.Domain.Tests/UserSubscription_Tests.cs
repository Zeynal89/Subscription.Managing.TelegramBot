namespace Subscription.Managing.TelegramBot.Domain.Tests;

public class UserSubscription_Tests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        long userId = 1;
        int serviceDetailId = 1;
        DateTime endDate = DateTime.Now.AddMonths(1);

        // Act
        var subscription = new UserSubscription(userId, serviceDetailId, endDate);

        // Assert
        Assert.Equal(userId, subscription.UserId);
        Assert.Equal(serviceDetailId, subscription.ServiceDetailId);
        Assert.Equal(endDate, subscription.EndDate);
        Assert.Equal(UserSubscriptionStatus.Active, subscription.UserSubscriptionStatus);
    }

    [Fact]
    public void UpdateSubscription_ShouldChangeServiceDetailIdAndEndDate()
    {
        // Arrange
        long userId = 1;
        int serviceDetailId = 1;
        DateTime endDate = DateTime.Now.AddMonths(1);
        var subscription = new UserSubscription(userId, serviceDetailId, endDate);

        // Act
        int newServiceDetailId = 2;
        DateTime newEndDate = DateTime.Now.AddMonths(2);
        subscription.UpdateSubscription(newServiceDetailId, newEndDate);

        // Assert
        Assert.Equal(newServiceDetailId, subscription.ServiceDetailId);
        Assert.Equal(newEndDate, subscription.EndDate);
    }

    [Fact]
    public void ChangeUserSubscriptionStatus_ShouldToggleStatus()
    {
        // Arrange
        long userId = 1;
        int serviceDetailId = 1;
        DateTime endDate = DateTime.Now.AddMonths(1);
        var subscription = new UserSubscription(userId, serviceDetailId, endDate);

        // Act
        subscription.ChangeUserSubscriptionStatus();

        // Assert
        Assert.Equal(UserSubscriptionStatus.Stopped, subscription.UserSubscriptionStatus);

        // Act again
        subscription.ChangeUserSubscriptionStatus();

        // Assert
        Assert.Equal(UserSubscriptionStatus.Active, subscription.UserSubscriptionStatus);
    }
}