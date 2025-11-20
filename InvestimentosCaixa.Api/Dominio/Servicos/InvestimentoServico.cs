using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    /// <summary>
    /// Realiza operações relacionadas a investimentos, como cadastro e listagem de investimentos por cliente.
    /// </summary>
    public class InvestimentoServico : IInvestimentoServico
    {
        private readonly IInvestimentoRepositorio _investimentoRepositorio;
        private readonly IInvestimentoMapper _investimentoMapper;
        private readonly ILogger<InvestimentoServico> _logger;

        public InvestimentoServico(
            IInvestimentoRepositorio investimentoRepositorio,
            IInvestimentoMapper investimentoMapper,
            ILogger<InvestimentoServico> logger)
        {
            _investimentoRepositorio = investimentoRepositorio;
            _investimentoMapper = investimentoMapper;
            _logger = logger;
        }

        /// <summary>
        /// Cadastra o investimento solicitado pelo cliente
        /// </summary>
        /// <param name="investimentoDto">Dados de requisição para investimento</param>
        /// <returns>Retorna dados de investimento cadastrado</returns>
        public async Task<InvestimentoDTOResponse> CadastrarInvestimentoAsync(InvestimentoDTOBaseRequest investimentoDto)
        {
            var investimentoEntity = _investimentoMapper.ToBaseEntity(investimentoDto);

            await _investimentoRepositorio.AdicionarAsync(investimentoEntity);

            _logger.LogInformation($"Investimento cadastrado: {investimentoEntity}");

            return _investimentoMapper.ToDtoResponse(investimentoEntity);
        }

        /// <summary>
        /// Lista todos os investimentos realizados por um cliente específico
        /// </summary>
        /// <param name="idCliente">Id do cliente</param>
        /// <returns>Lista de todos os investimentos do cliente</returns>
        public async Task<List<InvestimentoDTOResponse>> ListarInvestimentosPorClienteAsync(int idCliente)
        {
            var investimentos = await _investimentoRepositorio.ListarInvestimentosPorClienteAsync(idCliente);

            if (investimentos.IsNullOrEmpty())
            {
                _logger.LogInformation($"Nenhum investimento encontrado para o cliente com Id {idCliente}.");
                return Enumerable.Empty<InvestimentoDTOResponse>().ToList();
            }

            _logger.LogInformation($"Listagem de investimentos realizada com sucesso para o cliente com Id {idCliente}.");

            return _investimentoMapper.ToDtoResponseList(investimentos);
        }
    }
}
