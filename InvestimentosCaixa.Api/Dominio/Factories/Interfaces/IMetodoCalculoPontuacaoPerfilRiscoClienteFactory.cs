using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Factories.Interfaces
{
    /// <summary>
    /// Factory para criação do método de cálculo da pontuação do perfil de risco do cliente
    /// </summary>
    public interface IMetodoCalculoPontuacaoPerfilRiscoClienteFactory
    {
        /// <summary>
        /// Gera o tipo de serviço de cálculo de pontuação do perfil de risco do cliente conforme o método de cálculo informado
        /// </summary>
        /// <param name="metodoCalculo">Tipo de cálculo de perfil utilizado</param>
        /// <returns>Classe de cálculo do perfil</returns>
        /// <exception cref="NotImplementedException">Exceção lançada quando <see cref="CalculoParaPerfilRiscoEnum"/>
        /// náo foi selecionado ou não está implementado</exception>
        IPerfilPontuacaoClienteServico Criar(CalculoParaPerfilRiscoEnum metodoCalculo);
    }
}
