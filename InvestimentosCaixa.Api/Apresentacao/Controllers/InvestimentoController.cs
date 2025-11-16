using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class InvestimentoController : Controller
    {
        private readonly IInvestimentoServico _investimentoServico;

        public InvestimentoController(IInvestimentoServico investimentoServico)
        {
            _investimentoServico = investimentoServico;
        }

        [HttpGet("{clienteId}")]
        public async Task<ActionResult> InvestimentosPorCliente(int clienteId)
        {
            try
            {
                var investimentosPorCliente = await _investimentoServico.ListarInvestimentosPorClienteAsync(clienteId);

                return Ok(investimentosPorCliente);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }
    }
}
