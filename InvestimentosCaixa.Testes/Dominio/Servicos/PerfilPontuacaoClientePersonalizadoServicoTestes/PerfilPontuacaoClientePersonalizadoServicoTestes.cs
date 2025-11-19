using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Servicos;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.PerfilPontuacaoClientePersonalizadoServicoTestes
{
    public class PerfilPontuacaoClientePersonalizadoServicoTestes
    {
        [Theory]
        [InlineData(0, 10)]
        [InlineData(1000, 10)]
        [InlineData(5000, 10)]
        [InlineData(5001, 30)]
        [InlineData(10000, 30)]
        [InlineData(20000, 30)]
        [InlineData(20001, 50)]
        [InlineData(35000, 50)]
        [InlineData(50000, 50)]
        [InlineData(50001, 80)]
        [InlineData(75000, 80)]
        [InlineData(100000, 80)]
        [InlineData(100001, 100)]
        [InlineData(150000, 100)]
        [InlineData(1000000, 100)]
        public void GerarPerfilCarteiraCliente_ComDiferentesValores_RetornaPontuacaoCorreta(decimal totalInvestido, int pontuacaoEsperada)
        {
            //Arrange
            var perfilPontuacao = new PerfilPontuacaoClientePersonalizadoServico();
            // Act
            var resultado = perfilPontuacao.GerarPerfilCarteiraCliente(totalInvestido);

            // Assert
            Assert.Equal(pontuacaoEsperada, resultado);
        }

        [Theory]
        [InlineData(0, 20)]
        [InlineData(1, 20)]
        [InlineData(2, 20)]
        [InlineData(3, 50)]
        [InlineData(4, 50)]
        [InlineData(5, 50)]
        [InlineData(6, 80)]
        [InlineData(10, 80)]
        [InlineData(100, 80)]
        public void GerarPerfilMovimentacoesCliente_ComDiferentesQuantidades_RetornaPontuacaoCorreta(int quantidadeMovimentacoes, int pontuacaoEsperada)
        {
            //Arrange
            var perfilPontuacao = new PerfilPontuacaoClientePersonalizadoServico();

            // Act
            var resultado = perfilPontuacao.GerarPerfilMovimentacoesCliente(quantidadeMovimentacoes);

            // Assert
            Assert.Equal(pontuacaoEsperada, resultado);
        }

        [Theory]
        [InlineData(PerfilRiscoClienteEnum.Conservador, 20)]
        [InlineData(PerfilRiscoClienteEnum.Moderado, 50)]
        [InlineData(PerfilRiscoClienteEnum.Agressivo, 80)]
        public void GerarPerfilLiquidezCliente_ComPerfisValidos_RetornaPontuacaoCorreta(PerfilRiscoClienteEnum liquidez, int pontuacaoEsperada)
        {
            //Arrange
            var perfilPontuacao = new PerfilPontuacaoClientePersonalizadoServico();

            // Act
            var resultado = perfilPontuacao.GerarPerfilLiquidezCliente(liquidez);

            // Assert
            Assert.Equal(pontuacaoEsperada, resultado);
        }
    }
}
