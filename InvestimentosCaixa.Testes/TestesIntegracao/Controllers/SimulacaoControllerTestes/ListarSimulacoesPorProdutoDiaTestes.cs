using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes.Servicos;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes
{
    public class ListarSimulacoesPorProdutoDiaTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public ListarSimulacoesPorProdutoDiaTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ListarSimulacoesPorDia_QuandoExistemSimulacoes_RetornaOk()
        {
            // Arrange: inserir simulações no banco para que o serviço retorne dados reais
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                var cliente = new Cliente { Nome = "Cliente 1", Email = "x@y.com", SenhaHash = "123", Liquidez = 1, Ativo = true };
                db.Clientes.Add(cliente);

                var produto = new Produto
                {
                    Nome = "CDB",
                    Tipo = (int)TipoProdutoEnum.CDB,
                    Rentabilidade = 0.12m,
                    Risco = (int)RiscoProdutoEnum.Baixo
                };
                db.Produtos.Add(produto);

                db.Simulacoes.Add(new Simulacao
                {
                    Cliente = cliente,
                    Produto = produto,
                    ValorInvestido = 1000,
                    PrazoMeses = 12,
                    DataSimulacao = DateTime.UtcNow
                });

                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/por-produto-dia");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ListarSimulacoesPorDia_QuandoNaoExistemSimulacoes_RetornaNoContent()
        {
            // Arrange: garantir banco vazio
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Simulacoes.RemoveRange(db.Simulacoes);
                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/por-produto-dia");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ListarSimulacoesPorDia_QuandoServicoLancaExcecao_RetornaStatus500() 
        {
            // Arrange – forçar erro no serviço

            var factoryFake = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<ISimulacaoServico, SimulacaoServicoErroFake>(services);
                });
            });

            var client = factoryFake.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");

            // Act
            var response = await client.GetAsync("/por-produto-dia");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
