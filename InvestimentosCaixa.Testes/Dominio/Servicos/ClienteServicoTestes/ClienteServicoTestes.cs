using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Servicos;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.ClienteServicoTestes
{
    public class ClienteServicoTestes : IClassFixture<ClienteServicoFixture>
    {
        private readonly ClienteServicoFixture _fixture;

        public ClienteServicoTestes()
        {
            _fixture = new ClienteServicoFixture();
        }

        [Fact]
        public async Task AtualizarClienteAsync_QuandoClienteNaoEncontrado_DeveRetornarNull_()
        {
            //Arrange
            _fixture._repoMock.Setup(r => r.ListarPorIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((Cliente)null);

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                    _fixture._mapperMock.Object,
                                                    _fixture._loggerMock,
                                                    _fixture._segurancaMock.Object);

            //Act
            var cliente = await clienteServico.AtualizarClienteAsync(
                new ClienteDTORequest { Id = 1, Nome = "Rodrigo", Email = "rodrigo@gmail.com", Liquidez = 1 });

            //Assert
            Assert.Null(cliente);
            _fixture._repoMock.Verify(r => r.ListarPorIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarClienteAsync_DeveAtualizarClienteEConverterParaDto()
        {
            //Arrange
            var cliente = new Cliente { Id = 1 , Nome = "Antigo", Liquidez = 1};
            var clienteAtualizado = new Cliente { Id = 1, Nome = "Novo", Liquidez = 1 };
            var dtoResponse = new ClienteDTOResponse { Id = 1, Nome = "Novo", Liquidez = 1 };

            _fixture._repoMock
                .Setup(r => r.ListarPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cliente);
            _fixture._repoMock
                .Setup(r => r.AtualizarAsync(It.IsAny<Cliente>()))
                .ReturnsAsync(clienteAtualizado);
            _fixture._mapperMock
                .Setup(m => m.ToDtoResponse(It.IsAny<Cliente>()))
                .Returns(dtoResponse);

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                    _fixture._mapperMock.Object,
                                                    _fixture._loggerMock,
                                                    _fixture._segurancaMock.Object);

            var resultado = await clienteServico.AtualizarClienteAsync(new ClienteDTORequest { Id = 1, Nome = "Novo" , Liquidez = 1 });

            Assert.Equal("Novo", resultado.Nome);
            _fixture._repoMock.Verify(r => r.ListarPorIdAsync(It.IsAny<int>()), Times.Once);
            _fixture._repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Cliente>()), Times.Once);
            _fixture._mapperMock.Verify(m => m.ToDtoResponse(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarSenhaClienteAsync_QuandoClienteNaoEncontrado_DeveRetornarFalse()
        {
            //Arrange
            _fixture._repoMock.Setup(r => r.ListarClienteAtivoPorEmailAsync(It.IsAny<string>()))
                     .ReturnsAsync((Cliente)null);

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                    _fixture._mapperMock.Object,
                                                    _fixture._loggerMock,
                                                    _fixture._segurancaMock.Object);

            //Act
            var result = await clienteServico.AtualizarSenhaClienteAsync("email", "123", "456");

            //Assert
            Assert.False(result);
            _fixture._repoMock.Verify(r => r.ListarClienteAtivoPorEmailAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarSenhaClienteAsync_QuandoSenhaIncorreta_DeveLancarExcecao()
        {
            //Arrange
            var cliente = new Cliente { Email = "email", SenhaHash = "HASH" };

            _fixture._repoMock.Setup(r => r.ListarClienteAtivoPorEmailAsync(It.IsAny<string>()))
                     .ReturnsAsync(cliente);

            _fixture._segurancaMock.Setup(s => s.CriptografarPasswordHash(It.IsAny<string>()))
                          .Returns("HASH-ERRADO");

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                    _fixture._mapperMock.Object,
                                                    _fixture._loggerMock,
                                                    _fixture._segurancaMock.Object);

            //Act 
            //Assert
            await Assert.ThrowsAsync<SenhaIncorretaException>(() =>
                clienteServico.AtualizarSenhaClienteAsync("email", "123", "999")
            );
            _fixture._repoMock.Verify(r => r.ListarClienteAtivoPorEmailAsync(It.IsAny<string>()), Times.Once);
            _fixture._segurancaMock.Verify(s => s.CriptografarPasswordHash(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarSenhaClienteAsync_QuandoExecutado_DeveAtualizarSenha()
        {
            //Arrange
            var cliente = new Cliente { Email = "email", SenhaHash = "OLD_HASH", Ativo = true };

            _fixture._repoMock.Setup(r => r.ListarClienteAtivoPorEmailAsync(It.IsAny<string>()))
                     .ReturnsAsync(cliente);
            _fixture._segurancaMock.SetupSequence(s => s.CriptografarPasswordHash(It.IsAny<string>()))
                          .Returns("OLD_HASH")
                          .Returns("NEW_HASH");

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                    _fixture._mapperMock.Object,
                                                    _fixture._loggerMock,
                                                    _fixture._segurancaMock.Object);
            //Act
            var result = await clienteServico.AtualizarSenhaClienteAsync("email", "123", "999");

            //Assert
            Assert.True(result);
            _fixture._repoMock.Verify(r => r.ListarClienteAtivoPorEmailAsync(It.IsAny<string>()), Times.Once);
            _fixture._segurancaMock.Verify(s => s.CriptografarPasswordHash(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CadastrarClienteAsync_QuandoExecutado_DeveCadastrarERetornarId()
        {
            //Arrange
            var dto = new ClienteDTOCadastroRequest
            {
                Nome = "Rodrigo",
                Email = "a@a.com",
                Senha = "HASH"
            };

            var cliente = new Cliente { Id = 10 };

            //Assert

            _fixture._mapperMock.Setup(m => m.ToEntity(It.IsAny<ClienteDTOCadastroRequest>())).Returns(cliente);
            _fixture._segurancaMock.Setup(s => s.CriptografarPasswordHash(dto.Senha)).Returns("HASH");
            _fixture._repoMock.Setup(r => r.AdicionarAsync(It.IsAny<Cliente>())).ReturnsAsync(cliente);

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                _fixture._mapperMock.Object,
                                                _fixture._loggerMock,
                                                _fixture._segurancaMock.Object);
            //Act

            var idCadastrado = await clienteServico.CadastrarClienteAsync(dto);

            Assert.Equal(cliente.Id, idCadastrado);

            _fixture._mapperMock.Verify(x => x.ToEntity(It.IsAny<ClienteDTOCadastroRequest>()), Times.Once);
            _fixture._segurancaMock.Verify(x => x.CriptografarPasswordHash(It.IsAny<string>()), Times.Once);
            _fixture._repoMock.Verify(x => x.AdicionarAsync(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public async Task DetalhesClienteAsync_QuandoNaoEncontrado_DeveRetornarNull()
        {
            _fixture._repoMock.Setup(r => r.ListarPorIdAsync(1)).ReturnsAsync((Cliente)null);

            var result = await _fixture._servico.DetalhesClienteAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task DetalhesClienteAsync_QuandoEncontrado_DeveRetornarDto()
        {
            //Arrange
            var cliente = new Cliente { Id = 1, Ativo = true };
            var dto = new ClienteDTOResponse { Id = 1 };

            _fixture._repoMock.Setup(r => r.ListarPorIdAsync(It.IsAny<int>())).ReturnsAsync(cliente);
            _fixture._mapperMock.Setup(m => m.ToDtoResponse(It.IsAny<Cliente>())).Returns(dto);

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                _fixture._mapperMock.Object,
                                                _fixture._loggerMock,
                                                _fixture._segurancaMock.Object);

            //Act
            var result = await clienteServico.DetalhesClienteAsync(1);

            //Assert
            Assert.Equal(1, result.Id);
            _fixture._repoMock.Verify(r => r.ListarPorIdAsync(It.IsAny<int>()), Times.Once);
            _fixture._mapperMock.Verify(m => m.ToDtoResponse(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public async Task ListarClienteAtivoPorEmailAsync_QuandoInativo_DeveRetornarNull()
        {
            //Arrange
            var cliente = new Cliente { Ativo = false };

            _fixture._repoMock.Setup(r => r.ListarClienteAtivoPorEmailAsync(It.IsAny<string>()))
                     .ReturnsAsync(cliente);

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                _fixture._mapperMock.Object,
                                                _fixture._loggerMock,
                                                _fixture._segurancaMock.Object);

            //Act
            var result = await _fixture._servico.ListarClienteAtivoPorEmailAsync("email");

            //Assert
            Assert.Null(result);
            _fixture._repoMock.Verify(r => r.ListarClienteAtivoPorEmailAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ListarClienteAtivoPorEmailAsync_DeveRetornarDto()
        {
            //Arrange
            var cliente = new Cliente { Email = "email", Ativo = true };
            var dto = new ClienteDTOResponse { Email = "email" };

            _fixture._repoMock.Setup(r => r.ListarClienteAtivoPorEmailAsync(It.IsAny<string>()))
                     .ReturnsAsync(cliente);

            _fixture._mapperMock.Setup(m => m.ToDtoResponse(It.IsAny<Cliente>()))
                .Returns(dto);

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                _fixture._mapperMock.Object,
                                                _fixture._loggerMock,
                                                _fixture._segurancaMock.Object);
            //Act
            var result = await clienteServico.ListarClienteAtivoPorEmailAsync("email");

            //Assert
            Assert.Equal("email", result.Email);
            _fixture._repoMock.Verify(r => r.ListarClienteAtivoPorEmailAsync(It.IsAny<string>()), Times.Once);
            _fixture._mapperMock.Verify(m => m.ToDtoResponse(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public async Task ListarTodosClientesAtivosAsync_QuandoListaVazia_DeveRetornarNull()
        {
            //Arrange
            _fixture._repoMock.Setup(r => r.ListarTodosAsync())
                     .ReturnsAsync(new List<Cliente>());

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                    _fixture._mapperMock.Object,
                                                    _fixture._loggerMock,
                                                    _fixture._segurancaMock.Object);
            //Act
            var result = await clienteServico.ListarTodosClientesAtivosAsync();

            //Assert
            Assert.Null(result);
            _fixture._repoMock.Verify(r => r.ListarTodosAsync(), Times.Once);
        }

        [Fact]
        public async Task ListarTodosClientesAtivosAsync_DeveRetornarApenasAtivos()
        {
            //Arrange
            var lista = new List<Cliente>
            {
                new Cliente { Id = 1, Ativo = true },
                new Cliente { Id = 2, Ativo = false }
            };

            var dtoList = new List<ClienteDTOResponse>
            {
                new ClienteDTOResponse { Id = 1 }
            };

            _fixture._repoMock.Setup(r => r.ListarTodosAsync())
                .ReturnsAsync(lista);
            _fixture._mapperMock.Setup(m => m.ToDtoResponseList(It.IsAny<List<Cliente>>()))
                .Returns(dtoList);

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                    _fixture._mapperMock.Object,
                                                    _fixture._loggerMock,
                                                    _fixture._segurancaMock.Object);

            //Act
            var result = await clienteServico.ListarTodosClientesAtivosAsync();

            //Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
            _fixture._repoMock.Verify(r => r.ListarTodosAsync(), Times.Once);
            _fixture._mapperMock.Verify(m => m.ToDtoResponseList(It.IsAny<List<Cliente>>()), Times.Once);
        }

        [Fact]
        public async Task RemoverClienteAsync_DeveRetornarFalse_QuandoNaoEncontrado()
        {
            //Arrange
            _fixture._repoMock.Setup(r => r.ListarPorIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((Cliente)null);

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                    _fixture._mapperMock.Object,
                                                    _fixture._loggerMock,
                                                    _fixture._segurancaMock.Object);

            //Act
            var result = await clienteServico.RemoverClienteAsync(1);

            //Assert
            Assert.False(result);
            _fixture._repoMock.Verify(r => r.ListarPorIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task RemoverClienteAsync_DeveInativarCliente()
        {
            //Arrange
            var cliente = new Cliente { Id = 1, Ativo = true };

            _fixture._repoMock.Setup(r => r.ListarPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cliente);
            _fixture._repoMock.Setup(r => r.AtualizarAsync(It.IsAny<Cliente>()))
                .ReturnsAsync(cliente);

            var clienteServico = new ClienteServico(_fixture._repoMock.Object,
                                                    _fixture._mapperMock.Object,
                                                    _fixture._loggerMock,
                                                    _fixture._segurancaMock.Object);

            //Act
            var result = await clienteServico.RemoverClienteAsync(1);

            //Assert
            Assert.True(result);
            Assert.False(cliente.Ativo);
            _fixture._repoMock.Verify(r => r.ListarPorIdAsync(It.IsAny<int>()), Times.Once);
            _fixture._repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Cliente>()), Times.Once);
        }
    }
}
