using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Builder;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    public class SimulacaoServico : ISimulacaoServico
    {
        private readonly ISimulacaoRepositorio _simulacaoRepositorio;
        private readonly ISimulacaoMapper _simulacaoMapper;
        public SimulacaoServico(ISimulacaoRepositorio simulacaoRepositorio,
                                ISimulacaoMapper simulacaoMapper) 
        {
            _simulacaoRepositorio = simulacaoRepositorio;
            _simulacaoMapper = simulacaoMapper;
        }

        public async Task<List<SimulacaoProdutoDiaDTOResponse>?> ListarSimulacoesDeProdutosPorDia()
        {
            var simulacoes = await _simulacaoRepositorio.ListarTodosAsync();

            return simulacoes
                .SelectMany(z => z.SimulacoesCliente.Select(y => new
                {
                    Produto = y.Produto.Nome,
                    Data = z.DataSimulacao.Date,
                    ValorFinal = z.ValorFinal
                }))
                .GroupBy(x => new { x.Produto, x.Data })
                .Select(g => new SimulacaoProdutoDiaDTOResponse
                {
                    Produto = g.Key.Produto,
                    Data = g.Key.Data.ToString("yyyy-MM-dd"),
                    QuantidadeSimulacoes = g.Count(),
                    MediaValorFinal = g.Average(x => x.ValorFinal)
                })
                .ToList();
        }

        public async Task<List<SimulacaoDTOResponse>?> ListarSimulacoesInvestimentos()
        {
            var simulacoes = await _simulacaoRepositorio.ListarTodosAsync();

            return _simulacaoMapper.ToDtoResponseList(simulacoes);
        }

        public async Task<SimulacaoInvestimentoDTOResponse> SimularInvestimento(
            Produto produto, 
            SimulacaoInvestimentoDTORequest simulacaoInvestimento)
        {
            var simulacao = new Simulacao();
            simulacao.IdCliente = simulacaoInvestimento.ClienteId;
            simulacao.IdProduto = produto.Id;
            simulacao.PrazoMeses = simulacaoInvestimento.PrazoMeses;
            simulacao.ValorInvestido = simulacaoInvestimento.Valor;
            simulacao.DataSimulacao = DateTime.UtcNow;

            await _simulacaoRepositorio.AdicionarAsync(simulacao);

            return new SimulacaoInvestimentoBuilder(produto)
                .ComProdutoValidado()
                .ComResultadoSimulacao(simulacaoInvestimento)
                .ComDataSimulacao(simulacao)
                .Build();
        }
    }
}
