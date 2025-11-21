using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Enums;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.PerfilRiscoClientePersonalizadoServicoTestes
{
    public class PerfilRiscoClientePersonalizadoServicoTestes : IClassFixture<PerfilRiscoClientePersonalizadoFixture>
    {
        private readonly PerfilRiscoClientePersonalizadoFixture _fixture;

        public PerfilRiscoClientePersonalizadoServicoTestes()
        {
            _fixture = new PerfilRiscoClientePersonalizadoFixture();
        }

        [Theory]
        [InlineData(10, 20, 20, 16.0, PerfilRiscoClienteEnum.Conservador)]
        [InlineData(30, 50, 50, 42.0, PerfilRiscoClienteEnum.Moderado)]
        [InlineData(80, 80, 80, 80.0, PerfilRiscoClienteEnum.Agressivo)]
        [InlineData(20, 20, 20, 20.0, PerfilRiscoClienteEnum.Conservador)]
        [InlineData(60, 60, 60, 60.0, PerfilRiscoClienteEnum.Moderado)]
        public async Task CalcularPerfilRiscoCliente_ComDiferentesPontuacoes_RetornaPerfilCorreto(
            int pontuacaoCarteira,
            int pontuacaoMovimentacoes,
            int pontuacaoLiquidez,
            decimal pontuacaoTotalEsperada,
            PerfilRiscoClienteEnum perfilEsperado)
        {
            // Arrange
            int idCliente = 1;
            var investimentos = new List<InvestimentoDTOResponse>
            {
                new InvestimentoDTOResponse { Valor = 1000 },
                new InvestimentoDTOResponse { Valor = 2000 }
            };

            var cliente = new ClienteDTOResponse { Liquidez = (int)PerfilRiscoClienteEnum.Moderado };

            _fixture.InvestimentoServicoMock
                .Setup(s => s.ListarInvestimentosPorClienteAsync(It.IsAny<int>()))
                .ReturnsAsync(investimentos);

            _fixture.ClienteServicoMock
                .Setup(s => s.DetalhesClienteAsync(It.IsAny<int>()))
                .ReturnsAsync(cliente);

            _fixture.PerfilPontuacaoClienteServicoMock
                .Setup(s => s.GerarPerfilCarteiraCliente(3000)) // 1000 + 2000
                .Returns(pontuacaoCarteira);

            _fixture.PerfilPontuacaoClienteServicoMock
                .Setup(s => s.GerarPerfilMovimentacoesCliente(2)) // 2 investimentos
                .Returns(pontuacaoMovimentacoes);

            _fixture.PerfilPontuacaoClienteServicoMock
                .Setup(s => s.GerarPerfilLiquidezCliente(PerfilRiscoClienteEnum.Moderado))
                .Returns(pontuacaoLiquidez);

            // Act
            var (perfil, pontuacaoTotal) = await _fixture.Servico.CalcularPerfilRiscoCliente(idCliente);

            // Assert
            Assert.Equal(perfilEsperado, perfil);
            Assert.Equal(pontuacaoTotalEsperada, pontuacaoTotal);
        }

        [Theory]
        [InlineData(PerfilRiscoClienteEnum.Conservador, 20.0)]
        [InlineData(PerfilRiscoClienteEnum.Moderado, 60.0)]
        [InlineData(PerfilRiscoClienteEnum.Agressivo, 80.0)]
        public async Task CalcularPerfilRiscoCliente_QuandoNaoHaInvestimento_RetornaLiquidezDoCliente(
            PerfilRiscoClienteEnum perfilCliente,
            decimal pontuacaoTotalEsperada)
        {
            // Arrange
            int idCliente = 1;
            var investimentos = new List<InvestimentoDTOResponse> { }; ;

            var cliente = new ClienteDTOResponse { Liquidez = (int)perfilCliente };

            _fixture.InvestimentoServicoMock
                .Setup(s => s.ListarInvestimentosPorClienteAsync(It.IsAny<int>()))
                .ReturnsAsync(investimentos);

            _fixture.ClienteServicoMock
                .Setup(s => s.DetalhesClienteAsync(It.IsAny<int>()))
                .ReturnsAsync(cliente);

            // Act
            var (perfil, pontuacaoTotal) = await _fixture.Servico.CalcularPerfilRiscoCliente(idCliente);

            // Assert
            Assert.Equal(perfilCliente, perfil);
            Assert.Equal(pontuacaoTotalEsperada, pontuacaoTotal);
        }

        [Fact]
        public async Task CalcularPerfilRiscoCliente_ComClienteComMuitosInvestimentos_RetornaPerfilAgressivo()
        {
            // Arrange
            int idCliente = 3;
            var investimentos = Enumerable.Range(1, 10)
                .Select(i => new InvestimentoDTOResponse { Valor = 10000 * i })
                .ToList();

            var cliente = new ClienteDTOResponse { Liquidez = (int)PerfilRiscoClienteEnum.Agressivo };

            _fixture.InvestimentoServicoMock
                .Setup(s => s.ListarInvestimentosPorClienteAsync(idCliente))
                .ReturnsAsync(investimentos);

            _fixture.ClienteServicoMock
                .Setup(s => s.DetalhesClienteAsync(idCliente))
                .ReturnsAsync(cliente);

            _fixture.PerfilPontuacaoClienteServicoMock
                .Setup(s => s.GerarPerfilCarteiraCliente(550000))
                .Returns(100);

            _fixture.PerfilPontuacaoClienteServicoMock
                .Setup(s => s.GerarPerfilMovimentacoesCliente(10))
                .Returns(80);

            _fixture.PerfilPontuacaoClienteServicoMock
                .Setup(s => s.GerarPerfilLiquidezCliente(PerfilRiscoClienteEnum.Agressivo))
                .Returns(80);

            // Act
            var (perfil, pontuacaoTotal) = await _fixture.Servico.CalcularPerfilRiscoCliente(idCliente);

            // Assert
            Assert.Equal(PerfilRiscoClienteEnum.Agressivo, perfil);
            Assert.Equal(88.0m, pontuacaoTotal);
        }

        [Fact]
        public async Task CalcularPerfilRiscoCliente_VerificaChamadasDosServicos()
        {
            // Arrange
            int idCliente = 6;
            var investimentos = new List<InvestimentoDTOResponse>
        {
            new InvestimentoDTOResponse { Valor = 5000 },
            new InvestimentoDTOResponse { Valor = 15000 }
        };

            var cliente = new ClienteDTOResponse { Liquidez = (int)PerfilRiscoClienteEnum.Moderado };

            _fixture.InvestimentoServicoMock
                .Setup(s => s.ListarInvestimentosPorClienteAsync(idCliente))
                .ReturnsAsync(investimentos);

            _fixture.ClienteServicoMock
                .Setup(s => s.DetalhesClienteAsync(idCliente))
                .ReturnsAsync(cliente);

            _fixture.PerfilPontuacaoClienteServicoMock
                .Setup(s => s.GerarPerfilCarteiraCliente(20000))
                .Returns(30);

            _fixture.PerfilPontuacaoClienteServicoMock
                .Setup(s => s.GerarPerfilMovimentacoesCliente(2))
                .Returns(20);

            _fixture.PerfilPontuacaoClienteServicoMock
                .Setup(s => s.GerarPerfilLiquidezCliente(PerfilRiscoClienteEnum.Moderado))
                .Returns(50);

            // Act
            var result = await _fixture.Servico.CalcularPerfilRiscoCliente(idCliente);

            // Assert
            _fixture.InvestimentoServicoMock.Verify(s => s.ListarInvestimentosPorClienteAsync(idCliente), Times.Once);
            _fixture.ClienteServicoMock.Verify(s => s.DetalhesClienteAsync(idCliente), Times.Once);
            _fixture.PerfilPontuacaoClienteServicoMock.Verify(s => s.GerarPerfilCarteiraCliente(20000), Times.Once);
            _fixture.PerfilPontuacaoClienteServicoMock.Verify(s => s.GerarPerfilMovimentacoesCliente(2), Times.Once);
            _fixture.PerfilPontuacaoClienteServicoMock.Verify(s => s.GerarPerfilLiquidezCliente(PerfilRiscoClienteEnum.Moderado), Times.Once);
        }
    }
}
