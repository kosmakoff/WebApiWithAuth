using Microsoft.AspNet.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJwtAuth
{
    public static class JwtUtils
    {
        public static JsonWebToken Create(string audience, string issuer, string userId, DateTime expirationTime, string secret)
        {
            var jwt = new JsonWebToken();

            jwt.Header = new JwtHeader
            {
                Algorithm = "HS256",
                Type = "JWT"
            };

            jwt.Payload = new JwtPayload
            {
                Audience = audience,
                Issuer = issuer,
                UserId = userId,
                ExpirationTime = expirationTime
            };

            jwt.Signature = CalculateSignature(jwt.Header, jwt.Payload, secret);

            return jwt;
        }

        public static string CalculateSignature(JwtHeader header, JwtPayload payload, string secret)
        {
            var headerString = JsonConvert.SerializeObject(header);
            var payloadString = JsonConvert.SerializeObject(payload);

            var headerStringBytes = Encoding.UTF8.GetBytes(headerString);
            var payloadStringBytes = Encoding.UTF8.GetBytes(payloadString);

            var headerBase64 = Base64UrlTextEncoder.Encode(headerStringBytes);
            var payloadBase64 = Base64UrlTextEncoder.Encode(payloadStringBytes);

            return CalculateSignature(headerBase64, payloadBase64, secret);
        }

        public static string CalculateSignature(string headerBase64, string payloadBase64, string secret)
        {
            var input = $"{headerBase64}.{payloadBase64}";
            var key = Encoding.UTF8.GetBytes(secret);
            var inputBytes = Encoding.UTF8.GetBytes(input);

            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                using (var stream = new MemoryStream(inputBytes))
                {
                    var hash = hmac.ComputeHash(stream);
                    return Base64UrlTextEncoder.Encode(hash);
                }
            }
        }
    }
}
