using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControlerTestes.Mapper;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes.Servico;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes
{
    public class AtualizarClienteTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public AtualizarClienteTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AtualizarCliente_QuandoIdDiferente_RetornaBadRequest()
        {
            // Arrange
            var request = new ClienteDTORequest
            {
                Id = 2,
                Nome = "Cliente 1",
                Email = "rodrigo@gmail.com",
                Liquidez = 2
            };

            // Act
            var response = await _client.PutAsJsonAsync("/atualizar-cliente/1", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarCliente_QuandoNomeJaExiste_RetornaBadRequest()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Clientes.Add(new Cliente
                {
                    Nome = "ClienteExistente",
                    Email = "existente@teste.com",
                    SenhaHash = "123",
                    Liquidez = 10,
                    Ativo = true
                });

                await db.SaveChangesAsync();
            }

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");

            var request = new ClienteDTORequest
            {
                Id = 2,
                Nome = "ClienteExistente",
                Email = "teste@novo.com",
                Liquidez = 10
            };

            // Act
            var response = await _client.PutAsJsonAsync("/atualizar-cliente/1", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarCliente_QuandoNaoEncontrado_RetornaNotFound()
        {
            // Arrange
            var factoryCustom = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IClienteServico, ClienteServicoNuloFake>(services);
                });
            });

            var client = factoryCustom.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");

            var request = new ClienteDTORequest
            {
                Id = 1,
                Nome = "Cliente Novo",
                Email = "novo@teste.com",
                Liquidez = 1
            };

            // Act
            var response = await client.PutAsJsonAsync("/atualizar-cliente/1", request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarCliente_QuandoDadosValidos_RetornaOk()
        {
            // Arrange - Adiciona um cliente real no DB para permitir atualização
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Clientes.Add(new Cliente
                {
                    Id = 1,
                    Nome = "Antigo",
                    Email = "antigo@teste.com",
                    SenhaHash = "abc",
                    Liquidez = 1,
                    Ativo = true
                });

                await db.SaveChangesAsync();
            }

            var request = new ClienteDTORequest
            {
                Id = 1,
                Nome = "Cliente Atualizado",
                Email = "novo@teste.com",
                Liquidez = 1
            };

            // Act
            var response = await _client.PutAsJsonAsync("/atualizar-cliente/1", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarCliente_QuandoEnumErro_RetornaBadRequest()
        {
            // Arrange — força erro de enum

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
            }

            var request = new ClienteDTORequest
            {
                Id = 1,
                Nome = "Cliente 1",
                Email = "rodrigo@teste.com",
                Liquidez = 10
            };

            // Act
            var response = await _client.PutAsJsonAsync("/atualizar-cliente/1", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarCliente_QuandoErroInterno_Retorna500()
        {
            // Arrange — força exception geral
            var factoryCustom = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IClienteServico, ClienteServicoErroFake>(services);
                });
            });

            var client = factoryCustom.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");

            var request = new ClienteDTORequest
            {
                Id = 1,
                Nome = "Erro",
                Email = "erro@teste.com",
                Liquidez = 1
            };

            // Act
            var response = await client.PutAsJsonAsync("/atualizar-cliente/1", request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
