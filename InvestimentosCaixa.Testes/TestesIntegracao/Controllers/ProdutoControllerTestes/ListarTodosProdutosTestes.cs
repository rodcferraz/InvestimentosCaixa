using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ProdutoControllerTestes
{
    public class ListarTodosProdutosTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public ListarTodosProdutosTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ListarTodosProdutos_QuandoProdutosAtivosExistem_DeveRetornarOk()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                db.Produtos.AddRange(
                    new Produto
                    {
                        Nome = "CDB Caixa 2026",
                        Tipo = 2,
                        Risco = 1,
                        Ativo = true,
                        Rentabilidade = 5.5m,
                    },
                    new Produto
                    {
                        Nome = "Tesouro Selic",
                        Tipo = 1,
                        Risco = 1,
                        Ativo = true,
                        Rentabilidade = 6.0m,
                    }
                );

                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/listar-produtos");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        //[Fact]
        //public async Task ListarTodosProdutos_QuandoNaoExistemProdutosAtivos_DeveRetornarNoContent()
        //{
        //    // Arrange
        //    using (var scope = _factory.Services.CreateScope())
        //    {
        //        var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

        //        // Adiciona apenas produtos inativos
        //        db.Produtos.Add(new Produto
        //        {
        //            Nome = "CDB Caixa 2026",
        //            Tipo = 2,
        //            Risco = 1,
        //            Ativo = false,
        //            Rentabilidade = 5.5m,
        //        });

        //        await db.SaveChangesAsync();
        //    }

        //    // Act
        //    var response = await _client.GetAsync("/listar-produtos");

        //    // Assert
        //    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        //}

        [Fact]
        public async Task ListarTodosProdutos_QuandoErroInterno_DeveRetornar500()
        {
            // Arrange - Força erro deletando o banco
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                // Força erro apagando o banco inteiro
                db.Database.EnsureDeleted();
            }

            // Act
            var response = await _client.GetAsync("/listar-produtos");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Erro interno no servidor", content);
        }
    }
}
