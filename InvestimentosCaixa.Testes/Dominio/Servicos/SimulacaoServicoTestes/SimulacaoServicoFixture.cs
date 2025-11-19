using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.SimulacaoServicoTestes
{
    public class SimulacaoServicoFixture
    {
        public Mock<ISimulacaoRepositorio> SimulacaoRepositorioMock { get; }
        public Mock<ISimulacaoMapper> SimulacaoMapperMock { get; }
        public ILogger<SimulacaoServico> LoggerMock => Mock.Of<ILogger<SimulacaoServico>>();
        public SimulacaoServico Servico { get; }

        public SimulacaoServicoFixture()
        {
            SimulacaoRepositorioMock = new Mock<ISimulacaoRepositorio>();
            SimulacaoMapperMock = new Mock<ISimulacaoMapper>();
            Servico = new SimulacaoServico(
                SimulacaoRepositorioMock.Object,
                SimulacaoMapperMock.Object,
                LoggerMock);
        }
    }
}
