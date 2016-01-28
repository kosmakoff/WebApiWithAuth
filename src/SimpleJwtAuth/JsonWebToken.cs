using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleJwtAuth
{
    public class JsonWebToken
    {
        public JwtHeader Header { get; set; }

        public JwtPayload Payload { get; set; }

        public string Signature { get; set; }
    }
}
