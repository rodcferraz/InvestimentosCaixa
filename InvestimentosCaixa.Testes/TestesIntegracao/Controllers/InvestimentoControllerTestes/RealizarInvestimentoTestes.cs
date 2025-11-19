using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.InvestimentoControllerTestes.Servicos;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.InvestimentoControllerTestes
{
    public class RealizarInvestimentoTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public RealizarInvestimentoTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RealizarInvestimento_QuandoDadosValidos_RetornaOk()
        {
            // Arrange: preparar dados no banco (cliente e produto, se necessário)
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                var cliente = new Cliente
                {
                    Nome = "Cliente 1",
                    Email = "rodrigo@gmail.com",
                    SenhaHash = "senha",
                    Liquidez = 10,
                    Ativo = true
                };

                var produto = new Produto
                {
                    Nome = "CDB",
                    Tipo = 1,
                    Rentabilidade = 0.12m,
                    Risco = 1
                };

                db.Clientes.Add(cliente);
                db.Produtos.Add(produto);

                await db.SaveChangesAsync();
            }

            var request = new InvestimentoDTOBaseRequest
            {
                IdCliente = 1,
                IdProduto = 1,
                Valor = 1000,
            };

            // Act
            var response = await _client.PostAsJsonAsync("/investimento", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Investimento realizado com sucesso", body);
        }

        [Fact]
        public async Task RealizarInvestimento_QuandoServicoLancaExcecao_RetornaStatus500()
        {
            // Arrange — substituir serviço real por fake com erro
            // Cria uma instância isolada da factory substituindo o serviço
            var factoryFake = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IInvestimentoServico, InvestimentoServicoErroFake>(services);
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
                    SenhaHash = "senha",
                    Liquidez = 10,
                    Ativo = true
                };

                var produto = new Produto
                {
                    Nome = "CDB",
                    Tipo = 1,
                    Rentabilidade = 0.12m,
                    Risco = 1
                };

                db.Clientes.Add(cliente);
                db.Produtos.Add(produto);

                await db.SaveChangesAsync();
            }

            var request = new InvestimentoDTOBaseRequest
            {
                IdCliente = 1,
                IdProduto = 1,
                Valor = 500,
            };

            // Act
            var response = await client.PostAsJsonAsync("/investimento", request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task RealizarInvestimento_QuandoClienteNaoExiste_RetornaNotFound()
        {
            //Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Produtos.Add(new Produto
                {
                    Nome = "CDB",
                    Tipo = 1,
                    Rentabilidade = 0.12m,
                    Risco = 1
                });

                await db.SaveChangesAsync();
            }

            var request = new InvestimentoDTOBaseRequest
            {
                IdCliente = 999,  // não existe
                IdProduto = 1,
                Valor = 1000
            };

            //Act
            var response = await _client.PostAsJsonAsync("/investimento", request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Cliente não encontrado", body);
        }

        [Fact]
        public async Task RealizarInvestimento_QuandoProdutoNaoExiste_RetornaNotFound()
        {
            //Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Clientes.Add(new Cliente
                {
                    Nome = "Cliente Rodrigo",
                    Email = "rodrigo@gmail.com",
                    SenhaHash = "abc",
                    Liquidez = 10,
                    Ativo = true
                });

                await db.SaveChangesAsync();
            }

            var request = new InvestimentoDTOBaseRequest
            {
                IdCliente = 1,
                IdProduto = 999,  // não existe
                Valor = 500
            };

            //Act
            var response = await _client.PostAsJsonAsync("/investimento", request);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Produto não encontrado", body);
        }
    }
}
