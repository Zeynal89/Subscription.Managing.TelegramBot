namespace Subscription.Managing.TelegramBot.Application.Services.Queries.GetAll;

public class GetlAllServicesQueryHandler : IRequestHandler<GetlAllServicesQuery, List<ServiceDto>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public GetlAllServicesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<List<ServiceDto>> Handle(GetlAllServicesQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(dbContext.Set<Service>().Include(p => p.ServiceDetails).ProjectTo<ServiceDto>(mapper.ConfigurationProvider).ToList());
    }
}
