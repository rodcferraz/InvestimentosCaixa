namespace InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces
{
    public interface IGenericoRepositorio<T> where T : class
    {
        Task<T?> ListarPorIdAsync(int id);
        Task<List<T>> ListarTodosAsync();
        Task<T> AdicionarAsync(T entidade);
        Task<T> AtualizarAsync(T entidade);
    }
}
