using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    public interface IPerfilPontuacaoClienteServico
    {
        int GerarPerfilCarteiraCliente(decimal totalInvestido);
        int GerarPerfilMovimentacoesCliente(int quantidadeMovimentacoes);
        int GerarPerfilLiquidezCliente(PerfilRiscoClienteEnum perfilClienteLiquidez);
    }
}
