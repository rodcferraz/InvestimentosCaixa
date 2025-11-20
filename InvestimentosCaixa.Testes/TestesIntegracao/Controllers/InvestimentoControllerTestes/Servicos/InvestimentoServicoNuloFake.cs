using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.InvestimentoControllerTestes.Servicos
{
    public class InvestimentoServicoNuloFake : IInvestimentoServico
    {
        public Task<InvestimentoDTOResponse> CadastrarInvestimentoAsync(InvestimentoDTOBaseRequest investimentoDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<InvestimentoDTOResponse>> ListarInvestimentosPorClienteAsync(int cliente)
        {
            throw new NotImplementedException();
        }
    }
}
