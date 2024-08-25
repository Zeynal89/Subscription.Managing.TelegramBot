namespace Subscription.Managing.TelegramBot.Application.Services.Commands.UpdateService;

public class UpdateUserCommandValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty();

        RuleFor(v => v.Description)
        .NotEmpty();
    }
}