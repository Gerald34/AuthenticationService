using Microsoft.Extensions.Options;
using AuthenticationService.Config;
using AuthenticationService.Services;

namespace AuthenticationService.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public JwtMiddleware(RequestDelegate next,
        IOptions<JwtSettings> jwtSettings,
        JwtService jwtService)
        {
            _next = next;
            _jwtSettings = jwtSettings.Value;
            _jwtService = jwtService;
        }

        public async Task Invoke(HttpContext context, UserService userService)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var isValid = _jwtService.ValidateToken(token);

            if (!isValid.error)
            {
                context.Items["User"] = userService.GetById(isValid.userID);
            }

            await _next(context);
        }

    }
}