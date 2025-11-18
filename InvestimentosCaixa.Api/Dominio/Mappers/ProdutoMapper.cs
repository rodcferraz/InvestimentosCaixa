using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    public class ProdutoMapper : IProdutoMapper
    {
        public ProdutoDTOResponse ToDtoResponse(Produto produto)
        {
            return new ProdutoDTOResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Tipo = ((TipoProdutoEnum)produto.Tipo).ToString(),
                Rentabilidade = produto.Rentabilidade,
                Risco = ((RiscoProdutoEnum)produto.Risco).ToString()
            };
        }

        public List<ProdutoDTOResponse> ToDtoResponseList(IEnumerable<Produto> produtos)
        {
            return produtos
                .Select(x => ToDtoResponse(x))
                .ToList();
        }

        public Produto ToBaseEntity(ProdutoDTOBaseRequest produtoDto)
        {
            if (!Enum.TryParse(produtoDto.Tipo, out TipoProdutoEnum tipoProduto))
            {
                throw new ConvertEnumException(typeof(TipoProdutoEnum), produtoDto.Tipo);
            }

            if (!Enum.TryParse(produtoDto.Risco, out RiscoProdutoEnum riscoProduto))
            {
                throw new ConvertEnumException(typeof(RiscoProdutoEnum), produtoDto.Risco);
            }

            return new Produto
            {
                Nome = produtoDto.Nome,
                Tipo = (int) tipoProduto,
                Rentabilidade = produtoDto.Rentabilidade,
                Risco = (int) riscoProduto
            };
        }

        public Produto ToEntity(ProdutoDTORequest produtoDto)
        {
            var produto = ToBaseEntity(produtoDto);
            produto.Id = produtoDto.Id;
            return produto;
        }
    }
}
