using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using StationMonitorAPI.Configurations.Constants;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace XTest.Api2.Common
{
    public static class HttpClientExtensions
    {
        public static void SetAdminClaimsViaHeaders(this HttpClient client, AppSettings setting)
        {
            var claims = new[]
            {
                new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Name, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.Key));

            var token = new JwtSecurityToken(
                claims: claims,
                audience: "NuxtApp",
                issuer: "StationMonitorAPI",
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
            var t = new JwtSecurityTokenHandler().WriteToken(token);
            client.DefaultRequestHeaders.Add(AuthenticatedTestRequestMiddleware.TestAuthorizationHeader, t);
        }
    }

    public class AuthenticatedTestRequestMiddleware
    {
        private readonly RequestDelegate _next;
        public static readonly string TestAuthorizationHeader = "FakeAuthorization";
        public AuthenticatedTestRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains(TestAuthorizationHeader))
            {
                var token = context.Request.Headers[TestAuthorizationHeader].Single();
                var jwt = new JwtSecurityToken(token);
                var claimsIdentity = new ClaimsIdentity(jwt.Claims, JwtBearerDefaults.AuthenticationScheme, JwtClaimTypes.Name, JwtClaimTypes.Role);
                context.User = new ClaimsPrincipal(claimsIdentity);
            }

            await _next(context);
        }
    }
}
