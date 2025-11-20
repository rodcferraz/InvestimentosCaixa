using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Factories;
using InvestimentosCaixa.Api.Dominio.Servicos;

namespace InvestimentosCaixa.Testes.Dominio.Factories
{
    public class MetodoCalculoPontuacaoPerfilRiscoClienteFactoryTestes
    {
        [Fact]
        public void Criar_QuandoMetodoPersonalizado_RetorneServicoPersonalizado()
        {
            // Arrange
            var factory = new MetodoCalculoPontuacaoPerfilRiscoClienteFactory();

            // Act
            var servico = factory.Criar(CalculoParaPerfilRiscoEnum.Personalizado);

            // Assert
            Assert.NotNull(servico);
            Assert.IsType<PerfilPontuacaoClientePersonalizadoServico>(servico);
        }

        [Fact]
        public void Criar_QuandoMetodoAnbima_RetorneExcecaoNotImplemented()
        {
            // Arrange
            var factory = new MetodoCalculoPontuacaoPerfilRiscoClienteFactory();

            // Act & Assert
            var ex = Assert.Throws<NotImplementedException>(() =>
                factory.Criar(CalculoParaPerfilRiscoEnum.Anbima)
            );

            Assert.Equal("Calculo de perfil de risco ANBIMA não implementado.", ex.Message);
        }

        [Fact]
        public void Criar_QuandoMetodoInvalido_RetorneExcecaoNotImplemented()
        {
            // Arrange
            var factory = new MetodoCalculoPontuacaoPerfilRiscoClienteFactory();

            // Act & Assert
            var ex = Assert.Throws<NotImplementedException>(() =>
                factory.Criar((CalculoParaPerfilRiscoEnum)999)
            );

            Assert.Equal("Calculo de perfil de risco não implementado.", ex.Message);
        }
    }
}
