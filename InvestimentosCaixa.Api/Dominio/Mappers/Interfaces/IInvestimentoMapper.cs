using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    /// <summary>
    /// Mapeador para conversão entre entidades de Investimento e seus respectivos DTOs.
    /// </summary>
    public interface IInvestimentoMapper
    {
        /// <summary>
        /// Conversão entre <see cref="InvestimentoDTOBaseRequest"/> e <see cref="Investimento"/>."/>
        /// </summary>
        /// <param name="investimentoDto">Requisição de investimento</param>
        /// <returns>Entidade investimento</returns>
        Investimento ToBaseEntity(InvestimentoDTOBaseRequest investimentoDto);

        /// <summary>
        /// Conversão entre <see cref="List{Investimento}"/> e <see cref="List{InvestimentoDTOResponse}"/>."/>
        /// </summary>
        /// <param name="investimentos">Lista de entidades de investimento</param>
        /// <returns>Lista de resposta de requisição de investimento</returns>
        List<InvestimentoDTOResponse> ToDtoResponseList(List<Investimento> investimentos);

        /// <summary>
        /// Conversão entre <see cref="Investimento"/> e <see cref="InvestimentoDTOResponse"/>."/>
        /// </summary>
        /// <param name="investimento">Entidade de investimento</param>
        /// <returns>Resposta de requisição de investimento</returns>
        InvestimentoDTOResponse ToDtoResponse(Investimento investimento);
    }
}
