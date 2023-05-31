using AutoMapper;
using QuizApp.Shared.DTO;
using QuizApp.Shared.Models;

namespace QuizApp.Server.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, RegisterUserDTO>().ReverseMap();
        CreateMap<User, LoginUserDTO>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
    }
}
