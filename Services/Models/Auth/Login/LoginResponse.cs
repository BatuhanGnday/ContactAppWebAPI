using ContactApp.Entities;

namespace ContactApp.Services.Models.Auth.login
{
    public class LoginResponse
    {
        public string Token { get; set; }
        
        public RolePermissions[] Permissions { get; set; }
    }
}