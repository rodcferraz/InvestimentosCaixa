using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    public interface ISimulacaoServico
    {
        Task<List<SimulacaoDTOResponse>?> ListarSimulacoesInvestimentos();

        Task<List<SimulacaoProdutoDiaDTOResponse>?> ListarSimulacoesDeProdutosPorDia();
        Task<SimulacaoInvestimentoDTOResponse> SimularInvestimento(
            Produto produto, 
            SimulacaoInvestimentoDTORequest simulacaoInvestimento);
    }
}
