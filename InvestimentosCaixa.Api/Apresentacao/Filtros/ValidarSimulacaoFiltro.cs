using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InvestimentosCaixa.Api.Apresentacao.Filtros
{
    public class ValidarSimulacaoFiltro : IAsyncActionFilter
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ILogger<ValidarSimulacaoFiltro> _logger;

        public ValidarSimulacaoFiltro(IClienteRepositorio clienteRepositorio, 
                                      IProdutoRepositorio produtoRepositorio,
                                      ILogger<ValidarSimulacaoFiltro> logger)
        {
            _clienteRepositorio = clienteRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue("simulacaoRequest", out var dtoObj)
                || dtoObj is not SimulacaoInvestimentoDTORequest simulacaoDtoRequest)
            {
                _logger.LogWarning($"{nameof(SimulacaoInvestimentoDTORequest)} não encontrado na requisição de simulação.");
                context.Result = new BadRequestObjectResult("Parâmetro 'clienteId' não encontrado.");
                return;
            }

            if (!Enum.TryParse(simulacaoDtoRequest.TipoProduto, out TipoProdutoEnum TipoProduto))
            {
                _logger.LogError($"Erro ao converter enum {nameof(TipoProdutoEnum)} durante a busca do produto por tipo {simulacaoDtoRequest.TipoProduto}.");
                throw new ConvertEnumException(typeof(TipoProdutoEnum), simulacaoDtoRequest.TipoProduto);
            }


            var clienteDb = await _clienteRepositorio.ListarPorIdAsync(simulacaoDtoRequest.ClienteId);

            if (clienteDb == null)
            {
                context.Result = new NotFoundObjectResult($"Cliente {simulacaoDtoRequest.ClienteId} não encontrado.");
                return;
            }

            var produtoDb = await _produtoRepositorio.ListarProdutoPorTipo((int)TipoProduto);

            if (produtoDb == null)
            {
                context.Result = new NotFoundObjectResult($"Produto {simulacaoDtoRequest.TipoProduto} não encontrado.");
                return;
            }

            context.HttpContext.Items["Cliente"] = clienteDb;
            context.HttpContext.Items["Produto"] = produtoDb;

            await next();
        }
    }
}
