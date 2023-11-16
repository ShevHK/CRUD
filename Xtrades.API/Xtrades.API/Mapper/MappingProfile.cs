using AutoMapper;
using Xtrades.BLL.Requests;
using Xtrades.DAL.Entities;

namespace Xtrades.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRequest, User>();
        }
    }

}
