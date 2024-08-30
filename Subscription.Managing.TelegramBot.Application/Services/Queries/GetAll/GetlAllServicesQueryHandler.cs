namespace Subscription.Managing.TelegramBot.Application.Services.Queries.GetAll;

public class GetlAllServicesQueryHandler : IRequestHandler<GetlAllServicesQuery, List<ServicesDto>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public GetlAllServicesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<List<ServicesDto>> Handle(GetlAllServicesQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(dbContext.Set<Service>().ProjectTo<ServicesDto>(mapper.ConfigurationProvider).ToList());
    }
}
