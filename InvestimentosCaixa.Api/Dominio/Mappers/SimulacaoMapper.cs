using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    public class SimulacaoMapper : ISimulacaoMapper
    {
        public SimulacaoDTOResponse ToDtoResponse(Simulacao simulacao)
        {
            return new SimulacaoDTOResponse
            {
                Id = simulacao.Id,
                Produto = simulacao.Produto.Nome,
                ClienteId = simulacao.IdCliente,
                ValorInvestido = simulacao.ValorInvestido,
                ValorFinal = Math.Round(simulacao.ValorInvestido + 
                            simulacao.ValorInvestido * ((1 + simulacao.Produto.Rentabilidade) / 12),2),
                PrazoMeses = simulacao.PrazoMeses,
                DataSimulacao = simulacao.DataSimulacao.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };
        }

        public List<SimulacaoDTOResponse> ToDtoResponseList(IEnumerable<Simulacao> clientes)
        {
            return clientes != null ? 
                        clientes
                            .Select(x => ToDtoResponse(x))
                            .ToList() :
                        new List<SimulacaoDTOResponse>();
        }

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
