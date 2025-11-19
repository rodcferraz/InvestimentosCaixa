using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.InvestimentoServicoTestes
{
    public class InvestimentoServicoFixture
    {
        public Mock<IInvestimentoRepositorio> InvestimentoRepositorioMock { get; }
        public Mock<IInvestimentoMapper> InvestimentoMapperMock { get; }
        public ILogger<InvestimentoServico> LoggerMock => Mock.Of<ILogger<InvestimentoServico>>();
        public InvestimentoServico Servico { get; }

        public InvestimentoServicoFixture()
        {
            InvestimentoRepositorioMock = new Mock<IInvestimentoRepositorio>();
            InvestimentoMapperMock = new Mock<IInvestimentoMapper>();
            Servico = new InvestimentoServico(
                InvestimentoRepositorioMock.Object,
                InvestimentoMapperMock.Object,
                LoggerMock);
        }
    }
}
