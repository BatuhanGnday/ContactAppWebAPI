using AutoMapper;
using ContactApp.Dto;
using ContactApp.Entities;

namespace ContactApp.Helpers.Mapper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Contact, ContactDto>();
        }
    }
}