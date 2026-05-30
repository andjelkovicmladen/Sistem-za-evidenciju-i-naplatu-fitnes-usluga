using BrokerBazePodataka;
using Domen;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Services
{
    public interface IAuthService
    {
        (bool success, Administrator admin, string token, string error) Login(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly BrokerBP _broker;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _broker = new BrokerBP();
            _configuration = configuration;
        }

        public (bool success, Administrator admin, string token, string error) Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, null, null, "Email i lozinka su obavezni.");

            try
            {
                string upit = "SELECT * FROM Administrator WHERE Email=@email AND Password=@pw";
                var parametri = new Dictionary<string, object>
                {
                    { "@email", email },
                    { "@pw", password }
                };

                List<IEntity> rezultat = _broker.ExecuteQuery(new Administrator(), upit, parametri);

                if (rezultat.Count == 0)
                    return (false, null, null, "Pogrešni kredencijali. Proverite email i lozinku.");

                Administrator admin = (Administrator)rezultat[0];
                string token = GenerateJwtToken(admin);

                return (true, admin, token, null);
            }
            catch (Exception ex)
            {
                return (false, null, null, $"Greška pri prijavi: {ex.Message}");
            }
        }

        private string GenerateJwtToken(Administrator admin)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, admin.IdAdministrator.ToString()),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Name, admin.ImePrezime)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
