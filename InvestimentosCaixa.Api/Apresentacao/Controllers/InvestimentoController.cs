using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Apresentacao.Atributos;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    [ApiController]
    public class InvestimentoController : Controller
    {
        private readonly IInvestimentoServico _investimentoServico;
        private readonly ILogger<InvestimentoController> _logger;
        private readonly IClienteServico _clienteServico;
        private readonly IProdutoServico _produtoServico;

        public InvestimentoController(
            IInvestimentoServico investimentoServico, 
            ILogger<InvestimentoController> logger,
            IClienteServico clienteServico,
            IProdutoServico produtoServico)
        {
            _investimentoServico = investimentoServico;
            _logger = logger;
            _clienteServico = clienteServico;
            _produtoServico = produtoServico;
        }

        [Telemetria]
        [Authorize]
        [HttpPost("investimento")]
        public async Task<ActionResult> RealizarInvestimento(InvestimentoDTOBaseRequest request)
        {
            try
            {
                var clienteDb = await _clienteServico.DetalhesClienteAsync(request.IdCliente);

                if (clienteDb == null)
                {
                    _logger.LogWarning($"Cliente com {request.IdCliente} não encontrado.");
                    return NotFound($"Cliente não encontrado");
                }

                var produtoDb = await _produtoServico.DetalhesProdutoAsync(request.IdProduto);

                if (produtoDb == null)
                {
                    _logger.LogWarning($"Produto com Id {request.IdProduto} não encontrado.");
                    return NotFound($"Produto não encontrado");
                }

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
        [Authorize]
        [HttpGet("investimentos/{clienteId}")]
        public async Task<ActionResult> ListarInvestimentosPorCliente(int clienteId)
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
