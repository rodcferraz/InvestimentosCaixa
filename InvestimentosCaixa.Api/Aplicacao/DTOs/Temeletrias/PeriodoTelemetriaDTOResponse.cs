namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias
{
    /// <summary>
    /// Intervalo de tempo para consulta de telemetria
    /// </summary>
    public class PeriodoTelemetriaDTOResponse
    {
        /// <summary>
        /// Data inicil do registro de telemetria dos endpoints cadastrados
        /// </summary>
        public string Inicio { get; set; }

        /// <summary>
        /// Data final do registro de telemetria dos endpoints cadastrados
        /// </summary>
        public string Fim { get; set; }
    }
}
