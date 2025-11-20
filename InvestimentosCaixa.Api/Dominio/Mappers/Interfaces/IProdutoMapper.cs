using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    /// <summary>
    /// Classe de conversão entre a entidade Produto e seus respectivos DTOs.
    /// </summary>
    public interface IProdutoMapper
    {
        // <summary>
        /// Converte uma entidade <see cref="Produto"/> para <see cref="ProdutoDTOResponse"/> .
        /// </summary>
        /// <param name="produto"></param>
        /// <returns></returns>
        ProdutoDTOResponse ToDtoResponse(Produto produto);

        /// <summary>
        /// Converte um DTO de requisição <see cref="ProdutoDTOBaseRequest"/> para a entidade <see cref="Produto"/> .
        /// </summary>
        Produto ToBaseEntity(ProdutoDTOBaseRequest produtoDto);

        /// <summary>
        /// Converte um DTO de requisição <see cref="ProdutoDTORequest"/> para a entidade <see cref="Produto"/> .
        /// </summary>
        Produto ToEntity(ProdutoDTORequest produtoDto);

        /// <summary>
        /// Converte uma lista de entidades <see cref="Produto"/> para uma lista de <see cref="ProdutoDTOResponse"/> .
        /// </summary>
        List<ProdutoDTOResponse> ToDtoResponseList(IEnumerable<Produto> produtos);
    }
}
