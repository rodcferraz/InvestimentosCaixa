using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    public interface IInvestimentoServico
    {
        Task CadastrarInvestimentoAsync(InvestimentoDTOBaseRequest investimentoDto);
        Task<List<InvestimentoDTOResponse>> ListarInvestimentosPorClienteAsync(int cliente);

    }
}
