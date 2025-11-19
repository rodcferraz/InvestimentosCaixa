using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Factories.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Factories
{
    public class MetodoCalculoPontuacaoPerfilRiscoClienteFactory 
        : IMetodoCalculoPontuacaoPerfilRiscoClienteFactory
    {
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
