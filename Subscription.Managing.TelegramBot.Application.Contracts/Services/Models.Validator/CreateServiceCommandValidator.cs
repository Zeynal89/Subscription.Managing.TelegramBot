namespace Subscription.Managing.TelegramBot.Application.Contracts.Services.Models.Validator;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty();

        RuleFor(v => v.Description)
        .NotEmpty();
    }
}