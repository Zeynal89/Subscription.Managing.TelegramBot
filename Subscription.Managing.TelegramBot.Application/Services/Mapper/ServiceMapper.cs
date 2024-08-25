namespace Subscription.Managing.TelegramBot.Application.Services.Mapper;

public class ServiceMapper : Profile
{
    public ServiceMapper()
    {
        CreateMap<CreateServiceCommand, Service>();
        CreateMap<UpdateServiceCommand, Service>();
        CreateMap<Service, ServiceDto>();
    }
}
