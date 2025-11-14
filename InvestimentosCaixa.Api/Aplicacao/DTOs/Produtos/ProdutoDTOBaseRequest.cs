using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos
{
    public class ProdutoDTOBaseRequest
    {
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public decimal Rentabilidade { get; set; }
        public string Risco { get; set; }
    }
}
