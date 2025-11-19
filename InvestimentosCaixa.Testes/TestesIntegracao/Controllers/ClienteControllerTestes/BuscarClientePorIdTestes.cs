using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes.Servico;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes
{
    public class BuscarClientePorIdTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public BuscarClientePorIdTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task BuscarClientePorId_QuandoClienteExiste_DeveRetornar200()
        {
            // Arrange
            int idCliente;
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                var cliente = new Cliente
                {
                    Nome = "Cliente 1",
                    Email = "rodrigo@gmail.com",
                    SenhaHash = "123",
                    Liquidez = 10,
                    Ativo = true
                };

                db.Clientes.Add(cliente);
                await db.SaveChangesAsync();

                idCliente = cliente.Id;
            }

            // Act
            var response = await _client.GetAsync($"/buscar-cliente/{idCliente}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var resultado = await response.Content.ReadFromJsonAsync<ClienteDTOResponse>();
            Assert.NotNull(resultado);
            Assert.Equal(idCliente, resultado.Id);
            Assert.Equal("Cliente 1", resultado.Nome);
        }

        [Fact]
        public async Task BuscarClientePorId_QuandoClienteNaoExiste_DeveRetornar404()
        {
            // Arrange — não adiciona nada no banco
            var idInexistente = 9999;

            // Act
            var response = await _client.GetAsync($"/buscar-cliente/{idInexistente}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task BuscarClientePorId_QuandoServicoLancaExcecao_DeveRetornar500()
        {
            // Substitui serviço real por fake que lança exceção
            var factoryComErro = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IClienteServico, ClienteServicoErroFake>(services);
                });
            });

            var clientErro = factoryComErro.CreateClient();
            clientErro.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Teste");

            // Arrange — cria banco com cliente válido
            int idCliente;
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                var cliente = new Cliente
                {
                    Nome = "Cliente Excecao",
                    Email = "erro@teste.com",
                    SenhaHash = "123",
                    Liquidez = 10,
                    Ativo = true
                };

                db.Clientes.Add(cliente);
                await db.SaveChangesAsync();

                idCliente = cliente.Id;
            }

            // Act
            var response = await clientErro.GetAsync($"/buscar-cliente/{idCliente}");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
