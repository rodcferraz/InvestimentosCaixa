using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.ProdutoServicoTestes
{
    public class ProdutoServicoFixture
    {
        public Mock<IProdutoMapper> ProdutoMapperMock { get; }
        public Mock<IProdutoRepositorio> ProdutoRepositorioMock { get; }
        public ILogger<ProdutoServico> LoggerMock => Mock.Of<ILogger<ProdutoServico>>();
        public ProdutoServico Servico { get; }

        public ProdutoServicoFixture()
        {
            ProdutoMapperMock = new Mock<IProdutoMapper>();
            ProdutoRepositorioMock = new Mock<IProdutoRepositorio>();
            Servico = new ProdutoServico(
                ProdutoMapperMock.Object,
                ProdutoRepositorioMock.Object,
                LoggerMock);
        }
    }
}
