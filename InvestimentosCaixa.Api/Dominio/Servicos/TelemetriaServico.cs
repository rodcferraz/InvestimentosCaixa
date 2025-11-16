using InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    public class TelemetriaServico : ITelemetriaServico
    {
        private readonly ILogger<TelemetriaServico> _logger;
        private readonly ITelemetriaRepositorio _telemetriaRepositorio;
        private readonly ITelemetriaMapper _telemetriaMapper;
        public TelemetriaServico(
            ILogger<TelemetriaServico> logger,
            ITelemetriaRepositorio telemetriaRepositorio,
            ITelemetriaMapper telemetriaMapper)
        {
            _logger = logger;
            _telemetriaRepositorio = telemetriaRepositorio;
            _telemetriaMapper = telemetriaMapper;
        }
        public async Task CadastrarTelemetria(Telemetria telemetria)
        {
            await _telemetriaRepositorio.AdicionarAsync(telemetria);

            _logger.LogInformation($"Telemetria de método {telemetria.NomeRota} cadastrada.");
        }

        public async Task<TelemetriaDTOResponse> ListarRelatorioTelemetria()
        {
            var telemetrias = await _telemetriaRepositorio.ListarTodosAsync();

            return (telemetrias == null || telemetrias.Any()) ?
                        new TelemetriaDTOResponse() :
                        _telemetriaMapper.ToDtoResponse(telemetrias);
        }
    }
}
