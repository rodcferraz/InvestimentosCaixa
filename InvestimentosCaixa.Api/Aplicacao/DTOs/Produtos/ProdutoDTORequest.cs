using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos
{
    /// <summary>
    /// Requisição para atualização de produto
    /// </summary>
    public class ProdutoDTORequest : ProdutoDTOBaseRequest
    {
        /// <summary>
        /// Id do produto
        /// </summary>
        [Required(ErrorMessage = $"Campo {nameof(Id)} é obrigatório")]
        public int Id { get; set; }
    }
}
