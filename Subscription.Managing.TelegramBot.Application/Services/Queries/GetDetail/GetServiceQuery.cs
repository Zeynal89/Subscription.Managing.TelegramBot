namespace Subscription.Managing.TelegramBot.Application.Services.Queries.GetDetail;

public record GetServiceQuery(int id) : IRequest<ServiceDto>;