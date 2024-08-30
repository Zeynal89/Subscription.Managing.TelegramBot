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

    public UserSubscription(long userId, int serviceDetailId, DateTime endDate)
    {
        UserId = userId;
        ServiceDetailId = serviceDetailId;
        StartDate = DateTime.Now;
        UserSubscriptionStatus = UserSubscriptionStatus.Active;
        SetEndDate(endDate);
    }

    public void UpdateSubscription(int serviceDetailId, DateTime endDate)
    {
        ServiceDetailId = serviceDetailId;
        SetEndDate(endDate);
    }

    public void StopSubscriptionStatus()
    {
        UserSubscriptionStatus = UserSubscriptionStatus.Stopped;
    }

    public void ResumeSubscriptionStatus()
    {
        UserSubscriptionStatus = UserSubscriptionStatus.Active;
    }

    public void SetEndDate(DateTime endDate)
    {
        if (endDate <= StartDate)
        {
            throw new ArgumentException("Дата окончания должна быть позже даты начала.", nameof(endDate));
        }
        EndDate = endDate;
    }
}
