using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Shared.JWToken
{
    public class TokenGenerationRequest
    {
        public string Email { get; set; }
       
        public string JWTSubject { get; set; }
        public string JWTKey { get; set;}
        public string JWTIssuer { get; set; }

        public string JwtAudience { get; set; }
    }
}
