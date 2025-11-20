using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    /// <summary>
    /// Serviço para cálculo de pontuação do perfil de risco do cliente utilizando critérios personalizados
    /// </summary>
    public class PerfilPontuacaoClientePersonalizadoServico : IPerfilPontuacaoClienteServico
    {
        /// <summary>
        /// Gera pontuação do perfil de carteira do cliente com base no total investido
        /// </summary>
        /// <param name="totalInvestido">Total investido pelo cliente</param>
        /// <returns> Pontuação gerada pela carteira do cliente</returns>
        public int GerarPerfilCarteiraCliente(decimal totalInvestido)
        {
            if (totalInvestido <= 5000) return 10;
            else if (totalInvestido <= 20000) return 30;
            else if (totalInvestido <= 50000) return 50;
            else if (totalInvestido <= 100000) return 80;
            else return 100;
        }

        /// <summary>
        /// Gera pontuação baseado na quantidade de movimentações de investimento realizada pelo cliente
        /// </summary>
        /// <param name="quantidadeMovimentacoes">Total de movimentações de investimento</param>
        /// <returns>Pontuação gerada pelas movimentações do cliente</returns>
        public int GerarPerfilMovimentacoesCliente(int quantidadeMovimentacoes)
        {
            if (quantidadeMovimentacoes <= 2) return 20;
            else if (quantidadeMovimentacoes <= 5) return 50;
            else return 80;
        }

        /// <summary>
        /// Gerar pontuação baseado no perfil de liquidez do cliente
        /// </summary>
        /// <param name="liquidez">Perfil de liquidez do cliente</param>
        /// <returns>Pontuação gerada pelo perfil de liquidez do cliente</returns>
        public int GerarPerfilLiquidezCliente(PerfilRiscoClienteEnum liquidez)
        {
            var pontuacao = 0;

            if (liquidez == PerfilRiscoClienteEnum.Conservador) return 20;
            else if (liquidez == PerfilRiscoClienteEnum.Moderado) return 50;
            else if (liquidez == PerfilRiscoClienteEnum.Agressivo) return 80;

            return pontuacao;
        }
    }
}
