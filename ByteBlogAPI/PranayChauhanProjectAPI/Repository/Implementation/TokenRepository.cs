using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PranayChauhanProjectAPI.Repository.Interface;

namespace PranayChauhanProjectAPI.Repository.Implementation
{
    public class TokenRepository : ITokenReoisitory
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {

            var claims = new List<Claim>
            {

                new Claim(ClaimTypes.Email,user.Email)

            };

            claims.AddRange(roles.Select(role
                 => new Claim(ClaimTypes.Role, role)));


            // JWT Security Token Parameters

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials =  new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(

                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims = claims,
                expires: DateTime.Now.AddMinutes(9),
                signingCredentials: credentials
                );

            // Return Token

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
