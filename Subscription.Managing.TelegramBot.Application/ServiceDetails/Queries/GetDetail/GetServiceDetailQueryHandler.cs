namespace Subscription.Managing.TelegramBot.Application.ServiceDetails.Queries.GetDetail;

public class GetServiceDetailQueryHandler : IRequestHandler<GetServiceDetailQuery, ServiceDetailDto>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public GetServiceDetailQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ServiceDetailDto> Handle(GetServiceDetailQuery request, CancellationToken cancellationToken)
    {
        var serviceDetail = await dbContext.Set<ServiceDetail>()
                                    .FirstOrDefaultAsync(p => p.Id == request.id);
        return mapper.Map<ServiceDetailDto>(serviceDetail);
    }
}
