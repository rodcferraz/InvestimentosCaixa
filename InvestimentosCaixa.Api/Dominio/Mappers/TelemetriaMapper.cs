using InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    public class TelemetriaMapper : ITelemetriaMapper
    {
        public TelemetriaDTOResponse ToDtoResponse(List<Telemetria> telemetrias)
        {
            var servicos = telemetrias
                .GroupBy(r => r.NomeRota)
                .Select(g => new ServicoTelemetriaDTOResponse
                {
                    Nome = g.Key,
                    QuantidadeChamadas = g.Count(),
                    MediaTempoRespostaMs = (int)g.Average(x => x.TempoResposta)
                })
                .ToList();

            return new TelemetriaDTOResponse
            {
                Servicos = servicos,
                Periodo = new PeriodoTelemetriaDTOResponse
                {
                    Inicio = telemetrias.Min(r => r.DataRegistro).ToString("yyyy-MM-dd"),
                    Fim = telemetrias.Max(r => r.DataRegistro).ToString("yyyy-MM-dd")
                }
            };
        }
    }
}
