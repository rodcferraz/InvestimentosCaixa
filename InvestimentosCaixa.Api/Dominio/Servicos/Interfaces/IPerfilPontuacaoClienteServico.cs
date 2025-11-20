using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    /// <summary>
    /// Serviço para cálculo de pontuação do perfil de risco do cliente utilizando critérios personalizados
    /// </summary>
    public interface IPerfilPontuacaoClienteServico
    {
        /// <summary>
        /// Gera pontuação do perfil de carteira do cliente com base no total investido
        /// </summary>
        /// <param name="totalInvestido">Total investido pelo cliente</param>
        /// <returns> Pontuação gerada pela carteira do cliente</returns>
        int GerarPerfilCarteiraCliente(decimal totalInvestido);

        /// <summary>
        /// Gera pontuação baseado na quantidade de movimentações de investimento realizada pelo cliente
        /// </summary>
        /// <param name="quantidadeMovimentacoes">Total de movimentações de investimento</param>
        /// <returns>Pontuação gerada pelas movimentações do cliente</returns>
        int GerarPerfilMovimentacoesCliente(int quantidadeMovimentacoes);

        /// <summary>
        /// Gerar pontuação baseado no perfil de liquidez do cliente
        /// </summary>
        /// <param name="perfilClienteLiquidez">Perfil de liquidez do cliente</param>
        /// <returns>Pontuação gerada pelo perfil de liquidez do cliente</returns>
        int GerarPerfilLiquidezCliente(PerfilRiscoClienteEnum perfilClienteLiquidez);
    }
}
