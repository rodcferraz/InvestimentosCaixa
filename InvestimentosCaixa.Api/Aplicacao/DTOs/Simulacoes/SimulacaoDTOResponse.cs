namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    public class SimulacaoDTOResponse
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Produto { get; set; }
        public decimal ValorInvestido { get; set; }
        public decimal ValorFinal { get; set; }
        public int PrazoMeses { get; set; }
        public string DataSimulacao { get; set; }
    }
}
