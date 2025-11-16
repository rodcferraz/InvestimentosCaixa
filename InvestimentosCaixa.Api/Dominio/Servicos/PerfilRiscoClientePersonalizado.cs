using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
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

        public async Task<(PerfilRiscoClienteEnum, decimal)> CalcularPerfilRiscoCliente(int idCliente)
        {
            PerfilRiscoClienteEnum perfilRisco = default;

            var investimentosCliente = await _investimentoServico.ListarInvestimentosPorClienteAsync(idCliente);
            var cliente = await _clienteServico.DetalhesClienteAsync(idCliente);


            var totalInvestido = investimentosCliente.Sum(x => x.Valor);
            var quantidadeMovimentacoes = investimentosCliente.Count();

            var pontuacaoCarteira = _perfilPontuacaoClienteServico.GerarPerfilCarteiraCliente(totalInvestido);
            var pontuacaoMovimentacoes = _perfilPontuacaoClienteServico.GerarPerfilMovimentacoesaCliente(quantidadeMovimentacoes);
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
