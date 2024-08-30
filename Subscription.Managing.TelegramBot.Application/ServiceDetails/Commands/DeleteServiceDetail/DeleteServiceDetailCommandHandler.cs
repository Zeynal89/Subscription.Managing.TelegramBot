namespace Subscription.Managing.TelegramBot.Application.ServiceDetails.Commands.DeleteServiceDetail;

public class DeleteServiceDetailCommandHandler : IRequestHandler<DeleteServiceDetailCommand>
{
    private readonly IApplicationDbContext dbContext;

    public DeleteServiceDetailCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Handle(DeleteServiceDetailCommand request, CancellationToken cancellationToken)
    {
        var serviceDetail = await dbContext.Set<ServiceDetail>().FirstOrDefaultAsync(p => p.Id == request.id);
        if (serviceDetail == null)
        {
            throw new NotFoundException();
        }

        dbContext.Set<ServiceDetail>().Remove(serviceDetail);
        await dbContext.SaveChangesAsync();
    }
}
