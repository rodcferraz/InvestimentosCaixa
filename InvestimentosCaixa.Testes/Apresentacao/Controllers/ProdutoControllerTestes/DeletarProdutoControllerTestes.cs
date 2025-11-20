using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InvestimentosCaixa.Testes.Apresentacao.Controllers.ProdutoControllerTestes
{
    public class DeletarProdutoControllerTestes : IClassFixture<ProdutoControllerFixture>
    {
        private readonly ProdutoControllerFixture _fixture;

        public DeletarProdutoControllerTestes()
        {
            _fixture = new ProdutoControllerFixture();
        }

        [Fact]
        public async Task RemoverProduto_QuandoProdutoNaoEncontrado_RetornaNotFound()
        {
            // Arrange
            int id = 10;

            _fixture.ProdutoServicoMock
                .Setup(s => s.RemoverProdutoAsync(id))
                .ReturnsAsync(false);

            // Act
            var result = await _fixture.Controller.DeletarProduto(id);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Produto com ID {id} não encontrado para deleção.", notFound.Value);
        }

        [Fact]
        public async Task RemoverProduto_QuandoSucesso_RetornaOk()
        {
            // Arrange
            int id = 1;

            _fixture.ProdutoServicoMock
                .Setup(s => s.RemoverProdutoAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await _fixture.Controller.DeletarProduto(id);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200 , ok.StatusCode);
        }

        [Fact]
        public async Task RemoverProduto_QuandoLancaExcecao_RetornaBadRequest()
        {
            // Arrange
            int id = 10;

            _fixture.ProdutoServicoMock
                .Setup(s => s.RemoverProdutoAsync(id))
                .ThrowsAsync(new Exception("Erro"));

            // Act
            var result = await _fixture.Controller.DeletarProduto(id);

            // Assert
            var badRequest = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, badRequest.StatusCode);
            Assert.Equal("Erro interno no servidor.", badRequest.Value);
        }
    }
}
