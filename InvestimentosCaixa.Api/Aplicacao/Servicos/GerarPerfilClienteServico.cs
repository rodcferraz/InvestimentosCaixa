using InvestimentosCaixa.Api.Aplicacao.DTOs.Perfis;
using InvestimentosCaixa.Api.Aplicacao.Servicos.Interfaces;
using InvestimentosCaixa.Api.Configuracoes;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Factories.Interfaces;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Aplicacao.Servicos
{
    /// <summary>
    /// Responsável por gerar o perfil de risco do cliente
    /// </summary>
    public class GerarPerfilClienteServico : IGerarPerfilClienteServico
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<GerarPerfilClienteServico> _logger;
        private readonly IMetodoCalculoPontuacaoPerfilRiscoClienteFactory _metodoCalculoPontuacaoFactory;
        private readonly IGerarPerfilRiscoClienteFactory _gerarPerfilRiscoClienteFactory;
        private readonly ICalculoPerfilRiscoMapper _calculoMapper;

        public GerarPerfilClienteServico(
            AppSettings appSettings,
            ICalculoPerfilRiscoMapper calculoMapper,
            ILogger<GerarPerfilClienteServico> logger,
            IMetodoCalculoPontuacaoPerfilRiscoClienteFactory metodoCalculoPontuacaoFactory,
            IGerarPerfilRiscoClienteFactory gerarPerfilRiscoClienteFactory)
        {
            _appSettings = appSettings;
            _calculoMapper = calculoMapper;
            _logger = logger;
            _metodoCalculoPontuacaoFactory = metodoCalculoPontuacaoFactory;
            _gerarPerfilRiscoClienteFactory = gerarPerfilRiscoClienteFactory;
        }

        /// <summary>
        /// Gera o perfil de risco do cliente. O factory <see cref="IGerarPerfilRiscoClienteFactory"/>será responsável por orquestrar o cálculo
        /// efetivado pela fábrica de métodos de cálculo <see cref="IMetodoCalculoPontuacaoPerfilRiscoClienteFactory"/> a fim 
        /// de gerar o perfil de risco do cliente.
        /// </summary>
        /// <param name="idCliente">Id do cliente para geração do perfil</param>
        /// <returns>Resposta para o perfil do cliente gerado.</returns>
        public async Task<PerfilClienteDTOResponse> GerarPerfilCiente(int idCliente)
        {
            var metodoCalculoPerfilRisco = _calculoMapper.ParaPerfilRiscoClienteEnum(_appSettings.CalculoPerfilRisco);

            _logger.LogInformation("Iniciando cálculo do perfil do cliente {IdCliente} utilizando o método {MetodoCalculo}.",
                idCliente, metodoCalculoPerfilRisco.ToString());

            var metodoCalculo = _metodoCalculoPontuacaoFactory.Criar(metodoCalculoPerfilRisco);

            _logger.LogInformation($"Cálculo de pontuacao utilizando o método: {metodoCalculo.GetType().Name} .");

            var perfilRiscoCliente = _gerarPerfilRiscoClienteFactory.Criar(metodoCalculoPerfilRisco, metodoCalculo);

            _logger.LogInformation($"Definição de perfil de risco utilizando o método: {perfilRiscoCliente.GetType().Name} .");

            var (perfilCliente, pontuacao) = await perfilRiscoCliente.CalcularPerfilRiscoCliente(idCliente);

            _logger.LogInformation($"Perfil de risco efetuado: Perfil do cliente {perfilCliente}, Pontuação do cliente {pontuacao}",
                perfilCliente.ToString(), pontuacao);

            return new PerfilClienteDTOResponse
            {
                ClienteId = idCliente,
                Perfil = perfilCliente.ToString(),
                Pontuacao = pontuacao,
                Descricao = perfilCliente.ExibirDescricao()
            };
        }
    }
}
