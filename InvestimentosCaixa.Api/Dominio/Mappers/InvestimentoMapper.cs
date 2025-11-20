using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    /// <summary>
    /// Mapeador para conversão entre entidades de Investimento e seus respectivos DTOs.
    /// </summary>
    public class InvestimentoMapper : IInvestimentoMapper
    {
        /// <summary>
        /// Conversão entre <see cref="InvestimentoDTOBaseRequest"/> e <see cref="Investimento"/>."/>
        /// </summary>
        /// <param name="investimentoDto">Requisição de investimento</param>
        /// <returns>Entidade investimento</returns>
        public Investimento ToBaseEntity(InvestimentoDTOBaseRequest investimentoDto)
        {

            return new Investimento
            {
                IdCliente = investimentoDto.IdCliente,
                IdProduto = investimentoDto.IdProduto,
                Valor = investimentoDto.Valor,
                Data = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Conversão entre <see cref="Investimento"/> e <see cref="InvestimentoDTOResponse"/>."/>
        /// </summary>
        /// <param name="investimento">Entidade de investimento</param>
        /// <returns>Resposta de requisição de investimento</returns>
        public InvestimentoDTOResponse ToDtoResponse(Investimento investimento)
        {
            return new InvestimentoDTOResponse
            {
                Id = investimento.Id,
                Valor = investimento.Valor,
                Tipo = ((TipoProdutoEnum)investimento.Produto.Tipo).ToString(),
                Rentabilidade = investimento.Produto.Rentabilidade,
                Data = investimento.Data.ToString("yyyy-MM-dd")
            };
        }

        /// <summary>
        /// Conversão entre <see cref="List{Investimento}"/> e <see cref="List{InvestimentoDTOResponse}"/>."/>
        /// </summary>
        /// <param name="investimentos">Lista de entidades de investimento</param>
        /// <returns>Lista de resposta de requisição de investimento</returns>
        public List<InvestimentoDTOResponse> ToDtoResponseList(List<Investimento> investimentos)
        {
            return investimentos.Select(i => ToDtoResponse(i)).ToList();
        }
    }
}
