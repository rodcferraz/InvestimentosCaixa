using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Apresentacao.Atributos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    /// <summary>
    /// Fornece endpoints para gerenciar investimentos, incluindo a criação de novos investimentos 
    /// e a recuperação de investimentos associados a um cliente específico.
    /// </summary>
    /// <remarks>
    /// Esse controller requer autenticação para todos os métodos. 
    /// Possui integração com serviços de produtos e clientes.
    /// </remarks>
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

        /// <summary>
        /// Processa um pedido de investimento para um cliente e produto específico.
        /// </summary>
        /// <remarks> Este método requer autenticação e é monitorado por telemetria</remarks>
        /// <param name="request">Pedido de investimento que contém cliente id, produto id e valor.</param>
        /// <returns>Investimento cadastrado</returns>
        /// <response code = "201"> Investimento realizado com sucesso </response>
        /// <response code = "404"> Solicitação não encontrada </response>
        /// <response code = "500"> Erro interno no servidor </response>
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

                var investimentoCriado = await _investimentoServico.CadastrarInvestimentoAsync(request);

                _logger.LogInformation($"Investimento realizado com sucesso. Id do investimento: {investimentoCriado.Id}");

                return Created("/investimento", investimentoCriado);
            }
            catch (Exception erro)
            {
                _logger.LogError($"Erro ao realizar investimento: {erro.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        /// <summary>
        ///  Retorna uma lista de investimento associado a um cliente
        /// </summary>
        /// <remarks>Este método requer autenticação e é monitorado por telemetria</remarks>
        /// <param name="clienteId">Identificador único do cliente cujos investimentos deverão ser retornados</param>
        /// <returns>Lista de investimentos do cliente</returns>
        /// <response code = "200"> Investimentos listados com sucesso </response>
        /// <response code = "500"> Erro interno no servidor </response>
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
