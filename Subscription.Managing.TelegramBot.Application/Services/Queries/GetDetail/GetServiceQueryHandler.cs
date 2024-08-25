using System;

namespace Subscription.Managing.TelegramBot.Application.Services.Queries.GetDetail;

public class GetServiceQueryHandler : IRequestHandler<GetServiceQuery, ServiceDto>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public GetServiceQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ServiceDto> Handle(GetServiceQuery request, CancellationToken cancellationToken)
    {
        var service = await dbContext.Set<Service>().FirstOrDefaultAsync(p => p.Id == request.id);
        return mapper.Map<ServiceDto>(service);
    }
}
