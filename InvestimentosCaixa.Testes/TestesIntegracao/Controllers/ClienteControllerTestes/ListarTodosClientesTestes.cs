using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes.Servico;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes.Servicos;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes
{
    public class ListarTodosClientesTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public ListarTodosClientesTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task ListarTodosClientes_QuandoExistemClientesAtivos_RetornaOk()
        {
            using (var scope = _factory.Services.CreateScope())
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

                db.Clientes.Add(new Cliente
                {
                    Nome = "Cliente 2",
                    Email = "neide@gmail.com",
                    SenhaHash = "123",
                    Liquidez = 10,
                    Ativo = true
                });

                await db.SaveChangesAsync();
            }

            _client.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue("Teste");

            var response = await _client.GetAsync("/listar-clientes");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ListarTodosClientes_QuandoNaoExistemClientesAtivos_RetornaNotFound()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                // Apenas clientes inativos
                db.Clientes.Add(new Cliente
                {
                    Nome = "Cliente 1",
                    Email = "rodrigo@gmail.com",
                    SenhaHash = "123",
                    Liquidez = 2,
                    Ativo = false
                });

                await db.SaveChangesAsync();
            }

            _client.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue("Teste");

            var response = await _client.GetAsync("/listar-clientes");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ListarTodosClientes_QuandoServicoLancaExcecao_RetornaStatus500()
        {
            var factoryFake = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IClienteServico, ClienteServicoErroFake>(services);
                });
            });

            var cliente = factoryFake.CreateClient();
            cliente.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");

            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IClienteServico, ClienteServicoErroFake>(services);
                });
            });

            var response = await cliente.GetAsync("/listar-clientes");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
