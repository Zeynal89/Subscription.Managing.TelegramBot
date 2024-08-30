namespace Subscription.Managing.TelegramBot.Application.UserSubscriptions.Mapper
{
    internal class UserSubscriptionMapper : Profile
    {
        public UserSubscriptionMapper()
        {
            CreateMap<UserSubscription, UserSubscriptionDto>();
        }
    }
}