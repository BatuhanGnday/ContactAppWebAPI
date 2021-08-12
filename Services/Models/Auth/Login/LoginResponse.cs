using ContactApp.Entities;

namespace ContactApp.Services.Models.Auth.login
{
    public class LoginResponse
    {
        public string Token { get; set; }
        
        public User User { get; set; }

        public LoginResponse(string token, User user)
        {
            Token = token;
            User = user;
        }
    }
}