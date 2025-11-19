using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Factories.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Factories
{
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
