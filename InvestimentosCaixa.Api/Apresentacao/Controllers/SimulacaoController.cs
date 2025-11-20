using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Apresentacao.Atributos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    /// <summary>
    /// Fornece endpoints para gerenciar simulações de investimento, incluindo a criação de simulações, a recuperação de simulações,
    /// e a listagem de simulações por critérios específicos.
    /// </summary>
    /// <remarks>Este controlador lida com operações relacionadas a simulações de investimento.
    /// Ele depende de serviços injetados para processamento de simulação, gerenciamento de clientes e
    /// recuperação de produtos.
    /// </remarks>
    [ApiController]
    public class SimulacaoController : Controller
    {
        private readonly ISimulacaoServico _simulacaoServico;
        private readonly IClienteServico _clienteServico;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ILogger<SimulacaoController> _logger;
        public SimulacaoController(
            ISimulacaoServico simulacaoServico,
            ILogger<SimulacaoController> logger,
            IClienteServico clienteServico,
            IProdutoRepositorio produtoRepositorio)
        {
            _simulacaoServico = simulacaoServico;
            _logger = logger;
            _clienteServico = clienteServico;
            _produtoRepositorio = produtoRepositorio;
        }

        /// <summary>
        /// Simula um investimento com base nos dados da solicitação fornecida.
        /// </summary>
        /// <param name="simulacaoRequest">O objeto de solicitação contendo os parâmetros da simulação de investimento, 
        /// incluindo o ID do cliente e o tipo do produto.
        /// </param>
        /// <returns>Retorna o resultado da simulação</returns>
        /// <response code = "201"> Simulação de Investimento realizado com sucesso </response>
        /// <response code = "400"> Requisição inválida </response>
        /// <response code = "404"> Requisição não encontrada </response>
        /// <response code = "500"> Erro interno no servidor </response>
        [Telemetria]
        [Authorize]
        [HttpPost("simular-investimento")]
        public async Task<ActionResult> SimularInvestimento(SimulacaoInvestimentoDTORequest simulacaoRequest)
        {
            try
            {
                if (!Enum.TryParse(simulacaoRequest.TipoProduto, out TipoProdutoEnum TipoProduto))
                {
                    _logger.LogError($"Erro ao converter enum {nameof(TipoProdutoEnum)} durante a busca do produto por tipo {simulacaoRequest.TipoProduto}.");
                    throw new ConvertEnumException(typeof(TipoProdutoEnum), simulacaoRequest.TipoProduto);
                }

                var clienteDb = await _clienteServico.DetalhesClienteAsync(simulacaoRequest.ClienteId);

                if (clienteDb == null)
                {
                    _logger.LogWarning($"Cliente com {simulacaoRequest.ClienteId} não encontrado.");
                    return NotFound($"Cliente não encontrado");
                }

                var produtoDb = await _produtoRepositorio.ListarProdutoPorTipo((int)TipoProduto);

                if (produtoDb == null)
                {
                    _logger.LogWarning($"Tipo de produto{simulacaoRequest.TipoProduto} não encontrado.");
                    return NotFound($"Produto não encontrado");
                }

                var simulacaoFinalizada = 
                    await _simulacaoServico.SimularInvestimento(
                        produtoDb,
                        simulacaoRequest);

                if (simulacaoFinalizada == null)
                {
                    _logger.LogError($"Erro ao processar a simulação de investimento para o clienteId {simulacaoRequest.ClienteId} com produtoId {produtoDb.Id}.");
                    return BadRequest("Erro ao processar a simulação de investimento.");
                }

                return Created("simular-investimento", simulacaoFinalizada);
            }
            catch (ConvertEnumException erro)
            {
                _logger.LogError($"Erro ao converter enum durante a simulação de investimento: {erro.Message}");
                return BadRequest(erro.Message);
            }
            catch(Exception erro)
            {
                _logger.LogError($"Erro ao simular investimento: {erro.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        /// <summary>
        /// Recupera uma lista de simulações de investimento.
        /// </summary>
        /// <remarks>Este método retorna todas as simulações de investimento disponíveis. </remarks>
        /// <returns>Retorna uma lista de simulações de investimento</returns>
        /// <response code = "200"> Listagem de simulação de investimento realizado com sucesso </response>
        /// <response code = "204"> Nenhuma listagem encontrada </response>
        /// <response code = "500"> Erro interno no servidor </response>
        [Telemetria]
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

        /// <summary>
        /// Recupera uma lista de simulações de investimento agrupadas por produto para o dia atual.
        /// </summary>
        /// <returns>Retorna uma lista de simulações de investimento por dia.</returns>
        /// <response code = "200"> Listagem de simulação de investimento para o dia realizado com sucesso </response>
        /// <response code = "204"> Nenhuma listagem encontrada </response>
        /// <response code = "500"> Erro interno no servidor </response>
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
