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
        public ActionResult<User> GetUserByGuidApi(string guid);
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

            return _applicationContext.Users.Find(id);
        }

    }
}