using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Security.Encryption;
using OrderManagement.Application.Security.Entities;
using OrderManagement.Domain.Entities;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Application.Security.Extensions;
using Newtonsoft.Json;

namespace OrderManagement.Application.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
 
        public IConfiguration Configuration { get; }
        private readonly TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = new TokenOptions
            {
                Issuer = configuration["TokenOptions:Issuer"],
                Audience = configuration["TokenOptions:Audience"],
                SecurityKey = configuration["TokenOptions:SecurityKey"],
                AccessTokenExpiration = int.Parse(configuration["TokenOptions:AccessTokenExpiration"])
            };
        }

        public AccessToken CreateToken(User user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var jwt = CreateJwtToken(_tokenOptions, user, signingCredentials, jwtSecurityTokenHandler);
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = accessTokenExpiration
            };
        }

        public SecurityToken CreateJwtToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials,
            JwtSecurityTokenHandler tokenHandler)
        {
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(SetClaims(user)),
                Expires = DateTime.Now.AddDays(3),
                Issuer = tokenOptions.Issuer,
                Audience = tokenOptions.Audience,
                NotBefore = DateTime.Now,
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
            //JwtSecurityToken jwt = new(
            //    tokenOptions.Issuer,
            //    tokenOptions.Audience,
            //    expires: DateTime.Now.AddMinutes(tokenOptions.AccessTokenExpiration),
            //    notBefore: DateTime.Now,
            //    claims: SetClaims(user),
            //    signingCredentials: signingCredentials
            //);
            //return jwt;
        }

        public RefreshToken CreateRefreshToken(User user, string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            return refreshToken;
        }


        private IEnumerable<Claim> SetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email),
            };

            return claims;
        }
    }
}
