namespace Subscription.Managing.TelegramBot.Application.ServiceDetails.Commands.CreateServiceDetail;

public class CreateServiceDetailCommandHandler : IRequestHandler<CreateServiceDetailCommand>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public CreateServiceDetailCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task Handle(CreateServiceDetailCommand request, CancellationToken cancellationToken)
    {
        var serviceDetail = mapper.Map<ServiceDetail>(request);
        await dbContext.Set<ServiceDetail>().AddAsync(serviceDetail);
        await dbContext.SaveChangesAsync();
    }
}
