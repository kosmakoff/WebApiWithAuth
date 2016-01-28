﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace SimpleJwtAuth
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class SimpleJwtAuthTokenMiddleware<T> where T : IdentityUser
    {
        private readonly RequestDelegate _next;
        private readonly SimpleJwtAuthOptions _options;

        public SimpleJwtAuthTokenMiddleware(RequestDelegate next, SimpleJwtAuthOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext, UserManager<T> userManager)
        {
            if (httpContext.Request.Method == HttpMethod.Post.Method &&
                httpContext.Request.Path.StartsWithSegments(_options.TokenEndpoint))
            {
                var username = httpContext.Request.Form["username"];
                var password = httpContext.Request.Form["password"];

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await httpContext.Response.WriteAsync("Username or password missing");

                    return;
                }

                var user = await userManager.FindByEmailAsync(username);

                if (user == null)
                {
                    await WriteResult(httpContext, isSuccess: false);
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

        private async Task WriteResult(HttpContext httpContext, bool isSuccess, string token = null)
        {
            var response = JsonConvert.SerializeObject(new
            {
                success = isSuccess,
                token = token
            });

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)(isSuccess
                ? HttpStatusCode.OK
                : HttpStatusCode.BadRequest);

            await httpContext.Response.WriteAsync(response);
        }
    }
}