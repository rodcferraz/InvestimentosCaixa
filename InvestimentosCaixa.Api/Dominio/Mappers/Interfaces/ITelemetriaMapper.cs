using InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    public interface ITelemetriaMapper
    {
        TelemetriaDTOResponse ToDtoResponse(List<Telemetria> telemetrias);
    }
}
