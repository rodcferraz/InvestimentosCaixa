using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    public class CalculoParaPerfilRiscoMapper : ICalculoPerfilRiscoMapper
    {
        public CalculoParaPerfilRiscoEnum ParaPerfilRiscoClienteEnum(string perfil)
        {
            if (!Enum.TryParse(perfil, out CalculoParaPerfilRiscoEnum perfilRiscoCliente))
            {
                throw new ConvertEnumException(typeof(CalculoParaPerfilRiscoEnum), perfil);
            }

            return perfilRiscoCliente;
        }
    }
}
