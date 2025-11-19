using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InvestimentosCaixa.Testes.Apresentacao.Controllers.ProdutoControllerTestes
{
    public class AtualizarProdutoControllerTestes : IClassFixture<ProdutoControllerFixture>
    {
        private readonly ProdutoControllerFixture _fixture;

        public AtualizarProdutoControllerTestes()
        {
            _fixture = new ProdutoControllerFixture();
        }

        [Fact]
        public async Task AtualizarProduto_QuandoIdPathEIdBodySaoDiferente_RetornaBadRequest()
        {
            // Arrange
            var dto = new ProdutoDTORequest { Id = 2, Nome = "Caixa CDB" };

            // Act
            var result = await _fixture.Controller.AtualizarProduto(1, dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("O ID do produto não corresponde ao ID informado na URL.", badRequest.Value);
        }

        [Fact]
        public async Task AtualizarProduto_QuandoProdutoTemMesmoNomeDeProdutoNoBanco_RetornaBadRequest()
        {
            // Arrange
            var dto = new ProdutoDTORequest { Id = 1, Nome = "Caixa CDB" };

            _fixture.ProdutoServicoMock
                .Setup(s => s.ListarProdutoAtivoPorNomeAsync("Caixa CDB"))
                .ReturnsAsync(new ProdutoDTOResponse());

            // Act
            var result = await _fixture.Controller.AtualizarProduto(1, dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Já existe um produto cadastrado com esse nome.", badRequest.Value);
        }

        [Fact]
        public async Task AtualizarProduto_QuandoProdutoNaoEncontrado_RetornaNotFound()
        {
            // Arrange
            var dto = new ProdutoDTORequest { Id = 1, Nome = "Caixa CDB" };

            _fixture.ProdutoServicoMock
                .Setup(s => s.ListarProdutoAtivoPorNomeAsync("Caixa CDB"))
                .ReturnsAsync((ProdutoDTOResponse?)null);

            _fixture.ProdutoServicoMock
                .Setup(s => s.AtualizarProdutoAsync(dto))
                .ReturnsAsync((ProdutoDTOResponse?)null);

            // Act
            var result = await _fixture.Controller.AtualizarProduto(1, dto);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Produto com ID 1 não encontrado para atualização.", notFound.Value);
        }

        [Fact]
        public async Task AtualizarProduto_QuandoAtualizadoComSucesso_RetornaOk()
        {
            // Arrange
            var dto = new ProdutoDTORequest { Id = 1, Nome = "Caixa CDB" };
            var atualizado = new ProdutoDTOResponse { Id = 1, Nome = "Caixa CDB" };

            _fixture.ProdutoServicoMock
                .Setup(s => s.ListarProdutoAtivoPorNomeAsync("Caixa CDB"))
                .ReturnsAsync((ProdutoDTOResponse?)null);

            _fixture.ProdutoServicoMock
                .Setup(s => s.AtualizarProdutoAsync(dto))
                .ReturnsAsync(atualizado);

            // Act
            var result = await _fixture.Controller.AtualizarProduto(1, dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(atualizado, ok.Value);
        }

        [Fact]
        public async Task AtualizarProduto_LancaConvertEnumException_RetornaBadRequest()
        {
            // Arrange
            var dto = new ProdutoDTORequest { Id = 1, Nome = "Caixa CDB" };

            _fixture.ProdutoServicoMock
                .Setup(s => s.ListarProdutoAtivoPorNomeAsync("Caixa CDB"))
                .ThrowsAsync(new ConvertEnumException(typeof(TipoProdutoEnum), "CDB2"));

            // Act
            var result = await _fixture.Controller.AtualizarProduto(1, dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ConvertEnumException.MensagemErroConversaoEnum(typeof(TipoProdutoEnum), "CDB2"),
                        badRequest.Value);
        }

        [Fact]
        public async Task AtualizarProduto_LancaExcecaoGeral_RetornaStatus500()
        {
            // Arrange
            var dto = new ProdutoDTORequest { Id = 1, Nome = "Caixa CDB" };

            _fixture.ProdutoServicoMock
                .Setup(s => s.ListarProdutoAtivoPorNomeAsync(dto.Nome))
                .ThrowsAsync(new Exception("Erro inesperado"));

            // Act
            var result = await _fixture.Controller.AtualizarProduto(1, dto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Erro interno no servidor.", objectResult.Value);
        }
    }
}
