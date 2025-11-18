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
        private readonly ILogger<SimulacaoServico> _logger;
        public SimulacaoServico(ISimulacaoRepositorio simulacaoRepositorio,
                                ISimulacaoMapper simulacaoMapper,
                                ILogger<SimulacaoServico> logger) 
        {
            _simulacaoRepositorio = simulacaoRepositorio;
            _simulacaoMapper = simulacaoMapper;
            _logger = logger;
        }

        public async Task<List<SimulacaoProdutoDiaDTOResponse>?> ListarSimulacoesDeProdutosPorDia()
        {
            var simulacoes = await _simulacaoRepositorio.ListarTodosAsync();

            if (simulacoes == null || simulacoes.Count == 0)
            {
                _logger.LogInformation("Nenhuma simulação de investimento encontrada no dia.");
                return null;
            }

            return _simulacaoMapper.ToDtoProdutoDiaList(simulacoes);
        }

        public async Task<List<SimulacaoDTOResponse>?> ListarSimulacoesInvestimentos()
        {
            var simulacoes = await _simulacaoRepositorio.ListarTodosAsync();

            if (simulacoes == null || simulacoes.Count == 0)
            {
                _logger.LogInformation("Nenhuma simulação de investimento encontrada.");
                return null;
            }

            _logger.LogInformation("Listagem de simulações de investimento realizada com sucesso.");

            return _simulacaoMapper.ToDtoResponseList(simulacoes);
        }

        public async Task<SimulacaoInvestimentoDTOResponse?> SimularInvestimento(
            Produto produto, 
            SimulacaoInvestimentoDTORequest simulacaoInvestimento)
        {
            var simulacao = new Simulacao
            {
                PrazoMeses = simulacaoInvestimento.PrazoMeses,
                ValorInvestido = Math.Round(simulacaoInvestimento.Valor, 2),
                DataSimulacao = DateTime.UtcNow,
                IdCliente = simulacaoInvestimento.ClienteId,
                IdProduto = produto.Id
            };

            _ = await _simulacaoRepositorio.AdicionarAsync(simulacao);

            if (simulacao.Id == 0)
            {
                _logger.LogError($"Erro ao salvar a simulação do cliente {simulacaoInvestimento.ClienteId} " +
                    $"de investimento do produto {produto.Id} no banco de dados.");
                return null;
            }

            _logger.LogInformation($"Simulação de investimento realizada com sucesso para o cliente {simulacaoInvestimento.ClienteId} " +
                $"no produto {produto.Nome} com valor {simulacao.ValorInvestido}.");

            return new SimulacaoInvestimentoBuilder(produto)
                .ComProdutoValidado()
                .ComResultadoSimulacao(simulacaoInvestimento)
                .ComDataSimulacao(simulacao)
                .Build();
        }
    }
}
