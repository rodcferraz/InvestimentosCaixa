using InvestimentosCaixa.Api.Aplicacao.Servicos.Interfaces;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes.Servico;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes
{
    public class ExibirPerfilRiscoClienteTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public ExibirPerfilRiscoClienteTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ExibirPerfilRisco_Valido_DeveRetornar200()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Clientes.Add(new Cliente
                {
                    Nome = "Cliente Risco",
                    Email = "teste@teste.com",
                    SenhaHash = "123",
                    Liquidez = 10,
                    Ativo = true
                });

                await db.SaveChangesAsync();
            }

            var response = await _client.GetAsync("/perfil-risco/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ExibirPerfilRisco_QuandoConvertEnumException_DeveRetornar400()
        {
            // Cria uma instância isolada da factory substituindo o serviço

           
            var factoryFake = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, configBuilder) =>
                {
                    // Sobrescreve as configurações para este teste
                    var overrideConfig = new Dictionary<string, string?>
                    {
                        { "CalculoPerfilRisco", "PerfilFake" },
                        { "Jwt:Key", "ChaveDeTeste" },
                        { "Jwt:Issuer", "IssuerTeste" },
                        { "Jwt:Audience", "AudienceTeste" },
                        { "ChaveHash", "HashFake123" }
                    };

                    configBuilder.AddInMemoryCollection(overrideConfig);
                });
            });

            var client = factoryFake.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");

            using (var scope = factoryFake.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Clientes.Add(new Cliente
                {
                    Nome = "Cliente 1",
                    Email = "rodrigo@gmail.com",
                    SenhaHash = "123",
                    Liquidez = 10,
                    Ativo = true
                });

                await db.SaveChangesAsync();
            }

            var response = await client.GetAsync("/perfil-risco/1");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ExibirPerfilRisco_QuandoExcecaoGeral_DeveRetornar500()
        {
            var factoryFake = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IGerarPerfilClienteServico, GerarPerfilClienteServicoErroFake>(services);
                });
            });

            var client = factoryFake.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");


            var response = await client.GetAsync("/perfil-risco/1");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var msg = await response.Content.ReadAsStringAsync();
            Assert.Contains("erro interno", msg.ToLower());
        }
    }
}
