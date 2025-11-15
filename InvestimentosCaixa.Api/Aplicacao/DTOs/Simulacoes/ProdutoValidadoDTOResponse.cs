namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes
{
    public class ProdutoValidadoDTOResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public decimal Rentabilidade { get; set; }
        public String Risco { get; set; }
    }
}
