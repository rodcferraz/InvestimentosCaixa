using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers;

namespace InvestimentosCaixa.Testes.Dominio.Mappers
{
    public class ClienteMapperTestes
    {
        private readonly ClienteMapper _mapper;

        public ClienteMapperTestes()
        {
            _mapper = new ClienteMapper();
        }

        [Fact]
        public void ToEntity_QuandoCadastroRequestForValido_DeveRetornarCliente()
        {
            // Arrange
            var dto = new ClienteDTOCadastroRequest
            {
                Nome = "Rodrigo",
                Email = "rodrigo@gmail.com",
                Liquidez = (int)PerfilRiscoClienteEnum.Moderado
            };

            // Act
            var cliente = _mapper.ToEntity(dto);

            // Assert
            Assert.Equal("Rodrigo", cliente.Nome);
            Assert.Equal("rodrigo@gmail.com", cliente.Email);
            Assert.Equal(dto.Liquidez, cliente.Liquidez);
        }

        [Fact]
        public void ToEntity_QuandoCadastroRequestTiverLiquidezInvalida_DeveLancarConvertEnumException()
        {
            // Arrange
            var dto = new ClienteDTOCadastroRequest
            {
                Nome = "Rodrigo",
                Email = "rodrigo@gmail.com",
                Liquidez = 999
            };

            // Act & Assert
            var ex = Assert.Throws<ConvertEnumException>(() => _mapper.ToEntity(dto));
            Assert.Contains(nameof(PerfilRiscoClienteEnum), ex.Message);
        }

        [Fact]
        public void ToBaseEntity_QuandoClienteRequestForValido_DeveRetornarCliente()
        {
            // Arrange
            var dto = new ClienteDTOBaseRequest
            {
                Nome = "Maria",
                Email = "maria@example.com",
                Liquidez = (int)PerfilRiscoClienteEnum.Conservador
            };

            // Act
            var cliente = _mapper.ToBaseEntity(dto);

            // Assert
            Assert.Equal(dto.Nome, cliente.Nome);
            Assert.Equal(dto.Email, cliente.Email);
            Assert.Equal(dto.Liquidez, cliente.Liquidez);
        }

        [Fact]
        public void ToBaseEntity_QuandoLiquidezForInvalida_DeveLancarConvertEnumException()
        {
            // Arrange
            var dto = new ClienteDTOBaseRequest
            {
                Nome = "Rodrigo",
                Email = "rodrigo@gmail.com",
                Liquidez =888
            };

            // Act & Assert
            Assert.Throws<ConvertEnumException>(() => _mapper.ToBaseEntity(dto));
        }

        [Fact]
        public void ToDtoResponse_DeveConverterClienteParaResponse()
        {
            // Arrange
            var cliente = new Cliente
            {
                Id = 10,
                Nome = "Rodrigo",
                Email = "rodrigo@gmail.com",
                Liquidez = (int)PerfilRiscoClienteEnum.Agressivo
            };

            // Act
            var dto = _mapper.ToDtoResponse(cliente);

            // Assert
            Assert.Equal(cliente.Id, dto.Id);
            Assert.Equal(cliente.Nome, dto.Nome);
            Assert.Equal(cliente.Email, dto.Email);
            Assert.Equal(cliente.Liquidez, dto.Liquidez);
        }

        [Fact]
        public void ToDtoResponseList_DeveConverterListaDeClientes()
        {
            // Arrange
            var clientes = new List<Cliente>
        {
            new Cliente { Id = 1, Nome = "rodrigo", Email = "rodrigo@gmail.com", Liquidez = (int) PerfilRiscoClienteEnum.Conservador },
            new Cliente { Id = 2, Nome = "pedro", Email = "pedro@gmail.com", Liquidez = (int) PerfilRiscoClienteEnum.Moderado }
        };

            // Act
            var lista = _mapper.ToDtoResponseList(clientes);

            // Assert
            Assert.Equal(2, lista.Count);
            
            Assert.Equal(1, lista[0].Id);
            Assert.Equal("rodrigo", lista[0].Nome);
            Assert.Equal("rodrigo@gmail.com",lista[0].Email);
            Assert.Equal(1, lista[0].Liquidez);

            Assert.Equal(1, lista[0].Id);
            Assert.Equal("pedro", lista[1].Nome);
            Assert.Equal("rodrigo@gmail.com", lista[0].Email);
            Assert.Equal(1, lista[0].Liquidez);

        }

        [Fact]
        public void ToEntity_QuandoExecutado_DeveRetornarClienteComId()
        {
            // Arrange
            var dto = new ClienteDTORequest
            {
                Id = 42,
                Nome = "Rodrigo",
                Email = "rodrigo@gmail.com",
                Liquidez = (int)PerfilRiscoClienteEnum.Agressivo
            };

            // Act
            var cliente = _mapper.ToEntity(dto);

            // Assert
            Assert.Equal(42, cliente.Id);
            Assert.Equal(dto.Nome, cliente.Nome);
            Assert.Equal(dto.Email, cliente.Email);
            Assert.Equal(dto.Liquidez, cliente.Liquidez);
        }

        [Fact]
        public void ToEntity_Request_LiquidezInvalida_DeveLancarConvertEnumException()
        {
            var dto = new ClienteDTORequest
            {
                Id = 1,
                Nome = "Rodrigo",
                Email = "rodrigo@gmail.com",
                Liquidez = 555
            };

            Assert.Throws<ConvertEnumException>(() => _mapper.ToEntity(dto));
        }
    }
}
