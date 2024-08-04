using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Security.JWT
{
    public class TokenOptions
    {
        public string SecurityKey { get; set; } 
        public string Issuer { get; set; } 
        public string Audience { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenTTL { get; set; }
    }
}
