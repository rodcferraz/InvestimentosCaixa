namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    /// <summary>
    /// Resposta de simulação realizada por produto em um determinado dia
    /// </summary>
    public class SimulacaoProdutoDiaDTOResponse
    {
        /// <summary>
        /// Nome do produto
        /// </summary>
        public string Produto { get; set; }

        /// <summary>
        /// Data de simulação realizada para o produto
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Quantidade total de simulaçãoes realizadas para o produto na data informada
        /// </summary>
        public int QuantidadeSimulacoes { get; set; }

        /// <summary>
        /// Média do valor final das simulações realizadas para o produto na data informada
        /// </summary>
        public decimal MediaValorFinal { get; set; }
    }
}
