using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Shared.JWToken;
using System.Text;
using System;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthentication
{
    public class JWTokenManager :IJWTokenManager
    {
      
        public JWTokenResult GenerateToken(TokenGenerationRequest request)
        {
            //create claims details based on the user information
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, request.JWTSubject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Email", request.Email)
                   };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(request.JWTKey));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // var expiry = DateTime.UtcNow.AddDays(1);

            var expiry = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(request.JWTIssuer, request.JwtAudience, claims, expires: expiry, signingCredentials: signIn);

            JWTokenResult jwtResult = new JWTokenResult()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiry = expiry
            };

            return jwtResult;

        }
    }
}