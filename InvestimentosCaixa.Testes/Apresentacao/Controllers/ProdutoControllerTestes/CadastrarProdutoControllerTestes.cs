using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InvestimentosCaixa.Testes.Apresentacao.Controllers.ProdutoControllerTestes
{
    public class CadastrarProdutoControllerTestes : IClassFixture<ProdutoControllerFixture>
    {
        private readonly ProdutoControllerFixture _fixture;

        public CadastrarProdutoControllerTestes()
        {
            _fixture = new ProdutoControllerFixture();
        }

        [Fact]
        public async Task CadastrarProduto_QuandoSucesso_RetornaOk()
        {
            // Arrange
            var dto = new ProdutoDTOBaseRequest { Nome = "Caixa CDB" };

            _fixture.ProdutoServicoMock
                .Setup(s => s.AdicionarProdutoAsync(dto))
                .ReturnsAsync(123);

            // Act
            var result = await _fixture.Controller.CadastrarProduto(dto);

            // Assert
            var ok = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, ok.StatusCode);
        }

        [Fact]
        public async Task CadastrarProduto_QuandoLancaConvertEnumException_RetornaBadRequest()
        {
            // Arrange
            var dto = new ProdutoDTOBaseRequest { Nome = "CDB2" };

            _fixture.ProdutoServicoMock
                .Setup(s => s.AdicionarProdutoAsync(dto))
                .ThrowsAsync(new ConvertEnumException(typeof(TipoProdutoEnum), "CDB2"));

            // Act
            var result = await _fixture.Controller.CadastrarProduto(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ConvertEnumException.MensagemErroConversaoEnum(typeof(TipoProdutoEnum), "CDB2"),
                        badRequest.Value);
        }

        [Fact]
        public async Task CadastrarProduto_QuandoLancaExcecao_RetornaBadRequest()
        {
            // Arrange
            var dto = new ProdutoDTOBaseRequest { Nome = "CDB" };

            _fixture.ProdutoServicoMock
                .Setup(s => s.AdicionarProdutoAsync(dto))
                .ThrowsAsync(new Exception("Erro"));

            // Act
            var result = await _fixture.Controller.CadastrarProduto(dto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Erro interno no servidor.", objectResult.Value);
        }
    }
}
