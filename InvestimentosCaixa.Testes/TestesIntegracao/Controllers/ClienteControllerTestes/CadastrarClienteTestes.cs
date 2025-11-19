using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControlerTestes.Mapper;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes.Servico;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControllerTestes
{
    public class CadastrarClienteTestes : IClassFixture<WebApplicationFactoryCustomizado>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactoryCustomizado _factory;

        public CadastrarClienteTestes(WebApplicationFactoryCustomizado factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CadastrarCliente_QuandoDadosValidos_DeveRetornar200()
        {
            // Arrange
            var request = new ClienteDTOCadastroRequest
            {
                Nome = "Cliente 1",
                Email = "rodrigo@gmail.com",
                Senha = "1234",
                Liquidez = 1,
            };

            // Act
            var response = await _client.PostAsJsonAsync("/cadastrar-cliente", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CadastrarCliente_QuandoEnumInvalido_DeveRetornar400()
        {
            // Arrange
            var factoryComErro = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IClienteMapper, ClienteMapperErroFake>(services);
                });
            });

            var client = factoryComErro.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");

            var request = new ClienteDTOCadastroRequest
            {
                Nome = "Cliente 1 ",
                Email = "rodrigo@gmail.com",
                Senha = "123",
                Liquidez = 10,
            };
            // Act
            var response = await client.PostAsJsonAsync("/cadastrar-cliente", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CadastrarCliente_QuandoServicoLancaExcecao_DeveRetornar500()
        {
            // Arrange — substitui serviço real por fake que lança Exception
            var factoryComErro = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _factory.ReplaceService<IClienteServico, ClienteServicoErroFake>(services);
                });
            });

            var client = factoryComErro.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Teste");

            var request = new ClienteDTOCadastroRequest
            {
                Nome = "Cliente 1",
                Email = "rodrigo@gmail.com",
                Senha = "123",
                Liquidez = 2,
            };

            // Act
            var response = await client.PostAsJsonAsync("/cadastrar-cliente", request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
