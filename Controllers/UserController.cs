using System.Collections.Generic;
using ContactApp.Entities;
using ContactApp.Services;
using ContactApp.Services.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController
    {
        private readonly IUserService _userService;
        private readonly IContactService _contactService;
        

        public UserController(IUserService userService, IContactService contactService)
        {
            _userService = userService;
            _contactService = contactService;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _userService.GetUsers();
        }

        [HttpGet("{guid}")]
        public ActionResult<User> GetUserByGuid(string guid)
        {
            return _userService.GetUserByGuidApi(guid);
        }
        
        [HttpGet("{guid}/contacts")]
        public ActionResult<IEnumerable<Contact>> GetContactsByUserId(string guid)
        {
            return _contactService.GetContactsByUserId(guid);
        }
    }
}