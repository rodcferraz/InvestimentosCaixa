namespace InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces
{
    /// <summary>
    /// Repositório genétco para operações CRUD básicas
    /// </summary>
    public interface IGenericoRepositorio<T> where T : class
    {

        /// <summary>
        /// Lista por id a entidade de forma assíncrona
        /// </summary>
        Task<T?> ListarPorIdAsync(int id);

        /// <summary>
        /// Lista todas as entidades de forma assíncrona
        /// </summary>
        Task<List<T>> ListarTodosAsync();

        /// <summary>
        /// Adiciona uma nova entidade ao banco de dados de forma assíncrona
        /// </summary>
        Task<T> AdicionarAsync(T entidade);

        /// <summary>
        /// Atualiza uma entidade existente no banco de dados de forma assíncrona
        /// </summary>
        Task<T> AtualizarAsync(T entidade);
    }
}
