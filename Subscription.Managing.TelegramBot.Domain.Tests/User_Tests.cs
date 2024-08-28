namespace Subscription.Managing.TelegramBot.Domain.Tests;

public class User_Tests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        long id = 1;
        string firstName = "John";
        string lastName = "Doe";
        string username = "johndoe";

        // Act
        var user = new User(id, firstName, lastName, username);

        // Assert
        Assert.Equal(id, user.Id);
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
        Assert.Equal(username, user.Username);
    }

    [Fact]
    public void UserSubscriptions_ShouldInitializeAsEmptyList()
    {
        // Arrange
        long id = 1;
        string firstName = "John";
        string lastName = "Doe";
        string username = "johndoe";

        // Act
        var user = new User(id, firstName, lastName, username);

        // Assert
        Assert.Null(user.UserSubscriptions);
    }
}