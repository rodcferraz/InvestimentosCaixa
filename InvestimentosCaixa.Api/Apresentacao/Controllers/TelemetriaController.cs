using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class TelemetriaController : Controller
    {
        private readonly ITelemetriaServico _telemetriaServico;
        public TelemetriaController(ITelemetriaServico telemetriaServico)
        {
            _telemetriaServico = telemetriaServico;
        }

        [HttpGet("telemetria")]
        public async Task<ActionResult> ListarRelatorioDeTelemetria()
        {
            try
            {
                var relatorioTelemetria = await _telemetriaServico.ListarRelatorioTelemetria();
                return Ok(relatorioTelemetria);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
