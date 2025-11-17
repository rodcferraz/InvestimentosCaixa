using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Apresentacao.Atributos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    [ApiController]
    public class InvestimentoController : Controller
    {
        private readonly IInvestimentoServico _investimentoServico;
        private readonly ILogger<InvestimentoController> _logger;

        public InvestimentoController(IInvestimentoServico investimentoServico, ILogger<InvestimentoController> logger)
        {
            _investimentoServico = investimentoServico;
            _logger = logger;
        }

        [Telemetria]
        [HttpPost("investimento")]
        public async Task<ActionResult> RealizarInvestimento(InvestimentoDTOBaseRequest request)
        {
            try
            {
                var idInvestimento = await _investimentoServico.CadastrarInvestimentoAsync(request);

                _logger.LogInformation($"Investimento realizado com sucesso. Id do investimento: {idInvestimento}");

                return Ok("Investimento realizado com sucesso!");
            }
            catch (Exception erro)
            {
                _logger.LogError($"Erro ao realizar investimento: {erro.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        [Telemetria]
        [HttpGet("investimentos/{clienteId}")]
        public async Task<ActionResult> InvestimentosPorCliente(int clienteId)
        {
            try
            {
                var investimentosPorCliente = await _investimentoServico.ListarInvestimentosPorClienteAsync(clienteId);

                return Ok(investimentosPorCliente);
            }
            catch (Exception erro)
            {
                _logger.LogError($"Erro ao listar investimentos do cliente {clienteId}: {erro.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }
    }
}
