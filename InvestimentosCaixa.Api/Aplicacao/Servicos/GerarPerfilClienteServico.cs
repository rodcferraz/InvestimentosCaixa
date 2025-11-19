using InvestimentosCaixa.Api.Aplicacao.DTOs.Perfis;
using InvestimentosCaixa.Api.Aplicacao.Servicos.Interfaces;
using InvestimentosCaixa.Api.Configuracoes;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Factories.Interfaces;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Aplicacao.Servicos
{
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

            _logger.LogInformation("Perfil de risco efetuado: Perfil do cliente {perfilCliente}, Pontuação do cliente {pontuacao}",
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
