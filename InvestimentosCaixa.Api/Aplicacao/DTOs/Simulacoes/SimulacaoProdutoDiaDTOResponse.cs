namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    public class SimulacaoProdutoDiaDTOResponse
    {
        public string Produto { get; set; }
        public string Data { get; set; }
        public int QuantidadeSimulacoes { get; set; }
        public decimal MediaValorFinal { get; set; }
    }
}
