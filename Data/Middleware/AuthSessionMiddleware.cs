using ASP_spr321.Data;
using ASP_spr321.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ASP_SPR311.Middleware
{
    public class AuthSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // !! Інжекція до Middleware здійснюється через метод (конструктор "зайнятий")
        public async Task InvokeAsync(HttpContext context, DataContext dataContext)
        {
            if (context.Request.Query.ContainsKey("logout"))
            {
                context.Session.Remove("userAccessId");
                context.Response.Redirect(context.Request.Path);
                return;
            }
            if (context.Session.Keys.Contains("userAccessId"))
            {
                // Користувач автентифікований
                //context.Items.Add("auth", "OK");

               
                // У сесії - лише ID, знаходимо усі дані про користувача
                if(dataContext
                    .UserAccesses
                        .Include(ua=>ua.UserData)
                        .Include(ua=>ua.UserRole)
                    .FirstOrDefault(ua =>
                         ua.Id.ToString() == context.Session.GetString("userAccessId"))
                    is UserAccess userAccess)
                {
                    context.User = new System.Security.Claims.ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Sid,        userAccess.Id.ToString()),
                            new Claim(ClaimTypes.Name,       userAccess.UserData.Name),
                            new Claim(ClaimTypes.Email,      userAccess.UserData.Email),
                            new Claim(ClaimTypes.MobilePhone,userAccess.UserData.Phone),
                            new Claim(ClaimTypes.Role,       userAccess.UserRole.Id),
                            new Claim("CanCreate",userAccess.UserRole.CanCreate.ToString()),
                            new Claim("CanRead",  userAccess.UserRole.CanRead.ToString()),
                            new Claim("CanUpdate",userAccess.UserRole.CanUpdate.ToString()),
                            new Claim("CanDelete",userAccess.UserRole.CanDelete.ToString()),
                        },
                        nameof(AuthSessionMiddleware)
                        )
                    );
                }

                

            }
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }


    public static class AuthSessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthSession(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthSessionMiddleware>();
        }
    }
}