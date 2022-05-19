using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AuthenticationService.Entities;

namespace AuthenticationService.Config
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<RoleIdentifiers> _roles;
        public AuthorizeAttribute(params RoleIdentifiers[] roles)
        {
            _roles = roles ?? new RoleIdentifiers[] { };
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            UserEntity user = (UserEntity)context.HttpContext.Items["UserEntity"];
            Console.Write("UserEntity: ", user);
            if (user == null || (_roles.Any() && !_roles.Contains(user.role)))
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}

