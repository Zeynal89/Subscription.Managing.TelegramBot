namespace Subscription.Managing.TelegramBot.Application.Contracts.ServiceDetails.Models.Queries;

public record GetServiceDetailQuery(int id) : IRequest<ServiceDetailDto>;