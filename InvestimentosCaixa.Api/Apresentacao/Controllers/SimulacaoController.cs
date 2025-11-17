using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Apresentacao.Atributos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    [ApiController]
    public class SimulacaoController : Controller
    {
        private readonly ISimulacaoServico _simulacaoServico;
        private readonly ILogger<SimulacaoController> _logger;
        public SimulacaoController(ISimulacaoServico simulacaoServico, ILogger<SimulacaoController> logger)
        {
            _simulacaoServico = simulacaoServico;
            _logger = logger;
        }

        [Telemetria]
        [Authorize]
        [ValidarSimulacao]
        [HttpPost("simular-investimento")]
        public async Task<ActionResult> SimularInvestimento(SimulacaoInvestimentoDTORequest simulacaoRequest)
        {
            try
            {
                var produto = HttpContext.Items["Produto"] as Produto;

                var simulacaoFinalizada = 
                    await _simulacaoServico.SimularInvestimento(
                        produto,
                        simulacaoRequest);

                if (simulacaoFinalizada == null)
                {
                    _logger.LogError("Erro ao processar a simulação de investimento.");
                    return BadRequest("Erro ao processar a simulação de investimento.");
                }

                return Ok(simulacaoFinalizada);
            }
            catch(Exception erro)
            {
                _logger.LogError($"Erro ao simular investimento: {erro.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        [Authorize]
        [HttpGet("listar-simulacoes")]
        public async Task<ActionResult> ListarSimulacoes()
        {
            try
            {
                var simulacoes = await _simulacaoServico.ListarSimulacoesInvestimentos();

                if (simulacoes.IsNullOrEmpty())
                {
                    _logger.LogInformation("Nenhuma simulação de investimento encontrada.");
                    return NoContent();
                }

                return Ok(simulacoes);
            }
            catch(Exception erro)
            {
                _logger.LogError($"Erro ao listar simulações de investimento: {erro.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        [Telemetria]
        [Authorize]
        [HttpGet("por-produto-dia")]
        public async Task<ActionResult> ListarSimulacoesPorDia()
        {
            try
            {
                var simulacoes = await _simulacaoServico.ListarSimulacoesDeProdutosPorDia();

                if (simulacoes.IsNullOrEmpty())
                {
                    _logger.LogInformation("Nenhuma simulação de investimento encontrada no dia.");
                    return NoContent();
                }

                _logger.LogInformation("Listagem de simulações de investimento no dia realizada com sucesso.");
                return Ok(simulacoes);
            }
            catch (Exception erro)
            {
                _logger.LogError($"Erro ao listar simulações de investimento por dia: {erro.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }
    }
}
