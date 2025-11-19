using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ProdutoControllerTestes
{
    public class ListarProdutosRecomendadosPorPerfilTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public ListarProdutosRecomendadosPorPerfilTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        //[Fact]
        //public async Task ListarProdutosRecomendados_DeveRetornar200()
        //{
        //    int perfil = (int)PerfilRiscoClienteEnum.Conservador;

        //    using (var scope = _factory.Services.CreateScope())
        //    {
        //        var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

        //        db.Produtos.Add(new Produto
        //        {
        //            Nome = "CDB Caixa 2026",
        //            Tipo = (int)TipoProdutoEnum.CDB,
        //            Rentabilidade = 0.12m,
        //            Risco = (int)RiscoProdutoEnum.Baixo,
        //            Ativo = true
        //        });

        //        db.Produtos.Add(new Produto
        //        {
        //            Nome = "LCI",
        //            Tipo = (int)TipoProdutoEnum.LCI,
        //            Rentabilidade = 0.10m,
        //            Risco = (int)RiscoProdutoEnum.Baixo,
        //            Ativo = true
        //        });

        //        await db.SaveChangesAsync();
        //    }

        //    // Act
        //    var response = await _client.GetAsync($"/produtos-recomendados/{perfil}");

        //    // Assert
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        //[Fact]
        //public async Task ListarProdutosRecomendados_QuandoNaoExistemProdutos_DeveRetornar204()
        //{
        //    int perfil = (int)PerfilRiscoClienteEnum.Conservador;

        //    //using (var scope = _factory.Services.CreateScope())
        //    //{
        //    //    var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

        //    //    // Certifica que não há produtos ativos para este perfil
        //    //    db.Produtos.RemoveRange(db.Produtos);
        //    //    await db.SaveChangesAsync();
        //    //}

        //    // Act
        //    var response = await _client.GetAsync($"/produtos-recomendados/{perfil}");

        //    // Assert
        //    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        //}

        [Fact]
        public async Task ListarProdutosRecomendados_QuandoErroInterno_DeveRetornar500()
        {
            int perfil = 99;

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                // Força erro apagando o banco inteiro
                db.Database.EnsureDeleted();
            }

            // Act
            var response = await _client.GetAsync($"/produtos-recomendados/{perfil}");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
