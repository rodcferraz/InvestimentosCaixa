namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos
{
    /// <summary>
    /// Resposta de um investimento realizado
    /// </summary>
    public class InvestimentoDTOResponse
    {
        /// <summary>
        /// Id de investimento cadastrado
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tipo de investimento cadastrado
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Valor do investimento
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Rentabilidade do investimento
        /// </summary>
        public decimal Rentabilidade { get; set; }

        /// <summary>
        /// Data do investimento
        /// </summary>
        public string Data { get; set; }
    }
}
