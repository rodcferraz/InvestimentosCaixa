namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    /// <summary>
    /// Reposta de um produto validado para simulação de investimento
    /// </summary>
    public class ProdutoValidadoDTOResponse
    {
        /// <summary>
        /// Id do produto validado pela simulação
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do produto validado pela simulação
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Tipo do produto validado pela simulação
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Rentabilidade do produto validado pela simulação
        /// </summary>
        public decimal Rentabilidade { get; set; }

        /// <summary>
        /// Risco do produto validado pela simulação
        /// </summary>
        public string Risco { get; set; }
    }
}
