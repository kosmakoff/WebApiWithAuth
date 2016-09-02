using Newtonsoft.Json;

namespace SimpleJwtAuth
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JwtHeader
    {
        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonProperty("typ")]
        public string Type { get; set; }
    }
}
