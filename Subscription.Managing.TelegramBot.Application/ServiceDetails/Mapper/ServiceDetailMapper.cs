namespace Subscription.Managing.TelegramBot.Application.ServiceDetails.Mapper;

public class ServiceDetailMapper : Profile
{
    public ServiceDetailMapper()
    {
        CreateMap<CreateServiceDetailCommand, ServiceDetail>();
        CreateMap<UpdateServiceDetailCommand, ServiceDetail>();
        CreateMap<ServiceDetail, ServiceDetailDto>();
    }
}
