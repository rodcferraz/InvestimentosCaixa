using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.TelemetriaControllerTestes
{
    public class ListarRelatoriosDeTelemetriaTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public ListarRelatoriosDeTelemetriaTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ListarRelatorioTelemetria_DeveRetornar200()
        {
            // Arrange – prepara registros fake de telemetria na base
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Telemetrias.Add(new Telemetria
                {
                    NomeRota = "/cliente",
                    TempoResposta = 120,
                    DataRegistro = DateTime.Now
                });

                db.Telemetrias.Add(new Telemetria
                {
                    NomeRota = "/cliente",
                    TempoResposta = 120,
                    DataRegistro = DateTime.Now
                });

                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/telemetria");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        //[Fact]
        //public async Task ListarRelatorioTelemetria_QuandoErro_DeveRetornar400()
        //{
        //    // Arrange

        //    //using (var scope = _factory.Services.CreateScope())
        //    //{
        //    //    var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

        //    //    db.Database.EnsureDeleted();
        //    //    // Isso faz qualquer consulta gerar exceção na execução do serviço

        //    //    await db.SaveChangesAsync();
        //    //}

        //    //// Act
        //    //var response = await _client.GetAsync("/telemetria");

        //    //// Assert
        //    //Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //}
    }
}
