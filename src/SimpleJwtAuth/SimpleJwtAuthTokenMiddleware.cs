using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace SimpleJwtAuth
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class SimpleJwtAuthTokenMiddleware<T> where T : IdentityUser
    {
        private readonly RequestDelegate _next;
        private readonly SimpleJwtAuthOptions _options;

        public SimpleJwtAuthTokenMiddleware(RequestDelegate next, IOptions<SimpleJwtAuthOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext, UserManager<T> userManager)
        {
            if (httpContext.Request.Method == HttpMethod.Post.Method &&
                httpContext.Request.Path.StartsWithSegments(_options.TokenEndpoint))
            {
                if (!httpContext.Request.HasFormContentType)
                {
                    await WriteResult(httpContext, isSuccess: false, error: "Request should include form data.");
                    return;
                }

                var username = httpContext.Request.Form["username"];
                var password = httpContext.Request.Form["password"];

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    await WriteResult(httpContext, isSuccess: false, error: "Username or password missing.");
                    return;
                }

                var user = await userManager.FindByEmailAsync(username);

                if (user == null)
                {
                    await WriteResult(httpContext, isSuccess: false, error: "User not found.");
                    return;
                }

                var validPassword = await userManager.CheckPasswordAsync(user, password);
                if (!validPassword)
                {
                    await WriteResult(httpContext, isSuccess: false);
                    return;
                }

                var token = JwtUtils.Create(
                    audience: _options.Audience,
                    issuer: _options.ClaimsIssuer,
                    userId: user.Id,
                    expirationTime: DateTime.Now.Add(_options.ExpirationTimeout),
                    secret: _options.Secret);

                var tokenString = JwtUtils.TokenToString(token);

                await WriteResult(httpContext, isSuccess: true, token: tokenString);
            }
            else
            {
                await _next(httpContext);
            }
        }

        private async Task WriteResult(HttpContext httpContext, bool isSuccess, string token = null, string error = null)
        {
            var response = JsonConvert.SerializeObject(new
            {
                success = isSuccess,
                token = token,
                error = error
            });

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)(isSuccess
                ? HttpStatusCode.OK
                : HttpStatusCode.BadRequest);

            await httpContext.Response.WriteAsync(response);
        }
    }
}
