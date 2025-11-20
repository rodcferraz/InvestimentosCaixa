namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias
{
    /// <summary>
    /// Resposta de telemetria dos serviços
    /// </summary>
    public class TelemetriaDTOResponse
    {
        /// <summary>
        /// Dados de telemetria do endpoint
        /// </summary>
        public List<ServicoTelemetriaDTOResponse> Servicos { get; set; }

        /// <summary>
        /// Intervalo de tempo de consultas realizadas de telemetria para o endoint
        /// </summary>
        public PeriodoTelemetriaDTOResponse Periodo { get; set; }
    }
}
