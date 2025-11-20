using InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    /// <summary>
    /// Serviço responsável por gerenciar operações relacionadas à utilização de endpoints
    /// </summary>
    public interface ITelemetriaServico
    {
        /// <summary>
        /// Registra parâmetros de telemetria para um endpoint específico
        /// </summary>
        /// <param name="telemetria">Entidade de <see cref="Telemetria"/> a ser cadastrada</param>
        Task CadastrarTelemetria(Telemetria telemetria);

        /// <summary>
        /// Lista todo o histórico de telemetria registrado por período e quantidade de vezes que foram chamadas
        /// </summary>
        /// <returns>Listagem de telemetria efetuada</returns>
        Task<TelemetriaDTOResponse?> ListarRelatorioTelemetria();
    }
}
