using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Applicazione_1.Services
{
    public class JWT
    {
        public string GenerateJwtToken(string Role, string UserName)
        {
            var Header = Role +"|"+ UserName;
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, Header),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AhmedMahmoudAbdelkarimAhmedEbrahimKhaleefa123456789"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "VeniceCom.com",
                audience: "VeniceCom.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(90),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string? ValidateAndGetUsername(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("AhmedMahmoudAbdelkarimAhmedEbrahimKhaleefa123456789");

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = "VeniceCom.com",

                    ValidateAudience = true,
                    ValidAudience = "VeniceCom.com",

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // ✅ Extract the username (stored in "sub" claim)
                var username = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string Role = username.Split("|").Last()?? "";


                return Role;
            }
            catch
            {
                // ❌ Invalid or expired token
                return null;
            }
        }
    }
}
