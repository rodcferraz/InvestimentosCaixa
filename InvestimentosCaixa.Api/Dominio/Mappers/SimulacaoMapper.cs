using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    public class SimulacaoMapper : ISimulacaoMapper
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public SimulacaoMapper(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public SimulacaoDTOResponse ToDtoResponse(Simulacao simulacao)
        {
            var produto = simulacao.SimulacoesCliente
                .Select(x => x.Produto)
                .FirstOrDefault();

            return new SimulacaoDTOResponse
            {
                Id = simulacao.Id,
                Produto = produto.Nome,
                ValorInvestido = simulacao.ValorInvestido,
                ValorFinal = simulacao.ValorFinal,
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
    }
}
