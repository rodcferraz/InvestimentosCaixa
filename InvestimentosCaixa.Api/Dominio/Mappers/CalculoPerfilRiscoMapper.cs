using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    public class CalculoPerfilRiscoMapper : ICalculoPerfilRiscoMapper
    {
        public CalculoPerfilRiscoEnum ToPerfilRiscoClienteEnum(string perfil)
        {
            if (!Enum.TryParse(perfil, out CalculoPerfilRiscoEnum perfilRiscoCliente))
            {
                throw new ConvertEnumException(typeof(CalculoPerfilRiscoEnum), perfil);
            }

            return perfilRiscoCliente;
        }
    }
}
