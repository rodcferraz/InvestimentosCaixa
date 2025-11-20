namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    /// <summary>
    /// Resposta de simulação de investimento
    /// </summary>
    public class SimulacaoInvestimentoDTOResponse
    {
        /// <summary>
        /// Produto de simulação validado
        /// </summary>
        public ProdutoValidadoDTOResponse ProdutoValidado { get; set; }

        /// <summary>
        /// Resultado de valores da aplicação simulada
        /// </summary>
        public ResultadoSimulacaoDTOResponse ResultadoSimulacao { get; set; }

        /// <summary>
        /// Data da simulação
        /// </summary>
        public DateTime DataSimulacao { get; set; }
    }
}
