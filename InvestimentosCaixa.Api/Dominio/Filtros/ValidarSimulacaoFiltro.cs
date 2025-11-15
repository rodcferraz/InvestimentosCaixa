using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InvestimentosCaixa.Api.Dominio.Filtros
{
    public class ValidarSimulacaoFiltro : IAsyncActionFilter
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ValidarSimulacaoFiltro(IClienteRepositorio clienteRepositorio, 
                                      IProdutoRepositorio produtoRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue("clienteId", out var cliente) || cliente is not int clientId)
            {
                context.Result = new BadRequestObjectResult("Parâmetro 'clientId' não encontrado.");
                return;
            }

            if (!context.ActionArguments.TryGetValue("produtoId", out var produto) || produto is not int produtoId)
            {
                context.Result = new BadRequestObjectResult("Parâmetro 'produtoId' não encontrado.");
                return;
            }

            var clienteDb = await _clienteRepositorio.ListarPorIdAsync(clientId);

            if (clienteDb == null)
            {
                context.Result = new NotFoundObjectResult($"Cliente {clientId} não encontrado.");
                return;
            }

            var produtoDb = await _produtoRepositorio.ListarPorIdAsync(clientId);

            if (produtoDb == null)
            {
                context.Result = new NotFoundObjectResult($"Produto {produtoId} não encontrado.");
                return;
            }

            context.HttpContext.Items["Cliente"] = clienteDb;
            context.HttpContext.Items["Produto"] = produtoDb;

            await next();
        }
    }
}
