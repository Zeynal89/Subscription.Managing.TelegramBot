namespace Subscription.Managing.TelegramBot.Application.Services.Commands.CreateService;

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