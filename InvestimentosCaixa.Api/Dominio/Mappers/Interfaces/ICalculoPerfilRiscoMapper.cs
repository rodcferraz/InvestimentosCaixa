using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    public interface ICalculoPerfilRiscoMapper
    {
        CalculoPerfilRiscoEnum ToPerfilRiscoClienteEnum(string perfil);
    }
}
