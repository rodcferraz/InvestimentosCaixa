using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers;

namespace InvestimentosCaixa.Testes.Dominio.Mappers
{
    public class CalculoParaPerfilRiscoMapperTestes
    {
        private readonly CalculoParaPerfilRiscoMapper _mapper;

        public CalculoParaPerfilRiscoMapperTestes()
        {
            _mapper = new CalculoParaPerfilRiscoMapper();
        }

        [Theory]
        [InlineData("Personalizado", CalculoParaPerfilRiscoEnum.Personalizado)]
        [InlineData("Anbima", CalculoParaPerfilRiscoEnum.Anbima)]
        public void ToPerfilRiscoClienteEnum_QuandoValoresDoEnumForemCorretos_DeveRetornarEnumCorretamente(string texto, CalculoParaPerfilRiscoEnum esperado)
        {
            // Act
            var resultado = _mapper.ParaPerfilRiscoClienteEnum(texto);

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData("OutroCalculo")]
        [InlineData("")]
        [InlineData(null)]
        public void ToPerfilRiscoClienteEnum_QuandoEnumForValorInvalido_DeveLancarConvertEnumException(string valor)
        {
            // Act Assert
            var ex = Assert.Throws<ConvertEnumException>(() =>
                _mapper.ParaPerfilRiscoClienteEnum(valor)
            );

            Assert.Contains(nameof(CalculoParaPerfilRiscoEnum), ex.Message);
        }

        [Fact]
        public void ToPerfilRiscoClienteEnum_QuandoLancaExcecao_DeveConterValoresEnum()
        {
            // Arrange
            var invalido = "AAAA";

            // Act
            var ex = Assert.Throws<ConvertEnumException>(() =>
                _mapper.ParaPerfilRiscoClienteEnum(invalido)
            );

            // Assert
            Assert.Contains("Personalizado (0)", ex.Message);
            Assert.Contains("Anbima (1)", ex.Message);
        }
    }
}
