using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace WebApplication1.Strategy
{
    public class DefaultSignInStrategy : ISignInStrategy
    {
        public async Task SignInAsync(HttpContext httpContext, string username)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = false,
                RedirectUri = "/Home/Index",
            };

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}
