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
    public class ListarSimulacoesTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public ListarSimulacoesTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ListarSimulacoes_QuandoExistiremSimulacoes_RetornaOk()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
                
                var cliente = new Cliente { Nome = "Cliente Ativo 1", Email = "rodrigo@gmail.com", SenhaHash = "1234", Liquidez = 1, Ativo = true };
                db.Clientes.Add(cliente);

                var produto = new Produto
                {
                    Nome = "CDB",
                    Tipo = (int)TipoProdutoEnum.CDB,
                    Rentabilidade = 0.12m,
                    Risco = (int)RiscoProdutoEnum.Baixo
                };
                db.Produtos.Add(produto);

                // Inserir simulação manualmente
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

            var response = await _client.GetAsync("/listar-simulacoes");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var resultado =
                await response.Content.ReadFromJsonAsync<List<SimulacaoInvestimentoDTOResponse>>();

            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
        }

        [Fact]
        public async Task ListarSimulacoes_QuandoNaoExistiremSimulacoes_RetornaNoContent()
        {
            var response = await _client.GetAsync("/listar-simulacoes");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ListarSimulacoes_QuandoOcorreExcecaoInterna_RetornaErroInterno()
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
                new AuthenticationHeaderValue("Teste");

            using (var scope = factoryFake.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                var cliente = new Cliente { Nome = "Cliente Ativo 1", Email = "rodrigo@gmail.com", SenhaHash = "1234", Liquidez = 1, Ativo = true };
                db.Clientes.Add(cliente);

                var produto =
                    db.Produtos.Add(new Produto
                    {
                        Nome = "CDB",
                        Tipo = (int)TipoProdutoEnum.CDB,
                        Rentabilidade = 0.12m,
                        Risco = (int)RiscoProdutoEnum.Baixo
                    });

                // Inserir simulação manualmente
                db.Simulacoes.Add(new Simulacao
                {
                    Cliente = cliente,
                    Produto = produto.Entity,
                    ValorInvestido = 1000,
                    PrazoMeses = 12,
                    DataSimulacao = DateTime.UtcNow
                });

                await db.SaveChangesAsync();
            }

            var response = await client.GetAsync("/listar-simulacoes");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
