using InvestimentosCaixa.Api.Aplicacao.DTOs.Perfis;
using InvestimentosCaixa.Api.Aplicacao.Servicos.Interfaces;
using InvestimentosCaixa.Api.Configuracoes;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Aplicacao.Servicos
{
    public class GerarPerfilClienteServico : IGerarPerfilClienteServico
    {
        private readonly AppSettings _appSettings;
        private readonly IInvestimentoServico _investimentoServico;
        private readonly IClienteServico _clienteServico;
        private readonly ILogger<GerarPerfilClienteServico> _logger;
        private readonly ICalculoPerfilRiscoMapper _calculoPerfilRiscoMapper;

        public GerarPerfilClienteServico(
            AppSettings appSettings,
            IInvestimentoServico investimentoServico,
            IClienteServico clienteServico,
            ILogger<GerarPerfilClienteServico> logger,
            ICalculoPerfilRiscoMapper calculoPerfilRiscoMapper)
        {
            _appSettings = appSettings;
            _investimentoServico = investimentoServico;
            _clienteServico = clienteServico;
            _logger = logger;
            _calculoPerfilRiscoMapper = calculoPerfilRiscoMapper;
        }

        public async Task<PerfilClienteDTOResponse> GerarPerfilCiente(int idCliente)
        {
            var metodoCalculoPerfilRisco = _calculoPerfilRiscoMapper.ParaPerfilRiscoClienteEnum(_appSettings.CalculoPerfilRisco);

            _logger.LogInformation("Iniciando cálculo do perfil do cliente {IdCliente} utilizando o método {MetodoCalculo}.",
                idCliente, metodoCalculoPerfilRisco.ToString());

            IPerfilPontuacaoClienteServico perfilPontuacao = metodoCalculoPerfilRisco switch
            {
                CalculoParaPerfilRiscoEnum.Personalizado => new PerfilPontuacaoClientePersonalizadoServico(),
                CalculoParaPerfilRiscoEnum.Anbima => throw new NotImplementedException("Calculo de perfil de risco ANBIMA não implementado."),
                _ => throw new NotImplementedException("Calculo de perfil de risco não implementado.")       
            };

            _logger.LogInformation($"Cálculo de pontuacao utilizando o método: {perfilPontuacao.GetType().Name} .");

            IPerfilRiscoClienteServico perfilRiscoCliente = metodoCalculoPerfilRisco switch
            {
                CalculoParaPerfilRiscoEnum.Personalizado => new PerfilRiscoClientePersonalizado(
                                                                perfilPontuacao, 
                                                                _investimentoServico, 
                                                                _clienteServico),
                CalculoParaPerfilRiscoEnum.Anbima => throw new NotImplementedException("Calculo de perfil de risco ANBIMA não implementado."),
                _ => throw new NotImplementedException("Calculo de perfil de risco não implementado.")
            };

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
