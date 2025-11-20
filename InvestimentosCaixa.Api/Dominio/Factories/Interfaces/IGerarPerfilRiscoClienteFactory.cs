using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Factories.Interfaces
{
    /// <summary>
    /// Responsel por criar a instância do serviço de geração de perfil de risco do cliente
    /// </summary>
    public interface IGerarPerfilRiscoClienteFactory
    {
        /// <summary>
        /// Responsável por criar a instância do serviço de geração de perfil de risco do cliente
        /// </summary>
        /// <param name="metodoCalculo">Método de cálculo utilizado para a geração de perfil</param>
        /// <param name="perfilPontuacao">Pontuaçao que será gerada</param>
        /// <returns>Retorna o serviço responsável por orquestar o cálculo</returns>
        /// <exception cref="NotImplementedException">Lança exceção quando <see cref="CalculoParaPerfilRiscoEnum"/>
        /// não existe ou não foi implementado
        /// </exception>
        public IPerfilRiscoClienteServico Criar(
            CalculoParaPerfilRiscoEnum metodoCalculo,
            IPerfilPontuacaoClienteServico perfilPontuacao);
    }
}
