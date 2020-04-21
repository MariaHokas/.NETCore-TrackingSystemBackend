using AutoMapper;
using timeTrackingSystemBackend.Entities;
using timeTrackingSystemBackend.Models;

namespace timeTrackingSystemBackend.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}
