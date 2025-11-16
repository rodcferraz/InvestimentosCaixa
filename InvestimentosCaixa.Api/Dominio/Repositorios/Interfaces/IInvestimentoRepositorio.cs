using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces
{
    public interface IInvestimentoRepositorio : IGenericoRepositorio<Investimento>
    {
        Task<List<Investimento>> ListarInvestimentosPorClienteAsync(int idCliente);
    }
}
