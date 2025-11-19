using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Servicos;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.ProdutoServicoTestes
{
    public class ProdutoServicoTestes : IClassFixture<ProdutoServicoFixture>
    {
        private readonly ProdutoServicoFixture _fixture;

        public ProdutoServicoTestes()
        {
            _fixture = new ProdutoServicoFixture();
        }

        [Fact]
        public async Task AdicionarProdutoAsync_QuandoExecutado_RetornaId()
        {
            // Arrange
            var produtoDto = new ProdutoDTOBaseRequest
            {
                Nome = "Tesouro Direto",
                Tipo = "RendaFixa",
                Risco = "Baixo",
                Rentabilidade = 0.05m
            };
            var produtoEntity = new Produto { Id = 1, Nome = "Tesouro Direto" };

            _fixture.ProdutoMapperMock
                .Setup(m => m.ToBaseEntity(produtoDto))
                .Returns(produtoEntity);

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.AdicionarAsync(produtoEntity))
                .ReturnsAsync(produtoEntity);

            // Act
            var resultado = await _fixture.Servico.AdicionarProdutoAsync(produtoDto);

            // Assert
            Assert.Equal(1, resultado);
            _fixture.ProdutoMapperMock.Verify(m => m.ToBaseEntity(produtoDto), Times.Once);
            _fixture.ProdutoRepositorioMock.Verify(r => r.AdicionarAsync(produtoEntity), Times.Once);
        }

        [Fact]
        public async Task AtualizarProdutoAsync_ComProdutoExistente_RetornaProdutoAtualizado()
        {
            // Arrange
            var produtoDto = new ProdutoDTORequest
            {
                Id = 1,
                Nome = "Novo Nome",
                Tipo = "TesouroSelic",
                Risco = "Baixo",
                Rentabilidade = 0.06m
            };
            var produtoDb = new Produto { Id = 1, Nome = "Nome Antigo" };
            var produtoAtualizado = new Produto { Id = 1, Nome = "Novo Nome" };
            var produtoDtoResponse = new ProdutoDTOResponse { Id = 1, Nome = "Novo Nome" };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarPorIdAsync(1))
                .ReturnsAsync(produtoDb);

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.AtualizarAsync(It.IsAny<Produto>()))
                .ReturnsAsync(produtoAtualizado);

            _fixture.ProdutoMapperMock
                .Setup(m => m.ToDtoResponse(produtoAtualizado))
                .Returns(produtoDtoResponse);

            // Act
            var resultado = await _fixture.Servico.AtualizarProdutoAsync(produtoDto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Novo Nome", resultado.Nome);
            _fixture.ProdutoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarProdutoAsync_ComProdutoNaoEncontrado_RetornaNull()
        {
            // Arrange
            var produtoDto = new ProdutoDTORequest { Id = 999 };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarPorIdAsync(999))
                .ReturnsAsync((Produto)null);

            // Act
            var resultado = await _fixture.Servico.AtualizarProdutoAsync(produtoDto);

            // Assert
            Assert.Null(resultado);
            _fixture.ProdutoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Never);
        }

        [Fact]
        public async Task AtualizarProdutoAsync_ComRiscoInvalido_LancaExcecao()
        {
            // Arrange
            var produtoDto = new ProdutoDTORequest
            {
                Id = 1,
                Nome = "Produto",
                Tipo = "RendaFixa",
                Risco = "RiscoInvalido",
                Rentabilidade = 0.05m
            };
            var produtoDb = new Produto { Id = 1 };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarPorIdAsync(1))
                .ReturnsAsync(produtoDb);

            // Act & Assert
            await Assert.ThrowsAsync<ConvertEnumException>(() =>
                _fixture.Servico.AtualizarProdutoAsync(produtoDto));
        }

        [Fact]
        public async Task DetalhesProdutoAsync_ComProdutoExistente_RetornaProduto()
        {
            // Arrange
            var produto = new Produto { Id = 1, Nome = "CDB" };
            var produtoDto = new ProdutoDTOResponse { Id = 1, Nome = "CDB" };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarPorIdAsync(1))
                .ReturnsAsync(produto);

            _fixture.ProdutoMapperMock
                .Setup(m => m.ToDtoResponse(produto))
                .Returns(produtoDto);

            // Act
            var resultado = await _fixture.Servico.DetalhesProdutoAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("CDB", resultado.Nome);
        }

        [Fact]
        public async Task DetalhesProdutoAsync_ComProdutoNaoExistente_RetornaNull()
        {
            // Arrange
            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarPorIdAsync(999))
                .ReturnsAsync((Produto)null);

            // Act
            var resultado = await _fixture.Servico.DetalhesProdutoAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ListarProdutoAtivoPorNomeAsync_ComProdutoAtivo_RetornaProduto()
        {
            // Arrange
            var produto = new Produto { Id = 1, Nome = "Tesouro Selic", Ativo = true };
            var produtoDto = new ProdutoDTOResponse { Id = 1, Nome = "Tesouro Selic" };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarProdutoPorNome("Tesouro Selic"))
                .ReturnsAsync(produto);

            _fixture.ProdutoMapperMock
                .Setup(m => m.ToDtoResponse(produto))
                .Returns(produtoDto);

            // Act
            var resultado = await _fixture.Servico.ListarProdutoAtivoPorNomeAsync("Tesouro Selic");

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Tesouro Selic", resultado.Nome);
        }

        [Fact]
        public async Task ListarProdutoAtivoPorNomeAsync_ComProdutoInativo_RetornaNull()
        {
            // Arrange
            var produto = new Produto { Id = 1, Nome = "Produto Inativo", Ativo = false };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarProdutoPorNome("Produto Inativo"))
                .ReturnsAsync(produto);

            // Act
            var resultado = await _fixture.Servico.ListarProdutoAtivoPorNomeAsync("Produto Inativo");

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ListarProdutoAtivoPorNomeAsync_ComProdutoNaoEncontrado_RetornaNull()
        {
            // Arrange
            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarProdutoPorNome("Inexistente"))
                .ReturnsAsync((Produto)null);

            // Act
            var resultado = await _fixture.Servico.ListarProdutoAtivoPorNomeAsync("Inexistente");

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ListarProdutoAtivoPorTipoAsync_ComTipoValido_RetornaProduto()
        {
            // Arrange
            var produto = new Produto { Id = 1, Tipo = (int)TipoProdutoEnum.TesouroSelic };
            var produtoDto = new ProdutoDTOResponse { Id = 1 };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarProdutoPorTipo((int)TipoProdutoEnum.TesouroSelic))
                .ReturnsAsync(produto);

            _fixture.ProdutoMapperMock
                .Setup(m => m.ToDtoResponse(produto))
                .Returns(produtoDto);

            // Act
            var resultado = await _fixture.Servico.ListarProdutoAtivoPorTipoAsync("TesouroSelic");

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
        }

        [Fact]
        public async Task ListarProdutoAtivoPorTipoAsync_ComTipoInvalido_LancaExcecao()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ConvertEnumException>(() =>
                _fixture.Servico.ListarProdutoAtivoPorTipoAsync("TipoInvalido"));
        }

        [Fact]
        public async Task ListarProdutosAtivosPorPerfilAsync_ComProdutosDoPerfil_RetornaListaFiltrada()
        {
            // Arrange
            var produtosEntidade = new List<Produto>
            {
                new Produto { Id = 1, Risco = (int)RiscoProdutoEnum.Baixo, Ativo = true },
                new Produto { Id = 2, Risco = (int)RiscoProdutoEnum.Baixo, Ativo = true },
                new Produto { Id = 3, Risco = (int)RiscoProdutoEnum.Alto, Ativo = true },
                new Produto { Id = 4, Risco = (int)RiscoProdutoEnum.Baixo, Ativo = false } // Inativo
            };

            var produtosDtoAtivos = new List<ProdutoDTOResponse>
            {
                new ProdutoDTOResponse { Id = 1, Risco = "Baixo" },
                new ProdutoDTOResponse { Id = 2, Risco = "Baixo" },
                new ProdutoDTOResponse { Id = 3, Risco = "Alto" }
            };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarTodosAsync())
                .ReturnsAsync(produtosEntidade);

            _fixture.ProdutoMapperMock
                .Setup(m => m.ToDtoResponseList(It.Is<List<Produto>>(p => p.Count == 3))) // Apenas os ativos
                .Returns(produtosDtoAtivos);

            // Act
            var resultado = await _fixture.Servico.ListarProdutosAtivosPorPerfilAsync((int)RiscoProdutoEnum.Baixo);

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, p => Assert.Equal("Baixo", p.Risco));
        }

        [Fact]
        public async Task ListarTodosProdutosAtivosAsync_ComProdutosAtivos_RetornaLista()
        {
            // Arrange
            var produtos = new List<Produto>
        {
            new Produto { Id = 1, Nome = "Produto 1", Ativo = true },
            new Produto { Id = 2, Nome = "Produto 2", Ativo = true },
            new Produto { Id = 3, Nome = "Produto 3", Ativo = false }
        };
            var produtosDto = new List<ProdutoDTOResponse>
        {
            new ProdutoDTOResponse { Id = 1, Nome = "Produto 1" },
            new ProdutoDTOResponse { Id = 2, Nome = "Produto 2" }
        };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarTodosAsync())
                .ReturnsAsync(produtos);

            _fixture.ProdutoMapperMock
                .Setup(m => m.ToDtoResponseList(It.IsAny<List<Produto>>()))
                .Returns(produtosDto);

            // Act
            var resultado = await _fixture.Servico.ListarTodosProdutosAtivosAsync();

            // Assert
            Assert.Equal(2, resultado.Count);
            _fixture.ProdutoMapperMock.Verify(m =>
                m.ToDtoResponseList(It.Is<List<Produto>>(p => p.Count == 2)), Times.Once);
        }

        [Fact]
        public async Task ListarTodosProdutosAtivosAsync_SemProdutosAtivos_RetornaListaVazia()
        {
            // Arrange
            var produtos = new List<Produto>
        {
            new Produto { Id = 1, Nome = "Produto 1", Ativo = false },
            new Produto { Id = 2, Nome = "Produto 2", Ativo = false }
        };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarTodosAsync())
                .ReturnsAsync(produtos);

            // Act
            var resultado = await _fixture.Servico.ListarTodosProdutosAtivosAsync();

            // Assert
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task RemoverProdutoAsync_ComProdutoExistente_RetornaTrueEInativaProduto()
        {
            // Arrange
            var produto = new Produto { Id = 1, Nome = "Produto Teste", Ativo = true };

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarPorIdAsync(1))
                .ReturnsAsync(produto);

            _fixture.ProdutoRepositorioMock
                .Setup(r => r.AtualizarAsync(It.IsAny<Produto>()))
                .ReturnsAsync(produto);

            // Act
            var resultado = await _fixture.Servico.RemoverProdutoAsync(1);

            // Assert
            Assert.True(resultado);
            _fixture.ProdutoRepositorioMock.Verify(r =>
                r.AtualizarAsync(It.Is<Produto>(p => p.Ativo == false)), Times.Once);
        }

        [Fact]
        public async Task RemoverProdutoAsync_ComProdutoNaoExistente_RetornaFalse()
        {
            // Arrange
            _fixture.ProdutoRepositorioMock
                .Setup(r => r.ListarPorIdAsync(999))
                .ReturnsAsync((Produto)null);

            // Act
            var resultado = await _fixture.Servico.RemoverProdutoAsync(999);

            // Assert
            Assert.False(resultado);
            _fixture.ProdutoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Produto>()), Times.Never);
        }
    }
}
