using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
