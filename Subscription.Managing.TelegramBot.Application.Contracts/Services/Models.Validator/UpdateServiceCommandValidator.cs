namespace Subscription.Managing.TelegramBot.Application.Contracts.Services.Models.Validator;

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