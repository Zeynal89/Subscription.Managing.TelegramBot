namespace Subscription.Managing.TelegramBot.Application.ServiceDetails.Queries.GetDetail;

public record GetServiceDetailQuery(int id) : IRequest<ServiceDetailDto>;