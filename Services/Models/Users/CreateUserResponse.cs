using System;
using System.ComponentModel;
using ContactApp.Entities;

namespace ContactApp.Services.Models.Users
{
    public class CreateUserResponse
    {
        public CreateUserResponseType Type { get; set; }
        public User? User { get; set; }

        public CreateUserResponse(CreateUserResponseType type, User? user)
        {
            Type = type;
            User = user;
        }

    }
    
    public enum CreateUserResponseType
    {
        UsernameAlreadyExist = 0,
        Success = 1,
        PasswordNotStrongEnough
    }
}