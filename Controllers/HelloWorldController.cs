using System;
using AutoMapper;
using ContactApp.Dto;
using ContactApp.Entities;
using ContactApp.Helpers;
using ContactApp.Helpers.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
    [ApiController]
    [RequirePermission]
    [Route("/hello-world")]
    public class HelloWorldController
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;

        public HelloWorldController(IHttpContextAccessor accessor, IMapper mapper)
        {
            _accessor = accessor;
            _mapper = mapper;
        }

        [HttpPost]
        [RequirePermission(
            RolePermissions.DeleteUser,
            RolePermissions.ReadContact,
            RolePermissions.UpdateContact)]
        public ActionResult<string> HelloWorld()
        {
            if (_accessor.HttpContext == null) return new UnauthorizedResult();
            var user = (User)_accessor.HttpContext.Items["User"]!;
            
            return user.Password;
        }
    }
}