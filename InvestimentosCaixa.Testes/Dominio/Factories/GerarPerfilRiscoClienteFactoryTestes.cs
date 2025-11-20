using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Servicos;

namespace InvestimentosCaixa.Testes.Dominio.Factories
{
    public class GerarPerfilRiscoClienteFactoryTestes :
    IClassFixture<GerarPerfilRiscoClienteFactoryFixture>
    {
        private readonly GerarPerfilRiscoClienteFactoryFixture _fixture;

        public GerarPerfilRiscoClienteFactoryTestes(GerarPerfilRiscoClienteFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Criar_QuandoMetodoPersonalizado_RetorneInstanciaDePerfilRiscoClientePersonalizado()
        {
            // Arrange
            var metodo = CalculoParaPerfilRiscoEnum.Personalizado;

            // Act
            var result = _fixture.Factory.Criar(metodo, _fixture.PerfilPontuacaoMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PerfilRiscoClientePersonalizado>(result);
        }

        [Fact]
        public void Criar_QuandoMetodoAnbima_RetorneExcecaoNotImplemented()
        {
            // Arrange
            var metodo = CalculoParaPerfilRiscoEnum.Anbima;

            // Act & Assert
            var ex = Assert.Throws<NotImplementedException>(() =>
                _fixture.Factory.Criar(metodo, _fixture.PerfilPontuacaoMock.Object)
            );

            Assert.Contains("ANBIMA", ex.Message);
        }

        [Fact]
        public void Criar_QuandoMetodoInvalido_RetorneExcecaoNotImplemented()
        {
            // Arrange
            var metodo = (CalculoParaPerfilRiscoEnum)999;

            // Act & Assert
            var ex = Assert.Throws<NotImplementedException>(() =>
                _fixture.Factory.Criar(metodo, _fixture.PerfilPontuacaoMock.Object)
            );

            Assert.Contains("não implementado", ex.Message);
        }
    }
}
