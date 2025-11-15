using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos
{
    public class ProdutoDTOBaseRequest
    {
        [Required(ErrorMessage = "Campo 'Nome' é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo 'Tipo' é obrigatório")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Campo 'Rentabilidade' é obrigatório")]
        public decimal Rentabilidade { get; set; }
        [Required(ErrorMessage = "Campo 'Risco' é obrigatório")]
        public string Risco { get; set; }
    }
}
