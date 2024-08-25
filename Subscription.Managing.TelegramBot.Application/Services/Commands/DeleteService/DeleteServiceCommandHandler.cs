namespace Subscription.Managing.TelegramBot.Application.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
{
    private readonly IApplicationDbContext dbContext;

    public DeleteServiceCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await dbContext.Set<Service>().FirstOrDefaultAsync(p => p.Id == request.id);
        if (service == null)
        {
            throw new NotFoundException();
        }

        dbContext.Set<Service>().Remove(service);
        await dbContext.SaveChangesAsync();
    }
}
