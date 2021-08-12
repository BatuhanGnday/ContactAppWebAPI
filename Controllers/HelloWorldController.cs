using ContactApp.Helpers;
using ContactApp.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/hello-world")]
    public class HelloWorldController
    {
        [HttpPost]
        public ActionResult<HelloWorldResponse> HelloWorld([FromBody] HelloWorldRequest request)
        {
            return new HelloWorldResponse
            {
                Username = ">username<"
            };
        }
    }
}