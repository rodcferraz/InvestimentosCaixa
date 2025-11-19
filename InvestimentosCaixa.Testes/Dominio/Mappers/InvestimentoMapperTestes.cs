using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Mappers;

namespace InvestimentosCaixa.Testes.Dominio.Mappers
{
    public class InvestimentoMapperTestes
    {
        private readonly InvestimentoMapper _mapper;

        public InvestimentoMapperTestes()
        {
            _mapper = new InvestimentoMapper();
        }

        [Fact]
        public void ToBaseEntity_QuandoNaoForNulo_DeveMapearCorretamente()
        {
            // Arrange
            var dto = new InvestimentoDTOBaseRequest
            {
                IdCliente = 1,
                IdProduto = 10,
                Valor = 500.75m
            };

            // Act
            var investimento = _mapper.ToBaseEntity(dto);

            // Assert
            Assert.Equal(dto.IdCliente, investimento.IdCliente);
            Assert.Equal(dto.IdProduto, investimento.IdProduto);
            Assert.Equal(dto.Valor, investimento.Valor);
        }

        [Fact]
        public void ToDtoResponseList_QuandoNaoForNulo_DeveConverterInvestimentosParaDto()
        {
            // Arrange
            var investimentos = new List<Investimento>
        {
            new Investimento
            {
                Id = 1,
                Valor = 1000,
                Data = new DateTime(2025, 11, 17),
                Produto = new Produto
                {
                    Tipo = (int)TipoProdutoEnum.Criptomoeda,
                    Rentabilidade = 0.05m
                }
            }
        };

            // Act
            var dtoList = _mapper.ToDtoResponseList(investimentos);

            // Assert
            Assert.Equal(1, dtoList[0].Id);
            Assert.Equal(1000, dtoList[0].Valor);
            Assert.Equal("Criptomoeda", dtoList[0].Tipo);
            Assert.Equal(0.05m, dtoList[0].Rentabilidade);
            Assert.Equal("2025-11-17", dtoList[0].Data);
        }

        [Fact]
        public void ToDtoResponseList_QuandoForListaVazia_DeveRetornarListaVazia()
        {
            // Arrange
            var investimentos = new List<Investimento>();

            // Act
            var result = _mapper.ToDtoResponseList(investimentos);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
