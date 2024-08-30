namespace Subscription.Managing.TelegramBot.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public List<UserSubscription>? UserSubscriptions { get; set; }

    public User(long id, string firstName, string lastName, string username)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
    }
}
