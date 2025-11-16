using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace InvestimentosCaixa.Api.Apresentacao.Filtros
{
    public class TelemetriaFiltro : IAsyncActionFilter
    {
        private readonly ILogger<TelemetriaFiltro> _logger;
        private readonly ITelemetriaServico _telemetriaServico;

        public TelemetriaFiltro(
            ILogger<TelemetriaFiltro> logger,
            ITelemetriaServico telemetriaServico)
        {
            _logger = logger;
            _telemetriaServico = telemetriaServico;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var timer = Stopwatch.StartNew();

            await next();

            timer.Stop();

            var telemetria = new Telemetria();
            telemetria.NomeRota = context.HttpContext.Request.Path;
            telemetria.TempoResposta = timer.ElapsedMilliseconds;
            telemetria.DataRegistro = DateTime.UtcNow;

            await _telemetriaServico.CadastrarTelemetria(telemetria);

            _logger.LogInformation(
                "Requisição {Method} {Path} levou {Elapsed} ms",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                timer.ElapsedMilliseconds
            );
        }
    }
}
