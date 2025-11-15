using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos
{
    public class ProdutoDTORequest : ProdutoDTOBaseRequest
    {
        [Required(ErrorMessage = $"Campo {nameof(Id)} é obrigatório")]
        public int Id { get; set; }
    }
}
