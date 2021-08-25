using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContactApp.Dto;
using ContactApp.Entities;
using ContactApp.Helpers.Mapper;
using ContactApp.Services.Models.Auth.login;
using ContactApp.Services.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Services
{
    public interface IUserService
    {
        public IEnumerable<UserDto> GetUsers();
        public ActionResult<User> GetUserByGuidApi(string guid);
        public User GetUserByGuid(string guid);


    }
    public class UserService: IUserService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _applicationContext;

        public UserService(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _applicationContext.Users
                .Include(e => e.Roles).ToList();
            return _mapper.Map<IEnumerable<UserDto>>(users);
            //return users;
        }

        public ActionResult<User> GetUserByGuidApi(string guid)
        {
            var tryParse = Guid.TryParse(guid, out var id);
            if (!tryParse)
            {
                return new NotFoundResult();
            }

            return _applicationContext.Users.Find(id);
        }
        
        public User GetUserByGuid(string guid)
        {
            var tryParse = Guid.TryParse(guid, out var id);

            return _applicationContext.Users.Include(e => e.Roles).First(t => t.Guid == id);
        }

    }
}