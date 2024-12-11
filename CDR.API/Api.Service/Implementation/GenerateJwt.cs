using CDR.API.Api.Service.Interface;
using CDR.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CDR.API.Api.Service.Implementation
{
    public class GenerateJwt : IGenerateJwt
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public GenerateJwt(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var name = user.FirstName + " " + user.LastName;
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
        new Claim(ClaimTypes.Name, name),
        new Claim(ClaimTypes.UserData, user.Id.ToString())
    };
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddYears(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha384Signature));
            var Jwttoken = new JwtSecurityTokenHandler().WriteToken(token);
            return Jwttoken;
        }
    }
}
