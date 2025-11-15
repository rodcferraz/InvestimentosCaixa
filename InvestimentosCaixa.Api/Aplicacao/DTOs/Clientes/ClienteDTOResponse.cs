namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes
{
    public class ClienteDTOResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public decimal RendaMensal { get; set; }
        public decimal PercentualInvestimentoRenda { get; set; }
        public int PerfilDeclarado { get; set; }
    }
}
