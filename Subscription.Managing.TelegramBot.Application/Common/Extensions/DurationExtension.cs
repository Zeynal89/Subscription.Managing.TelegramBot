namespace Subscription.Managing.TelegramBot.Application.Common.Extensions;

public static class DurationExtension
{
    public static DateTime GetEndDate(this Duration duration)
    {
        switch (duration)
        {
            case Duration.Week:
                return DateTime.Now.AddDays(7);
            case Duration.TwoWeek:
                return DateTime.Now.AddDays(14);
            case Duration.Month:
                return DateTime.Now.AddMonths(1);
            case Duration.ThreeMonth:
                return DateTime.Now.AddMonths(3);
            case Duration.SixMonth:
                return DateTime.Now.AddMonths(6);
            default:
                return DateTime.Now.AddYears(1);
        }
    }
}
