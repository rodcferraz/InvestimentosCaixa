using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.InvestimentoControllerTestes.Servicos
{
    public class InvestimentoServicoErroFake : IInvestimentoServico
    {
        public Task<int> CadastrarInvestimentoAsync(InvestimentoDTOBaseRequest request)
        {
            throw new Exception("Erro simulado no serviço");
        }

        public Task<List<InvestimentoDTOResponse>> ListarInvestimentosPorClienteAsync(int cliente)
        {
            throw new Exception("Erro simulado no serviço");
        }
    }
}
