using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Builder;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Testes.Dominio.Builder
{
    public class SimulacaoInvestimentoBuilderTests
    {
        private Produto CriarProdutoValidoParaTeste()
        {
            return new Produto
            {
                Id = 1,
                Nome = "CDB Banco XP 2025",
                Tipo = (int)TipoProdutoEnum.CDB,
                Rentabilidade = 0.12m,
                Risco = (int)RiscoProdutoEnum.Moderado
            };
        }

        private SimulacaoInvestimentoDTORequest CriarDadosSimulacaoTeste()
        {
            return new SimulacaoInvestimentoDTORequest
            {
                Valor = 1000,
                PrazoMeses = 6
            };
        }

        [Fact]
        public void ComProdutoValidado_QuandoProdutoEhValido_DevePreencherDadosProdutoCorretamente()
        {
            // Arrange
            var produtoTeste = CriarProdutoValidoParaTeste();
            var builder = new SimulacaoInvestimentoBuilder(produtoTeste);

            // Act
            builder.ComProdutoValidado();

            // Assert
            Assert.NotNull(builder.ProdutoValidado);
            Assert.Equal(produtoTeste.Id, builder.ProdutoValidado.Id);
            Assert.Equal("CDB Banco XP 2025", builder.ProdutoValidado.Nome);
            Assert.Equal(0.12m, builder.ProdutoValidado.Rentabilidade);
            Assert.Equal("CDB", builder.ProdutoValidado.Tipo);
            Assert.Equal("Moderado", builder.ProdutoValidado.Risco);
        }

        [Fact]
        public void ComResultadoSimulacao_QuandoDadosSaoValidos_DeveCalcularRentabilidadeEValorFinal()
        {
            // Arrange
            var produto = CriarProdutoValidoParaTeste();
            var dadosSimulacao = CriarDadosSimulacaoTeste();
            var builder = new SimulacaoInvestimentoBuilder(produto);

            var rentabilidadeEsperada = 0.12m / 12 * 6;
            var valorFinalEsperado = 1000 * (1 + rentabilidadeEsperada);

            // Act
            builder.ComResultadoSimulacao(dadosSimulacao);

            // Assert
            Assert.NotNull(builder.ResultadoSimulacao);
            Assert.Equal(rentabilidadeEsperada, builder.ResultadoSimulacao.RentabilidadeEfetiva);
            Assert.Equal(valorFinalEsperado, builder.ResultadoSimulacao.ValorFinal);
            Assert.Equal(6, builder.ResultadoSimulacao.PrazoMeses);
        }

        [Fact]
        public void ComDataSimulacao_QuandoDataEhInformada_DeveArmazenarDataCorretamente()
        {
            // Arrange
            var produto = CriarProdutoValidoParaTeste();
            var data = DateTime.UtcNow;
            var simulacaoComData = new Simulacao
            {
                DataSimulacao = data
            };

            var builder = new SimulacaoInvestimentoBuilder(produto);

            // Act
            builder.ComDataSimulacao(simulacaoComData);

            // Assert
            Assert.Equal(data, builder.DataSimulacao);
        }

        [Fact]
        public void Build_QuandoTodosDadosEstaoPreenchidos_DeveRetornarObjetoCompleto()
        {
            // Arrange
            var produto = CriarProdutoValidoParaTeste();
            var dadosSimulacao = CriarDadosSimulacaoTeste();
            var dataSimulacao = new Simulacao { DataSimulacao = DateTime.Now };

            var builder = new SimulacaoInvestimentoBuilder(produto)
                .ComProdutoValidado()
                .ComResultadoSimulacao(dadosSimulacao)
                .ComDataSimulacao(dataSimulacao);

            // Act
            var resultado = builder.Build();

            // Assert
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.ProdutoValidado);
            Assert.NotNull(resultado.ResultadoSimulacao);
            Assert.Equal(dataSimulacao.DataSimulacao, resultado.DataSimulacao);
        }
    }
}
