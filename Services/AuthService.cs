using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ContactApp.Entities;
using ContactApp.Helpers;
using ContactApp.Services.Models.Auth.login;
using ContactApp.Services.Models.Auth.Register;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ContactApp.Services
{
    public interface IAuthService
    {
        public ActionResult<LoginResponse> Login(LoginRequestBody requestBody);
        public ActionResult<User> Register(RegisterRequestBody requestBody);
    }
    public class AuthService: IAuthService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly AppSettings _appSettings;

        public AuthService(IOptions<AppSettings> appSettings, ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _appSettings = appSettings.Value;
        }

        public ActionResult<LoginResponse> Login(LoginRequestBody requestBody)
        {
            var user = _applicationContext.Users.Include(user1 => user1.Roles).FirstOrDefault(e => e.UserName == requestBody.Username && e.Password == requestBody.Password);
            if (user==null)
            {
                return new UnauthorizedResult();
            }
            var token = GenerateJwtToken(user);
            Console.WriteLine(user.Roles.Count);
            
            HashSet<RolePermissions[]> permissionsArray = user.Roles.Select(e => e.Permissions).ToHashSet();
            var permissions = permissionsArray.SelectMany(x => x).Distinct();

            return new LoginResponse{Token = token, Permissions = permissions.ToArray()};
        }

        public ActionResult<User> Register(RegisterRequestBody requestBody)
        {
            var userExistByUsername = _applicationContext.Users.Any(x => x.UserName == requestBody.Username);
            if (userExistByUsername)
            {
                return new ConflictResult();
            }

            var user = new User()
            {
                UserName = requestBody.Username,
                Password = requestBody.Password,
                Email = requestBody.Email,
                FullName = requestBody.FullName
            };
            
            _applicationContext.Users.Add(user);
            _applicationContext.SaveChanges();
            
            return user;
        }
        
        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Guid.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}