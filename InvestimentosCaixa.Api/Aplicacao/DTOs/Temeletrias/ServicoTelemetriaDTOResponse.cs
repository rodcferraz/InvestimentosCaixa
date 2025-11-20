namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias
{
    /// <summary>
    /// Resposta de registro de telemetria de um serviço
    /// </summary>
    public class ServicoTelemetriaDTOResponse
    {
        /// <summary>
        /// Nome do endpoint registrado pela telemetria
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Quantidade chamadas efetivadas para o endpoint
        /// </summary>
        public int QuantidadeChamadas { get; set; }

        /// <summary>
        /// Média de tempo de resposta do endpoint em milissegundos
        /// </summary>
        public int MediaTempoRespostaMs { get; set; }
    }
}
