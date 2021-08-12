using System.ComponentModel.DataAnnotations;

namespace ContactApp.Services.Models.Auth.Register
{
    public class RegisterRequestBody
    {
        public string Username { get; set; }
        public string Password { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}