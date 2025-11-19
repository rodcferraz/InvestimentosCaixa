using InvestimentosCaixa.Api.Aplicacao.DTOs.Autenticar;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.AutenticarControllerTestes.Repositorio;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes.Servico;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.AutenticarControllerTestes
{
    public class LoginTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public LoginTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        //[Fact]
        //public async Task Login_QuandoCredenciaisValidas_DeveRetornar200EToken()
        //{
        //    // Arrange

        //    using (var scope = _factory.Services.CreateScope())
        //    {
        //        var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
        //        var seguranca = scope.ServiceProvider.GetRequiredService<ISegurancaServico>();

        //        var senhaHash = seguranca.CriptografarPasswordHash("senhaatual");

        //        // ⭐ VERIFICAR se já existe o cliente para evitar duplicação
        //        var clienteExistente = await db.Clientes
        //            .FirstOrDefaultAsync(c => c.Email == "rodrigo@teste.com");

        //        if (clienteExistente == null)
        //        {
        //            db.Clientes.Add(new Cliente
        //            {
        //                Nome = "Rodrigo",
        //                Email = "rodrigo@teste.com",
        //                SenhaHash = senhaHash,
        //                Liquidez = 1,
        //                Ativo = true
        //            });

        //            await db.SaveChangesAsync();
        //        }
        //    }

        //    var request = new AutenticarRequest
        //    {
        //        Email = "rodrigo@teste.com",
        //        Senha = "senhaatual"
        //    };

        //    // Act
        //    var response = await _client.PostAsJsonAsync("/login", request);

        //    // Assert
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        [Fact]
        public async Task Login_QuandoClienteNaoExisteOuInativo_DeveRetornar404()
        {
            // Arrange
            var request = new AutenticarRequest
            {
                Email = "naoexiste@teste.com",
                Senha = "1234"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/login", request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Cliente não encontrado ou inativo", body);
        }

        //[Fact]
        //public async Task Login_QuandoSenhaIncorreta_DeveRetornar401()
        //{
        //    // Arrange
        //    using (var scope = _factory.Services.CreateScope())
        //    {
        //        var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
        //        var seguranca = scope.ServiceProvider.GetRequiredService<ISegurancaServico>();

        //        var senhaHash = seguranca.CriptografarPasswordHash("senhaCorreta");

        //        db.Clientes.Add(new Cliente
        //        {
        //            Nome = "Cliente 1",
        //            Email = "email@teste.com",
        //            SenhaHash = senhaHash,
        //            Liquidez = 1,
        //            Ativo = true
        //        });

        //        await db.SaveChangesAsync();
        //    }

        //    var request = new AutenticarRequest
        //    {
        //        Email = "email@teste.com",
        //        Senha = "senhaErrada"
        //    };

        //    // Act
        //    var response = await _client.PostAsJsonAsync("/login", request);

        //    // Assert
        //    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        //}

        //[Fact]
        //public async Task Login_QuandoErroInterno_DeveRetornar500()
        //{
        //    // Arrange
        //    // Substituindo o IClienteRepositorio para gerar erro real
        //    var factoryFake = _factory.WithWebHostBuilder(builder =>
        //    {
        //        builder.ConfigureServices(services =>
        //        {
        //            _factory.ReplaceService<IClienteRepositorio, ClienteRepositorioErroFake>(services);
        //        });
        //    });

        //    var client = factoryFake.CreateClient();

        //    var request = new AutenticarRequest
        //    {
        //        Email = "teste@teste.com",
        //        Senha = "1234"
        //    };

        //    // Act
        //    var response = await client.PostAsJsonAsync("/login", request);

        //    // Assert
        //    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        //}
    }
}
