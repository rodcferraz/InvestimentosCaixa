using InvestimentosCaixa.Api.Aplicacao.DTOs.Autenticar;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    public class AutenticarController : ControllerBase
    {
        private readonly JwtServico _jwt;
        private readonly SegurancaServico _segurancaServico;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly ILogger<AutenticarController> _logger;

        public AutenticarController(
            JwtServico jwt, 
            SegurancaServico segurancaServico,
            IClienteRepositorio clienteRepositorio, 
            ILogger<AutenticarController> logger)
        {
            _jwt = jwt;
            _segurancaServico = segurancaServico;
            _clienteRepositorio = clienteRepositorio;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(AutenticarRequest request)
        {
            try
            {
                var clienteDb = await _clienteRepositorio.ListarClienteAtivoPorEmailAsync(request.Email);

                if (clienteDb == null)
                {
                    _logger.LogWarning($"Tentativa de login falhou para o email: {request.Email} - Cliente não encontrado ou inativo.");
                    return NotFound("Cliente não encontrado ou inativo.");
                }

                var senhaClienteHash = _segurancaServico.CriptografarPasswordHash(request.Senha);

                if (clienteDb.SenhaHash == senhaClienteHash)
                {
                    var token = _jwt.GerarToken(clienteDb.Id.ToString(), clienteDb.Email);
                    _logger.LogInformation($"Token gerado para o cliente {clienteDb.Id}");
                    return Ok(new { token });
                }

                return Unauthorized(new { message = "Credenciais inválidas" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro durante o login: {ex.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }
    }
}
