using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.TelemetriaServicoTestes
{
    public class TelemetriaServicoFixture
    {
        public Mock<ILogger<TelemetriaServico>> LoggerMock { get; }
        public Mock<ITelemetriaRepositorio> TelemetriaRepositorioMock { get; }
        public Mock<ITelemetriaMapper> TelemetriaMapperMock { get; }
        public TelemetriaServico Servico { get; }

        public TelemetriaServicoFixture()
        {
            LoggerMock = new Mock<ILogger<TelemetriaServico>>();
            TelemetriaRepositorioMock = new Mock<ITelemetriaRepositorio>();
            TelemetriaMapperMock = new Mock<ITelemetriaMapper>();
            Servico = new TelemetriaServico(
                LoggerMock.Object,
                TelemetriaRepositorioMock.Object,
                TelemetriaMapperMock.Object);
        }
    }
}
