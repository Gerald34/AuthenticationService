using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AuthenticationService.Entities;
using AuthenticationService.DTO;

namespace AuthenticationService.Config
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MainAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<RoleIdentifiers> _roles;
        private HttpContext _httpContext;
        public MainAuthorizeAttribute()
        {

        }
        public void OnAuthorization(AuthorizationFilterContext authorizationFilterContext)
        {
            var allowAnonymous = authorizationFilterContext.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            AuthenticateDTO user = (AuthenticateDTO)authorizationFilterContext.HttpContext.Items["User"];

            if (user == null)
            {
                authorizationFilterContext.Result = new JsonResult(new { error = true, message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}

