using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    public interface IProdutoMapper
    {
        ProdutoDTOResponse ToDtoResponse(Produto produto);
        Produto ToBaseEntity(ProdutoDTOBaseRequest produtoDto);
        Produto ToEntity(ProdutoDTORequest produtoDto);
        List<ProdutoDTOResponse> ToDtoResponseList(IEnumerable<Produto> produtos);
    }
}
