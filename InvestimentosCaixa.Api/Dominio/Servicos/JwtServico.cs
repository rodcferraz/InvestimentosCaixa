using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    public class JwtServico
    {
        private readonly IConfiguration _config;

        public JwtServico(IConfiguration config)
        {
            _config = config;
        }

        public string GerarToken(string usuarioId, string email)
        {
            var settings = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(settings["Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracao = DateTime.UtcNow.AddHours(int.Parse(settings["ExpiresInHours"]));

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuarioId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("id", usuarioId),
            new Claim("role", "User") // exemplo
        };

            var token = new JwtSecurityToken(
                issuer: settings["Issuer"],
                audience: settings["Audience"],
                claims: claims,
                expires: expiracao,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
