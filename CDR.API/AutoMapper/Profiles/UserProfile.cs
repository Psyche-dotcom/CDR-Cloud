using AutoMapper;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;

namespace CDR.API.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisterDto, User>().ReverseMap();
        }
    }
}
