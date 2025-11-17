using InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    public interface ITelemetriaServico
    {
        Task CadastrarTelemetria(Telemetria telemetria);
        Task<TelemetriaDTOResponse?> ListarRelatorioTelemetria();
    }
}
