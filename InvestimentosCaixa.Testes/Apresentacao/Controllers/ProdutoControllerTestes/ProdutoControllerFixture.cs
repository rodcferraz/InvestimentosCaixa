using InvestimentosCaixa.Api.Apresentacao.Controllers;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Moq;

namespace InvestimentosCaixa.Testes.Apresentacao.Controllers.ProdutoControllerTestes
{
    public class ProdutoControllerFixture
    {
        public Mock<IProdutoServico> ProdutoServicoMock { get; }
        public ProdutoController Controller { get; }

        public ProdutoControllerFixture()
        {
            ProdutoServicoMock = new Mock<IProdutoServico>();
            Controller = new ProdutoController(ProdutoServicoMock.Object);
        }
    }
}
