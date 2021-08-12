using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ContactApp.Entities;
using ContactApp.Services.Models.Auth.login;
using ContactApp.Services.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Services
{
    public interface IUserService
    {
        public IEnumerable<User> GetUsers();
        public User GetUserByGuid(string guid);
    }
    public class UserService: IUserService
    {
        private readonly ApplicationContext _applicationContext;

        public UserService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IEnumerable<User> GetUsers()
        {
            return _applicationContext.Users.ToList();
        }

        public User GetUserByGuid(string guid)
        {
            var iGuid = new Guid(guid);

            return _applicationContext.Users.Find(iGuid);
        }
    }
}