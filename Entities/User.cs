using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ContactApp.Entities
{
    public class User
    {
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        [Required]
        public string Username { get; set; }
        
        [JsonIgnore]
        public string Password { get; set; }
        
        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string FullName { get; set; }

        public User(string username, string password, string email, string fullName)
        {
            Username = username;
            Password = password;
            Email = email;
            FullName = fullName;
        }
    }
}