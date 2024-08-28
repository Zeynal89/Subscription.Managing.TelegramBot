namespace Subscription.Managing.TelegramBot.Application.Users.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDto>();
    }
}