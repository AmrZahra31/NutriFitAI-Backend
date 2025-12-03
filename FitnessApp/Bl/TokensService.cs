using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FitnessApp.Domain.Entities;

namespace FitnessApp.Bl
{
    public class TokensService
    {
        public static async Task<string> GenerateTokenAsync(ApplicationUsers user, UserManager<ApplicationUsers> userManager, IConfiguration configuration)
        {
            try
            {
                var Roles = await userManager.GetRolesAsync(user);

                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];
                var key = configuration["Jwt:SecretKey"];

                var claims = new List<Claim>();
                claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? "unKnown"));
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email ?? "unKnown"));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                foreach (var role in Roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));
                if (Roles == null)
                    claims.Add(new Claim(ClaimTypes.Role, "Custmer"));
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(

                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: credentials
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
