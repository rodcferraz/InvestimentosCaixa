using InvestimentosCaixa.Api.Aplicacao.DTOs.Perfis;

namespace InvestimentosCaixa.Api.Aplicacao.Servicos.Interfaces
{
    public interface IGerarPerfilClienteServico
    {
        Task<PerfilClienteDTOResponse> GerarPerfilCiente(int idCliente);
    }
}
