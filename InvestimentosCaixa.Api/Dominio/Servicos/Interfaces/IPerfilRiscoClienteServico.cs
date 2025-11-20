using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    /// <summary>
    /// Orquestrador para definir o perfil de risco do cliente utilizando os cálculos de pontuação e seus respectivos pesos
    /// </summary>
    public interface IPerfilRiscoClienteServico
    {
        /// <summary>
        /// Realiza cálculos para gerar o perfil de risco do cliente
        /// </summary>
        /// <param name="idCliente">Id do cliente para que será gerado o perfil de risco</param>
        /// <returns>Perfil de risco e pontuação associada</returns>
        /// <exception cref="EntityNotFoundException">Lança exceção quando cliente não for encontrado</exception>
        Task<(PerfilRiscoClienteEnum, decimal)> CalcularPerfilRiscoCliente(int idCliente);
    }
}
