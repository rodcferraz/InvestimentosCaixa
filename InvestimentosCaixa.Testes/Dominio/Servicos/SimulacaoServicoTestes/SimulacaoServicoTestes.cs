using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.SimulacaoServicoTestes
{
    public class SimulacaoServicoTestes : IClassFixture<SimulacaoServicoFixture>
    {
        private readonly SimulacaoServicoFixture _fixture;

        public SimulacaoServicoTestes()
        {
            _fixture = new SimulacaoServicoFixture();
        }

        [Fact]
        public async Task SimularInvestimento_ComDadosValidos_RetornaSimulacaoComResultadosCalculados()
        {
            // Arrange
            var produto = new Produto
            {
                Id = 1,
                Nome = "LCI Caixa 2026",
                Tipo = (int)TipoProdutoEnum.LCI,
                Risco = (int)RiscoProdutoEnum.Baixo,
                Rentabilidade = 0.12m // 12% ao ano
            };

            var simulacaoInvestimento = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1,
                Valor = 1000m,
                PrazoMeses = 6
            };

            var simulacaoSalva = new Simulacao
            {
                Id = 123,
                ValorInvestido = 1000m,
                IdCliente = 1,
                IdProduto = 1,
            };

            _fixture.SimulacaoRepositorioMock
                .Setup(r => r.AdicionarAsync(It.IsAny<Simulacao>()))
                .ReturnsAsync(simulacaoSalva);

            // Act
            var resultado = await _fixture.Servico.SimularInvestimento(produto, simulacaoInvestimento);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.ProdutoValidado);
            Assert.NotNull(resultado.ResultadoSimulacao);

            // Verifica o produto validado
            Assert.Equal(1, resultado.ProdutoValidado.Id);
            Assert.Equal("LCI Caixa 2026", resultado.ProdutoValidado.Nome);
            Assert.Equal("LCI", resultado.ProdutoValidado.Tipo);
            Assert.Equal("Baixo", resultado.ProdutoValidado.Risco);
            Assert.Equal(0.12m, resultado.ProdutoValidado.Rentabilidade);

            // Verifica o resultado da simulação
            var rentabilidadeEsperada = 0.12m / 12 * 6; // 6% em 6 meses
            var valorFinalEsperado = 1000m * (1 + rentabilidadeEsperada); // 1060.00

            Assert.Equal(valorFinalEsperado, resultado.ResultadoSimulacao.ValorFinal);
            Assert.Equal(rentabilidadeEsperada, resultado.ResultadoSimulacao.RentabilidadeEfetiva);
            Assert.Equal(6, resultado.ResultadoSimulacao.PrazoMeses);

        }

        [Fact]
        public async Task SimularInvestimento_ComValorDecimal_ArredondaParaDuasCasasDecimais()
        {
            // Arrange
            var produto = new Produto
            {
                Id = 1,
                Nome = "Tesouro Direto",
                Tipo = (int)TipoProdutoEnum.TesouroSelic,
                Risco = (int)RiscoProdutoEnum.Moderado,
                Rentabilidade = 0.10m
            };

            var simulacaoInvestimento = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1,
                Valor = 1000.567m, // 3 casas decimais
                PrazoMeses = 12
            };

            var simulacaoSalva = new Simulacao { Id = 1 };

            _fixture.SimulacaoRepositorioMock
                .Setup(r => r.AdicionarAsync(It.IsAny<Simulacao>()))
                .ReturnsAsync(simulacaoSalva);

            // Act
            var resultado = await _fixture.Servico.SimularInvestimento(produto, simulacaoInvestimento);

            // Assert
            _fixture.SimulacaoRepositorioMock.Verify(r =>
                r.AdicionarAsync(It.Is<Simulacao>(s => s.ValorInvestido == 1000.57m)),
                Times.Once);
        }

        [Fact]
        public async Task SimularInvestimento_ComFalhaAoSalvar_RetornaNull()
        {
            // Arrange
            var produto = new Produto
            {
                Id = 1,
                Nome = "CDB",
                Tipo = (int)TipoProdutoEnum.CDB,
                Risco = (int)RiscoProdutoEnum.Baixo,
                Rentabilidade = 0.15m
            };

            var simulacaoInvestimento = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1,
                Valor = 1000m,
                PrazoMeses = 12
            };

            var simulacaoNaoSalva = new Simulacao { Id = 0 }; // ID 0 indica falha

            _fixture.SimulacaoRepositorioMock
                .Setup(r => r.AdicionarAsync(It.IsAny<Simulacao>()))
                .ReturnsAsync(simulacaoNaoSalva);

            // Act
            var resultado = await _fixture.Servico.SimularInvestimento(produto, simulacaoInvestimento);

            // Assert
            Assert.Null(resultado);
            _fixture.SimulacaoRepositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Simulacao>()), Times.Once);
        }

        [Theory]
        [InlineData(0.12, 1, 1000, 1010.00)]
        [InlineData(0.12, 6, 1000, 1060.00)]
        [InlineData(0.12, 12, 1000, 1120.00)]
        [InlineData(0.24, 6, 1000, 1120.00)]
        [InlineData(0.06, 12, 2000, 2120.00)]
        public async Task SimularInvestimento_ComDiferentesParametros_CalculaResultadoCorreto(
            decimal rentabilidadeAnual,
            int prazoMeses,
            decimal valorInvestido,
            decimal valorFinalEsperado)
        {
            // Arrange
            var produto = new Produto
            {
                Id = 1,
                Nome = "Produto Calculo",
                Tipo = (int)TipoProdutoEnum.TesouroSelic,
                Risco = (int)RiscoProdutoEnum.Moderado,
                Rentabilidade = rentabilidadeAnual
            };

            var simulacaoInvestimento = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1,
                Valor = valorInvestido,
                PrazoMeses = prazoMeses
            };

            var simulacaoSalva = new Simulacao { Id = 1 };

            _fixture.SimulacaoRepositorioMock
                .Setup(r => r.AdicionarAsync(It.IsAny<Simulacao>()))
                .ReturnsAsync(simulacaoSalva);

            // Act
            var resultado = await _fixture.Servico.SimularInvestimento(produto, simulacaoInvestimento);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.ResultadoSimulacao);
            Assert.Equal(valorFinalEsperado, resultado.ResultadoSimulacao.ValorFinal);

            var rentabilidadeEsperada = rentabilidadeAnual / 12 * prazoMeses;
            Assert.Equal(rentabilidadeEsperada, resultado.ResultadoSimulacao.RentabilidadeEfetiva);
            Assert.Equal(prazoMeses, resultado.ResultadoSimulacao.PrazoMeses);
        }

        [Theory]
        [InlineData(1, "TesouroSelic", "Baixo")]
        [InlineData(3, "LCI", "Baixo")]
        [InlineData(5, "TesouroIPCA", "Moderado")]
        [InlineData(7, "Acoes", "Alto")]
        [InlineData(9, "Criptomoeda", "Alto")]
        public async Task SimularInvestimento_ComDiferentesTiposERiscos_MapeiaCorretamente(
            int tipoProduto,
            string tipoEsperado,
            string riscoEsperado)
        {
            // Arrange
            var produto = new Produto
            {
                Id = 1,
                Nome = $"Produto {tipoEsperado}",
                Tipo = tipoProduto, // Tipo do produto
                Risco = riscoEsperado switch // Converte string para valor enum do risco
                {
                    "Baixo" => (int)RiscoProdutoEnum.Baixo,
                    "Moderado" => (int)RiscoProdutoEnum.Moderado,
                    "Alto" => (int)RiscoProdutoEnum.Alto,
                    _ => (int)RiscoProdutoEnum.Baixo
                },
                Rentabilidade = 0.10m
            };

            var simulacaoInvestimento = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1,
                Valor = 1000m,
                PrazoMeses = 12
            };

            var simulacaoSalva = new Simulacao { Id = 1 };

            _fixture.SimulacaoRepositorioMock
                .Setup(r => r.AdicionarAsync(It.IsAny<Simulacao>()))
                .ReturnsAsync(simulacaoSalva);

            // Act
            var resultado = await _fixture.Servico.SimularInvestimento(produto, simulacaoInvestimento);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.ProdutoValidado);
            Assert.Equal(tipoEsperado, resultado.ProdutoValidado.Tipo);
            Assert.Equal(riscoEsperado, resultado.ProdutoValidado.Risco);
        }
    }
}
