using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace SimpleJwtAuth
{
    public class SimpleJwtAuthHandler<TUser> : AuthenticationHandler<SimpleJwtAuthOptions>
        where TUser :IdentityUser
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;

            // get authorization header
            string authorization = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorization))
            {
                return AuthenticateResult.Fail("No authorization header.");
            }

            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring("Bearer ".Length).Trim();
            }

            // If no token found, no further work possible
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("No bearer token.");
            }

            // decode token

            var tokenParts = token.Split('.');

            if (tokenParts.Length != 3)
            {
                return AuthenticateResult.Fail("Invalid token format.");
            }

            var headerPart = tokenParts[0];
            var payloadPart = tokenParts[1];
            var signature = tokenParts[2];

            // verify 

            var validSignature = JwtUtils.CalculateSignature(headerPart, payloadPart, Options.Secret);
            if (string.CompareOrdinal(signature, validSignature) != 0)
            {
                return AuthenticateResult.Fail("Signature doesn't match.");
            }

            // ignore header for now

            // decode payload
            var payloadBytes = Base64UrlTextEncoder.Decode(payloadPart);
            var payloadString = Encoding.UTF8.GetString(payloadBytes);
            var payload = JsonConvert.DeserializeObject<JwtPayload>(payloadString);

            var userId = payload.UserId;

            if (string.IsNullOrEmpty(userId))
            {
                return AuthenticateResult.Fail("User ID is null.");
            }

            var userManager = Context.RequestServices.GetRequiredService<UserManager<TUser>>();
            var signinManager = Context.RequestServices.GetRequiredService<SignInManager<TUser>>();

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return AuthenticateResult.Fail("User not found.");
            }

            var principal = await signinManager.CreateUserPrincipalAsync(user);

            // var claimsIdentity = new ClaimsIdentity();
            // claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            // var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Options.AuthenticationScheme);

            return AuthenticateResult.Success(ticket);
        }
    }
}
