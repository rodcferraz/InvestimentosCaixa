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
    public class AtualizarSenhaClienteTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public AtualizarSenhaClienteTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AtualizarSenha_Valido_DeveRetornar200()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
                var seguranca = scope.ServiceProvider.GetRequiredService<ISegurancaServico>();

                var senhaHash = seguranca.CriptografarPasswordHash("senhaatual");

                db.Clientes.Add(new Cliente
                {
                    Nome = "Rodrigo",
                    Email = "rodrigo@gmail.com",
                    SenhaHash = senhaHash, //senhaatual
                    Liquidez = 1,
                    Ativo = true
                });

                await db.SaveChangesAsync();
            }

            var request = new AtualizarSenhaClienteDTORequest
            {
                Email = "rodrigo@gmail.com",
                SenhaAtual = "senhaatual",
                NovaSenha = "nova123",
                ConfirmarNovaSenha = "nova123"
            };

            var response = await _client.PostAsJsonAsync("/atualizar-senha", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarSenha_NovaSenhaDiferente_DeveRetornar400()
        {
            var request = new AtualizarSenhaClienteDTORequest
            {
                Email = "rodrigo@gmail.com",
                SenhaAtual = "senhaatual",
                NovaSenha = "abc",
                ConfirmarNovaSenha = "xyz" // diferente!
            };

            var response = await _client.PostAsJsonAsync("/atualizar-senha", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarSenha_SenhaAtualIncorreta_DeveRetornar400()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
                var seguranca = scope.ServiceProvider.GetRequiredService<ISegurancaServico>();

                var senhaHash = seguranca.CriptografarPasswordHash("senhaatual");

                db.Clientes.Add(new Cliente
                {
                    Nome = "Rodrigo",
                    Email = "rodrigo@gmail.com",
                    SenhaHash = "bE+y5WG3/VVmZbRBLCYSumsdfraKS8OI6+AWGDo8HiI=", //senhaatual
                    Liquidez = 1,
                    Ativo = true
                });

                await db.SaveChangesAsync();
            }

            var request = new AtualizarSenhaClienteDTORequest
            {
                Email = "rodrigo@gmail.com",
                SenhaAtual = "errada",
                NovaSenha = "nova123",
                ConfirmarNovaSenha = "nova123"
            };

            var response = await _client.PostAsJsonAsync("/atualizar-senha", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarSenha_QuandoLancaExcecaoNoServico_DeveRetornar500()
        {
            // Cria uma instância isolada da factory substituindo o serviço
            var factoryFake = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IClienteServico, ClienteServicoErroFake>(services);
                });

            });

            var client = factoryFake.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");

            using (var scope = factoryFake.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();

                var seguranca = scope.ServiceProvider.GetRequiredService<ISegurancaServico>();

                var senhaHash = seguranca.CriptografarPasswordHash("senhaatual");

                db.Clientes.Add(new Cliente
                {
                    Nome = "Rodrigo",
                    Email = "rodrigo@gmail.com",
                    SenhaHash = senhaHash, //senhaatual
                    Liquidez = 1,
                    Ativo = true
                });

                await db.SaveChangesAsync();
            }

            var request = new AtualizarSenhaClienteDTORequest
            {
                Email = "teste@teste.com",
                SenhaAtual = "senhaatual",
                NovaSenha = "nova123",
                ConfirmarNovaSenha = "nova123"
            };

            var response = await client.PostAsJsonAsync("/atualizar-senha", request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}