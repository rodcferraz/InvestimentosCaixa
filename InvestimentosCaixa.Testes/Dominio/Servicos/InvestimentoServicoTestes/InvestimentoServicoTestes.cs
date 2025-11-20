using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.InvestimentoServicoTestes
{
    public class InvestimentoServicoTestes : IClassFixture<InvestimentoServicoFixture>
    {
        private readonly InvestimentoServicoFixture _fixture;

        public InvestimentoServicoTestes()
        {
            _fixture = new InvestimentoServicoFixture();
        }

        [Fact]
        public async Task CadastrarInvestimentoAsync_ComDadosValidos_RetornaId()
        {
            // Arrange
            var investimentoDto = new InvestimentoDTOBaseRequest
            {
                IdCliente = 1,
                IdProduto = 1,
                Valor = 1000.00m,
            };

            var investimento = new Investimento()
            {
                Id = 1,
                IdCliente = 1,
                IdProduto = 1,
                Valor = 1000.00m,
                Data = DateTime.UtcNow
            };

            var investimentoDtoResponse = new InvestimentoDTOResponse()
            {
                Id = 1,
                Tipo = "CDB",
                Rentabilidade = 0.12m,
                Valor = 1000.00m,
                Data = DateTime.UtcNow.ToString()
            };

            _fixture.InvestimentoMapperMock
                .Setup(m => m.ToBaseEntity(It.IsAny<InvestimentoDTOBaseRequest>()))
                .Returns(investimento);

            _fixture.InvestimentoRepositorioMock
                .Setup(r => r.AdicionarAsync(It.IsAny<Investimento>()))
                .ReturnsAsync(investimento);

            _fixture.InvestimentoMapperMock
                .Setup(m => m.ToDtoResponse(It.IsAny<Investimento>()))
                .Returns(investimentoDtoResponse);

            // Act
            var resultado = await _fixture.Servico.CadastrarInvestimentoAsync(investimentoDto);

            // Assert
            Assert.Equal(1, resultado.Id);
            _fixture.InvestimentoMapperMock.Verify(m => m.ToBaseEntity(investimentoDto), Times.Once);
            _fixture.InvestimentoRepositorioMock.Verify(r => r.AdicionarAsync(investimento), Times.Once);
            _fixture.InvestimentoMapperMock.Verify(m => m.ToDtoResponse(investimento), Times.Once);
        }

        [Fact]
        public async Task ListarInvestimentosPorClienteAsync_QuandoEncontraInvestimentos_RetornaLista()
        {
            // Arrange
            int idCliente = 1;
            var investimentosEntities = new List<Investimento>
            {
                new Investimento { Id = 1, IdCliente = idCliente },
                new Investimento { Id = 2, IdCliente = idCliente }
            };

            var investimentosDto = new List<InvestimentoDTOResponse>
            {
                new InvestimentoDTOResponse { Id = 1 },
                new InvestimentoDTOResponse { Id = 2 }
            };

            _fixture.InvestimentoRepositorioMock
                .Setup(r => r.ListarInvestimentosPorClienteAsync(It.IsAny<int>()))
                .ReturnsAsync(investimentosEntities);

            _fixture.InvestimentoMapperMock
                .Setup(m => m.ToDtoResponseList(It.IsAny<List<Investimento>>()))
                .Returns(investimentosDto);

            // Act
            var resultado = await _fixture.Servico.ListarInvestimentosPorClienteAsync(idCliente);

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(1, resultado[0].Id);
            Assert.Equal(2, resultado[1].Id);

            _fixture.InvestimentoRepositorioMock.Verify(r => r.ListarInvestimentosPorClienteAsync(idCliente), Times.Once);
            _fixture.InvestimentoMapperMock.Verify(m => m.ToDtoResponseList(investimentosEntities), Times.Once);
        }

        [Fact]
        public async Task ListarInvestimentosPorClienteAsync_QuandoNaoEncontraInvestimentos_RetornaListaVazia()
        {
            // Arrange
            int idCliente = 999;
            var listaVazia = new List<Investimento>();

            _fixture.InvestimentoRepositorioMock
                .Setup(r => r.ListarInvestimentosPorClienteAsync(It.IsAny<int>()))
                .ReturnsAsync(listaVazia);

            _fixture.InvestimentoMapperMock
                .Setup(m => m.ToDtoResponseList(It.IsAny<List<Investimento>>()))
                .Returns(new List<InvestimentoDTOResponse>());

            // Act
            var resultado = await _fixture.Servico.ListarInvestimentosPorClienteAsync(idCliente);

            // Assert
            Assert.Empty(resultado);
            _fixture.InvestimentoRepositorioMock.Verify(r => r.ListarInvestimentosPorClienteAsync(idCliente), Times.Once);
        }
    }
}
