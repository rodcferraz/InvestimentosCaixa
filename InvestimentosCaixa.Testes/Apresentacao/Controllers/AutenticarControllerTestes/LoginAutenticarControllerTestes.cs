using InvestimentosCaixa.Api.Aplicacao.DTOs.Autenticar;
using InvestimentosCaixa.Api.Apresentacao.Controllers;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Testes.Apresentacao.Controllers.AutenticarControllerTestes
{
    public class LoginAutenticarControllerTestes
    {
        private readonly Mock<JwtServico> _jwtMock;
        private readonly Mock<ISegurancaServico> _segMock;
        private readonly Mock<IClienteRepositorio> _clienteRepoMock;
        private readonly Mock<ILogger<AutenticarController>> _loggerMock;
        private readonly AutenticarController _controller;

        public LoginAutenticarControllerTestes()
        {
            _jwtMock = new Mock<JwtServico>();
            _segMock = new Mock<ISegurancaServico>();
            _clienteRepoMock = new Mock<IClienteRepositorio>();
            _loggerMock = new Mock<ILogger<AutenticarController>>();

            _controller = new AutenticarController(
                _jwtMock.Object,
                _segMock.Object,
                _clienteRepoMock.Object,
                _loggerMock.Object
            );
        }

        //[Fact]
        //public async Task Login_QuandoClienteNaoExiste_DeveRetornarNotFound_()
        //{
        //    // Arrange
        //    var request = new AutenticarRequest { Email = "teste@x.com", Senha = "123" };

        //    _clienteRepoMock
        //        .Setup(r => r.ListarClienteAtivoPorEmailAsync(request.Email))
        //        .ReturnsAsync((Cliente)null);

        //    // Act
        //    var resposta = await _controller.Login(request) as NotFoundObjectResult;

        //    // Assert
        //    Assert.NotNull(resposta);
        //    Assert.Equal(404, resposta.StatusCode);
        //}

        //[Fact]
        //public async Task Login_QuandoSenhaIncorreta_DeveRetornarUnauthorized()
        //{
        //    // Arrange
        //    var request = new AutenticarRequest { Email = "teste@x.com", Senha = "senhaerrada" };

        //    var cliente = new Cliente
        //    {
        //        Id = 1,
        //        Email = request.Email,
        //        SenhaHash = "HASH_CORRETO"
        //    };

        //    _clienteRepoMock
        //        .Setup(r => r.ListarClienteAtivoPorEmailAsync(request.Email))
        //        .ReturnsAsync(cliente);

        //    _segMock
        //        .Setup(s => s.CriptografarPasswordHash(request.Senha))
        //        .Returns("HASH_ERRADO");

        //    // Act
        //    var resposta = await _controller.Login(request) as UnauthorizedObjectResult;

        //    // Assert
        //    Assert.NotNull(resposta);
        //    Assert.Equal(401, resposta.StatusCode);
        //}

        //[Fact]
        //public async Task Login_QuandoCredenciaisCorretas_DeveRetornarOk_()
        //{
        //    // Arrange
        //    var request = new AutenticarRequest { Email = "teste@x.com", Senha = "123" };

        //    var cliente = new Cliente
        //    {
        //        Id = 1,
        //        Email = request.Email,
        //        SenhaHash = "HASH_CORRETO"
        //    };

        //    _clienteRepoMock
        //        .Setup(r => r.ListarClienteAtivoPorEmailAsync(request.Email))
        //        .ReturnsAsync(cliente);

        //    _segMock
        //        .Setup(s => s.CriptografarPasswordHash(request.Senha))
        //        .Returns("HASH_CORRETO");

        //    _jwtMock
        //        .Setup(j => j.GerarToken(cliente.Id.ToString(), cliente.Email))
        //        .Returns("TOKEN123");

        //    // Act
        //    var resposta = await _controller.Login(request) as OkObjectResult;

        //    // Assert
        //    Assert.NotNull(resposta);
        //    Assert.Equal(200, resposta.StatusCode);

        //    var retorno = resposta.Value as dynamic;
        //    Assert.Equal("TOKEN123", retorno.token);
        //}

        //[Fact]
        //public async Task Login_QuandoOcorrerExcecao_DeveRetornarErro500()
        //{
        //    // Arrange
        //    var request = new AutenticarRequest { Email = "teste@x.com", Senha = "123" };

        //    _clienteRepoMock
        //        .Setup(r => r.ListarClienteAtivoPorEmailAsync(request.Email))
        //        .ThrowsAsync(new Exception("Erro inesperado no banco"));

        //    // Act
        //    var resposta = await _controller.Login(request) as ObjectResult;

        //    // Assert
        //    Assert.NotNull(resposta);
        //    Assert.Equal(500, resposta.StatusCode);
        //    Assert.Equal("Erro interno no servidor.", resposta.Value);
        //}
    }
}
