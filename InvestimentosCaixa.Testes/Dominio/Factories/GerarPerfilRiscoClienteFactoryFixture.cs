using InvestimentosCaixa.Api.Dominio.Factories;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Factories
{
    public class GerarPerfilRiscoClienteFactoryFixture
    {
        public Mock<IInvestimentoServico> InvestimentoServicoMock { get; }
        public Mock<IClienteServico> ClienteServicoMock { get; }
        public Mock<IPerfilPontuacaoClienteServico> PerfilPontuacaoMock { get; }
        public GerarPerfilRiscoClienteFactory Factory { get; }

        public GerarPerfilRiscoClienteFactoryFixture()
        {
            InvestimentoServicoMock = new Mock<IInvestimentoServico>();
            ClienteServicoMock = new Mock<IClienteServico>();
            PerfilPontuacaoMock = new Mock<IPerfilPontuacaoClienteServico>();

            Factory = new GerarPerfilRiscoClienteFactory(
                InvestimentoServicoMock.Object,
                ClienteServicoMock.Object,
                PerfilPontuacaoMock.Object
            );
        }
    }
}
