using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers;

namespace InvestimentosCaixa.Testes.Dominio.Mappers
{
    public class TelemetriaMapperTestes
    {
        private readonly TelemetriaMapper _mapper = new TelemetriaMapper();

        private List<Telemetria> CriarListaFake()
        {
            return new List<Telemetria>
        {
            new Telemetria
            {
                NomeRota = "GET /produtos",
                TempoResposta = 120,
                DataRegistro = new DateTime(2025, 1, 10)
            },
            new Telemetria
            {
                NomeRota = "GET /produtos",
                TempoResposta = 240,
                DataRegistro = new DateTime(2025, 1, 11)
            },
            new Telemetria
            {
                NomeRota = "POST /clientes",
                TempoResposta = 300,
                DataRegistro = new DateTime(2025, 1, 12)
            }
        };
        }

        [Fact]
        public void ToDtoResponse_QuandoExecutado_DeveGerarServicosAgrupadosCorretamente()
        {
            // Arrange
            var telemetrias = CriarListaFake();

            // Act
            var result = _mapper.ToDtoResponse(telemetrias);

            // Assert
            Assert.Equal(2, result.Servicos.Count);

            var rotaProdutos = result.Servicos.First(x => x.Nome == "GET /produtos");
            Assert.Equal(2, rotaProdutos.QuantidadeChamadas);
            Assert.Equal(180, rotaProdutos.MediaTempoRespostaMs); 

            var rotaClientes = result.Servicos.First(x => x.Nome == "POST /clientes");
            Assert.Equal(1, rotaClientes.QuantidadeChamadas);
            Assert.Equal(300, rotaClientes.MediaTempoRespostaMs);
        }

        [Fact]
        public void ToDtoResponse_DeveGerarPeriodoCorreto()
        {
            // Arrange
            var telemetrias = CriarListaFake();

            // Act
            var result = _mapper.ToDtoResponse(telemetrias);

            // Assert
            Assert.Equal("2025-01-10", result.Periodo.Inicio);
            Assert.Equal("2025-01-12", result.Periodo.Fim);
        }

        [Fact]
        public void ToDtoResponse_DeveFuncionarComApenasUmRegistro()
        {
            // Arrange
            var telemetrias = new List<Telemetria>
            {
                new Telemetria
                {
                    NomeRota = "GET /produtos",
                    TempoResposta = 150,
                    DataRegistro = new DateTime(2025, 2, 5)
                }
            };

            // Act
            var result = _mapper.ToDtoResponse(telemetrias);

            // Assert
            Assert.Single(result.Servicos);

            var servico = result.Servicos.First();
            Assert.Equal("GET /produtos", servico.Nome);
            Assert.Equal(1, servico.QuantidadeChamadas);
            Assert.Equal(150, servico.MediaTempoRespostaMs);

            Assert.Equal("2025-02-05", result.Periodo.Inicio);
            Assert.Equal("2025-02-05", result.Periodo.Fim);
        }

        [Fact]
        public void ToDtoResponse_DeveCalcularMediaComNumerosInteiros()
        {
            // Arrange
            var telemetrias = new List<Telemetria>
        {
            new Telemetria { NomeRota = "GET /produtos", TempoResposta = 100, DataRegistro = DateTime.UtcNow },
            new Telemetria { NomeRota = "GET /produtos", TempoResposta = 101, DataRegistro = DateTime.UtcNow },
            new Telemetria { NomeRota = "GET /produtos", TempoResposta = 102, DataRegistro = DateTime.UtcNow }
        };

            // Act
            var result = _mapper.ToDtoResponse(telemetrias);

            // Média = (100 + 101 + 102) / 3 = 101
            var servico = result.Servicos.First();

            // Assert
            Assert.Equal(101, servico.MediaTempoRespostaMs);
            Assert.Equal(3, servico.QuantidadeChamadas);
        }

        [Fact]
        public void ToDtoResponse_DeveAgruparRotasDeNomeIgual()
        {
            // Arrange
            var telemetrias = new List<Telemetria>
        {
            new Telemetria { NomeRota = "POST /login", TempoResposta = 10, DataRegistro = DateTime.Now },
            new Telemetria { NomeRota = "POST /login", TempoResposta = 20, DataRegistro = DateTime.Now },
            new Telemetria { NomeRota = "POST /login", TempoResposta = 30, DataRegistro = DateTime.Now }
        };

            // Act
            var result = _mapper.ToDtoResponse(telemetrias);

            var servico = result.Servicos.Single();

            // Assert
            Assert.Equal("POST /login", servico.Nome);
            Assert.Equal(3, servico.QuantidadeChamadas);
            Assert.Equal(20, servico.MediaTempoRespostaMs);
        }
    }
}
