using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes.Servicos
{
    public class SimulacaoServicoNuloFake : ISimulacaoServico
    {
        public Task<List<SimulacaoProdutoDiaDTOResponse>?> ListarSimulacoesDeProdutosPorDia()
        {
            return Task.FromResult<List<SimulacaoProdutoDiaDTOResponse>>(null);
        }

        public Task<List<SimulacaoDTOResponse>?> ListarSimulacoesInvestimentos()
        {
            return Task.FromResult<List<SimulacaoDTOResponse>>(null);
        }

        public Task<SimulacaoInvestimentoDTOResponse?> SimularInvestimento(Produto produto, SimulacaoInvestimentoDTORequest simulacaoInvestimento)
        {
            return Task.FromResult<SimulacaoInvestimentoDTOResponse>(null);
        }
    }
}
