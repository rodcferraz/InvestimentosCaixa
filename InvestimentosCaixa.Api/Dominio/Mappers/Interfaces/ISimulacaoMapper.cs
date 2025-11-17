using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    public interface ISimulacaoMapper
    {
        SimulacaoDTOResponse ToDtoResponse(Simulacao simulacao);
        List<SimulacaoProdutoDiaDTOResponse> ToDtoProdutoDiaList(List<Simulacao> simulacoes);
        List<SimulacaoDTOResponse> ToDtoResponseList(IEnumerable<Simulacao> clientes);
    }
}
