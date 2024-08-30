namespace Subscription.Managing.TelegramBot.Application.ServiceDetails.Queries.GetAll;

public class GetlAllServiceDetailsQueryHandler : IRequestHandler<GetlAllServiceDetailsQuery, List<ServiceDetailDto>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public GetlAllServiceDetailsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<List<ServiceDetailDto>> Handle(GetlAllServiceDetailsQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(dbContext.Set<ServiceDetail>().ProjectTo<ServiceDetailDto>(mapper.ConfigurationProvider).ToList());
    }
}
