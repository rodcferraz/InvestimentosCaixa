using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    /// <summary>
    /// Fornece endpoints para gerenciar e recuperar relatórios de telemetria.
    /// </summary>
    [ApiController]
    public class TelemetriaController : Controller
    {
        private readonly ITelemetriaServico _telemetriaServico;
        public TelemetriaController(ITelemetriaServico telemetriaServico)
        {
            _telemetriaServico = telemetriaServico;
        }

        /// <summary>
        /// Recupera registros de telemetria
        /// </summary>
        /// <returns> Lista de todos os de dados de telemetria dos endpoints cadastrados.</returns>
        /// <response code = "200"> Registro de telemetria cadastrado </response>
        /// <response code = "500"> Erro interno no servidor </response>
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
