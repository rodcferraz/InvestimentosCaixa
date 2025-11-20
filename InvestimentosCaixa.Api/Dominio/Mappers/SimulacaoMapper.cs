using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    /// <summary>
    /// Realiza o mapeamento entre a entidade Simulacao e seus respectivos DTOs.
    /// </summary>
    public class SimulacaoMapper : ISimulacaoMapper
    {
        /// <summary>
        /// Realiza o mapeamento de uma entidade Simulacao para um DTO de resposta SimulacaoDTOResponse.
        /// </summary>
        public SimulacaoDTOResponse ToDtoResponse(Simulacao simulacao)
        {
            var taxaMensal = simulacao.Produto.Rentabilidade / 12;

            var valorFinal = simulacao.ValorInvestido * (1 + taxaMensal * simulacao.PrazoMeses);

            return new SimulacaoDTOResponse
            {
                Id = simulacao.Id,
                Produto = simulacao.Produto.Nome,
                ClienteId = simulacao.IdCliente,
                ValorInvestido = simulacao.ValorInvestido,
                ValorFinal = Math.Round(valorFinal, 2),
                PrazoMeses = simulacao.PrazoMeses,
                DataSimulacao = simulacao.DataSimulacao.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };
        }

        /// <summary>
        /// Realiza o mapeamento de uma lista de entidades Simulacao para uma lista de DTOs de resposta SimulacaoDTOResponse.
        /// </summary>
        public List<SimulacaoDTOResponse> ToDtoResponseList(IEnumerable<Simulacao> clientes)
        {
            return clientes != null ? 
                        clientes
                            .Select(x => ToDtoResponse(x))
                            .ToList() :
                        new List<SimulacaoDTOResponse>();
        }

        /// <summary>
        /// Realiza o mapeamento de uma lista de entidades Simulacao para uma lista de DTOs de resposta SimulacaoProdutoDiaDTOResponse agrupados por produto e dia.
        /// </summary>
        public List<SimulacaoProdutoDiaDTOResponse> ToDtoProdutoDiaList(List<Simulacao> simulacoes)
        {
           return simulacoes
                        .GroupBy(s => new
                        {
                            Produto = s.Produto.Nome,
                            Data = s.DataSimulacao.Date
                        })
                        .Select(g => new SimulacaoProdutoDiaDTOResponse
                        {
                            Produto = g.Key.Produto,
                            Data = g.Key.Data.ToString("yyyy-MM-dd"),
                            QuantidadeSimulacoes = g.Count(),
                            MediaValorFinal = g.Average(s => s.ValorInvestido)
                        })
                        .ToList();
        }
    }
}
