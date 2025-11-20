using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes.Servicos;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes
{
    public class SimularInvestimentoTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public SimularInvestimentoTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task SimularInvestimento_DeveRetornar200()
        {
            // Arrange
            var request = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1,
                TipoProduto = "CDB",
                Valor = 1000m,
                PrazoMeses = 12
            };

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Clientes.Add(new Cliente { Nome = "Cliente Ativo 1", Email = "rodrigo@gmail.com", SenhaHash = "1234", Liquidez = 1, Ativo = true });
                db.Clientes.Add(new Cliente { Nome = "Cliente Inativo", Email = "rodrigo@gmail.com", SenhaHash = "1234", Liquidez = 1, Ativo = false });
                db.Clientes.Add(new Cliente { Nome = "Cliente Ativo 2", Email = "rodrigo@gmail.com", SenhaHash = "1234", Liquidez = 1, Ativo = true });

                db.Produtos.Add(new Produto
                {
                    Id = 10,
                    Nome = "CDB",
                    Tipo = (int)TipoProdutoEnum.CDB,
                    Rentabilidade = 0.12m,
                    Risco = (int)RiscoProdutoEnum.Baixo
                });

                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.PostAsJsonAsync("/simular-investimento", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<SimulacaoInvestimentoDTOResponse>();
            Assert.NotNull(result);

            Assert.NotNull(result.ProdutoValidado);
            Assert.NotNull(result.ResultadoSimulacao);
        }

        [Fact]
        public async Task SimularInvestimento_QuandoClienteNaoExiste_RetornaNotFound()
        {
            // Arrange: garantir banco vazio
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Clientes.RemoveRange(db.Clientes);
                await db.SaveChangesAsync();
            }

            var request = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1, // não existe
                TipoProduto = "CDB",
                Valor = 1000,
                PrazoMeses = 12
            };

            var response = await _client.PostAsJsonAsync("/simular-investimento", request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SimularInvestimento_QuandoProdutoNaoExiste_RetornaNotFound()
        {
            // Arrange: garantir banco vazio
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Clientes.Add(new Cliente { Nome = "Cliente Ativo 1", Email = "rodrigo@gmail.com", SenhaHash = "1234", Liquidez = 1, Ativo = true });

                db.Produtos.RemoveRange(db.Produtos);

                await db.SaveChangesAsync();
            }

            var request = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1,
                TipoProduto = "CDB",
                Valor = 1000,
                PrazoMeses = 12
            };

            var response = await _client.PostAsJsonAsync("/simular-investimento", request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SimularInvestimento_QuandoTipoProdutoInvalido_RetornaBadRequest()
        {
            var request = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1,
                TipoProduto = "TIPOINVALIDO",
                Valor = 1000,
                PrazoMeses = 12
            };

            var response = await _client.PostAsJsonAsync("/simular-investimento", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SimularInvestimento_QuandoServicoRetornaNulo_RetornaBadRequest()
        {
            // Cria uma instância isolada da factory substituindo o serviço
            var factoryFake = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<ISimulacaoServico, SimulacaoServicoNuloFake>(services);
                });
            });

            var client = factoryFake.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            using (var scope = factoryFake.Services.CreateScope())
            {

                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Clientes.Add(new Cliente { Nome = "Cliente Ativo 1", Email = "rodrigo@gmail.com", SenhaHash = "1234", Liquidez = 1, Ativo = true });
                db.Clientes.Add(new Cliente { Nome = "Cliente Inativo", Email = "rodrigo@gmail.com", SenhaHash = "1234", Liquidez = 1, Ativo = false });
                db.Clientes.Add(new Cliente { Nome = "Cliente Ativo 2", Email = "rodrigo@gmail.com", SenhaHash = "1234", Liquidez = 1, Ativo = true });

                db.Produtos.Add(new Produto
                {
                    Id = 10,
                    Nome = "CDB",
                    Tipo = (int)TipoProdutoEnum.CDB,
                    Rentabilidade = 0.12m,
                    Risco = (int)RiscoProdutoEnum.Baixo
                });

                await db.SaveChangesAsync();
            }

            var request = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1,
                TipoProduto = "CDB",
                Valor = 1000,
                PrazoMeses = 12
            };

            var response = await client.PostAsJsonAsync("/simular-investimento", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SimularInvestimento_QuandoOcorreErroInterno_RetornaErro500()
        {
            // Cria uma instância isolada da factory substituindo o serviço
            var factoryFake = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<ISimulacaoServico, SimulacaoServicoErroFake>(services);
                });
            });

            var client = factoryFake.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            using (var scope = factoryFake.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Clientes.Add(new Cliente { Nome = "Cliente Ativo 1", Email = "rodrigo@gmail.com", SenhaHash = "1234", Liquidez = 1, Ativo = true });

                db.Produtos.Add(new Produto
                {
                    Id = 10,
                    Nome = "CDB",
                    Tipo = (int)TipoProdutoEnum.CDB,
                    Rentabilidade = 0.12m,
                    Risco = (int)RiscoProdutoEnum.Baixo
                });

                await db.SaveChangesAsync();
            }

            var request = new SimulacaoInvestimentoDTORequest
            {
                ClienteId = 1,
                TipoProduto = "CDB",
                Valor = 1000,
                PrazoMeses = 12
            };

            var response = await client.PostAsJsonAsync("/simular-investimento", request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
