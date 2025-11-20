using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Factories.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Factories
{
    /// <summary>
    /// Responsel por criar a instância do serviço de geração de perfil de risco do cliente
    /// </summary>
    public class GerarPerfilRiscoClienteFactory : 
        IGerarPerfilRiscoClienteFactory
    {
        private readonly IInvestimentoServico _investimentoServico; 
        private readonly IClienteServico _clienteServico;

        public GerarPerfilRiscoClienteFactory(
            IInvestimentoServico investimentoServico,
            IClienteServico clienteServico,
            IPerfilPontuacaoClienteServico perfilPontuacao)
        {
            _investimentoServico = investimentoServico;
            _clienteServico = clienteServico;
        }

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
            IPerfilPontuacaoClienteServico perfilPontuacao)
        {
            return metodoCalculo switch
            {
                CalculoParaPerfilRiscoEnum.Personalizado => new PerfilRiscoClientePersonalizado(
                                                                perfilPontuacao,
                                                                _investimentoServico,
                                                                _clienteServico),
                CalculoParaPerfilRiscoEnum.Anbima => throw new NotImplementedException("Calculo de perfil de risco ANBIMA não implementado."),
                _ => throw new NotImplementedException("Calculo de perfil de risco não implementado.")
            };
        }
    }
}
