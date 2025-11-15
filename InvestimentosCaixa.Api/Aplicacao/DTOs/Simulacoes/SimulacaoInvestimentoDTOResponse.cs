namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    public class SimulacaoInvestimentoDTOResponse
    {
        public ProdutoValidadoDTOResponse ProdutoValidado { get; set; }
        public ResultadoSimulacaoDTOResponse ResultadoSimulacao { get; set; }
        public DateTime DataSimulacao { get; set; }
    }
}
