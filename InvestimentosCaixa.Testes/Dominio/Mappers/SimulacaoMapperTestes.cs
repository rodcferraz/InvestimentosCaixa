using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers;

namespace InvestimentosCaixa.Testes.Dominio.Mappers
{
    public class SimulacaoMapperTestes
    {
        private readonly SimulacaoMapper _mapper;

        public SimulacaoMapperTestes()
        {
            _mapper = new SimulacaoMapper();
        }

        [Fact]
        public void ToDtoResponse_QuandoExecutado_DeveMapearCorretamente()
        {
            // Arrange
            var produto = new Produto
            {
                Nome = "CDB Caixa 2026",
                Rentabilidade = 0.12m
            };

            var dataSimulacao = DateTime.UtcNow;

            var simulacao = new Simulacao
            {
                Id = 1,
                Produto = produto,
                IdCliente = 10,
                ValorInvestido = 1000m,
                PrazoMeses = 12,
                DataSimulacao = dataSimulacao
            };

            // Act
            var dto = _mapper.ToDtoResponse(simulacao);

            // Assert
            Assert.Equal(1, dto.Id);
            Assert.Equal("CDB Caixa 2026", dto.Produto);
            Assert.Equal(10, dto.ClienteId);
            Assert.Equal(1000m, dto.ValorInvestido);
            Assert.Equal(12, dto.PrazoMeses);
            Assert.Equal(dataSimulacao.ToString("yyyy-MM-ddTHH:mm:ssZ"), dto.DataSimulacao);

            var esperado = 1000m + 1000m * dto.PrazoMeses *(produto.Rentabilidade / 12);
            esperado = Math.Round(esperado, 2);

            Assert.Equal(esperado, dto.ValorFinal);
        }

        [Fact]
        public void ToDtoResponseList_QuandoExecutado_DeveConverterLista()
        {
            // Arrange
            var produto = new Produto { Nome = "CDB Caixa 2026", Rentabilidade = 0.10m };

            var dataSimulacao = DateTime.UtcNow;

            var simulacoes = new List<Simulacao>
            {
                new Simulacao { Id = 1, Produto = produto, IdCliente = 1, ValorInvestido = 1000, PrazoMeses = 6, DataSimulacao = dataSimulacao }
            };

            // Act
            var lista = _mapper.ToDtoResponseList(simulacoes);

            // Assert
            Assert.Equal(1, lista.Count);

            //Simulacao 1
            Assert.Equal(1, lista[0].Id);
            Assert.Equal("CDB Caixa 2026", lista[0].Produto);
            Assert.Equal(1, lista[0].ClienteId);
            Assert.Equal(1000m, lista[0].ValorInvestido);
            Assert.Equal(6, lista[0].PrazoMeses);
            Assert.Equal(dataSimulacao.ToString("yyyy-MM-ddTHH:mm:ssZ"), lista[0].DataSimulacao);

            var taxaMensal = produto.Rentabilidade / 12;

            var valorFinal = simulacoes[0].ValorInvestido * (1 + taxaMensal * simulacoes[0].PrazoMeses);

            Assert.Equal(Math.Round(valorFinal, 2), lista[0].ValorFinal);
        }

        [Fact]
        public void ToDtoResponseList_QuandoListaForVazia_DeveRetornarVazio()
        {
            // Arrange
            var simulacoes = new List<Simulacao>();

            // Act
            var lista = _mapper.ToDtoResponseList(simulacoes);

            // Assert
            Assert.NotNull(lista);
            Assert.Empty(lista);
        }

        [Fact]
        public void ToDtoResponseList_QuandoListaForNull_DeveRetornarListaVazia()
        {
            // Act
            var lista = _mapper.ToDtoResponseList(null);

            // Assert
            Assert.NotNull(lista);
            Assert.Empty(lista);
        }

        [Fact]
        public void ToDtoProdutoDiaList_DeveAgruparPorProdutoEData()
        {
            // Arrange
            var produtoA = new Produto { Nome = "CDB Caixa 2026", Rentabilidade = 0.1m };
            var produtoB = new Produto { Nome = "Tesouro Selic", Rentabilidade = 0.05m };

            var simulacoes = new List<Simulacao>
        {
            new Simulacao { Produto = produtoA, ValorInvestido = 1000m, DataSimulacao = new DateTime(2025,11,18) },
            new Simulacao { Produto = produtoA, ValorInvestido = 3000m, DataSimulacao = new DateTime(2025,11,18) },

            new Simulacao { Produto = produtoB, ValorInvestido = 500m, DataSimulacao = new DateTime(2025,11,19) },
            new Simulacao { Produto = produtoB, ValorInvestido = 1500m, DataSimulacao = new DateTime(2025,11,19) },
        };

            // Act
            var lista = _mapper.ToDtoProdutoDiaList(simulacoes);

            // Assert
            Assert.Equal(2, lista.Count);

            var grupoA = lista.First(x => x.Produto == "CDB Caixa 2026");
            Assert.Equal("2025-11-18", grupoA.Data);
            Assert.Equal(2, grupoA.QuantidadeSimulacoes);
            Assert.Equal(2000m, grupoA.MediaValorFinal);

            var grupoB = lista.First(x => x.Produto == "Tesouro Selic");
            Assert.Equal("2025-11-19", grupoB.Data);
            Assert.Equal(2, grupoB.QuantidadeSimulacoes);
            Assert.Equal(1000m, grupoB.MediaValorFinal);
        }

        [Fact]
        public void ToDtoProdutoDiaList_ListaVazia_DeveRetornarVazio()
        {
            // Act
            var lista = _mapper.ToDtoProdutoDiaList(new List<Simulacao>());

            // Assert
            Assert.Empty(lista);
        }

    }
}
