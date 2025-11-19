using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestimentosCaixa.Testes.Dominio.Servicos.ClienteServicoTestes
{
    public class ClienteServicoFixture
    {
        public Mock<IClienteRepositorio> _repoMock;
        public Mock<IClienteMapper> _mapperMock;
        public Mock<ISegurancaServico> _segurancaMock;
        public ILogger<ClienteServico> _loggerMock => Mock.Of<ILogger<ClienteServico>>();
        public ClienteServico _servico;

        public ClienteServicoFixture()
        {
            _repoMock = new Mock<IClienteRepositorio>();
            _mapperMock = new Mock<IClienteMapper>();
            _segurancaMock = new Mock<ISegurancaServico>();

            _servico = new ClienteServico(
                _repoMock.Object,
                _mapperMock.Object,
                _loggerMock,
                _segurancaMock.Object);
        }
    }
}
