using System;
using System.Collections.Generic;
using System.Linq;
using ContactApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactApp.Helpers.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequirePermissionAttribute: Attribute, IAuthorizationFilter
    {   
        private readonly RolePermissions[] _permissions;

        public RequirePermissionAttribute(params RolePermissions[] permissions)
        {
            _permissions = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userToCast = context.HttpContext.Items["User"];
            if (userToCast == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

                return;
            }
            var user = (User)userToCast;
            HashSet<RolePermissions[]> permissionsArray = user.Roles.Select(e => e.Permissions).ToHashSet();
            var permissions = permissionsArray.SelectMany(x => x).Distinct();
            
            if (!_permissions.ToHashSet().IsSubsetOf(permissions))
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
            }
        }
    }
}