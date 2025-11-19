using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers;

namespace InvestimentosCaixa.Testes.Dominio.Mappers
{
    public class ProdutoMapperTestes
    {
        private readonly ProdutoMapper _mapper;

        public ProdutoMapperTestes()
        {
            _mapper = new ProdutoMapper();
        }

        [Fact]
        public void ToDtoResponse_QuandoExecutado_DeveMapearCorretamente()
        {
            // Arrange
            var produto = new Produto
            {
                Id = 10,
                Nome = "CDB Caixa 2025",
                Tipo = (int)TipoProdutoEnum.CDB,
                Rentabilidade = 0.12m,
                Risco = (int)RiscoProdutoEnum.Baixo
            };

            // Act
            var dto = _mapper.ToDtoResponse(produto);

            // Assert
            Assert.Equal(produto.Id, dto.Id);
            Assert.Equal(produto.Nome, dto.Nome);
            Assert.Equal("CDB", dto.Tipo);
            Assert.Equal(produto.Rentabilidade, dto.Rentabilidade);
            Assert.Equal("Baixo", dto.Risco);
        }

        [Fact]
        public void ToDtoResponseList_QuandoExcecutado_DeveConverterLista()
        {
            // Arrange
            var produtos = new List<Produto>
        {
            new Produto { Id = 1, Nome = "CDB", Tipo = 1, Rentabilidade = 0.1m, Risco = 0 },
            new Produto { Id = 2, Nome = "LCI", Tipo = 2, Rentabilidade = 0.2m, Risco = 2 },
        };

            // Act
            var listaDto = _mapper.ToDtoResponseList(produtos);

            // Assert
            Assert.Equal(2, listaDto.Count);

            Assert.Equal(1, listaDto[0].Id);
            Assert.Equal("CDB", listaDto[0].Nome);
            Assert.Equal(((TipoProdutoEnum)produtos[0].Tipo).ToString(), 
                listaDto[0].Tipo);
            Assert.Equal(0.1m, listaDto[0].Rentabilidade);
            Assert.Equal(((RiscoProdutoEnum)produtos[0].Risco).ToString(), 
                listaDto[0].Risco);

            Assert.Equal(2, listaDto[1].Id);
            Assert.Equal("LCI", listaDto[1].Nome);
            Assert.Equal(((TipoProdutoEnum)produtos[1].Tipo).ToString(),
                listaDto[1].Tipo);
            Assert.Equal(0.2m, listaDto[1].Rentabilidade);
            Assert.Equal(((RiscoProdutoEnum)produtos[1].Risco).ToString(),
                listaDto[1].Risco);
        }

        [Fact]
        public void ToDtoResponseList_QuandoForListaVazia_DeveRetornarListaVazia()
        {
            // Arrange
            var produtos = new List<Produto>();

            // Act
            var listaDto = _mapper.ToDtoResponseList(produtos);

            // Assert
            Assert.NotNull(listaDto);
            Assert.Empty(listaDto);
        }

        [Fact]
        public void ToBaseEntity_QuandoExecutado_DeveConverterDtoParaEntity()
        {
            // Arrange
            var dto = new ProdutoDTOBaseRequest
            {
                Nome = "Tesouro Selic",
                Tipo = "LCA",
                Rentabilidade = 0.08m,
                Risco = "Baixo"
            };

            // Act
            var entity = _mapper.ToBaseEntity(dto);

            // Assert
            Assert.Equal(dto.Nome, entity.Nome);
            Assert.Equal((int)TipoProdutoEnum.LCA, entity.Tipo);
            Assert.Equal(dto.Rentabilidade, entity.Rentabilidade);
            Assert.Equal((int)RiscoProdutoEnum.Baixo, entity.Risco);
        }

        [Fact]
        public void ToBaseEntity_QuandoTiverTipoInvalido_DeveLancarConvertEnumException()
        {
            // Arrange
            var dto = new ProdutoDTOBaseRequest
            {
                Nome = "Produto Inválido",
                Tipo = "Inexistente",
                Risco = "Baixo",
                Rentabilidade = 0.1m
            };

            // Act & Assert
            var ex = Assert.Throws<ConvertEnumException>(() => _mapper.ToBaseEntity(dto));
            Assert.Contains("TipoProdutoEnum", ex.Message);
            Assert.Contains("Inexistente", ex.Message);
        }

        [Fact]
        public void ToBaseEntity_QuandoTiverRiscoInvalido_DeveLancarConvertEnumException()
        {
            // Arrange
            var dto = new ProdutoDTOBaseRequest
            {
                Nome = "Produto Inválido",
                Tipo = "LCI",
                Risco = "SuperArriscado",
                Rentabilidade = 0.1m
            };

            // Act & Assert
            var ex = Assert.Throws<ConvertEnumException>(() => _mapper.ToBaseEntity(dto));
            Assert.Contains("RiscoProduto", ex.Message);
            Assert.Contains("SuperArriscado", ex.Message);
        }

        [Fact]
        public void ToEntity_QuandoExecutado_DeveAdicionarIdAposConversao()
        {
            // Arrange
            var dto = new ProdutoDTORequest
            {
                Id = 99,
                Nome = "LCI 2026",
                Tipo = "LCI",
                Risco = "Baixo",
                Rentabilidade = 0.05m
            };

            // Act
            var entity = _mapper.ToEntity(dto);

            // Assert
            Assert.Equal(99, entity.Id);
            Assert.Equal(dto.Nome, entity.Nome);
            Assert.Equal((int)TipoProdutoEnum.LCI, entity.Tipo);
            Assert.Equal((int)RiscoProdutoEnum.Baixo, entity.Risco);
        }
    }
}
