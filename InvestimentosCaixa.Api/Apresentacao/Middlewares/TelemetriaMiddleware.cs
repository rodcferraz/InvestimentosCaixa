using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using System.Diagnostics;

namespace InvestimentosCaixa.Api.Apresentacao.Middlewares
{
    public class TelemetriaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TelemetriaMiddleware> _logger;
        //private readonly ITelemetriaServico _telemetriaServico;
        private readonly ITelemetriaRepositorio _telemetriaServico;
        public TelemetriaMiddleware(
            RequestDelegate next, 
            ILogger<TelemetriaMiddleware> logger,
            ITelemetriaRepositorio telemetriaServico
            )
        {
            _next = next;
            _logger = logger;
            //_telemetriaServico = telemetriaServico;
        }

        public async Task Invoke(HttpContext context)
        {
            var timer = Stopwatch.StartNew();

            await _next(context);

            timer.Stop();

            var telemetria = new Telemetria();
            telemetria.NomeRota = context.Request.Path;
            telemetria.TempoResposta = timer.ElapsedMilliseconds;
            telemetria.DataRegistro = DateTime.UtcNow;

            await _telemetriaServico.AdicionarAsync(telemetria);

            _logger.LogInformation(
                "Requisição {Method} {Path} levou {Elapsed} ms",
                context.Request.Method,
                context.Request.Path,
                timer.ElapsedMilliseconds
            );
        }
    }
}
