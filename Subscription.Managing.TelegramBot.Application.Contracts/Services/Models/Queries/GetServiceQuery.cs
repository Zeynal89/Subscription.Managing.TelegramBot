namespace Subscription.Managing.TelegramBot.Application.Contracts.Services.Models.Queries;

public record GetServiceQuery(int id) : IRequest<ServiceDto>;