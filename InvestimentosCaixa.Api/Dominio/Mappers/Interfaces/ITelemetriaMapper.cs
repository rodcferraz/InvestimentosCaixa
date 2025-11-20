using InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    /// <summary>
    /// Serviço responsável por gerenciar operações relacionadas à utilização de endpoints
    /// </summary>
    public interface ITelemetriaMapper
    {
        /// <summary>
        /// Realiza o mapeamento de uma lista de <see cref="List{Telemetria}"/> para <see cref="TelemetriaDTOResponse"/>
        /// </summary>
        TelemetriaDTOResponse ToDtoResponse(List<Telemetria> telemetrias);
    }
}
