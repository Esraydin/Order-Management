using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OrderManagement.Application.Security.Encryption
{
    public static class SecurityKeyHelper
    {
        public static SymmetricSecurityKey CreateSecurityKey(string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            
            // Eğer anahtar 64 byte'dan kısa ise, 64 byte'a doldur
            if (keyBytes.Length < 64)
            {
                Array.Resize(ref keyBytes, 64);
            }

            return new SymmetricSecurityKey(keyBytes);
        }
    }
}
