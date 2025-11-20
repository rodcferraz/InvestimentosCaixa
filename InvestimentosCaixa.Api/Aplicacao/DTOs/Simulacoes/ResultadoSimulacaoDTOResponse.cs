namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    /// <summary>
    /// Resultado de aplicação de simulação de investimento
    /// </summary>
    public class ResultadoSimulacaoDTOResponse
    {
        /// <summary>
        /// Valor final após o período de investimento
        /// </summary>
        public decimal ValorFinal { get;set; }

        /// <summary>
        /// Rentabilidade total obtida na simulação
        /// </summary>
        public decimal RentabilidadeEfetiva { get; set; }

        /// <summary>
        /// Prazo total do investimento em meses
        /// </summary>
        public int PrazoMeses { get; set; }
    }
}
