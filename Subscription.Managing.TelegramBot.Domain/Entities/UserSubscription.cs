namespace Subscription.Managing.TelegramBot.Domain.Entities;

public class UserSubscription
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public int ServiceDetailId { get; set; }
    public ServiceDetail ServiceDetail { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public UserSubscriptionStatus UserSubscriptionStatus { get; set; }

    public void UpdateSubscription(int serviceDetailId, DateTime endDate)
    {
        ServiceDetailId = serviceDetailId;
        EndDate = endDate;
    }

    public void ChangeUserSubscriptionStatus()
    {
        UserSubscriptionStatus = UserSubscriptionStatus == UserSubscriptionStatus.Active ? UserSubscriptionStatus.Stopped : UserSubscriptionStatus.Active;
    }
}
