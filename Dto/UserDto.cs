using System.Collections.Generic;
using ContactApp.Entities;

namespace ContactApp.Dto
{
    public class UserDto
    {
        public string Guid { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserRoleDto> Roles { get; set; }
    }
}