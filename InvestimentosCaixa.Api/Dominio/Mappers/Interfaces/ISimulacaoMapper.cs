using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    public interface ISimulacaoMapper
    {
        /// <summary>
        /// Realiza o mapeamento de uma entidade Simulacao para um DTO de resposta SimulacaoDTOResponse.
        /// </summary>
        SimulacaoDTOResponse ToDtoResponse(Simulacao simulacao);

        /// <summary>
        /// Realiza o mapeamento de uma lista de entidades Simulacao para uma lista de DTOs de resposta SimulacaoDTOResponse.
        /// </summary>
        List<SimulacaoProdutoDiaDTOResponse> ToDtoProdutoDiaList(List<Simulacao> simulacoes);

        /// <summary>
        /// Realiza o mapeamento de uma lista de entidades Simulacao para uma lista de DTOs de resposta SimulacaoProdutoDiaDTOResponse agrupados por produto e dia.
        /// </summary>
        List<SimulacaoDTOResponse> ToDtoResponseList(IEnumerable<Simulacao> clientes);
    }
}
