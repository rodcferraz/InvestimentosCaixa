namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos
{
    public class InvestimentoDTOResponse
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public decimal Rentabilidade { get; set; }
        public string Data { get; set; }
    }
}
