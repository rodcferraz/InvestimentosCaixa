
using InvestimentosCaixa.Api.Aplicacao.DTOs.Temeletrias;
using InvestimentosCaixa.Api.Dominio.Entidades;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.TelemetriaServicoTestes
{
    public class TelemetriaServicoTestes : IClassFixture<TelemetriaServicoFixture>
    {
        private readonly TelemetriaServicoFixture _fixture;

        public TelemetriaServicoTestes()
        {
            _fixture = new TelemetriaServicoFixture();
        }

        [Fact]
        public async Task ListarRelatorioTelemetria_ComTelemetriasExistentes_RetornaTelemetriaDTOResponse()
        {
            // Arrange
            var telemetrias = new List<Telemetria>
            {
                new Telemetria { Id = 1, NomeRota = "api/clientes/cadastrar", TempoResposta = 100, DataRegistro = DateTime.UtcNow },
                new Telemetria { Id = 2, NomeRota = "api/produtos/listar", TempoResposta = 200, DataRegistro = DateTime.UtcNow }
            };

            _fixture.TelemetriaRepositorioMock
                .Setup(r => r.ListarTodosAsync())
                .ReturnsAsync(telemetrias);

            _fixture.TelemetriaMapperMock
                .Setup(m => m.ToDtoResponse(It.IsAny<List<Telemetria>>()))
                .Returns(new TelemetriaDTOResponse());

            // Act
            var resultado = await _fixture.Servico.ListarRelatorioTelemetria();

            // Assert
            Assert.IsType<TelemetriaDTOResponse>(resultado);

            _fixture.TelemetriaRepositorioMock.Verify(r => r.ListarTodosAsync(), Times.Once);
            _fixture.TelemetriaMapperMock.Verify(m => m.ToDtoResponse(telemetrias), Times.Once);
        }

        [Fact]
        public async Task ListarRelatorioTelemetria_ComListaVazia_RetornaNull()
        {
            // Arrange
            var telemetriasVazias = new List<Telemetria>();

            _fixture.TelemetriaRepositorioMock
                .Setup(r => r.ListarTodosAsync())
                .ReturnsAsync(telemetriasVazias);

            // Act
            var resultado = await _fixture.Servico.ListarRelatorioTelemetria();

            // Assert
            Assert.Null(resultado);
            _fixture.TelemetriaMapperMock.Verify(m => m.ToDtoResponse(It.IsAny<List<Telemetria>>()), Times.Never);
        }

        [Fact]
        public async Task ListarRelatorioTelemetria_ComNullDoRepositorio_RetornaNull()
        {
            // Arrange
            _fixture.TelemetriaRepositorioMock
                .Setup(r => r.ListarTodosAsync())
                .ReturnsAsync((List<Telemetria>)null);

            // Act
            var resultado = await _fixture.Servico.ListarRelatorioTelemetria();

            // Assert
            Assert.Null(resultado);
            _fixture.TelemetriaMapperMock.Verify(m => m.ToDtoResponse(It.IsAny<List<Telemetria>>()), Times.Never);
        }

        [Fact]
        public async Task ListarRelatorioTelemetria_ComMapperRetornandoNull_RetornaNull()
        {
            // Arrange
            var telemetrias = new List<Telemetria>
            {
                new Telemetria { Id = 1, NomeRota = "api/teste", TempoResposta = 100, DataRegistro = DateTime.UtcNow }
            };

            _fixture.TelemetriaRepositorioMock
                .Setup(r => r.ListarTodosAsync())
                .ReturnsAsync(telemetrias);

            _fixture.TelemetriaMapperMock
                .Setup(m => m.ToDtoResponse(It.IsAny<List<Telemetria>>()))
                .Returns((TelemetriaDTOResponse)null); // Mapper retorna null

            // Act
            var resultado = await _fixture.Servico.ListarRelatorioTelemetria();

            // Assert
            Assert.Null(resultado);
            _fixture.TelemetriaMapperMock.Verify(m => m.ToDtoResponse(telemetrias), Times.Once);
        }
    }
}

