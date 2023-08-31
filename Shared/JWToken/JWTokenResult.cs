using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.JWToken
{
    public class JWTokenResult
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
