using InvestimentosCaixa.Api.Dominio.Enums;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    public interface IPerfilRiscoClienteServico
    {
        Task<(PerfilRiscoClienteEnum, decimal)> CalcularPerfilRiscoCliente(int idCliente);
    }
}
