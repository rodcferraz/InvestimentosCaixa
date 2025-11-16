namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos
{
    public class ProdutoRecomendadoDTOResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public decimal Rentabilidade { get; set; }
        public string Risco { get; set; }
    }
}
