using System;
using ContactApp.Entities;
using ContactApp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/hello-world")]
    public class HelloWorldController
    {
        private readonly IHttpContextAccessor _accessor;

        public HelloWorldController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        [HttpPost]
        public ActionResult<string> HelloWorld()
        {
            if (_accessor.HttpContext == null) return new UnauthorizedResult();
            var user = (User)_accessor.HttpContext.Items["User"]!;
            
            return user.Password;
        }
    }
}