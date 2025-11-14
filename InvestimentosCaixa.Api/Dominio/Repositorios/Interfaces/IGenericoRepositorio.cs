namespace InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces
{
    public interface IGenericoRepositorio<T> where T : class
    {
        Task<T?> ListarPorId(int id);
        Task<List<T>> ListarTodos();
        Task Adicionar(T entidade);
        Task<T> Atualizar(T entidade);
        Task Deletar(T entidade);
    }
}
