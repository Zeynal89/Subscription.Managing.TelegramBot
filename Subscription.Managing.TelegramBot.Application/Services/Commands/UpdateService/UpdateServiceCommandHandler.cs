namespace Subscription.Managing.TelegramBot.Application.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public UpdateServiceCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await dbContext.Set<Service>().FirstOrDefaultAsync(x => x.Id == request.Id);
        if (service == null)
        {
            throw new NotFoundException();
        }

        mapper.Map(request, service);
        await dbContext.SaveChangesAsync();
    }
}
