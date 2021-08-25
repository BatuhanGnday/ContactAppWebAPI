using System.Linq;
using AutoMapper;
using ContactApp.Dto;
using ContactApp.Entities;

namespace ContactApp.Helpers.Mapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            
            CreateMap<Contact, ContactDto>();
            CreateMap<UserRole, UserRoleDto>();
            CreateMap<User, UserDto>();
            //     .ForMember(
            //      dto => dto.Roles, 
            // opt=> opt.MapFrom(
            //     user => user.Roles.Select(e => e.Name)));
        }
    }
}