using Newtonsoft.Json;
using System;

namespace SimpleJwtAuth
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JwtPayload
    {
        [JsonProperty("iss")]
        public string Issuer { get; set; }

        [JsonProperty("aud")]
        public string Audience { get; set; }

        [JsonProperty("exp")]
        public DateTime ExpirationTime { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
