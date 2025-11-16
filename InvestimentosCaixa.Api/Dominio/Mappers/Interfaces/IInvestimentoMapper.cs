using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    public interface IInvestimentoMapper
    {
        Investimento ToBaseEntity(InvestimentoDTOBaseRequest investimentoDto);
        List<InvestimentoDTOResponse> ToDtoResponseList(List<Investimento> investimentos);
    }
}
