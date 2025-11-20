namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    /// <summary>
    /// Resposta da simulação de investimento
    /// </summary>
    public class SimulacaoDTOResponse
    {
        /// <summary>
        /// Id da simulação de investimento realizada
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id do cliente que realizou a simulação
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Nome do produto simulado
        /// </summary>
        public string Produto { get; set; }

        /// <summary>
        /// Valor investido na simulação
        /// </summary>
        public decimal ValorInvestido { get; set; }

        /// <summary>
        /// Valor total com juros após o prazo da simulação
        /// </summary>
        public decimal ValorFinal { get; set; }

        /// <summary>
        /// Prazo total em meses de aplicação na simulação
        /// </summary>
        public int PrazoMeses { get; set; }

        /// <summary>
        /// Data de efetivação da simulação
        /// </summary>
        public string DataSimulacao { get; set; }
    }
}
