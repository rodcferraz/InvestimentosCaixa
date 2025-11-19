using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Moq;
namespace InvestimentosCaixa.Testes.Dominio.Servicos.PerfilRiscoClientePersonalizadoServicoTestes
{
    public class PerfilRiscoClientePersonalizadoFixture
    {
        public Mock<IPerfilPontuacaoClienteServico> PerfilPontuacaoClienteServicoMock { get; }
        public Mock<IInvestimentoServico> InvestimentoServicoMock { get; }
        public Mock<IClienteServico> ClienteServicoMock { get; }
        public PerfilRiscoClientePersonalizado Servico { get; }

        public PerfilRiscoClientePersonalizadoFixture()
        {
            PerfilPontuacaoClienteServicoMock = new Mock<IPerfilPontuacaoClienteServico>();
            InvestimentoServicoMock = new Mock<IInvestimentoServico>();
            ClienteServicoMock = new Mock<IClienteServico>();

            Servico = new PerfilRiscoClientePersonalizado(
                PerfilPontuacaoClienteServicoMock.Object,
                InvestimentoServicoMock.Object,
                ClienteServicoMock.Object);
        }
    }
}
