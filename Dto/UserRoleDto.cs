using System.Collections.Generic;
using ContactApp.Entities;

namespace ContactApp.Dto
{
    public class UserRoleDto
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public IEnumerable<RolePermissions> Permissions { get; set; }
    }
}