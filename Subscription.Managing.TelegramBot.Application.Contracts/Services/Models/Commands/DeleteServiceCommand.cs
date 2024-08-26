namespace Subscription.Managing.TelegramBot.Application.Contracts.Services.Models.Commands;

public record DeleteServiceCommand(int id) : IRequest;