using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    /// <summary>
    /// Mapper para conversões da classe <see cref = "CalculoParaPerfilRiscoMapper"/>.
    /// </summary>
    public class CalculoParaPerfilRiscoMapper : ICalculoPerfilRiscoMapper
    {
        /// <summary>
        /// Realiza a conversão de string para o enum <see cref="CalculoParaPerfilRiscoEnum"/>.
        /// </summary>
        /// <param name="perfil">Perfil escolhido para realizar o cáclulo do cliente</param>
        /// <returns>Retornar enum para o cálculo</returns>
        /// <exception cref="ConvertEnumException">Exceção lançada quando perfil não existe no <see cref="CalculoParaPerfilRiscoEnum"/></exception>
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
