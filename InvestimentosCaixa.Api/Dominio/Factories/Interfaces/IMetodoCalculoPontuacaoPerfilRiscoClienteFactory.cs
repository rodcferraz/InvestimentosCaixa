using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Factories.Interfaces
{
    public interface IMetodoCalculoPontuacaoPerfilRiscoClienteFactory
    {
        IPerfilPontuacaoClienteServico Criar(CalculoParaPerfilRiscoEnum metodoCalculo);
    }
}
