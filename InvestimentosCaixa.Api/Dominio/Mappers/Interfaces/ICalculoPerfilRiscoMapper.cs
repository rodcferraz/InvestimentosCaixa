using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    public interface ICalculoPerfilRiscoMapper
    {
        CalculoParaPerfilRiscoEnum ParaPerfilRiscoClienteEnum(string perfil);
    }
}
