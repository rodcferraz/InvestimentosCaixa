using InvestimentosCaixa.Api.Apresentacao.Controllers;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Testes.Apresentacao.Controllers.ProdutoControllerTestes
{
    public class ProdutoControllerFixture
    {
        public Mock<IProdutoServico> ProdutoServicoMock { get; }
        public ILogger<ProdutoController> LoggerMock => Mock.Of<ILogger<ProdutoController>>();
        public ProdutoController Controller { get; }

        public ProdutoControllerFixture()
        {
            ProdutoServicoMock = new Mock<IProdutoServico>();
            Controller = new ProdutoController(ProdutoServicoMock.Object, LoggerMock);
        }
    }
}
