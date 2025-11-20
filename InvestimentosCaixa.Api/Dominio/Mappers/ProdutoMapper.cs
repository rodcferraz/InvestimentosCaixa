using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    /// <summary>
    /// Classe de conversão entre a entidade Produto e seus respectivos DTOs.
    /// </summary>
    public class ProdutoMapper : IProdutoMapper
    {
        /// <summary>
        /// Converte uma entidade <see cref="Produto"/> para <see cref="ProdutoDTOResponse"/> .
        /// </summary>
        /// <param name="produto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converte uma lista de entidades <see cref="Produto"/> para uma lista de <see cref="ProdutoDTOResponse"/> .
        /// </summary>
        public List<ProdutoDTOResponse> ToDtoResponseList(IEnumerable<Produto> produtos)
        {
            return produtos
                .Select(x => ToDtoResponse(x))
                .ToList();
        }

        /// <summary>
        /// Converte um DTO de requisição <see cref="ProdutoDTOBaseRequest"/> para a entidade <see cref="Produto"/> .
        /// </summary>
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

        /// <summary>
        /// Converte um DTO de requisição <see cref="ProdutoDTORequest"/> para a entidade <see cref="Produto"/> .
        /// </summary>
        public Produto ToEntity(ProdutoDTORequest produtoDto)
        {
            var produto = ToBaseEntity(produtoDto);
            produto.Id = produtoDto.Id;
            return produto;
        }
    }
}
