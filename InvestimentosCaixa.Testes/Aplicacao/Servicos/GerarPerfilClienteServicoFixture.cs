using InvestimentosCaixa.Api.Aplicacao.Servicos;
using InvestimentosCaixa.Api.Configuracoes;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Factories.Interfaces;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Testes.Aplicacao.Servicos
{
    public class GerarPerfilClienteServicoFixture
    {
        public Mock<IConfigurationRoot> ConfigurationMock { get; }
        public AppSettings AppSettings { get; }
        public Mock<ICalculoPerfilRiscoMapper> CalculoMapperMock { get; }
        public Mock<ILogger<GerarPerfilClienteServico>> LoggerMock { get; }
        public Mock<IMetodoCalculoPontuacaoPerfilRiscoClienteFactory> MetodoCalculoFactoryMock { get; }
        public Mock<IGerarPerfilRiscoClienteFactory> PerfilRiscoFactoryMock { get; }
        public GerarPerfilClienteServico Servico { get; }

        public GerarPerfilClienteServicoFixture()
        {
            ConfigurationMock = new Mock<IConfigurationRoot>();
            AppSettings = new AppSettings(ConfigurationMock.Object);
            CalculoMapperMock = new Mock<ICalculoPerfilRiscoMapper>();
            LoggerMock = new Mock<ILogger<GerarPerfilClienteServico>>();
            MetodoCalculoFactoryMock = new Mock<IMetodoCalculoPontuacaoPerfilRiscoClienteFactory>();
            PerfilRiscoFactoryMock = new Mock<IGerarPerfilRiscoClienteFactory>();

            Servico = new GerarPerfilClienteServico(
                AppSettings,
                CalculoMapperMock.Object,
                LoggerMock.Object,
                MetodoCalculoFactoryMock.Object,
                PerfilRiscoFactoryMock.Object);
        }

        public void ConfigurarMetodoCalculo(string metodoConfigurado)
        {
            ConfigurationMock.Setup(x => x["CalculoPerfilRisco"]).Returns(metodoConfigurado);
        }
    }
}
