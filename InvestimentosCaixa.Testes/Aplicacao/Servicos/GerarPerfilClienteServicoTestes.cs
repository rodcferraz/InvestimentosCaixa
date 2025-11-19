using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Moq;

namespace InvestimentosCaixa.Testes.Aplicacao.Servicos
{
    public class GerarPerfilClienteServicoTestes : IClassFixture<GerarPerfilClienteServicoFixture>
    {
        private readonly GerarPerfilClienteServicoFixture _fixture;

        public GerarPerfilClienteServicoTestes()
        {
            _fixture = new GerarPerfilClienteServicoFixture();
        }

        [Fact]
        public async Task GerarPerfilCliente_QuandoConfiguradoComoPersonalizado_DeveRetornarPerfilComPontuacaoCalculada()
        {
            // Arrange
            var clienteId = 123;
            var perfilEsperado = PerfilRiscoClienteEnum.Moderado;
            var pontuacaoCalculada = 48m;

            _fixture.ConfigurarMetodoCalculo("Personalizado");

            var metodoCalculoMock = new Mock<IPerfilPontuacaoClienteServico>();
            var perfilRiscoClienteMock = new Mock<IPerfilRiscoClienteServico>();

            _fixture.MetodoCalculoFactoryMock
                .Setup(x => x.Criar(CalculoParaPerfilRiscoEnum.Personalizado))
                .Returns(metodoCalculoMock.Object);

            _fixture.PerfilRiscoFactoryMock
                .Setup(x => x.Criar(CalculoParaPerfilRiscoEnum.Personalizado, metodoCalculoMock.Object))
                .Returns(perfilRiscoClienteMock.Object);

            perfilRiscoClienteMock
                .Setup(x => x.CalcularPerfilRiscoCliente(clienteId))
                .ReturnsAsync((perfilEsperado, pontuacaoCalculada));

            // Act
            var resultado = await _fixture.Servico.GerarPerfilCiente(clienteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(clienteId, resultado.ClienteId);
            Assert.Equal("Moderado", resultado.Perfil);
            Assert.Equal(48m, resultado.Pontuacao);
            Assert.False(string.IsNullOrEmpty(resultado.Descricao));

            _fixture.CalculoMapperMock.Verify(x => x.ParaPerfilRiscoClienteEnum(It.IsAny<string>()), Times.Once);
            _fixture.MetodoCalculoFactoryMock.Verify(x => x.Criar(It.IsAny<CalculoParaPerfilRiscoEnum>()), Times.Once);
            _fixture.PerfilRiscoFactoryMock.Verify(x => x.Criar(It.IsAny<CalculoParaPerfilRiscoEnum>(), metodoCalculoMock.Object), Times.Once);
        }
    }
}
