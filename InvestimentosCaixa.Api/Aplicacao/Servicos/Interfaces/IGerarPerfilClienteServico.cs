using InvestimentosCaixa.Api.Aplicacao.DTOs.Perfis;
using InvestimentosCaixa.Api.Dominio.Factories.Interfaces;

namespace InvestimentosCaixa.Api.Aplicacao.Servicos.Interfaces
{
    /// <summary>
    /// Responsável por gerar o perfil de risco do cliente
    /// </summary>
    public interface IGerarPerfilClienteServico
    {
        /// <summary>
        /// Gera o perfil de risco do cliente. O factory <see cref="IGerarPerfilRiscoClienteFactory"/>será responsável por orquestrar o cálculo
        /// efetivado pela fábrica de métodos de cálculo <see cref="IMetodoCalculoPontuacaoPerfilRiscoClienteFactory"/> a fim 
        /// de gerar o perfil de risco do cliente.
        /// </summary>
        /// <param name="idCliente">Id do cliente para geração do perfil</param>
        /// <returns>Resposta para o perfil do cliente gerado.</returns>
        Task<PerfilClienteDTOResponse> GerarPerfilCiente(int idCliente);
    }
}
