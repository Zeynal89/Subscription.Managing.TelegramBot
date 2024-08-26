namespace Subscription.Managing.TelegramBot.Application.Contracts.ServiceDetails.Models.Commands;

public record DeleteServiceDetailCommand(int id) : IRequest;