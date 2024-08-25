namespace Subscription.Managing.TelegramBot.Application.Services.Commands.CreateService;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public CreateServiceCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = mapper.Map<Service>(request);
        await dbContext.Set<Service>().AddAsync(service);
        await dbContext.SaveChangesAsync();
    }
}
