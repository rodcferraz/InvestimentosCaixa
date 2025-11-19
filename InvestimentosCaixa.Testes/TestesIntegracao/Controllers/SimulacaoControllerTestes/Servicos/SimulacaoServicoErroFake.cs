using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes.Servicos
{
    public class SimulacaoServicoErroFake : ISimulacaoServico
    {
        public Task<List<SimulacaoProdutoDiaDTOResponse>?> ListarSimulacoesDeProdutosPorDia()
        {
            throw new Exception("Erro inesperado");
        }

        public Task<List<SimulacaoDTOResponse>?> ListarSimulacoesInvestimentos()
        {
            throw new Exception("Erro inesperado");
        }

        public Task<SimulacaoInvestimentoDTOResponse?> SimularInvestimento(Produto produto, SimulacaoInvestimentoDTORequest simulacaoInvestimento)
        {
            throw new Exception("Erro inesperado");
        }
    }
}
