namespace Subscription.Managing.TelegramBot.Application.ServiceDetails.Commands.UpdateServiceDetail;

public class UpdateServiceDetailCommandHandler : IRequestHandler<UpdateServiceDetailCommand>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public UpdateServiceDetailCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task Handle(UpdateServiceDetailCommand request, CancellationToken cancellationToken)
    {
        var serviceDetail = await dbContext.Set<ServiceDetail>().FirstOrDefaultAsync(x => x.Id == request.Id);
        if (serviceDetail == null)
        {
            throw new NotFoundException();
        }

        mapper.Map(request, serviceDetail);
        await dbContext.SaveChangesAsync();
    }
}
