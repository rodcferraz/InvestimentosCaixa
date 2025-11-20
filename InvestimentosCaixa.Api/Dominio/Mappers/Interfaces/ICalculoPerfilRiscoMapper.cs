using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    /// <summary>
    /// Mapper para conversões da classe <see cref = "CalculoParaPerfilRiscoMapper"/>.
    /// </summary>
    public interface ICalculoPerfilRiscoMapper
    {
        /// <summary>
        /// Realiza a conversão de string para o enum <see cref="CalculoParaPerfilRiscoEnum"/>.
        /// </summary>
        /// <param name="perfil">Perfil escolhido para realizar o cáclulo do cliente</param>
        /// <returns>Retornar enum para o cálculo</returns>
        /// <exception cref="ConvertEnumException">Exceção lançada quando perfil não existe no <see cref="CalculoParaPerfilRiscoEnum"/></exception>
        CalculoParaPerfilRiscoEnum ParaPerfilRiscoClienteEnum(string perfil);
    }
}
