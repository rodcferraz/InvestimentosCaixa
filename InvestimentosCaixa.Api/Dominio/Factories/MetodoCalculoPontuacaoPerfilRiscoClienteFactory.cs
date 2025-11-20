using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Factories.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Factories
{
    /// <summary>
    /// Factory para criação do método de cálculo da pontuação do perfil de risco do cliente
    /// </summary>
    public class MetodoCalculoPontuacaoPerfilRiscoClienteFactory 
        : IMetodoCalculoPontuacaoPerfilRiscoClienteFactory
    {

        /// <summary>
        /// Gera o tipo de serviço de cálculo de pontuação do perfil de risco do cliente conforme o método de cálculo informado
        /// </summary>
        /// <param name="metodoCalculo">Tipo de cálculo de perfil utilizado</param>
        /// <returns>Classe de cálculo do perfil</returns>
        /// <exception cref="NotImplementedException">Exceção lançada quando <see cref="CalculoParaPerfilRiscoEnum"/>
        /// náo foi selecionado ou não está implementado</exception>
        public IPerfilPontuacaoClienteServico Criar(CalculoParaPerfilRiscoEnum metodoCalculo)
        {
            return metodoCalculo switch
            {
                CalculoParaPerfilRiscoEnum.Personalizado => new PerfilPontuacaoClientePersonalizadoServico(),
                CalculoParaPerfilRiscoEnum.Anbima => throw new NotImplementedException("Calculo de perfil de risco ANBIMA não implementado."),
                _ => throw new NotImplementedException("Calculo de perfil de risco não implementado.")
            };
        }
    }
}
