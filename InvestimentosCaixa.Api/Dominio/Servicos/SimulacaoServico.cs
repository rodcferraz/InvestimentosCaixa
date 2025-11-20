using InvestimentosCaixa.Api.Aplicacao.DTOs.Simulacoes;
using InvestimentosCaixa.Api.Dominio.Builder;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    /// <summary>
    /// Serviço responsável por gerenciar simulações de investimento.
    /// </summary>
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

        /// <summary>
        /// Listar simulações de produtos efetuados no dia
        /// </summary>
        /// <returns>Lista com simulações efetuadas no dia</returns>
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

        /// <summary>
        /// Listar todas as simulações realizadas pelos clientes
        /// </summary>
        /// <returns>Lista todas as simulações realizadas pelos clientes</returns>
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

        /// <summary>
        /// Realiza a simulação de investimento para um produto e cliente específico
        /// </summary>
        /// <param name="produto">Classe de produto utilizada para cadastro de investimento</param>
        /// <param name="simulacaoInvestimento">Informações para cadastro de simulações como Id do cliente, prazo, valor e rentabilidade</param>
        /// <returns>Simulação efetuada</returns>
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

            var simulacaoDb = await _simulacaoRepositorio.AdicionarAsync(simulacao);

            if (simulacaoDb.Id == 0)
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
