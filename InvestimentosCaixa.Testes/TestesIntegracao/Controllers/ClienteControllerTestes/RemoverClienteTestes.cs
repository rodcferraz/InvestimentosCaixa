
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes.Servico;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes.Servicos;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes
{
    public class RemoverClienteTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public RemoverClienteTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RemoverCliente_QuandoClienteExistente_DeveRetornar200()
        {
            int clienteId;

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                var cliente = new Cliente
                {
                    Nome = "Cliente 1",
                    Email = "rodrigo@gmail.com",
                    SenhaHash = "123",
                    Liquidez = 1,
                    Ativo = true
                };

                db.Clientes.Add(cliente);
                await db.SaveChangesAsync();

                clienteId = cliente.Id;
            }

            var response = await _client.DeleteAsync($"/remover-cliente/{clienteId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RemoverCliente_QuandoClienteNaoEncontrado_DeveRetornar404()
        {
            var response = await _client.DeleteAsync("/remover-cliente/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task RemoverCliente_QuandoLancaExcecao_DeveRetornar500()
        {
            int clienteId;

            var factoryFake = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IClienteServico, ClienteServicoErroFake>(services);
                });
            });

            var client = factoryFake.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");

            using (var scope = factoryFake.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                var cliente = new Cliente
                {
                    Nome = "Cliente 1",
                    Email = "rodrigo@gmail.com",
                    SenhaHash = "123",
                    Liquidez = 1,
                    Ativo = true
                };

                db.Clientes.Add(cliente);
                await db.SaveChangesAsync();

                clienteId = cliente.Id;
            }

            var response = await client.DeleteAsync($"/remover-cliente/{clienteId}");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
