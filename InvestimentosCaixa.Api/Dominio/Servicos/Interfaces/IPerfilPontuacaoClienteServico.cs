using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    public interface IPerfilPontuacaoClienteServico
    {
        int GerarPerfilCarteiraCliente(decimal totalInvestido);
        int GerarPerfilMovimentacoesaCliente(int quantidadeMovimentacoes);
        int GerarPerfilLiquidezCliente(PerfilRiscoClienteEnum perfilClienteLiquidez);
    }
}
