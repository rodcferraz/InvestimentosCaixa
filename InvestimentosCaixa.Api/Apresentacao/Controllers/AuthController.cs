using InvestimentosCaixa.Api.Dominio.Servicos;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly JwtServico _jwt;

        public AuthController(JwtServico jwt)
        {
            _jwt = jwt;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simulação: valide usuário no banco aqui
            if (request.Email == "teste@teste.com")
            {
                var token = _jwt.GerarToken("123", request.Email);
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Credenciais inválidas" });
        }
    }
}
