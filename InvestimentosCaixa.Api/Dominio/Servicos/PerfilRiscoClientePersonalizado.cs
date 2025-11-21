using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    /// <summary>
    /// Orquestrador para definir o perfil de risco do cliente utilizando os cálculos de pontuação e seus respectivos pesos
    /// </summary>
    public class PerfilRiscoClientePersonalizado : IPerfilRiscoClienteServico
    {
        private readonly IPerfilPontuacaoClienteServico _perfilPontuacaoClienteServico;
        private readonly IInvestimentoServico _investimentoServico;
        private readonly IClienteServico _clienteServico;

        const decimal PONTUACAO_CARTEIRA = 0.4m;
        const decimal PONTUACAO_MOVIMENTACOES = 0.3m;
        const decimal PONTUACAO_LIQUIDEZ = 0.3m;

        public PerfilRiscoClientePersonalizado(
            IPerfilPontuacaoClienteServico perfilPontuacaoClienteServico, 
            IInvestimentoServico investimentoServico,
            IClienteServico clienteServico)
        {
            _perfilPontuacaoClienteServico = perfilPontuacaoClienteServico;
            _investimentoServico = investimentoServico;
            _clienteServico = clienteServico;
        }

        /// <summary>
        /// Realiza cálculos para gerar o perfil de risco do cliente
        /// </summary>
        /// <param name="idCliente">Id do cliente para que será gerado o perfil de risco</param>
        /// <returns>Perfil de risco e pontuação associada</returns>
        /// <exception cref="EntityNotFoundException">Lança exceção quando cliente não for encontrado</exception>
        public async Task<(PerfilRiscoClienteEnum, decimal)> CalcularPerfilRiscoCliente(int idCliente)
        {
            PerfilRiscoClienteEnum perfilRisco = default;

            var investimentosCliente = await _investimentoServico.ListarInvestimentosPorClienteAsync(idCliente);
            var cliente = await _clienteServico.DetalhesClienteAsync(idCliente);

            if (cliente == null)
            {
                throw new EntityNotFoundException($"Cliente com id {idCliente} não encontrado");
            }

            var totalInvestido = investimentosCliente.Sum(x => x.Valor);
            var quantidadeMovimentacoes = investimentosCliente.Count();

            if (totalInvestido == 0 && quantidadeMovimentacoes == 0)
            {
                if (cliente.Liquidez == (int)PerfilRiscoClienteEnum.Conservador)
                    return (PerfilRiscoClienteEnum.Conservador, 20);
                else if (cliente.Liquidez == (int)PerfilRiscoClienteEnum.Moderado)
                    return (PerfilRiscoClienteEnum.Moderado, 60);
                else
                    return (PerfilRiscoClienteEnum.Agressivo, 80);
            }

            var pontuacaoCarteira = _perfilPontuacaoClienteServico.GerarPerfilCarteiraCliente(totalInvestido);
            var pontuacaoMovimentacoes = _perfilPontuacaoClienteServico.GerarPerfilMovimentacoesCliente(quantidadeMovimentacoes);
            var pontuacaoLiquidez =_perfilPontuacaoClienteServico.GerarPerfilLiquidezCliente((PerfilRiscoClienteEnum) cliente.Liquidez);

            var pontuacaoTotal = pontuacaoCarteira * PONTUACAO_CARTEIRA + 
                                  pontuacaoMovimentacoes * PONTUACAO_MOVIMENTACOES +
                                  pontuacaoLiquidez * PONTUACAO_LIQUIDEZ;

            if (pontuacaoTotal <= 20) perfilRisco = PerfilRiscoClienteEnum.Conservador;
            else if (pontuacaoTotal <= 60) perfilRisco = PerfilRiscoClienteEnum.Moderado;
            else perfilRisco = PerfilRiscoClienteEnum.Agressivo;

            return (perfilRisco, pontuacaoTotal);
        }
    }
}
