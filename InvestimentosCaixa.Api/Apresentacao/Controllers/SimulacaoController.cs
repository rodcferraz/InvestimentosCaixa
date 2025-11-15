using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Atributos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class SimulacaoController : Controller
    {
        private readonly ISimulacaoServico _simulacaoServico;
        public SimulacaoController(ISimulacaoServico simulacaoServico)
        {
            _simulacaoServico = simulacaoServico;
        }

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

                return Ok(simulacaoFinalizada);
            }
            catch(Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ListarSimulacoes()
        {
            try
            {
                var simulacoes = await _simulacaoServico.ListarSimulacoesInvestimentos();

                if (simulacoes.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(simulacoes);
            }
            catch(Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        public async Task<ActionResult> ListSimulacoesPorDia()
        {
            try
            {
                var simulacoes = await _simulacaoServico.ListarSimulacoesInvestimentos();

                if (simulacoes.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(simulacoes);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }
    }
}
