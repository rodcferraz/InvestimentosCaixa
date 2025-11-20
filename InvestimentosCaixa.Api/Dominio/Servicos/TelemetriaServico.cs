using InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    /// <summary>
    /// Serviço responsável por gerenciar operações relacionadas à utilização de endpoints
    /// </summary>
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

        /// <summary>
        /// Registra parâmetros de telemetria para um endpoint específico
        /// </summary>
        /// <param name="telemetria">Entidade de <see cref="Telemetria"/> a ser cadastrada</param>
        public async Task CadastrarTelemetria(Telemetria telemetria)
        {
            await _telemetriaRepositorio.AdicionarAsync(telemetria);

            _logger.LogInformation($"Telemetria com id {telemetria.Id} para o  método {telemetria.NomeRota} foi cadastrado.");
        }

        /// <summary>
        /// Lista todo o histórico de telemetria registrado por período e quantidade de vezes que foram chamadas
        /// </summary>
        /// <returns>Listagem de telemetria efetuada</returns>
        public async Task<TelemetriaDTOResponse?> ListarRelatorioTelemetria()
        {
            var telemetrias = await _telemetriaRepositorio.ListarTodosAsync();

            if (telemetrias == null || telemetrias.Count == 0)
            {
                _logger.LogInformation("Nenhum dado de telemetria encontrado.");
                return null;
            }

            _logger.LogInformation("Relatório de telemetria gerado com sucesso.");

            return _telemetriaMapper.ToDtoResponse(telemetrias);
        }
    }
}
